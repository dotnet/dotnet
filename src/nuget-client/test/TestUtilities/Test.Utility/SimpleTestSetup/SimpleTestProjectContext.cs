// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Internal.NuGet.Testing.SignedPackages;
using Newtonsoft.Json.Linq;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.ProjectModel;
using NuGet.Versioning;

namespace NuGet.Test.Utility
{
    public class SimpleTestProjectContext
    {
        private static string ProjectExt = ".csproj";

        public SimpleTestProjectContext(string projectName, ProjectStyle type, string solutionRoot)
        {
            if (string.IsNullOrWhiteSpace(projectName))
            {
                throw new ArgumentException(nameof(projectName));
            }

            if (string.IsNullOrWhiteSpace(solutionRoot))
            {
                throw new ArgumentException(nameof(solutionRoot));
            }

            ProjectName = projectName;
            ProjectPath = Path.Combine(solutionRoot, projectName, $"{projectName}{ProjectExt}");
            ProjectExtensionsPath = Path.Combine(solutionRoot, projectName, "obj");
            Type = type;
        }

        public string Version { get; set; } = "1.0.0";

        public Guid ProjectGuid { get; set; } = Guid.NewGuid();

        /// <summary>
        /// True for non-xplat.
        /// </summary>
        public bool IsLegacyPackageReference { get; set; }

        /// <summary>
        /// MSBuild project name
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Project file full path.
        /// </summary>
        public string ProjectPath { get; set; }

        /// <summary>
        /// MSBuildProjectExtensionsPath
        /// </summary>
        public string ProjectExtensionsPath { get; set; }

        /// <summary>
        /// Additional MSBuild properties
        /// </summary>
        public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Target frameworks containing dependencies.
        /// </summary>
        public List<SimpleTestProjectFrameworkContext> Frameworks { get; set; } = new List<SimpleTestProjectFrameworkContext>();

        /// <summary>
        /// Project type
        /// </summary>
        public ProjectStyle Type { get; set; }

        /// <summary>
        /// Tool references
        /// </summary>
        public List<SimpleTestPackageContext> DotnetCLIToolReferences { get; set; } = new List<SimpleTestPackageContext>();

        /// <summary>
        /// Project.json file
        /// </summary>
        public JObject ProjectJson { get; set; }

        /// <summary>
        /// Include attributes for the parent project referencing this one.
        /// </summary>
        public string IncludeAssets { get; set; } = string.Empty;

        /// <summary>
        /// Include attributes for the parent project referencing this one.
        /// </summary>
        public string ExcludeAssets { get; set; } = string.Empty;

        /// <summary>
        /// Include attributes for the parent project referencing this one.
        /// </summary>
        public string PrivateAssets { get; set; } = string.Empty;

        public bool ToolingVersion15 { get; set; } = false;

        public IEnumerable<PackageSource> Sources { get; set; }

        public IList<string> FallbackFolders { get; set; }

        public string GlobalPackagesFolder { get; set; }

        public bool WarningsAsErrors { get; set; }

        /// <summary>
        /// If true TargetFramework will be used instead of TargetFrameworks
        /// </summary>
        public bool SingleTargetFramework { get; set; }

        public bool SetMSBuildProjectExtensionsPath { get; set; } = true;

        /// <summary>
        /// project.lock.json or project.assets.json
        /// </summary>
        public string AssetsFileOutputPath
        {
            get
            {
                switch (Type)
                {
                    case ProjectStyle.PackageReference:
                        return Path.Combine(ProjectExtensionsPath, "project.assets.json");

                    case ProjectStyle.ProjectJson:
                        return Path.Combine(Path.GetDirectoryName(ProjectPath), "project.lock.json");

                    default:
                        return null;
                }
            }
        }

        public string CacheFileOutputPath
        {
            get
            {
                switch (Type)
                {
                    case ProjectStyle.PackageReference:
                        return Path.Combine(ProjectExtensionsPath, NoOpRestoreUtilities.NoOpCacheFileName);

                    default:
                        return null;
                }
            }
        }

