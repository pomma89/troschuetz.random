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

namespace Troschuetz.Random.Distributions.Discrete
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using Generators;

    /// <summary>
    ///   Provides generation of discrete uniformly distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The discrete uniform distribution generates only discrete numbers.<br />
    ///   The implementation of the <see cref="DiscreteUniformDistribution"/> type bases upon information presented on
    ///   <a href="http://en.wikipedia.org/wiki/Uniform_distribution_%28discrete%29">
    ///   Wikipedia - Uniform distribution (discrete)</a>.
    /// </remarks>
    [Serializable]
    public class DiscreteUniformDistribution<TGen> : Distribution<TGen>, IDiscreteDistribution, IAlphaDistribution<int>,
                                                            IBetaDistribution<int>
        where TGen : IGenerator
    {
        #region Class Fields

        /// <summary>
        ///   The default value assigned to <see cref="Alpha"/> if none is specified. 
        /// </summary>
        public const int DefaultAlpha = 0;

        /// <summary>
        ///   The default value assigned to <see cref="Beta"/> if none is specified. 
        /// </summary>
        public const int DefaultBeta = 1;

        #endregion

        #region Instance Fields

        /// <summary>
        ///   Stores the parameter beta which is used for generation of uniformly distributed random numbers.
        /// </summary>
        int _beta;
        
        /// <summary>
        ///   Stores the parameter beta which is used for generation of uniformly distributed random numbers.
        /// </summary>
        int _alpha;

        /// <summary>
        ///   Gets or sets the parameter alpha which is used for generation of uniformly distributed random numbers.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value"/> is greater than <see cref="Beta"/>.
        /// </exception>
        /// <remarks>
        ///   Calls <see cref="AreValidParams"/> to determine whether a value is valid and therefore assignable.
        /// </remarks>
        public int Alpha
        {
            get { return _alpha; }
            set { _alpha = value; }
        }

        /// <summary>
        ///   Gets or sets the parameter beta which is used for generation of uniformly distributed random numbers.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <see cref="Alpha"/> is greater than <paramref name="value"/>,
        ///   or <paramref name="value"/> is equal to <see cref="int.MaxValue"/>.
        /// </exception>
        /// <remarks>
        ///   Calls <see cref="AreValidParams"/> to determine whether a value is valid and therefore assignable.
        /// </remarks>
        public int Beta
        {
            get { return _beta; }
            set { _beta = value; }
        }

        #endregion

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="DiscreteUniformDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of discrete uniform distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of discrete uniform distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is greater than <paramref name="beta"/>,
        ///   or <paramref name="beta"/> is equal to <see cref="int.MaxValue"/>.
        /// </exception>
        public DiscreteUniformDistribution(TGen generator, int alpha, int beta) : base(generator)
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
        ///   <see langword="true"/> if value is less than or equal to <see cref="Beta"/>;
        ///   otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsValidAlpha(int value)
        {
            return AreValidParams(value, _beta);
        }

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Beta"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>
        ///   <see langword="true"/> if value is greater than or equal to <see cref="Alpha"/>, and less than 
        ///   <see cref="int.MaxValue"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsValidBeta(int value)
        {
            return AreValidParams(_alpha, value);
        }

        #endregion

        #region IDiscreteDistribution Members

        public double Minimum
        {
            get { return Alpha; }
        }

        public double Maximum
        {
            get { return _beta; }
        }

        public double Mean
        {
            get { return Alpha/2.0 + _beta/2.0; }
        }

        public double Median
        {
            get { return Alpha/2.0 + _beta/2.0; }
        }

        public double Variance
        {
            get { return (Math.Pow(_beta - Alpha + 1.0, 2.0) - 1.0)/12.0; }
        }

        public double[] Mode
        {
            get { throw new NotSupportedException(ErrorMessages.UndefinedMode); }
        }

        public int Next()
        {
            return Sample(Gen, _alpha, _beta);
        }

        public double NextDouble()
        {
            return Sample(Gen, _alpha, _beta);
        }

        #endregion

        #region TRandom Helpers

        /// <summary>
        ///   Determines whether discrete uniform distribution is defined under given parameters.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of discrete uniform distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of discrete uniform distributed random numbers.
        /// </param>
        /// <returns>
        ///   True if <paramref name="alpha"/> is less than or equal to <paramref name="beta"/>,
        ///   and if <paramref name="beta"/> is less than <see cref="int.MaxValue"/>; otherwise, it returns false.
        /// </returns>
        [System.Diagnostics.Contracts.Pure]
        public static bool AreValidParams(int alpha, int beta)
        {
            return alpha <= beta && beta < int.MaxValue;
        }

        /// <summary>
        ///   Returns a discrete uniform distributed 32-bit signed integer.
        /// </summary>
        /// <param name="generator">The generator from which random number are drawn.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of discrete uniform distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of discrete uniform distributed random numbers.
        /// </param>
        /// <returns>
        ///   A discrete uniform distributed 32-bit signed integer.
        /// </returns>
        [System.Diagnostics.Contracts.Pure]
        internal static int Sample(TGen generator, int alpha, int beta)
        {
            return generator.Next(alpha, beta + 1);
        }

        #endregion
    }

    /// <summary>
    ///   Provides generation of discrete uniformly distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The discrete uniform distribution generates only discrete numbers.<br />
    ///   The implementation of the <see cref="DiscreteUniformDistribution"/> type bases upon information presented on
    ///   <a href="http://en.wikipedia.org/wiki/Uniform_distribution_%28discrete%29">
    ///   Wikipedia - Uniform distribution (discrete)</a>.
    /// </remarks>
    [Serializable]
    public sealed class DiscreteUniformDistribution : DiscreteUniformDistribution<IGenerator>
    {
        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="DiscreteUniformDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> as underlying random number generator. 
        /// </summary>
        public DiscreteUniformDistribution() : this(new XorShift128Generator(), DefaultAlpha, DefaultBeta)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Beta, DefaultBeta));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="DiscreteUniformDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        [CLSCompliant(false)]
        public DiscreteUniformDistribution(uint seed) : this(new XorShift128Generator(seed), DefaultAlpha, DefaultBeta)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Beta, DefaultBeta));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="DiscreteUniformDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        public DiscreteUniformDistribution(IGenerator generator) : this(generator, DefaultAlpha, DefaultBeta)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Beta, DefaultBeta));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="DiscreteUniformDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> as underlying random number generator. 
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of discrete uniform distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of discrete uniform distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is greater than <paramref name="beta"/>,
        ///   or <paramref name="beta"/> is equal to <see cref="int.MaxValue"/>.
        /// </exception>
        public DiscreteUniformDistribution(int alpha, int beta) : this(new XorShift128Generator(), alpha, beta)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Beta, beta));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="DiscreteUniformDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of discrete uniform distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of discrete uniform distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is greater than <paramref name="beta"/>,
        ///   or <paramref name="beta"/> is equal to <see cref="int.MaxValue"/>.
        /// </exception>
        [CLSCompliant(false)]
        public DiscreteUniformDistribution(uint seed, int alpha, int beta)
            : this(new XorShift128Generator(seed), alpha, beta)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Beta, beta));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="DiscreteUniformDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of discrete uniform distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of discrete uniform distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is greater than <paramref name="beta"/>,
        ///   or <paramref name="beta"/> is equal to <see cref="int.MaxValue"/>.
        /// </exception>
        public DiscreteUniformDistribution(IGenerator generator, int alpha, int beta) : base(generator, alpha, beta)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Beta, beta));
        }

        #endregion
    }
}