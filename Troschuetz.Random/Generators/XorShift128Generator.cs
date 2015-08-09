/*
 * Copyright © 2006 Stefan Troschütz (stefan@troschuetz.de)
 * Copyright © 2012-2014 Alessio Parma (alessio.parma@gmail.com)
 * 
 * This file is part of Troschuetz.Random Class Library.
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

#region Original Summary

/*
/// <summary>
/// A fast random number generator for .NET
/// Colin Green, January 2005
/// 
/// September 4th 2005
///	 Added NextBytesUnsafe() - commented out by default.
///	 Fixed bug in Reinitialise() - y,z and w variables were not being reset.
/// 
/// Key points:
///  1) Based on a simple and fast xor-shift pseudo random number generator (RNG) specified in: 
///  Marsaglia, George. (2003). Xorshift RNGs.
///  http://www.jstatsoft.org/v08/i14/xorshift.pdf
///  
///  This particular implementation of xorshift has a period of 2^128-1. See the above paper to see
///  how this can be easily extened if you need a longer period. At the time of writing I could find no 
///  information on the period of System.Random for comparison.
/// 
///  2) Faster than System.Random. Up to 15x faster, depending on which methods are called.
/// 
///  3) Direct replacement for System.Random. This class implements all of the methods that System.Random 
///  does plus some additional methods. The like named methods are functionally equivalent.
///  
///  4) Allows fast re-initialisation with a seed, unlike System.Random which accepts a seed at construction
///  time which then executes a relatively expensive initialisation routine. This provides a vast speed improvement
///  if you need to reset the pseudo-random number sequence many times, e.g. if you want to re-generate the same
///  sequence many times. An alternative might be to cache random numbers in an array, but that approach is limited
///  by memory capacity and the fact that you may also want a large number of different sequences cached. Each sequence
///  can each be represented by a single seed value (int) when using FastRandom.
///  
///  Notes.
///  A further performance improvement can be obtained by declaring local variables as static, thus avoiding 
///  re-allocation of variables on each call. However care should be taken if multiple instances of
///  FastRandom are in use or if being used in a multi-threaded environment.
/// </summary>
 */

#endregion

namespace Troschuetz.Random.Generators
{
    using PommaLabs.Thrower;
    using System;
    using Core;

    /// <summary>
    ///   Represents a xorshift pseudo-random number generator with period 2^128-1.
    /// </summary>
    /// <remarks>
    ///   The <see cref="XorShift128Generator"/> type bases upon the implementation presented in the CP article
    ///   "<a href="http://www.codeproject.com/csharp/fastrandom.asp">A fast equivalent for System.Random</a>"
    ///   and the theoretical background on xorshift random number generators published by George Marsaglia 
    ///   in this paper "<a href="http://www.jstatsoft.org/v08/i14/xorshift.pdf">Xorshift RNGs</a>".
    /// </remarks>
    [Serializable]
    public sealed class XorShift128Generator : GeneratorBase<XorShift128Generator>, IGenerator
    {
        #region Class Fields

        /// <summary>
        ///   Represents the seed for the <see cref="_y"/> variable. This field is constant.
        /// </summary>
        /// <remarks>The value of this constant is 362436069.</remarks>
        const uint SeedY = 362436069;

        /// <summary>
        ///   Represents the seed for the <see cref="_z"/> variable. This field is constant.
        /// </summary>
        /// <remarks>The value of this constant is 521288629.</remarks>
        const uint SeedZ = 521288629;

        /// <summary>
        ///   Represents the seed for the <see cref="_w"/> variable. This field is constant.
        /// </summary>
        /// <remarks>The value of this constant is 88675123.</remarks>
        const uint SeedW = 88675123;

        #endregion

        #region Instance Fields

        /// <summary>
        ///   Stores an <see cref="uint"/> used to generate up to 32 random <see cref="bool"/> values.
        /// </summary>
        uint _bitBuffer;

        /// <summary>
        ///   Stores how many random <see cref="bool"/> values still can be generated from <see cref="_bitBuffer"/>.
        /// </summary>
        int _bitCount;

        /// <summary>
        ///   Stores the used seed.
        /// </summary>
        readonly uint _seed;

        /// <summary>
        ///   Stores the last generated unsigned random number. 
        /// </summary>
        uint _w;

        /// <summary>
        ///   Stores the last but three unsigned random number. 
        /// </summary>
        uint _x;

        /// <summary>
        ///   Stores the last but two unsigned random number. 
        /// </summary>
        uint _y;

        /// <summary>
        ///   Stores the last but one unsigned random number. 
        /// </summary>
        uint _z;

