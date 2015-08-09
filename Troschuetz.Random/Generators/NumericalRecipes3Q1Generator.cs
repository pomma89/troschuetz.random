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
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. *
 * See the GNU Lesser General Public License for more details.
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
 */

using System;

namespace Troschuetz.Random.Generators
{
    [Serializable]
    sealed class NumericalRecipes3Q1Generator
    {
        ulong _v;

        #region Next

        public int Next()
        {
            var result = (int) (StaticNextULong(ref _v) >> 1);
            // Exclude int.MaxValue from the range of return values.
            return result == int.MaxValue ? Next() : result;
        }

        #endregion Next

        [CLSCompliant(false)]
        public uint NextUInt() => (uint) StaticNextULong(ref _v);

        [CLSCompliant(false)]
        public ulong NextULong() => StaticNextULong(ref _v);

        static ulong StaticNextULong(ref ulong v)
        {
            v ^= v >> 21;
            v ^= v << 35;
            v ^= v >> 4;
            return v * 2685821657736338717UL;
        }
    }
}
