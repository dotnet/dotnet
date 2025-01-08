// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;
using NuGet.RuntimeModel;
using NuGet.Shared;
using NuGet.Versioning;
using NuGet.VisualStudio;
using static NuGet.Frameworks.FrameworkConstants;

namespace NuGet.SolutionRestoreManager
{
    internal class VSNominationUtilities
    {
        #region ToolReferenceAPIs
        /******************
        * ToolReferences *
        ******************/

        internal static void ProcessToolReferences(ProjectNames projectNames, IReadOnlyList<IVsTargetFrameworkInfo4> targetFrameworks, IReadOnlyList<IVsReferenceItem2> toolReferences, DependencyGraphSpec dgSpec)
        {
            var toolFramework = GetToolFramework(targetFrameworks);
            var packagesPath = GetRestoreProjectPath(targetFrameworks);
            var fallbackFolders = GetRestoreFallbackFolders(targetFrameworks).AsList();
            var sources = GetRestoreSources(targetFrameworks)
                .Select(e => new PackageSource(e))
                .ToList();

            toolReferences
                .Select(r => ToolRestoreUtility.GetSpec(
                    projectNames.FullName,
                    r.Name,
                    GetVersionRange(r),
                    toolFramework,
                    packagesPath,
                    fallbackFolders,
                    sources,
                    projectWideWarningProperties: null))
                .ForEach(ts =>
                {
                    dgSpec.AddRestore(ts.RestoreMetadata.ProjectUniqueName);
                    dgSpec.AddProject(ts);
                });
        }
        #endregion ToolReferenceAPIs

        #region IVSTargetFrameworksAPIs
        /**********************************************************************
         * IVSTargetFrameworks based APIs                                     *
         **********************************************************************/

        internal static RuntimeGraph GetRuntimeGraph(IReadOnlyList<IVsTargetFrameworkInfo4> targetFrameworks)
        {
            var runtimes = targetFrameworks
                .SelectMany(tfi => new[]
                {
                    GetPropertyValueOrNull(tfi.Properties, ProjectBuildProperties.RuntimeIdentifier),
                    GetPropertyValueOrNull(tfi.Properties, ProjectBuildProperties.RuntimeIdentifiers),
                })
                .OfType<string>()
                .SelectMany(MSBuildStringUtility.Split)
                .Distinct(StringComparer.Ordinal)
                .Select(rid => new RuntimeDescription(rid))
                .ToList();

            var supports = targetFrameworks
                .Select(tfi => GetPropertyValueOrNull(tfi.Properties, ProjectBuildProperties.RuntimeSupports))
                .OfType<string>()
                .SelectMany(MSBuildStringUtility.Split)
                .Distinct(StringComparer.Ordinal)
                .Select(s => new CompatibilityProfile(s))
                .ToList();

            return new RuntimeGraph(runtimes, supports);
        }

