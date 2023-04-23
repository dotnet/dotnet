// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Shared;

namespace NuGet.ProjectModel
{
    public class ProjectRestoreMetadata : IEquatable<ProjectRestoreMetadata>
    {
        /// <summary>
        /// Restore behavior type.
        /// </summary>
        public ProjectStyle ProjectStyle { get; set; } = ProjectStyle.Unknown;

        /// <summary>
        /// MSBuild project file path.
        /// </summary>
        public string ProjectPath { get; set; }

        /// <summary>
        /// Full path to the project.json file if it exists.
        /// </summary>
        public string ProjectJsonPath { get; set; }

        /// <summary>
        /// Assets file output path.
        /// </summary>
        public string OutputPath { get; set; }

        /// <summary>
        /// Friendly project name.
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Name unique to the project across the solution.
        /// </summary>
        public string ProjectUniqueName { get; set; }

        /// <summary>
        /// Package feed sources.
        /// </summary>
        public IList<PackageSource> Sources { get; set; } = new List<PackageSource>();

        /// <summary>
        /// User packages folder path.
        /// </summary>
        public string PackagesPath { get; set; }

        /// <summary>
        /// Cache file path
        /// </summary>
        public string CacheFilePath { get; set; }

        /// <summary>
        /// Fallback folders.
        /// </summary>
        public IList<string> FallbackFolders { get; set; } = new List<string>();

        /// <summary>
        /// ConfigFilePaths used.
        /// </summary>
        public IList<string> ConfigFilePaths { get; set; } = new List<string>();

        /// <summary>
        /// Framework specific metadata, this may be a subset of the project's frameworks.
        /// Operations to determine the nearest framework should be done against the project's frameworks,
        /// and then matched directly to this section.
        /// </summary>
        public IList<ProjectRestoreMetadataFrameworkInfo> TargetFrameworks { get; set; } = new List<ProjectRestoreMetadataFrameworkInfo>();

        /// <summary>
        /// Original target frameworks strings. These are used to match msbuild conditionals to $(TargetFramework)
        /// </summary>
        public IList<string> OriginalTargetFrameworks { get; set; } = new List<string>();

        /// <summary>
        /// True if $(TargetFrameworks) is used and the build is using Cross Targeting.
        /// </summary>
        public bool CrossTargeting { get; set; }

        /// <summary>
        /// Whether or not to restore the packages directory using the legacy format, which write original case paths
        /// instead of lowercase.
        /// </summary>
        public bool LegacyPackagesDirectory { get; set; }

        /// <summary>
        /// Asset files. These should be equivalent to the files that would be
        /// in the nupkg after packing the project.
        /// </summary>
        public IList<ProjectRestoreMetadataFile> Files { get; set; } = new List<ProjectRestoreMetadataFile>();

        /// <summary>
        /// Compatibility check for runtime framework assets.
        /// </summary>
        public bool ValidateRuntimeAssets { get; set; }

        /// <summary>
        /// True if this is a Legacy Package Reference project
        /// </summary>
        public bool SkipContentFileWrite { get; set; }

        /// <summary>
        /// Contains Project wide properties for Warnings.
        /// </summary>
        public WarningProperties ProjectWideWarningProperties { get; set; } = new WarningProperties();

        public RestoreLockProperties RestoreLockProperties { get; set; } = new RestoreLockProperties();

        /// <summary>
        /// Gets or sets a value indicating whether or not central package management is enabled.
        /// </summary>
        public bool CentralPackageVersionsEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not a package version specified centrally can be overridden.
        /// </summary>
        public bool CentralPackageVersionOverrideDisabled { get; set; }

        public bool CentralPackageTransitivePinningEnabled { get; set; }

        public RestoreAuditProperties RestoreAuditProperties { get; set; }

        public override int GetHashCode()
        {
            var hashCode = new HashCodeCombiner();

            hashCode.AddStruct(ProjectStyle);

            StringComparer osStringComparer = PathUtility.GetStringComparerBasedOnOS();
            if (ProjectPath != null)
            {
                hashCode.AddObject(osStringComparer.GetHashCode(ProjectPath));
            }
            if (ProjectJsonPath != null)
            {
                hashCode.AddObject(osStringComparer.GetHashCode(ProjectJsonPath));
            }
            if (OutputPath != null)
            {
                hashCode.AddObject(osStringComparer.GetHashCode(OutputPath));
            }
            if (ProjectName != null)
            {
                hashCode.AddObject(osStringComparer.GetHashCode(ProjectName));
            }
            if (ProjectUniqueName != null)
            {
                hashCode.AddObject(osStringComparer.GetHashCode(ProjectUniqueName));
            }
            hashCode.AddSequence(Sources.OrderBy(e => e.Source, StringComparer.OrdinalIgnoreCase));
            if (PackagesPath != null)
            {
                hashCode.AddObject(osStringComparer.GetHashCode(PackagesPath));
            }
            foreach (var reference in ConfigFilePaths.OrderBy(s => s, osStringComparer))
            {
                hashCode.AddObject(osStringComparer.GetHashCode(reference));
            }
            foreach (var reference in FallbackFolders.OrderBy(s => s, osStringComparer))
            {
                hashCode.AddObject(osStringComparer.GetHashCode(reference));
            }
            hashCode.AddSequence(TargetFrameworks.OrderBy(dep => dep.TargetAlias, StringComparer.OrdinalIgnoreCase));
            foreach (var reference in OriginalTargetFrameworks.OrderBy(s => s, StringComparer.OrdinalIgnoreCase))
            {
                hashCode.AddObject(StringComparer.OrdinalIgnoreCase.GetHashCode(reference));
            }
            hashCode.AddObject(CrossTargeting);
            hashCode.AddObject(LegacyPackagesDirectory);
            hashCode.AddSequence(Files);
            hashCode.AddObject(ValidateRuntimeAssets);
            hashCode.AddObject(SkipContentFileWrite);
            hashCode.AddObject(ProjectWideWarningProperties);
            hashCode.AddObject(RestoreLockProperties);
            hashCode.AddObject(CentralPackageVersionsEnabled);
            hashCode.AddObject(CentralPackageVersionOverrideDisabled);
            hashCode.AddObject(CentralPackageTransitivePinningEnabled);
            hashCode.AddObject(RestoreAuditProperties);

            return hashCode.CombinedHash;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ProjectRestoreMetadata);
        }

