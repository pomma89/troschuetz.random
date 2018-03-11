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
    using Shouldly;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public abstract partial class GeneratorTests : TestBase
    {
        #region Setup/Teardown

        [SetUp]
        public virtual void SetUp()
        {
            _generator = (_currGen) ? GetGenerator() : GetGenerator(Rand.Next());
            _currGen = !_currGen;
        }

        #endregion Setup/Teardown

        private const int RepetitionCount = 2;
        private IGenerator _generator;
        private bool _currGen = true;

        protected abstract IGenerator GetGenerator();

        protected abstract IGenerator GetGenerator(int seed);

        protected abstract IGenerator GetGenerator(uint seed);

        [Test]
        [Repeat(RepetitionCount)]
        public void Booleans_SameOutputAsNextBoolean()
        {
            var otherGen = GetGenerator(_generator.Seed);
            Assert.True(_generator.Booleans().Take(Iterations).All(b => b == otherGen.NextBoolean()));
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Booleans_SameOutputAsNextBoolean_AfterReset()
        {
            var otherGen = GetGenerator(_generator.Seed);
            Assert.True(_generator.Booleans().Take(Iterations).All(b => b == otherGen.NextBoolean()));
            _generator.Reset();
            otherGen.Reset();
            Assert.True(_generator.Booleans().Take(Iterations).All(b => b == otherGen.NextBoolean()));
        }

        [Test]
        public void Bytes_NullBuffer()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _generator.Bytes(null).GetEnumerator().MoveNext();
            });
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Bytes_SameOutputAsNextBytes()
        {
            var b1 = new byte[5];
            var b2 = new byte[5];
            var otherGen = GetGenerator(_generator.Seed);
            var bytesEn = _generator.Bytes(b1).GetEnumerator();
            bytesEn.MoveNext();
            for (var i = 0; i < Iterations; ++i, bytesEn.MoveNext())
            {
                otherGen.NextBytes(b2);
                Assert.AreEqual(b2, b1);
            }
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Bytes_SameOutputAsNextBytes_AfterReset()
        {
            var b1 = new byte[5];
            var b2 = new byte[5];
            var otherGen = GetGenerator(_generator.Seed);
            var bytesEn = _generator.Bytes(b1).GetEnumerator();
            bytesEn.MoveNext();
            for (var i = 0; i < Iterations; ++i, bytesEn.MoveNext())
            {
                otherGen.NextBytes(b2);
                Assert.AreEqual(b2, b1);
            }
            _generator.Reset();
            otherGen.Reset();
            bytesEn.MoveNext();
            for (var i = 0; i < Iterations; ++i, bytesEn.MoveNext())
            {
                otherGen.NextBytes(b2);
                Assert.AreEqual(b2, b1);
            }
        }

        /*=============================================================================
            Serialization
        =============================================================================*/

#if !NETSTD16

        [Test]
        [Repeat(RepetitionCount)]
        public void Bytes_SameOutputAsNextBytes_WithSerialization()
        {
            var b1 = new byte[5];
            var b2 = new byte[5];
            var otherGen = GetGenerator(_generator.Seed);
            var bytesEn = _generator.Bytes(b1).GetEnumerator();
            bytesEn.MoveNext();
            for (var i = 0; i < Iterations; ++i, bytesEn.MoveNext())
            {
                otherGen.NextBytes(b2);
                Assert.AreEqual(b2, b1);
            }
            var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, _generator);
                ms.Position = 0;

                _generator = bf.Deserialize(ms) as IGenerator;
                for (var i = 0; i < Iterations; ++i, bytesEn.MoveNext())
                {
                    otherGen.NextBytes(b2);
                    Assert.AreEqual(b2, b1);
                }
            }
        }

