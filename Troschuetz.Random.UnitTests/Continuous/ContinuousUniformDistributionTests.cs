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

    public sealed class ContinuousUniformDistributionTests : ContinuousDistributionTests<ContinuousUniformDistribution>
    {
        protected override ContinuousUniformDistribution GetDist(ContinuousUniformDistribution other = null)
        {
            return new ContinuousUniformDistribution { Beta = GetBeta(other), Alpha = GetAlpha(other) };
        }

        protected override ContinuousUniformDistribution GetDist(uint seed, ContinuousUniformDistribution other = null)
        {
            return new ContinuousUniformDistribution(seed) { Beta = GetBeta(other), Alpha = GetAlpha(other) };
        }

        protected override ContinuousUniformDistribution GetDist(IGenerator gen, ContinuousUniformDistribution other = null)
        {
            return new ContinuousUniformDistribution(gen) { Beta = GetBeta(other), Alpha = GetAlpha(other) };
        }

        protected override ContinuousUniformDistribution GetDistWithParams(ContinuousUniformDistribution other = null)
        {
            return new ContinuousUniformDistribution(GetAlpha(other), GetBeta(other));
        }

        protected override ContinuousUniformDistribution GetDistWithParams(uint seed, ContinuousUniformDistribution other = null)
        {
            return new ContinuousUniformDistribution(seed, GetAlpha(other), GetBeta(other));
        }

        protected override ContinuousUniformDistribution GetDistWithParams(IGenerator gen, ContinuousUniformDistribution other = null)
        {
            return new ContinuousUniformDistribution(gen, GetAlpha(other), GetBeta(other));
        }

        [TestCase(double.NaN, 1.0)]
        [TestCase(1.0, double.NaN)]
        [TestCase(TinyPos, TinyNeg)]
        [TestCase(SmallPos, SmallNeg)]
        [TestCase(LargePos, LargeNeg)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AlphaBeta_WrongValues(double a, double b)
        {
            Assert.False(ContinuousUniformDistribution.AreValidParams(a, b));
            Dist.Alpha = Dist.Beta = a;
            Assert.False(Dist.IsValidBeta(b));
            Dist.Beta = b;
        }

        // alpha <= beta
        double GetAlpha(IAlphaDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(5) : d.Alpha;
        }

        // alpha <= beta
        double GetBeta(IBetaDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(5.1, 10) : d.Beta;
        }
    }
}
