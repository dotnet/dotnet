// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.PackageManagement;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Project system data cache that stores project metadata indexed by multiple names,
    /// e.g. EnvDTE.Project can be retrieved by name (if non-conflicting), unique name,
    /// custom unique name, or project id (guid).
    /// </summary>
    public interface IProjectSystemCache
    {
        /// <summary>
        /// This event is used to inform VSSolutionManager that cache has been changed.
        /// </summary>
        event EventHandler<NuGetEventArgs<string>> CacheUpdated;

        /// <summary>
        /// Returns the _isCacheDirty value for the cache. This can be used by the caller to get the status of the cache. 
        /// 0 - Not Dirty; 1 - Dirty;
        /// Can be out of date and should not be used for synchronization.
        /// </summary>
        int IsCacheDirty { get; }

        /// <summary>
        /// Retrieves instance of <see cref="NuGetProject"/> associated with project name.
        /// </summary>
        /// <param name="name">Project name, full path, unique name, or project id (guid).</param>
        /// <param name="nuGetProject">Desired project object, not initialized if not found.</param>
        /// <returns>True if found, false otherwise.</returns>
        bool TryGetNuGetProject(string name, out NuGetProject nuGetProject);

        /// <summary>
        /// Retrieves instance of <see cref="IVsProjectAdapter"/> associated with project name.
        /// </summary>
        /// <param name="name">Project name, full path, unique name, or project id (guid).</param>
        /// <param name="vsProjectAdapter">Desired project adapter, not initialized if not found.</param>
        /// <returns>True if found, false otherwise.</returns>
        bool TryGetVsProjectAdapter(string name, out IVsProjectAdapter vsProjectAdapter);

        /// <summary>
        /// Retrieves project restore info as of <see cref="PackageSpec"/> associated with project name.
        /// </summary>
        /// <param name="name">Project name, full path, unique name, or project id (guid).</param>
        /// <param name="projectRestoreInfo">Desired project restore info object, or null if not found.</param>
        /// <param name="nominationMessages"></param>
        /// <returns>True if found, false otherwise.</returns>
        bool TryGetProjectRestoreInfo(string name, out DependencyGraphSpec projectRestoreInfo, out IReadOnlyList<IAssetsLogMessage> nominationMessages);

        /// <summary>
        /// </summary>
        /// <param name="name">Project name, full path or unique name.</param>
        /// <param name="projectNames">Primary key if found.</param>
        /// <returns>True if the project name with the specified name is found.</returns>
        bool TryGetProjectNames(string name, out ProjectNames projectNames);

        /// <summary>
        /// Tries to find a project by its short name. Returns the project name if and only if it is non-ambiguous.
        /// </summary>
        /// <param name="name">Project short name.</param>
        /// <param name="projectNames">Primary key if found</param>
        /// <returns>True if the project name with the specified short name is found.</returns>
        bool TryGetProjectNameByShortName(string name, out ProjectNames projectNames);

        /// <summary>
        /// Checks if cache contains a project associated with given name or full name.
        /// </summary>
        /// <param name="name">Project name, full path or unique name.</param>
        /// <returns>True if the project name with the specified name is found.</returns>
        bool ContainsKey(string name);

        /// <summary>
        /// Retrieves collection of all project instances stored in the cache.
        /// </summary>
        /// <returns>Collection of projects</returns>
        IReadOnlyList<NuGetProject> GetNuGetProjects();

        /// <summary>
        /// Retrieves collection of all project adapters stored in the cache.
        /// </summary>
        /// <returns>Collection of projects</returns>
        IReadOnlyList<IVsProjectAdapter> GetVsProjectAdapters();

        /// <summary>
        /// Determines if a short name is ambiguous.
        /// </summary>
        /// <param name="shortName">Short name of the project</param>
        /// <returns>True if there are multiple projects with the specified short name.</returns>
        bool IsAmbiguous(string shortName);

        /// <summary>
        /// Adds or updates a project to the project cache.
        /// </summary>
        /// <param name="projectNames">The project name.</param>
        /// <param name="vsProjectAdapter">The VS project adapter.</param>
        /// <param name="nuGetProject">The NuGet project.</param>
        /// <returns>Returns true if the project was successfully added to the cache.</returns>
        bool AddProject(ProjectNames projectNames, IVsProjectAdapter vsProjectAdapter, NuGetProject nuGetProject);

        /// <summary>
        /// Adds or updates project restore info in the project cache.
        /// </summary>
        /// <param name="projectNames">Primary key.</param>
        /// <param name="projectRestoreInfo">The project restore info including tools.</param>
        /// <returns>True if operation succeeded.</returns>
        bool AddProjectRestoreInfo(ProjectNames projectNames, DependencyGraphSpec projectRestoreInfo, IReadOnlyList<IAssetsLogMessage> additionalMessages);

        /// <summary>
        /// Removes a project associated with given name out of the cache.
        /// </summary>
        /// <param name="name">Project name, full path or unique name.</param>
        void RemoveProject(string name);

        /// <summary>
        /// Clears all project cache data.
        /// </summary>
        void Clear();

        /// <summary>
        /// Reset the dirty flag to 0 (is Not Dirty) if the flag was already set.
        /// This is public so that external callers can inform the cache that they have consumed the updated cache event.
        /// </summary>
        /// <returns><code>true</code> if the cache was dirty before and <code>false</code> otherwise</returns>
        bool TestResetDirtyFlag();

        /// <summary>
        /// Adds a project restore info source.
        /// </summary>
        /// <param name="projectNames">The names for the projectNames in question.</param>
        /// <param name="restoreInfoSource">The restore info source object.</param>
        /// <returns>True if operation succeeded.</returns>
        bool AddProjectRestoreInfoSource(ProjectNames projectNames, object restoreInfoSource);

        /// <summary>
        /// Retrieves collection of all project info sources stored in the cache.
        /// </summary>
        /// <returns>Collection of project restore info sources.</returns>
        IReadOnlyList<object> GetProjectRestoreInfoSources();
    }
}
