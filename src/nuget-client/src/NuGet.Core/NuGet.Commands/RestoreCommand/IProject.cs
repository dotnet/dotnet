// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NuGet.Commands.Restore
{
    /// <summary>
    /// Restore has many entry points, and each gets project items and properties with different APIs.
    /// This interface abstracts the project model to allow a single implementation of
    /// <see cref="MSBuildRestoreUtility.GetPackageSpec(IProject)"/> to be shared across all restore entry points.
    /// </summary>
    public interface IProject
    {
        /// <summary>The full, absolute path to the project file.</summary>
        string FullPath { get; }

        /// <summary>The directory the project is in.</summary>
        string Directory { get; }

        /// <summary>
        /// Multitargeting projects do "inner builds", one for each target framework. This property represents the
        /// project without doing any of the per-target framework inner builds.
        /// </summary>
        public ITargetFramework OuterBuild { get; }

        /// <summary>
        /// The inner build  <see langword="null"/>
        /// </summary>
        public IReadOnlyDictionary<string, ITargetFramework> TargetFrameworks { get; }

        /// <summary>
        /// Get the value for a global property in the project.
        /// Global properties represent values set via command line arguments (e.g. /p:Property=Value),
        /// which take highest priority over properties defined in the project file.
        /// </summary>
        /// <param name="propertyName">The name of the global property.</param>
        /// <returns>The value of the requested global property, or <see langword="null"/> if the property was not set as a global property.</returns>
        string? GetGlobalProperty(string propertyName);
    }
}