        internal static TargetFrameworkInformation ToTargetFrameworkInformation(
            IVsTargetFrameworkInfo4 targetFrameworkInfo, bool cpvmEnabled, string projectFullPath)
        {
            var frameworkName = GetTargetFramework(targetFrameworkInfo.Properties, projectFullPath);

            string? ptfString = GetPropertyValueOrNull(targetFrameworkInfo.Properties, ProjectBuildProperties.PackageTargetFallback);
            List<NuGetFramework>? ptf = ptfString is not null
                ? MSBuildStringUtility.Split(ptfString).Select(NuGetFramework.Parse).ToList()
                : null;

            string? atfString = GetPropertyValueOrNull(targetFrameworkInfo.Properties, ProjectBuildProperties.AssetTargetFallback);
            List<NuGetFramework>? atf = atfString is not null
                ? MSBuildStringUtility.Split(atfString).Select(NuGetFramework.Parse).ToList()
                : null;

            bool isPackagePruningEnabled = MSBuildStringUtility.IsTrue(GetPropertyValueOrNull(targetFrameworkInfo.Properties, ProjectBuildProperties.RestoreEnablePackagePruning));

            // Get fallback properties
            (frameworkName, var imports, var assetTargetFallback, var warn) = AssetTargetFallbackUtility.GetFallbackFrameworkInformation(frameworkName, ptf, atf);

            ImmutableArray<LibraryDependency> dependencies = [];
            ImmutableArray<DownloadDependency> downloadDependencies = [];
            IReadOnlyDictionary<string, CentralPackageVersion>? centralPackageVersions = null;
            IReadOnlyCollection<FrameworkDependency>? frameworkReferences = null;
            IReadOnlyDictionary<string, PrunePackageReference>? packagesToPrune = null;

            if (targetFrameworkInfo.Items is not null)
            {
                if (cpvmEnabled && targetFrameworkInfo.Items.TryGetValue("PackageVersion", out var packageVersions))
                {
                    centralPackageVersions = packageVersions
                        .Select(ToCentralPackageVersion)
                        .Distinct(CentralPackageVersionNameComparer.Default)
                        .ToDictionary(cpv => cpv.Name, StringComparer.OrdinalIgnoreCase);
                }

                if (targetFrameworkInfo.Items.TryGetValue(ProjectItems.PackageReference, out var packageReferences))
                {
                    dependencies = packageReferences.Select(pr => ToPackageLibraryDependency(pr, cpvmEnabled, centralPackageVersions)).ToImmutableArray();
                }

                if (targetFrameworkInfo.Items.TryGetValue("PackageDownload", out var packageDownloads))
                {
                    downloadDependencies = packageDownloads.SelectMany(ToPackageDownloadDependency).ToImmutableArray();
                }

                if (targetFrameworkInfo.Items.TryGetValue("FrameworkReference", out var frameworkReference))
                {
                    frameworkReferences = PopulateFrameworkDependencies(frameworkReference);
                }

                if (isPackagePruningEnabled && targetFrameworkInfo.Items.TryGetValue("PrunePackageReference", out var PrunePackageReferences))
                {
                    packagesToPrune = PrunePackageReferences
                        .Select(ToPrunePackageReference)
                        .ToDictionary(packageToPrune => packageToPrune.Name, StringComparer.OrdinalIgnoreCase);
                }
            }

            var tfi = new TargetFrameworkInformation
            {
                AssetTargetFallback = assetTargetFallback,
                CentralPackageVersions = centralPackageVersions,
                Dependencies = dependencies,
                DownloadDependencies = downloadDependencies,
                FrameworkName = frameworkName,
                FrameworkReferences = frameworkReferences,
                Imports = imports,
                RuntimeIdentifierGraphPath = GetPropertyValueOrNull(targetFrameworkInfo.Properties, ProjectBuildProperties.RuntimeIdentifierGraphPath),
                TargetAlias = GetPropertyValueOrNull(targetFrameworkInfo.Properties, ProjectBuildProperties.TargetFramework),
                Warn = warn,
                PackagesToPrune = packagesToPrune,
            };

            return tfi;
        }

        internal static NuGetFramework GetTargetFramework(IReadOnlyDictionary<string, string> properties, string projectFullPath)
        {
            var targetFrameworkMoniker = GetPropertyValueOrNull(properties, ProjectBuildProperties.TargetFrameworkMoniker);
            var targetPlatformMoniker = GetPropertyValueOrNull(properties, ProjectBuildProperties.TargetPlatformMoniker);
            var targetPlatformMinVersion = GetPropertyValueOrNull(properties, ProjectBuildProperties.TargetPlatformMinVersion);
            var clrSupport = GetPropertyValueOrNull(properties, ProjectBuildProperties.CLRSupport);
            var windowsTargetPlatformMinVersion = GetPropertyValueOrNull(properties, ProjectBuildProperties.WindowsTargetPlatformMinVersion);

            return MSBuildProjectFrameworkUtility.GetProjectFramework(
                projectFullPath,
                targetFrameworkMoniker,
                targetPlatformMoniker,
                targetPlatformMinVersion,
                clrSupport,
                windowsTargetPlatformMinVersion);
        }

        internal static ProjectRestoreMetadataFrameworkInfo ToProjectRestoreMetadataFrameworkInfo(
            IVsTargetFrameworkInfo4 targetFrameworkInfo,
            string projectDirectory,
            string projectFullPath)
        {
            var tfi = new ProjectRestoreMetadataFrameworkInfo
            {
                FrameworkName = GetTargetFramework(targetFrameworkInfo.Properties, projectFullPath),
                TargetAlias = GetPropertyValueOrNull(targetFrameworkInfo.Properties, ProjectBuildProperties.TargetFramework)
            };

            if (targetFrameworkInfo.Items?.TryGetValue(ProjectItems.ProjectReference, out var projectReferences) ?? false)
            {
                tfi.ProjectReferences.AddRange(
                    projectReferences
                        .Where(IsReferenceOutputAssemblyTrueOrEmpty)
                        .Select(item => ToProjectRestoreReference(item, projectDirectory))
                        .Distinct(ProjectRestoreReferenceComparer.Default));
            }

            return tfi;
        }

