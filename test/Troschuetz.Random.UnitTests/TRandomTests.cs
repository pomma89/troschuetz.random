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

namespace Troschuetz.Random.Tests
{
    using Distributions.Continuous;
    using Distributions.Discrete;
    using NUnit.Framework;
    using Random.Generators;
    using System;
    using System.Collections.Generic;

    public sealed class TRandomTests : GeneratorTests
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            _seed = Environment.TickCount;
            _rand = new TRandom(GetCurrentGenerator());
            _otherRand = new TRandom(GetCurrentGenerator());
        }

        [TearDown]
        public void TearDown()
        {
            _currGen = (_currGen + 1) % GeneratorCount;
        }

        #endregion Setup/Teardown

        private const int GeneratorCount = 5;
        private TRandom _rand;
        private TRandom _otherRand;
        private int _currGen;
        private int _seed;

        protected override IGenerator GetGenerator()
        {
            return new TRandom();
        }

        protected override IGenerator GetGenerator(int seed)
        {
            return new TRandom(seed);
        }

        protected override IGenerator GetGenerator(uint seed)
        {
            return new TRandom(seed);
        }

        private IGenerator GetCurrentGenerator()
        {
            switch (_currGen)
            {
                case 0:
                    return new ALFGenerator(_seed);

                case 1:
                    return new MT19937Generator(_seed);

                case 2:
                    return new StandardGenerator(_seed);

                case 3:
                    return new TRandom(_seed);

                case 4:
                    return new XorShift128Generator(_seed);
                // Until GeneratorCount - 1
                default:
                    throw new Exception("Should not be here!!!");
            }
        }

        /*=============================================================================
            Bernoulli distribution
        =============================================================================*/

        [TestCase(0.00)]
        [TestCase(0.25)]
        [TestCase(0.50)]
        [TestCase(0.75)]
        [TestCase(1.00)]
        [Repeat(GeneratorCount)]
        public void Bernoulli_RightParameters(double a)
        {
            var d = new BernoulliDistribution { Alpha = a };
            Assert.True(d.IsValidAlpha(a));
            Assert.AreEqual(d.Alpha, a);
            AssertRightDiscreteOutput(_rand.Bernoulli, d.Alpha, d);
        }

        [TestCase(0.00)]
        [TestCase(0.25)]
        [TestCase(0.50)]
        [TestCase(0.75)]
        [TestCase(1.00)]
        [Repeat(GeneratorCount)]
        public void Bernoulli_SameOutput(double a)
        {
            AssertSameDiscreteOutput(_rand.Bernoulli, _otherRand.BernoulliSamples, a);
        }

        [TestCase(0.00, 0.25)]
        [TestCase(0.25, 0.50)]
        [TestCase(0.50, 0.75)]
        [TestCase(0.75, 1.00)]
        [TestCase(1.00, 0.00)]
        [Repeat(GeneratorCount)]
        public void Bernoulli_SameInterleavedOutput(double a1, double a2)
        {
            AssertSameInterleavedDiscreteOutput(_rand.Bernoulli, _otherRand.BernoulliSamples, a1, a2);
        }

        /*=============================================================================
            Binomial distribution
        =============================================================================*/

        [TestCase(0.00, 1)]
        [TestCase(0.25, 1)]
        [TestCase(0.50, 1)]
        [TestCase(1.00, 1)]
        [TestCase(0.50, 0)]
        [TestCase(0.25, 5)]
        [Repeat(GeneratorCount)]
        public void Binomial_RightParameters(double a, int b)
        {
            var d = new BinomialDistribution { Alpha = a, Beta = b };
            Assert.True(d.IsValidAlpha(a) && d.IsValidBeta(b));
            Assert.AreEqual(d.Alpha, a);
            Assert.AreEqual(d.Beta, b);
            AssertRightDiscreteOutput(_rand.Binomial, d.Alpha, d.Beta, d);
        }

        [TestCase(0.00, 1)]
        [TestCase(0.25, 1)]
        [TestCase(0.50, 1)]
        [TestCase(1.00, 1)]
        [TestCase(0.50, 0)]
        [TestCase(0.25, 5)]
        [Repeat(GeneratorCount)]
        public void Binomial_SameOutput(double a, int b)
        {
            AssertSameDiscreteOutput(_rand.Binomial, _otherRand.BinomialSamples, a, b);
        }

        [TestCase(0.00, 1, 0.25, 2)]
        [TestCase(0.25, 1, 0.50, 2)]
        [TestCase(0.50, 1, 0.75, 2)]
        [TestCase(1.00, 1, 1.00, 1)]
        [TestCase(0.50, 0, 0.00, 1)]
        [TestCase(0.25, 5, 0.75, 2)]
        [Repeat(GeneratorCount)]
        public void Binomial_SameInterleavedOutput(double a1, int b1, double a2, int b2)
        {
            AssertSameInterleavedDiscreteOutput(_rand.Binomial, _otherRand.BinomialSamples, a1, b1, a2, b2);
        }

        /*=============================================================================
            Categorical distribution
        =============================================================================*/

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        [TestCase(8)]
        [TestCase(16)]
        [TestCase(32)]
        [Repeat(GeneratorCount)]
        public void Categorical_ValueCount_RightParameters(int valueCount)
        {
            var d = new CategoricalDistribution(valueCount);
            AssertRightDiscreteOutput(_rand.Categorical, valueCount, d);
        }

        [TestCase(0.1, 0.2, 0.1)]
        [TestCase(0.1, 0.2, 0.1, 0.2, 0.1)]
        [TestCase(0.1, 0.2, 0.3)]
        [TestCase(0.1, 0.2, 0.1, 0.1, 0.5)]
        [Repeat(GeneratorCount)]
        public void Categorical_Weights_RightParameters(params double[] args)
        {
            var d = new CategoricalDistribution { Weights = args };
            Assert.True(d.AreValidWeights(args));
            AssertRightDiscreteOutput(_rand.Categorical, args, d);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        [TestCase(8)]
        [TestCase(16)]
        [TestCase(32)]
        [Repeat(GeneratorCount)]
        public void Categorical_ValueCount_SameOutput(int valueCount)
        {
            AssertSameDiscreteOutput(_rand.Categorical, _otherRand.CategoricalSamples, valueCount);
        }

        [TestCase(0.1, 0.2, 0.1)]
        [TestCase(0.1, 0.2, 0.1, 0.2, 0.1)]
        [TestCase(0.1, 0.2, 0.3)]
        [TestCase(0.1, 0.2, 0.1, 0.1, 0.5)]
        [Repeat(GeneratorCount)]
        public void Categorical_Weights_SameOutput(params double[] args)
        {
            AssertSameDiscreteOutput(_rand.Categorical, _otherRand.CategoricalSamples, args);
        }

        [TestCase(1, 32)]
        [TestCase(2, 16)]
        [TestCase(4, 8)]
        [TestCase(8, 4)]
        [TestCase(16, 2)]
        [TestCase(32, 1)]
        [Repeat(GeneratorCount)]
        public void Categorical_ValueCount_SameInterleavedOutput(int v1, int v2)
        {
            AssertSameInterleavedDiscreteOutput(_rand.Categorical, _otherRand.CategoricalSamples, v1, v2);
        }

        /*=============================================================================
            DiscreteUniform distribution
        =============================================================================*/

        [TestCase(5, 10)]
        [TestCase(10, 20)]
        [TestCase(1, 5)]
        [TestCase(2, 10)]
        [TestCase(3, 15)]
        [Repeat(GeneratorCount)]
        public void DiscreteUniform_RightParameters(int a, int b)
        {
            var d = new DiscreteUniformDistribution { Beta = b, Alpha = a };
            Assert.True(d.IsValidBeta(b) && d.IsValidAlpha(a));
            Assert.AreEqual(d.Alpha, a);
            Assert.AreEqual(d.Beta, b);
            AssertRightDiscreteOutput(_rand.DiscreteUniform, d.Alpha, d.Beta, d);
        }

        [TestCase(5, 10)]
        [TestCase(10, 20)]
        [TestCase(1, 5)]
        [TestCase(2, 10)]
        [TestCase(3, 15)]
        [Repeat(GeneratorCount)]
        public void DiscreteUniform_SameOutput(int a, int b)
        {
            AssertSameDiscreteOutput(_rand.DiscreteUniform, _otherRand.DiscreteUniformSamples, a, b);
        }

        [TestCase(5, 10, 3, 15)]
        [TestCase(10, 20, 2, 10)]
        [TestCase(1, 5, 2, 10)]
        [TestCase(2, 10, 1, 5)]
        [TestCase(3, 15, 2, 10)]
        [Repeat(GeneratorCount)]
        public void DiscreteUniform_SameInterleavedOutput(int a1, int b1, int a2, int b2)
        {
            AssertSameInterleavedDiscreteOutput(_rand.DiscreteUniform, _otherRand.DiscreteUniformSamples, a1, b1, a2, b2);
        }

        /*=============================================================================
            Geometric distribution
        =============================================================================*/

        [TestCase(0.10)]
        [TestCase(0.25)]
        [TestCase(0.50)]
        [TestCase(0.75)]
        [TestCase(1.00)]
        [Repeat(GeneratorCount)]
        public void Geometric_RightParameters(double a)
        {
            var d = new GeometricDistribution { Alpha = a };
            Assert.True(d.IsValidAlpha(a));
            Assert.AreEqual(d.Alpha, a);
            AssertRightDiscreteOutput(_rand.Geometric, d.Alpha, d);
        }

        [TestCase(0.10)]
        [TestCase(0.25)]
        [TestCase(0.50)]
        [TestCase(0.75)]
        [TestCase(1.00)]
        [Repeat(GeneratorCount)]
        public void Geometric_SameOutput(double a)
        {
            AssertSameDiscreteOutput(_rand.Geometric, _otherRand.GeometricSamples, a);
        }

        [TestCase(0.10, 0.25)]
        [TestCase(0.25, 0.50)]
        [TestCase(0.50, 0.75)]
        [TestCase(0.75, 1.00)]
        [TestCase(1.00, 0.10)]
        [Repeat(GeneratorCount)]
        public void Geometric_SameInterleavedOutput(double a1, double a2)
        {
            AssertSameInterleavedDiscreteOutput(_rand.Geometric, _otherRand.GeometricSamples, a1, a2);
        }

        /*=============================================================================
            Poisson distribution
        =============================================================================*/

        [TestCase(0.25)]
        [TestCase(0.50)]
        [TestCase(0.75)]
        [TestCase(1.00)]
        [TestCase(5.00)]
        [Repeat(GeneratorCount)]
        public void Poisson_RightParameters(double l)
        {
            var d = new PoissonDistribution { Lambda = l };
            Assert.True(d.IsValidLambda(l));
            AssertRightDiscreteOutput(_rand.Poisson, d.Lambda, d);
        }

        [TestCase(0.25)]
        [TestCase(0.50)]
        [TestCase(0.75)]
        [TestCase(1.00)]
        [TestCase(5.00)]
        [Repeat(GeneratorCount)]
        public void Poisson_SameOutput(double l)
        {
            AssertSameDiscreteOutput(_rand.Poisson, _otherRand.PoissonSamples, l);
        }

        [TestCase(0.25, 0.10)]
        [TestCase(0.50, 0.25)]
        [TestCase(0.75, 1.00)]
        [TestCase(1.00, 5.00)]
        [TestCase(5.00, 0.10)]
        [Repeat(GeneratorCount)]
        public void Poisson_SameInterleavedOutput(double l1, double l2)
        {
            AssertSameInterleavedDiscreteOutput(_rand.Poisson, _otherRand.PoissonSamples, l1, l2);
        }

        /*=============================================================================
            Beta distribution
        =============================================================================*/

        [TestCase(5, 10)]
        [TestCase(10, 20)]
        [Repeat(GeneratorCount)]
        public void Beta_RightParameters(double a, double b)
        {
            var d = new BetaDistribution { Alpha = a, Beta = b };
            Assert.True(d.IsValidAlpha(a) && d.IsValidBeta(b));
            Assert.AreEqual(d.Alpha, a);
            Assert.AreEqual(d.Beta, b);
            AssertRightContinuousOutput(_rand.Beta, d.Alpha, d.Beta, d);
        }

        [TestCase(5, 10)]
        [TestCase(10, 20)]
        [Repeat(GeneratorCount)]
        public void Beta_SameOutput(double a, double b)
        {
            AssertSameContinuousOutput(_rand.Beta, _otherRand.BetaSamples, a, b);
        }

        [TestCase(5, 10, 15, 20)]
        [TestCase(1, 2, 3, 4)]
        [Repeat(GeneratorCount)]
        public void Beta_SameInterleavedOutput(double a1, double b1, double a2, double b2)
        {
            AssertSameInterleavedContinuousOutput(_rand.Beta, _otherRand.BetaSamples, a1, b1, a2, b2);
        }

        /*=============================================================================
            BetaPrime distribution
        =============================================================================*/

        [TestCase(5, 10)]
        [TestCase(10, 20)]
        [Repeat(GeneratorCount)]
        public void BetaPrime_RightParameters(double a, double b)
        {
            var d = new BetaPrimeDistribution { Alpha = a, Beta = b };
            Assert.True(d.IsValidAlpha(a) && d.IsValidBeta(b));
            Assert.AreEqual(d.Alpha, a);
            Assert.AreEqual(d.Beta, b);
            AssertRightContinuousOutput(_rand.BetaPrime, d.Alpha, d.Beta, d);
        }

        [TestCase(5, 10)]
        [TestCase(10, 20)]
        [Repeat(GeneratorCount)]
        public void BetaPrime_SameOutput(double a, double b)
        {
            AssertSameContinuousOutput(_rand.BetaPrime, _otherRand.BetaPrimeSamples, a, b);
        }

        [TestCase(5, 10, 15, 20)]
        [TestCase(2, 3, 4, 5)]
        [Repeat(GeneratorCount)]
        public void BetaPrime_SameInterleavedOutput(double a1, double b1, double a2, double b2)
        {
            AssertSameInterleavedContinuousOutput(_rand.BetaPrime, _otherRand.BetaPrimeSamples, a1, b1, a2, b2);
        }

        /*=============================================================================
            Cauchy distribution
        =============================================================================*/

        [TestCase(5, 10)]
        [TestCase(10, 20)]
        [Repeat(GeneratorCount)]
        public void Cauchy_RightParameters(double a, double g)
        {
            var d = new CauchyDistribution { Alpha = a, Gamma = g };
            Assert.True(d.IsValidAlpha(a) && d.IsValidGamma(g));
            Assert.AreEqual(d.Alpha, a);
            Assert.AreEqual(d.Gamma, g);
            AssertRightContinuousOutput(_rand.Cauchy, d.Alpha, d.Gamma, d);
        }

        [TestCase(5, 10)]
        [TestCase(10, 20)]
        [Repeat(GeneratorCount)]
        public void Cauchy_SameOutput(double a, double g)
        {
            AssertSameContinuousOutput(_rand.Cauchy, _otherRand.CauchySamples, a, g);
        }

        [TestCase(5, 10, 15, 20)]
        [TestCase(1, 2, 3, 4)]
        [Repeat(GeneratorCount)]
        public void Cauchy_SameInterleavedOutput(double a1, double g1, double a2, double g2)
        {
            AssertSameInterleavedContinuousOutput(_rand.Cauchy, _otherRand.CauchySamples, a1, g1, a2, g2);
        }

        /*=============================================================================
            Chi distribution
        =============================================================================*/

        [TestCase(1)]
        [TestCase(2)]
        [Repeat(GeneratorCount)]
        public void Chi_RightParameters(int a)
        {
            var d = new ChiDistribution { Alpha = a };
            Assert.True(d.IsValidAlpha(a));
            Assert.AreEqual(d.Alpha, a);
            AssertRightContinuousOutput(_rand.Chi, d.Alpha, d);
        }

        [TestCase(1)]
        [TestCase(2)]
        [Repeat(GeneratorCount)]
        public void Chi_SameOutput(int a)
        {
            AssertSameContinuousOutput(_rand.Chi, _otherRand.ChiSamples, a);
        }

        [TestCase(1, 2)]
        [TestCase(2, 4)]
        [Repeat(GeneratorCount)]
        public void Chi_SameInterleavedOutput(int a1, int a2)
        {
            AssertSameInterleavedContinuousOutput(_rand.Chi, _otherRand.ChiSamples, a1, a2);
        }

        /*=============================================================================
            ChiSquare distribution
        =============================================================================*/

        [TestCase(1)]
        [TestCase(5)]
        [TestCase(10)]
        [Repeat(GeneratorCount)]
        public void ChiSquare_RightParameters(int a)
        {
            var d = new ChiSquareDistribution { Alpha = a };
            Assert.True(d.IsValidAlpha(a));
            Assert.AreEqual(d.Alpha, a);
            AssertRightContinuousOutput(_rand.ChiSquare, d.Alpha, d);
        }

        [TestCase(1)]
        [TestCase(5)]
        [TestCase(10)]
        [Repeat(GeneratorCount)]
        public void ChiSquare_SameOutput(int a)
        {
            AssertSameContinuousOutput(_rand.ChiSquare, _otherRand.ChiSquareSamples, a);
        }

        [TestCase(1, 5)]
        [TestCase(5, 10)]
        [TestCase(10, 1)]
        [Repeat(GeneratorCount)]
        public void ChiSquare_SameInterleavedOutput(int a1, int a2)
        {
            AssertSameInterleavedContinuousOutput(_rand.ChiSquare, _otherRand.ChiSquareSamples, a1, a2);
        }

        /*=============================================================================
            ContinuousUniform distribution
        =============================================================================*/

        [TestCase(5, 10)]
        [TestCase(10, 20)]
        [Repeat(GeneratorCount)]
        public void ContinuousUniform_RightParameters(double a, double b)
        {
            var d = new ContinuousUniformDistribution { Beta = b, Alpha = a };
            Assert.True(d.IsValidAlpha(a) && d.IsValidBeta(b));
            Assert.AreEqual(d.Alpha, a);
            Assert.AreEqual(d.Beta, b);
            AssertRightContinuousOutput(_rand.ContinuousUniform, d.Alpha, d.Beta, d);
        }

        [TestCase(5, 10)]
        [TestCase(10, 20)]
        [Repeat(GeneratorCount)]
        public void ContinuousUniform_SameOutput(double a, double b)
        {
            AssertSameContinuousOutput(_rand.ContinuousUniform, _otherRand.ContinuousUniformSamples, a, b);
        }

        [TestCase(5, 10, 15, 20)]
        [TestCase(1, 2, 3, 4)]
        [Repeat(GeneratorCount)]
        public void ContinuousUniform_SameInterleavedOutput(double a1, double b1, double a2, double b2)
        {
            AssertSameInterleavedContinuousOutput(_rand.ContinuousUniform, _otherRand.ContinuousUniformSamples, a1, b1, a2,
                                                  b2);
        }

        /*=============================================================================
            Erlang distribution
        =============================================================================*/

        [TestCase(5, 0.5)]
        [TestCase(500, 50)]
        [TestCase(1, 2)]
        [TestCase(2, 2)]
        [TestCase(10, 0.5)]
        [Repeat(GeneratorCount)]
        public void Erlang_RightParameters(int a, double l)
        {
            var d = new ErlangDistribution { Alpha = a, Lambda = l };
            Assert.True(d.IsValidAlpha(a) && d.IsValidLambda(l));
            Assert.AreEqual(d.Alpha, a);
            Assert.AreEqual(d.Lambda, l);
            AssertRightContinuousOutput(_rand.Erlang, d.Alpha, d.Lambda, d);
        }

        [TestCase(5, 0.5)]
        [TestCase(500, 50)]
        [TestCase(1, 2)]
        [TestCase(2, 2)]
        [TestCase(10, 0.5)]
        [Repeat(GeneratorCount)]
        public void Erlang_SameOutput(int a, double l)
        {
            AssertSameContinuousOutput(_rand.Erlang, _otherRand.ErlangSamples, a, l);
        }

        [TestCase(5, 0.5, 500, 50)]
        [TestCase(500, 50, 1, 2)]
        [Repeat(GeneratorCount)]
        public void Erlang_SameInterleavedOutput(int a1, double l1, int a2, double l2)
        {
            AssertSameInterleavedContinuousOutput(_rand.Erlang, _otherRand.ErlangSamples, a1, l1, a2, l2);
        }

        /*=============================================================================
            Exponential distribution
        =============================================================================*/

        [TestCase(5)]
        [TestCase(500)]
        [Repeat(GeneratorCount)]
        public void Exponential_RightParameters(double l)
        {
            var d = new ExponentialDistribution { Lambda = l };
            Assert.True(d.IsValidLambda(l));
            Assert.AreEqual(d.Lambda, l);
            AssertRightContinuousOutput(_rand.Exponential, d.Lambda, d);
        }

        [TestCase(5)]
        [TestCase(500)]
        [Repeat(GeneratorCount)]
        public void Exponential_SameOutput(double l)
        {
            AssertSameContinuousOutput(_rand.Exponential, _otherRand.ExponentialSamples, l);
        }

        [TestCase(5)]
        [TestCase(500)]
        [Repeat(GeneratorCount)]
        public void Exponential_SameOutputForDist(double l)
        {
            var d = new ExponentialDistribution(GetCurrentGenerator()) { Lambda = l };
            Assert.True(d.IsValidLambda(l));
            Assert.AreEqual(d.Lambda, l);
            AssertSameContinuousOutputForDist(_rand.Exponential, d, l);
        }

        [TestCase(5, 500)]
        [TestCase(500, 1)]
        [Repeat(GeneratorCount)]
        public void Exponential_SameInterleavedOutput(double l1, double l2)
        {
            AssertSameInterleavedContinuousOutput(_rand.Exponential, _otherRand.ExponentialSamples, l1, l2);
        }

        /*=============================================================================
            FisherSnedecor distribution
        =============================================================================*/

        [TestCase(5, 10)]
        [TestCase(10, 20)]
        [Repeat(GeneratorCount)]
        public void FisherSnedecor_RightParameters(int a, int b)
        {
            var d = new FisherSnedecorDistribution { Alpha = a, Beta = b };
            Assert.True(d.IsValidAlpha(a) && d.IsValidBeta(b));
            Assert.AreEqual(d.Alpha, a);
            Assert.AreEqual(d.Beta, b);
            AssertRightContinuousOutput(_rand.FisherSnedecor, d.Alpha, d.Beta, d);
        }

        [TestCase(5, 10)]
        [TestCase(10, 20)]
        [Repeat(GeneratorCount)]
        public void FisherSnedecor_SameOutput(int a, int b)
        {
            AssertSameContinuousOutput(_rand.FisherSnedecor, _otherRand.FisherSnedecorSamples, a, b);
        }

        [TestCase(5, 10, 15, 20)]
        [TestCase(1, 2, 3, 4)]
        [Repeat(GeneratorCount)]
        public void FisherSnedecor_SameInterleavedOutput(int a1, int b1, int a2, int b2)
        {
            AssertSameInterleavedContinuousOutput(_rand.FisherSnedecor, _otherRand.FisherSnedecorSamples, a1, b1, a2, b2);
        }

        /*=============================================================================
            FisherTippett distribution
        =============================================================================*/

        [TestCase(5, 0.5)]
        [TestCase(10, 1)]
        [Repeat(GeneratorCount)]
        public void FisherTippett_RightParameters(double a, double m)
        {
            var d = new FisherTippettDistribution { Alpha = a, Mu = m };
            Assert.True(d.IsValidAlpha(a) && d.IsValidMu(m));
            Assert.AreEqual(d.Alpha, a);
            Assert.AreEqual(d.Mu, m);
            AssertRightContinuousOutput(_rand.FisherTippett, d.Alpha, d.Mu, d);
        }

        [TestCase(5, 0.5)]
        [TestCase(10, 1)]
        [Repeat(GeneratorCount)]
        public void FisherTippett_SameOutput(double a, double m)
        {
            AssertSameContinuousOutput(_rand.FisherTippett, _otherRand.FisherTippettSamples, a, m);
        }

        [TestCase(5, 0.5, 10, 1)]
        [TestCase(10, 1, 20, 2)]
        [Repeat(GeneratorCount)]
        public void FisherTippett_SameInterleavedOutput(double a1, double m1, double a2, double m2)
        {
            AssertSameInterleavedContinuousOutput(_rand.FisherTippett, _otherRand.FisherTippettSamples, a1, m1, a2, m2);
        }

        /*=============================================================================
            Gamma distribution
        =============================================================================*/

        [TestCase(5, 0.5)]
        [TestCase(10, 1)]
        [Repeat(GeneratorCount)]
        public void Gamma_RightParameters(double a, double b)
        {
            var d = new GammaDistribution { Alpha = a, Beta = b };
            Assert.True(d.IsValidAlpha(a) && d.IsValidBeta(b));
            Assert.AreEqual(d.Alpha, a);
            Assert.AreEqual(d.Beta, b);
            AssertRightContinuousOutput(_rand.Gamma, d.Alpha, d.Beta, d);
        }

        [TestCase(5, 0.5)]
        [TestCase(10, 1)]
        [Repeat(GeneratorCount)]
        public void Gamma_SameOutput(double a, double t)
        {
            AssertSameContinuousOutput(_rand.Gamma, _otherRand.GammaSamples, a, t);
        }

        [TestCase(5, 0.5, 10, 1)]
        [TestCase(10, 1, 20, 2)]
        [Repeat(GeneratorCount)]
        public void Gamma_SameInterleavedOutput(double a1, double t1, double a2, double t2)
        {
            AssertSameInterleavedContinuousOutput(_rand.Gamma, _otherRand.GammaSamples, a1, t1, a2, t2);
        }

        /*=============================================================================
            Laplace distribution
        =============================================================================*/

        [TestCase(5, 0.5)]
        [TestCase(10, 1)]
        [Repeat(GeneratorCount)]
        public void Laplace_RightParameters(double a, double m)
        {
            var d = new LaplaceDistribution { Alpha = a, Mu = m };
            Assert.True(d.IsValidAlpha(a) && d.IsValidMu(m));
            Assert.AreEqual(d.Alpha, a);
            Assert.AreEqual(d.Mu, m);
            AssertRightContinuousOutput(_rand.Laplace, d.Alpha, d.Mu, d);
        }

        [TestCase(5, 0.5)]
        [TestCase(10, 1)]
        [Repeat(GeneratorCount)]
        public void Laplace_SameOutput(double a, double m)
        {
            AssertSameContinuousOutput(_rand.Laplace, _otherRand.LaplaceSamples, a, m);
        }

        [TestCase(5, 0.5, 10, 1)]
        [TestCase(10, 1, 20, 2)]
        [Repeat(GeneratorCount)]
        public void Laplace_SameInterleavedOutput(double a1, double m1, double a2, double m2)
        {
            AssertSameInterleavedContinuousOutput(_rand.Laplace, _otherRand.LaplaceSamples, a1, m1, a2, m2);
        }

        /*=============================================================================
            Logistic distribution
        =============================================================================*/

        [TestCase(5, 0.5)]
        [TestCase(10, 1)]
        [Repeat(GeneratorCount)]
        public void Logistic_RightParameters(double m, double s)
        {
            var d = new LogisticDistribution { Mu = m, Sigma = s };
            Assert.True(d.IsValidMu(m) && d.IsValidSigma(s));
            Assert.AreEqual(d.Mu, m);
            Assert.AreEqual(d.Sigma, s);
            AssertRightContinuousOutput(_rand.Logistic, d.Mu, d.Sigma, d);
        }

        [TestCase(5, 0.5)]
        [TestCase(10, 1)]
        [Repeat(GeneratorCount)]
        public void Logistic_SameOutput(double m, double s)
        {
            AssertSameContinuousOutput(_rand.Logistic, _otherRand.LogisticSamples, m, s);
        }

        [TestCase(5, 0.5, 10, 1)]
        [TestCase(10, 1, 20, 2)]
        [Repeat(GeneratorCount)]
        public void Logistic_SameInterleavedOutput(double m1, double s1, double m2, double s2)
        {
            AssertSameInterleavedContinuousOutput(_rand.Logistic, _otherRand.LogisticSamples, m1, s1, m2, s2);
        }

        /*=============================================================================
            Lognormal distribution
        =============================================================================*/

        [TestCase(5, 0.5)]
        [TestCase(10, 1)]
        [Repeat(GeneratorCount)]
        public void Lognormal_RightParameters(double m, double s)
        {
            var d = new LognormalDistribution { Mu = m, Sigma = s };
            Assert.True(d.IsValidMu(m) && d.IsValidSigma(s));
            Assert.AreEqual(d.Mu, m);
            Assert.AreEqual(d.Sigma, s);
            AssertRightContinuousOutput(_rand.Lognormal, d.Mu, d.Sigma, d);
        }

        [TestCase(5, 0.5)]
        [TestCase(10, 1)]
        [Repeat(GeneratorCount)]
        public void Lognormal_SameOutput(double m, double s)
        {
            AssertSameContinuousOutput(_rand.Lognormal, _otherRand.LognormalSamples, m, s);
        }

        [TestCase(5, 0.5, 10, 1)]
        [TestCase(10, 1, 20, 2)]
        [Repeat(GeneratorCount)]
        public void Lognormal_SameInterleavedOutput(double m1, double s1, double m2, double s2)
        {
            AssertSameInterleavedContinuousOutput(_rand.Lognormal, _otherRand.LognormalSamples, m1, s1, m2, s2);
        }

        /*=============================================================================
            Normal distribution
        =============================================================================*/

        [TestCase(5, 0.5)]
        [TestCase(500, 50)]
        [Repeat(GeneratorCount)]
        public void Normal_RightParameters(double m, double s)
        {
            var d = new NormalDistribution { Mu = m, Sigma = s };
            Assert.True(d.IsValidMu(m) && d.IsValidSigma(s));
            Assert.AreEqual(d.Mu, m);
            Assert.AreEqual(d.Sigma, s);
            AssertRightContinuousOutput(_rand.Normal, d.Mu, d.Sigma, d);
        }

        [TestCase(5.0, 0.10)]
        [TestCase(10.0, 0.20)]
        [TestCase(1.0, 0.50)]
        [TestCase(2.0, 0.10)]
        [TestCase(3.0, 0.15)]
        [Repeat(GeneratorCount)]
        public void Normal_SameOutput(double m, double s)
        {
            AssertSameContinuousOutput(_rand.Normal, _otherRand.NormalSamples, m, s);
        }

        [TestCase(5.0, 0.10)]
        [TestCase(10.0, 0.20)]
        [TestCase(1.0, 0.50)]
        [TestCase(2.0, 0.10)]
        [TestCase(3.0, 0.15)]
        [Repeat(GeneratorCount)]
        public void Normal_SameOutputForDist(double m, double s)
        {
            var d = new NormalDistribution(GetCurrentGenerator()) { Mu = m, Sigma = s };
            Assert.True(d.IsValidMu(m) && d.IsValidSigma(s));
            Assert.AreEqual(d.Mu, m);
            Assert.AreEqual(d.Sigma, s);
            AssertSameContinuousOutput(_rand.Normal, d, m, s);
        }

        [TestCase(5.0, 0.10, 10.0, 0.20)]
        [TestCase(1.0, 0.50, 2.0, 0.10)]
        [Repeat(GeneratorCount)]
        public void Normal_SameInterleavedOutput(double m1, double s1, double m2, double s2)
        {
            AssertSameInterleavedContinuousOutput(_rand.Normal, _otherRand.NormalSamples, m1, s1, m2, s2);
        }

        /*=============================================================================
            Pareto distribution
        =============================================================================*/

        [TestCase(5, 0.5)]
        [TestCase(10, 1)]
        [Repeat(GeneratorCount)]
        public void Pareto_RightParameters(double a, double b)
        {
            var d = new ParetoDistribution { Alpha = a, Beta = b };
            Assert.True(d.IsValidAlpha(a) && d.IsValidBeta(b));
            Assert.AreEqual(d.Alpha, a);
            Assert.AreEqual(d.Beta, b);
            AssertRightContinuousOutput(_rand.Pareto, d.Alpha, d.Beta, d);
        }

        [TestCase(5, 0.5)]
        [TestCase(10, 1)]
        [Repeat(GeneratorCount)]
        public void Pareto_SameOutput(double a, double b)
        {
            AssertSameContinuousOutput(_rand.Pareto, _otherRand.ParetoSamples, a, b);
        }

        [TestCase(5, 0.5, 10, 1)]
        [TestCase(10, 1, 20, 2)]
        [Repeat(GeneratorCount)]
        public void Pareto_SameInterleavedOutput(double a1, double b1, double a2, double b2)
        {
            AssertSameInterleavedContinuousOutput(_rand.Pareto, _otherRand.ParetoSamples, a1, b1, a2, b2);
        }

        /*=============================================================================
            Power distribution
        =============================================================================*/

        [TestCase(5, 0.5)]
        [TestCase(10, 1)]
        [Repeat(GeneratorCount)]
        public void Power_RightParameters(double a, double b)
        {
            var d = new PowerDistribution { Alpha = a, Beta = b };
            Assert.True(d.IsValidAlpha(a) && d.IsValidBeta(b));
            Assert.AreEqual(d.Alpha, a);
            Assert.AreEqual(d.Beta, b);
            AssertRightContinuousOutput(_rand.Power, d.Alpha, d.Beta, d);
        }

        [TestCase(5, 0.5)]
        [TestCase(10, 1)]
        [Repeat(GeneratorCount)]
        public void Power_SameOutput(double a, double b)
        {
            AssertSameContinuousOutput(_rand.Power, _otherRand.PowerSamples, a, b);
        }

        [TestCase(5, 0.5, 10, 1)]
        [TestCase(10, 1, 20, 2)]
        [Repeat(GeneratorCount)]
        public void Power_SameInterleavedOutput(double a1, double b1, double a2, double b2)
        {
            AssertSameInterleavedContinuousOutput(_rand.Power, _otherRand.PowerSamples, a1, b1, a2, b2);
        }

        /*=============================================================================
            Rayleigh distribution
        =============================================================================*/

        [TestCase(0.5)]
        [TestCase(5)]
        [TestCase(50)]
        [Repeat(GeneratorCount)]
        public void Rayleigh_RightParameters(double s)
        {
            var d = new RayleighDistribution { Sigma = s };
            Assert.True(d.IsValidSigma(s));
            Assert.AreEqual(d.Sigma, s);
            AssertRightContinuousOutput(_rand.Rayleigh, d.Sigma, d);
        }

        [TestCase(0.5)]
        [TestCase(5)]
        [TestCase(50)]
        [Repeat(GeneratorCount)]
        public void Rayleigh_SameOutput(double s)
        {
            AssertSameContinuousOutput(_rand.Rayleigh, _otherRand.RayleighSamples, s);
        }

        [TestCase(0.5, 5)]
        [TestCase(5, 50)]
        [Repeat(GeneratorCount)]
        public void Rayleigh_SameInterleavedOutput(double s1, double s2)
        {
            AssertSameInterleavedContinuousOutput(_rand.Rayleigh, _otherRand.RayleighSamples, s1, s2);
        }

        /*=============================================================================
            StudentsT distribution
        =============================================================================*/

        [TestCase(1)]
        [TestCase(10)]
        [TestCase(100)]
        [Repeat(GeneratorCount)]
        public void StudentsT_RightParameters(int n)
        {
            var d = new StudentsTDistribution { Nu = n };
            Assert.True(d.IsValidNu(n));
            Assert.AreEqual(d.Nu, n);
            AssertRightContinuousOutput(_rand.StudentsT, d.Nu = n, d);
        }

        [TestCase(1)]
        [TestCase(10)]
        [TestCase(100)]
        [Repeat(GeneratorCount)]
        public void StudentsT_SameOutput(int n)
        {
            AssertSameContinuousOutput(_rand.StudentsT, _otherRand.StudentsTSamples, n);
        }

        [TestCase(1, 10)]
        [TestCase(10, 100)]
        [Repeat(GeneratorCount)]
        public void StudentsT_SameInterleavedOutput(int n1, int n2)
        {
            AssertSameInterleavedContinuousOutput(_rand.StudentsT, _otherRand.StudentsTSamples, n1, n2);
        }

        /*=============================================================================
            Triangular distribution
        =============================================================================*/

        [TestCase(1, 3, 2)]
        [TestCase(4, 6, 5)]
        [Repeat(GeneratorCount)]
        public void Triangular_RightParameters(double a, double b, double g)
        {
            var d = new TriangularDistribution { Beta = b, Gamma = g, Alpha = a };
            Assert.True(d.IsValidAlpha(a) && d.IsValidBeta(b) && d.IsValidGamma(g));
            Assert.AreEqual(d.Alpha, a);
            Assert.AreEqual(d.Beta, b);
            Assert.AreEqual(d.Gamma, g);
            AssertRightContinuousOutput(_rand.Triangular, d.Alpha, d.Beta, d.Gamma, d);
        }

        [TestCase(1, 3, 2)]
        [TestCase(4, 6, 5)]
        [Repeat(GeneratorCount)]
        public void Triangular_SameOutput(double a, double b, double g)
        {
            AssertSameContinuousOutput(_rand.Triangular, _otherRand.TriangularSamples, a, b, g);
        }

        [TestCase(1, 3, 2, 4, 6, 5)]
        [TestCase(4, 6, 5, 7, 9, 8)]
        [Repeat(GeneratorCount)]
        public void Triangular_SameInterleavedOutput(double a1, double b1, double g1, double a2, double b2, double g2)
        {
            AssertSameInterleavedContinuousOutput(_rand.Triangular, _otherRand.TriangularSamples, a1, b1, g1, a2, b2, g2);
        }

        /*=============================================================================
            Weibull distribution
        =============================================================================*/

        [TestCase(5, 0.5)]
        [TestCase(500, 50)]
        [Repeat(GeneratorCount)]
        public void Weibull_RightParameters(double a, double l)
        {
            var d = new WeibullDistribution { Alpha = a, Lambda = l };
            Assert.True(d.IsValidAlpha(a) && d.IsValidLambda(l));
            Assert.AreEqual(d.Alpha, a);
            Assert.AreEqual(d.Lambda, l);
            AssertRightContinuousOutput(_rand.Weibull, d.Alpha, d.Lambda, d);
        }

        [TestCase(5, 0.5)]
        [TestCase(500, 50)]
        [Repeat(GeneratorCount)]
        public void Weibull_SameOutput(double a, double l)
        {
            AssertSameContinuousOutput(_rand.Weibull, _otherRand.WeibullSamples, a, l);
        }

        [TestCase(5, 0.5, 500, 50)]
        [TestCase(500, 50, 2, 1)]
        [Repeat(GeneratorCount)]
        public void Weibull_SameInterleavedOutput(double a1, double l1, double a2, double l2)
        {
            AssertSameInterleavedContinuousOutput(_rand.Weibull, _otherRand.WeibullSamples, a1, l1, a2, l2);
        }

        /*=============================================================================
            Reset
        =============================================================================*/

        [Test]
        [Repeat(GeneratorCount)]
        public void Reset_SameSequence()
        {
            if (!_rand.CanReset) Assert.Pass();
            for (var i = 0; i < Iterations / 2; ++i)
                Results[i] = _rand.Exponential(1);
            for (var i = Iterations / 2; i < Iterations; ++i)
                Results[i] = _rand.Normal(1, 0.1);
            _rand.Reset();
            for (var i = 0; i < Iterations / 2; ++i)
                Assert.AreEqual(Results[i], _rand.Exponential(1));
            for (var i = Iterations / 2; i < Iterations; ++i)
                Assert.AreEqual(Results[i], _rand.Normal(1, 0.1));
        }

        /*=============================================================================
            Asserts for discrete distributions
        =============================================================================*/

        private void AssertRightDiscreteOutput<T>(Func<T, int> gen, T arg, IDistribution dist)
        {
            for (var i = 0; i < Iterations; ++i) Results[i] = gen(arg);
            AssertDist(dist);
        }

        private void AssertRightDiscreteOutput<T1, T2>(Func<T1, T2, int> gen, T1 arg1, T2 arg2, IDistribution dist)
        {
            for (var i = 0; i < Iterations; ++i) Results[i] = gen(arg1, arg2);
            AssertDist(dist);
        }

        private static void AssertSameDiscreteOutput<T>(Func<T, int> gen, Func<T, IEnumerable<int>> infGen, T arg)
        {
            var infGenEn = infGen?.Invoke(arg).GetEnumerator();
            for (var i = 0; i < Iterations; ++i)
            {
                infGenEn.MoveNext();
                Assert.AreEqual(gen(arg), infGenEn.Current);
            }
        }

        private static void AssertSameDiscreteOutput<T1, T2>(Func<T1, T2, int> gen,
                                                     Func<T1, T2, IEnumerable<int>> infGen,
                                                     T1 arg1, T2 arg2)
        {
            var infGenEn = infGen?.Invoke(arg1, arg2).GetEnumerator();
            for (var i = 0; i < Iterations; ++i)
            {
                infGenEn.MoveNext();
                Assert.AreEqual(gen(arg1, arg2), infGenEn.Current);
            }
        }

        private static void AssertSameInterleavedDiscreteOutput<T>(Func<T, int> gen,
                                                           Func<T, IEnumerable<int>> infGen,
                                                           T a, T b)
        {
            var aEn = infGen?.Invoke(a).GetEnumerator();
            var bEn = infGen?.Invoke(b).GetEnumerator();
            for (var i = 0; i < Iterations; ++i)
            {
                if (i % 2 == 0)
                {
                    aEn.MoveNext();
                    Assert.AreEqual(gen(a), aEn.Current);
                }
                else
                {
                    bEn.MoveNext();
                    Assert.AreEqual(gen(b), bEn.Current);
                }
            }
        }

        private static void AssertSameInterleavedDiscreteOutput<T1, T2>(Func<T1, T2, int> gen,
                                                                Func<T1, T2, IEnumerable<int>> infGen,
                                                                T1 a1, T2 a2, T1 b1, T2 b2)
        {
            var aEn = infGen?.Invoke(a1, a2).GetEnumerator();
            var bEn = infGen?.Invoke(b1, b2).GetEnumerator();
            for (var i = 0; i < Iterations; ++i)
            {
                if (i % 2 == 0)
                {
                    aEn.MoveNext();
                    Assert.AreEqual(gen(a1, a2), aEn.Current);
                }
                else
                {
                    bEn.MoveNext();
                    Assert.AreEqual(gen(b1, b2), bEn.Current);
                }
            }
        }

        /*=============================================================================
            Asserts for continuous distributions
        =============================================================================*/

        private void AssertRightContinuousOutput<T>(Func<T, double> gen, T arg, IDistribution dist)
        {
            for (var i = 0; i < Iterations; ++i) Results[i] = gen(arg);
            AssertDist(dist);
        }

        private void AssertRightContinuousOutput<T1, T2>(Func<T1, T2, double> gen, T1 arg1, T2 arg2, IDistribution dist)
        {
            for (var i = 0; i < Iterations; ++i) Results[i] = gen(arg1, arg2);
            AssertDist(dist);
        }

        private void AssertRightContinuousOutput<T1, T2, T3>(Func<T1, T2, T3, double> gen, T1 arg1, T2 arg2, T3 arg3,
                                                     IDistribution dist)
        {
            for (var i = 0; i < Iterations; ++i) Results[i] = gen(arg1, arg2, arg3);
            AssertDist(dist);
        }

        private static void AssertSameContinuousOutputForDist<T>(Func<T, double> gen, IContinuousDistribution dist, T arg)
        {
            for (var i = 0; i < Iterations; ++i)
            {
                Assert.AreEqual(gen(arg), dist.NextDouble());
            }
        }

        private static void AssertSameContinuousOutput<T1, T2>(Func<T1, T2, double> gen, IContinuousDistribution dist,
                                                       T1 arg1, T2 arg2)
        {
            for (var i = 0; i < Iterations; ++i)
            {
                Assert.AreEqual(gen(arg1, arg2), dist.NextDouble());
            }
        }

        private static void AssertSameContinuousOutput<T>(Func<T, double> gen, Func<T, IEnumerable<double>> infGen, T arg)
        {
            var infGenEn = infGen?.Invoke(arg).GetEnumerator();
            for (var i = 0; i < Iterations; ++i)
            {
                infGenEn.MoveNext();
                Assert.AreEqual(gen(arg), infGenEn.Current);
            }
        }

        private static void AssertSameContinuousOutput<T1, T2>(Func<T1, T2, double> gen,
                                                       Func<T1, T2, IEnumerable<double>> infGen,
                                                       T1 arg1, T2 arg2)
        {
            var infGenEn = infGen?.Invoke(arg1, arg2).GetEnumerator();
            for (var i = 0; i < Iterations; ++i)
            {
                infGenEn.MoveNext();
                Assert.AreEqual(gen(arg1, arg2), infGenEn.Current);
            }
        }

        private static void AssertSameContinuousOutput<T1, T2, T3>(Func<T1, T2, T3, double> gen,
                                                           Func<T1, T2, T3, IEnumerable<double>> infGen,
                                                           T1 arg1, T2 arg2, T3 arg3)
        {
            var infGenEn = infGen?.Invoke(arg1, arg2, arg3).GetEnumerator();
            for (var i = 0; i < Iterations; ++i)
            {
                infGenEn.MoveNext();
                Assert.AreEqual(gen(arg1, arg2, arg3), infGenEn.Current);
            }
        }

        private static void AssertSameInterleavedContinuousOutput<T>(Func<T, double> gen,
                                                             Func<T, IEnumerable<double>> infGen,
                                                             T a, T b)
        {
            var aEn = infGen?.Invoke(a).GetEnumerator();
            var bEn = infGen?.Invoke(b).GetEnumerator();
            for (var i = 0; i < Iterations; ++i)
            {
                if (i % 2 == 0)
                {
                    aEn.MoveNext();
                    Assert.AreEqual(gen(a), aEn.Current);
                }
                else
                {
                    bEn.MoveNext();
                    Assert.AreEqual(gen(b), bEn.Current);
                }
            }
        }

        private static void AssertSameInterleavedContinuousOutput<T1, T2>(Func<T1, T2, double> gen,
                                                                  Func<T1, T2, IEnumerable<double>> infGen,
                                                                  T1 a1, T2 a2, T1 b1, T2 b2)
        {
            var aEn = infGen?.Invoke(a1, a2).GetEnumerator();
            var bEn = infGen?.Invoke(b1, b2).GetEnumerator();
            for (var i = 0; i < Iterations; ++i)
            {
                if (i % 2 == 0)
                {
                    aEn.MoveNext();
                    Assert.AreEqual(gen(a1, a2), aEn.Current);
                }
                else
                {
                    bEn.MoveNext();
                    Assert.AreEqual(gen(b1, b2), bEn.Current);
                }
            }
        }

        private static void AssertSameInterleavedContinuousOutput<T1, T2, T3>(Func<T1, T2, T3, double> gen,
                                                                      Func<T1, T2, T3, IEnumerable<double>> infGen,
                                                                      T1 a1, T2 a2, T3 a3, T1 b1, T2 b2, T3 b3)
        {
            var aEn = infGen?.Invoke(a1, a2, a3).GetEnumerator();
            var bEn = infGen?.Invoke(b1, b2, b3).GetEnumerator();
            for (var i = 0; i < Iterations; ++i)
            {
                if (i % 2 == 0)
                {
                    aEn.MoveNext();
                    Assert.AreEqual(gen(a1, a2, a3), aEn.Current);
                }
                else
                {
                    bEn.MoveNext();
                    Assert.AreEqual(gen(b1, b2, b3), bEn.Current);
                }
            }
        }

        [Test]
        public void Bernoulli_LargeGreaterThanOneAlpha()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _rand.Bernoulli(LargePos + 1);
            });
        }

        [Test]
        public void Bernoulli_LargeNegativeAlpha()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _rand.Bernoulli(LargeNeg);
            });
        }

        [Test]
        public void Bernoulli_NaN_Alpha()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _rand.Bernoulli(double.NaN);
            });
        }

        [Test]
        public void Bernoulli_SmallGreaterThanOneAlpha()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _rand.Bernoulli(SmallPos + 1);
            });
        }

        [Test]
        public void Bernoulli_SmallNegativeAlpha()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _rand.Bernoulli(SmallNeg);
            });
        }

        [Test]
        public void Bernoulli_TinyGreaterThanOneAlpha()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _rand.Bernoulli(TinyPos + 1);
            });
        }

        [Test]
        public void Bernoulli_TinyNegativeAlpha()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _rand.Bernoulli(TinyNeg);
            });
        }

        [Test]
        public void Binomial_LargeGreaterThanOneAlpha()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _rand.Binomial(LargePos + 1, 1);
            });
        }

        [Test]
        public void Binomial_LargeNegativeAlpha()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _rand.Binomial(LargeNeg, 1);
            });
        }

        [Test]
        public void Binomial_LargeNegativeBeta()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _rand.Binomial(0.5, (int) LargeNeg);
            });
        }

        [Test]
        public void Binomial_SmallGreaterThanOneAlpha()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _rand.Binomial(SmallPos + 1, 1);
            });
        }

        [Test]
        public void Binomial_SmallNegativeAlpha()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _rand.Binomial(SmallNeg, 1);
            });
        }

        [Test]
        public void Binomial_SmallNegativeBeta()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _rand.Binomial(0.5, (int) SmallNeg);
            });
        }

        [Test]
        public void Binomial_TinyGreaterThanOneAlpha()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _rand.Binomial(TinyPos + 1, 1);
            });
        }

        [Test]
        public void Binomial_TinyNegativeAlpha()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _rand.Binomial(TinyNeg, 1);
            });
        }

        [Test]
        public void Geometric_LargeNegativeAlpha()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _rand.Geometric(-100);
            });
        }

        [Test]
        public void Geometric_SmallNegativeAlpha()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _rand.Geometric(-1);
            });
        }

        [Test]
        public void Geometric_TinyNegativeAlpha()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _rand.Geometric(-0.01);
            });
        }

        [Test]
        [Repeat(GeneratorCount)]
        public void Normal_SameOutput_WithReset()
        {
            AssertSameContinuousOutput(_rand.Normal, _otherRand.NormalSamples, 5.0, 0.10);
            _rand.Reset();
            _otherRand.Reset();
            AssertSameContinuousOutput(_rand.Normal, _otherRand.NormalSamples, 10.0, 0.20);
        }

        [Test]
        public void StudentsT_LargeNegativeNu()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _rand.StudentsT(-100);
            });
        }

        [Test]
        public void StudentsT_SmallNegativeNu()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _rand.StudentsT(-1);
            });
        }

        [Test]
        public void StudentsT_ZeroNu()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _rand.StudentsT(0);
            });
        }
    }
}