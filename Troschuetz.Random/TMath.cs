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

namespace Troschuetz.Random
{
    /// <summary>
    ///   Simple math utilities used inside the library. They are also public, should you need them
    ///   for custom generators and distributions.
    /// </summary>
    public static class TMath
    {
        /// <summary>
        ///   The delta used when comparing doubles.
        /// </summary>
        public const double Tolerance = 1E-6;

        /// <summary>
        ///   Safely checks if given double is zero.
        /// </summary>
        /// <param name="d">A double.</param>
        /// <returns>True if given double is near zero, false otherwise.</returns>
        public static bool IsZero(double d) => d > -Tolerance && d < Tolerance;

        /// <summary>
        ///   Safely checks if given doubles are equal.
        /// </summary>
        /// <param name="d1">A double.</param>
        /// <param name="d2">A double.</param>
        /// <returns>True if given doubles are safely equal, false otherwise.</returns>
        public static bool AreEqual(double d1, double d2) => IsZero(d1 - d2);

        /// <summary>
        ///   Fast square power.
        /// </summary>
        /// <param name="d">A double.</param>
        /// <returns>The square of given double.</returns>
        public static double Square(double d) => IsZero(d) ? 0.0 : d * d;

        /// <summary>
        ///   Generates a new seed, using all information available, including time.
        /// </summary>
        public static uint Seed()
        {
            unchecked
            {
                const uint factor = 19U;
                var seed = 1777771U;
                seed = factor * seed + (uint) Environment.TickCount;

#if !PORTABLE
                seed = factor * seed + (uint) System.Threading.Thread.CurrentThread.ManagedThreadId;
#endif

                return seed;
            }
        }
    }
}
