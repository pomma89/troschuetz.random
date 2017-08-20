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

namespace Troschuetz.Random.Tests
{
    using NUnit.Framework;
    using Random.Generators;
    using Shouldly;
    using System;
    using System.IO;

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
        private int _currDist;

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

#if !NETSTD16

        [Test]
        [Repeat(RepetitionCount)]
        public void NextDouble_Serialization_AfterManyRand()
        {
            for (var i = 0; i < Iterations; ++i)
            {
                Dist.NextDouble();
            }
            var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
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

#endif
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
            var integers = Dist.DistributedIntegers().GetEnumerator();
            integers.MoveNext();
            for (var i = 0; i < Iterations; ++i, integers.MoveNext())
            {
                OtherDist.Next().ShouldBe(integers.Current);
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

#if !NETSTD16

        [Test]
        [Repeat(RepetitionCount)]
        public void Next_Serialization_AfterManyRand()
        {
            for (var i = 0; i < Iterations; ++i)
            {
                Dist.Next();
            }
            var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
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

#endif
    }
}