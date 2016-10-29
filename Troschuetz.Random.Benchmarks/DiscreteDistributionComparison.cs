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
using Troschuetz.Random.Distributions.Discrete;
using Troschuetz.Random.Generators;

namespace Troschuetz.Random.Benchmarks
{
    [Config(typeof(Config))]
    public class DiscreteDistributionComparison
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

        private static readonly Dictionary<string, IGenerator> Generators = new Dictionary<string, IGenerator>
        {
            [nameof(XorShift128Generator)] = new XorShift128Generator(),
            [nameof(MT19937Generator)] = new MT19937Generator(),
            [nameof(NR3Generator)] = new NR3Generator(),
            [nameof(NR3Q1Generator)] = new NR3Q1Generator(),
            [nameof(NR3Q2Generator)] = new NR3Q2Generator(),
            [nameof(ALFGenerator)] = new ALFGenerator(),
            [nameof(StandardGenerator)] = new StandardGenerator()
        };

        private readonly Dictionary<string, Dictionary<string, IDiscreteDistribution>> Distributions = new Dictionary<string, Dictionary<string, IDiscreteDistribution>>
        {
            [nameof(BernoulliDistribution)] = Generators.ToDictionary(x => x.Key, x => new BernoulliDistribution(x.Value) as IDiscreteDistribution),
            [nameof(BinomialDistribution)] = Generators.ToDictionary(x => x.Key, x => new BinomialDistribution(x.Value) as IDiscreteDistribution),
            [nameof(CategoricalDistribution)] = Generators.ToDictionary(x => x.Key, x => new CategoricalDistribution(x.Value) as IDiscreteDistribution),
            [nameof(DiscreteUniformDistribution)] = Generators.ToDictionary(x => x.Key, x => new DiscreteUniformDistribution(x.Value) as IDiscreteDistribution),
            [nameof(GeometricDistribution)] = Generators.ToDictionary(x => x.Key, x => new GeometricDistribution(x.Value) as IDiscreteDistribution),
            [nameof(PoissonDistribution)] = Generators.ToDictionary(x => x.Key, x => new PoissonDistribution(x.Value) as IDiscreteDistribution),
        };

        [Params(nameof(XorShift128Generator), nameof(MT19937Generator), nameof(NR3Generator), nameof(NR3Q1Generator), nameof(NR3Q2Generator), nameof(ALFGenerator), nameof(StandardGenerator))]
        public string Generator { get; set; }

        [Params(nameof(BernoulliDistribution), nameof(BinomialDistribution), nameof(CategoricalDistribution), nameof(DiscreteUniformDistribution), nameof(GeometricDistribution), nameof(PoissonDistribution))]
        public string Distribution { get; set; }

        [Benchmark]
        public double NextDouble() => Distributions[Distribution][Generator].NextDouble();

        [Benchmark]
        public int Next() => Distributions[Distribution][Generator].Next();
    }
}