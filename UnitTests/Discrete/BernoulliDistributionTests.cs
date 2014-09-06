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
    using Distributions.Discrete;
    using NUnit.Framework;

    public sealed class BernoulliDistributionTests : DiscreteDistributionTests<BernoulliDistribution>
    {
        protected override BernoulliDistribution GetDist(BernoulliDistribution other = null)
        {
            return new BernoulliDistribution {Alpha = GetAlpha(other)};
        }

        protected override BernoulliDistribution GetDist(uint seed, BernoulliDistribution other = null)
        {
            return new BernoulliDistribution(seed) {Alpha = GetAlpha(other)};
        }

        protected override BernoulliDistribution GetDist(IGenerator gen, BernoulliDistribution other = null)
        {
            return new BernoulliDistribution(gen) {Alpha = GetAlpha(other)};
        }

        protected override BernoulliDistribution GetDistWithParams(BernoulliDistribution other = null)
        {
            return new BernoulliDistribution(GetAlpha(other));
        }

        protected override BernoulliDistribution GetDistWithParams(uint seed, BernoulliDistribution other = null)
        {
            return new BernoulliDistribution(seed, GetAlpha(other));
        }

        protected override BernoulliDistribution GetDistWithParams(IGenerator gen, BernoulliDistribution other = null)
        {
            return new BernoulliDistribution(gen, GetAlpha(other));
        }
        
        [TestCase(double.NaN)]
        [TestCase(TinyNeg)]
        [TestCase(SmallNeg)]
        [TestCase(LargeNeg)]
        [TestCase(1 + TinyPos)]
        [TestCase(1 + SmallPos)]
        [TestCase(1 + LargePos)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Alpha_WrongValues(double d)
        {
            Assert.False(BernoulliDistribution.IsValidParam(d));
            Assert.False(Dist.IsValidAlpha(d));
            Dist.Alpha = d;
        }

        // alpha >= 0.0 && alpha <= 1.0
        double GetAlpha(IAlphaDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(0, 1) : d.Alpha;
        }
    }
}