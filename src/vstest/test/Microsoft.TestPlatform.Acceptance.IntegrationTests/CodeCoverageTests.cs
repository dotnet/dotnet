// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.IO;
using System.Linq;

using Microsoft.CodeCoverage.Core;
using Microsoft.CodeCoverage.Core.Reports.Coverage;
using Microsoft.TestPlatform.TestUtilities;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

internal struct TestParameters
{
    public enum SettingsType
    {
        None = 0,
        Default = 1,
        Custom = 2,
        XmlOutput = 3,
        CoberturaOutput = 4
    }

    public string AssemblyName { get; set; }

    public string TargetPlatform { get; set; }

    public SettingsType RunSettingsType { get; set; }

    public string RunSettingsPath { get; set; }

    public int ExpectedPassedTests { get; set; }

    public int ExpectedSkippedTests { get; set; }

    public int ExpectedFailedTests { get; set; }

    public bool CheckSkipped { get; set; }
}

[TestClass]
//Code coverage only supported on windows (based on the message in output)
[TestCategory("Windows-Review")]
public class CodeCoverageTests : CodeCoverageAcceptanceTestBase
{
    [TestMethod]
    [NetFullTargetFrameworkDataSource(useDesktopRunner: false)]
    [NetCoreTargetFrameworkDataSource(useDesktopRunner: false)]
    public void CollectCodeCoverageWithCollectOptionForx86(RunnerInfo runnerInfo)
    {
        var parameters = new TestParameters()
        {
            AssemblyName = "SimpleTestProject.dll",
            TargetPlatform = "x86",
            RunSettingsPath = string.Empty,
            RunSettingsType = TestParameters.SettingsType.None,
            ExpectedPassedTests = 1,
            ExpectedSkippedTests = 1,
            ExpectedFailedTests = 1
        };

        CollectCodeCoverage(runnerInfo, parameters);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource(useDesktopRunner: false)]
    [NetCoreTargetFrameworkDataSource(useDesktopRunner: false)]
    public void CollectCodeCoverageWithCollectOptionForx64(RunnerInfo runnerInfo)
    {
        var parameters = new TestParameters()
        {
            AssemblyName = "SimpleTestProject.dll",
            TargetPlatform = "x64",
            RunSettingsPath = string.Empty,
            RunSettingsType = TestParameters.SettingsType.None,
            ExpectedPassedTests = 1,
            ExpectedSkippedTests = 1,
            ExpectedFailedTests = 1
        };

        CollectCodeCoverage(runnerInfo, parameters);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource(useDesktopRunner: false)]
    [NetCoreTargetFrameworkDataSource(useDesktopRunner: false)]
    public void CollectCodeCoverageX86WithRunSettings(RunnerInfo runnerInfo)
    {
        var parameters = new TestParameters()
        {
            AssemblyName = "SimpleTestProject.dll",
            TargetPlatform = "x86",
            RunSettingsPath = string.Empty,
            RunSettingsType = TestParameters.SettingsType.Default,
            ExpectedPassedTests = 1,
            ExpectedSkippedTests = 1,
            ExpectedFailedTests = 1
        };

        CollectCodeCoverage(runnerInfo, parameters);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource(useDesktopRunner: false)]
    [NetCoreTargetFrameworkDataSource(useDesktopRunner: false)]
    public void CollectCodeCoverageX64WithRunSettings(RunnerInfo runnerInfo)
    {
        var parameters = new TestParameters()
        {
            AssemblyName = "SimpleTestProject.dll",
            TargetPlatform = "x64",
            RunSettingsPath = string.Empty,
            RunSettingsType = TestParameters.SettingsType.Default,
            ExpectedPassedTests = 1,
            ExpectedSkippedTests = 1,
            ExpectedFailedTests = 1
        };

        CollectCodeCoverage(runnerInfo, parameters);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource(useDesktopRunner: false)]
    [NetCoreTargetFrameworkDataSource(useDesktopRunner: false)]
    public void CodeCoverageShouldAvoidExclusionsX86(RunnerInfo runnerInfo)
    {
        var parameters = new TestParameters()
        {
            AssemblyName = "CodeCoverageTest.dll",
            TargetPlatform = "x86",
            RunSettingsPath = Path.Combine(
                IntegrationTestEnvironment.RepoRootDirectory,
                @"scripts", "vstest-codecoverage2.runsettings"),
            RunSettingsType = TestParameters.SettingsType.Custom,
            ExpectedPassedTests = 3,
            ExpectedSkippedTests = 0,
            ExpectedFailedTests = 0,
            CheckSkipped = true
        };

        CollectCodeCoverage(runnerInfo, parameters);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource(useDesktopRunner: false)]
    [NetCoreTargetFrameworkDataSource(useDesktopRunner: false)]
    public void CodeCoverageShouldAvoidExclusionsX64(RunnerInfo runnerInfo)
    {
        var parameters = new TestParameters()
        {
            AssemblyName = "CodeCoverageTest.dll",
            TargetPlatform = "x64",
            RunSettingsPath = Path.Combine(
                IntegrationTestEnvironment.RepoRootDirectory,
                @"scripts", "vstest-codecoverage2.runsettings"),
            RunSettingsType = TestParameters.SettingsType.Custom,
            ExpectedPassedTests = 3,
            ExpectedSkippedTests = 0,
            ExpectedFailedTests = 0,
            CheckSkipped = true
        };

        CollectCodeCoverage(runnerInfo, parameters);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource(useDesktopRunner: false)]
    [NetCoreTargetFrameworkDataSource(useDesktopRunner: false)]
    public void CollectCodeCoverageSpecifyOutputFormatXml(RunnerInfo runnerInfo)
    {
        var parameters = new TestParameters()
        {
            AssemblyName = "SimpleTestProject.dll",
            TargetPlatform = "x64",
            RunSettingsPath = string.Empty,
            RunSettingsType = TestParameters.SettingsType.XmlOutput,
            ExpectedPassedTests = 1,
            ExpectedSkippedTests = 1,
            ExpectedFailedTests = 1
        };

        CollectCodeCoverage(runnerInfo, parameters);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource(useDesktopRunner: false)]
    [NetCoreTargetFrameworkDataSource(useDesktopRunner: false)]
    public void CollectCodeCoverageSpecifyOutputFormatCoberturaOverrideRunSettingsConfiguration(RunnerInfo runnerInfo)
    {
        var parameters = new TestParameters()
        {
            AssemblyName = "SimpleTestProject.dll",
            TargetPlatform = "x64",
            RunSettingsPath = Path.Combine(
                IntegrationTestEnvironment.RepoRootDirectory,
                @"scripts", "vstest-codecoverage2.runsettings"),
            RunSettingsType = TestParameters.SettingsType.CoberturaOutput,
            ExpectedPassedTests = 1,
            ExpectedSkippedTests = 1,
            ExpectedFailedTests = 1
        };

        CollectCodeCoverage(runnerInfo, parameters);
    }

