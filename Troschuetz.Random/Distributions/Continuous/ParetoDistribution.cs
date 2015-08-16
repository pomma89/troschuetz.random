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

namespace Troschuetz.Random.Distributions.Continuous
{
    using Core;
    using Generators;
    using PommaLabs.Thrower;
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    /// <summary>
    ///   Provides generation of pareto distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="ParetoDistribution"/> type bases upon information
    ///   presented on <a href="http://en.wikipedia.org/wiki/Pareto_distribution">Wikipedia - Pareto
    ///   distribution</a> and <a href="http://www.xycoon.com/par_random.htm">Xycoon - Pareto Distribution</a>.
    /// </remarks>
    [Serializable]
    public class ParetoDistribution<TGen> : AbstractDistribution<TGen>, IContinuousDistribution, IAlphaDistribution<double>,
                                            IBetaDistribution<double>
        where TGen : IGenerator
    {
        #region Constants

        /// <summary>
        ///   The default value assigned to <see cref="Alpha"/> if none is specified.
        /// </summary>
        public const double DefaultAlpha = 1;

        /// <summary>
        ///   The default value assigned to <see cref="Beta"/> if none is specified.
        /// </summary>
        public const double DefaultBeta = 1;

        #endregion Constants

        #region Fields

        /// <summary>
        ///   Stores the parameter beta which is used for generation of pareto distributed random numbers.
        /// </summary>
        double _beta;

        /// <summary>
        ///   Stores the parameter alpha which is used for generation of pareto distributed random numbers.
        /// </summary>
        double _alpha;

        /// <summary>
        ///   Gets or sets the parameter alpha which is used for generation of pareto distributed
        ///   random numbers.
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
            set
            {
                Raise<ArgumentOutOfRangeException>.IfNot(IsValidAlpha(value), ErrorMessages.InvalidParams);
                _alpha = value;
            }
        }

        /// <summary>
        ///   Gets or sets the parameter beta which is used for generation of pareto distributed
        ///   random numbers.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   Calls <see cref="AreValidParams"/> to determine whether a value is valid and therefore assignable.
        /// </remarks>
        public double Beta
        {
            get { return _beta; }
            set
            {
                Raise<ArgumentOutOfRangeException>.IfNot(IsValidBeta(value), ErrorMessages.InvalidParams);
                _beta = value;
            }
        }

