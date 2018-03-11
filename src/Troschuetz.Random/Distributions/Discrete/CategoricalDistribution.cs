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
    ///   The distribution is parameterized by a vector of ratios: in other words, the parameter does
    ///   not have to be normalized and sum to 1. The reason is that some vectors can't be exactly
    ///   normalized to sum to 1 in floating point representation.
    ///
    ///   The thread safety of this class depends on the one of the underlying generator.
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
        ///   Stores the cumulative distribution of current normalized weights.
        /// </summary>
        private double[] _cdf;

        /// <summary>
        ///   Stores the unnormalized categorical weights.
        /// </summary>
        private List<double> _weights;

        /// <summary>
        ///   Gets or sets the normalized probability vector of the categorical distribution.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> is empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   Any of the weights in <paramref name="value"/> are negative or they sum to zero.
        /// </exception>
        /// <remarks>
        ///   Sometimes the normalized probability vector cannot be represented exactly in a floating
        ///   point representation.
        /// </remarks>
        public ICollection<double> Weights
        {
            get { return _weights.ToList(); }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(Weights), ErrorMessages.NullWeights);
                if (value.Count == 0) throw new ArgumentException(ErrorMessages.EmptyList, nameof(Weights));
                if (!AreValidWeights(value)) throw new ArgumentOutOfRangeException(nameof(Weights), ErrorMessages.InvalidParams);
                _weights = value.ToList();
                UpdateHelpers();
            }
        }

        #endregion Fields

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="BinomialDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        public CategoricalDistribution() : this(new XorShift128Generator(), DefaultValueCount)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Weights.Count, DefaultValueCount));
            Debug.Assert(Weights.All(w => TMath.AreEqual(w, 1.0 / DefaultValueCount)));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="BinomialDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        public CategoricalDistribution(uint seed) : this(new XorShift128Generator(seed), DefaultValueCount)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Weights.Count, DefaultValueCount));
            Debug.Assert(Weights.All(w => TMath.AreEqual(w, 1.0 / DefaultValueCount)));
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
            Debug.Assert(Weights.All(w => TMath.AreEqual(w, 1.0 / DefaultValueCount)));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="BinomialDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="valueCount">
        ///   The parameter valueCount which is used for generation of binomial distributed random
        ///   numbers by setting the number of equi-distributed "weights" the distribution will have.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="valueCount"/> is less than or equal to zero.
        /// </exception>
        public CategoricalDistribution(int valueCount) : this(new XorShift128Generator(), valueCount)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Weights.Count, valueCount));
            Debug.Assert(Weights.All(w => TMath.AreEqual(w, 1.0 / valueCount)));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="BinomialDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> with the specified seed value.
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
        public CategoricalDistribution(uint seed, int valueCount) : this(new XorShift128Generator(seed), valueCount)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Weights.Count, valueCount));
            Debug.Assert(Weights.All(w => TMath.AreEqual(w, 1.0 / valueCount)));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="BinomialDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="weights">
        ///   An enumerable of nonnegative weights: this enumerable does not need to be normalized as
        ///   this is often impossible using floating point arithmetic.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="weights"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="weights"/> is empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   Any of the weights in <paramref name="weights"/> are negative or they sum to zero.
        /// </exception>
        public CategoricalDistribution(ICollection<double> weights) : this(new XorShift128Generator(), weights)
        {
            Debug.Assert(Generator is XorShift128Generator);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="BinomialDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        /// <param name="weights">
        ///   An enumerable of nonnegative weights: this enumerable does not need to be normalized as
        ///   this is often impossible using floating point arithmetic.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="weights"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="weights"/> is empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   Any of the weights in <paramref name="weights"/> are negative or they sum to zero.
        /// </exception>
        public CategoricalDistribution(uint seed, ICollection<double> weights)
            : this(new XorShift128Generator(seed), weights)
        {
            Debug.Assert(Generator is XorShift128Generator);
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
            if (valueCount == 0) throw new ArgumentOutOfRangeException(nameof(valueCount), ErrorMessages.InvalidParams);
            _weights = Ones(valueCount);
            UpdateHelpers();
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="CategoricalDistribution"/> class, using
        ///   the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="weights">
        ///   An enumerable of nonnegative weights: this enumerable does not need to be normalized as
        ///   this is often impossible using floating point arithmetic.
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
            if (weights == null) throw new ArgumentNullException(nameof(weights), ErrorMessages.NullWeights);
            if (weights.Count == 0) throw new ArgumentException(ErrorMessages.EmptyList, nameof(weights));
            if (!AreValidWeights(weights)) throw new ArgumentOutOfRangeException(nameof(weights), ErrorMessages.InvalidParams);
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
        private static List<double> Ones(int valueCount)
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
        ///   Computes the unnormalized cumulative distribution function and other attributes for the
        ///   distribution (like mean, variance, and so on).
        /// </summary>
        /// <remarks>
        ///   Also remember to change <see cref="SetUp(int, IEnumerable{double}, out double[])"/>
        ///   when changing this method.
        /// </remarks>
        private void UpdateHelpers()
        {
            var weightsSum = _weights.Sum(); // It will store the sum of all UNNORMALIZED weights.
            var cdf = new double[_weights.Count]; // It will store NORMALIZED cdf.
            var tmpMean = 0.0;
            var maxW = 0.0; // It will store max weight (all weights are positive).
            var maxI = 0; // It will store max weight index.

            // Let's normalize all weights, if necessary.
            if (!TMath.AreEqual(weightsSum, 1.0))
            {
                for (var i = 0; i < _weights.Count; ++i)
                {
                    _weights[i] /= weightsSum;
                }
                Debug.Assert(TMath.AreEqual(_weights.Sum(), 1.0));
            }

            weightsSum = 0.0; // Reset weight sum to use it for cdf.

            // One big loop to compute all helpers needed by this distribution.
            for (var i = 0; i < _weights.Count; ++i)
            {
                var w = _weights[i];
                weightsSum += w;
                cdf[i] = weightsSum;
                tmpMean += w * (i + 1.0); // Plus one because it is zero-based.
                if (w > maxW)
                {
                    maxW = w;
                    maxI = i;
                }
            }

            // Finalize some results...
            _cdf = cdf;
            Mean = tmpMean - 1.0; // Minus one to make it zero-based.
            Mode = new double[] { maxI };

            var halfWeightsSum = weightsSum / 2.0;
            var tmpMedian = double.NaN;
            var tmpVar = 0.0;

            // We still need another loop to compute variance; as for the mean, the plus/minus one is needed.
            for (var i = 0; i < _weights.Count; ++i)
            {
                if (double.IsNaN(tmpMedian) && _cdf[i] >= halfWeightsSum)
                {
                    tmpMedian = i;
                }
                tmpVar += _weights[i] * TMath.Square(i + 1 - Mean);
            }

            // Finalize last results...
            Median = tmpMedian;
            Variance = tmpVar - 1.0;
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
        public int Next() => Sample(Generator, _cdf);

        /// <summary>
        ///   Returns a distributed floating point random number.
        /// </summary>
        /// <returns>A distributed double-precision floating point number.</returns>
        public double NextDouble() => Sample(Generator, _cdf);

        #endregion IDiscreteDistribution Members

        #region Object Members

        /// <summary>
        ///   Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() => "Categorical(ValueCount = " + _weights.Count + ")";

        #endregion Object Members

        #region TRandom Helpers

        /// <summary>
        ///   Determines whether categorical distribution is defined under given weights. The default
        ///   definition returns false if any of the weights is negative or if the sum of parameters
        ///   is 0.0; otherwise, it returns true.
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
            return !TMath.IsZero(sum);
        };

        /// <summary>
        ///   Declares a function returning a categorical distributed 32-bit signed integer.
        /// </summary>
        /// <remarks>
        ///   This is an extensibility point for the <see cref="CategoricalDistribution"/> class.
        /// </remarks>
        public static Func<IGenerator, double[], int> Sample { get; set; } = (generator, cdf) =>
        {
            var u = generator.NextDouble();
            var minIdx = 0;
            var maxIdx = cdf.Length - 1;
            while (minIdx < maxIdx)
            {
                var idx = (maxIdx - minIdx) / 2 + minIdx;
                var c = cdf[idx];
                if (TMath.AreEqual(u, c))
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

        /// <summary>
        ///   Prepares parameters for <see cref="TRandom"/>.
        /// </summary>
        /// <param name="weightsCount">The number of weights.</param>
        /// <param name="weights">Weights, or null.</param>
        /// <param name="cdf">The output cdf.</param>
        /// <remarks>
        ///   Also remember to change <see cref="UpdateHelpers"/> when changing this method.
        /// </remarks>
        internal static void SetUp(int weightsCount, IEnumerable<double> weights, out double[] cdf)
        {
            var weightsList = (weights == null) ? Ones(weightsCount) : weights.ToList();
            var weightsSum = weightsList.Sum(); // It will store the sum of all UNNORMALIZED weights.
            cdf = new double[weightsList.Count]; // It will store NORMALIZED cdf.
            var maxW = 0.0; // It will store max weight (all weights are positive).

            // Let's normalize all weights, if necessary.
            if (!TMath.AreEqual(weightsSum, 1.0))
            {
                for (var i = 0; i < weightsList.Count; ++i)
                {
                    weightsList[i] /= weightsSum;
                }
                Debug.Assert(TMath.AreEqual(weightsList.Sum(), 1.0));
            }

            weightsSum = 0.0; // Reset weight sum to use it for cdf.

            // One big loop to compute all helpers needed by this distribution.
            for (var i = 0; i < weightsList.Count; ++i)
            {
                var w = weightsList[i];
                weightsSum += w;
                cdf[i] = weightsSum;
                if (w > maxW)
                {
                    maxW = w;
                }
            }
        }

        #endregion TRandom Helpers
    }
}