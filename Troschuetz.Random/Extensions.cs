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

namespace Troschuetz.Random
{
    using Core;
    using PommaLabs.Thrower;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    /// <summary>
    ///   Module containing extension methods for many interfaces exposed by this library.
    /// </summary>
    public static class Extensions
    {
        #region IContinuousDistribution Extensions

        /// <summary>
        ///   Returns an infinites series of random double numbers, by repeating calls to
        ///   NextDouble. Therefore, the series obtained will follow given distribution.
        /// </summary>
        /// <param name="distribution">The distribution.</param>
        /// <returns>An infinites series of random double numbers, following given distribution.</returns>
        [Pure]
        public static IEnumerable<double> DistributedDoubles<T>(this T distribution) where T : IContinuousDistribution
        {
            RaiseArgumentNullException.IfIsNull(distribution, nameof(distribution), ErrorMessages.NullDistribution);
            while (true)
            {
                yield return distribution.NextDouble();
            }
        }

        #endregion IContinuousDistribution Extensions

        #region IDiscreteDistribution Extensions

        /// <summary>
        ///   Returns an infinites series of random numbers, by repeating calls to Next. Therefore,
        ///   the series obtained will follow given distribution.
        /// </summary>
        /// <param name="distribution">The distribution.</param>
        /// <returns>An infinites series of random numbers, following given distribution.</returns>
        [Pure]
        public static IEnumerable<int> DistributedIntegers<T>(this T distribution) where T : IDiscreteDistribution
        {
            RaiseArgumentNullException.IfIsNull(distribution, nameof(distribution), ErrorMessages.NullDistribution);
            while (true)
            {
                yield return distribution.Next();
            }
        }

        #endregion IDiscreteDistribution Extensions

        #region IGenerator Extensions

        /// <summary>
        ///   Returns an infinite sequence random Boolean values.
        /// </summary>
        /// <remarks>
        ///   Buffers 31 random bits for future calls, so the random number generator is only
        ///   invoked once in every 31 calls.
        /// </remarks>
        /// <typeparam name="TGen">The type of the random numbers generator.</typeparam>
        /// <param name="generator">The generator from which random numbers are drawn.</param>
        /// <returns>An infinite sequence random Boolean values.</returns>
        [Pure]
        public static IEnumerable<bool> Booleans<TGen>(this TGen generator) where TGen : IGenerator
        {
            RaiseArgumentNullException.IfIsNull(generator, nameof(generator), ErrorMessages.NullGenerator);
            while (true)
            {
                yield return generator.NextBoolean();
            }
        }

        /// <summary>
        ///   Repeatedly fills the elements of a specified array of bytes with random numbers.
        /// </summary>
        /// <remarks>
        ///   Each element of the array of bytes is set to a random number greater than or equal to
        ///   0, and less than or equal to <see cref="byte.MaxValue"/>.
        /// </remarks>
        /// <typeparam name="TGen">The type of the random numbers generator.</typeparam>
        /// <param name="generator">The generator from which random numbers are drawn.</param>
        /// <param name="buffer">An array of bytes to contain random numbers.</param>
        /// <returns>An infinite sequence of true values.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="buffer"/> is null.</exception>
        [Pure]
        public static IEnumerable<bool> Bytes<TGen>(this TGen generator, byte[] buffer) where TGen : IGenerator
        {
            RaiseArgumentNullException.IfIsNull(generator, nameof(generator), ErrorMessages.NullGenerator);
            RaiseArgumentNullException.IfIsNull(buffer, nameof(buffer), ErrorMessages.NullBuffer);
            while (true)
            {
                generator.NextBytes(buffer);
                yield return true;
            }
        }

