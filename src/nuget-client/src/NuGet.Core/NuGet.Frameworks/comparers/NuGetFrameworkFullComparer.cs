// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Shared;

namespace NuGet.Frameworks
{
    /// <summary>
    /// A case insensitive compare of the framework, version, and profile
    /// </summary>
    public class NuGetFrameworkFullComparer : IEqualityComparer<NuGetFramework>
    {
#pragma warning disable CS0618 // Type or member is obsolete
        public static NuGetFrameworkFullComparer Instance { get; } = new();
#pragma warning restore CS0618 // Type or member is obsolete

        [Obsolete("Use singleton via NuGetFrameworkFullComparer.Instance instead")]
        public NuGetFrameworkFullComparer() { }

        public bool Equals(NuGetFramework? x, NuGetFramework? y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(x, null)
                || ReferenceEquals(y, null))
            {
                return false;
            }

            return x.Version == y.Version
                   && StringComparer.OrdinalIgnoreCase.Equals(x.Framework, y.Framework)
                   && StringComparer.OrdinalIgnoreCase.Equals(x.Profile, y.Profile)
                   && StringComparer.OrdinalIgnoreCase.Equals(x.Platform, y.Platform)
                   && x.PlatformVersion == y.PlatformVersion
                   && !x.IsUnsupported;
        }

        public int GetHashCode(NuGetFramework obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return 0;
            }

            var combiner = new HashCodeCombiner();

            combiner.AddStringIgnoreCase(obj.Framework);
            combiner.AddObject(obj.Version);
            combiner.AddStringIgnoreCase(obj.Profile);

            return combiner.CombinedHash;
        }
    }
}
