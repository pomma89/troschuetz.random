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
    ///   Provides generation of t-distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="StudentsTDistribution"/> type bases upon information
    ///   presented on <a href="http://en.wikipedia.org/wiki/Student%27s_t-distribution">Wikipedia -
    ///   Student's t-distribution</a> and <a href="http://www.xycoon.com/stt_random.htm">Xycoon -
    ///   Student t Distribution</a>.
    ///
    ///   The thread safety of this class depends on the one of the underlying generator.
    /// </remarks>
    [Serializable]
    public sealed class StudentsTDistribution : AbstractDistribution, IContinuousDistribution, INuDistribution<int>
    {
        #region Constants

        /// <summary>
        ///   The default value assigned to <see cref="Nu"/> if none is specified.
        /// </summary>
        public const int DefaultNu = 1;

        #endregion Constants

        #region Fields

        /// <summary>
        ///   Stores the parameter nu which is used for generation of t-distributed random numbers.
        /// </summary>
        private int _nu;

        /// <summary>
        ///   Gets or sets the parameter nu which is used for generation of t-distributed random numbers.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   Calls <see cref="IsValidParam"/> to determine whether a value is valid and therefore assignable.
        /// </remarks>
        public int Nu
        {
            get { return _nu; }
            set
            {
                if (!IsValidNu(value)) throw new ArgumentOutOfRangeException(nameof(Nu), ErrorMessages.InvalidParams);
                _nu = value;
            }
        }

        #endregion Fields

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="StudentsTDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        public StudentsTDistribution() : this(new XorShift128Generator(), DefaultNu)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Nu, DefaultNu));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="StudentsTDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        public StudentsTDistribution(uint seed) : this(new XorShift128Generator(seed), DefaultNu)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Nu, DefaultNu));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="StudentsTDistribution"/> class, using the
        ///   specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        public StudentsTDistribution(IGenerator generator) : this(generator, DefaultNu)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Nu, DefaultNu));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="StudentsTDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="nu">
        ///   The parameter nu which is used for generation of student's t distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="nu"/> is less than or equal to zero.
        /// </exception>
        public StudentsTDistribution(int nu) : this(new XorShift128Generator(), nu)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Nu, nu));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="StudentsTDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        /// <param name="nu">
        ///   The parameter nu which is used for generation of student's t distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="nu"/> is less than or equal to zero.
        /// </exception>
        public StudentsTDistribution(uint seed, int nu) : this(new XorShift128Generator(seed), nu)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Nu, nu));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="StudentsTDistribution"/> class, using the
        ///   specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="nu">
        ///   The parameter nu which is used for generation of student's t distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="nu"/> is less than or equal to zero.
        /// </exception>
        public StudentsTDistribution(IGenerator generator, int nu) : base(generator)
        {
            var vp = IsValidParam;
            if (!vp(nu)) throw new ArgumentOutOfRangeException(nameof(nu), ErrorMessages.InvalidParams);
            _nu = nu;
        }

        #endregion Construction

        #region Instance Methods

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Nu"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns><see langword="true"/> if value is greater than 0; otherwise, <see langword="false"/>.</returns>
        public bool IsValidNu(int value) => IsValidParam(value);

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
            get
            {
                if (_nu > 1)
                {
                    return 0.0;
                }
                throw new NotSupportedException(ErrorMessages.UndefinedMeanForParams);
            }
        }

        /// <summary>
        ///   Gets the median of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if median is not defined for given distribution with some parameters.
        /// </exception>
        public double Median => 0.0;

        /// <summary>
        ///   Gets the variance of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if variance is not defined for given distribution with some parameters.
        /// </exception>
        public double Variance
        {
            get
            {
                if (_nu > 2)
                {
                    return _nu / (_nu - 2.0);
                }
                throw new NotSupportedException(ErrorMessages.UndefinedVarianceForParams);
            }
        }

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
        public double NextDouble() => Sample(Generator, _nu);

        #endregion IContinuousDistribution Members

        #region TRandom Helpers

        /// <summary>
        ///   Determines whether student's t distribution is defined under given parameter. The
        ///   default definition returns true if nu is greater than zero; otherwise, it returns false.
        /// </summary>
        /// <remarks>
        ///   This is an extensibility point for the <see cref="StudentsTDistribution"/> class.
        /// </remarks>
        public static Func<int, bool> IsValidParam { get; set; } = nu =>
        {
            return nu > 0;
        };

        /// <summary>
        ///   Declares a function returning a student's t distributed floating point random number.
        /// </summary>
        /// <remarks>
        ///   This is an extensibility point for the <see cref="StudentsTDistribution"/> class.
        /// </remarks>
        public static Func<IGenerator, int, double> Sample { get; set; } = (generator, nu) =>
        {
            const double mu = 0.0;
            const double sigma = 1.0;
            var n = NormalDistribution.Sample(generator, mu, sigma);
            var c = ChiSquareDistribution.Sample(generator, nu);
            return n / Math.Sqrt(c / nu);
        };

        #endregion TRandom Helpers
    }
}