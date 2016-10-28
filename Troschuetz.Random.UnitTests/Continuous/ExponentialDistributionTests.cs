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

    public sealed class ExponentialDistributionTests : ContinuousDistributionTests<ExponentialDistribution>
    {
        protected override ExponentialDistribution GetDist(ExponentialDistribution other = null)
        {
            return new ExponentialDistribution { Lambda = GetLambda(other) };
        }

        protected override ExponentialDistribution GetDist(uint seed, ExponentialDistribution other = null)
        {
            return new ExponentialDistribution(seed) { Lambda = GetLambda(other) };
        }

        protected override ExponentialDistribution GetDist(IGenerator gen, ExponentialDistribution other = null)
        {
            return new ExponentialDistribution(gen) { Lambda = GetLambda(other) };
        }

        protected override ExponentialDistribution GetDistWithParams(ExponentialDistribution other = null)
        {
            return new ExponentialDistribution(GetLambda(other));
        }

        protected override ExponentialDistribution GetDistWithParams(uint seed, ExponentialDistribution other = null)
        {
            return new ExponentialDistribution(seed, GetLambda(other));
        }

        protected override ExponentialDistribution GetDistWithParams(IGenerator gen, ExponentialDistribution other = null)
        {
            return new ExponentialDistribution(gen, GetLambda(other));
        }

        [TestCase(double.NaN)]
        [TestCase(0)]
        [TestCase(TinyNeg)]
        [TestCase(SmallNeg)]
        [TestCase(LargeNeg)]
        public void Lambda_WrongValues(double d)
        {
            Assert.False(ExponentialDistribution.IsValidParam(d));
            Assert.False(Dist.IsValidLambda(d));
            Assert.Throws<ArgumentOutOfRangeException>(() => { Dist.Lambda = d; });
        }

        // lambda > 0
        double GetLambda(ILambdaDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(0.1, 10) : d.Lambda;
        }
    }
}