        /// <summary>
        ///   Returns a random item from given list, according to a uniform distribution.
        /// </summary>
        /// <typeparam name="TGen">The type of the random numbers generator.</typeparam>
        /// <typeparam name="TItem">The type of the elements of the list.</typeparam>
        /// <param name="generator">The generator from which random numbers are drawn.</param>
        /// <param name="list">The list from which an item should be randomly picked.</param>
        /// <returns>A random item belonging to given list.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="list"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="list"/> is empty.</exception>
        [Pure]
        public static TItem Choice<TGen, TItem>(this TGen generator, IList<TItem> list) where TGen : IGenerator
        {
            RaiseArgumentNullException.IfIsNull(generator, nameof(generator), ErrorMessages.NullGenerator);
            RaiseArgumentNullException.IfIsNull(list, nameof(list), ErrorMessages.NullList);
            Raise<ArgumentException>.IfIsEmpty(list, ErrorMessages.EmptyList);
            var idx = generator.Next(list.Count);
            Debug.Assert(idx >= 0 && idx < list.Count);
            return list[idx];
        }

        /// <summary>
        ///   Returns an infinite sequence of random items from given list, according to a uniform distribution.
        /// </summary>
        /// <typeparam name="TGen">The type of the random numbers generator.</typeparam>
        /// <typeparam name="TItem">The type of the elements of the list.</typeparam>
        /// <param name="generator">The generator from which random numbers are drawn.</param>
        /// <param name="list">The list from which items should be randomly picked.</param>
        /// <returns>An infinite sequence of random items belonging to given list.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="list"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="list"/> is empty.</exception>
        [Pure]
        public static IEnumerable<TItem> Choices<TGen, TItem>(this TGen generator, IList<TItem> list) where TGen : IGenerator
        {
            RaiseArgumentNullException.IfIsNull(generator, nameof(generator), ErrorMessages.NullGenerator);
            RaiseArgumentNullException.IfIsNull(list, nameof(list), ErrorMessages.NullList);
            Raise<ArgumentException>.IfIsEmpty(list, ErrorMessages.EmptyList);
            while (true)
            {
                var idx = generator.Next(list.Count);
                Debug.Assert(idx >= 0 && idx < list.Count);
                yield return list[idx];
            }
        }

        /// <summary>
        ///   Returns an infinite sequence of nonnegative floating point random numbers less than 1.0.
        /// </summary>
        /// <typeparam name="TGen">The type of the random numbers generator.</typeparam>
        /// <param name="generator">The generator from which random numbers are drawn.</param>
        /// <returns>
        ///   An infinite sequence of double-precision floating point numbers greater than or equal
        ///   to 0.0, and less than 1.0; that is, the range of return values includes 0.0 but not 1.0.
        /// </returns>
        [Pure]
        public static IEnumerable<double> Doubles<TGen>(this TGen generator) where TGen : IGenerator
        {
            RaiseArgumentNullException.IfIsNull(generator, nameof(generator), ErrorMessages.NullGenerator);

            while (true)
            {
                yield return generator.NextDouble();
            }
        }

        /// <summary>
        ///   Returns an infinite sequence of nonnegative floating point random numbers less than
        ///   the specified maximum.
        /// </summary>
        /// <typeparam name="TGen">The type of the random numbers generator.</typeparam>
        /// <param name="generator">The generator from which random numbers are drawn.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated.</param>
        /// <returns>
        ///   An infinite sequence of double-precision floating point numbers greater than or equal
        ///   to 0.0, and less than <paramref name="maxValue"/>; that is, the range of return values
        ///   includes 0 but not <paramref name="maxValue"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="maxValue"/> must be greater than 0.0.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="maxValue"/> cannot be <see cref="double.PositiveInfinity"/>.</exception>
        [Pure]
        public static IEnumerable<double> Doubles<TGen>(this TGen generator, double maxValue) where TGen : IGenerator
        {
            RaiseArgumentNullException.IfIsNull(generator, nameof(generator), ErrorMessages.NullGenerator);
            RaiseArgumentOutOfRangeException.IfIsLessOrEqual(maxValue, 0.0, nameof(maxValue), ErrorMessages.NegativeMaxValue);
            Raise<ArgumentException>.If(double.IsPositiveInfinity(maxValue));

            while (true)
            {
                yield return generator.NextDouble(maxValue);
            }
        }

