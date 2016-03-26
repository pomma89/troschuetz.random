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

namespace Troschuetz.Random.Tests.Continuous
{
    using Distributions.Continuous;
    using NUnit.Framework;
    using System;

    public sealed class BetaDistributionTests : ContinuousDistributionTests<BetaDistribution>
    {
        protected override BetaDistribution GetDist(BetaDistribution other = null)
        {
            return new BetaDistribution { Alpha = GetAlpha(other), Beta = GetBeta(other) };
        }

        protected override BetaDistribution GetDist(uint seed, BetaDistribution other = null)
        {
            return new BetaDistribution(seed) { Alpha = GetAlpha(other), Beta = GetBeta(other) };
        }

        protected override BetaDistribution GetDist(IGenerator gen, BetaDistribution other = null)
        {
            return new BetaDistribution(gen) { Alpha = GetAlpha(other), Beta = GetBeta(other) };
        }

        protected override BetaDistribution GetDistWithParams(BetaDistribution other = null)
        {
            return new BetaDistribution(GetAlpha(other), GetBeta(other));
        }

        protected override BetaDistribution GetDistWithParams(uint seed, BetaDistribution other = null)
        {
            return new BetaDistribution(seed, GetAlpha(other), GetBeta(other));
        }

        protected override BetaDistribution GetDistWithParams(IGenerator gen, BetaDistribution other = null)
        {
            return new BetaDistribution(gen, GetAlpha(other), GetBeta(other));
        }

        [TestCase(double.NaN)]
        [TestCase(0)]
        [TestCase(TinyNeg)]
        [TestCase(SmallNeg)]
        [TestCase(LargeNeg)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Alpha_WrongValues(double d)
        {
            Assert.False(BetaDistribution.AreValidParams(d, 1));
            Assert.False(Dist.IsValidAlpha(d));
            Dist.Alpha = d;
        }

        [TestCase(double.NaN)]
        [TestCase(0)]
        [TestCase(TinyNeg)]
        [TestCase(SmallNeg)]
        [TestCase(LargeNeg)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Beta_WrongValues(double d)
        {
            Assert.False(BetaDistribution.AreValidParams(1, d));
            Assert.False(Dist.IsValidBeta(d));
            Dist.Beta = d;
        }

        // alpha > 0
        double GetAlpha(IAlphaDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(10) : d.Alpha;
        }

        // beta > 0
        double GetBeta(IBetaDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(10) : d.Beta;
        }
    }
}
