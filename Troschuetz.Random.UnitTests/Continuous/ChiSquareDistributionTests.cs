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

    public sealed class ChiSquareDistributionTests : ContinuousDistributionTests<ChiSquareDistribution>
    {
        protected override ChiSquareDistribution GetDist(ChiSquareDistribution other = null)
        {
            return new ChiSquareDistribution { Alpha = GetAlpha(other) };
        }

        protected override ChiSquareDistribution GetDist(uint seed, ChiSquareDistribution other = null)
        {
            return new ChiSquareDistribution(seed) { Alpha = GetAlpha(other) };
        }

        protected override ChiSquareDistribution GetDist(IGenerator gen, ChiSquareDistribution other = null)
        {
            return new ChiSquareDistribution(gen) { Alpha = GetAlpha(other) };
        }

        protected override ChiSquareDistribution GetDistWithParams(ChiSquareDistribution other = null)
        {
            return new ChiSquareDistribution(GetAlpha(other));
        }

        protected override ChiSquareDistribution GetDistWithParams(uint seed, ChiSquareDistribution other = null)
        {
            return new ChiSquareDistribution(seed, GetAlpha(other));
        }

        protected override ChiSquareDistribution GetDistWithParams(IGenerator gen, ChiSquareDistribution other = null)
        {
            return new ChiSquareDistribution(gen, GetAlpha(other));
        }

        [TestCase(double.NaN)]
        [TestCase(0)]
        [TestCase(SmallNeg)]
        [TestCase(LargeNeg)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Alpha_WrongValues(double d)
        {
            var i = (int) d;
            Assert.False(ChiSquareDistribution.IsValidParam(i));
            Assert.False(Dist.IsValidAlpha(i));
            Dist.Alpha = i;
        }

        // alpha > 0
        int GetAlpha(IAlphaDistribution<int> d)
        {
            return d == null ? Rand.Next(1, 10) : d.Alpha;
        }
    }
}
