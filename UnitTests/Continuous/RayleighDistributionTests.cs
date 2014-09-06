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
    using System;
    using Distributions.Continuous;
    using NUnit.Framework;

    public sealed class RayleighDistributionTests : ContinuousDistributionTests<RayleighDistribution>
    {
        protected override RayleighDistribution GetDist(RayleighDistribution other = null)
        {
            return new RayleighDistribution {Sigma = GetSigma(other)};
        }

        protected override RayleighDistribution GetDist(uint seed, RayleighDistribution other = null)
        {
            return new RayleighDistribution(seed) {Sigma = GetSigma(other)};
        }

        protected override RayleighDistribution GetDist(IGenerator gen, RayleighDistribution other = null)
        {
            return new RayleighDistribution(gen) {Sigma = GetSigma(other)};
        }
        
        protected override RayleighDistribution GetDistWithParams(RayleighDistribution other = null)
        {
            return new RayleighDistribution(GetSigma(other));
        }

        protected override RayleighDistribution GetDistWithParams(uint seed, RayleighDistribution other = null)
        {
            return new RayleighDistribution(seed, GetSigma(other));
        }

        protected override RayleighDistribution GetDistWithParams(IGenerator gen, RayleighDistribution other = null)
        {
            return new RayleighDistribution(gen, GetSigma(other));
        }
        
        [TestCase(double.NaN)]
        [TestCase(0)]
        [TestCase(TinyNeg)]
        [TestCase(SmallNeg)]
        [TestCase(LargeNeg)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Sigma_WrongValues(double d)
        {
            Assert.False(RayleighDistribution.IsValidParam(d));
            Assert.False(Dist.IsValidSigma(d));
            Dist.Sigma = d;
        }

        // sigma > 0
        double GetSigma(ISigmaDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(0.1, 10) : d.Sigma;
        }
    }
}