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
    using System;

    [Serializable]
    public abstract class GeneratorBase<TGen> where TGen : GeneratorBase<TGen>, IGenerator
    {
        #region Class Fields

        /// <summary>
        ///   Represents the multiplier that computes a double-precision 
        ///   floating point number greater than or equal to 0.0 and less than 1.0
        ///   when it gets applied to a nonnegative 32-bit signed integer.
        /// </summary>
        protected const double IntToDoubleMultiplier = 1.0 / (int.MaxValue + 1.0);

        /// <summary>
        ///   Represents the multiplier that computes a double-precision 
        ///   floating point number greater than or equal to 0.0 and less than 1.0
        ///   when it gets applied to a nonnegative 32-bit unsigned integer.
        /// </summary>
        protected const double UIntToDoubleMultiplier = 1.0 / (uint.MaxValue + 1.0);

        #endregion

        #region IGenerator Members

        [CLSCompliant(false)]
        public uint NextUIntExclusiveMaxValue()
        {
            uint x;
            while ((x = Generator.NextUInt()) == uint.MaxValue) {}
            return x;
        }

        #endregion

        #region Private Members

        TGen Generator => this as TGen;

        #endregion
    }
}
