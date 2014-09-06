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
    ///   Provides generation of continuous uniformly distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="ContinuousUniformDistribution"/> type bases upon information presented on
    ///   <a href="http://en.wikipedia.org/wiki/Uniform_distribution_%28continuous%29"> 
    ///   Wikipedia - Uniform distribution (continuous)</a>.
    /// </remarks>
    [Serializable]
    public class ContinuousUniformDistribution<TGen> : Distribution<TGen>, IContinuousDistribution, IAlphaDistribution<double>,
                                                       IBetaDistribution<double>
        where TGen : IGenerator
    {
        #region Class Fields

        /// <summary>
        ///   The default value assigned to <see cref="Alpha"/> if none is specified. 
        /// </summary>
        public const double DefaultAlpha = 0;

        /// <summary>
        ///   The default value assigned to <see cref="Beta"/> if none is specified. 
        /// </summary>
        public const double DefaultBeta = 1;

        #endregion

        #region Instance Fields

        /// <summary>
        ///   Stores the parameter alpha which is used for generation of uniformly distributed random numbers.
        /// </summary>
        double _alpha;

        /// <summary>
        ///   Stores the parameter beta which is used for generation of uniformly distributed random numbers.
        /// </summary>
        double _beta;

        /// <summary>
        ///   Gets or sets the parameter alpha which is used for generation of uniformly distributed random numbers.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value"/> is greater than <see cref="Beta"/>.
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
        ///   Gets or sets the parameter beta which is used for generation of uniformly distributed random numbers.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <see cref="Alpha"/> is greater than <paramref name="value"/>.
        /// </exception>
        /// <remarks>
        ///   Calls <see cref="AreValidParams"/> to determine whether a value is valid and therefore assignable.
        /// </remarks>
        public double Beta
        {
            get { return _beta; }
            set { _beta = value; }
        }

        #endregion

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="ContinuousUniformDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of continuous uniform distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of continuous uniform distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is greater than <paramref name="beta"/>.
        /// </exception>
        public ContinuousUniformDistribution(TGen generator, double alpha, double beta)
            : base(generator)
        {
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(generator, null), ErrorMessages.NullGenerator);
            Contract.Requires<ArgumentOutOfRangeException>(AreValidParams(alpha, beta), ErrorMessages.InvalidParams);
            _alpha = alpha;
            _beta = beta;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Alpha"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>
        ///   <see langword="true"/> if value is less than or equal to <see cref="Beta"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsValidAlpha(double value)
        {
            return AreValidParams(value, _beta);
        }

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Beta"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>
        ///   <see langword="true"/> if value is greater than or equal to <see cref="Alpha"/>;
        ///   otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsValidBeta(double value)
        {
            return AreValidParams(_alpha, value);
        }

        #endregion

        #region IContinuousDistribution Members

        public double Minimum
        {
            get { return _alpha; }
        }

        public double Maximum
        {
            get { return _beta; }
        }

        public double Mean
        {
            get { return _alpha/2.0 + _beta/2.0; }
        }

        public double Median
        {
            get { return _alpha/2.0 + _beta/2.0; }
        }

        public double Variance
        {
            get { return Math.Pow(_beta - _alpha, 2.0)/12.0; }
        }

        public double[] Mode
        {
            get { throw new NotSupportedException(ErrorMessages.UndefinedMode); }
        }

        public double NextDouble()
        {
            return Sample(Gen, _alpha, _beta);
        }

        #endregion

        #region TRandom Helpers

        /// <summary>
        ///   Determines whether continuous uniform distribution is defined under given parameters.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of continuous uniform distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of continuous uniform distributed random numbers.
        /// </param>
        /// <returns>
        ///   True if <paramref name="alpha"/> is less than or equal to <paramref name="beta"/>;
        ///   otherwise, it returns false.
        /// </returns>
        [System.Diagnostics.Contracts.Pure]
        public static bool AreValidParams(double alpha, double beta)
        {
            return alpha <= beta;
        }

        /// <summary>
        ///   Returns a continuous uniform distributed floating point random number.
        /// </summary>
        /// <param name="generator">The generator from which random number are drawn.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of continuous uniform distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of continuous uniform distributed random numbers.
        /// </param>
        /// <returns>
        ///   A continuous uniform distributed floating point random number.
        /// </returns>
        [System.Diagnostics.Contracts.Pure]
        internal static double Sample(TGen generator, double alpha, double beta)
        {
            var helper1 = beta - alpha;
            return alpha + generator.NextDouble()*helper1;
        }

        #endregion
    }

    /// <summary>
    ///   Provides generation of continuous uniformly distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="ContinuousUniformDistribution"/> type bases upon information presented on
    ///   <a href="http://en.wikipedia.org/wiki/Uniform_distribution_%28continuous%29"> 
    ///   Wikipedia - Uniform distribution (continuous)</a>.
    /// </remarks>
    [Serializable]
    public sealed class ContinuousUniformDistribution : ContinuousUniformDistribution<IGenerator>
    {
        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="ContinuousUniformDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        public ContinuousUniformDistribution()
            : base(new XorShift128Generator(), DefaultAlpha, DefaultBeta)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Beta, DefaultBeta));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ContinuousUniformDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        [CLSCompliant(false)]
        public ContinuousUniformDistribution(uint seed)
            : base(new XorShift128Generator(seed), DefaultAlpha, DefaultBeta)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Beta, DefaultBeta));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ContinuousUniformDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        public ContinuousUniformDistribution(IGenerator generator)
            : base(generator, DefaultAlpha, DefaultBeta)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Beta, DefaultBeta));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ContinuousUniformDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of continuous uniform distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of continuous uniform distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is greater than <paramref name="beta"/>.
        /// </exception>
        public ContinuousUniformDistribution(double alpha, double beta)
            : base(new XorShift128Generator(), alpha, beta)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Beta, beta));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ContinuousUniformDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of continuous uniform distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of continuous uniform distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is greater than <paramref name="beta"/>.
        /// </exception>
        [CLSCompliant(false)]
        public ContinuousUniformDistribution(uint seed, double alpha, double beta)
            : base(new XorShift128Generator(seed), alpha, beta)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Beta, beta));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ContinuousUniformDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of continuous uniform distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of continuous uniform distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is greater than <paramref name="beta"/>.
        /// </exception>
        public ContinuousUniformDistribution(IGenerator generator, double alpha, double beta) : base(generator, alpha, beta)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Beta, beta));
        }

        #endregion
    }
}