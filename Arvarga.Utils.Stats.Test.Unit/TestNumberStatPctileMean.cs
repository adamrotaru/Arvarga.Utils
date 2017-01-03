/*
 * See the LICENSE file for copyright and license conditions.
 * (c) 2016 Adam Rotaru
 */

using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;

namespace Arvarga.Utils.Stats.Test.Unit
{
    //[TestClass]
    public class TestNumberStatPctileMean
    {
        /// <summary>
        /// Test a simple, normal use case
        /// </summary>
        //[TestMethod]
        [Fact]
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
            Assert.Equal<double>(3, stat.AverageAll);
            Assert.Equal<long>(5, stat.CountAll);
            Assert.Equal<double>(1, stat.MinAll);
            Assert.Equal<double>(5, stat.MaxAll);
        }

        /// <summary>
        /// Test for the no-numbers use case
        /// </summary>
        //[TestMethod]
        [Fact]
        public void TestNumberStatPctileMean_TestZero()
        {
            NumberStatPctileMean stat = new NumberStatPctileMean(100, 0);
            Console.WriteLine($"{stat}");
            // check avg
            Assert.Equal<double>(0, stat.AverageAll);
            Assert.Equal<long>(0, stat.CountAll);
            Assert.Equal<double>(0, stat.MinAll);
            Assert.Equal<double>(0, stat.MaxAll);
        }

        /// <summary>
        /// Test with many numbers (10000)
        /// </summary>
        //[TestMethod]
        [Fact]
        public void TestNumberStatPctileMean_TestMany()
        {
            NumberStatPctileMean stat = new NumberStatPctileMean(100, 0);
            int n = 10000;
            for (int i = 0; i < n; ++i) stat.Add(i);
            Console.WriteLine($"{stat}");
            // check avg
            Assert.Equal<double>(n - 1 - (double)(100-1)/(double)2, stat.Average);
            Assert.Equal<double>((double)(n-1)/(double)2, stat.AverageAll);
            Assert.Equal<long>(n, stat.CountAll);
            Assert.Equal<double>(0, stat.MinAll);
            Assert.Equal<double>(n-1, stat.MaxAll);
        }

        /// <summary>
        /// Test median with an uneven distribution
        /// </summary>
        //[TestMethod]
        [Fact]
        public void TestNumberStatPctileMean_TestUnevenDistrib()
        {
            NumberStatPctileMean stat = new NumberStatPctileMean(100, 0);
            int n = 11;
            for (int i = 0; i < n; ++i) stat.Add(i*i);
            Console.WriteLine($"{stat}");
            // check avg
            double avg = stat.Average;
            Console.WriteLine($"avg: {avg}");
            Assert.Equal<double>(35, avg);
            double med = stat.Median;
            Console.WriteLine($"median: {med}");
            Assert.Equal<double>(25, med);
            Assert.Equal<long>(n, stat.CountAll);
            Assert.Equal<double>(0, stat.MinAll);
            Assert.Equal<double>((n-1)*(n-1), stat.MaxAll);
            // percentiles
            Assert.True(Math.Abs(0 - stat.GetPercentile(0)) < 0.000001, "wrong percentile 0");

            Assert.True(Math.Abs(0.5 - stat.GetPercentile(0.05)) < 0.000001, "wrong percentile 0.05");
            Assert.True(Math.Abs(4 - stat.GetPercentile(0.2)) < 0.000001, "wrong percentile 0.2");
            Assert.True(Math.Abs(25 - stat.GetPercentile(0.5)) < 0.000001, "wrong percentile 0.5");
            Assert.True(Math.Abs(64 - stat.GetPercentile(0.8)) < 0.000001, "wrong percentile 0.8");
            Assert.True(Math.Abs(81 - stat.GetPercentile(0.9)) < 0.000001, "wrong percentile 0.9");
            Assert.True(Math.Abs(90.5 - stat.GetPercentile(0.95)) < 0.000001, "wrong percentile 0.95");
            Assert.True(Math.Abs(96.2 - stat.GetPercentile(0.98)) < 0.000001, "wrong percentile 0.98");
            Assert.True(Math.Abs(98.1 - stat.GetPercentile(0.99)) < 0.000001, "wrong percentile 0.99");
            Assert.True(Math.Abs(100 - stat.GetPercentile(1)) < 0.000001, "wrong percentile 1");
        }

