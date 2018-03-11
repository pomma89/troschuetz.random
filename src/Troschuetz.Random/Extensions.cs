// The MIT License (MIT)
//
// Copyright (c) 2006-2007 Stefan Troschütz <stefan@troschuetz.de>
//
// Copyright (c) 2012-2019 Alessio Parma <alessio.parma@gmail.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute,
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT
// OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Troschuetz.Random
{
    using Core;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    ///   Module containing extension methods for many interfaces exposed by this library.
    /// </summary>
    public static class Extensions
    {
        #region IContinuousDistribution Extensions

        /// <summary>
        ///   Returns an infinites series of random double numbers, by repeating calls to NextDouble.
        ///   Therefore, the series obtained will follow given distribution.
        /// </summary>
        /// <param name="distribution">The distribution.</param>
        /// <returns>An infinites series of random double numbers, following given distribution.</returns>
        public static IEnumerable<double> DistributedDoubles<T>(this T distribution)
            where T : class, IContinuousDistribution
        {
            // Preconditions
            if (distribution == null) throw new ArgumentNullException(nameof(distribution), ErrorMessages.NullDistribution);

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
        public static IEnumerable<int> DistributedIntegers<T>(this T distribution)
            where T : class, IDiscreteDistribution
        {
            // Preconditions
            if (distribution == null) throw new ArgumentNullException(nameof(distribution), ErrorMessages.NullDistribution);

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
        ///   Buffers 31 random bits for future calls, so the random number generator is only invoked
        ///   once in every 31 calls.
        /// </remarks>
        /// <typeparam name="TGen">The type of the random numbers generator.</typeparam>
        /// <param name="generator">The generator from which random numbers are drawn.</param>
        /// <returns>An infinite sequence random Boolean values.</returns>
        public static IEnumerable<bool> Booleans<TGen>(this TGen generator)
            where TGen : class, IGenerator
        {
            // Preconditions
            if (generator == null) throw new ArgumentNullException(nameof(generator), ErrorMessages.NullGenerator);

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
        public static IEnumerable<bool> Bytes<TGen>(this TGen generator, byte[] buffer)
            where TGen : class, IGenerator
        {
            // Preconditions
            if (generator == null) throw new ArgumentNullException(nameof(generator), ErrorMessages.NullGenerator);
            if (buffer == null) throw new ArgumentNullException(nameof(buffer), ErrorMessages.NullBuffer);

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
        public static TItem Choice<TGen, TItem>(this TGen generator, IList<TItem> list)
            where TGen : class, IGenerator
        {
            // Preconditions
            if (generator == null) throw new ArgumentNullException(nameof(generator), ErrorMessages.NullGenerator);
            if (list == null) throw new ArgumentNullException(nameof(list), ErrorMessages.NullList);
            if (list.Count == 0) throw new ArgumentException(ErrorMessages.EmptyList, nameof(list));

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
        public static IEnumerable<TItem> Choices<TGen, TItem>(this TGen generator, IList<TItem> list)
            where TGen : class, IGenerator
        {
            // Preconditions
            if (generator == null) throw new ArgumentNullException(nameof(generator), ErrorMessages.NullGenerator);
            if (list == null) throw new ArgumentNullException(nameof(list), ErrorMessages.NullList);
            if (list.Count == 0) throw new ArgumentException(ErrorMessages.EmptyList, nameof(list));

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
        public static IEnumerable<double> Doubles<TGen>(this TGen generator)
            where TGen : class, IGenerator
        {
            // Preconditions
            if (generator == null) throw new ArgumentNullException(nameof(generator), ErrorMessages.NullGenerator);

            while (true)
            {
                yield return generator.NextDouble();
            }
        }

        /// <summary>
        ///   Returns an infinite sequence of nonnegative floating point random numbers less than the
        ///   specified maximum.
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
        ///   <paramref name="maxValue"/> must be greater than or equal to 0.0.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="maxValue"/> cannot be <see cref="double.PositiveInfinity"/>.</exception>
        public static IEnumerable<double> Doubles<TGen>(this TGen generator, double maxValue)
            where TGen : class, IGenerator
        {
            // Preconditions
            if (generator == null) throw new ArgumentNullException(nameof(generator), ErrorMessages.NullGenerator);
            if (maxValue < 0.0) throw new ArgumentOutOfRangeException(nameof(maxValue), ErrorMessages.NegativeMaxValue);
            if (double.IsPositiveInfinity(maxValue)) throw new ArgumentException(ErrorMessages.InfiniteMaxValue, nameof(maxValue));

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
        ///   Returns an infinite sequence of double-precision floating point numbers greater than or
        ///   equal to <paramref name="minValue"/>, and less than <paramref name="maxValue"/>; that
        ///   is, the range of return values includes <paramref name="minValue"/> but not <paramref name="maxValue"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///   The difference between <paramref name="maxValue"/> and <paramref name="minValue"/>
        ///   cannot be <see cref="double.PositiveInfinity"/>.
        /// </exception>
        public static IEnumerable<double> Doubles<TGen>(this TGen generator, double minValue, double maxValue)
            where TGen : class, IGenerator
        {
            // Preconditions
            if (generator == null) throw new ArgumentNullException(nameof(generator), ErrorMessages.NullGenerator);
            if (minValue > maxValue) throw new ArgumentOutOfRangeException(nameof(minValue), ErrorMessages.MinValueGreaterThanMaxValue);
            if (double.IsPositiveInfinity(maxValue - minValue)) throw new ArgumentException(ErrorMessages.InfiniteMaxValueMinusMinValue, nameof(minValue));

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
        ///   than <see cref="int.MaxValue"/>; that is, the range of return values includes 0 but not <see cref="int.MaxValue"/>.
        /// </returns>
        public static IEnumerable<int> Integers<TGen>(this TGen generator)
            where TGen : class, IGenerator
        {
            // Preconditions
            if (generator == null) throw new ArgumentNullException(nameof(generator), ErrorMessages.NullGenerator);

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
        ///   <paramref name="maxValue"/> must be greater than or equal to 0.
        /// </exception>
        public static IEnumerable<int> Integers<TGen>(this TGen generator, int maxValue)
            where TGen : class, IGenerator
        {
            // Preconditions
            if (generator == null) throw new ArgumentNullException(nameof(generator), ErrorMessages.NullGenerator);
            if (maxValue < 0) throw new ArgumentOutOfRangeException(nameof(maxValue), ErrorMessages.NegativeMaxValue);

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
        ///   <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.
        /// </exception>
        public static IEnumerable<int> Integers<TGen>(this TGen generator, int minValue, int maxValue)
            where TGen : class, IGenerator
        {
            // Preconditions
            if (generator == null) throw new ArgumentNullException(nameof(generator), ErrorMessages.NullGenerator);
            if (minValue > maxValue) throw new ArgumentOutOfRangeException(nameof(minValue), ErrorMessages.MinValueGreaterThanMaxValue);

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
        public static IEnumerable<uint> UnsignedIntegers<TGen>(this TGen generator)
            where TGen : class, IGenerator
        {
            // Preconditions
            if (generator == null) throw new ArgumentNullException(nameof(generator), ErrorMessages.NullGenerator);

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
        ///   <paramref name="maxValue"/> must be greater than or equal to 0.
        /// </exception>
        public static IEnumerable<uint> UnsignedIntegers<TGen>(this TGen generator, uint maxValue)
            where TGen : class, IGenerator
        {
            // Preconditions
            if (generator == null) throw new ArgumentNullException(nameof(generator), ErrorMessages.NullGenerator);

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
        ///   <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.
        /// </exception>
        public static IEnumerable<uint> UnsignedIntegers<TGen>(this TGen generator, uint minValue, uint maxValue)
            where TGen : class, IGenerator
        {
            // Preconditions
            if (generator == null) throw new ArgumentNullException(nameof(generator), ErrorMessages.NullGenerator);
            if (minValue > maxValue) throw new ArgumentOutOfRangeException(nameof(minValue), ErrorMessages.MinValueGreaterThanMaxValue);

            while (true)
            {
                yield return generator.NextUInt(minValue, maxValue);
            }
        }

        #endregion IGenerator Extensions
    }
}