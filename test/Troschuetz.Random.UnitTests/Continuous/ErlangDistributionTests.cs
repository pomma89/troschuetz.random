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
        public void Alpha_WrongValues(double d)
        {
            var i = (int) d;
            Assert.False(ErlangDistribution.AreValidParams(i, 1));
            Assert.False(Dist.IsValidAlpha(i));
            Assert.Throws<ArgumentOutOfRangeException>(() => { Dist.Alpha = i; });
        }

        [TestCase(double.NaN)]
        [TestCase(0)]
        [TestCase(TinyNeg)]
        [TestCase(SmallNeg)]
        [TestCase(LargeNeg)]
        public void Lambda_WrongValues(double d)
        {
            Assert.False(ErlangDistribution.AreValidParams(1, d));
            Assert.False(Dist.IsValidLambda(d));
            Assert.Throws<ArgumentOutOfRangeException>(() => { Dist.Lambda = d; });
        }

        // alpha > 0
        private int GetAlpha(IAlphaDistribution<int> d)
        {
            return d == null ? Rand.Next(1, 10) : d.Alpha;
        }

        // lambda > 0
        private double GetLambda(ILambdaDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(0.1, 10) : d.Lambda;
        }
    }
}