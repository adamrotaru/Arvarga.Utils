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
    public class TestNumberStatAvg
    {
        /// <summary>
        /// Test a simple, normal use case
        /// </summary>
        //[TestMethod]
        [Fact]
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
            Assert.Equal<double>(3, stat.Average);
            Assert.Equal<int>(5, stat.Count);
            Assert.Equal<double>(1, stat.Min);
            Assert.Equal<double>(5, stat.Max);
        }

        /// <summary>
        /// Test for the no-numbers use case
        /// </summary>
        //[TestMethod]
        [Fact]
        public void TestNumberStatAvg_TestZero()
        {
            NumberStatAvg stat = new NumberStatAvg();
            Console.WriteLine($"{stat}");
            // check avg
            Assert.Equal<double>(0, stat.Average);
            Assert.Equal<int>(0, stat.Count);
            Assert.Equal<double>(0, stat.Min);
            Assert.Equal<double>(0, stat.Max);
        }

        /// <summary>
        /// Test with many numbers (10000)
        /// </summary>
        //[TestMethod]
        [Fact]
        public void TestMany()
        {
            NumberStatAvg stat = new NumberStatAvg();
            int n = 10000;
            for (int i = 0; i < n; ++i) stat.Add(i);
            Console.WriteLine($"{stat}");
            // check avg
            Assert.Equal<double>((double)(n - 1) / (double)2, stat.Average);
            Assert.Equal<int>(n, stat.Count);
            Assert.Equal<double>(0, stat.Min);
            Assert.Equal<double>(n-1, stat.Max);
        }

        /// <summary>
        /// Test median being updated all the time.
        /// </summary>
        //[TestMethod]
        [Fact]
        public void TestNumberStatAvg_TestMedianUpdate()
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
    }
}
