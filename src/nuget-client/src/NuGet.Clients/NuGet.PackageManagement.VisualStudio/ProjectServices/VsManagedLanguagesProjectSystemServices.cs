// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.VisualStudio.Shell;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;
using NuGet.Versioning;
using NuGet.VisualStudio;
using VSLangProj150;
using Task = System.Threading.Tasks.Task;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// Contains the information specific to a Visual Basic or C# project.
    /// </summary>
    internal class VsManagedLanguagesProjectSystemServices :
        INuGetProjectServices
        , IProjectSystemCapabilities
        , IProjectSystemReferencesReader
        , IProjectSystemReferencesService
    {
        private static readonly string[] ReferenceMetadata;

        private readonly IVsProjectAdapter _vsProjectAdapter;
        private readonly IVsProjectThreadingService _threadingService;
        private readonly VSProject4 _vsProject4;

        public bool SupportsPackageReferences => true;

        public bool NominatesOnSolutionLoad { get; private set; } = false;

        #region INuGetProjectServices

        public IProjectBuildProperties BuildProperties => _vsProjectAdapter.BuildProperties;

        public IProjectSystemCapabilities Capabilities => this;

        public IProjectSystemReferencesReader ReferencesReader => this;

        public IProjectSystemReferencesService References => this;

        public IProjectSystemService ProjectSystem => throw new NotSupportedException();

        public IProjectScriptHostService ScriptService { get; }

        #endregion INuGetProjectServices

        static VsManagedLanguagesProjectSystemServices()
        {
            ReferenceMetadata = new string[]
            {
                ProjectItemProperties.IncludeAssets,
                ProjectItemProperties.ExcludeAssets,
                ProjectItemProperties.PrivateAssets,
                ProjectItemProperties.NoWarn,
                ProjectItemProperties.GeneratePathProperty,
                ProjectItemProperties.Aliases,
                ProjectItemProperties.VersionOverride,
                ProjectItemProperties.IsImplicitlyDefined,
            };
        }

        public VsManagedLanguagesProjectSystemServices(
            IVsProjectAdapter vsProjectAdapter,
            IVsProjectThreadingService threadingService,
            VSProject4 vsProject4,
            bool nominatesOnSolutionLoad,
            Lazy<IScriptExecutor> scriptExecutor)
        {
            Assumes.Present(vsProjectAdapter);
            Assumes.Present(threadingService);
            Assumes.Present(vsProject4);

            _vsProjectAdapter = vsProjectAdapter;
            _threadingService = threadingService;
            _vsProject4 = vsProject4;

            ScriptService = new VsProjectScriptHostService(vsProjectAdapter, scriptExecutor);

            NominatesOnSolutionLoad = nominatesOnSolutionLoad;
        }

        public async Task<IEnumerable<LibraryDependency>> GetPackageReferencesAsync(
            NuGetFramework targetFramework, CancellationToken _)
        {
            Assumes.Present(targetFramework);

            await _threadingService.JoinableTaskFactory.SwitchToMainThreadAsync();

            var installedPackages = _vsProject4.PackageReferences?.InstalledPackages;

            if (installedPackages == null)
            {
                return Array.Empty<LibraryDependency>();
            }

            bool isCpvmEnabled = await IsCentralPackageManagementVersionsEnabledAsync();

            var references = installedPackages
                .Cast<string>()
                .Where(r => !string.IsNullOrEmpty(r))
                .Select(installedPackage =>
                {
                    if (_vsProject4.PackageReferences.TryGetReference(
                        installedPackage,
                        ReferenceMetadata,
                        out var version,
                        out var metadataElements,
                        out var metadataValues))
                    {
                        return new PackageReference(
                            name: installedPackage,
                            version: version,
                            metadataElements: metadataElements,
                            metadataValues: metadataValues,
                            targetNuGetFramework: targetFramework);
                    }

                    return null;
                })
                .Where(p => p != null)
                .Select(p => ToPackageLibraryDependency(p, isCpvmEnabled));

            return references.ToList();
        }

        public async Task<IEnumerable<ProjectRestoreReference>> GetProjectReferencesAsync(
            ILogger _, CancellationToken __)
        {
            await _threadingService.JoinableTaskFactory.SwitchToMainThreadAsync();

            if (_vsProject4.References == null)
            {
                return Array.Empty<ProjectRestoreReference>();
            }

            var references = new List<ProjectRestoreReference>();
            foreach (Reference6 r in _vsProject4.References.Cast<Reference6>())
            {
                if (r.SourceProject != null && await EnvDTEProjectUtility.IsSupportedAsync(r.SourceProject))
                {
                    Array metadataElements;
                    Array metadataValues;
                    r.GetMetadata(ReferenceMetadata, out metadataElements, out metadataValues);

                    references.Add(ToProjectRestoreReference(new ProjectReference(
                        uniqueName: r.SourceProject.FullName,
                        metadataElements: metadataElements,
                        metadataValues: metadataValues)));
                }
            }

            return references;
        }

        private static ProjectRestoreReference ToProjectRestoreReference(ProjectReference item)
        {
            var reference = new ProjectRestoreReference
            {
                ProjectUniqueName = item.UniqueName,
                ProjectPath = item.UniqueName
            };

            MSBuildRestoreUtility.ApplyIncludeFlags(
                reference,
                GetReferenceMetadataValue(item, ProjectItemProperties.IncludeAssets),
                GetReferenceMetadataValue(item, ProjectItemProperties.ExcludeAssets),
                GetReferenceMetadataValue(item, ProjectItemProperties.PrivateAssets));

            return reference;
        }

        private static string GetReferenceMetadataValue(ProjectReference reference, string metadataElement)
        {
            Assumes.Present(reference);
            Assumes.NotNullOrEmpty(metadataElement);

            if (reference.MetadataElements == null || reference.MetadataValues == null)
            {
                return string.Empty; // no metadata for package
            }

            var index = Array.IndexOf(reference.MetadataElements, metadataElement);
            if (index >= 0)
            {
                return reference.MetadataValues.GetValue(index) as string;
            }

            return string.Empty;
        }

        private static LibraryDependency ToPackageLibraryDependency(PackageReference reference, bool isCpvmEnabled)
        {
            var dependency = new LibraryDependency
            {
                AutoReferenced = MSBuildStringUtility.IsTrue(GetReferenceMetadataValue(reference, ProjectItemProperties.IsImplicitlyDefined)),
                GeneratePathProperty = MSBuildStringUtility.IsTrue(GetReferenceMetadataValue(reference, ProjectItemProperties.GeneratePathProperty)),
                Aliases = GetReferenceMetadataValue(reference, ProjectItemProperties.Aliases, defaultValue: null),
                VersionOverride = GetVersionOverride(reference),
                LibraryRange = new LibraryRange(
                    name: reference.Name,
                    versionRange: ToVersionRange(reference.Version, isCpvmEnabled),
                    typeConstraint: LibraryDependencyTarget.Package)
            };

            MSBuildRestoreUtility.ApplyIncludeFlags(
                dependency,
                GetReferenceMetadataValue(reference, ProjectItemProperties.IncludeAssets),
                GetReferenceMetadataValue(reference, ProjectItemProperties.ExcludeAssets),
                GetReferenceMetadataValue(reference, ProjectItemProperties.PrivateAssets));


            // Add warning suppressions
            foreach (var code in MSBuildStringUtility.GetNuGetLogCodes(GetReferenceMetadataValue(reference, ProjectItemProperties.NoWarn)))
            {
                dependency.NoWarn.Add(code);
            }

            return dependency;
        }

        private static VersionRange ToVersionRange(string version, bool isCpvmEnabled)
        {
            if (string.IsNullOrEmpty(version))
            {
                if (isCpvmEnabled)
                {
                    // Projects that have their packages managed centrally will not have Version metadata on PackageReference items.
                    return null;
                }
                else
                {
                    return VersionRange.All;
                }
            }

            return VersionRange.Parse(version);
        }

        private static VersionRange GetVersionOverride(PackageReference reference)
        {
            Assumes.Present(reference);

            string versionOverride = GetReferenceMetadataValue(reference, ProjectItemProperties.VersionOverride, defaultValue: null);

            if (string.IsNullOrWhiteSpace(versionOverride))
            {
                return null;
            }

            return VersionRange.Parse(versionOverride);
        }

        private static string GetReferenceMetadataValue(PackageReference reference, string metadataElement, string defaultValue = "")
        {
            Assumes.Present(reference);
            Assumes.NotNullOrEmpty(metadataElement);

            if (reference.MetadataElements == null || reference.MetadataValues == null)
            {
                return defaultValue; // no metadata for package
            }

            var index = Array.IndexOf(reference.MetadataElements, metadataElement);
            if (index >= 0)
            {
                return reference.MetadataValues.GetValue(index) as string;
            }

            return defaultValue;
        }

        public async Task AddOrUpdatePackageReferenceAsync(LibraryDependency packageReference, CancellationToken _)
        {
            Assumes.Present(packageReference);

            await _threadingService.JoinableTaskFactory.SwitchToMainThreadAsync();

            var includeFlags = packageReference.IncludeType;
            var privateAssetsFlag = packageReference.SuppressParent;
            var metadataElements = new List<string>();
            var metadataValues = new List<string>();
            if (includeFlags != LibraryIncludeFlags.All)
            {
                metadataElements.Add(ProjectItemProperties.IncludeAssets);
                metadataValues.Add(LibraryIncludeFlagUtils.GetFlagString(includeFlags).Replace(',', ';'));
            }

            if (privateAssetsFlag != LibraryIncludeFlagUtils.DefaultSuppressParent)
            {
                metadataElements.Add(ProjectItemProperties.PrivateAssets);
                metadataValues.Add(LibraryIncludeFlagUtils.GetFlagString(privateAssetsFlag).Replace(',', ';'));
            }

            AddOrUpdatePackageReference(
                packageReference.Name,
                packageReference.LibraryRange.VersionRange,
                metadataElements.ToArray(),
                metadataValues.ToArray());
        }

        private void AddOrUpdatePackageReference(string packageName, VersionRange packageVersion, string[] metadataElements, string[] metadataValues)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            // Note that API behavior is:
            // - specify a metadata element name with a value => add/replace that metadata item on the package reference
            // - specify a metadata element name with no value => remove that metadata item from the project reference
            // - don't specify a particular metadata name => if it exists on the package reference, don't change it (e.g. for user defined metadata)
            _vsProject4.PackageReferences.AddOrUpdate(
                packageName,
                packageVersion.OriginalString ?? packageVersion.ToShortString(),
                metadataElements,
                metadataValues);
        }

        public async Task RemovePackageReferenceAsync(string packageName)
        {
            Assumes.NotNullOrEmpty(packageName);

            await _threadingService.JoinableTaskFactory.SwitchToMainThreadAsync();

            _vsProject4.PackageReferences.Remove(packageName);
        }

        private async Task<bool> IsCentralPackageManagementVersionsEnabledAsync()
        {
            return MSBuildStringUtility.IsTrue(await _vsProjectAdapter.BuildProperties.GetPropertyValueAsync(ProjectBuildProperties.ManagePackageVersionsCentrally));
        }

        private class ProjectReference
        {
            public ProjectReference(string uniqueName, Array metadataElements, Array metadataValues)
            {
                UniqueName = uniqueName;
                MetadataElements = metadataElements;
                MetadataValues = metadataValues;
            }

            public string UniqueName { get; }
            public Array MetadataElements { get; }
            public Array MetadataValues { get; }
        }

        private class PackageReference
        {
            public PackageReference(
                string name,
                string version,
                Array metadataElements,
                Array metadataValues,
                NuGetFramework targetNuGetFramework)
            {
                Name = name;
                Version = version;
                MetadataElements = metadataElements;
                MetadataValues = metadataValues;
                TargetNuGetFramework = targetNuGetFramework;
            }

            public string Name { get; }
            public string Version { get; }
            public string VersionOverride { get; }
            public Array MetadataElements { get; }
            public Array MetadataValues { get; }
            public NuGetFramework TargetNuGetFramework { get; }
        }
    }
}
