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
    // Represents the data from a single nuget package
    public class PackageData
    {
        private static List<PackageData> pkgDataCache = new List<PackageData>();

        public string Key { get; set; }

        public string Id { get; set; }

        public string Version { get; set; }

        public string IdVersion => this.Id + ":" + this.Version;

        public string RelativePath { get; set; }

        public string ProjectPath { get; set; }

        public string RelativeProjectPath { get; set; }

        public string V2ProjectPath { get; set; }

        public string V2RelativeProjectPath { get; set; }

        public string DirectoryBuildPropsPath { get; set; }

        public List<ProjectData> ReferenceProjects { get; set; }

        public override string ToString()
        {
            return this.ProjectPath;
        }

        public string[] RelatedPackageDataIdVersions
        {
            get
            {
                return this.ReferenceProjects
                    .SelectMany(rp => rp.Dependencies)
                    .Distinct()
                    .Select(p => p.IdVersion)
                    .ToArray();
            }
        }

        public static PackageData[] GetAll()
        {
            return pkgDataCache.ToArray();
        }

        public static string GetKey(string id, string version)
        {
            return Path.Combine(id.ToLower(), version);
        }

        public static PackageData GetPackageData(string id, string version)
        {
            return pkgDataCache.FirstOrDefault(p => p.Key == GetKey(id, version));
        }

        public static PackageData GetOrCreatePackageData(string srcPath, string id, string version)
        {
            var pkgData = pkgDataCache.FirstOrDefault(p => p.Key == GetKey(id, version));
            return pkgData ?? new PackageData(id, version, srcPath);
        }

        private PackageData(string id, string version, string srcPath)
        {
            this.Id = id;
            this.Version = version;
            this.Key = GetKey(id, version);
            this.RelativePath = Path.Combine(id.ToLower(), version);
            this.ProjectPath = Path.Combine(srcPath, this.RelativePath, id + ".csproj");
            this.RelativeProjectPath = Path.Combine(this.RelativePath, id + ".csproj");
            // V2 project file names contain version as well
            string filename = string.Join(".", new string[] {id, version, "csproj"});
            this.V2ProjectPath = Path.Combine(srcPath, this.RelativePath, filename);
            this.V2RelativeProjectPath = Path.Combine(this.RelativePath, filename);
            this.DirectoryBuildPropsPath = Path.Combine(srcPath, this.RelativePath, "..", "Directory.Build.props");
            this.ReferenceProjects = new List<ProjectData>();
            pkgDataCache.Add(this);
        }
    }
}
