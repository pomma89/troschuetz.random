// The MIT License (MIT)
//
// Copyright (c) 2006-2007 Stefan Troschütz <stefan@troschuetz.de>
//
// Copyright (c) 2012-2019 Alessio Parma <alessio.parma@gmail.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute,
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT
// OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Troschuetz.Random
{
    using Core;
    using Distributions.Continuous;
    using Distributions.Discrete;
    using Generators;
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///   A random generator class similar to the one Python offers, providing functions similar to
    ///   the ones found in <see cref="System.Random"/> and functions returning random numbers
    ///   according to a particular kind of distribution.
    /// </summary>
    /// <remarks>The thread safety of this class depends on the one of the underlying generator.</remarks>
    [Serializable]
    public sealed class TRandom : IGenerator
    {
        #region Properties

        /// <summary>
        ///   The generator used by <see cref="TRandom"/>.
        /// </summary>
        public IGenerator Generator { get; }

        #endregion Properties

        #region Construction

        /// <summary>
        ///   Constructs a new instance with <see cref="XorShift128Generator"/> as underlying
        ///   generator and the default seed (which corresponds to <see cref="TMath.Seed()"/>).
        /// </summary>
        public TRandom() : this(new XorShift128Generator(TMath.Seed()))
        {
        }

        /// <summary>
        ///   Constructs a new instance with <see cref="XorShift128Generator"/> as underlying
        ///   generator and the specified seed.
        /// </summary>
        /// <param name="seed">The seed used to initialize the generator.</param>
        public TRandom(int seed) : this(new XorShift128Generator(seed))
        {
        }

        /// <summary>
        ///   Constructs a new instance with <see cref="XorShift128Generator"/> as underlying
        ///   generator and the specified seed.
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
        public TRandom(IGenerator generator)
        {
            Generator = generator ?? throw new ArgumentNullException(nameof(generator), ErrorMessages.NullGenerator);
        }

        /// <summary>
        ///   Constructs a new instance with <see cref="XorShift128Generator"/> as underlying
        ///   generator and the default seed (which corresponds to <see cref="TMath.Seed()"/>).
        /// </summary>
        public static TRandom New() => new TRandom(new XorShift128Generator(TMath.Seed()));

        /// <summary>
        ///   Constructs a new instance with <see cref="XorShift128Generator"/> as underlying
        ///   generator and the specified seed.
        /// </summary>
        /// <param name="seed">The seed used to initialize the generator.</param>
        public static TRandom New(int seed) => new TRandom(new XorShift128Generator(seed));

        /// <summary>
        ///   Constructs a new instance with <see cref="XorShift128Generator"/> as underlying
        ///   generator and the specified seed.
        /// </summary>
        /// <param name="seed">The seed used to initialize the generator.</param>
        public static TRandom New(uint seed) => new TRandom(new XorShift128Generator(seed));

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
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="BernoulliDistribution"/> for each group of parameters.
        /// </remarks>
        public int Bernoulli(double alpha)
        {
            if (!BernoulliDistribution.IsValidParam(alpha)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return BernoulliDistribution.Sample(Generator, alpha);
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
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="BernoulliDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<int> BernoulliSamples(double alpha)
        {
            if (!BernoulliDistribution.IsValidParam(alpha)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return InfiniteLoop(BernoulliDistribution.Sample, Generator, alpha);
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
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="BinomialDistribution"/> for each group of parameters.
        /// </remarks>
        public int Binomial(double alpha, int beta)
        {
            if (!BinomialDistribution.AreValidParams(alpha, beta)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return BinomialDistribution.Sample(Generator, alpha, beta);
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
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="BinomialDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<int> BinomialSamples(double alpha, int beta)
        {
            if (!BinomialDistribution.AreValidParams(alpha, beta)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return InfiniteLoop(BinomialDistribution.Sample, Generator, alpha, beta);
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
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="CategoricalDistribution"/> for each group of parameters.
        /// </remarks>
        public int Categorical(int valueCount)
        {
            if (valueCount <= 0) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            CategoricalDistribution.SetUp(valueCount, null, out double[] cdf);
            return CategoricalDistribution.Sample(Generator, cdf);
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
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="CategoricalDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<int> CategoricalSamples(int valueCount)
        {
            if (valueCount <= 0) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            CategoricalDistribution.SetUp(valueCount, null, out double[] cdf);
            return InfiniteLoop(CategoricalDistribution.Sample, Generator, cdf);
        }

        /// <summary>
        ///   Returns a categorical distributed 32-bit signed integer.
        /// </summary>
        /// <param name="weights">
        ///   An enumerable of nonnegative weights: this enumerable does not need to be normalized as
        ///   this is often impossible using floating point arithmetic.
        /// </param>
        /// <returns>A categorical distributed 32-bit signed integer.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="weights"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="weights"/> is empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   Any of the weights in <paramref name="weights"/> are negative or they sum to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="CategoricalDistribution.Next"/>, with a
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="CategoricalDistribution"/> for each group of parameters.
        /// </remarks>
        public int Categorical(ICollection<double> weights)
        {
            if (!CategoricalDistribution.IsValidParam(weights)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            CategoricalDistribution.SetUp(weights.Count, weights, out double[] cdf);
            return CategoricalDistribution.Sample(Generator, cdf);
        }

        /// <summary>
        ///   Returns an infinite sequence of categorical distributed 32-bit signed integers.
        /// </summary>
        /// <param name="weights">
        ///   An enumerable of nonnegative weights: this enumerable does not need to be normalized as
        ///   this is often impossible using floating point arithmetic.
        /// </param>
        /// <returns>An infinite sequence of categorical distributed 32-bit signed integers.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="weights"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="weights"/> is empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   Any of the weights in <paramref name="weights"/> are negative or they sum to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="CategoricalDistribution.Next"/>, with a
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="CategoricalDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<int> CategoricalSamples(ICollection<double> weights)
        {
            if (!CategoricalDistribution.IsValidParam(weights)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            CategoricalDistribution.SetUp(weights.Count, weights, out double[] cdf);
            return InfiniteLoop(CategoricalDistribution.Sample, Generator, cdf);
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
        /// <returns>A discrete uniform distributed 32-bit signed integer.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is greater than <paramref name="beta"/>, or
        ///   <paramref name="beta"/> is equal to <see cref="int.MaxValue"/>.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="DiscreteUniformDistribution.Next"/>, with
        ///   a prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="DiscreteUniformDistribution"/> for each group of parameters.
        /// </remarks>
        public int DiscreteUniform(int alpha, int beta)
        {
            if (!DiscreteUniformDistribution.AreValidParams(alpha, beta)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return DiscreteUniformDistribution.Sample(Generator, alpha, beta);
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
        /// <returns>An infinite sequence of discrete uniform distributed 32-bit signed integers.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is greater than <paramref name="beta"/>, or
        ///   <paramref name="beta"/> is equal to <see cref="int.MaxValue"/>.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="DiscreteUniformDistribution.Next"/>, with
        ///   a prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="DiscreteUniformDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<int> DiscreteUniformSamples(int alpha, int beta)
        {
            if (!DiscreteUniformDistribution.AreValidParams(alpha, beta)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return InfiniteLoop(DiscreteUniformDistribution.Sample, Generator, alpha, beta);
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
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="GeometricDistribution"/> for each group of parameters.
        /// </remarks>
        public int Geometric(double alpha)
        {
            if (!GeometricDistribution.IsValidParam(alpha)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return GeometricDistribution.Sample(Generator, alpha);
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
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="GeometricDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<int> GeometricSamples(double alpha)
        {
            if (!GeometricDistribution.IsValidParam(alpha)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return InfiniteLoop(GeometricDistribution.Sample, Generator, alpha);
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
        ///   This method simply wraps a call to <see cref="PoissonDistribution.Next"/>, with a prior
        ///   check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="PoissonDistribution"/> for each group of parameters.
        /// </remarks>
        public int Poisson(double lambda)
        {
            if (!PoissonDistribution.IsValidParam(lambda)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return PoissonDistribution.Sample(Generator, lambda);
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
        ///   This method simply wraps a call to <see cref="PoissonDistribution.Next"/>, with a prior
        ///   check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="PoissonDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<int> PoissonSamples(double lambda)
        {
            if (!PoissonDistribution.IsValidParam(lambda)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return InfiniteLoop(PoissonDistribution.Sample, Generator, lambda);
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
        ///   This method simply wraps a call to <see cref="BetaDistribution.Sample"/>, with a prior
        ///   check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="BetaDistribution"/> for each group of parameters.
        /// </remarks>
        public double Beta(double alpha, double beta)
        {
            if (!BetaDistribution.AreValidParams(alpha, beta)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return BetaDistribution.Sample(Generator, alpha, beta);
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
        ///   This method simply wraps a call to <see cref="BetaDistribution.Sample"/>, with a prior
        ///   check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="BetaDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> BetaSamples(double alpha, double beta)
        {
            if (!BetaDistribution.AreValidParams(alpha, beta)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return InfiniteLoop(BetaDistribution.Sample, Generator, alpha, beta);
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
        ///   This method simply wraps a call to <see cref="BetaPrimeDistribution.Sample"/>, with a
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="BetaPrimeDistribution"/> for each group of parameters.
        /// </remarks>
        public double BetaPrime(double alpha, double beta)
        {
            if (!BetaPrimeDistribution.AreValidParams(alpha, beta)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return BetaPrimeDistribution.Sample(Generator, alpha, beta);
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
        ///   This method simply wraps a call to <see cref="BetaPrimeDistribution.Sample"/>, with a
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="BetaPrimeDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> BetaPrimeSamples(double alpha, double beta)
        {
            if (!BetaPrimeDistribution.AreValidParams(alpha, beta)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return InfiniteLoop(BetaPrimeDistribution.Sample, Generator, alpha, beta);
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
        ///   This method simply wraps a call to <see cref="CauchyDistribution.Sample"/>, with a
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="CauchyDistribution"/> for each group of parameters.
        /// </remarks>
        public double Cauchy(double alpha, double gamma)
        {
            if (!CauchyDistribution.AreValidParams(alpha, gamma)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return CauchyDistribution.Sample(Generator, alpha, gamma);
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
        ///   This method simply wraps a call to <see cref="CauchyDistribution.Sample"/>, with a
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="CauchyDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> CauchySamples(double alpha, double gamma)
        {
            if (!CauchyDistribution.AreValidParams(alpha, gamma)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return InfiniteLoop(CauchyDistribution.Sample, Generator, alpha, gamma);
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
        ///   This method simply wraps a call to <see cref="ChiDistribution.Sample"/>, with a prior
        ///   check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="ChiDistribution"/> for each group of parameters.
        /// </remarks>
        public double Chi(int alpha)
        {
            if (!ChiDistribution.IsValidParam(alpha)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return ChiDistribution.Sample(Generator, alpha);
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
        ///   This method simply wraps a call to <see cref="ChiDistribution.Sample"/>, with a prior
        ///   check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="ChiDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> ChiSamples(int alpha)
        {
            if (!ChiDistribution.IsValidParam(alpha)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return InfiniteLoop(ChiDistribution.Sample, Generator, alpha);
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
        ///   This method simply wraps a call to <see cref="ChiSquareDistribution.Sample"/>, with a
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="ChiSquareDistribution"/> for each group of parameters.
        /// </remarks>
        public double ChiSquare(int alpha)
        {
            if (!ChiSquareDistribution.IsValidParam(alpha)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return ChiSquareDistribution.Sample(Generator, alpha);
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
        ///   This method simply wraps a call to <see cref="ChiSquareDistribution.Sample"/>, with a
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="ChiSquareDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> ChiSquareSamples(int alpha)
        {
            if (!ChiSquareDistribution.IsValidParam(alpha)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return InfiniteLoop(ChiSquareDistribution.Sample, Generator, alpha);
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
        ///   This method simply wraps a call to <see cref="ContinuousUniformDistribution.Sample"/>,
        ///   with a prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="ContinuousUniformDistribution"/> for each group of parameters.
        /// </remarks>
        public double ContinuousUniform(double alpha, double beta)
        {
            if (!ContinuousUniformDistribution.AreValidParams(alpha, beta)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return ContinuousUniformDistribution.Sample(Generator, alpha, beta);
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
        ///   This method simply wraps a call to <see cref="ContinuousUniformDistribution.Sample"/>,
        ///   with a prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="ContinuousUniformDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> ContinuousUniformSamples(double alpha, double beta)
        {
            if (!ContinuousUniformDistribution.AreValidParams(alpha, beta)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return InfiniteLoop(ContinuousUniformDistribution.Sample, Generator, alpha, beta);
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
        ///   This method simply wraps a call to <see cref="ErlangDistribution.Sample"/>, with a
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="ErlangDistribution"/> for each group of parameters.
        /// </remarks>
        public double Erlang(int alpha, double lambda)
        {
            if (!ErlangDistribution.AreValidParams(alpha, lambda)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return ErlangDistribution.Sample(Generator, alpha, lambda);
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
        ///   This method simply wraps a call to <see cref="ErlangDistribution.Sample"/>, with a
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="ErlangDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> ErlangSamples(int alpha, double lambda)
        {
            if (!ErlangDistribution.AreValidParams(alpha, lambda)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return InfiniteLoop(ErlangDistribution.Sample, Generator, alpha, lambda);
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
        ///   This method simply wraps a call to <see cref="ExponentialDistribution.Sample"/>, with a
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="ExponentialDistribution"/> for each group of parameters.
        /// </remarks>
        public double Exponential(double lambda)
        {
            if (!ExponentialDistribution.IsValidParam(lambda)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return ExponentialDistribution.Sample(Generator, lambda);
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
        ///   This method simply wraps a call to <see cref="ExponentialDistribution.Sample"/>, with a
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="ExponentialDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> ExponentialSamples(double lambda)
        {
            if (!ExponentialDistribution.IsValidParam(lambda)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return InfiniteLoop(ExponentialDistribution.Sample, Generator, lambda);
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
        ///   This method simply wraps a call to <see cref="FisherSnedecorDistribution.Sample"/>,
        ///   with a prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="FisherSnedecorDistribution"/> for each group of parameters.
        /// </remarks>
        public double FisherSnedecor(int alpha, int beta)
        {
            if (!FisherSnedecorDistribution.AreValidParams(alpha, beta)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return FisherSnedecorDistribution.Sample(Generator, alpha, beta);
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
        ///   This method simply wraps a call to <see cref="FisherSnedecorDistribution.Sample"/>,
        ///   with a prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="FisherSnedecorDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> FisherSnedecorSamples(int alpha, int beta)
        {
            if (!FisherSnedecorDistribution.AreValidParams(alpha, beta)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return InfiniteLoop(FisherSnedecorDistribution.Sample, Generator, alpha, beta);
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
        ///   This method simply wraps a call to <see cref="FisherTippettDistribution.Sample"/>, with
        ///   a prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="FisherTippettDistribution"/> for each group of parameters.
        /// </remarks>
        public double FisherTippett(double alpha, double mu)
        {
            if (!FisherTippettDistribution.AreValidParams(alpha, mu)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return FisherTippettDistribution.Sample(Generator, alpha, mu);
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
        ///   This method simply wraps a call to <see cref="FisherTippettDistribution.Sample"/>, with
        ///   a prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="FisherTippettDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> FisherTippettSamples(double alpha, double mu)
        {
            if (!FisherTippettDistribution.AreValidParams(alpha, mu)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return InfiniteLoop(FisherTippettDistribution.Sample, Generator, alpha, mu);
        }

        /// <summary>
        ///   Returns a gamma distributed floating point random number.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of gamma distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of gamma distributed random numbers.
        /// </param>
        /// <returns>A gamma distributed floating point random number.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="beta"/> are less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="GammaDistribution.Sample"/>, with a prior
        ///   check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="GammaDistribution"/> for each group of parameters.
        /// </remarks>
        public double Gamma(double alpha, double beta)
        {
            if (!GammaDistribution.AreValidParams(alpha, beta)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return GammaDistribution.Sample(Generator, alpha, beta);
        }

        /// <summary>
        ///   Returns an infinite sequence of gamma distributed floating point random numbers.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of gamma distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of gamma distributed random numbers.
        /// </param>
        /// <returns>An infinite sequence of gamma distributed floating point random numbers.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="beta"/> are less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="GammaDistribution.Sample"/>, with a prior
        ///   check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="GammaDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> GammaSamples(double alpha, double beta)
        {
            if (!GammaDistribution.AreValidParams(alpha, beta)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return InfiniteLoop(GammaDistribution.Sample, Generator, alpha, beta);
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
        ///   This method simply wraps a call to <see cref="LaplaceDistribution.Sample"/>, with a
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="LaplaceDistribution"/> for each group of parameters.
        /// </remarks>
        public double Laplace(double alpha, double mu)
        {
            if (!LaplaceDistribution.AreValidParams(alpha, mu)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return LaplaceDistribution.Sample(Generator, alpha, mu);
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
        ///   This method simply wraps a call to <see cref="LaplaceDistribution.Sample"/>, with a
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="LaplaceDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> LaplaceSamples(double alpha, double mu)
        {
            if (!LaplaceDistribution.AreValidParams(alpha, mu)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return InfiniteLoop(LaplaceDistribution.Sample, Generator, alpha, mu);
        }

        /// <summary>
        ///   Returns a logistic distributed floating point random number.
        /// </summary>
        /// <param name="mu">
        ///   The parameter mu which is used for generation of logistic distributed random numbers.
        /// </param>
        /// <param name="sigma">
        ///   The parameter sigma which is used for generation of logistic distributed random numbers.
        /// </param>
        /// <returns>A logistic distributed floating point random number.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="sigma"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="LogisticDistribution.Sample"/>, with a
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="LogisticDistribution"/> for each group of parameters.
        /// </remarks>
        public double Logistic(double mu, double sigma)
        {
            if (!LogisticDistribution.AreValidParams(mu, sigma)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return LogisticDistribution.Sample(Generator, mu, sigma);
        }

        /// <summary>
        ///   Returns an infinite sequence of logistic distributed floating point random numbers.
        /// </summary>
        /// <param name="mu">
        ///   The parameter mu which is used for generation of logistic distributed random numbers.
        /// </param>
        /// <param name="sigma">
        ///   The parameter sigma which is used for generation of logistic distributed random numbers.
        /// </param>
        /// <returns>An infinite sequence of logistic distributed floating point random numbers.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="sigma"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="LogisticDistribution.Sample"/>, with a
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="LogisticDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> LogisticSamples(double mu, double sigma)
        {
            if (!LogisticDistribution.AreValidParams(mu, sigma)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return InfiniteLoop(LogisticDistribution.Sample, Generator, mu, sigma);
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
        ///   This method simply wraps a call to <see cref="LognormalDistribution.Sample"/>, with a
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="LognormalDistribution"/> for each group of parameters.
        /// </remarks>
        public double Lognormal(double mu, double sigma)
        {
            if (!LognormalDistribution.AreValidParams(mu, sigma)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return LognormalDistribution.Sample(Generator, mu, sigma);
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
        ///   This method simply wraps a call to <see cref="LognormalDistribution.Sample"/>, with a
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="LognormalDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> LognormalSamples(double mu, double sigma)
        {
            if (!LognormalDistribution.AreValidParams(mu, sigma)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return InfiniteLoop(LognormalDistribution.Sample, Generator, mu, sigma);
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
        ///   This method simply wraps a call to <see cref="NormalDistribution.Sample"/>, with a
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="NormalDistribution"/> for each group of parameters.
        /// </remarks>
        public double Normal(double mu, double sigma)
        {
            if (!NormalDistribution.AreValidParams(mu, sigma)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return NormalDistribution.Sample(Generator, mu, sigma);
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
        ///   This method simply wraps a call to <see cref="NormalDistribution.Sample"/>, with a
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="NormalDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> NormalSamples(double mu, double sigma)
        {
            if (!NormalDistribution.AreValidParams(mu, sigma)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return InfiniteLoop(NormalDistribution.Sample, Generator, mu, sigma);
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
        ///   This method simply wraps a call to <see cref="ParetoDistribution.Sample"/>, with a
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="ParetoDistribution"/> for each group of parameters.
        /// </remarks>
        public double Pareto(double alpha, double beta)
        {
            if (!ParetoDistribution.AreValidParams(alpha, beta)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return ParetoDistribution.Sample(Generator, alpha, beta);
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
        ///   This method simply wraps a call to <see cref="ParetoDistribution.Sample"/>, with a
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="ParetoDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> ParetoSamples(double alpha, double beta)
        {
            if (!ParetoDistribution.AreValidParams(alpha, beta)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return InfiniteLoop(ParetoDistribution.Sample, Generator, alpha, beta);
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
        ///   This method simply wraps a call to <see cref="PowerDistribution.Sample"/>, with a prior
        ///   check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="PowerDistribution"/> for each group of parameters.
        /// </remarks>
        public double Power(double alpha, double beta)
        {
            if (!PowerDistribution.AreValidParams(alpha, beta)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return PowerDistribution.Sample(Generator, alpha, beta);
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
        ///   This method simply wraps a call to <see cref="PowerDistribution.Sample"/>, with a prior
        ///   check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="PowerDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> PowerSamples(double alpha, double beta)
        {
            if (!PowerDistribution.AreValidParams(alpha, beta)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return InfiniteLoop(PowerDistribution.Sample, Generator, alpha, beta);
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
        ///   This method simply wraps a call to <see cref="RayleighDistribution.Sample"/>, with a
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="RayleighDistribution"/> for each group of parameters.
        /// </remarks>
        public double Rayleigh(double sigma)
        {
            if (!RayleighDistribution.IsValidParam(sigma)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return RayleighDistribution.Sample(Generator, sigma);
        }

        /// <summary>
        ///   Returns an infinite sequence of rayleigh distributed floating point random numbers.
        /// </summary>
        /// <param name="sigma">
        ///   The parameter sigma which is used for generation of rayleigh distributed random numbers.
        /// </param>
        /// <returns>An infinite sequence of rayleigh distributed floating point random numbers.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="sigma"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   This method simply wraps a call to <see cref="RayleighDistribution.Sample"/>, with a
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="RayleighDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> RayleighSamples(double sigma)
        {
            if (!RayleighDistribution.IsValidParam(sigma)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return InfiniteLoop(RayleighDistribution.Sample, Generator, sigma);
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
        ///   This method simply wraps a call to <see cref="StudentsTDistribution.Sample"/>, with a
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="StudentsTDistribution"/> for each group of parameters.
        /// </remarks>
        public double StudentsT(int nu)
        {
            if (!StudentsTDistribution.IsValidParam(nu)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return StudentsTDistribution.Sample(Generator, nu);
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
        ///   This method simply wraps a call to <see cref="StudentsTDistribution.Sample"/>, with a
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="StudentsTDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> StudentsTSamples(int nu)
        {
            if (!StudentsTDistribution.IsValidParam(nu)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return InfiniteLoop(StudentsTDistribution.Sample, Generator, nu);
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
        ///   This method simply wraps a call to <see cref="TriangularDistribution.Sample"/>, with a
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="TriangularDistribution"/> for each group of parameters.
        /// </remarks>
        public double Triangular(double alpha, double beta, double gamma)
        {
            if (!TriangularDistribution.AreValidParams(alpha, beta, gamma)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return TriangularDistribution.Sample(Generator, alpha, beta, gamma);
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
        ///   This method simply wraps a call to <see cref="TriangularDistribution.Sample"/>, with a
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="TriangularDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> TriangularSamples(double alpha, double beta, double gamma)
        {
            if (!TriangularDistribution.AreValidParams(alpha, beta, gamma)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return InfiniteLoop(TriangularDistribution.Sample, Generator, alpha, beta, gamma);
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
        ///   This method simply wraps a call to <see cref="WeibullDistribution.Sample"/>, with a
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="WeibullDistribution"/> for each group of parameters.
        /// </remarks>
        public double Weibull(double alpha, double lambda)
        {
            if (!WeibullDistribution.AreValidParams(alpha, lambda)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return WeibullDistribution.Sample(Generator, alpha, lambda);
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
        ///   This method simply wraps a call to <see cref="WeibullDistribution.Sample"/>, with a
        ///   prior check of the distribution parameters.
        /// </remarks>
        /// <remarks>
        ///   If you absolutely need the best performance, you may consider using an instance of
        ///   <see cref="WeibullDistribution"/> for each group of parameters.
        /// </remarks>
        public IEnumerable<double> WeibullSamples(double alpha, double lambda)
        {
            if (!WeibullDistribution.AreValidParams(alpha, lambda)) throw new ArgumentOutOfRangeException(ErrorMessages.InvalidParams);
            return InfiniteLoop(WeibullDistribution.Sample, Generator, alpha, lambda);
        }

        #endregion Continuous Distributions

        #region IGenerator Members

        /// <summary>
        ///   The seed value used by the generator.
        /// </summary>
        public uint Seed => Generator.Seed;

        /// <summary>
        ///   Gets a value indicating whether the random number generator can be reset, so that it
        ///   produces the same random number sequence again.
        /// </summary>
        public bool CanReset => Generator.CanReset;

        /// <summary>
        ///   Resets the random number generator using the initial seed, so that it produces the same
        ///   random number sequence again. To understand whether this generator can be reset, you
        ///   can query the <see cref="IGenerator.CanReset"/> property.
        /// </summary>
        /// <returns>True if the random number generator was reset; otherwise, false.</returns>
        public bool Reset() => Generator.Reset(Seed);

        /// <summary>
        ///   Resets the random number generator using the specified seed, so that it produces the
        ///   same random number sequence again. To understand whether this generator can be reset,
        ///   you can query the <see cref="IGenerator.CanReset"/> property.
        /// </summary>
        /// <param name="seed">The seed value used by the generator.</param>
        /// <returns>True if the random number generator was reset; otherwise, false.</returns>
        public bool Reset(uint seed) => Generator.Reset(seed);

        /// <summary>
        ///   Returns a nonnegative random number less than <see cref="int.MaxValue"/>.
        /// </summary>
        /// <returns>
        ///   A 32-bit signed integer greater than or equal to 0, and less than
        ///   <see cref="int.MaxValue"/>; that is, the range of return values includes 0 but not <see cref="int.MaxValue"/>.
        /// </returns>
        public int Next() => Generator.Next();

        /// <summary>
        ///   Returns a nonnegative random number less than or equal to <see cref="int.MaxValue"/>.
        /// </summary>
        /// <returns>
        ///   A 32-bit signed integer greater than or equal to 0, and less than or equal to
        ///   <see cref="int.MaxValue"/>; that is, the range of return values includes 0 and <see cref="int.MaxValue"/>.
        /// </returns>
        public int NextInclusiveMaxValue() => Generator.NextInclusiveMaxValue();

        /// <summary>
        ///   Returns a nonnegative random number less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated.</param>
        /// <returns>
        ///   A 32-bit signed integer greater than or equal to 0, and less than
        ///   <paramref name="maxValue"/>; that is, the range of return values includes 0 but not <paramref name="maxValue"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="maxValue"/> must be greater than or equal to 0.
        /// </exception>
        public int Next(int maxValue) => Generator.Next(maxValue);

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
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="maxValue"/> must be greater than <paramref name="minValue"/>.
        /// </exception>
        public int Next(int minValue, int maxValue) => Generator.Next(minValue, maxValue);

        /// <summary>
        ///   Returns a nonnegative floating point random number less than 1.0.
        /// </summary>
        /// <returns>
        ///   A double-precision floating point number greater than or equal to 0.0, and less than
        ///   1.0; that is, the range of return values includes 0.0 but not 1.0.
        /// </returns>
        public double NextDouble() => Generator.NextDouble();

        /// <summary>
        ///   Returns a nonnegative floating point random number less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated.</param>
        /// <returns>
        ///   A double-precision floating point number greater than or equal to 0.0, and less than
        ///   <paramref name="maxValue"/>; that is, the range of return values includes 0 but not <paramref name="maxValue"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="maxValue"/> must be greater than or equal to 0.0.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="maxValue"/> cannot be <see cref="double.PositiveInfinity"/>.</exception>
        public double NextDouble(double maxValue) => Generator.NextDouble(maxValue);

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
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="maxValue"/> must be greater than <paramref name="minValue"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///   The difference between <paramref name="maxValue"/> and <paramref name="minValue"/>
        ///   cannot be <see cref="double.PositiveInfinity"/>.
        /// </exception>
        public double NextDouble(double minValue, double maxValue) => Generator.NextDouble(minValue, maxValue);

        /// <summary>
        ///   Returns an unsigned random number.
        /// </summary>
        /// <returns>
        ///   A 32-bit unsigned integer greater than or equal to 0, and less than
        ///   <see cref="uint.MaxValue"/>; that is, the range of return values includes 0 but not <see cref="uint.MaxValue"/>.
        /// </returns>
        public uint NextUInt() => Generator.NextUInt();

        /// <summary>
        ///   Returns an unsigned random number less than <see cref="uint.MaxValue"/>.
        /// </summary>
        /// <returns>
        ///   A 32-bit unsigned integer greater than or equal to 0, and less than
        ///   <see cref="uint.MaxValue"/>; that is, the range of return values includes 0 but not <see cref="uint.MaxValue"/>.
        /// </returns>
        public uint NextUIntExclusiveMaxValue() => Generator.NextUInt();

        /// <summary>
        ///   Returns an unsigned random number.
        /// </summary>
        /// <returns>
        ///   A 32-bit unsigned integer greater than or equal to <see cref="uint.MinValue"/> and less
        ///   than or equal to <see cref="uint.MaxValue"/>.
        /// </returns>
        public uint NextUIntInclusiveMaxValue() => Generator.NextUIntInclusiveMaxValue();

        /// <summary>
        ///   Returns an unsigned random number less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated.</param>
        /// <returns>
        ///   A 32-bit unsigned integer greater than or equal to 0, and less than
        ///   <paramref name="maxValue"/>; that is, the range of return values includes 0 but not <paramref name="maxValue"/>.
        /// </returns>
        public uint NextUInt(uint maxValue) => Generator.NextUInt(maxValue);

        /// <summary>
        ///   Returns an unsigned random number within the specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number to be generated.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated.</param>
        /// <returns>
        ///   A 32-bit unsigned integer greater than or equal to <paramref name="minValue"/> and less
        ///   than <paramref name="maxValue"/>; that is, the range of return values includes
        ///   <paramref name="minValue"/> but not <paramref name="maxValue"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="maxValue"/> must be greater than <paramref name="minValue"/>.
        /// </exception>
        public uint NextUInt(uint minValue, uint maxValue) => Generator.NextUInt(minValue, maxValue);

        /// <summary>
        ///   Returns a random Boolean value.
        /// </summary>
        /// <remarks>
        ///   Buffers 31 random bits for future calls, so the random number generator is only invoked
        ///   once in every 31 calls.
        /// </remarks>
        /// <returns>A <see cref="bool"/> value.</returns>
        public bool NextBoolean() => Generator.NextBoolean();

        /// <summary>
        ///   Fills the elements of a specified array of bytes with random numbers.
        /// </summary>
        /// <remarks>
        ///   Each element of the array of bytes is set to a random number greater than or equal to
        ///   0, and less than or equal to <see cref="byte.MaxValue"/>.
        /// </remarks>
        /// <param name="buffer">An array of bytes to contain random numbers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="buffer"/> is null.</exception>
        public void NextBytes(byte[] buffer)
        {
            Generator.NextBytes(buffer);
        }

#if HAS_SPAN
        /// <summary>
        ///   Fills the elements of a specified span of bytes with random numbers.
        /// </summary>
        /// <remarks>
        ///   Each element of the array of bytes is set to a random number greater than or equal to
        ///   0, and less than or equal to <see cref="byte.MaxValue"/>.
        /// </remarks>
        /// <param name="buffer">A span of bytes to contain random numbers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="buffer"/> is null.</exception>
        public void NextBytes(Span<byte> buffer)
        {
            Generator.NextBytes(buffer);
        }
#endif

        #endregion IGenerator Members

        #region Object Members

        /// <summary>
        ///   Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() => string.Format("Generator: {0}", Generator);

        #endregion Object Members

        #region Private Members

#pragma warning disable RECS0135 // Function does not reach its end or a 'return' statement by any of possible execution paths

        private static IEnumerable<TRet> InfiniteLoop<T1, T2, TRet>(Func<T1, T2, TRet> f, T1 a1, T2 a2)
#pragma warning restore RECS0135 // Function does not reach its end or a 'return' statement by any of possible execution paths
        {
            while (true)
            {
#pragma warning disable CC0031 // Check for null before calling a delegate
                yield return f(a1, a2);
#pragma warning restore CC0031 // Check for null before calling a delegate
            }
        }

#pragma warning disable RECS0135 // Function does not reach its end or a 'return' statement by any of possible execution paths

        private static IEnumerable<TRet> InfiniteLoop<T1, T2, T3, TRet>(Func<T1, T2, T3, TRet> f, T1 a1, T2 a2, T3 a3)
#pragma warning restore RECS0135 // Function does not reach its end or a 'return' statement by any of possible execution paths
        {
            while (true)
            {
#pragma warning disable CC0031 // Check for null before calling a delegate
                yield return f(a1, a2, a3);
#pragma warning restore CC0031 // Check for null before calling a delegate
            }
        }

#pragma warning disable RECS0135 // Function does not reach its end or a 'return' statement by any of possible execution paths

        private static IEnumerable<TRet> InfiniteLoop<T1, T2, T3, T4, TRet>(Func<T1, T2, T3, T4, TRet> f, T1 a1, T2 a2, T3 a3, T4 a4)
#pragma warning restore RECS0135 // Function does not reach its end or a 'return' statement by any of possible execution paths
        {
            while (true)
            {
#pragma warning disable CC0031 // Check for null before calling a delegate
                yield return f(a1, a2, a3, a4);
#pragma warning restore CC0031 // Check for null before calling a delegate
            }
        }

        #endregion Private Members
    }

#if NETSTD10

    /// <summary>
    ///   Stub.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class SerializableAttribute : Attribute
    {
    }

#endif
}