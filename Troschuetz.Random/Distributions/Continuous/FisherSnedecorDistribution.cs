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
    ///   Provides generation of Fisher-Snedecor distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="FisherSnedecorDistribution"/> type bases upon information presented on
    ///   <a href="http://en.wikipedia.org/wiki/F-distribution">Wikipedia - F-distribution</a>.
    /// </remarks>
    public class FisherSnedecorDistribution<TGen> : Distribution<TGen>, IContinuousDistribution, IAlphaDistribution<int>, IBetaDistribution<int>
        where TGen : IGenerator
    {
        #region Class Fields

        /// <summary>
        ///   The default value assigned to <see cref="Alpha"/> if none is specified. 
        /// </summary>
        public const int DefaultAlpha = 1;

        /// <summary>
        ///   The default value assigned to <see cref="Beta"/> if none is specified. 
        /// </summary>
        public const int DefaultBeta = 1;

        #endregion

        #region Instance Fields

        /// <summary>
        ///   Stores the parameter alpha which is used for generation of Fisher-Snedecor distributed random numbers.
        /// </summary>
        int _alpha;

        /// <summary>
        ///   Stores the parameter beta which is used for generation of Fisher-Snedecor distributed random numbers.
        /// </summary>
        int _beta;

        /// <summary>
        ///   Gets or sets the parameter alpha which is used for generation of Fisher-Snedecor distributed random numbers.
        /// </summary>
        /// <remarks>
        ///   Calls <see cref="AreValidParams"/> to determine whether a value is valid and therefore assignable.
        /// </remarks>
        public int Alpha
        {
            get { return _alpha; }
            set { _alpha = value; }
        }

        /// <summary>
        ///   Gets or sets the parameter beta which is used for generation of Fisher-Snedecor distributed random numbers.
        /// </summary>
        /// <remarks>
        ///   Calls <see cref="AreValidParams"/> to determine whether a value is valid and therefore assignable.
        /// </remarks>
        public int Beta
        {
            get { return _beta; }
            set { _beta = value; }
        }

        #endregion

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="FisherSnedecorDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of fisher snedecor distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of fisher snedecor distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="beta"/> are less than or equal to zero.
        /// </exception>
        public FisherSnedecorDistribution(TGen generator, int alpha, int beta) : base(generator)
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
        ///   <see langword="true"/> if value is greater than 0; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsValidAlpha(int value)
        {
            return AreValidParams(value, _beta);
        }

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Beta"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>
        ///   <see langword="true"/> if value is greater than 0; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsValidBeta(int value)
        {
            return AreValidParams(_alpha, value);
        }

        #endregion

        #region IContinuousDistribution Members

        public double Minimum
        {
            get { return 0.0; }
        }

        public double Maximum
        {
            get { return double.PositiveInfinity; }
        }

        public double Mean
        {
            get
            {
                if (_beta > 2) {
                    return _beta/(_beta - 2.0);
                }
                throw new NotSupportedException(ErrorMessages.UndefinedMeanForParams);
            }
        }

        public double Median
        {
            get { throw new NotSupportedException(ErrorMessages.UndefinedMedian); }
        }

        public double Variance
        {
            get
            {
                if (_beta > 4) {
                    var a = _alpha;
                    var b = _beta;
                    return 2*Math.Pow(_beta, 2.0)*(a + b - 2.0)/a/Math.Pow(_beta - 2.0, 2.0)/(_beta - 4.0);
                }
                throw new NotSupportedException(ErrorMessages.UndefinedVarianceForParams);
            }
        }

        public double[] Mode
        {
            get
            {
                if (_alpha > 2) {
                    var a = _alpha;
                    var b = _beta;
                    return new[] {(a - 2.0)/a*b/(b + 2.0)};
                }
                throw new NotSupportedException(ErrorMessages.UndefinedModeForParams);
            }
        }

        public double NextDouble()
        {
            return Sample(Gen, _alpha, _beta);
        }

        #endregion

        #region TRandom Helpers

        /// <summary>
        ///   Determines whether fisher snedecor distribution is defined under given parameters.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of fisher snedecor distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of fisher snedecor distributed random numbers.
        /// </param>
        /// <returns>
        ///   True if <paramref name="alpha"/> and <paramref name="beta"/> are greater than zero;
        ///   otherwise, it returns false.
        /// </returns>
        [System.Diagnostics.Contracts.Pure]
        public static bool AreValidParams(int alpha, int beta)
        {
            return alpha > 0 && beta > 0;
        }

        /// <summary>
        ///   Returns a fisher snedecor distributed floating point random number.
        /// </summary>
        /// <param name="generator">The generator from which random number are drawn.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of fisher snedecor distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of fisher snedecor distributed random numbers.
        /// </param>
        /// <returns>
        ///   A fisher snedecor distributed floating point random number.
        /// </returns>
        [System.Diagnostics.Contracts.Pure]
        internal static double Sample(TGen generator, int alpha, int beta)
        {
            var helper1 = beta/(double) alpha;
            var csa = ChiSquareDistribution<TGen>.Sample(generator, alpha);
            var csb = ChiSquareDistribution<TGen>.Sample(generator, beta);
            return csa/csb*helper1;
        }

        #endregion
    }

    /// <summary>
    ///   Provides generation of Fisher-Snedecor distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="FisherSnedecorDistribution"/> type bases upon information presented on
    ///   <a href="http://en.wikipedia.org/wiki/F-distribution">Wikipedia - F-distribution</a>.
    /// </remarks>
    public sealed class FisherSnedecorDistribution : FisherSnedecorDistribution<IGenerator>
    {
        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="FisherSnedecorDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        public FisherSnedecorDistribution() : base(new XorShift128Generator(), DefaultAlpha, DefaultBeta)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Beta, DefaultBeta));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="FisherSnedecorDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        [CLSCompliant(false)]
        public FisherSnedecorDistribution(uint seed) : base(new XorShift128Generator(seed), DefaultAlpha, DefaultBeta)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Beta, DefaultBeta));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="FisherSnedecorDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        public FisherSnedecorDistribution(IGenerator generator) : base(generator, DefaultAlpha, DefaultBeta)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Beta, DefaultBeta));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="FisherSnedecorDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of fisher snedecor distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of fisher snedecor distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="beta"/> are less than or equal to zero.
        /// </exception>
        public FisherSnedecorDistribution(int alpha, int beta) : base(new XorShift128Generator(), alpha, beta)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Beta, beta));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="FisherSnedecorDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of fisher snedecor distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of fisher snedecor distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="beta"/> are less than or equal to zero.
        /// </exception>
        [CLSCompliant(false)]
        public FisherSnedecorDistribution(uint seed, int alpha, int beta)
            : base(new XorShift128Generator(seed), alpha, beta)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Beta, beta));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="FisherSnedecorDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of fisher snedecor distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of fisher snedecor distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="beta"/> are less than or equal to zero.
        /// </exception>
        public FisherSnedecorDistribution(IGenerator generator, int alpha, int beta) : base(generator, alpha, beta)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Beta, beta));
        }

        #endregion
    }
}