// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Client;
using NuGet.Common;
using NuGet.ContentModel;
using NuGet.DependencyResolver;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;
using NuGet.Repositories;

namespace NuGet.Commands
{
    internal class CompatibilityChecker
    {
        private readonly IReadOnlyList<NuGetv3LocalRepository> _localRepositories;
        private readonly LockFile _lockFile;
        private readonly ILogger _log;
        private readonly bool _validateRuntimeAssets;

        public CompatibilityChecker(IReadOnlyList<NuGetv3LocalRepository> localRepositories, LockFile lockFile, bool validateRuntimeAssets, ILogger log)
        {
            _localRepositories = localRepositories;
            _lockFile = lockFile;
            _log = log;
            _validateRuntimeAssets = validateRuntimeAssets;
        }

        internal async Task<CompatibilityCheckResult> CheckAsync(
            RestoreTargetGraph graph,
            Dictionary<string, LibraryIncludeFlags> includeFlags,
            PackageSpec packageSpec)
        {
            // The Compatibility Check is designed to alert the user to cases where packages are not behaving as they would
            // expect, due to compatibility issues.
            //
            // During this check, we scan all packages for a given restore graph and check the following conditions
            // (using an example TxM 'foo' and an example Runtime ID 'bar'):
            //
            // * If any package provides a "ref/foo/Thingy.dll", there MUST be a matching "lib/foo/Thingy.dll" or
            //   "runtimes/bar/lib/foo/Thingy.dll" provided by a package in the graph.
            // * All packages that contain Managed Assemblies must provide assemblies for 'foo'. If a package
            //   contains any of 'ref/' folders, 'lib/' folders, or framework assemblies, it must provide at least
            //   one of those for the 'foo' framework. Otherwise, the package is intending to provide managed assemblies
            //   but it does not support the target platform. If a package contains only 'content/', 'build/', 'tools/' or
            //   other NuGet convention folders, it is exempt from this check. Thus, content-only packages are always considered
            //   compatible, regardless of if they actually provide useful content.
            //
            // It is up to callers to invoke the compatibility check on the graphs they wish to check, but the general behavior in
            // the restore command is to invoke a compatibility check for each of:
            //
            // * The Targets (TxMs) defined in the project.json, with no Runtimes
            // * All combinations of TxMs and Runtimes defined in the project.json
            // * Additional (TxMs, Runtime) pairs defined by the "supports" mechanism in project.json
            var runtimeAssemblies = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var compileAssemblies = new Dictionary<string, LibraryIdentity>(StringComparer.OrdinalIgnoreCase);
            var issues = new List<CompatibilityIssue>();

            if (packageSpec.RestoreMetadata?.ProjectStyle == ProjectStyle.DotnetToolReference)
            {
                // Autoreferenced packages are allowed. Currently they're using Microsoft.NET.Platforms as an auto-ref package
                if (packageSpec.GetAllPackageDependencies().Count(e => !e.AutoReferenced) != 1)
                {
                    // Create issue
                    var issue = CompatibilityIssue.IncompatibleProjectType(
                        new PackageIdentity(packageSpec.Name, packageSpec.Version));

                    issues.Add(issue);
                    await _log.LogAsync(GetErrorMessage(NuGetLogCode.NU1211, issue, graph));
                }
            }

            // Verify framework assets also as part of runtime assets validation.
            foreach (var node in graph.Flattened)
            {
                // Check project compatibility
                if (node.Key.Type == LibraryType.Project)
                {
                    // Get the full library
                    var localMatch = node.Data?.Match as LocalMatch;
                    if (localMatch == null || !IsProjectFrameworkCompatible(localMatch.LocalLibrary))
                    {
                        var available = new List<NuGetFramework>();

                        // If the project info is available find all available frameworks
                        if (localMatch?.LocalLibrary != null)
                        {
                            available = GetProjectFrameworks(localMatch.LocalLibrary);
                        }

                        // Create issue
                        var issue = CompatibilityIssue.IncompatibleProject(
                            new PackageIdentity(node.Key.Name, node.Key.Version),
                            graph.Framework,
                            graph.RuntimeIdentifier,
                            available);

                        issues.Add(issue);
                        await _log.LogAsync(GetErrorMessage(NuGetLogCode.NU1201, issue, graph));
                    }

                    // Skip further checks on projects
                    continue;
                }

                // Find the include/exclude flags for this package
                LibraryIncludeFlags packageIncludeFlags;
                if (!includeFlags.TryGetValue(node.Key.Name, out packageIncludeFlags))
                {
                    packageIncludeFlags = LibraryIncludeFlags.All;
                }

                // If the package has compile and runtime assets excluded the compatibility check
                // is not needed. Packages with no ref or lib entries are considered
                // compatible in IsCompatible.
                if ((packageIncludeFlags &
                        (LibraryIncludeFlags.Compile
                        | LibraryIncludeFlags.Runtime)) == LibraryIncludeFlags.None)
                {
                    continue;
                }

                var compatibilityData = GetCompatibilityData(graph, node.Key, packageSpec);
                if (compatibilityData == null)
                {
                    continue;
                }

                if (!IsPackageCompatible(compatibilityData))
                {
                    var available = GetPackageFrameworks(compatibilityData, graph);

                    var issue = CompatibilityIssue.IncompatiblePackage(
                        new PackageIdentity(node.Key.Name, node.Key.Version),
                        graph.Framework,
                        graph.RuntimeIdentifier,
                        available);

                    issues.Add(issue);
                    await _log.LogAsync(GetErrorMessage(NuGetLogCode.NU1202, issue, graph));
                }

                if (!IsPackageTypeCompatible(compatibilityData))
                {
                    var issue = CompatibilityIssue.IncompatiblePackageType(
                        new PackageIdentity(node.Key.Name, node.Key.Version),
                        graph.Framework,
                        graph.RuntimeIdentifier);

                    issues.Add(issue);
                    await _log.LogAsync(GetErrorMessage(NuGetLogCode.NU1213, issue, graph));
                }

                await VerifyDotnetToolCompatibilityChecks(compatibilityData, node, graph, issues);

                // Check for matching ref/libs if we're checking a runtime-specific graph
                var targetLibrary = compatibilityData.TargetLibrary;
                if (_validateRuntimeAssets && !string.IsNullOrEmpty(graph.RuntimeIdentifier))
                {
                    // Skip runtime checks for packages that have runtime references excluded,
                    // this allows compile only packages that do not have runtimes for the 
                    // graph RID to be used.
                    if ((packageIncludeFlags & LibraryIncludeFlags.Runtime) == LibraryIncludeFlags.Runtime)
                    {
                        // Scan the package for ref assemblies
                        foreach (var compile in targetLibrary.CompileTimeAssemblies
                            .Where(p => Path.GetExtension(p.Path)
                                .Equals(".dll", StringComparison.OrdinalIgnoreCase)))
                        {
                            var name = Path.GetFileNameWithoutExtension(compile.Path);

                            // If we haven't already started tracking this compile-time assembly, AND there isn't already a runtime-loadable version
                            if (!compileAssemblies.ContainsKey(name) && !runtimeAssemblies.Contains(name))
                            {
                                // Track this assembly as potentially compile-time-only
                                compileAssemblies.Add(name, node.Key);
                            }
                        }

                        // Match up runtime assemblies
                        foreach (var runtime in targetLibrary.RuntimeAssemblies
                            .Where(p => Path.GetExtension(p.Path)
                                .Equals(".dll", StringComparison.OrdinalIgnoreCase)))
                        {
                            var name = Path.GetFileNameWithoutExtension(runtime.Path);

                            // If there was a compile-time-only assembly under this name...
                            if (compileAssemblies.ContainsKey(name))
                            {
                                // Remove it, we've found a matching runtime ref
                                compileAssemblies.Remove(name);
                            }

                            // Track this assembly as having a runtime assembly
                            runtimeAssemblies.Add(name);

                            // Fix for NuGet/Home#752 - Consider ".ni.dll" (native image/ngen) files matches for ref/ assemblies
                            if (name.EndsWith(".ni", StringComparison.OrdinalIgnoreCase))
                            {
                                var withoutNi = name.Substring(0, name.Length - 3);

                                if (compileAssemblies.ContainsKey(withoutNi))
                                {
                                    compileAssemblies.Remove(withoutNi);
                                }

                                runtimeAssemblies.Add(withoutNi);
                            }
                        }
                    }
                }
            }

            // Generate errors for un-matched reference assemblies, if we're checking a runtime-specific graph
            if (_validateRuntimeAssets && !string.IsNullOrEmpty(graph.RuntimeIdentifier))
            {
                foreach (var compile in compileAssemblies)
                {
                    var issue = CompatibilityIssue.ReferenceAssemblyNotImplemented(
                        compile.Key,
                        new PackageIdentity(compile.Value.Name, compile.Value.Version),
                        graph.Framework,
                        graph.RuntimeIdentifier);

                    issues.Add(issue);
                    await _log.LogAsync(GetErrorMessage(NuGetLogCode.NU1203, issue, graph));
                }
            }

            return new CompatibilityCheckResult(graph, issues);
        }


