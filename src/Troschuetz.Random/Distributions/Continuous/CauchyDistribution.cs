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

namespace Troschuetz.Random.Distributions.Continuous
{
    using Core;
    using Generators;
    using System;
    using System.Diagnostics;

    /// <summary>
    ///   Provides generation of cauchy distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="CauchyDistribution"/> type bases upon information
    ///   presented on <a href="http://en.wikipedia.org/wiki/Cauchy_distribution">Wikipedia - Cauchy
    ///   distribution</a> and <a href="http://www.xycoon.com/cauchy2p_random.htm">Xycoon - Cauchy Distribution</a>.
    ///
    ///   The thread safety of this class depends on the one of the underlying generator.
    /// </remarks>
    [Serializable]
    public sealed class CauchyDistribution : AbstractDistribution, IContinuousDistribution, IAlphaDistribution<double>, IGammaDistribution<double>
    {
        #region Constants

        /// <summary>
        ///   The default value assigned to <see cref="Alpha"/> if none is specified.
        /// </summary>
        public const double DefaultAlpha = 1;

        /// <summary>
        ///   The default value assigned to <see cref="Gamma"/> if none is specified.
        /// </summary>
        public const double DefaultGamma = 1;

        #endregion Constants

        #region Fields

        /// <summary>
        ///   Stores the parameter alpha which is used for generation of cauchy distributed random numbers.
        /// </summary>
        private double _alpha;

        /// <summary>
        ///   Stores the parameter gamma which is used for generation of cauchy distributed random numbers.
        /// </summary>
        private double _gamma;

        /// <summary>
        ///   Gets or sets the parameter alpha of cauchy distributed random numbers which is used for
        ///   their generation.
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
            set
            {
                if (!IsValidAlpha(value)) throw new ArgumentOutOfRangeException(nameof(Alpha), ErrorMessages.InvalidParams);
                _alpha = value;
            }
        }

        /// <summary>
        ///   Gets or sets the parameter gamma which is used for generation of cauchy distributed
        ///   random numbers.
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
            set
            {
                if (!IsValidGamma(value)) throw new ArgumentOutOfRangeException(nameof(Gamma), ErrorMessages.InvalidParams);
                _gamma = value;
            }
        }

        #endregion Fields

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="CauchyDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        public CauchyDistribution()
            : this(new XorShift128Generator(), DefaultAlpha, DefaultGamma)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Gamma, DefaultGamma));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="CauchyDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        public CauchyDistribution(uint seed)
            : this(new XorShift128Generator(seed), DefaultAlpha, DefaultGamma)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Gamma, DefaultGamma));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="CauchyDistribution"/> class, using the
        ///   specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        public CauchyDistribution(IGenerator generator)
            : this(generator, DefaultAlpha, DefaultGamma)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Gamma, DefaultGamma));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="CauchyDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> as underlying random number generator.
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
            : this(new XorShift128Generator(), alpha, gamma)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Gamma, gamma));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="CauchyDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> with the specified seed value.
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
        public CauchyDistribution(uint seed, double alpha, double gamma)
            : this(new XorShift128Generator(seed), alpha, gamma)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Gamma, gamma));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="CauchyDistribution"/> class, using the
        ///   specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of cauchy distributed random numbers.
        /// </param>
        /// <param name="gamma">
        ///   The parameter gamma which is used for generation of cauchy distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="gamma"/> is less than or equal to zero.
        /// </exception>
        public CauchyDistribution(IGenerator generator, double alpha, double gamma)
            : base(generator)
        {
            var vp = AreValidParams;
            if (!vp(alpha, gamma)) throw new ArgumentOutOfRangeException($"{nameof(alpha)}/{nameof(gamma)}", ErrorMessages.InvalidParams);
            _alpha = alpha;
            _gamma = gamma;
        }

        #endregion Construction

        #region Instance Methods

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Alpha"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns><see langword="true"/>.</returns>
        public bool IsValidAlpha(double value) => AreValidParams(value, _gamma);

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Gamma"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns><see langword="true"/> if value is greater than 0; otherwise, <see langword="false"/>.</returns>
        public bool IsValidGamma(double value) => AreValidParams(_alpha, value);

        #endregion Instance Methods

        #region IContinuousDistribution Members

        /// <summary>
        ///   Gets the minimum possible value of distributed random numbers.
        /// </summary>
        public double Minimum => double.NegativeInfinity;

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
        public double Mean
        {
            get { throw new NotSupportedException(ErrorMessages.UndefinedMean); }
        }

        /// <summary>
        ///   Gets the median of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if median is not defined for given distribution with some parameters.
        /// </exception>
        public double Median => Alpha;

        /// <summary>
        ///   Gets the variance of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if variance is not defined for given distribution with some parameters.
        /// </exception>
        public double Variance
        {
            get { throw new NotSupportedException(ErrorMessages.UndefinedVariance); }
        }

        /// <summary>
        ///   Gets the mode of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if mode is not defined for given distribution with some parameters.
        /// </exception>
        public double[] Mode => new[] { Alpha };

        /// <summary>
        ///   Returns a distributed floating point random number.
        /// </summary>
        /// <returns>A distributed double-precision floating point number.</returns>
        public double NextDouble() => Sample(Generator, _alpha, _gamma);

        #endregion IContinuousDistribution Members

        #region TRandom Helpers

        /// <summary>
        ///   Determines whether cauchy distribution is defined under given parameters. The default
        ///   definition returns true if gamma is greater than zero; otherwise, it returns false.
        /// </summary>
        /// <remarks>
        ///   This is an extensibility point for the <see cref="CauchyDistribution"/> class.
        /// </remarks>
        public static Func<double, double, bool> AreValidParams { get; set; } = (alpha, gamma) =>
        {
            return !double.IsNaN(alpha) && gamma > 0.0;
        };

        /// <summary>
        ///   Declares a function returning a cauchy distributed floating point random number.
        /// </summary>
        /// <remarks>
        ///   This is an extensibility point for the <see cref="CauchyDistribution"/> class.
        /// </remarks>
        public static Func<IGenerator, double, double, double> Sample { get; set; } = (generator, alpha, gamma) =>
        {
            return alpha + gamma * Math.Tan(Math.PI * (generator.NextDouble() - 0.5));
        };

        #endregion TRandom Helpers
    }
}