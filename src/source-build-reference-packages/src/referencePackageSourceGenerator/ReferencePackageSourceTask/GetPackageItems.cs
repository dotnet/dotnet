// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NuGet.Client;
using NuGet.ContentModel;
using NuGet.Packaging;
using NuGet.Frameworks;
using NuGet.Packaging.Core;

namespace Microsoft.DotNet.SourceBuild.Tasks
{
    public partial class GetPackageItems : Task
    {
        private const string PlaceholderFile = "_._";
        private const char TargetFrameworkDelimiter = ';';
        private static readonly Dictionary<string, (string, string)> s_strongNameKeyToNameMap = new()
        {
            { "b03f5f7f11d50a3a", ("Microsoft", "MSFT") },
            { "31bf3856ad364e35", ("MicrosoftShared", "35MSSharedLib1024") },
            { "adb9793829ddae60", ("MicrosoftAspNetCore", "AspNetCore") },
            { "b77a5c561934e089", ("ECMA", "ECMA") },
            { "cc7b13ffcd2ddd51", ("Open", "Open") }
        };

        /// <summary>
        /// The path to the .nupkg package file.
        /// </summary>
        [Required]
        public string PackagePath { get; set; }

        /// <summary>
        /// Include only the package compile items and dependencies with a matching target framework.
        /// </summary>
        public string IncludeTargetFrameworks { get; set; }

        /// <summary>
        /// Exclude package compile items and dependencies with a matching target framework.
        /// </summary>
        public string ExcludeTargetFrameworks { get; set; }

        /// <summary>
        /// The package's compile items, including target framework metadata.
        /// </summary>
        [Output]
        public ITaskItem[] CompileItems { get; set; }

        /// <summary>
        /// Placeholder files from compile target framework groups.
        /// </summary>
        [Output]
        public ITaskItem[] PlaceholderFiles { get; set; }

        /// <summary>
        /// The package's dependencies with the PackageId as the identity and the version and target framework as metadata.
        /// </summary>
        [Output]
        public ITaskItem[] PackageDependencies { get; set; }

        /// <summary>
        /// The package's framework references with the framework reference assembly as the identity and the target framework as metadata.
        /// </summary>
        [Output]
        public ITaskItem[] FrameworkReferences { get; set; }

        /// <summary>
        /// The package id.
        /// </summary>
        [Output]
        public string PackageId { get; set; }

        public override bool Execute()
        {
            Regex[] includeTargetFrameworks = TransformPatternsToRegexList(IncludeTargetFrameworks).ToArray();
            Regex[] excludeTargetFrameworks = TransformPatternsToRegexList(ExcludeTargetFrameworks).ToArray();
            HashSet<string> excludedTargetFrameworkLogBag = new();

            using PackageArchiveReader packageArchiveReader = new(PackagePath);
            bool isTargetFrameworkIncluded(string targetFramework)
            {
                // Skip empty target frameworks.
                if (string.IsNullOrWhiteSpace(targetFramework))
                    return false;

                // Skip "services" target framework added by NuGet.
                if (targetFramework == "services")
                    return false;

                // Skip target frameworks that aren't included in the IncludeTargetFrameworks filter.
                if (includeTargetFrameworks.Length > 0 && includeTargetFrameworks.All(r => !r.IsMatch(targetFramework)))
                    return false;

                // Skip target frameworks that are excluded.
                if (excludeTargetFrameworks.Length > 0 && excludeTargetFrameworks.Any(r => r.IsMatch(targetFramework)))
                {
                    // Make sure that a warning is only logged once per target framework.
                    if (!excludedTargetFrameworkLogBag.Contains(targetFramework))
                    {
                        Log.LogMessage(MessageImportance.High,
                            "Encountered package '{0}' with excluded target framework '{1}'.",
                            Path.GetFileNameWithoutExtension(PackagePath),
                            targetFramework);
                        excludedTargetFrameworkLogBag.Add(targetFramework);
                    }

                    return false;
                }

                return true;
            }

            SetCompileItems(packageArchiveReader, isTargetFrameworkIncluded);
            SetPackageDependencies(packageArchiveReader, isTargetFrameworkIncluded);
            SetFrameworkReferences(packageArchiveReader, isTargetFrameworkIncluded);
            PackageId = packageArchiveReader.GetIdentity().Id;

            return true;
        }

