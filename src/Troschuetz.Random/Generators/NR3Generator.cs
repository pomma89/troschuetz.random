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

namespace Troschuetz.Random.Generators
{
    using System;
    using System.Diagnostics;

    /// <summary>
    ///   A generator whose original code has been found in a famous book about numerical analysis.
    ///   Inside the book, it is the highest quality recommended generator.
    /// </summary>
    /// <remarks>
    ///   This generator has a period of ~ 3.138 * 10^57.
    ///
    ///   This generator is NOT thread safe.
    /// </remarks>
#if HAS_SERIALIZABLE
    [Serializable]
#endif

    public sealed class NR3Generator : AbstractGenerator
    {
        #region Constants

        /// <summary>
        ///   Represents the seed for the <see cref="_v"/> variable. This field is constant.
        /// </summary>
        /// <remarks>The value of this constant is 4101842887655102017.</remarks>
        public const ulong SeedV = 4101842887655102017UL;

        /// <summary>
        ///   Represents the seed for the <see cref="_w"/> variable. This field is constant.
        /// </summary>
        /// <remarks>The value of this constant is 1.</remarks>
        public const ulong SeedW = 1UL;

        /// <summary>
        ///   Represents the seed for the <see cref="ulong"/> numbers generation. This field is constant.
        /// </summary>
        /// <remarks>The value of this constant is 2862933555777941757.</remarks>
        public const ulong SeedU1 = 2862933555777941757UL;

        /// <summary>
        ///   Represents the seed for the <see cref="ulong"/> numbers generation. This field is constant.
        /// </summary>
        /// <remarks>The value of this constant is 7046029254386353087.</remarks>
        public const ulong SeedU2 = 7046029254386353087UL;

        /// <summary>
        ///   Represents the seed for the <see cref="ulong"/> numbers generation. This field is constant.
        /// </summary>
        /// <remarks>The value of this constant is 2685821657736338717.</remarks>
        public const ulong SeedU3 = 4294957665UL;

        #endregion Constants

        #region Fields

        private ulong _u;
        private ulong _v;
        private ulong _w;
        private ulong _x;

        /// <summary>
        ///   Generators like <see cref="NextDouble"/> and <see cref="NextInclusiveMaxValue"/> use
        ///   only 32 bits to produce a random result, even if the core algorithm of this generator
        ///   produces 64 random bits at each iteration. Therefore, instead of throwing 32 bits away
        ///   every time those methods are called, we use this flag to signal that there 32 bits
        ///   still available and ready to be used.
        /// </summary>
        private bool _bytesAvailable;

        #endregion Fields

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="NR3Generator"/> class, using a
        ///   time-dependent default seed value.
        /// </summary>
        public NR3Generator() : base(TMath.Seed())
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="NR3Generator"/> class, using the specified
        ///   seed value.
        /// </summary>
        /// <param name="seed">
        ///   A number used to calculate a starting value for the pseudo-random number sequence. If a
        ///   negative number is specified, the absolute value of the number is used.
        /// </param>
        public NR3Generator(int seed) : base((uint) Math.Abs(seed))
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="NR3Generator"/> class, using the specified
        ///   seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        public NR3Generator(uint seed) : base(seed)
        {
        }

        #endregion Construction

        #region IGenerator members

        /// <summary>
        ///   Gets a value indicating whether the random number generator can be reset, so that it
        ///   produces the same random number sequence again.
        /// </summary>
        public override bool CanReset => true;

        /// <summary>
        ///   Resets the random number generator using the specified seed, so that it produces the
        ///   same random number sequence again. To understand whether this generator can be reset,
        ///   you can query the <see cref="CanReset"/> property.
        /// </summary>
        /// <param name="seed">The seed value used by the generator.</param>
        /// <returns>True if the random number generator was reset; otherwise, false.</returns>
        public override bool Reset(uint seed)
        {
            base.Reset(seed);

            _v = SeedV;
            _w = SeedW;
            _u = seed ^ _v;
            NextULong();
            _v = _u;
            NextULong();
            _w = _v;
            NextULong();
            _bytesAvailable = false;
            return true;
        }

