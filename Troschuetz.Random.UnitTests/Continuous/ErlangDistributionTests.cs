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

    public sealed class ErlangDistributionTests : ContinuousDistributionTests<ErlangDistribution>
    {
        protected override ErlangDistribution GetDist(ErlangDistribution other = null)
        {
            return new ErlangDistribution { Alpha = GetAlpha(other), Lambda = GetLambda(other) };
        }

        protected override ErlangDistribution GetDist(uint seed, ErlangDistribution other = null)
        {
            return new ErlangDistribution(seed) { Alpha = GetAlpha(other), Lambda = GetLambda(other) };
        }

        protected override ErlangDistribution GetDist(IGenerator gen, ErlangDistribution other = null)
        {
            return new ErlangDistribution(gen) { Alpha = GetAlpha(other), Lambda = GetLambda(other) };
        }

        protected override ErlangDistribution GetDistWithParams(ErlangDistribution other = null)
        {
            return new ErlangDistribution(GetAlpha(other), GetLambda(other));
        }

        protected override ErlangDistribution GetDistWithParams(uint seed, ErlangDistribution other = null)
        {
            return new ErlangDistribution(seed, GetAlpha(other), GetLambda(other));
        }

        protected override ErlangDistribution GetDistWithParams(IGenerator gen, ErlangDistribution other = null)
        {
            return new ErlangDistribution(gen, GetAlpha(other), GetLambda(other));
        }

        [TestCase(double.NaN)]
        [TestCase(0)]
        [TestCase(SmallNeg)]
        [TestCase(LargeNeg)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Alpha_WrongValues(double d)
        {
            var i = (int) d;
            Assert.False(ErlangDistribution.AreValidParams(i, 1));
            Assert.False(Dist.IsValidAlpha(i));
            Dist.Alpha = i;
        }

        [TestCase(double.NaN)]
        [TestCase(0)]
        [TestCase(TinyNeg)]
        [TestCase(SmallNeg)]
        [TestCase(LargeNeg)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Lambda_WrongValues(double d)
        {
            Assert.False(ErlangDistribution.AreValidParams(1, d));
            Assert.False(Dist.IsValidLambda(d));
            Dist.Lambda = d;
        }

        // alpha > 0
        int GetAlpha(IAlphaDistribution<int> d)
        {
            return d == null ? Rand.Next(1, 10) : d.Alpha;
        }

        // lambda > 0
        double GetLambda(ILambdaDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(0.1, 10) : d.Lambda;
        }
    }
}
