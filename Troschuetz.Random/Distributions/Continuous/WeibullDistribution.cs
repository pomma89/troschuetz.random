/*
 * Copyright © 2006 Stefan Troschütz (stefan@troschuetz.de)
 * Copyright © 2012-2014 Alessio Parma (alessio.parma@gmail.com)
 * 
 * This file is part of Troschuetz.Random Class Library.
 * 
 * Troschuetz.Random is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
 */

namespace Troschuetz.Random.Distributions.Continuous
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using Generators;
    using JetBrains.Annotations;

    /// <summary>
    ///   Provides generation of weibull distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="WeibullDistribution"/> type bases upon information presented on
    ///   <a href="http://en.wikipedia.org/wiki/Weibull_distribution">Wikipedia - Weibull distribution</a>.
    /// </remarks>
    [PublicAPI]
    public class WeibullDistribution<TGen> : Distribution<TGen>, IContinuousDistribution, IAlphaDistribution<double>, 
                                                    ILambdaDistribution<double>
        where TGen : IGenerator
    {
        #region Class Fields

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
        static readonly double[] LanczosCoefficients = {
            1.000000000190015, 76.18009172947146, -86.50532032941677,
            24.01409824083091, -1.231739572450155, 1.208650973866179e-3,
            -5.395239384953e-6
        };

        #endregion

        #region Instance Fields

        /// <summary>
        ///   Stores the parameter alpha which is used for generation of weibull distributed random numbers.
        /// </summary>
        double _alpha;
        
        /// <summary>
        ///   Stores the parameter lambda which is used for generation of weibull distributed random numbers.
        /// </summary>
        double _lambda;

        /// <summary>
        ///   Gets or sets the parameter alpha which is used for generation of weibull distributed random numbers.
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
            set { _alpha = value; }
        }

        /// <summary>
        ///   Gets or sets the parameter lambda which is used for generation of erlang distributed random numbers.
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
            set { _lambda = value; }
        }

        #endregion

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="WeibullDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of weibull distributed random numbers.
        /// </param>
        /// <param name="lambda">
        ///   The parameter lambda which is used for generation of weibull distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="lambda"/> are less than or equal to zero. 
        /// </exception>
        public WeibullDistribution(TGen generator, double alpha, double lambda) : base(generator)
        {
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(generator, null), ErrorMessages.NullGenerator);
            Contract.Requires<ArgumentOutOfRangeException>(AreValidParams(alpha, lambda), ErrorMessages.InvalidParams);
            _alpha = alpha;
            _lambda = lambda;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Alpha"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>
        ///   <see langword="true"/> if value is greater than 0.0; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsValidAlpha(double value)
        {
            return AreValidParams(value, Lambda);
        }

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Lambda"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>
        ///   <see langword="true"/> if value is greater than 0.0; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsValidLambda(double value)
        {
            return AreValidParams(_alpha, value);
        }

        /// <summary>
        ///   Represents a Lanczos approximation of the Gamma function.
        /// </summary>
        /// <param name="x">A double-precision floating point number.</param>
        /// <returns>
        ///   A double-precision floating point number representing an approximation of Gamma(<paramref name="x"/>).
        /// </returns>
        static double Gamma(double x)
        {
            var sum = LanczosCoefficients[0];
            for (var index = 1; index <= 6; index++) {
                sum += LanczosCoefficients[index]/(x + index);
            }

            return Math.Sqrt(2.0*Math.PI)/x*Math.Pow(x + 5.5, x + 0.5)/Math.Exp(x + 5.5)*sum;
        }

        #endregion

        #region IContinuousDistribution Members

        public double Minimum
        {
            get { return 0.0; }
        }

        public double Maximum
        {
            get { return double.PositiveInfinity; }
        }

        public double Mean
        {
            get { return Lambda*Gamma(1.0 + 1.0/_alpha); }
        }

        public double Median
        {
            get { return Lambda*Math.Pow(Math.Log(2.0), 1.0/_alpha); }
        }

        public double Variance
        {
            get { return Math.Pow(Lambda, 2.0)*Gamma(1.0 + 2.0/_alpha) - Math.Pow(Mean, 2.0); }
        }

        public double[] Mode
        {
            get
            {
                if (_alpha >= 1.0) {
                    return new[] {Lambda*Math.Pow(1.0 - 1.0/_alpha, 1.0/_alpha)};
                }
                throw new NotSupportedException(ErrorMessages.UndefinedModeForParams);
            }
        }

        public double NextDouble()
        {
            return Sample(Gen, _alpha, _lambda);
        }

        #endregion

        #region TRandom Helpers

        /// <summary>
        ///   Determines whether weibull distribution is defined under given parameters.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of weibull distributed random numbers.
        /// </param>
        /// <param name="lambda">
        ///   The parameter lambda which is used for generation of weibull distributed random numbers.
        /// </param>
        /// <returns>
        ///   True if <paramref name="alpha"/> and <paramref name="lambda"/> are greater than zero;
        ///   otherwise, it returns false.
        /// </returns>
        [System.Diagnostics.Contracts.Pure]
        public static bool AreValidParams(double alpha, double lambda)
        {
            return alpha > 0 && lambda > 0;
        }

        /// <summary>
        ///   Returns a weibull distributed floating point random number.
        /// </summary>
        /// <param name="generator">The generator from which random number are drawn.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of weibull distributed random numbers.
        /// </param>
        /// <param name="lambda">
        ///   The parameter lambda which is used for generation of weibull distributed random numbers.
        /// </param>
        /// <returns>
        ///   A weibull distributed floating point random number.
        /// </returns>
        [System.Diagnostics.Contracts.Pure]
        internal static double Sample(TGen generator, double alpha, double lambda)
        {
            var helper1 = 1.0/alpha;
            // Subtracts a random number from 1.0 to avoid Math.Log(0.0).
            return lambda*Math.Pow(-Math.Log(1.0 - generator.NextDouble()), helper1);
        }

        #endregion
    }

    /// <summary>
    ///   Provides generation of weibull distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="WeibullDistribution"/> type bases upon information presented on
    ///   <a href="http://en.wikipedia.org/wiki/Weibull_distribution">Wikipedia - Weibull distribution</a>.
    /// </remarks>
    [PublicAPI]
    public sealed class WeibullDistribution : WeibullDistribution<IGenerator>
    {
        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="WeibullDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        public WeibullDistribution() : base(new XorShift128Generator(), DefaultAlpha, DefaultLambda)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Lambda, DefaultLambda));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="WeibullDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        [CLSCompliant(false)]
        public WeibullDistribution(uint seed) : base(new XorShift128Generator(seed), DefaultAlpha, DefaultLambda)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Lambda, DefaultLambda));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="WeibullDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        public WeibullDistribution(IGenerator generator) : base(generator, DefaultAlpha, DefaultLambda)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Lambda, DefaultLambda));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="WeibullDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> as underlying random number generator.
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
        public WeibullDistribution(double alpha, double lambda) : base(new XorShift128Generator(), alpha, lambda)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Lambda, lambda));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="WeibullDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> with the specified seed value.
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
        [CLSCompliant(false)]
        public WeibullDistribution(uint seed, double alpha, double lambda)
            : base(new XorShift128Generator(seed), alpha, lambda)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Lambda, lambda));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="WeibullDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of weibull distributed random numbers.
        /// </param>
        /// <param name="lambda">
        ///   The parameter lambda which is used for generation of weibull distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="lambda"/> are less than or equal to zero. 
        /// </exception>
        public WeibullDistribution(IGenerator generator, double alpha, double lambda) : base(generator, alpha, lambda)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Lambda, lambda));
        }

        #endregion
    }
}