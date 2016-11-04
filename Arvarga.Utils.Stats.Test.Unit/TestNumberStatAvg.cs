/*
 * See the LICENSE file for copyright and license conditions.
 * (c) 2016 Adam Rotaru
 */

﻿﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Arvarga.Utils.Stats.Test.Unit
{
    [TestClass]
    public class TestNumberStatAvg
    {
        /// <summary>
        /// Test a simple, normal use case
        /// </summary>
        [TestMethod]
        public void TestNumberStatAvg_TestSimple()
        {
            NumberStatAvg stat = new NumberStatAvg();
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
            Assert.AreEqual(3, stat.Average, "wrong avg");
            Assert.AreEqual(5, stat.Count, "wrong count");
            Assert.AreEqual(1, stat.Min, "wrong min");
            Assert.AreEqual(5, stat.Max, "wrong max");
        }

        /// <summary>
        /// Test for the no-numbers use case
        /// </summary>
        [TestMethod]
        public void TestNumberStatAvg_TestZero()
        {
            NumberStatAvg stat = new NumberStatAvg();
            Console.WriteLine($"{stat}");
            // check avg
            Assert.AreEqual(0, stat.Average, "wrong avg");
            Assert.AreEqual(0, stat.Count, "wrong count");
            Assert.AreEqual(0, stat.Min, "wrong min");
            Assert.AreEqual(0, stat.Max, "wrong max");
        }

        /// <summary>
        /// Test with many numbers (10000)
        /// </summary>
        [TestMethod]
        public void TestMany()
        {
            NumberStatAvg stat = new NumberStatAvg();
            int n = 10000;
            for (int i = 0; i < n; ++i) stat.Add(i);
            Console.WriteLine($"{stat}");
            // check avg
            Assert.AreEqual((double)(n - 1) / (double)2, stat.Average, "wrong avg");
            Assert.AreEqual(n, stat.Count, "wrong count");
            Assert.AreEqual(0, stat.Min, "wrong min");
            Assert.AreEqual(n-1, stat.Max, "wrong max");
        }

        /// <summary>
        /// Test median being updated all the time.
        /// </summary>
        [TestMethod]
        public void TestNumberStatAvg_TestMedianUpdate()
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
    }
}
