// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.LibraryModel;

namespace NuGet.DependencyResolver
{
    public static class PackagingUtility
    {
        /// <summary>
        /// Convert a nuspec dependency to a library dependency.
        /// </summary>
        public static LibraryDependency GetLibraryDependencyFromNuspec(Packaging.Core.PackageDependency dependency)
        {
            // Start with the default. For package to package this is everything but content v2.
            var includeType = LibraryIncludeFlagUtils.NoContent;

            // Add includes
            if (dependency.Include.Count > 0)
            {
                includeType = LibraryIncludeFlagUtils.GetFlags(dependency.Include);
            }

            // Remove excludes
            if (dependency.Exclude.Count > 0)
            {
                var excludeType = LibraryIncludeFlagUtils.GetFlags(dependency.Exclude);

                includeType = includeType & ~excludeType;
            }

            // Create the library
            // Nuspec references cannot contain suppress parent flags
            var libraryDependency = new LibraryDependency
            {
                LibraryRange = new LibraryRange
                {
                    Name = dependency.Id,
                    VersionRange = dependency.VersionRange,
                    TypeConstraint = LibraryDependencyTarget.PackageProjectExternal
                },
                IncludeType = includeType,
                SuppressParent = LibraryIncludeFlags.None
            };

            return libraryDependency;
        }
    }
}
