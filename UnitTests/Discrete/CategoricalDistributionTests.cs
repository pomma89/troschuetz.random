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

namespace Troschuetz.Random.Tests.Discrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Distributions.Discrete;
    using NUnit.Framework;

    public sealed class CategoricalDistributionTests : DiscreteDistributionTests<CategoricalDistribution>
    {
        protected override CategoricalDistribution GetDist(CategoricalDistribution other = null)
        {
            return new CategoricalDistribution(GetValueCount(other));
        }

        protected override CategoricalDistribution GetDist(uint seed, CategoricalDistribution other = null)
        {
            return new CategoricalDistribution(seed, GetValueCount(other));
        }

        protected override CategoricalDistribution GetDist(IGenerator gen, CategoricalDistribution other = null)
        {
            return new CategoricalDistribution(gen, GetValueCount(other));
        }

        protected override CategoricalDistribution GetDistWithParams(CategoricalDistribution other = null)
        {
            return new CategoricalDistribution(GetWeights(other));
        }

        protected override CategoricalDistribution GetDistWithParams(uint seed, CategoricalDistribution other = null)
        {
            return new CategoricalDistribution(seed, GetWeights(other));
        }

        protected override CategoricalDistribution GetDistWithParams(IGenerator gen, CategoricalDistribution other = null)
        {
            return new CategoricalDistribution(gen, GetWeights(other));
        }

        [TestCase(double.NaN, 1, 2)]
        [TestCase(SmallNeg, 1, 2)]
        [TestCase(1, LargeNeg, 2)]
        [TestCase(1, 2, TinyNeg)]
        [TestCase(0, -2, 2)]
        [TestCase(-3, -2, -1)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Weights_WrongValues(double d1, double d2, double d3)
        {
            var w = new List<double> {d1, d2, d3};
            Assert.False(CategoricalDistribution.IsValidParam(w));
            Assert.False(Dist.AreValidWeights(w));
            Dist.Weights = w;
        }

        [Test]
        public void Median_EvenEquiWeights()
        {
            Dist = new CategoricalDistribution(new double[] {1, 1, 1, 1, 1, 1});
            for (var i = 0; i < Iterations; ++i)
                Results[i] = Dist.Next();
            AssertDist(Dist);
        }

        [Test]
        public void Median_OddEquiWeights()
        {
            Dist = new CategoricalDistribution(new double[] {1, 1, 1, 1, 1});
            for (var i = 0; i < Iterations; ++i)
                Results[i] = Dist.Next();
            AssertDist(Dist);
        }

        // value count > 0
        int GetValueCount(IWeightsDistribution<double> d)
        {
            return d == null ? Rand.Next(1, 10) : d.Weights.Count();
        }

        // all weights > 0
        ICollection<double> GetWeights(IWeightsDistribution<double> d)
        {
            Func<double> r = () => Rand.NextDouble(0.1, 10);
            return d == null ? new List<double> {r(), r(), r(), r(), r()} : d.Weights;
        }
    }
}