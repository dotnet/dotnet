// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft;
using NuGet.Commands.Restore;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;
using VSLangProj150;

namespace NuGet.PackageManagement.VisualStudio.Projects;

internal class LegacyProjectAdapter : IProject
{
    private readonly IVsProjectAdapter _projectAdapter;

    public LegacyProjectAdapter(IVsProjectAdapter projectAdapter, VSProject4 project4)
    {
        _projectAdapter = projectAdapter;
        OuterBuild = new TargetFrameworkAdapter(_projectAdapter, project4);
        TargetFrameworks = new Dictionary<string, ITargetFramework>(StringComparer.OrdinalIgnoreCase)
        {
            [""] = OuterBuild
        };
    }

    public string FullPath => _projectAdapter.FullProjectPath;

    public string Directory => _projectAdapter.ProjectDirectory;

    public ITargetFramework OuterBuild { get; }

    public IReadOnlyDictionary<string, ITargetFramework> TargetFrameworks { get; }

    // LegacyProjectAdapter is used only in Visual Studio, where global properties
    // (command line arguments like /p:Property=Value) are not applicable.
    public string? GetGlobalProperty(string propertyName) => null;

    private class TargetFrameworkAdapter : ITargetFramework
    {
        private readonly IVsProjectAdapter _projectAdapter;
        private readonly VSProject4 _project4;

        public TargetFrameworkAdapter(IVsProjectAdapter projectAdapter, VSProject4 project4)
        {
            _projectAdapter = projectAdapter;
            _project4 = project4;
        }

        public IReadOnlyList<IItem> GetItems(string itemType)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            if (!ItemMetadataNames.TryGetValue(itemType, out var metadataNames))
            {
                throw new ArgumentException($"Unknown item type: {itemType}", nameof(itemType));
            }

            if (itemType.Equals(ProjectItems.PackageReference, StringComparison.OrdinalIgnoreCase))
            {
                var packageReferences = GetPackageReferences(metadataNames);
                return packageReferences;
            }
            else if (itemType.Equals(ProjectItems.ProjectReference, StringComparison.OrdinalIgnoreCase))
            {
                var projectReferences = GetProjectReferences(metadataNames);
                return projectReferences;
            }

            var items = _projectAdapter.GetBuildItemInformation(itemType, metadataNames);
            var result = new List<IItem>(items is ICollection collection ? collection.Count : 0);
            foreach (var (itemId, itemMetadata) in items)
            {
                var metadataDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                for (int i = 0; i < metadataNames.Length; i++)
                {
                    string? value = itemMetadata[i];
                    if (!string.IsNullOrEmpty(value))
                    {
                        metadataDictionary[metadataNames[i]] = itemMetadata[i];
                    }
                }

                var itemAdapter = new ItemAdapter
                {
                    Identity = itemId,
                    Metadata = metadataDictionary
                };
                result.Add(itemAdapter);
            }

            return result;
        }

        private IReadOnlyList<IItem> GetPackageReferences(string[] metadataNames)
        {
            if (_project4.PackageReferences is null)
            {
                return [];
            }

            string[]? installedPackages = (string[]?)_project4.PackageReferences.InstalledPackages;

            if (installedPackages is null || installedPackages.Length == 0)
            {
                return [];
            }

            List<IItem> packageReferences = new List<IItem>(installedPackages.Length);
            foreach (var packageId in installedPackages)
            {
                if (_project4.PackageReferences.TryGetReference(packageId, metadataNames, out string version, out var metadataElements, out var metadataValues))
                {
                    Dictionary<string, string> metadataDictionary = ToMetadataDictionary(metadataElements, metadataValues);

                    if (!string.IsNullOrEmpty(version))
                    {
                        metadataDictionary[ProjectBuildProperties.Version] = version;
                    }

                    var itemAdapter = new ItemAdapter
                    {
                        Identity = packageId,
                        Metadata = metadataDictionary
                    };
                    packageReferences.Add(itemAdapter);
                }
            }

            return packageReferences;
        }

        private IReadOnlyList<IItem> GetProjectReferences(string[] metadataNames)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            if (_project4.References is null)
            {
                return [];
            }

            List<IItem> references = new List<IItem>();
            foreach (Reference6 projectReference in _project4.References)
            {
                if (projectReference.SourceProject != null && _projectAdapter.IsSupported(projectReference))
                {
                    Array metadataElements;
                    Array metadataValues;
                    projectReference.GetMetadata(metadataNames, out metadataElements, out metadataValues);
                    var metadataDictionary = ToMetadataDictionary(metadataElements, metadataValues);

                    var itemAdapter = new ItemAdapter
                    {
                        Identity = projectReference.SourceProject.FullName,
                        Metadata = metadataDictionary
                    };

                    references.Add(itemAdapter);
                }
            }

