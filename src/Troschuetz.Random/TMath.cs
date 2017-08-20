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

namespace Troschuetz.Random
{
    using System;

    /// <summary>
    ///   Simple math utilities used inside the library. They are also public, should you need them
    ///   for custom generators and distributions.
    /// </summary>
    public static class TMath
    {
        /// <summary>
        ///   The delta used when comparing doubles.
        /// </summary>
        public const double Tolerance = 1E-6;

        /// <summary>
        ///   Safely checks if given double is zero.
        /// </summary>
        /// <param name="d">A double.</param>
        /// <returns>True if given double is near zero, false otherwise.</returns>
        public static bool IsZero(double d) => d > -Tolerance && d < Tolerance;

        /// <summary>
        ///   Safely checks if given doubles are equal.
        /// </summary>
        /// <param name="d1">A double.</param>
        /// <param name="d2">A double.</param>
        /// <returns>True if given doubles are safely equal, false otherwise.</returns>
        public static bool AreEqual(double d1, double d2) => IsZero(d1 - d2);

        /// <summary>
        ///   Fast square power.
        /// </summary>
        /// <param name="d">A double.</param>
        /// <returns>The square of given double.</returns>
        public static double Square(double d) => IsZero(d) ? 0.0 : d * d;

        /// <summary>
        ///   Generates a new seed, using all information available, including time.
        /// </summary>
        public static uint Seed()
        {
            unchecked
            {
                const uint factor = 19U;
                var seed = 1777771U;

                seed = factor * seed + (uint) Environment.TickCount;

                var guid = Guid.NewGuid().ToByteArray();
                seed = factor * seed + BitConverter.ToUInt32(guid, 0);
                seed = factor * seed + BitConverter.ToUInt32(guid, 8);

#if !NETSTD10
                seed = factor * seed + (uint) System.Threading.Thread.CurrentThread.ManagedThreadId;
                seed = factor * seed + (uint) System.Diagnostics.Process.GetCurrentProcess().Id;
#endif

                return seed;
            }
        }
    }
}