/*
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

namespace Troschuetz.Random
{
    /// <summary>
    ///   Represents the internal state of a random number generator.
    /// </summary>
    public interface IGeneratorState
    {
        /// <summary>
        ///   Gets a value indicating whether the random number generator state can be reset,
        ///   so that the generator produces the same random number sequence again.
        /// </summary>
        bool CanReset { get; }

        /// <summary>
        ///   Resets the random number generator state, so that the generator produces the same random number sequence again.
        ///   To understand whether this generator state can be reset, you can query the <see cref="CanReset"/> property.
        /// </summary>
        /// <param name="seed">The seed used to set the initial generator state.</param>
        /// <returns>
        ///   True if the random number generator was reset; otherwise, false.
        /// </returns>
        bool Reset(uint seed);
    }
}
