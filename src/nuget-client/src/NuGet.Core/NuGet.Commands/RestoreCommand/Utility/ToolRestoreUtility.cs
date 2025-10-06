// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.ProjectModel;
using NuGet.Versioning;

namespace NuGet.Commands
{
    public static class ToolRestoreUtility
    {
        /// <summary>
        /// Build a package spec in memory to execute the tool restore as if it were
        /// its own project. For now, we always restore for a null runtime and a single
        /// constant framework.
        /// </summary>
        public static PackageSpec GetSpec(string projectFilePath, string id, VersionRange versionRange, NuGetFramework framework, string packagesPath, IList<string> fallbackFolders, IList<PackageSource> sources, WarningProperties projectWideWarningProperties)

        {
            var frameworkShortFolderName = framework.GetShortFolderName();
            var name = GetUniqueName(id, frameworkShortFolderName, versionRange);

            return new PackageSpec()
            {
                Name = name, // make sure this package never collides with a dependency
                FilePath = projectFilePath,
                Dependencies = new List<LibraryDependency>(),
                TargetFrameworks =
                {
                    new TargetFrameworkInformation
                    {
                        TargetAlias = frameworkShortFolderName,
                        FrameworkName = framework,
                        Dependencies =
                        [
                            new LibraryDependency()
                            {
                                LibraryRange = new LibraryRange(id, versionRange, LibraryDependencyTarget.Package)
                            }
                        ]
                    }
                },
                RestoreMetadata = new ProjectRestoreMetadata()
                {
                    ProjectStyle = ProjectStyle.DotnetCliTool,
                    ProjectName = name,
                    ProjectUniqueName = name,
                    ProjectPath = projectFilePath,
                    PackagesPath = packagesPath,
                    FallbackFolders = fallbackFolders,
                    Sources = sources,
                    OriginalTargetFrameworks = {
                        frameworkShortFolderName
                    },
                    TargetFrameworks =
                    {
                        new ProjectRestoreMetadataFrameworkInfo
                        {
                            TargetAlias = frameworkShortFolderName,
                            FrameworkName = framework,
                            ProjectReferences = { }
                        }
                    },
                    ProjectWideWarningProperties = projectWideWarningProperties ?? new WarningProperties()
                }
            };
        }

        public static string GetUniqueName(string id, string framework, VersionRange versionRange)
        {
            return $"{id}-{framework}-{versionRange.ToNormalizedString()}".ToLowerInvariant();
        }

        /// <summary>
        /// Only one output can win per packages folder/version range. Between colliding requests take
        /// the intersection of the inputs used.
        /// </summary>
        /// <returns></returns>
        public static IReadOnlyList<RestoreSummaryRequest> GetSubSetRequests(IEnumerable<RestoreSummaryRequest> requestSummaries)
        {
            var results = new List<RestoreSummaryRequest>();
            var tools = new List<RestoreSummaryRequest>();

            foreach (var requestSummary in requestSummaries)
            {
                if (requestSummary.Request.Project.RestoreMetadata?.ProjectStyle == ProjectStyle.DotnetCliTool)
                {
                    tools.Add(requestSummary);
                }
                else
                {
                    // Pass non-tools to the output
                    results.Add(requestSummary);
                }
            }

            foreach (var toolIdGroup in tools.GroupBy(e => GetToolIdOrNullFromSpec(e.Request.Project), StringComparer.OrdinalIgnoreCase))
            {
                if (string.IsNullOrEmpty(toolIdGroup.Key))
                {
                    // Pass problem requests on to fail with a better error message
                    results.AddRange(toolIdGroup);
                }
                else
                {
                    // Actually narrow down the requests now
                    results.AddRange(GetSubSetRequestsForSingleId(toolIdGroup));
                }
            }

            return results;
        }

        public static IReadOnlyList<RestoreSummaryRequest> GetSubSetRequestsForSingleId(IEnumerable<RestoreSummaryRequest> requests)
        {
            var results = new List<RestoreSummaryRequest>();

            // Unique by packages folder
            foreach (var packagesFolderGroup in requests.GroupBy(e => e.Request.PackagesDirectory, StringComparer.Ordinal))
            {
                // Unique by version range
                foreach (var versionRangeGroup in packagesFolderGroup.GroupBy(e =>
                    GetToolDependencyOrNullFromSpec(e.Request.Project)?.LibraryRange?.VersionRange))
                {
                    // This could be improved in the future, for now take the request with the least sources
                    // to ensure that if this is going to fail anywhere it will *probably* consistently fail.
                    // Take requests with no imports over requests that do need imports to increase the chance
                    // of failing.
                    var bestRequest = versionRangeGroup
                        .OrderBy(e => e.Request.Project.TargetFrameworks.Any(f => f.FrameworkName is FallbackFramework) ? 1 : 0)
                        .ThenBy(e => e.Request.DependencyProviders.RemoteProviders.Count)
                        .First();

                    results.Add(bestRequest);
                }
            }

            return results;
        }

        /// <summary>
        /// Returns the name of the single dependency in the spec or null.
        /// </summary>
        public static string GetToolIdOrNullFromSpec(PackageSpec spec)
        {
            return GetToolDependencyOrNullFromSpec(spec)?.Name;
        }

        /// <summary>
        /// Returns the name of the single dependency in the spec or null.
        /// </summary>
        public static LibraryDependency GetToolDependencyOrNullFromSpec(PackageSpec spec)
        {
            if (spec == null)
            {
                return null;
            }

            return spec.Dependencies.Concat(spec.TargetFrameworks.SelectMany(e => e.Dependencies)).SingleOrDefault();
        }

        public static LockFileTargetLibrary GetToolTargetLibrary(LockFile toolLockFile, string toolId)
        {
            var target = toolLockFile.Targets.Single();
            return target
                .Libraries
                .FirstOrDefault(l => StringComparer.OrdinalIgnoreCase.Equals(toolId, l.Name));
        }
    }
}