        internal static string GetPackageId(ProjectNames projectNames, IReadOnlyList<IVsTargetFrameworkInfo4> tfms)
        {
            var packageId = GetSingleNonEvaluatedPropertyOrNull(tfms, ProjectBuildProperties.PackageId, v => v);
            return packageId ?? projectNames.ShortName;
        }

        internal static NuGetVersion GetPackageVersion(IReadOnlyList<IVsTargetFrameworkInfo4> tfms)
        {
            // $(PackageVersion) property if set overrides the $(Version)
            var versionPropertyValue =
                GetSingleNonEvaluatedPropertyOrNull(tfms, ProjectItems.PackageVersion, NuGetVersion.Parse)
                ?? GetSingleNonEvaluatedPropertyOrNull(tfms, ProjectBuildProperties.Version, NuGetVersion.Parse);

            return versionPropertyValue ?? PackageSpec.DefaultVersion;
        }

        internal static string? GetRestoreProjectPath(IReadOnlyList<IVsTargetFrameworkInfo4> values)
        {
            return GetSingleNonEvaluatedPropertyOrNull(values, ProjectBuildProperties.RestorePackagesPath, e => e);
        }

        internal static RestoreLockProperties GetRestoreLockProperties(IReadOnlyList<IVsTargetFrameworkInfo4> values)
        {
            return new RestoreLockProperties(
                        GetRestorePackagesWithLockFile(values),
                        GetNuGetLockFilePath(values),
                        IsLockFileFreezeOnRestore(values));
        }

        internal static WarningProperties GetProjectWideWarningProperties(IReadOnlyList<IVsTargetFrameworkInfo4> targetFrameworks)
        {
            var warningsAsErrors = GetSingleOrDefaultNuGetLogCodes(targetFrameworks, ProjectBuildProperties.WarningsAsErrors, MSBuildStringUtility.GetNuGetLogCodes);
            var noWarn = GetSingleOrDefaultNuGetLogCodes(targetFrameworks, ProjectBuildProperties.NoWarn, MSBuildStringUtility.GetNuGetLogCodes);
            var warningsNotAsErrors = GetSingleOrDefaultNuGetLogCodes(targetFrameworks, ProjectBuildProperties.WarningsNotAsErrors, MSBuildStringUtility.GetNuGetLogCodes);

            return WarningProperties.GetWarningProperties(
                        treatWarningsAsErrors: GetSingleOrDefaultPropertyValue(targetFrameworks, ProjectBuildProperties.TreatWarningsAsErrors, e => e),
                        warningsAsErrors: warningsAsErrors.IsDefault ? [] : warningsAsErrors,
                        noWarn: noWarn.IsDefault ? [] : noWarn,
                        warningsNotAsErrors: warningsNotAsErrors.IsDefault ? [] : warningsNotAsErrors);
        }

        /// <summary>
        /// The result will contain CLEAR and no sources specified in RestoreSources if the clear keyword is in it.
        /// If there are additional sources specified, the value AdditionalValue will be set in the result and then all the additional sources will follow
        /// </summary>
        internal static IEnumerable<string> GetRestoreSources(IReadOnlyList<IVsTargetFrameworkInfo4> values)
        {
            string? propertyValue = GetSingleNonEvaluatedPropertyOrNull(values, ProjectBuildProperties.RestoreSources, e => e);

            string[] sources = propertyValue is not null ?
                HandleClear(MSBuildStringUtility.Split(propertyValue))
                : Array.Empty<string>();

            // Read RestoreAdditionalProjectSources from the inner build, these may be different between frameworks.
            // Exclude is not allowed for sources
            var additional = MSBuildRestoreUtility.AggregateSources(
                values: GetAggregatePropertyValues(values, ProjectBuildProperties.RestoreAdditionalProjectSources),
                excludeValues: Enumerable.Empty<string>());

            return VSRestoreSettingsUtilities.GetEntriesWithAdditional(sources, additional.ToArray());
        }

