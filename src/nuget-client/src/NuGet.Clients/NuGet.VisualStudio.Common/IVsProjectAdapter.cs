// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell.Interop;
using NuGet.Frameworks;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Represents an abstraction over Visual Studio project object. Provides access to project's properties and capabilities.
    /// </summary>
    public interface IVsProjectAdapter
    {
        /// <summary>
        /// MSBuildProjectExtensionsPath project property (e.g. c:\projFoo\obj)
        /// </summary>
        string GetMSBuildProjectExtensionsPath();

        IVsProjectBuildProperties BuildProperties { get; }

        string CustomUniqueName { get; }

        string FullName { get; }

        string FullProjectPath { get; }

        Task<bool> IsSupportedAsync();

        /// <summary>
        /// In unavoidable circumstances where we need to DTE object, it's exposed here
        /// </summary>
        EnvDTE.Project Project { get; }

        string ProjectId { get; }

        /// <summary>
        /// Full path to a parent directory containing project file.
        /// </summary>
        string ProjectDirectory { get; }

        string ProjectName { get; }

        ProjectNames ProjectNames { get; }

        string UniqueName { get; }

        /// <summary>
        /// Version
        /// </summary>
        string Version { get; }

        IVsHierarchy VsHierarchy { get; }

        Task<string[]> GetProjectTypeGuidsAsync();

        Task<FrameworkName> GetDotNetFrameworkNameAsync();

        Task<IEnumerable<string>> GetReferencedProjectsAsync();

        /// <summary>
        /// Project's target framework
        /// </summary>
        Task<NuGetFramework> GetTargetFrameworkAsync();

        /// <summary>
        /// Reads a project build items and the requested metadata.
        /// </summary>
        /// <param name="itemName">The item name.</param>
        /// <param name="metadataNames">The metadata names to read.</param>
        /// <returns>An <see cref="IEnumerable{(string ItemId, string[] ItemMetadata)}"/> containing the itemId and the metadata values.</returns>
        Task<IEnumerable<(string ItemId, string[] ItemMetadata)>> GetBuildItemInformationAsync(string itemName, params string[] metadataNames);

        /// <summary>
        /// See <see cref="Microsoft.VisualStudio.Shell.PackageUtilities.IsCapabilityMatch(IVsHierarchy, string)"/>
        /// </summary>
        Task<bool> IsCapabilityMatchAsync(string capabilityExpression);
    }
}
