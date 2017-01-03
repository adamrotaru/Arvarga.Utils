/*
 * See the LICENSE file for copyright and license conditions.
 * (c) 2016 Adam Rotaru
 */

﻿﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Arvarga.Utils.Stats
{
    /// <summary>
    /// Maintain percentile and mean statistics about a set of samples, by storing a subset of them.
    /// Only a limited set of samples are stored, to reduce memory footprint.
    /// The number of samples to store is limited by a max count, or by a max time period.
    /// </summary>
    public class NumberStatPctileMean
    {
        protected struct SampleEntry
        {
            public double sample;
            public long timestamp;
        }

        protected int myMaxCount;
        protected int myMaxAgeSec;
        protected long myCountAll;
        protected Queue<SampleEntry> mySamples = new Queue<SampleEntry>();
        protected double myTotal;
        protected double myTotalAll;
        protected double myMinAll;
        protected double myMaxAll;
        protected object myLock = new object();
        protected long myCreatedTime;
        protected List<double> mySortedSamples;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="maxCount">The maximum number of samples to be stored</param>
        /// <param name="maxAgeSec">The maximum age of samples to be stored, in sec.  If it is 0, time does not matter.</param>
        public NumberStatPctileMean(int maxCount = 100, int maxAgeSec = 0)
        {
            myMaxCount = maxCount;
            myMaxAgeSec = maxAgeSec;
            myCreatedTime = DateTime.Now.Ticks;
        }

        public void SetCreatedTimeTestonly(DateTime createdNow) { myCreatedTime = createdNow.Ticks; }

        /// <summary>
        /// Add a new sample to the statistics.
        /// </summary>
        /// <param name="n">The sample to be added</param>
        public void Add(double n)
        {
            Add(n, DateTime.Now.Ticks);
        }

        /// <summary>
        /// Add a new sample to the statistics.
        /// </summary>
        /// <param name="n">The sample to be added</param>
        public void AddTestOnly(double n, DateTime addTime)
        {
            Add(n, addTime.Ticks);
        }

        protected void Add(double n, long addTime)
        {
            lock (myLock)
            {
                bool addItNow = true;
                // check if we need to add it, if there is room for it
                if (myMaxAgeSec > 0)
                {
                    // imiting is based on time only
                    int optimalSize = myMaxCount;
                    double queueAge = (double)(addTime - myCreatedTime) / 10000000.0;
                    if (queueAge < myMaxAgeSec)
                    {
                        // not old enough to fill up to myMaxCount, only proportional length
                        optimalSize = 1 + (int)(queueAge / (double)myMaxAgeSec * (myMaxCount - 1));
                    }
                    // if we need to remove old ones to make room
                    while (mySamples.Count > 0 && ((addTime - mySamples.Peek().timestamp) / 10000000) > myMaxAgeSec)
                    {
                        SampleEntry oldest = mySamples.Dequeue();
                        myTotal -= oldest.sample;
                    }
                    if (mySamples.Count >= optimalSize)
                    {
                        // no room for it now, don't add
                        addItNow = false;
                    }
                }
                else
                {
                    // remove first one if needed
                    if (mySamples.Count >= myMaxCount)
                    {
                        SampleEntry oldest = mySamples.Dequeue();
                        myTotal -= oldest.sample;
                    }
                }

                // add the new sample
                ++myCountAll;
                if (addItNow)
                {
                    mySamples.Enqueue(new SampleEntry { sample = n, timestamp = addTime });
                    myTotal += n;
                    myTotalAll += n;
                    if (myCountAll == 1)
                    {
                        // special handling for first min/max
                        myMinAll = myMaxAll = n;
                    }
                    else
                    {
                        myMinAll = Math.Min(myMinAll, n);
                        myMaxAll = Math.Max(myMaxAll, n);
                    }

                    // invalidate cached sorted samples
                    mySortedSamples = null;
                }
            }
        }

        /// <summary>
        /// Get the current all-time count of the samples.
        /// </summary>
        public long CountAll { get { return myCountAll; } }

        /// <summary>
        /// Get the current count of the samples.
        /// </summary>
        public int Count { get { return mySamples.Count; } }

        /// <summary>
        /// Get the current average of the samples (0 for the case of no samples).
        /// </summary>
        public double Average
        {
            get
            {
                lock (myLock)
                {
                    int c = mySamples.Count;
                    return (c == 0) ? 0 : myTotal / c;
                }
            }
        }

        /// <summary>
        /// Get the current all-time average of the samples (0 for the case of no samples).
        /// </summary>
        public double AverageAll
        {
            get
            {
                lock (myLock)
                {
                    return (myCountAll == 0) ? 0 : myTotalAll / myCountAll;
                }
            }
        }

        /// <summary>
        /// Get the current median (0 for the case of no samples).
        /// </summary>
        public double Median
        {
            get
            {
                lock (myLock)
                {
                    if (mySamples.Count == 0) return 0;
                    SortSamples();
                    // take the 'middle'
                    return mySortedSamples[(mySortedSamples.Count - 1) / 2];
                }
            }
        }

        protected void SortSamples()
        {
            lock (myLock)
            {
                // if exists, reuse
                if (mySortedSamples != null) return;
                // rebuild: take current samples and sort
                mySortedSamples = new List<double>(mySamples.ToList().Select(x => x.sample));
                mySortedSamples.Sort();
            }
        }

        /// <summary>
        /// Get the p-th percentile (0 for the case of no samples).
        /// It gets the one sample, or value interpolated between two samples, for which p percentage of samples is smaller. 
        /// </summary>
        public double GetPercentile(double p)
        {
            lock (myLock)
            {
                if (mySamples.Count == 0) return 0;
                SortSamples();
                // take the percentile
                if (p < 0) p = 0;
                if (p > 1) p = 1;
                double pindex = p * (mySortedSamples.Count - 1);
                int index = (int)Math.Floor(pindex);
                double w2 = pindex - index;
                if (Math.Abs(w2) < 0.000001)
                {
                    // p falls exactly at an element
                    return mySortedSamples[index];
                }
                // interpolate
                double w1 = (double)1 - w2;
                return w1 * mySortedSamples[index] + w2 * mySortedSamples[index + 1];
            }
        }

        /// <summary>
        /// Get the current all-time minimum of the samples (0 for the case of no samples).
        /// </summary>
        public double MinAll { get { return myMinAll; } }

        /// <summary>
        /// Get the current all-time maximum of the samples (0 for the case of no samples).
        /// </summary>
        public double MaxAll { get { return myMaxAll; } }

        public override string ToString() { return $"med: {Median} ({MinAll} - {MaxAll} cnt {CountAll})"; }
    }
}
