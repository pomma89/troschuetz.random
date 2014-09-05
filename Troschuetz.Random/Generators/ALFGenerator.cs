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

#region Original Copyright

/* boost random/lagged_fibonacci.hpp header file
 *
 * Copyright Jens Maurer 2000-2001
 * Distributed under the Boost Software License, Version 1.0. (See
 * accompanying file LICENSE_1_0.txt or copy at
 * http://www.boost.org/LICENSE_1_0.txt)
 *
 * See http://www.boost.org for most recent version including documentation.
 *
 * $Id: lagged_fibonacci.hpp,v 1.28 2005/05/21 15:57:00 dgregor Exp $
 *
 * Revision history
 *  2001-02-18  moved to individual header files
 */

#endregion

namespace Troschuetz.Random.Generators
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    ///   Represents a Additive Lagged Fibonacci pseudo-random number generator.
    /// </summary>
    /// <remarks>
    ///   The <see cref="ALFGenerator"/> type bases upon the implementation in the 
    ///   <a href="http://www.boost.org/libs/random/index.html">Boost Random Number Library</a>.
    ///   It uses the modulus 2<sup>32</sup> and by default the "lags" 418 and 1279, which can be adjusted through the 
    ///   associated <see cref="ShortLag"/> and <see cref="LongLag"/> properties. Some popular pairs are presented on 
    ///   <a href="http://en.wikipedia.org/wiki/Lagged_Fibonacci_generator">Wikipedia - Lagged Fibonacci generator</a>.
    /// </remarks>
// ReSharper disable InconsistentNaming
    public sealed class ALFGenerator : IGenerator
