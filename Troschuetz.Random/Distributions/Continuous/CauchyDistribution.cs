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
    using Core;

    /// <summary>
    ///   Provides generation of cauchy distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="CauchyDistribution"/> type bases upon information presented on
    ///   <a href="http://en.wikipedia.org/wiki/Cauchy_distribution">Wikipedia - Cauchy distribution</a> and
    ///   <a href="http://www.xycoon.com/cauchy2p_random.htm">Xycoon - Cauchy Distribution</a>.
    /// </remarks>
    [Serializable]
    public class CauchyDistribution<TGen> : Distribution<TGen>, IContinuousDistribution, IAlphaDistribution<double>, IGammaDistribution<double>
        where TGen : IGenerator
    {
        #region Class Fields

        /// <summary>
        ///   The default value assigned to <see cref="Alpha"/> if none is specified. 
        /// </summary>
        public const double DefaultAlpha = 1;

        /// <summary>
        ///   The default value assigned to <see cref="Gamma"/> if none is specified. 
        /// </summary>
        public const double DefaultGamma = 1;

        #endregion

        #region Instance Fields

        /// <summary>
        ///   Stores the parameter alpha which is used for generation of cauchy distributed random numbers.
        /// </summary>
        double _alpha;

        /// <summary>
        ///   Stores the parameter gamma which is used for generation of cauchy distributed random numbers.
        /// </summary>
        double _gamma;

        /// <summary>
        ///   Gets or sets the parameter alpha of cauchy distributed random numbers which is used for their generation.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value"/> is equal to <see cref="double.NaN"/>.
        /// </exception>
        /// <remarks>
        ///   Calls <see cref="AreValidParams"/> to determine whether a value is valid and therefore assignable.
        /// </remarks>
        public double Alpha
        {
            get { return _alpha; }
            set { _alpha = value; }
        }

        /// <summary>
        ///   Gets or sets the parameter gamma which is used for generation of cauchy distributed random numbers.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   Calls <see cref="AreValidParams"/> to determine whether a value is valid and therefore assignable.
        /// </remarks>
        public double Gamma
        {
            get { return _gamma; }
            set { _gamma = value; }
        }

        #endregion

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="CauchyDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of cauchy distributed random numbers.
        /// </param>
        /// <param name="gamma">
        ///   The parameter gamma which is used for generation of cauchy distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="gamma"/> is less than or equal to zero.
        /// </exception>
        public CauchyDistribution(TGen generator, double alpha, double gamma)
            : base(generator)
        {
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(generator, null), ErrorMessages.NullGenerator);
            Contract.Requires<ArgumentOutOfRangeException>(AreValidParams(alpha, gamma), ErrorMessages.InvalidParams);
            _alpha = alpha;
            _gamma = gamma;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Alpha"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns><see langword="true"/>.</returns>
        public bool IsValidAlpha(double value)
        {
            return AreValidParams(value, _gamma);
        }

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Gamma"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>
        ///   <see langword="true"/> if value is greater than 0; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsValidGamma(double value)
        {
            return AreValidParams(_alpha, value);
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
            get { throw new NotSupportedException(ErrorMessages.UndefinedMean); }
        }

        public double Median
        {
            get { return Alpha; }
        }

        public double Variance
        {
            get { throw new NotSupportedException(ErrorMessages.UndefinedVariance); }
        }

        public double[] Mode
        {
            get { return new[] {Alpha}; }
        }

        public double NextDouble()
        {
            return Sample(Gen, _alpha, _gamma);
        }

        #endregion

        #region TRandom Helpers

        /// <summary>
        ///   Determines whether cauchy distribution is defined under given parameters.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of cauchy distributed random numbers.
        /// </param>
        /// <param name="gamma">
        ///   The parameter gamma which is used for generation of cauchy distributed random numbers.
        /// </param>
        /// <returns>
        ///   True if <paramref name="gamma"/> is greater than zero; otherwise, it returns false.
        /// </returns>
        [System.Diagnostics.Contracts.Pure]
        public static bool AreValidParams(double alpha, double gamma)
        {
            return !double.IsNaN(alpha) && gamma > 0;
        }

        /// <summary>
        ///   Returns a cauchy distributed floating point random number.
        /// </summary>
        /// <param name="generator">The generator from which random number are drawn.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of cauchy distributed random numbers.
        /// </param>
        /// <param name="gamma">
        ///   The parameter gamma which is used for generation of cauchy distributed random numbers.
        /// </param>
        /// <returns>
        ///   A cauchy distributed floating point random number.
        /// </returns>
        [System.Diagnostics.Contracts.Pure]
        internal static double Sample(TGen generator, double alpha, double gamma)
        {
            return alpha + gamma*Math.Tan(Math.PI*(generator.NextDouble() - 0.5));
        }

        #endregion
    }

    /// <summary>
    ///   Provides generation of cauchy distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="CauchyDistribution"/> type bases upon information presented on
    ///   <a href="http://en.wikipedia.org/wiki/Cauchy_distribution">Wikipedia - Cauchy distribution</a> and
    ///   <a href="http://www.xycoon.com/cauchy2p_random.htm">Xycoon - Cauchy Distribution</a>.
    /// </remarks>
    [Serializable]
    public sealed class CauchyDistribution : CauchyDistribution<IGenerator>
    {
        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="CauchyDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        public CauchyDistribution()
            : base(new XorShift128Generator(), DefaultAlpha, DefaultGamma)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Gamma, DefaultGamma));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="CauchyDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        [CLSCompliant(false)]
        public CauchyDistribution(uint seed)
            : base(new XorShift128Generator(seed), DefaultAlpha, DefaultGamma)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Gamma, DefaultGamma));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="CauchyDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        public CauchyDistribution(IGenerator generator)
            : base(generator, DefaultAlpha, DefaultGamma)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Gamma, DefaultGamma));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="CauchyDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of cauchy distributed random numbers.
        /// </param>
        /// <param name="gamma">
        ///   The parameter gamma which is used for generation of cauchy distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="gamma"/> is less than or equal to zero.
        /// </exception>
        public CauchyDistribution(double alpha, double gamma)
            : base(new XorShift128Generator(), alpha, gamma)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Gamma, gamma));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="CauchyDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of cauchy distributed random numbers.
        /// </param>
        /// <param name="gamma">
        ///   The parameter gamma which is used for generation of cauchy distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="gamma"/> is less than or equal to zero.
        /// </exception>
        [CLSCompliant(false)]
        public CauchyDistribution(uint seed, double alpha, double gamma)
            : base(new XorShift128Generator(seed), alpha, gamma)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Gamma, gamma));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="CauchyDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of cauchy distributed random numbers.
        /// </param>
        /// <param name="gamma">
        ///   The parameter gamma which is used for generation of cauchy distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="gamma"/> is less than or equal to zero.
        /// </exception>
        public CauchyDistribution(IGenerator generator, double alpha, double gamma) : base(generator, alpha, gamma)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Gamma, gamma));
        }

        #endregion
    }
}