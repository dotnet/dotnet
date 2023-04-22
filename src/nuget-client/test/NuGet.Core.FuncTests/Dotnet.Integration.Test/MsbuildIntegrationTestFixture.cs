// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Test.Utility;
using Xunit;

namespace Dotnet.Integration.Test
{
    public class MsbuildIntegrationTestFixture : IDisposable
    {
        private readonly TestDirectory _cliDirectory;
        private readonly SimpleTestPathContext _templateDirectory;
        internal readonly string TestDotnetCli;
        internal readonly string MsBuildSdksPath;
        internal string SdkVersion { get; private set; }
        internal DirectoryInfo SdkDirectory { get; }
        private readonly Dictionary<string, string> _processEnvVars = new Dictionary<string, string>();

        public MsbuildIntegrationTestFixture()
        {
            string testAssemblyPath = Path.GetFullPath(Assembly.GetExecutingAssembly().Location);
            _cliDirectory = TestDotnetCLiUtility.CopyAndPatchLatestDotnetCli(testAssemblyPath);
            var dotnetExecutableName = RuntimeEnvironmentHelper.IsWindows ? "dotnet.exe" : "dotnet";
            TestDotnetCli = Path.Combine(_cliDirectory, dotnetExecutableName);

            var sdkPath = Directory.EnumerateDirectories(Path.Combine(_cliDirectory, "sdk"))
                            .Where(d => !string.Equals(Path.GetFileName(d), "NuGetFallbackFolder", StringComparison.OrdinalIgnoreCase))
                            .Single();

            SdkDirectory = new DirectoryInfo(sdkPath);
            MsBuildSdksPath = Path.Combine(sdkPath, "Sdks");

            _templateDirectory = new SimpleTestPathContext();
            TestDotnetCLiUtility.WriteGlobalJson(_templateDirectory.WorkingDirectory);

            // some project templates use implicit packages. For example, class libraries targeting netstandard2.0
            // will have an implicit package reference for NETStandard.Library, and its dependencies.
            // .NET Core SDK 3.0 and later no longer ship these packages in a NuGetFallbackFolder. Therefore, we need
            // to be able to download these packages. We'll download it once into the template cache's global packages
            // folder, and then use that as a local source for individual tests, to minimise network access.
            AddPackageSource("nuget.org", "https://api.nuget.org/v3/index.json");

            // This is for pre-release packages.
            AddPackageSource("dotnet", Constants.DotNetPackageSource.AbsoluteUri);

            _processEnvVars.Add("MSBuildSDKsPath", MsBuildSdksPath);
            _processEnvVars.Add("UseSharedCompilation", "false");
            _processEnvVars.Add("DOTNET_MULTILEVEL_LOOKUP", "0");
            _processEnvVars.Add("DOTNET_ROOT", _cliDirectory);
            _processEnvVars.Add("MSBUILDDISABLENODEREUSE ", "true");
        }

        private void AddPackageSource(string name, string source)
        {
            AddSourceArgs addSourceArgs = new()
            {
                Configfile = _templateDirectory.NuGetConfig,
                Name = name,
                Source = source
            };

            AddSourceRunner.Run(addSourceArgs, () => NullLogger.Instance);
        }

        /// <summary>
        /// Creates a new dotnet project of the specified type. Note that restore/build are not run when this command is invoked.
        /// That is because the project generation is cached.
        /// </summary>
        internal void CreateDotnetNewProject(string solutionRoot, string projectName, string args = "console", int timeOut = 60000)
        {
            args = args.Trim();
            var workingDirectory = Path.Combine(solutionRoot, projectName);
            if (!Directory.Exists(workingDirectory))
            {
                Directory.CreateDirectory(workingDirectory);
            }
            var templateDirectory = new DirectoryInfo(Path.Combine(_templateDirectory.SolutionRoot, args));

            if (!templateDirectory.Exists)
            {
                // According to https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/configure-language-version
                // The latest C# compiler will set default language version based on the TFM.
                // When the version of testing dotnet changed from 5.x to 6.x, the default TFM is changed to net6.0.
                // so the default C# language version for project targeting net6.0 will be set to 10, and it's not compatible with other TFMs.
                // We manually set the langVersion to the lowest 7.3, to make it compatible with other TFMs.
                string templateArgs = args;
                if (!templateArgs.Contains("langVersion") && (templateArgs.Equals("console") || templateArgs.Equals("classlib")))
                {
                    templateArgs = templateArgs + " --langVersion 7.3";
                }
                templateDirectory.Create();

                var result = CommandRunner.Run(TestDotnetCli,
                    templateDirectory.FullName,
                    $"new {templateArgs}",
                    waitForExit: true,
                    timeOutInMilliseconds: timeOut,
                    environmentVariables: _processEnvVars);
                Assert.True(result.Success, $"Creating project failed with following log information :\n {result.AllOutput}");
                Assert.True(string.IsNullOrWhiteSpace(result.Errors), $"Creating project failed with following message in error stream :\n {result.AllOutput}");
                // Delete the obj directory because it contains assets generated by running restore at dotnet new <template> time.
                // These are not relevant when the project is renamed
                Directory.Delete(Path.Combine(templateDirectory.FullName, "obj"), recursive: true);
            }
            CopyFromTemplate(projectName, args, workingDirectory, templateDirectory);
        }