            return references;
        }

        private static Dictionary<string, string> ToMetadataDictionary(Array names, Array values)
        {
            Assumes.True(names.Length == values.Length);

            var dictionary = new Dictionary<string, string>(names.Length, StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < names.Length; i++)
            {
                string? name = names.GetValue(i) as string;
                string? value = values.GetValue(i) as string;
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                {
                    dictionary[name!] = value!;
                }
            }

            return dictionary;
        }

        public string? GetProperty(string propertyName)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            string? value;
            if (FallbackProperties.Contains(propertyName))
            {
#pragma warning disable CS0618 // Type or member is obsolete
                value = _projectAdapter.BuildProperties.GetPropertyValueWithDteFallback(propertyName);
#pragma warning restore CS0618 // Type or member is obsolete
            }
            else
            {
                value = _projectAdapter.BuildProperties.GetPropertyValue(propertyName);
            }

            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }

            if (propertyName.Equals(ProjectBuildProperties.RestoreProjectStyle, StringComparison.OrdinalIgnoreCase))
            {
                return "PackageReference";
            }

            return null;
        }

        // Do not add new properties here. New properties should only use new project system APIs.
        // In fact, ideally we should be migrating these properties
        private static readonly ImmutableHashSet<string> FallbackProperties = [
            ProjectBuildProperties.PackageId,
            ProjectBuildProperties.AssemblyName,
            ProjectBuildProperties.RestorePackagesPath,
            ProjectBuildProperties.RestoreSources,
            ProjectBuildProperties.RestoreAdditionalProjectSources,
            ProjectBuildProperties.RestoreFallbackFolders,
            ProjectBuildProperties.RestoreAdditionalProjectFallbackFolders,
            ProjectBuildProperties.PackageTargetFallback,
            ProjectBuildProperties.AssetTargetFallback,
            ProjectBuildProperties.ManagePackageVersionsCentrally,
            ProjectBuildProperties.RuntimeIdentifier,
            ProjectBuildProperties.RuntimeIdentifiers,
            ProjectBuildProperties.RuntimeSupports,
            ProjectBuildProperties.TreatWarningsAsErrors,
            ProjectBuildProperties.NoWarn,
            ProjectBuildProperties.WarningsAsErrors,
            ProjectBuildProperties.WarningsNotAsErrors,
            ProjectBuildProperties.RestorePackagesWithLockFile,
            ProjectBuildProperties.NuGetLockFilePath,
            ProjectBuildProperties.RestoreLockedMode,
            ProjectBuildProperties.CentralPackageVersionOverrideEnabled,
            ProjectBuildProperties.CentralPackageTransitivePinningEnabled,
            ProjectBuildProperties.MSBuildProjectExtensionsPath,
            ProjectBuildProperties.PackageVersion,
            ProjectBuildProperties.Version,
            ProjectBuildProperties.TargetPlatformIdentifier,
            ProjectBuildProperties.TargetPlatformVersion,
            ProjectBuildProperties.TargetPlatformMinVersion,
            ProjectBuildProperties.TargetFrameworkMoniker,
            ];
    }

    private record ItemAdapter : IItem
    {
        public required string Identity { get; init; }
        internal required IReadOnlyDictionary<string, string> Metadata { get; init; }

        public string? GetMetadata(string name)
        {
            if (Metadata.TryGetValue(name, out var value))
            {
                return value;
            }
            else
            {
                return null;
            }
        }
    }

    private static readonly string[] PackageReferenceMetadataNames =
        [
        "IsImplicitlyDefined",
        "Version",
        "VersionOverride",
        "GeneratePathProperty",
        "Aliases",
        "IncludeAssets",
        "ExcludeAssets",
        "PrivateAssets",
        "NoWarn",
        ];

    private static readonly string[] FrameworkReferenceMetadataNames = ["PrivateAssets"];

    private static readonly string[] VersionOnlyMetadataNames = ["Version"];

    private static readonly string[] ProjectReferenceMetadataNames =
        [
        "ReferenceOutputAssembly",
        "FullPath",
        "ExcludeAssets",
        "IncludeAssets",
        "PrivateAssets",
        ];

    internal static readonly IReadOnlyDictionary<string, string[]> ItemMetadataNames =
        new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
        {
            [ProjectItems.PackageReference] = PackageReferenceMetadataNames,
            [ProjectItems.PrunePackageReference] = VersionOnlyMetadataNames,
            [ProjectItems.PackageDownload] = VersionOnlyMetadataNames,
            [ProjectItems.FrameworkReference] = FrameworkReferenceMetadataNames,
            [ProjectItems.PackageVersion] = VersionOnlyMetadataNames,
            [ProjectItems.NuGetAuditSuppress] = [],
            [ProjectItems.ProjectReference] = ProjectReferenceMetadataNames,
        };
}
