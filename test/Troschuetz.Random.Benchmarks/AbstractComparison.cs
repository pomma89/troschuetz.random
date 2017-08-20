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