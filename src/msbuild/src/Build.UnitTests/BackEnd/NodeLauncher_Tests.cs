// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using Microsoft.Build.BackEnd;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using Microsoft.Build.UnitTests;
using Microsoft.Build.UnitTests.Shared;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

#nullable disable

namespace Microsoft.Build.UnitTests.BackEnd
{
    public class NodeLauncher_Tests : IDisposable
    {
        private const string s_msbuildArgs = "/noautoresponse /nologo /nodemode:1 /nodeReuse:true /low:false";

        /// <summary>
        /// Path to the bootstrapped MSBuild native app host (e.g. MSBuild.exe on Windows, MSBuild on Linux).
        /// </summary>
        private static readonly string s_appHostPath = Path.Combine(
            RunnerUtilities.BootstrapMsBuildBinaryLocation,
            "sdk",
            RunnerUtilities.BootstrapSdkVersion,
            Constants.MSBuildExecutableName);

        /// <summary>
        /// Path to the bootstrapped MSBuild managed assembly (MSBuild.dll).
        /// </summary>
        private static readonly string s_msbuildDllPath = Path.ChangeExtension(s_appHostPath, ".dll");

        private readonly MockLogger _logger;
        private readonly BuildManager _buildManager;
        private readonly TestEnvironment _env;

        public NodeLauncher_Tests(ITestOutputHelper output)
        {
            BuildManager.DefaultBuildManager.Dispose();
            _logger = new MockLogger(output);
            _buildManager = new BuildManager();
            _env = TestEnvironment.Create(output);
        }

        [WindowsOnlyFact]

        public void BuildCommandLineArgs_NativeAppHost_PrependsExeAsArgv0()
        {
            string result = NodeLauncher.BuildWindowsCommandLineArgs(
                new NodeLaunchData(s_appHostPath, s_msbuildArgs),
                hostExeName: s_appHostPath,
                isNativeAppHost: true);

            // CreateProcess expects argv[0] as the first token, no dotnet host prefix.
            result.ShouldBe($"\"{s_appHostPath}\" {s_msbuildArgs}");
        }

        [WindowsOnlyFact]
        public void BuildCommandLineArgs_DotnetHosted_PrependsDotnetAndAssembly()
        {
            string dotnetExe = Path.Combine(RunnerUtilities.BootstrapMsBuildBinaryLocation, Constants.DotnetProcessName);

            string result = NodeLauncher.BuildWindowsCommandLineArgs(
                new NodeLaunchData(s_msbuildDllPath, s_msbuildArgs),
                hostExeName: dotnetExe,
                isNativeAppHost: false);

            // CreateProcess: dotnet.exe as argv[0], then MSBuild.dll, then actual args.
            result.ShouldBe($"\"{dotnetExe}\" \"{s_msbuildDllPath}\" {s_msbuildArgs}");
        }

        [UnixOnlyFact]
        public void BuildCommandLineArgs_NativeAppHost_DoesNotDuplicateExePath()
        {
            string result = NodeLauncher.BuildUnixCommandLineArgs(
                new NodeLaunchData(s_appHostPath, s_msbuildArgs),
                isNativeAppHost: true);

            // On Unix, Process.Start sets argv[0] from FileName, so Arguments must NOT
            // include the executable path — that would create a duplicate argv[1].
            result.ShouldBe(s_msbuildArgs);
        }

        [UnixOnlyFact]
        public void BuildCommandLineArgs_DotnetHosted_IncludesAssemblyPath()
        {
            string result = NodeLauncher.BuildUnixCommandLineArgs(
                new NodeLaunchData(s_msbuildDllPath, s_msbuildArgs),
                isNativeAppHost: false);

            // dotnet host needs the assembly path to know which assembly to run.
            result.ShouldBe($"\"{s_msbuildDllPath}\" {s_msbuildArgs}");
        }

        /// <summary>
        /// Verifies that out-of-proc node launching produces a well-formed command line with
        /// the expected arguments and no duplicate executable path.
        /// </summary>
        [Fact]
        public void OutOfProcBuild_CommandLineIsWellFormed()
        {
            string cmdLine = GetOutOfProcNodeCommandLine(enableNodeReuse: false);

            cmdLine.ShouldContain("/noautoresponse");
            cmdLine.ShouldContain("/nologo");
            cmdLine.ShouldContain("/nodemode:1");
            cmdLine.ShouldContain("/nodeReuse:false");

            string msbuildToken = cmdLine.Split(' ')[0];
            int occurrences = cmdLine.Split(' ').Count(t => t == msbuildToken);
            occurrences.ShouldBe(1, $"MSBuild path '{msbuildToken}' should appear once in command line, but found {occurrences} times. Full command line: {cmdLine}");
        }

        /// <summary>
        /// Verifies that NodeLauncher correctly propagates the EnableNodeReuse setting
        /// into the child node's command line arguments.
        /// </summary>
        [Fact]
        public void OutOfProcBuild_NodeReuseFlagIsRespected()
        {
            string cmdLine = GetOutOfProcNodeCommandLine(enableNodeReuse: true);

            cmdLine.ShouldContain("/nodeReuse:true");
            cmdLine.ShouldContain("/nodemode:1");
        }

        private string GetOutOfProcNodeCommandLine(bool enableNodeReuse)
        {
            _env.SetEnvironmentVariable("MSBUILDENABLEALLPROPERTYFUNCTIONS", "1");

            string contents = """
                <Project>
                    <Target Name="Build">
                        <Message Text="CMDLINE: $([System.Environment]::CommandLine)" Importance="High" />
                    </Target>
                </Project>
                """.Cleanup();

            var projectInstance = new ProjectInstance(ObjectModelHelpers.CreateFileInTempProjectDirectory("nodelaunch.proj", contents));
            var data = new BuildRequestData(projectInstance, ["Build"]);

            var parameters = new BuildParameters
            {
                DisableInProcNode = true,
                EnableNodeReuse = enableNodeReuse,
                Loggers = [_logger],
            };

            BuildResult result = _buildManager.Build(parameters, data);
            result.OverallResult.ShouldBe(BuildResultCode.Success);

            string cmdLineMessage = _logger.BuildMessageEvents
                .Select(e => e.Message)
                .First(m => m.StartsWith("CMDLINE: "));
            return cmdLineMessage.Substring("CMDLINE: ".Length);
        }

        public void Dispose()
        {
            try
            {
                _buildManager.Dispose();
            }
            finally
            {
                _env.Dispose();
            }
        }
    }
}