        private static void CopyFromTemplate(string projectName, string args, string workingDirectory, DirectoryInfo templateDirectoryInfo)
        {
            foreach (var file in Directory.EnumerateFiles(templateDirectoryInfo.FullName))
            {
                File.Copy(file, Path.Combine(workingDirectory, Path.GetFileName(file)));
            }
            File.Move(
                Path.Combine(workingDirectory, args + ".csproj"),
                Path.Combine(workingDirectory, projectName + ".csproj"));
        }

        internal void CreateDotnetToolProject(string solutionRoot, string projectName, string targetFramework, string rid, string packageSources = null, IList<PackageIdentity> packages = null, int timeOut = 60000)
        {
            var workingDirectory = Path.Combine(solutionRoot, projectName);
            if (!Directory.Exists(workingDirectory))
            {
                Directory.CreateDirectory(workingDirectory);
            }

            var projectFileName = Path.Combine(workingDirectory, projectName + ".csproj");

            packageSources ??= string.Empty;
            var restorePackagesPath = Path.Combine(workingDirectory, "tools", "packages");
            var restoreSolutionDirectory = workingDirectory;
            var msbuildProjectExtensionsPath = Path.Combine(workingDirectory);
            var packageReferences = string.Empty;

            if (packages != null)
            {
                packageReferences = string.Join(Environment.NewLine, packages.Select(p => $@"        <PackageReference Include='{p.Id}' Version='{p.Version}'/>"));
            }

            var projectFile = $@"<Project>
    <PropertyGroup>
        <!-- Things that do change and before common props -->
        <MSBuildProjectExtensionsPath>{msbuildProjectExtensionsPath}</MSBuildProjectExtensionsPath>
    </PropertyGroup>
    <!-- Import it via Sdk attribute for local testing -->
    <Import Sdk='Microsoft.NET.Sdk' Project='Sdk.props'/>
    <PropertyGroup>
        <OutputType>Exe</OutputType>
        <RuntimeIdentifier>{rid}</RuntimeIdentifier>
        <TargetFramework>{targetFramework}</TargetFramework>
        <RestoreProjectStyle>DotnetToolReference</RestoreProjectStyle>
        <!-- Things that do change -->
        <RestoreSources>{packageSources}</RestoreSources>
        <RestorePackagesPath>{restorePackagesPath}</RestorePackagesPath>
        <RestoreSolutionDirectory>{restoreSolutionDirectory}</RestoreSolutionDirectory>
        <!--Things that don't change -->
        <RestoreAdditionalProjectSources/>
        <RestoreAdditionalProjectFallbackFolders/>
        <RestoreAdditionalProjectFallbackFoldersExcludes/>
        <RestoreFallbackFolders>clear</RestoreFallbackFolders>
        <CheckEolTargetFramework>false</CheckEolTargetFramework>
        <DisableImplicitFrameworkReferences>true</DisableImplicitFrameworkReferences>
    </PropertyGroup>
    <ItemGroup>
{packageReferences}
    </ItemGroup>
    <Import Sdk='Microsoft.NET.Sdk' Project='Sdk.targets'/>
</Project>";

            try
            {
                File.WriteAllText(projectFileName, projectFile);
            }
            catch
            {
                // ignore
            }
            Assert.True(File.Exists(projectFileName));
        }