        /// <summary>
        /// Test median being updated all the time.
        /// </summary>
        //[TestMethod]
        [Fact]
        public void TestNumberStatPctileMean_TestMedianUpdate()
        {
            NumberStatPctileMean stat = new NumberStatPctileMean(100, 0);
            stat.Add(1);
            Assert.Equal<double>(1, stat.Median);
            stat.Add(2);
            Assert.Equal<double>(1, stat.Median);
            stat.Add(3);
            Assert.Equal<double>(2, stat.Median);
            stat.Add(4);
            Assert.Equal<double>(2, stat.Median);
            stat.Add(5);
            Assert.Equal<double>(3, stat.Median);
            stat.Add(6);
            Assert.Equal<double>(3, stat.Median);
            stat.Add(7);
            Assert.Equal<double>(4, stat.Median);
            Assert.Equal<int>(7, stat.Count);
            Assert.Equal<long>(7, stat.CountAll);
        }

        /// <summary>
        /// Test how too many values are expired
        /// </summary>
        //[TestMethod]
        [Fact]
        public void TestNumberStatPctileMean_TestExpireWithCount()
        {
            NumberStatPctileMean stat = new NumberStatPctileMean(2, 0);  // keep only 2
            stat.Add(1);
            Assert.Equal<int>(1, stat.Count);
            Assert.Equal<double>(1, stat.Median);
            stat.Add(2);
            Assert.Equal<int>(2, stat.Count);
            Assert.Equal<double>(1, stat.Median);
            stat.Add(3);
            Assert.Equal<int>(2, stat.Count);
            Assert.Equal<double>(2, stat.Median);
            stat.Add(4);
            Assert.Equal<int>(2, stat.Count);
            Assert.Equal<double>(3, stat.Median);
            stat.Add(5);
            Assert.Equal<int>(2, stat.Count);
            Assert.Equal<double>(4, stat.Median);
            stat.Add(6);
            Assert.Equal<int>(2, stat.Count);
            Assert.Equal<double>(5, stat.Median);
            stat.Add(7);
            Assert.Equal<int>(2, stat.Count);
            Assert.Equal<double>(6, stat.Median);
            Console.WriteLine($"{stat}");
        }

        /// <summary>
        /// Test how old values are expired
        /// </summary>
        //[TestMethod]
        [Fact]
        public void TestNumberStatPctileMean_TestExpireWithTime()
        {
            NumberStatPctileMean stat = new NumberStatPctileMean(100, 150);  // keep only for last 210 secs
            DateTime T0 = DateTime.Now.Subtract(new TimeSpan(1, 0, 0));
            stat.SetCreatedTimeTestonly(T0);
            stat.AddTestOnly(1, T0.AddSeconds(100));
            Assert.Equal<int>(1, stat.Count);
            Assert.Equal<double>(1, stat.Median);
            stat.AddTestOnly(2, T0.AddSeconds(200));
            Assert.Equal<int>(2, stat.Count);
            Assert.Equal<double>(1, stat.Median);
            stat.AddTestOnly(3, T0.AddSeconds(300));
            Assert.Equal<int>(2, stat.Count);
            Assert.Equal<double>(2, stat.Median);
            stat.AddTestOnly(4, T0.AddSeconds(400));
            Assert.Equal<int>(2, stat.Count);
            Assert.Equal<double>(3, stat.Median);
            stat.AddTestOnly(5, T0.AddSeconds(500));
            Assert.Equal<int>(2, stat.Count);
            Assert.Equal<double>(4, stat.Median);
            stat.AddTestOnly(6, T0.AddSeconds(600));
            Assert.Equal<int>(2, stat.Count);
            Assert.Equal<double>(5, stat.Median);
            stat.AddTestOnly(7, T0.AddSeconds(700));
            Assert.Equal<int>(2, stat.Count);
            Assert.Equal<double>(6, stat.Median);
            Console.WriteLine($"{stat}");
        }
    }
}
