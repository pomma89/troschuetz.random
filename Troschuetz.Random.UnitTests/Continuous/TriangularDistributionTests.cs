/*
 * Copyright © 2012 Alessio Parma (alessio.parma@gmail.com)
 *
 * This file is part of Troschuetz.Random.Tests Class Library.
 *
 * Troschuetz.Random is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
 */

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
        double GetAlpha(IAlphaDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(-2, 2) : d.Alpha;
        }

        // alpha < beta && alpha <= gamma && beta >= gamma
        double GetBeta(IBetaDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(4, 6) : d.Beta;
        }

        // alpha < beta && alpha <= gamma && beta >= gamma
        double GetGamma(IGammaDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(2, 4) : d.Gamma;
        }
    }
}
