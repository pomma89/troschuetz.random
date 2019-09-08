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

#region Original Summary

/*
/// <summary>
///   A fast random number generator for .NET Colin Green, January 2005
///
///   September 4th 2005 Added NextBytesUnsafe() - commented out by default. Fixed bug in
///   Reinitialise() - y,z and w variables were not being reset.
///
///   Key points:
///   1) Based on a simple and fast xor-shift pseudo random number generator (RNG) specified in:
///   Marsaglia, George. (2003). Xorshift RNGs. http://www.jstatsoft.org/v08/i14/xorshift.pdf
///
///   This particular implementation of xorshift has a period of 2^128-1. See the above paper to see
///   how this can be easily extened if you need a longer period. At the time of writing I could find
///   no information on the period of System.Random for comparison.
///
///   2) Faster than System.Random. Up to 15x faster, depending on which methods are called.
///
///   3) Direct replacement for System.Random. This class implements all of the methods that
///   System.Random does plus some additional methods. The like named methods are functionally equivalent.
///
///   4) Allows fast re-initialisation with a seed, unlike System.Random which accepts a seed at
///   construction time which then executes a relatively expensive initialisation routine. This
///   provides a vast speed improvement if you need to reset the pseudo-random number sequence many
///   times, e.g. if you want to re-generate the same sequence many times. An alternative might be to
///   cache random numbers in an array, but that approach is limited by memory capacity and the fact
///   that you may also want a large number of different sequences cached. Each sequence can each be
///   represented by a single seed value (int) when using FastRandom.
///
///   Notes. A further performance improvement can be obtained by declaring local variables as
///   static, thus avoiding re-allocation of variables on each call. However care should be taken if
///   multiple instances of FastRandom are in use or if being used in a multi-threaded environment.
/// </summary>
 */

#endregion Original Summary

namespace Troschuetz.Random.Generators
{
    using System;
    using System.Diagnostics;

    /// <summary>
    ///   Represents a xorshift pseudo-random number generator with period 2^128-1.
    /// </summary>
    /// <remarks>
    ///   The <see cref="XorShift128Generator"/> type bases upon the implementation presented in the
    ///   CP article " <a href="http://www.codeproject.com/csharp/fastrandom.asp">A fast equivalent
    ///   for System.Random</a>" and the theoretical background on xorshift random number generators
    ///   published by George Marsaglia in this paper " <a
    ///   href="http://www.jstatsoft.org/v08/i14/xorshift.pdf">Xorshift RNGs</a>".
    ///
    ///   This generator is NOT thread safe.
    /// </remarks>
#if HAS_SERIALIZABLE
    [Serializable]
#endif

    public sealed class XorShift128Generator : AbstractGenerator
    {
        #region Constants

        /// <summary>
        ///   Represents the seed for the <see cref="_x"/> variable. This field is constant.
        /// </summary>
        /// <remarks>
        ///   The value of this constant is 521288629, left shifted by 32 bits. The right side will
        ///   be filled with the specified seed.
        /// </remarks>
        public const ulong SeedX = 521288629UL << 32;

        /// <summary>
        ///   Represents the seed for the <see cref="_y"/> variable. This field is constant.
        /// </summary>
        /// <remarks>The value of this constant is 362436069.</remarks>
        public const ulong SeedY = 362436069UL;

        #endregion Constants

        #region Fields

        /// <summary>
        ///   The first part of the generator state. It is important that <see cref="_x"/> and <see
        ///   cref="_y"/> are not zero at the same time.
        /// </summary>
        private ulong _x;

        /// <summary>
        ///   The second part of the generator state. It is important that <see cref="_x"/> and <see
        ///   cref="_y"/> are not zero at the same time.
        /// </summary>
        private ulong _y;

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
        ///   Initializes a new instance of the <see cref="XorShift128Generator"/> class, using a
        ///   time-dependent default seed value.
        /// </summary>
        public XorShift128Generator() : base(TMath.Seed())
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="XorShift128Generator"/> class, using the
        ///   specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   A number used to calculate a starting value for the pseudo-random number sequence. If a
        ///   negative number is specified, the absolute value of the number is used.
        /// </param>
        public XorShift128Generator(int seed) : base((uint)Math.Abs(seed))
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="XorShift128Generator"/> class, using the
        ///   specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        public XorShift128Generator(uint seed) : base(seed)
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

            // "The seed set for xor128 is two 64-bit integers x,y not all 0, ..." (George
            // Marsaglia) To meet that requirement the x, y seeds are constant values greater 0.
            _x = SeedX + seed;
            _y = SeedY * ((ulong)seed << 32);
            _bytesAvailable = false;

            // Discard first result to achieve better randomness.
            NextUInt();

            return true;
        }

        /// <summary>
        ///   Returns a nonnegative random number less than or equal to <see cref="int.MaxValue"/>.
        /// </summary>
        /// <returns>
        ///   A 32-bit signed integer greater than or equal to 0, and less than or equal to <see
        ///   cref="int.MaxValue"/>; that is, the range of return values includes 0 and <see cref="int.MaxValue"/>.
        /// </returns>
        public override int NextInclusiveMaxValue()
        {
            if (_bytesAvailable)
            {
                _bytesAvailable = false;
                return (int)((_x + _y) << ULongToIntShift >> ULongToIntShift);
            }

            // Its faster to explicitly calculate the unsigned random number than simply call NextULong().
            var tx = _x;
            var ty = _y;
            _x = ty;
            tx ^= tx << 23;
            tx ^= tx >> 17;
            tx ^= ty ^ (ty >> 26);
            _y = tx;
            _bytesAvailable = true;

            var result = (int)((tx + ty) >> ULongToIntShift);

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
                return (int)((_x + _y) << ULongToIntShift >> ULongToIntShift) * IntToDoubleMultiplier;
            }

            // Its faster to explicitly calculate the unsigned random number than simply call NextULong().
            var tx = _x;
            var ty = _y;
            _x = ty;
            tx ^= tx << 23;
            tx ^= tx >> 17;
            tx ^= ty ^ (ty >> 26);
            _y = tx;
            _bytesAvailable = true;

            var result = (int)((tx + ty) >> ULongToIntShift) * IntToDoubleMultiplier;

            // Postconditions
            Debug.Assert(result >= 0.0 && result < 1.0);
            return result;
        }

        /// <summary>
        ///   Returns an unsigned random number.
        /// </summary>
        /// <returns>
        ///   A 32-bit unsigned integer greater than or equal to 0, and less than or equal to <see
        ///   cref="uint.MaxValue"/>; that is, the range of return values includes 0 and <see cref="uint.MaxValue"/>.
        /// </returns>
        public override uint NextUIntInclusiveMaxValue()
        {
            if (_bytesAvailable)
            {
                _bytesAvailable = false;
                return (uint)((_x + _y) << ULongToUIntShift >> ULongToUIntShift);
            }

            // Its faster to explicitly calculate the unsigned random number than simply call NextULong().
            var tx = _x;
            var ty = _y;
            _x = ty;
            tx ^= tx << 23;
            tx ^= tx >> 17;
            tx ^= ty ^ (ty >> 26);
            _y = tx;
            _bytesAvailable = true;

            return (uint)((tx + ty) >> ULongToUIntShift);
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
            var tx = _x;
            var ty = _y;
            _x = ty;
            tx ^= tx << 23;
            tx ^= tx >> 17;
            tx ^= ty ^ (ty >> 26);
            _y = tx;
            _bytesAvailable = false;
            return tx + ty;
        }

        #endregion IGenerator members
    }
}