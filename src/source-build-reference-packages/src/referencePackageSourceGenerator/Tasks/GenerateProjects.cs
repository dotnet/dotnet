// Licensed to the .NET Foundation under one or more agreements.
// // The .NET Foundation licenses this file to you under the MIT license.
// // See the LICENSE file in the project root for more information.
//
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Repositories;
using NuGet.Frameworks;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.DotNet.SourceBuild.Tasks
{
     public class GenerateProjects : Task
    {
        [Required]
        public ITaskItem[] PackageDlls { get; set; }

        [Required]
        public string SrcPath { get; set; }

        [Required]
        public string TargetPackagesPath { get; set; }

        [Required]
        public int GeneratorVersion { get; set; }

        public override bool Execute()
        {
            DateTime startTime = DateTime.Now;
            string assemblyVersionTemplate = File.ReadAllText("AssemblyVersionTemplate.txt");
            string pkgProjectTemplate = File.ReadAllText("PackageProjectTemplate.xml");
            string[] standardUsings = File.ReadAllLines("AssemblyStandardUsings.txt");

            Log.LogMessage(MessageImportance.Low, "Start generating csproj files.");

            // First, create project data for all passed in dlls
            foreach (ITaskItem dll in PackageDlls)
            {
                var pd = ProjectData.GetOrCreateProjectData(TargetPackagesPath, dll.GetMetadata("FullPath"), SrcPath);
                Log.LogMessage(MessageImportance.Low, $"Generating data for {pd.PackageName}/{pd.ShortTfm}");
            }

            // Second, setup dependencies between all generated project data
            ProjectData.GenerateAllDependencies();

            foreach (var pkgData in PackageData.GetAll())
            {
                string pkgProjectOutput = pkgProjectTemplate;

                string packageReferenceIncludes = "\n";
                string outputPathByTfm = "\n";
                bool includesNetstandard21 = pkgData.ReferenceProjects.Any(dep => dep.ShortTfm == "netstandard2.1");
                bool includesNetcoreapp30 = pkgData.ReferenceProjects.Any(dep => dep.ShortTfm == "netcoreapp3.0");
                foreach (var dep in pkgData.ReferenceProjects)
                {
                    string packageReferences = "";
                    string netStandardTag = "NETStandardImplicitPackageVersion";
                    if (GeneratorVersion == 1)
                    {
                        netStandardTag = "NETStandardLibraryPackageVersion";
                    }

                    if (dep.ShortTfm == "netstandard2.0" && !includesNetstandard21 && !includesNetcoreapp30)
                    {
                        packageReferences += $"    <PackageReference Include=\"NETStandard.Library\" Version=\"$({netStandardTag})\" />\n";
                    }
                    foreach (var pkgRef in dep.GetPackageReferences())
                    {
                        string version = pkgRef.VersionRange.OriginalString;
                        // TODO: Generate a lookup table from source-build/PackageVersions.props.  For now, there is only one...
                        if (pkgRef.Id == "NETStandard.Library")
                        {
                            if (!includesNetstandard21 && !includesNetcoreapp30)
                            {
                                packageReferences += $"    <PackageReference Include=\"{pkgRef.Id}\" Version=\"$({netStandardTag})\" />\n";
                            }
                        }
                        else
                        {
                            packageReferences += $"    <PackageReference Include=\"{pkgRef.Id}\" Version=\"{version}\" />\n";
                        }
                    }
                    if (dep.ShortTfm.StartsWith("net4"))
                    {
                        packageReferences += $"    <PackageReference Include=\"Microsoft.NETFramework.ReferenceAssemblies.{dep.ShortTfm.Replace("net463","net462")}\" Version=\"1.0.2\" />\n";
                    }
                    foreach (var fwkRef in dep.GetFrameworkReferences())
                    {
                        if (fwkRef != "mscorlib")
                        {
                            packageReferences += $"    <Reference Include=\"{fwkRef}\" />\n";
                        }
                    }
                    if (packageReferences != "")
                    {
                        packageReferenceIncludes += $"  <ItemGroup Condition=\" '$(TargetFramework)' == '{dep.ShortTfm.Replace("net463","net462")}' \">\n";
                        packageReferenceIncludes += packageReferences;
                        packageReferenceIncludes += $"  </ItemGroup>\n\n";
                    }
                    if (dep.SubPath == "lib")
                    {
                        outputPathByTfm += $"  <PropertyGroup Condition=\" '$(TargetFramework)' == '{dep.ShortTfm.Replace("net463","net462")}' \">\n";
                        outputPathByTfm += $"    <OutputPath>$(ArtifactsBinDir){pkgData.RelativePath}/{dep.SubPath}/</OutputPath>\n";
                        outputPathByTfm += $"  </PropertyGroup>\n\n";
                    }
                }

                string targetFrameworks = pkgData.ReferenceProjects.Select(p => p.ShortTfm.Replace("net463","net462")).Distinct().Aggregate((tfmSet,tfm) => tfmSet + ";" + tfm);

                string appStrongNameKeyFileName = GetStrongNameKeyFileName(pkgData.ReferenceProjects.First());
                string keyFileTag = "";
                if (GeneratorVersion == 1)
                {
                    keyFileTag = $"\n    <AssemblyOriginatorKeyFile>$(KeyFileDir){appStrongNameKeyFileName}.snk</AssemblyOriginatorKeyFile>";
                }
                // Don't generate StrongNameKeyId for MSFT key for V2 Generator
                else if (appStrongNameKeyFileName != "MSFT")
                {
                    string appStrongNameKeyId = GetStrongNameKeyId(pkgData.ReferenceProjects.First());
                    keyFileTag = $"\n    <StrongNameKeyId>{appStrongNameKeyId}</StrongNameKeyId>";
                }

                string enableImplicitReferencesTag = "";
                if (includesNetstandard21 || includesNetcoreapp30)
                {
                    enableImplicitReferencesTag = "\n    <DisableImplicitFrameworkReferences>false</DisableImplicitFrameworkReferences>";
                }
                pkgProjectOutput = pkgProjectOutput.Replace("#LowerCaseFileName#", pkgData.Id.ToLower());
                pkgProjectOutput = pkgProjectOutput.Replace("#OutputPathByTfm#", outputPathByTfm);
                pkgProjectOutput = pkgProjectOutput.Replace("#RelativePath#", pkgData.RelativePath);
                pkgProjectOutput = pkgProjectOutput.Replace("#PackageReferences#", packageReferenceIncludes);
                pkgProjectOutput = pkgProjectOutput.Replace("#TargetFrameworks#", targetFrameworks);
                pkgProjectOutput = pkgProjectOutput.Replace("#KeyFileTag#", keyFileTag);
                pkgProjectOutput = pkgProjectOutput.Replace("#EnableImplicitReferencesTag#", enableImplicitReferencesTag);

                string projectOutputPath = pkgData.ProjectPath;
                if (GeneratorVersion == 2) projectOutputPath = pkgData.V2ProjectPath;
                Log.LogMessage(MessageImportance.High, $"Writing {projectOutputPath}");
                Directory.CreateDirectory(Path.GetDirectoryName(projectOutputPath));
                File.WriteAllText(projectOutputPath, pkgProjectOutput);

                // Write out Directory.Build.props for V2
                if (GeneratorVersion == 2)
                {
                    var directoryBuildPropsText =
                        "<Project>\n" +
                        "\n" +
                        "  <Import Project=\"$([MSBuild]::GetPathOfFileAbove(Directory.Build.props, $(MSBuildThisFileDirectory)..))\" />\n" +
                        "\n" +
                        "  <PropertyGroup>\n" +
                        $"    <AssemblyName>{pkgData.Id}</AssemblyName>\n" +
                        "  </PropertyGroup>\n" +
                        "\n" +
                        "</Project>\n";

                    File.WriteAllText(pkgData.DirectoryBuildPropsPath, directoryBuildPropsText);
                }
            }
            // Last, generate the AssemblyVersion and csproj files for each dll, inserting the dependencies
            foreach (ProjectData pd in ProjectData.GetAllProjectData())
            {
                string assemblyVersionData = assemblyVersionTemplate;
                string codeArtifactsPath = Path.GetDirectoryName(pd.ProjectFileName);

                string pacakageReferenceIncludes = "";
                foreach (var dep in pd.Dependencies)
                {
                    pacakageReferenceIncludes += $"<PackageReference Include=\"{dep.Id}\" Version=\"{dep.Version}\" />\n";
                }

                List<string> usings = pd.GetAdditionalUsings();
                usings.AddRange(standardUsings);
                var usingsText = usings.Distinct().OrderBy(u => u).Aggregate((a, b) => a + "\n" + b);

                assemblyVersionData = assemblyVersionData.Replace("#AssemblyUsings#", usingsText);
                assemblyVersionData = assemblyVersionData.Replace("#FileName#", pd.PackageName);
                assemblyVersionData = assemblyVersionData.Replace("#AssemblyVersion#", pd.GetAssemblyVersion());
                assemblyVersionData = assemblyVersionData.Replace("#FileVersion#", pd.GetFileVersion());
                assemblyVersionData = assemblyVersionData.Replace("#TypeForwards#", pd.GetTypeForwards());

                // Some packages have a net463 TFM, which we can't work with.  If both net462 and net463 is in a package,
                // just eliminate net463, otherwise, rename it to net462
                string outputFilename = pd.SourceFileName;
                bool updateSrcText = true;
                if (pd.SourceFileName.Contains("net463"))
                {
                    Log.LogMessage(MessageImportance.High, $"WARNING: Encountered package with net463 TFM: {pd.SourceFileName}");
                    outputFilename = pd.SourceFileName.Replace("net463", "net462");
                    if (File.Exists(outputFilename))
                    {
                        File.Delete(pd.SourceFileName);
                        updateSrcText = false;
                    }
                    else
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(outputFilename));
                        File.Move(pd.SourceFileName, outputFilename);
                    }
                    Directory.Delete(Path.GetDirectoryName(pd.SourceFileName));
                }

                if (updateSrcText)
                {
                    if (!File.Exists(outputFilename))
                    {
                        Log.LogError($"Source file '{outputFilename}' not found");
                        return false;
                    }

                    Log.LogMessage(MessageImportance.Low, $"Updating {outputFilename} with assembly version attributes");
                    Directory.CreateDirectory(codeArtifactsPath);
                    string srcText = File.ReadAllText(outputFilename);
                    srcText = srcText.Replace("#AssemblyVersionAttributes#", assemblyVersionData);
                    File.WriteAllText(outputFilename, srcText);
                }
            }
            Log.LogMessage(MessageImportance.High, $"Done generating csproj files. Took {DateTime.Now - startTime}");

            return true;
        }

        private string GetStrongNameKeyId(ProjectData assemblyInfo)
        {
            return GetStrongNameKeyTuple(assemblyInfo).Id;
        }

        private string GetStrongNameKeyFileName(ProjectData assemblyInfo)
        {
            return GetStrongNameKeyTuple(assemblyInfo).FileName;
        }

        private (string Id, string FileName) GetStrongNameKeyTuple(ProjectData assemblyInfo)
        {
            string appStrongNameKey = assemblyInfo.GetStrongNameKey();
            var KeyToNameMap = new Dictionary<string, (string, string)>()
            {
                { "b03f5f7f11d50a3a", ("Microsoft", "MSFT") },
                { "31bf3856ad364e35", ("MicrosoftShared", "35MSSharedLib1024") },
                { "adb9793829ddae60", ("MicrosoftAspNetCore", "AspNetCore") },
                { "b77a5c561934e089", ("ECMA", "ECMA") },
                { "cc7b13ffcd2ddd51", ("Open", "Open") }
            };

            if (KeyToNameMap.ContainsKey(appStrongNameKey))
            {
                return KeyToNameMap[appStrongNameKey];
            }
            else
            {
                throw new ArgumentException($"Found strong name key that doesn't map: Key={appStrongNameKey} Dll={assemblyInfo.DllPath}");
            }
        }
    }
}
