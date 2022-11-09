// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using NuGet.Versioning;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// Extension methods for collection of <see cref="NuGetVersion"/>
    /// </summary>
    public static class VersionCollectionExtensions
    {
        public static NuGetVersion MinOrDefault(this IEnumerable<NuGetVersion> versions)
        {
            return versions
                .OrderBy(v => v, VersionComparer.Default)
                .FirstOrDefault();
        }

        public static NuGetVersion MaxOrDefault(this IEnumerable<NuGetVersion> versions)
        {
            return versions
                .OrderByDescending(v => v, VersionComparer.Default)
                .FirstOrDefault();
        }
    }
}
