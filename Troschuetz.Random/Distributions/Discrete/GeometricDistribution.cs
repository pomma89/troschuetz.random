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

#region Original Copyrights

//   -*- C++ -*-
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

#endregion

namespace Troschuetz.Random.Distributions.Discrete
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using Generators;
    using JetBrains.Annotations;

    /// <summary>
    ///   Provides generation of geometric distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The geometric distribution generates only discrete numbers.<br />
    ///   The implementation of the <see cref="GeometricDistribution"/> type bases upon information presented on
    ///   <a href="http://en.wikipedia.org/wiki/Geometric_distribution">Wikipedia - Geometric distribution</a>
    ///   and the implementation in the <a href="http://www.lkn.ei.tum.de/lehre/scn/cncl/doc/html/cncl_toc.html">
    ///   Communication Networks Class Library</a>.
    /// </remarks>
    [PublicAPI]
    public class GeometricDistribution<TGen> : Distribution<TGen>, IDiscreteDistribution, IAlphaDistribution<double>
        where TGen : IGenerator
    {
        #region Class Fields

        /// <summary>
        ///   The default value assigned to <see cref="Alpha"/> if none is specified. 
        /// </summary>
        public const double DefaultAlpha = 0.5;

        #endregion

        #region Instance Fields
        
        /// <summary>
        ///   Stores the parameter beta which is used for generation of uniformly distributed random numbers.
        /// </summary>
        double _alpha;

        /// <summary>
        ///   Gets or sets the parameter alpha which is used for generation of geometric distributed random numbers.
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
            set { _alpha = value; }
        }

        #endregion

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="GeometricDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of geometric distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero or it is greater than one.
        /// </exception>
        public GeometricDistribution(TGen generator, double alpha) : base(generator)
        {
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(generator, null), ErrorMessages.NullGenerator);
            Contract.Requires<ArgumentOutOfRangeException>(IsValidParam(alpha), ErrorMessages.InvalidParams);
            _alpha = alpha;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Alpha"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>
        ///   <see langword="true"/> if value is greater than 0.0, and less than or equal to 1.0;
        ///   otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsValidAlpha(double value)
        {
            return IsValidParam(value);
        }

        #endregion

        #region IDiscreteDistribution Members

        public double Minimum
        {
            get { return 1.0; }
        }

        public double Maximum
        {
            get { return double.PositiveInfinity; }
        }

        public double Mean
        {
            get { return 1.0/Alpha; }
        }

        public double Median
        {
            get { throw new NotSupportedException(ErrorMessages.UndefinedMedian); }
        }

        public double Variance
        {
            get { return (1.0 - Alpha)/Math.Pow(Alpha, 2.0); }
        }

        public double[] Mode
        {
            get { return new[] {1.0}; }
        }

        public int Next()
        {
            return Sample(Gen, _alpha);
        }

        public double NextDouble()
        {
            return Sample(Gen, _alpha);
        }

        #endregion

        #region TRandom Helpers

        /// <summary>
        ///   Determines whether geometric distribution is defined under given parameter.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of geometric distributed random numbers.
        /// </param>
        /// <returns>
        ///   True if <paramref name="alpha"/> is greater than zero and
        ///   if it is less than or equal to one; otherwise, it returns false.
        /// </returns>
        [System.Diagnostics.Contracts.Pure]
        public static bool IsValidParam(double alpha)
        {
            return alpha > 0 && alpha <= 1;
        }

        /// <summary>
        ///   Returns a geometric distributed 32-bit signed integer.
        /// </summary>
        /// <param name="generator">The generator from which random number are drawn.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of geometric distributed random numbers.
        /// </param>
        /// <returns>
        ///   A geometric distributed 32-bit signed integer.
        /// </returns>
        [System.Diagnostics.Contracts.Pure]
        internal static int Sample(TGen generator, double alpha)
        {
            var samples = 1;
            for (; generator.NextDouble() >= alpha; samples++) {
                // Empty
            }
            return samples;
        }

        #endregion
    }

    /// <summary>
    ///   Provides generation of geometric distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The geometric distribution generates only discrete numbers.<br />
    ///   The implementation of the <see cref="GeometricDistribution"/> type bases upon information presented on
    ///   <a href="http://en.wikipedia.org/wiki/Geometric_distribution">Wikipedia - Geometric distribution</a>
    ///   and the implementation in the <a href="http://www.lkn.ei.tum.de/lehre/scn/cncl/doc/html/cncl_toc.html">
    ///   Communication Networks Class Library</a>.
    /// </remarks>
    [PublicAPI]
    public sealed class GeometricDistribution : GeometricDistribution<IGenerator>
    {
        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="GeometricDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        public GeometricDistribution() : base(new XorShift128Generator(), DefaultAlpha)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="GeometricDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        [CLSCompliant(false)]
        public GeometricDistribution(uint seed) : base(new XorShift128Generator(seed), DefaultAlpha)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="GeometricDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        public GeometricDistribution(IGenerator generator) : base(generator, DefaultAlpha)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Alpha, DefaultAlpha));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="GeometricDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of geometric distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero or it is greater than one.
        /// </exception>
        public GeometricDistribution(double alpha) : base(new XorShift128Generator(), alpha)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, alpha));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="GeometricDistribution"/> class,
        ///   using a <see cref="XorShift128Generator"/> with the specified seed value.
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
        [CLSCompliant(false)]
        public GeometricDistribution(uint seed, double alpha) : base(new XorShift128Generator(seed), alpha)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, alpha));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="GeometricDistribution"/> class,
        ///   using the specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of geometric distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> is less than or equal to zero or it is greater than one.
        /// </exception>
        public GeometricDistribution(IGenerator generator, double alpha) : base(generator, alpha)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Alpha, alpha));
        }

        #endregion
    }
}