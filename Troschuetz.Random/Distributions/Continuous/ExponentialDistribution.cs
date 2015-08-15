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
    using Core;
    using Generators;
    using PommaLabs.Thrower;
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    /// <summary>
    ///   Provides generation of exponential distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="ExponentialDistribution"/> type bases upon
    ///   information presented on
    ///   <a href="http://en.wikipedia.org/wiki/Exponential_distribution">Wikipedia - Exponential distribution</a>.
    /// </remarks>
    [Serializable]
    public class ExponentialDistribution<TGen> : Distribution<TGen>, IContinuousDistribution, ILambdaDistribution<double>
        where TGen : IGenerator
    {
        #region Class Fields

        /// <summary>
        ///   The default value assigned to <see cref="Lambda"/> if none is specified.
        /// </summary>
        public const double DefaultLambda = 1;

        #endregion Class Fields

        #region Instance Fields

        /// <summary>
        ///   Stores the parameter lambda which is used for generation of exponential distributed
        ///   random numbers.
        /// </summary>
        double _lambda;

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
                Raise<ArgumentOutOfRangeException>.IfNot(IsValidLambda(value), ErrorMessages.InvalidParams);
                _lambda = value;
            }
        }

        #endregion Instance Fields

        #region Construction

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
        public ExponentialDistribution(TGen generator, double lambda) : base(generator)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(IsValidParam(lambda), ErrorMessages.InvalidParams);
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

        public double Minimum => 0.0;

        public double Maximum => double.PositiveInfinity;

        public double Mean => 1.0 / _lambda;

        public double Median => Math.Log(2.0) / _lambda;

        public double Variance => Math.Pow(_lambda, -2.0);

        public double[] Mode => new[] { 0.0 };

        public double NextDouble() => Sample(Gen, _lambda);

        #endregion IContinuousDistribution Members

        #region TRandom Helpers

        /// <summary>
        ///   Determines whether exponential distribution is defined under given parameter.
        /// </summary>
        /// <param name="lambda">
        ///   The parameter lambda which is used for generation of exponential distributed random numbers.
        /// </param>
        /// <returns>
        ///   True if <paramref name="lambda"/> is greater than zero; otherwise, it returns false.
        /// </returns>
        [Pure]
        public static bool IsValidParam(double lambda) => lambda > 0;

        /// <summary>
        ///   Returns an exponential distributed floating point random number.
        /// </summary>
        /// <param name="generator">The generator from which random number are drawn.</param>
        /// <param name="lambda">
        ///   The parameter lambda which is used for generation of exponential distributed random numbers.
        /// </param>
        /// <returns>An exponential distributed floating point random number.</returns>
        [Pure]
        internal static double Sample(TGen generator, double lambda)
        {
            return (-1.0 / lambda) * Math.Log(1.0 - generator.NextDouble());
        }

        #endregion TRandom Helpers
    }

    /// <summary>
    ///   Provides generation of exponential distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="ExponentialDistribution"/> type bases upon
    ///   information presented on
    ///   <a href="http://en.wikipedia.org/wiki/Exponential_distribution">Wikipedia - Exponential distribution</a>.
    /// </remarks>
    [Serializable]
    public sealed class ExponentialDistribution : ExponentialDistribution<IGenerator>
    {
        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="ExponentialDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        public ExponentialDistribution() : base(new XorShift128Generator(), DefaultLambda)
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
        [CLSCompliant(false)]
        public ExponentialDistribution(uint seed) : base(new XorShift128Generator(seed), DefaultLambda)
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
        public ExponentialDistribution(IGenerator generator) : base(generator, DefaultLambda)
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
        public ExponentialDistribution(double lambda) : base(new XorShift128Generator(), lambda)
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
        [CLSCompliant(false)]
        public ExponentialDistribution(uint seed, double lambda) : base(new XorShift128Generator(seed), lambda)
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
        public ExponentialDistribution(IGenerator generator, double lambda) : base(generator, lambda)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Lambda, lambda));
        }

        #endregion Construction
    }
}
