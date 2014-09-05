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

namespace Troschuetz.Random.Generators
{
    using System;

    /// <summary>
    ///   Represents a simple pseudo-random number generator.
    /// </summary>
    /// <remarks>
    ///   The <see cref="StandardGenerator"/> type internally uses an instance of the
    ///   <see cref="System.Random"/> type to generate pseudo-random numbers.
    /// </remarks>
    public sealed class StandardGenerator : GeneratorBase, IGenerator
    {
        #region Instance Fields

        /// <summary>
        ///   Stores an <see cref="int"/> used to generate up to 31 random <see cref="bool"/> values.
        /// </summary>
        int _bitBuffer;

        /// <summary>
        ///   Stores how many random <see cref="bool"/> values still can be generated from <see cref="_bitBuffer"/>.
        /// </summary>
        int _bitCount;

        /// <summary>
        ///   Stores an instance of <see cref="System.Random"/> type that is used to generate random numbers.
        /// </summary>
        Random _generator;

        /// <summary>
        ///   Stores the used seed.
        /// </summary>
        readonly uint _seed;

        #endregion

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="StandardGenerator"/> class,
        ///   using a time-dependent default seed value.
        /// </summary>
        public StandardGenerator() : this(Environment.TickCount)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="StandardGenerator"/> class,
        ///   using the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   A number used to calculate a starting value for the pseudo-random number sequence.
        ///   If a negative number is specified, the absolute value of the number is used. 
        /// </param>
        public StandardGenerator(int seed)
        {
            _seed = (uint) Math.Abs(seed);
            ResetGenerator();
        }

        #endregion

        #region Instance Methods

        /// <summary>
        ///   Resets the <see cref="StandardGenerator"/>, so that it produces
        ///   the same pseudo-random number sequence again.
        /// </summary>
        void ResetGenerator()
        {
            // Create a new Random object using the same seed.
            _generator = new Random((int) _seed); // Safe cast, seed is always positive.

            // Reset helper variables used for generation of random bools.
            _bitBuffer = 0;
            _bitCount = 0;
        }

        #endregion

        #region IGenerator Members

        [CLSCompliant(false)]
        public uint Seed
        {
            get { return _seed; }
        }

        public bool CanReset
        {
            get { return true; }
        }

        public bool Reset()
        {
            ResetGenerator();
            return true;
        }

        public int Next()
        {
            return _generator.Next();
        }

        public int NextInclusiveMaxValue()
        {
            return _generator.Next();
        }

        public int Next(int maxValue)
        {
            return _generator.Next(maxValue);
        }

        public int Next(int minValue, int maxValue)
        {
            return _generator.Next(minValue, maxValue);
        }

        public double NextDouble()
        {
            return _generator.NextDouble();
        }

        public double NextDouble(double maxValue)
        {
            return _generator.NextDouble()*maxValue;
        }

        public double NextDouble(double minValue, double maxValue)
        {
            return minValue + _generator.NextDouble()*(maxValue - minValue);
        }

        public uint NextUInt()
        {
            return (uint) _generator.Next();
        }

        public bool NextBoolean()
        {
            if (_bitCount == 0) {
                // Generate 31 more bits (1 int) and store it for future calls.
                _bitBuffer = _generator.Next();

                // Reset the bitCount and use rightmost bit of buffer to generate random bool.
                _bitCount = 30;
                return (_bitBuffer & 0x1) == 1;
            }

            // Decrease the bitCount and use rightmost bit of shifted buffer to generate random bool.
            _bitCount--;
            return ((_bitBuffer >>= 1) & 0x1) == 1;
        }

        public void NextBytes(byte[] buffer)
        {
            _generator.NextBytes(buffer);
        }

        #endregion

        #region Object Members

        public override string ToString()
        {
            return string.Format("Generator: {0}, BitBuffer: {1}, BitCount: {2}", _generator, _bitBuffer, _bitCount);
        }

        #endregion
    }
}