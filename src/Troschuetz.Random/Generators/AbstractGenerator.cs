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
    using Core;
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    ///   An abstract generator which efficiently implements everything required by the <see
    ///   cref="IGenerator"/> interface using only few methods: <see cref="NextUInt()"/>, <see
    ///   cref="NextDouble()"/>, <see cref="NextInclusiveMaxValue()"/>.
    ///
    ///   Therefore, in order to build a new generator, one must "simply" override the <see
    ///   cref="Reset(uint)"/>, which is used to automatically initialize the generator, and the
    ///   generator methods, which, as stated above, are used to generate every kind of random object
    ///   exposed by the interface.
    ///
    ///   All generators implemented in this library extend this abstract class.
    /// </summary>
    /// <remarks>
    ///   Methods <see cref="NextBoolean()"/> and <see cref="NextBytes(byte[])"/> are NOT thread
    ///   safe. The thread safety of other methods depends on the one of the extending class, that
    ///   is, if all abstract methods are implemented in a thread safe manner, then other methods,
    ///   excluding <see cref="NextBoolean()"/> and <see cref="NextBytes(byte[])"/>, are thread safe too.
    ///
    ///   Please note that all generators implemented in this library are NOT thread safe.
    /// </remarks>
#if HAS_SERIALIZABLE
    [Serializable]
