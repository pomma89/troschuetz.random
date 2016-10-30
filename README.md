![](http://pomma89.altervista.org/troschuetz.random/logo-64.png "Troschuetz.Random Logo") Troschuetz.Random
================================================================================================================================================

*Fully managed library providing various random number generators and distributions.*

## Summary ##

* Latest release version: `v4.0.7`
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

### Generator comparison ###

```ini

Host Process Environment Information:
BenchmarkDotNet.Core=v0.9.9.0
OS=Microsoft Windows NT 6.2.9200.0
Processor=AMD A10 Extreme Edition Radeon R8, 4C+8G, ProcessorCount=4
Frequency=1949469 ticks, Resolution=512.9602 ns, Timer=TSC
CLR=MS.NET 4.0.30319.42000, Arch=32-bit RELEASE
GC=Concurrent Workstation
JitModules=clrjit-v4.6.1586.0

Type=GeneratorComparison  Mode=Throughput  

```
       Method |            Generator |        Median |     StdDev | Gen 0 | Gen 1 | Gen 2 | Bytes Allocated/Op |
------------- |--------------------- |-------------- |----------- |------ |------ |------ |------------------- |
  **NextBytes64** |         **ALF** |   **401.3753 ns** | **12.6876 ns** |     **-** |     **-** |     **-** |               **0,04** |
 NextBytes128 |         ALF |   536.9896 ns | 13.6032 ns |     - |     - |     - |               0,03 |
   NextDouble |         ALF |    64.4221 ns |  3.6562 ns |     - |     - |     - |               0,00 |
     NextUInt |         ALF |    58.9418 ns |  3.2873 ns |     - |     - |     - |               0,00 |
  NextBoolean |         ALF |    55.6178 ns |  2.7219 ns |     - |     - |     - |               0,00 |
  **NextBytes64** |     **MT19937** |   **461.2521 ns** | **21.3807 ns** |     **-** |     **-** |     **-** |               **0,23** |
 NextBytes128 |     MT19937 |   648.9628 ns | 31.2974 ns |  4.10 |     - |     - |               0,39 |
   NextDouble |     MT19937 |    70.0887 ns |  3.7549 ns |     - |     - |     - |               0,02 |
     NextUInt |     MT19937 |    63.8670 ns |  4.2052 ns |     - |     - |     - |               0,02 |
  NextBoolean |     MT19937 |    59.8829 ns |  3.4424 ns |     - |     - |     - |               0,00 |
  **NextBytes64** |         **NR3** |   **581.3656 ns** | **17.8818 ns** |     **-** |     **-** |     **-** |               **0,04** |
 NextBytes128 |         NR3 |   897.1520 ns | 44.5458 ns |     - |     - |     - |               0,07 |
   NextDouble |         NR3 |    75.0910 ns |  3.6353 ns |     - |     - |     - |               0,00 |
     NextUInt |         NR3 |    67.1369 ns |  3.0800 ns |     - |     - |     - |               0,00 |
  NextBoolean |         NR3 |    55.1278 ns |  2.6357 ns |     - |     - |     - |               0,00 |
  **NextBytes64** |       **NR3Q1** |   **518.8682 ns** | **21.9741 ns** |     **-** |     **-** |     **-** |               **0,03** |
 NextBytes128 |       NR3Q1 |   780.3807 ns | 30.1026 ns |     - |     - |     - |               0,07 |
   NextDouble |       NR3Q1 |    71.0899 ns |  3.0172 ns |     - |     - |     - |               0,00 |
     NextUInt |       NR3Q1 |    64.7113 ns |  3.3801 ns |     - |     - |     - |               0,00 |
  NextBoolean |       NR3Q1 |    55.1773 ns |  1.9266 ns |     - |     - |     - |               0,00 |
  **NextBytes64** |       **NR3Q2** |   **471.3648 ns** | **13.5829 ns** |     **-** |     **-** |     **-** |               **0,03** |
 NextBytes128 |       NR3Q2 |   692.3409 ns | 16.4802 ns |     - |     - |     - |               0,04 |
   NextDouble |       NR3Q2 |    68.0224 ns |  3.1411 ns |     - |     - |     - |               0,00 |
     NextUInt |       NR3Q2 |    60.6154 ns |  2.3698 ns |     - |     - |     - |               0,00 |
  NextBoolean |       NR3Q2 |    55.1637 ns |  2.3313 ns |     - |     - |     - |               0,00 |
  **NextBytes64** |    **Standard** | **1,469.9635 ns** | **40.9723 ns** |     **-** |     **-** |     **-** |               **0,08** |
 NextBytes128 |    Standard | 2,706.4343 ns | 75.6779 ns |     - |     - |     - |               0,15 |
   NextDouble |    Standard |    80.0627 ns |  5.5169 ns |     - |     - |     - |               0,00 |
     NextUInt |    Standard |   130.6997 ns |  5.3495 ns |     - |     - |     - |               0,01 |
  NextBoolean |    Standard |    62.2924 ns |  3.2914 ns |     - |     - |     - |               0,00 |
  **NextBytes64** | **XorShift128** |   **422.0735 ns** | **18.0436 ns** |     **-** |     **-** |     **-** |               **0,03** |
 NextBytes128 | XorShift128 |   605.3578 ns | 24.9150 ns |     - |     - |     - |               0,04 |
   NextDouble | XorShift128 |    75.9253 ns |  4.5045 ns |     - |     - |     - |               0,00 |
     NextUInt | XorShift128 |    70.4676 ns |  4.9054 ns |     - |     - |     - |               0,00 |
  NextBoolean | XorShift128 |    65.5909 ns |  3.0118 ns |     - |     - |     - |               0,00 |

### Continuous distribution comparison ###

```ini

Host Process Environment Information:
BenchmarkDotNet.Core=v0.9.9.0
OS=Microsoft Windows NT 6.2.9200.0
Processor=AMD A10 Extreme Edition Radeon R8, 4C+8G, ProcessorCount=4
Frequency=1949469 ticks, Resolution=512.9602 ns, Timer=TSC
CLR=MS.NET 4.0.30319.42000, Arch=32-bit RELEASE
GC=Concurrent Workstation
JitModules=clrjit-v4.6.1586.0

Type=ContinuousDistributionComparison  Mode=Throughput  

```
     Method |           Generator |                  Distribution |      Median |     StdDev | Gen 0 | Gen 1 | Gen 2 | Bytes Allocated/Op |
----------- |--------------------- |------------------------------ |------------ |----------- |------ |------ |------ |------------------- |
 **NextDouble** |         **ALF** |              **Beta** | **564.7818 ns** | **45.2765 ns** |     **-** |     **-** |     **-** |               **0,03** |
 **NextDouble** |         **ALF** |         **BetaPrime** | **544.4464 ns** | **22.4608 ns** |     **-** |     **-** |     **-** |               **0,04** |
 **NextDouble** |         **ALF** |            **Cauchy** | **201.4533 ns** | **13.2333 ns** |     **-** |     **-** |     **-** |               **0,02** |
 **NextDouble** |         **ALF** |               **Chi** | **253.0308 ns** | **12.1230 ns** |     **-** |     **-** |     **-** |               **0,02** |
 **NextDouble** |         **ALF** |         **ChiSquare** | **242.1054 ns** | **13.7031 ns** |     **-** |     **-** |     **-** |               **0,02** |
 **NextDouble** |         **ALF** | **ContinuousUniform** | **139.9917 ns** | **13.9336 ns** |     **-** |     **-** |     **-** |               **0,01** |
 **NextDouble** |         **ALF** |       **Exponential** | **161.1631 ns** |  **6.5344 ns** |     **-** |     **-** |     **-** |               **0,01** |
 **NextDouble** |         **ALF** |            **Normal** | **211.8438 ns** |  **6.0067 ns** |     **-** |     **-** |     **-** |               **0,02** |
 **NextDouble** |     **MT19937** |              **Beta** | **585.9670 ns** | **18.4013 ns** |     **-** |     **-** |     **-** |               **0,13** |
 **NextDouble** |     **MT19937** |         **BetaPrime** | **583.3253 ns** | **21.1971 ns** |     **-** |     **-** |     **-** |               **0,13** |
 **NextDouble** |     **MT19937** |            **Cauchy** | **208.2277 ns** |  **7.0745 ns** |     **-** |     **-** |     **-** |               **0,03** |
 **NextDouble** |     **MT19937** |               **Chi** | **260.3910 ns** | **10.3120 ns** |     **-** |     **-** |     **-** |               **0,05** |
 **NextDouble** |     **MT19937** |         **ChiSquare** | **262.8935 ns** | **25.8162 ns** |     **-** |     **-** |     **-** |               **0,05** |
 **NextDouble** |     **MT19937** | **ContinuousUniform** | **140.7257 ns** |  **3.4135 ns** |     **-** |     **-** |     **-** |               **0,02** |
 **NextDouble** |     **MT19937** |       **Exponential** | **171.3253 ns** |  **8.6901 ns** |     **-** |     **-** |     **-** |               **0,02** |
 **NextDouble** |     **MT19937** |            **Normal** | **221.5486 ns** |  **8.5723 ns** |     **-** |     **-** |     **-** |               **0,05** |
 **NextDouble** |         **NR3** |              **Beta** | **682.5746 ns** | **24.9644 ns** |     **-** |     **-** |     **-** |               **0,03** |
 **NextDouble** |         **NR3** |         **BetaPrime** | **665.3213 ns** | **25.2054 ns** |     **-** |     **-** |     **-** |               **0,03** |
 **NextDouble** |         **NR3** |            **Cauchy** | **202.7514 ns** |  **6.5672 ns** |     **-** |     **-** |     **-** |               **0,02** |
 **NextDouble** |         **NR3** |               **Chi** | **271.3742 ns** | **12.6290 ns** |     **-** |     **-** |     **-** |               **0,02** |
 **NextDouble** |         **NR3** |         **ChiSquare** | **269.0114 ns** | **13.5559 ns** |     **-** |     **-** |     **-** |               **0,02** |
 **NextDouble** |         **NR3** | **ContinuousUniform** | **143.7341 ns** |  **4.4296 ns** |     **-** |     **-** |     **-** |               **0,01** |
 **NextDouble** |         **NR3** |       **Exponential** | **167.1895 ns** |  **8.1451 ns** |     **-** |     **-** |     **-** |               **0,01** |
 **NextDouble** |         **NR3** |            **Normal** | **235.4710 ns** |  **8.9343 ns** |     **-** |     **-** |     **-** |               **0,02** |
 **NextDouble** |       **NR3Q1** |              **Beta** | **607.9451 ns** | **17.0967 ns** |     **-** |     **-** |     **-** |               **0,03** |
 **NextDouble** |       **NR3Q1** |         **BetaPrime** | **617.5248 ns** | **15.8400 ns** |     **-** |     **-** |     **-** |               **0,04** |
 **NextDouble** |       **NR3Q1** |            **Cauchy** | **205.6989 ns** |  **7.3714 ns** |     **-** |     **-** |     **-** |               **0,02** |
 **NextDouble** |       **NR3Q1** |               **Chi** | **261.1984 ns** | **12.8840 ns** |     **-** |     **-** |     **-** |               **0,02** |
 **NextDouble** |       **NR3Q1** |         **ChiSquare** | **253.0269 ns** | **14.7162 ns** |     **-** |     **-** |     **-** |               **0,02** |
 **NextDouble** |       **NR3Q1** | **ContinuousUniform** | **138.6705 ns** |  **8.0530 ns** |     **-** |     **-** |     **-** |               **0,01** |
 **NextDouble** |       **NR3Q1** |       **Exponential** | **160.6003 ns** |  **5.0955 ns** |     **-** |     **-** |     **-** |               **0,01** |
 **NextDouble** |       **NR3Q1** |            **Normal** | **221.5319 ns** |  **9.3278 ns** |     **-** |     **-** |     **-** |               **0,02** |
 **NextDouble** |       **NR3Q2** |              **Beta** | **601.1913 ns** | **37.4823 ns** |     **-** |     **-** |     **-** |               **0,03** |
 **NextDouble** |       **NR3Q2** |         **BetaPrime** | **589.3537 ns** | **27.0709 ns** |     **-** |     **-** |     **-** |               **0,04** |
 **NextDouble** |       **NR3Q2** |            **Cauchy** | **194.0564 ns** |  **5.7606 ns** |     **-** |     **-** |     **-** |               **0,01** |
 **NextDouble** |       **NR3Q2** |               **Chi** | **254.3404 ns** |  **9.1691 ns** |     **-** |     **-** |     **-** |               **0,02** |
 **NextDouble** |       **NR3Q2** |         **ChiSquare** | **251.3655 ns** | **10.6823 ns** |     **-** |     **-** |     **-** |               **0,02** |
 **NextDouble** |       **NR3Q2** | **ContinuousUniform** | **136.5725 ns** |  **6.8590 ns** |     **-** |     **-** |     **-** |               **0,01** |
 **NextDouble** |       **NR3Q2** |       **Exponential** | **158.4307 ns** |  **3.9975 ns** |     **-** |     **-** |     **-** |               **0,01** |
 **NextDouble** |       **NR3Q2** |            **Normal** | **223.3736 ns** |  **6.7194 ns** |     **-** |     **-** |     **-** |               **0,01** |
 **NextDouble** |    **Standard** |              **Beta** | **602.8002 ns** | **27.7162 ns** |     **-** |     **-** |     **-** |               **0,03** |
 **NextDouble** |    **Standard** |         **BetaPrime** | **621.6925 ns** | **18.3455 ns** |     **-** |     **-** |     **-** |               **0,04** |
 **NextDouble** |    **Standard** |            **Cauchy** | **201.4281 ns** |  **5.0959 ns** |     **-** |     **-** |     **-** |               **0,02** |
 **NextDouble** |    **Standard** |               **Chi** | **268.0968 ns** | **11.7280 ns** |     **-** |     **-** |     **-** |               **0,02** |
 **NextDouble** |    **Standard** |         **ChiSquare** | **258.6477 ns** |  **7.7946 ns** |     **-** |     **-** |     **-** |               **0,02** |
 **NextDouble** |    **Standard** | **ContinuousUniform** | **143.1884 ns** |  **4.5356 ns** |     **-** |     **-** |     **-** |               **0,01** |
 **NextDouble** |    **Standard** |       **Exponential** | **170.6359 ns** |  **7.2669 ns** |     **-** |     **-** |     **-** |               **0,01** |
 **NextDouble** |    **Standard** |            **Normal** | **225.8471 ns** |  **7.2494 ns** |     **-** |     **-** |     **-** |               **0,02** |
 **NextDouble** | **XorShift128** |              **Beta** | **580.0863 ns** | **17.8966 ns** |     **-** |     **-** |     **-** |               **0,04** |
 **NextDouble** | **XorShift128** |         **BetaPrime** | **583.0347 ns** | **18.2748 ns** |     **-** |     **-** |     **-** |               **0,03** |
 **NextDouble** | **XorShift128** |            **Cauchy** | **202.8847 ns** |  **6.1327 ns** |     **-** |     **-** |     **-** |               **0,02** |
 **NextDouble** | **XorShift128** |               **Chi** | **253.6144 ns** | **13.0590 ns** |     **-** |     **-** |     **-** |               **0,02** |
 **NextDouble** | **XorShift128** |         **ChiSquare** | **243.8094 ns** |  **7.4287 ns** |     **-** |     **-** |     **-** |               **0,02** |
 **NextDouble** | **XorShift128** | **ContinuousUniform** | **144.7021 ns** |  **5.6169 ns** |     **-** |     **-** |     **-** |               **0,01** |
 **NextDouble** | **XorShift128** |       **Exponential** | **164.0857 ns** |  **6.2893 ns** |     **-** |     **-** |     **-** |               **0,01** |
 **NextDouble** | **XorShift128** |            **Normal** | **217.5251 ns** | **15.5689 ns** |     **-** |     **-** |     **-** |               **0,02** |

### Discrete distribution comparison ###

```ini

Host Process Environment Information:
BenchmarkDotNet.Core=v0.9.9.0
OS=Microsoft Windows NT 6.2.9200.0
Processor=AMD A10 Extreme Edition Radeon R8, 4C+8G, ProcessorCount=4
Frequency=1949469 ticks, Resolution=512.9602 ns, Timer=TSC
CLR=MS.NET 4.0.30319.42000, Arch=32-bit RELEASE
GC=Concurrent Workstation
JitModules=clrjit-v4.6.1586.0

Type=DiscreteDistributionComparison  Mode=Throughput  

```
     Method |            Generator |                Distribution |      Median |     StdDev | Gen 0 | Gen 1 | Gen 2 | Bytes Allocated/Op |
----------- |--------------------- |---------------------------- |------------ |----------- |------ |------ |------ |------------------- |
 **NextDouble** |         **ALF** |       **Bernoulli** | **119.2830 ns** |  **5.8111 ns** |     **-** |     **-** |     **-** |               **0,01** |
       Next |         ALF |       Bernoulli | 119.4595 ns |  4.0621 ns |     - |     - |     - |               0,01 |
 **NextDouble** |         **ALF** |        **Binomial** | **118.1262 ns** |  **5.2042 ns** |     **-** |     **-** |     **-** |               **0,01** |
       Next |         ALF |        Binomial | 116.0250 ns |  6.0589 ns |     - |     - |     - |               0,01 |
 **NextDouble** |         **ALF** |     **Categorical** | **162.3266 ns** |  **5.9275 ns** |     **-** |     **-** |     **-** |               **0,01** |
       Next |         ALF |     Categorical | 157.5542 ns |  6.6664 ns |     - |     - |     - |               0,01 |
 **NextDouble** |         **ALF** | **DiscreteUniform** | **130.1418 ns** |  **4.3669 ns** |     **-** |     **-** |     **-** |               **0,01** |
       Next |         ALF | DiscreteUniform | 126.8054 ns |  6.0544 ns |     - |     - |     - |               0,01 |
 **NextDouble** |         **ALF** |       **Geometric** | **138.3377 ns** |  **6.8605 ns** |     **-** |     **-** |     **-** |               **0,01** |
       Next |         ALF |       Geometric | 130.1346 ns |  5.3675 ns |     - |     - |     - |               0,01 |
 **NextDouble** |         **ALF** |         **Poisson** | **189.3246 ns** | **10.3094 ns** |     **-** |     **-** |     **-** |               **0,01** |
       Next |         ALF |         Poisson | 184.1934 ns | 12.3198 ns |     - |     - |     - |               0,01 |
 **NextDouble** |     **MT19937** |       **Bernoulli** | **166.3347 ns** |  **8.1093 ns** |     **-** |     **-** |     **-** |               **0,02** |
       Next |     MT19937 |       Bernoulli | 125.4137 ns |  8.8573 ns |     - |     - |     - |               0,02 |
 **NextDouble** |     **MT19937** |        **Binomial** | **127.1335 ns** |  **6.9697 ns** |     **-** |     **-** |     **-** |               **0,02** |
       Next |     MT19937 |        Binomial | 127.5652 ns |  8.3479 ns |     - |     - |     - |               0,02 |
 **NextDouble** |     **MT19937** |     **Categorical** | **179.0222 ns** | **11.2939 ns** |     **-** |     **-** |     **-** |               **0,02** |
       Next |     MT19937 |     Categorical | 169.9243 ns |  6.7980 ns |     - |     - |     - |               0,02 |
 **NextDouble** |     **MT19937** | **DiscreteUniform** | **137.5204 ns** |  **5.6537 ns** |     **-** |     **-** |     **-** |               **0,02** |
       Next |     MT19937 | DiscreteUniform | 148.2287 ns | 12.8818 ns |     - |     - |     - |               0,02 |
 **NextDouble** |     **MT19937** |       **Geometric** | **148.5860 ns** |  **7.3860 ns** |     **-** |     **-** |     **-** |               **0,03** |
       Next |     MT19937 |       Geometric | 145.4794 ns | 10.8896 ns |     - |     - |     - |               0,03 |
 **NextDouble** |     **MT19937** |         **Poisson** | **194.4949 ns** |  **6.7210 ns** |     **-** |     **-** |     **-** |               **0,05** |
       Next |     MT19937 |         Poisson | 190.8595 ns | 11.3811 ns |     - |     - |     - |               0,03 |
 **NextDouble** |         **NR3** |       **Bernoulli** | **129.0464 ns** |  **4.8193 ns** |     **-** |     **-** |     **-** |               **0,01** |
       Next |         NR3 |       Bernoulli | 128.9043 ns |  5.1086 ns |     - |     - |     - |               0,01 |
 **NextDouble** |         **NR3** |        **Binomial** | **127.4425 ns** |  **4.4049 ns** |     **-** |     **-** |     **-** |               **0,01** |
       Next |         NR3 |        Binomial | 128.1422 ns |  5.9819 ns |     - |     - |     - |               0,01 |
 **NextDouble** |         **NR3** |     **Categorical** | **178.6449 ns** |  **5.5638 ns** |     **-** |     **-** |     **-** |               **0,01** |
       Next |         NR3 |     Categorical | 173.8394 ns |  6.5011 ns |     - |     - |     - |               0,01 |
 **NextDouble** |         **NR3** | **DiscreteUniform** | **143.6357 ns** |  **4.7112 ns** |     **-** |     **-** |     **-** |               **0,01** |
       Next |         NR3 | DiscreteUniform | 139.1345 ns |  7.7602 ns |     - |     - |     - |               0,01 |
 **NextDouble** |         **NR3** |       **Geometric** | **154.8823 ns** |  **9.9655 ns** |     **-** |     **-** |     **-** |               **0,01** |
       Next |         NR3 |       Geometric | 153.4500 ns |  4.4693 ns |     - |     - |     - |               0,01 |
 **NextDouble** |         **NR3** |         **Poisson** | **205.8942 ns** |  **7.4231 ns** |     **-** |     **-** |     **-** |               **0,02** |
       Next |         NR3 |         Poisson | 211.3698 ns |  7.4695 ns |     - |     - |     - |               0,02 |
 **NextDouble** |       **NR3Q1** |       **Bernoulli** | **124.2010 ns** | **10.5891 ns** |     **-** |     **-** |     **-** |               **0,01** |
       Next |       NR3Q1 |       Bernoulli | 122.2264 ns |  3.1275 ns |     - |     - |     - |               0,01 |
 **NextDouble** |       **NR3Q1** |        **Binomial** | **123.5844 ns** |  **6.8370 ns** |     **-** |     **-** |     **-** |               **0,01** |
       Next |       NR3Q1 |        Binomial | 121.8930 ns |  3.2296 ns |     - |     - |     - |               0,01 |
 **NextDouble** |       **NR3Q1** |     **Categorical** | **169.1586 ns** |  **4.9083 ns** |     **-** |     **-** |     **-** |               **0,01** |
       Next |       NR3Q1 |     Categorical | 168.3734 ns |  6.5751 ns |     - |     - |     - |               0,01 |
 **NextDouble** |       **NR3Q1** | **DiscreteUniform** | **138.4093 ns** |  **5.8611 ns** |     **-** |     **-** |     **-** |               **0,01** |
       Next |       NR3Q1 | DiscreteUniform | 134.0003 ns |  6.0810 ns |     - |     - |     - |               0,01 |
 **NextDouble** |       **NR3Q1** |       **Geometric** | **149.3405 ns** |  **7.5240 ns** |     **-** |     **-** |     **-** |               **0,01** |
       Next |       NR3Q1 |       Geometric | 145.4122 ns |  3.9322 ns |     - |     - |     - |               0,01 |
 **NextDouble** |       **NR3Q1** |         **Poisson** | **199.9778 ns** | **16.4269 ns** |     **-** |     **-** |     **-** |               **0,02** |
       Next |       NR3Q1 |         Poisson | 199.1589 ns | 12.8700 ns |     - |     - |     - |               0,02 |
 **NextDouble** |       **NR3Q2** |       **Bernoulli** | **121.8425 ns** |  **3.5704 ns** |     **-** |     **-** |     **-** |               **0,01** |
       Next |       NR3Q2 |       Bernoulli | 122.3650 ns |  3.9288 ns |     - |     - |     - |               0,01 |
 **NextDouble** |       **NR3Q2** |        **Binomial** | **120.6262 ns** |  **3.4427 ns** |     **-** |     **-** |     **-** |               **0,01** |
       Next |       NR3Q2 |        Binomial | 120.6807 ns |  3.9826 ns |     - |     - |     - |               0,01 |
 **NextDouble** |       **NR3Q2** |     **Categorical** | **167.6676 ns** | **10.4181 ns** |     **-** |     **-** |     **-** |               **0,01** |
       Next |       NR3Q2 |     Categorical | 164.8703 ns | 10.5710 ns |     - |     - |     - |               0,01 |
 **NextDouble** |       **NR3Q2** | **DiscreteUniform** | **135.6440 ns** |  **6.6638 ns** |     **-** |     **-** |     **-** |               **0,01** |
       Next |       NR3Q2 | DiscreteUniform | 130.1909 ns |  5.8340 ns |     - |     - |     - |               0,01 |
 **NextDouble** |       **NR3Q2** |       **Geometric** | **141.1170 ns** |  **3.8857 ns** |     **-** |     **-** |     **-** |               **0,01** |
       Next |       NR3Q2 |       Geometric | 141.7685 ns |  4.9574 ns |     - |     - |     - |               0,01 |
 **NextDouble** |       **NR3Q2** |         **Poisson** | **196.0464 ns** |  **9.1919 ns** |     **-** |     **-** |     **-** |               **0,02** |
       Next |       NR3Q2 |         Poisson | 191.8061 ns |  6.7616 ns |     - |     - |     - |               0,01 |
 **NextDouble** |    **Standard** |       **Bernoulli** | **132.4460 ns** |  **8.1172 ns** |     **-** |     **-** |     **-** |               **0,01** |
       Next |    Standard |       Bernoulli | 123.6537 ns |  3.6683 ns |     - |     - |     - |               0,01 |
 **NextDouble** |    **Standard** |        **Binomial** | **123.3657 ns** |  **6.8106 ns** |     **-** |     **-** |     **-** |               **0,01** |
       Next |    Standard |        Binomial | 123.6060 ns |  6.3782 ns |     - |     - |     - |               0,01 |
 **NextDouble** |    **Standard** |     **Categorical** | **174.3826 ns** | **10.4026 ns** |     **-** |     **-** |     **-** |               **0,01** |
       Next |    Standard |     Categorical | 168.1125 ns |  3.6631 ns |     - |     - |     - |               0,01 |
 **NextDouble** |    **Standard** | **DiscreteUniform** | **141.2165 ns** |  **5.0143 ns** |     **-** |     **-** |     **-** |               **0,01** |
       Next |    Standard | DiscreteUniform | 138.0848 ns |  4.3336 ns |     - |     - |     - |               0,01 |
 **NextDouble** |    **Standard** |       **Geometric** | **147.0533 ns** |  **2.2836 ns** |     **-** |     **-** |     **-** |               **0,01** |
       Next |    Standard |       Geometric | 146.8081 ns |  4.4159 ns |     - |     - |     - |               0,01 |
 **NextDouble** |    **Standard** |         **Poisson** | **205.9187 ns** |  **7.2529 ns** |     **-** |     **-** |     **-** |               **0,02** |
       Next |    Standard |         Poisson | 202.9169 ns | 10.2237 ns |     - |     - |     - |               0,01 |
 **NextDouble** | **XorShift128** |       **Bernoulli** | **125.3874 ns** |  **3.0229 ns** |     **-** |     **-** |     **-** |               **0,01** |
       Next | XorShift128 |       Bernoulli | 127.1274 ns |  7.4002 ns |     - |     - |     - |               0,01 |
 **NextDouble** | **XorShift128** |        **Binomial** | **125.1437 ns** |  **4.1121 ns** |     **-** |     **-** |     **-** |               **0,01** |
       Next | XorShift128 |        Binomial | 125.7723 ns |  2.9257 ns |     - |     - |     - |               0,01 |
 **NextDouble** | **XorShift128** |     **Categorical** | **171.7676 ns** |  **4.2417 ns** |     **-** |     **-** |     **-** |               **0,01** |
       Next | XorShift128 |     Categorical | 168.1748 ns |  6.9556 ns |     - |     - |     - |               0,01 |
 **NextDouble** | **XorShift128** | **DiscreteUniform** | **140.7030 ns** |  **6.5082 ns** |     **-** |     **-** |     **-** |               **0,01** |
       Next | XorShift128 | DiscreteUniform | 138.7947 ns |  6.0298 ns |     - |     - |     - |               0,01 |
 **NextDouble** | **XorShift128** |       **Geometric** | **141.7219 ns** |  **3.6250 ns** |     **-** |     **-** |     **-** |               **0,01** |
       Next | XorShift128 |       Geometric | 142.1299 ns |  7.2412 ns |     - |     - |     - |               0,01 |
 **NextDouble** | **XorShift128** |         **Poisson** | **194.8024 ns** |  **6.0905 ns** |     **-** |     **-** |     **-** |               **0,02** |
       Next | XorShift128 |         Poisson | 195.3065 ns |  9.0045 ns |     - |     - |     - |               0,01 |
