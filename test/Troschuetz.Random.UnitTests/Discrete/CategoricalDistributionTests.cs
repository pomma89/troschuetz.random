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
    using System.Collections.Generic;
    using System.Linq;

    public sealed class CategoricalDistributionTests : DiscreteDistributionTests<CategoricalDistribution>
    {
        protected override CategoricalDistribution GetDist(CategoricalDistribution other = null)
        {
            return new CategoricalDistribution(GetValueCount(other));
        }

        protected override CategoricalDistribution GetDist(uint seed, CategoricalDistribution other = null)
        {
            return new CategoricalDistribution(seed, GetValueCount(other));
        }

        protected override CategoricalDistribution GetDist(IGenerator gen, CategoricalDistribution other = null)
        {
            return new CategoricalDistribution(gen, GetValueCount(other));
        }

        protected override CategoricalDistribution GetDistWithParams(CategoricalDistribution other = null)
        {
            return new CategoricalDistribution(GetWeights(other));
        }

        protected override CategoricalDistribution GetDistWithParams(uint seed, CategoricalDistribution other = null)
        {
            return new CategoricalDistribution(seed, GetWeights(other));
        }

        protected override CategoricalDistribution GetDistWithParams(IGenerator gen, CategoricalDistribution other = null)
        {
            return new CategoricalDistribution(gen, GetWeights(other));
        }

        [TestCase(double.NaN, 1, 2)]
        [TestCase(SmallNeg, 1, 2)]
        [TestCase(1, LargeNeg, 2)]
        [TestCase(1, 2, TinyNeg)]
        [TestCase(0, -2, 2)]
        [TestCase(-3, -2, -1)]
        public void Weights_WrongValues(double d1, double d2, double d3)
        {
            var w = new List<double> { d1, d2, d3 };
            Assert.False(CategoricalDistribution.IsValidParam(w));
            Assert.False(Dist.AreValidWeights(w));
            Assert.Throws<ArgumentOutOfRangeException>(() => { Dist.Weights = w; });
        }

        [Test]
        public void Median_EvenEquiWeights()
        {
            Dist = new CategoricalDistribution(new double[] { 1, 1, 1, 1, 1, 1 });
            for (var i = 0; i < Iterations; ++i)
                Results[i] = Dist.Next();
            AssertDist(Dist);
        }

        [Test]
        public void Median_OddEquiWeights()
        {
            Dist = new CategoricalDistribution(new double[] { 1, 1, 1, 1, 1 });
            for (var i = 0; i < Iterations; ++i)
                Results[i] = Dist.Next();
            AssertDist(Dist);
        }

        // value count > 0
        private int GetValueCount(IWeightsDistribution<double> d)
        {
            return d == null ? Rand.Next(1, 10) : d.Weights.Count();
        }

        // all weights > 0
        private ICollection<double> GetWeights(IWeightsDistribution<double> d)
        {
            Func<double> r = () => Rand.NextDouble(0.1, 10);
            return d == null ? new List<double> { r(), r(), r(), r(), r() } : d.Weights;
        }
    }
}