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
    using Core;

    /// <summary>
    ///   Provides generation of power distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="PowerDistribution"/> type bases upon information presented on
    ///   <a href="http://www.xycoon.com/power.htm">Xycoon - Power Distribution</a>.
    /// </remarks>
    [Serializable]
    public class PowerDistribution<TGen> : Distribution<TGen>, IContinuousDistribution, IAlphaDistribution<double>, 
                                           IBetaDistribution<double>
        where TGen : IGenerator
    {
        #region Class Fields

        /// <summary>
        ///   The default value assigned to <see cref="Alpha"/> if none is specified. 
        /// </summary>
        public const double DefaultAlpha = 1;

        /// <summary>
        ///   The default value assigned to <see cref="Beta"/> if none is specified. 
        /// </summary>
        public const double DefaultBeta = 1;

        #endregion

        #region Instance Fields

        /// <summary>
        ///   Stores the parameter alpha which is used for generation of power distributed random numbers.
        /// </summary>
        double _alpha;

        /// <summary>
        ///   Stores the parameter beta which is used for generation of power distributed random numbers.
        /// </summary>
        double _beta;

        /// <summary>
        ///   Gets or sets the parameter alpha which is used for generation of power distributed random numbers.
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
        ///   Gets or sets the parameter beta which is used for generation of power distributed random numbers.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   Calls <see cref="IsValidBeta"/> to determine whether a value is valid and therefore assignable.
        /// </remarks>
        public double Beta
        {
            get { return _beta; }
            set { _beta = value; }
        }

        #endregion

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="PowerDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of power distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of power distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="beta"/> are less than or equal to zero.
        /// </exception>
        public PowerDistribution(TGen generator, double alpha, double beta) : base(generator)
        {
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(generator, null), ErrorMessages.NullGenerator);
            Contract.Requires<ArgumentOutOfRangeException>(AreValidParams(alpha, beta), ErrorMessages.InvalidParams);
            _alpha = alpha;
            _beta = beta;
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
            return AreValidParams(value, Beta);
        }

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Beta"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>
        ///   <see langword="true"/> if value is greater than 0.0; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsValidBeta(double value)
        {
            return AreValidParams(_alpha, value);
        }

        #endregion

        #region IContinuousDistribution Members

        public double Minimum
        {
            get { return 0.0; }
        }

        public double Maximum
        {
            get { return 1.0/Beta; }
        }

        public double Mean
        {
            get { return _alpha/Beta/(_alpha + 1.0); }
        }

        public double Median
        {
            get { throw new NotSupportedException(ErrorMessages.UndefinedMedian); }
        }

        public double Variance
        {
            get { return _alpha/Math.Pow(Beta, 2.0)/Math.Pow(_alpha + 1.0, 2.0)/(_alpha + 2.0); }
        }

        public double[] Mode
        {
            get
            {
                if (_alpha > 1.0) {
                    return new[] {1.0/Beta};
                }
                if (_alpha < 1.0) {
                    return new[] {0.0};
                }
                throw new NotSupportedException(ErrorMessages.UndefinedModeForParams);
            }
        }

        public double NextDouble()
        {
            return Sample(Gen, _alpha, _beta);
        }

        #endregion

        #region TRandom Helpers

        /// <summary>
        ///   Determines whether power distribution is defined under given parameters.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of power distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of power distributed random numbers.
        /// </param>
        /// <returns>
        ///   True if <paramref name="alpha"/> and <paramref name="beta"/> are greater than zero;
        ///   otherwise, it returns false.
        /// </returns>
        [System.Diagnostics.Contracts.Pure]
        public static bool AreValidParams(double alpha, double beta)
        {
            return alpha > 0 && beta > 0;
        }

        /// <summary>
        ///   Returns a power distributed floating point random number.
        /// </summary>
        /// <param name="generator">The generator from which random number are drawn.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of power distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of power distributed random numbers.
        /// </param>
        /// <returns>
        ///   A power distributed floating point random number.
        /// </returns>
        [System.Diagnostics.Contracts.Pure]
        internal static double Sample(TGen generator, double alpha, double beta)
        {
            var helper1 = 1.0/alpha;
            return Math.Pow(generator.NextDouble(), helper1)/beta;
        }

        #endregion
    }

    /// <summary>
    ///   Provides generation of power distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="PowerDistribution"/> type bases upon information presented on
    ///   <a href="http://www.xycoon.com/power.htm">Xycoon - Power Distribution</a>.
    /// </remarks>
    [Serializable]
    public sealed class PowerDistribution : PowerDistribution<IGenerator>
    {
        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="PowerDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        public PowerDistribution() : this(new XorShift128Generator(), DefaultAlpha, DefaultBeta)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Beta, DefaultBeta));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="PowerDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        [CLSCompliant(false)]
        public PowerDistribution(uint seed) : this(new XorShift128Generator(seed), DefaultAlpha, DefaultBeta)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Beta, DefaultBeta));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="PowerDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        public PowerDistribution(IGenerator generator) : this(generator, DefaultAlpha, DefaultBeta)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Beta, DefaultBeta));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="PowerDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of power distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of power distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="beta"/> are less than or equal to zero.
        /// </exception>
        public PowerDistribution(double alpha, double beta) : this(new XorShift128Generator(), alpha, beta)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Beta, beta));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="PowerDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of power distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of power distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="beta"/> are less than or equal to zero.
        /// </exception>
        [CLSCompliant(false)]
        public PowerDistribution(uint seed, double alpha, double beta)
            : this(new XorShift128Generator(seed), alpha, beta)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Beta, beta));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="PowerDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of power distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of power distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="beta"/> are less than or equal to zero.
        /// </exception>
        public PowerDistribution(IGenerator generator, double alpha, double beta) : base(generator, alpha, beta)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Beta, beta));
        }

        #endregion
    }
}