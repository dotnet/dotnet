// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

using Microsoft.TestPlatform.Library.IntegrationTests.TranslationLayerTests.EventHandler;
using Microsoft.TestPlatform.TestUtilities;
using Microsoft.TestPlatform.VsTestConsole.TranslationLayer.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.Library.IntegrationTests.TranslationLayerTests;

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
        Dictionary<string, string?>? environmentVariables = new()
        {
            // Times out sometimes in CI with the 5 second timeout we normally use to get faster failure feedback.
            // Probably because we start many testhosts here and the server is slow and busy.
            [EnvironmentHelper.VstestConnectionTimeout] = "90",
            ["VSTEST_LOGFOLDER"] = TempDirectory.Path,
        };
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
        Assert.HasCount(10, _discoveryEventHandler.DiscoveredTestCases);
        int failedTests = _runEventHandler.TestResults.Count(x => x.Outcome == TestOutcome.Failed);
        Assert.IsLessThanOrEqualTo(0, failedTests, $"Number of failed tests {failedTests}");

        string[] hosts = Directory.GetFiles(TempDirectory.Path, "TestHost*");
        Assert.HasCount(expectedHost == -1 ? 1 : expectedHost > 10 ? 10 : expectedHost, hosts);

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
                Assert.IsGreaterThanOrEqualTo(3, testsRunInsideHost);
            }
        }

        Assert.HasCount(10, tests);
        for (int i = 1; i <= 10; i++)
        {
            tests.Remove($"TestMethod{i}");
        }
        Assert.IsEmpty(tests);
    }
}
