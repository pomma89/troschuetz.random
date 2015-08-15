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

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Troschuetz.Random.Generators
{
    /// <summary>
    ///   A generator whose original code has been found on "Numerical Recipes in C++", 3rd edition.
    ///   Inside the book, it is named "Ranq1" and it is the recommended generator for everyday use.
    /// </summary>
    /// <remarks>This generator has a period of ~ 1.8 * 10^19.</remarks>
    [Serializable]
    public sealed class NumericalRecipes3Q1Generator : AbstractGenerator
    {
        #region Constants

        /// <summary>
        ///   Represents the seed for the <see cref="_v"/> variable. This field is constant.
        /// </summary>
        /// <remarks>The value of this constant is 4101842887655102017.</remarks>
        public const ulong SeedV = 4101842887655102017UL;

        /// <summary>
        ///   Represents the seed for the <see cref="ulong"/> numbers generation. This field is constant.
        /// </summary>
        /// <remarks>The value of this constant is 2685821657736338717.</remarks>
        public const ulong SeedU = 2685821657736338717UL;

        #endregion Constants

        ulong _v;

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="NumericalRecipes3Q1Generator"/> class,
        ///   using a time-dependent default seed value.
        /// </summary>
        public NumericalRecipes3Q1Generator() : base((uint) Math.Abs(Environment.TickCount))
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="NumericalRecipes3Q1Generator"/> class,
        ///   using the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   A number used to calculate a starting value for the pseudo-random number sequence. If
        ///   a negative number is specified, the absolute value of the number is used.
        /// </param>
        public NumericalRecipes3Q1Generator(int seed) : base((uint) Math.Abs(seed))
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="NumericalRecipes3Q1Generator"/> class,
        ///   using the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        public NumericalRecipes3Q1Generator(uint seed) : base(seed)
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
            _v ^= seed;
            _v = NextULong();
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
            _v ^= _v >> 21;
            _v ^= _v << 35;
            _v ^= _v >> 4;
            var result = (int) ((_v * SeedU) >> 33);

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
            _v ^= _v >> 21;
            _v ^= _v << 35;
            _v ^= _v >> 4;
            var result = (_v * SeedU) * ULongToDoubleMultiplier;

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
            _v ^= _v >> 21;
            _v ^= _v << 35;
            _v ^= _v >> 4;
            return (uint) (_v * SeedU);
        }

        /// <summary>
        ///   Returns an unsigned long random number.
        /// </summary>
        /// <returns>
        ///   A 64-bit unsigned integer greater than or equal to <see cref="ulong.MinValue"/> and
        ///   less than or equal to <see cref="ulong.MaxValue"/>.
        /// </returns>
#if PORTABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public ulong NextULong()
        {
            _v ^= _v >> 21;
            _v ^= _v << 35;
            _v ^= _v >> 4;
            return _v * SeedU;
        }

        #endregion IGenerator members
    }
}
