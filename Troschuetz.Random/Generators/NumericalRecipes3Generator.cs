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
    ///   Inside the book, it is named "Ran" and it is the highest quality recommended generator.
    /// </summary>
    /// <remarks>This generator has a period of ~ 3.138 * 10^57.</remarks>
    [Serializable]
    public sealed class NumericalRecipes3Generator : AbstractGenerator
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

        ulong _u;
        ulong _v;
        ulong _w;

        #endregion Fields

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="NumericalRecipes3Generator"/> class,
        ///   using a time-dependent default seed value.
        /// </summary>
        public NumericalRecipes3Generator() : base((uint) Math.Abs(Environment.TickCount))
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="NumericalRecipes3Generator"/> class,
        ///   using the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   A number used to calculate a starting value for the pseudo-random number sequence. If
        ///   a negative number is specified, the absolute value of the number is used.
        /// </param>
        public NumericalRecipes3Generator(int seed) : base((uint) Math.Abs(seed))
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="NumericalRecipes3Generator"/> class,
        ///   using the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        public NumericalRecipes3Generator(uint seed) : base(seed)
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
            _u = _u * SeedU1 + SeedU2;
            _v ^= _v >> 17;
            _v ^= _v << 31;
            _v ^= _v >> 8;
            _w = SeedU3 * (_w & 0xFFFFFFFFUL) + (_w >> 32);
            var x = _u ^ (_u << 21);
            x ^= x >> 35;
            x ^= x << 4;
            return (int) (((x + _v) ^ _w) >> 33);
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
            _u = _u * SeedU1 + SeedU2;
            _v ^= _v >> 17;
            _v ^= _v << 31;
            _v ^= _v >> 8;
            _w = SeedU3 * (_w & 0xFFFFFFFFUL) + (_w >> 32);
            var x = _u ^ (_u << 21);
            x ^= x >> 35;
            x ^= x << 4;
            var result = ((x + _v) ^ _w) * ULongToDoubleMultiplier;

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
            _u = _u * SeedU1 + SeedU2;
            _v ^= _v >> 17;
            _v ^= _v << 31;
            _v ^= _v >> 8;
            _w = SeedU3 * (_w & 0xFFFFFFFFUL) + (_w >> 32);
            var x = _u ^ (_u << 21);
            x ^= x >> 35;
            x ^= x << 4;
            return (uint) ((x + _v) ^ _w);
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
            _u = _u * SeedU1 + SeedU2;
            _v ^= _v >> 17;
            _v ^= _v << 31;
            _v ^= _v >> 8;
            _w = SeedU3 * (_w & 0xFFFFFFFFUL) + (_w >> 32);
            var x = _u ^ (_u << 21);
            x ^= x >> 35;
            x ^= x << 4;
            return (x + _v) ^ _w;
        }

        #endregion IGenerator members
    }
}
