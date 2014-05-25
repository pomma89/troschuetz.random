/*
 * Copyright © 2012 Alessio Parma (alessio.parma@gmail.com)
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

#region Original Copyright

// <copyright file="Categorical.cs" company="Math.NET">
// Math.NET Numerics, part of the Math.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
// http://mathnetnumerics.codeplex.com
// Copyright (c) 2009-2010 Math.NET
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

#endregion

namespace Troschuetz.Random.Distributions.Discrete
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Generators;
    using JetBrains.Annotations;

    /// <summary>
    ///   Implements the categorical distribution. For details about this distribution, see 
    ///   <a href="http://en.wikipedia.org/wiki/Categorical_distribution">Wikipedia - Categorical distribution</a>.
    ///   This distribution is sometimes called the Discrete distribution.
    /// </summary>
    /// <remarks>
    ///   The distribution is parameterized by a vector of ratios: in other words, the parameter
    ///   does not have to be normalized and sum to 1. The reason is that some vectors can't be exactly normalized
    ///   to sum to 1 in floating point representation.
    /// </remarks>
    [PublicAPI]
    public class CategoricalDistribution<TGen> : Distribution<TGen>, IDiscreteDistribution, IWeightsDistribution<double>
        where TGen : IGenerator
    {
        #region Class Fields

        /// <summary>
        ///   The default number of values used for categorical distribution,
        ///   if not specified otherwise.
        /// </summary>
        public const int DefaultValueCount = 3;

        #endregion

        #region Instance Fields

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
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="value"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> is empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   Any of the weights in <paramref name="value"/> are negative or they sum to zero.
        /// </exception>
        /// <remarks>
        ///   Sometimes the normalized probability vector cannot be represented
        ///   exactly in a floating point representation.
        /// </remarks>
        public ICollection<double> Weights
        {
            get { return _weights.Select(w => w/_weightsSum).ToList(); }
            set
            {
                _weights = value.ToList();
                UpdateHelpers();
            }
        }

        #endregion

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="CategoricalDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="valueCount">
        ///   The parameter valueCount which is used for generation of binomial distributed random numbers
        ///   by setting the number of equi-distributed "weights" the distribution will have.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="valueCount"/> is less than or equal to zero.
        /// </exception>
        public CategoricalDistribution(TGen generator, int valueCount) : base(generator)
        {
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(generator, null), ErrorMessages.NullGenerator); 
            Contract.Requires<ArgumentNullException>(valueCount != 0, ErrorMessages.InvalidParams);
            _weights = Ones(valueCount);
            UpdateHelpers();
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="CategoricalDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
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
        public CategoricalDistribution(TGen generator, ICollection<double> weights) : base(generator)
        {
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(generator, null), ErrorMessages.NullGenerator);          
            Contract.Requires<ArgumentNullException>(weights != null, ErrorMessages.NullWeights);     
            Contract.Requires<ArgumentException>(weights.Count > 0);
            Contract.Requires<ArgumentOutOfRangeException>(IsValidParam(weights), ErrorMessages.InvalidParams);
            _weights = weights.ToList();
            UpdateHelpers();
        }

        #endregion

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
            for (var i = 0; i < valueCount; ++i) {
                ones.Add(1.0);
            }
            return ones;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        ///   Determines whether categorical distribution is defined under given weights.
        /// </summary>
        /// <param name="weights">
        ///   The weights which are used for generation of categorical distributed random numbers.
        ///   Weights do not need to be normalized as this is often impossible using floating point arithmetic.
        /// </param>
        /// <returns>
        ///   False if any of the weights is negative or if the sum of parameters is 0.0;
        ///   otherwise, it returns true.
        /// </returns>
        public bool AreValidWeights(IEnumerable<double> weights)
        {
            return IsValidParam(weights);
        }

        /// <summary>
        ///   Computes the unnormalized cumulative distribution function
        ///   and other attributes for the distribution (like mean, variance, and so on).
        /// </summary>
        void UpdateHelpers()
        {
            var weightsSum = 0.0; // It will store the sum of all weights.
            var cdf = new double[_weights.Count]; // It will store UNNORMALIZED cdf.
            var tmpMean = 0.0;
            var maxW = 0.0; // It will store max weight (all weights are positive).
            var maxI = 0; // It will store max weight index.

            // One big loop to compute all helpers needed by this distribution.
            for (var i = 0; i < _weights.Count; ++i) {
                var w = _weights[i];
                weightsSum += w;
                cdf[i] = weightsSum;
                tmpMean += w*(i + 1); // Plus one because it is zero-based.
                if (w <= maxW) {
                    continue;
                }
                maxW = w;
                maxI = i;
            }

            // Finalize some results...
            _weightsSum = weightsSum;
            _cdf = cdf;
            Mean = (tmpMean/weightsSum) - 1; // Minus one to make it zero-based.
            Mode = new double[] {maxI};

            var halfWeightsSum = weightsSum/2;
            var tmpMedian = double.NaN;
            var tmpVar = 0.0;

            // We still need another loop to compute variance;
            // as for the mean, the plus/minus one is needed.
            for (var i = 0; i < _weights.Count; ++i) {
                if (double.IsNaN(tmpMedian) && _cdf[i] >= halfWeightsSum) {
                    tmpMedian = i;
                }
                tmpVar += _weights[i]*Math.Pow(i + 1 - Mean, 2);
            }

            // Finalize last results...
            Median = tmpMedian;
            Variance = (tmpVar/weightsSum) - 1;
        }

        #endregion

        #region IDiscreteDistribution Members

        public double Minimum
        {
            get { return 0; }
        }

        public double Maximum
        {
            get { return _weights.Count - 1; }
        }

        public double Mean { get; private set; }

        public double Median { get; private set; }

        public double Variance { get; private set; }

        public double[] Mode { get; private set; }

        public int Next()
        {
            return Sample(Gen, _weights.Count, _cdf, _weightsSum);
        }

        public double NextDouble()
        {
            return Sample(Gen, _weights.Count, _cdf, _weightsSum);
        }

        #endregion

        #region Object Members

        public override string ToString()
        {
            return "Categorical(ValueCount = " + _weights.Count + ")";
        }

        #endregion

        #region TRandom Helpers

        /// <summary>
        ///   Determines whether categorical distribution is defined under given weights.
        /// </summary>
        /// <param name="weights">
        ///   The weights which are used for generation of categorical distributed random numbers.
        ///   Weights do not need to be normalized as this is often impossible using floating point arithmetic.
        /// </param>
        /// <returns>
        ///   False if any of the weights is negative or if the sum of parameters is 0.0;
        ///   otherwise, it returns true.
        /// </returns>
        [System.Diagnostics.Contracts.Pure]
        public static bool IsValidParam(IEnumerable<double> weights)
        {
            var sum = 0.0;
            foreach (var w in weights) {
                if (w < 0.0 || Double.IsNaN(w)) {
                    return false;
                }
                sum += w;
            }
            return sum != 0.0;
        }

        /// <summary>
        ///   Returns a categorical distributed 32-bit signed integer.
        /// </summary>
        /// <param name="generator">The generator from which random number are drawn.</param>
        /// <param name="weightsCount">The number of weights from which we should sample.</param>
        /// <param name="cdf">The cumulative distribution of weights.</param>
        /// <param name="weightsSum">The sum of weights.</param>
        /// <returns>
        ///   A categorical distributed 32-bit signed integer.
        /// </returns>
        [System.Diagnostics.Contracts.Pure]
        internal static int Sample(TGen generator, int weightsCount, double[] cdf, double weightsSum)
        {
            var u = generator.NextDouble(weightsSum);
            var minIdx = 0;
            var maxIdx = weightsCount - 1;
            while (minIdx < maxIdx) {
                var idx = (maxIdx - minIdx) / 2 + minIdx;
                var c = cdf[idx];
                if (u == c) {
                    minIdx = idx;
                    break;
                } if (u < c) {
                    maxIdx = idx;
                } else {
                    minIdx = idx + 1;
                }
            }
            return minIdx;
        }

        internal static void SetUp(int weightsCount, IEnumerable<double> weights, out double[] cdf,
                                   out double weightsSum)
        {
            var weightsList = (weights == null) ? Ones(weightsCount) : weights.ToList();
            weightsSum = 0.0; // It will store the sum of all weights.
            cdf = new double[weightsCount]; // It will store UNNORMALIZED cdf.
            var maxW = 0.0; // It will store max weight (all weights are positive).

            for (var i = 0; i < weightsCount; ++i) {
                var w = weightsList[i];
                weightsSum += w;
                cdf[i] = weightsSum;
                if (w <= maxW) {
                    continue;
                }
                maxW = w;
            }
        }

        #endregion
    }

    /// <summary>
    ///   Implements the categorical distribution. For details about this distribution, see 
    ///   <a href="http://en.wikipedia.org/wiki/Categorical_distribution">Wikipedia - Categorical distribution</a>.
    ///   This distribution is sometimes called the Discrete distribution.
    /// </summary>
    /// <remarks>
    ///   The distribution is parameterized by a vector of ratios: in other words, the parameter
    ///   does not have to be normalized and sum to 1. The reason is that some vectors can't be exactly normalized
    ///   to sum to 1 in floating point representation.
    /// </remarks>
    [PublicAPI]
    public sealed class CategoricalDistribution : CategoricalDistribution<IGenerator>
    {
        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="BinomialDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        public CategoricalDistribution() : base(new XorShift128Generator(), DefaultValueCount)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Weights.Count, DefaultValueCount));
            Debug.Assert(Weights.All(w => w == 1.0/DefaultValueCount));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="BinomialDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        [CLSCompliant(false)]
        public CategoricalDistribution(uint seed) : base(new XorShift128Generator(seed), DefaultValueCount)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Weights.Count, DefaultValueCount));
            Debug.Assert(Weights.All(w => w == 1.0/DefaultValueCount));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="CategoricalDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        public CategoricalDistribution(IGenerator generator) : base(generator, DefaultValueCount)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Weights.Count, DefaultValueCount));
            Debug.Assert(Weights.All(w => w == 1.0/DefaultValueCount));
        }
        
        /// <summary>
        ///   Initializes a new instance of the <see cref="BinomialDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="valueCount">
        ///   The parameter valueCount which is used for generation of binomial distributed random numbers
        ///   by setting the number of equi-distributed "weights" the distribution will have.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="valueCount"/> is less than or equal to zero.
        /// </exception>
        public CategoricalDistribution(int valueCount) : base(new XorShift128Generator(), valueCount)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Weights.Count, valueCount));
            Debug.Assert(Weights.All(w => w == 1.0/valueCount));
        }
        
        /// <summary>
        ///   Initializes a new instance of the <see cref="BinomialDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        /// <param name="valueCount">
        ///   The parameter valueCount which is used for generation of binomial distributed random numbers
        ///   by setting the number of equi-distributed "weights" the distribution will have.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="valueCount"/> is less than or equal to zero.
        /// </exception>
        [CLSCompliant(false)]
        public CategoricalDistribution(uint seed, int valueCount) : base(new XorShift128Generator(seed), valueCount)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Weights.Count, valueCount));
            Debug.Assert(Weights.All(w => w == 1.0/valueCount));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="CategoricalDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="valueCount">
        ///   The parameter valueCount which is used for generation of binomial distributed random numbers
        ///   by setting the number of equi-distributed "weights" the distribution will have.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="valueCount"/> is less than or equal to zero.
        /// </exception>
        public CategoricalDistribution(IGenerator generator, int valueCount) : base(generator, valueCount)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Weights.Count, valueCount));
            Debug.Assert(Weights.All(w => w == 1.0/valueCount));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="BinomialDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="weights">
        ///   An enumerable of nonnegative weights: this enumerable does not need to be normalized 
        ///   as this is often impossible using floating point arithmetic.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="weights"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="weights"/> is empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   Any of the weights in <paramref name="weights"/> are negative or they sum to zero.
        /// </exception>
        public CategoricalDistribution(ICollection<double> weights) : base(new XorShift128Generator(), weights)
        {
            Debug.Assert(Generator is XorShift128Generator);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="BinomialDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        /// <param name="weights">
        ///   An enumerable of nonnegative weights: this enumerable does not need to be normalized 
        ///   as this is often impossible using floating point arithmetic.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="weights"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="weights"/> is empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   Any of the weights in <paramref name="weights"/> are negative or they sum to zero.
        /// </exception>
        [CLSCompliant(false)]
        public CategoricalDistribution(uint seed, ICollection<double> weights)
            : base(new XorShift128Generator(seed), weights)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="CategoricalDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
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
        public CategoricalDistribution(IGenerator generator, ICollection<double> weights) : base(generator, weights)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
        }

        #endregion
    }
}