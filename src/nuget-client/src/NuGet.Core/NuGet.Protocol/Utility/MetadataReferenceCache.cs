// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using NuGet.Versioning;

namespace NuGet.Protocol
{
    /// <summary>
    /// Cache strings, dates, and versions to reduce memory.
    /// </summary>
    public class MetadataReferenceCache
    {
        private readonly Dictionary<string, string> _stringCache = new Dictionary<string, string>(StringComparer.Ordinal);
        private readonly Dictionary<string, NuGetVersion> _versionCache = new Dictionary<string, NuGetVersion>(StringComparer.Ordinal);

        /// <summary>
        /// Checks if <paramref name="s"/> already exists in the cache.
        /// If so, returns the cached instance.
        /// If not, caches <paramref name="s"/> and returns it.
        /// </summary>
        public string GetString(string s)
        {
            if (ReferenceEquals(s, null))
            {
                return null;
            }

            if (s.Length == 0)
            {
                return string.Empty;
            }

            string cachedValue;

            if (!_stringCache.TryGetValue(s, out cachedValue))
            {
                _stringCache.Add(s, s);
                cachedValue = s;
            }

            return cachedValue;
        }

        /// <summary>
        /// Parses <paramref name="s"/> into a <see cref="NuGetVersion"/>.
        /// </summary>
        public NuGetVersion GetVersion(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return NuGetVersion.Parse(s);
            }

            NuGetVersion version;
            if (!_versionCache.TryGetValue(s, out version))
            {
                version = NuGetVersion.Parse(s);
                _versionCache.Add(s, version);
            }

            return version;
        }

        /// <summary>
        /// This method is a no-op retained for API compatibility.
        /// </summary>
#pragma warning disable CA1822 // Member does not access instance data - retained as instance method for API compatibility
        public T GetObject<T>(T input)
#pragma warning restore CA1822
        {
            return input;
        }
    }
}
