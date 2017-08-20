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

    public sealed class BernoulliDistributionTests : DiscreteDistributionTests<BernoulliDistribution>
    {
        protected override BernoulliDistribution GetDist(BernoulliDistribution other = null)
        {
            return new BernoulliDistribution { Alpha = GetAlpha(other) };
        }

        protected override BernoulliDistribution GetDist(uint seed, BernoulliDistribution other = null)
        {
            return new BernoulliDistribution(seed) { Alpha = GetAlpha(other) };
        }

        protected override BernoulliDistribution GetDist(IGenerator gen, BernoulliDistribution other = null)
        {
            return new BernoulliDistribution(gen) { Alpha = GetAlpha(other) };
        }

        protected override BernoulliDistribution GetDistWithParams(BernoulliDistribution other = null)
        {
            return new BernoulliDistribution(GetAlpha(other));
        }

        protected override BernoulliDistribution GetDistWithParams(uint seed, BernoulliDistribution other = null)
        {
            return new BernoulliDistribution(seed, GetAlpha(other));
        }

        protected override BernoulliDistribution GetDistWithParams(IGenerator gen, BernoulliDistribution other = null)
        {
            return new BernoulliDistribution(gen, GetAlpha(other));
        }

        [TestCase(double.NaN)]
        [TestCase(TinyNeg)]
        [TestCase(SmallNeg)]
        [TestCase(LargeNeg)]
        [TestCase(1 + TinyPos)]
        [TestCase(1 + SmallPos)]
        [TestCase(1 + LargePos)]
        public void Alpha_WrongValues(double d)
        {
            Assert.False(BernoulliDistribution.IsValidParam(d));
            Assert.False(Dist.IsValidAlpha(d));
            Assert.Throws<ArgumentOutOfRangeException>(() => { Dist.Alpha = d; });
        }

        // alpha >= 0.0 && alpha <= 1.0
        private double GetAlpha(IAlphaDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(0, 1) : d.Alpha;
        }
    }
}