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
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;
using System.Collections.Generic;
using System.Linq;
using Troschuetz.Random.Distributions.Discrete;

namespace Troschuetz.Random.Benchmarks
{
    [Config(typeof(Config))]
    public class DiscreteDistributionComparison : AbstractComparison
    {
        private class Config : ManualConfig
        {
            public Config()
            {
                Add(Job.LegacyJitX86);
                Add(CsvExporter.Default, HtmlExporter.Default, MarkdownExporter.GitHub, PlainExporter.Default);
                Add(new MemoryDiagnoser());
                Add(EnvironmentAnalyser.Default);
            }
        }

        private readonly Dictionary<string, Dictionary<string, IDiscreteDistribution>> Distributions = new Dictionary<string, Dictionary<string, IDiscreteDistribution>>
        {
            [N<BernoulliDistribution>()] = Generators.ToDictionary(x => x.Key, x => new BernoulliDistribution(x.Value) as IDiscreteDistribution),
            [N<BinomialDistribution>()] = Generators.ToDictionary(x => x.Key, x => new BinomialDistribution(x.Value) as IDiscreteDistribution),
            [N<CategoricalDistribution>()] = Generators.ToDictionary(x => x.Key, x => new CategoricalDistribution(x.Value) as IDiscreteDistribution),
            [N<DiscreteUniformDistribution>()] = Generators.ToDictionary(x => x.Key, x => new DiscreteUniformDistribution(x.Value) as IDiscreteDistribution),
            [N<GeometricDistribution>()] = Generators.ToDictionary(x => x.Key, x => new GeometricDistribution(x.Value) as IDiscreteDistribution),
            [N<PoissonDistribution>()] = Generators.ToDictionary(x => x.Key, x => new PoissonDistribution(x.Value) as IDiscreteDistribution),
        };

        [Params("Bernoulli", "Binomial", "Categorical", "DiscreteUniform", "Geometric", "Poisson")]
        public string Distribution { get; set; }

        [Benchmark]
        public double NextDouble() => Distributions[Distribution][Generator].NextDouble();

        [Benchmark]
        public int Next() => Distributions[Distribution][Generator].Next();
    }
}