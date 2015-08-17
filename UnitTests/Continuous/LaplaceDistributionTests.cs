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

    public sealed class LaplaceDistributionTests : ContinuousDistributionTests<LaplaceDistribution>
    {
        protected override LaplaceDistribution GetDist(LaplaceDistribution other = null)
        {
            return new LaplaceDistribution { Alpha = GetAlpha(other), Mu = GetMu(other) };
        }

        protected override LaplaceDistribution GetDist(uint seed, LaplaceDistribution other = null)
        {
            return new LaplaceDistribution(seed) { Alpha = GetAlpha(other), Mu = GetMu(other) };
        }

        protected override LaplaceDistribution GetDist(IGenerator gen, LaplaceDistribution other = null)
        {
            return new LaplaceDistribution(gen) { Alpha = GetAlpha(other), Mu = GetMu(other) };
        }

        protected override LaplaceDistribution GetDistWithParams(LaplaceDistribution other = null)
        {
            return new LaplaceDistribution(GetAlpha(other), GetMu(other));
        }

        protected override LaplaceDistribution GetDistWithParams(uint seed, LaplaceDistribution other = null)
        {
            return new LaplaceDistribution(seed, GetAlpha(other), GetMu(other));
        }

        protected override LaplaceDistribution GetDistWithParams(IGenerator gen, LaplaceDistribution other = null)
        {
            return new LaplaceDistribution(gen, GetAlpha(other), GetMu(other));
        }

        [TestCase(double.NaN)]
        [TestCase(0)]
        [TestCase(TinyNeg)]
        [TestCase(SmallNeg)]
        [TestCase(LargeNeg)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Alpha_WrongValues(double d)
        {
            Assert.False(LaplaceDistribution.AreValidParams(d, 1));
            Assert.False(Dist.IsValidAlpha(d));
            Dist.Alpha = d;
        }

        // alpha > 0
        double GetAlpha(IAlphaDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(0.1, 10) : d.Alpha;
        }

        // any value
        double GetMu(IMuDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(-10, 10) : d.Mu;
        }
    }
}
