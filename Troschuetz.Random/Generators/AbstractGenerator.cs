/*
 * Copyright © 2006 Stefan Troschütz (stefan@troschuetz.de)
 * Copyright © 2012-2016 Alessio Parma (alessio.parma@gmail.com)
 *
 * This file is part of Troschuetz.Random Class Library.
 *
 * Troschuetz.Random is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *
 * See the GNU Lesser General Public License for more details.
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
 */

using PommaLabs.Thrower;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Troschuetz.Random.Core;

namespace Troschuetz.Random.Generators
{
    /// <summary>
    ///   An abstract generator which efficiently implements everything required by the
    ///   <see cref="IGenerator"/> interface using only few methods: <see cref="NextUInt()"/>,
    ///   <see cref="NextDouble()"/>, <see cref="NextInclusiveMaxValue()"/>.
    /// 
    ///   Therefore, in order to build a new generator, one must "simply" override the
    ///   <see cref="Reset(uint)"/>, which is used to automatically initialize the generator, and
    ///   the generator methods, which, as stated above, are used to generate every kind of random
    ///   object exposed by the interface.
    /// 
    ///   All generators implemented in this library extend this abstract class.
    /// </summary>
    [Serializable]
    public abstract class AbstractGenerator : IGenerator
    {
        #region Constants

        /// <summary>
        ///   Represents the multiplier that computes a double-precision floating point number
        ///   greater than or equal to 0.0 and less than 1.0 when it gets applied to a nonnegative
        ///   32-bit unsigned integer.
        /// </summary>
        /// <remarks>The value has been generated from 1.0 / (uint.MaxValue + 1.0).</remarks>
        protected const double UIntToDoubleMultiplier = 2.32830643653869628E-10;

        /// <summary>
        ///   Represents the multiplier that computes a double-precision floating point number
        ///   greater than or equal to 0.0 and less than 1.0 when it gets applied to a nonnegative
        ///   32-bit unsigned integer.
        /// </summary>
        /// <remarks>The value has been generated from 1.0 / (ulong.MaxValue + 1.0).</remarks>
        protected const double ULongToDoubleMultiplier = 5.42101086242752217E-20;

        #endregion Constants

        /// <summary>
        ///   Stores an <see cref="uint"/> used to generate up to 32 random <see cref="bool"/> values.
        /// </summary>
        uint _bitBuffer;

        /// <summary>
        ///   Stores how many random <see cref="bool"/> values still can be generated from <see cref="_bitBuffer"/>.
        /// </summary>
        int _bitCount;

        /// <summary>
        ///   Initializes a new instance of the generator, using the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        [SuppressMessage("Potential Code Quality Issues", "RECS0021:Warns about calls to virtual member functions occuring in the constructor", Justification = "In this case, it is safe")]
        [SuppressMessage("Usage", "CC0067:Virtual Method Called On Constructor", Justification = "In this case, it is safe")]
        protected AbstractGenerator(uint seed)
        {
            // Set the initial state for this generator.
            Reset(seed);

            // The seed is stored in order to allow resetting the generator.
            Seed = seed;
        }

        #region IGenerator members

        /// <summary>
        ///   The seed value used by the generator.
        /// </summary>
        public uint Seed { get; set; }

        /// <summary>
        ///   Gets a value indicating whether the random number generator can be reset, so that it
        ///   produces the same random number sequence again.
        /// </summary>
        public abstract bool CanReset { get; }

        /// <summary>
        ///   Resets the random number generator using the initial seed, so that it produces the
        ///   same random number sequence again. To understand whether this generator can be reset,
        ///   you can query the <see cref="CanReset"/> property.
        /// </summary>
        /// <returns>True if the random number generator was reset; otherwise, false.</returns>
        public bool Reset() => Reset(Seed);

        /// <summary>
        ///   Resets the random number generator using the specified seed, so that it produces the
        ///   same random number sequence again. To understand whether this generator can be reset,
        ///   you can query the <see cref="CanReset"/> property.
        /// </summary>
        /// <param name="seed">The seed value used by the generator.</param>
        /// <returns>True if the random number generator was reset; otherwise, false.</returns>
        /// <remarks>
        ///   If this method is overridden, always remember to call it inside the override.
        /// </remarks>
        public virtual bool Reset(uint seed)
        {
            if (!CanReset)
            {
                return false;
            }

            // Reset helper variables used for generation of random bools.
            _bitBuffer = 0U;
            _bitCount = 0;

            // Store the new seed.
            Seed = seed;

            return true;
        }

        /// <summary>
        ///   Returns a nonnegative random number less than <see cref="int.MaxValue"/>.
        /// </summary>
        /// <returns>
        ///   A 32-bit signed integer greater than or equal to 0, and less than
        ///   <see cref="int.MaxValue"/>; that is, the range of return values includes 0 but not <see cref="int.MaxValue"/>.
        /// </returns>
#if PORTABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public int Next()
        {
            int result;
            do result = NextInclusiveMaxValue(); while (result == int.MaxValue);

            // Postconditions
            Debug.Assert(result >= 0 && result < int.MaxValue);
            return result;
        }