        /// <summary>
        /// The result will contain CLEAR and no sources specified in RestoreFallbackFolders if the clear keyword is in it.
        /// If there are additional fallback folders specified, the value AdditionalValue will be set in the result and then all the additional fallback folders will follow
        /// </summary>
        internal static IEnumerable<string> GetRestoreFallbackFolders(IReadOnlyList<IVsTargetFrameworkInfo4> tfms)
        {
            var value = GetSingleNonEvaluatedPropertyOrNull(tfms, ProjectBuildProperties.RestoreFallbackFolders, e => e);

            string[] folders = value is not null
                ? HandleClear(MSBuildStringUtility.Split(value))
                : Array.Empty<string>();

            // Read RestoreAdditionalProjectFallbackFolders from the inner build.
            // Remove all excluded fallback folders listed in RestoreAdditionalProjectFallbackFoldersExcludes.
            var additional = MSBuildRestoreUtility.AggregateSources(
                values: GetAggregatePropertyValues(tfms, ProjectBuildProperties.RestoreAdditionalProjectFallbackFolders),
                excludeValues: GetAggregatePropertyValues(tfms, ProjectBuildProperties.RestoreAdditionalProjectFallbackFoldersExcludes));

            return VSRestoreSettingsUtilities.GetEntriesWithAdditional(folders, additional.ToArray());
        }

        private static string? GetRestorePackagesWithLockFile(IReadOnlyList<IVsTargetFrameworkInfo4> tfms)
        {
            return GetSingleNonEvaluatedPropertyOrNull(tfms, ProjectBuildProperties.RestorePackagesWithLockFile, v => v);
        }

        private static string? GetNuGetLockFilePath(IReadOnlyList<IVsTargetFrameworkInfo4> tfms)
        {
            return GetSingleNonEvaluatedPropertyOrNull(tfms, ProjectBuildProperties.NuGetLockFilePath, v => v);
        }

        private static bool IsLockFileFreezeOnRestore(IReadOnlyList<IVsTargetFrameworkInfo4> tfms)
        {
            return GetSingleNonEvaluatedPropertyOrNull(tfms, ProjectBuildProperties.RestoreLockedMode, MSBuildStringUtility.IsTrue);
        }

        /// <summary>
        /// Evaluates the msbuild properties and returns the value of the ManagePackageVersionsCentrally property.
        /// If it is not defined the default value will be disabled.
        /// </summary>
        internal static bool IsCentralPackageVersionManagementEnabled(IReadOnlyList<IVsTargetFrameworkInfo4> tfms)
        {
            return GetSingleNonEvaluatedPropertyOrNull(tfms, ProjectBuildProperties.ManagePackageVersionsCentrally, MSBuildStringUtility.IsTrue);
        }

        internal static bool IsCentralPackageVersionOverrideDisabled(IReadOnlyList<IVsTargetFrameworkInfo4> tfms)
        {
            return GetSingleNonEvaluatedPropertyOrNull(tfms, ProjectBuildProperties.CentralPackageVersionOverrideEnabled, (value) => value.EqualsFalse());
        }

        internal static bool IsCentralPackageFloatingVersionsEnabled(IReadOnlyList<IVsTargetFrameworkInfo4> tfms)
        {
            return GetSingleNonEvaluatedPropertyOrNull(tfms, ProjectBuildProperties.CentralPackageFloatingVersionsEnabled, MSBuildStringUtility.IsTrue);
        }

        internal static bool IsCentralPackageTransitivePinningEnabled(IReadOnlyList<IVsTargetFrameworkInfo4> tfms)
        {
            return GetSingleNonEvaluatedPropertyOrNull(tfms, ProjectBuildProperties.CentralPackageTransitivePinningEnabled, MSBuildStringUtility.IsTrue);
        }

        internal static bool GetUseLegacyDependencyResolver(IReadOnlyList<IVsTargetFrameworkInfo4> tfms)
        {
            return GetSingleNonEvaluatedPropertyOrNull(tfms, ProjectBuildProperties.RestoreUseLegacyDependencyResolver, MSBuildStringUtility.IsTrue);
        }