        public bool Equals(ProjectRestoreMetadata other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            StringComparer osStringComparer = PathUtility.GetStringComparerBasedOnOS();
            return ProjectStyle == other.ProjectStyle &&
                   osStringComparer.Equals(ProjectPath, other.ProjectPath) &&
                   osStringComparer.Equals(ProjectJsonPath, other.ProjectJsonPath) &&
                   osStringComparer.Equals(OutputPath, other.OutputPath) &&
                   osStringComparer.Equals(ProjectName, other.ProjectName) &&
                   osStringComparer.Equals(ProjectUniqueName, other.ProjectUniqueName) &&
                   Sources.OrderedEquals(other.Sources.Distinct(), source => source.Source, StringComparer.OrdinalIgnoreCase) &&
                   osStringComparer.Equals(PackagesPath, other.PackagesPath) &&
                   ConfigFilePaths.OrderedEquals(other.ConfigFilePaths, filePath => filePath, osStringComparer, osStringComparer) &&
                   FallbackFolders.OrderedEquals(other.FallbackFolders, fallbackFolder => fallbackFolder, osStringComparer, osStringComparer) &&
                   EqualityUtility.OrderedEquals(TargetFrameworks, other.TargetFrameworks, dep => dep.TargetAlias, StringComparer.OrdinalIgnoreCase) &&
                   OriginalTargetFrameworks.OrderedEquals(other.OriginalTargetFrameworks, fw => fw, StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase) &&
                   CrossTargeting == other.CrossTargeting &&
                   LegacyPackagesDirectory == other.LegacyPackagesDirectory &&
                   ValidateRuntimeAssets == other.ValidateRuntimeAssets &&
                   SkipContentFileWrite == other.SkipContentFileWrite &&
                   EqualityUtility.SequenceEqualWithNullCheck(Files, other.Files) &&
                   EqualityUtility.EqualsWithNullCheck(ProjectWideWarningProperties, other.ProjectWideWarningProperties) &&
                   EqualityUtility.EqualsWithNullCheck(RestoreLockProperties, other.RestoreLockProperties) &&
                   EqualityUtility.EqualsWithNullCheck(CentralPackageVersionsEnabled, other.CentralPackageVersionsEnabled) &&
                   EqualityUtility.EqualsWithNullCheck(CentralPackageVersionOverrideDisabled, other.CentralPackageVersionOverrideDisabled) &&
                   EqualityUtility.EqualsWithNullCheck(CentralPackageTransitivePinningEnabled, other.CentralPackageTransitivePinningEnabled) &&
                   RestoreAuditProperties == other.RestoreAuditProperties;
        }

        public virtual ProjectRestoreMetadata Clone()
        {
            var clone = new ProjectRestoreMetadata();
            FillClone(clone);
            return clone;
        }

        protected void FillClone(ProjectRestoreMetadata clone)
        {
            clone.ProjectStyle = ProjectStyle;
            clone.ProjectPath = ProjectPath;
            clone.ProjectJsonPath = ProjectJsonPath;
            clone.OutputPath = OutputPath;
            clone.ProjectName = ProjectName;
            clone.ProjectUniqueName = ProjectUniqueName;
            clone.PackagesPath = PackagesPath;
            clone.CacheFilePath = CacheFilePath;
            clone.CrossTargeting = CrossTargeting;
            clone.LegacyPackagesDirectory = LegacyPackagesDirectory;
            clone.SkipContentFileWrite = SkipContentFileWrite;
            clone.ValidateRuntimeAssets = ValidateRuntimeAssets;
            clone.FallbackFolders = FallbackFolders != null ? new List<string>(FallbackFolders) : null;
            clone.ConfigFilePaths = ConfigFilePaths != null ? new List<string>(ConfigFilePaths) : null;
            clone.OriginalTargetFrameworks = OriginalTargetFrameworks != null ? new List<string>(OriginalTargetFrameworks) : null;
            clone.Sources = Sources?.Select(c => c.Clone()).ToList();
            clone.TargetFrameworks = TargetFrameworks?.Select(c => c.Clone()).ToList();
            clone.Files = Files?.Select(c => c.Clone()).ToList();
            clone.ProjectWideWarningProperties = ProjectWideWarningProperties?.Clone();
            clone.RestoreLockProperties = RestoreLockProperties?.Clone();
            clone.CentralPackageVersionsEnabled = CentralPackageVersionsEnabled;
            clone.CentralPackageVersionOverrideDisabled = CentralPackageVersionOverrideDisabled;
            clone.CentralPackageTransitivePinningEnabled = CentralPackageTransitivePinningEnabled;
            clone.RestoreAuditProperties = RestoreAuditProperties?.Clone();
        }
    }
}