        /// <summary>
        ///   Returns an infinite sequence of floating point random numbers within the specified range.
        /// </summary>
        /// <typeparam name="TGen">The type of the random numbers generator.</typeparam>
        /// <param name="generator">The generator from which random numbers are drawn.</param>
        /// <param name="minValue">The inclusive lower bound of the random number to be generated.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated.</param>
        /// <returns>
        ///   Returns an infinite sequence of double-precision floating point numbers greater than
        ///   or equal to <paramref name="minValue"/>, and less than <paramref name="maxValue"/>;
        ///   that is, the range of return values includes <paramref name="minValue"/> but not <paramref name="maxValue"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///   The difference between <paramref name="maxValue"/> and <paramref name="minValue"/>
        ///   cannot be <see cref="double.PositiveInfinity"/>.
        /// </exception>
        [Pure]
        public static IEnumerable<double> Doubles<TGen>(this TGen generator, double minValue, double maxValue) where TGen : IGenerator
        {
            RaiseArgumentNullException.IfIsNull(generator, nameof(generator), ErrorMessages.NullGenerator);
            RaiseArgumentOutOfRangeException.IfIsGreaterOrEqual(minValue, maxValue, nameof(minValue), ErrorMessages.MinValueGreaterThanOrEqualToMaxValue);
            Raise<ArgumentException>.If(double.IsPositiveInfinity(maxValue - minValue));

            while (true)
            {
                yield return generator.NextDouble(minValue, maxValue);
            }
        }

        /// <summary>
        ///   Returns an infinite sequence of nonnegative random numbers less than <see cref="int.MaxValue"/>.
        /// </summary>
        /// <typeparam name="TGen">The type of the random numbers generator.</typeparam>
        /// <param name="generator">The generator from which random numbers are drawn.</param>
        /// <returns>
        ///   An infinite sequence of 32-bit signed integers greater than or equal to 0, and less
        ///   than <see cref="int.MaxValue"/>; that is, the range of return values includes 0 but
        ///   not <see cref="int.MaxValue"/>.
        /// </returns>
        [Pure]
        public static IEnumerable<int> Integers<TGen>(this TGen generator) where TGen : IGenerator
        {
            RaiseArgumentNullException.IfIsNull(generator, nameof(generator), ErrorMessages.NullGenerator);

            while (true)
            {
                yield return generator.Next();
            }
        }

        /// <summary>
        ///   Returns an infinite sequence of nonnegative random numbers less than the specified maximum.
        /// </summary>
        /// <typeparam name="TGen">The type of the random numbers generator.</typeparam>
        /// <param name="generator">The generator from which random numbers are drawn.</param>
        /// <param name="maxValue">The exclusive upper bound of the random numbers to be generated.</param>
        /// <returns>
        ///   An infinite sequence of 32-bit signed integers greater than or equal to 0, and less
        ///   than <paramref name="maxValue"/>; that is, the range of return values includes 0 but
        ///   not <paramref name="maxValue"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="maxValue"/> must be greater than or equal to 1.
        /// </exception>
        [Pure]
        public static IEnumerable<int> Integers<TGen>(this TGen generator, int maxValue) where TGen : IGenerator
        {
            RaiseArgumentNullException.IfIsNull(generator, nameof(generator), ErrorMessages.NullGenerator);
            RaiseArgumentOutOfRangeException.IfIsLessOrEqual(maxValue, 0, nameof(maxValue), ErrorMessages.NegativeMaxValue);

            while (true)
            {
                yield return generator.Next(maxValue);
            }
        }

        /// <summary>
        ///   Returns an infinite sequence of random numbers within the specified range.
        /// </summary>
        /// <typeparam name="TGen">The type of the random numbers generator.</typeparam>
        /// <param name="generator">The generator from which random numbers are drawn.</param>
        /// <param name="minValue">The inclusive lower bound of the random number to be generated.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated.</param>
        /// <returns>
        ///   An infinite sequence of 32-bit signed integers greater than or equal to
        ///   <paramref name="minValue"/>, and less than <paramref name="maxValue"/>; that is, the
        ///   range of return values includes <paramref name="minValue"/> but not <paramref name="maxValue"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="maxValue"/> must be greater than <paramref name="minValue"/>.
        /// </exception>
        [Pure]
        public static IEnumerable<int> Integers<TGen>(this TGen generator, int minValue, int maxValue) where TGen : IGenerator
        {
            RaiseArgumentNullException.IfIsNull(generator, nameof(generator), ErrorMessages.NullGenerator);
            RaiseArgumentOutOfRangeException.IfIsGreaterOrEqual(minValue, maxValue, nameof(minValue), ErrorMessages.MinValueGreaterThanOrEqualToMaxValue);

            while (true)
            {
                yield return generator.Next(minValue, maxValue);
            }
        }

