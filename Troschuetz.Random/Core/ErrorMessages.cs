/*
 * Copyright © 2012-2014 Alessio Parma (alessio.parma@gmail.com)
 *
 * This file is part of Troschuetz.Random Class Library.
 *
 * Troschuetz.Random is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
 */

namespace Troschuetz.Random.Core
{
    static class ErrorMessages
    {
        public const string EmptyList = "List must not be empty.";
        public const string InvalidParams = "Given parameter (or parameters) are not valid.";
        public const string MaxValueIsTooSmall = "Given max value is too small.";
        public const string MinValueGreaterThanOrEqualToMaxValue = "maxValue should be greater than minValue.";
        public const string NegativeMaxValue = "maxValue must be greater than or equal to zero.";
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
