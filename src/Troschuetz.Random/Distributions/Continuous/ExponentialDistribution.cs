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
    ///   Provides generation of exponential distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="ExponentialDistribution"/> type bases upon information
    ///   presented on <a href="http://en.wikipedia.org/wiki/Exponential_distribution">Wikipedia -
    ///   Exponential distribution</a>.
    ///
    ///   The thread safety of this class depends on the one of the underlying generator.
    /// </remarks>
    [Serializable]
    public sealed class ExponentialDistribution : AbstractDistribution, IContinuousDistribution, ILambdaDistribution<double>
    {
        #region Constants

        /// <summary>
        ///   The default value assigned to <see cref="Lambda"/> if none is specified.
        /// </summary>
        public const double DefaultLambda = 1;

        #endregion Constants

        #region Fields

        /// <summary>
        ///   Stores the parameter lambda which is used for generation of exponential distributed
        ///   random numbers.
        /// </summary>
        private double _lambda;

        /// <summary>
        ///   Gets or sets the parameter lambda which is used for generation of exponential
        ///   distributed random numbers.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   Calls <see cref="IsValidParam"/> to determine whether a value is valid and therefore assignable.
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
        ///   Initializes a new instance of the <see cref="ExponentialDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        public ExponentialDistribution() : this(new XorShift128Generator(), DefaultLambda)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Lambda, DefaultLambda));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ExponentialDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        public ExponentialDistribution(uint seed) : this(new XorShift128Generator(seed), DefaultLambda)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Lambda, DefaultLambda));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ExponentialDistribution"/> class, using
        ///   the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        public ExponentialDistribution(IGenerator generator) : this(generator, DefaultLambda)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Lambda, DefaultLambda));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ExponentialDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="lambda">
        ///   The parameter lambda which is used for generation of exponential distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="lambda"/> is less than or equal to zero.
        /// </exception>
        public ExponentialDistribution(double lambda) : this(new XorShift128Generator(), lambda)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Lambda, lambda));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ExponentialDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        /// <param name="lambda">
        ///   The parameter lambda which is used for generation of exponential distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="lambda"/> is less than or equal to zero.
        /// </exception>
        public ExponentialDistribution(uint seed, double lambda) : this(new XorShift128Generator(seed), lambda)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Lambda, lambda));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ExponentialDistribution"/> class, using
        ///   the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="lambda">
        ///   The parameter lambda which is used for generation of exponential distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="lambda"/> is less than or equal to zero.
        /// </exception>
        public ExponentialDistribution(IGenerator generator, double lambda) : base(generator)
        {
            var vp = IsValidParam;
            if (!vp(lambda)) throw new ArgumentOutOfRangeException(nameof(lambda), ErrorMessages.InvalidParams);
            _lambda = lambda;
        }

        #endregion Construction

        #region Instance Methods

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Lambda"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns><see langword="true"/> if value is greater than 0.0; otherwise, <see langword="false"/>.</returns>
        public bool IsValidLambda(double value) => IsValidParam(value);

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
        public double Mean => 1.0 / _lambda;

        /// <summary>
        ///   Gets the median of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if median is not defined for given distribution with some parameters.
        /// </exception>
        public double Median => Math.Log(2.0) / _lambda;

        /// <summary>
        ///   Gets the variance of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if variance is not defined for given distribution with some parameters.
        /// </exception>
        public double Variance => Math.Pow(_lambda, -2.0);

        /// <summary>
        ///   Gets the mode of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if mode is not defined for given distribution with some parameters.
        /// </exception>
        public double[] Mode => new[] { 0.0 };

        /// <summary>
        ///   Returns a distributed floating point random number.
        /// </summary>
        /// <returns>A distributed double-precision floating point number.</returns>
        public double NextDouble() => Sample(Generator, _lambda);

        #endregion IContinuousDistribution Members

        #region TRandom Helpers

        /// <summary>
        ///   Determines whether exponential distribution is defined under given parameter. The
        ///   default definition returns true if lambda is greater than zero; otherwise, it returns false.
        /// </summary>
        /// <remarks>
        ///   This is an extensibility point for the <see cref="ExponentialDistribution"/> class.
        /// </remarks>
        public static Func<double, bool> IsValidParam { get; set; } = lambda => lambda > 0.0;

        /// <summary>
        ///   Declares a function returning an exponential distributed floating point random number.
        /// </summary>
        /// <remarks>
        ///   This is an extensibility point for the <see cref="ExponentialDistribution"/> class.
        /// </remarks>
        public static Func<IGenerator, double, double> Sample { get; set; } = (generator, lambda) =>
        {
            double u;
            do u = generator.NextDouble(); while (TMath.IsZero(u));
            return -Math.Log(u) / lambda;
        };

        #endregion TRandom Helpers
    }
}