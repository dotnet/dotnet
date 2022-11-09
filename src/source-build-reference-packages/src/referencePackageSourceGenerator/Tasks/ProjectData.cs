// Licensed to the .NET Foundation under one or more agreements.
// // The .NET Foundation licenses this file to you under the MIT license.
// // See the LICENSE file in the project root for more information.
//
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Repositories;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;

namespace Microsoft.DotNet.SourceBuild.Tasks
{
    // Represents data for a project to build an individual dll from a nuget package
    // PackageData references multiple ProjectData instances, one for each dll in the package
    public class ProjectData
    {
        private static Dictionary<string, ProjectData> projectDataCache = new Dictionary<string, ProjectData>();

        public static ProjectData[] GetAllProjectData()
        {
            return projectDataCache.Values.ToArray();
        }

        public static ProjectData GetOrCreateProjectData(string targetPackagesPath, string dllPath, string srcPath)
        {
            if (projectDataCache.ContainsKey(dllPath))
            {
                return projectDataCache[dllPath];
            }
            var pd = new ProjectData(targetPackagesPath, dllPath, srcPath);
            projectDataCache.Add(dllPath, pd);
            return pd;
        }

        public static void GenerateAllDependencies()
        {
            foreach (var projData in GetAllProjectData())
            {
                projData.InitializeDependencies();
            }
        }

        public static ProjectData GetProjectData(string dllPath)
        {
            return projectDataCache.ContainsKey(dllPath) ? projectDataCache[dllPath] : null;
        }

        private NuspecReader nReader;

        private string targetPackagesPath;

        private ProjectData(string targetPackagesPath, string dllPath, string srcPath)
        {
            this.DllPath = dllPath;
            this.targetPackagesPath = targetPackagesPath;

            // Find the corresponding nuspec
            var splitPath = dllPath.Replace(targetPackagesPath, "").Split(Path.DirectorySeparatorChar);
            if (splitPath.Length < 4)
            {
                throw new ArgumentException("Path does not have expected depth", nameof(dllPath));
            }
            var rootDir = Path.Combine(targetPackagesPath, Path.Combine(splitPath.Take(2).ToArray()));
            this.NuSpecFile = Directory.GetFiles(rootDir, "*.nuspec", SearchOption.TopDirectoryOnly)[0];
            nReader = new NuspecReader(NuSpecFile);

            // Find the package name
            this.PackageName = nReader.GetId();

            // Find the package version
            this.PackageVersion = nReader.GetVersion();

            // Find the tfm for the dll
            this.ShortTfm = splitPath[splitPath.Length - 2];

            // Find the project name for the dll
            this.ProjectFileName = dllPath.Replace(targetPackagesPath, srcPath).Replace(".dll", ".csproj");

            // Find the source file name for the dll
            this.SourceFileName = dllPath.Replace(targetPackagesPath, srcPath).Replace(".dll", ".cs");

            // Find the subPath for the dll (either ref or lib, usually)
            this.SubPath = splitPath[splitPath.Length - 3];

            var pkgData = PackageData.GetOrCreatePackageData(srcPath, this.PackageName, this.PackageVersion.ToString());
            pkgData.ReferenceProjects.Add(this);
        }
        public void InitializeDependencies()
        {
            // Get the dependencies
            this.Dependencies = GetPackageDependencies(targetPackagesPath, GetTargetFramework(this.ShortTfm));
        }

        public NuGetFramework GetTargetFramework(string tfmFolderName)
        {
            string numRegex = @"[^0-9]*(?<major>[0-9])[.]*(?<minor>[0-9])(?<patch>[0-9]?)";
            var result = Regex.Match(tfmFolderName, numRegex);
            if (!result.Success)
            {
                return NuGetFramework.AnyFramework;
            }

            int majorVer = result.Groups["major"].Value != "" ? int.Parse(result.Groups["major"].Value) : 0;
            int minorVer = result.Groups["minor"].Value != "" ? int.Parse(result.Groups["minor"].Value) : 0;
            int patchVer = result.Groups["patch"].Value != "" ? int.Parse(result.Groups["patch"].Value) : 0;

            string identifierLongName = DefaultFrameworkMappings.Instance.IdentifierShortNames.FirstOrDefault(t => tfmFolderName.StartsWith(t.Value)).Key;
            return new NuGetFramework(identifierLongName, new System.Version(majorVer, minorVer, patchVer));
        }

        public string GetTfmFolderName(NuGetFramework targetFramework)
        {
            string framework = DefaultFrameworkMappings.Instance.IdentifierShortNames.FirstOrDefault(t => t.Key == targetFramework.Framework).Value;
            string version = framework == "netstandard" ? $"{targetFramework.Version.Major}.{targetFramework.Version.Minor}" : $"{targetFramework.Version.Major}{targetFramework.Version.Minor}";
            return framework + version;
        }

        public string GetFileVersion()
        {
            return FileVersionInfo.GetVersionInfo(this.DllPath)?.FileVersion;
        }

        public string[] GetFrameworkReferences()
        {
            return this.nReader.GetFrameworkAssemblyGroups().Where(frg => frg.TargetFramework == GetTargetFramework(this.ShortTfm)).SelectMany(frg => frg.Items).ToArray();
        }

        public NuGet.Packaging.Core.PackageDependency[] GetPackageReferences()
        {
            var retval = this.nReader.GetDependencyGroups().Where(dg => dg.TargetFramework == GetTargetFramework(this.ShortTfm)).SelectMany(dg => dg.Packages);
            retval = retval.Where(p => !p.Id.StartsWith("runtime.native"));
            return retval.ToArray();
        }

