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

    public sealed class CauchyDistributionTests : ContinuousDistributionTests<CauchyDistribution>
    {
        protected override CauchyDistribution GetDist(CauchyDistribution other = null)
        {
            return new CauchyDistribution { Alpha = GetAlpha(other), Gamma = GetGamma(other) };
        }

        protected override CauchyDistribution GetDist(uint seed, CauchyDistribution other = null)
        {
            return new CauchyDistribution(seed) { Alpha = GetAlpha(other), Gamma = GetGamma(other) };
        }

        protected override CauchyDistribution GetDist(IGenerator gen, CauchyDistribution other = null)
        {
            return new CauchyDistribution(gen) { Alpha = GetAlpha(other), Gamma = GetGamma(other) };
        }

        protected override CauchyDistribution GetDistWithParams(CauchyDistribution other = null)
        {
            return new CauchyDistribution(GetAlpha(other), GetGamma(other));
        }

        protected override CauchyDistribution GetDistWithParams(uint seed, CauchyDistribution other = null)
        {
            return new CauchyDistribution(seed, GetAlpha(other), GetGamma(other));
        }

        protected override CauchyDistribution GetDistWithParams(IGenerator gen, CauchyDistribution other = null)
        {
            return new CauchyDistribution(gen, GetAlpha(other), GetGamma(other));
        }

        [TestCase(double.NaN)]
        [TestCase(0)]
        [TestCase(TinyNeg)]
        [TestCase(SmallNeg)]
        [TestCase(LargeNeg)]
        public void Gamma_WrongValues(double d)
        {
            Assert.False(CauchyDistribution.AreValidParams(1, d));
            Assert.False(Dist.IsValidGamma(d));
            Assert.Throws<ArgumentOutOfRangeException>(() => { Dist.Gamma = d; });
        }

        // any value
        private double GetAlpha(IAlphaDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(-10, 10) : d.Alpha;
        }

        // gamma > 0
        private double GetGamma(IGammaDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(0.1, 10) : d.Gamma;
        }
    }
}