        /// <summary>
        ///   Returns a nonnegative random number less than or equal to <see cref="int.MaxValue"/>.
        /// </summary>
        /// <returns>
        ///   A 32-bit signed integer greater than or equal to 0, and less than or equal to
        ///   <see cref="int.MaxValue"/>; that is, the range of return values includes 0 and <see cref="int.MaxValue"/>.
        /// </returns>
        public override int NextInclusiveMaxValue()
        {
            if (_bytesAvailable)
            {
                _bytesAvailable = false;
                return (int) (((_x + _v) ^ _w) << ULongToIntShift >> ULongToIntShift);
            }

            // Its faster to explicitly calculate the unsigned random number than simply call NextULong().
            _u = _u * SeedU1 + SeedU2;
            _v ^= _v >> 17;
            _v ^= _v << 31;
            _v ^= _v >> 8;
            _w = SeedU3 * (_w & 0xFFFFFFFFUL) + (_w >> 32);
            _x = _u ^ (_u << 21);
            _x ^= _x >> 35;
            _x ^= _x << 4;
            _bytesAvailable = true;

            var result = (int) (((_x + _v) ^ _w) >> ULongToIntShift);

            // Postconditions
            Debug.Assert(result >= 0);
            return result;
        }

        /// <summary>
        ///   Returns a nonnegative floating point random number less than 1.0.
        /// </summary>
        /// <returns>
        ///   A double-precision floating point number greater than or equal to 0.0, and less than
        ///   1.0; that is, the range of return values includes 0.0 but not 1.0.
        /// </returns>
        public override double NextDouble()
        {
            if (_bytesAvailable)
            {
                _bytesAvailable = false;
                return (int) (((_x + _v) ^ _w) << ULongToIntShift >> ULongToIntShift) * IntToDoubleMultiplier;
            }

            // Its faster to explicitly calculate the unsigned random number than simply call NextULong().
            _u = _u * SeedU1 + SeedU2;
            _v ^= _v >> 17;
            _v ^= _v << 31;
            _v ^= _v >> 8;
            _w = SeedU3 * (_w & 0xFFFFFFFFUL) + (_w >> 32);
            _x = _u ^ (_u << 21);
            _x ^= _x >> 35;
            _x ^= _x << 4;
            _bytesAvailable = true;

            var result = (int) (((_x + _v) ^ _w) >> ULongToIntShift) * IntToDoubleMultiplier;

            // Postconditions
            Debug.Assert(result >= 0.0 && result < 1.0);
            return result;
        }

        /// <summary>
        ///   Returns an unsigned random number.
        /// </summary>
        /// <returns>
        ///   A 32-bit unsigned integer greater than or equal to 0, and less than or equal to
        ///   <see cref="uint.MaxValue"/>; that is, the range of return values includes 0 and <see cref="uint.MaxValue"/>.
        /// </returns>
        public override uint NextUIntInclusiveMaxValue()
        {
            if (_bytesAvailable)
            {
                _bytesAvailable = false;
                return (uint) (((_x + _v) ^ _w) << ULongToUIntShift >> ULongToUIntShift);
            }

            // Its faster to explicitly calculate the unsigned random number than simply call NextULong().
            _u = _u * SeedU1 + SeedU2;
            _v ^= _v >> 17;
            _v ^= _v << 31;
            _v ^= _v >> 8;
            _w = SeedU3 * (_w & 0xFFFFFFFFUL) + (_w >> 32);
            _x = _u ^ (_u << 21);
            _x ^= _x >> 35;
            _x ^= _x << 4;
            _bytesAvailable = true;
            return (uint) (((_x + _v) ^ _w) >> ULongToUIntShift);
        }

        /// <summary>
        ///   Returns an unsigned long random number.
        /// </summary>
        /// <returns>
        ///   A 64-bit unsigned integer greater than or equal to <see cref="ulong.MinValue"/> and
        ///   less than or equal to <see cref="ulong.MaxValue"/>.
        /// </returns>
        public ulong NextULong()
        {
            _u = _u * SeedU1 + SeedU2;
            _v ^= _v >> 17;
            _v ^= _v << 31;
            _v ^= _v >> 8;
            _w = SeedU3 * (_w & 0xFFFFFFFFUL) + (_w >> 32);
            _x = _u ^ (_u << 21);
            _x ^= _x >> 35;
            _x ^= _x << 4;
            _bytesAvailable = false;
            return (_x + _v) ^ _w;
        }

        #endregion IGenerator members
    }
}