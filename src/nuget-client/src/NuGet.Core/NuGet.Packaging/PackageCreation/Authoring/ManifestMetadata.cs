// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using NuGet.Packaging.Core;
using NuGet.Packaging.PackageCreation.Resources;
using NuGet.Versioning;

namespace NuGet.Packaging
{
    /// <summary>
    /// Manifest (user created .nuspec) file metadata model
    /// </summary>
    public class ManifestMetadata : IPackageMetadata
    {
        private string _minClientVersionString;

        private IEnumerable<string> _authors = Enumerable.Empty<string>();
        private IEnumerable<string> _owners = Enumerable.Empty<string>();

        private string _iconUrl;
        private string _licenseUrl;
        private string _projectUrl;

        public ManifestMetadata()
        {
        }

        /// <summary>
        /// Constructs a ManifestMetadata instance from an IPackageMetadata instance
        /// </summary>
        public ManifestMetadata(IPackageMetadata copy)
        {
            Id = copy.Id?.Trim();
            Version = copy.Version;
            Title = copy.Title?.Trim();
            Authors = copy.Authors;
            Owners = copy.Owners;
            Tags = string.IsNullOrEmpty(copy.Tags) ? null : copy.Tags.Trim();
            Serviceable = copy.Serviceable;
            _licenseUrl = copy.LicenseUrl?.OriginalString;
            _projectUrl = copy.ProjectUrl?.OriginalString;
            _iconUrl = copy.IconUrl?.OriginalString;
            RequireLicenseAcceptance = copy.RequireLicenseAcceptance;
            EmitRequireLicenseAcceptance = (copy as PackageBuilder)?.EmitRequireLicenseAcceptance ?? true;
            Description = copy.Description?.Trim();
            Copyright = copy.Copyright?.Trim();
            Summary = copy.Summary?.Trim();
            ReleaseNotes = copy.ReleaseNotes?.Trim();
            Language = copy.Language?.Trim();
            DependencyGroups = copy.DependencyGroups;
            FrameworkReferences = copy.FrameworkReferences;
            FrameworkReferenceGroups = copy.FrameworkReferenceGroups;
            PackageAssemblyReferences = copy.PackageAssemblyReferences;
            PackageTypes = copy.PackageTypes;
            MinClientVersionString = copy.MinClientVersion?.ToString();
            ContentFiles = copy.ContentFiles;
            DevelopmentDependency = copy.DevelopmentDependency;
            Repository = copy.Repository;
            LicenseMetadata = copy.LicenseMetadata;
            Icon = copy.Icon;
            Readme = copy.Readme;
        }

        [ManifestVersion(5)]
        public string MinClientVersionString
        {
            get { return _minClientVersionString; }
            set
            {
                Version version = null;
                if (!String.IsNullOrEmpty(value) && !System.Version.TryParse(value, out version))
                {
                    throw new InvalidDataException(NuGetResources.Manifest_InvalidMinClientVersion);
                }

                _minClientVersionString = value;
                MinClientVersion = version;
            }
        }

        public Version MinClientVersion { get; private set; }

        public string Id { get; set; }

        public NuGetVersion Version { get; set; }

        public string Title { get; set; }

        public IEnumerable<string> Authors
        {
            get { return _authors; }
            set { _authors = value ?? Enumerable.Empty<string>(); }
        }

        public IEnumerable<string> Owners
        {
            get { return _owners; }
            set { _owners = value ?? Enumerable.Empty<string>(); }
        }

        // The (Icon/License/Project)Url properties have backing strings as we need to be able to differentiate
        //   between the property not being set (valid) and set to an empty value (invalid).
        public void SetIconUrl(string iconUrl)
        {
            _iconUrl = iconUrl;
        }

        public Uri IconUrl
        {
            get
            {
                if (_iconUrl == null)
                {
                    return null;
                }

                return new Uri(_iconUrl);
            }
        }

        public string Icon { get; set; }

        public void SetLicenseUrl(string licenseUrl)
        {
            _licenseUrl = licenseUrl;
        }

        public Uri LicenseUrl
        {
            get
            {
                if (_licenseUrl == null)
                {
                    return null;
                }

                return new Uri(_licenseUrl);
            }
        }

