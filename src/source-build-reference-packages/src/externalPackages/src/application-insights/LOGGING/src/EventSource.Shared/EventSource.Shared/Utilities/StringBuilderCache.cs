//-----------------------------------------------------------------------
// <copyright file="StringBuilderCache.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.TraceEvent.Shared.Utilities
{
    using System;
    using System.Text;

    /// <summary>
    /// Provides a cached reusable instance of a StringBuilder per thread. It is an optimization that reduces the number of instances constructed and collected.
    /// </summary>
    internal static class StringBuilderCache
    {
        // The value 360 was chosen in discussion with performance experts as a compromise between using
        // as litle memory (per thread) as possible and still covering a large part of short-lived
        // StringBuilder creations.
        private const int MaxBuilderSize = 360;

        [ThreadStatic]
        private static StringBuilder cachedInstance;

        /// <summary>
        /// Gets a string builder to use of a particular size.
        /// </summary>
        /// <param name="capacity">Initial capacity of the requested StringBuilder.</param>
        /// <returns>An instance of a StringBuilder.</returns>
        /// <remarks>
        /// It can be called any number of times. If a StringBuilder is in the cache then it will be returned and the cache emptied.
        /// A StringBuilder instance is cached in Thread Local Storage and so there is one per thread.
        /// Subsequent calls will return a new StringBuilder. 
        /// </remarks>
        public static StringBuilder Acquire(int capacity = 16 /*StringBuilder.DefaultCapacity*/)
        {
            if (capacity <= MaxBuilderSize)
            {
                StringBuilder sb = StringBuilderCache.cachedInstance;
                if (sb != null)
                {
                    // Avoid stringbuilder block fragmentation by getting a new StringBuilder
                    // when the requested size is larger than the current capacity
                    if (capacity <= sb.Capacity)
                    {
                        StringBuilderCache.cachedInstance = null;
                        sb.Clear();
                        return sb;
                    }
                }
            }

            return new StringBuilder(capacity);
        }

        /// <summary>
        /// Place the specified builder in the cache if it is not too big. 
        /// </summary>
        /// <param name="sb">StringBuilder that is no longer used.</param>
        /// <remarks>
        /// The StringBuilder should not be used after it has been released. Unbalanced Releases are perfectly acceptable. 
        /// It will merely cause the runtime to create a new StringBuilder next time Acquire is called.
        /// </remarks>
        public static void Release(StringBuilder sb)
        {
            if (sb.Capacity <= MaxBuilderSize)
            {
                StringBuilderCache.cachedInstance = sb;
            }
        }

        /// <summary>
        /// Gets the resulting string and releases a StringBuilder instance.
        /// </summary>
        /// <param name="sb">StringBuilder to be released.</param>
        /// <returns>The output of the <paramref name="sb"/> StringBuilder.</returns>
        public static string GetStringAndRelease(StringBuilder sb)
        {
            string result = sb.ToString();
            Release(sb);
            return result;
        }
    }
}