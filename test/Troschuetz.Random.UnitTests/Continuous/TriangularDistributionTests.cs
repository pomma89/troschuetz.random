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

    public sealed class TriangularDistributionTests : ContinuousDistributionTests<TriangularDistribution>
    {
        protected override TriangularDistribution GetDist(TriangularDistribution other = null)
        {
            return new TriangularDistribution { Beta = GetBeta(other), Gamma = GetGamma(other), Alpha = GetAlpha(other) };
        }

        protected override TriangularDistribution GetDist(uint seed, TriangularDistribution other = null)
        {
            return new TriangularDistribution(seed) { Beta = GetBeta(other), Gamma = GetGamma(other), Alpha = GetAlpha(other) };
        }

        protected override TriangularDistribution GetDist(IGenerator gen, TriangularDistribution other = null)
        {
            return new TriangularDistribution(gen) { Beta = GetBeta(other), Gamma = GetGamma(other), Alpha = GetAlpha(other) };
        }

        protected override TriangularDistribution GetDistWithParams(TriangularDistribution other = null)
        {
            return new TriangularDistribution(GetAlpha(other), GetBeta(other), GetGamma(other));
        }

        protected override TriangularDistribution GetDistWithParams(uint seed, TriangularDistribution other = null)
        {
            return new TriangularDistribution(seed, GetAlpha(other), GetBeta(other), GetGamma(other));
        }

        protected override TriangularDistribution GetDistWithParams(IGenerator gen, TriangularDistribution other = null)
        {
            return new TriangularDistribution(gen, GetAlpha(other), GetBeta(other), GetGamma(other));
        }

        // alpha < beta && alpha <= gamma && beta >= gamma
        private double GetAlpha(IAlphaDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(-2, 2) : d.Alpha;
        }

        // alpha < beta && alpha <= gamma && beta >= gamma
        private double GetBeta(IBetaDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(4, 6) : d.Beta;
        }

        // alpha < beta && alpha <= gamma && beta >= gamma
        private double GetGamma(IGammaDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(2, 4) : d.Gamma;
        }
    }
}