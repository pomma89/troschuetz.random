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

    public sealed class StudentsTDistributionTests : ContinuousDistributionTests<StudentsTDistribution>
    {
        protected override StudentsTDistribution GetDist(StudentsTDistribution other = null)
        {
            return new StudentsTDistribution {Nu = GetNu(other)};
        }

        protected override StudentsTDistribution GetDist(uint seed, StudentsTDistribution other = null)
        {
            return new StudentsTDistribution(seed) {Nu = GetNu(other)};
        }

        protected override StudentsTDistribution GetDist(IGenerator gen, StudentsTDistribution other = null)
        {
            return new StudentsTDistribution(gen) {Nu = GetNu(other)};
        }
        
        protected override StudentsTDistribution GetDistWithParams(StudentsTDistribution other = null)
        {
            return new StudentsTDistribution(GetNu(other));
        }

        protected override StudentsTDistribution GetDistWithParams(uint seed, StudentsTDistribution other = null)
        {
            return new StudentsTDistribution(seed, GetNu(other));
        }

        protected override StudentsTDistribution GetDistWithParams(IGenerator gen, StudentsTDistribution other = null)
        {
            return new StudentsTDistribution(gen, GetNu(other));
        }
        
        [TestCase(double.NaN)]
        [TestCase(0)]
        [TestCase(SmallNeg)]
        [TestCase(LargeNeg)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Nu_WrongValues(double d)
        {
            var i = (int) d;
            Assert.False(StudentsTDistribution.IsValidParam(i));
            Assert.False(Dist.IsValidNu(i));
            Dist.Nu = i;
        }

        // nu > 0
        int GetNu(INuDistribution<int> d)
        {
            return d == null ? Rand.Next(1, 10) : d.Nu;
        }
    }
}