        public string NuGetLockFileOutputPath
        {
            get
            {
                switch (Type)
                {
                    case ProjectStyle.PackageReference:
                        if (Properties.ContainsKey("NuGetLockFilePath"))
                        {
                            return Properties["NuGetLockFilePath"];
                        }
                        return Path.Combine(Path.GetDirectoryName(ProjectPath), "packages.lock.json");
                    default:
                        return null;
                }
            }
        }

        public string TargetsOutput
        {
            get
            {
                switch (Type)
                {
                    case ProjectStyle.PackageReference:
                        return Path.Combine(ProjectExtensionsPath, $"{Path.GetFileName(ProjectPath)}.nuget.g.targets");

                    case ProjectStyle.ProjectJson:
                        return Path.Combine(Path.GetDirectoryName(ProjectPath), $"{Path.GetFileNameWithoutExtension(ProjectPath)}.nuget.targets");

                    default:
                        return ProjectPath;
                }
            }
        }

        public string PropsOutput
        {
            get
            {
                switch (Type)
                {
                    case ProjectStyle.PackageReference:
                        return Path.Combine(ProjectExtensionsPath, $"{Path.GetFileName(ProjectPath)}.nuget.g.props");

                    case ProjectStyle.ProjectJson:
                        return Path.Combine(Path.GetDirectoryName(ProjectPath), $"{Path.GetFileNameWithoutExtension(ProjectPath)}.nuget.props");

                    default:
                        return ProjectPath;
                }
            }
        }

        private LockFile _assetsFileCache = null;
        private DateTime _assetsFileLastModified = DateTime.MinValue;

        public LockFile AssetsFile
        {
            get
            {
                var path = AssetsFileOutputPath;
                if (File.Exists(path))
                {
                    var lastWriteTime = File.GetLastWriteTimeUtc(path);
                    if (_assetsFileLastModified < lastWriteTime)
                    {
                        var format = new LockFileFormat();
                        _assetsFileCache = format.Read(path);
                        _assetsFileLastModified = lastWriteTime;
                    }

                    return _assetsFileCache;
                }

                return null;
            }
        }

        public PackageSpec PackageSpec
        {
            get
            {
                var _packageSpec = new PackageSpec(Frameworks
                    .Select(f => new TargetFrameworkInformation()
                    {
                        FrameworkName = f.Framework,
                        Dependencies = f.PackageReferences.Select(e => new LibraryDependency() { LibraryRange = new LibraryRange(e.Id, VersionRange.Parse(e.Version), LibraryDependencyTarget.Package) }).ToImmutableArray(),
                        TargetAlias = f.TargetAlias,
                    }).ToList());
                _packageSpec.RestoreMetadata = new ProjectRestoreMetadata();
                _packageSpec.Name = ProjectName;
                _packageSpec.FilePath = ProjectPath;
                _packageSpec.RestoreMetadata.ProjectUniqueName = ProjectName;
                _packageSpec.RestoreMetadata.ProjectName = ProjectName;
                _packageSpec.RestoreMetadata.ProjectPath = ProjectPath;
                _packageSpec.RestoreMetadata.ProjectStyle = Type;
                _packageSpec.RestoreMetadata.OutputPath = ProjectExtensionsPath;
                _packageSpec.RestoreMetadata.OriginalTargetFrameworks = _packageSpec.TargetFrameworks.Select(e => e.TargetAlias).ToList();
                _packageSpec.RestoreMetadata.TargetFrameworks = Frameworks
                    .Select(f => new ProjectRestoreMetadataFrameworkInfo(f.Framework))
                    .ToList();
                _packageSpec.RestoreMetadata.Sources = Sources?.ToList();
                _packageSpec.RestoreMetadata.PackagesPath = GlobalPackagesFolder;
                _packageSpec.RestoreMetadata.FallbackFolders = FallbackFolders;
                if (Type == ProjectStyle.ProjectJson)
                {
                    _packageSpec.RestoreMetadata.ProjectJsonPath = Path.Combine(Path.GetDirectoryName(ProjectPath), "project.json");
                }
                if (Frameworks.Count() > 1)
                {
                    _packageSpec.RestoreMetadata.CrossTargeting = true;
                }

                return _packageSpec;
            }
        }

