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
        [Required]
        public string PackageId { get; set; }

        [Required]
        public string[] TargetFrameworks { get; set; }

        [Required]
        public ITaskItem[] CompileItems { get; set; }

        [Required]
        public ITaskItem[] PackageDependencies { get; set; }

        [Required]
        public ITaskItem[] FrameworkReferences { get; set; }

        [Required]
        public string TargetPath { get; set; }

        [Required]
        public string ProjectTemplate { get; set; }

        public override bool Execute()
        {
            string pkgProjectOutput = File.ReadAllText(ProjectTemplate);
            string packageReferenceIncludes = "";

            StrongNameData strongNameData = default;
            string[] orderedTargetFrameworks = TargetFrameworks.Order().ToArray();

            foreach (string targetFramework in orderedTargetFrameworks)
            {
                string packageReferences = "";

                // Add package dependencies
                foreach (ITaskItem packageDependency in PackageDependencies.Where(packageDependency => packageDependency.GetMetadata(SharedMetadata.TargetFrameworkMetadataName) == targetFramework))
                {
                    // Don't emit package references for targeting packs as those are added implicitly by the SDK.
                    if (packageDependency.ItemSpec == "NETStandard.Library")
                        continue;

                    packageReferences += $"    <PackageReference Include=\"{packageDependency.ItemSpec}\" Version=\"{packageDependency.GetMetadata("Version")}\" />\n";
                }

                // Add framework references
                foreach (ITaskItem frameworkReference in FrameworkReferences.Where(frameworkReference => frameworkReference.GetMetadata(SharedMetadata.TargetFrameworkMetadataName) == targetFramework))
                {
                    if (frameworkReference.ItemSpec != "mscorlib")
                    {
                        packageReferences += $"    <Reference Include=\"{frameworkReference.ItemSpec}\" />\n";
                    }
                }

                // Write the gathered package references into the project file.
                if (packageReferences != "")
                {
                    packageReferenceIncludes += $"  <ItemGroup Condition=\"'$(TargetFramework)' == '{targetFramework}'\">\n";
                    packageReferenceIncludes += packageReferences;
                    packageReferenceIncludes += $"  </ItemGroup>\n\n";
                }

                // Retrieve the target framework's strong name data. For historical reasons,
                // we just use the first item that has the data available.
                if (strongNameData == default)
                {
                    ITaskItem[] compileItems = CompileItems.Where(compileItem => compileItem.GetMetadata(SharedMetadata.TargetFrameworkMetadataName) == targetFramework).ToArray();
                    foreach (ITaskItem compileItem in compileItems)
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

            // If necessary, write the strong name key into the project file.
            string keyFileTag = "";
            // Don't generate StrongNameKeyId for MSFT key
            if (strongNameData != default && strongNameData.Filename != "MSFT")
            {
                keyFileTag = $"\n    <StrongNameKeyId>{strongNameData.Id}</StrongNameKeyId>";
            }

            // Calculate the assembly name from the compile items assembly name metadata. If more than one
            // distinct name is found (i.e. multi assembly package), use the PackageId instead.
            string[] assemblyNames = CompileItems.Select(compileItem => compileItem.GetMetadata(SharedMetadata.AssemblyNameMetadataName))
                .Distinct()
                .ToArray();
            string assemblyName = assemblyNames.Length == 1 ?
                assemblyNames[0] :
                PackageId;

            pkgProjectOutput = pkgProjectOutput.Replace("$$TargetFrameworks$$", string.Join(';', orderedTargetFrameworks));
            pkgProjectOutput = pkgProjectOutput.Replace("$$KeyFileTag$$", keyFileTag);
            pkgProjectOutput = pkgProjectOutput.Replace("$$AssemblyName$$", assemblyName);
            pkgProjectOutput = pkgProjectOutput.Replace("$$PackageReferences$$", packageReferenceIncludes);

            // Generate the project file
            Directory.CreateDirectory(Path.GetDirectoryName(TargetPath));
            File.WriteAllText(TargetPath, pkgProjectOutput);

            return true;
        }
    }
}
