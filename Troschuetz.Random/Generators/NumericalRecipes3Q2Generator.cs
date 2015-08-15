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
using System.Runtime.CompilerServices;

namespace Troschuetz.Random.Generators
{
    [Serializable]
    public sealed class NumericalRecipes3Q2Generator : AbstractGenerator<NumericalRecipes3Q2Generator>, IGenerator
    {
        #region Constants

        /// <summary>
        ///   Represents the seed for the <see cref="GeneratorState.V"/> variable. This field is constant.
        /// </summary>
        /// <remarks>The value of this constant is 4101842887655102017.</remarks>
        public const ulong SeedV = 4101842887655102017UL;

        /// <summary>
        ///   Represents the seed for the <see cref="GeneratorState.W"/> variable. This field is constant.
        /// </summary>
        /// <remarks>The value of this constant is 1.</remarks>
        public const ulong SeedW = 1UL;

        /// <summary>
        ///   Represents the seed for the <see cref="ulong"/> numbers generation. This field is constant.
        /// </summary>
        /// <remarks>The value of this constant is 4294957665.</remarks>
        public const ulong SeedU = 4294957665UL;

        /// <summary>
        ///   Represents the seed for the <see cref="double"/> numbers generation. This field is constant.
        /// </summary>
        /// <remarks>The value of this constant is 5.42101086242752217e-20.</remarks>
        public const double SeedD = 5.42101086242752217e-20;

        #endregion

        ulong _v;
        ulong _w;

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="NumericalRecipes3Q2Generator"/> class, 
        ///   using a time-dependent default seed value.
        /// </summary>
        public NumericalRecipes3Q2Generator() : base((uint) Math.Abs(Environment.TickCount))
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="NumericalRecipes3Q2Generator"/> class, 
        ///   using the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   A number used to calculate a starting value for the pseudo-random number sequence.
        ///   If a negative number is specified, the absolute value of the number is used. 
        /// </param>
        public NumericalRecipes3Q2Generator(int seed) : base((uint) Math.Abs(seed))
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="NumericalRecipes3Q2Generator"/> class, 
        ///   using the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        public NumericalRecipes3Q2Generator(uint seed) : base(seed)
        {
        }

        #endregion

        #region IGenerator members

        public bool CanReset => true;

        public bool Reset(uint seed)
        {
            _v = SeedV;
            _w = SeedW;
            _v ^= seed;
            _w = NextULong();
            _v = NextULong();
            return true;
        }

#if PORTABLE

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public double NextDouble()
        {
            _v ^= _v >> 17;
            _v ^= _v << 31;
            _v ^= _v >> 8;
            _w = SeedU * (_w & 0xFFFFFFFFUL) + (_w >> 32);
            return SeedD * (_v ^ _w);
        }

#if PORTABLE

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public uint NextUInt()
        {
            _v ^= _v >> 17;
            _v ^= _v << 31;
            _v ^= _v >> 8;
            _w = SeedU * (_w & 0xFFFFFFFFUL) + (_w >> 32);
            return (uint) (_v ^ _w);
        }

#if PORTABLE

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public ulong NextULong()
        {
            _v ^= _v >> 17;
            _v ^= _v << 31;
            _v ^= _v >> 8;
            _w = SeedU * (_w & 0xFFFFFFFFUL) + (_w >> 32);
            return _v ^ _w;
        }

        #endregion
    }
}
