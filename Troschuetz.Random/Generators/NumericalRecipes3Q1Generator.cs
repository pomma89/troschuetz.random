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
    sealed class NumericalRecipes3Q1Generator : AbstractGenerator<NumericalRecipes3Q1Generator.State>
    {
        #region Constants

        /// <summary>
        ///   Represents the seed for the <see cref="State.V"/> variable. This field is constant.
        /// </summary>
        /// <remarks>The value of this constant is 4101842887655102017.</remarks>
        public const ulong SeedV = 4101842887655102017UL;

        /// <summary>
        ///   Represents the seed for the <see cref="ulong"/> numbers generation. This field is constant.
        /// </summary>
        /// <remarks>The value of this constant is 2685821657736338717.</remarks>
        public const ulong SeedU = 2685821657736338717UL;

        /// <summary>
        ///   Represents the seed for the <see cref="double"/> numbers generation. This field is constant.
        /// </summary>
        /// <remarks>The value of this constant is 5.42101086242752217e-20.</remarks>
        public const double SeedD = 5.42101086242752217e-20;

        #endregion

        public NumericalRecipes3Q1Generator(uint seed) : base(seed)
        {
        }

        #region AbstractGenerator members

        protected override int NextInclusiveMaxValue(State state)
        {
            unchecked
            {
                return (int) (((uint) state.NextULong()) >> 1);
            }
        }

        protected override double NextDouble(State state)
        {
            unchecked
            {
                return SeedD * state.NextULong();
            }
        }

        protected override uint NextUInt(State state)
        {
            unchecked
            {
                return (uint) state.NextULong();
            }
        }

        #endregion

        public sealed class State : IState
        {
            public ulong V;

            public void Reset(uint seed)
            {
                V = SeedV;
                V ^= seed;
                V = NextULong();
            }

#if PORTABLE

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public ulong NextULong()
            {
                V ^= V >> 21;
                V ^= V << 35;
                V ^= V >> 4;
                return V * SeedU;
            }
        }
    }
}
