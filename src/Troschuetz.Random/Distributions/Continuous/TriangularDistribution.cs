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
    using System;
    using System.Diagnostics;

    /// <summary>
    ///   Provides generation of triangular distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="TriangularDistribution"/> type bases upon information
    ///   presented on <a href="http://en.wikipedia.org/wiki/Triangular_distribution">Wikipedia -
    ///   Triangular distribution</a> and the implementation in the
    ///   <a href="http://www.boost.org/libs/random/index.html">Boost Random Number Library</a>.
    ///
    ///   The thread safety of this class depends on the one of the underlying generator.
    /// </remarks>
    [Serializable]
    public sealed class TriangularDistribution : AbstractDistribution, IContinuousDistribution, IAlphaDistribution<double>, IBetaDistribution<double>, IGammaDistribution<double>
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
        private double _alpha;

        /// <summary>
        ///   Stores the parameter beta which is used for generation of triangular distributed random numbers.
        /// </summary>
        private double _beta;

        /// <summary>
        ///   Stores the parameter gamma which is used for generation of triangular distributed
        ///   random numbers.
        /// </summary>
        private double _gamma;

        /// <summary>
        ///   Gets or sets the parameter alpha which is used for generation of triangular distributed
        ///   random numbers.
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
                if (!IsValidAlpha(value)) throw new ArgumentOutOfRangeException(nameof(Alpha), ErrorMessages.InvalidParams);
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
                if (!IsValidBeta(value)) throw new ArgumentOutOfRangeException(nameof(Beta), ErrorMessages.InvalidParams);
                _beta = value;
            }
        }

        /// <summary>
        ///   Gets or sets the parameter gamma which is used for generation of triangular distributed
        ///   random numbers.
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
                if (!IsValidGamma(value)) throw new ArgumentOutOfRangeException(nameof(Gamma), ErrorMessages.InvalidParams);
                _gamma = value;
            }
        }

        #endregion Fields

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="TriangularDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        public TriangularDistribution() : this(new XorShift128Generator(), DefaultAlpha, DefaultBeta, DefaultGamma)
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
            : this(new XorShift128Generator(seed), DefaultAlpha, DefaultBeta, DefaultGamma)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Beta, DefaultBeta));
            Debug.Assert(Equals(Gamma, DefaultGamma));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="TriangularDistribution"/> class, using the
        ///   specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        public TriangularDistribution(IGenerator generator) : this(generator, DefaultAlpha, DefaultBeta, DefaultGamma)
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
            : this(new XorShift128Generator(), alpha, beta, gamma)
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
            : this(new XorShift128Generator(seed), alpha, beta, gamma)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Beta, beta));
            Debug.Assert(Equals(Gamma, gamma));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="TriangularDistribution"/> class, using the
        ///   specified <see cref="IGenerator"/> as underlying random number generator.
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
        public TriangularDistribution(IGenerator generator, double alpha, double beta, double gamma) : base(generator)
        {
            var vp = AreValidParams;
            if (!vp(alpha, beta, gamma)) throw new ArgumentOutOfRangeException($"{nameof(alpha)}/{nameof(beta)}/{gamma}", ErrorMessages.InvalidParams);
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
        ///   <see langword="true"/> if value is less than <see cref="Beta"/>, and less than or equal
        ///   to <see cref="Gamma"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsValidAlpha(double value) => AreValidParams(value, _beta, _gamma);

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Beta"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>
        ///   <see langword="true"/> if value is greater than <see cref="Alpha"/>, and greater than
        ///   or equal to <see cref="Gamma"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsValidBeta(double value) => AreValidParams(_alpha, value, _gamma);

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Gamma"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>
        ///   <see langword="true"/> if value is greater than or equal to <see cref="Alpha"/>, and
        ///   greater than or equal to <see cref="Beta"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsValidGamma(double value) => AreValidParams(_alpha, _beta, value);

        #endregion Instance Methods

        #region IContinuousDistribution Members

        /// <summary>
        ///   Gets the minimum possible value of distributed random numbers.
        /// </summary>
        public double Minimum => _alpha;

        /// <summary>
        ///   Gets the maximum possible value of distributed random numbers.
        /// </summary>
        public double Maximum => _beta;

        /// <summary>
        ///   Gets the mean of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if mean is not defined for given distribution with some parameters.
        /// </exception>
        public double Mean => _alpha / 3.0 + _beta / 3.0 + _gamma / 3.0;

        /// <summary>
        ///   Gets the median of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if median is not defined for given distribution with some parameters.
        /// </exception>
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

        /// <summary>
        ///   Gets the variance of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if variance is not defined for given distribution with some parameters.
        /// </exception>
        public double Variance => (TMath.Square(_alpha) + TMath.Square(_beta) + TMath.Square(_gamma) - _alpha * _beta -
                        _alpha * _gamma - _beta * _gamma) / 18.0;

        /// <summary>
        ///   Gets the mode of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if mode is not defined for given distribution with some parameters.
        /// </exception>
        public double[] Mode => new[] { _gamma };

        /// <summary>
        ///   Returns a distributed floating point random number.
        /// </summary>
        /// <returns>A distributed double-precision floating point number.</returns>
        public double NextDouble() => Sample(Generator, _alpha, _beta, _gamma);

        #endregion IContinuousDistribution Members

        #region TRandom Helpers

        /// <summary>
        ///   Determines whether triangular distribution is defined under given parameters. The
        ///   default definition returns true if alpha is less than beta, and if alpha is less than
        ///   or equal to gamma, and if beta is greater than or equal to gamma; otherwise, it returns false.
        /// </summary>
        /// <remarks>
        ///   This is an extensibility point for the <see cref="TriangularDistribution"/> class.
        /// </remarks>
        public static Func<double, double, double, bool> AreValidParams { get; set; } = (alpha, beta, gamma) =>
        {
            return alpha < beta && alpha <= gamma && beta >= gamma;
        };

        /// <summary>
        ///   Declares a function returning a triangular distributed floating point random number.
        /// </summary>
        /// <remarks>
        ///   This is an extensibility point for the <see cref="TriangularDistribution"/> class.
        /// </remarks>
        public static Func<IGenerator, double, double, double, double> Sample { get; set; } = (generator, alpha, beta, gamma) =>
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
        };

        #endregion TRandom Helpers
    }
}