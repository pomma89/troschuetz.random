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

using System;

namespace Troschuetz.Random.Generators
{
    [Serializable]
    sealed class NumericalRecipes3Generator : AbstractGenerator<NumericalRecipes3Generator.State>
    {
        public NumericalRecipes3Generator(uint seed) : base(seed)
        {
        }

        protected override int NextInclusiveMaxValue(State state)
        {
            throw new NotImplementedException();
        }

        protected override double NextDouble(State state)
        {
            throw new NotImplementedException();
        }

        protected override uint NextUInt(State state)
        {
            throw new NotImplementedException();
        }

        public sealed class State : IState
        {
            public void Reset(uint seed)
            {
                throw new NotImplementedException();
            }
        }
    }
}
