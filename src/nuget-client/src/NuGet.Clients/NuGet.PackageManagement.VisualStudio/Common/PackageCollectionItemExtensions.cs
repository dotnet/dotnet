// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.Linq;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// Extension methods for <see cref="PackageCollectionItem"/>
    /// </summary>
    internal static class PackageCollectionItemExtensions
    {
        public static bool IsAutoReferenced(this PackageCollectionItem package)
        {
            return package.PackageReferences.Any(e => e.IsAutoReferenced);
        }
    }
}
