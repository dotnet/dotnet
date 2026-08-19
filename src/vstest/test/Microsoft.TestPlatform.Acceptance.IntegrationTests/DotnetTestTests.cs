// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
public class DotnetTestTests : AcceptanceTestBase
{
    private static string GetFinalVersion(string version)
    {
        var end = version.IndexOf("-release", StringComparison.Ordinal);
        return (end >= 0) ? version.Substring(0, end) : version;
    }

    [TestMethod]
    [TestMatrix(console: Net, testHost: Net)]
    [TestCategory("Smoke")]
    public void RunDotnetTestWithCsproj(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var projectPath = GetIsolatedTestAsset("SimpleTestProject.csproj", runnerInfo.TargetFramework);
        InvokeDotnetTest($@"{projectPath} -tl:off /p:VSTestNoLogo=false --logger:""Console;Verbosity=normal"" /p:PackageVersion={IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion}", workingDirectory: Path.GetDirectoryName(projectPath));

        // ensure our dev version is used
        StdOutputContains(GetFinalVersion(IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion));
        ValidateSummaryStatus(1, 1, 1);
        ExitCodeEquals(1);
    }

    [TestMethod]
    [TestMatrix(console: Net, testHost: Net)]
    public void RunDotnetTestWithDll(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPath = GetAssetFullPath("SimpleTestProject.dll");
        InvokeDotnetTest($@"{assemblyPath} --logger:""Console;Verbosity=normal""", workingDirectory: Path.GetDirectoryName(assemblyPath));

        // ensure our dev version is used
        StdOutputContains(GetFinalVersion(IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion));
        ValidateSummaryStatus(1, 1, 1);
        ExitCodeEquals(1);
    }

    [TestMethod]
    [TestMatrix(console: Net, testHost: Net)]
    public void RunDotnetTestWithCsprojPassInlineSettings(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var projectPath = GetIsolatedTestAsset("ParametrizedTestProject.csproj", runnerInfo.TargetFramework);
        InvokeDotnetTest($@"{projectPath} --logger:""Console;Verbosity=normal"" -tl:off /p:VSTestNoLogo=false /p:PackageVersion={IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion} -- TestRunParameters.Parameter(name =\""weburl\"", value=\""http://localhost//def\"")", workingDirectory: Path.GetDirectoryName(projectPath));

        // ensure our dev version is used
        StdOutputContains(GetFinalVersion(IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion));
        ValidateSummaryStatus(1, 0, 0);
        ExitCodeEquals(0);
    }

    [TestMethod]
    [TestMatrix(console: Net, testHost: Net)]
    public void RunDotnetTestWithDllPassInlineSettings(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPath = GetAssetFullPath("ParametrizedTestProject.dll");
        InvokeDotnetTest($@"{assemblyPath} --logger:""Console;Verbosity=normal"" -- TestRunParameters.Parameter(name=\""weburl\"", value=\""http://localhost//def\"")", workingDirectory: Path.GetDirectoryName(assemblyPath));

        ValidateSummaryStatus(1, 0, 0);
        ExitCodeEquals(0);
    }

    [TestMethod]
    [TestMatrix(console: Net, testHost: Net)]
    [Ignore("TODO: This scenario is broken in real environment as well (running with shipped `dotnet test`. Old tests (before arcade) use location of vstest.console that have more dlls in place than what we ship, and they make it work.")]
    public void RunDotnetTestWithNativeDll(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        string assemblyRelativePath = @"microsoft.testplatform.testasset.nativecpp\2.0.0\contentFiles\any\any\x64\Microsoft.TestPlatform.TestAsset.NativeCPP.dll";
        var assemblyAbsolutePath = Path.Combine(_testEnvironment.GlobalPackageDirectory, assemblyRelativePath);

        InvokeDotnetTest($@"{assemblyAbsolutePath} --logger:""Console;Verbosity=normal""", workingDirectory: Path.GetDirectoryName(assemblyAbsolutePath));

        ValidateSummaryStatus(1, 1, 0);
        ExitCodeEquals(1);
    }

    [TestMethod]
    [TestMatrix(console: Net, testHost: Net)]
    public void RunDotnetTestAndSeeOutputFromConsoleWriteLine(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPath = GetAssetFullPath("OutputtingTestProject.dll");
        InvokeDotnetTest($@"{assemblyPath} --logger:""Console;Verbosity=normal"" ", workingDirectory: Path.GetDirectoryName(assemblyPath));

        StdOutputContains("MY OUTPUT FROM TEST");

        ValidateSummaryStatus(1, 0, 0);
        ExitCodeEquals(0);
    }

    [TestMethod]
    [TestMatrix(console: Net, testHost: Net)]
    public void RunDotnetTestShouldRespectLoggerVerbosityFromRunSettings(RunnerInfo runnerInfo)
    {
        // Regression test for https://github.com/microsoft/vstest/issues/10369
        // When a .runsettings file configures the console logger with Verbosity=normal,
        // that verbosity must be respected, not silently overridden to minimal by the
        // MSBuild task injecting --logger:Console;Verbosity=minimal.
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var projectPath = GetIsolatedTestAsset("SimpleTestProject.csproj", runnerInfo.TargetFramework);
        var runsettingsPath = Path.Combine(TempDirectory.Path, "logger-verbosity.runsettings");
        File.WriteAllText(runsettingsPath, """
            <?xml version="1.0" encoding="utf-8"?>
            <RunSettings>
              <RunConfiguration>
                <MaxCpuCount>1</MaxCpuCount>
              </RunConfiguration>
              <LoggerRunSettings>
                <Loggers>
                  <Logger friendlyName="console" enabled="True">
                    <Configuration>
                      <Verbosity>normal</Verbosity>
                    </Configuration>
                  </Logger>
                </Loggers>
              </LoggerRunSettings>
            </RunSettings>
            """);

        InvokeDotnetTest(
            $@"""{projectPath}"" --settings ""{runsettingsPath}"" -tl:off /p:VSTestNoLogo=false /p:VSTestUseMSBuildOutput=false /p:PackageVersion={IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion}",
            workingDirectory: Path.GetDirectoryName(projectPath));

        // ensure our dev version is used
        StdOutputContains(GetFinalVersion(IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion));

        // At normal verbosity the console logger prints individual skipped test names.
        // Assert only on the name because Unix may insert ANSI color sequences around the
        // localized result indicator. At minimal verbosity the skipped test name is absent.
        StdOutputContains("SkippingTest");
        ExitCodeEquals(1);
    }
}
