/*
 * Copyright © 2006 Stefan Troschütz (stefan@troschuetz.de)
 * Copyright © 2012-2016 Alessio Parma (alessio.parma@gmail.com)
 *
 * This file is part of Troschuetz.Random Class Library.
 *
 * Troschuetz.Random is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *
 * See the GNU Lesser General Public License for more details.
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
 */

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
