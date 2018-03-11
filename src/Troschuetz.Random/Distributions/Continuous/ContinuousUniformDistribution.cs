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
    ///   Provides generation of continuous uniformly distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="ContinuousUniformDistribution"/> type bases upon
    ///   information presented on
    ///   <a href="http://en.wikipedia.org/wiki/Uniform_distribution_%28continuous%29">Wikipedia -
    ///   Uniform distribution (continuous)</a>.
    ///
    ///   The thread safety of this class depends on the one of the underlying generator.
    /// </remarks>
    [Serializable]
    public sealed class ContinuousUniformDistribution : AbstractDistribution, IContinuousDistribution, IAlphaDistribution<double>, IBetaDistribution<double>
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

        #endregion Constants

        #region Fields

        /// <summary>
        ///   Stores the parameter alpha which is used for generation of uniformly distributed random numbers.
        /// </summary>
        private double _alpha;

        /// <summary>
        ///   Stores the parameter beta which is used for generation of uniformly distributed random numbers.
        /// </summary>
        private double _beta;

        /// <summary>
        ///   Gets or sets the parameter alpha which is used for generation of uniformly distributed
        ///   random numbers.
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
            set
            {
                if (!IsValidAlpha(value)) throw new ArgumentOutOfRangeException(nameof(Alpha), ErrorMessages.InvalidParams);
                _alpha = value;
            }
        }

        /// <summary>
        ///   Gets or sets the parameter beta which is used for generation of uniformly distributed
        ///   random numbers.
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
            set
            {
                if (!IsValidBeta(value)) throw new ArgumentOutOfRangeException(nameof(Beta), ErrorMessages.InvalidParams);
                _beta = value;
            }
        }

        #endregion Fields

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="ContinuousUniformDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        public ContinuousUniformDistribution()
            : this(new XorShift128Generator(), DefaultAlpha, DefaultBeta)
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
        public ContinuousUniformDistribution(uint seed)
            : this(new XorShift128Generator(seed), DefaultAlpha, DefaultBeta)
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
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        public ContinuousUniformDistribution(IGenerator generator)
            : this(generator, DefaultAlpha, DefaultBeta)
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
        ///   The parameter alpha which is used for generation of continuous uniform distributed
        ///   random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of continuous uniform distributed
        ///   random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is greater than <paramref name="beta"/>.
        /// </exception>
        public ContinuousUniformDistribution(double alpha, double beta)
            : this(new XorShift128Generator(), alpha, beta)
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
        ///   The parameter alpha which is used for generation of continuous uniform distributed
        ///   random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of continuous uniform distributed
        ///   random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is greater than <paramref name="beta"/>.
        /// </exception>
        public ContinuousUniformDistribution(uint seed, double alpha, double beta)
            : this(new XorShift128Generator(seed), alpha, beta)
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
        ///   The parameter alpha which is used for generation of continuous uniform distributed
        ///   random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of continuous uniform distributed
        ///   random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is greater than <paramref name="beta"/>.
        /// </exception>
        public ContinuousUniformDistribution(IGenerator generator, double alpha, double beta)
            : base(generator)
        {
            var vp = AreValidParams;
            if (!vp(alpha, beta)) throw new ArgumentOutOfRangeException($"{nameof(alpha)}/{nameof(beta)}", ErrorMessages.InvalidParams);
            _alpha = alpha;
            _beta = beta;
        }

        #endregion Construction

        #region Instance Methods

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Alpha"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>
        ///   <see langword="true"/> if value is less than or equal to <see cref="Beta"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsValidAlpha(double value) => AreValidParams(value, _beta);

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Beta"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>
        ///   <see langword="true"/> if value is greater than or equal to <see cref="Alpha"/>;
        ///   otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsValidBeta(double value) => AreValidParams(_alpha, value);

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
        public double Mean => _alpha / 2.0 + _beta / 2.0;

        /// <summary>
        ///   Gets the median of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if median is not defined for given distribution with some parameters.
        /// </exception>
        public double Median => _alpha / 2.0 + _beta / 2.0;

        /// <summary>
        ///   Gets the variance of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if variance is not defined for given distribution with some parameters.
        /// </exception>
        public double Variance => TMath.Square(_beta - _alpha) / 12.0;

        /// <summary>
        ///   Gets the mode of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if mode is not defined for given distribution with some parameters.
        /// </exception>
        public double[] Mode
        {
            get { throw new NotSupportedException(ErrorMessages.UndefinedMode); }
        }

        /// <summary>
        ///   Returns a distributed floating point random number.
        /// </summary>
        /// <returns>A distributed double-precision floating point number.</returns>
        public double NextDouble() => Sample(Generator, _alpha, _beta);

        #endregion IContinuousDistribution Members

        #region TRandom Helpers

        /// <summary>
        ///   Determines whether continuous uniform distribution is defined under given parameters.
        ///   The default definition returns true if alpha is less than or equal to beta; otherwise,
        ///   it returns false.
        /// </summary>
        /// <remarks>
        ///   This is an extensibility point for the <see cref="ContinuousUniformDistribution"/> class.
        /// </remarks>
        public static Func<double, double, bool> AreValidParams { get; set; } = (alpha, beta) =>
        {
            return alpha <= beta;
        };

        /// <summary>
        ///   Declares a function returning a continuous uniform distributed floating point random number.
        /// </summary>
        /// <remarks>
        ///   This is an extensibility point for the <see cref="ContinuousUniformDistribution"/> class.
        /// </remarks>
        public static Func<IGenerator, double, double, double> Sample { get; set; } = (generator, alpha, beta) =>
        {
            return generator.NextDouble(alpha, beta);
        };

        #endregion TRandom Helpers
    }
}