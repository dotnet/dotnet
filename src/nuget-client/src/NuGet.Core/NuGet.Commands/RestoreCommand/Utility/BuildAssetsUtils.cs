// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using NuGet.Common;
using NuGet.DependencyResolver;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;
using NuGet.Repositories;
using NuGet.Versioning;
using XmlUtility = NuGet.Shared.XmlUtility;

namespace NuGet.Commands
{
    public static class BuildAssetsUtils
    {
        private static readonly XNamespace Namespace = XNamespace.Get("http://schemas.microsoft.com/developer/msbuild/2003");
        internal const string CrossTargetingCondition = "'$(TargetFramework)' == ''";
        internal const string TargetFrameworkCondition = "'$(TargetFramework)' == '{0}'";
        internal const string LanguageCondition = "'$(Language)' == '{0}'";
        internal const string NegativeLanguageCondition = "'$(Language)' != '{0}'";
        internal const string ExcludeAllCondition = "'$(ExcludeRestorePackageImports)' != 'true'";
        public const string TargetsExtension = ".targets";
        public const string PropsExtension = ".props";

        /// <summary>
        /// The macros that we may use in MSBuild to replace path roots.
        /// </summary>
        public static readonly string[] MacroCandidates = new[]
        {
            "UserProfile", // e.g. C:\users\myusername
        };

        /// <summary>
        /// Write XML to disk.
        /// Delete files which do not have new XML.
        /// </summary>
        public static void WriteFiles(IEnumerable<MSBuildOutputFile> files, ILogger log)
        {
            foreach (var file in files)
            {
                if (file.Content == null)
                {
                    // Remove the file if the XML is null
                    FileUtility.Delete(file.Path);
                }
                else
                {
                    log.LogInformation(string.Format(CultureInfo.CurrentCulture, Strings.Log_GeneratingMsBuildFile, file.Path));

                    // Create the directory if it doesn't exist
                    Directory.CreateDirectory(Path.GetDirectoryName(file.Path));

                    // Write out XML file
                    WriteXML(file.Path, file.Content);
                }
            }
        }

        /// <summary>
        /// Create MSBuild targets and props files.
        /// Null will be returned for files that should be removed.
        /// </summary>
        public static List<MSBuildOutputFile> GenerateMultiTargetFailureFiles(
            string targetsPath,
            string propsPath,
            ProjectStyle restoreType)
        {
            XDocument targetsXML = null;
            XDocument propsXML = null;

            // Create an error file for MSBuild to stop the build.
            targetsXML = GenerateMultiTargetFrameworkWarning();

            if (restoreType == ProjectStyle.PackageReference)
            {
                propsXML = GenerateEmptyImportsFile();
            }

            var files = new List<MSBuildOutputFile>()
            {
                new MSBuildOutputFile(propsPath, propsXML),
                new MSBuildOutputFile(targetsPath, targetsXML),
            };

            return files;
        }

        private static string ReplacePathsWithMacros(string path)
        {
            foreach (var macroName in MacroCandidates)
            {
                string macroValue = Environment.GetEnvironmentVariable(macroName);
                if (!string.IsNullOrEmpty(macroValue)
                    && path.StartsWith(macroValue, StringComparison.OrdinalIgnoreCase))
                {
                    path = $"$({macroName})" + $"{path.Substring(macroValue.Length)}";
                }

                break;
            }

            return path;
        }

        public static XDocument GenerateMultiTargetFrameworkWarning()
        {
            var doc = GenerateEmptyImportsFile();

            doc.Root.Add(new XElement(Namespace + "Target",
                        new XAttribute("Name", "EmitMSBuildWarning"),
                        new XAttribute("BeforeTargets", "Build"),

                        new XElement(Namespace + "Warning",
                            new XAttribute("Text", Strings.MSBuildWarning_MultiTarget))));

            return doc;
        }

