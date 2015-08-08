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
    ///   Provides generation of laplace distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="LaplaceDistribution"/> type bases upon information presented on
    ///   <a href="http://en.wikipedia.org/wiki/Laplace_distribution">Wikipedia - Laplace distribution</a>.
    /// </remarks>
    [Serializable]
    public class LaplaceDistribution<TGen> : Distribution<TGen>, IContinuousDistribution, IAlphaDistribution<double>, 
                                             IMuDistribution<double>
        where TGen : IGenerator
    {
        #region Class Fields

        /// <summary>
        ///   The default value assigned to <see cref="Alpha"/> if none is specified. 
        /// </summary>
        public const double DefaultAlpha = 1;

        /// <summary>
        ///   The default value assigned to <see cref="Mu"/> if none is specified. 
        /// </summary>
        public const double DefaultMu = 0;

        #endregion

        #region Instance Fields

        /// <summary>
        ///   Stores the parameter alpha which is used for generation of laplace distributed random numbers.
        /// </summary>
        double _alpha;

        /// <summary>
        ///   Stores the parameter mu which is used for generation of laplace distributed random numbers.
        /// </summary>
        double _mu;

        /// <summary>
        ///   Gets or sets the parameter alpha which is used for generation of laplace distributed random numbers.
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
            set { _alpha = value; }
        }

        /// <summary>
        ///   Gets or sets the parameter mu which is used for generation of laplace distributed random numbers.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value"/> is equal to <see cref="double.NaN"/>.
        /// </exception>
        /// <remarks>
        ///   Calls <see cref="AreValidParams"/> to determine whether a value is valid and therefore assignable.
        /// </remarks>
        public double Mu
        {
            get { return _mu; }
            set { _mu = value; }
        }

        #endregion

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="LaplaceDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of laplace distributed random numbers.
        /// </param>
        /// <param name="mu">
        ///   The parameter mu which is used for generation of laplace distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero.
        /// </exception>
        public LaplaceDistribution(TGen generator, double alpha, double mu) : base(generator)
        {
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(generator, null), ErrorMessages.NullGenerator);
            Contract.Requires<ArgumentOutOfRangeException>(AreValidParams(alpha, mu), ErrorMessages.InvalidParams);
            _alpha = alpha;
            _mu = mu;
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
            return AreValidParams(value, Mu);
        }

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Mu"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns><see langword="true"/>.</returns>
        public bool IsValidMu(double value)
        {
            return AreValidParams(Alpha, value);
        }

        #endregion

        #region IContinuousDistribution Members

        public double Minimum
        {
            get { return double.NegativeInfinity; }
        }

        public double Maximum
        {
            get { return double.PositiveInfinity; }
        }

        public double Mean
        {
            get { return Mu; }
        }

        public double Median
        {
            get { return Mu; }
        }

        public double Variance
        {
            get { return 2.0*Math.Pow(Alpha, 2.0); }
        }

        public double[] Mode
        {
            get { return new[] {Mu}; }
        }

        public double NextDouble()
        {
            return Sample(Gen, _alpha, _mu);
        }

        #endregion

        #region TRandom Helpers

        /// <summary>
        ///   Determines whether laplace distribution is defined under given parameters.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of laplace distributed random numbers.
        /// </param>
        /// <param name="mu">
        ///   The parameter mu which is used for generation of laplace distributed random numbers.
        /// </param>
        /// <returns>
        ///   True if <paramref name="alpha"/> is greater than zero; otherwise, it returns false.
        /// </returns>
        [System.Diagnostics.Contracts.Pure]
        public static bool AreValidParams(double alpha, double mu)
        {
            return alpha > 0 && !double.IsNaN(mu);
        }

        /// <summary>
        ///   Returns a laplace distributed floating point random number.
        /// </summary>
        /// <param name="generator">The generator from which random number are drawn.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of laplace distributed random numbers.
        /// </param>
        /// <param name="mu">
        ///   The parameter mu which is used for generation of laplace distributed random numbers.
        /// </param>
        /// <returns>
        ///   A laplace distributed floating point random number.
        /// </returns>
        [System.Diagnostics.Contracts.Pure]
        internal static double Sample(TGen generator, double alpha, double mu)
        {
            var rand = 0.5 - generator.NextDouble();
            var tmp = (rand == 0) ? double.NegativeInfinity : Math.Log(2.0*Math.Abs(rand));
            return mu - alpha*Math.Sign(rand)*tmp;
        }

        #endregion
    }

    /// <summary>
    ///   Provides generation of laplace distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="LaplaceDistribution"/> type bases upon information presented on
    ///   <a href="http://en.wikipedia.org/wiki/Laplace_distribution">Wikipedia - Laplace distribution</a>.
    /// </remarks>
    [Serializable]
    public sealed class LaplaceDistribution : LaplaceDistribution<IGenerator>
    {
        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="LaplaceDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        public LaplaceDistribution() : base(new XorShift128Generator(), DefaultAlpha, DefaultMu)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Mu, DefaultMu));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="LaplaceDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        [CLSCompliant(false)]
        public LaplaceDistribution(uint seed) : base(new XorShift128Generator(seed), DefaultAlpha, DefaultMu)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Mu, DefaultMu));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="LaplaceDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        public LaplaceDistribution(IGenerator generator) : base(generator, DefaultAlpha, DefaultMu)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Mu, DefaultMu));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="LaplaceDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of laplace distributed random numbers.
        /// </param>
        /// <param name="mu">
        ///   The parameter mu which is used for generation of laplace distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero.
        /// </exception>
        public LaplaceDistribution(double alpha, double mu) : base(new XorShift128Generator(), alpha, mu)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Mu, mu));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="LaplaceDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of laplace distributed random numbers.
        /// </param>
        /// <param name="mu">
        ///   The parameter mu which is used for generation of laplace distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero.
        /// </exception>
        [CLSCompliant(false)]
        public LaplaceDistribution(uint seed, double alpha, double mu) : base(new XorShift128Generator(seed), alpha, mu)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Mu, mu));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="LaplaceDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of laplace distributed random numbers.
        /// </param>
        /// <param name="mu">
        ///   The parameter mu which is used for generation of laplace distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero.
        /// </exception>
        public LaplaceDistribution(IGenerator generator, double alpha, double mu) : base(generator, alpha, mu)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Mu, mu));
        }

        #endregion
    }
}