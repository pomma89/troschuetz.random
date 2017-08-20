// The MIT License (MIT)
//
// Copyright (c) 2006-2007 Stefan Troschütz <stefan@troschuetz.de>
//
// Copyright (c) 2012-2019 Alessio Parma <alessio.parma@gmail.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute,
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT
// OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

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