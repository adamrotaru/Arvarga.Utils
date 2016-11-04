/*
 * See the LICENSE file for copyright and license conditions.
 * (c) 2016 Adam Rotaru
 */

﻿﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Arvarga.Utils.Stats.Test.Unit
{
    [TestClass]
    public class TestNumberStatPctileMean
    {
        /// <summary>
        /// Test a simple, normal use case
        /// </summary>
        [TestMethod]
        public void TestNumberStatPctileMean_TestSimple()
        {
            NumberStatPctileMean stat = new NumberStatPctileMean(100, 0);
            // Add numbers 1-5, but not in sequential order
            Console.WriteLine($"{stat}");
            stat.Add(3);
            Console.WriteLine($"{stat}");
            stat.Add(1);
            Console.WriteLine($"{stat}");
            stat.Add(4);
            Console.WriteLine($"{stat}");
            stat.Add(2);
            Console.WriteLine($"{stat}");
            stat.Add(5);
            Console.WriteLine($"{stat}");
            // check avg
            Assert.AreEqual(3, stat.AverageAll, "wrong avg");
            Assert.AreEqual(5, stat.CountAll, "wrong count");
            Assert.AreEqual(1, stat.MinAll, "wrong min");
            Assert.AreEqual(5, stat.MaxAll, "wrong max");
        }

        /// <summary>
        /// Test for the no-numbers use case
        /// </summary>
        [TestMethod]
        public void TestNumberStatPctileMean_TestZero()
        {
            NumberStatPctileMean stat = new NumberStatPctileMean(100, 0);
            Console.WriteLine($"{stat}");
            // check avg
            Assert.AreEqual(0, stat.AverageAll, "wrong avg");
            Assert.AreEqual(0, stat.CountAll, "wrong count");
            Assert.AreEqual(0, stat.MinAll, "wrong min");
            Assert.AreEqual(0, stat.MaxAll, "wrong max");
        }

        /// <summary>
        /// Test with many numbers (10000)
        /// </summary>
        [TestMethod]
        public void TestNumberStatPctileMean_TestMany()
        {
            NumberStatPctileMean stat = new NumberStatPctileMean(100, 0);
            int n = 10000;
            for (int i = 0; i < n; ++i) stat.Add(i);
            Console.WriteLine($"{stat}");
            // check avg
            Assert.AreEqual(n - 1 - (double)(100-1)/(double)2, stat.Average, "wrong avg");
            Assert.AreEqual((double)(n-1)/(double)2, stat.AverageAll, "wrong avg all");
            Assert.AreEqual(n, stat.CountAll, "wrong count");
            Assert.AreEqual(0, stat.MinAll, "wrong min all");
            Assert.AreEqual(n-1, stat.MaxAll, "wrong max all");
        }

        /// <summary>
        /// Test median with an uneven distribution
        /// </summary>
        [TestMethod]
        public void TestNumberStatPctileMean_TestUnevenDistrib()
        {
            NumberStatPctileMean stat = new NumberStatPctileMean(100, 0);
            int n = 11;
            for (int i = 0; i < n; ++i) stat.Add(i*i);
            Console.WriteLine($"{stat}");
            // check avg
            double avg = stat.Average;
            Console.WriteLine($"avg: {avg}");
            Assert.AreEqual(35, avg, "wrong avg");
            double med = stat.Median;
            Console.WriteLine($"median: {med}");
            Assert.AreEqual(25, med, "wrong avg");
            Assert.AreEqual(n, stat.CountAll, "wrong count");
            Assert.AreEqual(0, stat.MinAll, "wrong min all");
            Assert.AreEqual((n-1)*(n-1), stat.MaxAll, "wrong max all");
            // percentiles
            Assert.IsTrue(Math.Abs(0 - stat.GetPercentile(0)) < 0.000001, "wrong percentile 0");

            Assert.IsTrue(Math.Abs(0.5 - stat.GetPercentile(0.05)) < 0.000001, "wrong percentile 0.05");
            Assert.IsTrue(Math.Abs(4 - stat.GetPercentile(0.2)) < 0.000001, "wrong percentile 0.2");
            Assert.IsTrue(Math.Abs(25 - stat.GetPercentile(0.5)) < 0.000001, "wrong percentile 0.5");
            Assert.IsTrue(Math.Abs(64 - stat.GetPercentile(0.8)) < 0.000001, "wrong percentile 0.8");
            Assert.IsTrue(Math.Abs(81 - stat.GetPercentile(0.9)) < 0.000001, "wrong percentile 0.9");
            Assert.IsTrue(Math.Abs(90.5 - stat.GetPercentile(0.95)) < 0.000001, "wrong percentile 0.95");
            Assert.IsTrue(Math.Abs(96.2 - stat.GetPercentile(0.98)) < 0.000001, "wrong percentile 0.98");
            Assert.IsTrue(Math.Abs(98.1 - stat.GetPercentile(0.99)) < 0.000001, "wrong percentile 0.99");
            Assert.IsTrue(Math.Abs(100 - stat.GetPercentile(1)) < 0.000001, "wrong percentile 1");
        }

        /// <summary>
        /// Test median being updated all the time.
        /// </summary>
        [TestMethod]
        public void TestNumberStatPctileMean_TestMedianUpdate()
        {
            NumberStatPctileMean stat = new NumberStatPctileMean(100, 0);
            stat.Add(1);
            Assert.AreEqual(1, stat.Median, "wrong med 1");
            stat.Add(2);
            Assert.AreEqual(1, stat.Median, "wrong med 2");
            stat.Add(3);
            Assert.AreEqual(2, stat.Median, "wrong med 3");
            stat.Add(4);
            Assert.AreEqual(2, stat.Median, "wrong med 4");
            stat.Add(5);
            Assert.AreEqual(3, stat.Median, "wrong med 5");
            stat.Add(6);
            Assert.AreEqual(3, stat.Median, "wrong med 6");
            stat.Add(7);
            Assert.AreEqual(4, stat.Median, "wrong med 7");
            Assert.AreEqual(7, stat.Count, "wrong count");
            Assert.AreEqual(7, stat.CountAll, "wrong count all");
        }

        /// <summary>
        /// Test how too many values are expired
        /// </summary>
        [TestMethod]
        public void TestNumberStatPctileMean_TestExpireWithCount()
        {
            NumberStatPctileMean stat = new NumberStatPctileMean(2, 0);  // keep only 2
            stat.Add(1);
            Assert.AreEqual(1, stat.Count, "wrong count 1");
            Assert.AreEqual(1, stat.Median, "wrong med 1");
            stat.Add(2);
            Assert.AreEqual(2, stat.Count, "wrong count 2");
            Assert.AreEqual(1, stat.Median, "wrong med 2");
            stat.Add(3);
            Assert.AreEqual(2, stat.Count, "wrong count 3");
            Assert.AreEqual(2, stat.Median, "wrong med 3");
            stat.Add(4);
            Assert.AreEqual(2, stat.Count, "wrong count 4");
            Assert.AreEqual(3, stat.Median, "wrong med 4");
            stat.Add(5);
            Assert.AreEqual(2, stat.Count, "wrong count 5");
            Assert.AreEqual(4, stat.Median, "wrong med 5");
            stat.Add(6);
            Assert.AreEqual(2, stat.Count, "wrong count 6");
            Assert.AreEqual(5, stat.Median, "wrong med 6");
            stat.Add(7);
            Assert.AreEqual(2, stat.Count, "wrong count 7");
            Assert.AreEqual(6, stat.Median, "wrong med 7");
            Console.WriteLine($"{stat}");
        }

        /// <summary>
        /// Test how old values are expired
        /// </summary>
        [TestMethod]
        public void TestNumberStatPctileMean_TestExpireWithTime()
        {
            NumberStatPctileMean stat = new NumberStatPctileMean(100, 150);  // keep only for last 210 secs
            DateTime T0 = DateTime.Now.Subtract(new TimeSpan(1, 0, 0));
            stat.SetCreatedTimeTestonly(T0);
            stat.AddTestOnly(1, T0.AddSeconds(100));
            Assert.AreEqual(1, stat.Count, "wrong count 1");
            Assert.AreEqual(1, stat.Median, "wrong med 1");
            stat.AddTestOnly(2, T0.AddSeconds(200));
            Assert.AreEqual(2, stat.Count, "wrong count 2");
            Assert.AreEqual(1, stat.Median, "wrong med 2");
            stat.AddTestOnly(3, T0.AddSeconds(300));
            Assert.AreEqual(2, stat.Count, "wrong count 3");
            Assert.AreEqual(2, stat.Median, "wrong med 3");
            stat.AddTestOnly(4, T0.AddSeconds(400));
            Assert.AreEqual(2, stat.Count, "wrong count 4");
            Assert.AreEqual(3, stat.Median, "wrong med 4");
            stat.AddTestOnly(5, T0.AddSeconds(500));
            Assert.AreEqual(2, stat.Count, "wrong count 5");
            Assert.AreEqual(4, stat.Median, "wrong med 5");
            stat.AddTestOnly(6, T0.AddSeconds(600));
            Assert.AreEqual(2, stat.Count, "wrong count 6");
            Assert.AreEqual(5, stat.Median, "wrong med 6");
            stat.AddTestOnly(7, T0.AddSeconds(700));
            Assert.AreEqual(2, stat.Count, "wrong count 7");
            Assert.AreEqual(6, stat.Median, "wrong med 7");
            Console.WriteLine($"{stat}");
        }
    }
}