        #endregion

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="XorShift128Generator"/> class, 
        ///   using a time-dependent default seed value.
        /// </summary>
        public XorShift128Generator() : this((uint) Math.Abs(Environment.TickCount))
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="XorShift128Generator"/> class, 
        ///   using the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   A number used to calculate a starting value for the pseudo-random number sequence.
        ///   If a negative number is specified, the absolute value of the number is used. 
        /// </param>
        public XorShift128Generator(int seed) : this((uint) Math.Abs(seed))
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="XorShift128Generator"/> class, 
        ///   using the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        [CLSCompliant(false)]
        public XorShift128Generator(uint seed)
        {
            _seed = seed;
            ResetGenerator();
        }

        #endregion

        #region Instance Methods

        /// <summary>
        ///   Resets the <see cref="XorShift128Generator"/>, so that it produces
        ///   the same pseudo-random number sequence again.
        /// </summary>
        void ResetGenerator()
        {
            // "The seed set for xor128 is four 32-bit integers x,y,z,w not all 0, ..." (George Marsaglia)
            // To meet that requirement the y, z, w seeds are constant values greater 0.
            _x = Seed;
            _y = SeedY;
            _z = SeedZ;
            _w = SeedW;

            // Reset helper variables used for generation of random bools.
            _bitBuffer = 0;
            _bitCount = 0;
        }

        #endregion

        #region IGenerator Members

        [CLSCompliant(false)]
        public uint Seed
        {
            get { return _seed; }
        }

        public bool CanReset
        {
            get { return true; }
        }

        public bool Reset()
        {
            ResetGenerator();
            return true;
        }

        public int Next()
        {
            // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
            var t = (_x ^ (_x << 11));
            _x = _y;
            _y = _z;
            _z = _w;
            var w = (_w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8)));

