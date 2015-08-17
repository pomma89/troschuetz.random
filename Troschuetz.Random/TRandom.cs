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

namespace Troschuetz.Random
{
    using Core;
    using Distributions.Continuous;
    using Distributions.Discrete;
    using Generators;
    using PommaLabs.Thrower;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    /// <summary>
    ///   A random generator class similar to the one Python offers, providing functions similar to
    ///   the ones found in <see cref="System.Random"/> and functions returning random numbers
    ///   according to a particular kind of distribution.
    /// </summary>
    [Serializable]
    public sealed class TRandom : IGenerator
    {
        #region Fields

        /// <summary>
        ///   The generator used by <see cref="TRandom"/>.
        /// </summary>
        readonly IGenerator _gen;

        #endregion Fields

        #region Construction

        /// <summary>
        ///   Constructs a new instance with <see cref="NumericalRecipes3Q1Generator"/> as
        ///   underlying generator and the default seed (which corresponds to <see cref="Environment.TickCount"/>).
        /// </summary>
        public TRandom() : this(new NumericalRecipes3Q1Generator(Environment.TickCount))
        {
        }

        /// <summary>
        ///   Constructs a new instance with <see cref="NumericalRecipes3Q1Generator"/> as
        ///   underlying generator and the specified seed.
        /// </summary>
        /// <param name="seed">The seed used to initialize the generator.</param>
        public TRandom(int seed) : this(new NumericalRecipes3Q1Generator(seed))
        {
        }

        /// <summary>
        ///   Constructs a new instance with <see cref="NumericalRecipes3Q1Generator"/> as
        ///   underlying generator and the specified seed.
        /// </summary>
        /// <param name="seed">The seed used to initialize the generator.</param>
        public TRandom(uint seed) : this(new NumericalRecipes3Q1Generator(seed))
        {
        }

        /// <summary>
        ///   Constructs a new instance with the specified generator.
        /// </summary>
        /// <param name="generator">The generator used to produce random numbers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is null.</exception>
        public TRandom(IGenerator generator)
        {
            RaiseArgumentNullException.IfIsNull(generator, nameof(generator));
            _gen = generator;
        }

        /// <summary>
        ///   Constructs a new instance with <see cref="NumericalRecipes3Q1Generator"/> as
        ///   underlying generator and the default seed (which corresponds to <see cref="Environment.TickCount"/>).
        /// </summary>
        public static TRandom New() => new TRandom(new NumericalRecipes3Q1Generator(Environment.TickCount));

        /// <summary>
        ///   Constructs a new instance with <see cref="NumericalRecipes3Q1Generator"/> as
        ///   underlying generator and the specified seed.
        /// </summary>
        /// <param name="seed">The seed used to initialize the generator.</param>
        public static TRandom New(int seed) => new TRandom(new NumericalRecipes3Q1Generator(seed));

        /// <summary>
        ///   Constructs a new instance with <see cref="NumericalRecipes3Q1Generator"/> as
        ///   underlying generator and the specified seed.
        /// </summary>
        /// <param name="seed">The seed used to initialize the generator.</param>
        public static TRandom New(uint seed) => new TRandom(new NumericalRecipes3Q1Generator(seed));

        /// <summary>
        ///   Constructs a new instance with the specified generator.
        /// </summary>
        /// <param name="generator">The generator used to produce random numbers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is null.</exception>
        public static TRandom New(IGenerator generator) => new TRandom(generator);

        #endregion Construction

        #region Discrete Distributions

        /// <summary>
        ///   Returns a bernoulli distributed 32-bit signed integer.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of bernoulli distributed random numbers.
        /// </param>
        /// <returns>A bernoulli distributed 32-bit signed integer.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than zero or greater than one.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="BernoulliDistribution.Next"/>, with a
        ///   prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="BernoulliDistribution"/> for each group of parameters.
        /// </remarks>
        public int Bernoulli(double alpha)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(BernoulliDistribution.IsValidParam(alpha), ErrorMessages.InvalidParams);
            return BernoulliDistribution.Sample(_gen, alpha);
        }