#endif

    public abstract class AbstractGenerator : IGenerator
    {
        #region Constants

        /// <summary>
        ///   The number of left shifts required to transform a 64-bit unsigned integer into a
        ///   nonnegative 32-bit signed integer.
        /// </summary>
        protected const byte ULongToIntShift = 33;

        /// <summary>
        ///   The number of left shifts required to transform a 64-bit unsigned integer into a 32-bit
        ///   unsigned integer.
        /// </summary>
        protected const byte ULongToUIntShift = 32;

        /// <summary>
        ///   Represents the multiplier that computes a double-precision floating point number
        ///   greater than or equal to 0.0 and less than 1.0 when it gets applied to a nonnegative
        ///   32-bit signed integer.
        /// </summary>
        protected const double IntToDoubleMultiplier = 1.0 / (int.MaxValue + 1.0);

        /// <summary>
        ///   Represents the multiplier that computes a double-precision floating point number
        ///   greater than or equal to 0.0 and less than 1.0 when it gets applied to a 32-bit
        ///   unsigned integer.
        /// </summary>
        protected const double UIntToDoubleMultiplier = 1.0 / (uint.MaxValue + 1.0);

        /// <summary>
        ///   Represents the multiplier that computes a double-precision floating point number
        ///   greater than or equal to 0.0 and less than 1.0 when it gets applied to a 64-bit
        ///   unsigned integer.
        /// </summary>
        protected const double ULongToDoubleMultiplier = 1.0 / (ulong.MaxValue + 1.0);

        #endregion Constants

        #region Fields

        /// <summary>
        ///   Stores an <see cref="uint"/> used to generate up to 32 random <see cref="bool"/> values.
        /// </summary>
        private uint _bitBuffer;

        /// <summary>
        ///   Stores how many random <see cref="bool"/> values still can be generated from <see cref="_bitBuffer"/>.
        /// </summary>
        private int _bitCount;

        #endregion Fields

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
        ///   Resets the random number generator using the initial seed, so that it produces the same
        ///   random number sequence again. To understand whether this generator can be reset, you
        ///   can query the <see cref="CanReset"/> property.
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
        /// <remarks>If this method is overridden, always remember to call it inside the override.</remarks>
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
        ///   A 32-bit signed integer greater than or equal to 0, and less than <see
        ///   cref="int.MaxValue"/>; that is, the range of return values includes 0 but not <see cref="int.MaxValue"/>.
        /// </returns>
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
        ///   A 32-bit signed integer greater than or equal to 0, and less than or equal to <see
        ///   cref="int.MaxValue"/>; that is, the range of return values includes 0 and <see cref="int.MaxValue"/>.
        /// </returns>
        public abstract int NextInclusiveMaxValue();

        /// <summary>
        ///   Returns a nonnegative random number less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated.</param>
        /// <returns>
        ///   A 32-bit signed integer greater than or equal to 0, and less than <paramref
        ///   name="maxValue"/>; that is, the range of return values includes 0 but not <paramref name="maxValue"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="maxValue"/> must be greater than 0.
        /// </exception>
        public int Next(int maxValue)
        {
            // Preconditions
            if (maxValue < 0) throw new ArgumentOutOfRangeException(nameof(maxValue), ErrorMessages.NegativeMaxValue);

            var result = (int)(NextDouble() * maxValue);

            // Postconditions
            Debug.Assert(result >= 0 && result < maxValue);
            return result;
        }

        /// <summary>
        ///   Returns a random number within the specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number to be generated.</param>
        /// <param name="maxValue">
        ///   The exclusive upper bound of the random number to be generated. <paramref
        ///   name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.
        /// </param>
        /// <returns>
        ///   A 32-bit signed integer greater than or equal to <paramref name="minValue"/>, and less
        ///   than <paramref name="maxValue"/>; that is, the range of return values includes
        ///   <paramref name="minValue"/> but not <paramref name="maxValue"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.
        /// </exception>
        public int Next(int minValue, int maxValue)
        {
            // Preconditions
            if (minValue > maxValue) throw new ArgumentOutOfRangeException(nameof(minValue), ErrorMessages.MinValueGreaterThanMaxValue);

            // The cast to double enables 64 bit arithmetic, which is needed when the condition
            // ((maxValue - minValue) > int.MaxValue) is true.
            var result = minValue + (int)(NextDouble() * (maxValue - (double)minValue));

            // Postconditions
            Debug.Assert((result == minValue && minValue == maxValue) || (result >= minValue && result < maxValue));
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
        ///   <paramref name="maxValue"/>; that is, the range of return values includes 0 but not
        ///   <paramref name="maxValue"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="maxValue"/> must be greater than 0.0.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///   <paramref name="maxValue"/> cannot be <see cref="double.PositiveInfinity"/>.
        /// </exception>
        public double NextDouble(double maxValue)
        {
            // Preconditions
            if (maxValue < 0.0) throw new ArgumentOutOfRangeException(nameof(maxValue), ErrorMessages.NegativeMaxValue);
            if (double.IsPositiveInfinity(maxValue)) throw new ArgumentException(ErrorMessages.InfiniteMaxValue, nameof(maxValue));

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
        ///   A double-precision floating point number greater than or equal to <paramref
        ///   name="minValue"/>, and less than <paramref name="maxValue"/>; that is, the range of
        ///   return values includes <paramref name="minValue"/> but not <paramref name="maxValue"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///   The difference between <paramref name="maxValue"/> and <paramref name="minValue"/>
        ///   cannot be <see cref="double.PositiveInfinity"/>.
        /// </exception>
        public double NextDouble(double minValue, double maxValue)
        {
            // Preconditions
            if (minValue > maxValue) throw new ArgumentOutOfRangeException(nameof(minValue), ErrorMessages.MinValueGreaterThanMaxValue);
            if (double.IsPositiveInfinity(maxValue - minValue)) throw new ArgumentException(ErrorMessages.InfiniteMaxValueMinusMinValue, nameof(minValue));

            var result = minValue + NextDouble() * (maxValue - minValue);

            // Postconditions
            Debug.Assert((TMath.AreEqual(result, minValue) && TMath.AreEqual(minValue, maxValue)) || (result >= minValue && result < maxValue));
            return result;
        }

        /// <summary>
        ///   Returns an unsigned random number.
        /// </summary>
        /// <returns>
        ///   A 32-bit unsigned integer greater than or equal to 0, and less than <see
        ///   cref="uint.MaxValue"/>; that is, the range of return values includes 0 but not <see cref="uint.MaxValue"/>.
        /// </returns>
        public uint NextUInt()
        {
            uint result;
            do result = NextUIntInclusiveMaxValue(); while (result == uint.MaxValue);

            // Postconditions
            Debug.Assert(result < uint.MaxValue);
            return result;
        }

        /// <summary>
        ///   Returns an unsigned random number less than <see cref="uint.MaxValue"/>.
        /// </summary>
        /// <returns>
        ///   A 32-bit unsigned integer greater than or equal to 0, and less than <see
        ///   cref="uint.MaxValue"/>; that is, the range of return values includes 0 but not <see cref="uint.MaxValue"/>.
        /// </returns>
        public uint NextUIntExclusiveMaxValue() => NextUInt();

        /// <summary>
        ///   Returns an unsigned random number.
        /// </summary>
        /// <returns>
        ///   A 32-bit unsigned integer greater than or equal to 0, and less than or equal to <see
        ///   cref="uint.MaxValue"/>; that is, the range of return values includes 0 and <see cref="uint.MaxValue"/>.
        /// </returns>
        public abstract uint NextUIntInclusiveMaxValue();

        /// <summary>
        ///   Returns an unsigned random number less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated.</param>
        /// <returns>
        ///   A 32-bit unsigned integer greater than or equal to 0, and less than <paramref
        ///   name="maxValue"/>; that is, the range of return values includes 0 but not <paramref name="maxValue"/>.
        /// </returns>
        public uint NextUInt(uint maxValue)
        {
            var result = (uint)(NextDouble() * maxValue);

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
        ///   A 32-bit unsigned integer greater than or equal to <paramref name="minValue"/> and less
        ///   than <paramref name="maxValue"/>; that is, the range of return values includes
        ///   <paramref name="minValue"/> but not <paramref name="maxValue"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.
        /// </exception>
        public uint NextUInt(uint minValue, uint maxValue)
        {
            // Preconditions
            if (minValue > maxValue) throw new ArgumentOutOfRangeException(nameof(minValue), ErrorMessages.MinValueGreaterThanMaxValue);

            var result = minValue + (uint)(NextDouble() * (maxValue - minValue));

            // Postconditions
            Debug.Assert((result == minValue && minValue == maxValue) || (result >= minValue && result < maxValue));
            return result;
        }

        /// <summary>
        ///   Returns a random Boolean value.
        /// </summary>
        /// <remarks>
        ///   Buffers 31 random bits for future calls, so the random number generator is only invoked
        ///   once in every 31 calls.
        /// </remarks>
        /// <returns>A <see cref="bool"/> value.</returns>
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
        public void NextBytes(byte[] buffer)
        {
            // Preconditions
            if (buffer == null) throw new ArgumentNullException(nameof(buffer), ErrorMessages.NullBuffer);

#if HAS_SPAN
            NextBytes(new Span<byte>(buffer));
        }

        /// <summary>
        ///   Fills the elements of a specified span of bytes with random numbers.
        /// </summary>
        /// <remarks>
        ///   Each element of the array of bytes is set to a random number greater than or equal to
        ///   0, and less than or equal to <see cref="byte.MaxValue"/>.
        /// </remarks>
        /// <param name="buffer">A span of bytes to contain random numbers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="buffer"/> is null.</exception>
        public void NextBytes(Span<byte> buffer)
        {
#endif
            // Fill the buffer with 4 bytes (1 uint) at a time.
            var i = 0;
            while (i < buffer.Length - 3)
            {
                var u = NextUInt();
                buffer[i++] = (byte)u;
                buffer[i++] = (byte)(u >> 8);
                buffer[i++] = (byte)(u >> 16);
                buffer[i++] = (byte)(u >> 24);
            }

            // Fill up any remaining bytes in the buffer.
            if (i < buffer.Length)
            {
                var u = NextUInt();
                buffer[i++] = (byte)u;
                if (i < buffer.Length)
                {
                    buffer[i++] = (byte)(u >> 8);
                    if (i < buffer.Length)
                    {
                        buffer[i++] = (byte)(u >> 16);
                        if (i < buffer.Length)
                        {
                            buffer[i] = (byte)(u >> 24);
                        }
                    }
                }
            }
        }

        #endregion IGenerator members

        #region Equality members

        /// <summary>
        ///   Two <see cref="AbstractGenerator"/> instances are equal if they have the same state.
        /// </summary>
        /// <param name="obj">The object.</param>
        public override bool Equals(object obj)
        {
            var o = obj as AbstractGenerator;
            return o != null && Seed == o.Seed && _bitCount == o._bitCount && _bitBuffer == o._bitBuffer;
        }

        /// <summary>
        ///   Hash code is computed from the state of the generator.
        /// </summary>
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 1777771;
#pragma warning disable RECS0025 // Non-readonly field referenced in 'GetHashCode()'
                hash = hash * 31 + (int)Seed;
                hash = hash * 31 + _bitCount;
                hash = hash * 31 + (int)_bitBuffer;
#pragma warning restore RECS0025 // Non-readonly field referenced in 'GetHashCode()'
                return hash;
            }
        }

        /// <summary>
        ///   A string with the name of the generator and the seed.
        /// </summary>
        public override string ToString()
        {
            const string generatorSuffix = "Generator";
            var generatorName = GetType().Name;
            if (generatorName.EndsWith(generatorSuffix, StringComparison.Ordinal))
            {
                generatorName = generatorName.Substring(0, generatorName.Length - generatorSuffix.Length);
            }
            return $"Generator: {generatorName}, Seed: {Seed}";
        }

        #endregion Equality members
    }
}