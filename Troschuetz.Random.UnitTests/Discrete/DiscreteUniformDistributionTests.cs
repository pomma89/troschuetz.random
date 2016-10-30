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

    public sealed class DiscreteUniformDistributionTests : DiscreteDistributionTests<DiscreteUniformDistribution>
    {
        protected override DiscreteUniformDistribution GetDist(DiscreteUniformDistribution other = null)
        {
            return new DiscreteUniformDistribution { Beta = GetBeta(other), Alpha = GetAlpha(other) };
        }

        protected override DiscreteUniformDistribution GetDist(uint seed, DiscreteUniformDistribution other = null)
        {
            return new DiscreteUniformDistribution(seed) { Beta = GetBeta(other), Alpha = GetAlpha(other) };
        }

        protected override DiscreteUniformDistribution GetDist(IGenerator gen, DiscreteUniformDistribution other = null)
        {
            return new DiscreteUniformDistribution(gen) { Beta = GetBeta(other), Alpha = GetAlpha(other) };
        }

        protected override DiscreteUniformDistribution GetDistWithParams(DiscreteUniformDistribution other = null)
        {
            return new DiscreteUniformDistribution(GetAlpha(other), GetBeta(other));
        }

        protected override DiscreteUniformDistribution GetDistWithParams(uint seed, DiscreteUniformDistribution other = null)
        {
            return new DiscreteUniformDistribution(seed, GetAlpha(other), GetBeta(other));
        }

        protected override DiscreteUniformDistribution GetDistWithParams(IGenerator gen, DiscreteUniformDistribution other = null)
        {
            return new DiscreteUniformDistribution(gen, GetAlpha(other), GetBeta(other));
        }

        [Test]
        public void InvalidParameters1()
        {
            Assert.False(DiscreteUniformDistribution.AreValidParams(50, 1));            
            Assert.Throws<ArgumentOutOfRangeException>(() => { Dist.Beta = 1; Dist.Alpha = 50; });
        }

        [Test]
        public void InvalidParameters2()
        {
            Assert.False(DiscreteUniformDistribution.AreValidParams(Dist.Alpha, Dist.Alpha - 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => { Dist.Beta = Dist.Alpha - 1; });
        }

        [Test]
        public void InvalidParameters3()
        {
            Assert.False(DiscreteUniformDistribution.AreValidParams(50, int.MaxValue));
            Assert.Throws<ArgumentOutOfRangeException>(() => { Dist.Beta = int.MaxValue; });
        }

        // alpha <= beta && beta < int.MaxValue
        private int GetAlpha(IAlphaDistribution<int> d)
        {
            return d == null ? Rand.Next(10) : d.Alpha;
        }

        // alpha <= beta && beta < int.MaxValue
        private int GetBeta(IBetaDistribution<int> d)
        {
            return d == null ? Rand.Next(10, 100) : d.Beta;
        }
    }
}