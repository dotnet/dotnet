// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Frameworks;

namespace NuGet.LibraryModel
{
    public class LibraryDependencyInfo
    {
        /// <summary>
        /// False if the package could not be found.
        /// </summary>
        public bool Resolved { get; }

        /// <summary>
        /// Original library identity from the nuspec.
        /// This contains the original casing for the id/version.
        /// </summary>
        public LibraryIdentity Library { get; }

        /// <summary>
        /// Dependencies
        /// </summary>
        public IEnumerable<LibraryDependency> Dependencies { get; }

        /// <summary>
        /// Target framework used to select the dependencies.
        /// </summary>
        public NuGetFramework Framework { get; }

        public LibraryDependencyInfo(
            LibraryIdentity library,
            bool resolved,
            NuGetFramework framework,
            IEnumerable<LibraryDependency> dependencies)
        {
            Resolved = resolved;
            Library = library ?? throw new ArgumentNullException(nameof(library));
            Framework = framework ?? throw new ArgumentNullException(nameof(framework));
            Dependencies = dependencies ?? throw new ArgumentNullException(nameof(dependencies));
        }

        /// <summary>
        /// Unresolved
        /// </summary>
        public static LibraryDependencyInfo CreateUnresolved(LibraryIdentity library, NuGetFramework framework)
        {
            return new LibraryDependencyInfo(library, resolved: false, framework: framework, dependencies: Enumerable.Empty<LibraryDependency>());
        }

        /// <summary>
        /// Resolved
        /// </summary>
        public static LibraryDependencyInfo Create(LibraryIdentity library, NuGetFramework framework, IEnumerable<LibraryDependency> dependencies)
        {
            return new LibraryDependencyInfo(library, resolved: false, framework: framework, dependencies: dependencies);
        }
    }
}
