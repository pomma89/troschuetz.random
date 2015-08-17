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

/* boost random/lagged_fibonacci.hpp header file
 *
 * Copyright Jens Maurer 2000-2001
 * Distributed under the Boost Software License, Version 1.0. (See
 * accompanying file LICENSE_1_0.txt or copy at
 * http://www.boost.org/LICENSE_1_0.txt)
 *
 * See http://www.boost.org for most recent version including documentation.
 *
 * $Id: lagged_fibonacci.hpp,v 1.28 2005/05/21 15:57:00 dgregor Exp $
 *
 * Revision history
 *  2001-02-18  moved to individual header files
 */

#endregion Original Copyright

namespace Troschuetz.Random.Generators
{
    using PommaLabs.Thrower;
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Runtime.CompilerServices;

    /// <summary>
    ///   Represents a Additive Lagged Fibonacci pseudo-random number generator.
    /// </summary>
    /// <remarks>
    ///   The <see cref="ALFGenerator"/> type bases upon the implementation in the
    ///   <a href="http://www.boost.org/libs/random/index.html">Boost Random Number Library</a>. It
    ///   uses the modulus 2 <sup>32</sup> and by default the "lags" 418 and 1279, which can be
    ///   adjusted through the associated <see cref="ShortLag"/> and <see cref="LongLag"/> properties.
    /// 
    ///   Some popular pairs are presented on
    ///   <a href="http://en.wikipedia.org/wiki/Lagged_Fibonacci_generator">Wikipedia - Lagged
    ///   Fibonacci generator</a>.
    /// </remarks>
    [Serializable]
    public sealed class ALFGenerator : AbstractGenerator
    {
        #region Fields

        /// <summary>
        ///   Stores an index for the random number array element that will be accessed next.
        /// </summary>
        int _i;

        /// <summary>
        ///   Stores the long lag of the Lagged Fibonacci pseudo-random number generator.
        /// </summary>
        int _longLag = 1279;

        /// <summary>
        ///   Stores the short lag of the Lagged Fibonacci pseudo-random number generator.
        /// </summary>
        int _shortLag = 418;

        /// <summary>
        ///   Stores an array of <see cref="_longLag"/> random numbers
        /// </summary>
        uint[] _x;

        /// <summary>
        ///   Gets or sets the short lag of the Lagged Fibonacci pseudo-random number generator.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   Calls <see cref="IsValidShortLag"/> to determine whether a value is valid and
        ///   therefore assignable.
        /// </remarks>
        public int ShortLag
        {
            get { return _shortLag; }
            set
            {
                Raise<ArgumentOutOfRangeException>.IfNot(IsValidShortLag(value));
                _shortLag = value;
            }
        }

        /// <summary>
        ///   Gets or sets the long lag of the Lagged Fibonacci pseudo-random number generator.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value"/> is less than or equal to <see cref="ShortLag"/>.
        /// </exception>
        /// <remarks>
        ///   Calls <see cref="IsValidLongLag"/> to determine whether a value is valid and therefore assignable.
        /// </remarks>
        public int LongLag
        {
            get { return _longLag; }
            set
            {
                Raise<ArgumentOutOfRangeException>.IfNot(IsValidShortLag(value));
                _longLag = value;
                Reset();
            }
        }

        #endregion Fields

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="ALFGenerator"/> class, using a
        ///   time-dependent default seed value.
        /// </summary>
        public ALFGenerator() : base(TMath.Seed())
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ALFGenerator"/> class, using the
        ///   specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   A number used to calculate a starting value for the pseudo-random number sequence. If
        ///   a negative number is specified, the absolute value of the number is used.
        /// </param>
        public ALFGenerator(int seed) : base((uint) Math.Abs(seed))
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="StandardGenerator"/> class, using the
        ///   specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        public ALFGenerator(uint seed) : base(seed)
        {
        }

        #endregion Construction

