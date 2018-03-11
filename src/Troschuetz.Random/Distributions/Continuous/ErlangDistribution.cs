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
    ///   Provides generation of erlang distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="ErlangDistribution"/> type bases upon information
    ///   presented on <a href="http://en.wikipedia.org/wiki/Erlang_distribution">Wikipedia - Erlang
    ///   distribution</a> and <a href="http://www.xycoon.com/erlang_random.htm">Xycoon - Erlang Distribution</a>.
    ///
    ///   The thread safety of this class depends on the one of the underlying generator.
    /// </remarks>
    [Serializable]
    public sealed class ErlangDistribution : AbstractDistribution, IContinuousDistribution, IAlphaDistribution<int>, ILambdaDistribution<double>
    {
        #region Constants

        /// <summary>
        ///   The default value assigned to <see cref="Alpha"/> if none is specified.
        /// </summary>
        public const int DefaultAlpha = 1;

        /// <summary>
        ///   The default value assigned to <see cref="Lambda"/> if none is specified.
        /// </summary>
        public const double DefaultLambda = 1;

        #endregion Constants

        #region Fields

        /// <summary>
        ///   Stores the parameter lambda which is used for generation of rayleigh distributed random numbers.
        /// </summary>
        private double _lambda;

        /// <summary>
        ///   Stores the parameter alpha which is used for generation of rayleigh distributed random numbers.
        /// </summary>
        private int _alpha;

        /// <summary>
        ///   Gets or sets the parameter alpha which is used for generation of erlang distributed
        ///   random numbers.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   Calls <see cref="AreValidParams"/> to determine whether a value is valid and therefore assignable.
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

        /// <summary>
        ///   Gets or sets the parameter lambda which is used for generation of erlang distributed
        ///   random numbers.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   Calls <see cref="AreValidParams"/> to determine whether a value is valid and therefore assignable.
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
        ///   Initializes a new instance of the <see cref="ErlangDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        public ErlangDistribution()
            : this(new XorShift128Generator(), DefaultAlpha, DefaultLambda)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Lambda, DefaultLambda));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ErlangDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        public ErlangDistribution(uint seed)
            : this(new XorShift128Generator(seed), DefaultAlpha, DefaultLambda)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Lambda, DefaultLambda));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ErlangDistribution"/> class, using the
        ///   specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        public ErlangDistribution(IGenerator generator)
            : this(generator, DefaultAlpha, DefaultLambda)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Lambda, DefaultLambda));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ErlangDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of erlang distributed random numbers.
        /// </param>
        /// <param name="lambda">
        ///   The parameter lambda which is used for generation of erlang distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="lambda"/> are less than or equal to zero.
        /// </exception>
        public ErlangDistribution(int alpha, double lambda)
            : this(new XorShift128Generator(), alpha, lambda)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Lambda, lambda));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ErlangDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of erlang distributed random numbers.
        /// </param>
        /// <param name="lambda">
        ///   The parameter lambda which is used for generation of erlang distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="lambda"/> are less than or equal to zero.
        /// </exception>
        public ErlangDistribution(uint seed, int alpha, double lambda)
            : this(new XorShift128Generator(seed), alpha, lambda)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Lambda, lambda));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ErlangDistribution"/> class, using the
        ///   specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of erlang distributed random numbers.
        /// </param>
        /// <param name="lambda">
        ///   The parameter lambda which is used for generation of erlang distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="lambda"/> are less than or equal to zero.
        /// </exception>
        public ErlangDistribution(IGenerator generator, int alpha, double lambda)
            : base(generator)
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
        /// <returns><see langword="true"/> if value is greater than 0; otherwise, <see langword="false"/>.</returns>
        public bool IsValidAlpha(int value) => AreValidParams(value, _lambda);

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Lambda"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns><see langword="true"/> if value is greater than 0.0; otherwise, <see langword="false"/>.</returns>
        public bool IsValidLambda(double value) => AreValidParams(_alpha, value);

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
        public double Mean => Alpha / Lambda;

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
        public double Variance => Alpha / TMath.Square(Lambda);

        /// <summary>
        ///   Gets the mode of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if mode is not defined for given distribution with some parameters.
        /// </exception>
        public double[] Mode => new[] { (Alpha - 1) / Lambda };

        /// <summary>
        ///   Returns a distributed floating point random number.
        /// </summary>
        /// <returns>A distributed double-precision floating point number.</returns>
        public double NextDouble() => Sample(Generator, _alpha, _lambda);

        #endregion IContinuousDistribution Members

        #region TRandom Helpers

        /// <summary>
        ///   Determines whether erlang distribution is defined under given parameters. The default
        ///   definition returns true if alpha and lambda are greater than zero; otherwise, it
        ///   returns false.
        /// </summary>
        /// <remarks>
        ///   This is an extensibility point for the <see cref="ErlangDistribution"/> class.
        /// </remarks>
        public static Func<int, double, bool> AreValidParams { get; set; } = (alpha, lambda) =>
        {
            return alpha > 0 && lambda > 0.0;
        };

        /// <summary>
        ///   Declares a function returning an erlang distributed floating point random number.
        /// </summary>
        /// <remarks>
        ///   This is an extensibility point for the <see cref="ErlangDistribution"/> class.
        /// </remarks>
        public static Func<IGenerator, int, double, double> Sample { get; set; } = (generator, alpha, lambda) =>
        {
            if (double.IsPositiveInfinity(lambda))
            {
                return alpha;
            }

            const double mu = 0.0;
            const double sigma = 1.0;
            var a = alpha;
            var alphafix = 1.0;

            // Fix when alpha is less than one.
            if (alpha < 1.0)
            {
                a = alpha + 1;
                alphafix = Math.Pow(generator.NextDouble(), 1.0 / alpha);
            }

            var d = a - (1.0 / 3.0);
            var c = 1.0 / Math.Sqrt(9.0 * d);
            while (true)
            {
                var x = NormalDistribution.Sample(generator, mu, sigma);
                var v = 1.0 + (c * x);
                while (v <= 0.0)
                {
                    x = NormalDistribution.Sample(generator, mu, sigma);
                    v = 1.0 + (c * x);
                }

                v = v * v * v;
                var u = generator.NextDouble();
                x = x * x;
                if (u < 1.0 - (0.0331 * x * x))
                {
                    return alphafix * d * v / lambda;
                }

                if (Math.Log(u) < (0.5 * x) + (d * (1.0 - v + Math.Log(v))))
                {
                    return alphafix * d * v / lambda;
                }
            }
        };

        #endregion TRandom Helpers
    }
}