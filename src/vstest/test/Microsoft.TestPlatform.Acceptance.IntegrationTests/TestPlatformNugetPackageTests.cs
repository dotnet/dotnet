// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
public class TestPlatformNugetPackageTests : CodeCoverageAcceptanceTestBase
{
    private static string s_nugetPackageFolder = string.Empty;

    [ClassInitialize]
    public static void ClassInit(TestContext _)
    {
        s_nugetPackageFolder = Path.Combine(IntegrationTestEnvironment.PublishDirectory, $"Microsoft.TestPlatform.{IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion}.nupkg");
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    [TestMatrix(console: NetFx, testHost: NetFx)]
    [TestMatrix(console: NetFx, testHost: Net)]
    public void RunMultipleTestAssembliesWithCodeCoverage(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPaths = BuildMultipleAssemblyPath("SimpleTestProject.dll", "SimpleTestProject2.dll");

        var arguments = CreateCodeCoverageArguments(runnerInfo, assemblyPaths, out var trxFilePath);
        InvokeVsTest(arguments);

        ExitCodeEquals(1); // failing tests

        var actualCoverageFile = GetCoverageFileNameFromTrx(trxFilePath, TempDirectory.Path);
        Console.WriteLine($@"Coverage file: {actualCoverageFile}  Results directory: {TempDirectory} trxfile: {trxFilePath}");
        Assert.IsTrue(File.Exists(actualCoverageFile), "Coverage file not found: {0}", actualCoverageFile);
    }

    public override string GetConsoleRunnerPath()
    {
        string consoleRunnerPath = string.Empty;

        if (IsDesktopRunner())
        {
            consoleRunnerPath = Path.Combine(s_nugetPackageFolder, "tools", Net462TargetFramework, "Common7", "IDE", "Extensions", "TestPlatform", "vstest.console.exe");
        }

        Assert.IsTrue(File.Exists(consoleRunnerPath), "GetConsoleRunnerPath: Path not found: \"{0}\"", consoleRunnerPath);
        return consoleRunnerPath;
    }

    private string CreateCodeCoverageArguments(
        RunnerInfo runnerInfo,
        string assemblyPaths,
        out string trxFilePath)
    {
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty,
            FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);

        arguments = string.Concat(arguments, $" /EnableCodeCoverage");

        trxFilePath = Path.Combine(TempDirectory.Path, Guid.NewGuid() + ".trx");
        arguments = string.Concat(arguments, " /logger:trx;logfilename=" + trxFilePath);

        return arguments;
    }
}