        /// <summary>
        /// Create an error message for the given issue.
        /// </summary>
        private static RestoreLogMessage GetErrorMessage(NuGetLogCode logCode, CompatibilityIssue issue, RestoreTargetGraph graph)
        {
            return RestoreLogMessage.CreateError(logCode, issue.Format(), issue.Package.Id, graph.TargetGraphName);
        }

        private static IEnumerable<NuGetFramework> GetPackageFrameworks(
            CompatibilityData compatibilityData,
            RestoreTargetGraph graph)
        {
            var available = new HashSet<NuGetFramework>();

            var contentItems = new ContentItemCollection();
            contentItems.Load(compatibilityData.Files);

            var patterns = new[]
            {
                graph.Conventions.Patterns.ResourceAssemblies,
                graph.Conventions.Patterns.CompileRefAssemblies,
                graph.Conventions.Patterns.RuntimeAssemblies,
                graph.Conventions.Patterns.EmbedAssemblies,
                graph.Conventions.Patterns.ContentFiles
            };

            List<ContentItemGroup> itemGroups = new();
            foreach (var pattern in patterns)
            {
                itemGroups.Clear();
                contentItems.PopulateItemGroups(pattern, itemGroups);
                foreach (var group in itemGroups)
                {
                    // lib/net45/subfolder/a.dll will be returned as a group with zero items since sub
                    // folders are not allowed. Completely empty groups are not compatible, a group with
                    // _._ would contain _._ as an item.
                    if (group.Items.Count > 0)
                    {
                        group.Properties.TryGetValue(ManagedCodeConventions.PropertyNames.RuntimeIdentifier, out var ridObj);
                        group.Properties.TryGetValue(ManagedCodeConventions.PropertyNames.TargetFrameworkMoniker, out var tfmObj);

                        var tfm = tfmObj as NuGetFramework;

                        // RID specific items should be ignored here since they are only used in the runtime assembly check
                        if (ridObj == null && tfm?.IsSpecificFramework == true)
                        {
                            available.Add(tfm);
                        }
                    }
                }
            }

            return available;
        }

