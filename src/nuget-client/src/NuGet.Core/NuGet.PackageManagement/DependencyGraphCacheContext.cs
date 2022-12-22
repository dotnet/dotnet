// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.ProjectModel;

namespace NuGet.ProjectManagement
{
    public class DependencyGraphCacheContext
    {
        public DependencyGraphCacheContext(ILogger logger, ISettings settings)
        {
            Logger = logger;
            Settings = settings;
        }

        public DependencyGraphCacheContext()
        {
            Logger = NullLogger.Instance;
            Settings = NullSettings.Instance;
        }

        /// <summary>
        /// Unique name to dg
        /// </summary>
        public Dictionary<string, DependencyGraphSpec> DependencyGraphCache { get; set; } =
            new Dictionary<string, DependencyGraphSpec>(StringComparer.Ordinal);

        /// <summary>
        /// Unique name to PackageSpec
        /// </summary>
        public Dictionary<string, PackageSpec> PackageSpecCache { get; set; } =
            new Dictionary<string, PackageSpec>(StringComparer.Ordinal);

        /// <summary>
        /// Cache for direct project references of a project
        /// </summary>
        public Dictionary<string, IReadOnlyList<IDependencyGraphProject>> DirectReferenceCache { get; set; } = new Dictionary<string, IReadOnlyList<IDependencyGraphProject>>(StringComparer.Ordinal);

        /// <summary>
        /// Logger
        /// </summary>
        public ILogger Logger { get; }

        public ISettings Settings { get; }
    }
}
