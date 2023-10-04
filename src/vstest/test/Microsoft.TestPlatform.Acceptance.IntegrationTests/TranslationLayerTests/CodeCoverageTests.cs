// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using Castle.Core.Internal;

using Microsoft.TestPlatform.VsTestConsole.TranslationLayer.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests.TranslationLayerTests;

[TestClass]
//Code coverage only supported on windows (based on the message in output)
[TestCategory("Windows-Review")]
public class CodeCoverageTests : CodeCoverageAcceptanceTestBase
{
    private IVsTestConsoleWrapper? _vstestConsoleWrapper;
    private RunEventHandler? _runEventHandler;
    private TelemetryEventsHandler? _telemetryEventsHandler;
    private TestRunAttachmentsProcessingEventHandler? _testRunAttachmentsProcessingEventHandler;

    [MemberNotNull(nameof(_vstestConsoleWrapper), nameof(_testRunAttachmentsProcessingEventHandler), nameof(_runEventHandler), nameof(_telemetryEventsHandler))]
    private void Setup()
    {
        _vstestConsoleWrapper = GetVsTestConsoleWrapper();
        _runEventHandler = new RunEventHandler();
        _testRunAttachmentsProcessingEventHandler = new TestRunAttachmentsProcessingEventHandler();
        _telemetryEventsHandler = new TelemetryEventsHandler();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _vstestConsoleWrapper?.EndSession();
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void TestRunWithCodeCoverage(RunnerInfo runnerInfo)
    {
        // arrange
        SetTestEnvironment(_testEnvironment, runnerInfo);
        Setup();

        // act
        _vstestConsoleWrapper.RunTests(GetTestAssemblies(), GetCodeCoverageRunSettings(1),
            new TestPlatformOptions { CollectMetrics = true }, null, _runEventHandler, _telemetryEventsHandler);

        // assert
        Assert.AreEqual(6, _runEventHandler.TestResults.Count);

        int expectedNumberOfAttachments = 1;
        Assert.AreEqual(expectedNumberOfAttachments, _runEventHandler.Attachments.Count);
        Assert.IsTrue(_telemetryEventsHandler.Events.Any(e => e.Name.StartsWith("vs/codecoverage") && e.Properties.Any()));

        AssertCoverageResults(_runEventHandler.Attachments);

        Assert.AreEqual("324f817a-7420-4e6d-b3c1-143fbed6d855", _runEventHandler.Metrics!["VS.TestPlatform.DataCollector.CorProfiler.datacollector://microsoft/CodeCoverage/2.0"]);
        Assert.AreEqual("324f817a-7420-4e6d-b3c1-143fbed6d855", _runEventHandler.Metrics["VS.TestPlatform.DataCollector.CoreClrProfiler.datacollector://microsoft/CodeCoverage/2.0"]);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void TestRunWithCodeCoverageUsingClrIe(RunnerInfo runnerInfo)
    {
        // arrange
        SetTestEnvironment(_testEnvironment, runnerInfo);
        Setup();

        // act
        _vstestConsoleWrapper.RunTests(GetTestAssemblies(), GetCodeCoverageRunSettings(1, true),
            new TestPlatformOptions { CollectMetrics = true }, null, _runEventHandler, _telemetryEventsHandler);

        // assert
        Assert.AreEqual(6, _runEventHandler.TestResults.Count);
        Assert.IsTrue(_telemetryEventsHandler.Events.Any(e => e.Name.StartsWith("vs/codecoverage") && e.Properties.Any()));

        int expectedNumberOfAttachments = 1;
        Assert.AreEqual(expectedNumberOfAttachments, _runEventHandler.Attachments.Count);

        AssertCoverageResults(_runEventHandler.Attachments);

        Assert.AreEqual("324f817a-7420-4e6d-b3c1-143fbed6d855", _runEventHandler.Metrics!["VS.TestPlatform.DataCollector.CorProfiler.datacollector://microsoft/CodeCoverage/2.0"]);
        Assert.AreEqual("324f817a-7420-4e6d-b3c1-143fbed6d855", _runEventHandler.Metrics["VS.TestPlatform.DataCollector.CoreClrProfiler.datacollector://microsoft/CodeCoverage/2.0"]);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void TestRunWithCodeCoverageParallel(RunnerInfo runnerInfo)
    {
        // arrange
        SetTestEnvironment(_testEnvironment, runnerInfo);
        Setup();

        // act
        _vstestConsoleWrapper.RunTests(GetTestAssemblies(), GetCodeCoverageRunSettings(4),
            new TestPlatformOptions { CollectMetrics = true }, null, _runEventHandler, _telemetryEventsHandler);

        // assert
        Assert.AreEqual(6, _runEventHandler.TestResults.Count);
        Assert.AreEqual(1, _runEventHandler.Attachments.Count);
        Assert.IsTrue(_telemetryEventsHandler.Events.Any(e => e.Name.StartsWith("vs/codecoverage") && e.Properties.Any()));

        AssertCoverageResults(_runEventHandler.Attachments);

        Assert.AreEqual("324f817a-7420-4e6d-b3c1-143fbed6d855", _runEventHandler.Metrics!["VS.TestPlatform.DataCollector.CorProfiler.datacollector://microsoft/CodeCoverage/2.0"]);
        Assert.AreEqual("324f817a-7420-4e6d-b3c1-143fbed6d855", _runEventHandler.Metrics["VS.TestPlatform.DataCollector.CoreClrProfiler.datacollector://microsoft/CodeCoverage/2.0"]);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public async Task TestRunWithCodeCoverageAndAttachmentsProcessingWithInvokedDataCollectors(RunnerInfo runnerInfo)
        => await TestRunWithCodeCoverageAndAttachmentsProcessingInternal(runnerInfo, true);

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public async Task TestRunWithCodeCoverageAndAttachmentsProcessingWithoutInvokedDataCollectors(RunnerInfo runnerInfo)
        => await TestRunWithCodeCoverageAndAttachmentsProcessingInternal(runnerInfo, false);

    private async Task TestRunWithCodeCoverageAndAttachmentsProcessingInternal(RunnerInfo runnerInfo, bool withInvokedDataCollectors)
    {
        // arrange
        SetTestEnvironment(_testEnvironment, runnerInfo);
        Setup();

        _vstestConsoleWrapper.RunTests(GetTestAssemblies().Take(1), GetCodeCoverageRunSettings(1), null, null, _runEventHandler, _telemetryEventsHandler);
        _vstestConsoleWrapper.RunTests(GetTestAssemblies().Skip(1), GetCodeCoverageRunSettings(1), null, null, _runEventHandler, _telemetryEventsHandler);

        Assert.AreEqual(6, _runEventHandler.TestResults.Count);
        Assert.AreEqual(2, _runEventHandler.Attachments.Count);
        Assert.AreEqual(2, _runEventHandler.InvokedDataCollectors.Count);
        Assert.IsFalse(_telemetryEventsHandler.Events.Any());

        // act
        await _vstestConsoleWrapper.ProcessTestRunAttachmentsAsync(
            _runEventHandler.Attachments,
            withInvokedDataCollectors ? _runEventHandler.InvokedDataCollectors : null,
            withInvokedDataCollectors ? GetCodeCoverageRunSettings(1) : null,
            true,
            true,
            _testRunAttachmentsProcessingEventHandler, CancellationToken.None);

        // Assert
        _testRunAttachmentsProcessingEventHandler.EnsureSuccess();
        Assert.AreEqual(1, _testRunAttachmentsProcessingEventHandler.Attachments.Count);

        AssertCoverageResults(_testRunAttachmentsProcessingEventHandler.Attachments);

        Assert.IsFalse(_testRunAttachmentsProcessingEventHandler.CompleteArgs!.IsCanceled);
        Assert.IsNull(_testRunAttachmentsProcessingEventHandler.CompleteArgs.Error);

        for (int i = 0; i < _testRunAttachmentsProcessingEventHandler.ProgressArgs.Count; i++)
        {
            TestRunAttachmentsProcessingProgressEventArgs progressArgs = _testRunAttachmentsProcessingEventHandler.ProgressArgs[i];
            Assert.AreEqual(i + 1, progressArgs.CurrentAttachmentProcessorIndex);
            Assert.AreEqual(1, progressArgs.CurrentAttachmentProcessorUris.Count);
            Assert.AreEqual("datacollector://microsoft/CodeCoverage/2.0", progressArgs.CurrentAttachmentProcessorUris.First().AbsoluteUri);
            Assert.AreEqual(withInvokedDataCollectors ? 2 : 1, progressArgs.AttachmentProcessorsCount);
            if (_testRunAttachmentsProcessingEventHandler.ProgressArgs.Count == 2)
            {
                Assert.AreEqual(100, progressArgs.CurrentAttachmentProcessorProgress);
            }
        }

        Assert.AreEqual("Completed", _testRunAttachmentsProcessingEventHandler.CompleteArgs.Metrics![TelemetryDataConstants.AttachmentsProcessingState]);
        Assert.AreEqual(2L, _testRunAttachmentsProcessingEventHandler.CompleteArgs.Metrics[TelemetryDataConstants.NumberOfAttachmentsSentForProcessing]);
        Assert.AreEqual(1L, _testRunAttachmentsProcessingEventHandler.CompleteArgs.Metrics[TelemetryDataConstants.NumberOfAttachmentsAfterProcessing]);
        Assert.IsTrue(_testRunAttachmentsProcessingEventHandler.CompleteArgs.Metrics.ContainsKey(TelemetryDataConstants.TimeTakenInSecForAttachmentsProcessing));

        Assert.IsTrue(File.Exists(_runEventHandler.Attachments.First().Attachments.First().Uri.LocalPath));
        Assert.IsFalse(File.Exists(_runEventHandler.Attachments.Last().Attachments.First().Uri.LocalPath));
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public async Task TestRunWithCodeCoverageAndAttachmentsProcessingNoMetrics(RunnerInfo runnerInfo)
    {
        // System.Environment.SetEnvironmentVariable("VSTEST_RUNNER_DEBUG_ATTACHVS", "1");
        // arrange
        SetTestEnvironment(_testEnvironment, runnerInfo);
        Setup();

        _vstestConsoleWrapper.RunTests(GetTestAssemblies().Take(1), GetCodeCoverageRunSettings(1), null, null, _runEventHandler, _telemetryEventsHandler);
        _vstestConsoleWrapper.RunTests(GetTestAssemblies().Skip(1), GetCodeCoverageRunSettings(1), null, null, _runEventHandler, _telemetryEventsHandler);

        Assert.AreEqual(6, _runEventHandler.TestResults.Count);
        Assert.AreEqual(2, _runEventHandler.Attachments.Count);
        Assert.AreEqual(2, _runEventHandler.InvokedDataCollectors.Count);
        Assert.IsFalse(_telemetryEventsHandler.Events.Any());

        // act
        await _vstestConsoleWrapper.ProcessTestRunAttachmentsAsync(_runEventHandler.Attachments, _runEventHandler.InvokedDataCollectors, GetCodeCoverageRunSettings(1), true, false, _testRunAttachmentsProcessingEventHandler, CancellationToken.None);

        // Assert
        Assert.AreEqual(1, _runEventHandler.InvokedDataCollectors.Select(x => x.FilePath).Distinct().Count());
        Assert.AreEqual(1, _runEventHandler.InvokedDataCollectors.Select(x => x.AssemblyQualifiedName).Distinct().Count());
        Assert.IsTrue(Regex.IsMatch(_runEventHandler.InvokedDataCollectors.Select(x => x.AssemblyQualifiedName).Distinct().Single(),
            @"Microsoft\.VisualStudio\.Coverage\.DynamicCoverageDataCollectorWithAttachmentProcessorAndTelemetry, Microsoft\.VisualStudio\.TraceDataCollector, Version=.*, Culture=neutral, PublicKeyToken=.*"));

        _testRunAttachmentsProcessingEventHandler.EnsureSuccess();
        Assert.AreEqual(1, _testRunAttachmentsProcessingEventHandler.Attachments.Count);

        AssertCoverageResults(_testRunAttachmentsProcessingEventHandler.Attachments);

        Assert.IsFalse(_testRunAttachmentsProcessingEventHandler.CompleteArgs!.IsCanceled);
        Assert.IsNull(_testRunAttachmentsProcessingEventHandler.CompleteArgs.Error);

        for (int i = 0; i < _testRunAttachmentsProcessingEventHandler.ProgressArgs.Count; i++)
        {
            TestRunAttachmentsProcessingProgressEventArgs progressArgs = _testRunAttachmentsProcessingEventHandler.ProgressArgs[i];
            Assert.AreEqual(i + 1, progressArgs.CurrentAttachmentProcessorIndex);
            Assert.AreEqual(1, progressArgs.CurrentAttachmentProcessorUris.Count);
            Assert.AreEqual("datacollector://microsoft/CodeCoverage/2.0", progressArgs.CurrentAttachmentProcessorUris.First().AbsoluteUri);
            Assert.AreEqual(2, progressArgs.AttachmentProcessorsCount);
            if (_testRunAttachmentsProcessingEventHandler.ProgressArgs.Count == 2)
            {
                Assert.AreEqual(100, progressArgs.CurrentAttachmentProcessorProgress);
            }
        }

        Assert.IsTrue(_testRunAttachmentsProcessingEventHandler.CompleteArgs.Metrics.IsNullOrEmpty());

        Assert.IsTrue(File.Exists(_runEventHandler.Attachments.First().Attachments.First().Uri.LocalPath));
        Assert.IsFalse(File.Exists(_runEventHandler.Attachments.Last().Attachments.First().Uri.LocalPath));
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public async Task TestRunWithCodeCoverageAndAttachmentsProcessingModuleDuplicated(RunnerInfo runnerInfo)
    {
        // arrange
        SetTestEnvironment(_testEnvironment, runnerInfo);
        Setup();

        _vstestConsoleWrapper.RunTests(GetTestAssemblies().Take(1), GetCodeCoverageRunSettings(1), null, null, _runEventHandler, _telemetryEventsHandler);
        _vstestConsoleWrapper.RunTests(GetTestAssemblies().Skip(1), GetCodeCoverageRunSettings(1), null, null, _runEventHandler, _telemetryEventsHandler);
        _vstestConsoleWrapper.RunTests(GetTestAssemblies().Skip(1), GetCodeCoverageRunSettings(1), null, null, _runEventHandler, _telemetryEventsHandler);

        Assert.AreEqual(9, _runEventHandler.TestResults.Count);
        Assert.AreEqual(3, _runEventHandler.Attachments.Count);
        Assert.AreEqual(3, _runEventHandler.InvokedDataCollectors.Count);
        Assert.IsFalse(_telemetryEventsHandler.Events.Any());

        // act
        await _vstestConsoleWrapper.ProcessTestRunAttachmentsAsync(_runEventHandler.Attachments, _runEventHandler.InvokedDataCollectors, GetCodeCoverageRunSettings(1), true, true, _testRunAttachmentsProcessingEventHandler, CancellationToken.None);

        // Assert
        Assert.AreEqual(1, _runEventHandler.InvokedDataCollectors.Select(x => x.FilePath).Distinct().Count());
        Assert.AreEqual(1, _runEventHandler.InvokedDataCollectors.Select(x => x.AssemblyQualifiedName).Distinct().Count());
        Assert.IsTrue(Regex.IsMatch(_runEventHandler.InvokedDataCollectors.Select(x => x.AssemblyQualifiedName).Distinct().Single(),
            @"Microsoft\.VisualStudio\.Coverage\.DynamicCoverageDataCollectorWithAttachmentProcessorAndTelemetry, Microsoft\.VisualStudio\.TraceDataCollector, Version=.*, Culture=neutral, PublicKeyToken=.*"));

        _testRunAttachmentsProcessingEventHandler.EnsureSuccess();
        Assert.AreEqual(1, _testRunAttachmentsProcessingEventHandler.Attachments.Count);

        AssertCoverageResults(_testRunAttachmentsProcessingEventHandler.Attachments);

        Assert.IsFalse(_testRunAttachmentsProcessingEventHandler.CompleteArgs!.IsCanceled);
        Assert.IsNull(_testRunAttachmentsProcessingEventHandler.CompleteArgs.Error);

        for (int i = 0; i < _testRunAttachmentsProcessingEventHandler.ProgressArgs.Count; i++)
        {
            TestRunAttachmentsProcessingProgressEventArgs progressArgs = _testRunAttachmentsProcessingEventHandler.ProgressArgs[i];
            Assert.AreEqual(i + 1, progressArgs.CurrentAttachmentProcessorIndex);
            Assert.AreEqual("datacollector://microsoft/CodeCoverage/2.0", progressArgs.CurrentAttachmentProcessorUris.First().AbsoluteUri);
            Assert.AreEqual(2, progressArgs.AttachmentProcessorsCount);

            if (_testRunAttachmentsProcessingEventHandler.ProgressArgs.Count == 2)
            {
                Assert.AreEqual(100, progressArgs.CurrentAttachmentProcessorProgress);
            }
        }

        Assert.AreEqual("Completed", _testRunAttachmentsProcessingEventHandler.CompleteArgs.Metrics![TelemetryDataConstants.AttachmentsProcessingState]);
        Assert.AreEqual(3L, _testRunAttachmentsProcessingEventHandler.CompleteArgs.Metrics[TelemetryDataConstants.NumberOfAttachmentsSentForProcessing]);
        Assert.AreEqual(1L, _testRunAttachmentsProcessingEventHandler.CompleteArgs.Metrics[TelemetryDataConstants.NumberOfAttachmentsAfterProcessing]);
        Assert.IsTrue(_testRunAttachmentsProcessingEventHandler.CompleteArgs.Metrics.ContainsKey(TelemetryDataConstants.TimeTakenInSecForAttachmentsProcessing));

        Assert.IsTrue(File.Exists(_runEventHandler.Attachments.First().Attachments.First().Uri.LocalPath));
        Assert.IsFalse(File.Exists(_runEventHandler.Attachments.Last().Attachments.First().Uri.LocalPath));
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public async Task TestRunWithCodeCoverageAndAttachmentsProcessingSameReportFormat(RunnerInfo runnerInfo)
    {
        // arrange
        SetTestEnvironment(_testEnvironment, runnerInfo);
        Setup();

        _vstestConsoleWrapper.RunTests(GetTestAssemblies().Take(1), GetCodeCoverageRunSettings(1, outputFormat: "Coverage"),
            null, null, _runEventHandler, _telemetryEventsHandler);
        _vstestConsoleWrapper.RunTests(GetTestAssemblies().Skip(1), GetCodeCoverageRunSettings(1, outputFormat: "Coverage"),
            null, null, _runEventHandler, _telemetryEventsHandler);

        Assert.AreEqual(6, _runEventHandler.TestResults.Count);
        Assert.AreEqual(2, _runEventHandler.Attachments.Count);
        Assert.AreEqual(2, _runEventHandler.InvokedDataCollectors.Count);
        Assert.IsFalse(_telemetryEventsHandler.Events.Any());

        // act
        await _vstestConsoleWrapper.ProcessTestRunAttachmentsAsync(_runEventHandler.Attachments, _runEventHandler.InvokedDataCollectors, GetCodeCoverageRunSettings(1), true, true, _testRunAttachmentsProcessingEventHandler, CancellationToken.None);

        // Assert
        Assert.AreEqual(1, _runEventHandler.InvokedDataCollectors.Select(x => x.FilePath).Distinct().Count());
        Assert.AreEqual(1, _runEventHandler.InvokedDataCollectors.Select(x => x.AssemblyQualifiedName).Distinct().Count());
        Assert.IsTrue(Regex.IsMatch(_runEventHandler.InvokedDataCollectors.Select(x => x.AssemblyQualifiedName).Distinct().Single(),
            @"Microsoft\.VisualStudio\.Coverage\.DynamicCoverageDataCollectorWithAttachmentProcessorAndTelemetry, Microsoft\.VisualStudio\.TraceDataCollector, Version=.*, Culture=neutral, PublicKeyToken=.*"));

        _testRunAttachmentsProcessingEventHandler.EnsureSuccess();
        Assert.AreEqual(1, _testRunAttachmentsProcessingEventHandler.Attachments.Count);
        Assert.AreEqual(1, _testRunAttachmentsProcessingEventHandler.Attachments[0].Attachments.Count);
        Assert.IsTrue(_testRunAttachmentsProcessingEventHandler.Attachments[0].Attachments[0].Uri.LocalPath.Contains(".coverage"));

        AssertCoverageResults(_testRunAttachmentsProcessingEventHandler.Attachments);

        Assert.IsFalse(_testRunAttachmentsProcessingEventHandler.CompleteArgs!.IsCanceled);
        Assert.IsNull(_testRunAttachmentsProcessingEventHandler.CompleteArgs.Error);

        for (int i = 0; i < _testRunAttachmentsProcessingEventHandler.ProgressArgs.Count; i++)
        {
            TestRunAttachmentsProcessingProgressEventArgs progressArgs = _testRunAttachmentsProcessingEventHandler.ProgressArgs[i];
            Assert.AreEqual(i + 1, progressArgs.CurrentAttachmentProcessorIndex);
            Assert.AreEqual("datacollector://microsoft/CodeCoverage/2.0", progressArgs.CurrentAttachmentProcessorUris.First().AbsoluteUri);
            Assert.AreEqual(2, progressArgs.AttachmentProcessorsCount);

            if (_testRunAttachmentsProcessingEventHandler.ProgressArgs.Count == 2)
            {
                Assert.AreEqual(100, progressArgs.CurrentAttachmentProcessorProgress);
            }
        }

        Assert.AreEqual("Completed", _testRunAttachmentsProcessingEventHandler.CompleteArgs.Metrics![TelemetryDataConstants.AttachmentsProcessingState]);
        Assert.AreEqual(2L, _testRunAttachmentsProcessingEventHandler.CompleteArgs.Metrics[TelemetryDataConstants.NumberOfAttachmentsSentForProcessing]);
        Assert.AreEqual(1L, _testRunAttachmentsProcessingEventHandler.CompleteArgs.Metrics[TelemetryDataConstants.NumberOfAttachmentsAfterProcessing]);
        Assert.IsTrue(_testRunAttachmentsProcessingEventHandler.CompleteArgs.Metrics.ContainsKey(TelemetryDataConstants.TimeTakenInSecForAttachmentsProcessing));

        Assert.IsTrue(File.Exists(_runEventHandler.Attachments.First().Attachments.First().Uri.LocalPath));
        Assert.IsFalse(File.Exists(_runEventHandler.Attachments.Last().Attachments.First().Uri.LocalPath));
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public async Task TestRunWithCodeCoverageAndAttachmentsProcessingDifferentReportFormats(RunnerInfo runnerInfo)
    {
        // arrange
        SetTestEnvironment(_testEnvironment, runnerInfo);
        Setup();

        _vstestConsoleWrapper.RunTests(GetTestAssemblies().Take(1), GetCodeCoverageRunSettings(1, outputFormat: "Coverage"), null, null, _runEventHandler, _telemetryEventsHandler);
        _vstestConsoleWrapper.RunTests(GetTestAssemblies().Skip(1), GetCodeCoverageRunSettings(1, outputFormat: "Coverage"), null, null, _runEventHandler, _telemetryEventsHandler);
        _vstestConsoleWrapper.RunTests(GetTestAssemblies().Take(1), GetCodeCoverageRunSettings(1, outputFormat: "Cobertura"), null, null, _runEventHandler, _telemetryEventsHandler);
        _vstestConsoleWrapper.RunTests(GetTestAssemblies().Skip(1), GetCodeCoverageRunSettings(1, outputFormat: "Cobertura"), null, null, _runEventHandler, _telemetryEventsHandler);

        Assert.AreEqual(12, _runEventHandler.TestResults.Count);
        Assert.AreEqual(4, _runEventHandler.Attachments.Count);
        Assert.AreEqual(4, _runEventHandler.InvokedDataCollectors.Count);
        Assert.IsFalse(_telemetryEventsHandler.Events.Any());

        // act
        await _vstestConsoleWrapper.ProcessTestRunAttachmentsAsync(_runEventHandler.Attachments, _runEventHandler.InvokedDataCollectors, GetCodeCoverageRunSettings(1, outputFormat: "Coverage"), true, true, _testRunAttachmentsProcessingEventHandler, CancellationToken.None);

        // Assert
        Assert.AreEqual(1, _runEventHandler.InvokedDataCollectors.Select(x => x.FilePath).Distinct().Count());
        Assert.AreEqual(1, _runEventHandler.InvokedDataCollectors.Select(x => x.AssemblyQualifiedName).Distinct().Count());
        Assert.IsTrue(Regex.IsMatch(_runEventHandler.InvokedDataCollectors.Select(x => x.AssemblyQualifiedName).Distinct().Single(),
            @"Microsoft\.VisualStudio\.Coverage\.DynamicCoverageDataCollectorWithAttachmentProcessorAndTelemetry, Microsoft\.VisualStudio\.TraceDataCollector, Version=.*, Culture=neutral, PublicKeyToken=.*"));


        _testRunAttachmentsProcessingEventHandler.EnsureSuccess();
        Assert.AreEqual(1, _testRunAttachmentsProcessingEventHandler.Attachments.Count);
        Assert.AreEqual(2, _testRunAttachmentsProcessingEventHandler.Attachments[0].Attachments.Count);
        Assert.IsTrue(_testRunAttachmentsProcessingEventHandler.Attachments[0].Attachments[0].Uri.LocalPath.Contains(".cobertura.xml"));
        Assert.IsTrue(_testRunAttachmentsProcessingEventHandler.Attachments[0].Attachments[1].Uri.LocalPath.Contains(".coverage"));

        AssertCoverageResults(_testRunAttachmentsProcessingEventHandler.Attachments);

        Assert.IsFalse(_testRunAttachmentsProcessingEventHandler.CompleteArgs!.IsCanceled);
        Assert.IsNull(_testRunAttachmentsProcessingEventHandler.CompleteArgs.Error);

        for (int i = 0; i < _testRunAttachmentsProcessingEventHandler.ProgressArgs.Count; i++)
        {
            TestRunAttachmentsProcessingProgressEventArgs progressArgs = _testRunAttachmentsProcessingEventHandler.ProgressArgs[i];
            Assert.AreEqual(i + 1, progressArgs.CurrentAttachmentProcessorIndex);
            Assert.AreEqual("datacollector://microsoft/CodeCoverage/2.0", progressArgs.CurrentAttachmentProcessorUris.First().AbsoluteUri);

            // We have two processor because we append always CodeCoverage attachment processor shipped with VSTest+Attachment processor shipped from code coverage repo.
            Assert.AreEqual(2, progressArgs.AttachmentProcessorsCount);

            if (_testRunAttachmentsProcessingEventHandler.ProgressArgs.Count == 2)
            {
                Assert.AreEqual(100, progressArgs.CurrentAttachmentProcessorProgress);
            }
        }

        Assert.AreEqual("Completed", _testRunAttachmentsProcessingEventHandler.CompleteArgs.Metrics![TelemetryDataConstants.AttachmentsProcessingState]);
        Assert.AreEqual(4L, _testRunAttachmentsProcessingEventHandler.CompleteArgs.Metrics[TelemetryDataConstants.NumberOfAttachmentsSentForProcessing]);
        Assert.AreEqual(1L, _testRunAttachmentsProcessingEventHandler.CompleteArgs.Metrics[TelemetryDataConstants.NumberOfAttachmentsAfterProcessing]);
        Assert.IsTrue(_testRunAttachmentsProcessingEventHandler.CompleteArgs.Metrics.ContainsKey(TelemetryDataConstants.TimeTakenInSecForAttachmentsProcessing));

        Assert.IsTrue(File.Exists(_runEventHandler.Attachments.First().Attachments.First().Uri.LocalPath));
        Assert.IsFalse(File.Exists(_runEventHandler.Attachments.Last().Attachments.First().Uri.LocalPath));
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    [DoNotParallelize]
    public async Task EndSessionShouldEnsureVstestConsoleProcessDies(RunnerInfo runnerInfo)
    {
        var numOfProcesses = Process.GetProcessesByName("vstest.console").Length;

        SetTestEnvironment(_testEnvironment, runnerInfo);
        Setup();

        _vstestConsoleWrapper.RunTests(GetTestAssemblies().Take(1), GetCodeCoverageRunSettings(1), null, null, _runEventHandler, _telemetryEventsHandler);
        _vstestConsoleWrapper.RunTests(GetTestAssemblies().Skip(1), GetCodeCoverageRunSettings(1), null, null, _runEventHandler, _telemetryEventsHandler);

        Assert.AreEqual(6, _runEventHandler.TestResults.Count);
        Assert.AreEqual(2, _runEventHandler.Attachments.Count);
        Assert.AreEqual(2, _runEventHandler.InvokedDataCollectors.Count);
        Assert.IsFalse(_telemetryEventsHandler.Events.Any());

        await _vstestConsoleWrapper.ProcessTestRunAttachmentsAsync(_runEventHandler.Attachments, _runEventHandler.InvokedDataCollectors, GetCodeCoverageRunSettings(1), true, true, _testRunAttachmentsProcessingEventHandler, CancellationToken.None);

        // act
        _vstestConsoleWrapper?.EndSession();

        // Assert
        Assert.AreEqual(numOfProcesses, Process.GetProcessesByName("vstest.console").Length);

        _vstestConsoleWrapper = null;
    }

    private IList<string> GetTestAssemblies()
    {
        return GetProjects().Select(p => GetAssetFullPath(p)).ToList();
    }

    private static IList<string> GetProjects()
    {
        return new List<string> { "SimpleTestProject.dll", "SimpleTestProject2.dll" };
    }

    /// <summary>
    /// Default RunSettings
    /// </summary>
    /// <returns></returns>
    private string GetCodeCoverageRunSettings(int cpuCount, bool useClrIeInstrumentationEngine = false, string outputFormat = "Coverage")
    {
        string runSettingsXml = $@"<?xml version=""1.0"" encoding=""utf-8""?>
                                    <RunSettings>
                                        <RunConfiguration>
                                            <TargetFrameworkVersion>{FrameworkArgValue}</TargetFrameworkVersion>
                                            <TestAdaptersPaths>{GetNetStandardAdapterPath()}</TestAdaptersPaths>
                                            <MaxCpuCount>{cpuCount}</MaxCpuCount>
                                        </RunConfiguration>
                                        <DataCollectionRunSettings>
                                            <DataCollectors>
                                                <DataCollector friendlyName=""Code Coverage"" uri=""datacollector://Microsoft/CodeCoverage/2.0"" assemblyQualifiedName=""Microsoft.VisualStudio.Coverage.DynamicCoverageDataCollector, Microsoft.VisualStudio.TraceCollector, Version=11.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"">
                                                    <Configuration>
                                                      <CLRIEInstrumentationNetFramework>{useClrIeInstrumentationEngine}</CLRIEInstrumentationNetFramework>
                                                      <Format>{outputFormat}</Format>
                                                      <TelemetryEnabled>true</TelemetryEnabled>
                                                      <CodeCoverage>
                                                        <ModulePaths>
                                                          <Exclude>
                                                            <ModulePath>.*CPPUnitTestFramework.*</ModulePath>
                                                          </Exclude>
                                                        </ModulePaths>

                                                        <!-- We recommend you do not change the following values: -->
                                                        <UseVerifiableInstrumentation>True</UseVerifiableInstrumentation>
                                                        <AllowLowIntegrityProcesses>True</AllowLowIntegrityProcesses>
                                                        <CollectFromChildProcesses>True</CollectFromChildProcesses>
                                                        <CollectAspDotNet>False</CollectAspDotNet>
                                                      </CodeCoverage>
                                                    </Configuration>
                                                </DataCollector>
                                            </DataCollectors>
                                        </DataCollectionRunSettings>
                                    </RunSettings>";
        return runSettingsXml;
    }

    private void AssertCoverageResults(IList<AttachmentSet> attachments)
    {
        foreach (var attachmentSet in attachments)
        {
            foreach (var attachment in attachmentSet.Attachments)
            {
                var xmlCoverage = GetXmlCoverage(attachments.First().Attachments.First().Uri.LocalPath, TempDirectory);

                foreach (var project in GetProjects())
                {
                    var moduleNode = GetModuleNode(xmlCoverage.DocumentElement!, project)!;
                    AssertCoverage(moduleNode, ExpectedMinimalModuleCoverage);
                }
            }
        }
    }
}