        #endregion Fields

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="ParetoDistribution"/> class, using the
        ///   specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of pareto distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of pareto distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="beta"/> are less than or equal to zero.
        /// </exception>
        public ParetoDistribution(TGen generator, double alpha, double beta) : base(generator)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(AreValidParams(alpha, beta), ErrorMessages.InvalidParams);
            _alpha = alpha;
            _beta = beta;
        }

        #endregion Construction

        #region Instance Methods

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Alpha"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns><see langword="true"/> if value is greater than 0.0; otherwise, <see langword="false"/>.</returns>
        public bool IsValidAlpha(double value)
        {
            return AreValidParams(value, _beta);
        }

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Beta"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns><see langword="true"/> if value is greater than 0.0; otherwise, <see langword="false"/>.</returns>
        public bool IsValidBeta(double value)
        {
            return AreValidParams(_alpha, value);
        }

        #endregion Instance Methods

        #region IContinuousDistribution Members

        public double Minimum
        {
            get { return Alpha; }
        }

        public double Maximum
        {
            get { return double.PositiveInfinity; }
        }

        public double Mean
        {
            get
            {
                if (_beta > 1.0)
                {
                    return Alpha * _beta / (_beta - 1.0);
                }
                throw new NotSupportedException(ErrorMessages.UndefinedMeanForParams);
            }
        }

        public double Median
        {
            get { return Alpha * Math.Pow(2.0, 1.0 / _beta); }
        }

        public double Variance
        {
            get
            {
                if (_beta > 2.0)
                {
                    return _beta * Math.Pow(Alpha, 2.0) / Math.Pow(_beta - 1.0, 2.0) / (_beta - 2.0);
                }
                throw new NotSupportedException(ErrorMessages.UndefinedVarianceForParams);
            }
        }

        public double[] Mode
        {
            get { return new[] { Alpha }; }
        }

        public double NextDouble()
        {
            return Sample(TypedGenerator, _alpha, _beta);
        }

        #endregion IContinuousDistribution Members

        #region TRandom Helpers

        /// <summary>
        ///   Determines whether pareto distribution is defined under given parameters.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of pareto distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of pareto distributed random numbers.
        /// </param>
        /// <returns>
        ///   True if <paramref name="alpha"/> and <paramref name="beta"/> are greater than zero;
        ///   otherwise, it returns false.
        /// </returns>
        [Pure]
        public static bool AreValidParams(double alpha, double beta)
        {
            return alpha > 0 && beta > 0;
        }

        /// <summary>
        ///   Returns a pareto distributed floating point random number.
        /// </summary>
        /// <param name="generator">The generator from which random number are drawn.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of pareto distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of pareto distributed random numbers.
        /// </param>
        /// <returns>A pareto distributed floating point random number.</returns>
        [Pure]
        internal static double Sample(TGen generator, double alpha, double beta)
        {
            var helper1 = 1.0 / beta;
            return alpha / Math.Pow(1.0 - generator.NextDouble(), helper1);
        }

        #endregion TRandom Helpers
    }

    /// <summary>
    ///   Provides generation of pareto distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="ParetoDistribution"/> type bases upon information
    ///   presented on <a href="http://en.wikipedia.org/wiki/Pareto_distribution">Wikipedia - Pareto
    ///   distribution</a> and <a href="http://www.xycoon.com/par_random.htm">Xycoon - Pareto Distribution</a>.
    /// </remarks>
    [Serializable]
    public sealed class ParetoDistribution : ParetoDistribution<IGenerator>
    {
        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="ParetoDistribution"/> class, using a
        ///   <see cref="NumericalRecipes3Q1Generator"/> as underlying random number generator.
        /// </summary>
        public ParetoDistribution() : base(new NumericalRecipes3Q1Generator(), DefaultAlpha, DefaultBeta)
        {
            Debug.Assert(Generator is NumericalRecipes3Q1Generator);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Beta, DefaultBeta));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ParetoDistribution"/> class, using a
        ///   <see cref="NumericalRecipes3Q1Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        public ParetoDistribution(uint seed) : base(new NumericalRecipes3Q1Generator(seed), DefaultAlpha, DefaultBeta)
        {
            Debug.Assert(Generator is NumericalRecipes3Q1Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Beta, DefaultBeta));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ParetoDistribution"/> class, using the
        ///   specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        public ParetoDistribution(IGenerator generator) : base(generator, DefaultAlpha, DefaultBeta)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Beta, DefaultBeta));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ParetoDistribution"/> class, using a
        ///   <see cref="NumericalRecipes3Q1Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of pareto distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of pareto distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="beta"/> are less than or equal to zero.
        /// </exception>
        public ParetoDistribution(double alpha, double beta) : base(new NumericalRecipes3Q1Generator(), alpha, beta)
        {
            Debug.Assert(Generator is NumericalRecipes3Q1Generator);
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Beta, beta));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ParetoDistribution"/> class, using a
        ///   <see cref="NumericalRecipes3Q1Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of pareto distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of pareto distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="beta"/> are less than or equal to zero.
        /// </exception>
        public ParetoDistribution(uint seed, double alpha, double beta)
            : base(new NumericalRecipes3Q1Generator(seed), alpha, beta)
        {
            Debug.Assert(Generator is NumericalRecipes3Q1Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Beta, beta));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ParetoDistribution"/> class, using the
        ///   specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of pareto distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of pareto distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="beta"/> are less than or equal to zero.
        /// </exception>
        public ParetoDistribution(IGenerator generator, double alpha, double beta) : base(generator, alpha, beta)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Beta, beta));
        }

        #endregion Construction
    }
}
