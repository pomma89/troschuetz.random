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

    public sealed class ContinuousUniformDistributionTests : ContinuousDistributionTests<ContinuousUniformDistribution>
    {
        protected override ContinuousUniformDistribution GetDist(ContinuousUniformDistribution other = null)
        {
            return new ContinuousUniformDistribution { Beta = GetBeta(other), Alpha = GetAlpha(other) };
        }

        protected override ContinuousUniformDistribution GetDist(uint seed, ContinuousUniformDistribution other = null)
        {
            return new ContinuousUniformDistribution(seed) { Beta = GetBeta(other), Alpha = GetAlpha(other) };
        }

        protected override ContinuousUniformDistribution GetDist(IGenerator gen, ContinuousUniformDistribution other = null)
        {
            return new ContinuousUniformDistribution(gen) { Beta = GetBeta(other), Alpha = GetAlpha(other) };
        }

        protected override ContinuousUniformDistribution GetDistWithParams(ContinuousUniformDistribution other = null)
        {
            return new ContinuousUniformDistribution(GetAlpha(other), GetBeta(other));
        }

        protected override ContinuousUniformDistribution GetDistWithParams(uint seed, ContinuousUniformDistribution other = null)
        {
            return new ContinuousUniformDistribution(seed, GetAlpha(other), GetBeta(other));
        }

        protected override ContinuousUniformDistribution GetDistWithParams(IGenerator gen, ContinuousUniformDistribution other = null)
        {
            return new ContinuousUniformDistribution(gen, GetAlpha(other), GetBeta(other));
        }

        [TestCase(double.NaN, 1.0)]
        [TestCase(1.0, double.NaN)]
        [TestCase(TinyPos, TinyNeg)]
        [TestCase(SmallPos, SmallNeg)]
        [TestCase(LargePos, LargeNeg)]
        public void AlphaBeta_WrongValues(double a, double b)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Assert.False(ContinuousUniformDistribution.AreValidParams(a, b));
                Dist.Alpha = Dist.Beta = a;
                Assert.False(Dist.IsValidBeta(b));
                Dist.Beta = b;
            });
        }

        // alpha <= beta
        private double GetAlpha(IAlphaDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(5) : d.Alpha;
        }

        // alpha <= beta
        private double GetBeta(IBetaDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(5.1, 10) : d.Beta;
        }
    }
}