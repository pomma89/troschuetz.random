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

using System;
using System.Diagnostics.Contracts;

namespace Troschuetz.Random
{
    /// <summary>
    ///   Declares common functionalities for all random number generators.
    /// </summary>
    public interface IGenerator
    {
        /// <summary>
        ///   The seed value used by the generator.
        /// </summary>
        [Pure]
        uint Seed { get; }

        /// <summary>
        ///   Gets a value indicating whether the random number generator can be reset,
        ///   so that it produces the same random number sequence again.
        /// </summary>
        [Pure]
        bool CanReset { get; }

        /// <summary>
        ///   Resets the random number generator using the initial seed, so that it produces the same random number sequence again.
        ///   To understand whether this generator can be reset, you can query the <see cref="CanReset"/> property.
        /// </summary>
        /// <returns>
        ///   True if the random number generator was reset; otherwise, false.
        /// </returns>
        bool Reset();

        /// <summary>
        ///   Resets the random number generator using the specified seed, so that it produces the same random number sequence again.
        ///   To understand whether this generator can be reset, you can query the <see cref="CanReset"/> property.
        /// </summary>
        /// <param name="seed">The seed value used by the generator.</param>
        /// <returns>
        ///   True if the random number generator was reset; otherwise, false.
        /// </returns>
        bool Reset(uint seed);

        /// <summary>
        ///   Returns a nonnegative random number less than <see cref="int.MaxValue"/>.
        /// </summary>
        /// <returns>
        ///   A 32-bit signed integer greater than or equal to 0, and less than <see cref="int.MaxValue"/>;
        ///   that is, the range of return values includes 0 but not <see cref="int.MaxValue"/>.
        /// </returns>
        int Next();

        /// <summary>
        ///   Returns a nonnegative random number less than or equal to <see cref="int.MaxValue"/>.
        /// </summary>
        /// <returns>
        ///   A 32-bit signed integer greater than or equal to 0, and less than or equal to <see cref="int.MaxValue"/>;
        ///   that is, the range of return values includes 0 and <see cref="int.MaxValue"/>.
        /// </returns>
        int NextInclusiveMaxValue();

        /// <summary>
        ///   Returns a nonnegative random number less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">
        ///   The exclusive upper bound of the random number to be generated.
        /// </param>
        /// <returns>
        ///   A 32-bit signed integer greater than or equal to 0, and less than <paramref name="maxValue"/>;
        ///   that is, the range of return values includes 0 but not <paramref name="maxValue"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="maxValue"/> must be greater than or equal to 0.
        /// </exception>
        int Next(int maxValue);

        /// <summary>
        ///   Returns a random number within the specified range.
        /// </summary>
        /// <param name="minValue">
        ///   The inclusive lower bound of the random number to be generated.
        /// </param>
        /// <param name="maxValue">
        ///   The exclusive upper bound of the random number to be generated.
        ///   <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.
        /// </param>
        /// <returns>
        ///   A 32-bit signed integer greater than or equal to <paramref name="minValue"/>, and less than
        ///   <paramref name="maxValue"/>; that is, the range of return values includes <paramref name="minValue"/> but
        ///   not <paramref name="maxValue"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.
        /// </exception>
        int Next(int minValue, int maxValue);

        /// <summary>
        ///   Returns a nonnegative floating point random number less than 1.0.
        /// </summary>
        /// <returns>
        ///   A double-precision floating point number greater than or equal to 0.0, and less than 1.0;
        ///   that is, the range of return values includes 0.0 but not 1.0.
        /// </returns>
        double NextDouble();

        /// <summary>
        ///   Returns a nonnegative floating point random number less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">
        ///   The exclusive upper bound of the random number to be generated.
        /// </param>
        /// <returns>
        ///   A double-precision floating point number greater than or equal to 0.0,
        ///   and less than <paramref name="maxValue"/>; that is, the range of return values
        ///   includes 0 but not <paramref name="maxValue"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="maxValue"/> must be greater than or equal to 0.0.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///   <paramref name="maxValue"/> cannot be <see cref="double.PositiveInfinity"/>.
        /// </exception>
        double NextDouble(double maxValue);

        /// <summary>
        ///   Returns a floating point random number within the specified range.
        /// </summary>
        /// <param name="minValue">
        ///   The inclusive lower bound of the random number to be generated.
        /// </param>
        /// <param name="maxValue">
        ///   The exclusive upper bound of the random number to be generated.
        /// </param>
        /// <returns>
        ///   A double-precision floating point number greater than or equal to <paramref name="minValue"/>,
        ///   and less than <paramref name="maxValue"/>; that is, the range of return values
        ///   includes <paramref name="minValue"/> but not <paramref name="maxValue"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///   The difference between <paramref name="maxValue"/> and <paramref name="minValue"/>
        ///   cannot be <see cref="double.PositiveInfinity"/>.
        /// </exception>
        double NextDouble(double minValue, double maxValue);

        /// <summary>
        ///   Returns an unsigned random number.
        /// </summary>
        /// <returns>
        ///   A 32-bit unsigned integer greater than or equal to <see cref="uint.MinValue"/> and
        ///   less than or equal to <see cref="uint.MaxValue"/>.
        /// </returns>
        uint NextUInt();

        /// <summary>
        ///   Returns an unsigned random number less than <see cref="uint.MaxValue"/>.
        /// </summary>
        /// <returns>
        ///   A 32-bit unsigned integer greater than or equal to <see cref="uint.MinValue"/> and
        ///   less than <see cref="uint.MaxValue"/>.
        /// </returns>
        uint NextUIntExclusiveMaxValue();

        /// <summary>
        ///   Returns an unsigned random number less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">
        ///   The exclusive upper bound of the random number to be generated.
        /// </param>
        /// <returns>
        ///   A 32-bit unsigned integer greater than or equal to <see cref="uint.MinValue"/> and
        ///   less than <paramref name="maxValue"/>; that is, the range of return values includes
        ///   <see cref="uint.MinValue"/> but not <paramref name="maxValue"/>.
        /// </returns>
        uint NextUInt(uint maxValue);

        /// <summary>
        ///   Returns an unsigned random number within the specified range.
        /// </summary>
        /// <param name="minValue">
        ///   The inclusive lower bound of the random number to be generated.
        /// </param>
        /// <param name="maxValue">
        ///   The exclusive upper bound of the random number to be generated.
        /// </param>
        /// <returns>
        ///   A 32-bit unsigned integer greater than or equal to <paramref name="minValue"/> and
        ///   less than <paramref name="maxValue"/>; that is, the range of return values includes <paramref name="minValue"/> but
        ///   not <paramref name="maxValue"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.
        /// </exception>
        uint NextUInt(uint minValue, uint maxValue);

        /// <summary>
        ///   Returns a random Boolean value.
        /// </summary>
        /// <remarks>
        ///   Buffers 31 random bits for future calls, so the random number generator
        ///   is only invoked once in every 31 calls.
        /// </remarks>
        /// <returns>A <see cref="bool"/> value.</returns>
        bool NextBoolean();

        /// <summary>
        ///   Fills the elements of a specified array of bytes with random numbers.
        /// </summary>
        /// <remarks>
        ///   Each element of the array of bytes is set to a random number greater than or equal to 0,
        ///   and less than or equal to <see cref="byte.MaxValue"/>.
        /// </remarks>
        /// <param name="buffer">An array of bytes to contain random numbers.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="buffer"/> is null.
        /// </exception>
        void NextBytes(byte[] buffer);
    }
}