        public void AddPackageToAllFrameworks(params SimpleTestPackageContext[] packages)
        {
            foreach (var framework in Frameworks)
            {
                framework.PackageReferences.AddRange(packages);
            }
        }

        public void CleanPackagesFromAllFrameworks()
        {
            foreach (var framework in Frameworks)
            {
                framework.PackageReferences.Clear();
            }
        }

        public void AddPackageToFramework(string packageFramework, params SimpleTestPackageContext[] packages)
        {
            var framework = Frameworks
                .First(f => f.Framework == NuGetFramework.Parse(packageFramework));
            framework.PackageReferences.AddRange(packages);
        }

        public void AddPackageDownloadToAllFrameworks(params SimpleTestPackageContext[] packages)
        {
            foreach (var framework in Frameworks)
            {
                framework.PackageDownloads.AddRange(packages);
            }
        }

        public void AddPackageDownloadToFramework(string packageFramework, params SimpleTestPackageContext[] packages)
        {
            var framework = Frameworks
                .First(f => f.Framework == NuGetFramework.Parse(packageFramework));
            framework.PackageDownloads.AddRange(packages);
        }

        public void AddProjectToAllFrameworks(params SimpleTestProjectContext[] projects)
        {
            foreach (var framework in Frameworks)
            {
                framework.ProjectReferences.AddRange(projects);
            }
        }

        /// <summary>
        /// Package references from all TFMs
        /// </summary>
        public List<SimpleTestPackageContext> AllPackageDependencies
        {
            get
            {
                return Frameworks.SelectMany(f => f.PackageReferences).Distinct().ToList();
            }
        }

        /// <summary>
        /// Project references from all TFMs
        /// </summary>
        public List<SimpleTestProjectContext> AllProjectReferences => Frameworks.SelectMany(f => f.ProjectReferences).Distinct().ToList();

        public void Save()
        {
            Save(ProjectPath);
        }

        public void Save(string path)
        {
            var projectFile = new FileInfo(path);
            projectFile.Directory.Create();

            var xml = GetXML();

            File.WriteAllText(path, xml.ToString());

            if (ProjectJson != null)
            {
                var jsonPath = ProjectJsonPathUtilities.GetProjectConfigPath(
                    projectFile.Directory.FullName,
                    Path.GetFileNameWithoutExtension(ProjectPath));

                File.WriteAllText(jsonPath, ProjectJson.ToString());
            }
        }

        /// <summary>
        /// Create a UAP package reference project. Framework is only used internally.
        /// </summary>
        public static SimpleTestProjectContext CreateLegacyPackageReference(
            string projectName,
            string solutionRoot,
            NuGetFramework framework)
        {
            var context = new SimpleTestProjectContext(projectName, ProjectStyle.PackageReference, solutionRoot);
            context.Frameworks.Add(new SimpleTestProjectFrameworkContext(framework));
            context.IsLegacyPackageReference = true;
            return context;
        }

        public static SimpleTestProjectContext CreateNETCore(
            string projectName,
            string solutionRoot,
            params NuGetFramework[] frameworks)
        {
            var context = new SimpleTestProjectContext(projectName, ProjectStyle.PackageReference, solutionRoot);
            context.Frameworks.AddRange(frameworks.Select(e => new SimpleTestProjectFrameworkContext(e)));
            context.Properties.Add("RestoreProjectStyle", "PackageReference");
            return context;
        }

        public static SimpleTestProjectContext CreateNETCore(
            string projectName,
            string solutionRoot,
            params string[] frameworks)
        {
            var context = new SimpleTestProjectContext(projectName, ProjectStyle.PackageReference, solutionRoot);
            context.Frameworks.AddRange(frameworks.Select(e =>
            {
                var frameworkContext = new SimpleTestProjectFrameworkContext(NuGetFramework.Parse(e));
                frameworkContext.TargetAlias = e;
                return frameworkContext;
            }));
            context.Properties.Add("RestoreProjectStyle", "PackageReference");
            return context;
        }

