//-----------------------------------------------------------------------
// <copyright file="DeclaredPropertiesCache.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.DiagnosticSourceListener
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Reflection;

    internal static class DeclaredPropertiesCache
    {
        private const int MaxCacheSize = 100;
        private static ConcurrentDictionary<Type, IEnumerable<PropertyInfo>> cache = new ConcurrentDictionary<Type, IEnumerable<PropertyInfo>>();

        public static IEnumerable<PropertyInfo> GetDeclaredProperties(object obj)
        {
            if (cache.Count > MaxCacheSize)
            {
                cache.Clear();
            }

            return cache.GetOrAdd(obj.GetType(), t => t.GetTypeInfo().DeclaredProperties);
        }
    }
}
