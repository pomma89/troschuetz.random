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
using Troschuetz.Random.Core;

namespace Troschuetz.Random.Distributions
{
    /// <summary>
    ///   Abstract class which implements some features shared across all distributions.
    /// </summary>
    /// <remarks>The thread safety of this class depends on the one of the underlying generator.</remarks>
    [Serializable]
    public abstract class AbstractDistribution
    {
        /// <summary>
        ///   Builds a distribution using given generator.
        /// </summary>
        /// <param name="generator">The generator that will be used by the distribution.</param>
        /// <exception cref="ArgumentNullException">Given generator is null.</exception>
        protected AbstractDistribution(IGenerator generator)
        {
            Generator = generator ?? throw new ArgumentNullException(nameof(generator), ErrorMessages.NullGenerator);
        }

        #region IDistribution members

        /// <summary>
        ///   Gets a value indicating whether the random number distribution can be reset, so that it
        ///   produces the same random number sequence again.
        /// </summary>
        public bool CanReset => Generator.CanReset;

        /// <summary>
        ///   Gets the <see cref="IGenerator"/> object that is used as underlying random number generator.
        /// </summary>
        public IGenerator Generator { get; }

        /// <summary>
        ///   Resets the random number distribution, so that it produces the same random number
        ///   sequence again.
        /// </summary>
        /// <returns>
        ///   <see langword="true"/>, if the random number distribution was reset; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Reset() => Generator.Reset();

        #endregion IDistribution members
    }
}