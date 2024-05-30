// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NuGet.ProjectManagement;

namespace NuGet.SolutionRestoreManager
{
    internal static class ProjectRestoreInfo3Adapter
    {
        public static IVsProjectRestoreInfo3 Create(IVsProjectRestoreInfo projectRestoreInfo)
        {
            if (projectRestoreInfo is null) { throw new ArgumentNullException(nameof(projectRestoreInfo)); }

            if (string.IsNullOrEmpty(projectRestoreInfo.BaseIntermediatePath))
            {
                throw new ArgumentException(message: "BaseIntermediatePath must have a non-empty value",
                    paramName: nameof(projectRestoreInfo));
            }

            return new VsProjectRestoreInfo3Adapter
            {
                MSBuildProjectExtensionsPath = projectRestoreInfo.BaseIntermediatePath,
                OriginalTargetFrameworks = projectRestoreInfo.OriginalTargetFrameworks,
                TargetFrameworks = ToTargetFrameworkInfoList(projectRestoreInfo.TargetFrameworks),
                ToolReferences = ToReferenceItemList(projectRestoreInfo.ToolReferences)
            };
        }

        public static IVsProjectRestoreInfo3 Create(IVsProjectRestoreInfo2 projectRestoreInfo)
        {
            if (projectRestoreInfo is null) { throw new ArgumentNullException(nameof(projectRestoreInfo)); }

            if (string.IsNullOrEmpty(projectRestoreInfo.BaseIntermediatePath))
            {
                throw new ArgumentException(message: "BaseIntermediatePath must have a non-empty value",
                    paramName: nameof(projectRestoreInfo));
            }

            return new VsProjectRestoreInfo3Adapter
            {
                MSBuildProjectExtensionsPath = projectRestoreInfo.BaseIntermediatePath,
                OriginalTargetFrameworks = projectRestoreInfo.OriginalTargetFrameworks,
                TargetFrameworks = ToTargetFrameworkInfoList(projectRestoreInfo.TargetFrameworks),
                ToolReferences = ToReferenceItemList(projectRestoreInfo.ToolReferences)
            };
        }

        [return: NotNullIfNotNull(nameof(references))]
        private static IReadOnlyList<IVsReferenceItem2>? ToReferenceItemList(IVsReferenceItems? references)
        {
            if (references is null) { return null; }

            var result = new List<IVsReferenceItem2>(references.Count);

            for (var i = 0; i < references.Count; i++)
            {
                var reference = references.Item(i);
                IVsReferenceItem2? adapter = ToReferenceItem(reference);
                // null needs to be handled in VsSolutionRestoreService.ToDependencySpec and reported back to caller
                result.Add(adapter!);
            }

            return result;
        }

        private static IVsReferenceItem2? ToReferenceItem(IVsReferenceItem? reference)
        {
            if (reference is null) { return null; }

            var a = new VsReferenceItem2Adapter
            {
                Name = reference.Name,
                Metadata = ToPropertiesDictionary(reference.Properties),
            };
            return a;
        }

        private static IReadOnlyList<IVsTargetFrameworkInfo4> ToTargetFrameworkInfoList(IVsTargetFrameworks targetFrameworks)
        {
            var result = new List<IVsTargetFrameworkInfo4>(targetFrameworks.Count);

            for (var i = 0; i < targetFrameworks.Count; i++)
            {
                IVsTargetFrameworkInfo? targetFramework = targetFrameworks.Item(i);
                IVsTargetFrameworkInfo4? adapter = ToTargetFrameworkInfo(targetFramework);
                // null needs to be handled in VsSolutionRestoreService.ToDependencySpec and reported back to caller
                result.Add(adapter!);
            }

            return result;
        }

        private static IVsTargetFrameworkInfo4? ToTargetFrameworkInfo(IVsTargetFrameworkInfo? targetFramework)
        {
            if (targetFramework is null) { return null; }

            var adapter = new VsTargetFramework4Adapter()
            {
                TargetFrameworkMoniker = targetFramework.TargetFrameworkMoniker,
                // null needs to be handled in VsSolutionRestoreService.ToDependencySpec and reported back to caller
                Properties = ToPropertiesDictionary(targetFramework.Properties)!,
                Items = ToItemsDictionary(targetFramework)
            };

            return adapter;
        }

        private static IReadOnlyList<IVsTargetFrameworkInfo4> ToTargetFrameworkInfoList(IVsTargetFrameworks2 targetFrameworks)
        {
            var result = new List<IVsTargetFrameworkInfo4>(targetFrameworks.Count);

            for (var i = 0; i < targetFrameworks.Count; i++)
            {
                IVsTargetFrameworkInfo2? targetFramework = targetFrameworks.Item(i);
                IVsTargetFrameworkInfo4? adapter = ToTargetFrameworkInfo(targetFramework);
                // null needs to be handled in VsSolutionRestoreService.ToDependencySpec and reported back to caller
                result.Add(adapter!);
            }

            return result;
        }

