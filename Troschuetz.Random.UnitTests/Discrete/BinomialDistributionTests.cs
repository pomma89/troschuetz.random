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
    using Distributions.Discrete;
    using NUnit.Framework;
    using System;

    public sealed class BinomialDistributionTests : DiscreteDistributionTests<BinomialDistribution>
    {
        protected override BinomialDistribution GetDist(BinomialDistribution other = null)
        {
            return new BinomialDistribution { Alpha = GetAlpha(other), Beta = GetBeta(other) };
        }

        protected override BinomialDistribution GetDist(uint seed, BinomialDistribution other = null)
        {
            return new BinomialDistribution(seed) { Alpha = GetAlpha(other), Beta = GetBeta(other) };
        }

        protected override BinomialDistribution GetDist(IGenerator gen, BinomialDistribution other = null)
        {
            return new BinomialDistribution(gen) { Alpha = GetAlpha(other), Beta = GetBeta(other) };
        }

        protected override BinomialDistribution GetDistWithParams(BinomialDistribution other = null)
        {
            return new BinomialDistribution(GetAlpha(other), GetBeta(other));
        }

        protected override BinomialDistribution GetDistWithParams(uint seed, BinomialDistribution other = null)
        {
            return new BinomialDistribution(seed, GetAlpha(other), GetBeta(other));
        }

        protected override BinomialDistribution GetDistWithParams(IGenerator gen, BinomialDistribution other = null)
        {
            return new BinomialDistribution(gen, GetAlpha(other), GetBeta(other));
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
            Assert.False(BinomialDistribution.AreValidParams(d, 1));
            Assert.False(Dist.IsValidAlpha(d));
            Dist.Alpha = d;
        }

        [TestCase(double.NaN)]
        [TestCase(SmallNeg)]
        [TestCase(LargeNeg)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Beta_WrongValues(double d)
        {
            var i = (int) d;
            Assert.False(BinomialDistribution.AreValidParams(0.5, i));
            Assert.False(Dist.IsValidBeta(i));
            Dist.Beta = i;
        }

        // alpha >= 0 && alpha <= 1
        double GetAlpha(IAlphaDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(0, 1) : d.Alpha;
        }

        // beta >= 0
        int GetBeta(IBetaDistribution<int> d)
        {
            return d == null ? Rand.Next(1, 10) : d.Beta;
        }
    }
}
