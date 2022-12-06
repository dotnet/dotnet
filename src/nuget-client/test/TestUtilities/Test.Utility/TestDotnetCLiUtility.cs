// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Frameworks;
using NuGet.Protocol;
using NuGet.Versioning;

namespace NuGet.Test.Utility
{
    public static class TestDotnetCLiUtility
    {
        internal static string SdkVersion { get; private set; }
        internal static NuGetFramework SdkTfm { get; private set; }
        internal static string CliDirSource { get; private set; }
        internal static string SdkDirSource { get; private set; }

#if !IS_DESKTOP
        // For non fullframework code path, we could dynamically determine which SDK version to copy by checking the TFM of test project assembly and the dotnet.dll.
        public static TestDirectory CopyAndPatchLatestDotnetCli(string testAssemblyPath)
        {
            CliDirSource = Path.GetDirectoryName(TestFileSystemUtility.GetDotnetCli());
            SdkDirSource = Path.Combine(CliDirSource, "sdk" + Path.DirectorySeparatorChar);

            // Dynamically determine which SDK version to copy
            SdkVersion = GetSdkToTestByAssemblyPath(testAssemblyPath);

            // Dynamically determine the TFM of the dotnet.dll
            SdkTfm = AssemblyReader.GetTargetFramework(Path.Combine(SdkDirSource, SdkVersion, "dotnet.dll"));

            var cliDirDestination = TestDirectory.Create();
            CopyLatestCliToTestDirectory(cliDirDestination);
            UpdateCliWithLatestNuGetAssemblies(cliDirDestination);

            return cliDirDestination;
        }
#else
        // For fullframework code path, the test project dll could not be used to dynamically determine which SDK version to copy,
        // so we need to specify the sdkVersion and sdkTfm in order to patch the right version of SDK.
        public static TestDirectory CopyAndPatchLatestDotnetCli(string sdkVersion, string sdkTfm)
        {
            CliDirSource = Path.GetDirectoryName(TestFileSystemUtility.GetDotnetCli());
            SdkDirSource = Path.Combine(CliDirSource, "sdk" + Path.DirectorySeparatorChar);

            // Use specified sdkVersion
            SdkVersion = GetSdkToTestByVersion(sdkVersion);

            // Use specified sdkTfm
            SdkTfm = NuGetFramework.Parse(sdkTfm);

            var cliDirDestination = TestDirectory.Create();
            CopyLatestCliToTestDirectory(cliDirDestination);
            UpdateCliWithLatestNuGetAssemblies(cliDirDestination);

            return cliDirDestination;
        }
#endif