        internal CommandRunnerResult RestoreToolProject(string workingDirectory, string projectName, string args = "")
        {
            var result = CommandRunner.Run(TestDotnetCli,
                workingDirectory,
                $"restore {projectName}.csproj {args}",
                waitForExit: true,
                environmentVariables: _processEnvVars);
            return result;
        }

        internal void RestoreProject(string workingDirectory, string projectName, string args, bool validateSuccess = true)
            => RestoreProjectOrSolution(workingDirectory, $"{projectName}.csproj", args, validateSuccess);

        internal void RestoreSolution(string workingDirectory, string solutionName, string args, bool validateSuccess = true)
            => RestoreProjectOrSolution(workingDirectory, $"{solutionName}.sln", args, validateSuccess);

        private void RestoreProjectOrSolution(string workingDirectory, string fileName, string args, bool validateSuccess)
        {
            var result = CommandRunner.Run(TestDotnetCli,
                workingDirectory,
                $"restore {fileName} {args}",
                waitForExit: true,
                environmentVariables: _processEnvVars);
            if (validateSuccess)
            {
                Assert.True(result.ExitCode == 0, $"Restore failed with following log information :\n {result.AllOutput}");
                Assert.True(result.Errors == "", $"Restore failed with following message in error stream :\n {result.AllOutput}");
            }
        }

        /// <summary>
        /// dotnet.exe args
        /// </summary>
        internal CommandRunnerResult RunDotnet(
            string workingDirectory,
            string args,
            bool ignoreExitCode = false,
            IReadOnlyDictionary<string, string> additionalEnvVars = null)
        {
            IDictionary<string, string> envVars;
            if (additionalEnvVars == null)
            {
                envVars = _processEnvVars;
            }
            else
            {
                // GroupBy respects sequence order, so taking the last pair per environment variable name will allow the
                // input dictionary to override the defaults.
                envVars = _processEnvVars
                    .Concat(additionalEnvVars)
                    .GroupBy(x => x.Key, _processEnvVars.Comparer)
                    .ToDictionary(x => x.Key, x => x.Last().Value);
            }

            var result = CommandRunner.Run(TestDotnetCli,
                workingDirectory,
                args,
                waitForExit: true,
                environmentVariables: envVars);

            if (!ignoreExitCode)
            {
                Assert.True(result.ExitCode == 0, $"dotnet.exe {args} command failed with following log information :\n {result.AllOutput}");
            }

            return result;
        }

        internal CommandRunnerResult PackProject(string workingDirectory, string projectName, string args, string nuspecOutputPath = "obj", bool validateSuccess = true, string configuration = "Debug")
        {
            // We can't provide empty or spaces as arguments if we used `string.IsNullOrEmpty` or `string.IsNullOrWhiteSpace`.
            if (nuspecOutputPath != null)
            {
                args = $"{args} /p:NuspecOutputPath={nuspecOutputPath}";
            }

            args = $"{args} /Property:Configuration={configuration}";

            return PackProjectOrSolution(workingDirectory, $"{projectName}.csproj", args, validateSuccess);
        }

        internal CommandRunnerResult PackSolution(string workingDirectory, string solutionName, string args, string nuspecOutputPath = "obj", bool validateSuccess = true, string configuration = "Debug")
        {
            if (nuspecOutputPath != null)
            {
                args = $"{args} /p:NuspecOutputPath={nuspecOutputPath}";
            }

            args = $"{args} /Property:Configuration={configuration}";

            return PackProjectOrSolution(workingDirectory, $"{solutionName}.sln", args, validateSuccess);
        }

        private CommandRunnerResult PackProjectOrSolution(string workingDirectory, string file, string args, bool validateSuccess)
        {
            var result = CommandRunner.Run(TestDotnetCli,
                workingDirectory,
                $"pack {file} {args}",
                waitForExit: true,
                environmentVariables: _processEnvVars);
            if (validateSuccess)
            {
                Assert.True(result.ExitCode == 0, $"Pack failed with following log information :\n {result.AllOutput}");
                Assert.True(result.Errors == "", $"Pack failed with following message in error stream :\n {result.AllOutput}");
            }
            return result;
        }

        internal void BuildProject(string workingDirectory, string projectName, string args, bool? appendRidToOutputPath = false, bool validateSuccess = true)
        {
            if (appendRidToOutputPath != null)
            {
                args = $"{args} /p:AppendRuntimeIdentifierToOutputPath={appendRidToOutputPath}";
            }
            BuildProjectOrSolution(workingDirectory, $"{projectName}.csproj", args, validateSuccess);
        }