        /// <summary>
        /// Add standard properties to only props file if it exists, otherwise the targets.
        /// </summary>
        public static void AddNuGetPropertiesToFirstImport(IEnumerable<MSBuildOutputFile> files,
            IEnumerable<string> packageFolders,
            string repositoryRoot,
            ProjectStyle projectStyle,
            string assetsFilePath,
            bool success)
        {
            // For project.json not all files are written out. Find the first one
            // or if no files exist skip this.
            var firstImport = files.Where(file => file.Content != null)
                .OrderByDescending(file => file.Path.EndsWith(PropsExtension, StringComparison.Ordinal) ? 1 : 0)
                .FirstOrDefault();

            if (firstImport != null)
            {
                // Write the assets file path to the props file in an MSBuild resolvable manner.
                // This allows the project to be moved and avoid a large number of project errors
                // until restore can run again.
                var resolvableAssetsFilePath = @"$(MSBuildThisFileDirectory)" + Path.GetFileName(assetsFilePath);

                AddNuGetProperties(firstImport.Content, packageFolders, repositoryRoot, projectStyle, resolvableAssetsFilePath, success);
            }
        }

        /// <summary>
        /// Apply standard properties in a property group.
        /// Additionally add a SourceRoot item to point to the package folders.
        /// </summary>
        public static void AddNuGetProperties(
            XDocument doc,
            IEnumerable<string> packageFolders,
            string repositoryRoot,
            ProjectStyle projectStyle,
            string assetsFilePath,
            bool success)
        {

            doc.Root.AddFirst(
                new XElement(Namespace + "PropertyGroup",
                            new XAttribute("Condition", $" {ExcludeAllCondition} "),
                            GenerateProperty("RestoreSuccess", success.ToString(CultureInfo.CurrentCulture)),
                            GenerateProperty("RestoreTool", "NuGet"),
                            GenerateProperty("ProjectAssetsFile", assetsFilePath),
                            GenerateProperty("NuGetPackageRoot", ReplacePathsWithMacros(repositoryRoot)),
                            GenerateProperty("NuGetPackageFolders", string.Join(";", packageFolders)),
                            GenerateProperty("NuGetProjectStyle", projectStyle.ToString()),
                            GenerateProperty("NuGetToolVersion", MinClientVersionUtility.GetNuGetClientVersion().ToFullString())),
                new XElement(Namespace + "ItemGroup",
                            new XAttribute("Condition", $" {ExcludeAllCondition} "),
                            packageFolders.Select(e => GenerateItem("SourceRoot", PathUtility.EnsureTrailingSlash(e)))));
        }

        /// <summary>
        /// Get empty file with the base properties.
        /// </summary>
        public static XDocument GenerateEmptyImportsFile()
        {
            var doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "no"),

                new XElement(Namespace + "Project",
                    new XAttribute("ToolsVersion", "14.0"),
                    new XAttribute("xmlns", Namespace.NamespaceName)));

