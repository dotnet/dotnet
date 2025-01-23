// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;

namespace Microsoft.DotNet.VersionTools.Util
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrDefault<TKey, TValue>(
            this IDictionary<TKey, TValue> attributes,
            TKey key)
        {
            TValue value;
            attributes.TryGetValue(key, out value);
            return value;
        }
    }
}
