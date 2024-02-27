// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.RuntimeModel;
using NuGet.Versioning;

namespace NuGet.ProjectModel
{
    /// <summary>
    /// Writes out a PackageSpec object graph.
    ///
    /// This is non-private only to facilitate unit testing.
    /// </summary>
    public sealed class PackageSpecWriter
    {
        /// <summary>
        /// Writes a PackageSpec to an <c>NuGet.Common.IObjectWriter</c> instance.
        /// </summary>
        /// <param name="packageSpec">A <c>PackageSpec</c> instance.</param>
        /// <param name="writer">An <c>NuGet.Common.IObjectWriter</c> instance.</param>
        public static void Write(PackageSpec packageSpec, IObjectWriter writer)
        {
            Write(packageSpec, writer, hashing: false, EnvironmentVariableWrapper.Instance);
        }

        internal static void Write(PackageSpec packageSpec, IObjectWriter writer, bool hashing, IEnvironmentVariableReader environmentVariableReader)
        {
            if (packageSpec == null)
            {
                throw new ArgumentNullException(nameof(packageSpec));
            }

            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            SetValue(writer, "title", packageSpec.Title);

#pragma warning disable CS0612 // Type or member is obsolete
            if (!packageSpec.IsDefaultVersion)
            {
                SetValue(writer, "version", packageSpec.Version?.ToFullString());
            }

            SetValue(writer, "description", packageSpec.Description);
            SetArrayValue(writer, "authors", packageSpec.Authors);
            SetValue(writer, "copyright", packageSpec.Copyright);
            SetValue(writer, "language", packageSpec.Language);
            SetArrayValue(writer, "contentFiles", packageSpec.ContentFiles);
            SetDictionaryValue(writer, "packInclude", packageSpec.PackInclude);
            SetPackOptions(writer, packageSpec);
#pragma warning restore CS0612 // Type or member is obsolete
            SetMSBuildMetadata(writer, packageSpec, environmentVariableReader);
#pragma warning disable CS0612 // Type or member is obsolete
            SetDictionaryValues(writer, "scripts", packageSpec.Scripts);
#pragma warning restore CS0612 // Type or member is obsolete


            if (packageSpec.Dependencies.Count > 0)
            {
                SetDependencies(writer, packageSpec.Dependencies);
            }

            SetFrameworks(writer, packageSpec.TargetFrameworks, hashing);

            JsonRuntimeFormat.WriteRuntimeGraph(writer, packageSpec.RuntimeGraph);
        }

        /// <summary>
        /// Writes a PackageSpec to a file.
        /// </summary>
        /// <param name="packageSpec">A <c>PackageSpec</c> instance.</param>
        /// <param name="filePath">A file path to write to.</param>
        public static void WriteToFile(PackageSpec packageSpec, string filePath)
        {
            if (packageSpec == null)
            {
                throw new ArgumentNullException(nameof(packageSpec));
            }

            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException(Strings.ArgumentNullOrEmpty, nameof(filePath));
            }

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            using (var textWriter = new StreamWriter(fileStream))
            using (var jsonWriter = new JsonTextWriter(textWriter))
            using (var writer = new JsonObjectWriter(jsonWriter))
            {
                jsonWriter.Formatting = Formatting.Indented;

                Write(packageSpec, writer);
            }
        }