        internal static RestoreAuditProperties? GetRestoreAuditProperties(IReadOnlyList<IVsTargetFrameworkInfo4> tfms)
        {
            string? enableAudit = GetSingleNonEvaluatedPropertyOrNull(tfms, ProjectBuildProperties.NuGetAudit, s => s);
            string? auditLevel = GetSingleNonEvaluatedPropertyOrNull(tfms, ProjectBuildProperties.NuGetAuditLevel, s => s);
            string? auditMode = GetSingleNonEvaluatedPropertyOrNull(tfms, ProjectBuildProperties.NuGetAuditMode, s => s);
            HashSet<string>? suppressedAdvisories = GetSuppressedAdvisories(tfms);

            return !string.IsNullOrEmpty(enableAudit) || !string.IsNullOrEmpty(auditLevel) || !string.IsNullOrEmpty(auditMode) || suppressedAdvisories is not null
                ? new RestoreAuditProperties()
                {
                    EnableAudit = enableAudit,
                    AuditLevel = auditLevel,
                    AuditMode = auditMode,
                    SuppressedAdvisories = suppressedAdvisories,
                }
                : null;

            static HashSet<string>? GetSuppressedAdvisories(IReadOnlyList<IVsTargetFrameworkInfo4> tfms)
            {
                if (tfms.Count == 0) { return null; }

                // Create the hash set from the first TargetFramework
                HashSet<string>? suppressedAdvisories = null;
                if (tfms[0].Items?.TryGetValue(ProjectItems.NuGetAuditSuppress, out IReadOnlyList<IVsReferenceItem2>? suppressItems) ?? false)
                {
                    if (suppressItems.Count > 0)
                    {
                        suppressedAdvisories = new(suppressItems.Count, StringComparer.Ordinal);
                        for (int i = 0; i < suppressItems.Count; i++)
                        {
                            string url = suppressItems[i].Name;
                            suppressedAdvisories.Add(url);
                        }
                    }
                }

                // Validate that other TargetFrameworks use the same collection
                for (int i = 1; i < tfms.Count; i++)
                {
                    if (!AreSameAdvisories(suppressedAdvisories, tfms[i].Items))
                    {
                        string message = string.Format(Resources.ItemValuesAreDifferentAcrossTargetFrameworks, ProjectItems.NuGetAuditSuppress);
                        throw new InvalidOperationException(message);
                    }
                }

                return suppressedAdvisories;

                static bool AreSameAdvisories(HashSet<string>? suppressedAdvisories, IReadOnlyDictionary<string, IReadOnlyList<IVsReferenceItem2>>? items)
                {
                    IReadOnlyList<IVsReferenceItem2>? suppressItems = null;
                    _ = items?.TryGetValue(ProjectItems.NuGetAuditSuppress, out suppressItems);

                    int expectedCount = suppressedAdvisories?.Count ?? 0;
                    int actualCount = suppressItems?.Count ?? 0;
                    if (expectedCount == 0 || actualCount == 0)
                    {
                        if (expectedCount == 0 && actualCount == 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }

                    if (suppressedAdvisories!.Count != suppressItems!.Count) { return false; }

                    for (int i = 0; i < suppressItems.Count; i++)
                    {
                        var url = suppressItems[i].Name;
                        if (!suppressedAdvisories.Contains(url))
                            return false;
                    }

                    return true;
                }
            }
        }

        private static NuGetFramework GetToolFramework(IReadOnlyList<IVsTargetFrameworkInfo4> targetFrameworks)
        {
            return GetSingleNonEvaluatedPropertyOrNull(
                    targetFrameworks,
                    ProjectBuildProperties.DotnetCliToolTargetFramework,
                    NuGetFramework.Parse) ?? CommonFrameworks.NetCoreApp10;
        }

        private static TValue? GetSingleOrDefaultPropertyValue<TValue>(
            IReadOnlyList<IVsTargetFrameworkInfo4> values,
            string propertyName,
            Func<string, TValue> valueFactory)
        {
            var properties = GetNonEvaluatedPropertyOrNull(values, propertyName, valueFactory);

            return properties.Length > 1 ? default(TValue) : properties.SingleOrDefault();
        }

        private static ImmutableArray<NuGetLogCode> GetSingleOrDefaultNuGetLogCodes(
            IReadOnlyList<IVsTargetFrameworkInfo4> values,
            string propertyName,
            Func<string, ImmutableArray<NuGetLogCode>> valueFactory)
        {
            var logCodeProperties = GetNonEvaluatedPropertyOrNull(values, propertyName, valueFactory);

            return GetDistinctNuGetLogCodesOrDefault(logCodeProperties);
        }

        /// <summary>
        /// Return empty list of NuGetLogCode if all lists of NuGetLogCode are not the same.
        /// </summary>
        public static ImmutableArray<NuGetLogCode> GetDistinctNuGetLogCodesOrDefault(ImmutableArray<ImmutableArray<NuGetLogCode>> nugetLogCodeLists)
        {
            if (nugetLogCodeLists.Length == 0)
            {
                return [];
            }

            ImmutableArray<NuGetLogCode> result = [];
            var first = true;

            foreach (ImmutableArray<NuGetLogCode> logCodeList in nugetLogCodeLists)
            {
                // If this is first item, assign it to result
                if (first)
                {
                    result = logCodeList;
                    first = false;
                }
                // Compare the rest items to the first one.
                else if (result == null || logCodeList == null || result.Length != logCodeList.Length || !result.All(logCodeList.Contains))
                {
                    return [];
                }
            }

            return result;
        }

        // Trying to fetch a list of property value from all tfm property bags.
        private static ImmutableArray<TValue?> GetNonEvaluatedPropertyOrNull<TValue>(
            IReadOnlyList<IVsTargetFrameworkInfo4> values,
            string propertyName,
            Func<string, TValue> valueFactory)
        {
            return values
                .Select(tfm =>
                {
                    var val = tfm.Properties is not null ? GetPropertyValueOrNull(tfm.Properties, propertyName) : null;
                    return val != null ? valueFactory(val) : default(TValue);
                })
                .Distinct()
                .ToImmutableArray();
        }

        // Trying to fetch a property value from tfm property bags.
        // If defined the property should have identical values in all of the occurances.
        private static TValue? GetSingleNonEvaluatedPropertyOrNull<TValue>(
            IReadOnlyList<IVsTargetFrameworkInfo4> values,
            string propertyName,
            Func<string, TValue> valueFactory)
        {
            ImmutableArray<TValue?> distinctValues = GetNonEvaluatedPropertyOrNull(values, propertyName, valueFactory);

            if (distinctValues.Length == 0)
            {
                return default(TValue);
            }
            else if (distinctValues.Length == 1)
            {
                return distinctValues[0];
            }
            else
            {
                distinctValues.Sort();
                var distinctValueStrings = string.Join(", ", distinctValues);
                var message = string.Format(CultureInfo.CurrentCulture, Resources.PropertyDoesNotHaveSingleValue, propertyName, distinctValueStrings);
                throw new InvalidOperationException(message);
            }
        }

        /// <summary>
        /// Fetch all property values from each target framework and combine them.
        /// </summary>
        private static IEnumerable<string> GetAggregatePropertyValues(
                IEnumerable<IVsTargetFrameworkInfo4> values,
                string propertyName)
        {
            // Only non-null values are added to the list as part of the split.
            return values
                .SelectMany(tfm =>
                {
                    var valueString = GetPropertyValueOrNull(tfm.Properties, propertyName);
                    return valueString is not null ? MSBuildStringUtility.Split(valueString) : Enumerable.Empty<string>();
                });
        }

        #endregion IVSTargetFrameworksAPIs

        #region IVSReferenceItemAPIs

        private static LibraryDependency ToPackageLibraryDependency(IVsReferenceItem2 item, bool cpvmEnabled, IReadOnlyDictionary<string, CentralPackageVersion>? centralPackageVersions)
        {
            bool autoReferenced = GetPropertyBoolOrFalse(item, "IsImplicitlyDefined");
            VersionRange? versionRange = ParseVersionRange(item, "Version");
            bool versionDefined = versionRange != null;
            if (versionRange == null && !cpvmEnabled)
            {
                versionRange = VersionRange.All;
            }

            VersionRange? versionOverrideRange = ParseVersionRange(item, "VersionOverride");

            CentralPackageVersion? centralPackageVersion = null;
            bool isCentrallyManaged = !versionDefined && !autoReferenced && cpvmEnabled && versionOverrideRange == null && centralPackageVersions != null && centralPackageVersions.TryGetValue(item.Name, out centralPackageVersion);

            if (centralPackageVersion != null)
            {
                versionRange = centralPackageVersion.VersionRange;
            }
            versionRange = versionOverrideRange ?? versionRange;

            // Get warning suppressions
            string? noWarnString = GetPropertyValueOrNull(item, ProjectBuildProperties.NoWarn);
            ImmutableArray<NuGetLogCode> noWarn = noWarnString is not null ? MSBuildStringUtility.GetNuGetLogCodes(noWarnString) : [];

            (var includeType, var suppressParent) = MSBuildRestoreUtility.GetLibraryDependencyIncludeFlags(
                includeAssets: GetPropertyValueOrNull(item, ProjectBuildProperties.IncludeAssets),
                excludeAssets: GetPropertyValueOrNull(item, ProjectBuildProperties.ExcludeAssets),
                privateAssets: GetPropertyValueOrNull(item, ProjectBuildProperties.PrivateAssets));

            var dependency = new LibraryDependency()
            {
                LibraryRange = new LibraryRange(
                    name: item.Name,
                    versionRange: versionRange,
                    typeConstraint: LibraryDependencyTarget.Package),

                // Mark packages coming from the SDK as AutoReferenced
                AutoReferenced = GetPropertyBoolOrFalse(item, "IsImplicitlyDefined"),
                GeneratePathProperty = GetPropertyBoolOrFalse(item, "GeneratePathProperty"),
                Aliases = GetPropertyValueOrNull(item, "Aliases"),
                VersionOverride = versionOverrideRange,
                NoWarn = noWarn,
                IncludeType = includeType,
                SuppressParent = suppressParent,
                VersionCentrallyManaged = isCentrallyManaged,
            };

            return dependency;
        }

        private static IEnumerable<DownloadDependency> ToPackageDownloadDependency(IVsReferenceItem2 item)
        {
            var id = item.Name;
            var versionRanges = GetVersionRangeList(item);
            foreach (var versionRange in versionRanges)
            {
                if (!(versionRange.HasLowerAndUpperBounds && versionRange.MinVersion.Equals(versionRange.MaxVersion)))
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Error_PackageDownload_OnlyExactVersionsAreAllowed, id, versionRange.OriginalString));
                }

