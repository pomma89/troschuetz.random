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

using BenchmarkDotNet.Attributes;
using Troschuetz.Random.Generators;

namespace Troschuetz.Random.Benchmarks.Generators
{
    [Config("jobs=AllJits")]
    public class NextBytesComparison
    {
        private readonly byte[] Bytes = new byte[64];
        private readonly IGenerator XorShift128 = new XorShift128Generator();
        private readonly IGenerator MT19937 = new MT19937Generator();
        private readonly IGenerator NR3 = new NR3Generator();
        private readonly IGenerator NR3Q1 = new NR3Q1Generator();
        private readonly IGenerator NR3Q2 = new NR3Q2Generator();
        private readonly IGenerator ALF = new ALFGenerator();
        private readonly IGenerator Standard = new StandardGenerator();

        [Benchmark]
        public void NextBytes64_XorShift128() => XorShift128.NextBytes(Bytes);

        [Benchmark]
        public void NextBytes64_MT19937() => MT19937.NextBytes(Bytes);

        [Benchmark]
        public void NextBytes64_NR3() => NR3.NextBytes(Bytes);

        [Benchmark]
        public void NextBytes64_NR3Q1() => NR3Q1.NextBytes(Bytes);

        [Benchmark]
        public void NextBytes64_NR3Q2() => NR3Q2.NextBytes(Bytes);

        [Benchmark]
        public void NextBytes64_ALF() => ALF.NextBytes(Bytes);

        [Benchmark]
        public void NextBytes64_Standard() => Standard.NextBytes(Bytes);
    }
}