        public static SimpleTestProjectContext CreateNETCoreWithSDK(
            string projectName,
            string solutionRoot,
            params string[] frameworks)
        {
            var context = new SimpleTestProjectContext(projectName, ProjectStyle.PackageReference, solutionRoot);
            context.Frameworks.AddRange(frameworks.Select(f => new SimpleTestProjectFrameworkContext(NuGetFramework.Parse(f)) { TargetAlias = f }));
            context.ToolingVersion15 = true;
            context.Properties.Add("RestoreProjectStyle", "PackageReference");
            context.Properties.Add("BuildWithNetFrameworkHostedCompiler", bool.FalseString);
            return context;
        }

        public static SimpleTestProjectContext CreateNonNuGet(
            string projectName,
            string solutionRoot,
            NuGetFramework framework)
        {
            var context = new SimpleTestProjectContext(projectName, ProjectStyle.Unknown, solutionRoot);
            context.Frameworks.Add(new SimpleTestProjectFrameworkContext(framework));
            return context;
        }

        public static SimpleTestProjectContext CreateUAP(
            string projectName,
            string solutionRoot,
            NuGetFramework framework,
            JObject projectJson)
        {
            var context = new SimpleTestProjectContext(projectName, ProjectStyle.ProjectJson, solutionRoot);
            context.Frameworks.Add(new SimpleTestProjectFrameworkContext(framework));
            context.ProjectJson = projectJson;
            return context;
        }

