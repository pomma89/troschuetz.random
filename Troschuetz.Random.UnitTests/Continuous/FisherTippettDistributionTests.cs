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

    public sealed class FisherTippettDistributionTests : ContinuousDistributionTests<FisherTippettDistribution>
    {
        protected override FisherTippettDistribution GetDist(FisherTippettDistribution other = null)
        {
            return new FisherTippettDistribution { Alpha = GetAlpha(other), Mu = GetMu(other) };
        }

        protected override FisherTippettDistribution GetDist(uint seed, FisherTippettDistribution other = null)
        {
            return new FisherTippettDistribution(seed) { Alpha = GetAlpha(other), Mu = GetMu(other) };
        }

        protected override FisherTippettDistribution GetDist(IGenerator gen, FisherTippettDistribution other = null)
        {
            return new FisherTippettDistribution(gen) { Alpha = GetAlpha(other), Mu = GetMu(other) };
        }

        protected override FisherTippettDistribution GetDistWithParams(FisherTippettDistribution other = null)
        {
            return new FisherTippettDistribution(GetAlpha(other), GetMu(other));
        }

        protected override FisherTippettDistribution GetDistWithParams(uint seed, FisherTippettDistribution other = null)
        {
            return new FisherTippettDistribution(seed, GetAlpha(other), GetMu(other));
        }

        protected override FisherTippettDistribution GetDistWithParams(IGenerator gen, FisherTippettDistribution other = null)
        {
            return new FisherTippettDistribution(gen, GetAlpha(other), GetMu(other));
        }

        [TestCase(double.NaN)]
        [TestCase(0)]
        [TestCase(TinyNeg)]
        [TestCase(SmallNeg)]
        [TestCase(LargeNeg)]
        public void Alpha_WrongValues(double d)
        {
            Assert.False(FisherTippettDistribution.AreValidParams(d, 1));
            Assert.False(Dist.IsValidAlpha(d));
            Assert.Throws<ArgumentOutOfRangeException>(() => { Dist.Alpha = d; });
        }

        // alpha > 0
        private double GetAlpha(IAlphaDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(0.1, 10) : d.Alpha;
        }

        // any value
        private double GetMu(IMuDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(-10, 10) : d.Mu;
        }
    }
}