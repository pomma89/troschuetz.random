/*
 * Copyright © 2012 Alessio Parma (alessio.parma@gmail.com)
 * 
 * This file is part of Troschuetz.Random.Tests Class Library.
 * 
 * Troschuetz.Random is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA 
 */

namespace Troschuetz.Random.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public abstract class TestBase
    {
        const double Epsilon = 0.20; // Relative error: less than 20%
        protected const int Iterations = 200000;

        protected const double TinyNeg = -0.01;
        protected const double SmallNeg = -1;
        protected const double LargeNeg = -100;

        protected const double TinyPos = -TinyNeg;
        protected const double SmallPos = -SmallNeg;
        protected const double LargePos = -LargeNeg;

        protected readonly double[] Results = new double[Iterations];
        protected readonly TRandom Rand = new TRandom();

        protected void AssertDist(IDistribution dist)
        {
            var filtered = FilterSeries(Results, dist);
            var mean = ComputeMean(filtered);
            var median = ComputeMedian(filtered);
            var variance = ComputeVariance(filtered, mean);

            try {
                Assert.NotNull(dist.Mode);
                Assert.False(dist.Mode.Any(double.IsNaN));
            } catch (NotSupportedException) {
                // Mode may be undefined
            }
            
            try {
                var c = ApproxEquals(dist.Mean, mean);
                Assert.True(c, "Wrong mean! Expected ({0}), found ({1})", dist.Mean, mean);
            } catch (NotSupportedException) {
                // Mean may be undefined
            }
            
            
            try {
                var c = ApproxEquals(dist.Median, median);
                Assert.True(c, "Wrong median! Expected ({0}), found ({1})", dist.Median, median);
            } catch (NotSupportedException) {
                // Median may be undefined
            }

            try {
                var c = ApproxEquals(dist.Variance, variance);
                Assert.True(c, "Wrong variance! Expected ({0}), found ({1})", dist.Variance, variance);
            } catch (NotSupportedException) {
                // Variance may be undefined
            }  
        }

        static IList<double> FilterSeries(double[] series, IDistribution dist)
        {
            Assert.True(series.All(x => x >= dist.Minimum && x <= dist.Maximum));
            Func<double, bool> filter = (d => !double.IsPositiveInfinity(d) && !double.IsNegativeInfinity(d));
            var filtered = series.Where(filter).ToList();
            filtered.Sort();
            return filtered;
        }

        static double ComputeMean(ICollection<double> series)
        {
            return series.Select(x => x/series.Count).Sum();
        }

        static double ComputeMedian(IList<double> series)
        {
            var hc = series.Count/2;
            if (hc%2 == 0)
                return series[hc - 1]/2 + series[hc]/2;
            return series[hc];
        }

        static double ComputeVariance(ICollection<double> series, double mean)
        {
            return series.Select(x => Math.Pow(x - mean, 2)/(series.Count - 1)).Sum();
        }

        static bool ApproxEquals(double expected, double observed)
        {
            if (double.IsNaN(expected)) {
                Assert.Fail("NaN should not be returned");
            }
            if (expected.Equals(0)) {
                return Math.Abs(expected - observed) < Epsilon;
            }
            return Math.Abs((expected - observed)/expected) < Epsilon;
        }
    }
}