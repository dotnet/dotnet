// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Protocol.Core.Types;
using NuGet.Resolver;

namespace NuGet.PackageManagement
{
    /// <summary>
    /// Resolution context such as DependencyBehavior, IncludePrerelease and so on
    /// </summary>
    public class ResolutionContext
    {
        /// <summary>
        /// Public constructor to create the resolution context
        /// </summary>
        public ResolutionContext()
            : this(DependencyBehavior.Lowest,
                  includePrelease: false,
                  includeUnlisted: true,
                  versionConstraints: VersionConstraints.None,
                  gatherCache: new GatherCache(),
                  sourceCacheContext: NullSourceCacheContext.Instance)
        {
        }

        /// <summary>
        /// Public constructor to create the resolution context
        /// </summary>
        public ResolutionContext(
            DependencyBehavior dependencyBehavior,
            bool includePrelease,
            bool includeUnlisted,
            VersionConstraints versionConstraints)
            : this(dependencyBehavior,
                  includePrelease,
                  includeUnlisted,
                  versionConstraints,
                  new GatherCache(),
                  NullSourceCacheContext.Instance)
        {
        }

        /// <summary>
        /// Public constructor to create the resolution context
        /// </summary>
        public ResolutionContext(
            DependencyBehavior dependencyBehavior,
            bool includePrelease,
            bool includeUnlisted,
            VersionConstraints versionConstraints,
            GatherCache gatherCache,
            SourceCacheContext sourceCacheContext)
        {
            if (gatherCache == null)
            {
                throw new ArgumentNullException(nameof(gatherCache));
            }

            DependencyBehavior = dependencyBehavior;
            IncludePrerelease = includePrelease;
            IncludeUnlisted = includeUnlisted;
            VersionConstraints = versionConstraints;
            GatherCache = gatherCache;
            SourceCacheContext = sourceCacheContext;
        }

        /// <summary>
        /// Determines the dependency behavior
        /// </summary>
        public DependencyBehavior DependencyBehavior { get; }

        /// <summary>
        /// Determines if prerelease may be included in the installation
        /// </summary>
        public bool IncludePrerelease { get; }

        /// <summary>
        /// Determines if unlisted packages may be included in installation
        /// </summary>
        public bool IncludeUnlisted { get; }

        /// <summary>
        /// Determines the containts that are placed on package update selection with respect to the installed packages
        /// </summary>
        public VersionConstraints VersionConstraints { get; }

        /// <summary>
        /// Gathe cache containing cached packages that can be used across operations.
        /// Ex: Update-Package updates all packages across all projects, GatherCache stores
        /// the gathered packages and re-uses them across all sub operations.
        /// </summary>
        public GatherCache GatherCache { get; }

        /// <summary>
        /// Http source cache context which will be shared across operations.
        /// </summary>
        public SourceCacheContext SourceCacheContext { get; }
    }
}
