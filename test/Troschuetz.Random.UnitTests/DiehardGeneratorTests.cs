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

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Troschuetz.Random.Tests
{
    /// <summary>
    ///   Unit tests taken from the DIEHARD test suite.
    /// </summary>
    public abstract partial class GeneratorTests
    {
        /// <summary>
        ///   Choose random points on a large interval. The spacings between the points should be
        ///   asymptotically exponentially distributed. The name is based on the birthday paradox.
        /// </summary>
        [Test]
        public void Diehard_BirthdaySpacings()
        {
            const int days = 365;
            const int sampleCount = 1000;

            var samples = _generator.Integers(days).Take(sampleCount).ToArray();
            var distances = new List<double>(sampleCount * sampleCount);

            //Parallel.For(0, sampleCount, i =>
            //{
            //    Parallel.For(0, sampleCount, j =>
            //    {
            //        distances.Add(Math.Abs(samples[i] - samples[j]));
            //    });
            //});

            for (var i = 0; i < samples.Length; ++i)
            {
                for (var j = 0; j < samples.Length; ++j)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    distances.Add(Math.Abs(samples[i] - samples[j]));
                }
            }

            distances.Sort();

            var mean = ComputeMean(distances);
            var lambda = (distances.Count - 2) / (mean * distances.Count);
            var lambdaLow = lambda * (1 - 1.96 / Math.Sqrt(distances.Count));
            var lambdaUpp = lambda * (1 + 1.96 / Math.Sqrt(distances.Count));

            var median = ComputeMedian(distances);
            var medianLow = Math.Log(2.0) / lambdaUpp;
            var medianUpp = Math.Log(2.0) / lambdaLow;

            const double adj = 1.28; // Factor found while testing...
            Assert.True(ApproxEquals(median / adj, medianLow), $"Generator {_generator.GetType().Name} failed: {median} < {medianLow}");
            Assert.True(ApproxEquals(median / adj, medianUpp), $"Generator {_generator.GetType().Name} failed: {median} > {medianUpp}");
        }
    }
}