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
    using System;
    using Distributions.Continuous;
    using NUnit.Framework;

    public sealed class LogisticDistributionTests : ContinuousDistributionTests<LogisticDistribution>
    {
        protected override LogisticDistribution GetDist(LogisticDistribution other = null)
        {
            return new LogisticDistribution {Mu = GetMu(other), Sigma = GetSigma(other)};
        }

        protected override LogisticDistribution GetDist(uint seed, LogisticDistribution other = null)
        {
            return new LogisticDistribution(seed) {Mu = GetMu(other), Sigma = GetSigma(other)};
        }

        protected override LogisticDistribution GetDist(IGenerator gen, LogisticDistribution other = null)
        {
            return new LogisticDistribution(gen) {Mu = GetMu(other), Sigma = GetSigma(other)};
        }
        
        protected override LogisticDistribution GetDistWithParams(LogisticDistribution other = null)
        {
            return new LogisticDistribution(GetMu(other), GetSigma(other));
        }

        protected override LogisticDistribution GetDistWithParams(uint seed, LogisticDistribution other = null)
        {
            return new LogisticDistribution(seed, GetMu(other), GetSigma(other));
        }

        protected override LogisticDistribution GetDistWithParams(IGenerator gen, LogisticDistribution other = null)
        {
            return new LogisticDistribution(gen, GetMu(other), GetSigma(other));
        }
        
        [TestCase(double.NaN)]
        [TestCase(TinyNeg)]
        [TestCase(SmallNeg)]
        [TestCase(LargeNeg)]
        [TestCase(0.0)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Sigma_WrongValues(double d)
        {
            Assert.False(LogisticDistribution.AreValidParams(1, d));
            Assert.False(Dist.IsValidSigma(d));
            Dist.Sigma = d;
        }

        // any value
        double GetMu(IMuDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(-10, 10) : d.Mu;
        }

        // sigma >= 0, better keep it low
        double GetSigma(ISigmaDistribution<double> d)
        {
            if (d != null)
            {
                return d.Sigma;
            }
            double s;
            do s = Rand.NextDouble(); while (s == 0.0);
            return s;
        }
    }
}