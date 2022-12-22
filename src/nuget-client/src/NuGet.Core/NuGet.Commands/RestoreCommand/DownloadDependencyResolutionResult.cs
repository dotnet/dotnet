// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.DependencyResolver;
using NuGet.Frameworks;
using NuGet.LibraryModel;

namespace NuGet.Commands
{
    public class DownloadDependencyResolutionResult
    {
        /// <summary>
        /// The framework for which the resolution happened.
        /// </summary>
        public NuGetFramework Framework { get; }

        /// <summary>
        /// A list of library ranges and the dependencies.
        /// </summary>
        public IList<Tuple<LibraryRange, RemoteMatch>> Dependencies { get; }

        /// <summary>
        /// The set of matches that are being installed.
        /// </summary>
        public ISet<RemoteMatch> Install { get; }

        /// <summary>
        /// A set of unresolved library ranges.
        /// </summary>
        public ISet<LibraryRange> Unresolved { get; }


        private DownloadDependencyResolutionResult(NuGetFramework framework, IList<Tuple<LibraryRange, RemoteMatch>> dependencies, ISet<RemoteMatch> install, ISet<LibraryRange> unresolved)
        {
            Framework = framework ?? throw new ArgumentNullException(nameof(framework));
            Dependencies = dependencies ?? throw new ArgumentNullException(nameof(dependencies));
            Install = install ?? throw new ArgumentNullException(nameof(install));
            Unresolved = unresolved ?? throw new ArgumentNullException(nameof(unresolved));
        }

        public static DownloadDependencyResolutionResult Create(NuGetFramework framework, IList<Tuple<LibraryRange, RemoteMatch>> dependencies, IList<IRemoteDependencyProvider> remoteDependencyProviders)
        {
            var install = new HashSet<RemoteMatch>();
            var unresolved = new HashSet<LibraryRange>();

            foreach (var dependency in dependencies)
            {
                if (LibraryType.Unresolved == dependency.Item2.Library.Type)
                {
                    unresolved.Add(dependency.Item1);
                }
                else if (LibraryType.Package == dependency.Item2.Library.Type)
                {

                    var isRemote = remoteDependencyProviders.Contains(dependency.Item2.Provider);
                    if (isRemote)
                    {
                        install.Add(dependency.Item2);
                    }
                }
            }

            return new DownloadDependencyResolutionResult(framework, dependencies, install, unresolved);
        }
    }
}
