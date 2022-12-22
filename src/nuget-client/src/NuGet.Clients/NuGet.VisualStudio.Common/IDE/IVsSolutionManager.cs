// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// Extends core <see cref="ISolutionManager"/> API with VisualStudio specific methods.
    /// </summary>
    public interface IVsSolutionManager : ISolutionManager
    {
        /// <summary>
        /// Gets the default <see cref="NuGetProject" />. Default NuGetProject is the selected NuGetProject in the IDE.
        /// </summary>
        Task<NuGetProject> GetDefaultNuGetProjectAsync();

        /// <summary>
        /// Gets the name of the default <see cref="NuGetProject" />. Default NuGetProject is the selected NuGetProject
        /// in the IDE.
        /// </summary>
        string DefaultNuGetProjectName { get; set; }

        /// <summary>
        /// Get the initialization status of SolutionManager
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Gets the SolutionManager initialization task, if there is one running.
        /// </summary>
        Task InitializationTask { get; set; }

        /// <summary>
        /// Retrieves <see cref="NuGetProject"/> instance associated with VS project.
        /// Creates new instance if not found in project system cache.
        /// </summary>
        /// <param name="project">VS project.</param>
        /// <param name="projectContext">Context object for new instance creation.</param>
        /// <returns>Existing or new <see cref="NuGetProject"/> instance.</returns>
        Task<NuGetProject> GetOrCreateProjectAsync(EnvDTE.Project project, INuGetProjectContext projectContext);

        /// <summary>
        /// Retrieves instance of <see cref="EnvDTE.Project"/> associated with project name, path, or id.
        /// </summary>
        /// <param name="name">Project name, full path or unique name.</param>
        /// <returns>Desired project object.</returns>
        Task<IVsProjectAdapter> GetVsProjectAdapterAsync(string name);

        /// <summary>
        /// Retrieves instance of <see cref="EnvDTE.Project"/> associated with project name, path, or id.
        /// </summary>
        /// <param name="name">Project name, full path or unique name.</param>
        /// <returns>Desired project object.</returns>
        Task<IVsProjectAdapter> GetVsProjectAdapterAsync(NuGetProject project);

        /// <summary>
        /// Return true if all projects in the solution have been loaded in background.
        /// </summary>
        Task<bool> IsSolutionFullyLoadedAsync();

        /// <summary>
        /// Retrieves collection of <see cref="IVsProjectAdapter"/> for all supported projects in a solution.
        /// </summary>
        /// <returns>Collection of <see cref="IVsProjectAdapter"/></returns>
        Task<IEnumerable<IVsProjectAdapter>> GetAllVsProjectAdaptersAsync();

        /// <summary>
        /// Creates a new instance of <see cref="NuGetProject"/> supporting package references.
        /// </summary>
        /// <param name="project">Existing project to upgrade.</param>
        /// <returns>New project instance.</returns>
        Task<NuGetProject> UpgradeProjectToPackageReferenceAsync(NuGetProject project);

        /// <summary>
        /// Return true if all the .net core projects are nominated.
        /// </summary>
        Task<bool> IsAllProjectsNominatedAsync();

        /// <summary>
        /// Returns Solution FullName
        /// </summary>
        /// <returns></returns>
        Task<string> GetSolutionFilePathAsync();

        /// <summary>
        /// Returns whether the solution is open.
        /// </summary>
        Task<bool> IsSolutionOpenAsync();

        /// <summary>
        /// Returns the list of project restore info sources. Empty if none are available.
        /// </summary>
        IReadOnlyList<object> GetAllProjectRestoreInfoSources();

        /// <summary>
        /// Gets the current open solution directory. <see cref="null"/> if the there's no open solution.
        /// </summary>
        Task<string> GetSolutionDirectoryAsync();

        /// <summary>Mockable indirection for <see cref="Microsoft.VisualStudio.Shell.VsShellUtilities.ShutdownToken"/></summary>
        CancellationToken VsShutdownToken { get; }
    }
}
