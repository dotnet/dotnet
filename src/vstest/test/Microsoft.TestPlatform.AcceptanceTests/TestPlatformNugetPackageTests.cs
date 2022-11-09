// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

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
        var packageLocation = Path.Combine(IntegrationTestEnvironment.TestPlatformRootDirectory, "artifacts", IntegrationTestEnvironment.BuildConfiguration, "packages");
        var nugetPackage = Directory.EnumerateFiles(packageLocation, "Microsoft.TestPlatform.*.nupkg").OrderBy(a => a).First();
        s_nugetPackageFolder = Path.Combine(new TempDirectory().Path, Path.GetFileNameWithoutExtension(nugetPackage)!);
        ZipFile.ExtractToDirectory(nugetPackage, s_nugetPackageFolder);

        TryMoveDirectory(
            sourceDirName: Path.Combine(s_nugetPackageFolder, "tools", "net462", "Team%20Tools"),
            destDirName: Path.Combine(s_nugetPackageFolder, "tools", "net462", "Team Tools")
        );

        TryMoveDirectory(
            sourceDirName: Path.Combine(s_nugetPackageFolder, "tools", "net462", "Team Tools", "Dynamic%20Code%20Coverage%20Tools"),
            destDirName: Path.Combine(s_nugetPackageFolder, "tools", "net462", "Team Tools", "Dynamic Code Coverage Tools")
        );
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        Directory.Delete(s_nugetPackageFolder, true);
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    [NetFullTargetFrameworkDataSourceAttribute(useCoreRunner: false)]
    [NetCoreTargetFrameworkDataSourceAttribute(useCoreRunner: false)]
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
        string diagFileName = Path.Combine(TempDirectory.Path, "diaglog.txt");

        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty,
            FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);

        arguments = string.Concat(arguments, $" /Diag:{diagFileName}", $" /EnableCodeCoverage");

        trxFilePath = Path.Combine(TempDirectory.Path, Guid.NewGuid() + ".trx");
        arguments = string.Concat(arguments, " /logger:trx;logfilename=" + trxFilePath);

        return arguments;
    }

    private static void TryMoveDirectory(string sourceDirName, string destDirName)
    {
        if (Directory.Exists(sourceDirName))
        {
            Directory.Move(sourceDirName, destDirName);
        }
    }
}