        public void SetProjectUrl(string projectUrl)
        {
            _projectUrl = projectUrl;
        }

        public Uri ProjectUrl
        {
            get
            {
                if (_projectUrl == null)
                {
                    return null;
                }

                return new Uri(_projectUrl);
            }
        }

        public bool RequireLicenseAcceptance { get; set; }

        public bool EmitRequireLicenseAcceptance { get; set; } = true;

        public bool DevelopmentDependency { get; set; }

        public string Description { get; set; }

        public string Summary { get; set; }

        [ManifestVersion(2)]
        public string ReleaseNotes { get; set; }

        [ManifestVersion(2)]
        public string Copyright { get; set; }

        public string Language { get; set; }

        public string Tags { get; set; }

        public string Readme { get; set; }

        public bool Serviceable { get; set; }

        public RepositoryMetadata Repository { get; set; }

        private IEnumerable<PackageDependencyGroup> _dependencyGroups = [];
        public IEnumerable<PackageDependencyGroup> DependencyGroups
        {
            get
            {
                return _dependencyGroups;
            }
            set
            {
                _dependencyGroups = MergeDependencyGroups(value);
            }
        }

        public IEnumerable<FrameworkReferenceGroup> FrameworkReferenceGroups { get; set; } = [];

        public IEnumerable<FrameworkAssemblyReference> FrameworkReferences { get; set; } = [];

        private IEnumerable<PackageReferenceSet> _packageAssemblyReferences = [];

        [ManifestVersion(2)]
        public IEnumerable<PackageReferenceSet> PackageAssemblyReferences
        {
            get
            {
                return _packageAssemblyReferences;
            }

            set
            {
                _packageAssemblyReferences = MergePackageAssemblyReferences(value);
            }
        }

        private static IEnumerable<PackageReferenceSet> MergePackageAssemblyReferences(IEnumerable<PackageReferenceSet> referenceSets)
        {
            if (referenceSets == null)
            {
                Enumerable.Empty<PackageReferenceSet>();
            }

            var referenceSetGroups = referenceSets.GroupBy(set => set.TargetFramework);
            var groupedReferenceSets = referenceSetGroups.Select(group => new PackageReferenceSet(group.Key, group.SelectMany(g => g.References)))
                                                            .ToList();

            int nullTargetFrameworkIndex = groupedReferenceSets.FindIndex(set => set.TargetFramework == null);
            if (nullTargetFrameworkIndex > -1)
            {
                var nullFxReferenceSet = groupedReferenceSets[nullTargetFrameworkIndex];
                groupedReferenceSets.RemoveAt(nullTargetFrameworkIndex);
                groupedReferenceSets.Insert(0, nullFxReferenceSet);
            }

            return groupedReferenceSets;
        }

        public IEnumerable<ManifestContentFiles> ContentFiles { get; set; } = new List<ManifestContentFiles>();

        public IEnumerable<PackageType> PackageTypes { get; set; } = new List<PackageType>();

        public LicenseMetadata LicenseMetadata { get; set; } = null;

        private static IEnumerable<PackageDependencyGroup> MergeDependencyGroups(IEnumerable<PackageDependencyGroup> actualDependencyGroups)
        {
            if (actualDependencyGroups == null)
            {
                return Enumerable.Empty<PackageDependencyGroup>();
            }

            var dependencyGroups = actualDependencyGroups.Select(CreatePackageDependencyGroup);

            // group the dependency sets with the same target framework together.
            var dependencySetGroups = dependencyGroups.GroupBy(set => set.TargetFramework);
            var groupedDependencySets = dependencySetGroups.Select(group => new PackageDependencyGroup(group.Key, new HashSet<PackageDependency>(group.SelectMany(g => g.Packages))))
                                                            .ToList();
            // move the group with the any target framework (if any) to the front just for nicer display in UI
            int anyTargetFrameworkIndex = groupedDependencySets.FindIndex(set => set.TargetFramework.IsAny);
            if (anyTargetFrameworkIndex > -1)
            {
                var anyFxDependencySet = groupedDependencySets[anyTargetFrameworkIndex];
                groupedDependencySets.RemoveAt(anyTargetFrameworkIndex);
                groupedDependencySets.Insert(0, anyFxDependencySet);
            }

            return groupedDependencySets;
        }