        private static bool IsMetadataValid(ProjectRestoreMetadata msbuildMetadata)
        {
            if (msbuildMetadata == null)
            {
                return false;
            }

            if (msbuildMetadata.ProjectUniqueName == null && msbuildMetadata.ProjectName == null
                && msbuildMetadata.ProjectPath == null && msbuildMetadata.ProjectJsonPath == null
                && msbuildMetadata.PackagesPath == null && msbuildMetadata.OutputPath == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// This method sets the msbuild metadata that's important for restore. Ensures that frameworks regardless of which way they're stores in the metadata(full name or short tfm name) are written out the same.
        /// </summary>
        private static void SetMSBuildMetadata(IObjectWriter writer, PackageSpec packageSpec, IEnvironmentVariableReader environmentVariableReader)
        {
            var msbuildMetadata = packageSpec.RestoreMetadata;

            if (!IsMetadataValid(msbuildMetadata))
            {
                return;
            }

            bool useMacros = MSBuildStringUtility.IsTrue(environmentVariableReader.GetEnvironmentVariable(MacroStringsUtility.NUGET_ENABLE_EXPERIMENTAL_MACROS));
            var userSettingsDirectory = NuGetEnvironment.GetFolderPath(NuGetFolderPath.UserSettingsDirectory);

            writer.WriteObjectStart(JsonPackageSpecReader.RestoreOptions);

            SetValue(writer, "projectUniqueName", msbuildMetadata.ProjectUniqueName);
            SetValue(writer, "projectName", msbuildMetadata.ProjectName);
            SetValue(writer, "projectPath", msbuildMetadata.ProjectPath);
            SetValue(writer, "projectJsonPath", msbuildMetadata.ProjectJsonPath);
            SetValue(writer, "packagesPath", ApplyMacro(msbuildMetadata.PackagesPath, userSettingsDirectory, useMacros));
            SetValue(writer, "outputPath", msbuildMetadata.OutputPath);

            if (msbuildMetadata.ProjectStyle != ProjectStyle.Unknown)
            {
                SetValue(writer, "projectStyle", msbuildMetadata.ProjectStyle.ToString());
            }

            WriteMetadataBooleans(writer, msbuildMetadata);

            if (useMacros)
            {
                var fallbackFolderCopy = msbuildMetadata.FallbackFolders.ToList();
                var configFilePathsCopy = msbuildMetadata.ConfigFilePaths.ToList();
                MacroStringsUtility.ApplyMacros(fallbackFolderCopy, userSettingsDirectory, MacroStringsUtility.UserMacro, PathUtility.GetStringComparisonBasedOnOS());
                MacroStringsUtility.ApplyMacros(configFilePathsCopy, userSettingsDirectory, MacroStringsUtility.UserMacro, PathUtility.GetStringComparisonBasedOnOS());

                SetArrayValue(writer, "fallbackFolders", fallbackFolderCopy);
                SetArrayValue(writer, "configFilePaths", configFilePathsCopy);
            }
            else
            {
                SetArrayValue(writer, "fallbackFolders", msbuildMetadata.FallbackFolders);
                SetArrayValue(writer, "configFilePaths", msbuildMetadata.ConfigFilePaths);
            }

            // This need to stay the original strings because the nuget.g.targets have conditional imports based on the original framework name
            SetArrayValue(writer, "originalTargetFrameworks", msbuildMetadata.OriginalTargetFrameworks.OrderBy(c => c, StringComparer.Ordinal));

            WriteMetadataSources(writer, msbuildMetadata);
            WriteMetadataFiles(writer, msbuildMetadata);
            WriteMetadataTargetFrameworks(writer, msbuildMetadata);
            SetWarningProperties(writer, msbuildMetadata);

            WriteNuGetLockFileProperties(writer, msbuildMetadata);
            WriteNuGetAuditProperties(writer, msbuildMetadata.RestoreAuditProperties);

            if (msbuildMetadata is PackagesConfigProjectRestoreMetadata pcMsbuildMetadata)
            {
                SetValue(writer, "packagesConfigPath", pcMsbuildMetadata.PackagesConfigPath);
            }

            writer.WriteObjectEnd();
        }

        private static string ApplyMacro(string value, string userSettingsDirectory, bool useMacros)
        {
            if (useMacros)
            {
                return MacroStringsUtility.ApplyMacro(value, userSettingsDirectory, MacroStringsUtility.UserMacro, PathUtility.GetStringComparisonBasedOnOS());
            }
            return value;
        }

        private static void WriteMetadataBooleans(IObjectWriter writer, ProjectRestoreMetadata msbuildMetadata)
        {
            SetValueIfTrue(writer, "crossTargeting", msbuildMetadata.CrossTargeting);
            SetValueIfTrue(writer, "legacyPackagesDirectory", msbuildMetadata.LegacyPackagesDirectory);
            SetValueIfTrue(writer, "validateRuntimeAssets", msbuildMetadata.ValidateRuntimeAssets);
            SetValueIfTrue(writer, "skipContentFileWrite", msbuildMetadata.SkipContentFileWrite);
            SetValueIfTrue(writer, "centralPackageVersionsManagementEnabled", msbuildMetadata.CentralPackageVersionsEnabled);
            SetValueIfTrue(writer, "centralPackageFloatingVersionsEnabled", msbuildMetadata.CentralPackageFloatingVersionsEnabled);
            SetValueIfTrue(writer, "centralPackageVersionOverrideDisabled", msbuildMetadata.CentralPackageVersionOverrideDisabled);
            SetValueIfTrue(writer, "CentralPackageTransitivePinningEnabled", msbuildMetadata.CentralPackageTransitivePinningEnabled);
        }


        private static void WriteNuGetLockFileProperties(IObjectWriter writer, ProjectRestoreMetadata msbuildMetadata)
        {
            if (msbuildMetadata.RestoreLockProperties != null &&
                (!string.IsNullOrEmpty(msbuildMetadata.RestoreLockProperties.RestorePackagesWithLockFile) ||
                 !string.IsNullOrEmpty(msbuildMetadata.RestoreLockProperties.NuGetLockFilePath) ||
                 msbuildMetadata.RestoreLockProperties.RestoreLockedMode))
            {
                writer.WriteObjectStart("restoreLockProperties");

                SetValue(writer, "restorePackagesWithLockFile", msbuildMetadata.RestoreLockProperties.RestorePackagesWithLockFile);
                SetValue(writer, "nuGetLockFilePath", msbuildMetadata.RestoreLockProperties.NuGetLockFilePath);
                SetValueIfTrue(writer, "restoreLockedMode", msbuildMetadata.RestoreLockProperties.RestoreLockedMode);

                writer.WriteObjectEnd();
            }
        }

        private static void WriteNuGetAuditProperties(IObjectWriter writer, RestoreAuditProperties auditProperties)
        {
            if (auditProperties == null) return;

            writer.WriteObjectStart("restoreAuditProperties");

            SetValueIfNotNull(writer, "enableAudit", auditProperties.EnableAudit);
            SetValueIfNotNull(writer, "auditLevel", auditProperties.AuditLevel);
            SetValueIfNotNull(writer, "auditMode", auditProperties.AuditMode);

            writer.WriteObjectEnd();
        }

        private static void WriteMetadataTargetFrameworks(IObjectWriter writer, ProjectRestoreMetadata msbuildMetadata)
        {
            if (msbuildMetadata.TargetFrameworks?.Count > 0)
            {
                writer.WriteObjectStart("frameworks");

                var frameworkNames = new HashSet<string>();
                var frameworkSorter = NuGetFrameworkSorter.Instance;
                foreach (var framework in msbuildMetadata.TargetFrameworks.OrderBy(c => c.FrameworkName, frameworkSorter))
                {
                    var frameworkName = framework.FrameworkName.GetShortFolderName();

                    if (!frameworkNames.Contains(frameworkName))
                    {
                        frameworkNames.Add(frameworkName);

                        writer.WriteObjectStart(frameworkName);

                        SetValueIfNotNull(writer, "targetAlias", framework.TargetAlias);

                        writer.WriteObjectStart("projectReferences");

                        foreach (var project in framework.ProjectReferences.OrderBy(e => e.ProjectPath, PathUtility.GetStringComparerBasedOnOS()))
                        {
                            writer.WriteObjectStart(project.ProjectUniqueName);

                            writer.WriteNameValue("projectPath", project.ProjectPath);

                            if (project.IncludeAssets != LibraryIncludeFlags.All)
                            {
                                writer.WriteNameValue("includeAssets", LibraryIncludeFlagUtils.GetFlagString(project.IncludeAssets));
                            }

                            if (project.ExcludeAssets != LibraryIncludeFlags.None)
                            {
                                writer.WriteNameValue("excludeAssets", LibraryIncludeFlagUtils.GetFlagString(project.ExcludeAssets));
                            }

                            if (project.PrivateAssets != LibraryIncludeFlagUtils.DefaultSuppressParent)
                            {
                                writer.WriteNameValue("privateAssets", LibraryIncludeFlagUtils.GetFlagString(project.PrivateAssets));
                            }

                            writer.WriteObjectEnd();
                        }

                        writer.WriteObjectEnd();
                        writer.WriteObjectEnd();
                    }
                }

                writer.WriteObjectEnd();
            }
        }

        private static void WriteMetadataFiles(IObjectWriter writer, ProjectRestoreMetadata msbuildMetadata)
        {
            if (msbuildMetadata.Files?.Count > 0)
            {
                writer.WriteObjectStart("files");

                foreach (var file in msbuildMetadata.Files)
                {
                    SetValue(writer, file.PackagePath, file.AbsolutePath);
                }

                writer.WriteObjectEnd();
            }
        }

        private static void WriteMetadataSources(IObjectWriter writer, ProjectRestoreMetadata msbuildMetadata)
        {
            if (msbuildMetadata.Sources?.Count > 0)
            {
                writer.WriteObjectStart("sources");

                foreach (var source in msbuildMetadata.Sources.OrderBy(e => e.Source, StringComparer.Ordinal))
                {
                    writer.WriteObjectStart(source.Source);
                    writer.WriteObjectEnd();
                }

                writer.WriteObjectEnd();
            }
        }

        private static void SetWarningProperties(IObjectWriter writer, ProjectRestoreMetadata msbuildMetadata)
        {
            if (msbuildMetadata.ProjectWideWarningProperties != null &&
                (msbuildMetadata.ProjectWideWarningProperties.AllWarningsAsErrors ||
                 msbuildMetadata.ProjectWideWarningProperties.NoWarn.Count > 0 ||
                 msbuildMetadata.ProjectWideWarningProperties.WarningsAsErrors.Count > 0))
            {
                writer.WriteObjectStart("warningProperties");

                SetValueIfTrue(writer, "allWarningsAsErrors", msbuildMetadata.ProjectWideWarningProperties.AllWarningsAsErrors);

                if (msbuildMetadata.ProjectWideWarningProperties.NoWarn.Count > 0)
                {
                    SetArrayValue(writer, "noWarn", msbuildMetadata
                       .ProjectWideWarningProperties
                       .NoWarn
                       .OrderBy(c => c)
                       .Select(c => c.GetName())
                       .Where(c => !string.IsNullOrEmpty(c)));
                }

                if (msbuildMetadata.ProjectWideWarningProperties.WarningsAsErrors.Count > 0)
                {
                    SetArrayValue(writer, "warnAsError", msbuildMetadata
                        .ProjectWideWarningProperties
                        .WarningsAsErrors
                        .OrderBy(c => c)
                        .Select(c => c.GetName())
                        .Where(c => !string.IsNullOrEmpty(c)));
                }

                if (msbuildMetadata.ProjectWideWarningProperties.WarningsNotAsErrors.Count > 0)
                {
                    SetArrayValue(writer, "warnNotAsError", msbuildMetadata
                        .ProjectWideWarningProperties
                        .WarningsNotAsErrors
                        .OrderBy(c => c)
                        .Select(c => c.GetName())
                        .Where(c => !string.IsNullOrEmpty(c)));
                }

                writer.WriteObjectEnd();
            }
        }

        [Obsolete]
        private static void SetPackOptions(IObjectWriter writer, PackageSpec packageSpec)
        {
            var packOptions = packageSpec.PackOptions;
            if (packOptions == null)
            {
                return;
            }

            if ((packageSpec.Owners == null || packageSpec.Owners.Length == 0)
                && (packageSpec.Tags == null || packageSpec.Tags.Length == 0)
                && packageSpec.ProjectUrl == null && packageSpec.IconUrl == null && packageSpec.Summary == null
                && packageSpec.ReleaseNotes == null && packageSpec.LicenseUrl == null
                && !packageSpec.RequireLicenseAcceptance
                && (packOptions.PackageType == null || packOptions.PackageType.Count == 0))
            {
                return;
            }

            writer.WriteObjectStart(JsonPackageSpecReader.PackOptions);

            SetArrayValue(writer, "owners", packageSpec.Owners);
            SetArrayValue(writer, "tags", packageSpec.Tags);
            SetValue(writer, "projectUrl", packageSpec.ProjectUrl);
            SetValue(writer, "iconUrl", packageSpec.IconUrl);
            SetValue(writer, "summary", packageSpec.Summary);
            SetValue(writer, "releaseNotes", packageSpec.ReleaseNotes);
            SetValue(writer, "licenseUrl", packageSpec.LicenseUrl);

            SetValueIfTrue(writer, "requireLicenseAcceptance", packageSpec.RequireLicenseAcceptance);

            if (packOptions.PackageType != null)
            {
                if (packOptions.PackageType.Count == 1)
                {
                    SetValue(writer, JsonPackageSpecReader.PackageType, packOptions.PackageType[0].Name);
                }
                else if (packOptions.PackageType.Count > 1)
                {
                    var packageTypeNames = packOptions.PackageType.Select(p => p.Name);
                    SetArrayValue(writer, JsonPackageSpecReader.PackageType, packageTypeNames);
                }
            }

            writer.WriteObjectEnd();
        }

        private static void SetDependencies(IObjectWriter writer, IList<LibraryDependency> libraryDependencies)
        {
            SetDependencies(writer, "dependencies", libraryDependencies.Where(dependency => dependency.LibraryRange.TypeConstraint != LibraryDependencyTarget.Reference));
            SetDependencies(writer, "frameworkAssemblies", libraryDependencies.Where(dependency => dependency.LibraryRange.TypeConstraint == LibraryDependencyTarget.Reference));
        }

        /// <summary>
        /// This method sorts the libraries based on the name
        /// This method also writes out the normalized versions to avoid cases where original string is set because it was gotten through project system vs being installed from PM UI
        /// </summary>
        internal static void SetDependencies(IObjectWriter writer, string name, IEnumerable<LibraryDependency> libraryDependencies)
        {
            if (!libraryDependencies.Any())
            {
                return;
            }

            writer.WriteObjectStart(name);

            foreach (var dependency in libraryDependencies.OrderBy(e => e.Name, StringComparer.Ordinal))
            {
                var expandedMode = dependency.IncludeType != LibraryIncludeFlags.All
                    || dependency.SuppressParent != LibraryIncludeFlagUtils.DefaultSuppressParent
                    || dependency.AutoReferenced
                    || (dependency.LibraryRange.TypeConstraint != LibraryDependencyTarget.Reference
                        && dependency.LibraryRange.TypeConstraint != (LibraryDependencyTarget.All & ~LibraryDependencyTarget.Reference))
                    || !string.IsNullOrEmpty(dependency.Aliases)
                    || dependency.GeneratePathProperty
                    || dependency.VersionCentrallyManaged;

                var versionRange = dependency.LibraryRange.VersionRange ?? VersionRange.All;
                var versionString = versionRange.ToNormalizedString();

                if (expandedMode)
                {
                    writer.WriteObjectStart(dependency.Name);

                    if (dependency.IncludeType != LibraryIncludeFlags.All)
                    {
                        SetValue(writer, "include", dependency.IncludeType.AsString());
                    }

                    if (dependency.SuppressParent != LibraryIncludeFlagUtils.DefaultSuppressParent)
                    {
                        SetValue(writer, "suppressParent", dependency.SuppressParent.AsString());
                    }

                    if (dependency.LibraryRange.TypeConstraint != LibraryDependencyTarget.Reference
                        && dependency.LibraryRange.TypeConstraint != (LibraryDependencyTarget.All & ~LibraryDependencyTarget.Reference))
                    {
                        SetValue(writer, "target", dependency.LibraryRange.TypeConstraint.AsString());
                    }

                    if (VersionRange.All.Equals(versionRange)
                        && !dependency.LibraryRange.TypeConstraintAllows(LibraryDependencyTarget.Package)
                        && !dependency.LibraryRange.TypeConstraintAllows(LibraryDependencyTarget.Reference)
                        && !dependency.LibraryRange.TypeConstraintAllows(LibraryDependencyTarget.ExternalProject))
                    {
                        // Allow this specific case to skip the version property
                    }
                    else
                    {
                        SetValue(writer, "version", versionString);
                    }

                    if (dependency.VersionOverride != null)
                    {
                        SetValue(writer, "versionOverride", dependency.VersionOverride.ToNormalizedString());
                    }

                    SetValueIfTrue(writer, "autoReferenced", dependency.AutoReferenced);

                    if (dependency.NoWarn.Count > 0)
                    {
                        SetArrayValue(writer, "noWarn", dependency
                            .NoWarn
                            .Distinct()
                            .OrderBy(c => c)
                            .Select(code => code.GetName())
                            .Where(s => !string.IsNullOrEmpty(s)));
                    }

                    SetValueIfTrue(writer, "generatePathProperty", dependency.GeneratePathProperty);
                    SetValueIfTrue(writer, "versionCentrallyManaged", dependency.VersionCentrallyManaged);
                    SetValueIfNotNull(writer, "aliases", dependency.Aliases);
                    writer.WriteObjectEnd();
                }
                else
                {
                    writer.WriteNameValue(dependency.Name, versionString);
                }
            }

            writer.WriteObjectEnd();
        }

        /// <summary>
        /// The central transitive dependecy groups are used for pack operation.
        /// The metadata needed for pack is composed from:
        ///     Name, IncludeType, SuppressParent and Version
        /// </summary>
        internal static void SetCentralTransitveDependencyGroup(IObjectWriter writer, string name, IEnumerable<LibraryDependency> libraryDependencies)
        {
            if (!libraryDependencies.Any())
            {
                return;
            }

            writer.WriteObjectStart(name);

            foreach (var dependency in libraryDependencies.OrderBy(e => e.Name, StringComparer.Ordinal))
            {
                var versionRange = dependency.LibraryRange.VersionRange ?? VersionRange.All;
                var versionString = versionRange.ToNormalizedString();

                writer.WriteObjectStart(dependency.Name);

                if (dependency.IncludeType != LibraryIncludeFlags.All)
                {
                    SetValue(writer, "include", dependency.IncludeType.ToString());
                }

                if (dependency.SuppressParent != LibraryIncludeFlagUtils.DefaultSuppressParent)
                {
                    SetValue(writer, "suppressParent", dependency.SuppressParent.ToString());
                }

                SetValue(writer, "version", versionString);

                writer.WriteObjectEnd();
            }

            writer.WriteObjectEnd();
        }

        private static void SetImports(IObjectWriter writer, IList<NuGetFramework> frameworks)
        {
            if (frameworks?.Count > 0)
            {
                var imports = frameworks.Select(framework => framework.GetShortFolderName());

                writer.WriteNameArray("imports", imports);
            }
        }

        private static void SetDownloadDependencies(IObjectWriter writer, IList<DownloadDependency> downloadDependencies)
        {
            if (!(downloadDependencies.Count > 0))
            {
                return;
            }

            writer.WriteArrayStart("downloadDependencies");

            foreach (var dependency in downloadDependencies.GroupBy(dep => dep.Name).OrderBy(dep => dep.Key))
            {
                var version = string.Join(";", dependency.Select(dep => dep.VersionRange).OrderBy(dep => dep.MinVersion).Select(dep => dep.ToNormalizedString()));

                writer.WriteObjectStart();
                SetValue(writer, "name", dependency.Key);
                SetValue(writer, "version", version);
                writer.WriteObjectEnd();

            }
            writer.WriteArrayEnd();
        }

        private static void SetFrameworks(IObjectWriter writer, IList<TargetFrameworkInformation> frameworks, bool hashing)
        {
            if (frameworks.Count > 0)
            {
                writer.WriteObjectStart("frameworks");
                var frameworkSorter = NuGetFrameworkSorter.Instance;
                foreach (var framework in frameworks.OrderBy(c => c.FrameworkName, frameworkSorter))
                {
                    writer.WriteObjectStart(framework.FrameworkName.GetShortFolderName());
                    SetValueIfNotNull(writer, "targetAlias", framework.TargetAlias);
                    SetDependencies(writer, framework.Dependencies);
                    SetCentralDependencies(writer, framework.CentralPackageVersions.Values, hashing);
                    SetImports(writer, framework.Imports);
                    SetValueIfTrue(writer, "assetTargetFallback", framework.AssetTargetFallback);
                    SetValueIfNotNull(writer, "secondaryFramework",
                        (DeconstructFallbackFrameworks(framework.FrameworkName) as DualCompatibilityFramework)?.SecondaryFramework.GetShortFolderName());
                    SetValueIfTrue(writer, "warn", framework.Warn);
                    SetDownloadDependencies(writer, framework.DownloadDependencies);
                    SetFrameworkReferences(writer, framework.FrameworkReferences);
                    SetValueIfNotNull(writer, "runtimeIdentifierGraphPath", framework.RuntimeIdentifierGraphPath);
                    writer.WriteObjectEnd();
                }

                writer.WriteObjectEnd();
            }
        }

        // An AssetTargetFallbackFramework or FallbackFramework *might* have a DualCompatibilityFramework as it's root.
        private static NuGetFramework DeconstructFallbackFrameworks(NuGetFramework nuGetFramework)
        {
            if (nuGetFramework is AssetTargetFallbackFramework assetTargetFallbackFramework)
            {
                return assetTargetFallbackFramework.RootFramework;
            }

            if (nuGetFramework is FallbackFramework fallbackFramework)
            {
                return fallbackFramework;
            }

            return nuGetFramework;
        }

        private static void SetFrameworkReferences(IObjectWriter writer, ISet<FrameworkDependency> frameworkReferences)
        {
            if (frameworkReferences?.Count > 0)
            {
                writer.WriteObjectStart("frameworkReferences");

                foreach (var dependency in frameworkReferences.OrderBy(dep => dep))
                {
                    writer.WriteObjectStart(dependency.Name);
                    SetValue(writer, "privateAssets", FrameworkDependencyFlagsUtils.GetFlagString(dependency.PrivateAssets));
                    writer.WriteObjectEnd();
                }
                writer.WriteObjectEnd();
            }
        }

        private static void SetCentralDependencies(IObjectWriter writer, ICollection<CentralPackageVersion> centralPackageVersions, bool hashing)
        {
            if (!(centralPackageVersions.Count > 0))
            {
                return;
            }

            writer.WriteObjectStart("centralPackageVersions");

            if (hashing)
            {
                foreach (var dependency in centralPackageVersions)
                {
                    writer.WriteNameValue(name: dependency.Name, value: dependency.VersionRange.OriginalString ?? dependency.VersionRange.ToNormalizedString());
                }
            }
            else
            {
                foreach (var dependency in centralPackageVersions.OrderBy(dep => dep.Name, StringComparer.OrdinalIgnoreCase))
                {
                    writer.WriteNameValue(name: dependency.Name, value: dependency.VersionRange.OriginalString ?? dependency.VersionRange.ToNormalizedString());
                }
            }

            writer.WriteObjectEnd();
        }

        private static void SetValueIfTrue(IObjectWriter writer, string name, bool value)
        {
            if (value)
            {
                writer.WriteNameValue(name, value);
            }
        }

        private static void SetValueIfNotNull(IObjectWriter writer, string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                writer.WriteNameValue(name, value);
            }
        }

        private static void SetValue(IObjectWriter writer, string name, string value)
        {
            if (value != null)
            {
                writer.WriteNameValue(name, value);
            }
        }

        private static void SetArrayValue(IObjectWriter writer, string name, IEnumerable<string> values)
        {
            if (values != null)
            {
                writer.WriteNonEmptyNameArray(name, values);
            }
        }

        private static void SetDictionaryValue(IObjectWriter writer, string name, IDictionary<string, string> values)
        {
            if (values != null && values.Count > 0)
            {
                writer.WriteObjectStart(name);

                var sortedValues = values.OrderBy(pair => pair.Key, StringComparer.Ordinal);

                foreach (var pair in sortedValues)
                {
                    writer.WriteNameValue(pair.Key, pair.Value);
                }

                writer.WriteObjectEnd();
            }
        }

        private static void SetDictionaryValues(IObjectWriter writer, string name, IDictionary<string, IEnumerable<string>> values)
        {
            if (values != null && values.Count > 0)
            {
                writer.WriteObjectStart(name);

                var sortedValues = values.OrderBy(pair => pair.Key, StringComparer.Ordinal);

                foreach (var pair in sortedValues)
                {
                    writer.WriteNameArray(pair.Key, pair.Value);
                }

                writer.WriteObjectEnd();
            }
        }
    }
}
