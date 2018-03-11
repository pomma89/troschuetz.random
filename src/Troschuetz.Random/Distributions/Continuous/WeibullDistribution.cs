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
    ///   Provides generation of weibull distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="WeibullDistribution"/> type bases upon information
    ///   presented on <a href="http://en.wikipedia.org/wiki/Weibull_distribution">Wikipedia -
    ///   Weibull distribution</a>.
    ///
    ///   The thread safety of this class depends on the one of the underlying generator.
    /// </remarks>
    [Serializable]
    public sealed class WeibullDistribution : AbstractDistribution, IContinuousDistribution, IAlphaDistribution<double>, ILambdaDistribution<double>
    {
        #region Constants

        /// <summary>
        ///   The default value assigned to <see cref="Alpha"/> if none is specified.
        /// </summary>
        public const double DefaultAlpha = 1;

        /// <summary>
        ///   The default value assigned to <see cref="Lambda"/> if none is specified.
        /// </summary>
        public const double DefaultLambda = 1;

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
        ///   Stores the parameter alpha which is used for generation of weibull distributed random numbers.
        /// </summary>
        private double _alpha;

        /// <summary>
        ///   Stores the parameter lambda which is used for generation of weibull distributed random numbers.
        /// </summary>
        private double _lambda;

        /// <summary>
        ///   Gets or sets the parameter alpha which is used for generation of weibull distributed
        ///   random numbers.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   Calls <see cref="IsValidAlpha"/> to determine whether a value is valid and therefore assignable.
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
        ///   Gets or sets the parameter lambda which is used for generation of erlang distributed
        ///   random numbers.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   Calls <see cref="IsValidLambda"/> to determine whether a value is valid and therefore assignable.
        /// </remarks>
        public double Lambda
        {
            get { return _lambda; }
            set
            {
                if (!IsValidLambda(value)) throw new ArgumentOutOfRangeException(nameof(Lambda), ErrorMessages.InvalidParams);
                _lambda = value;
            }
        }

        #endregion Fields

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="WeibullDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        public WeibullDistribution() : this(new XorShift128Generator(), DefaultAlpha, DefaultLambda)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Lambda, DefaultLambda));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="WeibullDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        public WeibullDistribution(uint seed) : this(new XorShift128Generator(seed), DefaultAlpha, DefaultLambda)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Lambda, DefaultLambda));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="WeibullDistribution"/> class, using the
        ///   specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        public WeibullDistribution(IGenerator generator) : this(generator, DefaultAlpha, DefaultLambda)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Lambda, DefaultLambda));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="WeibullDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of weibull distributed random numbers.
        /// </param>
        /// <param name="lambda">
        ///   The parameter lambda which is used for generation of weibull distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="lambda"/> are less than or equal to zero.
        /// </exception>
        public WeibullDistribution(double alpha, double lambda) : this(new XorShift128Generator(), alpha, lambda)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Lambda, lambda));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="WeibullDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of weibull distributed random numbers.
        /// </param>
        /// <param name="lambda">
        ///   The parameter lambda which is used for generation of weibull distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="lambda"/> are less than or equal to zero.
        /// </exception>
        public WeibullDistribution(uint seed, double alpha, double lambda)
            : this(new XorShift128Generator(seed), alpha, lambda)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Lambda, lambda));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="WeibullDistribution"/> class, using the
        ///   specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of weibull distributed random numbers.
        /// </param>
        /// <param name="lambda">
        ///   The parameter lambda which is used for generation of weibull distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="lambda"/> are less than or equal to zero.
        /// </exception>
        public WeibullDistribution(IGenerator generator, double alpha, double lambda) : base(generator)
        {
            var vp = AreValidParams;
            if (!vp(alpha, lambda)) throw new ArgumentOutOfRangeException($"{nameof(alpha)}/{nameof(lambda)}", ErrorMessages.InvalidParams);
            _alpha = alpha;
            _lambda = lambda;
        }

        #endregion Construction

        #region Instance Methods

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Alpha"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns><see langword="true"/> if value is greater than 0.0; otherwise, <see langword="false"/>.</returns>
        public bool IsValidAlpha(double value) => AreValidParams(value, Lambda);

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Lambda"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns><see langword="true"/> if value is greater than 0.0; otherwise, <see langword="false"/>.</returns>
        public bool IsValidLambda(double value) => AreValidParams(_alpha, value);

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
        public double Mean => Lambda * Gamma(1.0 + 1.0 / _alpha);

        /// <summary>
        ///   Gets the median of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if median is not defined for given distribution with some parameters.
        /// </exception>
        public double Median => Lambda * Math.Pow(Math.Log(2.0), 1.0 / _alpha);

        /// <summary>
        ///   Gets the variance of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if variance is not defined for given distribution with some parameters.
        /// </exception>
        public double Variance => TMath.Square(Lambda) * Gamma(1.0 + 2.0 / _alpha) - TMath.Square(Mean);

        /// <summary>
        ///   Gets the mode of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if mode is not defined for given distribution with some parameters.
        /// </exception>
        public double[] Mode
        {
            get
            {
                if (_alpha >= 1.0)
                {
                    return new[] { Lambda * Math.Pow(1.0 - 1.0 / _alpha, 1.0 / _alpha) };
                }
                throw new NotSupportedException(ErrorMessages.UndefinedModeForParams);
            }
        }

        /// <summary>
        ///   Returns a distributed floating point random number.
        /// </summary>
        /// <returns>A distributed double-precision floating point number.</returns>
        public double NextDouble() => Sample(Generator, _alpha, _lambda);

        #endregion IContinuousDistribution Members

        #region TRandom Helpers

        /// <summary>
        ///   Determines whether weibull distribution is defined under given parameters. The default
        ///   definition returns true if alpha and lambda are greater than zero; otherwise, it
        ///   returns false.
        /// </summary>
        /// <remarks>
        ///   This is an extensibility point for the <see cref="WeibullDistribution"/> class.
        /// </remarks>
        public static Func<double, double, bool> AreValidParams { get; set; } = (alpha, lambda) =>
        {
            return alpha > 0.0 && lambda > 0.0;
        };

        /// <summary>
        ///   Declares a function returning a weibull distributed floating point random number.
        /// </summary>
        /// <remarks>
        ///   This is an extensibility point for the <see cref="WeibullDistribution"/> class.
        /// </remarks>
        public static Func<IGenerator, double, double, double> Sample { get; set; } = (generator, alpha, lambda) =>
        {
            var helper1 = 1.0 / alpha;
            // Subtracts a random number from 1.0 to avoid Math.Log(0.0).
            return lambda * Math.Pow(-Math.Log(1.0 - generator.NextDouble()), helper1);
        };

        #endregion TRandom Helpers
    }
}