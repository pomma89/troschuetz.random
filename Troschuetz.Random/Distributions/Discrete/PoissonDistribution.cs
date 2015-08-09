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

// -*- C++ -*-
/*****************************************************************************
 *
 *   |_|_|_  |_|_    |_    |_|_|_  |_		     C O M M U N I C A T I O N
 * |_        |_  |_  |_  |_        |_		               N E T W O R K S
 * |_        |_  |_  |_  |_        |_		                     C L A S S
 *   |_|_|_  |_    |_|_    |_|_|_  |_|_|_|_	                 L I B R A R Y
 *
 * $Id: Poisson.c,v 1.2 2002/01/14 11:37:33 spee Exp $
 *
 * CNClass: CNPoisson --- CNPoisson distributed random numbers
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
    using PommaLabs.Thrower;
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    /// <summary>
    ///   Provides generation of poisson distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The poisson distribution generates only discrete numbers. <br/> The implementation of the
    ///   <see cref="PoissonDistribution"/> type bases upon information presented on
    ///   <a href="http://en.wikipedia.org/wiki/Poisson_distribution">Wikipedia - Poisson
    ///   distribution</a> and the implementation in the
    ///   <a href="http://www.lkn.ei.tum.de/lehre/scn/cncl/doc/html/cncl_toc.html">Communication
    ///   Networks Class Library</a>.
    /// </remarks>
    [Serializable]
    public class PoissonDistribution<TGen> : Distribution<TGen>, IDiscreteDistribution, ILambdaDistribution<double>
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
        ///   Stores the the parameter lambda which is used for generation of poisson distributed
        ///   random numbers.
        /// </summary>
        double _lambda;

        /// <summary>
        ///   Gets or sets the parameter lambda which is used for generation of poisson distributed
        ///   random numbers.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   Calls <see cref="IsValidLambda"/> to determine whether a value is valid and therefore assignable.
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
        ///   Initializes a new instance of the <see cref="PoissonDistribution"/> class, using the
        ///   specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="lambda">
        ///   The parameter lambda which is used for generation of poisson distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="lambda"/> is less than or equal to zero.
        /// </exception>
        public PoissonDistribution(TGen generator, double lambda) : base(generator)
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
        public bool IsValidLambda(double value)
        {
            return IsValidParam(value);
        }

        #endregion Instance Methods

        #region IDiscreteDistribution Members

        public double Minimum
        {
            get { return 0.0; }
        }

        public double Maximum
        {
            get { return double.PositiveInfinity; }
        }

        public double Mean
        {
            get { return _lambda; }
        }

        public double Median
        {
            get { throw new NotSupportedException(ErrorMessages.UndefinedMedian); }
        }

        public double Variance
        {
            get { return _lambda; }
        }

        public double[] Mode
        {
            get
            {
                // Checks if the value of lambda is a whole number.
                return _lambda == Math.Floor(_lambda) ? new[] { _lambda - 1.0, _lambda } : new[] { Math.Floor(_lambda) };
            }
        }

        public int Next()
        {
            return Sample(Gen, _lambda);
        }

        public double NextDouble()
        {
            return Sample(Gen, _lambda);
        }

        #endregion IDiscreteDistribution Members

        #region TRandom Helpers

        /// <summary>
        ///   Determines whether poisson distribution is defined under given parameter.
        /// </summary>
        /// <param name="lambda">
        ///   The parameter lambda which is used for generation of poisson distributed random numbers.
        /// </param>
        /// <returns>
        ///   True if <paramref name="lambda"/> is greater than zero; otherwise, it returns false.
        /// </returns>
        [Pure]
        public static bool IsValidParam(double lambda) => lambda > 0;

        /// <summary>
        ///   Returns a poisson distributed 32-bit signed integer.
        /// </summary>
        /// <param name="generator">The generator from which random number are drawn.</param>
        /// <param name="lambda">
        ///   The parameter lambda which is used for generation of poisson distributed random numbers.
        /// </param>
        /// <returns>A poisson distributed 32-bit signed integer.</returns>
        [Pure]
        internal static int Sample(TGen generator, double lambda)
        {
            // See contribution of JcBernack on BitBucket (issue #2) and see Wikipedia page about
            // Poisson distribution (https://en.wikipedia.org/wiki/Poisson_distribution#Generating_Poisson-distributed_random_variables).
            const int step = 500;
            var k = 0;
            var p = 1.0;
            double r;
            do
            {
                k++;
                while ((r = generator.NextDouble()).Equals(0.0))
                {
                    // Cycle until the random number is not zero. According to the Wikipedia page,
                    // we should multiply p by a random number that must be greater than zero and
                    // less than 1. NextDouble guarantees only the second clause, not the first one.
                }
                p *= r;
                if (p < Math.E && lambda > 0)
                {
                    p *= Math.Exp(lambda > step ? step : lambda);
                    lambda -= step;
                }
            } while (p > 1);
            return k - 1;
        }

        #endregion TRandom Helpers
    }

    /// <summary>
    ///   Provides generation of poisson distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The poisson distribution generates only discrete numbers. <br/> The implementation of the
    ///   <see cref="PoissonDistribution"/> type bases upon information presented on
    ///   <a href="http://en.wikipedia.org/wiki/Poisson_distribution">Wikipedia - Poisson
    ///   distribution</a> and the implementation in the
    ///   <a href="http://www.lkn.ei.tum.de/lehre/scn/cncl/doc/html/cncl_toc.html">Communication
    ///   Networks Class Library</a>.
    /// </remarks>
    [Serializable]
    public sealed class PoissonDistribution : PoissonDistribution<IGenerator>
    {
        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="PoissonDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        public PoissonDistribution() : base(new XorShift128Generator(), DefaultLambda)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Lambda, DefaultLambda));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="PoissonDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        [CLSCompliant(false)]
        public PoissonDistribution(uint seed) : base(new XorShift128Generator(seed), DefaultLambda)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Lambda, DefaultLambda));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="PoissonDistribution"/> class, using the
        ///   specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        public PoissonDistribution(IGenerator generator) : base(generator, DefaultLambda)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Lambda, DefaultLambda));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="PoissonDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="lambda">
        ///   The parameter lambda which is used for generation of poisson distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="lambda"/> is less than or equal to zero.
        /// </exception>
        public PoissonDistribution(double lambda) : base(new XorShift128Generator(), lambda)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Lambda, lambda));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="PoissonDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        /// <param name="lambda">
        ///   The parameter lambda which is used for generation of poisson distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="lambda"/> is less than or equal to zero.
        /// </exception>
        [CLSCompliant(false)]
        public PoissonDistribution(uint seed, double lambda) : base(new XorShift128Generator(seed), lambda)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Lambda, lambda));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="PoissonDistribution"/> class, using the
        ///   specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="lambda">
        ///   The parameter lambda which is used for generation of poisson distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="lambda"/> is less than or equal to zero.
        /// </exception>
        public PoissonDistribution(IGenerator generator, double lambda) : base(generator, lambda)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Lambda, lambda));
        }

        #endregion Construction
    }
}
