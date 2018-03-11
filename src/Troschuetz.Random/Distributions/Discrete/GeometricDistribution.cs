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

#region Original Copyrights

// -*- C++ -*-
/*****************************************************************************
 *
 *   |_|_|_  |_|_    |_    |_|_|_  |_		     C O M M U N I C A T I O N
 * |_        |_  |_  |_  |_        |_		               N E T W O R K S
 * |_        |_  |_  |_  |_        |_		                     C L A S S
 *   |_|_|_  |_    |_|_    |_|_|_  |_|_|_|_	                 L I B R A R Y
 *
 * $Id: Geometric.c,v 1.2 2002/01/14 11:37:33 spee Exp $
 *
 * CNClass: CNGeometric --- CNGeometric distributed random numbers
 *
 *****************************************************************************
 * Copyright (C) 1992-1996   Communication Networks
 *                           Aachen University of Technology
 *                           D-52056 Aachen
 *                           Germany
 *                           Email: cncl-adm@comnets.rwth-aachen.de
 *****************************************************************************
 * This file is part of the CN class library. All files marked with
 * this header are free software; you can redistribute it and/or modify
 * it under the terms of the GNU Library General Public License as
 * published by the Free Software Foundation; either version 2 of the
 * License, or (at your option) any later version.  This library is
 * distributed in the hope that it will be useful, but WITHOUT ANY
 * WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE.  See the GNU Library General Public
 * License for more details.  You should have received a copy of the GNU
 * Library General Public License along with this library; if not, write
 * to the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139,
 * USA.
 *****************************************************************************
 * original Copyright:
 * -------------------
 * Copyright (C) 1988 Free Software Foundation
 *    written by Dirk Grunwald (grunwald@cs.uiuc.edu)
 *
 * This file is part of the GNU C++ Library.  This library is free
 * software; you can redistribute it and/or modify it under the terms of
 * the GNU Library General Public License as published by the Free
 * Software Foundation; either version 2 of the License, or (at your
 * option) any later version.  This library is distributed in the hope
 * that it will be useful, but WITHOUT ANY WARRANTY; without even the
 * implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
 * PURPOSE.  See the GNU Library General Public License for more details.
 * You should have received a copy of the GNU Library General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
 *****************************************************************************/

#endregion Original Copyrights

namespace Troschuetz.Random.Distributions.Discrete
{
    using Core;
    using Generators;
    using System;
    using System.Diagnostics;

    /// <summary>
    ///   Provides generation of geometric distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The geometric distribution generates only discrete numbers. <br/> The implementation of the
    ///   <see cref="GeometricDistribution"/> type bases upon information presented on
    ///   <a href="http://en.wikipedia.org/wiki/Geometric_distribution">Wikipedia - Geometric
    ///   distribution</a> and the implementation in the
    ///   <a href="http://www.lkn.ei.tum.de/lehre/scn/cncl/doc/html/cncl_toc.html">Communication
    ///   Networks Class Library</a>.
    ///
    ///   The thread safety of this class depends on the one of the underlying generator.
    /// </remarks>
    [Serializable]
    public sealed class GeometricDistribution : AbstractDistribution, IDiscreteDistribution, IAlphaDistribution<double>
    {
        #region Constants

        /// <summary>
        ///   The default value assigned to <see cref="Alpha"/> if none is specified.
        /// </summary>
        public const double DefaultAlpha = 0.5;

        #endregion Constants

        #region Fields

        /// <summary>
        ///   Stores the parameter beta which is used for generation of uniformly distributed random numbers.
        /// </summary>
        private double _alpha;

        /// <summary>
        ///   Gets or sets the parameter alpha which is used for generation of geometric distributed
        ///   random numbers.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value"/> is less than or equal to zero or it is greater than one.
        /// </exception>
        /// <remarks>
        ///   Calls <see cref="IsValidParam"/> to determine whether a value is valid and therefore assignable.
        /// </remarks>
        public double Alpha
        {
            get { return _alpha; }
            set
            {
                if (!IsValidAlpha(value)) throw new ArgumentOutOfRangeException(nameof(Alpha), ErrorMessages.InvalidParams);
                _alpha = value;
            }
        }

