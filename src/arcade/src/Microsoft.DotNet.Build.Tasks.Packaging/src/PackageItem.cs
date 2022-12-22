// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using NuGet.Frameworks;
using System;
using System.IO;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace Microsoft.DotNet.Build.Tasks.Packaging
{
    public class PackageItem
    {
        public PackageItem(ITaskItem item)
        {
            OriginalItem = item;
            SourcePath = item.GetMetadata("FullPath");
            SourceProject = GetMetadata("MSBuildSourceProjectFile");
            string value = GetMetadata("TargetFramework");
            if (!String.IsNullOrWhiteSpace(value))
            {
                TargetFramework = NuGetFramework.Parse(value);
            }
            TargetPath = item.GetMetadata(nameof(TargetPath));
            AdditionalProperties = GetMetadata(nameof(AdditionalProperties));
            UndefineProperties = GetMetadata(nameof(UndefineProperties));
            HarvestedFrom = GetMetadata(nameof(HarvestedFrom));
            Package = GetMetadata("PackageId");
            PackageVersion = GetMetadata("PackageVersion");
            IsDll = Path.GetExtension(SourcePath).Equals(".dll", StringComparison.OrdinalIgnoreCase);
            IsPlaceholder = NuGetAssetResolver.IsPlaceholder(SourcePath);
            IsRef = TargetPath.StartsWith("ref/", StringComparison.OrdinalIgnoreCase);

            // determine if we need to append filename to TargetPath
            // see https://docs.nuget.org/create/nuspec-reference#specifying-files-to-include-in-the-package
            // SourcePath specifies file and target specifies file - do nothing
            // SourcePath specifies file and Target specifies directory - copy filename
            // SourcePath specifies wildcard files - copy wildcard
            // SourcePath specifies recursive wildcard - do not allow, recursive directory may impact asset selection
            //   we don't want to attempt to expand the wildcard since the build may not yet be complete.

            if (SourcePath.Contains("**"))
            {
                throw new ArgumentException($"Recursive wildcards \"**\" are not permitted in source paths for packages: {SourcePath}.  Recursive directory may impact asset selection and we don't want to attempt to expand the wildcard since the build may not yet be complete.");
            }

            string sourceFile = Path.GetFileName(SourcePath);
            if (!Path.GetExtension(TargetPath).Equals(Path.GetExtension(sourceFile), StringComparison.OrdinalIgnoreCase) ||
                sourceFile.Contains("*"))
            {
                TargetPath = Path.Combine(TargetPath, sourceFile);
            }

            // standardize to /
            TargetPath = TargetPath.Replace('\\', '/');

            int dirLength = TargetPath.LastIndexOf('/');
            TargetDirectory = (dirLength > 0) ? TargetPath.Substring(0, dirLength) : String.Empty;
        }

        private Version _version;
        public Version Version
        {
            get
            {
                if (_version == null)
                {
                    string versionString = OriginalItem.GetMetadata("AssemblyVersion");

                    if (!String.IsNullOrWhiteSpace(versionString))
                    {
                        Version.TryParse(versionString, out _version);
                    }

                    if (_version == null && IsDll && File.Exists(SourcePath))
                    {
                        _version = VersionUtility.GetAssemblyVersion(SourcePath);
                    }
                }

                return _version;
            }
        }

        public bool IsDll { get; }
        public bool IsPlaceholder { get; }
        public bool IsRef { get; }
        public ITaskItem OriginalItem { get; }
        public string SourcePath { get; }
        public string SourceProject { get; }
        public string AdditionalProperties { get; }
        public string UndefineProperties { get; }
        public string HarvestedFrom { get; }
        public NuGetFramework TargetFramework { get; }
        public string TargetDirectory { get; }
        public string TargetPath { get; }
        public string Package { get; }
        public string PackageVersion { get; }

        private string GetMetadata(string name)
        {
            var value = OriginalItem.GetMetadata(name);
            return (value?.Length > 0) ? value : null;
        }
    }
}
