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

namespace Troschuetz.Random.Distributions.Continuous
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using Generators;

    /// <summary>
    ///   Provides generation of t-distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="StudentsTDistribution"/> type bases upon information presented on
    ///   <a href="http://en.wikipedia.org/wiki/Student%27s_t-distribution">Wikipedia - Student's t-distribution</a> and
    ///   <a href="http://www.xycoon.com/stt_random.htm">Xycoon - Student t Distribution</a>.
    /// </remarks>
    [Serializable]
    public class StudentsTDistribution<TGen> : Distribution<TGen>, IContinuousDistribution, INuDistribution<int>
        where TGen : IGenerator
    {
        #region Class Fields

        /// <summary>
        ///   The default value assigned to <see cref="Nu"/> if none is specified. 
        /// </summary>
        public const int DefaultNu = 1;

        #endregion

        #region Instance Fields

        /// <summary>
        ///   Stores the parameter nu which is used for generation of t-distributed random numbers.
        /// </summary>
        int _nu;

        /// <summary>
        ///   Gets or sets the parameter nu which is used for generation of t-distributed random numbers.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   Calls <see cref="IsValidParam"/> to determine whether a value is valid and therefore assignable.
        /// </remarks>
        public int Nu
        {
            get { return _nu; }
            set { _nu = value; }
        }

        #endregion

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="StudentsTDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="nu">
        ///   The parameter nu which is used for generation of student's t distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="nu"/> is less than or equal to zero.
        /// </exception>
        public StudentsTDistribution(TGen generator, int nu) : base(generator)
        {
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(generator, null), ErrorMessages.NullGenerator);
            Contract.Requires<ArgumentOutOfRangeException>(IsValidParam(nu), ErrorMessages.InvalidParams);
            _nu = nu;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Nu"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>
        ///   <see langword="true"/> if value is greater than 0; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsValidNu(int value)
        {
            return IsValidParam(value);
        }

        #endregion

        #region IContinuousDistribution Members

        public double Minimum
        {
            get { return double.NegativeInfinity; }
        }

        public double Maximum
        {
            get { return double.PositiveInfinity; }
        }

        public double Mean
        {
            get
            {
                if (_nu > 1) {
                    return 0.0;
                }
                throw new NotSupportedException(ErrorMessages.UndefinedMeanForParams);
            }
        }

        public double Median
        {
            get { return 0.0; }
        }

        public double Variance
        {
            get
            {
                if (_nu > 2) {
                    return _nu/(_nu - 2.0);
                }
                throw new NotSupportedException(ErrorMessages.UndefinedVarianceForParams);
            }
        }

        public double[] Mode
        {
            get { return new[] {0.0}; }
        }

        public double NextDouble()
        {
            return Sample(Gen, _nu);
        }

        #endregion

        #region TRandom Helpers

        /// <summary>
        ///   Determines whether student's t distribution is defined under given parameter.
        /// </summary>
        /// <param name="nu">
        ///   The parameter nu which is used for generation of student's t distributed random numbers.
        /// </param>
        /// <returns>
        ///   True if <paramref name="nu"/> is greater than zero; otherwise, it returns false.
        /// </returns>
        [System.Diagnostics.Contracts.Pure]
        public static bool IsValidParam(int nu)
        {
            return nu > 0;
        }

        /// <summary>
        ///   Returns a student's t distributed floating point random number.
        /// </summary>
        /// <param name="generator">The generator from which random number are drawn.</param>
        /// <param name="nu">
        ///   The parameter nu which is used for generation of student's t distributed random numbers.
        /// </param>
        /// <returns>
        ///   A student's t distributed floating point random number.
        /// </returns>
        [System.Diagnostics.Contracts.Pure]
        internal static double Sample(TGen generator, int nu)
        {
            const double mu = 0.0;
            const double sigma = 1.0;
            var n = NormalDistribution<TGen>.Sample(generator, mu, sigma);
            var c = ChiSquareDistribution<TGen>.Sample(generator, nu);
            return n/Math.Sqrt(c/nu);
        }

        #endregion
    }

    /// <summary>
    ///   Provides generation of t-distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="StudentsTDistribution"/> type bases upon information presented on
    ///   <a href="http://en.wikipedia.org/wiki/Student%27s_t-distribution">Wikipedia - Student's t-distribution</a> and
    ///   <a href="http://www.xycoon.com/stt_random.htm">Xycoon - Student t Distribution</a>.
    /// </remarks>
    [Serializable]
    public sealed class StudentsTDistribution : StudentsTDistribution<IGenerator>
    {
        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="StudentsTDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        public StudentsTDistribution() : base(new XorShift128Generator(), DefaultNu)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Nu, DefaultNu));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="StudentsTDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        [CLSCompliant(false)]
        public StudentsTDistribution(uint seed) : base(new XorShift128Generator(seed), DefaultNu)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Nu, DefaultNu));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="StudentsTDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        public StudentsTDistribution(IGenerator generator) : base(generator, DefaultNu)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Nu, DefaultNu));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="StudentsTDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="nu">
        ///   The parameter nu which is used for generation of student's t distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="nu"/> is less than or equal to zero.
        /// </exception>
        public StudentsTDistribution(int nu) : base(new XorShift128Generator(), nu)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Nu, nu));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="StudentsTDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        /// <param name="nu">
        ///   The parameter nu which is used for generation of student's t distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="nu"/> is less than or equal to zero.
        /// </exception>
        [CLSCompliant(false)]
        public StudentsTDistribution(uint seed, int nu) : base(new XorShift128Generator(seed), nu)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Nu, nu));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="StudentsTDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="nu">
        ///   The parameter nu which is used for generation of student's t distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="nu"/> is less than or equal to zero.
        /// </exception>
        public StudentsTDistribution(IGenerator generator, int nu) : base(generator, nu)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Nu, nu));
        }

        #endregion
    }
}