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

using System;
using System.Diagnostics;

namespace Troschuetz.Random.Generators
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TState">The type of the object containing the current generator state.</typeparam>
    public abstract class AbstractGenerator<TState> : IGenerator
        where TState : class, AbstractGenerator<TState>.IState, new()
    {
        TState _state;

        protected AbstractGenerator(uint seed)
        {
            _state = new TState();
            _state.Reset(seed);

            // The seed is stored in order to allow resetting the generator.
            Seed = seed;
        }

        #region Abstract members

        protected abstract int NextInclusiveMaxValue(TState state);

        protected abstract double NextDouble(TState state);

        protected abstract uint NextUInt(TState state);

        #endregion

        #region IGenerator members

        public uint Seed { get; }

        public bool CanReset
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool Reset()
        {
            throw new NotImplementedException();
        }

        public int Next()
        {
            throw new NotImplementedException();
        }

        public int Next(int maxValue)
        {
            throw new NotImplementedException();
        }

        public int Next(int minValue, int maxValue)
        {
            throw new NotImplementedException();
        }

        public int NextInclusiveMaxValue()
        {
            var result = NextInclusiveMaxValue(_state);

            // Postconditions
            Debug.Assert(result >= 0);
            return result;
        }

        public bool NextBoolean()
        {
            throw new NotImplementedException();
        }

        public void NextBytes(byte[] buffer)
        {
            throw new NotImplementedException();
        }

        public double NextDouble()
        {
            var result = NextDouble(_state);

            // Postconditions
            Debug.Assert(result >= 0.0 && result < 1.0);
            return result;
        }

        public double NextDouble(double maxValue)
        {
            throw new NotImplementedException();
        }

        public double NextDouble(double minValue, double maxValue)
        {
            throw new NotImplementedException();
        }

        public uint NextUInt()
        {
            // Here we have no preconditions and no postconditions.
            return NextUInt(_state);
        }

        public uint NextUInt(uint maxValue)
        {
            throw new NotImplementedException();
        }

        public uint NextUInt(uint minValue, uint maxValue)
        {
            throw new NotImplementedException();
        }

        public uint NextUIntExclusiveMaxValue()
        {
            throw new NotImplementedException();
        }

        #endregion

        public interface IState
        {
            void Reset(uint seed);
        }
    }
}
