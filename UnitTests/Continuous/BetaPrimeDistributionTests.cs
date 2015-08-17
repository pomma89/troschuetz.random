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

    public sealed class BetaPrimeDistributionTests : ContinuousDistributionTests<BetaPrimeDistribution>
    {
        protected override BetaPrimeDistribution GetDist(BetaPrimeDistribution other = null)
        {
            return new BetaPrimeDistribution { Alpha = GetAlpha(other), Beta = GetBeta(other) };
        }

        protected override BetaPrimeDistribution GetDist(uint seed, BetaPrimeDistribution other = null)
        {
            return new BetaPrimeDistribution(seed) { Alpha = GetAlpha(other), Beta = GetBeta(other) };
        }

        protected override BetaPrimeDistribution GetDist(IGenerator gen, BetaPrimeDistribution other = null)
        {
            return new BetaPrimeDistribution(gen) { Alpha = GetAlpha(other), Beta = GetBeta(other) };
        }

        protected override BetaPrimeDistribution GetDistWithParams(BetaPrimeDistribution other = null)
        {
            return new BetaPrimeDistribution(GetAlpha(other), GetBeta(other));
        }

        protected override BetaPrimeDistribution GetDistWithParams(uint seed, BetaPrimeDistribution other = null)
        {
            return new BetaPrimeDistribution(seed, GetAlpha(other), GetBeta(other));
        }

        protected override BetaPrimeDistribution GetDistWithParams(IGenerator gen, BetaPrimeDistribution other = null)
        {
            return new BetaPrimeDistribution(gen, GetAlpha(other), GetBeta(other));
        }

        [TestCase(double.NaN)]
        [TestCase(1 + TinyNeg)]
        [TestCase(0)]
        [TestCase(SmallNeg)]
        [TestCase(LargeNeg)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Alpha_WrongValues(double d)
        {
            Assert.False(BetaPrimeDistribution.AreValidParams(d, 2));
            Assert.False(Dist.IsValidAlpha(d));
            Dist.Alpha = d;
        }

        [TestCase(double.NaN)]
        [TestCase(1 + TinyNeg)]
        [TestCase(0)]
        [TestCase(SmallNeg)]
        [TestCase(LargeNeg)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Beta_WrongValues(double d)
        {
            Assert.False(BetaPrimeDistribution.AreValidParams(2, d));
            Assert.False(Dist.IsValidBeta(d));
            Dist.Beta = d;
        }

        // alpha > 1
        double GetAlpha(IAlphaDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(1.1, 10) : d.Alpha;
        }

        // beta > 1
        double GetBeta(IBetaDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(1.1, 10) : d.Beta;
        }
    }
}