        /// <summary>
        ///   Returns a nonnegative random number less than or equal to <see cref="int.MaxValue"/>.
        /// </summary>
        /// <returns>
        ///   A 32-bit signed integer greater than or equal to 0, and less than or equal to
        ///   <see cref="int.MaxValue"/>; that is, the range of return values includes 0 and <see cref="int.MaxValue"/>.
        /// </returns>
        public abstract int NextInclusiveMaxValue();

        /// <summary>
        ///   Returns a nonnegative random number less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated.</param>
        /// <returns>
        ///   A 32-bit signed integer greater than or equal to 0, and less than
        ///   <paramref name="maxValue"/>; that is, the range of return values includes 0 but not <paramref name="maxValue"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="maxValue"/> must be greater than or equal to 0.
        /// </exception>
#if PORTABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public int Next(int maxValue)
        {
            // Preconditions
            RaiseArgumentOutOfRangeException.IfIsLessOrEqual(maxValue, 0, nameof(maxValue), ErrorMessages.NegativeMaxValue);

            var result = (int) (NextDouble() * maxValue);

            // Postconditions
            Debug.Assert(result >= 0 && result < maxValue);
            return result;
        }

        /// <summary>
        ///   Returns a random number within the specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number to be generated.</param>
        /// <param name="maxValue">
        ///   The exclusive upper bound of the random number to be generated.
        ///   <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.
        /// </param>
        /// <returns>
        ///   A 32-bit signed integer greater than or equal to <paramref name="minValue"/>, and less
        ///   than <paramref name="maxValue"/>; that is, the range of return values includes
        ///   <paramref name="minValue"/> but not <paramref name="maxValue"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.
        /// </exception>
#if PORTABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public int Next(int minValue, int maxValue)
        {
            // Preconditions
            RaiseArgumentOutOfRangeException.IfIsGreaterOrEqual(minValue, maxValue, nameof(minValue), ErrorMessages.MinValueGreaterThanOrEqualToMaxValue);

            // The cast to double enables 64 bit arithmetic, which is needed when the condition
            // ((maxValue - minValue) > int.MaxValue) is true.
            var result = minValue + (int) (NextDouble() * (maxValue - (double) minValue));

            // Postconditions
            Debug.Assert(result >= minValue && result < maxValue);
            return result;
        }

        /// <summary>
        ///   Returns a nonnegative floating point random number less than 1.0.
        /// </summary>
        /// <returns>
        ///   A double-precision floating point number greater than or equal to 0.0, and less than
        ///   1.0; that is, the range of return values includes 0.0 but not 1.0.
        /// </returns>
        public abstract double NextDouble();

        /// <summary>
        ///   Returns a nonnegative floating point random number less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated.</param>
        /// <returns>
        ///   A double-precision floating point number greater than or equal to 0.0, and less than
        ///   <paramref name="maxValue"/>; that is, the range of return values includes 0 but not <paramref name="maxValue"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="maxValue"/> must be greater than or equal to 0.0.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="maxValue"/> cannot be <see cref="double.PositiveInfinity"/>.</exception>
#if PORTABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public double NextDouble(double maxValue)
        {
            // Preconditions
            RaiseArgumentOutOfRangeException.IfIsLessOrEqual(maxValue, 0.0, nameof(maxValue), ErrorMessages.NegativeMaxValue);
            Raise<ArgumentException>.If(double.IsPositiveInfinity(maxValue));

            var result = NextDouble() * maxValue;

            // Postconditions
            Debug.Assert(result >= 0.0 && result < maxValue);
            return result;
        }

        /// <summary>
        ///   Returns a floating point random number within the specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number to be generated.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated.</param>
        /// <returns>
        ///   A double-precision floating point number greater than or equal to
        ///   <paramref name="minValue"/>, and less than <paramref name="maxValue"/>; that is, the
        ///   range of return values includes <paramref name="minValue"/> but not <paramref name="maxValue"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///   The difference between <paramref name="maxValue"/> and <paramref name="minValue"/>
        ///   cannot be <see cref="double.PositiveInfinity"/>.
        /// </exception>
#if PORTABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public double NextDouble(double minValue, double maxValue)
        {
            // Preconditions
            RaiseArgumentOutOfRangeException.IfIsGreaterOrEqual(minValue, maxValue, nameof(minValue), ErrorMessages.MinValueGreaterThanOrEqualToMaxValue);
            Raise<ArgumentException>.If(double.IsPositiveInfinity(maxValue - minValue));

            var result = minValue + NextDouble() * (maxValue - minValue);

            // Postconditions
            Debug.Assert(result >= minValue && result < maxValue);
            return result;
        }

