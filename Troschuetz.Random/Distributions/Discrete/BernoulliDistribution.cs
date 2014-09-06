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
    ///   Provides generation of bernoulli distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The bernoulli distribution generates only discrete numbers.<br />
    ///   The implementation of the <see cref="BernoulliDistribution"/> type bases upon information presented on
    ///   <a href="http://en.wikipedia.org/wiki/Bernoulli_distribution">Wikipedia - Bernoulli distribution</a>.
    /// </remarks>
    [Serializable]
    public class BernoulliDistribution<TGen> : Distribution<TGen>, IDiscreteDistribution, IAlphaDistribution<double>
        where TGen : IGenerator
    {
        #region Class Fields

        /// <summary>
        ///   The default value assigned to <see cref="Alpha"/> if none is specified. 
        /// </summary>
        public const double DefaultAlpha = 0.5;

        #endregion

        #region Instance Fields

        /// <summary>
        ///   Stores the parameter alpha which is used for generation of bernoulli distributed random numbers.
        /// </summary>
        double _alpha;

        /// <summary>
        ///   Gets or sets the parameter alpha which is used for generation of bernoulli distributed random numbers.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value"/> is less than zero or greater than one.
        /// </exception>
        /// <remarks>
        ///   Calls <see cref="IsValidParam"/> to determine whether a value is valid and therefore assignable.
        /// </remarks>
        public double Alpha
        {
            get { return _alpha; }
            set { _alpha = value; }
        }

        #endregion

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="BernoulliDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of bernoulli distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than zero or greater than one.
        /// </exception>
        public BernoulliDistribution(TGen generator, double alpha) : base(generator)
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
        ///   <see langword="true"/> if value is greater than or equal to 0.0, and less than or equal to 1.0;
        ///   otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsValidAlpha(double value)
        {
            return IsValidParam(value);
        }

        #endregion

        #region IDiscreteDistribution Members

        public double Minimum
        {
            get { return 0.0; }
        }

        public double Maximum
        {
            get { return 1.0; }
        }

        public double Mean
        {
            get { return Alpha; }
        }

        public double Median
        {
            get { throw new NotSupportedException(ErrorMessages.UndefinedMedian); }
        }

        public double Variance
        {
            get { return Alpha*(1.0 - Alpha); }
        }

        public double[] Mode
        {
            get
            {
                if (Alpha > (1 - Alpha)) {
                    return new[] {1.0};
                }
                return Alpha < (1 - Alpha) ? new[] {0.0} : new[] {0.0, 1.0};
            }
        }

        public int Next()
        {
            return Sample(Gen, _alpha);
        }

        public double NextDouble()
        {
            return Sample(Gen, _alpha);
        }

        #endregion

        #region TRandom Helpers

        /// <summary>
        ///   Determines whether bernoulli distribution is defined under given parameter.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of bernoulli distributed random numbers.
        /// </param>
        /// <returns>
        ///   True if <paramref name="alpha"/> is greater than or equal to zero
        ///   and less than or equal to one; otherwise, it returns false.
        /// </returns>
        [System.Diagnostics.Contracts.Pure]
        public static bool IsValidParam(double alpha)
        {
            return alpha >= 0.0 && alpha <= 1.0;
        }

        /// <summary>
        ///   Returns a bernoulli distributed 32-bit signed integer.
        /// </summary>
        /// <param name="generator">The generator from which random number are drawn.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of bernoulli distributed random numbers.
        /// </param>
        /// <returns>
        ///   A bernoulli distributed 32-bit signed integer.
        /// </returns>
        [System.Diagnostics.Contracts.Pure]
        internal static int Sample(TGen generator, double alpha)
        {
            return generator.NextDouble() < alpha ? 1 : 0;
        }

        #endregion
    }

    /// <summary>
    ///   Provides generation of bernoulli distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The bernoulli distribution generates only discrete numbers.<br />
    ///   The implementation of the <see cref="BernoulliDistribution"/> type bases upon information presented on
    ///   <a href="http://en.wikipedia.org/wiki/Bernoulli_distribution">Wikipedia - Bernoulli distribution</a>.
    /// </remarks>
    [Serializable]
    public sealed class BernoulliDistribution : BernoulliDistribution<IGenerator>
    {
        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="BernoulliDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        public BernoulliDistribution() : base(new XorShift128Generator(), DefaultAlpha)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="BernoulliDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        [CLSCompliant(false)]
        public BernoulliDistribution(uint seed) : base(new XorShift128Generator(seed), DefaultAlpha)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="BernoulliDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        public BernoulliDistribution(IGenerator generator) : base(generator, DefaultAlpha)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Alpha, DefaultAlpha));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="BernoulliDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of bernoulli distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than zero or greater than one.
        /// </exception>
        public BernoulliDistribution(double alpha) : base(new XorShift128Generator(), alpha)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, alpha));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="BernoulliDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of bernoulli distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than zero or greater than one.
        /// </exception>
        [CLSCompliant(false)]
        public BernoulliDistribution(uint seed, double alpha) : base(new XorShift128Generator(seed), alpha)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, alpha));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="BernoulliDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of bernoulli distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than zero or greater than one.
        /// </exception>
        public BernoulliDistribution(IGenerator generator, double alpha) : base(generator, alpha)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Alpha, alpha));
        }

        #endregion
    }
}