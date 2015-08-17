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

#region Original Copyright

// <copyright file="Categorical.cs" company="Math.NET">
//   Math.NET Numerics, part of the Math.NET Project http://numerics.mathdotnet.com
//   http://github.com/mathnet/mathnet-numerics http://mathnetnumerics.codeplex.com Copyright (c)
//   2009-2010 Math.NET Permission is hereby granted, free of charge, to any person obtaining a copy
//   of this software and associated documentation files (the "Software"), to deal in the Software
//   without restriction, including without limitation the rights to use, copy, modify, merge,
//   publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to
//   whom the Software is furnished to do so, subject to the following conditions: The above
//   copyright notice and this permission notice shall be included in all copies or substantial
//   portions of the Software. THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//   EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR
//   A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
//   LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
//   OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
//   IN THE SOFTWARE.
// </copyright>

#endregion Original Copyright

namespace Troschuetz.Random.Distributions.Discrete
{
    using Core;
    using Generators;
    using PommaLabs.Thrower;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    ///   Implements the categorical distribution. For details about this distribution, see
    ///   <a href="http://en.wikipedia.org/wiki/Categorical_distribution">Wikipedia - Categorical
    ///   distribution</a>. This distribution is sometimes called the Discrete distribution.
    /// </summary>
    /// <remarks>
    ///   The distribution is parameterized by a vector of ratios: in other words, the parameter
    ///   does not have to be normalized and sum to 1. The reason is that some vectors can't be
    ///   exactly normalized to sum to 1 in floating point representation.
    /// </remarks>
    [Serializable]
    public sealed class CategoricalDistribution : AbstractDistribution, IDiscreteDistribution, IWeightsDistribution<double>
    {
        #region Constants

        /// <summary>
        ///   The default number of values used for categorical distribution, if not specified otherwise.
        /// </summary>
        public const int DefaultValueCount = 3;

        #endregion Constants

        #region Fields

        /// <summary>
        ///   Stores the cumulative distribution of current weights.
        /// </summary>
        double[] _cdf;

        /// <summary>
        ///   Stores the unnormalized categorical weights.
        /// </summary>
        List<double> _weights;

        /// <summary>
        ///   Stores the sum of all weights currently available.
        /// </summary>
        double _weightsSum;

        /// <summary>
        ///   Gets or sets the normalized probability vector of the categorical distribution.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> is empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   Any of the weights in <paramref name="value"/> are negative or they sum to zero.
        /// </exception>
        /// <remarks>
        ///   Sometimes the normalized probability vector cannot be represented exactly in a
        ///   floating point representation.
        /// </remarks>
        public ICollection<double> Weights
        {
            get { return _weights.Select(w => w / _weightsSum).ToList(); }
            set
            {
                RaiseArgumentNullException.IfIsNull(value, nameof(value), ErrorMessages.NullWeights);
                Raise<ArgumentException>.IfIsEmpty(value);
                Raise<ArgumentOutOfRangeException>.IfNot(AreValidWeights(value), ErrorMessages.InvalidParams);
                _weights = value.ToList();
                UpdateHelpers();
            }
        }

