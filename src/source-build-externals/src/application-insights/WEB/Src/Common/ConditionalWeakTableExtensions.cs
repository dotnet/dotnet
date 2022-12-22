namespace Microsoft.ApplicationInsights.Common
{
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Extension methods for the ConditionalWeakTable class.
    /// </summary>
    public static class ConditionalWeakTableExtensions
    {
        /// <summary>
        /// Check if a key exists before adding the key/value pair.
        /// </summary>
        public static void AddIfNotExists<TKey, TValue>(this ConditionalWeakTable<TKey, TValue> conditionalWeakTable, TKey key, TValue value) where TKey : class where TValue : class
        {
            if (conditionalWeakTable == null)
            {
                throw new ArgumentNullException(nameof(conditionalWeakTable));
            }

            if (!conditionalWeakTable.TryGetValue(key, out TValue testValue))
            {
                conditionalWeakTable.Add(key, value);
            }
        }
    }
}