        private void SetCompileItems(PackageArchiveReader packageArchiveReader,
            Func<string, bool> isTargetFrameworkIncluded)
        {
            IEnumerable<string> packageAssets = packageArchiveReader.GetFiles();
            ContentItemCollection contentItemCollection = new();
            contentItemCollection.Load(packageAssets);

            // Don't use the RID graph for compile assets.
            ManagedCodeConventions managedCodeConventions = new(runtimeGraph: null);

            IEnumerable<NuGetFramework> packageFrameworks = contentItemCollection.FindItems(managedCodeConventions.Patterns.CompileRefAssemblies)
                .Concat(contentItemCollection.FindItems(managedCodeConventions.Patterns.CompileLibAssemblies))
                .Where(t => t.Properties.ContainsKey("tfm"))
                .Select(t => (NuGetFramework)t.Properties["tfm"])
                .Where(nugetFramework => isTargetFrameworkIncluded(nugetFramework.GetShortFolderName()))
                .Distinct()
                .ToArray();

            List<ITaskItem> compileTaskItems = new();
            List<ITaskItem> placeholderTaskItems = new();

            foreach (NuGetFramework packageFramework in packageFrameworks)
            {
                string targetFramework = packageFramework.GetShortFolderName();

                SelectionCriteria managedCriteria = managedCodeConventions.Criteria.ForFramework(packageFramework);
                ContentItemGroup compileItems = contentItemCollection.FindBestItemGroup(managedCriteria,
                    managedCodeConventions.Patterns.CompileRefAssemblies,
                    managedCodeConventions.Patterns.CompileLibAssemblies);

                if (compileItems is null)
                    continue;

                foreach (ContentItem compileItem in compileItems.Items)
                {
                    TaskItem compileTaskItem = new(compileItem.Path);
                    compileTaskItem.SetMetadata(SharedMetadata.TargetFrameworkMetadataName, targetFramework);

                    bool isPlaceholderFile = Path.GetFileName(compileItem.Path) == PlaceholderFile;
                    if (isPlaceholderFile)
                    {
                        placeholderTaskItems.Add(compileTaskItem);
                    }
                    else
                    {
                        // Retrieve the strong name key information and add it to the compile item as metadata.
                        string assemblyPath = Path.Combine(Path.GetDirectoryName(PackagePath), compileItem.Path);
                        AssemblyName assemblyName = AssemblyName.GetAssemblyName(assemblyPath);
                        if (TryGetStrongNameData(assemblyName, out StrongNameData strongNameData))
                        {
                            compileTaskItem.SetMetadata(SharedMetadata.StrongNameKeyMetadataName, strongNameData.Key);
                            compileTaskItem.SetMetadata(SharedMetadata.StrongNameIdMetadataName, strongNameData.Id);
                            compileTaskItem.SetMetadata(SharedMetadata.StrongNameFilenameMetadataName, strongNameData.Filename);
                        }

                        compileTaskItem.SetMetadata(SharedMetadata.AssemblyNameMetadataName, assemblyName.Name);
                        compileTaskItems.Add(compileTaskItem);
                    }
                }
            }

            CompileItems = compileTaskItems.ToArray();
            PlaceholderFiles = placeholderTaskItems.ToArray();
        }

        private void SetPackageDependencies(PackageArchiveReader packageArchiveReader,
            Func<string, bool> isTargetFrameworkIncluded)
        {
            IEnumerable<PackageDependencyGroup> packageDependencyGroups = packageArchiveReader.GetPackageDependencies();
            List<ITaskItem> packageDependencyTaskItems = new();

            foreach (PackageDependencyGroup packageDependencyGroup in packageDependencyGroups)
            {
                string targetFramework = packageDependencyGroup.TargetFramework.GetShortFolderName();
                if (!isTargetFrameworkIncluded(targetFramework))
                    continue;

                foreach (PackageDependency packageDependency in packageDependencyGroup.Packages)
                {
                    // Skip runtime packages which don't contribute to the compile time experience.
                    if (packageDependency.Id.StartsWith("runtime.native"))
                        continue;

                    TaskItem packageDependencyTaskItem = new(packageDependency.Id);
                    // Try to create a fixed version from the package dependency's version range.
                    packageDependencyTaskItem.SetMetadata(SharedMetadata.VersionMetadataName, packageDependency.VersionRange.ToShortString().Trim('[', ']'));
                    packageDependencyTaskItem.SetMetadata(SharedMetadata.TargetFrameworkMetadataName, targetFramework);
                    packageDependencyTaskItems.Add(packageDependencyTaskItem);
                }
            }

            PackageDependencies = packageDependencyTaskItems.ToArray();
        }

        private void SetFrameworkReferences(PackageArchiveReader packageArchiveReader,
            Func<string, bool> isTargetFrameworkIncluded)
        {
            IEnumerable<FrameworkSpecificGroup> frameworkSpecificGroups = packageArchiveReader.GetFrameworkItems();
            List<ITaskItem> frameworkReferenceTaskItems = new();

            foreach (FrameworkSpecificGroup frameworkSpecificGroup in frameworkSpecificGroups)
            {
                string targetFramework = frameworkSpecificGroup.TargetFramework.GetShortFolderName();
                if (!isTargetFrameworkIncluded(targetFramework))
                    continue;

                foreach (string frameworkReference in frameworkSpecificGroup.Items)
                {
                    TaskItem frameworkReferenceTaskItem = new(frameworkReference);
                    frameworkReferenceTaskItem.SetMetadata(SharedMetadata.TargetFrameworkMetadataName, targetFramework);
                    frameworkReferenceTaskItems.Add(frameworkReferenceTaskItem);
                }
            }

            FrameworkReferences = frameworkReferenceTaskItems.ToArray();
        }

        private static IEnumerable<Regex> TransformPatternsToRegexList(string patterns)
        {
            if (string.IsNullOrWhiteSpace(patterns))
                yield break;

            string[] patternsSplit = patterns.Split(TargetFrameworkDelimiter, StringSplitOptions.RemoveEmptyEntries);

            foreach (string pattern in patternsSplit)
                yield return new Regex(pattern, RegexOptions.NonBacktracking | RegexOptions.Compiled);
        }

        [GeneratedRegex(@"PublicKeyToken=([\w]*)")]
        private static partial Regex StrongNameKeyRegex();

        private static bool TryGetStrongNameData(AssemblyName assemblyName, out StrongNameData strongNameData)
        {
            Match match = StrongNameKeyRegex().Match(assemblyName.FullName);
            if (!match.Success)
            {
                strongNameData = default;
                return false;
            }

            string strongNameKey = match.Groups[1].Value;
            if (s_strongNameKeyToNameMap.TryGetValue(strongNameKey, out (string Id, string Filename) value))
            {
                strongNameData = new StrongNameData(strongNameKey, value.Id, value.Filename);
                return true;
            }

            throw new ArgumentException($"Found strong name key that doesn't map: Key={strongNameKey}, Assembly={assemblyName.Name}");
        }
    }
}
