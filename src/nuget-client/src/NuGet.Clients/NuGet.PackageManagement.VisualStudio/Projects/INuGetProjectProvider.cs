// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.ProjectManagement;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// Represents a provider interface capable of instantiating <see cref="NuGetProject"/>.
    /// </summary>
    public interface INuGetProjectProvider
    {
        /// <summary>
        /// Type of project this provider creates
        /// </summary>
        System.RuntimeTypeHandle ProjectType { get; }

        /// <summary>
        /// Attempts to create a <see cref="NuGetProject"/> instance if the input DTE project
        /// matches certain criteria.
        /// </summary>
        /// <param name="project">Existing Visual Studio object.</param>
        /// <param name="context">Context used to create a new project instance.</param>
        /// <param name="forceProjectType">Flag to control project type preference. <code>true</code> indicates provider is to create a project regardless of providers order limitations.</param>
        /// <returns>New instance if instantiation succeeds, null otherwise.</returns>
        NuGetProject TryCreateNuGetProject(
            IVsProjectAdapter project,
            ProjectProviderContext context,
            bool forceProjectType);
    }
}
