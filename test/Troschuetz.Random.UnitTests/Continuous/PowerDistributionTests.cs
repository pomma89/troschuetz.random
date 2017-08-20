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

    public sealed class PowerDistributionTests : ContinuousDistributionTests<PowerDistribution>
    {
        protected override PowerDistribution GetDist(PowerDistribution other = null)
        {
            return new PowerDistribution { Alpha = GetAlpha(other), Beta = GetBeta(other) };
        }

        protected override PowerDistribution GetDist(uint seed, PowerDistribution other = null)
        {
            return new PowerDistribution(seed) { Alpha = GetAlpha(other), Beta = GetBeta(other) };
        }

        protected override PowerDistribution GetDist(IGenerator gen, PowerDistribution other = null)
        {
            return new PowerDistribution(gen) { Alpha = GetAlpha(other), Beta = GetBeta(other) };
        }

        protected override PowerDistribution GetDistWithParams(PowerDistribution other = null)
        {
            return new PowerDistribution(GetAlpha(other), GetBeta(other));
        }

        protected override PowerDistribution GetDistWithParams(uint seed, PowerDistribution other = null)
        {
            return new PowerDistribution(seed, GetAlpha(other), GetBeta(other));
        }

        protected override PowerDistribution GetDistWithParams(IGenerator gen, PowerDistribution other = null)
        {
            return new PowerDistribution(gen, GetAlpha(other), GetBeta(other));
        }

        [TestCase(double.NaN)]
        [TestCase(0)]
        [TestCase(TinyNeg)]
        [TestCase(SmallNeg)]
        [TestCase(LargeNeg)]
        public void Alpha_WrongValues(double d)
        {
            Assert.False(PowerDistribution.AreValidParams(d, 1));
            Assert.False(Dist.IsValidAlpha(d));
            Assert.Throws<ArgumentOutOfRangeException>(() => { Dist.Alpha = d; });
        }

        [TestCase(double.NaN)]
        [TestCase(0)]
        [TestCase(TinyNeg)]
        [TestCase(SmallNeg)]
        [TestCase(LargeNeg)]
        public void Beta_WrongValues(double d)
        {
            Assert.False(PowerDistribution.AreValidParams(1, d));
            Assert.False(Dist.IsValidBeta(d));
            Assert.Throws<ArgumentOutOfRangeException>(() => { Dist.Beta = d; });
        }

        // alpha > 0
        private double GetAlpha(IAlphaDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(0.1, 10) : d.Alpha;
        }

        // beta > 0
        private double GetBeta(IBetaDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(0.1, 10) : d.Beta;
        }
    }
}