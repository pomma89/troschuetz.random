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

namespace Troschuetz.Random.Tests
{
    using NUnit.Framework;
    using Random.Generators;
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    public abstract class DistributionTests<TDist> : TestBase where TDist : IDistribution
    {
        [SetUp]
        public void SetUp()
        {
            switch (_currDist)
            {
                case 0:
                    Dist = GetDist();
                    OtherDist = GetDist(Dist.Generator.Seed, Dist);
                    break;

                case 1:
                    var s = (uint) Rand.Next();
                    Dist = GetDist(s);
                    OtherDist = GetDist(s, Dist);
                    break;

                case 2:
                    var g1 = new StandardGenerator(Rand.Next());
                    var g2 = new StandardGenerator((int) g1.Seed);
                    Dist = GetDist(g1);
                    OtherDist = GetDist(g2, Dist);
                    break;

                case 3:
                    Dist = GetDistWithParams();
                    OtherDist = GetDistWithParams(Dist.Generator.Seed, Dist);
                    break;

                case 4:
                    s = (uint) Rand.Next();
                    Dist = GetDistWithParams(s);
                    OtherDist = GetDistWithParams(s, Dist);
                    break;

                case 5:
                    g1 = new StandardGenerator(Rand.Next());
                    g2 = new StandardGenerator((int) g1.Seed);
                    Dist = GetDistWithParams(g1);
                    OtherDist = GetDistWithParams(g2, Dist);
                    break;

                default:
                    throw new Exception();
            }
            _currDist = (_currDist + 1) % RepetitionCount;
        }

        protected const int RepetitionCount = 6;
        protected TDist Dist;
        protected TDist OtherDist;
        int _currDist;

        protected abstract TDist GetDist(TDist other = default(TDist));

        protected abstract TDist GetDist(uint seed, TDist other = default(TDist));

        protected abstract TDist GetDist(IGenerator gen, TDist other = default(TDist));

        protected abstract TDist GetDistWithParams(TDist other = default(TDist));

        protected abstract TDist GetDistWithParams(uint seed, TDist other = default(TDist));

        protected abstract TDist GetDistWithParams(IGenerator gen, TDist other = default(TDist));

        /*=============================================================================
            Construction
        =============================================================================*/

        [Test]
        public void Construction_NullGenerator()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                GetDist(null);
            });
        }
    }

    public abstract class ContinuousDistributionTests<TDist> : DistributionTests<TDist>
        where TDist : class, IContinuousDistribution
    {
        /*=============================================================================
            DistributedDoubles
        =============================================================================*/

        [Test]
        [Repeat(RepetitionCount)]
        public void Doubles_ManyRand()
        {
            var doubles = Dist.DistributedDoubles().GetEnumerator();
            doubles.MoveNext();
            for (var i = 0; i < Iterations; ++i, doubles.MoveNext())
            {
                Results[i] = doubles.Current;
            }
            AssertDist(Dist);
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Doubles_SameOutput()
        {
            var doubles = Dist.DistributedDoubles().GetEnumerator();
            doubles.MoveNext();
            for (var i = 0; i < Iterations; ++i, doubles.MoveNext())
            {
                Assert.AreEqual(OtherDist.NextDouble(), doubles.Current);
            }
        }

        /*=============================================================================
            NextDouble
        =============================================================================*/

        [Test]
        [Repeat(RepetitionCount)]
        public void NextDouble_ManyRand()
        {
            for (var i = 0; i < Iterations; ++i)
            {
                Results[i] = Dist.NextDouble();
            }
            AssertDist(Dist);
        }

        /*=============================================================================
            Reset
        =============================================================================*/

        [Test]
        [Repeat(RepetitionCount)]
        public void Reset_AfterManyRand()
        {
            if (!Dist.CanReset)
            {
                Assert.Pass();
            }
            for (var i = 0; i < Iterations; ++i)
            {
                Results[i] = Dist.NextDouble();
            }
            AssertDist(Dist);
            Assert.True(Dist.Reset());
            for (var i = 0; i < Iterations; ++i)
            {
                Assert.AreEqual(Results[i], Dist.NextDouble());
            }
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Reset_AfterOneRand()
        {
            if (!Dist.CanReset)
            {
                Assert.Pass();
            }
            var d = Dist.NextDouble();
            Assert.True(Dist.Reset());
            Assert.AreEqual(d, Dist.NextDouble());
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Reset_DoubleCall_AfterManyRand()
        {
            if (!Dist.CanReset)
            {
                Assert.Pass();
            }
            for (var i = 0; i < Iterations; ++i)
            {
                Results[i] = Dist.NextDouble();
            }
            AssertDist(Dist);
            Assert.True(Dist.Reset());
            Assert.True(Dist.Reset());
            for (var i = 0; i < Iterations; ++i)
            {
                Assert.AreEqual(Results[i], Dist.NextDouble());
            }
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Reset_DoubleCall_AfterOneRand()
        {
            if (!Dist.CanReset)
            {
                Assert.Pass();
            }
            var d = Dist.NextDouble();
            Assert.True(Dist.Reset());
            Assert.True(Dist.Reset());
            Assert.AreEqual(d, Dist.NextDouble());
        }

        /*=============================================================================
            Serialization
        =============================================================================*/

        [Test]
        [Repeat(RepetitionCount)]
        public void NextDouble_Serialization_AfterManyRand()
        {
            for (var i = 0; i < Iterations; ++i)
            {
                Dist.NextDouble();
            }
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, Dist);
                ms.Position = 0;

                var otherDist = bf.Deserialize(ms) as TDist;
                for (var i = 0; i < Iterations; ++i)
                {
                    Assert.AreEqual(Dist.NextDouble(), otherDist.NextDouble());
                }
            }
        }
    }

    public abstract class DiscreteDistributionTests<TDist> : DistributionTests<TDist>
        where TDist : class, IDiscreteDistribution
    {
        /*=============================================================================
            Next
        =============================================================================*/

        /*=============================================================================
            DistributedIntegers
        =============================================================================*/

        [Test]
        [Repeat(RepetitionCount)]
        public void Integers_ManyRand()
        {
            var integers = Dist.DistributedIntegers().GetEnumerator();
            integers.MoveNext();
            for (var i = 0; i < Iterations; ++i, integers.MoveNext())
            {
                Results[i] = integers.Current;
            }
            AssertDist(Dist);
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Integers_Next_SameOutput()
        {
            var integers = Dist.DistributedIntegers().GetEnumerator();
            for (var i = 0; i < Iterations; ++i)
            {
                integers.MoveNext();
                Assert.AreEqual(integers.Current, OtherDist.Next());
            }
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Integers_SameOutput()
        {
            var doubles = Dist.DistributedIntegers().GetEnumerator();
            doubles.MoveNext();
            for (var i = 0; i < Iterations; ++i, doubles.MoveNext())
            {
                Assert.AreEqual(OtherDist.Next(), doubles.Current);
            }
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Next_ManyRand()
        {
            for (var i = 0; i < Iterations; ++i)
            {
                Results[i] = Dist.Next();
            }
            AssertDist(Dist);
        }

        /*=============================================================================
            Reset
        =============================================================================*/

        [Test]
        [Repeat(RepetitionCount)]
        public void Reset_AfterManyRand()
        {
            if (!Dist.CanReset)
            {
                Assert.Pass();
            }
            for (var i = 0; i < Iterations; ++i)
            {
                Results[i] = Dist.Next();
            }
            AssertDist(Dist);
            Assert.True(Dist.Reset());
            for (var i = 0; i < Iterations; ++i)
            {
                Assert.AreEqual(Results[i], Dist.Next());
            }
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Reset_AfterOneRand()
        {
            if (!Dist.CanReset)
            {
                Assert.Pass();
            }
            var d = Dist.Next();
            Assert.True(Dist.Reset());
            Assert.AreEqual(d, Dist.Next());
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Reset_DoubleCall_AfterManyRand()
        {
            if (!Dist.CanReset)
            {
                Assert.Pass();
            }
            for (var i = 0; i < Iterations; ++i)
            {
                Results[i] = Dist.Next();
            }
            AssertDist(Dist);
            Assert.True(Dist.Reset());
            Assert.True(Dist.Reset());
            for (var i = 0; i < Iterations; ++i)
            {
                Assert.AreEqual(Results[i], Dist.Next());
            }
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Reset_DoubleCall_AfterOneRand()
        {
            if (!Dist.CanReset)
            {
                Assert.Pass();
            }
            var d = Dist.Next();
            Assert.True(Dist.Reset());
            Assert.True(Dist.Reset());
            Assert.AreEqual(d, Dist.Next());
        }

        /*=============================================================================
            Serialization
        =============================================================================*/

        [Test]
        [Repeat(RepetitionCount)]
        public void Next_Serialization_AfterManyRand()
        {
            for (var i = 0; i < Iterations; ++i)
            {
                Dist.Next();
            }
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, Dist);
                ms.Position = 0;

                var otherDist = bf.Deserialize(ms) as TDist;
                for (var i = 0; i < Iterations; ++i)
                {
                    Assert.AreEqual(Dist.NextDouble(), otherDist.NextDouble());
                }
            }
        }
    }
}
