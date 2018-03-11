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
    ///   Provides generation of rayleigh distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="RayleighDistribution"/> type bases upon information
    ///   presented on <a href="http://en.wikipedia.org/wiki/Rayleigh_distribution">Wikipedia -
    ///   Rayleigh Distribution</a>.
    ///
    ///   The thread safety of this class depends on the one of the underlying generator.
    /// </remarks>
    [Serializable]
    public sealed class RayleighDistribution : AbstractDistribution, IContinuousDistribution, ISigmaDistribution<double>
    {
        #region Constants

        /// <summary>
        ///   The default value assigned to <see cref="Sigma"/> if none is specified.
        /// </summary>
        public const double DefaultSigma = 1;

        #endregion Constants

        #region Fields

        /// <summary>
        ///   Stores the parameter sigma which is used for generation of rayleigh distributed random numbers.
        /// </summary>
        private double _sigma;

        /// <summary>
        ///   Gets or sets the parameter sigma which is used for generation of rayleigh distributed
        ///   random numbers.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   Calls <see cref="IsValidParam"/> to determine whether a value is valid and therefore assignable.
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
        ///   Initializes a new instance of the <see cref="RayleighDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        public RayleighDistribution() : this(new XorShift128Generator(), DefaultSigma)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Sigma, DefaultSigma));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="RayleighDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        public RayleighDistribution(uint seed) : this(new XorShift128Generator(seed), DefaultSigma)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Sigma, DefaultSigma));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="RayleighDistribution"/> class, using the
        ///   specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        public RayleighDistribution(IGenerator generator) : this(generator, DefaultSigma)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Sigma, DefaultSigma));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="RayleighDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="sigma">
        ///   The parameter sigma which is used for generation of rayleigh distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="sigma"/> is less than or equal to zero.
        /// </exception>
        public RayleighDistribution(double sigma) : this(new XorShift128Generator(), sigma)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Sigma, sigma));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="RayleighDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        /// <param name="sigma">
        ///   The parameter sigma which is used for generation of rayleigh distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="sigma"/> is less than or equal to zero.
        /// </exception>
        public RayleighDistribution(uint seed, double sigma) : this(new XorShift128Generator(seed), sigma)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Sigma, sigma));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="RayleighDistribution"/> class, using the
        ///   specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="sigma">
        ///   The parameter sigma which is used for generation of rayleigh distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="sigma"/> is less than or equal to zero.
        /// </exception>
        public RayleighDistribution(IGenerator generator, double sigma)
            : base(generator)
        {
            var vp = IsValidParam;
            if (!vp(sigma)) throw new ArgumentOutOfRangeException(nameof(sigma), ErrorMessages.InvalidParams);
            _sigma = sigma;
        }

        #endregion Construction

        #region Instance Methods

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Sigma"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns><see langword="true"/> if value is greater than 0.0; otherwise, <see langword="false"/>.</returns>
        public bool IsValidSigma(double value) => IsValidParam(value);

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
        public double Mean => _sigma * Math.Sqrt(Math.PI / 2.0);

        /// <summary>
        ///   Gets the median of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if median is not defined for given distribution with some parameters.
        /// </exception>
        public double Median => _sigma * Math.Sqrt(Math.Log(4));

        /// <summary>
        ///   Gets the variance of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if variance is not defined for given distribution with some parameters.
        /// </exception>
        public double Variance => TMath.Square(_sigma) * (4.0 - Math.PI) / 2.0;

        /// <summary>
        ///   Gets the mode of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if mode is not defined for given distribution with some parameters.
        /// </exception>
        public double[] Mode => new[] { _sigma };

        /// <summary>
        ///   Returns a distributed floating point random number.
        /// </summary>
        /// <returns>A distributed double-precision floating point number.</returns>
        public double NextDouble() => Sample(Generator, _sigma);

        #endregion IContinuousDistribution Members

        #region TRandom Helpers

        /// <summary>
        ///   Determines whether rayleigh distribution is defined under given parameter. The default
        ///   definition returns true if sigma is greater than zero; otherwise, it returns false.
        /// </summary>
        /// <remarks>
        ///   This is an extensibility point for the <see cref="RayleighDistribution"/> class.
        /// </remarks>
        public static Func<double, bool> IsValidParam { get; set; } = sigma =>
        {
            return sigma > 0.0;
        };

        /// <summary>
        ///   Declares a function returning a rayleigh distributed floating point random number.
        /// </summary>
        /// <remarks>
        ///   This is an extensibility point for the <see cref="RayleighDistribution"/> class.
        /// </remarks>
        public static Func<IGenerator, double, double> Sample { get; set; } = (generator, sigma) =>
        {
            const double mu = 0.0;
            var n1 = TMath.Square(NormalDistribution.Sample(generator, mu, sigma));
            var n2 = TMath.Square(NormalDistribution.Sample(generator, mu, sigma));
            return Math.Sqrt(n1 + n2);
        };

        #endregion TRandom Helpers
    }
}