        /// <summary>
        ///   Returns an infinite sequence of bernoulli distributed 32-bit signed integers.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of bernoulli distributed random numbers.
        /// </param>
        /// <returns>An infinite sequence of bernoulli distributed 32-bit signed integers.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than zero or greater than one.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="BernoulliDistribution.Next"/>, with a
        ///   prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="BernoulliDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<int> BernoulliSamples(double alpha)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(BernoulliDistribution.IsValidParam(alpha), ErrorMessages.InvalidParams);
            return InfiniteLoop(BernoulliDistribution.Sample, _gen, alpha);
        }

        /// <summary>
        ///   Returns a binomial distributed 32-bit signed integer.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of binomial distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of binomial distributed random numbers.
        /// </param>
        /// <returns>A binomial distributed 32-bit signed integer.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than zero or greater than one, or
        ///   <paramref name="beta"/> is less than zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="BinomialDistribution.Next"/>, with a
        ///   prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="BinomialDistribution"/> for each group of parameters.
        /// </remarks>
        public int Binomial(double alpha, int beta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(BinomialDistribution.AreValidParams(alpha, beta), ErrorMessages.InvalidParams);
            return BinomialDistribution.Sample(_gen, alpha, beta);
        }

        /// <summary>
        ///   Returns an infinite sequence of binomial distributed 32-bit signed integers.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of binomial distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of binomial distributed random numbers.
        /// </param>
        /// <returns>An infinite sequence of binomial distributed 32-bit signed integers.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than zero or greater than one, or
        ///   <paramref name="beta"/> is less than zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="BinomialDistribution.Next"/>, with a
        ///   prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="BinomialDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<int> BinomialSamples(double alpha, int beta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(BinomialDistribution.AreValidParams(alpha, beta), ErrorMessages.InvalidParams);
            return InfiniteLoop(BinomialDistribution.Sample, _gen, alpha, beta);
        }

        /// <summary>
        ///   Returns a categorical distributed 32-bit signed integer.
        /// </summary>
        /// <param name="valueCount">
        ///   The parameter valueCount which is used for generation of binomial distributed random
        ///   numbers by setting the number of equi-distributed "weights" the distribution will have.
        /// </param>
        /// <returns>A categorical distributed 32-bit signed integer.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="valueCount"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="CategoricalDistribution.Next"/>, with a
        ///   prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="CategoricalDistribution"/> for each group of parameters.
        /// </remarks>
        public int Categorical(int valueCount)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(valueCount > 0, ErrorMessages.InvalidParams);
            double[] cdf;
            double weightsSum;
            CategoricalDistribution.SetUp(valueCount, null, out cdf, out weightsSum);
            return CategoricalDistribution.Sample(_gen, valueCount, cdf, weightsSum);
        }

        /// <summary>
        ///   Returns an infinite sequence of categorical distributed 32-bit signed integers.
        /// </summary>
        /// <param name="valueCount">
        ///   The parameter valueCount which is used for generation of binomial distributed random
        ///   numbers by setting the number of equi-distributed "weights" the distribution will have.
        /// </param>
        /// <returns>An infinite sequence of categorical distributed 32-bit signed integers.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="valueCount"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="CategoricalDistribution.Next"/>, with a
        ///   prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="CategoricalDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<int> CategoricalSamples(int valueCount)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(valueCount > 0, ErrorMessages.InvalidParams);
            double[] cdf;
            double weightsSum;
            CategoricalDistribution.SetUp(valueCount, null, out cdf, out weightsSum);
            return InfiniteLoop(CategoricalDistribution.Sample, _gen, valueCount, cdf, weightsSum);
        }

        /// <summary>
        ///   Returns a categorical distributed 32-bit signed integer.
        /// </summary>
        /// <param name="weights">
        ///   An enumerable of nonnegative weights: this enumerable does not need to be normalized
        ///   as this is often impossible using floating point arithmetic.
        /// </param>
        /// <returns>A categorical distributed 32-bit signed integer.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="weights"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="weights"/> is empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   Any of the weights in <paramref name="weights"/> are negative or they sum to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="CategoricalDistribution.Next"/>, with a
        ///   prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="CategoricalDistribution"/> for each group of parameters.
        /// </remarks>
        public int Categorical(ICollection<double> weights)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(CategoricalDistribution.IsValidParam(weights), ErrorMessages.InvalidParams);
            double[] cdf;
            double weightsSum;
            CategoricalDistribution.SetUp(weights.Count, weights, out cdf, out weightsSum);
            return CategoricalDistribution.Sample(_gen, weights.Count, cdf, weightsSum);
        }

        /// <summary>
        ///   Returns an infinite sequence of categorical distributed 32-bit signed integers.
        /// </summary>
        /// <param name="weights">
        ///   An enumerable of nonnegative weights: this enumerable does not need to be normalized
        ///   as this is often impossible using floating point arithmetic.
        /// </param>
        /// <returns>An infinite sequence of categorical distributed 32-bit signed integers.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="weights"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="weights"/> is empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   Any of the weights in <paramref name="weights"/> are negative or they sum to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="CategoricalDistribution.Next"/>, with a
        ///   prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="CategoricalDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<int> CategoricalSamples(ICollection<double> weights)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(CategoricalDistribution.IsValidParam(weights), ErrorMessages.InvalidParams);
            double[] cdf;
            double weightsSum;
            CategoricalDistribution.SetUp(weights.Count, weights, out cdf, out weightsSum);
            return InfiniteLoop(CategoricalDistribution.Sample, _gen, weights.Count, cdf, weightsSum);
        }

        /// <summary>
        ///   Returns a discrete uniform distributed 32-bit signed integer.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of discrete uniform distributed
        ///   random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of discrete uniform distributed random numbers.
        /// </param>
        /// <returns>A discrete uniform distributed 32-bit signed integer.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is greater than <paramref name="beta"/>, or
        ///   <paramref name="beta"/> is equal to <see cref="int.MaxValue"/>.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="DiscreteUniformDistribution.Next"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="DiscreteUniformDistribution"/> for each group of parameters.
        /// </remarks>
        public int DiscreteUniform(int alpha, int beta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(DiscreteUniformDistribution.AreValidParams(alpha, beta), ErrorMessages.InvalidParams);
            return DiscreteUniformDistribution.Sample(_gen, alpha, beta);
        }

        /// <summary>
        ///   Returns an infinite sequence of discrete uniform distributed 32-bit signed integers.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of discrete uniform distributed
        ///   random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of discrete uniform distributed random numbers.
        /// </param>
        /// <returns>
        ///   An infinite sequence of discrete uniform distributed 32-bit signed integers.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is greater than <paramref name="beta"/>, or
        ///   <paramref name="beta"/> is equal to <see cref="int.MaxValue"/>.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="DiscreteUniformDistribution.Next"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="DiscreteUniformDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<int> DiscreteUniformSamples(int alpha, int beta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(DiscreteUniformDistribution.AreValidParams(alpha, beta), ErrorMessages.InvalidParams);
            return InfiniteLoop(DiscreteUniformDistribution.Sample, _gen, alpha, beta);
        }

        /// <summary>
        ///   Returns a geometric distributed 32-bit signed integer.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of geometric distributed random numbers.
        /// </param>
        /// <returns>A geometric distributed 32-bit signed integer.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero or it is greater than one.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="GeometricDistribution.Next"/>, with a
        ///   prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="GeometricDistribution"/> for each group of parameters.
        /// </remarks>
        public int Geometric(double alpha)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(GeometricDistribution.IsValidParam(alpha), ErrorMessages.InvalidParams);
            return GeometricDistribution.Sample(_gen, alpha);
        }

        /// <summary>
        ///   Returns an infinite sequence of geometric distributed 32-bit signed integers.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of geometric distributed random numbers.
        /// </param>
        /// <returns>An infinite sequence of geometric distributed 32-bit signed integers.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero or it is greater than one.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="GeometricDistribution.Next"/>, with a
        ///   prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="GeometricDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<int> GeometricSamples(double alpha)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(GeometricDistribution.IsValidParam(alpha), ErrorMessages.InvalidParams);
            return InfiniteLoop(GeometricDistribution.Sample, _gen, alpha);
        }

        /// <summary>
        ///   Returns a poisson distributed 32-bit signed integer.
        /// </summary>
        /// <param name="lambda">
        ///   The parameter lambda which is used for generation of poisson distributed random numbers.
        /// </param>
        /// <returns>A poisson distributed 32-bit signed integer.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="lambda"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="PoissonDistribution.Next"/>, with a
        ///   prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="PoissonDistribution"/> for each group of parameters.
        /// </remarks>
        public int Poisson(double lambda)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(PoissonDistribution.IsValidParam(lambda), ErrorMessages.InvalidParams);
            return PoissonDistribution.Sample(_gen, lambda);
        }

        /// <summary>
        ///   Returns an infinite sequence of poisson distributed 32-bit signed integers.
        /// </summary>
        /// <param name="lambda">
        ///   The parameter lambda which is used for generation of poisson distributed random numbers.
        /// </param>
        /// <returns>An infinite sequence of poisson distributed 32-bit signed integers.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="lambda"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="PoissonDistribution.Next"/>, with a
        ///   prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="PoissonDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<int> PoissonSamples(double lambda)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(PoissonDistribution.IsValidParam(lambda), ErrorMessages.InvalidParams);
            return InfiniteLoop(PoissonDistribution.Sample, _gen, lambda);
        }

        #endregion Discrete Distributions

        #region Continuous Distributions

        /// <summary>
        ///   Returns a beta distributed floating point random number.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of beta distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of beta distributed random numbers.
        /// </param>
        /// <returns>A beta distributed floating point random number.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="beta"/> are less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="BetaDistribution.NextDouble"/>, with a
        ///   prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="BetaDistribution"/> for each group of parameters.
        /// </remarks>
        public double Beta(double alpha, double beta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(BetaDistribution.AreValidParams(alpha, beta), ErrorMessages.InvalidParams);
            return BetaDistribution.Sample(_gen, alpha, beta);
        }

        /// <summary>
        ///   Returns an infinite sequence of beta distributed floating point random numbers.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of beta distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of beta distributed random numbers.
        /// </param>
        /// <returns>An infinite sequence of beta distributed floating point random numbers.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="beta"/> are less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="BetaDistribution.NextDouble"/>, with a
        ///   prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="BetaDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> BetaSamples(double alpha, double beta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(BetaDistribution.AreValidParams(alpha, beta), ErrorMessages.InvalidParams);
            return InfiniteLoop(BetaDistribution.Sample, _gen, alpha, beta);
        }

        /// <summary>
        ///   Returns a beta prime distributed floating point random number.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of beta prime distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of beta prime distributed random numbers.
        /// </param>
        /// <returns>A beta prime distributed floating point random number.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="beta"/> are less than or equal to one.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="BetaPrimeDistribution.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="BetaPrimeDistribution"/> for each group of parameters.
        /// </remarks>
        public double BetaPrime(double alpha, double beta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(BetaPrimeDistribution.AreValidParams(alpha, beta), ErrorMessages.InvalidParams);
            return BetaPrimeDistribution.Sample(_gen, alpha, beta);
        }

        /// <summary>
        ///   Returns an infinite sequence of beta prime distributed floating point random numbers.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of beta prime distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of beta prime distributed random numbers.
        /// </param>
        /// <returns>
        ///   An infinite sequence of beta prime distributed floating point random numbers.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="beta"/> are less than or equal to one.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="BetaPrimeDistribution.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="BetaPrimeDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> BetaPrimeSamples(double alpha, double beta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(BetaPrimeDistribution.AreValidParams(alpha, beta), ErrorMessages.InvalidParams);
            return InfiniteLoop(BetaPrimeDistribution.Sample, _gen, alpha, beta);
        }

        /// <summary>
        ///   Returns a cauchy distributed floating point random number.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of cauchy distributed random numbers.
        /// </param>
        /// <param name="gamma">
        ///   The parameter gamma which is used for generation of cauchy distributed random numbers.
        /// </param>
        /// <returns>A cauchy distributed floating point random number.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="gamma"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="CauchyDistribution.NextDouble"/>, with a
        ///   prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="CauchyDistribution"/> for each group of parameters.
        /// </remarks>
        public double Cauchy(double alpha, double gamma)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(CauchyDistribution.AreValidParams(alpha, gamma), ErrorMessages.InvalidParams);
            return CauchyDistribution.Sample(_gen, alpha, gamma);
        }

        /// <summary>
        ///   Returns an infinite sequence of cauchy distributed floating point random numbers.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of cauchy distributed random numbers.
        /// </param>
        /// <param name="gamma">
        ///   The parameter gamma which is used for generation of cauchy distributed random numbers.
        /// </param>
        /// <returns>An infinite sequence of cauchy distributed floating point random numbers.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="gamma"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="CauchyDistribution.NextDouble"/>, with a
        ///   prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="CauchyDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> CauchySamples(double alpha, double gamma)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(CauchyDistribution.AreValidParams(alpha, gamma), ErrorMessages.InvalidParams);
            return InfiniteLoop(CauchyDistribution.Sample, _gen, alpha, gamma);
        }

        /// <summary>
        ///   Returns a chi distributed floating point random number.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of chi distributed random numbers.
        /// </param>
        /// <returns>A chi distributed floating point random number.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="ChiDistribution.NextDouble"/>, with a
        ///   prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="ChiDistribution"/> for each group of parameters.
        /// </remarks>
        public double Chi(int alpha)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(ChiDistribution.IsValidParam(alpha), ErrorMessages.InvalidParams);
            return ChiDistribution.Sample(_gen, alpha);
        }

        /// <summary>
        ///   Returns an infinite sequence of chi distributed floating point random numbers.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of chi distributed random numbers.
        /// </param>
        /// <returns>An infinite sequence of chi distributed floating point random numbers.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="ChiDistribution.NextDouble"/>, with a
        ///   prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="ChiDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> ChiSamples(int alpha)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(ChiDistribution.IsValidParam(alpha), ErrorMessages.InvalidParams);
            return InfiniteLoop(ChiDistribution.Sample, _gen, alpha);
        }

        /// <summary>
        ///   Returns a chi square distributed floating point random number.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of chi square distributed random numbers.
        /// </param>
        /// <returns>A chi square distributed floating point random number.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="ChiSquareDistribution.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="ChiSquareDistribution"/> for each group of parameters.
        /// </remarks>
        public double ChiSquare(int alpha)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(ChiSquareDistribution.IsValidParam(alpha), ErrorMessages.InvalidParams);
            return ChiSquareDistribution.Sample(_gen, alpha);
        }

        /// <summary>
        ///   Returns an infinite sequence of chi square distributed floating point random numbers.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of chi square distributed random numbers.
        /// </param>
        /// <returns>
        ///   An infinite sequence of chi square distributed floating point random numbers.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="ChiSquareDistribution.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="ChiSquareDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> ChiSquareSamples(int alpha)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(ChiSquareDistribution.IsValidParam(alpha), ErrorMessages.InvalidParams);
            return InfiniteLoop(ChiSquareDistribution.Sample, _gen, alpha);
        }

        /// <summary>
        ///   Returns a continuous uniform distributed floating point random number.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of continuous uniform distributed
        ///   random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of continuous uniform distributed
        ///   random numbers.
        /// </param>
        /// <returns>A continuous uniform distributed floating point random number.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is greater than <paramref name="beta"/>.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to
        ///   <see cref="ContinuousUniformDistribution.NextDouble"/>, with a prior adjustement of
        ///   the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="ContinuousUniformDistribution"/> for each group of parameters.
        /// </remarks>
        public double ContinuousUniform(double alpha, double beta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(ContinuousUniformDistribution.AreValidParams(alpha, beta), ErrorMessages.InvalidParams);
            return ContinuousUniformDistribution.Sample(_gen, alpha, beta);
        }

        /// <summary>
        ///   Returns an infinite sequence of continuous uniform distributed floating point random numbers.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of continuous uniform distributed
        ///   random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of continuous uniform distributed
        ///   random numbers.
        /// </param>
        /// <returns>
        ///   An infinite sequence of continuous uniform distributed floating point random numbers.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is greater than <paramref name="beta"/>.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to
        ///   <see cref="ContinuousUniformDistribution.NextDouble"/>, with a prior adjustement of
        ///   the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="ContinuousUniformDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> ContinuousUniformSamples(double alpha, double beta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(ContinuousUniformDistribution.AreValidParams(alpha, beta), ErrorMessages.InvalidParams);
            return InfiniteLoop(ContinuousUniformDistribution.Sample, _gen, alpha, beta);
        }

        /// <summary>
        ///   Returns an erlang distributed floating point random number.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of erlang distributed random numbers.
        /// </param>
        /// <param name="lambda">
        ///   The parameter lambda which is used for generation of erlang distributed random numbers.
        /// </param>
        /// <returns>An erlang distributed floating point random number.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="lambda"/> are less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="ErlangDistribution.NextDouble"/>, with a
        ///   prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="ErlangDistribution"/> for each group of parameters.
        /// </remarks>
        public double Erlang(int alpha, double lambda)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(ErlangDistribution.AreValidParams(alpha, lambda), ErrorMessages.InvalidParams);
            return ErlangDistribution.Sample(_gen, alpha, lambda);
        }

        /// <summary>
        ///   Returns an infinite sequence of erlang distributed floating point random numbers.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of erlang distributed random numbers.
        /// </param>
        /// <param name="lambda">
        ///   The parameter lambda which is used for generation of erlang distributed random numbers.
        /// </param>
        /// <returns>An infinite sequence of erlang distributed floating point random numbers.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="lambda"/> are less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="ErlangDistribution.NextDouble"/>, with a
        ///   prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="ErlangDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> ErlangSamples(int alpha, double lambda)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(ErlangDistribution.AreValidParams(alpha, lambda), ErrorMessages.InvalidParams);
            return InfiniteLoop(ErlangDistribution.Sample, _gen, alpha, lambda);
        }

        /// <summary>
        ///   Returns an exponential distributed floating point random number.
        /// </summary>
        /// <param name="lambda">
        ///   The parameter lambda which is used for generation of exponential distributed random numbers.
        /// </param>
        /// <returns>An exponential distributed floating point random number.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="lambda"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="ExponentialDistribution.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="ExponentialDistribution"/> for each group of parameters.
        /// </remarks>
        public double Exponential(double lambda)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(ExponentialDistribution.IsValidParam(lambda), ErrorMessages.InvalidParams);
            return ExponentialDistribution.Sample(_gen, lambda);
        }

        /// <summary>
        ///   Returns an infinite sequence of exponential distributed floating point random numbers.
        /// </summary>
        /// <param name="lambda">
        ///   The parameter lambda which is used for generation of exponential distributed random numbers.
        /// </param>
        /// <returns>
        ///   An infinite sequence of exponential distributed floating point random numbers.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="lambda"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="ExponentialDistribution.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="ExponentialDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> ExponentialSamples(double lambda)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(ExponentialDistribution.IsValidParam(lambda), ErrorMessages.InvalidParams);
            return InfiniteLoop(ExponentialDistribution.Sample, _gen, lambda);
        }

        /// <summary>
        ///   Returns a fisher snedecor distributed floating point random number.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of fisher snedecor distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of fisher snedecor distributed random numbers.
        /// </param>
        /// <returns>A fisher snedecor distributed floating point random number.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="beta"/> are less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to
        ///   <see cref="FisherSnedecorDistribution.NextDouble"/>, with a prior adjustement of the
        ///   distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="FisherSnedecorDistribution"/> for each group of parameters.
        /// </remarks>
        public double FisherSnedecor(int alpha, int beta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(FisherSnedecorDistribution.AreValidParams(alpha, beta), ErrorMessages.InvalidParams);
            return FisherSnedecorDistribution.Sample(_gen, alpha, beta);
        }

        /// <summary>
        ///   Returns an infinite sequence of fisher snedecor distributed floating point random numbers.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of fisher snedecor distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of fisher snedecor distributed random numbers.
        /// </param>
        /// <returns>
        ///   An infinite sequence of fisher snedecor distributed floating point random numbers.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="beta"/> are less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to
        ///   <see cref="FisherSnedecorDistribution.NextDouble"/>, with a prior adjustement of the
        ///   distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="FisherSnedecorDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> FisherSnedecorSamples(int alpha, int beta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(FisherSnedecorDistribution.AreValidParams(alpha, beta), ErrorMessages.InvalidParams);
            return InfiniteLoop(FisherSnedecorDistribution.Sample, _gen, alpha, beta);
        }

        /// <summary>
        ///   Returns a fisher tippett distributed floating point random number.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of fisher tippett distributed random numbers.
        /// </param>
        /// <param name="mu">
        ///   The parameter mu which is used for generation of fisher tippett distributed random numbers.
        /// </param>
        /// <returns>A fisher tippett distributed floating point random number.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="FisherTippettDistribution.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="FisherTippettDistribution"/> for each group of parameters.
        /// </remarks>
        public double FisherTippett(double alpha, double mu)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(FisherTippettDistribution.AreValidParams(alpha, mu), ErrorMessages.InvalidParams);
            return FisherTippettDistribution.Sample(_gen, alpha, mu);
        }

        /// <summary>
        ///   Returns an infinite sequence of fisher tippett distributed floating point random numbers.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of fisher tippett distributed random numbers.
        /// </param>
        /// <param name="mu">
        ///   The parameter mu which is used for generation of fisher tippett distributed random numbers.
        /// </param>
        /// <returns>
        ///   An infinite sequence of fisher tippett distributed floating point random numbers.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="FisherTippettDistribution.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="FisherTippettDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> FisherTippettSamples(double alpha, double mu)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(FisherTippettDistribution.AreValidParams(alpha, mu), ErrorMessages.InvalidParams);
            return InfiniteLoop(FisherTippettDistribution.Sample, _gen, alpha, mu);
        }

        /// <summary>
        ///   Returns a gamma distributed floating point random number.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of gamma distributed random numbers.
        /// </param>
        /// <param name="theta">
        ///   The parameter theta which is used for generation of gamma distributed random numbers.
        /// </param>
        /// <returns>A gamma distributed floating point random number.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="theta"/> are less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="GammaDistribution.NextDouble"/>, with a
        ///   prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="GammaDistribution"/> for each group of parameters.
        /// </remarks>
        public double Gamma(double alpha, double theta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(GammaDistribution.AreValidParams(alpha, theta), ErrorMessages.InvalidParams);
            return GammaDistribution.Sample(_gen, alpha, theta);
        }

        /// <summary>
        ///   Returns an infinite sequence of gamma distributed floating point random numbers.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of gamma distributed random numbers.
        /// </param>
        /// <param name="theta">
        ///   The parameter theta which is used for generation of gamma distributed random numbers.
        /// </param>
        /// <returns>An infinite sequence of gamma distributed floating point random numbers.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="theta"/> are less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="GammaDistribution.NextDouble"/>, with a
        ///   prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="GammaDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> GammaSamples(double alpha, double theta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(GammaDistribution.AreValidParams(alpha, theta), ErrorMessages.InvalidParams);
            return InfiniteLoop(GammaDistribution.Sample, _gen, alpha, theta);
        }

        /// <summary>
        ///   Returns a laplace distributed floating point random number.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of laplace distributed random numbers.
        /// </param>
        /// <param name="mu">
        ///   The parameter mu which is used for generation of laplace distributed random numbers.
        /// </param>
        /// <returns>A laplace distributed floating point random number.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="LaplaceDistribution.NextDouble"/>, with
        ///   a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="LaplaceDistribution"/> for each group of parameters.
        /// </remarks>
        public double Laplace(double alpha, double mu)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(LaplaceDistribution.AreValidParams(alpha, mu), ErrorMessages.InvalidParams);
            return LaplaceDistribution.Sample(_gen, alpha, mu);
        }

        /// <summary>
        ///   Returns an infinite sequence of laplace distributed floating point random numbers.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of laplace distributed random numbers.
        /// </param>
        /// <param name="mu">
        ///   The parameter mu which is used for generation of laplace distributed random numbers.
        /// </param>
        /// <returns>An infinite sequence of laplace distributed floating point random numbers.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="LaplaceDistribution.NextDouble"/>, with
        ///   a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="LaplaceDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> LaplaceSamples(double alpha, double mu)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(LaplaceDistribution.AreValidParams(alpha, mu), ErrorMessages.InvalidParams);
            return InfiniteLoop(LaplaceDistribution.Sample, _gen, alpha, mu);
        }

        /// <summary>
        ///   Returns a lognormal distributed floating point random number.
        /// </summary>
        /// <param name="mu">
        ///   The parameter mu which is used for generation of lognormal distributed random numbers.
        /// </param>
        /// <param name="sigma">
        ///   The parameter sigma which is used for generation of lognormal distributed random numbers.
        /// </param>
        /// <returns>A lognormal distributed floating point random number.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="sigma"/> is less than zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="LognormalDistribution.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="LognormalDistribution"/> for each group of parameters.
        /// </remarks>
        public double Lognormal(double mu, double sigma)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(LognormalDistribution.AreValidParams(mu, sigma), ErrorMessages.InvalidParams);
            return LognormalDistribution.Sample(_gen, mu, sigma);
        }

        /// <summary>
        ///   Returns an infinite sequence of lognormal distributed floating point random numbers.
        /// </summary>
        /// <param name="mu">
        ///   The parameter mu which is used for generation of lognormal distributed random numbers.
        /// </param>
        /// <param name="sigma">
        ///   The parameter sigma which is used for generation of lognormal distributed random numbers.
        /// </param>
        /// <returns>
        ///   An infinite sequence of lognormal distributed floating point random numbers.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="sigma"/> is less than zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="LognormalDistribution.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="LognormalDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> LognormalSamples(double mu, double sigma)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(LognormalDistribution.AreValidParams(mu, sigma), ErrorMessages.InvalidParams);
            return InfiniteLoop(LognormalDistribution.Sample, _gen, mu, sigma);
        }

        /// <summary>
        ///   Returns a normal distributed floating point random number.
        /// </summary>
        /// <param name="mu">
        ///   The parameter mu which is used for generation of normal distributed random numbers.
        /// </param>
        /// <param name="sigma">
        ///   The parameter sigma which is used for generation of normal distributed random numbers.
        /// </param>
        /// <returns>A normal distributed floating point random number.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="sigma"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="NormalDistribution.NextDouble"/>, with a
        ///   prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="NormalDistribution"/> for each group of parameters.
        /// </remarks>
        public double Normal(double mu, double sigma)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(NormalDistribution.AreValidParams(mu, sigma), ErrorMessages.InvalidParams);
            return NormalDistribution.Sample(_gen, mu, sigma);
        }

        /// <summary>
        ///   Returns an infinite sequence of normal distributed floating point random numbers.
        /// </summary>
        /// <param name="mu">
        ///   The parameter mu which is used for generation of normal distributed random numbers.
        /// </param>
        /// <param name="sigma">
        ///   The parameter sigma which is used for generation of normal distributed random numbers.
        /// </param>
        /// <returns>An infinite sequence of normal distributed floating point random numbers.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="sigma"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="NormalDistribution.NextDouble"/>, with a
        ///   prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="NormalDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> NormalSamples(double mu, double sigma)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(NormalDistribution.AreValidParams(mu, sigma), ErrorMessages.InvalidParams);
            return InfiniteLoop(NormalDistribution.Sample, _gen, mu, sigma);
        }

        /// <summary>
        ///   Returns a pareto distributed floating point random number.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of pareto distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of pareto distributed random numbers.
        /// </param>
        /// <returns>A pareto distributed floating point random number.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="beta"/> are less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="ParetoDistribution.NextDouble"/>, with a
        ///   prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="ParetoDistribution"/> for each group of parameters.
        /// </remarks>
        public double Pareto(double alpha, double beta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(ParetoDistribution.AreValidParams(alpha, beta), ErrorMessages.InvalidParams);
            return ParetoDistribution.Sample(_gen, alpha, beta);
        }

        /// <summary>
        ///   Returns an infinite sequence of pareto distributed floating point random numbers.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of pareto distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of pareto distributed random numbers.
        /// </param>
        /// <returns>An infinite sequence of pareto distributed floating point random numbers.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="beta"/> are less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="ParetoDistribution.NextDouble"/>, with a
        ///   prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="ParetoDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> ParetoSamples(double alpha, double beta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(ParetoDistribution.AreValidParams(alpha, beta), ErrorMessages.InvalidParams);
            return InfiniteLoop(ParetoDistribution.Sample, _gen, alpha, beta);
        }

        /// <summary>
        ///   Returns a power distributed floating point random number.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of power distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of power distributed random numbers.
        /// </param>
        /// <returns>A power distributed floating point random number.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="beta"/> are less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="PowerDistribution.NextDouble"/>, with a
        ///   prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="PowerDistribution"/> for each group of parameters.
        /// </remarks>
        public double Power(double alpha, double beta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(PowerDistribution.AreValidParams(alpha, beta), ErrorMessages.InvalidParams);
            return PowerDistribution.Sample(_gen, alpha, beta);
        }

        /// <summary>
        ///   Returns an infinite sequence of power distributed floating point random numbers.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of power distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of power distributed random numbers.
        /// </param>
        /// <returns>An infinite sequence of power distributed floating point random numbers.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="beta"/> are less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="PowerDistribution.NextDouble"/>, with a
        ///   prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="PowerDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> PowerSamples(double alpha, double beta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(PowerDistribution.AreValidParams(alpha, beta), ErrorMessages.InvalidParams);
            return InfiniteLoop(PowerDistribution.Sample, _gen, alpha, beta);
        }

        /// <summary>
        ///   Returns a rayleigh distributed floating point random number.
        /// </summary>
        /// <param name="sigma">
        ///   The parameter sigma which is used for generation of rayleigh distributed random numbers.
        /// </param>
        /// <returns>A rayleigh distributed floating point random number.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="sigma"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="RayleighDistribution.NextDouble"/>, with
        ///   a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="RayleighDistribution"/> for each group of parameters.
        /// </remarks>
        public double Rayleigh(double sigma)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(RayleighDistribution.IsValidParam(sigma), ErrorMessages.InvalidParams);
            return RayleighDistribution.Sample(_gen, sigma);
        }

        /// <summary>
        ///   Returns an infinite sequence of rayleigh distributed floating point random numbers.
        /// </summary>
        /// <param name="sigma">
        ///   The parameter sigma which is used for generation of rayleigh distributed random numbers.
        /// </param>
        /// <returns>
        ///   An infinite sequence of rayleigh distributed floating point random numbers.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="sigma"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="RayleighDistribution.NextDouble"/>, with
        ///   a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="RayleighDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> RayleighSamples(double sigma)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(RayleighDistribution.IsValidParam(sigma), ErrorMessages.InvalidParams);
            return InfiniteLoop(RayleighDistribution.Sample, _gen, sigma);
        }

        /// <summary>
        ///   Returns a student's t distributed floating point random number.
        /// </summary>
        /// <param name="nu">
        ///   The parameter nu which is used for generation of student's t distributed random numbers.
        /// </param>
        /// <returns>A student's t distributed floating point random number.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="nu"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="StudentsTDistribution.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="StudentsTDistribution"/> for each group of parameters.
        /// </remarks>
        public double StudentsT(int nu)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(StudentsTDistribution.IsValidParam(nu), ErrorMessages.InvalidParams);
            return StudentsTDistribution.Sample(_gen, nu);
        }

        /// <summary>
        ///   Returns an infinite sequence of student's t distributed floating point random numbers.
        /// </summary>
        /// <param name="nu">
        ///   The parameter nu which is used for generation of student's t distributed random numbers.
        /// </param>
        /// <returns>
        ///   An infinite sequence of student's t distributed floating point random numbers.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="nu"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="StudentsTDistribution.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="StudentsTDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> StudentsTSamples(int nu)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(StudentsTDistribution.IsValidParam(nu), ErrorMessages.InvalidParams);
            return InfiniteLoop(StudentsTDistribution.Sample, _gen, nu);
        }

        /// <summary>
        ///   Returns a triangular distributed floating point random number.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of triangular distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of triangular distributed random numbers.
        /// </param>
        /// <param name="gamma">
        ///   The parameter gamma which is used for generation of triangular distributed random numbers.
        /// </param>
        /// <returns>A triangular distributed floating point random number.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is greater than or equal to <paramref name="beta"/>, or
        ///   <paramref name="alpha"/> is greater than <paramref name="gamma"/>, or
        ///   <paramref name="beta"/> is less than <paramref name="gamma"/>.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="TriangularDistribution.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="TriangularDistribution"/> for each group of parameters.
        /// </remarks>
        public double Triangular(double alpha, double beta, double gamma)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(TriangularDistribution.AreValidParams(alpha, beta, gamma), ErrorMessages.InvalidParams);
            return TriangularDistribution.Sample(_gen, alpha, beta, gamma);
        }

        /// <summary>
        ///   Returns an infinite sequence of triangular distributed floating point random numbers.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of triangular distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of triangular distributed random numbers.
        /// </param>
        /// <param name="gamma">
        ///   The parameter gamma which is used for generation of triangular distributed random numbers.
        /// </param>
        /// <returns>
        ///   An infinite sequence of triangular distributed floating point random numbers.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is greater than or equal to <paramref name="beta"/>, or
        ///   <paramref name="alpha"/> is greater than <paramref name="gamma"/>, or
        ///   <paramref name="beta"/> is less than <paramref name="gamma"/>.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="TriangularDistribution.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="TriangularDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> TriangularSamples(double alpha, double beta, double gamma)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(TriangularDistribution.AreValidParams(alpha, beta, gamma), ErrorMessages.InvalidParams);
            return InfiniteLoop(TriangularDistribution.Sample, _gen, alpha, beta, gamma);
        }

        /// <summary>
        ///   Returns a weibull distributed floating point random number.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of weibull distributed random numbers.
        /// </param>
        /// <param name="lambda">
        ///   The parameter lambda which is used for generation of weibull distributed random numbers.
        /// </param>
        /// <returns>A weibull distributed floating point random number.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="lambda"/> are less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="WeibullDistribution.NextDouble"/>, with
        ///   a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="WeibullDistribution"/> for each group of parameters.
        /// </remarks>
        public double Weibull(double alpha, double lambda)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(WeibullDistribution.AreValidParams(alpha, lambda), ErrorMessages.InvalidParams);
            return WeibullDistribution.Sample(_gen, alpha, lambda);
        }

        /// <summary>
        ///   Returns an infinite sequence of weibull distributed floating point random numbers.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of weibull distributed random numbers.
        /// </param>
        /// <param name="lambda">
        ///   The parameter lambda which is used for generation of weibull distributed random numbers.
        /// </param>
        /// <returns>An infinite sequence of weibull distributed floating point random numbers.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="lambda"/> are less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="WeibullDistribution.NextDouble"/>, with
        ///   a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters. If you
        ///   absolutely need the best performance, you may consider using an instance of
        ///   <see cref="WeibullDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> WeibullSamples(double alpha, double lambda)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(WeibullDistribution.AreValidParams(alpha, lambda), ErrorMessages.InvalidParams);
            return InfiniteLoop(WeibullDistribution.Sample, _gen, alpha, lambda);
        }

        #endregion Continuous Distributions

        #region IGenerator Members

        /// <summary>
        ///   The seed value used by the generator.
        /// </summary>
        public uint Seed => _gen.Seed;

        /// <summary>
        ///   Gets a value indicating whether the random number generator can be reset, so that it
        ///   produces the same random number sequence again.
        /// </summary>
        public bool CanReset => _gen.CanReset;

        /// <summary>
        ///   Resets the random number generator using the initial seed, so that it produces the
        ///   same random number sequence again. To understand whether this generator can be reset,
        ///   you can query the <see cref="P:Troschuetz.Random.IGenerator.CanReset"/> property.
        /// </summary>
        /// <returns>True if the random number generator was reset; otherwise, false.</returns>
        public bool Reset() => _gen.Reset(Seed);

        /// <summary>
        ///   Resets the random number generator using the specified seed, so that it produces the
        ///   same random number sequence again. To understand whether this generator can be reset,
        ///   you can query the <see cref="P:Troschuetz.Random.IGenerator.CanReset"/> property.
        /// </summary>
        /// <param name="seed">The seed value used by the generator.</param>
        /// <returns>True if the random number generator was reset; otherwise, false.</returns>
        public bool Reset(uint seed) => _gen.Reset(seed);

        /// <summary>
        ///   Returns a nonnegative random number less than <see cref="F:System.Int32.MaxValue"/>.
        /// </summary>
        /// <returns>
        ///   A 32-bit signed integer greater than or equal to 0, and less than
        ///   <see cref="F:System.Int32.MaxValue"/>; that is, the range of return values includes 0
        ///   but not <see cref="F:System.Int32.MaxValue"/>.
        /// </returns>
        public int Next() => _gen.Next();

        /// <summary>
        ///   Returns a nonnegative random number less than or equal to <see cref="F:System.Int32.MaxValue"/>.
        /// </summary>
        /// <returns>
        ///   A 32-bit signed integer greater than or equal to 0, and less than or equal to
        ///   <see cref="F:System.Int32.MaxValue"/>; that is, the range of return values includes 0
        ///   and <see cref="F:System.Int32.MaxValue"/>.
        /// </returns>
        public int NextInclusiveMaxValue() => _gen.NextInclusiveMaxValue();

        /// <summary>
        ///   Returns a nonnegative random number less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated.</param>
        /// <returns>
        ///   A 32-bit signed integer greater than or equal to 0, and less than
        ///   <paramref name="maxValue"/>; that is, the range of return values includes 0 but not <paramref name="maxValue"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="maxValue"/> must be greater than or equal to 0.
        /// </exception>
        public int Next(int maxValue) => _gen.Next(maxValue);

        /// <summary>
        ///   Returns a random number within the specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number to be generated.</param>
        /// <param name="maxValue">
        ///   The exclusive upper bound of the random number to be generated.
        ///   <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.
        /// </param>
        /// <returns>
        ///   A 32-bit signed integer greater than or equal to <paramref name="minValue"/>, and less
        ///   than <paramref name="maxValue"/>; that is, the range of return values includes
        ///   <paramref name="minValue"/> but not <paramref name="maxValue"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.
        /// </exception>
        public int Next(int minValue, int maxValue) => _gen.Next(minValue, maxValue);

        /// <summary>
        ///   Returns a nonnegative floating point random number less than 1.0.
        /// </summary>
        /// <returns>
        ///   A double-precision floating point number greater than or equal to 0.0, and less than
        ///   1.0; that is, the range of return values includes 0.0 but not 1.0.
        /// </returns>
        public double NextDouble() => _gen.NextDouble();

        /// <summary>
        ///   Returns a nonnegative floating point random number less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated.</param>
        /// <returns>
        ///   A double-precision floating point number greater than or equal to 0.0, and less than
        ///   <paramref name="maxValue"/>; that is, the range of return values includes 0 but not <paramref name="maxValue"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="maxValue"/> must be greater than or equal to 0.0.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="maxValue"/> cannot be <see cref="F:System.Double.PositiveInfinity"/>.
        /// </exception>
        public double NextDouble(double maxValue) => _gen.NextDouble(maxValue);

        /// <summary>
        ///   Returns a floating point random number within the specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number to be generated.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated.</param>
        /// <returns>
        ///   A double-precision floating point number greater than or equal to
        ///   <paramref name="minValue"/>, and less than <paramref name="maxValue"/>; that is, the
        ///   range of return values includes <paramref name="minValue"/> but not <paramref name="maxValue"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///   The difference between <paramref name="maxValue"/> and <paramref name="minValue"/>
        ///   cannot be <see cref="F:System.Double.PositiveInfinity"/>.
        /// </exception>
        public double NextDouble(double minValue, double maxValue) => _gen.NextDouble(minValue, maxValue);

        /// <summary>
        ///   Returns an unsigned random number.
        /// </summary>
        /// <returns>
        ///   A 32-bit unsigned integer greater than or equal to
        ///   <see cref="F:System.UInt32.MinValue"/> and less than or equal to <see cref="F:System.UInt32.MaxValue"/>.
        /// </returns>
        public uint NextUInt() => _gen.NextUInt();

        /// <summary>
        ///   Returns an unsigned random number less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated.</param>
        /// <returns>
        ///   A 32-bit unsigned integer greater than or equal to
        ///   <see cref="F:System.UInt32.MinValue"/> and less than <paramref name="maxValue"/>; that
        ///   is, the range of return values includes <see cref="F:System.UInt32.MinValue"/> but not <paramref name="maxValue"/>.
        /// </returns>
        public uint NextUInt(uint maxValue) => _gen.NextUInt(maxValue);

        /// <summary>
        ///   Returns an unsigned random number less than <see cref="F:System.UInt32.MaxValue"/>.
        /// </summary>
        /// <returns>
        ///   A 32-bit unsigned integer greater than or equal to
        ///   <see cref="F:System.UInt32.MinValue"/> and less than <see cref="F:System.UInt32.MaxValue"/>.
        /// </returns>
        public uint NextUIntExclusiveMaxValue() => _gen.NextUIntExclusiveMaxValue();

        /// <summary>
        ///   Returns an unsigned random number within the specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number to be generated.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated.</param>
        /// <returns>
        ///   A 32-bit unsigned integer greater than or equal to <paramref name="minValue"/> and
        ///   less than <paramref name="maxValue"/>; that is, the range of return values includes
        ///   <paramref name="minValue"/> but not <paramref name="maxValue"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.
        /// </exception>
        public uint NextUInt(uint minValue, uint maxValue) => _gen.NextUInt(minValue, maxValue);

        /// <summary>
        ///   Returns a random Boolean value.
        /// </summary>
        /// <remarks>
        ///   Buffers 31 random bits for future calls, so the random number generator is only
        ///   invoked once in every 31 calls.
        /// </remarks>
        /// <returns>A <see cref="T:System.Boolean"/> value.</returns>
        public bool NextBoolean() => _gen.NextBoolean();

        /// <summary>
        ///   Fills the elements of a specified array of bytes with random numbers.
        /// </summary>
        /// <remarks>
        ///   Each element of the array of bytes is set to a random number greater than or equal to
        ///   0, and less than or equal to <see cref="F:System.Byte.MaxValue"/>.
        /// </remarks>
        /// <param name="buffer">An array of bytes to contain random numbers.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="buffer"/> is null.
        /// </exception>
        public void NextBytes(byte[] buffer)
        {
            _gen.NextBytes(buffer);
        }

        #endregion IGenerator Members

        #region Object Members

        /// <summary>
        ///   Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public string ToString() => string.Format("Generator: {0}", _gen);

        #endregion Object Members

        #region Private Members

#if PORTABLE

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        static IEnumerable<TRet> InfiniteLoop<T1, T2, TRet>(Func<T1, T2, TRet> f, T1 a1, T2 a2)
        {
            while (true)
            {
                yield return f(a1, a2);
            }
        }

#if PORTABLE

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        static IEnumerable<TRet> InfiniteLoop<T1, T2, T3, TRet>(Func<T1, T2, T3, TRet> f, T1 a1, T2 a2, T3 a3)
        {
            while (true)
            {
                yield return f(a1, a2, a3);
            }
        }

#if PORTABLE

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        static IEnumerable<TRet> InfiniteLoop<T1, T2, T3, T4, TRet>(Func<T1, T2, T3, T4, TRet> f, T1 a1, T2 a2, T3 a3, T4 a4)
        {
            while (true)
            {
                yield return f(a1, a2, a3, a4);
            }
        }

        #endregion Private Members
    }
}