        public XDocument GetXML()
        {
            var sampleCSProjPath = (Type == ProjectStyle.PackageReference && ToolingVersion15) ?
                "Microsoft.Internal.NuGet.Testing.SignedPackages.compiler.resources.project2.csproj" :
                "Microsoft.Internal.NuGet.Testing.SignedPackages.compiler.resources.project1.csproj";

            var s = ResourceTestUtility.GetResource(sampleCSProjPath, typeof(ResourceTestUtility));
            var xml = XDocument.Parse(s);

            //  MSBuildProjectExtensionsPath needs to be set before Microsoft.Common.props is imported, so add a new
            //  PropertyGroup as the first element under the Project
            var ns = xml.Root.GetDefaultNamespace();
            if (SetMSBuildProjectExtensionsPath)
            {
                var propertyGroup = new XElement(ns + "PropertyGroup");
                propertyGroup.Add(new XElement(ns + "MSBuildProjectExtensionsPath", ProjectExtensionsPath));
                xml.Root.AddFirst(propertyGroup);
            }

            ProjectFileUtils.AddProperties(xml, new Dictionary<string, string>()
            {
                { "ProjectGuid", "{" + ProjectGuid.ToString() + "}" },
                { "AssemblyName", ProjectName }
            });

            ProjectFileUtils.AddProperties(xml, Properties);

            if (Type == ProjectStyle.PackageReference)
            {
                if (WarningsAsErrors)
                {
                    ProjectFileUtils.AddProperties(xml, new Dictionary<string, string>()
                    {
                        { "WarningsAsErrors", "true" }
                    });
                }

                ProjectFileUtils.AddProperties(xml, new Dictionary<string, string>()
                {
                    { "Version", Version },
                    { "DebugType", "portable" }
                });

                if (!IsLegacyPackageReference)
                {
                    var tfPropName = SingleTargetFramework ? "TargetFramework" : "TargetFrameworks";

                    ProjectFileUtils.AddProperties(xml, new Dictionary<string, string>()
                    {
                        { tfPropName, string.Join(";", Frameworks.Select(f => f.TargetAlias)) },
                    });
                }

                var addedToAllProjectReferences = new HashSet<SimpleTestProjectContext>();
                var addedToAllPackageReferences = new HashSet<SimpleTestPackageContext>();
                var addedToAllPackageDownloads = new HashSet<SimpleTestPackageContext>();

                foreach (var frameworkInfo in Frameworks)
                {
                    // Add TFM properties
                    var tfmProps = new Dictionary<string, string>(frameworkInfo.Properties);

                    if (!(Type == ProjectStyle.PackageReference && ToolingVersion15))
                    {
                        tfmProps.Add("TargetFrameworkIdentifier", frameworkInfo.Framework.Framework);
                        tfmProps.Add("TargetFrameworkVersion", $"v{NormalizeVersionString(frameworkInfo.Framework.Version)}");
                        tfmProps.Add("_TargetFrameworkVersionWithoutV", NormalizeVersionString(frameworkInfo.Framework.Version));
                        tfmProps.Add("TargetFrameworkMoniker", $"{frameworkInfo.Framework.Framework}, Version={NormalizeVersionString(frameworkInfo.Framework.Version)}");

                        if (frameworkInfo.Framework.HasPlatform)
                        {
                            tfmProps.Add("TargetPlatformIdentifier", frameworkInfo.Framework.Platform);
                            tfmProps.Add("TargetPlatformVersion", NormalizeVersionString(frameworkInfo.Framework.PlatformVersion));
                            tfmProps.Add("TargetPlatformMoniker", $"{frameworkInfo.Framework.Platform}, Version={NormalizeVersionString(frameworkInfo.Framework.PlatformVersion)}");
                        }
                    }

                    // Add properties with a TFM condition
                    ProjectFileUtils.AddProperties(xml, tfmProps, $" '$(TargetFramework)' == '{frameworkInfo.TargetAlias}' ");

                    foreach (var package in frameworkInfo.PackageReferences)
                    {
                        var referenceFramework = frameworkInfo.Framework;

                        // Drop the conditional if it is not needed
                        if (Frameworks.All(f => f.PackageReferences.Contains(package)))
                        {
                            referenceFramework = NuGetFramework.AnyFramework;

                            if (!addedToAllPackageReferences.Add(package))
                            {
                                // Skip since this was already added
                                continue;
                            }
                        }

                        var props = new Dictionary<string, string>();
                        var attributes = new Dictionary<string, string>();

                        // To support CPVM scenarios the Version can be null
                        // In these cases do not write any version
                        if (package.Version != null)
                        {
                            if (ToolingVersion15)
                            {
                                attributes.Add("Version", package.Version.ToString());
                            }
                            else
                            {
                                props.Add("Version", package.Version.ToString());
                            }
                        }
                        if (!string.IsNullOrEmpty(package.VersionOverride))
                        {
                            props.Add("VersionOverride", package.VersionOverride);
                        }

                        if (!string.IsNullOrEmpty(package.Include))
                        {
                            props.Add("IncludeAssets", package.Include);
                        }

                        if (!string.IsNullOrEmpty(package.Exclude))
                        {
                            props.Add("ExcludeAssets", package.Exclude);
                        }

                        if (!string.IsNullOrEmpty(package.PrivateAssets))
                        {
                            props.Add("PrivateAssets", package.PrivateAssets);
                        }

                        if (!string.IsNullOrEmpty(package.Aliases))
                        {
                            props.Add("Aliases", package.Aliases);
                        }

                        if (!string.IsNullOrEmpty(package.NoWarn))
                        {
                            props.Add("NoWarn", package.NoWarn);
                        }

                        ProjectFileUtils.AddItem(
                            xml,
                            "PackageReference",
                            package.Id,
                            referenceFramework,
                            props,
                            attributes);
                    }

                    foreach (var package in frameworkInfo.PackageDownloads)
                    {
                        var referenceFramework = frameworkInfo.Framework;

                        // Drop the conditional if it is not needed
                        if (Frameworks.All(f => f.PackageDownloads.Contains(package)))
                        {
                            referenceFramework = NuGetFramework.AnyFramework;

                            if (!addedToAllPackageDownloads.Add(package))
                            {
                                // Skip since this was already added
                                continue;
                            }
                        }

                        var props = new Dictionary<string, string>();
                        var attributes = new Dictionary<string, string>();

                        props.Add("Version", $"[{package.Version.ToString()}]");

                        ProjectFileUtils.AddItem(
                            xml,
                            "PackageDownload",
                            package.Id,
                            referenceFramework,
                            props,
                            attributes);
                    }


                    foreach (var project in frameworkInfo.ProjectReferences)
                    {
                        var referenceFramework = frameworkInfo.Framework;

                        // Drop the conditional if it is not needed
                        if (Frameworks.All(f => f.ProjectReferences.Contains(project)))
                        {
                            referenceFramework = NuGetFramework.AnyFramework;

                            if (!addedToAllProjectReferences.Add(project))
                            {
                                // Skip since this was already added
                                continue;
                            }
                        }

                        var props = new Dictionary<string, string>
                        {
                            { "Name", project.ProjectName },
                            { "Project", project.ProjectGuid.ToString() }
                        };

                        if (!string.IsNullOrEmpty(project.ExcludeAssets))
                        {
                            props.Add("IncludeAssets", project.ExcludeAssets);
                        }

                        if (!string.IsNullOrEmpty(project.ExcludeAssets))
                        {
                            props.Add("ExcludeAssets", project.ExcludeAssets);
                        }

                        if (!string.IsNullOrEmpty(project.PrivateAssets))
                        {
                            props.Add("PrivateAssets", project.PrivateAssets);
                        }

                        ProjectFileUtils.AddItem(
                            xml,
                            "ProjectReference",
                            $"{project.ProjectPath}",
                            referenceFramework,
                            props,
                            new Dictionary<string, string>());
                    }
                }

                // Add tool references
                foreach (var tool in DotnetCLIToolReferences)
                {
                    var props = new Dictionary<string, string>();
                    var attributes = new Dictionary<string, string>();

                    if (ToolingVersion15)
                    {
                        attributes.Add("Version", tool.Version.ToString());
                    }
                    else
                    {
                        props.Add("Version", tool.Version.ToString());
                    }

                    ProjectFileUtils.AddItem(
                        xml,
                        "DotNetCliToolReference",
                        $"{tool.Id}",
                        NuGetFramework.AnyFramework,
                        props,
                        attributes);
                }
            }
            else
            {
                // Add all project references directly
                foreach (var project in Frameworks.SelectMany(f => f.ProjectReferences).Distinct())
                {
                    var props = new Dictionary<string, string>
                    {
                        { "Name", project.ProjectName },
                        { "Project", project.ProjectGuid.ToString() }
                    };

                    if (!string.IsNullOrEmpty(project.ExcludeAssets))
                    {
                        props.Add("IncludeAssets", project.ExcludeAssets);
                    }

                    if (!string.IsNullOrEmpty(project.ExcludeAssets))
                    {
                        props.Add("ExcludeAssets", project.ExcludeAssets);
                    }

                    if (!string.IsNullOrEmpty(project.PrivateAssets))
                    {
                        props.Add("PrivateAssets", project.PrivateAssets);
                    }

                    ProjectFileUtils.AddItem(
                        xml,
                        "ProjectReference",
                        $"{project.ProjectPath}",
                        NuGetFramework.AnyFramework,
                        props,
                        new Dictionary<string, string>());
                }
            }

            return xml;
        }

        private static string NormalizeVersionString(Version version)
        {
            if (version.Build != 0)
            {
                return version.ToString(4);
            }
            if (version.Revision != 0)
            {
                return version.ToString(3);
            }
            return version.ToString(2);
        }

        public override bool Equals(object obj)
        {
            return StringComparer.Ordinal.Equals(ProjectPath, (obj as SimpleTestProjectContext)?.ProjectPath);
        }

        public override int GetHashCode()
        {
            return StringComparer.Ordinal.GetHashCode(ProjectPath);
        }

        public override string ToString()
        {
            return ProjectName;
        }
    }
}
