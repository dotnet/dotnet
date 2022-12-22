// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using NuGet.ProjectModel;

namespace NuGet.ProjectManagement
{
    /// <summary>
    /// Represents the interface necessary for adding a project to a <see cref="DependencyGraphSpec"/>-based restore.
    /// This interface has logic for creating the <see cref="PackageSpec"/> as well as for detecting no-op cases where
    /// a restore is not necessary.
    /// </summary>
    public interface IDependencyGraphProject
    {
        /// <summary>
        /// Gets the path to the MSBuild project file. This is an absolute path.
        /// </summary>
        string MSBuildProjectPath { get; }

        /// <summary>
        /// Project specs related to this project. This must include the project's own spec, and may
        /// optionally include more specs to restore such as tools.
        /// </summary>
        Task<IReadOnlyList<PackageSpec>> GetPackageSpecsAsync(DependencyGraphCacheContext context);

        /// <summary>
        /// Project specs related to this project. This must include the project's own spec, and may
        /// optionally include more specs to restore such as tools.
        /// </summary>
        Task<(IReadOnlyList<PackageSpec> dgSpecs, IReadOnlyList<IAssetsLogMessage> additionalMessages)> GetPackageSpecsAndAdditionalMessagesAsync(DependencyGraphCacheContext context);
    }
}
