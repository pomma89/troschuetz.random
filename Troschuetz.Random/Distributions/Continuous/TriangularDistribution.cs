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

#region Original Copyright

/* boost random/triangle_distribution.hpp header file
 *
 * Copyright Jens Maurer 2000-2001
 * Distributed under the Boost Software License, Version 1.0. (See
 * accompanying file LICENSE_1_0.txt or copy at
 * http://www.boost.org/LICENSE_1_0.txt)
 *
 * See http://www.boost.org for most recent version including documentation.
 *
 * $Id: triangle_distribution.hpp,v 1.11 2004/07/27 03:43:32 dgregor Exp $
 *
 * Revision history
 *  2001-02-18  moved to individual header files
 */

#endregion Original Copyright

namespace Troschuetz.Random.Distributions.Continuous
{
    using Core;
    using Generators;
    using PommaLabs.Thrower;
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    /// <summary>
    ///   Provides generation of triangular distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="TriangularDistribution"/> type bases upon information
    ///   presented on <a href="http://en.wikipedia.org/wiki/Triangular_distribution">Wikipedia -
    ///   Triangular distribution</a> and the implementation in the
    ///   <a href="http://www.boost.org/libs/random/index.html">Boost Random Number Library</a>.
    /// </remarks>
    [Serializable]
    public class TriangularDistribution<TGen> : AbstractDistribution<TGen>, IContinuousDistribution, IAlphaDistribution<double>,
                                                       IBetaDistribution<double>, IGammaDistribution<double>
        where TGen : IGenerator
    {
        #region Constants

        /// <summary>
        ///   The default value assigned to <see cref="Alpha"/> if none is specified.
        /// </summary>
        public const double DefaultAlpha = 0;

        /// <summary>
        ///   The default value assigned to <see cref="Beta"/> if none is specified.
        /// </summary>
        public const double DefaultBeta = 1;

        /// <summary>
        ///   The default value assigned to <see cref="Gamma"/> if none is specified.
        /// </summary>
        public const double DefaultGamma = 0.5;

        #endregion Constants

        #region Fields

        /// <summary>
        ///   Stores the parameter alpha which is used for generation of triangular distributed
        ///   random numbers.
        /// </summary>
        double _alpha;

        /// <summary>
        ///   Stores the parameter beta which is used for generation of triangular distributed
        ///   random numbers.
        /// </summary>
        double _beta;

        /// <summary>
        ///   Stores the parameter gamma which is used for generation of triangular distributed
        ///   random numbers.
        /// </summary>
        double _gamma;

        /// <summary>
        ///   Gets or sets the parameter alpha which is used for generation of triangular
        ///   distributed random numbers.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value"/> is greater than or equal to <see cref="Beta"/>, or
        ///   <paramref name="value"/> is greater than <see cref="Gamma"/>.
        /// </exception>
        /// <remarks>
        ///   Calls <see cref="AreValidParams"/> to determine whether a value is valid and therefore assignable.
        /// </remarks>
        public double Alpha
        {
            get { return _alpha; }
            set
            {
                Raise<ArgumentOutOfRangeException>.IfNot(IsValidAlpha(value), ErrorMessages.InvalidParams);
                _alpha = value;
            }
        }

        /// <summary>
        ///   Gets or sets the parameter beta which is used for generation of triangular distributed
        ///   random numbers.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <see cref="Alpha"/> is greater than or equal to <paramref name="value"/>, or
        ///   <paramref name="value"/> is less than <see cref="Gamma"/>.
        /// </exception>
        /// <remarks>
        ///   Calls <see cref="AreValidParams"/> to determine whether a value is valid and therefore assignable.
        /// </remarks>
        public double Beta
        {
            get { return _beta; }
            set
            {
                Raise<ArgumentOutOfRangeException>.IfNot(IsValidBeta(value), ErrorMessages.InvalidParams);
                _beta = value;
            }
        }

        /// <summary>
        ///   Gets or sets the parameter gamma which is used for generation of triangular
        ///   distributed random numbers.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <see cref="Alpha"/> is greater than <paramref name="value"/>, or <see cref="Beta"/> is
        ///   less than <paramref name="value"/>.
        /// </exception>
        /// <remarks>
        ///   Calls <see cref="AreValidParams"/> to determine whether a value is valid and therefore assignable.
        /// </remarks>
        public double Gamma
        {
            get { return _gamma; }
            set
            {
                Raise<ArgumentOutOfRangeException>.IfNot(IsValidGamma(value), ErrorMessages.InvalidParams);
                _gamma = value;
            }
        }

        #endregion Fields

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="TriangularDistribution"/> class, using
        ///   the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of triangular distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of triangular distributed random numbers.
        /// </param>
        /// <param name="gamma">
        ///   The parameter gamma which is used for generation of triangular distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is greater than or equal to <paramref name="beta"/>, or
        ///   <paramref name="alpha"/> is greater than <paramref name="gamma"/>, or
        ///   <paramref name="beta"/> is less than <paramref name="gamma"/>.
        /// </exception>
        public TriangularDistribution(TGen generator, double alpha, double beta, double gamma) : base(generator)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(AreValidParams(alpha, beta, gamma), ErrorMessages.InvalidParams);
            _alpha = alpha;
            _beta = beta;
            _gamma = gamma;
        }

