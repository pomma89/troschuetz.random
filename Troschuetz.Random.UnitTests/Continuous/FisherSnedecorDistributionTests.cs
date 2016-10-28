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

    public sealed class FisherSnedecorDistributionTests : ContinuousDistributionTests<FisherSnedecorDistribution>
    {
        protected override FisherSnedecorDistribution GetDist(FisherSnedecorDistribution other = null)
        {
            return new FisherSnedecorDistribution { Alpha = GetAlpha(other), Beta = GetBeta(other) };
        }

        protected override FisherSnedecorDistribution GetDist(uint seed, FisherSnedecorDistribution other = null)
        {
            return new FisherSnedecorDistribution(seed) { Alpha = GetAlpha(other), Beta = GetBeta(other) };
        }

        protected override FisherSnedecorDistribution GetDist(IGenerator gen, FisherSnedecorDistribution other = null)
        {
            return new FisherSnedecorDistribution(gen) { Alpha = GetAlpha(other), Beta = GetBeta(other) };
        }

        protected override FisherSnedecorDistribution GetDistWithParams(FisherSnedecorDistribution other = null)
        {
            return new FisherSnedecorDistribution(GetAlpha(other), GetBeta(other));
        }

        protected override FisherSnedecorDistribution GetDistWithParams(uint seed, FisherSnedecorDistribution other = null)
        {
            return new FisherSnedecorDistribution(seed, GetAlpha(other), GetBeta(other));
        }

        protected override FisherSnedecorDistribution GetDistWithParams(IGenerator gen, FisherSnedecorDistribution other = null)
        {
            return new FisherSnedecorDistribution(gen, GetAlpha(other), GetBeta(other));
        }

        [TestCase(double.NaN)]
        [TestCase(0)]
        [TestCase(SmallNeg)]
        [TestCase(LargeNeg)]
        public void Alpha_WrongValues(double d)
        {
            var i = (int) d;
            Assert.False(FisherSnedecorDistribution.AreValidParams(i, 1));
            Assert.False(Dist.IsValidAlpha(i));
            Assert.Throws<ArgumentOutOfRangeException>(() => { Dist.Alpha = i; });
        }

        [TestCase(double.NaN)]
        [TestCase(0)]
        [TestCase(SmallNeg)]
        [TestCase(LargeNeg)]
        public void Beta_WrongValues(double d)
        {
            var i = (int) d;
            Assert.False(FisherSnedecorDistribution.AreValidParams(1, i));
            Assert.False(Dist.IsValidBeta(i));
            Assert.Throws<ArgumentOutOfRangeException>(() => { Dist.Beta = i; });
        }

        // alpha > 0
        int GetAlpha(IAlphaDistribution<int> d)
        {
            return d == null ? Rand.Next(1, 10) : d.Alpha;
        }

        // beta > 0
        int GetBeta(IBetaDistribution<int> d)
        {
            return d == null ? Rand.Next(1, 10) : d.Beta;
        }
    }
}
