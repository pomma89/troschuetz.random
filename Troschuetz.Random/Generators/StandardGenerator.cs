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
    using PommaLabs.Thrower;
    using System;
    using Core;

    /// <summary>
    ///   Represents a simple pseudo-random number generator.
    /// </summary>
    /// <remarks>
    ///   The <see cref="StandardGenerator"/> type internally uses an instance of the
    ///   <see cref="System.Random"/> type to generate pseudo-random numbers.
    /// </remarks>
    [Serializable]
    public sealed class StandardGenerator : GeneratorBase<StandardGenerator>, IGenerator
    {
        #region Instance Fields

        /// <summary>
        ///   Stores an <see cref="int"/> used to generate up to 31 random <see cref="bool"/> values.
        /// </summary>
        int _bitBuffer;

        /// <summary>
        ///   Stores a byte array used to compute the result of <see cref="NextUInt()"/>, starting from the output of <see cref="NextBytes"/>.
        /// </summary>
        byte[] _uintBuffer;

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

        public bool Reset(uint seed)
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
            // Preconditions
            RaiseArgumentOutOfRangeException.IfIsLessOrEqual(maxValue, 0, nameof(maxValue), ErrorMessages.NegativeMaxValue);

            return _generator.Next(maxValue);
        }

        public int Next(int minValue, int maxValue)
        {
            // Preconditions
            RaiseArgumentOutOfRangeException.IfIsGreaterOrEqual(minValue, maxValue, nameof(minValue), ErrorMessages.MinValueGreaterThanOrEqualToMaxValue);

            return _generator.Next(minValue, maxValue);
        }

        public double NextDouble()
        {
            return _generator.NextDouble();
        }

        public double NextDouble(double maxValue)
        {
            // Preconditions
            RaiseArgumentOutOfRangeException.IfIsLessOrEqual(maxValue, 0.0, nameof(maxValue), ErrorMessages.NegativeMaxValue);
            Raise<ArgumentException>.If(double.IsPositiveInfinity(maxValue));

            return _generator.NextDouble()*maxValue;
        }

        public double NextDouble(double minValue, double maxValue)
        {
            // Preconditions
            RaiseArgumentOutOfRangeException.IfIsGreaterOrEqual(minValue, maxValue, nameof(minValue), ErrorMessages.MinValueGreaterThanOrEqualToMaxValue);
            Raise<ArgumentException>.If(double.IsPositiveInfinity(maxValue - minValue));

            return minValue + _generator.NextDouble()*(maxValue - minValue);
        }

        [CLSCompliant(false)]
        public uint NextUInt()
        {
            // UInt32 requires four bytes in order to be built.
            NextBytes(_uintBuffer ?? (_uintBuffer = new byte[4]));
            return BitConverter.ToUInt32(_uintBuffer, 0);
        }

        [CLSCompliant(false)]
        public uint NextUInt(uint maxValue)
        {
            // Preconditions
            RaiseArgumentOutOfRangeException.IfIsLess(maxValue, 1U, nameof(maxValue), ErrorMessages.MaxValueIsTooSmall);

            // UInt32 requires four bytes in order to be built.
            NextBytes(_uintBuffer ?? (_uintBuffer = new byte[4]));
            var x = BitConverter.ToUInt32(_uintBuffer, 0);

            // The shift operation and extra int cast before the first multiplication give better performance.
            // See comment in NextDouble().
            return (uint) ((int) (x >> 1)*IntToDoubleMultiplier*maxValue);
        }

        [CLSCompliant(false)]
        public uint NextUInt(uint minValue, uint maxValue)
        {
            // Preconditions
            RaiseArgumentOutOfRangeException.IfIsGreaterOrEqual(minValue, maxValue, nameof(minValue), ErrorMessages.MinValueGreaterThanOrEqualToMaxValue);

            // UInt32 requires four bytes in order to be built.
            NextBytes(_uintBuffer ?? (_uintBuffer = new byte[4]));
            var x = BitConverter.ToUInt32(_uintBuffer, 0);

            // The shift operation and extra int cast before the first multiplication give better performance.
            // See comment in NextDouble().
            return minValue + (uint) ((int) (x >> 1)*IntToDoubleMultiplier*(maxValue - minValue));
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
            // Preconditions
            RaiseArgumentNullException.IfIsNull(buffer, nameof(buffer), ErrorMessages.NullBuffer);

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