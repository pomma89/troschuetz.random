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