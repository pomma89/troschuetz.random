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

    /// <summary>
    ///   Provides generation of chi distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="ChiDistribution"/> type bases upon information presented on
    ///   <a href="http://en.wikipedia.org/wiki/Chi_distribution">Wikipedia - Chi distribution</a>.
    /// </remarks>
    [Serializable]
    public class ChiDistribution<TGen> : Distribution<TGen>, IContinuousDistribution, IAlphaDistribution<int>
        where TGen : IGenerator
    {
        #region Class Fields

        /// <summary>
        ///   The default value assigned to <see cref="Alpha"/> if none is specified. 
        /// </summary>
        public const int DefaultAlpha = 1;

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
        ///   Stores the parameter alpha which is used for generation of chi distributed random numbers.
        /// </summary>
        int _alpha;

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
            set { _alpha = value; }
        }

        #endregion

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="ChiDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of chi distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero.
        /// </exception>
        public ChiDistribution(TGen generator, int alpha)
            : base(generator)
        {
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(generator, null), ErrorMessages.NullGenerator);
            Contract.Requires<ArgumentOutOfRangeException>(IsValidParam(alpha), ErrorMessages.InvalidParams);
            _alpha = alpha;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Alpha"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>
        ///   <see langword="true"/> if value is greater than 0; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsValidAlpha(int value)
        {
            return IsValidParam(value);
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
            get { return Math.Sqrt(2.0)*Gamma((Alpha + 1.0)/2.0)/Gamma(Alpha/2.0); }
        }

        public double Median
        {
            get { throw new NotSupportedException(ErrorMessages.UndefinedMedian); }
        }

        public double Variance
        {
            get { return Alpha - Math.Pow(Mean, 2.0); }
        }

        public double[] Mode
        {
            get { return new[] {Math.Sqrt(Alpha - 1.0)}; } // alpha >= 1
        }

        public double NextDouble()
        {
            return Sample(Gen, _alpha);
        }

        #endregion

        #region TRandom Helpers

        /// <summary>
        ///   Determines whether chi distribution is defined under given parameter.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of chi distributed random numbers.
        /// </param>
        /// <returns>
        ///   True if <paramref name="alpha"/> is greater than zero; otherwise, it returns false.
        /// </returns>
        [System.Diagnostics.Contracts.Pure]
        public static bool IsValidParam(int alpha)
        {
            return alpha > 0;
        }

        /// <summary>
        ///   Returns a chi distributed floating point random number.
        /// </summary>
        /// <param name="generator">The generator from which random number are drawn.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of chi distributed random numbers.
        /// </param>
        /// <returns>
        ///   A chi distributed floating point random number.
        /// </returns>
        [System.Diagnostics.Contracts.Pure]
        internal static double Sample(TGen generator, int alpha)
        {
            const double m = 0.0;
            const double s = 1.0;
            var sum = 0.0;
            for (var i = 0; i < alpha; i++) {
                sum += Math.Pow(NormalDistribution<TGen>.Sample(generator, m, s), 2);
            }
            return Math.Sqrt(sum);
        }

        #endregion
    }

    /// <summary>
    ///   Provides generation of chi distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="ChiDistribution"/> type bases upon information presented on
    ///   <a href="http://en.wikipedia.org/wiki/Chi_distribution">Wikipedia - Chi distribution</a>.
    /// </remarks>
    [Serializable]
    public sealed class ChiDistribution : ChiDistribution<IGenerator>
    {
        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="ChiDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        public ChiDistribution()
            : base(new XorShift128Generator(), DefaultAlpha)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ChiDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        [CLSCompliant(false)]
        public ChiDistribution(uint seed)
            : base(new XorShift128Generator(seed), DefaultAlpha)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ChiDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        public ChiDistribution(IGenerator generator)
            : base(generator, DefaultAlpha)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Alpha, DefaultAlpha));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ChiDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of chi distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero.
        /// </exception>
        public ChiDistribution(int alpha)
            : base(new XorShift128Generator(), alpha)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, alpha));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ChiDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> with the specified seed value.
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
        [CLSCompliant(false)]
        public ChiDistribution(uint seed, int alpha)
            : base(new XorShift128Generator(seed), alpha)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, alpha));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ChiDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of chi distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero.
        /// </exception>
        public ChiDistribution(IGenerator generator, int alpha) : base(generator, alpha)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Alpha, alpha));
        }

        #endregion
    }
}