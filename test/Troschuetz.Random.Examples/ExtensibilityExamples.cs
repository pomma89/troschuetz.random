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

using System;
using System.Linq;
using Troschuetz.Random.Distributions;
using Troschuetz.Random.Distributions.Continuous;
using Troschuetz.Random.Generators;

namespace Troschuetz.Random.Examples
{
    /// <summary>
    ///   Examples on how the library can be extended or modified.
    /// </summary>
    internal static class ExtensibilityExamples
    {
        public static void Main()
        {
            // 1) Use SuperSillyGenerator to generate a few numbers.
            Console.WriteLine("Super silly generator in action!");
            var ssg = new SuperSillyGenerator(21U);
            foreach (var x in ssg.Doubles().Take(5)) Console.WriteLine(x);

            Console.WriteLine();

            // 2) Use SuperSillyContinuousDistribution to generate a few numbers.
            Console.WriteLine("Super silly distribution in action!");
            var ssd = new SuperSillyContinuousDistribution(ssg);
            Console.WriteLine(ssd.NextDouble());
            Console.WriteLine(ssd.NextDouble());
            Console.WriteLine(ssd.NextDouble());

            Console.WriteLine();

            // 3) Use SuperSillyGenerator with a normal distribution.
            Console.WriteLine("Super silly generator with a normal distribution");
            var normal = new NormalDistribution(ssg, 0, 1);
            Console.WriteLine(normal.NextDouble());
            Console.WriteLine(normal.NextDouble());
            Console.WriteLine(normal.NextDouble());

            Console.WriteLine();

            // 4) Change the core logic of normal distribution with a... Silly one.
            Console.WriteLine("Super silly redefinition of a normal distribution");
            NormalDistribution.Sample = (generator, mu, sigma) =>
            {
                // Silly method!!!
                return generator.NextDouble() + mu + sigma + mu * sigma;
            };
            Console.WriteLine(normal.NextDouble());
            Console.WriteLine(normal.NextDouble());
            Console.WriteLine(normal.NextDouble());

            Console.WriteLine();

            // 5) Use the new logic, even through TRandom.
            Console.WriteLine("Super silly redefinition of a normal distribution - via TRandom");
            var trandom = TRandom.New();
            Console.WriteLine(trandom.Normal(5, 0.5));
            Console.WriteLine(trandom.Normal(5, 0.5));
            Console.WriteLine(trandom.Normal(5, 0.5));

            Console.WriteLine();

            // 6) Use the extension method you defined for SuperSillyContinuousDistribution.
            Console.WriteLine("Super silly distribution via TRandom");
            Console.WriteLine(trandom.SuperSilly());
            Console.WriteLine(trandom.SuperSilly());
            Console.WriteLine(trandom.SuperSilly());
        }
    }

    // Super silly generator, which is provided as an example on how one can build a new generator.
    // Of course, never use this generator in production... It is super silly, after all.
    internal class SuperSillyGenerator : AbstractGenerator
    {
        // The state of the generaror, which usually consists of one or more variables. The initial
        // state, maybe depending on the seed, should be set by overriding the Reset method.
        private uint _state;

        // Just a simple constructor which passes the seed to the base constructor.
        public SuperSillyGenerator(uint seed) : base(seed)
        {
        }

        // Should return true if your generator can reset, false otherwise.
        public override bool CanReset => true;

        // Here you should handle the state of your generator. ALWAYS remember to call
        // base.Reset(seed), since it is necessary to correctly reset the AbstractGenerator.
        public override bool Reset(uint seed)
        {
            base.Reset(seed);
            _state = seed;
            return true;
        }

        // You must provide only three generation methods; from these methods, the AbstractGenerator
        // returns all other necessary objects.
        public override int NextInclusiveMaxValue() => (int) (++_state >> 1);

        public override double NextDouble() => ++_state * UIntToDoubleMultiplier;

        public override uint NextUIntInclusiveMaxValue() => ++_state;
    }

    // Super silly continuous distribution which is provided as an example on how one can build a new
    // distribution. Of course, never use this distribution in production... It is super silly, after all.
    internal class SuperSillyContinuousDistribution : AbstractDistribution, IContinuousDistribution
    {
        // Just a simple constructor which passes the generator to the base constructor.
        public SuperSillyContinuousDistribution(IGenerator generator) : base(generator)
        {
        }

        public double Minimum => 0.0;

        public double Maximum => 1.0;

        public double Mean => 0.5;

        public double Median => 0.5;

        public double[] Mode
        {
            get { throw new NotSupportedException(); }
        }

        public double Variance => 1.0 / 12.0;

        // The generation method, in which you define the logic of your distribution.
        public double NextDouble() => Sample(Generator);

        // Optional: Define your logic in a delegate, so that you can reuse it in TRandom or let
        //           others change it, if necessary. All standard distributions do this.
        public static Func<IGenerator, double> Sample { get; set; } = generator => generator.NextDouble();
    }

    // An example of how to enrich the TRandom class with your custom distribution.
    internal static class TRandomExtensions
    {
        // Simply call the static Sample you defined above. In the Main function, you can see that
        // this method can be used as other methods defined in TRandom.
        public static double SuperSilly(this TRandom r) => SuperSillyContinuousDistribution.Sample(r.Generator);
    }
}