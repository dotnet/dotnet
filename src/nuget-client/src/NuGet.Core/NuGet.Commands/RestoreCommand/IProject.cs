// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

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
    }
}
