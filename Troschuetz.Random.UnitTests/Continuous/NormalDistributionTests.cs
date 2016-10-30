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

    public sealed class NormalDistributionTests : ContinuousDistributionTests<NormalDistribution>
    {
        protected override NormalDistribution GetDist(NormalDistribution other = null)
        {
            return new NormalDistribution { Mu = GetMu(other), Sigma = GetSigma(other) };
        }

        protected override NormalDistribution GetDist(uint seed, NormalDistribution other = null)
        {
            return new NormalDistribution(seed) { Mu = GetMu(other), Sigma = GetSigma(other) };
        }

        protected override NormalDistribution GetDist(IGenerator gen, NormalDistribution other = null)
        {
            return new NormalDistribution(gen) { Mu = GetMu(other), Sigma = GetSigma(other) };
        }

        protected override NormalDistribution GetDistWithParams(NormalDistribution other = null)
        {
            return new NormalDistribution(GetMu(other), GetSigma(other));
        }

        protected override NormalDistribution GetDistWithParams(uint seed, NormalDistribution other = null)
        {
            return new NormalDistribution(seed, GetMu(other), GetSigma(other));
        }

        protected override NormalDistribution GetDistWithParams(IGenerator gen, NormalDistribution other = null)
        {
            return new NormalDistribution(gen, GetMu(other), GetSigma(other));
        }

        [TestCase(double.NaN)]
        [TestCase(0)]
        [TestCase(TinyNeg)]
        [TestCase(SmallNeg)]
        [TestCase(LargeNeg)]
        public void Sigma_WrongValues(double d)
        {
            Assert.False(NormalDistribution.AreValidParams(1, d));
            Assert.False(Dist.IsValidSigma(d));
            Assert.Throws<ArgumentOutOfRangeException>(() => { Dist.Sigma = d; });
        }

        // any value
        private double GetMu(IMuDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(-10, 10) : d.Mu;
        }

        // sigma > 0, better keep it low
        private double GetSigma(ISigmaDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(0.1, 1) : d.Sigma;
        }
    }
}