            var result = (int) (w >> 1);
            // Exclude Int32.MaxValue from the range of return values.
            return result == Int32.MaxValue ? Next() : result;
        }

        public int NextInclusiveMaxValue()
        {
            // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
            var t = (_x ^ (_x << 11));
            _x = _y;
            _y = _z;
            _z = _w;
            var w = (_w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8)));

            return (int)(w >> 1);
        }

        public int Next(int maxValue)
        {
            // Preconditions
            RaiseArgumentOutOfRangeException.IfIsLessOrEqual(maxValue, 0, nameof(maxValue), ErrorMessages.NegativeMaxValue);

            // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
            var t = (_x ^ (_x << 11));
            _x = _y;
            _y = _z;
            _z = _w;
            var w = (_w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8)));

            // The shift operation and extra int cast before the first multiplication give better performance.
            // See comment in NextDouble().
            return (int) ((int) (w >> 1)*IntToDoubleMultiplier*maxValue);
        }

        public int Next(int minValue, int maxValue)
        {
            // Preconditions
            RaiseArgumentOutOfRangeException.IfIsGreaterOrEqual(minValue, maxValue, nameof(minValue), ErrorMessages.MinValueGreaterThanOrEqualToMaxValue);

            // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
            var t = (_x ^ (_x << 11));
            _x = _y;
            _y = _z;
            _z = _w;
            var w = (_w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8)));

            var range = maxValue - minValue;
            if (range < 0) {
                // The range is greater than Int32.MaxValue, so we have to use slower floating point arithmetic.
                // Also all 32 random bits (uint) have to be used which again is slower (See comment in NextDouble()).
                return minValue + (int) (w*UIntToDoubleMultiplier*(maxValue - (double) minValue));
            }

            // 31 random bits (int) will suffice which allows us to shift and cast to an int
            // before the first multiplication and gain better performance.
            // See comment in NextDouble().
            return minValue + (int) ((int) (w >> 1)*IntToDoubleMultiplier*range);
        }

        public double NextDouble()
        {
            // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
            var t = (_x ^ (_x << 11));
            _x = _y;
            _y = _z;
            _z = _w;
            var w = (_w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8)));

            // Here a ~2x speed improvement is gained by computing a value that can be cast to an int 
            // before casting to a double to perform the multiplication.
            // Casting a double from an int is a lot faster than from an uint and the extra shift operation 
            // and cast to an int are very fast (the allocated bits remain the same), so overall there's 
            // a significant performance improvement.
            return (int) (w >> 1)*IntToDoubleMultiplier;
        }

        public double NextDouble(double maxValue)
        {
            // Preconditions
            RaiseArgumentOutOfRangeException.IfIsLessOrEqual(maxValue, 0.0, nameof(maxValue), ErrorMessages.NegativeMaxValue);
            Raise<ArgumentException>.If(double.IsPositiveInfinity(maxValue));

            // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
            var t = (_x ^ (_x << 11));
            _x = _y;
            _y = _z;
            _z = _w;
            var w = (_w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8)));

            // The shift operation and extra int cast before the first multiplication give better performance.
            // See comment in NextDouble().
            return (int) (w >> 1)*IntToDoubleMultiplier*maxValue;
        }

        public double NextDouble(double minValue, double maxValue)
        {
            // Preconditions
            RaiseArgumentOutOfRangeException.IfIsGreaterOrEqual(minValue, maxValue, nameof(minValue), ErrorMessages.MinValueGreaterThanOrEqualToMaxValue);
            Raise<ArgumentException>.If(double.IsPositiveInfinity(maxValue - minValue));

            // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
            var t = (_x ^ (_x << 11));
            _x = _y;
            _y = _z;
            _z = _w;
            var w = (_w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8)));

            // The shift operation and extra int cast before the first multiplication give better performance.
            // See comment in NextDouble().
            return minValue + (int) (w >> 1)*IntToDoubleMultiplier*(maxValue - minValue);
        }

        [CLSCompliant(false)]
        public uint NextUInt()
        {
            var t = (_x ^ (_x << 11));
            _x = _y;
            _y = _z;
            _z = _w;
            return (_w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8)));
        }

        [CLSCompliant(false)]
        public uint NextUInt(uint maxValue)
        {
            // Preconditions
            RaiseArgumentOutOfRangeException.IfIsLess(maxValue, 1U, nameof(maxValue), ErrorMessages.MaxValueIsTooSmall);

            // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
            var t = (_x ^ (_x << 11));
            _x = _y;
            _y = _z;
            _z = _w;
            var w = (_w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8)));

            // The shift operation and extra int cast before the first multiplication give better performance.
            // See comment in NextDouble().
            return (uint) ((int) (w >> 1)*IntToDoubleMultiplier*maxValue);
        }

        [CLSCompliant(false)]
        public uint NextUInt(uint minValue, uint maxValue)
        {
            // Preconditions
            RaiseArgumentOutOfRangeException.IfIsGreaterOrEqual(minValue, maxValue, nameof(minValue), ErrorMessages.MinValueGreaterThanOrEqualToMaxValue);

            // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
            var t = (_x ^ (_x << 11));
            _x = _y;
            _y = _z;
            _z = _w;
            var w = (_w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8)));

            // The shift operation and extra int cast before the first multiplication give better performance.
            // See comment in NextDouble().
            return minValue + (uint) ((int) (w >> 1)*IntToDoubleMultiplier*(maxValue - minValue));
        }

        public bool NextBoolean()
        {
            if (_bitCount == 0) {
                // Generate 32 more bits (1 uint) and store it for future calls.
                // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
                var t = (_x ^ (_x << 11));
                _x = _y;
                _y = _z;
                _z = _w;
                _bitBuffer = (_w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8)));

                // Reset the bitCount and use rightmost bit of buffer to generate random bool.
                _bitCount = 31;
                return (_bitBuffer & 0x1) == 1;
            }

            // Decrease the bitCount and use rightmost bit of shifted buffer to generate random bool.
            _bitCount--;
            return ((_bitBuffer >>= 1) & 0x1) == 1;
        }

        public void NextBytes(byte[] buffer)
        {
            // Preconditions
            RaiseArgumentNullException.IfIsNull(buffer, nameof(buffer), ErrorMessages.NullBuffer);

            // Use local copies of x,y,z and w for better performance.
            var x = _x;
            var y = _y;
            var z = _z;
            var w = _w;

            // Fill the buffer with 4 bytes (1 uint) at a time.
            var i = 0;
            uint t;
            while (i < buffer.Length - 3) {
                // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
                t = (x ^ (x << 11));
                x = y;
                y = z;
                z = w;
                w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));

                buffer[i++] = (byte) w;
                buffer[i++] = (byte) (w >> 8);
                buffer[i++] = (byte) (w >> 16);
                buffer[i++] = (byte) (w >> 24);
            }

            // Fill up any remaining bytes in the buffer.
            if (i < buffer.Length) {
                // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
                t = (x ^ (x << 11));
                x = y;
                y = z;
                z = w;
                w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));

                buffer[i++] = (byte) w;
                if (i < buffer.Length) {
                    buffer[i++] = (byte) (w >> 8);
                    if (i < buffer.Length) {
                        buffer[i++] = (byte) (w >> 16);
                        if (i < buffer.Length) {
                            buffer[i] = (byte) (w >> 24);
                        }
                    }
                }
            }

            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }

        #endregion
    }
}