        private static List<NuGetFramework> GetProjectFrameworks(Library localLibrary)
        {
            var available = new List<NuGetFramework>();

            object frameworksObject;
            if (localLibrary.Items.TryGetValue(
                KnownLibraryProperties.ProjectFrameworks,
                out frameworksObject))
            {
                available = (List<NuGetFramework>)frameworksObject;
            }

            return available;
        }

        private static bool IsProjectFrameworkCompatible(Library library)
        {
            object frameworkInfoObject;
            if (library.Items.TryGetValue(
                KnownLibraryProperties.TargetFrameworkInformation,
                out frameworkInfoObject))
            {
                var targetFrameworkInformation = (TargetFrameworkInformation)frameworkInfoObject;

                // Verify that a valid framework was selected
                return (targetFrameworkInformation.FrameworkName != null
                    && targetFrameworkInformation.FrameworkName != NuGetFramework.UnsupportedFramework);
            }
            else
            {
                // For external projects that do not have any target framework info, assume
                // compatibility was checked before hand
                return true;
            }
        }

        private bool IsPackageCompatible(CompatibilityData compatibilityData)
        {
            // A package is compatible if it has...
            return
                HasCompatibleAssets(compatibilityData.TargetLibrary) ||
                !compatibilityData.Files.Any(p =>
                    p.StartsWith("ref/", StringComparison.OrdinalIgnoreCase)
                    || p.StartsWith("lib/", StringComparison.OrdinalIgnoreCase));                       // No assemblies at all (for any TxM)
        }