                var downloadDependency = new DownloadDependency(id, versionRange);

                yield return downloadDependency;
            }
        }

        private static CentralPackageVersion ToCentralPackageVersion(IVsReferenceItem2 item)
        {
            string id = item.Name;
            VersionRange versionRange = GetVersionRange(item);
            var centralPackageVersion = new CentralPackageVersion(id, versionRange);

            return centralPackageVersion;
        }

        private static PrunePackageReference ToPrunePackageReference(IVsReferenceItem2 item)
        {
            string id = item.Name;
            string? versionString = GetPropertyValueOrNull(item, ProjectBuildProperties.Version);
            return PrunePackageReference.Create(id, versionString!);
        }

        private static IReadOnlyCollection<FrameworkDependency>? PopulateFrameworkDependencies(IReadOnlyList<IVsReferenceItem2> frameworkReferences)
        {
            HashSet<FrameworkDependency>? newReferences = null;
            foreach (var item in frameworkReferences)
            {
                newReferences ??= new HashSet<FrameworkDependency>();
                if (!newReferences.Any(e => ComparisonUtility.FrameworkReferenceNameComparer.Equals(e.Name, item.Name)))
                {
                    newReferences.Add(ToFrameworkDependency(item));
                }
            }

            return newReferences;
        }

        private static FrameworkDependency ToFrameworkDependency(IVsReferenceItem2 item)
        {
            var privateAssets = GetFrameworkDependencyFlags(item, ProjectBuildProperties.PrivateAssets);
            return new FrameworkDependency(item.Name, privateAssets);
        }

        private static ProjectRestoreReference ToProjectRestoreReference(IVsReferenceItem2 item, string projectDirectory)
        {
            // The path may be a relative path, to match the project unique name as a
            // string this should be the full path to the project
            // Remove ../../ and any other relative parts of the path that were used in the project file
            var referencePath = Path.GetFullPath(Path.Combine(projectDirectory, item.Name));

            var dependency = new ProjectRestoreReference
            {
                ProjectPath = referencePath,
                ProjectUniqueName = referencePath,
            };

            MSBuildRestoreUtility.ApplyIncludeFlags(
                dependency,
                includeAssets: GetPropertyValueOrNull(item, ProjectBuildProperties.IncludeAssets),
                excludeAssets: GetPropertyValueOrNull(item, ProjectBuildProperties.ExcludeAssets),
                privateAssets: GetPropertyValueOrNull(item, ProjectBuildProperties.PrivateAssets));

            return dependency;
        }

        private static VersionRange? ParseVersionRange(IVsReferenceItem2 item, string propertyName)
        {
            string? versionRangeItemValue = GetPropertyValueOrNull(item, propertyName);

            if (versionRangeItemValue != null)
            {
                VersionRange versionRange = VersionRange.Parse(versionRangeItemValue);
                return versionRange;
            }

            return null;
        }

        private static IEnumerable<VersionRange> GetVersionRangeList(IVsReferenceItem2 item)
        {
            char[] splitChars = new[] { ';' };
            string? versionString = GetPropertyValueOrNull(item, "Version");
            if (string.IsNullOrEmpty(versionString))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Error_PackageDownload_NoVersion, item.Name));
            }

            if (versionString != null)
            {
                var versions = versionString.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
                foreach (var version in versions)
                {
                    yield return VersionRange.Parse(version);
                }
            }
            else
            {
                yield return VersionRange.All;
            }
        }

        private static VersionRange GetVersionRange(IVsReferenceItem2 item)
        {
            VersionRange versionRange = ParseVersionRange(item, "Version") ?? VersionRange.All;
            return versionRange;
        }

        /// <summary>
        /// Get the frameworkdependencyflag based on the name.
        /// </summary>
        private static FrameworkDependencyFlags GetFrameworkDependencyFlags(IVsReferenceItem2 item, string name)
        {
            var flags = GetPropertyValueOrNull(item, name);

            return FrameworkDependencyFlagsUtils.GetFlags(flags);
        }

        /// <summary>
        /// True if ReferenceOutputAssembly is true or empty.
        /// All other values will be false.
        /// </summary>
        private static bool IsReferenceOutputAssemblyTrueOrEmpty(IVsReferenceItem2 item)
        {
            var value = GetPropertyValueOrNull(item, ProjectBuildProperties.ReferenceOutputAssembly);

            return MSBuildStringUtility.IsTrueOrEmpty(value);
        }

        private static bool GetPropertyBoolOrFalse(
        IVsReferenceItem2 item, string propertyName)
        {
            try
            {
                if (item.Metadata?.TryGetValue(propertyName, out var value) ?? false)
                {
                    return MSBuildStringUtility.IsTrue(value);
                }
            }
            catch (ArgumentException)
            {
            }
            catch (KeyNotFoundException)
            {
            }

            return false;
        }

        internal static string? GetPropertyValueOrNull(
            IVsReferenceItem2 item, string propertyName)
        {
            try
            {
                if (item.Metadata?.TryGetValue(propertyName, out var value) ?? false)
                {
                    return MSBuildStringUtility.TrimAndGetNullForEmpty(value);
                }
            }
            catch (ArgumentException)
            {
            }
            catch (KeyNotFoundException)
            {
            }

            return null;
        }

        private static string? GetPropertyValueOrNull(
            IReadOnlyDictionary<string, string> properties, string propertyName)
        {
            try
            {
                if (properties?.TryGetValue(propertyName, out var value) ?? false)
                {
                    return MSBuildStringUtility.TrimAndGetNullForEmpty(value);
                }
            }
            catch (ArgumentException)
            {
            }
            catch (KeyNotFoundException)
            {
            }

            return null;
        }
        #endregion IVSReferenceItemAPIs

        private static string[] HandleClear(string[] input)
        {
            if (input.Any(e => StringComparer.OrdinalIgnoreCase.Equals(ProjectBuildProperties.Clear, e)))
            {
                return new string[] { ProjectBuildProperties.Clear };
            }

            return input;
        }

        internal static NuGetVersion? GetSdkAnalysisLevel(IReadOnlyList<IVsTargetFrameworkInfo4> targetFrameworks)
        {
            string? skdAnalysisLevelString = GetSingleNonEvaluatedPropertyOrNull(targetFrameworks, nameof(ProjectBuildProperties.SdkAnalysisLevel), v => v);

            return skdAnalysisLevelString is null
            ? null
            : MSBuildRestoreUtility.GetSdkAnalysisLevel(skdAnalysisLevelString);
        }

        internal static bool GetUsingMicrosoftNETSdk(IReadOnlyList<IVsTargetFrameworkInfo4> targetFrameworks)
        {
            string? usingNetSdk = GetSingleNonEvaluatedPropertyOrNull(targetFrameworks, nameof(ProjectBuildProperties.UsingMicrosoftNETSdk), v => v);

            if (usingNetSdk is not null)
            {
                return MSBuildRestoreUtility.GetUsingMicrosoftNETSdk(usingNetSdk);
            }

            return false;
        }

        internal static NuGetVersion? GetSdkVersion(IReadOnlyList<IVsTargetFrameworkInfo4> targetFrameworks)
        {
            string? sdkVersionString = GetSingleNonEvaluatedPropertyOrNull(targetFrameworks, "NETCoreSdkVersion", v => v);
            NuGetVersion.TryParse(sdkVersionString, out NuGetVersion? sdkVersion);

            return sdkVersion;
        }
    }
}