    private void CollectCodeCoverage(RunnerInfo runnerInfo, TestParameters testParameters)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var arguments = CreateArguments(TempDirectory, runnerInfo, testParameters, out var trxFilePath);

        InvokeVsTest(arguments);

        ValidateSummaryStatus(
            testParameters.ExpectedPassedTests,
            testParameters.ExpectedSkippedTests,
            testParameters.ExpectedFailedTests);

        var actualCoverageFile = GetCoverageFileNameFromTrx(trxFilePath, TempDirectory.Path);
        Console.WriteLine($@"Coverage file: {actualCoverageFile}  Results directory: {TempDirectory.Path} trxfile: {trxFilePath}");
        Assert.IsTrue(File.Exists(actualCoverageFile), "Coverage file not found: {0}", actualCoverageFile);

        if (testParameters.RunSettingsType == TestParameters.SettingsType.XmlOutput)
        {
            Assert.IsTrue(actualCoverageFile.EndsWith(".xml", StringComparison.InvariantCultureIgnoreCase));
        }
        else if (testParameters.RunSettingsType == TestParameters.SettingsType.CoberturaOutput)
        {
            Assert.IsTrue(actualCoverageFile.EndsWith(".cobertura.xml", StringComparison.InvariantCultureIgnoreCase));
        }
        else
        {
            Assert.IsTrue(actualCoverageFile.EndsWith(".coverage", StringComparison.InvariantCultureIgnoreCase));
        }

        var coverageReport = GetCoverageReport(actualCoverageFile);
        if (testParameters.CheckSkipped)
        {
            AssertSkippedMethod(coverageReport);
        }

