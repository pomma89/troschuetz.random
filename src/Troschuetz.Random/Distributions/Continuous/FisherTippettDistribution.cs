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
    ///   Provides generation of Fisher-Tippett distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="FisherTippettDistribution"/> type bases upon
    ///   information presented on
    ///   <a href="http://en.wikipedia.org/wiki/Laplace_distribution">Wikipedia - Fisher-Tippett distribution</a>.
    ///
    ///   The thread safety of this class depends on the one of the underlying generator.
    /// </remarks>
    [Serializable]
    public sealed class FisherTippettDistribution : AbstractDistribution, IContinuousDistribution, IAlphaDistribution<double>, IMuDistribution<double>
    {
        #region Constants

        /// <summary>
        ///   The default value assigned to <see cref="Alpha"/> if none is specified.
        /// </summary>
        public const double DefaultAlpha = 1;

        /// <summary>
        ///   The default value assigned to <see cref="Mu"/> if none is specified.
        /// </summary>
        public const double DefaultMu = 0;

        #endregion Constants

        #region Fields

        /// <summary>
        ///   Stores the parameter alpha which is used for generation of fisher tippett distributed
        ///   random numbers.
        /// </summary>
        private double _alpha;

        /// <summary>
        ///   Stores the parameter mu which is used for generation of fisher tippett distributed
        ///   random numbers.
        /// </summary>
        private double _mu;

        /// <summary>
        ///   Gets or sets the parameter alpha which is used for generation of Fisher-Tippett
        ///   distributed random numbers.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value"/> is less than or equal to zero.
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
        ///   Gets or sets the parameter mu which is used for generation of Fisher-Tippett
        ///   distributed random numbers.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value"/> is equal to <see cref="double.NaN"/>.
        /// </exception>
        /// <remarks>
        ///   Call <see cref="AreValidParams"/> to determine whether a value is valid and therefore assignable.
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

        #endregion Fields

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="FisherTippettDistribution"/> class, using
        ///   a <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        public FisherTippettDistribution() : this(new XorShift128Generator(), DefaultAlpha, DefaultMu)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Mu, DefaultMu));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="FisherTippettDistribution"/> class, using
        ///   a <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        public FisherTippettDistribution(uint seed) : this(new XorShift128Generator(seed), DefaultAlpha, DefaultMu)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Mu, DefaultMu));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="FisherTippettDistribution"/> class, using
        ///   the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        public FisherTippettDistribution(IGenerator generator) : this(generator, DefaultAlpha, DefaultMu)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Mu, DefaultMu));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="FisherTippettDistribution"/> class, using
        ///   a <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of fisher tippett distributed random numbers.
        /// </param>
        /// <param name="mu">
        ///   The parameter mu which is used for generation of fisher tippett distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero.
        /// </exception>
        public FisherTippettDistribution(double alpha, double mu) : this(new XorShift128Generator(), alpha, mu)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Mu, mu));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="FisherTippettDistribution"/> class, using
        ///   a <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of fisher tippett distributed random numbers.
        /// </param>
        /// <param name="mu">
        ///   The parameter mu which is used for generation of fisher tippett distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero.
        /// </exception>
        public FisherTippettDistribution(uint seed, double alpha, double mu)
            : this(new XorShift128Generator(seed), alpha, mu)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Mu, mu));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="FisherTippettDistribution"/> class, using
        ///   the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of fisher tippett distributed random numbers.
        /// </param>
        /// <param name="mu">
        ///   The parameter mu which is used for generation of fisher tippett distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero.
        /// </exception>
        public FisherTippettDistribution(IGenerator generator, double alpha, double mu) : base(generator)
        {
            var vp = AreValidParams;
            if (!vp(alpha, mu)) throw new ArgumentOutOfRangeException($"{nameof(alpha)}/{nameof(mu)}", ErrorMessages.InvalidParams);
            _alpha = alpha;
            _mu = mu;
        }

        #endregion Construction

        #region Instance Methods

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Alpha"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns><see langword="true"/> if value is greater than 0.0; otherwise, <see langword="false"/>.</returns>
        public bool IsValidAlpha(double value) => AreValidParams(value, Mu);

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Mu"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns><see langword="true"/>.</returns>
        public bool IsValidMu(double value) => AreValidParams(Alpha, value);

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
        public double Mean => Mu + Alpha * 0.577215664901532860606512090082402431042159335;

        /// <summary>
        ///   Gets the median of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if median is not defined for given distribution with some parameters.
        /// </exception>
        public double Median => Mu - Alpha * Math.Log(Math.Log(2));

        /// <summary>
        ///   Gets the variance of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if variance is not defined for given distribution with some parameters.
        /// </exception>
        public double Variance => TMath.Square(Math.PI) / 6.0 * TMath.Square(Alpha);

        /// <summary>
        ///   Gets the mode of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if mode is not defined for given distribution with some parameters.
        /// </exception>
        public double[] Mode => new[] { Mu };

        /// <summary>
        ///   Returns a distributed floating point random number.
        /// </summary>
        /// <returns>A distributed double-precision floating point number.</returns>
        public double NextDouble() => Sample(Generator, _alpha, _mu);

        #endregion IContinuousDistribution Members

        #region TRandom Helpers

        /// <summary>
        ///   Determines whether fisher tippett distribution is defined under given parameters. The
        ///   default definition returns true if alpha is greater than zero; otherwise, it returns false.
        /// </summary>
        /// <remarks>
        ///   This is an extensibility point for the <see cref="FisherTippettDistribution"/> class.
        /// </remarks>
        public static Func<double, double, bool> AreValidParams { get; set; } = (alpha, mu) =>
        {
            return alpha > 0.0 && !double.IsNaN(mu);
        };

        /// <summary>
        ///   Declares a function returning a fisher tippett distributed floating point random number.
        /// </summary>
        /// <remarks>
        ///   This is an extensibility point for the <see cref="FisherTippettDistribution"/> class.
        /// </remarks>
        public static Func<IGenerator, double, double, double> Sample { get; set; } = (generator, alpha, mu) =>
        {
            return mu - alpha * Math.Log(-Math.Log(1.0 - generator.NextDouble()));
        };

        #endregion TRandom Helpers
    }
}