// ReSharper restore InconsistentNaming
    {
        #region Class Fields

        /// <summary>
        ///   Represents the multiplier that computes a double-precision 
        ///   floating point number greater than or equal to 0.0 and less than 1.0
        ///   when it gets applied to a nonnegative 32-bit signed integer.
        /// </summary>
        const double IntToDoubleMultiplier = 1.0/(int.MaxValue + 1.0);

        /// <summary>
        ///   Represents the multiplier that computes a double-precision 
        ///   floating point number greater than or equal to 0.0 and less than 1.0
        ///   when it gets applied to a nonnegative 32-bit unsigned integer.
        /// </summary>
        const double UIntToDoubleMultiplier = 1.0/(uint.MaxValue + 1.0);

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
        ///   Stores an index for the random number array element that will be accessed next.
        /// </summary>
        int _i;

        /// <summary>
        ///   Stores the long lag of the Lagged Fibonacci pseudo-random number generator.
        /// </summary>
        int _longLag;

        /// <summary>
        ///   Stores the used seed.
        /// </summary>
        readonly uint _seed;

        /// <summary>
        ///   Stores the short lag of the Lagged Fibonacci pseudo-random number generator.
        /// </summary>
        int _shortLag;

        /// <summary>
        ///   Stores an array of <see cref="_longLag"/> random numbers
        /// </summary>
        uint[] _x;

        /// <summary>
        ///   Gets or sets the short lag of the Lagged Fibonacci pseudo-random number generator.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   Calls <see cref="IsValidShortLag"/> to determine whether a value is valid and therefore assignable.
        /// </remarks>
        public int ShortLag
        {
            get { return _shortLag; }
            set
            {
                Contract.Requires<ArgumentOutOfRangeException>(IsValidShortLag(value));
                Contract.Ensures(ShortLag == value);
                _shortLag = value;
            }
        }

        /// <summary>
        ///   Gets or sets the long lag of the Lagged Fibonacci pseudo-random number generator.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value"/> is less than or equal to <see cref="ShortLag"/>.
        /// </exception>
        /// <remarks>
        ///   Calls <see cref="IsValidLongLag"/> to determine whether a value is valid and therefore assignable.
        /// </remarks>
        public int LongLag
        {
            get { return _longLag; }
            set
            {
                Contract.Requires<ArgumentOutOfRangeException>(IsValidLongLag(value));
                Contract.Ensures(LongLag == value);
                _longLag = value;
                Reset();
            }
        }

        #endregion

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="ALFGenerator"/> class,
        ///   using a time-dependent default seed value.
        /// </summary>
        public ALFGenerator() : this((uint) Math.Abs(Environment.TickCount))
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ALFGenerator"/> class, using the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   A number used to calculate a starting value for the pseudo-random number sequence.
        ///   If a negative number is specified, the absolute value of the number is used. 
        /// </param>
        public ALFGenerator(int seed) : this((uint) Math.Abs(seed))
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="StandardGenerator"/> class, using the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        [CLSCompliant(false)]
        public ALFGenerator(uint seed)
        {
            _seed = seed;
            _shortLag = 418;
            _longLag = 1279;
            ResetGenerator();
        }

        #endregion

        #region Instance Methods

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="ShortLag"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>
        ///   <see langword="true"/> if value is greater than 0; otherwise, <see langword="false"/>.
        /// </returns>
        [System.Diagnostics.Contracts.Pure]
        public static bool IsValidShortLag(int value)
        {
            return value > 0;
        }

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="LongLag"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>
        ///   <see langword="true"/> if value is greater than <see cref="ShortLag"/>; otherwise, <see langword="false"/>.
        /// </returns>
        [System.Diagnostics.Contracts.Pure]
        public bool IsValidLongLag(int value)
        {
            return value > _shortLag;
        }

        /// <summary>
        ///   Resets the <see cref="ALFGenerator"/>, so that it produces the same pseudo-random number sequence again.
        /// </summary>
        void ResetGenerator()
        {
            var gen = new MT19937Generator(_seed);
            _x = new uint[_longLag];
            for (uint j = 0; j < _longLag; ++j) {
                _x[j] = gen.NextUInt();
            }
            _i = _longLag;

            // Reset helper variables used for generation of random bools.
            _bitBuffer = 0;
            _bitCount = 0;
        }

        /// <summary>
        ///   Fills the array <see cref="_x"/> with <see cref="_longLag"/> new unsigned random numbers.
        /// </summary>
        /// <remarks>
        ///   Generated random numbers are 32-bit unsigned integers greater than or equal to
        ///   <see cref="UInt32.MinValue"/> and less than or equal to <see cref="UInt32.MaxValue"/>.
        /// </remarks>
        void Fill()
        {
            // Two loops to avoid costly modulo operations
            for (var j = 0; j < _shortLag; ++j) {
                _x[j] = _x[j] + _x[j + (_longLag - _shortLag)];
            }
            for (var j = _shortLag; j < _longLag; ++j) {
                _x[j] = _x[j] + _x[j - _shortLag];
            }
            _i = 0;
        }

        /// <summary>
        ///   Returns a nonnegative random number less than or equal to <see cref="Int32.MaxValue"/>.
        /// </summary>
        /// <returns>
        ///   A 32-bit signed integer greater than or equal to 0, and less than or equal to <see cref="Int32.MaxValue"/>; 
        ///   that is, the range of return values includes 0 and <see cref="Int32.MaxValue"/>.
        /// </returns>
        public int NextInclusiveMaxValue()
        {
            // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
            if (_i >= _longLag) {
                Fill();
            }
            var x = _x[_i++];

            return (int) (x >> 1);
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
            if (_i >= _longLag) {
                Fill();
            }
            var x = _x[_i++];

            var result = (int) (x >> 1);
            // Exclude Int32.MaxValue from the range of return values.
            return result == Int32.MaxValue ? Next() : result;
        }

        public int Next(int maxValue)
        {
            // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
            if (_i >= _longLag) {
                Fill();
            }
            var x = _x[_i++];

            // The shift operation and extra int cast before the first multiplication give better performance.
            // See comment in NextDouble().
            return (int) ((int) (x >> 1)*IntToDoubleMultiplier*maxValue);
        }

        public int Next(int minValue, int maxValue)
        {
            // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
            if (_i >= _longLag) {
                Fill();
            }
            var x = _x[_i++];

            var range = maxValue - minValue;
            if (range < 0) {
                // The range is greater than Int32.MaxValue, so we have to use slower floating point arithmetic.
                // Also all 32 random bits (uint) have to be used which again is slower (See comment in NextDouble()).
                return minValue + (int) (x*UIntToDoubleMultiplier*(maxValue - (double) minValue));
            }
            // 31 random bits (int) will suffice which allows us to shift and cast to an int before 
            // the first multiplication and gain better performance.
            // See comment in NextDouble().
            return minValue + (int) ((int) (x >> 1)*IntToDoubleMultiplier*range);
        }

        public double NextDouble()
        {
            // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
            if (_i >= _longLag) {
                Fill();
            }
            var x = _x[_i++];

            // Here a ~2x speed improvement is gained by computing a value that can be cast to an int 
            // before casting to a double to perform the multiplication.
            // Casting a double from an int is a lot faster than from an uint and the extra shift operation 
            // and cast to an int are very fast (the allocated bits remain the same), so overall there's 
            // a significant performance improvement.
            return (int) (x >> 1)*IntToDoubleMultiplier;
        }

        public double NextDouble(double maxValue)
        {
            // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
            if (_i >= _longLag) {
                Fill();
            }
            var x = _x[_i++];

            // The shift operation and extra int cast before the first multiplication give better performance.
            // See comment in NextDouble().
            return (int) (x >> 1)*IntToDoubleMultiplier*maxValue;
        }

        public double NextDouble(double minValue, double maxValue)
        {
            // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
            if (_i >= _longLag) {
                Fill();
            }
            var x = _x[_i++];

            // The shift operation and extra int cast before the first multiplication give better performance.
            // See comment in NextDouble().
            return minValue + (int) (x >> 1)*IntToDoubleMultiplier*(maxValue - minValue);
        }

        [CLSCompliant(false)]
        public uint NextUInt()
        {
            if (_i >= _longLag)
            {
                Fill();
            }
            return _x[_i++];
        }

        public bool NextBoolean()
        {
            if (_bitCount == 0) {
                // Generate 32 more bits (1 uint) and store it for future calls.
                // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
                if (_i >= _longLag) {
                    Fill();
                }
                _bitBuffer = _x[_i++];

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
            // Fill the buffer with 4 bytes (1 uint) at a time.
            var i = 0;
            uint w;
            while (i < buffer.Length - 3) {
                // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
                if (_i >= _longLag) {
                    Fill();
                }
                w = _x[_i++];

                buffer[i++] = (byte) w;
                buffer[i++] = (byte) (w >> 8);
                buffer[i++] = (byte) (w >> 16);
                buffer[i++] = (byte) (w >> 24);
            }

            // Fill up any remaining bytes in the buffer.
            if (i >= buffer.Length) {
                return;
            }

            // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
            if (_i >= _longLag) {
                Fill();
            }
            w = _x[_i++];

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

        #endregion
    }
}