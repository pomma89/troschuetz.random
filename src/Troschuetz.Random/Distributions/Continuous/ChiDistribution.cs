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
    ///   Provides generation of chi distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="ChiDistribution"/> type bases upon information
    ///   presented on <a href="http://en.wikipedia.org/wiki/Chi_distribution">Wikipedia - Chi distribution</a>.
    ///
    ///   The thread safety of this class depends on the one of the underlying generator.
    /// </remarks>
    [Serializable]
    public sealed class ChiDistribution : AbstractDistribution, IContinuousDistribution, IAlphaDistribution<int>
    {
        #region Constants

        /// <summary>
        ///   The default value assigned to <see cref="Alpha"/> if none is specified.
        /// </summary>
        public const int DefaultAlpha = 1;

        /// <summary>
        ///   Represents coefficients for the Lanczos approximation of the Gamma function.
        /// </summary>
        private static readonly double[] LanczosCoefficients = {
            1.000000000190015, 76.18009172947146, -86.50532032941677,
            24.01409824083091, -1.231739572450155, 1.208650973866179e-3,
            -5.395239384953e-6
        };

        #endregion Constants

        #region Fields

        /// <summary>
        ///   Stores the parameter alpha which is used for generation of chi distributed random numbers.
        /// </summary>
        private int _alpha;

        /// <summary>
        ///   Gets or sets the parameter alpha which is used for generation of chi distributed random numbers.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   Calls <see cref="IsValidParam"/> to determine whether a value is valid and therefore assignable.
        /// </remarks>
        public int Alpha
        {
            get { return _alpha; }
            set
            {
                if (!IsValidAlpha(value)) throw new ArgumentOutOfRangeException(nameof(Alpha), ErrorMessages.InvalidParams);
                _alpha = value;
            }
        }

        #endregion Fields

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="ChiDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        public ChiDistribution()
            : this(new XorShift128Generator(), DefaultAlpha)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ChiDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        public ChiDistribution(uint seed)
            : this(new XorShift128Generator(seed), DefaultAlpha)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ChiDistribution"/> class, using the
        ///   specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        public ChiDistribution(IGenerator generator)
            : this(generator, DefaultAlpha)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Alpha, DefaultAlpha));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ChiDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of chi distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero.
        /// </exception>
        public ChiDistribution(int alpha)
            : this(new XorShift128Generator(), alpha)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, alpha));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ChiDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of chi distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero.
        /// </exception>
        public ChiDistribution(uint seed, int alpha)
            : this(new XorShift128Generator(seed), alpha)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, alpha));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ChiDistribution"/> class, using the
        ///   specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of chi distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero.
        /// </exception>
        public ChiDistribution(IGenerator generator, int alpha)
            : base(generator)
        {
            var vp = IsValidParam;
            if (!vp(alpha)) throw new ArgumentOutOfRangeException(nameof(alpha), ErrorMessages.InvalidParams);
            _alpha = alpha;
        }

        #endregion Construction

        #region Instance Methods

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Alpha"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns><see langword="true"/> if value is greater than 0; otherwise, <see langword="false"/>.</returns>
        public bool IsValidAlpha(int value) => IsValidParam(value);

        /// <summary>
        ///   Represents a Lanczos approximation of the Gamma function.
        /// </summary>
        /// <param name="x">A double-precision floating point number.</param>
        /// <returns>
        ///   A double-precision floating point number representing an approximation of Gamma( <paramref name="x"/>).
        /// </returns>
        private static double Gamma(double x)
        {
            var sum = LanczosCoefficients[0];
            for (var index = 1; index <= 6; index++)
            {
                sum += LanczosCoefficients[index] / (x + index);
            }

            return Math.Sqrt(2.0 * Math.PI) / x * Math.Pow(x + 5.5, x + 0.5) / Math.Exp(x + 5.5) * sum;
        }

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
        public double Mean => Math.Sqrt(2.0) * Gamma((Alpha + 1.0) / 2.0) / Gamma(Alpha / 2.0);

        /// <summary>
        ///   Gets the median of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if median is not defined for given distribution with some parameters.
        /// </exception>
        public double Median
        {
            get { throw new NotSupportedException(ErrorMessages.UndefinedMedian); }
        }

        /// <summary>
        ///   Gets the variance of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if variance is not defined for given distribution with some parameters.
        /// </exception>
        public double Variance => Alpha - TMath.Square(Mean);

        /// <summary>
        ///   Gets the mode of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if mode is not defined for given distribution with some parameters.
        /// </exception>
        public double[] Mode => new[] { Math.Sqrt(Alpha - 1.0) };

        /// <summary>
        ///   Returns a distributed floating point random number.
        /// </summary>
        /// <returns>A distributed double-precision floating point number.</returns>
        public double NextDouble() => Sample(Generator, _alpha);

        #endregion IContinuousDistribution Members

        #region TRandom Helpers

        /// <summary>
        ///   Determines whether chi distribution is defined under given parameter. The default
        ///   definition returns true if alpha is greater than zero; otherwise, it returns false.
        /// </summary>
        /// <remarks>
        ///   This is an extensibility point for the <see cref="ChiDistribution"/> class.
        /// </remarks>
        public static Func<int, bool> IsValidParam { get; set; } = alpha =>
        {
            return alpha > 0;
        };

        /// <summary>
        ///   Declares a function returning a chi distributed floating point random number.
        /// </summary>
        /// <remarks>
        ///   This is an extensibility point for the <see cref="ChiDistribution"/> class.
        /// </remarks>
        public static Func<IGenerator, int, double> Sample { get; set; } = (generator, alpha) =>
        {
            const double m = 0.0;
            const double s = 1.0;
            var sum = 0.0;
            for (var i = 0; i < alpha; i++)
            {
                sum += TMath.Square(NormalDistribution.Sample(generator, m, s));
            }
            return Math.Sqrt(sum);
        };

        #endregion TRandom Helpers
    }
}