// The MIT License (MIT)
//
// Copyright (c) 2006-2007 Stefan Troschütz <stefan@troschuetz.de>
//
// Copyright (c) 2012-2019 Alessio Parma <alessio.parma@gmail.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute,
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT
// OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using System.Linq;
using Troschuetz.Random.Distributions.Continuous;

namespace Troschuetz.Random.Benchmarks
{
    [Config(typeof(Program.Config))]
    public class ContinuousDistributionComparison : AbstractComparison
    {
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