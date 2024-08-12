// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;
using NuGet.VisualStudio;
using VSLangProj;
using VSLangProj150;
using VSLangProj80;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// Reference reader implementation for the core project system in the integrated development environment (IDE).
    /// </summary>
    internal class VsCoreProjectSystemReferenceReader
        : IProjectSystemReferencesReader
    {
        private readonly Array _referenceMetadata;

        private readonly IVsProjectAdapter _vsProjectAdapter;
        private readonly IVsProjectThreadingService _threadingService;

        public VsCoreProjectSystemReferenceReader(
            IVsProjectAdapter vsProjectAdapter,
            IVsProjectThreadingService threadingService)
        {
            Assumes.Present(vsProjectAdapter);
            Assumes.Present(threadingService);

            _vsProjectAdapter = vsProjectAdapter;
            _threadingService = threadingService;

            _referenceMetadata = new string[] { "ReferenceOutputAssembly" };
        }

        public async Task<IEnumerable<ProjectRestoreReference>> GetProjectReferencesAsync(
            Common.ILogger logger, CancellationToken _)
        {
            // DTE calls need to be done from the main thread
            await _threadingService.JoinableTaskFactory.SwitchToMainThreadAsync();

            var results = new List<ProjectRestoreReference>();
            var hasMissingReferences = false;
            var hasProjectsWithUnresolvedMetadata = false;

            // find all references in the project
            foreach (var childReference in GetVSProjectReferences())
            {
                try
                {
                    var reference3 = childReference as Reference3;

                    // Verify that this is a project reference
                    if (IsProjectReference(reference3, logger))
                    {
                        // Verify that this is a valid and resolved project reference
                        if (!IsReferenceResolved(reference3, logger))
                        {
                            hasMissingReferences = true;
                            continue;
                        }

                        if (await EnvDTEProjectUtility.HasUnsupportedProjectCapabilityAsync(reference3.SourceProject))
                        {
                            // Skip this shared project
                            continue;
                        }

                        var childProjectPath = reference3.SourceProject.GetFullProjectPath();

                        // Skip projects which have ReferenceOutputAssembly=false
                        var addProject = true;

                        if (childReference is Reference6 reference6)
                        {
                            reference6.GetMetadata(_referenceMetadata, out Array _, out Array metadataValues);
                            var referenceOutputAssembly = GetReferenceMetadataValue(metadataValues);
                            addProject = string.IsNullOrEmpty(referenceOutputAssembly) ||
                                !string.Equals(bool.FalseString, referenceOutputAssembly, StringComparison.OrdinalIgnoreCase);
                        }
                        else
                        {
                            hasProjectsWithUnresolvedMetadata = true;
                        }

                        if (addProject)
                        {
                            results.Add(new ProjectRestoreReference()
                            {
                                ProjectPath = childProjectPath,
                                ProjectUniqueName = childProjectPath
                            });
                        }
                    }
                    else
                    {
                        hasMissingReferences = true;
                    }
                }
                catch (Exception ex)
                {
                    // Exceptions are expected in some scenarios for native projects,
                    // ignore them and show a warning
                    hasMissingReferences = true;

                    logger.LogDebug(ex.ToString());

                    Debug.Fail("Unable to find project dependencies: " + ex.ToString());
                }
            }

            if (hasMissingReferences)
            {
                // Log a generic message once per project if any items could not be resolved.
                // In most cases this can be ignored, but in the rare case where the unresolved
                // item is actually a project the restore result will be incomplete.
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.UnresolvedItemDuringProjectClosureWalk,
                    _vsProjectAdapter.UniqueName);

                logger.LogVerbose(message);
            }

            if (hasProjectsWithUnresolvedMetadata)
            {
                IList<string> excludedProjects = await GetExcludedProjectsAsync(logger);
                if (excludedProjects.Count > 0)
                {
                    results = results.Where(e => !excludedProjects.Contains(e.ProjectPath, StringComparer.OrdinalIgnoreCase)).ToList();
                }
            }

            return results;

            static string GetReferenceMetadataValue(Array metadataValues)
            {
                if (metadataValues == null || metadataValues.Length == 0)
                {
                    return string.Empty; // no metadata for package
                }

                return metadataValues.GetValue(0) as string;
            }
        }

        private IEnumerable<Reference> GetVSProjectReferences()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var langProject = _vsProjectAdapter.Project.Object as VSProject;
            if (langProject != null)
            {
                return langProject.References.Cast<Reference>();
            }

            return Enumerable.Empty<Reference>();
        }

        private async Task<IList<string>> GetExcludedProjectsAsync(Common.ILogger logger)
        {
            var itemsFactory = await ServiceLocator.GetGlobalServiceAsync<IVsEnumHierarchyItemsFactory, IVsEnumHierarchyItemsFactory>();

            // Verify ReferenceOutputAssembly
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            var excludedProjects = GetExcludedReferences(itemsFactory, logger);

            return excludedProjects;
        }

        /// <summary>
        /// Get the unique names of all references which have ReferenceOutputAssembly set to false.
        /// </summary>
        private IList<string> GetExcludedReferences(
            IVsEnumHierarchyItemsFactory itemsFactory,
            Common.ILogger logger)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var excludedReferences = new List<string>();

            var hierarchy = _vsProjectAdapter.VsHierarchy;

            // Get all items in the hierarchy, this includes project references, files, and everything else.
            IEnumHierarchyItems items;
            if (ErrorHandler.Succeeded(itemsFactory.EnumHierarchyItems(
                hierarchy,
                (uint)__VSEHI.VSEHI_Leaf,
                (uint)VSConstants.VSITEMID.Root,
                out items)))
            {
                var buildPropertyStorage = (IVsBuildPropertyStorage)hierarchy;

                // Loop through all items
                uint fetched;
                var item = new VSITEMSELECTION[1];
                while (ErrorHandler.Succeeded(items.Next(1, item, out fetched)) && fetched == 1)
                {
                    // Check if the item has ReferenceOutputAssembly. This will
                    // return null for the vast majority of items.
                    string value;
                    if (ErrorHandler.Succeeded(buildPropertyStorage.GetItemAttribute(
                            item[0].itemid,
                            "ReferenceOutputAssembly",
                            out value))
                        && value != null)
                    {
                        // We only need to go farther if the flag exists and is not true
                        if (!string.Equals(value, bool.TrueString, StringComparison.OrdinalIgnoreCase))
                        {
                            // Get the DTE Project reference for the item id. This checks for nulls incase this is
                            // somehow not a project reference that had the ReferenceOutputAssembly flag.
                            object childObject;
                            if (ErrorHandler.Succeeded(hierarchy.GetProperty(
                                item[0].itemid,
                                (int)__VSHPROPID.VSHPROPID_ExtObject,
                                out childObject)))
                            {
                                // 1. Verify that this is a project reference
                                // 2. Check that it is valid and resolved
                                // 3. Follow the reference to the DTE project and get the unique name
                                var reference = childObject as Reference3;

                                if (IsProjectReference(reference, logger) && IsReferenceResolved(reference, logger))
                                {
                                    var childPath = reference.SourceProject.GetFullProjectPath();
                                    excludedReferences.Add(childPath);
                                }
                            }
                        }
                    }
                }
            }

            return excludedReferences;
        }

        private bool IsProjectReference(Reference3 reference, Common.ILogger logger)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            try
            {
                // Verify that this is a project reference
                return reference != null && reference.SourceProject != null;
            }
            catch (Exception ex)
            {
                logger.LogDebug(ex.ToString());
            }

            return false;
        }

        private bool IsReferenceResolved(Reference3 reference, Common.ILogger logger)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            try
            {
                // Verify that this is a valid and resolved reference
                return reference != null && reference.Resolved;
            }
            catch (Exception ex)
            {
                logger.LogDebug(ex.ToString());
            }

            return false;
        }

        public Task<IEnumerable<LibraryDependency>> GetPackageReferencesAsync(
            NuGetFramework targetFramework, CancellationToken _)
        {
            throw new NotSupportedException();
        }

        public async Task<IReadOnlyList<(string id, string[] metadata)>> GetItemsAsync(string itemTypeName, params string[] metadataNames)
        {
            await _threadingService.JoinableTaskFactory.SwitchToMainThreadAsync();
            return VsManagedLanguagesProjectSystemServices.GetItems(_vsProjectAdapter, itemTypeName, metadataNames);
        }
    }
}
