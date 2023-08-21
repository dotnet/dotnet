// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using FluentAssertions;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Test.Utility;
using Xunit;

namespace Dotnet.Integration.Test
{
    public class DotnetIntegrationTestFixture : IDisposable
    {
        /// <summary>
        /// A value indicating if the test is running on a hosted agent with diagnostics enabled.
        /// </summary>
        internal static readonly bool CIDebug = string.Equals(Environment.GetEnvironmentVariable("SYSTEM_DEBUG"), bool.TrueString, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// A value indicating the path to a directory where logs should be written to so they can be published as an artifact from a hosted build agent.
        /// </summary>
        internal static readonly string BinLogDirectory = Environment.GetEnvironmentVariable("BINLOG_DIRECTORY");

        private readonly TestDirectory _cliDirectory;
        private readonly SimpleTestPathContext _templateDirectory;
        internal readonly string TestDotnetCli;
        internal readonly string MsBuildSdksPath;
        internal string SdkVersion { get; private set; }
        internal DirectoryInfo SdkDirectory { get; }

        public DotnetIntegrationTestFixture()
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
        internal void CreateDotnetNewProject(string solutionRoot, string projectName, string args)
        {
            args = args.Trim();

            string template = args.Replace(" ", "_");

            var workingDirectory = Path.Combine(solutionRoot, projectName);
            if (!Directory.Exists(workingDirectory))
            {
                Directory.CreateDirectory(workingDirectory);
            }
            var templateDirectory = new DirectoryInfo(Path.Combine(_templateDirectory.SolutionRoot, template));

            if (!templateDirectory.Exists)
            {
                string templateArgs = args + " --name template";
                if (!templateArgs.Contains("langVersion") && (templateArgs.StartsWith("console") || templateArgs.StartsWith("classlib")))
                {
                    templateArgs = templateArgs + " --langVersion 7.3";
                }
                templateDirectory.Create();

                RunDotnetExpectSuccess(templateDirectory.FullName, $"new {templateArgs}");

                // Delete the obj directory because it contains assets generated by running restore at dotnet new <template> time.
                // These are not relevant when the project is renamed
                Directory.Delete(Path.Combine(templateDirectory.FullName, "template", "obj"), recursive: true);
            }

            foreach (var file in Directory.EnumerateFiles(new DirectoryInfo(Path.Combine(templateDirectory.FullName, "template")).FullName))
            {
                File.Copy(file, Path.Combine(workingDirectory, Path.GetFileName(file)));
            }

            File.Move(
                Path.Combine(workingDirectory, "template.csproj"),
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

        internal CommandRunnerResult RestoreToolProjectExpectFailure(string workingDirectory, string projectName, string args = "")
            => RunDotnetExpectFailure(workingDirectory, $"restore {projectName}.csproj {args}");

        internal CommandRunnerResult RestoreToolProjectExpectSuccess(string workingDirectory, string projectName, string args = "")
            => RunDotnetExpectSuccess(workingDirectory, $"restore {projectName}.csproj {args}");

        internal CommandRunnerResult RestoreProjectExpectFailure(string workingDirectory, string projectName, string args = "")
            => RestoreProjectOrSolution(workingDirectory, $"{projectName}.csproj", args, expectSuccess: false);

        internal CommandRunnerResult RestoreProjectExpectSuccess(string workingDirectory, string projectName, string args = "")
            => RestoreProjectOrSolution(workingDirectory, $"{projectName}.csproj", args, expectSuccess: true);

        internal CommandRunnerResult RestoreSolutionExpectFailure(string workingDirectory, string solutionName, string args = "")
            => RestoreProjectOrSolution(workingDirectory, $"{solutionName}.sln", args, expectSuccess: false);

        internal CommandRunnerResult RestoreSolutionExpectSuccess(string workingDirectory, string solutionName, string args = "")
            => RestoreProjectOrSolution(workingDirectory, $"{solutionName}.sln", args, expectSuccess: true);

        private CommandRunnerResult RestoreProjectOrSolution(string workingDirectory, string fileName, string args, bool expectSuccess)
            => RunDotnet(workingDirectory, $"restore {fileName} {args ?? string.Empty}", expectSuccess);

        /// <summary>
        /// Runs dotnet with the specified arguments and expects the command to succeed. If dotnet returns a non-zero exit code, an assertion is thrown with diagnostic information.
        /// </summary>
        /// <param name="workingDirectory">The working directory to use when executing the command.</param>
        /// <param name="args">The command-line arguments to pass to dotnet.</param>
        /// <param name="environmentVariables">An optional <see cref="IReadOnlyDictionary{TKey, TValue}" /> containing environment variables to use when executing the command.</param>
        internal CommandRunnerResult RunDotnetExpectSuccess(string workingDirectory, string args = "", IReadOnlyDictionary<string, string> environmentVariables = null)
            => RunDotnet(workingDirectory, args, expectSuccess: true, environmentVariables);

        /// <summary>
        /// Runs dotnet with the specified arguments and expects the command to fail. If dotnet returns an exit code of zero, an assertion is thrown with diagnostic information.
        /// </summary>
        /// <param name="workingDirectory">The working directory to use when executing the command.</param>
        /// <param name="args">The command-line arguments to pass to dotnet.</param>
        /// <param name="environmentVariables">An optional <see cref="IReadOnlyDictionary{TKey, TValue}" /> containing environment variables to use when executing the command.</param>
        internal CommandRunnerResult RunDotnetExpectFailure(string workingDirectory, string args = "", IReadOnlyDictionary<string, string> environmentVariables = null)
            => RunDotnet(workingDirectory, args, expectSuccess: false, environmentVariables);

        private CommandRunnerResult RunDotnet(string workingDirectory, string args = "", bool expectSuccess = true, IReadOnlyDictionary<string, string> environmentVariables = null)
        {
            bool enableDiagnostics = CIDebug && !string.IsNullOrWhiteSpace(BinLogDirectory);

            FileInfo coreHostLogFileInfo = null;

            Dictionary<string, string> finalEnvironmentVariables = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["MSBuildSDKsPath"] = MsBuildSdksPath,
                ["UseSharedCompilation"] = bool.FalseString,
                ["DOTNET_MULTILEVEL_LOOKUP"] = "0",
                ["DOTNET_ROOT"] = _cliDirectory,
                ["MSBUILDDISABLENODEREUSE"] = bool.TrueString,
                ["NUGET_SHOW_STACK"] = bool.TrueString
            };

            if (enableDiagnostics)
            {
                coreHostLogFileInfo = new FileInfo(Path.GetTempFileName());
                coreHostLogFileInfo.Delete();

                finalEnvironmentVariables["COREHOST_TRACE"] = "1";
                finalEnvironmentVariables["COREHOST_TRACEFILE"] = coreHostLogFileInfo.FullName;
                finalEnvironmentVariables["COMPlus_DbgEnableElfDumpOnMacOS"] = "1";
                finalEnvironmentVariables["COMPlus_DbgEnableMiniDump"] = "1";
                finalEnvironmentVariables["COMPLUS_DBGENABLEMINIDUMP"] = "1";
                finalEnvironmentVariables["COMPlus_DbgMiniDumpName"] = Path.Combine(BinLogDirectory, $"minidump-%p-%t.dmp");
                finalEnvironmentVariables["COMPLUS_DBGMINIDUMPNAME"] = Path.Combine(BinLogDirectory, $"minidump-%p-%t.dmp");
                finalEnvironmentVariables["COMPlus_DbgMiniDumpType"] = "4";
                finalEnvironmentVariables["COMPLUS_DBGMINIDUMPTYPE"] = "4";
                finalEnvironmentVariables["DOTNET_DbgEnableMiniDump"] = "1";
                finalEnvironmentVariables["DOTNET_DbgMiniDumpName"] = Path.Combine(BinLogDirectory, $"minidump-%p-%t.dmp");
            }

            if (environmentVariables != null)
            {
                foreach (var item in environmentVariables)
                {
                    finalEnvironmentVariables[item.Key] = item.Value;
                }
            }

            CommandRunnerResult result = null;

            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();

                result = CommandRunner.Run(TestDotnetCli, workingDirectory, args, environmentVariables: finalEnvironmentVariables);

                stopwatch.Stop();

                if (expectSuccess)
                {
                    result.ExitCode.Should().Be(
                        0,
                        "{0} {1} should have succeeded but returned exit code {2} after {3:N1}s with the following output:{4}{5}",
                        TestDotnetCli,
                        args,
                        result.ExitCode,
                        stopwatch.Elapsed.TotalSeconds,
                        Environment.NewLine,
                        result.AllOutput);

                }
                else
                {
                    result.ExitCode.Should().Be(
                        1,
                        "{0} {1} should have failed with exit code 1 but returned exit code {2} after {3:N1}s with the following output:{4}{5}",
                        TestDotnetCli,
                        args,
                        result.ExitCode,
                        stopwatch.Elapsed.TotalSeconds,
                        Environment.NewLine,
                        result.AllOutput);
                }

                return result;
            }
            catch (Exception e) when (enableDiagnostics)
            {
                CreateDiagnostics(args, workingDirectory, result, finalEnvironmentVariables, coreHostLogFileInfo, e);

                throw;
            }

            void CreateDiagnostics(string args, string workingDirectory, CommandRunnerResult result, Dictionary<string, string> environmentVariables, FileInfo coreHostLogFileInfo, Exception exception = null)
            {
                string key = Guid.NewGuid().ToString("N");

                FileInfo diagLogFileInfo = new FileInfo(Path.Combine(BinLogDirectory, $"diagnostics-{key}.log"));

                Directory.CreateDirectory(diagLogFileInfo.DirectoryName);

                if (coreHostLogFileInfo != null && coreHostLogFileInfo.Exists)
                {
                    coreHostLogFileInfo.CopyTo(Path.Combine(BinLogDirectory, $"corehost-{key}.log"));
                }

                using StreamWriter writer = diagLogFileInfo.CreateText();

                if (result != null)
                {
                    writer.WriteLine("Exit Code = {0}", result.ExitCode);
                    writer.WriteLine("Output:");
                    writer.WriteLine(result.Output);
                    writer.WriteLine("Errors:");
                    writer.WriteLine(result.Errors);
                }

                writer.WriteLine("Args = {0}", args);
                writer.WriteLine("Runtime description: {0}", RuntimeInformation.FrameworkDescription);
                writer.WriteLine("Runtime identifier: {0}", RuntimeInformation.RuntimeIdentifier);

                if (exception != null)
                {
                    writer.WriteLine("Exception = {0}", exception);
                }

                writer.WriteLine("Environment Variables:");

                IDictionary actualEnvironmentVariables = Environment.GetEnvironmentVariables();

                foreach (KeyValuePair<string, string> item in environmentVariables)
                {
                    actualEnvironmentVariables[item.Key] = item.Value;
                }

                foreach (DictionaryEntry item in actualEnvironmentVariables.Cast<DictionaryEntry>().OrderBy(i => i.Key))
                {
                    writer.WriteLine("  {0}={1}", item.Key, item.Value);
                }
            }
        }

        internal CommandRunnerResult PackProjectExpectFailure(string workingDirectory, string projectName, string args = "", string nuspecOutputPath = "obj", string configuration = "Debug")
            => PackProjectOrSolution(workingDirectory, $"{projectName}.csproj", args, expectSuccess: false, nuspecOutputPath, configuration);

        internal CommandRunnerResult PackProjectExpectSuccess(string workingDirectory, string projectName, string args = "", string nuspecOutputPath = "obj", string configuration = "Debug")
            => PackProjectOrSolution(workingDirectory, $"{projectName}.csproj", args, expectSuccess: true, nuspecOutputPath, configuration);

        internal CommandRunnerResult PackSolutionExpectFailure(string workingDirectory, string solutionName, string args = "", string nuspecOutputPath = "obj", string configuration = "Debug")
            => PackProjectOrSolution(workingDirectory, $"{solutionName}.sln", args, expectSuccess: false, nuspecOutputPath, configuration);

        internal CommandRunnerResult PackSolutionExpectSuccess(string workingDirectory, string solutionName, string args = "", string nuspecOutputPath = "obj", string configuration = "Debug")
            => PackProjectOrSolution(workingDirectory, $"{solutionName}.sln", args, expectSuccess: true, nuspecOutputPath, configuration);

        private CommandRunnerResult PackProjectOrSolution(string workingDirectory, string file, string args, bool expectSuccess, string nuspecOutputPath = "obj", string configuration = "Debug")
        {
            if (nuspecOutputPath != null)
            {
                args = $"{args} /p:NuspecOutputPath={nuspecOutputPath}";
            }

            args = $"{args} /Property:Configuration={configuration}";

            return RunDotnet(workingDirectory, $"pack {file} {args}", expectSuccess);
        }

        internal void BuildProjectExpectSuccess(string workingDirectory, string projectName, string args = "", bool? appendRidToOutputPath = false)
        {
            if (appendRidToOutputPath != null)
            {
                args = $"{args} /p:AppendRuntimeIdentifierToOutputPath={appendRidToOutputPath}";
            }
            BuildProjectOrSolution(workingDirectory, $"{projectName}.csproj", args, expectSuccess: true);
        }

        internal void BuildSolutionExpectFailure(string workingDirectory, string solutionName, string args = "", bool? appendRidToOutputPath = false)
            => BuildProjectOrSolution(workingDirectory, $"{solutionName}.sln", args, expectSuccess: false, appendRidToOutputPath);

        internal void BuildSolutionExpectSuccess(string workingDirectory, string solutionName, string args = "", bool? appendRidToOutputPath = false)
            => BuildProjectOrSolution(workingDirectory, $"{solutionName}.sln", args, expectSuccess: true, appendRidToOutputPath);

        private CommandRunnerResult BuildProjectOrSolution(string workingDirectory, string file, string args, bool expectSuccess = true, bool? appendRidToOutputPath = false)
        {
            if (appendRidToOutputPath != null)
            {
                args = $"{args} /p:AppendRuntimeIdentifierToOutputPath={appendRidToOutputPath}";
            }

            return RunDotnet(workingDirectory, $"msbuild {file} {args}", expectSuccess);
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
            KillDotnetExe(TestDotnetCli, _cliDirectory.Path, _templateDirectory.WorkingDirectory);
            _cliDirectory.Dispose();
            _templateDirectory.Dispose();
        }

        private static void KillDotnetExe(string pathToDotnetExe, params string[] workingDirectories)
        {
            foreach (Process process in Process.GetProcessesByName("dotnet"))
            {
                try
                {
                    if (string.Equals(process.MainModule.FileName, pathToDotnetExe, StringComparison.OrdinalIgnoreCase))
                    {
                        process.Kill();
                    }
                }
                catch (Exception)
                {
                }
            }

            foreach (Process process in Process.GetProcesses())
            {
                try
                {
                    if (workingDirectories.Any(i => process.MainModule.FileName.StartsWith(i, StringComparison.OrdinalIgnoreCase)))
                    {
                        process.Kill();
                    }
                }
                catch
                {
                }
            }
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
