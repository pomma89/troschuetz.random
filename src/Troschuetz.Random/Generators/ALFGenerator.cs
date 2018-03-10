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
    using System;
    using System.Diagnostics;

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
    ///
    ///   This generator is NOT thread safe.
    /// </remarks>
#if HAS_SERIALIZABLE
    [Serializable]
#endif

    public sealed class ALFGenerator : AbstractGenerator
    {
        #region Fields

        /// <summary>
        ///   Stores an index for the random number array element that will be accessed next.
        /// </summary>
        private int _i;

        /// <summary>
        ///   Stores the long lag of the Lagged Fibonacci pseudo-random number generator.
        /// </summary>
        private int _longLag = 1279;

        /// <summary>
        ///   Stores the short lag of the Lagged Fibonacci pseudo-random number generator.
        /// </summary>
        private int _shortLag = 418;

        /// <summary>
        ///   Stores an array of <see cref="_longLag"/> random numbers
        /// </summary>
        private uint[] _x;

        /// <summary>
        ///   Gets or sets the short lag of the Lagged Fibonacci pseudo-random number generator.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   Calls <see cref="IsValidShortLag"/> to determine whether a value is valid and therefore assignable.
        /// </remarks>
        public int ShortLag
        {
            get { return _shortLag; }
            set
            {
                _shortLag = IsValidShortLag(value) ? value : throw new ArgumentOutOfRangeException(nameof(ShortLag));
                Reset();
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
                _longLag = IsValidLongLag(value) ? value : throw new ArgumentOutOfRangeException(nameof(LongLag));
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
        ///   Initializes a new instance of the <see cref="ALFGenerator"/> class, using the specified
        ///   seed value.
        /// </summary>
        /// <param name="seed">
        ///   A number used to calculate a starting value for the pseudo-random number sequence. If a
        ///   negative number is specified, the absolute value of the number is used.
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
        public static bool IsValidShortLag(int value) => value > 0;

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="LongLag"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>
        ///   <see langword="true"/> if value is greater than <see cref="ShortLag"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsValidLongLag(int value) => value > _shortLag;

        /// <summary>
        ///   Fills the array <see cref="_x"/> with <see cref="_longLag"/> new unsigned random numbers.
        /// </summary>
        /// <remarks>
        ///   Generated random numbers are 32-bit unsigned integers greater than or equal to
        ///   <see cref="uint.MinValue"/> and less than or equal to <see cref="uint.MaxValue"/>.
        /// </remarks>
        private void Fill()
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
        ///   A 32-bit unsigned integer greater than or equal to 0, and less than or equal to
        ///   <see cref="uint.MaxValue"/>; that is, the range of return values includes 0 and <see cref="uint.MaxValue"/>.
        /// </returns>
        public override uint NextUIntInclusiveMaxValue()
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