        #region Instance methods

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="ShortLag"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns><see langword="true"/> if value is greater than 0; otherwise, <see langword="false"/>.</returns>
        [Pure]
        public static bool IsValidShortLag(int value) => value > 0;

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="LongLag"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>
        ///   <see langword="true"/> if value is greater than <see cref="ShortLag"/>; otherwise, <see langword="false"/>.
        /// </returns>
        [Pure]
        public bool IsValidLongLag(int value) => value > _shortLag;

        /// <summary>
        ///   Fills the array <see cref="_x"/> with <see cref="_longLag"/> new unsigned random numbers.
        /// </summary>
        /// <remarks>
        ///   Generated random numbers are 32-bit unsigned integers greater than or equal to
        ///   <see cref="uint.MinValue"/> and less than or equal to <see cref="uint.MaxValue"/>.
        /// </remarks>
        void Fill()
        {
            // Two loops to avoid costly modulo operations
            for (var j = 0; j < _shortLag; ++j)
            {
                _x[j] = _x[j] + _x[j + (_longLag - _shortLag)];
            }
            for (var j = _shortLag; j < _longLag; ++j)
            {
                _x[j] = _x[j] + _x[j - _shortLag];
            }
            _i = 0;
        }

        #endregion Instance methods

        #region IGenerator members

        /// <summary>
        ///   Gets a value indicating whether the random number generator can be reset, so that it
        ///   produces the same random number sequence again.
        /// </summary>
        public override bool CanReset => true;

        /// <summary>
        ///   Resets the random number generator using the specified seed, so that it produces the
        ///   same random number sequence again. To understand whether this generator can be reset,
        ///   you can query the <see cref="CanReset"/> property.
        /// </summary>
        /// <param name="seed">The seed value used by the generator.</param>
        /// <returns>True if the random number generator was reset; otherwise, false.</returns>
        public override bool Reset(uint seed)
        {
            base.Reset(seed);

            var gen = new MT19937Generator(seed);
            _x = new uint[_longLag];
            for (uint j = 0; j < _longLag; ++j)
            {
                _x[j] = gen.NextUInt();
            }
            _i = _longLag;
            return true;
        }

        /// <summary>
        ///   Returns a nonnegative random number less than or equal to <see cref="int.MaxValue"/>.
        /// </summary>
        /// <returns>
        ///   A 32-bit signed integer greater than or equal to 0, and less than or equal to
        ///   <see cref="int.MaxValue"/>; that is, the range of return values includes 0 and <see cref="int.MaxValue"/>.
        /// </returns>
#if PORTABLE

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public override int NextInclusiveMaxValue()
        {
            // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
            if (_i >= _longLag)
            {
                Fill();
            }
            var result = (int) (_x[_i++] >> 1);

            // Postconditions
            Debug.Assert(result >= 0);
            return result;
        }

        /// <summary>
        ///   Returns a nonnegative floating point random number less than 1.0.
        /// </summary>
        /// <returns>
        ///   A double-precision floating point number greater than or equal to 0.0, and less than
        ///   1.0; that is, the range of return values includes 0.0 but not 1.0.
        /// </returns>
#if PORTABLE

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public override double NextDouble()
        {
            // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
            if (_i >= _longLag)
            {
                Fill();
            }
            var result = (int) (_x[_i++] >> 1) * IntToDoubleMultiplier;

            // Postconditions
            Debug.Assert(result >= 0.0 && result < 1.0);
            return result;
        }

        /// <summary>
        ///   Returns an unsigned random number.
        /// </summary>
        /// <returns>
        ///   A 32-bit unsigned integer greater than or equal to <see cref="uint.MinValue"/> and
        ///   less than or equal to <see cref="uint.MaxValue"/>.
        /// </returns>
#if PORTABLE

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public override uint NextUInt()
        {
            if (_i >= _longLag)
            {
                Fill();
            }
            return _x[_i++];
        }

        #endregion IGenerator members
    }
}
