// The MIT License (MIT)
//
// Copyright (c) 2006-2007 Stefan Troschütz <stefan@troschuetz.de>
//
// Copyright (c) 2012-2017 Alessio Parma <alessio.parma@gmail.com>
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

using BenchmarkDotNet.Analysers;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnostics.Windows;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;

namespace Troschuetz.Random.Benchmarks
{
    [Config(typeof(Config))]
    public class GeneratorComparison : AbstractComparison
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

        private readonly byte[] Bytes64 = new byte[64];
        private readonly byte[] Bytes128 = new byte[128];

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