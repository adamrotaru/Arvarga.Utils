/*
 * See the LICENSE file for copyright and license conditions.
 * (c) 2016 Adam Rotaru
 */

﻿﻿using System;

namespace Arvarga.Utils.Stats
{
    /// <summary>
    /// Maintain statistics about a set of numerical samples: keep track of their average, their number, and minimum and maximum values.
    /// The actual samples are not stored, so it's fast and has minimal memory usage.
    /// </summary>
    public class NumberStatAvg
    {
        protected int myCount;
        protected double myTotal;
        protected double myMin;
        protected double myMax;
        protected object myLock = new object();

        /// <summary>
        /// Add a new sample to the statistics.
        /// </summary>
        /// <param name="n">The sample to be added</param>
        public void Add(double n)
        {
            lock (myLock)
            {
                myCount++;
                myTotal += n;
                if (myCount == 1)
                {
                    // special handling for first min/max
                    myMin = myMax = n;
                }
                else
                {
                    myMin = Math.Min(myMin, n);
                    myMax = Math.Max(myMax, n);
                }
            }
        }

        /// <summary>
        /// Get the current count of the samples.
        /// </summary>
        public double Count { get { return myCount; } }

        /// <summary>
        /// Get the current average of the samples (0 for the case of no samples).
        /// </summary>
        public double Average { get { return (myCount == 0) ? 0 : myTotal / myCount; } }

        /// <summary>
        /// Get the current minimum of the samples (0 for the case of no samples).
        /// </summary>
        public double Min { get { return myMin; } }

        /// <summary>
        /// Get the current maximum of the samples (0 for the case of no samples).
        /// </summary>
        public double Max { get { return myMax; } }

        public override string ToString() { return $"avg: {Average} ({Min} - {Max} cnt {Count})"; }
    }
}
