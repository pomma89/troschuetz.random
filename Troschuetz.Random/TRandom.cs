/*
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

namespace Troschuetz.Random
{
    using Distributions.Continuous;
    using Distributions.Discrete;
    using PommaLabs.Thrower;
    using Generators;
    using System;
    using System.Collections.Generic;
    using Core;
    using System.Runtime.CompilerServices;

    /// <summary>
    ///   A random generator class similar to the one Python offers,
    ///   providing functions similar to the ones found in <see cref="System.Random"/>
    ///   and functions returning random numbers according to a particular kind of distribution.
    /// </summary>
    [Serializable]
    public class TRandom<TGen> : IGenerator where TGen : IGenerator
    {
        readonly TGen _gen;

        internal TRandom(TGen generator)
        {
            _gen = generator;
        }

        #region Discrete Distributions

        /// <summary>
        ///   Returns a bernoulli distributed 32-bit signed integer.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of bernoulli distributed random numbers.
        /// </param>
        /// <returns>
        ///   A bernoulli distributed 32-bit signed integer.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than zero or greater than one.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="BernoulliDistribution{TGen}.Next"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="BernoulliDistribution"/> for each group of parameters.
        /// </remarks>
        public int Bernoulli(double alpha)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(BernoulliDistribution<TGen>.IsValidParam(alpha), ErrorMessages.InvalidParams);
            return BernoulliDistribution<TGen>.Sample(_gen, alpha);
        }

        /// <summary>
        ///   Returns an infinite sequence of bernoulli distributed 32-bit signed integers.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of bernoulli distributed random numbers.
        /// </param>
        /// <returns>
        ///   An infinite sequence of bernoulli distributed 32-bit signed integers.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than zero or greater than one.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="BernoulliDistribution{TGen}.Next"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="BernoulliDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<int> BernoulliSamples(double alpha)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(BernoulliDistribution<TGen>.IsValidParam(alpha),
                                                           ErrorMessages.InvalidParams);
            return InfiniteLoop(BernoulliDistribution<TGen>.Sample, _gen, alpha);
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
        /// <returns>
        ///   A binomial distributed 32-bit signed integer.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than zero or greater than one,
        ///   or <paramref name="beta"/> is less than zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="BinomialDistribution{TGen}.Next"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="BinomialDistribution"/> for each group of parameters.
        /// </remarks>
        public int Binomial(double alpha, int beta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(BinomialDistribution<TGen>.AreValidParams(alpha, beta),
                                                           ErrorMessages.InvalidParams);
            return BinomialDistribution<TGen>.Sample(_gen, alpha, beta);
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
        /// <returns>
        ///   An infinite sequence of binomial distributed 32-bit signed integers.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than zero or greater than one,
        ///   or <paramref name="beta"/> is less than zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="BinomialDistribution{TGen}.Next"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="BinomialDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<int> BinomialSamples(double alpha, int beta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(BinomialDistribution<TGen>.AreValidParams(alpha, beta),
                                                           ErrorMessages.InvalidParams);
            return InfiniteLoop(BinomialDistribution<TGen>.Sample, _gen, alpha, beta);
        }

        /// <summary>
        ///   Returns a categorical distributed 32-bit signed integer.
        /// </summary>
        /// <param name="valueCount">
        ///   The parameter valueCount which is used for generation of binomial distributed random numbers
        ///   by setting the number of equi-distributed "weights" the distribution will have.
        /// </param>
        /// <returns>
        ///   A categorical distributed 32-bit signed integer.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="valueCount"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="CategoricalDistribution{TGen}.Next"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="CategoricalDistribution"/> for each group of parameters.
        /// </remarks>
        public int Categorical(int valueCount)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(valueCount > 0, ErrorMessages.InvalidParams);
            double[] cdf;
            double weightsSum;
            CategoricalDistribution<TGen>.SetUp(valueCount, null, out cdf, out weightsSum);
            return CategoricalDistribution<TGen>.Sample(_gen, valueCount, cdf, weightsSum);
        }

        /// <summary>
        ///   Returns an infinite sequence of categorical distributed 32-bit signed integers.
        /// </summary>
        /// <param name="valueCount">
        ///   The parameter valueCount which is used for generation of binomial distributed random numbers
        ///   by setting the number of equi-distributed "weights" the distribution will have.
        /// </param>
        /// <returns>
        ///   An infinite sequence of categorical distributed 32-bit signed integers.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="valueCount"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="CategoricalDistribution{TGen}.Next"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="CategoricalDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<int> CategoricalSamples(int valueCount)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(valueCount > 0, ErrorMessages.InvalidParams);
            double[] cdf;
            double weightsSum;
            CategoricalDistribution<TGen>.SetUp(valueCount, null, out cdf, out weightsSum);
            return InfiniteLoop(CategoricalDistribution<TGen>.Sample, _gen, valueCount, cdf, weightsSum);
        }

        /// <summary>
        ///   Returns a categorical distributed 32-bit signed integer.
        /// </summary>
        /// <param name="weights">
        ///   An enumerable of nonnegative weights: this enumerable does not need to be normalized
        ///   as this is often impossible using floating point arithmetic.
        /// </param>
        /// <returns>
        ///   A categorical distributed 32-bit signed integer.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="weights"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="weights"/> is empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   Any of the weights in <paramref name="weights"/> are negative or they sum to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="CategoricalDistribution{TGen}.Next"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="CategoricalDistribution"/> for each group of parameters.
        /// </remarks>
        public int Categorical(ICollection<double> weights)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(CategoricalDistribution<TGen>.IsValidParam(weights),
                                                           ErrorMessages.InvalidParams);
            double[] cdf;
            double weightsSum;
            CategoricalDistribution<TGen>.SetUp(weights.Count, weights, out cdf, out weightsSum);
            return CategoricalDistribution<TGen>.Sample(_gen, weights.Count, cdf, weightsSum);
        }

        /// <summary>
        ///   Returns an infinite sequence of categorical distributed 32-bit signed integers.
        /// </summary>
        /// <param name="weights">
        ///   An enumerable of nonnegative weights: this enumerable does not need to be normalized
        ///   as this is often impossible using floating point arithmetic.
        /// </param>
        /// <returns>
        ///   An infinite sequence of categorical distributed 32-bit signed integers.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="weights"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="weights"/> is empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   Any of the weights in <paramref name="weights"/> are negative or they sum to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="CategoricalDistribution{TGen}.Next"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="CategoricalDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<int> CategoricalSamples(ICollection<double> weights)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(CategoricalDistribution<TGen>.IsValidParam(weights),
                                                           ErrorMessages.InvalidParams);
            double[] cdf;
            double weightsSum;
            CategoricalDistribution<TGen>.SetUp(weights.Count, weights, out cdf, out weightsSum);
            return InfiniteLoop(CategoricalDistribution<TGen>.Sample, _gen, weights.Count, cdf, weightsSum);
        }

        /// <summary>
        ///   Returns a discrete uniform distributed 32-bit signed integer.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of discrete uniform distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of discrete uniform distributed random numbers.
        /// </param>
        /// <returns>
        ///   A discrete uniform distributed 32-bit signed integer.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is greater than <paramref name="beta"/>,
        ///   or <paramref name="beta"/> is equal to <see cref="int.MaxValue"/>.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="DiscreteUniformDistribution{TGen}.Next"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="DiscreteUniformDistribution"/> for each group of parameters.
        /// </remarks>
        public int DiscreteUniform(int alpha, int beta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(DiscreteUniformDistribution<TGen>.AreValidParams(alpha, beta),
                                                           ErrorMessages.InvalidParams);
            return DiscreteUniformDistribution<TGen>.Sample(_gen, alpha, beta);
        }

        /// <summary>
        ///   Returns an infinite sequence of discrete uniform distributed 32-bit signed integers.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of discrete uniform distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of discrete uniform distributed random numbers.
        /// </param>
        /// <returns>
        ///   An infinite sequence of discrete uniform distributed 32-bit signed integers.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is greater than <paramref name="beta"/>,
        ///   or <paramref name="beta"/> is equal to <see cref="int.MaxValue"/>.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="DiscreteUniformDistribution{TGen}.Next"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="DiscreteUniformDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<int> DiscreteUniformSamples(int alpha, int beta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(DiscreteUniformDistribution<TGen>.AreValidParams(alpha, beta),
                                                           ErrorMessages.InvalidParams);
            return InfiniteLoop(DiscreteUniformDistribution<TGen>.Sample, _gen, alpha, beta);
        }

        /// <summary>
        ///   Returns a geometric distributed 32-bit signed integer.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of geometric distributed random numbers.
        /// </param>
        /// <returns>
        ///   A geometric distributed 32-bit signed integer.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero or it is greater than one.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="GeometricDistribution{TGen}.Next"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="GeometricDistribution"/> for each group of parameters.
        /// </remarks>
        public int Geometric(double alpha)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(GeometricDistribution<TGen>.IsValidParam(alpha),
                                                           ErrorMessages.InvalidParams);
            return GeometricDistribution<TGen>.Sample(_gen, alpha);
        }

        /// <summary>
        ///   Returns an infinite sequence of geometric distributed 32-bit signed integers.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of geometric distributed random numbers.
        /// </param>
        /// <returns>
        ///   An infinite sequence of geometric distributed 32-bit signed integers.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero or it is greater than one.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="GeometricDistribution{TGen}.Next"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="GeometricDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<int> GeometricSamples(double alpha)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(GeometricDistribution<TGen>.IsValidParam(alpha),
                                                           ErrorMessages.InvalidParams);
            return InfiniteLoop(GeometricDistribution<TGen>.Sample, _gen, alpha);
        }

        /// <summary>
        ///   Returns a poisson distributed 32-bit signed integer.
        /// </summary>
        /// <param name="lambda">
        ///   The parameter lambda which is used for generation of poisson distributed random numbers.
        /// </param>
        /// <returns>
        ///   A poisson distributed 32-bit signed integer.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="lambda"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="PoissonDistribution{TGen}.Next"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="PoissonDistribution"/> for each group of parameters.
        /// </remarks>
        public int Poisson(double lambda)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(PoissonDistribution<TGen>.IsValidParam(lambda),
                                                           ErrorMessages.InvalidParams);
            return PoissonDistribution<TGen>.Sample(_gen, lambda);
        }

        /// <summary>
        ///   Returns an infinite sequence of poisson distributed 32-bit signed integers.
        /// </summary>
        /// <param name="lambda">
        ///   The parameter lambda which is used for generation of poisson distributed random numbers.
        /// </param>
        /// <returns>
        ///   An infinite sequence of poisson distributed 32-bit signed integers.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="lambda"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="PoissonDistribution{TGen}.Next"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="PoissonDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<int> PoissonSamples(double lambda)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(PoissonDistribution<TGen>.IsValidParam(lambda),
                                                           ErrorMessages.InvalidParams);
            return InfiniteLoop(PoissonDistribution<TGen>.Sample, _gen, lambda);
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
        /// <returns>
        ///   A beta distributed floating point random number.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="beta"/> are less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="BetaDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="BetaDistribution"/> for each group of parameters.
        /// </remarks>
        public double Beta(double alpha, double beta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(BetaDistribution<TGen>.AreValidParams(alpha, beta));
            return BetaDistribution<TGen>.Sample(_gen, alpha, beta);
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
        /// <returns>
        ///   An infinite sequence of beta distributed floating point random numbers.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="beta"/> are less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="BetaDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="BetaDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> BetaSamples(double alpha, double beta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(BetaDistribution<TGen>.AreValidParams(alpha, beta));
            return InfiniteLoop(BetaDistribution<TGen>.Sample, _gen, alpha, beta);
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
        /// <returns>
        ///   A beta prime distributed floating point random number.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="beta"/> are less than or equal to one.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="BetaPrimeDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="BetaPrimeDistribution"/> for each group of parameters.
        /// </remarks>
        public double BetaPrime(double alpha, double beta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(BetaPrimeDistribution<TGen>.AreValidParams(alpha, beta));
            return BetaPrimeDistribution<TGen>.Sample(_gen, alpha, beta);
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
        ///   This method simply wraps a call to <see cref="BetaPrimeDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="BetaPrimeDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> BetaPrimeSamples(double alpha, double beta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(BetaPrimeDistribution<TGen>.AreValidParams(alpha, beta));
            return InfiniteLoop(BetaPrimeDistribution<TGen>.Sample, _gen, alpha, beta);
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
        /// <returns>
        ///   A cauchy distributed floating point random number.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="gamma"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="CauchyDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="CauchyDistribution"/> for each group of parameters.
        /// </remarks>
        public double Cauchy(double alpha, double gamma)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(CauchyDistribution<TGen>.AreValidParams(alpha, gamma));
            return CauchyDistribution<TGen>.Sample(_gen, alpha, gamma);
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
        /// <returns>
        ///   An infinite sequence of cauchy distributed floating point random numbers.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="gamma"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="CauchyDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="CauchyDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> CauchySamples(double alpha, double gamma)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(CauchyDistribution<TGen>.AreValidParams(alpha, gamma));
            return InfiniteLoop(CauchyDistribution<TGen>.Sample, _gen, alpha, gamma);
        }

        /// <summary>
        ///   Returns a chi distributed floating point random number.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of chi distributed random numbers.
        /// </param>
        /// <returns>
        ///   A chi distributed floating point random number.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="ChiDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="ChiDistribution"/> for each group of parameters.
        /// </remarks>
        public double Chi(int alpha)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(ChiDistribution<TGen>.IsValidParam(alpha));
            return ChiDistribution<TGen>.Sample(_gen, alpha);
        }

        /// <summary>
        ///   Returns an infinite sequence of chi distributed floating point random numbers.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of chi distributed random numbers.
        /// </param>
        /// <returns>
        ///   An infinite sequence of chi distributed floating point random numbers.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="ChiDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="ChiDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> ChiSamples(int alpha)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(ChiDistribution<TGen>.IsValidParam(alpha));
            return InfiniteLoop(ChiDistribution<TGen>.Sample, _gen, alpha);
        }

        /// <summary>
        ///   Returns a chi square distributed floating point random number.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of chi square distributed random numbers.
        /// </param>
        /// <returns>
        ///   A chi square distributed floating point random number.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="ChiSquareDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="ChiSquareDistribution"/> for each group of parameters.
        /// </remarks>
        public double ChiSquare(int alpha)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(ChiSquareDistribution<TGen>.IsValidParam(alpha));
            return ChiSquareDistribution<TGen>.Sample(_gen, alpha);
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
        ///   This method simply wraps a call to <see cref="ChiSquareDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="ChiSquareDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> ChiSquareSamples(int alpha)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(ChiSquareDistribution<TGen>.IsValidParam(alpha));
            return InfiniteLoop(ChiSquareDistribution<TGen>.Sample, _gen, alpha);
        }

        /// <summary>
        ///   Returns a continuous uniform distributed floating point random number.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of continuous uniform distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of continuous uniform distributed random numbers.
        /// </param>
        /// <returns>
        ///   A continuous uniform distributed floating point random number.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is greater than <paramref name="beta"/>.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="ContinuousUniformDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="ContinuousUniformDistribution"/> for each group of parameters.
        /// </remarks>
        public double ContinuousUniform(double alpha, double beta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(ContinuousUniformDistribution<TGen>.AreValidParams(alpha, beta));
            return ContinuousUniformDistribution<TGen>.Sample(_gen, alpha, beta);
        }

        /// <summary>
        ///   Returns an infinite sequence of continuous uniform distributed floating point random numbers.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of continuous uniform distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of continuous uniform distributed random numbers.
        /// </param>
        /// <returns>
        ///   An infinite sequence of continuous uniform distributed floating point random numbers.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is greater than <paramref name="beta"/>.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="ContinuousUniformDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="ContinuousUniformDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> ContinuousUniformSamples(double alpha, double beta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(ContinuousUniformDistribution<TGen>.AreValidParams(alpha, beta));
            return InfiniteLoop(ContinuousUniformDistribution<TGen>.Sample, _gen, alpha, beta);
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
        /// <returns>
        ///   An erlang distributed floating point random number.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="lambda"/> are less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="ErlangDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="ErlangDistribution"/> for each group of parameters.
        /// </remarks>
        public double Erlang(int alpha, double lambda)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(ErlangDistribution<TGen>.AreValidParams(alpha, lambda));
            return ErlangDistribution<TGen>.Sample(_gen, alpha, lambda);
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
        /// <returns>
        ///   An infinite sequence of erlang distributed floating point random numbers.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="lambda"/> are less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="ErlangDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="ErlangDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> ErlangSamples(int alpha, double lambda)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(ErlangDistribution<TGen>.AreValidParams(alpha, lambda));
            return InfiniteLoop(ErlangDistribution<TGen>.Sample, _gen, alpha, lambda);
        }

        /// <summary>
        ///   Returns an exponential distributed floating point random number.
        /// </summary>
        /// <param name="lambda">
        ///   The parameter lambda which is used for generation of exponential distributed random numbers.
        /// </param>
        /// <returns>
        ///   An exponential distributed floating point random number.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="lambda"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="ExponentialDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="ExponentialDistribution"/> for each group of parameters.
        /// </remarks>
        public double Exponential(double lambda)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(ExponentialDistribution<TGen>.IsValidParam(lambda),
                                                           ErrorMessages.InvalidParams);
            return ExponentialDistribution<TGen>.Sample(_gen, lambda);
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
        ///   This method simply wraps a call to <see cref="ExponentialDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="ExponentialDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> ExponentialSamples(double lambda)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(ExponentialDistribution<TGen>.IsValidParam(lambda),
                                                           ErrorMessages.InvalidParams);
            return InfiniteLoop(ExponentialDistribution<TGen>.Sample, _gen, lambda);
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
        /// <returns>
        ///   A fisher snedecor distributed floating point random number.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="beta"/> are less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="FisherSnedecorDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="FisherSnedecorDistribution"/> for each group of parameters.
        /// </remarks>
        public double FisherSnedecor(int alpha, int beta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(FisherSnedecorDistribution<TGen>.AreValidParams(alpha, beta));
            return FisherSnedecorDistribution<TGen>.Sample(_gen, alpha, beta);
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
        ///   This method simply wraps a call to <see cref="FisherSnedecorDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="FisherSnedecorDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> FisherSnedecorSamples(int alpha, int beta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(FisherSnedecorDistribution<TGen>.AreValidParams(alpha, beta));
            return InfiniteLoop(FisherSnedecorDistribution<TGen>.Sample, _gen, alpha, beta);
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
        /// <returns>
        ///   A fisher tippett distributed floating point random number.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="FisherTippettDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="FisherTippettDistribution"/> for each group of parameters.
        /// </remarks>
        public double FisherTippett(double alpha, double mu)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(FisherTippettDistribution<TGen>.AreValidParams(alpha, mu));
            return FisherTippettDistribution<TGen>.Sample(_gen, alpha, mu);
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
        ///   This method simply wraps a call to <see cref="FisherTippettDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="FisherTippettDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> FisherTippettSamples(double alpha, double mu)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(FisherTippettDistribution<TGen>.AreValidParams(alpha, mu));
            return InfiniteLoop(FisherTippettDistribution<TGen>.Sample, _gen, alpha, mu);
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
        /// <returns>
        ///   A gamma distributed floating point random number.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="theta"/> are less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="GammaDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="GammaDistribution"/> for each group of parameters.
        /// </remarks>
        public double Gamma(double alpha, double theta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(GammaDistribution<TGen>.AreValidParams(alpha, theta));
            return GammaDistribution<TGen>.Sample(_gen, alpha, theta);
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
        /// <returns>
        ///   An infinite sequence of gamma distributed floating point random numbers.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="theta"/> are less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="GammaDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="GammaDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> GammaSamples(double alpha, double theta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(GammaDistribution<TGen>.AreValidParams(alpha, theta));
            return InfiniteLoop(GammaDistribution<TGen>.Sample, _gen, alpha, theta);
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
        /// <returns>
        ///   A laplace distributed floating point random number.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="LaplaceDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="LaplaceDistribution"/> for each group of parameters.
        /// </remarks>
        public double Laplace(double alpha, double mu)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(LaplaceDistribution<TGen>.AreValidParams(alpha, mu));
            return LaplaceDistribution<TGen>.Sample(_gen, alpha, mu);
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
        /// <returns>
        ///   An infinite sequence of laplace distributed floating point random numbers.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="LaplaceDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="LaplaceDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> LaplaceSamples(double alpha, double mu)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(LaplaceDistribution<TGen>.AreValidParams(alpha, mu));
            return InfiniteLoop(LaplaceDistribution<TGen>.Sample, _gen, alpha, mu);
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
        /// <returns>
        ///   A lognormal distributed floating point random number.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="sigma"/> is less than zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="LognormalDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="LognormalDistribution"/> for each group of parameters.
        /// </remarks>
        public double Lognormal(double mu, double sigma)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(LognormalDistribution<TGen>.AreValidParams(mu, sigma));
            return LognormalDistribution<TGen>.Sample(_gen, mu, sigma);
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
        ///   This method simply wraps a call to <see cref="LognormalDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="LognormalDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> LognormalSamples(double mu, double sigma)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(LognormalDistribution<TGen>.AreValidParams(mu, sigma));
            return InfiniteLoop(LognormalDistribution<TGen>.Sample, _gen, mu, sigma);
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
        /// <returns>
        ///   A normal distributed floating point random number.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="sigma"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="NormalDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="NormalDistribution"/> for each group of parameters.
        /// </remarks>
        public double Normal(double mu, double sigma)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(NormalDistribution<TGen>.AreValidParams(mu, sigma),
                                                           ErrorMessages.InvalidParams);
            return NormalDistribution<TGen>.Sample(_gen, mu, sigma);
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
        /// <returns>
        ///   An infinite sequence of normal distributed floating point random numbers.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="sigma"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="NormalDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="NormalDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> NormalSamples(double mu, double sigma)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(NormalDistribution<TGen>.AreValidParams(mu, sigma),
                                                           ErrorMessages.InvalidParams);
            return InfiniteLoop(NormalDistribution<TGen>.Sample, _gen, mu, sigma);
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
        /// <returns>
        ///   A pareto distributed floating point random number.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="beta"/> are less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="ParetoDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="ParetoDistribution"/> for each group of parameters.
        /// </remarks>
        public double Pareto(double alpha, double beta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(ParetoDistribution<TGen>.AreValidParams(alpha, beta));
            return ParetoDistribution<TGen>.Sample(_gen, alpha, beta);
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
        /// <returns>
        ///   An infinite sequence of pareto distributed floating point random numbers.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="beta"/> are less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="ParetoDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="ParetoDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> ParetoSamples(double alpha, double beta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(ParetoDistribution<TGen>.AreValidParams(alpha, beta));
            return InfiniteLoop(ParetoDistribution<TGen>.Sample, _gen, alpha, beta);
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
        /// <returns>
        ///   A power distributed floating point random number.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="beta"/> are less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="PowerDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="PowerDistribution"/> for each group of parameters.
        /// </remarks>
        public double Power(double alpha, double beta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(PowerDistribution<TGen>.AreValidParams(alpha, beta));
            return PowerDistribution<TGen>.Sample(_gen, alpha, beta);
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
        /// <returns>
        ///   An infinite sequence of power distributed floating point random numbers.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="beta"/> are less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="PowerDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="PowerDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> PowerSamples(double alpha, double beta)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(PowerDistribution<TGen>.AreValidParams(alpha, beta));
            return InfiniteLoop(PowerDistribution<TGen>.Sample, _gen, alpha, beta);
        }

        /// <summary>
        ///   Returns a rayleigh distributed floating point random number.
        /// </summary>
        /// <param name="sigma">
        ///   The parameter sigma which is used for generation of rayleigh distributed random numbers.
        /// </param>
        /// <returns>
        ///   A rayleigh distributed floating point random number.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="sigma"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="RayleighDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="RayleighDistribution"/> for each group of parameters.
        /// </remarks>
        public double Rayleigh(double sigma)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(RayleighDistribution<TGen>.IsValidParam(sigma));
            return RayleighDistribution<TGen>.Sample(_gen, sigma);
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
        ///   This method simply wraps a call to <see cref="RayleighDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="RayleighDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> RayleighSamples(double sigma)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(RayleighDistribution<TGen>.IsValidParam(sigma));
            return InfiniteLoop(RayleighDistribution<TGen>.Sample, _gen, sigma);
        }

        /// <summary>
        ///   Returns a student's t distributed floating point random number.
        /// </summary>
        /// <param name="nu">
        ///   The parameter nu which is used for generation of student's t distributed random numbers.
        /// </param>
        /// <returns>
        ///   A student's t distributed floating point random number.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="nu"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="StudentsTDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="StudentsTDistribution"/> for each group of parameters.
        /// </remarks>
        public double StudentsT(int nu)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(StudentsTDistribution<TGen>.IsValidParam(nu));
            return StudentsTDistribution<TGen>.Sample(_gen, nu);
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
        ///   This method simply wraps a call to <see cref="StudentsTDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="StudentsTDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> StudentsTSamples(int nu)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(StudentsTDistribution<TGen>.IsValidParam(nu));
            return InfiniteLoop(StudentsTDistribution<TGen>.Sample, _gen, nu);
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
        /// <returns>
        ///   A triangular distributed floating point random number.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is greater than or equal to <paramref name="beta"/>,
        ///   or <paramref name="alpha"/> is greater than <paramref name="gamma"/>,
        ///   or <paramref name="beta"/> is less than <paramref name="gamma"/>.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="TriangularDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="TriangularDistribution"/> for each group of parameters.
        /// </remarks>
        public double Triangular(double alpha, double beta, double gamma)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(TriangularDistribution<TGen>.AreValidParams(alpha, beta, gamma));
            return TriangularDistribution<TGen>.Sample(_gen, alpha, beta, gamma);
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
        ///   <paramref name="alpha"/> is greater than or equal to <paramref name="beta"/>,
        ///   or <paramref name="alpha"/> is greater than <paramref name="gamma"/>,
        ///   or <paramref name="beta"/> is less than <paramref name="gamma"/>.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="TriangularDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="TriangularDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> TriangularSamples(double alpha, double beta, double gamma)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(TriangularDistribution<TGen>.AreValidParams(alpha, beta, gamma));
            return InfiniteLoop(TriangularDistribution<TGen>.Sample, _gen, alpha, beta, gamma);
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
        /// <returns>
        ///   A weibull distributed floating point random number.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="lambda"/> are less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="WeibullDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="WeibullDistribution"/> for each group of parameters.
        /// </remarks>
        public double Weibull(double alpha, double lambda)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(WeibullDistribution<TGen>.AreValidParams(alpha, lambda));
            return WeibullDistribution<TGen>.Sample(_gen, alpha, lambda);
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
        /// <returns>
        ///   An infinite sequence of weibull distributed floating point random numbers.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="lambda"/> are less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="WeibullDistribution{TGen}.NextDouble"/>,
        ///   with a prior adjustement of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   This method is slightly more efficient when called with the same parameters.
        ///   If you absolutely need the best performance, you may consider using
        ///   an instance of <see cref="WeibullDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> WeibullSamples(double alpha, double lambda)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(WeibullDistribution<TGen>.AreValidParams(alpha, lambda));
            return InfiniteLoop(WeibullDistribution<TGen>.Sample, _gen, alpha, lambda);
        }

        #endregion Continuous Distributions

        #region IGenerator Members

        public uint Seed => _gen.Seed;

        public bool CanReset => _gen.CanReset;

        public bool Reset() => _gen.Reset(Seed);
        public bool Reset(uint seed) => _gen.Reset(seed);

        public int Next() => _gen.Next();
        public int NextInclusiveMaxValue() => _gen.NextInclusiveMaxValue();
        public int Next(int maxValue) => _gen.Next(maxValue);
        public int Next(int minValue, int maxValue) => _gen.Next(minValue, maxValue);

        public double NextDouble() => _gen.NextDouble();
        public double NextDouble(double maxValue) => _gen.NextDouble(maxValue);
        public double NextDouble(double minValue, double maxValue) => _gen.NextDouble(minValue, maxValue);

        public uint NextUInt() => _gen.NextUInt();
        public uint NextUInt(uint maxValue) => _gen.NextUInt(maxValue);
        public uint NextUIntExclusiveMaxValue() => _gen.NextUIntExclusiveMaxValue();
        public uint NextUInt(uint minValue, uint maxValue) => _gen.NextUInt(minValue, maxValue);

        public bool NextBoolean() => _gen.NextBoolean();

        public void NextBytes(byte[] buffer)
        {
            _gen.NextBytes(buffer);
        }

        #endregion IGenerator Members

        #region Object Members

        public override string ToString() => string.Format("Generator: {0}", _gen);

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

    /// <summary>
    ///   A random generator class similar to the one Python offers,
    ///   providing functions similar to the ones found in <see cref="Random"/>
    ///   and functions returning random numbers according to a particular kind of distribution.
    /// </summary>
    [Serializable]
    public sealed class TRandom : TRandom<IGenerator>
    {
        /// <summary>
        ///   Constructs a new instance with <see cref="XorShift128Generator"/> as underlying generator
        ///   and the default seed (which corresponds to <see cref="Environment.TickCount"/>).
        /// </summary>
        public TRandom() : this(new XorShift128Generator(Environment.TickCount))
        {
        }

        /// <summary>
        ///   Constructs a new instance with <see cref="XorShift128Generator"/> as underlying generator
        ///   and the specified seed.
        /// </summary>
        /// <param name="seed">The seed used to initialize the generator.</param>
        public TRandom(int seed) : this(new XorShift128Generator(seed))
        {
        }

        /// <summary>
        ///   Constructs a new instance with <see cref="XorShift128Generator"/> as underlying generator
        ///   and the specified seed.
        /// </summary>
        /// <param name="seed">The seed used to initialize the generator.</param>
        public TRandom(uint seed) : this(new XorShift128Generator(seed))
        {
        }

        /// <summary>
        ///   Constructs a new instance with the specified generator.
        /// </summary>
        /// <param name="generator">The generator used to produce random numbers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is null.</exception>
        public TRandom(IGenerator generator) : base(generator)
        {
        }

        /// <summary>
        ///   Constructs a new instance with <see cref="XorShift128Generator"/> as underlying generator
        ///   and the default seed (which corresponds to <see cref="Environment.TickCount"/>).
        /// </summary>
        public static TRandom<XorShift128Generator> New() => new TRandom<XorShift128Generator>(new XorShift128Generator(Environment.TickCount));

        /// <summary>
        ///   Constructs a new instance with <see cref="XorShift128Generator"/> as underlying generator
        ///   and the specified seed.
        /// </summary>
        /// <param name="seed">The seed used to initialize the generator.</param>
        public static TRandom<XorShift128Generator> New(int seed) => new TRandom<XorShift128Generator>(new XorShift128Generator(seed));

        /// <summary>
        ///   Constructs a new instance with <see cref="XorShift128Generator"/> as underlying generator
        ///   and the specified seed.
        /// </summary>
        /// <param name="seed">The seed used to initialize the generator.</param>
        public static TRandom<XorShift128Generator> New(uint seed) => new TRandom<XorShift128Generator>(new XorShift128Generator(seed));

        /// <summary>
        ///   Constructs a new instance with the specified generator.
        /// </summary>
        /// <param name="generator">The generator used to produce random numbers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is null.</exception>
        public static TRandom<TGen> New<TGen>(TGen generator) where TGen : IGenerator => new TRandom<TGen>(generator);
    }
}