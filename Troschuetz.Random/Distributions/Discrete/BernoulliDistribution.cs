/*
 * Copyright © 2006 Stefan Troschütz (stefan@troschuetz.de)
 * Copyright © 2012-2016 Alessio Parma (alessio.parma@gmail.com)
 *
 * This file is part of Troschuetz.Random Class Library.
 *
 * Troschuetz.Random is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *
 * See the GNU Lesser General Public License for more details.
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
 */

namespace Troschuetz.Random.Distributions.Discrete
{
    using Core;
    using Generators;
    using PommaLabs.Thrower;
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    /// <summary>
    ///   Provides generation of bernoulli distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The bernoulli distribution generates only discrete numbers. <br/> The implementation of
    ///   the <see cref="BernoulliDistribution"/> type bases upon information presented on
    ///   <a href="http://en.wikipedia.org/wiki/Bernoulli_distribution">Wikipedia - Bernoulli distribution</a>.
    /// </remarks>
    [Serializable]
    public class BernoulliDistribution<TGen> : AbstractDistribution<TGen>, IDiscreteDistribution, IAlphaDistribution<double>
        where TGen : IGenerator
    {
        #region Constants

        /// <summary>
        ///   The default value assigned to <see cref="Alpha"/> if none is specified.
        /// </summary>
        public const double DefaultAlpha = 0.5;

        #endregion Constants

        #region Fields

        /// <summary>
        ///   Stores the parameter alpha which is used for generation of bernoulli distributed
        ///   random numbers.
        /// </summary>
        double _alpha;

        /// <summary>
        ///   Gets or sets the parameter alpha which is used for generation of bernoulli distributed
        ///   random numbers.
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
            set
            {
                Raise<ArgumentOutOfRangeException>.IfNot(IsValidAlpha(value), ErrorMessages.InvalidParams);
                _alpha = value;
            }
        }

        #endregion Fields

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="BernoulliDistribution"/> class, using the
        ///   specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of bernoulli distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than zero or greater than one.
        /// </exception>
        public BernoulliDistribution(TGen generator, double alpha) : base(generator)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(IsValidParam(alpha), ErrorMessages.InvalidParams);
            _alpha = alpha;
        }

        #endregion Construction

        #region Instance Methods

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Alpha"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>
        ///   <see langword="true"/> if value is greater than or equal to 0.0, and less than or
        ///   equal to 1.0; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsValidAlpha(double value) => IsValidParam(value);

        #endregion Instance Methods

        #region IDiscreteDistribution Members

        /// <summary>
        ///   Gets the minimum possible value of distributed random numbers.
        /// </summary>
        public double Minimum => 0.0;

        /// <summary>
        ///   Gets the maximum possible value of distributed random numbers.
        /// </summary>
        public double Maximum => 1.0;

        /// <summary>
        ///   Gets the mean of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if mean is not defined for given distribution with some parameters.
        /// </exception>
        public double Mean => Alpha;

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
        public double Variance => Alpha * (1.0 - Alpha);

        /// <summary>
        ///   Gets the mode of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if mode is not defined for given distribution with some parameters.
        /// </exception>
        public double[] Mode
        {
            get
            {
                if (Alpha > (1 - Alpha))
                {
                    return new[] { 1.0 };
                }
                return Alpha < (1 - Alpha) ? new[] { 0.0 } : new[] { 0.0, 1.0 };
            }
        }

        /// <summary>
        ///   Returns a distributed random number.
        /// </summary>
        /// <returns>A distributed 32-bit signed integer.</returns>
        public int Next() => Sample(TypedGenerator, _alpha);

        /// <summary>
        ///   Returns a distributed floating point random number.
        /// </summary>
        /// <returns>A distributed double-precision floating point number.</returns>
        public double NextDouble() => Sample(TypedGenerator, _alpha);

        #endregion IDiscreteDistribution Members

        #region TRandom Helpers

        /// <summary>
        ///   Determines whether bernoulli distribution is defined under given parameter. The
        ///   default definition returns true if alpha is greater than or equal to zero and less than or
        ///   equal to one; otherwise, it returns false.
        /// </summary>
        /// <remarks>
        ///   This is an extensibility point for the <see cref="BernoulliDistribution{TGen}"/> class.
        /// </remarks>
        public static Func<double, bool> IsValidParam { get; set; } = alpha =>
        {
            return alpha >= 0.0 && alpha <= 1.0;
        };

        /// <summary>
        ///   Declares a function returning a bernoulli distributed 32-bit signed integer.
        /// </summary>
        /// <remarks>
        ///   This is an extensibility point for the <see cref="BernoulliDistribution{TGen}"/> class.
        /// </remarks>
        public static Func<TGen, double, int> Sample { get; set; } = (generator, alpha) =>
        {
            return generator.NextDouble() < alpha ? 1 : 0;
        };

        #endregion TRandom Helpers
    }

    /// <summary>
    ///   Provides generation of bernoulli distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The bernoulli distribution generates only discrete numbers. <br/> The implementation of
    ///   the <see cref="BernoulliDistribution"/> type bases upon information presented on
    ///   <a href="http://en.wikipedia.org/wiki/Bernoulli_distribution">Wikipedia - Bernoulli distribution</a>.
    /// </remarks>
    [Serializable]
    public sealed class BernoulliDistribution : BernoulliDistribution<IGenerator>
    {
        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="BernoulliDistribution"/> class, using a
        ///   <see cref="NumericalRecipes3Q1Generator"/> as underlying random number generator.
        /// </summary>
        public BernoulliDistribution() : base(new NumericalRecipes3Q1Generator(), DefaultAlpha)
        {
            Debug.Assert(Generator is NumericalRecipes3Q1Generator);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="BernoulliDistribution"/> class, using a
        ///   <see cref="NumericalRecipes3Q1Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        public BernoulliDistribution(uint seed) : base(new NumericalRecipes3Q1Generator(seed), DefaultAlpha)
        {
            Debug.Assert(Generator is NumericalRecipes3Q1Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="BernoulliDistribution"/> class, using the
        ///   specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        public BernoulliDistribution(IGenerator generator) : base(generator, DefaultAlpha)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Alpha, DefaultAlpha));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="BernoulliDistribution"/> class, using a
        ///   <see cref="NumericalRecipes3Q1Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of bernoulli distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than zero or greater than one.
        /// </exception>
        public BernoulliDistribution(double alpha) : base(new NumericalRecipes3Q1Generator(), alpha)
        {
            Debug.Assert(Generator is NumericalRecipes3Q1Generator);
            Debug.Assert(Equals(Alpha, alpha));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="BernoulliDistribution"/> class, using a
        ///   <see cref="NumericalRecipes3Q1Generator"/> with the specified seed value.
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
        public BernoulliDistribution(uint seed, double alpha) : base(new NumericalRecipes3Q1Generator(seed), alpha)
        {
            Debug.Assert(Generator is NumericalRecipes3Q1Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, alpha));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="BernoulliDistribution"/> class, using the
        ///   specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of bernoulli distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than zero or greater than one.
        /// </exception>
        public BernoulliDistribution(IGenerator generator, double alpha) : base(generator, alpha)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Alpha, alpha));
        }

        #endregion Construction
    }
}