        public List<string> GetAdditionalUsings()
        {
            List<string> result = new List<string>();
            string usingDecl = "using {0};";
            using (var fileStream = new FileStream(this.DllPath, FileMode.Open, FileAccess.Read, FileShare.Delete | FileShare.Read))
            {
                try
                {
                    using (PEReader pereader = new PEReader(fileStream, PEStreamOptions.LeaveOpen))
                    {
                        if (pereader.HasMetadata)
                        {
                            MetadataReader reader = pereader.GetMetadataReader();
                            if (reader.IsAssembly)
                            {
                                foreach (var exportedTypeHandle in reader.ExportedTypes)
                                {
                                    var exportedType = reader.GetExportedType(exportedTypeHandle);

                                    if (exportedType.IsForwarder)
                                    {
                                        result.Add(string.Format(usingDecl, reader.GetString(exportedType.Namespace)));
                                    }
                                }
                            }
                        }
                    }
                }
                catch (BadImageFormatException)
                {
                    throw new ArgumentException($"Assembly {this.DllPath} has incorrect format", nameof(this.DllPath));
                }
            }
            return result;
        }

        public string ConvertGeneric(string genericType)
        {
            if (!genericType.Contains("`")) return genericType;
            var splitType = genericType.Split('`');
            var numberOfTypes = int.Parse(splitType[1]);
            var typeName = splitType[0];
            var commas = string.Concat(Enumerable.Repeat(", ", numberOfTypes - 1));
            return $"{typeName}<{commas}>";
        }

        public string GetTypeForwards()
        {
            // Don't generate type forwards for netstandard
            if (this.ShortTfm.StartsWith("netstandard")) return "";
            string result = "";
            string typeForwardDecl = "[assembly: TypeForwardedTo(typeof({0}))]\n";
            using (var fileStream = new FileStream(this.DllPath, FileMode.Open, FileAccess.Read, FileShare.Delete | FileShare.Read))
            {
                try
                {
                    using (PEReader pereader = new PEReader(fileStream, PEStreamOptions.LeaveOpen))
                    {
                        if (pereader.HasMetadata)
                        {
                            MetadataReader reader = pereader.GetMetadataReader();
                            if (reader.IsAssembly)
                            {
                                foreach (var exportedTypeHandle in reader.ExportedTypes)
                                {
                                    var exportedType = reader.GetExportedType(exportedTypeHandle);
                                    if (exportedType.IsForwarder)
                                    {
                                        var typeName = reader.GetString(exportedType.Name);
                                        var ns = reader.GetString(exportedType.Namespace);
                                        typeName = typeName == "Void" ? "void" : ns + "." + ConvertGeneric(typeName);
                                        result += string.Format(typeForwardDecl, typeName);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (BadImageFormatException)
                {
                    throw new ArgumentException($"Assembly {this.DllPath} has incorrect format", nameof(this.DllPath));
                }
            }
            return result;
        }

        public string GetStrongNameKey()
        {
            var fullname = AssemblyName.GetAssemblyName(this.DllPath).FullName;
            var pattern = "PublicKeyToken=([\\w]*)";
            var regex = new System.Text.RegularExpressions.Regex(pattern);
            var match = regex.Match(fullname); var sn = "";
            if (match.Success)
            {
                sn = match.Groups[1].Value;
            }
            return sn;
        }

        public string GetAssemblyVersion()
        {
            Version result = null;
            using (var fileStream = new FileStream(this.DllPath, FileMode.Open, FileAccess.Read, FileShare.Delete | FileShare.Read))
            {
                try
                {
                    using (PEReader pereader = new PEReader(fileStream, PEStreamOptions.LeaveOpen))
                    {
                        if (pereader.HasMetadata)
                        {
                            MetadataReader reader = pereader.GetMetadataReader();
                            if (reader.IsAssembly)
                            {
                                result = reader.GetAssemblyDefinition().Version;
                            }
                        }
                    }
                }
                catch (BadImageFormatException)
                {
                    throw new ArgumentException($"Assembly {this.DllPath} has incorrect format", nameof(this.DllPath));
                }
            }
            return result.ToString();
        }

        private PackageData[] GetPackageDependencies(string targetPackagesPath, NuGetFramework targetFramework)
        {
            List<PackageData> dependencies = new List<PackageData>();
            PackageDependencyGroup group = this.nReader.GetDependencyGroups().GetNearest(targetFramework);
            if (group != null)
            {
                foreach (var pkg in group.Packages)
                {
                    var repository = new NuGetv3LocalRepository(targetPackagesPath);
                    var dependencyPkgs = repository.FindPackagesById(pkg.Id);
                    var allVersions = dependencyPkgs.Select(d => d.Version);
                    NuGetVersion selectedVersion = pkg.VersionRange.FindBestMatch(allVersions);
                    var selectedPkg = dependencyPkgs.FirstOrDefault(d => d.Version == selectedVersion);
                    if (selectedPkg != null)
                    {
                        var pkgData = PackageData.GetPackageData(selectedPkg.Id, selectedPkg.Version.ToString());
                        if (pkgData != null)
                        {
                            dependencies.Add(pkgData);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine($"ERROR: Group not found for {this.SourceFileName} TFM {targetFramework}.");
            }
            return dependencies.ToArray();
        }

        public string DllPath { get; internal set; }

        public string NuSpecFile { get; internal set; }

        public string PackageName { get; internal set; }

        public NuGetVersion PackageVersion { get; internal set; }

        public string ShortTfm { get; internal set; }

        public string ProjectFileName { get; internal set; }

        public string SourceFileName { get; internal set; }

        public string SubPath { get; internal set; }

        public PackageData[] Dependencies { get; internal set; }
    }
}
