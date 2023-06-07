// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

using Microsoft.TestPlatform.VsTestConsole.TranslationLayer.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests.TranslationLayerTests;

[TestClass]
// We need to dogfood the package built in this repo *-dev and we pack tha tp only on windows
[TestCategory("Windows-Review")]
public class TargetFrameworkTestHostDemultiplexer : AcceptanceTestBase
{
    private IVsTestConsoleWrapper? _vstestConsoleWrapper;
    private RunEventHandler? _runEventHandler;
    private DiscoveryEventHandler? _discoveryEventHandler;

    [MemberNotNull(nameof(_vstestConsoleWrapper), nameof(_runEventHandler), nameof(_discoveryEventHandler))]
    private void Setup(Dictionary<string, string?>? environmentVariables = null)
    {
        _vstestConsoleWrapper = GetVsTestConsoleWrapper(environmentVariables);
        _discoveryEventHandler = new DiscoveryEventHandler();
        _runEventHandler = new RunEventHandler();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _vstestConsoleWrapper?.EndSession();
    }

    [TestMethod]
    [NetCoreTargetFrameworkDataSource]
    [NetFullTargetFrameworkDataSource]
    public void ExecuteContainerInMultiHost(RunnerInfo runnerInfo)
        => ExecuteContainerInMultiHost(runnerInfo, 3);

    [TestMethod]
    [NetCoreTargetFrameworkDataSource]
    [NetFullTargetFrameworkDataSource]
    public void ExecuteContainerInMultiHost_MoreHostsThanTests(RunnerInfo runnerInfo)
        => ExecuteContainerInMultiHost(runnerInfo, 20);

    [TestMethod]
    [NetCoreTargetFrameworkDataSource]
    [NetFullTargetFrameworkDataSource]
    public void ExecuteSingleContainerInDefaultSingleHost(RunnerInfo runnerInfo)
        => ExecuteContainerInMultiHost(runnerInfo, -1);

    private void ExecuteContainerInMultiHost(RunnerInfo runnerInfo, int expectedHost)
    {
        // Arrange
        SetTestEnvironment(_testEnvironment, runnerInfo);
        Dictionary<string, string?>? environmentVariables = new() { { "VSTEST_LOGFOLDER", TempDirectory.Path } };
        Setup(environmentVariables);
        string runsettings = $"""
<RunSettings>
    <RunConfiguration>
        <MaxCpuCount>{expectedHost}</MaxCpuCount>
        <TargetFrameworkTestHostDemultiplexer>{expectedHost}</TargetFrameworkTestHostDemultiplexer>
    </RunConfiguration>
</RunSettings>
""";

        // Act
        var testDll = GetAssetFullPath("MultiHostTestExecutionProject.dll");
        _vstestConsoleWrapper.DiscoverTests(new string[] { testDll }, GetDefaultRunSettings(), _discoveryEventHandler);
        _vstestConsoleWrapper.RunTests(_discoveryEventHandler.DiscoveredTestCases, expectedHost == -1 ? GetDefaultRunSettings() : runsettings, _runEventHandler);
        _runEventHandler.EnsureSuccess();

        // Assert
        Assert.AreEqual(10, _discoveryEventHandler.DiscoveredTestCases.Count);
        int failedTests = _runEventHandler.TestResults.Count(x => x.Outcome == TestOutcome.Failed);
        Assert.IsFalse(failedTests > 0, $"Number of failed tests {failedTests}");

        string[] hosts = Directory.GetFiles(TempDirectory.Path, "TestHost*");
        Assert.AreEqual(expectedHost == -1 ? 1 : expectedHost > 10 ? 10 : expectedHost, hosts.Length);

        List<string> tests = new();
        int testsRunInsideHost;
        foreach (var file in hosts)
        {
            testsRunInsideHost = 0;

            using StreamReader streamReader = new(file);
            while (!streamReader.EndOfStream)
            {
                string? line = streamReader.ReadLine();
                if (!string.IsNullOrEmpty(line))
                {
                    tests.Add(line);
                }
                testsRunInsideHost++;
            }


            if (expectedHost == 3)
            {
                Assert.IsTrue(testsRunInsideHost >= 3);
            }
        }

        Assert.AreEqual(10, tests.Count);
        for (int i = 1; i <= 10; i++)
        {
            tests.Remove($"TestMethod{i}");
        }
        Assert.AreEqual(0, tests.Count);
    }
}