        internal void BuildSolution(string workingDirectory, string solutionName, string args, bool? appendRidToOutputPath = false, bool validateSuccess = true)
        {
            if (appendRidToOutputPath != null)
            {
                args = $"{args} /p:AppendRuntimeIdentifierToOutputPath={appendRidToOutputPath}";
            }
            BuildProjectOrSolution(workingDirectory, $"{solutionName}.sln", args, validateSuccess);
        }

        private void BuildProjectOrSolution(string workingDirectory, string file, string args, bool validateSuccess)
        {
            var result = CommandRunner.Run(TestDotnetCli,
                workingDirectory,
                $"msbuild {file} {args}",
                waitForExit: true,
                environmentVariables: _processEnvVars);
            if (validateSuccess)
            {
                Assert.True(result.ExitCode == 0, $"Build failed with following log information :\n {result.AllOutput}");
                Assert.True(result.Errors == "", $"Build failed with following message in error stream :\n {result.AllOutput}");
            }
        }

        internal TestDirectory CreateTestDirectory()
        {
            var testDirectory = TestDirectory.Create();

            TestDotnetCLiUtility.WriteGlobalJson(testDirectory);

            return testDirectory;
        }

        internal SimpleTestPathContext CreateSimpleTestPathContext(bool addTemplateFeed = true)
        {
            var simpleTestPathContext = new SimpleTestPathContext();

            TestDotnetCLiUtility.WriteGlobalJson(simpleTestPathContext.WorkingDirectory);

            if (addTemplateFeed)
            {
                // Some template and TFM combinations need packages, for example NETStandard.Library.
                // The template cache should have downloaded it already, so use the template cache's
                // global packages folder as a local source.
                var addSourceArgs = new AddSourceArgs()
                {
                    Configfile = simpleTestPathContext.NuGetConfig,
                    Name = "template",
                    Source = _templateDirectory.UserPackagesFolder
                };
                AddSourceRunner.Run(addSourceArgs, () => NullLogger.Instance);
            }

            return simpleTestPathContext;
        }

        internal TestDirectory Build(TestDirectoryBuilder testDirectoryBuilder)
        {
            var testDirectory = testDirectoryBuilder.Build();

            TestDotnetCLiUtility.WriteGlobalJson(testDirectory);

            return testDirectory;
        }

        public void Dispose()
        {
            RunDotnet(Path.GetDirectoryName(TestDotnetCli), "build-server shutdown");
            KillDotnetExe(TestDotnetCli);
            _cliDirectory.Dispose();
            _templateDirectory.Dispose();
        }

        private static void KillDotnetExe(string pathToDotnetExe)
        {
            var processes = Process.GetProcessesByName("dotnet")
                .Where(t => string.Compare(t.MainModule.FileName, Path.GetFullPath(pathToDotnetExe), ignoreCase: true) == 0);
            var testDirProcesses = Process.GetProcesses()
                .Where(t => t.MainModule.FileName.StartsWith(TestFileSystemUtility.NuGetTestFolder, StringComparison.OrdinalIgnoreCase));
            try
            {
                if (processes != null)
                {
                    foreach (var process in processes)
                    {
                        if (string.Compare(process.MainModule.FileName, Path.GetFullPath(pathToDotnetExe), true) == 0)
                        {
                            process.Kill();
                        }
                    }
                }

                if (testDirProcesses != null)
                {
                    foreach (var process in testDirProcesses)
                    {
                        process.Kill();
                    }
                }

            }
            catch { }
        }

        /// <summary>
        /// Depth-first recursive delete, with handling for descendant
        /// directories open in Windows Explorer or used by another process
        /// </summary>
        private static void DeleteDirectory(string path)
        {
            foreach (string directory in Directory.EnumerateDirectories(path))
            {
                DeleteDirectory(directory);
            }

            try
            {
                Directory.Delete(path, true);
            }
            catch (IOException)
            {
                Directory.Delete(path, true);
            }
            catch (UnauthorizedAccessException)
            {
                var MaxTries = 100;

                for (var i = 0; i < MaxTries; i++)
                {

                    try
                    {
                        Directory.Delete(path, recursive: true);
                        break;
                    }
                    catch (UnauthorizedAccessException) when (i < (MaxTries - 1))
                    {
                        Thread.Sleep(100);
                    }
                }
            }
            catch
            {

            }
        }
    }
}
