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

using BenchmarkDotNet.Analysers;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnostics.Windows;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using System.Collections.Generic;
using System.Linq;
using Troschuetz.Random.Generators;

namespace Troschuetz.Random.Benchmarks
{
    [Config(typeof(Config))]
    public class GeneratorComparison
    {
        private class Config : ManualConfig
        {
            public Config()
            {
                Add(Job.Default);
                Add(GetColumns().ToArray());
                Add(CsvExporter.Default, HtmlExporter.Default, MarkdownExporter.GitHub, PlainExporter.Default);
                Add(new MemoryDiagnoser());
                Add(EnvironmentAnalyser.Default);
            }
        }

        private readonly Dictionary<string, IGenerator> Generators = new Dictionary<string, IGenerator>
        {
            [nameof(XorShift128Generator)] = new XorShift128Generator(),
            [nameof(MT19937Generator)] = new MT19937Generator(),
            [nameof(NR3Generator)] = new NR3Generator(),
            [nameof(NR3Q1Generator)] = new NR3Q1Generator(),
            [nameof(NR3Q2Generator)] = new NR3Q2Generator(),
            [nameof(ALFGenerator)] = new ALFGenerator(),
            [nameof(StandardGenerator)] = new StandardGenerator()
        };

        private readonly byte[] Bytes64 = new byte[64];
        private readonly byte[] Bytes128 = new byte[128];

        [Params(nameof(XorShift128Generator), nameof(MT19937Generator), nameof(NR3Generator), nameof(NR3Q1Generator), nameof(NR3Q2Generator), nameof(ALFGenerator), nameof(StandardGenerator))]
        public string Generator { get; set; }

        [Benchmark]
        public void NextBytes64() => Generators[Generator].NextBytes(Bytes64);

        [Benchmark]
        public void NextBytes128() => Generators[Generator].NextBytes(Bytes128);

        [Benchmark]
        public double NextDouble() => Generators[Generator].NextDouble();

        [Benchmark]
        public uint NextUInt() => Generators[Generator].NextUInt();

        [Benchmark]
        public bool NextBoolean() => Generators[Generator].NextBoolean();
    }
}