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
using Troschuetz.Random.Distributions.Continuous;

namespace Troschuetz.Random.Benchmarks
{
    [Config(typeof(Config))]
    public class ContinuousDistributionComparison : AbstractComparison
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

        private readonly Dictionary<string, Dictionary<string, IContinuousDistribution>> Distributions = new Dictionary<string, Dictionary<string, IContinuousDistribution>>
        {
            [N<BetaDistribution>()] = Generators.ToDictionary(x => x.Key, x => new BetaDistribution(x.Value) as IContinuousDistribution),
            [N<BetaPrimeDistribution>()] = Generators.ToDictionary(x => x.Key, x => new BetaPrimeDistribution(x.Value) as IContinuousDistribution),
            [N<CauchyDistribution>()] = Generators.ToDictionary(x => x.Key, x => new CauchyDistribution(x.Value) as IContinuousDistribution),
            [N<ChiDistribution>()] = Generators.ToDictionary(x => x.Key, x => new ChiDistribution(x.Value) as IContinuousDistribution),
            [N<ChiSquareDistribution>()] = Generators.ToDictionary(x => x.Key, x => new ChiSquareDistribution(x.Value) as IContinuousDistribution),
            [N<ContinuousUniformDistribution>()] = Generators.ToDictionary(x => x.Key, x => new ContinuousUniformDistribution(x.Value) as IContinuousDistribution),
            [N<ExponentialDistribution>()] = Generators.ToDictionary(x => x.Key, x => new ExponentialDistribution(x.Value) as IContinuousDistribution),
            [N<NormalDistribution>()] = Generators.ToDictionary(x => x.Key, x => new NormalDistribution(x.Value) as IContinuousDistribution),
        };

        [Params("Beta", "BetaPrime", "Cauchy", "Chi", "ChiSquare", "ContinuousUniform", "Exponential", "Normal")]
        public string Distribution { get; set; }

        [Benchmark]
        public double NextDouble() => Distributions[Distribution][Generator].NextDouble();
    }
}