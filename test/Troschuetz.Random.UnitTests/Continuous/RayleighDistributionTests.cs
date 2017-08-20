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

    public sealed class RayleighDistributionTests : ContinuousDistributionTests<RayleighDistribution>
    {
        protected override RayleighDistribution GetDist(RayleighDistribution other = null)
        {
            return new RayleighDistribution { Sigma = GetSigma(other) };
        }

        protected override RayleighDistribution GetDist(uint seed, RayleighDistribution other = null)
        {
            return new RayleighDistribution(seed) { Sigma = GetSigma(other) };
        }

        protected override RayleighDistribution GetDist(IGenerator gen, RayleighDistribution other = null)
        {
            return new RayleighDistribution(gen) { Sigma = GetSigma(other) };
        }

        protected override RayleighDistribution GetDistWithParams(RayleighDistribution other = null)
        {
            return new RayleighDistribution(GetSigma(other));
        }

        protected override RayleighDistribution GetDistWithParams(uint seed, RayleighDistribution other = null)
        {
            return new RayleighDistribution(seed, GetSigma(other));
        }

        protected override RayleighDistribution GetDistWithParams(IGenerator gen, RayleighDistribution other = null)
        {
            return new RayleighDistribution(gen, GetSigma(other));
        }

        [TestCase(double.NaN)]
        [TestCase(0)]
        [TestCase(TinyNeg)]
        [TestCase(SmallNeg)]
        [TestCase(LargeNeg)]
        public void Sigma_WrongValues(double d)
        {
            Assert.False(RayleighDistribution.IsValidParam(d));
            Assert.False(Dist.IsValidSigma(d));
            Assert.Throws<ArgumentOutOfRangeException>(() => { Dist.Sigma = d; });
        }

        // sigma > 0
        private double GetSigma(ISigmaDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(0.1, 10) : d.Sigma;
        }
    }
}