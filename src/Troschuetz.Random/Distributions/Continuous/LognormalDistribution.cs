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

/* boost random/lognormal_distribution.hpp header file
 *
 * Copyright Jens Maurer 2000-2001
 * Distributed under the Boost Software License, Version 1.0. (See
 * accompanying file LICENSE_1_0.txt or copy at
 * http://www.boost.org/LICENSE_1_0.txt)
 *
 * See http://www.boost.org for most recent version including documentation.
 *
 * $Id: lognormal_distribution.hpp,v 1.16 2004/07/27 03:43:32 dgregor Exp $
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
    ///   Provides generation of lognormal distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="LognormalDistribution"/> type bases upon information
    ///   presented on <a href="http://en.wikipedia.org/wiki/Log-normal_distribution">Wikipedia -
    ///   Lognormal Distribution</a> and the implementation in the
    ///   <a href="http://www.boost.org/libs/random/index.html">Boost Random Number Library</a>.
    ///
    ///   The thread safety of this class depends on the one of the underlying generator.
    /// </remarks>
    [Serializable]
    public sealed class LognormalDistribution : AbstractDistribution, IContinuousDistribution, IMuDistribution<double>, ISigmaDistribution<double>
    {
        #region Constants

        /// <summary>
        ///   The default value assigned to <see cref="Mu"/> if none is specified.
        /// </summary>
        public const double DefaultMu = 1;

        /// <summary>
        ///   The default value assigned to <see cref="Sigma"/> if none is specified.
        /// </summary>
        public const double DefaultSigma = 1;

        #endregion Constants

        #region Fields

        /// <summary>
        ///   Stores the parameter mu which is used for generation of lognormal distributed random numbers.
        /// </summary>
        private double _mu;

        /// <summary>
        ///   Stores the parameter sigma which is used for generation of lognormal distributed random numbers.
        /// </summary>
        private double _sigma;

        /// <summary>
        ///   Gets or sets the parameter mu which is used for generation of lognormal distributed
        ///   random numbers.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value"/> is equal to <see cref="double.NaN"/>.
        /// </exception>
        /// <remarks>
        ///   Calls <see cref="AreValidParams"/> to determine whether a value is valid and therefore assignable.
        /// </remarks>
        public double Mu
        {
            get { return _mu; }
            set
            {
                if (!IsValidMu(value)) throw new ArgumentOutOfRangeException(nameof(Mu), ErrorMessages.InvalidParams);
                _mu = value;
            }
        }

        /// <summary>
        ///   Gets or sets the parameter sigma which is used for generation of lognormal distributed
        ///   random numbers.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value"/> is less than zero.
        /// </exception>
        /// <remarks>
        ///   Calls <see cref="AreValidParams"/> to determine whether a value is valid and therefore assignable.
        /// </remarks>
        public double Sigma
        {
            get { return _sigma; }
            set
            {
                if (!IsValidSigma(value)) throw new ArgumentOutOfRangeException(nameof(Sigma), ErrorMessages.InvalidParams);
                _sigma = value;
            }
        }

        #endregion Fields

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="LognormalDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        public LognormalDistribution() : this(new XorShift128Generator(), DefaultMu, DefaultSigma)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Mu, DefaultMu));
            Debug.Assert(Equals(Sigma, DefaultSigma));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="LognormalDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        public LognormalDistribution(uint seed) : this(new XorShift128Generator(seed), DefaultMu, DefaultSigma)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Mu, DefaultMu));
            Debug.Assert(Equals(Sigma, DefaultSigma));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="LognormalDistribution"/> class, using the
        ///   specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        public LognormalDistribution(IGenerator generator) : this(generator, DefaultMu, DefaultSigma)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Mu, DefaultMu));
            Debug.Assert(Equals(Sigma, DefaultSigma));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="LognormalDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="mu">
        ///   The parameter mu which is used for generation of lognormal distributed random numbers.
        /// </param>
        /// <param name="sigma">
        ///   The parameter sigma which is used for generation of lognormal distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="sigma"/> is less than zero.
        /// </exception>
        public LognormalDistribution(double mu, double sigma) : this(new XorShift128Generator(), mu, sigma)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Mu, mu));
            Debug.Assert(Equals(Sigma, sigma));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="LognormalDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        /// <param name="mu">
        ///   The parameter mu which is used for generation of lognormal distributed random numbers.
        /// </param>
        /// <param name="sigma">
        ///   The parameter sigma which is used for generation of lognormal distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="sigma"/> is less than zero.
        /// </exception>
        public LognormalDistribution(uint seed, double mu, double sigma)
            : this(new XorShift128Generator(seed), mu, sigma)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Mu, mu));
            Debug.Assert(Equals(Sigma, sigma));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="LognormalDistribution"/> class, using the
        ///   specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="mu">
        ///   The parameter mu which is used for generation of lognormal distributed random numbers.
        /// </param>
        /// <param name="sigma">
        ///   The parameter sigma which is used for generation of lognormal distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="sigma"/> is less than zero.
        /// </exception>
        public LognormalDistribution(IGenerator generator, double mu, double sigma) : base(generator)
        {
            var vp = AreValidParams;
            if (!vp(mu, sigma)) throw new ArgumentOutOfRangeException($"{nameof(mu)}/{nameof(sigma)}", ErrorMessages.InvalidParams);
            _mu = mu;
            _sigma = sigma;
        }

        #endregion Construction

        #region Instance Methods

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Mu"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns><see langword="true"/>.</returns>
        public bool IsValidMu(double value) => AreValidParams(value, Sigma);

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Sigma"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>
        ///   <see langword="true"/> if value is greater than or equal to 0.0; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsValidSigma(double value) => AreValidParams(Mu, value);

        #endregion Instance Methods

        #region IContinuousDistribution Members

        /// <summary>
        ///   Gets the minimum possible value of distributed random numbers.
        /// </summary>
        public double Minimum => 0.0;

        /// <summary>
        ///   Gets the maximum possible value of distributed random numbers.
        /// </summary>
        public double Maximum => double.PositiveInfinity;

        /// <summary>
        ///   Gets the mean of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if mean is not defined for given distribution with some parameters.
        /// </exception>
        public double Mean => Math.Exp(Mu + 0.5 * TMath.Square(Sigma));

        /// <summary>
        ///   Gets the median of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if median is not defined for given distribution with some parameters.
        /// </exception>
        public double Median => Math.Exp(Mu);

        /// <summary>
        ///   Gets the variance of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if variance is not defined for given distribution with some parameters.
        /// </exception>
        public double Variance => (Math.Exp(TMath.Square(Sigma)) - 1.0) * Math.Exp(2.0 * Mu + TMath.Square(Sigma));

        /// <summary>
        ///   Gets the mode of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if mode is not defined for given distribution with some parameters.
        /// </exception>
        public double[] Mode => new[] { Math.Exp(Mu - TMath.Square(Sigma)) };

        /// <summary>
        ///   Returns a distributed floating point random number.
        /// </summary>
        /// <returns>A distributed double-precision floating point number.</returns>
        public double NextDouble() => Sample(Generator, _mu, _sigma);

        #endregion IContinuousDistribution Members

        #region TRandom Helpers

        /// <summary>
        ///   Determines whether lognormal distribution is defined under given parameters. The
        ///   default definition returns true if sigma is greater than or equal to zero; otherwise,
        ///   it returns false.
        /// </summary>
        /// <remarks>
        ///   This is an extensibility point for the <see cref="LognormalDistribution"/> class.
        /// </remarks>
        public static Func<double, double, bool> AreValidParams { get; set; } = (mu, sigma) =>
        {
            return !double.IsNaN(mu) && sigma >= 0.0;
        };

        /// <summary>
        ///   Declares a function returning a lognormal distributed floating point random number.
        /// </summary>
        /// <remarks>
        ///   This is an extensibility point for the <see cref="LognormalDistribution"/> class.
        /// </remarks>
        public static Func<IGenerator, double, double, double> Sample { get; set; } = (generator, mu, sigma) =>
        {
            const double nm = 0.0;
            const double ns = 1.0;
            return Math.Exp(NormalDistribution.Sample(generator, nm, ns) * sigma + mu);
        };

        #endregion TRandom Helpers
    }
}