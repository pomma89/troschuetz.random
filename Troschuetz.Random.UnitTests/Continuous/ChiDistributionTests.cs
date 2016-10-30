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

    public sealed class ChiDistributionTests : ContinuousDistributionTests<ChiDistribution>
    {
        protected override ChiDistribution GetDist(ChiDistribution other = null)
        {
            return new ChiDistribution { Alpha = GetAlpha(other) };
        }

        protected override ChiDistribution GetDist(uint seed, ChiDistribution other = null)
        {
            return new ChiDistribution(seed) { Alpha = GetAlpha(other) };
        }

        protected override ChiDistribution GetDist(IGenerator gen, ChiDistribution other = null)
        {
            return new ChiDistribution(gen) { Alpha = GetAlpha(other) };
        }

        protected override ChiDistribution GetDistWithParams(ChiDistribution other = null)
        {
            return new ChiDistribution(GetAlpha(other));
        }

        protected override ChiDistribution GetDistWithParams(uint seed, ChiDistribution other = null)
        {
            return new ChiDistribution(seed, GetAlpha(other));
        }

        protected override ChiDistribution GetDistWithParams(IGenerator gen, ChiDistribution other = null)
        {
            return new ChiDistribution(gen, GetAlpha(other));
        }

        [TestCase(double.NaN)]
        [TestCase(0)]
        [TestCase(SmallNeg)]
        [TestCase(LargeNeg)]
        public void Alpha_WrongValues(double d)
        {
            var i = (int) d;
            Assert.False(ChiDistribution.IsValidParam(i));
            Assert.False(Dist.IsValidAlpha(i));
            Assert.Throws<ArgumentOutOfRangeException>(() => { Dist.Alpha = i; });
        }

        // alpha > 0
        private int GetAlpha(IAlphaDistribution<int> d)
        {
            return d == null ? Rand.Next(1, 10) : d.Alpha;
        }
    }
}