        private static PackageDependencyGroup CreatePackageDependencyGroup(PackageDependencyGroup dependencyGroup)
        {
            ISet<PackageDependency> dependencies;

            if (dependencyGroup.Packages == null)
            {
                dependencies = new HashSet<PackageDependency>();
            }
            else
            {
                var dependenciesList = dependencyGroup.Packages.Select(dependency =>
                    new PackageDependency(
                        dependency.Id.SafeTrim(),
                        dependency.VersionRange,
                        dependency.Include,
                        dependency.Exclude)).ToList();
                dependencies = new HashSet<PackageDependency>(dependenciesList);
            }

            return new PackageDependencyGroup(dependencyGroup.TargetFramework, dependencies);
        }

        /// <summary>
        /// Checks that the metadata in the nuspec is enough to create a package
        /// </summary>
        /// <returns>A iterable collection with the validation error messages</returns>
        /// <remarks>Error codes are not associated with the validation error messages returned</remarks>
        public IEnumerable<string> Validate()
        {
            if (String.IsNullOrEmpty(Id))
            {
                yield return String.Format(CultureInfo.CurrentCulture, NuGetResources.Manifest_RequiredMetadataMissing, "Id");
            }
            else
            {
                if (Id.Length > PackageIdValidator.MaxPackageIdLength)
                {
                    yield return String.Format(CultureInfo.CurrentCulture, NuGetResources.Manifest_IdMaxLengthExceeded);
                }
                else if (!PackageIdValidator.IsValidPackageId(Id))
                {
                    yield return String.Format(CultureInfo.CurrentCulture, NuGetResources.InvalidPackageId, Id);
                }
            }

            if (Version == null)
            {
                yield return String.Format(CultureInfo.CurrentCulture, NuGetResources.Manifest_RequiredMetadataMissing, "Version");
            }

            if ((Authors == null || !Authors.Any(author => !String.IsNullOrEmpty(author))) && !PackageTypes.Contains(PackageType.SymbolsPackage))
            {
                yield return String.Format(CultureInfo.CurrentCulture, NuGetResources.Manifest_RequiredMetadataMissing, "Authors");
            }

            if (String.IsNullOrEmpty(Description))
            {
                yield return String.Format(CultureInfo.CurrentCulture, NuGetResources.Manifest_RequiredMetadataMissing, "Description");
            }

            if (_licenseUrl == String.Empty)
            {
                yield return String.Format(CultureInfo.CurrentCulture, NuGetResources.Manifest_UriCannotBeEmpty, "LicenseUrl");
            }

            if (_iconUrl == String.Empty)
            {
                yield return String.Format(CultureInfo.CurrentCulture, NuGetResources.Manifest_UriCannotBeEmpty, "IconUrl");
            }

            if (_projectUrl == String.Empty)
            {
                yield return String.Format(CultureInfo.CurrentCulture, NuGetResources.Manifest_UriCannotBeEmpty, "ProjectUrl");
            }

            if (Icon == string.Empty)
            {
                yield return NuGetResources.IconMissingRequiredValue;
            }

            if (Readme == string.Empty)
            {
                yield return NuGetResources.ReadmeMissingRequiredValue;
            }

            if (RequireLicenseAcceptance)
            {
                if ((string.IsNullOrWhiteSpace(_licenseUrl) && LicenseMetadata == null))
                {
                    yield return NuGetResources.Manifest_RequireLicenseAcceptanceRequiresLicenseUrl;
                }

                if (!EmitRequireLicenseAcceptance)
                {
                    yield return NuGetResources.Manifest_RequireLicenseAcceptanceRequiresEmit;
                }
            }

            if (_licenseUrl != null && LicenseMetadata != null && (string.IsNullOrWhiteSpace(_licenseUrl) || !LicenseUrl.Equals(LicenseMetadata.LicenseUrl)))
            {
                yield return NuGetResources.Manifest_LicenseUrlCannotBeUsedWithLicenseMetadata;
            }
        }
    }
}