        /// <summary>
        ///   Returns an unsigned random number.
        /// </summary>
        /// <returns>
        ///   A 32-bit unsigned integer greater than or equal to <see cref="uint.MinValue"/> and
        ///   less than or equal to <see cref="uint.MaxValue"/>.
        /// </returns>
        public abstract uint NextUInt();

        /// <summary>
        ///   Returns an unsigned random number less than <see cref="uint.MaxValue"/>.
        /// </summary>
        /// <returns>
        ///   A 32-bit unsigned integer greater than or equal to <see cref="uint.MinValue"/> and
        ///   less than <see cref="uint.MaxValue"/>.
        /// </returns>
#if PORTABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public uint NextUIntExclusiveMaxValue()
        {
            uint result;
            do result = NextUInt(); while (result == uint.MaxValue);

            // Postconditions
            Debug.Assert(result < uint.MaxValue);
            return result;
        }

        /// <summary>
        ///   Returns an unsigned random number less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated.</param>
        /// <returns>
        ///   A 32-bit unsigned integer greater than or equal to <see cref="uint.MinValue"/> and
        ///   less than <paramref name="maxValue"/>; that is, the range of return values includes
        ///   <see cref="uint.MinValue"/> but not <paramref name="maxValue"/>.
        /// </returns>
#if PORTABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public uint NextUInt(uint maxValue)
        {
            // Preconditions
            RaiseArgumentOutOfRangeException.IfIsLess(maxValue, 1U, nameof(maxValue), ErrorMessages.MaxValueIsTooSmall);

            var result = (uint) (NextDouble() * maxValue);

            // Postconditions
            Debug.Assert(result < maxValue);
            return result;
        }

        /// <summary>
        ///   Returns an unsigned random number within the specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number to be generated.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated.</param>
        /// <returns>
        ///   A 32-bit unsigned integer greater than or equal to <paramref name="minValue"/> and
        ///   less than <paramref name="maxValue"/>; that is, the range of return values includes
        ///   <paramref name="minValue"/> but not <paramref name="maxValue"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.
        /// </exception>
#if PORTABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public uint NextUInt(uint minValue, uint maxValue)
        {
            // Preconditions
            RaiseArgumentOutOfRangeException.IfIsGreaterOrEqual(minValue, maxValue, nameof(minValue), ErrorMessages.MinValueGreaterThanOrEqualToMaxValue);

            var result = minValue + (uint) (NextDouble() * (maxValue - minValue));

            // Postconditions
            Debug.Assert(result >= minValue && result < maxValue);
            return result;
        }

        /// <summary>
        ///   Returns a random Boolean value.
        /// </summary>
        /// <remarks>
        ///   Buffers 31 random bits for future calls, so the random number generator is only
        ///   invoked once in every 31 calls.
        /// </remarks>
        /// <returns>A <see cref="bool"/> value.</returns>
#if PORTABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public bool NextBoolean()
        {
            if (_bitCount == 0)
            {
                // Generate 32 more bits (1 uint) and store it for future calls.
                _bitBuffer = NextUInt();

                // Reset the bitCount and use rightmost bit of buffer to generate random bool.
                _bitCount = 31;
                return (_bitBuffer & 0x1) == 1;
            }

            // Decrease the bitCount and use rightmost bit of shifted buffer to generate random bool.
            _bitCount--;
            return ((_bitBuffer >>= 1) & 0x1) == 1;
        }

        /// <summary>
        ///   Fills the elements of a specified array of bytes with random numbers.
        /// </summary>
        /// <remarks>
        ///   Each element of the array of bytes is set to a random number greater than or equal to
        ///   0, and less than or equal to <see cref="byte.MaxValue"/>.
        /// </remarks>
        /// <param name="buffer">An array of bytes to contain random numbers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="buffer"/> is null.</exception>
#if PORTABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public void NextBytes(byte[] buffer)
        {
            // Preconditions
            RaiseArgumentNullException.IfIsNull(buffer, nameof(buffer), ErrorMessages.NullBuffer);

            // Fill the buffer with 4 bytes (1 uint) at a time.
            var i = 0;
            while (i < buffer.Length - 3)
            {
                var u = NextUInt();
                buffer[i++] = (byte) u;
                buffer[i++] = (byte) (u >> 8);
                buffer[i++] = (byte) (u >> 16);
                buffer[i++] = (byte) (u >> 24);
            }

            // Fill up any remaining bytes in the buffer.
            if (i < buffer.Length)
            {
                var u = NextUInt();
                buffer[i++] = (byte) u;
                if (i < buffer.Length)
                {
                    buffer[i++] = (byte) (u >> 8);
                    if (i < buffer.Length)
                    {
                        buffer[i++] = (byte) (u >> 16);
                        if (i < buffer.Length)
                        {
                            buffer[i] = (byte) (u >> 24);
                        }
                    }
                }
            }
        }

        #endregion IGenerator members
    }
}
