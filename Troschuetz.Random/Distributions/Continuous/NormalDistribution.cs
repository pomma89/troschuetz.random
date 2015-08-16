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

#region Original Copyright

// -*- C++ -*-
/*****************************************************************************
 *
 *   |_|_|_  |_|_    |_    |_|_|_  |_		     C O M M U N I C A T I O N
 * |_        |_  |_  |_  |_        |_		               N E T W O R K S
 * |_        |_  |_  |_  |_        |_		                     C L A S S
 *   |_|_|_  |_    |_|_    |_|_|_  |_|_|_|_	                 L I B R A R Y
 *
 * $Id: Normal.c,v 1.2 2002/01/14 11:37:33 spee Exp $
 *
 * CNClass: CNNormal --- CNNormal (Gaussian) distributed random numbers
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

#endregion Original Copyright

namespace Troschuetz.Random.Distributions.Continuous
{
    using Core;
    using Generators;
    using PommaLabs.Thrower;
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    /// <summary>
    ///   Provides generation of normal distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="NormalDistribution"/> type bases upon information
    ///   presented on <a href="http://en.wikipedia.org/wiki/Normal_distribution">Wikipedia - Normal
    ///   distribution</a> and the implementation in the
    ///   <a href="http://www.lkn.ei.tum.de/lehre/scn/cncl/doc/html/cncl_toc.html">Communication
    ///   Networks Class Library</a>.
    /// </remarks>
    [Serializable]
    public class NormalDistribution<TGen> : AbstractDistribution<TGen>, IContinuousDistribution, IMuDistribution<double>, ISigmaDistribution<double>
        where TGen : IGenerator
    {
        #region Constants

        /// <summary>
        ///   The default value assigned to <see cref="Mu"/> if none is specified.
        /// </summary>
        public const double DefaultMu = 1;

        /// <summary>
        ///   The default value assigned to <see cref="Sigma"/> if none is specified.
        /// </summary>
        public const double DefaultSigma = 1;

        #endregion Constants

        #region Fields

        /// <summary>
        ///   Stores the parameter mu which is used for generation of normal distributed random numbers.
        /// </summary>
        double _mu;

        /// <summary>
        ///   Stores the parameter sigma which is used for generation of normal distributed random numbers.
        /// </summary>
        double _sigma;

        /// <summary>
        ///   Gets or sets the parameter mu which is used for generation of normal distributed
        ///   random numbers.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value"/> is equal to <see cref="double.NaN"/>.
        /// </exception>
        /// <remarks>
        ///   Calls <see cref="AreValidParams"/> to determine whether a value is valid and therefore assignable.
        /// </remarks>
        public double Mu
        {
            get { return _mu; }
            set
            {
                Raise<ArgumentOutOfRangeException>.IfNot(IsValidMu(value), ErrorMessages.InvalidParams);
                _mu = value;
            }
        }

        /// <summary>
        ///   Gets or sets the parameter sigma which is used for generation of normal distributed
        ///   random numbers.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   Calls <see cref="AreValidParams"/> to determine whether a value is valid and therefore assignable.
        /// </remarks>
        public double Sigma
        {
            get { return _sigma; }
            set
            {
                Raise<ArgumentOutOfRangeException>.IfNot(IsValidSigma(value), ErrorMessages.InvalidParams);
                _sigma = value;
            }
        }

        #endregion Fields

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="NormalDistribution{T}"/> class, using the
        ///   specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="mu">
        ///   The parameter mu which is used for generation of normal distributed random numbers.
        /// </param>
        /// <param name="sigma">
        ///   The parameter sigma which is used for generation of normal distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <see cref="sigma"/> is less than or equal to zero.
        /// </exception>
        public NormalDistribution(TGen generator, double mu, double sigma) : base(generator)
        {
            Raise<ArgumentOutOfRangeException>.IfNot(AreValidParams(mu, sigma), ErrorMessages.InvalidParams);
            _mu = mu;
            _sigma = sigma;
        }

        #endregion Construction

        #region Instance Methods

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Mu"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns><see langword="true"/>.</returns>
        public bool IsValidMu(double value)
        {
            return AreValidParams(value, _sigma);
        }

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Sigma"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns><see langword="true"/> if value is greater than 0.0; otherwise, <see langword="false"/>.</returns>
        public bool IsValidSigma(double value)
        {
            return AreValidParams(_mu, value);
        }

        #endregion Instance Methods

        #region IContinuousDistribution Members

        public double Minimum
        {
            get { return double.NegativeInfinity; }
        }

        public double Maximum
        {
            get { return double.PositiveInfinity; }
        }

        public double Mean
        {
            get { return _mu; }
        }

        public double Median
        {
            get { return _mu; }
        }

        public double Variance
        {
            get { return Math.Pow(_sigma, 2.0); }
        }

        public double[] Mode
        {
            get { return new[] { _mu }; }
        }

        public double NextDouble()
        {
            return Sample(TypedGenerator, _mu, _sigma);
        }

        #endregion IContinuousDistribution Members

        #region TRandom Helpers

        /// <summary>
        ///   Determines whether normal distribution is defined under given parameters.
        /// </summary>
        /// <param name="mu">
        ///   The parameter mu which is used for generation of normal distributed random numbers.
        /// </param>
        /// <param name="sigma">
        ///   The parameter sigma which is used for generation of normal distributed random numbers.
        /// </param>
        /// <returns>
        ///   True if <see cref="_sigma"/> is greater than zero; otherwise, it returns false.
        /// </returns>
        [Pure]
        public static bool AreValidParams(double mu, double sigma)
        {
            return !double.IsNaN(mu) && sigma > 0;
        }

        /// <summary>
        ///   Returns a normal distributed floating point random number.
        /// </summary>
        /// <param name="generator">The generator from which random number are drawn.</param>
        /// <param name="mu">
        ///   The parameter mu which is used for generation of normal distributed random numbers.
        /// </param>
        /// <param name="sigma">
        ///   The parameter sigma which is used for generation of normal distributed random numbers.
        /// </param>
        /// <returns>A normal distributed floating point random number.</returns>
        [Pure]
        internal static double Sample(TGen generator, double mu, double sigma)
        {
            while (true)
            {
                var v1 = 2.0 * generator.NextDouble() - 1.0;
                var v2 = 2.0 * generator.NextDouble() - 1.0;
                var w = v1 * v1 + v2 * v2;
                if (w > 1)
                {
                    continue;
                }
                var y = Math.Sqrt(-2.0 * Math.Log(w) / w) * sigma;
                return generator.NextBoolean() ? v1 * y + mu : v2 * y + mu;
            }
        }

        #endregion TRandom Helpers
    }

    /// <summary>
    ///   Provides generation of normal distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="NormalDistribution"/> type bases upon information
    ///   presented on <a href="http://en.wikipedia.org/wiki/Normal_distribution">Wikipedia - Normal
    ///   distribution</a> and the implementation in the
    ///   <a href="http://www.lkn.ei.tum.de/lehre/scn/cncl/doc/html/cncl_toc.html">Communication
    ///   Networks Class Library</a>.
    /// </remarks>
    [Serializable]
    public sealed class NormalDistribution : NormalDistribution<IGenerator>
    {
        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="NormalDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        public NormalDistribution() : base(new XorShift128Generator(), DefaultMu, DefaultSigma)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Mu, DefaultMu));
            Debug.Assert(Equals(Sigma, DefaultSigma));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="NormalDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        public NormalDistribution(uint seed) : base(new XorShift128Generator(seed), DefaultMu, DefaultSigma)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Mu, DefaultMu));
            Debug.Assert(Equals(Sigma, DefaultSigma));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="NormalDistribution"/> class, using the
        ///   specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        public NormalDistribution(IGenerator generator) : base(generator, DefaultMu, DefaultSigma)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Mu, DefaultMu));
            Debug.Assert(Equals(Sigma, DefaultSigma));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="NormalDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="mu">
        ///   The parameter mu which is used for generation of normal distributed random numbers.
        /// </param>
        /// <param name="sigma">
        ///   The parameter sigma which is used for generation of normal distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <see cref="sigma"/> is less than or equal to zero.
        /// </exception>
        public NormalDistribution(double mu, double sigma) : base(new XorShift128Generator(), mu, sigma)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Mu, mu));
            Debug.Assert(Equals(Sigma, sigma));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="NormalDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        /// <param name="mu">
        ///   The parameter mu which is used for generation of normal distributed random numbers.
        /// </param>
        /// <param name="sigma">
        ///   The parameter sigma which is used for generation of normal distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <see cref="sigma"/> is less than or equal to zero.
        /// </exception>
        public NormalDistribution(uint seed, double mu, double sigma) : base(new XorShift128Generator(seed), mu, sigma)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Mu, mu));
            Debug.Assert(Equals(Sigma, sigma));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="NormalDistribution"/> class, using the
        ///   specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="mu">
        ///   The parameter mu which is used for generation of normal distributed random numbers.
        /// </param>
        /// <param name="sigma">
        ///   The parameter sigma which is used for generation of normal distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <see cref="sigma"/> is less than or equal to zero.
        /// </exception>
        public NormalDistribution(IGenerator generator, double mu, double sigma) : base(generator, mu, sigma)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Mu, mu));
            Debug.Assert(Equals(Sigma, sigma));
        }

        #endregion Construction
    }
}
