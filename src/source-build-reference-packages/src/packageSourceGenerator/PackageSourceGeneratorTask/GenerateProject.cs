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
        public required string PackageId { get; set; }

        /// <summary>
        /// The package version.
        /// </summary>
        [Required]
        public required string PackageVersion { get; set; }

        /// <summary>
        /// The path to the project template that is being transformed.
        /// </summary>
        [Required]
        public required string ProjectTemplate { get; set; }

        /// <summary>
        /// The target path that the project file is written to.
        /// </summary>
        [Required]
        public required string TargetPath { get; set; }

        /// <summary>
        /// The root directory that the projects are written into.
        /// </summary>
        [Required]
        public required string ProjectRoot { get; set; }

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

        /// <summary>
        /// The list of dependencies (package id) that should get emitted as PackageReference items.
        /// </summary>
        public string[]? AllowedPackageReference { get; set; }

        public override bool Execute()
        {
            string referenceIncludes = "";
            StrongNameData strongNameData = default;
            string projectContent = File.ReadAllText(ProjectTemplate);

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

            projectContent = projectContent.Replace("$$PackageVersion$$", PackageVersion);

            foreach (string targetFramework in targetFrameworks)
            {
                string packageReferences = string.Empty;
                string projectReferences = string.Empty;

                // Add package dependencies
                foreach (ITaskItem packageDependency in PackageDependencies.Where(packageDependency => packageDependency.GetMetadata(SharedMetadata.TargetFrameworkMetadataName) == targetFramework))
                {
                    string dependencyVersion = packageDependency.GetMetadata("Version");
                    string dependencyProjectRelativePath = Path.Combine(packageDependency.ItemSpec.ToLowerInvariant(), dependencyVersion, $"{packageDependency.ItemSpec}.{dependencyVersion}.csproj");

                    // If the dependency is on the package reference allowed list (i.e. for source-build-externals packages like Newtonsoft.Json), emit a package reference. Otherwise, emit a project reference.
                    if (AllowedPackageReference is not null && AllowedPackageReference.Contains(packageDependency.ItemSpec))
                    {
                        packageReferences += $"    <PackageReference Include=\"{packageDependency.ItemSpec}\" Version=\"{dependencyVersion}\" />{Environment.NewLine}";
                    }
                    else
                    {
                        // Make sure that the path always uses forward slashes, even on Windows.
                        projectReferences += $"    <ProjectReference Include=\"../../{dependencyProjectRelativePath.Replace('\\', '/')}\" />{Environment.NewLine}";
                    }
                }

                if (packageReferences != string.Empty || projectReferences != string.Empty)
                {
                    referenceIncludes += $"  <ItemGroup Condition=\"'$(TargetFramework)' == '{targetFramework}'\">{Environment.NewLine}";
                    referenceIncludes += packageReferences + projectReferences;
                    referenceIncludes += $"  </ItemGroup>{Environment.NewLine}{Environment.NewLine}";
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
                $"{Environment.NewLine}    <StrongNameKeyId>{strongNameData.Id}</StrongNameKeyId>" :
                string.Empty;
            projectContent = projectContent.Replace("$$KeyFileTag$$", keyFileTag);

            // Calculate the assembly name from the compile items assembly name metadata. If more than one
            // distinct name is found (i.e. multi assembly package), use the PackageId instead.
            string[] assemblyNames = CompileItems
                .Select(compileItem => compileItem.GetMetadata(SharedMetadata.AssemblyNameMetadataName))
                .Distinct()
                .ToArray();
            projectContent = projectContent.Replace("$$AssemblyName$$",
                 assemblyNames.Length == 1 ? assemblyNames[0] : PackageId);

            // Generate the project file
            Directory.CreateDirectory(Path.GetDirectoryName(TargetPath)!);
            File.WriteAllText(TargetPath, projectContent);

            return true;
        }
    }
}
