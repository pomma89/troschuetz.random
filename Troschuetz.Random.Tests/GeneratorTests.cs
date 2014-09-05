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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;

    public abstract class GeneratorTests : TestBase
    {
        #region Setup/Teardown

        [SetUp]
        public virtual void SetUp()
        {
            _generator = (_currGen) ? GetGenerator() : GetGenerator(Rand.Next());
            _currGen = !_currGen;
        }

        #endregion

        const int RepetitionCount = 2;
        IGenerator _generator;
        bool _currGen = true;

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
        [ExpectedException(typeof(ArgumentNullException))]
        public void Bytes_NullBuffer()
        {
            _generator.Bytes(null).GetEnumerator().MoveNext();
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
            for (var i = 0; i < Iterations; ++i, bytesEn.MoveNext()) {
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
            for (var i = 0; i < Iterations; ++i, bytesEn.MoveNext()) {
                otherGen.NextBytes(b2);
                Assert.AreEqual(b2, b1);
            }
            _generator.Reset();
            otherGen.Reset();
            bytesEn.MoveNext();
            for (var i = 0; i < Iterations; ++i, bytesEn.MoveNext()) {
                otherGen.NextBytes(b2);
                Assert.AreEqual(b2, b1);
            }
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
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Doubles_MaxValue_LargeNegativeMaxValue()
        {
            _generator.Doubles(LargeNeg).GetEnumerator().MoveNext();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Doubles_MaxValue_PositiveInfinityMaxValue()
        {
            const double max = double.PositiveInfinity;
            Assert.True(_generator.Doubles(max).Take(Iterations).All(x => x >= 0 && x < max));
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Doubles_MaxValue_SameOutputAsNextDouble()
        {
            var max = Rand.Next() + 1; // To avoid zero
            var otherGen = GetGenerator(_generator.Seed);
            Assert.True(_generator.Doubles(max).Take(Iterations).All(x => x == otherGen.NextDouble(max)));
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Doubles_MaxValue_SameOutputAsNextDouble_AfterReset()
        {
            var max = Rand.Next() + 1; // To avoid zero
            var otherGen = GetGenerator(_generator.Seed);
            Assert.True(_generator.Doubles(max).Take(Iterations).All(x => x == otherGen.NextDouble(max)));
            _generator.Reset();
            otherGen.Reset();
            Assert.True(_generator.Doubles(max).Take(Iterations).All(x => x == otherGen.NextDouble(max)));
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Doubles_MaxValue_SmallNegativeMaxValue()
        {
            _generator.Doubles(SmallNeg).GetEnumerator().MoveNext();
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
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Doubles_MinMaxValue_DiffNegativeInfinity()
        {
            _generator.Doubles(Double.PositiveInfinity, Double.NegativeInfinity).GetEnumerator().MoveNext();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Doubles_MinMaxValue_DiffPositiveInfinity()
        {
            _generator.Doubles(Double.NegativeInfinity, Double.PositiveInfinity).GetEnumerator().MoveNext();
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Doubles_MinMaxValue_MaxLessThanMin_LargeDiff()
        {
            _generator.Doubles(LargePos, LargeNeg).GetEnumerator().MoveNext();
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Doubles_MinMaxValue_MaxLessThanMin_SmallDiff()
        {
            _generator.Doubles(SmallPos, SmallNeg).GetEnumerator().MoveNext();
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Doubles_MinMaxValue_SameOutputAsNextDouble()
        {
            var min = -Rand.Next();
            var max = Rand.Next() + 1; // To avoid zero
            var otherGen = GetGenerator(_generator.Seed);
            Assert.True(_generator.Doubles(min, max).Take(Iterations).All(x => x == otherGen.NextDouble(min, max)));
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Doubles_MinMaxValue_SameOutputAsNextDouble_AfterReset()
        {
            var min = -Rand.Next();
            var max = Rand.Next() + 1; // To avoid zero
            var otherGen = GetGenerator(_generator.Seed);
            Assert.True(_generator.Doubles(min, max).Take(Iterations).All(x => x == otherGen.NextDouble(min, max)));
            _generator.Reset();
            otherGen.Reset();
            Assert.True(_generator.Doubles(min, max).Take(Iterations).All(x => x == otherGen.NextDouble(min, max)));
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Doubles_SameOutputAsNextDouble()
        {
            var otherGen = GetGenerator(_generator.Seed);
            Assert.True(_generator.Doubles().Take(Iterations).All(x => x == otherGen.NextDouble()));
        }

        [Test]
        [Repeat(RepetitionCount)]
        public void Doubles_SameOutputAsNextDouble_AfterReset()
        {
            var otherGen = GetGenerator(_generator.Seed);
            Assert.True(_generator.Doubles().Take(Iterations).All(x => x == otherGen.NextDouble()));
            _generator.Reset();
            otherGen.Reset();
            Assert.True(_generator.Doubles().Take(Iterations).All(x => x == otherGen.NextDouble()));
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
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Integers_MaxValue_LargeNegativeMaxValue()
        {
            _generator.Integers((int) LargeNeg).GetEnumerator().MoveNext();
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
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Integers_MaxValue_SmallNegativeMaxValue()
        {
            _generator.Integers((int) SmallNeg).GetEnumerator().MoveNext();
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
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Integers_MinMaxValue_MaxLessThanMin_LargeDiff()
        {
            _generator.Integers((int) LargePos, (int) LargeNeg).GetEnumerator().MoveNext();
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Integers_MinMaxValue_MaxLessThanMin_SmallDiff()
        {
            _generator.Integers((int) SmallPos, (int) SmallNeg).GetEnumerator().MoveNext();
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
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UnsignedIntegers_MinMaxValue_MaxLessThanMin_LargeDiff()
        {
            _generator.UnsignedIntegers((uint) LargePos, 0).GetEnumerator().MoveNext();
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UnsignedIntegers_MinMaxValue_MaxLessThanMin_SmallDiff()
        {
            _generator.UnsignedIntegers((uint) LargePos, (uint) LargePos - 1U).GetEnumerator().MoveNext();
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
            Next(max)
        =============================================================================*/

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Next_SmallNegativeMax()
        {
            _generator.Next((int) SmallNeg);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Next_LargeNegativeMax()
        {
            _generator.Next((int) LargeNeg);
        }

        /*=============================================================================
            Next(min, max)
        =============================================================================*/

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Next_MinSmallerThanMax()
        {
            _generator.Next((int) LargePos, (int) SmallPos);
        }

        /*=============================================================================
            Choice
        =============================================================================*/

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Choice_NullList()
        {
            _generator.Choice((IList<string>) null);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Choice_EmptyList()
        {
            _generator.Choice(new string[0]);
        }

        [Test]
        public void Chioce_OneItem()
        {
            var list = new[] {1};
            for (var i = 0; i < Iterations; ++i) {
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
        public void Chioce_ManyItems(int len)
        {
            var list = new int[len];
            for (var i = 0; i < len; ++i) {
                list[i] = i;
            }
            var counts = new double[len];
            for (var i = 0; i < Iterations*2; ++i) {
                counts[_generator.Choice(list)]++;
            }
            var expected = Iterations*2/len;
            var tolerance = expected/20;
            for (var i = 0; i < len; ++i) {
                Assert.Less(Math.Abs(expected - counts[i]), tolerance);
            }
        }

        [Test]
        public void Chioce_FromString()
        {
            var str = new[] {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I'};
            var counts = new double[str.Length];
            for (var i = 0; i < Iterations*2; ++i) {
                counts[_generator.Choice(str) - 'A']++;
            }
            var expected = Iterations*2/str.Length;
            var tolerance = expected/20;
            for (var i = 0; i < str.Length; ++i) {
                Assert.Less(Math.Abs(expected - counts[i]), tolerance);
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Choice_IEnumerable_NullList()
        {
            _generator.Choice((IList<string>) null);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Choice_IEnumerable_EmptyList()
        {
            _generator.Choice(new string[0]);
        }

        [Test]
        public void Chioce_IEnumerable_OneItem()
        {
            var list = new[] {1};
            for (var i = 0; i < Iterations; ++i) {
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
        public void Chioce_IEnumerable_ManyItems(int len)
        {
            var list = new int[len];
            for (var i = 0; i < len; ++i) {
                list[i] = i;
            }
            var counts = new double[len];
            for (var i = 0; i < Iterations*2; ++i) {
                counts[_generator.Choice(list)]++;
            }
            var expected = Iterations*2/len;
            var tolerance = expected/20;
            for (var i = 0; i < len; ++i) {
                Assert.Less(Math.Abs(expected - counts[i]), tolerance);
            }
        }

        /*=============================================================================
            Choices
        =============================================================================*/

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Choices_NullList()
        {
            _generator.Choices((IList<string>) null).GetEnumerator().MoveNext();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Choices_EmptyList()
        {
            _generator.Choices(new string[0]).GetEnumerator().MoveNext();
        }

        [Test]
        public void Chioces_OneItem()
        {
            var list = new[] {1};
            var en = _generator.Choices(list).GetEnumerator();
            for (var i = 0; i < Iterations; ++i) {
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
        public void Chioces_ManyItems(int len)
        {
            var list = new int[len];
            for (var i = 0; i < len; ++i) {
                list[i] = i;
            }
            var counts = new double[len];
            var en = _generator.Choices(list).GetEnumerator();
            for (var i = 0; i < Iterations*2; ++i) {
                en.MoveNext();
                counts[en.Current]++;
            }
            var expected = Iterations*2/len;
            var tolerance = expected/20;
            for (var i = 0; i < len; ++i) {
                Assert.Less(Math.Abs(expected - counts[i]), tolerance);
            }
        }

        [Test]
        public void Chioces_FromString()
        {
            var str = new[] {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I'};
            var counts = new double[str.Length];
            var en = _generator.Choices(str).GetEnumerator();
            for (var i = 0; i < Iterations*2; ++i) {
                en.MoveNext();
                counts[en.Current - 'A']++;
            }
            var expected = Iterations*2/str.Length;
            var tolerance = expected/20;
            for (var i = 0; i < str.Length; ++i) {
                Assert.Less(Math.Abs(expected - counts[i]), tolerance);
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Choices_IEnumerable_NullList()
        {
            _generator.Choices((IList<string>) null).GetEnumerator().MoveNext();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Choices_IEnumerable_EmptyList()
        {
            _generator.Choices(new string[0]).GetEnumerator().MoveNext();
        }

        [Test]
        public void Chioces_IEnumerable_OneItem()
        {
            var list = new[] {1};
            var en = _generator.Choices(list).GetEnumerator();
            for (var i = 0; i < Iterations; ++i) {
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
        public void Chioces_IEnumerable_ManyItems(int len)
        {
            var list = new int[len];
            for (var i = 0; i < len; ++i) {
                list[i] = i;
            }
            var counts = new double[len];
            var en = _generator.Choices(list).GetEnumerator();
            for (var i = 0; i < Iterations*2; ++i) {
                en.MoveNext();
                counts[en.Current]++;
            }
            var expected = Iterations*2/len;
            var tolerance = expected/20;
            for (var i = 0; i < len; ++i) {
                Assert.Less(Math.Abs(expected - counts[i]), tolerance);
            }
        }
    }
}