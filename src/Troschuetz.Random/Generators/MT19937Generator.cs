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

#region Original Copyright

/*
   A C-program for MT19937, with initialization improved 2002/1/26.
   Coded by Takuji Nishimura and Makoto Matsumoto.

   Before using, initialize the state by using init_genrand(seed)
   or init_by_array(init_key, key_length).

   Copyright (C) 1997 - 2002, Makoto Matsumoto and Takuji Nishimura,
   All rights reserved.

   Redistribution and use in source and binary forms, with or without
   modification, are permitted provided that the following conditions
   are met:

     1. Redistributions of source code must retain the above copyright
        notice, this list of conditions and the following disclaimer.

     2. Redistributions in binary form must reproduce the above copyright
        notice, this list of conditions and the following disclaimer in the
        documentation and/or other materials provided with the distribution.

     3. The names of its contributors may not be used to endorse or promote
        products derived from this software without specific prior written
        permission.

   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
   "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
   LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
   A PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE COPYRIGHT OWNER OR
   CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
   EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
   PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
   PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
   LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
   NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
   SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

   Any feedback is very welcome.
   http://www.math.sci.hiroshima-u.ac.jp/~m-mat/MT/emt.html
   email: m-mat @ math.sci.hiroshima-u.ac.jp (remove space)
*/

#endregion Original Copyright

namespace Troschuetz.Random.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    ///   Represents a Mersenne Twister pseudo-random number generator with period 2^19937-1.
    /// </summary>
    /// <remarks>
    ///   The <see cref="MT19937Generator"/> type bases upon information and the implementation
    ///   presented on the
    ///   <a href="http://www.math.sci.hiroshima-u.ac.jp/~m-mat/MT/emt.html">Mersenne Twister Home Page</a>.
    ///
    ///   This generator is NOT thread safe.
    /// </remarks>
#if HAS_SERIALIZABLE
    [Serializable]