        #endregion Construction

        #region Instance Methods

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Alpha"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>
        ///   <see langword="true"/> if value is less than <see cref="Beta"/>, and less than or
        ///   equal to <see cref="Gamma"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsValidAlpha(double value)
        {
            return AreValidParams(value, _beta, _gamma);
        }

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Beta"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>
        ///   <see langword="true"/> if value is greater than <see cref="Alpha"/>, and greater than
        ///   or equal to <see cref="Gamma"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsValidBeta(double value)
        {
            return AreValidParams(_alpha, value, _gamma);
        }

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Gamma"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>
        ///   <see langword="true"/> if value is greater than or equal to <see cref="Alpha"/>, and
        ///   greater than or equal to <see cref="Beta"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsValidGamma(double value)
        {
            return AreValidParams(_alpha, _beta, value);
        }

        #endregion Instance Methods

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
            get { return _alpha / 3.0 + _beta / 3.0 + _gamma / 3.0; }
        }

        public double Median
        {
            get
            {
                if (_gamma >= (_beta - _alpha) / 2.0)
                {
                    return _alpha + (Math.Sqrt((_beta - _alpha) * (_gamma - _alpha)) / Math.Sqrt(2.0));
                }
                return _beta - (Math.Sqrt((_beta - _alpha) * (_beta - _gamma)) / Math.Sqrt(2.0));
            }
        }

        public double Variance
        {
            get
            {
                return (Math.Pow(_alpha, 2.0) + Math.Pow(_beta, 2.0) + Math.Pow(_gamma, 2.0) - _alpha * _beta -
                        _alpha * _gamma - _beta * _gamma) / 18.0;
            }
        }

        public double[] Mode
        {
            get { return new[] { _gamma }; }
        }

        public double NextDouble()
        {
            return Sample(TypedGenerator, _alpha, _beta, _gamma);
        }

        #endregion IContinuousDistribution Members

        #region TRandom Helpers

        /// <summary>
        ///   Determines whether triangular distribution is defined under given parameters.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of triangular distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of triangular distributed random numbers.
        /// </param>
        /// <param name="gamma">
        ///   The parameter gamma which is used for generation of triangular distributed random numbers.
        /// </param>
        /// <returns>
        ///   True if <paramref name="alpha"/> is less than <paramref name="beta"/>, and if
        ///   <paramref name="alpha"/> is less than or equal to <paramref name="gamma"/>, and if
        ///   <paramref name="beta"/> is greater than or equal to <paramref name="gamma"/>;
        ///   otherwise, it returns false.
        /// </returns>
        [Pure]
        public static bool AreValidParams(double alpha, double beta, double gamma)
        {
            return alpha < beta && alpha <= gamma && beta >= gamma;
        }

        /// <summary>
        ///   Returns a triangular distributed floating point random number.
        /// </summary>
        /// <param name="generator">The generator from which random number are drawn.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of triangular distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of triangular distributed random numbers.
        /// </param>
        /// <param name="gamma">
        ///   The parameter gamma which is used for generation of triangular distributed random numbers.
        /// </param>
        /// <returns>A triangular distributed floating point random number.</returns>
        [Pure]
        internal static double Sample(TGen generator, double alpha, double beta, double gamma)
        {
            var helper1 = gamma - alpha;
            var helper2 = beta - alpha;
            var helper3 = Math.Sqrt(helper1 * helper2);
            var helper4 = Math.Sqrt(beta - gamma);
            var genNum = generator.NextDouble();
            if (genNum <= helper1 / helper2)
            {
                return alpha + Math.Sqrt(genNum) * helper3;
            }
            return beta - Math.Sqrt(genNum * helper2 - helper1) * helper4;
        }

        #endregion TRandom Helpers
    }

    /// <summary>
    ///   Provides generation of triangular distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="TriangularDistribution"/> type bases upon information
    ///   presented on <a href="http://en.wikipedia.org/wiki/Triangular_distribution">Wikipedia -
    ///   Triangular distribution</a> and the implementation in the
    ///   <a href="http://www.boost.org/libs/random/index.html">Boost Random Number Library</a>.
    /// </remarks>
    [Serializable]
    public sealed class TriangularDistribution : TriangularDistribution<IGenerator>
    {
        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="TriangularDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        public TriangularDistribution() : base(new XorShift128Generator(), DefaultAlpha, DefaultBeta, DefaultGamma)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Beta, DefaultBeta));
            Debug.Assert(Equals(Gamma, DefaultGamma));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="TriangularDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        public TriangularDistribution(uint seed)
            : base(new XorShift128Generator(seed), DefaultAlpha, DefaultBeta, DefaultGamma)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Beta, DefaultBeta));
            Debug.Assert(Equals(Gamma, DefaultGamma));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="TriangularDistribution"/> class, using
        ///   the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        public TriangularDistribution(IGenerator generator) : base(generator, DefaultAlpha, DefaultBeta, DefaultGamma)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Beta, DefaultBeta));
            Debug.Assert(Equals(Gamma, DefaultGamma));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="TriangularDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of triangular distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of triangular distributed random numbers.
        /// </param>
        /// <param name="gamma">
        ///   The parameter gamma which is used for generation of triangular distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is greater than or equal to <paramref name="beta"/>, or
        ///   <paramref name="alpha"/> is greater than <paramref name="gamma"/>, or
        ///   <paramref name="beta"/> is less than <paramref name="gamma"/>.
        /// </exception>
        public TriangularDistribution(double alpha, double beta, double gamma)
            : base(new XorShift128Generator(), alpha, beta, gamma)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Beta, beta));
            Debug.Assert(Equals(Gamma, gamma));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="TriangularDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of triangular distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of triangular distributed random numbers.
        /// </param>
        /// <param name="gamma">
        ///   The parameter gamma which is used for generation of triangular distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is greater than or equal to <paramref name="beta"/>, or
        ///   <paramref name="alpha"/> is greater than <paramref name="gamma"/>, or
        ///   <paramref name="beta"/> is less than <paramref name="gamma"/>.
        /// </exception>
        public TriangularDistribution(uint seed, double alpha, double beta, double gamma)
            : base(new XorShift128Generator(seed), alpha, beta, gamma)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Beta, beta));
            Debug.Assert(Equals(Gamma, gamma));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="TriangularDistribution"/> class, using
        ///   the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of triangular distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of triangular distributed random numbers.
        /// </param>
        /// <param name="gamma">
        ///   The parameter gamma which is used for generation of triangular distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is greater than or equal to <paramref name="beta"/>, or
        ///   <paramref name="alpha"/> is greater than <paramref name="gamma"/>, or
        ///   <paramref name="beta"/> is less than <paramref name="gamma"/>.
        /// </exception>
        public TriangularDistribution(IGenerator generator, double alpha, double beta, double gamma) : base(generator, alpha, beta, gamma)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Beta, beta));
            Debug.Assert(Equals(Gamma, gamma));
        }

        #endregion Construction
    }
}
