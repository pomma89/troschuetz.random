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

namespace Troschuetz.Random.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Core;

    [ContractClassFor(typeof(ILambdaDistribution<>))]
    abstract class LambdaDistributionContract<T> : ILambdaDistribution<T> where T : struct 
    {
        public T Lambda
        {
            get { return default(T); }
            set
            {
                Contract.Requires<ArgumentOutOfRangeException>(IsValidLambda(value), ErrorMessages.InvalidParams);
                Contract.Ensures(Lambda.Equals(value));
            }
        }

        public abstract bool IsValidLambda(T value);
    }

    [ContractClassFor(typeof(IMuDistribution<>))]
    abstract class MuDistributionContract<T> : IMuDistribution<T> where T : struct 
    {
        public T Mu
        {
            get { return default(T); }
            set
            {
                Contract.Requires<ArgumentOutOfRangeException>(IsValidMu(value), ErrorMessages.InvalidParams);
                Contract.Ensures(Mu.Equals(value));
            }
        }

        public abstract bool IsValidMu(T value);
    }

    [ContractClassFor(typeof(INuDistribution<>))]
    abstract class NuDistributionContract<T> : INuDistribution<T> where T : struct 
    {
        public T Nu
        {
            get { return default(T); }
            set
            {
                Contract.Requires<ArgumentOutOfRangeException>(IsValidNu(value), ErrorMessages.InvalidParams);
                Contract.Ensures(Nu.Equals(value));
            }
        }

        public abstract bool IsValidNu(T value);
    }

    [ContractClassFor(typeof(ISigmaDistribution<>))]
    abstract class SigmaDistributionContract<T> : ISigmaDistribution<T> where T : struct 
    {
        public T Sigma
        {
            get { return default(T); }
            set
            {
                Contract.Requires<ArgumentOutOfRangeException>(IsValidSigma(value), ErrorMessages.InvalidParams);
                Contract.Ensures(Sigma.Equals(value));
            }
        }

        public abstract bool IsValidSigma(T value);
    }

    [ContractClassFor(typeof(IThetaDistribution<>))]
    abstract class ThetaDistributionContract<T> : IThetaDistribution<T> where T : struct 
    {
        public T Theta
        {
            get { return default(T); }
            set
            {
                Contract.Requires<ArgumentOutOfRangeException>(IsValidTheta(value), ErrorMessages.InvalidParams);
                Contract.Ensures(Theta.Equals(value));
            }
        }

        public abstract bool IsValidTheta(T value);
    }
    
    [ContractClassFor(typeof(IWeightsDistribution<>))]
    abstract class WeightsDistributionContract<T> : IWeightsDistribution<T> where T : struct
    {
        public ICollection<T> Weights
        {    
            get
            {
                Contract.Ensures(Contract.Result<ICollection<T>>() != null);
// ReSharper disable AssignNullToNotNullAttribute
                return default(ICollection<T>);
// ReSharper restore AssignNullToNotNullAttribute
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null, ErrorMessages.NullWeights);
                Contract.Requires<ArgumentException>(value.Count != 0);
                Contract.Requires<ArgumentOutOfRangeException>(AreValidWeights(value), ErrorMessages.InvalidParams);
                Contract.Ensures(Equals(Weights.Count, value.Count));
            }
        }

        public abstract bool AreValidWeights(IEnumerable<T> values);
    }
}