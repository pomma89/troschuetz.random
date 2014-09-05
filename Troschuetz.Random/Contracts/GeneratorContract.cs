/*
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

namespace Troschuetz.Random.Contracts
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IGenerator))]
    abstract class GeneratorContract : IGenerator
    {
        public abstract uint Seed { get; }

        public abstract bool CanReset { get; }

        public bool Reset()
        {
            Contract.Ensures(Contract.Result<bool>() == CanReset);
            return default(bool);
        }

        public int Next()
        {
            Contract.Ensures(Contract.Result<int>() >= 0 && Contract.Result<int>() < int.MaxValue);
            return default(int);
        }

        public int NextInclusiveMaxValue()
        {
            Contract.Ensures(Contract.Result<int>() >= 0 && Contract.Result<int>() <= int.MaxValue);
            return default(int);
        }

        public int Next(int maxValue)
        {           
            Contract.Requires<ArgumentOutOfRangeException>(maxValue >= 0, ErrorMessages.NegativeMaxValue);
            Contract.Ensures(Contract.Result<int>() >= 0 && Contract.Result<int>() < maxValue);
            return default(int);
        }

        public int Next(int minValue, int maxValue)
        {
            Contract.Requires<ArgumentOutOfRangeException>(maxValue >= minValue, ErrorMessages.MinValueGreaterThanMaxValue);
            Contract.Ensures(Contract.Result<int>() >= minValue && Contract.Result<int>() < maxValue);
            return default(int);
        }

        public double NextDouble()
        {
            Contract.Ensures(Contract.Result<double>() >= 0 && Contract.Result<double>() < 1.0);
            return default(double);
        }

        public double NextDouble(double maxValue)
        {          
            Contract.Requires<ArgumentOutOfRangeException>(maxValue >= 0, ErrorMessages.NegativeMaxValue);
            Contract.Requires<ArgumentException>(!double.IsPositiveInfinity(maxValue));
            Contract.Ensures(Contract.Result<double>() >= 0 && Contract.Result<double>() < maxValue);
            return default(double);
        }

        public double NextDouble(double minValue, double maxValue)
        {
            Contract.Requires<ArgumentOutOfRangeException>(maxValue >= minValue, ErrorMessages.MinValueGreaterThanMaxValue);
            Contract.Requires<ArgumentException>(!double.IsPositiveInfinity(maxValue - minValue));
            Contract.Ensures(Contract.Result<double>() >= minValue && Contract.Result<double>() < maxValue);
            return default(double);
        }

        public uint NextUInt()
        {
            return default(uint);
        }

        public uint NextUInt(uint maxValue)
        {           
            Contract.Ensures(Contract.Result<uint>() < maxValue);
            return default(int);
        }

        public uint NextUInt(uint minValue, uint maxValue)
        {
            Contract.Requires<ArgumentOutOfRangeException>(maxValue >= minValue, ErrorMessages.MinValueGreaterThanMaxValue);
            Contract.Ensures(Contract.Result<uint>() >= minValue && Contract.Result<uint>() < maxValue);
            return default(int);
        }

        public abstract bool NextBoolean();

        public void NextBytes(byte[] buffer)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, ErrorMessages.NullBuffer);
        }
    }
}