// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.Protocol.Core.Types;
using NuGet.Resolver;

namespace NuGet.PackageManagement
{
    public class GatherContext
    {
        public GatherContext()
        {
            // Defaults
            AllowDowngrades = true;
        }

        public GatherContext(PackageSourceMapping _packageSourceMappingConfiguration) : this()
        {
            PackageSourceMapping = _packageSourceMappingConfiguration;
        }

        /// <summary>
        /// Project target framework
        /// </summary>
        public NuGetFramework TargetFramework { get; set; }

        /// <summary>
        /// Primary sources - Primary targets must exist here.
        /// </summary>
        public IReadOnlyList<SourceRepository> PrimarySources { get; set; }

        /// <summary>
        /// All sources - used for dependencies
        /// </summary>
        public IReadOnlyList<SourceRepository> AllSources { get; set; }

        /// <summary>
        /// Packages folder
        /// </summary>
        public SourceRepository PackagesFolderSource { get; set; }

        /// <summary>
        /// Target ids
        /// </summary>
        public IReadOnlyList<string> PrimaryTargetIds { get; set; }

        /// <summary>
        /// Targets with an id and version
        /// </summary>
        public IReadOnlyList<PackageIdentity> PrimaryTargets { get; set; }

        /// <summary>
        /// Already installed packages
        /// </summary>
        public IReadOnlyList<PackageIdentity> InstalledPackages { get; set; }

        /// <summary>
        /// If false dependencies from downgrades will be ignored.
        /// </summary>
        public bool AllowDowngrades { get; set; }

        /// <summary>
        /// Resolution context containing the GatherCache and DependencyBehavior.
        /// </summary>
        public ResolutionContext ResolutionContext { get; set; }

        /// <summary>
        /// Project context for logging
        /// </summary>
        public INuGetProjectContext ProjectContext { get; set; }

        /// <summary>
        /// If true, missing primary targets will be ignored.
        /// </summary>
        public bool IsUpdateAll { get; set; }

        /// <summary>
        /// PackageSourceMapping section value from nuget.config file, if section doesn't exist then it's null.
        /// </summary>
        public PackageSourceMapping PackageSourceMapping { get; }

        /// <summary>
        /// Logging adapter
        /// </summary>
        public Common.ILogger Log
        {
            get
            {
                return ProjectContext == null ? Common.NullLogger.Instance : new LoggerAdapter(ProjectContext);
            }
        }
    }
}
