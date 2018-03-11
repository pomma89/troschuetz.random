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

namespace Troschuetz.Random.Core
{
    internal static class ErrorMessages
    {
        public const string EmptyList = "List must not be empty.";
        public const string InvalidParams = "Given parameter (or parameters) are not valid.";
        public const string MaxValueIsTooSmall = "Given max value is too small.";
        public const string MinValueGreaterThanMaxValue = "maxValue should be greater than or equal to minValue.";
        public const string NegativeMaxValue = "maxValue must be greater than or equal to zero.";
        public const string InfiniteMaxValue = "maxValue cannot be equal to positive infinity.";
        public const string InfiniteMaxValueMinusMinValue = "maxValue minus minValue cannot be equal to infinity.";
        public const string NullBuffer = "Buffer must not be undefined.";
        public const string NullDistribution = "Distribution must not be undefined.";
        public const string NullGenerator = "Generator must not be undefined.";
        public const string NullList = "List must not be undefined.";
        public const string NullWeights = "Weights collection must not be undefined.";
        public const string UndefinedMean = "Mean is undefined for given distribution.";
        public const string UndefinedMeanForParams = "Mean is undefined for given distribution under given parameters.";
        public const string UndefinedMedian = "Median is undefined for given distribution.";
        public const string UndefinedMode = "Mode is undefined for given distribution.";
        public const string UndefinedModeForParams = "Mode is undefined for given distribution under given parameters.";
        public const string UndefinedVariance = "Variance is undefined for given distribution.";
        public const string UndefinedVarianceForParams = "Variance is undefined for given distribution under given parameters.";
    }
}