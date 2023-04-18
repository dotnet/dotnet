// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.TestPlatform.VsTestConsole.TranslationLayer.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.TestPlatform.AcceptanceTests.TranslationLayerTests;

[TestClass]
public class DiscoverTests : AcceptanceTestBase
{
    private IVsTestConsoleWrapper? _vstestConsoleWrapper;
    private DiscoveryEventHandler? _discoveryEventHandler;
    private DiscoveryEventHandler2? _discoveryEventHandler2;

    [MemberNotNull(nameof(_vstestConsoleWrapper), nameof(_discoveryEventHandler), nameof(_discoveryEventHandler2))]
    public void Setup()
    {
        _vstestConsoleWrapper = GetVsTestConsoleWrapper();
        _discoveryEventHandler = new DiscoveryEventHandler();
        _discoveryEventHandler2 = new DiscoveryEventHandler2();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _vstestConsoleWrapper?.EndSession();
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    [RunnerCompatibilityDataSource]
    public void DiscoverTestsUsingDiscoveryEventHandler1(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        // Setup();
        _discoveryEventHandler = new DiscoveryEventHandler();
        _discoveryEventHandler2 = new DiscoveryEventHandler2();

        var vstestConsoleWrapper = GetVsTestConsoleWrapper();
        vstestConsoleWrapper.DiscoverTests(GetTestDlls("MSTestProject1.dll", "MSTestProject2.dll"), GetDefaultRunSettings(), _discoveryEventHandler);

        // Assert.
        Assert.AreEqual(6, _discoveryEventHandler.DiscoveredTestCases.Count);
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    [RunnerCompatibilityDataSource]
    public void DiscoverTestsUsingDiscoveryEventHandler2AndTelemetryOptedOut(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        // Setup();

        _discoveryEventHandler = new DiscoveryEventHandler();
        _discoveryEventHandler2 = new DiscoveryEventHandler2();

        var vstestConsoleWrapper = GetVsTestConsoleWrapper();
        vstestConsoleWrapper.DiscoverTests(
            GetTestDlls("MSTestProject1.dll", "MSTestProject2.dll"),
            GetDefaultRunSettings(),
            new TestPlatformOptions() { CollectMetrics = false },
            _discoveryEventHandler2);

        // Assert.
        Assert.AreEqual(6, _discoveryEventHandler2.DiscoveredTestCases.Count);
        Assert.AreEqual(0, _discoveryEventHandler2.Metrics!.Count);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void DiscoverTestsUsingDiscoveryEventHandler2AndTelemetryOptedIn(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        Setup();

        _vstestConsoleWrapper.DiscoverTests(GetTestAssemblies(), GetDefaultRunSettings(), new TestPlatformOptions() { CollectMetrics = true }, _discoveryEventHandler2);

        // Assert.
        Assert.AreEqual(6, _discoveryEventHandler2.DiscoveredTestCases.Count);
        Assert.IsTrue(_discoveryEventHandler2.Metrics!.ContainsKey(TelemetryDataConstants.TargetDevice));
        Assert.IsTrue(_discoveryEventHandler2.Metrics.ContainsKey(TelemetryDataConstants.NumberOfAdapterUsedToDiscoverTests));
        Assert.IsTrue(_discoveryEventHandler2.Metrics.ContainsKey(TelemetryDataConstants.TimeTakenInSecByAllAdapters));
        Assert.IsTrue(_discoveryEventHandler2.Metrics.ContainsKey(TelemetryDataConstants.TimeTakenInSecForDiscovery));
        Assert.IsTrue(_discoveryEventHandler2.Metrics.ContainsKey(TelemetryDataConstants.DiscoveryState));
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void DiscoverTestsUsingEventHandler2AndBatchSize(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        Setup();

        var discoveryEventHandlerForBatchSize = new DiscoveryEventHandlerForBatchSize();
        var batchSize = 2;
        string runSettingsXml = $@"<?xml version=""1.0"" encoding=""utf-8""?>
                                    <RunSettings>
                                        <RunConfiguration>
                                        <BatchSize>{batchSize}</BatchSize>
                                        </RunConfiguration>
                                    </RunSettings>";

        _vstestConsoleWrapper.DiscoverTests(
            GetTestAssemblies(),
            runSettingsXml,
            null,
            discoveryEventHandlerForBatchSize);

        // Assert.
        discoveryEventHandlerForBatchSize.DiscoveredTestCases.Should().HaveCount(6, "we found 6 tests in total");
        // Batching happens based on size and time interva. The middle batch should almost always be 2,
        // if the discovery is fast enough, but the only requirement we can reliably check and enforce is that no batch is bigger than the expected size.
        discoveryEventHandlerForBatchSize.Batches.Should().OnlyContain(v => v <= batchSize, "all batches should be the same size or smaller than the batch size");
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void DiscoverTestsUsingEventHandler1AndBatchSize(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        Setup();

        var discoveryEventHandlerForBatchSize = new DiscoveryEventHandlerForBatchSize();
        var batchSize = 2;
        string runSettingsXml = $@"<?xml version=""1.0"" encoding=""utf-8""?>
                                    <RunSettings>
                                        <RunConfiguration>
                                        <BatchSize>{batchSize}</BatchSize>
                                        </RunConfiguration>
                                    </RunSettings>";

        _vstestConsoleWrapper.DiscoverTests(
            GetTestAssemblies(),
            runSettingsXml,
            discoveryEventHandlerForBatchSize);

        // Assert.
        discoveryEventHandlerForBatchSize.DiscoveredTestCases.Should().HaveCount(6, "we found 6 tests in total");
        // Batching happens based on size and time interva. The middle batch should almost always be 2,
        // if the discovery is fast enough, but the only requirement we can reliably check and enforce is that no batch is bigger than the expected size.
        discoveryEventHandlerForBatchSize.Batches.Should().OnlyContain(v => v <= batchSize, "all batches should be the same size or smaller than the batch size");
    }

    [TestMethod]
    [NetCoreTargetFrameworkDataSource]
    [NetFullTargetFrameworkDataSource]
    public void DiscoverTestUsingEventHandler2ShouldContainAllSourcesAsFullyDiscovered(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        Setup();

        var eventHandler2 = new DiscoveryEventHandler2();

        _vstestConsoleWrapper.DiscoverTests(
            GetTestAssemblies(),
            GetDefaultRunSettings(),
            null,
            eventHandler2);

        // Assert.
        Assert.AreEqual(2, eventHandler2.FullyDiscoveredSources!.Count);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void DiscoverTestsUsingSourceNavigation(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        Setup();

        _vstestConsoleWrapper.DiscoverTests(
            GetTestAssemblies(),
            GetDefaultRunSettings(),
            _discoveryEventHandler);

        // Assert.
        var testCase =
            _discoveryEventHandler.DiscoveredTestCases.Where(dt => dt.FullyQualifiedName.Equals("SampleUnitTestProject.UnitTest1.PassingTest"));

        // Release builds optimize code, hence line numbers are different.
        if (IntegrationTestEnvironment.BuildConfiguration.StartsWith("release", StringComparison.OrdinalIgnoreCase))
        {
            Assert.AreEqual(25, testCase.First().LineNumber);
        }
        else
        {
            Assert.AreEqual(24, testCase.First().LineNumber);
        }
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource(inProcess: true)]
    [NetCoreTargetFrameworkDataSource]
    [Ignore("Flaky on CI")]
    public async Task CancelTestDiscovery(RunnerInfo runnerInfo)
    {
        var sw = Stopwatch.StartNew();
        // Setup
        var testAssemblies = new List<string>
        {
            // This is fast to discover.
            GetAssetFullPath("SimpleTestProject.dll"),
            // This is slow to discover to keep us discovering while we cancel.
            GetAssetFullPath("DiscoveryTestProject.dll"),
        };

        SetTestEnvironment(_testEnvironment, runnerInfo);
        Setup();

        var discoveredTests = new List<TestCase>();
        var discoveryEvents = new Mock<ITestDiscoveryEventsHandler>();
        var alreadyCancelled = false;
        TimeSpan cancellationCalled = TimeSpan.Zero;
        discoveryEvents.Setup(events => events.HandleDiscoveredTests(It.IsAny<IEnumerable<TestCase>>()))
            .Callback((IEnumerable<TestCase> testcases) =>
            {
                Console.WriteLine($"Received test case {testcases.Single()}");
                // As soon as we get first test call cancel. That way we know there is discovery in progress.
                discoveredTests.AddRange(testcases);
                if (!alreadyCancelled)
                {
                    cancellationCalled = sw.Elapsed;
                    // Calling cancel many times crashes. https://github.com/microsoft/vstest/issues/3526
                    alreadyCancelled = true;
                    Console.WriteLine($"Cancelling at {cancellationCalled.TotalMilliseconds} ms.");
                    _vstestConsoleWrapper.CancelDiscovery();
                }
            });
        var isTestCancelled = false;
        discoveryEvents.Setup(events => events.HandleDiscoveryComplete(It.IsAny<long>(), It.IsAny<IEnumerable<TestCase>>(), It.IsAny<bool>()))
            .Callback((long _, IEnumerable<TestCase> testcases, bool isAborted) =>
            {
                Console.WriteLine($"Discovery complete at {sw.ElapsedMilliseconds} ms, with isAborted: {isAborted}.");
                isTestCancelled = isAborted;
                if (testcases != null)
                {
                    discoveredTests.AddRange(testcases);
                }
            });

        string runSettingsXml =
             $@"<?xml version=""1.0"" encoding=""utf-8""?>
            <RunSettings>
                <RunConfiguration>
                    <TargetFrameworkVersion>{FrameworkArgValue}</TargetFrameworkVersion>
                    <BatchSize>1</BatchSize>
                </RunConfiguration>
            </RunSettings>";

        // Act
        Console.WriteLine("Starting Discovery.");
        await Task.Run(() => _vstestConsoleWrapper.DiscoverTests(testAssemblies, runSettingsXml, discoveryEvents.Object));
        Console.WriteLine("Discovery finished.");

        // Assert
        Assert.IsTrue(isTestCancelled, "Discovery was not cancelled");

        // TODO: Review how much time it takes to actually cancel. It is not 2s on CI server. Are we waiting for anything?
        //var done = sw.Elapsed;
        //var timeTillCancelled = done - cancellationCalled;
        //timeTillCancelled.Should().BeLessThan(2.Seconds());
        int discoveredSourcesCount = discoveredTests.Select(testcase => testcase.Source).Distinct().Count();
        Assert.AreNotEqual(testAssemblies.Count, discoveredSourcesCount, "All test assemblies discovered");
    }

    private IList<string> GetTestAssemblies()
    {
        var testAssemblies = new List<string>
        {
            GetAssetFullPath("SimpleTestProject.dll"),
            GetAssetFullPath("SimpleTestProject2.dll")
        };

        return testAssemblies;
    }
}
