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

namespace Troschuetz.Random.Tests.Discrete
{
    using Distributions.Discrete;
    using NUnit.Framework;
    using System;

    public sealed class PoissonDistributionTests : DiscreteDistributionTests<PoissonDistribution>
    {
        protected override PoissonDistribution GetDist(PoissonDistribution other = null)
        {
            return new PoissonDistribution { Lambda = GetLambda(other) };
        }

        protected override PoissonDistribution GetDist(uint seed, PoissonDistribution other = null)
        {
            return new PoissonDistribution(seed) { Lambda = GetLambda(other) };
        }

        protected override PoissonDistribution GetDist(IGenerator gen, PoissonDistribution other = null)
        {
            return new PoissonDistribution(gen) { Lambda = GetLambda(other) };
        }

        protected override PoissonDistribution GetDistWithParams(PoissonDistribution other = null)
        {
            return new PoissonDistribution(GetLambda(other));
        }

        protected override PoissonDistribution GetDistWithParams(uint seed, PoissonDistribution other = null)
        {
            return new PoissonDistribution(seed, GetLambda(other));
        }

        protected override PoissonDistribution GetDistWithParams(IGenerator gen, PoissonDistribution other = null)
        {
            return new PoissonDistribution(gen, GetLambda(other));
        }

        [TestCase(double.NaN)]
        [TestCase(0)]
        [TestCase(TinyNeg)]
        [TestCase(SmallNeg)]
        [TestCase(LargeNeg)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Lambda_WrongValues(double d)
        {
            Assert.False(PoissonDistribution.IsValidParam(d));
            Assert.False(Dist.IsValidLambda(d));
            Dist.Lambda = d;
        }

        // lambda > 0
        double GetLambda(ILambdaDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(1, 10) : d.Lambda;
        }
    }
}
