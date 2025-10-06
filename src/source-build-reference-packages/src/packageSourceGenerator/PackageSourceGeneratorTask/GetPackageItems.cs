// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NuGet.Client;
using NuGet.ContentModel;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;

namespace Microsoft.DotNet.SourceBuild.Tasks
{
    public partial class GetPackageItems : Task
    {
        private const string PlaceholderFile = "_._";
        private static readonly Dictionary<string, (string, string)> s_strongNameKeyToNameMap = new()
        {
            { "b03f5f7f11d50a3a", ("Microsoft", "MSFT") },
            { "31bf3856ad364e35", ("MicrosoftShared", "35MSSharedLib1024") },
            { "adb9793829ddae60", ("MicrosoftAspNetCore", "AspNetCore") },
            { "b77a5c561934e089", ("ECMA", "ECMA") },
            { "cc7b13ffcd2ddd51", ("Open", "Open") },
            { "979442b78dfc278e", ("Humanizer", "Humanizer") }
        };

        /// <summary>
        /// The path to the .nupkg package file.
        /// </summary>
        [Required]
        public string? PackagePath { get; set; }

        /// <summary>
        /// Include only the package compile items and dependencies with a matching target framework.
        /// </summary>
        public string? IncludeTargetFrameworks { get; set; }

        /// <summary>
        /// Exclude package compile items and dependencies with a matching target framework.
        /// </summary>
        public string? ExcludeTargetFrameworks { get; set; }

        /// <summary>
        /// The package's compile items, including target framework metadata.
        /// </summary>
        [Output]
        public ITaskItem[]? CompileItems { get; set; }

        /// <summary>
        /// Placeholder files from compile target framework groups.
        /// </summary>
        [Output]
        public ITaskItem[]? PlaceholderFiles { get; set; }

        /// <summary>
        /// The package's dependencies with the PackageId as the identity and the version and target framework as metadata.
        /// </summary>
        [Output]
        public ITaskItem[]? PackageDependencies { get; set; }

        /// <summary>
        /// The package's framework references with the framework reference assembly as the identity and the target framework as metadata.
        /// </summary>
        [Output]
        public ITaskItem[]? FrameworkReferences { get; set; }

        /// <summary>
        /// The package id.
        /// </summary>
        [Output]
        public string? PackageId { get; set; }

        public override bool Execute()
        {
            using PackageArchiveReader packageArchiveReader = new(PackagePath!);
            TargetFrameworkRegexFilter targetFrameworkRegexFilter = new(IncludeTargetFrameworks,
                ExcludeTargetFrameworks);

            SetCompileItems(packageArchiveReader, targetFrameworkRegexFilter);
            SetPackageDependencies(packageArchiveReader, targetFrameworkRegexFilter);
            SetFrameworkReferences(packageArchiveReader, targetFrameworkRegexFilter);
            PackageId = packageArchiveReader.GetIdentity().Id;

            if (targetFrameworkRegexFilter.FoundExcludedTargetFrameworks.Count > 0)
            {
                Log.LogMessage(MessageImportance.High,
                    "Excluding target frameworks: {0}.",
                    string.Join(", ", targetFrameworkRegexFilter.FoundExcludedTargetFrameworks));
            }

            return true;
        }

        private void SetCompileItems(PackageArchiveReader packageArchiveReader,
            TargetFrameworkRegexFilter targetFrameworkRegexFilter)
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
                .Where(nugetFramework => targetFrameworkRegexFilter.IsIncludedAndNotExcluded(nugetFramework.GetShortFolderName()))
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
                    // Skip duplicate compile items. That can happen when different target frameworks choose
                    // the same asset as best compatible. E.g. System.Runtime.CompilerServices.Unsafe/4.7.0
                    // netcoreapp2.0 and netstandard2.0 TFMs both choose the netstandard2.0 compile asset.
                    if (compileTaskItems.Any(compileTaskItem => compileTaskItem.ItemSpec == compileItem.Path))
                        continue;

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
                        string assemblyPath = Path.Combine(Path.GetDirectoryName(PackagePath!)!, compileItem.Path);
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
            TargetFrameworkRegexFilter targetFrameworkRegexFilter)
        {
            IEnumerable<PackageDependencyGroup> packageDependencyGroups = packageArchiveReader.GetPackageDependencies();
            List<ITaskItem> packageDependencyTaskItems = new();

            foreach (PackageDependencyGroup packageDependencyGroup in packageDependencyGroups)
            {
                string targetFramework = packageDependencyGroup.TargetFramework.GetShortFolderName();
                if (!targetFrameworkRegexFilter.IsIncludedAndNotExcluded(targetFramework))
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
            TargetFrameworkRegexFilter targetFrameworkRegexFilter)
        {
            IEnumerable<FrameworkSpecificGroup> frameworkSpecificGroups = packageArchiveReader.GetFrameworkItems();
            List<ITaskItem> frameworkReferenceTaskItems = new();

            foreach (FrameworkSpecificGroup frameworkSpecificGroup in frameworkSpecificGroups)
            {
                string targetFramework = frameworkSpecificGroup.TargetFramework.GetShortFolderName();
                if (!targetFrameworkRegexFilter.IsIncludedAndNotExcluded(targetFramework))
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

        [GeneratedRegex(@"PublicKeyToken=([\w]*)")]
        private static partial Regex GetStrongNameKeyRegex();

        private static bool TryGetStrongNameData(AssemblyName assemblyName, out StrongNameData strongNameData)
        {
            Match match = GetStrongNameKeyRegex().Match(assemblyName.FullName);
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