            return doc;
        }

        public static XElement GenerateProperty(string propertyName, string content)
        {
            return new XElement(Namespace + propertyName,
                            new XAttribute("Condition", $" '$({propertyName})' == '' "),
                            content);
        }

        internal static XElement GenerateItem(string itemName, string path)
        {
            return new XElement(Namespace + itemName, new XAttribute("Include", path));
        }

        public static XElement GenerateImport(string path)
        {
            return new XElement(Namespace + "Import",
                                new XAttribute("Project", path),
                                new XAttribute("Condition", $"Exists('{path}')"));
        }

        public static XElement GenerateContentFilesItem(string path, LockFileContentFile item, string packageId, string packageVersion)
        {
            var entry = new XElement(Namespace + item.BuildAction.Value,
                                new XAttribute("Include", path),
                                new XAttribute("Condition", $"Exists('{path}')"),
                                new XElement(Namespace + "NuGetPackageId", packageId),
                                new XElement(Namespace + "NuGetPackageVersion", packageVersion),
                                new XElement(Namespace + "NuGetItemType", item.BuildAction),
                                new XElement(Namespace + "Pack", false));

            var privateFlag = false;

            if (item.CopyToOutput)
            {
                var outputPath = item.OutputPath ?? item.PPOutputPath;

                if (outputPath != null)
                {
                    // Convert / to \
                    outputPath = LockFileUtils.ToDirectorySeparator(outputPath);

                    privateFlag = true;
                    entry.Add(new XElement(Namespace + "CopyToOutputDirectory", "PreserveNewest"));

                    entry.Add(new XElement(Namespace + "TargetPath", outputPath));

                    var destinationSubDirectory = Path.GetDirectoryName(outputPath);

                    if (!string.IsNullOrEmpty(destinationSubDirectory))
                    {
                        entry.Add(new XElement(Namespace + "DestinationSubDirectory", destinationSubDirectory + Path.DirectorySeparatorChar));
                    }
                }
            }

            entry.Add(new XElement(Namespace + "Private", privateFlag.ToString(CultureInfo.CurrentCulture)));

            // Remove contentFile/lang/tfm/ from start of the path
            var linkPath = string.Join(string.Empty + Path.DirectorySeparatorChar,
                    item.Path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
                        .Skip(3)
                        .ToArray());

            if (linkPath != null)
            {
                entry.Add(new XElement(Namespace + "Link", linkPath));
            }

            return entry;
        }

        /// <summary>
        /// Returns null if the result should not exist on disk.
        /// </summary>
        public static XDocument GenerateMSBuildFile(List<MSBuildRestoreItemGroup> groups, ProjectStyle outputType)
        {
            XDocument doc = null;

            // Always write out netcore props/targets. For project.json only write the file if it has items.
            if (outputType == ProjectStyle.PackageReference || groups.SelectMany(e => e.Items).Any())
            {
                doc = GenerateEmptyImportsFile();

                // Add import groups, order by position, then by the conditions to keep the results deterministic
                // Skip empty groups
                foreach (var group in groups
                    .Where(e => e.Items.Count > 0)
                    .OrderBy(e => e.Position)
                    .ThenBy(e => e.Condition, StringComparer.OrdinalIgnoreCase))
                {
                    var itemGroup = new XElement(Namespace + group.RootName, group.Items);

                    // Add a conditional statement if multiple TFMs exist or cross targeting is present
                    var conditionValue = group.Condition;
                    if (!string.IsNullOrEmpty(conditionValue))
                    {
                        itemGroup.Add(new XAttribute("Condition", conditionValue));
                    }

                    // Add itemgroup to file
                    doc.Root.Add(itemGroup);
                }
            }

            return doc;
        }

        public static void WriteXML(string path, XDocument doc)
        {
            FileUtility.Replace((outputPath) =>
            {
                using (var output = new FileStream(outputPath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
                {
                    doc.Save(output);
                }
            },
            path);
        }

        public static string GetPathWithMacros(string absolutePath, string repositoryRoot)
        {
            var path = absolutePath;

            if (absolutePath.StartsWith(repositoryRoot, StringComparison.Ordinal))
            {
                path = $"$(NuGetPackageRoot){absolutePath.Substring(repositoryRoot.Length)}";
            }
            else
            {
                path = ReplacePathsWithMacros(absolutePath);
            }

            return path;
        }

        /// <summary>
        /// Check if the file has changes compared to the original on disk.
        /// </summary>
        public static bool HasChanges(XDocument newFile, string path, ILogger log)
        {
            if (newFile == null)
            {
                // The file should be deleted if it is null.
                return File.Exists(path);
            }
            else
            {
                var existing = ReadExisting(path, log);

                if (existing != null)
                {
                    return !XDocument.DeepEquals(existing, newFile);
                }
            }

            return true;
        }

        public static XDocument ReadExisting(string path, ILogger log)
        {
            XDocument result = null;

            if (File.Exists(path))
            {
                try
                {
                    result = XmlUtility.Load(path);
                }
                catch (Exception ex)
                {
                    // Log a debug message and ignore, this will force an overwrite
                    log.LogDebug($"Failed to open imports file: {path} Error: {ex.Message}");
                }
            }

            return result;
        }

        public static string GetMSBuildFilePath(PackageSpec project, string extension)
        {
            string path;

            if (project.RestoreMetadata?.ProjectStyle == ProjectStyle.PackageReference || project.RestoreMetadata?.ProjectStyle == ProjectStyle.DotnetToolReference)
            {
                // PackageReference style projects
                var projFileName = Path.GetFileName(project.RestoreMetadata.ProjectPath);
                path = Path.Combine(project.RestoreMetadata.OutputPath, $"{projFileName}.nuget.g{extension}");
            }
            else
            {
                // Project.json style projects
                var dir = Path.GetDirectoryName(project.FilePath);
                path = Path.Combine(dir, $"{project.Name}.nuget{extension}");
            }
            return path;

        }

        public static string GetMSBuildFilePathForPackageReferenceStyleProject(PackageSpec project, string extension)
        {
            var projFileName = Path.GetFileName(project.RestoreMetadata.ProjectPath);

            return Path.Combine(project.RestoreMetadata.OutputPath, $"{projFileName}.nuget.g{extension}");
        }

        public static List<MSBuildOutputFile> GetMSBuildOutputFiles(PackageSpec project,
            LockFile assetsFile,
            IEnumerable<RestoreTargetGraph> targetGraphs,
            IReadOnlyList<NuGetv3LocalRepository> repositories,
            RestoreRequest request,
            string assetsFilePath,
            bool restoreSuccess,
            ILogger log)
        {
            // Generate file names
            var targetsPath = GetMSBuildFilePath(project, TargetsExtension);
            var propsPath = GetMSBuildFilePath(project, PropsExtension);

            // Targets files contain a macro for the repository root. If only the user package folder was used
            // allow a replacement. If fallback folders were used the macro cannot be applied.
            // Do not use macros for fallback folders. Use only the first repository which is the user folder.
            var repositoryRoot = repositories.First().RepositoryRoot;

            // Invalid msbuild projects should write out an msbuild error target
            if (!targetGraphs.Any())
            {
                return GenerateMultiTargetFailureFiles(
                    targetsPath,
                    propsPath,
                    request.ProjectStyle);
            }

            // Add additional conditionals for multi targeting
            var multiTargetingFromMetadata = (request.Project.RestoreMetadata?.CrossTargeting == true);

            var isMultiTargeting = multiTargetingFromMetadata
                || request.Project.TargetFrameworks.Count > 1;

            // ItemGroups for each file.
            var props = new List<MSBuildRestoreItemGroup>();
            var targets = new List<MSBuildRestoreItemGroup>();

            // MultiTargeting imports are shared between TFMs, to avoid
            // duplicate import warnings only add each once.
            var multiTargetingImportsAdded = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // Skip runtime graphs, msbuild targets may not come from RID specific packages.
            var ridlessTargets = assetsFile.Targets
                .Where(e => string.IsNullOrEmpty(e.RuntimeIdentifier));

            var packagesWithTools = new HashSet<string>(assetsFile.Libraries.Where(i => i.HasTools).Select(i => i.Name), StringComparer.OrdinalIgnoreCase);

            foreach (var ridlessTarget in ridlessTargets)
            {
                var frameworkConditions = string.Format(
                        CultureInfo.InvariantCulture,
                        TargetFrameworkCondition,
                        GetMatchingFrameworkStrings(project, ridlessTarget.TargetFramework));

                // Find matching target in the original target graphs.
                var targetGraph = targetGraphs.FirstOrDefault(e =>
                    string.IsNullOrEmpty(e.RuntimeIdentifier)
                    && ridlessTarget.TargetFramework == e.Framework);

                // Sort by dependency order, child package assets should appear higher in the
                // msbuild targets and props files so that parents can depend on them.
                var sortedGraph = TopologicalSortUtility.SortPackagesByDependencyOrder(ConvertToPackageDependencyInfo(targetGraph.Flattened));

                // Filter out to packages only, exclude projects.
                var packageType = new HashSet<string>(
                    targetGraph.Flattened.Where(e => e.Key.Type == LibraryType.Package)
                        .Select(e => e.Key.Name),
                    StringComparer.OrdinalIgnoreCase);

                // Package -> PackageInfo
                // PackageInfo is kept lazy to avoid hitting the disk for packages
                // with no relevant assets.
                var sortedPackages = sortedGraph.Where(e => packageType.Contains(e.Id))
                                                .Select(sortedPkg =>
                                                    new KeyValuePair<LockFileTargetLibrary, Lazy<LocalPackageSourceInfo>>(
                                                        key: ridlessTarget.Libraries.FirstOrDefault(assetsPkg =>
                                                            sortedPkg.Version == assetsPkg.Version
                                                            && sortedPkg.Id.Equals(assetsPkg.Name, StringComparison.OrdinalIgnoreCase)),
                                                        value: new Lazy<LocalPackageSourceInfo>(() =>
                                                            NuGetv3LocalRepositoryUtility.GetPackage(
                                                                repositories,
                                                                sortedPkg.Id,
                                                                sortedPkg.Version))))
                                                .Where(e => e.Key != null)
                                                .ToArray();

                // build/ {packageId}.targets
                var buildTargetsGroup = new MSBuildRestoreItemGroup();
                buildTargetsGroup.RootName = MSBuildRestoreItemGroup.ImportGroup;
                buildTargetsGroup.Position = 2;

                buildTargetsGroup.Items.AddRange(sortedPackages.SelectMany(pkg =>
                    pkg.Key.Build.WithExtension(TargetsExtension)
                        .Where(e => pkg.Value.Exists())
                        .Select(e => pkg.Value.GetAbsolutePath(e)))
                        .Select(path => GetPathWithMacros(path, repositoryRoot))
                        .Select(GenerateImport));

                targets.AddRange(GenerateGroupsWithConditions(buildTargetsGroup, isMultiTargeting, frameworkConditions));

                // props/ {packageId}.props
                var buildPropsGroup = new MSBuildRestoreItemGroup();
                buildPropsGroup.RootName = MSBuildRestoreItemGroup.ImportGroup;
                buildPropsGroup.Position = 2;

                buildPropsGroup.Items.AddRange(sortedPackages.SelectMany(pkg =>
                    pkg.Key.Build.WithExtension(PropsExtension)
                        .Where(e => pkg.Value.Exists())
                        .Select(e => pkg.Value.GetAbsolutePath(e)))
                        .Select(path => GetPathWithMacros(path, repositoryRoot))
                        .Select(GenerateImport));

                props.AddRange(GenerateGroupsWithConditions(buildPropsGroup, isMultiTargeting, frameworkConditions));

                // Create an empty PropertyGroup for package properties
                var packagePathsPropertyGroup = MSBuildRestoreItemGroup.Create("PropertyGroup", Enumerable.Empty<XElement>(), 1000, isMultiTargeting ? new string[] { frameworkConditions } : Enumerable.Empty<string>());

                var projectGraph = targetGraph.Graphs.FirstOrDefault();

                // Packages with GeneratePathProperty=true
                var packages = projectGraph?.Item.Data.Dependencies.Where(i => i.GeneratePathProperty).Select(i => i.Name);
                var packageIdsToCreatePropertiesFor = packages != null ? new HashSet<string>(packages, StringComparer.OrdinalIgnoreCase) : Enumerable.Empty<string>();

                var localPackages = sortedPackages.Select(e => e.Value);

                // Find the packages with matching IDs in the list of sorted packages, filtering out ones that there was no match for or that don't exist
                var packagePathProperties = localPackages
                    .Where(pkg => pkg?.Value?.Package != null && (packagesWithTools.Contains(pkg.Value.Package.Id) || packageIdsToCreatePropertiesFor.Contains(pkg.Value.Package.Id)) && pkg.Exists())
                    .Select(pkg => pkg.Value.Package)
                    // Get the property
                    .Select(GeneratePackagePathProperty);

                packagePathsPropertyGroup.Items.AddRange(packagePathProperties);

                // Don't bother adding the PropertyGroup if there were no properties added
                if (packagePathsPropertyGroup.Items.Any())
                {
                    props.Add(packagePathsPropertyGroup);
                }

                if (isMultiTargeting)
                {
                    // buildMultiTargeting/ {packageId}.targets
                    var buildCrossTargetsGroup = new MSBuildRestoreItemGroup();
                    buildCrossTargetsGroup.RootName = MSBuildRestoreItemGroup.ImportGroup;
                    buildCrossTargetsGroup.Position = 0;

                    buildCrossTargetsGroup.Items.AddRange(sortedPackages.SelectMany(pkg =>
                        pkg.Key.BuildMultiTargeting.WithExtension(TargetsExtension)
                            .Where(e => pkg.Value.Exists())
                            .Select(e => pkg.Value.GetAbsolutePath(e)))
                            .Where(path => multiTargetingImportsAdded.Add(path))
                            .Select(path => GetPathWithMacros(path, repositoryRoot))
                            .Select(GenerateImport));

                    targets.AddRange(GenerateGroupsWithConditions(buildCrossTargetsGroup, isMultiTargeting, CrossTargetingCondition));

                    // buildMultiTargeting/ {packageId}.props
                    var buildCrossPropsGroup = new MSBuildRestoreItemGroup();
                    buildCrossPropsGroup.RootName = MSBuildRestoreItemGroup.ImportGroup;
                    buildCrossPropsGroup.Position = 0;

                    buildCrossPropsGroup.Items.AddRange(sortedPackages.SelectMany(pkg =>
                        pkg.Key.BuildMultiTargeting.WithExtension(PropsExtension)
                            .Where(e => pkg.Value.Exists())
                            .Select(e => pkg.Value.GetAbsolutePath(e)))
                            .Where(path => multiTargetingImportsAdded.Add(path))
                            .Select(path => GetPathWithMacros(path, repositoryRoot))
                            .Select(GenerateImport));

                    props.AddRange(GenerateGroupsWithConditions(buildCrossPropsGroup, isMultiTargeting, CrossTargetingCondition));
                }

                // Write out contentFiles only for XPlat PackageReference projects.
                if (request.ProjectStyle != ProjectStyle.ProjectJson
                    && request.Project.RestoreMetadata?.SkipContentFileWrite != true)
                {
                    // Create a group for every package, with the nearest from each of allLanguages
                    props.AddRange(sortedPackages.Select(pkg =>
                         pkg.Key.ContentFiles
                                .Where(e => pkg.Value.Exists())
                                .OrderBy(e => e.Path, StringComparer.Ordinal)
                                .Select(e =>
                                    Tuple.Create(
                                        item1: pkg.Key,
                                        item2: e,
                                        item3: GetPathWithMacros(pkg.Value.GetAbsolutePath(e), repositoryRoot))))
                         .SelectMany(e => GetLanguageGroups(e))
                         .SelectMany(group => GenerateGroupsWithConditions(group, isMultiTargeting, frameworkConditions)));
                }
            }

            // Add exclude all condition to all groups
            foreach (var group in props.Concat(targets))
            {
                group.Conditions.Add(ExcludeAllCondition);
            }

            // Create XML, these may be null if the file should be deleted/not written out.
            var propsXML = GenerateMSBuildFile(props, request.ProjectStyle);
            var targetsXML = GenerateMSBuildFile(targets, request.ProjectStyle);

            // Return all files to write out or delete.
            var files = new List<MSBuildOutputFile>
            {
                new MSBuildOutputFile(propsPath, propsXML),
                new MSBuildOutputFile(targetsPath, targetsXML)
            };

            var packageFolders = repositories.Select(e => e.RepositoryRoot);

            AddNuGetPropertiesToFirstImport(files, packageFolders, repositoryRoot, request.ProjectStyle, assetsFilePath, restoreSuccess);

            return files;
        }

        private static IEnumerable<string> GetLanguageConditions(string language, SortedSet<string> allLanguages)
        {
            if (PackagingConstants.AnyCodeLanguage.Equals(language, StringComparison.OrdinalIgnoreCase))
            {
                // Must not be any of the other package languages.
                foreach (var lang in allLanguages)
                {
                    yield return string.Format(CultureInfo.InvariantCulture, NegativeLanguageCondition, GetLanguage(lang));
                }
            }
            else
            {
                // Must be the language.
                yield return string.Format(CultureInfo.InvariantCulture, LanguageCondition, GetLanguage(language));
            }
        }

        public static string GetLanguage(string nugetLanguage)
        {
            var lang = nugetLanguage.ToUpperInvariant();

            // Translate S -> #
            switch (lang)
            {
                case "CS":
                    return "C#";
                case "FS":
                    return "F#";
            }

            // Return the language as it is
            return lang;
        }

        private static IEnumerable<MSBuildRestoreItemGroup> GetLanguageGroups(
            IEnumerable<Tuple<LockFileTargetLibrary, LockFileContentFile, string>> items)
        {
            var currentItems = items.ToArray();

            if (currentItems.Length == 0)
            {
                // Noop fast if this does not have content files.
                return Enumerable.Empty<MSBuildRestoreItemGroup>();
            }

            var packageId = currentItems[0].Item1.Name;
            var packageVersion = currentItems[0].Item1.Version.ToNormalizedString();

            // Find all languages used for the any group condition
            var allLanguages = new SortedSet<string>(
                currentItems.Select(e => e.Item2.CodeLanguage)
                            .Where(s => !PackagingConstants.AnyCodeLanguage.Equals(s, StringComparison.OrdinalIgnoreCase)),
                StringComparer.OrdinalIgnoreCase);

            // Convert content file items from a package into an ItemGroup with conditions.
            // Remove _._ entries
            // Filter empty groups
            var groups = currentItems.GroupBy(e => e.Item2.CodeLanguage, StringComparer.OrdinalIgnoreCase)
                                .Select(group => MSBuildRestoreItemGroup.Create(
                                    rootName: MSBuildRestoreItemGroup.ItemGroup,
                                    position: 1,
                                    conditions: GetLanguageConditions(group.Key, allLanguages),
                                    items: group.Where(e => !e.Item2.Path.EndsWith(PackagingCoreConstants.ForwardSlashEmptyFolder, StringComparison.Ordinal)
                                                            && !e.Item2.Path.EndsWith(".pp", StringComparison.OrdinalIgnoreCase)) // Skip .pp files
                                                .Select(e => GenerateContentFilesItem(e.Item3, e.Item2, packageId, packageVersion))))
                                .Where(group => group.Items.Count > 0);

            return groups;
        }

        private static IEnumerable<MSBuildRestoreItemGroup> GenerateGroupsWithConditions(
            MSBuildRestoreItemGroup original,
            bool isCrossTargeting,
            params string[] conditions)
        {
            if (isCrossTargeting)
            {
                foreach (var condition in conditions)
                {
                    yield return new MSBuildRestoreItemGroup()
                    {
                        RootName = original.RootName,
                        Position = original.Position,
                        Items = original.Items,
                        Conditions = original.Conditions.Concat(new[] { condition }).ToList()
                    };
                }
            }
            else
            {
                // No changes needed
                yield return original;
            }
        }

        private static string GetAbsolutePath(this Lazy<LocalPackageSourceInfo> package, LockFileItem item)
        {
            return Path.Combine(package.Value.Package.ExpandedPath, LockFileUtils.ToDirectorySeparator(item.Path));
        }

        private static bool Exists(this Lazy<LocalPackageSourceInfo> package)
        {
            return (package?.Value != null);
        }

        private static IEnumerable<LockFileItem> WithExtension(this IList<LockFileItem> items, string extension)
        {
            if (items == null)
            {
                return Enumerable.Empty<LockFileItem>();
            }

            return items.Where(c => extension.Equals(Path.GetExtension(c.Path), StringComparison.OrdinalIgnoreCase));
        }

        private static string GetMatchingFrameworkStrings(PackageSpec spec, NuGetFramework framework)
        {
            var frameworkString = spec.TargetFrameworks.Where(e => e.FrameworkName.Equals(framework)).FirstOrDefault()?.TargetAlias;

            // If there were no matches, use the generated name
            if (string.IsNullOrEmpty(frameworkString))
            {
                return framework.GetShortFolderName();
            }

            return frameworkString;
        }

        private static HashSet<PackageDependencyInfo> ConvertToPackageDependencyInfo(
            ISet<GraphItem<RemoteResolveResult>> items)
        {
            var result = new HashSet<PackageDependencyInfo>(PackageIdentity.Comparer);

            foreach (var item in items)
            {
                var dependencies =
                    item.Data?.Dependencies?.Select(
                        dependency => new PackageDependency(dependency.Name, VersionRange.All));

                result.Add(new PackageDependencyInfo(item.Key.Name, item.Key.Version, dependencies));
            }

            return result;
        }

        private static XElement GeneratePackagePathProperty(LocalPackageInfo localPackageInfo)
        {
#if NETCOREAPP
            return GenerateProperty($"Pkg{localPackageInfo.Id.Replace(".", "_", StringComparison.Ordinal)}", localPackageInfo.ExpandedPath);
#else
            return GenerateProperty($"Pkg{localPackageInfo.Id.Replace(".", "_")}", localPackageInfo.ExpandedPath);
#endif
        }
    }
}
