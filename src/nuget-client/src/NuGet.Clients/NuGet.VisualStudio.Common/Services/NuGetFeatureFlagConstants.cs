// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.VisualStudio
{
    public class NuGetFeatureFlagConstants
    {
        internal NuGetFeatureFlagConstants(string name, string environmentVariable, bool defaultState)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            EnvironmentVariable = environmentVariable;
            DefaultState = defaultState;
        }

        /// <summary>
        /// The feature flag name defined for the VS feature flag service.
        /// </summary>
        internal string Name { get; }

        /// <summary>
        /// The environment variable used to override the enabled or disabled state of this feature.
        /// Might be <c>null</c>.
        /// </summary>
        internal string EnvironmentVariable { get; }

        /// <summary>
        /// Default feature state, if the Feature Flag is not specified.
        /// </summary>
        internal bool DefaultState { get; }

        public static readonly NuGetFeatureFlagConstants BulkRestoreCoordination = new("NuGet.BulkRestoreCoordination", "NUGET_BULK_RESTORE_COORDINATION", defaultState: true);
        public static readonly NuGetFeatureFlagConstants NuGetSolutionCacheInitilization = new("NuGet.SolutionCacheInitialization", "NUGET_SOLUTION_CACHE_INITIALIZATION", defaultState: true);
        public static readonly NuGetFeatureFlagConstants RenderReadmeInPMUI = new("NuGet.RenderReadmeInPMUI", "NUGET_RENDER_README_IN_PMUI", defaultState: true);
    }
}
