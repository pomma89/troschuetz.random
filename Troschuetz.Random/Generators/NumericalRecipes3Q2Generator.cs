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
    sealed class NumericalRecipes3Q2Generator : AbstractGenerator<NumericalRecipes3Q2Generator.GeneratorState>
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
        /// <remarks>The value of this constant is 2685821657736338717.</remarks>
        public const ulong SeedU = 4294957665UL;

        /// <summary>
        ///   Represents the seed for the <see cref="double"/> numbers generation. This field is constant.
        /// </summary>
        /// <remarks>The value of this constant is 5.42101086242752217e-20.</remarks>
        public const double SeedD = 5.42101086242752217e-20;

        #endregion

        public NumericalRecipes3Q2Generator(uint seed) : base(seed)
        {
        }

        #region AbstractGenerator members

        protected override int NextInclusiveMaxValue(GeneratorState generatorState)
        {
            unchecked
            {
                return (int) (((uint) generatorState.NextULong()) >> 1);
            }
        }

        protected override double NextDouble(GeneratorState generatorState)
        {
            unchecked
            {
                return SeedD * generatorState.NextULong();
            }
        }

        protected override uint NextUInt(GeneratorState generatorState)
        {
            unchecked
            {
                return (uint) generatorState.NextULong();
            }
        }

        #endregion

        public sealed class GeneratorState : IGeneratorState
        {
            public ulong V;
            public ulong W;

            public bool CanReset => true;

            public bool Reset(uint seed)
            {
                V = SeedV;
                W = SeedW;
                V ^= seed;
                W = NextULong();
                V = NextULong();
                return true;
            }

#if PORTABLE

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public ulong NextULong()
            {
                V ^= V >> 17;
                V ^= V << 31;
                V ^= V >> 8;
                W = SeedU * (W & 0xFFFFFFFFUL) + (W >> 32);
                return V ^ W;
            }
        }
    }
}
