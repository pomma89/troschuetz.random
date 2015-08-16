using System;
using Troschuetz.Random.Distributions;
using Troschuetz.Random.Generators;

namespace Troschuetz.Random.Examples
{
    class ExtensibilityExamples
    {
    }

    // Super silly generator which is provided as an example on how one can build a new generator.
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
    class SuperSillyContinuousDistribution<TGen> : AbstractDistribution<TGen>, IContinuousDistribution
        where TGen : IGenerator
    {
        // Just a simple constructor which passes the generator to the base constructor.
        public SuperSillyContinuousDistribution(TGen generator) : base(generator)
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
        public double NextDouble() => TypedGenerator.NextDouble();
    }
}
