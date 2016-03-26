![](https://googledrive.com/host/0B8v0ikF4z2BiR29YQmxfSlE1Sms/Progetti/Troschuetz.Random/logo-64.png "Troschuetz.Random Logo") Troschuetz.Random
================================================================================================================================================

*Fully managed library providing various random number generators and distributions.*

## Summary ##

* Latest release version: `v4.0.4`
* Build status on [AppVeyor](https://ci.appveyor.com): [![Build status](https://ci.appveyor.com/api/projects/status/nrswa81ug0rsrpyp?svg=true)](https://ci.appveyor.com/project/pomma89/troschuetz-random)
* [NuGet](https://www.nuget.org) package(s):
    + [Troschuetz.Random](https://nuget.org/packages/Troschuetz.Random/)

## Introduction ##

All the hard work behind this library was done by Stefan Troschütz, and for which I thank him very much. What I have done with his great project, was simply to refactor and improve his code, while offering a new Python-style random class.

Please visit the [page of the original project](http://goo.gl/rN7my) in order to get an overview of the contents of this library. Unluckily, linked article also contains the only documentation available.

## About this repository and the maintainer ##

Everything done on this repository is freely offered on the terms of the project license. You are free to do everything you want with the code and its related files, as long as you respect the license and use common sense while doing it :-)

I maintain this project during my spare time, so I can offer limited assistance and I can offer **no kind of warranty**.

## Tester ##

A simple, yet effective, WinForms application is available in order to test the Troschuetz.Random library. As for the rest of the code, that application was completely written by Stefan Troschütz and what I did was simply to adapt it to the new refactored code.

The tester is now distributed on NuGet, embedded inside the main [Troschuetz.Random NuGet package](https://www.nuget.org/packages/Troschuetz.Random/). Just look for the "tester" folder, it contains everything needed to run and play with the tester.

## Basic usage ##

See example below to understand how you can use the library:

```cs

using System;
using System.Linq;
using Troschuetz.Random.Distributions.Continuous;
using Troschuetz.Random.Generators;

namespace Troschuetz.Random.Examples
{
    /// <summary>
    ///   Some examples of what you can do with this library.
    /// </summary>
    static class UsageExamples
    {
        public static void Main()
        {
            // 1) Use TRandom to generate a few random numbers - via IGenerator methods.
            Console.WriteLine("TRandom in action, used as an IGenerator");
            var trandom = new TRandom();
            Console.WriteLine(trandom.Next() - trandom.Next(5) + trandom.Next(3, 5));
            Console.WriteLine(trandom.NextDouble() * trandom.NextDouble(5.5) * trandom.NextDouble(10.1, 21.9));
            Console.WriteLine(trandom.NextBoolean());

            Console.WriteLine();

            // 2) Use TRandom to generate a few random numbers - via extension methods.
            Console.WriteLine("TRandom in action, used as an IGenerator augmented with extension methods");
            Console.WriteLine(string.Join(", ", trandom.Integers().Take(10)));
            Console.WriteLine(string.Join(", ", trandom.Doubles().Take(10)));
            Console.WriteLine(string.Join(", ", trandom.Booleans().Take(10)));

            Console.WriteLine();

            // 3) Use TRandom to generate a few distributed numbers.
            Console.WriteLine("TRandom in action, used as to get distributed numbers");
            Console.WriteLine(trandom.Normal(1.0, 0.1));
            Console.WriteLine(string.Join(", ", trandom.NormalSamples(1.0, 0.1).Take(20)));
            Console.WriteLine(trandom.Poisson(5));
            Console.WriteLine(string.Join(", ", trandom.PoissonSamples(5).Take(20)));

            Console.WriteLine();

            // 4) There are many generators available - XorShift128 is the default.
            var alf = new ALFGenerator(TMath.Seed());
            var nr3 = new NR3Generator();
            var std = new StandardGenerator(127);

            // 5) You can also use distribution directly, even with custom generators.
            Console.WriteLine("Showcase of some distributions");
            Console.WriteLine("Static sample for Normal: " + NormalDistribution.Sample(alf, 1.0, 0.1));
            Console.WriteLine("New instance for Normal: " + new NormalDistribution(1.0, 0.1).NextDouble());

            Console.WriteLine();
        }
    }
}
```

## Extensibility ##

After a request from a user, the library has been modified in order to allow it to be easily extended or modified. 

Starting from version 4.0, these extensibility cases are supported:

* Defining a custom generator by extending the AbstractGenerator class.
* Defining a custom distribution, by extending the AbstractDistribution class and by implementing either IContinuousDistribution or IDiscreteDistribution.
* Change the core definition of a standard distribution, by redefining the static Sample delegate, used to generate distributed numbers, and the static IsValidParam/AreValidParams delegate, used to validate parameters.

For your convenience, below you can find an example of how you can implement all features above (the code is also present in the Examples project):

```cs

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
    static class ExtensibilityExamples
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
    class SuperSillyGenerator : AbstractGenerator
    {
        // The state of the generaror, which usually consists of one or more variables. The initial
        // state, maybe depending on the seed, should be set by overriding the Reset method.
        uint _state;

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
        public override uint NextUInt() => ++_state;
    }

    // Super silly continuous distribution which is provided as an example on how one can build a
    // new distribution. Of course, never use this distribution in production... It is super silly,
    // after all.
    class SuperSillyContinuousDistribution : AbstractDistribution, IContinuousDistribution
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
    static class TRandomExtensions
    {
        // Simply call the static Sample you defined above. In the Main function, you can see that
        // this method can be used as other methods defined in TRandom.
        public static double SuperSilly(this TRandom r) => SuperSillyContinuousDistribution.Sample(r.Generator);
    }
}
```

## Benchmarks ##

All benchmarks are implemented with [BenchmarkDotNet](https://github.com/PerfDotNet/BenchmarkDotNet).

### IGenerator.NextDouble ###

```ini

BenchmarkDotNet=v0.9.4.0
OS=Microsoft Windows NT 6.2.9200.0
Processor=AMD A10-7850K Radeon R7, 12 Compute Cores 4C+8G, ProcessorCount=4
Frequency=14318180 ticks, Resolution=69.8413 ns, Timer=HPET
HostCLR=MS.NET 4.0.30319.42000, Arch=32-bit RELEASE
JitModules=clrjit-v4.6.1073.0

Type=NextDoubleComparison  Mode=Throughput  

```
                 Method | Platform |       Jit |     Median |    StdDev |
----------------------- |--------- |---------- |----------- |---------- |
         NextDouble_ALF |      X64 | LegacyJit |  5.5984 ns | 0.1719 ns |
         NextDouble_ALF |      X64 |    RyuJit |  6.5872 ns | 0.8043 ns |
         NextDouble_ALF |      X86 | LegacyJit |  6.1549 ns | 0.9234 ns |
     NextDouble_MT19937 |      X64 | LegacyJit |  8.6159 ns | 0.1740 ns |
     NextDouble_MT19937 |      X64 |    RyuJit |  9.4312 ns | 0.1711 ns |
     NextDouble_MT19937 |      X86 | LegacyJit |  9.6557 ns | 0.1818 ns |
         NextDouble_NR3 |      X64 | LegacyJit |  7.3716 ns | 0.1848 ns |
         NextDouble_NR3 |      X64 |    RyuJit |  5.3761 ns | 0.1471 ns |
         NextDouble_NR3 |      X86 | LegacyJit | 18.8395 ns | 0.4251 ns |
       NextDouble_NR3Q1 |      X64 | LegacyJit |  5.6026 ns | 0.1195 ns |
       NextDouble_NR3Q1 |      X64 |    RyuJit |  4.9740 ns | 0.1402 ns |
       NextDouble_NR3Q1 |      X86 | LegacyJit | 15.0855 ns | 0.3839 ns |
       NextDouble_NR3Q2 |      X64 | LegacyJit |  5.2523 ns | 0.1465 ns |
       NextDouble_NR3Q2 |      X64 |    RyuJit |  5.0888 ns | 0.1620 ns |
       NextDouble_NR3Q2 |      X86 | LegacyJit | 10.9995 ns | 0.2604 ns |
    NextDouble_Standard |      X64 | LegacyJit | 15.1002 ns | 0.5099 ns |
    NextDouble_Standard |      X64 |    RyuJit | 14.8925 ns | 0.4709 ns |
    NextDouble_Standard |      X86 | LegacyJit | 14.2089 ns | 0.2473 ns |
 NextDouble_XorShift128 |      X64 | LegacyJit |  4.1824 ns | 0.1384 ns |
 NextDouble_XorShift128 |      X64 |    RyuJit |  3.9391 ns | 0.1851 ns |
 NextDouble_XorShift128 |      X86 | LegacyJit |  8.6706 ns | 0.1574 ns |

### IGenerator.NextBytes ###

```ini

BenchmarkDotNet=v0.9.4.0
OS=Microsoft Windows NT 6.2.9200.0
Processor=AMD A10-7850K Radeon R7, 12 Compute Cores 4C+8G, ProcessorCount=4
Frequency=14318180 ticks, Resolution=69.8413 ns, Timer=HPET
HostCLR=MS.NET 4.0.30319.42000, Arch=32-bit RELEASE
JitModules=clrjit-v4.6.1073.0

Type=NextBytesComparison  Mode=Throughput  

```
                  Method | Platform |       Jit |        Median |     StdDev |
------------------------ |--------- |---------- |-------------- |----------- |
         NextBytes64_ALF |      X64 | LegacyJit |   229.5264 ns |  3.5215 ns |
         NextBytes64_ALF |      X64 |    RyuJit |   233.8902 ns |  5.4031 ns |
         NextBytes64_ALF |      X86 | LegacyJit |   240.4089 ns |  3.3763 ns |
     NextBytes64_MT19937 |      X64 | LegacyJit |   280.7194 ns |  5.2240 ns |
     NextBytes64_MT19937 |      X64 |    RyuJit |   304.2042 ns |  8.6313 ns |
     NextBytes64_MT19937 |      X86 | LegacyJit |   303.0285 ns |  5.1414 ns |
         NextBytes64_NR3 |      X64 | LegacyJit |   274.9566 ns |  6.4635 ns |
         NextBytes64_NR3 |      X64 |    RyuJit |   224.1195 ns |  3.5088 ns |
         NextBytes64_NR3 |      X86 | LegacyJit |   429.7071 ns |  6.2622 ns |
       NextBytes64_NR3Q1 |      X64 | LegacyJit |   222.8547 ns |  5.5028 ns |
       NextBytes64_NR3Q1 |      X64 |    RyuJit |   208.6428 ns |  4.6465 ns |
       NextBytes64_NR3Q1 |      X86 | LegacyJit |   362.6878 ns |  5.0926 ns |
       NextBytes64_NR3Q2 |      X64 | LegacyJit |   240.6030 ns |  3.9844 ns |
       NextBytes64_NR3Q2 |      X64 |    RyuJit |   205.4613 ns |  2.9763 ns |
       NextBytes64_NR3Q2 |      X86 | LegacyJit |   313.9187 ns |  6.2515 ns |
    NextBytes64_Standard |      X64 | LegacyJit | 1,018.5218 ns | 17.2124 ns |
    NextBytes64_Standard |      X64 |    RyuJit |   987.5373 ns | 13.4576 ns |
    NextBytes64_Standard |      X86 | LegacyJit | 1,072.7624 ns | 25.6522 ns |
 NextBytes64_XorShift128 |      X64 | LegacyJit |   197.8139 ns |  3.6545 ns |
 NextBytes64_XorShift128 |      X64 |    RyuJit |   193.6924 ns |  4.1886 ns |
 NextBytes64_XorShift128 |      X86 | LegacyJit |   267.4496 ns |  3.3618 ns |