        ValidateCoverageData(coverageReport, testParameters.AssemblyName, testParameters.RunSettingsType != TestParameters.SettingsType.CoberturaOutput);
    }

    private string CreateArguments(
        TempDirectory tempDirectory,
        RunnerInfo runnerInfo,
        TestParameters testParameters,
        out string trxFilePath)
    {
        var assemblyPaths = GetAssetFullPath(testParameters.AssemblyName);

        string traceDataCollectorDir = Path.Combine(IntegrationTestEnvironment.PublishDirectory,
            $"Microsoft.CodeCoverage.{IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion}.nupkg", "build", "netstandard2.0");

        string diagFileName = Path.Combine(tempDirectory.Path, "diaglog.txt");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty,
            FrameworkArgValue, runnerInfo.InIsolationValue, tempDirectory.Path);
        arguments = string.Concat(arguments, $" /Diag:{diagFileName}",
            $" /TestAdapterPath:{traceDataCollectorDir}");
        arguments = string.Concat(arguments, $" /Platform:{testParameters.TargetPlatform}");

        trxFilePath = Path.Combine(tempDirectory.Path, Guid.NewGuid() + ".trx");
        arguments = string.Concat(arguments, " /logger:trx;logfilename=" + trxFilePath);

        var defaultRunSettingsPath = Path.Combine(
            IntegrationTestEnvironment.RepoRootDirectory,
            @"scripts", "vstest-codecoverage.runsettings");

        var runSettings = string.Empty;
        switch (testParameters.RunSettingsType)
        {
            case TestParameters.SettingsType.None:
                runSettings = $" /collect:\"Code Coverage\"";
                break;
            case TestParameters.SettingsType.Default:
                runSettings = $" /settings:{defaultRunSettingsPath}";
                break;
            case TestParameters.SettingsType.Custom:
                runSettings = $" /settings:{testParameters.RunSettingsPath}";
                break;
            case TestParameters.SettingsType.XmlOutput:
                runSettings = $" /collect:\"Code Coverage;Format=Xml\"";
                if (!string.IsNullOrWhiteSpace(testParameters.RunSettingsPath))
                {
                    runSettings += $" /settings:{testParameters.RunSettingsPath}";
                }
                break;
            case TestParameters.SettingsType.CoberturaOutput:
                runSettings = $" /collect:\"Code Coverage;Format=Cobertura\"";
                if (!string.IsNullOrWhiteSpace(testParameters.RunSettingsPath))
                {
                    runSettings += $" /settings:{testParameters.RunSettingsPath}";
                }
                break;
        }

        arguments = string.Concat(arguments, runSettings);

        return arguments;
    }

    private static void AssertSkippedMethod(CoverageReport coverageReport)
    {
        var module = GetModule(coverageReport, "codecoveragetest.dll");
        Assert.IsNotNull(module);

        var coverage = double.Parse(module.BlockCoverage, CultureInfo.InvariantCulture);
        Assert.IsTrue(coverage > ExpectedMinimalModuleCoverage);

        var testSignFunction = module.SkippedFunctions.FirstOrDefault(f => f.Name.Equals("TestSign()"));
        Assert.IsNotNull(testSignFunction);
        Assert.AreEqual(FunctionSkipReason.name_excluded, testSignFunction.FunctionSkipReason);

        var skippedTestMethod = module.SkippedFunctions.FirstOrDefault(f => f.Name.Equals("__CxxPureMSILEntry_Test()"));
        Assert.IsNotNull(skippedTestMethod);
        Assert.AreEqual(FunctionSkipReason.name_excluded, skippedTestMethod.FunctionSkipReason);

        var testAbsFunction = module.Functions.FirstOrDefault(f => f.Name.Equals("TestAbs()"));
        Assert.IsNotNull(testAbsFunction);
    }

    private static void ValidateCoverageData(CoverageReport coverageReport, string moduleName, bool validateSourceFileNames)
    {
        var module = GetModule(coverageReport, moduleName.ToLowerInvariant());
        Assert.IsNotNull(module);

        AssertCoverage(module, ExpectedMinimalModuleCoverage);

        // In case of cobertura report. Cobertura report has different format.
        if (validateSourceFileNames)
        {
            AssertSourceFileName(module);
        }
    }

    private static void AssertSourceFileName(ModuleData module)
    {
        const string expectedFileName = "UnitTest1.cs";

        var found = false;

        foreach (var sourceFile in module.SourceFiles)
        {
            if (sourceFile.Path.Contains(expectedFileName))
            {
                found = true;
                break;
            }
        }

        Assert.IsTrue(found);
    }
}
