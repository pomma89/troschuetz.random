// The MIT License (MIT)
//
// Copyright (c) 2006-2007 Stefan Troschütz <stefan@troschuetz.de>
//
// Copyright (c) 2012-2019 Alessio Parma <alessio.parma@gmail.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute,
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT
// OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Troschuetz.Random.Tests
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    public abstract class TestBase
    {
        private const double Epsilon = 0.20; // Relative error: less than 20%
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

            try
            {
                Assert.NotNull(dist.Mode);
                Assert.False(dist.Mode.Any(double.IsNaN));
            }
#pragma warning disable CC0004 // Catch block cannot be empty
            catch (NotSupportedException)
            {
                // Mode may be undefined
            }
#pragma warning restore CC0004 // Catch block cannot be empty

            try
            {
                var c = ApproxEquals(dist.Mean, mean);
                Assert.True(c, "Wrong mean! Expected ({0}), found ({1})", dist.Mean, mean);
            }
#pragma warning disable CC0004 // Catch block cannot be empty
            catch (NotSupportedException)
            {
                // Mean may be undefined
            }
#pragma warning restore CC0004 // Catch block cannot be empty

            try
            {
                var c = ApproxEquals(dist.Median, median);
                Assert.True(c, "Wrong median! Expected ({0}), found ({1})", dist.Median, median);
            }
#pragma warning disable CC0004 // Catch block cannot be empty
            catch (NotSupportedException)
            {
                // Median may be undefined
            }
#pragma warning restore CC0004 // Catch block cannot be empty

            try
            {
                var c = ApproxEquals(dist.Variance, variance);
                Assert.True(c, "Wrong variance! Expected ({0}), found ({1})", dist.Variance, variance);
            }
#pragma warning disable CC0004 // Catch block cannot be empty
            catch (NotSupportedException)
            {
                // Variance may be undefined
            }
#pragma warning restore CC0004 // Catch block cannot be empty
        }

        private static IList<double> FilterSeries(double[] series, IDistribution dist)
        {
            Assert.True(series.All(x => x >= dist.Minimum && x <= dist.Maximum));
            Func<double, bool> filter = (d => !double.IsPositiveInfinity(d) && !double.IsNegativeInfinity(d));
            var filtered = series.Where(filter).ToList();
            filtered.Sort();
            return filtered;
        }

        protected static double ComputeMean(ICollection<double> series)
        {
            return series.Select(x => x / series.Count).Sum();
        }

        protected static double ComputeMedian(IList<double> series)
        {
            var hc = series.Count / 2;
            if (hc % 2 == 0)
                return series[hc - 1] / 2.0 + series[hc] / 2.0;
            return series[hc];
        }

        protected static double ComputeVariance(ICollection<double> series, double mean)
        {
            return series.Select(x => Math.Pow(x - mean, 2) / (series.Count - 1)).Sum();
        }

        protected static bool ApproxEquals(double expected, double observed)
        {
            return ApproxEquals(expected, observed, Epsilon);
        }

        protected static bool ApproxEquals(double expected, double observed, double epsilon)
        {
            if (double.IsNaN(expected))
            {
                Assert.Fail("NaN should not be returned");
            }
            if (expected.Equals(0))
            {
                return Math.Abs(expected - observed) < epsilon;
            }
            return Math.Abs((expected - observed) / expected) < epsilon;
        }
    }
}