        private static void CopyLatestCliToTestDirectory(string destinationDir)
        {
            WriteGlobalJson(destinationDir);

            var sdkPath = Path.Combine(SdkDirSource, SdkVersion + Path.DirectorySeparatorChar);
            var fallbackFolderPath = Path.Combine(SdkDirSource, "NuGetFallbackFolder");

            Func<string, bool> predicate = path =>
            {
                if (!path.StartsWith(SdkDirSource))
                {
                    return true;
                }

                return path.StartsWith(sdkPath) || path.StartsWith(fallbackFolderPath);
            };

            //Create sub-directory structure in destination, ignoring any SDK version not selected.
            foreach (var directory in Directory.EnumerateDirectories(CliDirSource, "*", SearchOption.AllDirectories).Where(predicate))
            {
                var destDir = destinationDir + directory.Substring(CliDirSource.Length);
                if (!Directory.Exists(destDir))
                {
                    Directory.CreateDirectory(destDir);
                }
            }

            var lastWriteTime = DateTime.Now.AddDays(-2);

            //Copy files recursively to destination directories, ignoring any SDK version not selected.
            foreach (var fileName in Directory.EnumerateFiles(CliDirSource, "*", SearchOption.AllDirectories).Where(predicate))
            {
                var destFileName = destinationDir + fileName.Substring(CliDirSource.Length);
                File.Copy(fileName, destFileName);
                File.SetLastWriteTime(destFileName, lastWriteTime);
            }
        }

#if !IS_DESKTOP
        // Dynamically determine which SDK version to copy by checking the TFM of test project assembly and the dotnet.dll.
        private static string GetSdkToTestByAssemblyPath(string testAssemblyPath)
        {
            // The TFM we're testing
            var testTfm = AssemblyReader.GetTargetFramework(testAssemblyPath);

            var selectedVersion =
                Directory.EnumerateDirectories(SdkDirSource) // get all directories in sdk folder
                .Where(path =>
                { // SDK is for TFM to test
                    if (string.Equals(Path.GetFileName(path), "NuGetFallbackFolder", StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }

                    var dotnetPath = Path.Combine(path, "dotnet.dll");
                    var sdkTfm = AssemblyReader.GetTargetFramework(dotnetPath);

                    return testTfm == sdkTfm;
                })
                .Select(Path.GetFileName) // just the folder name (version string)
                .OrderByDescending(path => NuGetVersion.Parse(Path.GetFileName(path))) // in case there are multiple matching SDKs, selected the highest version
                .FirstOrDefault();

            if (selectedVersion == null)
            {
                selectedVersion = Directory.EnumerateDirectories(SdkDirSource)
                    .Select(Path.GetFileName)
                    .OrderByDescending(directoryName => NuGetVersion.Parse(directoryName))
                    .FirstOrDefault();
            }

            if (selectedVersion == null)
            {
                var message = $@"Could not find suitable SDK to test in {SdkDirSource}
TFM being tested: {testTfm.DotNetFrameworkName}
SDKs found: {string.Join(", ", Directory.EnumerateDirectories(SdkDirSource).Select(Path.GetFileName).Where(d => !string.Equals(d, "NuGetFallbackFolder", StringComparison.OrdinalIgnoreCase)))}";

                throw new Exception(message);
            }

            return selectedVersion;
        }
#else
        // Use specified sdkVersion(could be just a major version) to determine which SDK version to copy.
        private static string GetSdkToTestByVersion(string sdkVersion)
        {
            var selectedVersion =
                Directory.EnumerateDirectories(SdkDirSource) // get all directories in sdk folder
                .Where(path =>
                { // SDK is for TFM to test
                    if (string.Equals(Path.GetFileName(path), "NuGetFallbackFolder", StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }

                    var version = path.Substring(Path.GetDirectoryName(path).Length + 1);

                    return version.StartsWith(sdkVersion);
                })
                .Select(Path.GetFileName) // just the folder name (version string)
                .OrderByDescending(path => NuGetVersion.Parse(Path.GetFileName(path))) // in case there are multiple matching SDKs, selected the highest version
                .FirstOrDefault();

            if (selectedVersion == null)
            {
                var message = $@"Could not find suitable SDK to test in {SdkDirSource}
sdkVersion specified: {sdkVersion}
SDKs found: {string.Join(", ", Directory.EnumerateDirectories(SdkDirSource).Select(Path.GetFileName).Where(d => !string.Equals(d, "NuGetFallbackFolder", StringComparison.OrdinalIgnoreCase)))}";

                throw new Exception(message);
            }

            return selectedVersion;
        }
#endif
        private static void UpdateCliWithLatestNuGetAssemblies(string cliDirectory)
        {
            var artifactsDirectory = TestFileSystemUtility.GetArtifactsDirectoryInRepo();
            var pathToSdkInCli = Path.Combine(
                    Directory.EnumerateDirectories(Path.Combine(cliDirectory, "sdk"))
                    .Where(d => !string.Equals(Path.GetFileName(d), "NuGetFallbackFolder", StringComparison.OrdinalIgnoreCase))
                    .First());
            const string configuration =
#if DEBUG
                "Debug";
#else
                "Release";
#endif
            CopyPackSdkArtifacts(artifactsDirectory, pathToSdkInCli, configuration);
            CopyRestoreArtifacts(artifactsDirectory, pathToSdkInCli, configuration);
        }

        private static void CopyRestoreArtifacts(string artifactsDirectory, string pathToSdkInCli, string configuration)
        {
            var fileExtensions = new[] { ".dll", ".pdb", ".targets" };

            var sdkDependencies = new List<string> { "NuGet.Build.Tasks.Console", "NuGet.CommandLine.XPlat" };

            foreach (var projectName in sdkDependencies)
            {
                var projectArtifactsBinFolder = Path.Combine(artifactsDirectory, projectName, "bin", configuration);

                var tfmToCopy = GetTfmToCopy(projectArtifactsBinFolder);
                var frameworkArtifactsFolder = new DirectoryInfo(Path.Combine(projectArtifactsBinFolder, tfmToCopy));

                foreach (string fileExtension in fileExtensions)
                {
                    foreach (FileInfo file in frameworkArtifactsFolder.EnumerateFiles($"*{fileExtension}"))
                    {
                        file.CopyTo(Path.Combine(pathToSdkInCli, file.Name), overwrite: true);
                    }
                }

                // Update NuGet.CommandLine.XPlat.runtimeconfig.json so that our dependencies are loaded correctly
                FileInfo runtimeConfigJsonFile = new FileInfo(Path.Combine(pathToSdkInCli, $"{projectName}.runtimeconfig.json"));

                JObject jObject = runtimeConfigJsonFile.Exists ? JsonUtility.LoadJson(runtimeConfigJsonFile.OpenText()) : new JObject();

                jObject["runtimeOptions"]["configProperties"]["Microsoft.NETCore.DotNetHostPolicy.SetAppPaths"] = true;

                using (StreamWriter streamWriter = File.CreateText(runtimeConfigJsonFile.FullName))
                using (JsonTextWriter writer = new JsonTextWriter(streamWriter)
                {
                    Formatting = Formatting.Indented
                })
                {
                    jObject.WriteTo(writer);
                }
            }
        }

        private static string GetTfmToCopy(string projectArtifactsBinFolder)
        {
            var compiledTfms =
                Directory.EnumerateDirectories(projectArtifactsBinFolder) // get all directories in bin folder
                .Select(Path.GetFileName) // just the folder name (tfm)
                .ToDictionary(folder => NuGetFramework.Parse(folder));

            var reducer = new FrameworkReducer();
            var selectedTfm = reducer.GetNearest(SdkTfm, compiledTfms.Keys);

            if (selectedTfm == null)
            {
                var message = $@"Could not find suitable assets to copy in {projectArtifactsBinFolder}
TFM being tested: {SdkTfm}
project TFMs found: {string.Join(", ", compiledTfms.Keys.Select(k => k.ToString()))}";

                throw new Exception(message);
            }

            var selectedVersion = compiledTfms[selectedTfm];

            return selectedVersion;
        }

        private static void CopyPackSdkArtifacts(string artifactsDirectory, string pathToSdkInCli, string configuration)
        {
            var pathToPackSdk = Path.Combine(pathToSdkInCli, "Sdks", "NuGet.Build.Tasks.Pack");

            const string packProjectName = "NuGet.Build.Tasks.Pack";
            const string packTargetsName = "NuGet.Build.Tasks.Pack.targets";

            // Copy the pack SDK.
            var packProjectBinDirectory = Path.Combine(artifactsDirectory, packProjectName, "bin", configuration);
            var tfmToCopy = GetTfmToCopy(packProjectBinDirectory);

            var packProjectCoreArtifactsDirectory = new DirectoryInfo(Path.Combine(packProjectBinDirectory, tfmToCopy));

            // We are only copying the CoreCLR assets, since, we're testing only them under Core MSBuild.
            var targetRuntimeType = "CoreCLR";

            var packAssemblyDestinationDirectory = Path.Combine(pathToPackSdk, targetRuntimeType);
            // Be smart here so we don't have to call ILMerge in the VS build. It takes ~15s total.
            // In VisualStudio, simply use the non il merged version.
            var ilMergedPackDirectoryPath = Path.Combine(packProjectCoreArtifactsDirectory.FullName, "ilmerge");
            if (Directory.Exists(ilMergedPackDirectoryPath))
            {
                var packFileName = packProjectName + ".dll";
                // Only use the il merged assembly if it's newer than the build.
                DateTime packAssemblyCreationDate = File.GetCreationTimeUtc(Path.Combine(packProjectCoreArtifactsDirectory.FullName, packFileName));
                DateTime ilMergedPackAssemblyCreationDate = File.GetCreationTimeUtc(Path.Combine(ilMergedPackDirectoryPath, packFileName));
                if (ilMergedPackAssemblyCreationDate > packAssemblyCreationDate)
                {
                    File.Copy(sourceFileName: Path.Combine(packProjectCoreArtifactsDirectory.FullName, "ilmerge", packFileName),
                        destFileName: Path.Combine(packAssemblyDestinationDirectory, packFileName),
                        overwrite: true);
                }
            }
            else
            {
                foreach (var assembly in packProjectCoreArtifactsDirectory.EnumerateFiles("*.dll"))
                {
                    File.Copy(
                        sourceFileName: assembly.FullName,
                        destFileName: Path.Combine(packAssemblyDestinationDirectory, assembly.Name),
                        overwrite: true);
                }
            }
            // Copy the pack targets
            var packTargetsSource = Path.Combine(packProjectCoreArtifactsDirectory.FullName, packTargetsName);
            var targetsDestination = Path.Combine(pathToPackSdk, "build", packTargetsName);
            var targetsDestinationCrossTargeting = Path.Combine(pathToPackSdk, "buildCrossTargeting", packTargetsName);
            File.Copy(packTargetsSource, targetsDestination, overwrite: true);
            File.Copy(packTargetsSource, targetsDestinationCrossTargeting, overwrite: true);
        }

        public static void WriteGlobalJson(string path)
        {
            string globalJsonText = $"{{\"sdk\": {{\"version\": \"{SdkVersion}\"}}}}";
            var globalJsonPath = Path.Combine(path, "global.json");
            File.WriteAllText(globalJsonPath, globalJsonText);
        }
    }
}
