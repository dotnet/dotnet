// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.LibraryModel;
using NuGet.ProjectModel;
using NuGet.RuntimeModel;

namespace NuGet.PackageManagement.VisualStudio.Migrate
{
    internal class PackageSpecProjectJsonMigrationCandidate
    {
        /// <summary>
        /// List of dependencies that apply to all frameworks.
        /// <see cref="ProjectStyle.PackageReference"/> based projects must not use this list and instead use the one in
        /// the <see cref="PackageSpec.TargetFrameworks"/> property which is a list of the <see cref="TargetFrameworkInformation"/> type.
        /// </summary>
        public IList<LibraryDependency> Dependencies { get; init; }
        public IList<TargetFrameworkInformation> TargetFrameworks { get; init; }
        public RuntimeGraph RuntimeGraph { get; init; }

        public PackageSpecProjectJsonMigrationCandidate(
            IList<LibraryDependency> dependencies,
            IList<TargetFrameworkInformation> targetFrameworks,
            RuntimeGraph runtimeGraph)
        {
            Dependencies = dependencies;
            TargetFrameworks = targetFrameworks;
            RuntimeGraph = runtimeGraph;
        }
    }
}
