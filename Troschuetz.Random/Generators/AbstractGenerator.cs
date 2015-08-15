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

using PommaLabs.Thrower;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Troschuetz.Random.Core;

namespace Troschuetz.Random.Generators
{
    /// <summary>
    ///   </summary>
    /// <typeparam name="TGenerator">
    ///   The type of the object containing the real generation methods.
    /// </typeparam>
    [Serializable]
    public abstract class AbstractGenerator<TGenerator> : IGenerator
        where TGenerator : AbstractGenerator<TGenerator>
    {
        #region Constants

        /// <summary>
        ///   Represents the multiplier that computes a double-precision floating point number
        ///   greater than or equal to 0.0 and less than 1.0 when it gets applied to a nonnegative
        ///   32-bit signed integer.
        /// </summary>
        protected const double IntToDoubleMultiplier = 1.0 / (int.MaxValue + 1.0);

        /// <summary>
        ///   Represents the multiplier that computes a double-precision floating point number
        ///   greater than or equal to 0.0 and less than 1.0 when it gets applied to a nonnegative
        ///   32-bit unsigned integer.
        /// </summary>
        protected const double UIntToDoubleMultiplier = 1.0 / (uint.MaxValue + 1.0);

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
        ///   Initializes a new instance of the <typeparamref name="TGenerator"/> class, using the
        ///   specified seed value.
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

        public uint Seed { get; set; }

        public abstract bool CanReset { get; }

        public bool Reset() => Reset(Seed);

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

#if PORTABLE

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public int Next()
        {
            var result = (int) (NextUInt() >> 1);
            if (result == int.MaxValue)
            {
                result = Next();
            }

            // Postconditions
            Debug.Assert(result >= 0 && result < int.MaxValue);
            return result;
        }

#if PORTABLE

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public int Next(int maxValue)
        {
            // Preconditions
            RaiseArgumentOutOfRangeException.IfIsLessOrEqual(maxValue, 0, nameof(maxValue), ErrorMessages.NegativeMaxValue);

            // Here a ~2x speed improvement is gained by computing a value that can be cast to an
            // int before casting to a double to perform the multiplication. Casting a double from
            // an int is a lot faster than from an uint and the extra shift operation and cast to an
            // int are very fast (the allocated bits remain the same), so overall there's a
            // significant performance improvement. NOTE TO SELF: DO NOT REMOVE THE SECOND (INT)
            // CAST, EVEN IF VISUAL STUDIO TELLS IT IS NOT NECESSARY.
            var result = (int) ((int) (NextUInt() >> 1) * IntToDoubleMultiplier * maxValue);

            // Postconditions
            Debug.Assert(result >= 0 && result < maxValue);
            return result;
        }

#if PORTABLE

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public int Next(int minValue, int maxValue)
        {
            // Preconditions
            RaiseArgumentOutOfRangeException.IfIsGreaterOrEqual(minValue, maxValue, nameof(minValue), ErrorMessages.MinValueGreaterThanOrEqualToMaxValue);

            int result;
            var range = maxValue - minValue;
            if (range < 0)
            {
                // The range is greater than int.MaxValue, so we have to use slower floating point
                // arithmetic. Also all 32 random bits (uint) have to be used which again is slower
                // (See comment in NextDouble()).
                result = minValue + (int) (NextUInt() * UIntToDoubleMultiplier * (maxValue - (double) minValue));
            }
            else
            {
                // 31 random bits (int) will suffice which allows us to shift and cast to an int
                // before the first multiplication and gain better performance. See comment in
                // Next(maxValue). NOTE TO SELF: DO NOT REMOVE THE SECOND (INT) CAST, EVEN IF VISUAL
                // STUDIO TELLS IT IS NOT NECESSARY.
                result = minValue + (int) ((int) (NextUInt() >> 1) * IntToDoubleMultiplier * range);
            }

            // Postconditions
            Debug.Assert(result >= minValue && result < maxValue);
            return result;
        }

#if PORTABLE

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public int NextInclusiveMaxValue()
        {
            var result = (int) (NextUInt() >> 1);

            // Postconditions
            Debug.Assert(result >= 0);
            return result;
        }

#if PORTABLE

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public double NextDouble()
        {
            // Here a ~2x speed improvement is gained by computing a value that can be cast to an
            // int before casting to a double to perform the multiplication. Casting a double from
            // an int is a lot faster than from an uint and the extra shift operation and cast to an
            // int are very fast (the allocated bits remain the same), so overall there's a
            // significant performance improvement. NOTE TO SELF: DO NOT REMOVE THE SECOND (INT)
            // CAST, EVEN IF VISUAL STUDIO TELLS IT IS NOT NECESSARY.
            var result = (int) (NextUInt() >> 1) * IntToDoubleMultiplier;

            // Postconditions
            Debug.Assert(result >= 0.0 && result < 1.0);
            return result;
        }

#if PORTABLE

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public double NextDouble(double maxValue)
        {
            // Preconditions
            RaiseArgumentOutOfRangeException.IfIsLessOrEqual(maxValue, 0.0, nameof(maxValue), ErrorMessages.NegativeMaxValue);
            Raise<ArgumentException>.If(double.IsPositiveInfinity(maxValue));

            // NOTE TO SELF: DO NOT REMOVE THE SECOND (INT) CAST, EVEN IF VISUAL STUDIO TELLS IT IS
            // NOT NECESSARY.
            var result = (int) (NextUInt() >> 1) * IntToDoubleMultiplier * maxValue;

            // Postconditions
            Debug.Assert(result >= 0.0 && result < maxValue);
            return result;
        }

#if PORTABLE

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public double NextDouble(double minValue, double maxValue)
        {
            // Preconditions
            RaiseArgumentOutOfRangeException.IfIsGreaterOrEqual(minValue, maxValue, nameof(minValue), ErrorMessages.MinValueGreaterThanOrEqualToMaxValue);
            Raise<ArgumentException>.If(double.IsPositiveInfinity(maxValue - minValue));

            // NOTE TO SELF: DO NOT REMOVE THE SECOND (INT) CAST, EVEN IF VISUAL STUDIO TELLS IT IS
            // NOT NECESSARY.
            var result = minValue + (int) (NextUInt() >> 1) * IntToDoubleMultiplier * (maxValue - minValue);

            // Postconditions
            Debug.Assert(result >= minValue && result < maxValue);
            return result;
        }

#if PORTABLE

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public uint NextUInt(uint maxValue)
        {
            // Preconditions
            RaiseArgumentOutOfRangeException.IfIsLess(maxValue, 1U, nameof(maxValue), ErrorMessages.MaxValueIsTooSmall);

            var result = (uint) (NextUInt() * UIntToDoubleMultiplier * maxValue);

            // Postconditions
            Debug.Assert(result < maxValue);
            return result;
        }

        public abstract uint NextUInt();

#if PORTABLE

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public uint NextUInt(uint minValue, uint maxValue)
        {
            // Preconditions
            RaiseArgumentOutOfRangeException.IfIsGreaterOrEqual(minValue, maxValue, nameof(minValue), ErrorMessages.MinValueGreaterThanOrEqualToMaxValue);

            var result = minValue + (uint) (NextUInt() * UIntToDoubleMultiplier * (maxValue - minValue));

            // Postconditions
            Debug.Assert(result >= minValue && result < maxValue);
            return result;
        }

#if PORTABLE

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public uint NextUIntExclusiveMaxValue()
        {
            uint result;
            while ((result = NextUInt()) == uint.MaxValue) { }

            // Postconditions
            Debug.Assert(result < uint.MaxValue);
            return result;
        }

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
