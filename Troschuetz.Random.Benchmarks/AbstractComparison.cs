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
using System.Collections.Generic;
using Troschuetz.Random.Generators;

namespace Troschuetz.Random.Benchmarks
{
    public abstract class AbstractComparison
    {
        protected static string N<T>() => typeof(T).Name.Replace(nameof(Generator), string.Empty).Replace("Distribution", string.Empty);

        protected static readonly Dictionary<string, IGenerator> Generators = new Dictionary<string, IGenerator>
        {
            [N<XorShift128Generator>()] = new XorShift128Generator(),
            [N<MT19937Generator>()] = new MT19937Generator(),
            [N<NR3Generator>()] = new NR3Generator(),
            [N<NR3Q1Generator>()] = new NR3Q1Generator(),
            [N<NR3Q2Generator>()] = new NR3Q2Generator(),
            [N<ALFGenerator>()] = new ALFGenerator(),
            [N<StandardGenerator>()] = new StandardGenerator()
        };

        [Params("XorShift128", "MT19937", "NR3", "NR3Q1", "NR3Q2", "ALF", "Standard")]
        public string Generator { get; set; }
    }
}