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

namespace Troschuetz.Random.Generators
{
    using PommaLabs.Thrower;
    using System;
    using Core;
    using System.Runtime.InteropServices;
    using System.Runtime.CompilerServices;
    using System.Diagnostics;

    /// <summary>
    ///   Represents a simple pseudo-random number generator.
    /// </summary>
    /// <remarks>
    ///   The <see cref="StandardGenerator"/> type internally uses an instance of the
    ///   <see cref="System.Random"/> type to generate pseudo-random numbers.
    /// </remarks>
    [Serializable]
    public sealed class StandardGenerator : AbstractGenerator
    {
        #region Fields

        /// <summary>
        ///   Stores a byte array used to compute the result of <see cref="NextUInt()"/>, starting from the output of <see cref="NextBytes"/>.
        /// </summary>
        byte[] _uintBuffer;

        /// <summary>
        ///   Stores an instance of <see cref="System.Random"/> type that is used to generate random numbers.
        /// </summary>
        Random _generator;

        #endregion

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="StandardGenerator"/> class,
        ///   using a time-dependent default seed value.
        /// </summary>
        public StandardGenerator() : this(Environment.TickCount)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="StandardGenerator"/> class,
        ///   using the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   A number used to calculate a starting value for the pseudo-random number sequence.
        ///   If a negative number is specified, the absolute value of the number is used. 
        /// </param>
        public StandardGenerator(int seed) : base((uint) Math.Abs(seed))
        {
        }

        #endregion

        #region IGenerator members

        /// <summary>
        ///   Gets a value indicating whether the random number generator can be reset, so that it
        ///   produces the same random number sequence again.
        /// </summary>
        public override bool CanReset => true;

        /// <summary>
        ///   Resets the random number generator using the specified seed, so that it produces the same random number sequence again.
        ///   To understand whether this generator can be reset, you can query the <see cref="CanReset"/> property.
        /// </summary>
        /// <param name="seed">The seed value used by the generator.</param>
        /// <returns>
        ///   True if the random number generator was reset; otherwise, false.
        /// </returns>
        public override bool Reset(uint seed)
        {
            base.Reset(seed);

            // Create a new Random object using the specified seed.
            _generator = new Random((int) seed); // Safe cast, seed is always positive.

            // Initialize the buffer used to store the bytes required to create an unsigned integer.
            _uintBuffer = new byte[4];

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
        public override int Next()
        {
            var result = _generator.Next();

            // Postconditions
            Debug.Assert(result >= 0 && result < int.MaxValue);
            return result;
        }

        /// <summary>
        ///   Returns a nonnegative floating point random number less than 1.0.
        /// </summary>
        /// <returns>
        ///   A double-precision floating point number greater than or equal to 0.0, and less than
        ///   1.0; that is, the range of return values includes 0.0 but not 1.0.
        /// </returns>
#if PORTABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public override double NextDouble()
        {
            var result = _generator.NextDouble();

            // Postconditions
            Debug.Assert(result >= 0.0 && result < 1.0);
            return result;
        }

        /// <summary>
        ///   Returns an unsigned random number.
        /// </summary>
        /// <returns>
        ///   A 32-bit unsigned integer greater than or equal to <see cref="uint.MinValue"/> and
        ///   less than or equal to <see cref="uint.MaxValue"/>.
        /// </returns>
#if PORTABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public override uint NextUInt()
        {
            _generator.NextBytes(_uintBuffer);
            return BitConverter.ToUInt32(_uintBuffer, 0);
        }

        #endregion
    }
}