        /// <summary>
        /// Whether a package has an incompatible package type. Currently, only 1 package type is incompatible in every project graph and that's DotnetPlatform.
        /// </summary>
        /// <param name="compatibilityData"></param>
        private bool IsPackageTypeCompatible(CompatibilityData compatibilityData)
        {
            // A package is compatible if it is not DotnetPlatform package type
            return !compatibilityData.TargetLibrary.PackageType.Contains(PackageType.DotnetPlatform);
        }

        private static HashSet<FrameworkRuntimePair> GetAvailableFrameworkRuntimePairs(CompatibilityData compatibilityData, RestoreTargetGraph graph)
        {
            var available = new HashSet<FrameworkRuntimePair>();

            var contentItems = new ContentItemCollection();
            contentItems.Load(compatibilityData.Files);

            List<ContentItemGroup> itemGroups = new List<ContentItemGroup>();
            if (compatibilityData.TargetLibrary.PackageType.Contains(PackageType.DotnetTool))
            {
                contentItems.PopulateItemGroups(graph.Conventions.Patterns.ToolsAssemblies, itemGroups);
                foreach (var group in itemGroups)
                {
                    group.Properties.TryGetValue(ManagedCodeConventions.PropertyNames.RuntimeIdentifier, out var ridObj);
                    group.Properties.TryGetValue(ManagedCodeConventions.PropertyNames.TargetFrameworkMoniker, out var tfmObj);

                    var tfm = tfmObj as NuGetFramework;
                    var rid = ridObj as string;
                    if (tfm?.IsSpecificFramework == true)
                    {
                        available.Add(new FrameworkRuntimePair(tfm, rid));
                    }
                }
            }

            return available;
        }