#endif

    public sealed class MT19937Generator : AbstractGenerator
    {
        #region Constants

        /// <summary>
        ///   Represents the number of unsigned random numbers generated at one time. This field is constant.
        /// </summary>
        /// <remarks>The value of this constant is 624.</remarks>
        private const int N = 624;

        /// <summary>
        ///   Represents a constant used for generation of unsigned random numbers. This field is constant.
        /// </summary>
        /// <remarks>The value of this constant is 397.</remarks>
        private const int M = 397;

        /// <summary>
        ///   Represents the constant vector a. This field is constant.
        /// </summary>
        /// <remarks>The value of this constant is 0x9908b0dfU.</remarks>
        private const uint VectorA = 0x9908b0dfU;

        /// <summary>
        ///   Represents the most significant w-r bits. This field is constant.
        /// </summary>
        /// <remarks>The value of this constant is 0x80000000.</remarks>
        private const uint UpperMask = 0x80000000U;

        /// <summary>
        ///   Represents the least significant r bits. This field is constant.
        /// </summary>
        /// <remarks>The value of this constant is 0x7fffffff.</remarks>
        private const uint LowerMask = 0x7fffffffU;

        #endregion Constants

        #region Fields

        /// <summary>
        ///   Stores the state vector array.
        /// </summary>
        private readonly uint[] _mt = new uint[N];

        /// <summary>
        ///   Stores the used seed array.
        /// </summary>
        private readonly uint[] _seedArray;

        /// <summary>
        ///   Stores an index for the state vector array element that will be accessed next.
        /// </summary>
        private uint _mti;

        #endregion Fields

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="MT19937Generator"/> class, using a
        ///   time-dependent default seed value.
        /// </summary>
        public MT19937Generator() : base(TMath.Seed())
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="MT19937Generator"/> class, using the
        ///   specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   A number used to calculate a starting value for the pseudo-random number sequence. If a
        ///   negative number is specified, the absolute value of the number is used.
        /// </param>
        public MT19937Generator(int seed) : base((uint) Math.Abs(seed))
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="MT19937Generator"/> class, using the
        ///   specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        public MT19937Generator(uint seed) : base(seed)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="MT19937Generator"/> class, using the
        ///   specified seed array.
        /// </summary>
        /// <param name="seedArray">
        ///   An array of numbers used to calculate a starting values for the pseudo-random number
        ///   sequence. If negative numbers are specified, the absolute values of them are used.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="seedArray"/> is NULL ( <see langword="Nothing"/> in Visual Basic).
        /// </exception>
        public MT19937Generator(IList<int> seedArray) : base(19650218U)
        {
            // Preconditions
            if (seedArray == null) throw new ArgumentNullException(nameof(seedArray));

            _seedArray = new uint[seedArray.Count];
            for (var index = 0; index < seedArray.Count; index++)
            {
                _seedArray[index] = (uint) Math.Abs(seedArray[index]);
            }

            // Necessary, because as seed array has been specified.
            Reset();
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="MT19937Generator"/> class, using the
        ///   specified seed array.
        /// </summary>
        /// <param name="seedArray">
        ///   An array of unsigned numbers used to calculate a starting values for the pseudo-random
        ///   number sequence.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="seedArray"/> is NULL ( <see langword="Nothing"/> in Visual Basic).
        /// </exception>
        public MT19937Generator(uint[] seedArray) : base(19650218U)
        {
            // Preconditions
            if (seedArray == null) throw new ArgumentNullException(nameof(seedArray));

            _seedArray = seedArray;

            // Necessary, because as seed array has been specified.
            Reset();
        }

        #endregion Construction

        #region Instance methods

        /// <summary>
        ///   Extends resetting of the <see cref="MT19937Generator"/> using the <see cref="_seedArray"/>.
        /// </summary>
        private void ResetBySeedArray()
        {
            uint i = 1;
            uint j = 0;
            var k = (N > _seedArray.Length) ? N : _seedArray.Length;
            for (; k > 0; k--)
            {
                _mt[i] = (_mt[i] ^ ((_mt[i - 1] ^ (_mt[i - 1] >> 30)) * 1664525U)) + _seedArray[j] + j; // non linear
                i++;
                j++;
                if (i >= N)
                {
                    _mt[0] = _mt[N - 1];
                    i = 1;
                }
                if (j >= _seedArray.Length)
                {
                    j = 0;
                }
            }
            for (k = N - 1; k > 0; k--)
            {
                _mt[i] = (_mt[i] ^ ((_mt[i - 1] ^ (_mt[i - 1] >> 30)) * 1566083941U)) - i; // non linear
                i++;
                if (i < N)
                {
                    continue;
                }
                _mt[0] = _mt[N - 1];
                i = 1;
            }

            _mt[0] = 0x80000000U; // MSB is 1; assuring non-0 initial array
        }

        /// <summary>
        ///   Generates <see cref="N"/> unsigned random numbers.
        /// </summary>
        /// <remarks>
        ///   Generated random numbers are 32-bit unsigned integers greater than or equal to
        ///   <see cref="uint.MinValue"/> and less than or equal to <see cref="uint.MaxValue"/>.
        /// </remarks>
        private void GenerateNUInts()
        {
            int kk;
            uint y;
            var mag01 = new[] { 0x0U, VectorA };

            for (kk = 0; kk < N - M; kk++)
            {
                y = (_mt[kk] & UpperMask) | (_mt[kk + 1] & LowerMask);
                _mt[kk] = _mt[kk + M] ^ (y >> 1) ^ mag01[y & 0x1U];
            }
            for (; kk < N - 1; kk++)
            {
                y = (_mt[kk] & UpperMask) | (_mt[kk + 1] & LowerMask);
                _mt[kk] = _mt[kk + (M - N)] ^ (y >> 1) ^ mag01[y & 0x1U];
            }
            y = (_mt[N - 1] & UpperMask) | (_mt[0] & LowerMask);
            _mt[N - 1] = _mt[M - 1] ^ (y >> 1) ^ mag01[y & 0x1U];

            _mti = 0;
        }

        #endregion Instance methods

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

            _mt[0] = seed & 0xffffffffU;
            for (_mti = 1; _mti < N; _mti++)
            {
                _mt[_mti] = (1812433253U * (_mt[_mti - 1] ^ (_mt[_mti - 1] >> 30)) + _mti);
                // See Knuth TAOCP Vol2. 3rd Ed. P.106 for multiplier. In the previous versions, MSBs
                // of the seed affect only MSBs of the array mt[]. 2002/01/09 modified by Makoto Matsumoto
            }

            // If the object was instanciated with a seed array do some further (re)initialisation.
            if (_seedArray != null)
            {
                ResetBySeedArray();
            }
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
            // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
            if (_mti >= N)
            {
                // Generate N words at one time
                GenerateNUInts();
            }
            var y = _mt[_mti++];
            // Tempering
            y ^= (y >> 11);
            y ^= (y << 7) & 0x9d2c5680U;
            y ^= (y << 15) & 0xefc60000U;

            var result = (int) ((y ^ (y >> 18)) >> 1);

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
            // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
            if (_mti >= N)
            {
                // Generate N words at one time
                GenerateNUInts();
            }
            var y = _mt[_mti++];
            // Tempering
            y ^= (y >> 11);
            y ^= (y << 7) & 0x9d2c5680U;
            y ^= (y << 15) & 0xefc60000U;

            var result = (int) ((y ^ (y >> 18)) >> 1) * IntToDoubleMultiplier;

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
            if (_mti >= N)
            {
                // Generate N words at one time
                GenerateNUInts();
            }

            var y = _mt[_mti++];
            // Tempering
            y ^= (y >> 11);
            y ^= (y << 7) & 0x9d2c5680U;
            y ^= (y << 15) & 0xefc60000U;
            return (y ^ (y >> 18));
        }

        #endregion IGenerator members
    }
}