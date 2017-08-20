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

namespace Troschuetz.Random.Tests.Continuous
{
    using Distributions.Continuous;
    using NUnit.Framework;
    using System;

    public sealed class ChiDistributionTests : ContinuousDistributionTests<ChiDistribution>
    {
        protected override ChiDistribution GetDist(ChiDistribution other = null)
        {
            return new ChiDistribution { Alpha = GetAlpha(other) };
        }

        protected override ChiDistribution GetDist(uint seed, ChiDistribution other = null)
        {
            return new ChiDistribution(seed) { Alpha = GetAlpha(other) };
        }

        protected override ChiDistribution GetDist(IGenerator gen, ChiDistribution other = null)
        {
            return new ChiDistribution(gen) { Alpha = GetAlpha(other) };
        }

        protected override ChiDistribution GetDistWithParams(ChiDistribution other = null)
        {
            return new ChiDistribution(GetAlpha(other));
        }

        protected override ChiDistribution GetDistWithParams(uint seed, ChiDistribution other = null)
        {
            return new ChiDistribution(seed, GetAlpha(other));
        }

        protected override ChiDistribution GetDistWithParams(IGenerator gen, ChiDistribution other = null)
        {
            return new ChiDistribution(gen, GetAlpha(other));
        }

        [TestCase(double.NaN)]
        [TestCase(0)]
        [TestCase(SmallNeg)]
        [TestCase(LargeNeg)]
        public void Alpha_WrongValues(double d)
        {
            var i = (int) d;
            Assert.False(ChiDistribution.IsValidParam(i));
            Assert.False(Dist.IsValidAlpha(i));
            Assert.Throws<ArgumentOutOfRangeException>(() => { Dist.Alpha = i; });
        }

        // alpha > 0
        private int GetAlpha(IAlphaDistribution<int> d)
        {
            return d == null ? Rand.Next(1, 10) : d.Alpha;
        }
    }
}