        #endregion Fields

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="GeometricDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        public GeometricDistribution() : this(new XorShift128Generator(), DefaultAlpha)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="GeometricDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        public GeometricDistribution(uint seed) : this(new XorShift128Generator(seed), DefaultAlpha)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="GeometricDistribution"/> class, using the
        ///   specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        public GeometricDistribution(IGenerator generator) : this(generator, DefaultAlpha)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Alpha, DefaultAlpha));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="GeometricDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of geometric distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero or it is greater than one.
        /// </exception>
        public GeometricDistribution(double alpha) : this(new XorShift128Generator(), alpha)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, alpha));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="GeometricDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of geometric distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero or it is greater than one.
        /// </exception>
        public GeometricDistribution(uint seed, double alpha) : this(new XorShift128Generator(seed), alpha)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, alpha));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="GeometricDistribution"/> class, using the
        ///   specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of geometric distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero or it is greater than one.
        /// </exception>
        public GeometricDistribution(IGenerator generator, double alpha) : base(generator)
        {
            var vp = IsValidParam;
            if (!vp(alpha)) throw new ArgumentOutOfRangeException(nameof(alpha), ErrorMessages.InvalidParams);
            _alpha = alpha;
        }

        #endregion Construction

        #region Instance Methods

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Alpha"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>
        ///   <see langword="true"/> if value is greater than 0.0, and less than or equal to 1.0;
        ///   otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsValidAlpha(double value) => IsValidParam(value);

        #endregion Instance Methods

        #region IDiscreteDistribution Members

        /// <summary>
        ///   Gets the minimum possible value of distributed random numbers.
        /// </summary>
        public double Minimum => 1.0;

        /// <summary>
        ///   Gets the maximum possible value of distributed random numbers.
        /// </summary>
        public double Maximum => double.PositiveInfinity;

        /// <summary>
        ///   Gets the mean of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if mean is not defined for given distribution with some parameters.
        /// </exception>
        public double Mean => 1.0 / Alpha;

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
        public double Variance => (1.0 - Alpha) / TMath.Square(Alpha);

        /// <summary>
        ///   Gets the mode of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if mode is not defined for given distribution with some parameters.
        /// </exception>
        public double[] Mode => new[] { 1.0 };

        /// <summary>
        ///   Returns a distributed random number.
        /// </summary>
        /// <returns>A distributed 32-bit signed integer.</returns>
        public int Next() => Sample(Generator, _alpha);

        /// <summary>
        ///   Returns a distributed floating point random number.
        /// </summary>
        /// <returns>A distributed double-precision floating point number.</returns>
        public double NextDouble() => Sample(Generator, _alpha);

        #endregion IDiscreteDistribution Members

        #region TRandom Helpers

        /// <summary>
        ///   Determines whether geometric distribution is defined under given parameter. The default
        ///   definition returns true if alpha is greater than zero and if it is less than or equal
        ///   to one; otherwise, it returns false.
        /// </summary>
        /// <remarks>
        ///   This is an extensibility point for the <see cref="GeometricDistribution"/> class.
        /// </remarks>
        public static Func<double, bool> IsValidParam { get; set; } = alpha =>
        {
            return alpha > 0.0 && alpha <= 1.0;
        };

        /// <summary>
        ///   Declares a function returning a geometric distributed 32-bit signed integer.
        /// </summary>
        /// <remarks>
        ///   This is an extensibility point for the <see cref="GeometricDistribution"/> class.
        /// </remarks>
        public static Func<IGenerator, double, int> Sample { get; set; } = (generator, alpha) =>
        {
            var samples = 1;
            for (; generator.NextDouble() >= alpha; samples++)
            {
                // Empty
            }
            return samples;
        };

        #endregion TRandom Helpers
    }
}