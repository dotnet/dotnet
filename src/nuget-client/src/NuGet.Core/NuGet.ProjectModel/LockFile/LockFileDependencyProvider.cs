// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.DependencyResolver;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Versioning;

namespace NuGet.ProjectModel
{
    [Obsolete("This is an unused class and will be removed in a future version.")]
    public class LockFileDependencyProvider : IDependencyProvider
    {
        private readonly IDictionary<Tuple<NuGetFramework, string>, LockFileTargetLibrary> _targetLibraries;
        private readonly IDictionary<Tuple<string, NuGetVersion>, LockFileLibrary> _libraries;

        public LockFileDependencyProvider(LockFile lockFile)
        {
            // List of all the libraries in the lock file, there can be multiple versions per id
            _libraries = lockFile.Libraries.ToDictionary(l => Tuple.Create(l.Name, l.Version));

            // Dependencies can only vary by target framework so only look at that target
            _targetLibraries = new Dictionary<Tuple<NuGetFramework, string>, LockFileTargetLibrary>();

            foreach (var target in lockFile.Targets)
            {
                if (!string.IsNullOrEmpty(target.RuntimeIdentifier))
                {
                    continue;
                }

                foreach (var library in target.Libraries)
                {
                    _targetLibraries[Tuple.Create(target.TargetFramework, library.Name)] = library;
                }
            }
        }

        public bool SupportsType(LibraryDependencyTarget libraryType)
        {
            return (libraryType & LibraryDependencyTarget.Package) == LibraryDependencyTarget.Package;
        }

        public Library GetLibrary(LibraryRange libraryRange, NuGetFramework targetFramework)
        {
            var key = Tuple.Create(targetFramework, libraryRange.Name);

            // Determine if we have a library for this target
            if (_targetLibraries.TryGetValue(key, out LockFileTargetLibrary library) &&
                libraryRange.VersionRange.IsBetter(current: null, considering: library.Version))
            {
                var dependencies = GetDependencies(library);

                var description = new Library
                {
                    LibraryRange = libraryRange,
                    Identity = new LibraryIdentity
                    {
                        Name = library.Name,
                        Version = library.Version,
                        Type = LibraryType.Package
                    },
                    Resolved = true,
                    Dependencies = dependencies,

                    [KnownLibraryProperties.LockFileLibrary] = _libraries[Tuple.Create(library.Name, library.Version)],
                    [KnownLibraryProperties.LockFileTargetLibrary] = library
                };

                return description;
            }

            return null;
        }

        private IList<LibraryDependency> GetDependencies(LockFileTargetLibrary library)
        {
            var libraryDependencies = new List<LibraryDependency>();

            libraryDependencies.AddRange(
                    library.Dependencies.Select(PackagingUtility.GetLibraryDependencyFromNuspec));

            return libraryDependencies;
        }
    }
}
