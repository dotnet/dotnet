// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.SourceBuild.Tasks
{
    public class GenerateProject : Task
    {
        /// <summary>
        /// The package id.
        /// </summary>
        [Required]
        public string? PackageId { get; set; }

        /// <summary>
        /// The path to the project template that is being transformed.
        /// </summary>
        [Required]
        public string? ProjectTemplate { get; set; }

        /// <summary>
        /// The target path that the project file is written to.
        /// </summary>
        [Required]
        public string? TargetPath { get; set; }

       /// <summary>
        /// The package's compile items, including target framework metadata.
        /// </summary>
        public ITaskItem[] CompileItems { get; set; } = Array.Empty<ITaskItem>();

        /// <summary>
        /// The package's dependencies with the PackageId as the identity and the version and target framework as metadata.
        /// </summary>
        public ITaskItem[] PackageDependencies { get; set; } = Array.Empty<ITaskItem>();

        /// <summary>
        /// The package's framework references with the framework reference assembly as the identity and the target framework as metadata.
        /// </summary>
        public ITaskItem[] FrameworkReferences { get; set; } = Array.Empty<ITaskItem>();

        public override bool Execute()
        {
            string referenceIncludes = "";
            StrongNameData strongNameData = default;
            string projectContent = File.ReadAllText(ProjectTemplate!);

            // Calculate the target frameworks based on the passed-in items.
            string[] targetFrameworks = CompileItems.Select(compileItem => compileItem.GetMetadata(SharedMetadata.TargetFrameworkMetadataName)).ToArray();

            if (targetFrameworks.Length == 0)
                targetFrameworks = PackageDependencies.Select(packageDependency => packageDependency.GetMetadata(SharedMetadata.TargetFrameworkMetadataName)).ToArray();
            
            if (targetFrameworks.Length == 0)
                targetFrameworks = FrameworkReferences.Select(frameworkReference => frameworkReference.GetMetadata(SharedMetadata.TargetFrameworkMetadataName)).ToArray();
                    
            targetFrameworks = targetFrameworks.Distinct()
                .Order()
                .ToArray();

            // If no target framework is supplied, fallback to netstandard2.0.
            projectContent = projectContent.Replace("$$TargetFrameworks$$", 
                targetFrameworks.Length > 0 ? string.Join(';', targetFrameworks) : "netstandard2.0");

            foreach (string targetFramework in targetFrameworks)
            {
                string references = string.Empty;

                // Add package dependencies
                foreach (ITaskItem packageDependency in PackageDependencies.Where(packageDependency => packageDependency.GetMetadata(SharedMetadata.TargetFrameworkMetadataName) == targetFramework))
                {
                    // Don't emit package references for targeting packs as those are added implicitly by the SDK.
                    if (packageDependency.ItemSpec == "NETStandard.Library")
                        continue;

                    references += $"    <PackageReference Include=\"{packageDependency.ItemSpec}\" Version=\"{packageDependency.GetMetadata("Version")}\" />\n";
                }

                // Add framework references
                foreach (ITaskItem frameworkReference in FrameworkReferences.Where(frameworkReference => frameworkReference.GetMetadata(SharedMetadata.TargetFrameworkMetadataName) == targetFramework))
                {
                    if (frameworkReference.ItemSpec != "mscorlib")
                    {
                        references += $"    <Reference Include=\"{frameworkReference.ItemSpec}\" />\n";
                    }
                }

                if (references != string.Empty)
                {
                    referenceIncludes += $"  <ItemGroup Condition=\"'$(TargetFramework)' == '{targetFramework}'\">\n";
                    referenceIncludes += references;
                    referenceIncludes += $"  </ItemGroup>\n\n";
                }

                // Retrieve the target framework's strong name data. For historical reasons,
                // we just use the first item that has the data available.
                if (strongNameData == default)
                {
                    foreach (ITaskItem compileItem in CompileItems.Where(compileItem => compileItem.GetMetadata(SharedMetadata.TargetFrameworkMetadataName) == targetFramework))
                    {
                        string strongNameKey = compileItem.GetMetadata(SharedMetadata.StrongNameKeyMetadataName);
                        string strongNameId = compileItem.GetMetadata(SharedMetadata.StrongNameIdMetadataName);
                        string strongNameFilename = compileItem.GetMetadata(SharedMetadata.StrongNameFilenameMetadataName);

                        if (!string.IsNullOrWhiteSpace(strongNameKey) &&
                            !string.IsNullOrWhiteSpace(strongNameId) &&
                            !string.IsNullOrWhiteSpace(strongNameFilename))
                        {
                            strongNameData = new(strongNameKey, strongNameId, strongNameFilename);
                            break;
                        }
                    }
                }
            }

            // Write the gathered package references into the project file.
            projectContent = projectContent.Replace("$$References$$", referenceIncludes);

            // If necessary, write the strong name key into the project file. Don't generate StrongNameKeyId for MSFT key.
            string keyFileTag = (strongNameData != default && strongNameData.Filename != "MSFT") ?
                $"\n    <StrongNameKeyId>{strongNameData.Id}</StrongNameKeyId>" :
                string.Empty;
            projectContent = projectContent.Replace("$$KeyFileTag$$", keyFileTag);

            // Calculate the assembly name from the compile items assembly name metadata. If more than one
            // distinct name is found (i.e. multi assembly package), use the PackageId instead.
            string[] assemblyNames = CompileItems
                .Select(compileItem => compileItem.GetMetadata(SharedMetadata.AssemblyNameMetadataName))
                .Distinct()
                .ToArray();
            projectContent = projectContent.Replace("$$AssemblyName$$",
                 assemblyNames.Length == 1 ? assemblyNames[0] : PackageId!);

            // Generate the project file
            Directory.CreateDirectory(Path.GetDirectoryName(TargetPath!)!);
            File.WriteAllText(TargetPath!, projectContent);

            return true;
        }
    }
}