        #endregion Fields

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="BinomialDistribution"/> class, using a
        ///   <see cref="NumericalRecipes3Q1Generator"/> as underlying random number generator.
        /// </summary>
        public CategoricalDistribution() : this(new NumericalRecipes3Q1Generator(), DefaultValueCount)
        {
            Debug.Assert(Generator is NumericalRecipes3Q1Generator);
            Debug.Assert(Equals(Weights.Count, DefaultValueCount));
            Debug.Assert(Weights.All(w => w == 1.0 / DefaultValueCount));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="BinomialDistribution"/> class, using a
        ///   <see cref="NumericalRecipes3Q1Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        public CategoricalDistribution(uint seed) : this(new NumericalRecipes3Q1Generator(seed), DefaultValueCount)
        {
            Debug.Assert(Generator is NumericalRecipes3Q1Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Weights.Count, DefaultValueCount));
            Debug.Assert(Weights.All(w => w == 1.0 / DefaultValueCount));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="CategoricalDistribution"/> class, using
        ///   the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        public CategoricalDistribution(IGenerator generator) : this(generator, DefaultValueCount)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Weights.Count, DefaultValueCount));
            Debug.Assert(Weights.All(w => w == 1.0 / DefaultValueCount));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="BinomialDistribution"/> class, using a
        ///   <see cref="NumericalRecipes3Q1Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="valueCount">
        ///   The parameter valueCount which is used for generation of binomial distributed random
        ///   numbers by setting the number of equi-distributed "weights" the distribution will have.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="valueCount"/> is less than or equal to zero.
        /// </exception>
        public CategoricalDistribution(int valueCount) : this(new NumericalRecipes3Q1Generator(), valueCount)
        {
            Debug.Assert(Generator is NumericalRecipes3Q1Generator);
            Debug.Assert(Equals(Weights.Count, valueCount));
            Debug.Assert(Weights.All(w => w == 1.0 / valueCount));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="BinomialDistribution"/> class, using a
        ///   <see cref="NumericalRecipes3Q1Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        /// <param name="valueCount">
        ///   The parameter valueCount which is used for generation of binomial distributed random
        ///   numbers by setting the number of equi-distributed "weights" the distribution will have.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="valueCount"/> is less than or equal to zero.
        /// </exception>
        public CategoricalDistribution(uint seed, int valueCount) : this(new NumericalRecipes3Q1Generator(seed), valueCount)
        {
            Debug.Assert(Generator is NumericalRecipes3Q1Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Weights.Count, valueCount));
            Debug.Assert(Weights.All(w => w == 1.0 / valueCount));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="BinomialDistribution"/> class, using a
        ///   <see cref="NumericalRecipes3Q1Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="weights">
        ///   An enumerable of nonnegative weights: this enumerable does not need to be normalized
        ///   as this is often impossible using floating point arithmetic.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="weights"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="weights"/> is empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   Any of the weights in <paramref name="weights"/> are negative or they sum to zero.
        /// </exception>
        public CategoricalDistribution(ICollection<double> weights) : this(new NumericalRecipes3Q1Generator(), weights)
        {
            Debug.Assert(Generator is NumericalRecipes3Q1Generator);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="BinomialDistribution"/> class, using a
        ///   <see cref="NumericalRecipes3Q1Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        /// <param name="weights">
        ///   An enumerable of nonnegative weights: this enumerable does not need to be normalized
        ///   as this is often impossible using floating point arithmetic.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="weights"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="weights"/> is empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   Any of the weights in <paramref name="weights"/> are negative or they sum to zero.
        /// </exception>
        public CategoricalDistribution(uint seed, ICollection<double> weights)
            : this(new NumericalRecipes3Q1Generator(seed), weights)
        {
            Debug.Assert(Generator is NumericalRecipes3Q1Generator);
            Debug.Assert(Generator.Seed == seed);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="CategoricalDistribution"/> class, using
        ///   the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="valueCount">
        ///   The parameter valueCount which is used for generation of binomial distributed random
        ///   numbers by setting the number of equi-distributed "weights" the distribution will have.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="valueCount"/> is less than or equal to zero.
        /// </exception>
        public CategoricalDistribution(IGenerator generator, int valueCount) : base(generator)
        {
            RaiseArgumentOutOfRangeException.IfIsEqual(valueCount, 0, ErrorMessages.InvalidParams);
            _weights = Ones(valueCount);
            UpdateHelpers();
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="CategoricalDistribution"/> class, using
        ///   the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="weights">
        ///   An enumerable of nonnegative weights: this enumerable does not need to be normalized
        ///   as this is often impossible using floating point arithmetic.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> or <paramref name="weights"/> are null.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="weights"/> is empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   Any of the weights in <paramref name="weights"/> are negative or they sum to zero.
        /// </exception>
        public CategoricalDistribution(IGenerator generator, ICollection<double> weights) : base(generator)
        {
            RaiseArgumentNullException.IfIsNull(weights, nameof(weights), ErrorMessages.NullWeights);
            Raise<ArgumentException>.IfIsEmpty(weights);
            Raise<ArgumentOutOfRangeException>.IfNot(AreValidWeights(weights), ErrorMessages.InvalidParams);
            _weights = weights.ToList();
            UpdateHelpers();
        }

        #endregion Construction

        #region Class Methods

        /// <summary>
        ///   Generates a list containing only ones.
        /// </summary>
        /// <param name="valueCount">The number of ones list will have.</param>
        /// <returns>A list containing only ones.</returns>
        static List<double> Ones(int valueCount)
        {
            Debug.Assert(valueCount > 0);
            var ones = new List<double>(valueCount);
            for (var i = 0; i < valueCount; ++i)
            {
                ones.Add(1.0);
            }
            return ones;
        }

        #endregion Class Methods

        #region Instance Methods

        /// <summary>
        ///   Determines whether categorical distribution is defined under given weights.
        /// </summary>
        /// <param name="weights">
        ///   The weights which are used for generation of categorical distributed random numbers.
        ///   Weights do not need to be normalized as this is often impossible using floating point arithmetic.
        /// </param>
        /// <returns>
        ///   False if any of the weights is negative or if the sum of parameters is 0.0; otherwise,
        ///   it returns true.
        /// </returns>
        public bool AreValidWeights(IEnumerable<double> weights) => IsValidParam(weights);

        /// <summary>
        ///   Computes the unnormalized cumulative distribution function and other attributes for
        ///   the distribution (like mean, variance, and so on).
        /// </summary>
        void UpdateHelpers()
        {
            var weightsSum = 0.0; // It will store the sum of all weights.
            var cdf = new double[_weights.Count]; // It will store UNNORMALIZED cdf.
            var tmpMean = 0.0;
            var maxW = 0.0; // It will store max weight (all weights are positive).
            var maxI = 0; // It will store max weight index.

            // One big loop to compute all helpers needed by this distribution.
            for (var i = 0; i < _weights.Count; ++i)
            {
                var w = _weights[i];
                weightsSum += w;
                cdf[i] = weightsSum;
                tmpMean += w * (i + 1); // Plus one because it is zero-based.
                if (w <= maxW)
                {
                    continue;
                }
                maxW = w;
                maxI = i;
            }

            // Finalize some results...
            _weightsSum = weightsSum;
            _cdf = cdf;
            Mean = (tmpMean / weightsSum) - 1; // Minus one to make it zero-based.
            Mode = new double[] { maxI };

            var halfWeightsSum = weightsSum / 2;
            var tmpMedian = double.NaN;
            var tmpVar = 0.0;

            // We still need another loop to compute variance; as for the mean, the plus/minus one
            // is needed.
            for (var i = 0; i < _weights.Count; ++i)
            {
                if (double.IsNaN(tmpMedian) && _cdf[i] >= halfWeightsSum)
                {
                    tmpMedian = i;
                }
                tmpVar += _weights[i] * Sqr(i + 1 - Mean);
            }

            // Finalize last results...
            Median = tmpMedian;
            Variance = (tmpVar / weightsSum) - 1;
        }

        #endregion Instance Methods

        #region IDiscreteDistribution Members

        /// <summary>
        ///   Gets the minimum possible value of distributed random numbers.
        /// </summary>
        public double Minimum => 0;

        /// <summary>
        ///   Gets the maximum possible value of distributed random numbers.
        /// </summary>
        public double Maximum => _weights.Count - 1;

        /// <summary>
        ///   Gets the mean of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if mean is not defined for given distribution with some parameters.
        /// </exception>
        public double Mean { get; private set; }

        /// <summary>
        ///   Gets the median of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if median is not defined for given distribution with some parameters.
        /// </exception>
        public double Median { get; private set; }

        /// <summary>
        ///   Gets the variance of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if variance is not defined for given distribution with some parameters.
        /// </exception>
        public double Variance { get; private set; }

        /// <summary>
        ///   Gets the mode of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if mode is not defined for given distribution with some parameters.
        /// </exception>
        public double[] Mode { get; private set; }

        /// <summary>
        ///   Returns a distributed random number.
        /// </summary>
        /// <returns>A distributed 32-bit signed integer.</returns>
        public int Next() => Sample(Generator, _weights.Count, _cdf, _weightsSum);

        /// <summary>
        ///   Returns a distributed floating point random number.
        /// </summary>
        /// <returns>A distributed double-precision floating point number.</returns>
        public double NextDouble() => Sample(Generator, _weights.Count, _cdf, _weightsSum);

        #endregion IDiscreteDistribution Members

        #region Object Members

        public override string ToString() => "Categorical(ValueCount = " + _weights.Count + ")";

        #endregion Object Members

        #region TRandom Helpers

        /// <summary>
        ///   Determines whether categorical distribution is defined under given weights. The
        ///   default definition returns false if any of the weights is negative or if the sum of
        ///   parameters is 0.0; otherwise, it returns true.
        /// 
        ///   Weights do not need to be normalized as this is often impossible using floating point arithmetic.
        /// </summary>
        /// <remarks>
        ///   This is an extensibility point for the <see cref="CategoricalDistribution"/> class.
        /// </remarks>
        public static Func<IEnumerable<double>, bool> IsValidParam { get; set; } = weights =>
        {
            var sum = 0.0;
            foreach (var w in weights)
            {
                if (w < 0.0 || double.IsNaN(w))
                {
                    return false;
                }
                sum += w;
            }
            return sum != 0.0;
        };

        /// <summary>
        ///   Declares a function returning a categorical distributed 32-bit signed integer.
        /// </summary>
        /// <remarks>
        ///   This is an extensibility point for the <see cref="CategoricalDistribution"/> class.
        /// </remarks>
        public static Func<IGenerator, int, double[], double, int> Sample { get; set; } = (generator, weightsCount, cdf, weightsSum) =>
        {
            var u = generator.NextDouble(weightsSum);
            var minIdx = 0;
            var maxIdx = weightsCount - 1;
            while (minIdx < maxIdx)
            {
                var idx = (maxIdx - minIdx) / 2 + minIdx;
                var c = cdf[idx];
                if (u == c)
                {
                    minIdx = idx;
                    break;
                }
                if (u < c)
                {
                    maxIdx = idx;
                }
                else
                {
                    minIdx = idx + 1;
                }
            }
            return minIdx;
        };

        internal static void SetUp(int weightsCount, IEnumerable<double> weights, out double[] cdf, out double weightsSum)
        {
            var weightsList = (weights == null) ? Ones(weightsCount) : weights.ToList();
            weightsSum = 0.0; // It will store the sum of all weights.
            cdf = new double[weightsCount]; // It will store UNNORMALIZED cdf.
            var maxW = 0.0; // It will store max weight (all weights are positive).

            for (var i = 0; i < weightsCount; ++i)
            {
                var w = weightsList[i];
                weightsSum += w;
                cdf[i] = weightsSum;
                if (w <= maxW)
                {
                    continue;
                }
                maxW = w;
            }
        }

        #endregion TRandom Helpers
    }
}