#endif

        /*=============================================================================
            Seed generation
        =============================================================================*/

        [Test]
        [Repeat(RepetitionCount)]
        public void SeedShouldBeUniqueEvenIfGeneratedAtTheSameTimeByMultipleThreads()
        {
            const int threadCount = 32;
            var randomNumbers = new uint[threadCount][];

            Parallel.ForEach(Enumerable.Range(0, threadCount), i =>
            {
                var generator = GetGenerator();
                randomNumbers[i] = generator.UnsignedIntegers().Take(128).ToArray();
            });

            // No sequence should be equal to other sequences.
            Assert.That(randomNumbers.All(rno => randomNumbers.Where(rni => rni != rno).All(rni => !rni.SequenceEqual(rno))), Is.True);
        }

        /*=============================================================================
            DistributedDoubles
        =============================================================================*/

        [Test]
        [Repeat(RepetitionCount)]
        public void Doubles_BetweenBounds()
        {
            Assert.True(_generator.Doubles().Take(Iterations).All(x => x >= 0 && x < 1));
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Doubles_MaxValue_BetweenBounds()
        {
            var max = Rand.Next() + 1; // To avoid zero
            Assert.True(_generator.Doubles(max).Take(Iterations).All(x => x >= 0 && x < max));
        }

        [Test]
        public void Doubles_MaxValue_LargeNegativeMaxValue()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _generator.Doubles(LargeNeg).GetEnumerator().MoveNext();
            });
        }

        [Test]
        public void Doubles_MaxValue_PositiveInfinityMaxValue()
        {
            const double max = double.PositiveInfinity;
            Assert.Throws<ArgumentException>(() =>
            {
                Assert.True(_generator.Doubles(max).Take(Iterations).All(x => x >= 0 && x < max));
            });
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Doubles_MaxValue_SameOutputAsNextDouble()
        {
            var max = Rand.Next() + 1; // To avoid zero
            var otherGen = GetGenerator(_generator.Seed);
            Assert.True(_generator.Doubles(max).Take(Iterations).All(x => TMath.AreEqual(x, otherGen.NextDouble(max))));
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Doubles_MaxValue_SameOutputAsNextDouble_AfterReset()
        {
            var max = Rand.Next() + 1; // To avoid zero
            var otherGen = GetGenerator(_generator.Seed);
            Assert.True(_generator.Doubles(max).Take(Iterations).All(x => TMath.AreEqual(x, otherGen.NextDouble(max))));
            _generator.Reset();
            otherGen.Reset();
            Assert.True(_generator.Doubles(max).Take(Iterations).All(x => TMath.AreEqual(x, otherGen.NextDouble(max))));
        }

        [Test]
        public void Doubles_MaxValue_SmallNegativeMaxValue()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _generator.Doubles(SmallNeg).GetEnumerator().MoveNext();
            });
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Doubles_MinMaxValue_BetweenBounds()
        {
            var min = -Rand.Next();
            var max = Rand.Next() + 1; // To avoid zero
            Assert.True(_generator.Doubles(min, max).Take(Iterations).All(x => x >= min && x < max));
        }

        [Test]
        public void Doubles_MinMaxValue_DiffNegativeInfinity()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _generator.Doubles(Double.PositiveInfinity, Double.NegativeInfinity).GetEnumerator().MoveNext();
            });
        }

        [Test]
        public void Doubles_MinMaxValue_DiffPositiveInfinity()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _generator.Doubles(Double.NegativeInfinity, Double.PositiveInfinity).GetEnumerator().MoveNext();
            });
        }

        [Test]
        public void Doubles_MinMaxValue_MaxLessThanMin_LargeDiff()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _generator.Doubles(LargePos, LargeNeg).GetEnumerator().MoveNext();
            });
        }

        [Test]
        public void Doubles_MinMaxValue_MaxLessThanMin_SmallDiff()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _generator.Doubles(SmallPos, SmallNeg).GetEnumerator().MoveNext();
            });
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Doubles_MinMaxValue_SameOutputAsNextDouble()
        {
            var min = -Rand.Next();
            var max = Rand.Next() + 1; // To avoid zero
            var otherGen = GetGenerator(_generator.Seed);
            Assert.True(_generator.Doubles(min, max).Take(Iterations).All(x => TMath.AreEqual(x, otherGen.NextDouble(min, max))));
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Doubles_MinMaxValue_SameOutputAsNextDouble_AfterReset()
        {
            var min = -Rand.Next();
            var max = Rand.Next() + 1; // To avoid zero
            var otherGen = GetGenerator(_generator.Seed);
            Assert.True(_generator.Doubles(min, max).Take(Iterations).All(x => TMath.AreEqual(x, otherGen.NextDouble(min, max))));
            _generator.Reset();
            otherGen.Reset();
            Assert.True(_generator.Doubles(min, max).Take(Iterations).All(x => TMath.AreEqual(x, otherGen.NextDouble(min, max))));
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Doubles_SameOutputAsNextDouble()
        {
            var otherGen = GetGenerator(_generator.Seed);
            Assert.True(_generator.Doubles().Take(Iterations).All(x => TMath.AreEqual(x, otherGen.NextDouble())));
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Doubles_SameOutputAsNextDouble_AfterReset()
        {
            var otherGen = GetGenerator(_generator.Seed);
            Assert.True(_generator.Doubles().Take(Iterations).All(x => TMath.AreEqual(x, otherGen.NextDouble())));
            _generator.Reset();
            otherGen.Reset();
            Assert.True(_generator.Doubles().Take(Iterations).All(x => TMath.AreEqual(x, otherGen.NextDouble())));
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Integers_BetweenBounds()
        {
            Assert.True(_generator.Integers().Take(Iterations).All(x => x >= 0 && x < int.MaxValue));
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Integers_MaxValue_BetweenBounds()
        {
            var max = Rand.Next() + 1; // To avoid zero
            Assert.True(_generator.Integers(max).Take(Iterations).All(x => x >= 0 && x < max));
        }

        [Test]
        public void Integers_MaxValue_LargeNegativeMaxValue()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _generator.Integers((int) LargeNeg).GetEnumerator().MoveNext();
            });
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Integers_MaxValue_SameOutputAsNextInteger()
        {
            var max = Rand.Next() + 1; // To avoid zero
            var otherGen = GetGenerator(_generator.Seed);
            Assert.True(_generator.Integers(max).Take(Iterations).All(x => x == otherGen.Next(max)));
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Integers_MaxValue_SameOutputAsNextInteger_AfterReset()
        {
            var max = Rand.Next() + 1; // To avoid zero
            var otherGen = GetGenerator(_generator.Seed);
            Assert.True(_generator.Integers(max).Take(Iterations).All(x => x == otherGen.Next(max)));
            _generator.Reset();
            otherGen.Reset();
            Assert.True(_generator.Integers(max).Take(Iterations).All(x => x == otherGen.Next(max)));
        }

        [Test]
        public void Integers_MaxValue_SmallNegativeMaxValue()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _generator.Integers((int) SmallNeg).GetEnumerator().MoveNext();
            });
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Integers_MinMaxValue_BetweenBounds()
        {
            var min = -Rand.Next();
            var max = Rand.Next() + 1; // To avoid zero
            Assert.True(_generator.Integers(min, max).Take(Iterations).All(x => x >= min && x < max));
        }

        [Test]
        public void Integers_MinMaxValue_MaxLessThanMin_LargeDiff()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _generator.Integers((int) LargePos, (int) LargeNeg).GetEnumerator().MoveNext();
            });
        }

        [Test]
        public void Integers_MinMaxValue_MaxLessThanMin_SmallDiff()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _generator.Integers((int) SmallPos, (int) SmallNeg).GetEnumerator().MoveNext();
            });
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Integers_MinMaxValue_SameOutputAsNextInteger()
        {
            var min = -Rand.Next();
            var max = Rand.Next() + 1; // To avoid zero
            var otherGen = GetGenerator(_generator.Seed);
            Assert.True(_generator.Integers(min, max).Take(Iterations).All(x => x == otherGen.Next(min, max)));
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Integers_MinMaxValue_SameOutputAsNextInteger_AfterReset()
        {
            var min = -Rand.Next();
            var max = Rand.Next() + 1; // To avoid zero
            var otherGen = GetGenerator(_generator.Seed);
            Assert.True(_generator.Integers(min, max).Take(Iterations).All(x => x == otherGen.Next(min, max)));
            _generator.Reset();
            otherGen.Reset();
            Assert.True(_generator.Integers(min, max).Take(Iterations).All(x => x == otherGen.Next(min, max)));
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Integers_SameOutputAsNextInteger()
        {
            var otherGen = GetGenerator(_generator.Seed);
            Assert.True(_generator.Integers().Take(Iterations).All(x => x == otherGen.Next()));
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Integers_SameOutputAsNextInteger_AfterReset()
        {
            var otherGen = GetGenerator(_generator.Seed);
            Assert.True(_generator.Integers().Take(Iterations).All(x => x == otherGen.Next()));
            _generator.Reset();
            otherGen.Reset();
            Assert.True(_generator.Integers().Take(Iterations).All(x => x == otherGen.Next()));
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void UnsignedIntegers_BetweenBounds()
        {
            Assert.True(_generator.UnsignedIntegers().Take(Iterations).All(x => x <= uint.MaxValue));
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void UnsignedIntegers_MaxValue_BetweenBounds()
        {
            var max = Rand.NextUInt() + 1U; // To avoid zero
            Assert.True(_generator.UnsignedIntegers(max).Take(Iterations).All(x => x < max));
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Integers_MaxValue_SameOutputAsNextUInt()
        {
            var max = Rand.NextUInt() + 1U; // To avoid zero
            var otherGen = GetGenerator(_generator.Seed);
            Assert.True(_generator.UnsignedIntegers(max).Take(Iterations).All(x => x == otherGen.NextUInt(max)));
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void UnsignedIntegers_MaxValue_SameOutputAsNextUInt_AfterReset()
        {
            var max = Rand.NextUInt() + 1U; // To avoid zero
            var otherGen = GetGenerator(_generator.Seed);
            Assert.True(_generator.UnsignedIntegers(max).Take(Iterations).All(x => x == otherGen.NextUInt(max)));
            _generator.Reset();
            otherGen.Reset();
            Assert.True(_generator.UnsignedIntegers(max).Take(Iterations).All(x => x == otherGen.NextUInt(max)));
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void UnsignedIntegers_MinMaxValue_BetweenBounds()
        {
            var min = Rand.NextUInt(100);
            var max = Rand.NextUInt(100, 1000);
            Assert.True(_generator.UnsignedIntegers(min, max).Take(Iterations).All(x => x >= min && x < max));
        }

        [Test]
        public void UnsignedIntegers_MinMaxValue_MaxLessThanMin_LargeDiff()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _generator.UnsignedIntegers((uint) LargePos, 0).GetEnumerator().MoveNext();
            });
        }

        [Test]
        public void UnsignedIntegers_MinMaxValue_MaxLessThanMin_SmallDiff()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _generator.UnsignedIntegers((uint) LargePos, (uint) LargePos - 1U).GetEnumerator().MoveNext();
            });
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void UnsignedIntegers_MinMaxValue_SameOutputAsNextUInt()
        {
            var min = Rand.NextUInt(100);
            var max = Rand.NextUInt(100, 1000);
            var otherGen = GetGenerator(_generator.Seed);
            Assert.True(_generator.UnsignedIntegers(min, max).Take(Iterations).All(x => x == otherGen.NextUInt(min, max)));
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void UnsignedIntegers_MinMaxValue_SameOutputAsNextUInt_AfterReset()
        {
            var min = Rand.NextUInt(100);
            var max = Rand.NextUInt(100, 1000);
            var otherGen = GetGenerator(_generator.Seed);
            Assert.True(_generator.UnsignedIntegers(min, max).Take(Iterations).All(x => x == otherGen.NextUInt(min, max)));
            _generator.Reset();
            otherGen.Reset();
            Assert.True(_generator.UnsignedIntegers(min, max).Take(Iterations).All(x => x == otherGen.NextUInt(min, max)));
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void UnsignedIntegers_SameOutputAsNextUInt()
        {
            var otherGen = GetGenerator(_generator.Seed);
            Assert.True(_generator.UnsignedIntegers().Take(Iterations).All(x => x == otherGen.NextUInt()));
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void UnsignedIntegers_SameOutputAsNextUInt_AfterReset()
        {
            var otherGen = GetGenerator(_generator.Seed);
            Assert.True(_generator.UnsignedIntegers().Take(Iterations).All(x => x == otherGen.NextUInt()));
            _generator.Reset();
            otherGen.Reset();
            Assert.True(_generator.UnsignedIntegers().Take(Iterations).All(x => x == otherGen.NextUInt()));
        }

        /*=============================================================================
            NextUInt
        =============================================================================*/

        [Test]
        [Repeat(RepetitionCount)]
        public void NextUInt_NoMaxValue()
        {
            for (var i = 0; i < Iterations; ++i)
            {
                Assert.AreNotEqual(uint.MaxValue, _generator.NextUInt());
            }
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void NextUInt_NoMaxValue_AfterReset()
        {
            for (var i = 0; i < Iterations; ++i)
            {
                Assert.AreNotEqual(uint.MaxValue, _generator.NextUInt());
            }
            _generator.Reset();
            for (var i = 0; i < Iterations; ++i)
            {
                Assert.AreNotEqual(uint.MaxValue, _generator.NextUInt());
            }
        }

        /*=============================================================================
            Next(max)
        =============================================================================*/

        [Test]
        public void Next_SmallNegativeMax()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _generator.Next((int) SmallNeg);
            });
        }

        [Test]
        public void Next_LargeNegativeMax()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _generator.Next((int) LargeNeg);
            });
        }

        /*=============================================================================
            Next(min, max)
        =============================================================================*/

        [Test]
        [Repeat(RepetitionCount)]
        public void Integers_MinEqualToMax()
        {
            _generator.Integers((int) SmallPos, (int) SmallPos).First().ShouldBe((int) SmallPos);
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Next_MinEqualToMax()
        {
            _generator.Next((int) SmallPos, (int) SmallPos).ShouldBe((int) SmallPos);
        }

        [Test]
        public void Next_MinGreaterThanMax()
        {
            Should.Throw<ArgumentOutOfRangeException>(() => _generator.Next((int) LargePos, (int) SmallPos));
        }

        /*=============================================================================
            NextDouble(min, max)
        =============================================================================*/

        [Test]
        [Repeat(RepetitionCount)]
        public void Doubles_MinEqualToMax()
        {
            _generator.Doubles(SmallPos, SmallPos).First().ShouldBe(SmallPos);
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void NextDouble_MinEqualToMax()
        {
            _generator.NextDouble(SmallPos, SmallPos).ShouldBe(SmallPos);
        }

        [Test]
        public void NextDouble_MinGreaterThanMax()
        {
            Should.Throw<ArgumentOutOfRangeException>(() => _generator.NextDouble(LargePos, SmallPos));
        }

        /*=============================================================================
            NextUInt(min, max)
        =============================================================================*/

        [Test]
        [Repeat(RepetitionCount)]
        public void UnsignedIntegers_MinEqualToMax()
        {
            _generator.UnsignedIntegers((uint) SmallPos, (uint) SmallPos).First().ShouldBe((uint) SmallPos);
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void NextUInt_MinEqualToMax()
        {
            _generator.NextUInt((uint) SmallPos, (uint) SmallPos).ShouldBe((uint) SmallPos);
        }

        [Test]
        public void NextUInt_MinGreaterThanMax()
        {
            Should.Throw<ArgumentOutOfRangeException>(() => _generator.NextUInt((uint) LargePos, (uint) SmallPos));
        }

        /*=============================================================================
            Choice
        =============================================================================*/

        [Test]
        public void Choice_NullList()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _generator.Choice((IList<string>) null);
            });
        }

        [Test]
        public void Choice_EmptyList()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _generator.Choice(new string[0]);
            });
        }

        [Test]
        public void Choice_OneItem()
        {
            var list = new[] { 1 };
            for (var i = 0; i < Iterations; ++i)
            {
                Assert.AreEqual(1, _generator.Choice(list));
            }
        }

        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(20)]
        [TestCase(50)]
        [TestCase(100)]
        public void Choice_ManyItems(int len)
        {
            var list = new int[len];
            for (var i = 0; i < len; ++i)
            {
                list[i] = i;
            }
            var counts = new double[len];
            for (var i = 0; i < Iterations * 2; ++i)
            {
                counts[_generator.Choice(list)]++;
            }
            var expected = Iterations * 2 / len;
            var tolerance = expected / 20;
            for (var i = 0; i < len; ++i)
            {
                Assert.Less(Math.Abs(expected - counts[i]), tolerance);
            }
        }

        [Test]
        public void Choice_FromString()
        {
            var str = new[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I' };
            var counts = new double[str.Length];
            for (var i = 0; i < Iterations * 2; ++i)
            {
                counts[_generator.Choice(str) - 'A']++;
            }
            var expected = Iterations * 2 / str.Length;
            var tolerance = expected / 20;
            for (var i = 0; i < str.Length; ++i)
            {
                Assert.Less(Math.Abs(expected - counts[i]), tolerance);
            }
        }

        [Test]
        public void Choice_IEnumerable_NullList()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _generator.Choice((IList<string>) null);
            });
        }

        [Test]
        public void Choice_IEnumerable_EmptyList()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _generator.Choice(new string[0]);
            });
        }

        [Test]
        public void Choice_IEnumerable_OneItem()
        {
            var list = new[] { 1 };
            for (var i = 0; i < Iterations; ++i)
            {
                Assert.AreEqual(1, _generator.Choice(list));
            }
        }

        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(20)]
        [TestCase(50)]
        [TestCase(100)]
        public void Choice_IEnumerable_ManyItems(int len)
        {
            var list = new int[len];
            for (var i = 0; i < len; ++i)
            {
                list[i] = i;
            }
            var counts = new double[len];
            for (var i = 0; i < Iterations * 2; ++i)
            {
                counts[_generator.Choice(list)]++;
            }
            var expected = Iterations * 2 / len;
            var tolerance = expected / 20;
            for (var i = 0; i < len; ++i)
            {
                Assert.Less(Math.Abs(expected - counts[i]), tolerance);
            }
        }

        /*=============================================================================
            Choices
        =============================================================================*/

        [Test]
        public void Choices_NullList()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _generator.Choices((IList<string>) null).GetEnumerator().MoveNext();
            });
        }

        [Test]
        public void Choices_EmptyList()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _generator.Choices(new string[0]).GetEnumerator().MoveNext();
            });
        }

        [Test]
        public void Choices_OneItem()
        {
            var list = new[] { 1 };
            var en = _generator.Choices(list).GetEnumerator();
            for (var i = 0; i < Iterations; ++i)
            {
                en.MoveNext();
                Assert.AreEqual(1, en.Current);
            }
        }

        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(20)]
        [TestCase(50)]
        [TestCase(100)]
        public void Choices_ManyItems(int len)
        {
            var list = new int[len];
            for (var i = 0; i < len; ++i)
            {
                list[i] = i;
            }
            var counts = new double[len];
            var en = _generator.Choices(list).GetEnumerator();
            for (var i = 0; i < Iterations * 2; ++i)
            {
                en.MoveNext();
                counts[en.Current]++;
            }
            var expected = Iterations * 2 / len;
            var tolerance = expected / 20;
            for (var i = 0; i < len; ++i)
            {
                Assert.Less(Math.Abs(expected - counts[i]), tolerance);
            }
        }

        [Test]
        public void Choices_FromString()
        {
            var str = new[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I' };
            var counts = new double[str.Length];
            var en = _generator.Choices(str).GetEnumerator();
            for (var i = 0; i < Iterations * 2; ++i)
            {
                en.MoveNext();
                counts[en.Current - 'A']++;
            }
            var expected = Iterations * 2 / str.Length;
            var tolerance = expected / 20;
            for (var i = 0; i < str.Length; ++i)
            {
                Assert.Less(Math.Abs(expected - counts[i]), tolerance);
            }
        }

        [Test]
        public void Choices_IEnumerable_NullList()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _generator.Choices((IList<string>) null).GetEnumerator().MoveNext();
            });
        }

        [Test]
        public void Choices_IEnumerable_EmptyList()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _generator.Choices(new string[0]).GetEnumerator().MoveNext();
            });
        }

        [Test]
        public void Choices_IEnumerable_OneItem()
        {
            var list = new[] { 1 };
            var en = _generator.Choices(list).GetEnumerator();
            for (var i = 0; i < Iterations; ++i)
            {
                en.MoveNext();
                Assert.AreEqual(1, en.Current);
            }
        }

        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(20)]
        [TestCase(50)]
        [TestCase(100)]
        public void Choices_IEnumerable_ManyItems(int len)
        {
            var list = new int[len];
            for (var i = 0; i < len; ++i)
            {
                list[i] = i;
            }
            var counts = new double[len];
            var en = _generator.Choices(list).GetEnumerator();
            for (var i = 0; i < Iterations * 2; ++i)
            {
                en.MoveNext();
                counts[en.Current]++;
            }
            var expected = Iterations * 2 / len;
            var tolerance = expected / 20;
            for (var i = 0; i < len; ++i)
            {
                Assert.Less(Math.Abs(expected - counts[i]), tolerance);
            }
        }
    }
}