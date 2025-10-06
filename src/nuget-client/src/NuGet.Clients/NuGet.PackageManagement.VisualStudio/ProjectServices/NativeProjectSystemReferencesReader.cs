// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft;
using Microsoft.VisualStudio.Threading;
using Microsoft.VisualStudio.VCProjectEngine;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// Reference reader implementation for the native project system. C++ does implement CPS,
    /// but some of those APIs sometimes lead to a lot more work than expected.
    /// </summary>
    internal class NativeProjectSystemReferencesReader
        : IProjectSystemReferencesReader
    {
        private readonly IVsProjectAdapter _vsProjectAdapter;
        private readonly IVsProjectThreadingService _threadingService;
        private readonly AsyncLazy<VCProject> _vcProject;

        public NativeProjectSystemReferencesReader(
            IVsProjectAdapter vsProjectAdapter,
            IVsProjectThreadingService threadingService)
        {
            Assumes.Present(vsProjectAdapter);
            Assumes.Present(threadingService);

            _vsProjectAdapter = vsProjectAdapter;
            _threadingService = threadingService;
            _vcProject = new AsyncLazy<VCProject>(async () =>
            {
                await threadingService.JoinableTaskFactory.SwitchToMainThreadAsync();
                return _vsProjectAdapter.Project.Object as VCProject;
            }, threadingService.JoinableTaskFactory);
        }

        public async Task<IEnumerable<ProjectRestoreReference>> GetProjectReferencesAsync(
            Common.ILogger logger, CancellationToken _)
        {
            var results = new List<ProjectRestoreReference>();

            await _threadingService.JoinableTaskFactory.SwitchToMainThreadAsync();
            var vcProject = await _vcProject.GetValueAsync();
            var references = vcProject.VCReferences as VCReferences;
            var projectReferences = references.GetReferencesOfType((uint)vcRefType.VCRT_PROJECT);
            bool hasMissingReferences = false;

            foreach (var reference in projectReferences)
            {
                try
                {
                    var vcReference = reference as VCProjectReference;
                    if (vcReference.UseInBuild)
                    {
                        if (vcReference.ReferencedProject != null)
                        {
                            var referencedProject = vcReference.ReferencedProject as Project;
                            var childProjectPath = referencedProject.FileName;
                            var projectRestoreReference = new ProjectRestoreReference()
                            {
                                ProjectPath = childProjectPath,
                                ProjectUniqueName = childProjectPath
                            };

                            results.Add(projectRestoreReference);
                        }
                        else
                        {
                            hasMissingReferences = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    hasMissingReferences = true;

                    logger.LogDebug(ex.ToString());
                }
            }

            if (hasMissingReferences)
            {
                // Log a generic message once per project if any items could not be resolved.
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.UnresolvedItemDuringProjectClosureWalk,
                    _vsProjectAdapter.UniqueName);

                logger.LogVerbose(message);
            }

            return results;
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
