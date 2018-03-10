# ![](http://pomma89.altervista.org/troschuetz.random/logo-64.png "Troschuetz.Random Logo") Troschuetz.Random

*Fully managed library providing various random number generators and distributions.*

[![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=ELJWKEYS9QGKA)

## Summary

* Latest release version: `v4.3.0`
* Build status on [Travis CI](https://travis-ci.org): [![Build Status](https://travis-ci.org/pomma89/Troschuetz.Random.svg?branch=dev-pomma89)](https://travis-ci.org/pomma89/Troschuetz.Random)
* Build status on [AppVeyor](https://ci.appveyor.com): [![Build status](https://ci.appveyor.com/api/projects/status/nrswa81ug0rsrpyp?svg=true)](https://ci.appveyor.com/project/pomma89/troschuetz-random)
* [Wyam](https://wyam.io/) generated API documentation: [https://pomma89.github.io/Troschuetz.Random/api/](https://pomma89.github.io/Troschuetz.Random/api/)
* [NuGet](https://www.nuget.org) package(s):
    + [Troschuetz.Random](https://nuget.org/packages/Troschuetz.Random/)

### How to build

#### Windows

Clone the project, go to the root and run PowerShell script `build.ps1`. In order for it to work, you need:

* At least Windows 10 Fall Creators Update
* At least Visual Studio 2017 Update 4
* .NET Framework 4.7.1 Developer Pack
* .NET Core 2.0 SDK

#### Linux

Clone the project, go to the root and run Bash script `build.sh`. In order for it to work, you need:

* TODO, still need to make it building reliably.

## Introduction

All the hard work behind this library was done by Stefan Troschütz, and for which I thank him very much. What I have done with his great project, was simply to refactor and improve his code, while offering a new Python-style random class.

Please visit the [page of the original project](http://goo.gl/rN7my) in order to get an overview of the contents of this library. Unluckily, linked article also contains the only documentation available.

## Tester

A simple, yet effective, WinForms application is available in order to test the Troschuetz.Random library. As for the rest of the code, that application was completely written by Stefan Troschütz and what I did was simply to adapt it to the new refactored code.

The tester is now distributed on NuGet, embedded inside the main [Troschuetz.Random NuGet package](https://www.nuget.org/packages/Troschuetz.Random/). Just look for the "tester" folder, it contains everything needed to run and play with the tester.

## Basic usage

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

## Extensibility

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

## Benchmarks

All benchmarks are implemented with [BenchmarkDotNet](https://github.com/PerfDotNet/BenchmarkDotNet).

### Generator comparison

``` ini

BenchmarkDotNet=v0.10.9, OS=Windows 7 SP1 (6.1.7601)
Processor=Intel Xeon CPU E5-2640 0 2.50GHzIntel Xeon CPU E5-2640 0 2.50GHz, ProcessorCount=4
Frequency=14318180 Hz, Resolution=69.8413 ns, Timer=HPET
  [Host]    : .NET Framework 4.6.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.2114.0
  RyuJitX64 : .NET Framework 4.6.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.2114.0

Job=RyuJitX64  Jit=RyuJit  Platform=X64  

```
 |       Method |   Generator |        Mean |       Error |        StdDev |      Median | Allocated |
 |------------- |------------ |------------:|------------:|--------------:|------------:|----------:|
 |  **NextBytes64** |         **ALF** |   **204.40 ns** |  **15.1778 ns** |    **43.7913 ns** |   **175.67 ns** |       **0 B** |
 | NextBytes128 |         ALF |   698.23 ns |  24.8372 ns |    70.0537 ns |   725.99 ns |       0 B |
 |   NextDouble |         ALF |    73.48 ns |   8.8984 ns |    26.2371 ns |    73.96 ns |       0 B |
 |     NextUInt |         ALF |    84.17 ns |   8.5350 ns |    25.1658 ns |    93.41 ns |       0 B |
 |  NextBoolean |         ALF |    83.22 ns |   8.8154 ns |    25.9925 ns |    97.18 ns |       0 B |
 |  **NextBytes64** |     **MT19937** |   **390.19 ns** |  **42.1964 ns** |   **124.4171 ns** |   **385.31 ns** |       **1 B** |
 | NextBytes128 |     MT19937 |   572.99 ns |  60.9795 ns |   176.9126 ns |   466.00 ns |       2 B |
 |   NextDouble |     MT19937 |   106.78 ns |   9.9708 ns |    29.3992 ns |   116.91 ns |       0 B |
 |     NextUInt |     MT19937 |    89.92 ns |  12.0589 ns |    35.5558 ns |    80.62 ns |       0 B |
 |  NextBoolean |     MT19937 |   123.84 ns |   1.3634 ns |     1.1385 ns |   123.83 ns |       0 B |
 |  **NextBytes64** |         **NR3** |   **332.23 ns** |  **30.9759 ns** |    **91.3330 ns** |   **367.56 ns** |       **0 B** |
 | NextBytes128 |         NR3 |   516.73 ns |  43.8408 ns |   129.2655 ns |   518.97 ns |       0 B |
 |   NextDouble |         NR3 |    47.71 ns |   0.9843 ns |     1.1335 ns |    47.39 ns |       0 B |
 |     NextUInt |         NR3 |    46.58 ns |   0.1341 ns |     0.1254 ns |    46.55 ns |       0 B |
 |  NextBoolean |         NR3 |    81.48 ns |   9.3388 ns |    27.5358 ns |    88.94 ns |       0 B |
 |  **NextBytes64** |       **NR3Q1** |   **293.89 ns** |  **28.4659 ns** |    **83.9323 ns** |   **341.38 ns** |       **0 B** |
 | NextBytes128 |       NR3Q1 |   525.39 ns |  48.5404 ns |   143.1226 ns |   597.57 ns |       0 B |
 |   NextDouble |       NR3Q1 |    80.49 ns |   8.6479 ns |    25.4984 ns |    92.60 ns |       0 B |
 |     NextUInt |       NR3Q1 |    69.00 ns |   9.0288 ns |    26.6217 ns |    67.20 ns |       0 B |
 |  NextBoolean |       NR3Q1 |    76.92 ns |   9.0602 ns |    26.7143 ns |    80.57 ns |       0 B |
 |  **NextBytes64** |       **NR3Q2** |   **268.61 ns** |  **30.4143 ns** |    **89.6772 ns** |   **287.60 ns** |       **0 B** |
 | NextBytes128 |       NR3Q2 |   501.28 ns |  42.7137 ns |   125.9424 ns |   531.24 ns |       0 B |
 |   NextDouble |       NR3Q2 |    88.23 ns |   8.2374 ns |    24.2883 ns |    99.62 ns |       0 B |
 |     NextUInt |       NR3Q2 |    80.96 ns |   8.7471 ns |    25.7910 ns |    90.41 ns |       0 B |
 |  NextBoolean |       NR3Q2 |    82.56 ns |   8.9897 ns |    26.5062 ns |    86.25 ns |       0 B |
 |  **NextBytes64** |    **Standard** | **2,024.02 ns** | **209.0748 ns** |   **616.4617 ns** | **2,400.90 ns** |       **0 B** |
 | NextBytes128 |    Standard | 3,918.49 ns | 366.4296 ns | 1,080.4260 ns | 4,520.70 ns |       0 B |
 |   NextDouble |    Standard |    63.75 ns |   0.1743 ns |     0.1361 ns |    63.72 ns |       0 B |
 |     NextUInt |    Standard |   215.26 ns |  20.9545 ns |    61.7849 ns |   249.28 ns |       0 B |
 |  NextBoolean |    Standard |    94.98 ns |   8.8891 ns |    26.2098 ns |   101.87 ns |       0 B |
 |  **NextBytes64** | **XorShift128** |   **272.72 ns** |  **30.7428 ns** |    **90.6458 ns** |   **296.95 ns** |       **0 B** |
 | NextBytes128 | XorShift128 |   484.33 ns |  55.2191 ns |   162.8147 ns |   550.33 ns |       0 B |
 |   NextDouble | XorShift128 |   101.53 ns |  11.2477 ns |    33.1640 ns |   103.34 ns |       0 B |
 |     NextUInt | XorShift128 |    61.39 ns |  12.1388 ns |    10.7607 ns |    58.20 ns |       0 B |
 |  NextBoolean | XorShift128 |   103.31 ns |  10.6770 ns |    31.4813 ns |   102.86 ns |       0 B |

### Continuous distribution comparison

``` ini

BenchmarkDotNet=v0.10.9, OS=Windows 7 SP1 (6.1.7601)
Processor=Intel Xeon CPU E5-2640 0 2.50GHzIntel Xeon CPU E5-2640 0 2.50GHz, ProcessorCount=4
Frequency=14318180 Hz, Resolution=69.8413 ns, Timer=HPET
  [Host]    : .NET Framework 4.6.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.2114.0
  RyuJitX64 : .NET Framework 4.6.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.2114.0

Job=RyuJitX64  Jit=RyuJit  Platform=X64  

```
 |     Method |      Distribution |   Generator |     Mean |      Error |      StdDev |   Median | Allocated |
 |----------- |------------------ |------------ |---------:|-----------:|------------:|---------:|----------:|
 | **NextDouble** |              **Beta** |         **ALF** | **338.8 ns** |  **6.8819 ns** |  **17.1383 ns** | **334.1 ns** |       **0 B** |
 | **NextDouble** |              **Beta** |     **MT19937** | **375.0 ns** |  **7.4529 ns** |   **7.6535 ns** | **372.5 ns** |       **0 B** |
 | **NextDouble** |              **Beta** |         **NR3** | **385.9 ns** | **29.4146 ns** |  **84.8678 ns** | **331.7 ns** |       **0 B** |
 | **NextDouble** |              **Beta** |       **NR3Q1** | **381.7 ns** | **31.7568 ns** |  **92.6361 ns** | **320.7 ns** |       **0 B** |
 | **NextDouble** |              **Beta** |       **NR3Q2** | **324.3 ns** |  **8.4574 ns** |  **10.6959 ns** | **321.6 ns** |       **0 B** |
 | **NextDouble** |              **Beta** |    **Standard** | **676.8 ns** | **72.1428 ns** | **212.7146 ns** | **740.6 ns** |       **0 B** |
 | **NextDouble** |              **Beta** | **XorShift128** | **525.4 ns** | **61.5321 ns** | **181.4288 ns** | **550.6 ns** |       **0 B** |
 | **NextDouble** |         **BetaPrime** |         **ALF** | **590.4 ns** | **63.1414 ns** | **186.1738 ns** | **611.7 ns** |       **0 B** |
 | **NextDouble** |         **BetaPrime** |     **MT19937** | **631.2 ns** | **69.2676 ns** | **204.2371 ns** | **654.7 ns** |       **0 B** |
 | **NextDouble** |         **BetaPrime** |         **NR3** | **574.2 ns** | **67.0142 ns** | **197.5929 ns** | **593.5 ns** |       **0 B** |
 | **NextDouble** |         **BetaPrime** |       **NR3Q1** | **529.6 ns** | **65.0692 ns** | **191.8580 ns** | **578.1 ns** |       **0 B** |
 | **NextDouble** |         **BetaPrime** |       **NR3Q2** | **568.9 ns** | **49.2187 ns** | **145.1224 ns** | **566.5 ns** |       **0 B** |
 | **NextDouble** |         **BetaPrime** |    **Standard** | **930.8 ns** | **14.6964 ns** |  **15.0921 ns** | **934.3 ns** |       **0 B** |
 | **NextDouble** |         **BetaPrime** | **XorShift128** | **619.9 ns** | **64.4471 ns** | **190.0238 ns** | **698.5 ns** |       **0 B** |
 | **NextDouble** |            **Cauchy** |         **ALF** | **284.1 ns** | **28.4789 ns** |  **83.9707 ns** | **322.2 ns** |       **0 B** |
 | **NextDouble** |            **Cauchy** |     **MT19937** | **305.8 ns** | **31.4928 ns** |  **92.8573 ns** | **340.4 ns** |       **0 B** |
 | **NextDouble** |            **Cauchy** |         **NR3** | **250.0 ns** | **25.2609 ns** |  **74.4824 ns** | **240.8 ns** |       **0 B** |
 | **NextDouble** |            **Cauchy** |       **NR3Q1** | **281.4 ns** | **26.9234 ns** |  **79.3843 ns** | **316.6 ns** |       **0 B** |
 | **NextDouble** |            **Cauchy** |       **NR3Q2** | **261.6 ns** | **23.9230 ns** |  **70.5375 ns** | **260.7 ns** |       **0 B** |
 | **NextDouble** |            **Cauchy** |    **Standard** | **292.1 ns** | **31.0940 ns** |  **91.6812 ns** | **321.7 ns** |       **0 B** |
 | **NextDouble** |            **Cauchy** | **XorShift128** | **314.6 ns** | **30.0235 ns** |  **88.5249 ns** | **359.6 ns** |       **0 B** |
 | **NextDouble** |               **Chi** |         **ALF** | **290.5 ns** | **24.4455 ns** |  **72.0782 ns** | **291.2 ns** |       **0 B** |
 | **NextDouble** |               **Chi** |     **MT19937** | **229.4 ns** | **20.0623 ns** |  **57.5624 ns** | **188.7 ns** |       **0 B** |
 | **NextDouble** |               **Chi** |         **NR3** | **315.2 ns** | **30.1669 ns** |  **88.9478 ns** | **341.8 ns** |       **0 B** |
 | **NextDouble** |               **Chi** |       **NR3Q1** | **312.2 ns** | **30.9188 ns** |  **91.1648 ns** | **353.1 ns** |       **0 B** |
 | **NextDouble** |               **Chi** |       **NR3Q2** | **370.4 ns** | **13.3744 ns** |  **35.6990 ns** | **386.5 ns** |       **0 B** |
 | **NextDouble** |               **Chi** |    **Standard** | **371.1 ns** | **36.5695 ns** | **107.8259 ns** | **428.8 ns** |       **0 B** |
 | **NextDouble** |               **Chi** | **XorShift128** | **336.5 ns** | **33.9692 ns** | **100.1589 ns** | **392.2 ns** |       **0 B** |
 | **NextDouble** |         **ChiSquare** |         **ALF** | **308.4 ns** | **34.0179 ns** | **100.3025 ns** | **343.7 ns** |       **0 B** |
 | **NextDouble** |         **ChiSquare** |     **MT19937** | **357.7 ns** | **37.1976 ns** | **109.6781 ns** | **408.7 ns** |       **0 B** |
 | **NextDouble** |         **ChiSquare** |         **NR3** | **320.5 ns** | **35.3093 ns** | **104.1104 ns** | **364.8 ns** |       **0 B** |
 | **NextDouble** |         **ChiSquare** |       **NR3Q1** | **320.2 ns** | **32.0234 ns** |  **94.4218 ns** | **355.5 ns** |       **0 B** |
 | **NextDouble** |         **ChiSquare** |       **NR3Q2** | **314.0 ns** | **32.9729 ns** |  **97.2214 ns** | **327.7 ns** |       **0 B** |
 | **NextDouble** |         **ChiSquare** |    **Standard** | **389.2 ns** | **38.0037 ns** | **112.0548 ns** | **447.8 ns** |       **0 B** |
 | **NextDouble** |         **ChiSquare** | **XorShift128** | **321.3 ns** | **37.5629 ns** | **110.7552 ns** | **340.6 ns** |       **0 B** |
 | **NextDouble** | **ContinuousUniform** |         **ALF** | **199.9 ns** | **22.7095 ns** |  **66.9595 ns** | **231.3 ns** |       **0 B** |
 | **NextDouble** | **ContinuousUniform** |     **MT19937** | **180.4 ns** | **24.7217 ns** |  **72.8926 ns** | **125.0 ns** |       **0 B** |
 | **NextDouble** | **ContinuousUniform** |         **NR3** | **251.1 ns** | **10.2896 ns** |  **27.9937 ns** | **266.0 ns** |       **0 B** |
 | **NextDouble** | **ContinuousUniform** |       **NR3Q1** | **254.6 ns** |  **5.0959 ns** |   **9.5713 ns** | **259.3 ns** |       **0 B** |
 | **NextDouble** | **ContinuousUniform** |       **NR3Q2** | **194.1 ns** | **19.3062 ns** |  **56.9249 ns** | **201.2 ns** |       **0 B** |
 | **NextDouble** | **ContinuousUniform** |    **Standard** | **238.8 ns** | **24.1279 ns** |  **71.1418 ns** | **271.1 ns** |       **0 B** |
 | **NextDouble** | **ContinuousUniform** | **XorShift128** | **191.6 ns** | **22.9668 ns** |  **67.7182 ns** | **187.2 ns** |       **0 B** |
 | **NextDouble** |       **Exponential** |         **ALF** | **284.6 ns** |  **5.6958 ns** |   **6.3309 ns** | **286.7 ns** |       **0 B** |
 | **NextDouble** |       **Exponential** |     **MT19937** | **307.3 ns** | **11.7584 ns** |  **32.1885 ns** | **323.7 ns** |       **0 B** |
 | **NextDouble** |       **Exponential** |         **NR3** | **206.6 ns** | **22.0528 ns** |  **65.0232 ns** | **195.7 ns** |       **0 B** |
 | **NextDouble** |       **Exponential** |       **NR3Q1** | **228.9 ns** | **24.6797 ns** |  **72.7686 ns** | **263.5 ns** |       **0 B** |
 | **NextDouble** |       **Exponential** |       **NR3Q2** | **214.4 ns** | **24.5962 ns** |  **72.5225 ns** | **241.8 ns** |       **0 B** |
 | **NextDouble** |       **Exponential** |    **Standard** | **229.5 ns** | **21.5897 ns** |  **63.6576 ns** | **214.6 ns** |       **0 B** |
 | **NextDouble** |       **Exponential** | **XorShift128** | **223.2 ns** | **27.7926 ns** |  **81.9471 ns** | **226.4 ns** |       **0 B** |
 | **NextDouble** |            **Normal** |         **ALF** | **278.4 ns** | **29.4686 ns** |  **86.8889 ns** | **321.5 ns** |       **0 B** |
 | **NextDouble** |            **Normal** |     **MT19937** | **273.2 ns** | **31.4820 ns** |  **92.8255 ns** | **272.5 ns** |       **0 B** |
 | **NextDouble** |            **Normal** |         **NR3** | **264.9 ns** | **30.8614 ns** |  **90.9954 ns** | **284.1 ns** |       **0 B** |
 | **NextDouble** |            **Normal** |       **NR3Q1** | **244.4 ns** | **24.4235 ns** |  **72.0134 ns** | **247.0 ns** |       **0 B** |
 | **NextDouble** |            **Normal** |       **NR3Q2** | **243.7 ns** | **23.5501 ns** |  **69.4379 ns** | **243.1 ns** |       **0 B** |
 | **NextDouble** |            **Normal** |    **Standard** | **187.3 ns** |  **0.3546 ns** |   **0.2769 ns** | **187.3 ns** |       **0 B** |
 | **NextDouble** |            **Normal** | **XorShift128** | **169.1 ns** |  **4.9066 ns** |   **6.2053 ns** | **167.4 ns** |       **0 B** |

### Discrete distribution comparison

``` ini

BenchmarkDotNet=v0.10.9, OS=Windows 7 SP1 (6.1.7601)
Processor=Intel Xeon CPU E5-2640 0 2.50GHzIntel Xeon CPU E5-2640 0 2.50GHz, ProcessorCount=4
Frequency=14318180 Hz, Resolution=69.8413 ns, Timer=HPET
  [Host]    : .NET Framework 4.6.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.2114.0
  RyuJitX64 : .NET Framework 4.6.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.2114.0

Job=RyuJitX64  Jit=RyuJit  Platform=X64  

```
 |     Method |    Distribution |   Generator |     Mean |      Error |     StdDev |   Median | Allocated |
 |----------- |---------------- |------------ |---------:|-----------:|-----------:|---------:|----------:|
 | **NextDouble** |       **Bernoulli** |         **ALF** | **160.4 ns** | **14.4213 ns** | **42.5215 ns** | **159.9 ns** |       **0 B** |
 |       Next |       Bernoulli |         ALF | 197.2 ns | 19.8672 ns | 58.5790 ns | 218.1 ns |       0 B |
 | **NextDouble** |       **Bernoulli** |     **MT19937** | **122.9 ns** |  **7.8789 ns** | **17.2943 ns** | **117.9 ns** |       **0 B** |
 |       Next |       Bernoulli |     MT19937 | 123.5 ns |  4.2766 ns |  4.9250 ns | 122.2 ns |       0 B |
 | **NextDouble** |       **Bernoulli** |         **NR3** | **111.6 ns** |  **4.2093 ns** |  **3.7315 ns** | **110.6 ns** |       **0 B** |
 |       Next |       Bernoulli |         NR3 | 201.8 ns | 21.8658 ns | 64.4719 ns | 235.6 ns |       0 B |
 | **NextDouble** |       **Bernoulli** |       **NR3Q1** | **180.2 ns** | **21.2046 ns** | **62.5223 ns** | **198.5 ns** |       **0 B** |
 |       Next |       Bernoulli |       NR3Q1 | 201.1 ns | 18.6531 ns | 54.9990 ns | 229.9 ns |       0 B |
 | **NextDouble** |       **Bernoulli** |       **NR3Q2** | **245.0 ns** |  **8.0769 ns** | **21.9739 ns** | **257.2 ns** |       **0 B** |
 |       Next |       Bernoulli |       NR3Q2 | 216.1 ns | 19.1118 ns | 56.3514 ns | 246.2 ns |       0 B |
 | **NextDouble** |       **Bernoulli** |    **Standard** | **200.3 ns** | **24.5942 ns** | **72.5165 ns** | **203.9 ns** |       **0 B** |
 |       Next |       Bernoulli |    Standard | 231.8 ns | 21.4910 ns | 63.3668 ns | 261.8 ns |       0 B |
 | **NextDouble** |       **Bernoulli** | **XorShift128** | **228.5 ns** | **22.2808 ns** | **65.6955 ns** | **248.7 ns** |       **0 B** |
 |       Next |       Bernoulli | XorShift128 | 212.7 ns | 20.9942 ns | 61.9017 ns | 208.3 ns |       0 B |
 | **NextDouble** |        **Binomial** |         **ALF** | **136.5 ns** | **13.5852 ns** | **40.0562 ns** | **107.3 ns** |       **0 B** |
 |       Next |        Binomial |         ALF | 204.5 ns | 17.6937 ns | 52.1703 ns | 231.6 ns |       0 B |
 | **NextDouble** |        **Binomial** |     **MT19937** | **196.5 ns** | **20.8854 ns** | **61.5810 ns** | **196.3 ns** |       **0 B** |
 |       Next |        Binomial |     MT19937 | 209.9 ns | 22.7067 ns | 66.9513 ns | 242.0 ns |       0 B |
 | **NextDouble** |        **Binomial** |         **NR3** | **210.2 ns** | **19.4362 ns** | **57.3080 ns** | **239.8 ns** |       **0 B** |
 |       Next |        Binomial |         NR3 | 203.3 ns | 20.3067 ns | 59.8747 ns | 237.6 ns |       0 B |
 | **NextDouble** |        **Binomial** |       **NR3Q1** | **251.8 ns** |  **0.9306 ns** |  **0.6729 ns** | **251.7 ns** |       **0 B** |
 |       Next |        Binomial |       NR3Q1 | 203.8 ns | 20.4505 ns | 60.2987 ns | 236.1 ns |       0 B |
 | **NextDouble** |        **Binomial** |       **NR3Q2** | **192.4 ns** | **21.5017 ns** | **63.3983 ns** | **228.4 ns** |       **0 B** |
 |       Next |        Binomial |       NR3Q2 | 165.7 ns | 15.5334 ns | 45.8007 ns | 166.7 ns |       0 B |
 | **NextDouble** |        **Binomial** |    **Standard** | **163.8 ns** | **22.7104 ns** | **66.9623 ns** | **113.2 ns** |       **0 B** |
 |       Next |        Binomial |    Standard | 202.4 ns | 22.6595 ns | 66.8120 ns | 228.1 ns |       0 B |
 | **NextDouble** |        **Binomial** | **XorShift128** | **207.7 ns** | **25.3699 ns** | **74.8038 ns** | **231.4 ns** |       **0 B** |
 |       Next |        Binomial | XorShift128 | 216.8 ns | 20.1394 ns | 59.3815 ns | 233.6 ns |       0 B |
 | **NextDouble** |     **Categorical** |         **ALF** | **270.5 ns** | **21.5656 ns** | **63.5867 ns** | **311.0 ns** |       **0 B** |
 |       Next |     Categorical |         ALF | 221.6 ns | 28.3732 ns | 83.6589 ns | 246.6 ns |       0 B |
 | **NextDouble** |     **Categorical** |     **MT19937** | **258.9 ns** | **27.7546 ns** | **81.8351 ns** | **292.4 ns** |       **0 B** |
 |       Next |     Categorical |     MT19937 | 273.2 ns | 25.3398 ns | 74.7149 ns | 311.0 ns |       0 B |
 | **NextDouble** |     **Categorical** |         **NR3** | **210.9 ns** | **27.4160 ns** | **80.8366 ns** | **206.7 ns** |       **0 B** |
 |       Next |     Categorical |         NR3 | 221.3 ns | 21.3502 ns | 62.9516 ns | 222.7 ns |       0 B |
 | **NextDouble** |     **Categorical** |       **NR3Q1** | **225.5 ns** | **25.6885 ns** | **75.7432 ns** | **257.5 ns** |       **0 B** |
 |       Next |     Categorical |       NR3Q1 | 219.8 ns | 26.3636 ns | 77.7335 ns | 233.2 ns |       0 B |
 | **NextDouble** |     **Categorical** |       **NR3Q2** | **221.3 ns** | **21.2556 ns** | **62.6725 ns** | **224.7 ns** |       **0 B** |
 |       Next |     Categorical |       NR3Q2 | 217.9 ns | 25.3020 ns | 74.6035 ns | 214.6 ns |       0 B |
 | **NextDouble** |     **Categorical** |    **Standard** | **258.7 ns** | **28.0689 ns** | **82.7619 ns** | **302.6 ns** |       **0 B** |
 |       Next |     Categorical |    Standard | 223.5 ns | 28.6035 ns | 84.3382 ns | 224.6 ns |       0 B |
 | **NextDouble** |     **Categorical** | **XorShift128** | **251.4 ns** | **26.8065 ns** | **79.0395 ns** | **273.6 ns** |       **0 B** |
 |       Next |     Categorical | XorShift128 | 257.2 ns | 27.1336 ns | 80.0041 ns | 280.5 ns |       0 B |
 | **NextDouble** | **DiscreteUniform** |         **ALF** | **150.4 ns** | **15.2359 ns** | **44.9233 ns** | **141.2 ns** |       **0 B** |
 |       Next | DiscreteUniform |         ALF | 215.8 ns | 20.1608 ns | 59.4445 ns | 247.6 ns |       0 B |
 | **NextDouble** | **DiscreteUniform** |     **MT19937** | **215.5 ns** | **25.1271 ns** | **74.0877 ns** | **243.9 ns** |       **0 B** |
 |       Next | DiscreteUniform |     MT19937 | 228.2 ns | 22.6271 ns | 66.7164 ns | 256.9 ns |       0 B |
 | **NextDouble** | **DiscreteUniform** |         **NR3** | **201.1 ns** | **19.5358 ns** | **57.6019 ns** | **209.7 ns** |       **0 B** |
 |       Next | DiscreteUniform |         NR3 | 206.3 ns | 22.2678 ns | 65.6570 ns | 241.6 ns |       0 B |
 | **NextDouble** | **DiscreteUniform** |       **NR3Q1** | **199.5 ns** | **22.1306 ns** | **65.2527 ns** | **226.3 ns** |       **0 B** |
 |       Next | DiscreteUniform |       NR3Q1 | 188.5 ns | 20.7400 ns | 61.1524 ns | 184.1 ns |       0 B |
 | **NextDouble** | **DiscreteUniform** |       **NR3Q2** | **174.7 ns** | **16.0187 ns** | **47.2315 ns** | **174.7 ns** |       **0 B** |
 |       Next | DiscreteUniform |       NR3Q2 | 176.7 ns | 23.4391 ns | 69.1108 ns | 151.1 ns |       0 B |
 | **NextDouble** | **DiscreteUniform** |    **Standard** | **227.7 ns** | **23.7944 ns** | **70.1582 ns** | **262.2 ns** |       **0 B** |
 |       Next | DiscreteUniform |    Standard | 209.3 ns | 26.5376 ns | 78.2467 ns | 198.8 ns |       0 B |
 | **NextDouble** | **DiscreteUniform** | **XorShift128** | **216.1 ns** | **25.4030 ns** | **74.9014 ns** | **243.1 ns** |       **0 B** |
 |       Next | DiscreteUniform | XorShift128 | 164.7 ns | 18.3713 ns | 54.1682 ns | 126.4 ns |       0 B |
 | **NextDouble** |       **Geometric** |         **ALF** | **276.2 ns** |  **7.8966 ns** | **21.6168 ns** | **287.8 ns** |       **0 B** |
 |       Next |       Geometric |         ALF | 232.5 ns | 23.4024 ns | 69.0024 ns | 271.3 ns |       0 B |
 | **NextDouble** |       **Geometric** |     **MT19937** | **262.1 ns** | **27.7712 ns** | **81.8840 ns** | **310.9 ns** |       **0 B** |
 |       Next |       Geometric |     MT19937 | 266.8 ns | 26.1382 ns | 77.0690 ns | 301.6 ns |       0 B |
 | **NextDouble** |       **Geometric** |         **NR3** | **240.3 ns** | **23.0080 ns** | **67.8395 ns** | **276.8 ns** |       **0 B** |
 |       Next |       Geometric |         NR3 | 212.5 ns | 25.0057 ns | 73.7298 ns | 233.6 ns |       0 B |
 | **NextDouble** |       **Geometric** |       **NR3Q1** | **209.8 ns** | **20.9796 ns** | **61.8590 ns** | **216.2 ns** |       **0 B** |
 |       Next |       Geometric |       NR3Q1 | 208.8 ns | 22.0906 ns | 65.1345 ns | 218.9 ns |       0 B |
 | **NextDouble** |       **Geometric** |       **NR3Q2** | **203.2 ns** | **20.7195 ns** | **61.0919 ns** | **202.0 ns** |       **0 B** |
 |       Next |       Geometric |       NR3Q2 | 120.4 ns |  3.7409 ns |  8.0527 ns | 117.9 ns |       0 B |
 | **NextDouble** |       **Geometric** |    **Standard** | **138.1 ns** |  **0.3606 ns** |  **0.3011 ns** | **138.1 ns** |       **0 B** |
 |       Next |       Geometric |    Standard | 140.4 ns |  6.9079 ns | 17.8316 ns | 134.7 ns |       0 B |
 | **NextDouble** |       **Geometric** | **XorShift128** | **133.2 ns** | **10.3824 ns** | **10.6620 ns** | **130.3 ns** |       **0 B** |
 |       Next |       Geometric | XorShift128 | 251.7 ns | 26.4673 ns | 78.0394 ns | 295.4 ns |       0 B |
 | **NextDouble** |         **Poisson** |         **ALF** | **276.0 ns** | **26.6990 ns** | **78.7225 ns** | **320.4 ns** |       **0 B** |
 |       Next |         Poisson |         ALF | 141.8 ns |  1.4151 ns |  1.1817 ns | 141.4 ns |       0 B |
 | **NextDouble** |         **Poisson** |     **MT19937** | **160.6 ns** |  **0.2389 ns** |  **0.2235 ns** | **160.6 ns** |       **0 B** |
 |       Next |         Poisson |     MT19937 | 182.0 ns | 14.8377 ns | 42.5722 ns | 157.9 ns |       0 B |
 | **NextDouble** |         **Poisson** |         **NR3** | **189.9 ns** | **24.5821 ns** | **72.4807 ns** | **142.0 ns** |       **0 B** |
 |       Next |         Poisson |         NR3 | 139.8 ns |  1.3770 ns |  1.0751 ns | 139.5 ns |       0 B |
 | **NextDouble** |         **Poisson** |       **NR3Q1** | **178.7 ns** | **71.5763 ns** | **73.5036 ns** | **139.5 ns** |       **0 B** |
 |       Next |         Poisson |       NR3Q1 | 140.7 ns |  2.9420 ns |  3.9275 ns | 139.5 ns |       0 B |
 | **NextDouble** |         **Poisson** |       **NR3Q2** | **274.8 ns** | **27.1575 ns** | **80.0746 ns** | **302.3 ns** |       **0 B** |
 |       Next |         Poisson |       NR3Q2 | 265.4 ns | 26.8771 ns | 79.2477 ns | 312.5 ns |       0 B |
 | **NextDouble** |         **Poisson** |    **Standard** | **286.9 ns** | **32.0805 ns** | **94.5900 ns** | **323.0 ns** |       **0 B** |
 |       Next |         Poisson |    Standard | 288.9 ns | 30.4191 ns | 89.6915 ns | 316.8 ns |       0 B |
 | **NextDouble** |         **Poisson** | **XorShift128** | **273.9 ns** | **30.1290 ns** | **88.8361 ns** | **298.3 ns** |       **0 B** |
 |       Next |         Poisson | XorShift128 | 277.5 ns | 29.6678 ns | 87.4762 ns | 312.3 ns |       0 B |

## About this repository and its maintainer

Everything done on this repository is freely offered on the terms of the project license. You are free to do everything you want with the code and its related files, as long as you respect the license and use common sense while doing it :-)

I maintain this project during my spare time, so I can offer limited assistance and I can offer **no kind of warranty**.

However, if this project helps you, then you might offer me an hot cup of coffee:

[![Donate](http://pomma89.altervista.org/buy-me-a-coffee.png)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=ELJWKEYS9QGKA)