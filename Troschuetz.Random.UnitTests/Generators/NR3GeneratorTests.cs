/*
 * Copyright © 2012 Alessio Parma (alessio.parma@gmail.com)
 *
 * This file is part of Troschuetz.Random.Tests Class Library.
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

namespace Troschuetz.Random.Tests.Generators
{
    using Random.Generators;

    public sealed class NR3GeneratorTests : GeneratorTests
    {
        protected override IGenerator GetGenerator() => new NR3Generator();

        protected override IGenerator GetGenerator(int seed) => new NR3Generator(seed);

        protected override IGenerator GetGenerator(uint seed) => new NR3Generator(seed);
    }

    public sealed class NR3Q1GeneratorTests : GeneratorTests
    {
        protected override IGenerator GetGenerator() => new NR3Q1Generator();

        protected override IGenerator GetGenerator(int seed) => new NR3Q1Generator(seed);

        protected override IGenerator GetGenerator(uint seed) => new NR3Q1Generator(seed);
    }

    public sealed class NR3Q2GeneratorTests : GeneratorTests
    {
        protected override IGenerator GetGenerator() => new NR3Q2Generator();

        protected override IGenerator GetGenerator(int seed) => new NR3Q2Generator(seed);

        protected override IGenerator GetGenerator(uint seed) => new NR3Q2Generator(seed);
    }
}