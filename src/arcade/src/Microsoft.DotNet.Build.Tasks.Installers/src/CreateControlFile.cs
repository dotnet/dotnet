// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;

namespace Microsoft.DotNet.Build.Tasks.Installers
{
    public sealed class CreateControlFile : BuildTask
    {
        [Required]
        public string PackageName { get; set; }

        [Required]
        public string PackageVersion { get; set; }

        [Required]
        public string PackageArchitecture { get; set; }

        [Required]
        public string Maintainer { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string InstalledSize { get; set; }

        [Required]
        public ITaskItem[] Depends { get; set; }

        [Required]
        public string Section { get; set; }

        public ITaskItem[] AdditionalProperties { get; set; }

        [Required]
        public string ControlFileOutputPath { get; set; }

        public override bool Execute()
        {
            using Stream stream = File.Create(ControlFileOutputPath);
            using StreamWriter writer = new(stream, Encoding.ASCII);
            writer.WriteLine($"Package: {PackageName}");
            writer.WriteLine($"Version: {PackageVersion}");
            writer.WriteLine($"Architecture: {PackageArchitecture}");
            writer.WriteLine($"Maintainer: {Maintainer}");
            writer.WriteLine($"Installed-Size: {InstalledSize}");
            List<string> dependencyItems = [];
            foreach (ITaskItem depend in Depends ?? [])
            {
                string dependencyItem = depend.ItemSpec;

                string version = depend.GetMetadata("Version");
                if (!string.IsNullOrEmpty(version))
                {
                    dependencyItem += $" (>= {version})";
                }

                dependencyItems.Add(dependencyItem);
            }
            if (Depends.Length > 0)
            {
                writer.WriteLine("Depends: " + string.Join(", ", dependencyItems));
            }
            writer.WriteLine($"Section: {Section}");
            writer.WriteLine("Priority: standard");
            writer.WriteLine("Homepage: https://github.com/dotnet/core");
            foreach (ITaskItem property in AdditionalProperties ?? [])
            {
                writer.WriteLine($"{property.ItemSpec}: {property.GetMetadata("Value")}");
            }

            // As per the control spec, multiline descriptions must have leading spaces
            // for each line after the first.
            // Two spaces represents a verbatim line break (as compared to one for package authoring that will not be preserved).
            writer.WriteLine($"Description: {Description.Replace("\n", "\n  ")}");
            return true;
        }
    }
}
