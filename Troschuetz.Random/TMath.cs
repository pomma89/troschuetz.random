using System.Runtime.CompilerServices;

namespace Troschuetz.Random
{
    /// <summary>
    ///   Simple math utilities used inside the library. They are also public, should you need them
    ///   for custom generators and distributions.
    /// </summary>
    public static class TMath
    {
        /// <summary>
        ///   The delta used when comparing doubles.
        /// </summary>
        public const double Epsilon = 1E-6;

        /// <summary>
        ///   Safely checks if given double is zero.
        /// </summary>
        /// <param name="d">A double.</param>
        /// <returns>True if given double is near zero, false otherwise.</returns>
#if PORTABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool IsZero(double d) => d > -Epsilon && d < Epsilon;

        /// <summary>
        ///   Safely checks if given doubles are equal.
        /// </summary>
        /// <param name="d1">A double.</param>
        /// <param name="d2">A double.</param>
        /// <returns>True if given doubles are safely equal, false otherwise.</returns>
#if PORTABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool AreEqual(double d1, double d2) => IsZero(d1 - d2);

        /// <summary>
        ///   Fast square power.
        /// </summary>
        /// <param name="d">A double.</param>
        /// <returns>The square of given double.</returns>
#if PORTABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static double Square(double d) => IsZero(d) ? 0.0 : d * d;
    }
}