        private static IVsTargetFrameworkInfo4? ToTargetFrameworkInfo(IVsTargetFrameworkInfo2? targetFramework)
        {
            if (targetFramework is null) { return null; }

            var adapter = new VsTargetFramework4Adapter()
            {
                TargetFrameworkMoniker = targetFramework.TargetFrameworkMoniker,
                // null needs to be handled in VsSolutionRestoreService.ToDependencySpec and reported back to caller
                Properties = ToPropertiesDictionary(targetFramework.Properties)!,
                Items = ToItemsDictionary(targetFramework)
            };

            return adapter;
        }

        private static IReadOnlyDictionary<string, string?>? ToPropertiesDictionary(IVsProjectProperties? properties)
        {
            if (properties is null) { return null; }

            var result = new Dictionary<string, string?>(properties.Count, StringComparer.OrdinalIgnoreCase);

            for (var i = 0; i < properties.Count; i++)
            {
                var property = properties.Item(i) ?? throw new Exception($"Property index {i} returned null");
                result.Add(property.Name, property.Value);
            }

            return result;
        }

        private static IReadOnlyDictionary<string, string>? ToPropertiesDictionary(IVsReferenceProperties? properties)
        {
            if (properties is null) { return null; }

            var result = new Dictionary<string, string>(properties.Count, StringComparer.OrdinalIgnoreCase);

            for (var i = 0; i < properties.Count; i++)
            {
                IVsReferenceProperty property = properties.Item(i) ?? throw new Exception($"Property index {i} returned null");
                result.Add(property.Name, property.Value ?? string.Empty);
            }

            return result;
        }

        private static IReadOnlyDictionary<string, IReadOnlyList<IVsReferenceItem2>> ToItemsDictionary(IVsTargetFrameworkInfo targetFrameworkInfo)
        {
            var result = new Dictionary<string, IReadOnlyList<IVsReferenceItem2>>(StringComparer.OrdinalIgnoreCase);

            if (targetFrameworkInfo.ProjectReferences?.Count > 0)
            {
                IReadOnlyList<IVsReferenceItem2> projectReferences = ToReferenceItemList(targetFrameworkInfo.ProjectReferences);
                result[ProjectItems.ProjectReference] = projectReferences;
            }

            if (targetFrameworkInfo.PackageReferences?.Count > 0)
            {
                IReadOnlyList<IVsReferenceItem2>? packageReferences = ToReferenceItemList(targetFrameworkInfo.PackageReferences);
                result[ProjectItems.PackageReference] = packageReferences;
            }

            if (targetFrameworkInfo is IVsTargetFrameworkInfo2 targetFrameworkInfo2)
            {
                if (targetFrameworkInfo2.PackageDownloads?.Count > 0)
                {
                    IReadOnlyList<IVsReferenceItem2>? packageDownloads = ToReferenceItemList(targetFrameworkInfo2.PackageDownloads);
                    result[ProjectItems.PackageDownload] = packageDownloads;
                }

                if (targetFrameworkInfo2.FrameworkReferences?.Count > 0)
                {
                    IReadOnlyList<IVsReferenceItem2>? frameworkReferences = ToReferenceItemList(targetFrameworkInfo2.FrameworkReferences);
                    result[ProjectItems.FrameworkReference] = frameworkReferences;
                }
            }

            if (targetFrameworkInfo is IVsTargetFrameworkInfo3 targetFrameworkInfo3
                && targetFrameworkInfo3.CentralPackageVersions?.Count > 0)
            {
                IReadOnlyList<IVsReferenceItem2>? centralPackageVersions = ToReferenceItemList(targetFrameworkInfo3.CentralPackageVersions);
                result[ProjectItems.PackageVersion] = centralPackageVersions;
            }

            return result;
        }

        private record VsProjectRestoreInfo3Adapter : IVsProjectRestoreInfo3
        {
            public required string MSBuildProjectExtensionsPath { get; init; }

            public required IReadOnlyList<IVsTargetFrameworkInfo4> TargetFrameworks { get; init; }

            public IReadOnlyList<IVsReferenceItem2>? ToolReferences { get; init; }

            public string? OriginalTargetFrameworks { get; init; }
        }

        private record VsTargetFramework4Adapter : IVsTargetFrameworkInfo4
        {
            public required string TargetFrameworkMoniker { get; init; }

            public IReadOnlyDictionary<string, IReadOnlyList<IVsReferenceItem2>>? Items { get; init; }

            public required IReadOnlyDictionary<string, string> Properties { get; init; }
        }

        private record VsReferenceItem2Adapter : IVsReferenceItem2
        {
            public required string Name { get; init; }

            public IReadOnlyDictionary<string, string>? Metadata { get; init; }
        }
    }
}
