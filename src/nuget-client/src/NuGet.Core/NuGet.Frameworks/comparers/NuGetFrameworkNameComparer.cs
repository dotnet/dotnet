// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.Frameworks
{
    /// <summary>
    /// A case insensitive compare of the framework name only
    /// </summary>
    public class NuGetFrameworkNameComparer : IEqualityComparer<NuGetFramework>
    {
#pragma warning disable CS0618 // Type or member is obsolete
        public static NuGetFrameworkNameComparer Instance { get; } = new();
#pragma warning restore CS0618 // Type or member is obsolete

        [Obsolete("Use singleton NuGetFrameworkNameComparer instead")]
        public NuGetFrameworkNameComparer()
        {
        }

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

            return StringComparer.OrdinalIgnoreCase.Equals(x.Framework, y.Framework);
        }

        public int GetHashCode(NuGetFramework obj)
        {
            if (ReferenceEquals(obj, null)
                || ReferenceEquals(obj.Framework, null))
            {
                return 0;
            }

            return obj.Framework.ToUpperInvariant().GetHashCode();
        }
    }
}