        /// <summary>
        ///   Returns an infinite sequence of unsigned random numbers.
        /// </summary>
        /// <typeparam name="TGen">The type of the random numbers generator.</typeparam>
        /// <param name="generator">The generator from which random numbers are drawn.</param>
        /// <returns>An infinite sequence of 32-bit unsigned integers.</returns>
        [Pure]
        public static IEnumerable<uint> UnsignedIntegers<TGen>(this TGen generator) where TGen : IGenerator
        {
            RaiseArgumentNullException.IfIsNull(generator, nameof(generator), ErrorMessages.NullGenerator);

            while (true)
            {
                yield return generator.NextUInt();
            }
        }

        /// <summary>
        ///   Returns an infinite sequence of unsigned random numbers less than the specified maximum.
        /// </summary>
        /// <typeparam name="TGen">The type of the random numbers generator.</typeparam>
        /// <param name="generator">The generator from which random numbers are drawn.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated.</param>
        /// <returns>
        ///   An infinite sequence of 32-bit unsigned integers greater than or equal to 0, and less
        ///   than <paramref name="maxValue"/>; that is, the range of return values includes 0 but
        ///   not <paramref name="maxValue"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="maxValue"/> must be greater than or equal to 1.
        /// </exception>
        [Pure]
        public static IEnumerable<uint> UnsignedIntegers<TGen>(this TGen generator, uint maxValue) where TGen : IGenerator
        {
            RaiseArgumentNullException.IfIsNull(generator, nameof(generator), ErrorMessages.NullGenerator);
            RaiseArgumentOutOfRangeException.IfIsLess(maxValue, 1U, nameof(maxValue), ErrorMessages.MaxValueIsTooSmall);

            while (true)
            {
                yield return generator.NextUInt(maxValue);
            }
        }

        /// <summary>
        ///   Returns an infinite sequence of unsigned random numbers within the specified range.
        /// </summary>
        /// <typeparam name="TGen">The type of the random numbers generator.</typeparam>
        /// <param name="generator">The generator from which random numbers are drawn.</param>
        /// <param name="minValue">The inclusive lower bound of the random number to be generated.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated.</param>
        /// <returns>
        ///   An infinite sequence of 32-bit unsigned integers greater than or equal to
        ///   <paramref name="minValue"/>, and less than <paramref name="maxValue"/>; that is, the
        ///   range of return values includes <paramref name="minValue"/> but not <paramref name="maxValue"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="maxValue"/> must be greater than <paramref name="minValue"/>.
        /// </exception>
        [Pure]
        public static IEnumerable<uint> UnsignedIntegers<TGen>(this TGen generator, uint minValue, uint maxValue) where TGen : IGenerator
        {
            RaiseArgumentNullException.IfIsNull(generator, nameof(generator), ErrorMessages.NullGenerator);
            RaiseArgumentOutOfRangeException.IfIsGreaterOrEqual(minValue, maxValue, nameof(minValue), ErrorMessages.MinValueGreaterThanOrEqualToMaxValue);

            while (true)
            {
                yield return generator.NextUInt(minValue, maxValue);
            }
        }

        #endregion IGenerator Extensions
    }
}

#if PORTABLE

namespace System
{
    /// <summary>
    ///   Fake, this is used only to allow serialization on portable platforms.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true, Inherited = false)]
    public sealed class SerializableAttribute : Attribute
    {
        // This does nothing and should do nothing.
    }
}

#endif