        private async Task VerifyDotnetToolCompatibilityChecks(CompatibilityData compatibilityData, GraphItem<RemoteResolveResult> node, RestoreTargetGraph graph, List<CompatibilityIssue> issues)
        {
            var containsDotnetToolPackageType = compatibilityData.TargetLibrary.PackageType.Contains(PackageType.DotnetTool);

            if (compatibilityData.TargetLibrary.PackageType.Count != 1 && containsDotnetToolPackageType)
            {
                var issue = CompatibilityIssue.ToolsPackageWithExtraPackageTypes(
                    new PackageIdentity(node.Key.Name, node.Key.Version));

                issues.Add(issue);
                await _log.LogAsync(GetErrorMessage(NuGetLogCode.NU1204, issue, graph));
            }

            if (containsDotnetToolPackageType &&
                    !(HasCompatibleToolsAssets(compatibilityData.TargetLibrary) || !compatibilityData.Files.Any(p => p.StartsWith("tools/", StringComparison.OrdinalIgnoreCase))))
            {
                var available = GetAvailableFrameworkRuntimePairs(compatibilityData, graph);
                var issue = CompatibilityIssue.IncompatibleToolsPackage(
                        new PackageIdentity(node.Key.Name, node.Key.Version),
                        graph.Framework,
                        graph.RuntimeIdentifier,
                        available);

                issues.Add(issue);
                await _log.LogAsync(GetErrorMessage(NuGetLogCode.NU1202, issue, graph));
            }

            if (ProjectStyle.DotnetToolReference == compatibilityData.PackageSpec.RestoreMetadata?.ProjectStyle)
            {
                // If the package is not autoreferenced or a tool package
                if (!containsDotnetToolPackageType && compatibilityData.PackageSpec.GetAllPackageDependencies().Where(e => !e.AutoReferenced).Any(e => e.Name.Equals(compatibilityData.TargetLibrary.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    var issue = CompatibilityIssue.IncompatiblePackageWithDotnetTool(new PackageIdentity(node.Key.Name, node.Key.Version));
                    issues.Add(issue);
                    await _log.LogAsync(GetErrorMessage(NuGetLogCode.NU1212, issue, graph));
                }
            }
            else
            {
                if (containsDotnetToolPackageType)
                {
                    var issue = CompatibilityIssue.IncompatiblePackageWithDotnetTool(new PackageIdentity(node.Key.Name, node.Key.Version));
                    issues.Add(issue);
                    await _log.LogAsync(GetErrorMessage(NuGetLogCode.NU1212, issue, graph));
                }
            }
        }


        /// <summary>
        /// Check if the library contains assets.
        /// </summary>
        internal static bool HasCompatibleAssets(LockFileTargetLibrary targetLibrary)
        {
            // A package is compatible if it has...
            return
                targetLibrary.RuntimeAssemblies.Count > 0 ||                          // Runtime Assemblies, or
                targetLibrary.CompileTimeAssemblies.Count > 0 ||                      // Compile-time Assemblies, or
                targetLibrary.FrameworkAssemblies.Count > 0 ||                        // Framework Assemblies, or
                targetLibrary.FrameworkReferences.Count > 0 ||                        // Framework References, or
                targetLibrary.ContentFiles.Count > 0 ||                               // Shared content
                targetLibrary.ResourceAssemblies.Count > 0 ||                         // Resources (satellite package)
                targetLibrary.Build.Count > 0 ||                                      // Build
                targetLibrary.BuildMultiTargeting.Count > 0;                          // Cross targeting build
        }

        internal static bool HasCompatibleToolsAssets(LockFileTargetLibrary targetLibrary)
        {
            return targetLibrary.ToolsAssemblies.Count > 0;  // Tools assemblies
        }


        private CompatibilityData GetCompatibilityData(RestoreTargetGraph graph, LibraryIdentity libraryId, PackageSpec packageSpec)
        {
            // Use data from the current lock file if it exists.
            LockFileTargetLibrary targetLibrary = null;

            for (int i = 0; i < _lockFile.Targets.Count; ++i)
            {
                var target = _lockFile.Targets[i];
                if (Equals(target.TargetFramework, graph.Framework) && string.Equals(target.RuntimeIdentifier, graph.RuntimeIdentifier, StringComparison.Ordinal))
                {
                    for (int j = 0; j < target.Libraries.Count; ++j)
                    {
                        var library = target.Libraries[j];
                        if (library.Name.Equals(libraryId.Name, StringComparison.OrdinalIgnoreCase) && library.Version.Equals(libraryId.Version))
                        {
                            targetLibrary = library;
                            break;
                        }
                    }

                    break;
                }
            }

            IEnumerable<string> files = null;
            for (var i = 0; i < _lockFile.Libraries.Count; i++)
            {
                LockFileLibrary library = _lockFile.Libraries[i];
                if (library.Name.Equals(libraryId.Name, StringComparison.OrdinalIgnoreCase) && library.Version.Equals(libraryId.Version))
                {
                    files = library.Files;
                    break;
                }
            }

            if (files == null || targetLibrary == null)
            {
                // We need to generate some of the data. We'll need the local package info to do that
                var packageInfo = NuGetv3LocalRepositoryUtility.GetPackage(
                    _localRepositories,
                    libraryId.Name,
                    libraryId.Version);

                if (packageInfo == null)
                {
                    return null;
                }

                // Collect the file list if necessary
                if (files == null)
                {
                    files = packageInfo.Package.Files;
                }

                // Generate the target library if necessary
                if (targetLibrary == null)
                {
                    targetLibrary = LockFileUtils.CreateLockFileTargetLibrary(
                        library: null,
                        package: packageInfo.Package,
                        targetGraph: graph,
                        dependencyType: LibraryIncludeFlags.All);
                }
            }

            return new CompatibilityData(files, targetLibrary, packageSpec);
        }

        private class CompatibilityData
        {
            public IEnumerable<string> Files { get; }
            public LockFileTargetLibrary TargetLibrary { get; }

            public PackageSpec PackageSpec { get; }

            public CompatibilityData(IEnumerable<string> files, LockFileTargetLibrary targetLibrary, PackageSpec packageSpec)
            {
                Files = files;
                TargetLibrary = targetLibrary;
                PackageSpec = packageSpec;
            }
        }
    }
}
