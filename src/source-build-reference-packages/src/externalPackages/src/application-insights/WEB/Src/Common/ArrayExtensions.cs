namespace Microsoft.ApplicationInsights.Common
{
    internal static class ArrayExtensions
    {
        /// <summary>
        /// Returns an empty array.
        /// </summary>
        /// <remarks>
        /// Array.Empty() was added to Net Framework in 4.6
        /// This adds support for net452.
        /// </remarks>
        internal static T[] Empty<T>()
        {
#if NET452
            return EmptyArray<T>.Value;
#else
            return System.Array.Empty<T>();
#endif
        }

#if NET452
        /// <summary>
        /// [net452 Only] Copied from Net Framework (https://referencesource.microsoft.com/#mscorlib/system/array.cs,bc9fd1be0e4f4e70,references).
        /// </summary>
        /// <remarks>
        /// Array.Empty() was added to Net Framework in 4.6
        /// This adds support for net452.
        /// </remarks>
        private static class EmptyArray<T>
        {
            public static readonly T[] Value = new T[0];
        }
#endif
    }
}
