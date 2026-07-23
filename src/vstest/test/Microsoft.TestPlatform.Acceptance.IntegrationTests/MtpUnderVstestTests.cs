// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

/// <summary>
/// End-to-end coverage for running a Microsoft.Testing.Platform (MTP) test application under
/// vstest.console. vstest detects the MTP app from its assembly metadata
/// (<c>[assembly: AssemblyMetadata("Microsoft.Testing.Platform.Application", "true")]</c>) and drives it
/// over the MTP JSON-RPC protocol via the MtpTestRuntimeProvider, translating MTP test-node updates back
/// into vstest results so the console summary and loggers keep working.
/// </summary>
[TestClass]
public class MtpUnderVstestTests : AcceptanceTestBase
{
    // MtpMSTestProject is an MSTest project built as an MTP application (EnableMSTestRunner): three tests
    // pass, one fails, one is skipped.
    private const string MtpApp = "MtpMSTestProject.dll";

    // MSTestProject1 is a classic vstest MSTest project driven by the vstest testhost: one passes, one
    // fails, one is skipped.
    private const string ClassicApp = "MSTestProject1.dll";

    [TestMethod]
    // MTP apps are .NET (Core) applications. Pin the testhost axis to .NET so we drive the net11.0 MTP app
    // from both the .NET Framework and the .NET console (the .NET Framework console exercises the Jsonite
    // JSON-RPC path; it is Windows-only and skipped elsewhere by the matrix).
    [TestMatrix(testHost: Target.Net)]
    public void RunMtpApplicationExecutesTestsOverMtpProtocol(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var arguments = PrepareArguments(
            GetAssetFullPath(MtpApp),
            testAdapterPath: null,
            runSettings: string.Empty,
            FrameworkArgValue,
            runnerInfo.InIsolationValue,
            resultsDirectory: TempDirectory.Path);

        InvokeVsTest(arguments);

        ValidateSummaryStatus(3, 1, 1);
    }

    [TestMethod]
    // A single vstest.console invocation over a classic vstest project AND an MTP application in one run:
    // the classic source is driven by the vstest testhost, the MTP source over the MTP protocol. vstest
    // groups the two sources into separate hosts and aggregates their results.
    [TestMatrix(testHost: Target.Net)]
    public void RunMixedClassicAndMtpApplicationsInSingleRun(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var arguments = PrepareArguments(
            [GetAssetFullPath(ClassicApp), GetAssetFullPath(MtpApp)],
            testAdapterPath: null,
            runSettings: string.Empty,
            FrameworkArgValue,
            runnerInfo.InIsolationValue,
            resultsDirectory: TempDirectory.Path);

        InvokeVsTest(arguments);

        // Classic 1/1/1 + MTP 3/1/1 aggregated into one run summary.
        ValidateSummaryStatus(4, 2, 2);
    }

    [TestMethod]
    // Prove a TRX logger aggregates results from both the classic and the MTP source in a mixed run into a
    // single .trx with all eight tests.
    [TestMatrix(testHost: Target.Net)]
    public void RunMixedClassicAndMtpApplicationsWritesSingleTrx(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var trxFileName = "mixed.trx";
        var arguments = PrepareArguments(
            [GetAssetFullPath(ClassicApp), GetAssetFullPath(MtpApp)],
            testAdapterPath: null,
            runSettings: string.Empty,
            FrameworkArgValue,
            runnerInfo.InIsolationValue,
            resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, $" /logger:trx;LogFileName={trxFileName}");

        InvokeVsTest(arguments);

        ValidateSummaryStatus(4, 2, 2);

        var trxPath = Path.Combine(TempDirectory.Path, trxFileName);
        Assert.IsTrue(File.Exists(trxPath), "Expected a single TRX to be written for the mixed run at '{0}'.", trxPath);
    }

    [TestMethod]
    // Blame runs the datacollector out of process; in the classic path testhost dials into it and reports
    // which test is currently running so collectors can track it. An MTP app has no testhost, so
    // vstest.console forwards those per-test-case started/ended notifications itself (see
    // MtpDataCollectionForwarder). Before that forwarding existed the datacollector waited on an event
    // channel nobody connected to and blocked until its connection timeout. This guards that enabling
    // /Blame on an MTP app connects the datacollector and completes the run with the expected results.
    [TestMatrix(testHost: Target.Net)]
    public void RunMtpApplicationWithBlameCompletesRun(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var arguments = PrepareArguments(
            GetAssetFullPath(MtpApp),
            testAdapterPath: null,
            runSettings: string.Empty,
            FrameworkArgValue,
            runnerInfo.InIsolationValue,
            resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " /Blame");

        InvokeVsTest(arguments);

        ValidateSummaryStatus(3, 1, 1);
    }

    [TestMethod]
    // Environment variables declared in a runsettings RunConfiguration/EnvironmentVariables block must be
    // injected into the self-hosted MTP process. There is no testhost here, so vstest.console applies them
    // to the MTP application launch. The guarded RunSettingsEnvironmentVariableIsInjected test asserts the
    // injected value; CHECK_RUNSETTINGS_VAR is passed as a process env var (inherited by the host) to opt
    // the check in, so the run passes only when runsettings injection actually delivered the value. If the
    // value did not reach the host that test fails and the summary would be 2/2/1 instead of 3/1/1.
    [TestMatrix(testHost: Target.Net)]
    public void RunMtpApplicationInjectsRunSettingsEnvironmentVariables(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var runsettingsXml = @"<RunSettings>
                                    <RunConfiguration>
                                      <EnvironmentVariables>
                                        <MTP_FROM_RUNSETTINGS>mtp-runsettings-value</MTP_FROM_RUNSETTINGS>
                                      </EnvironmentVariables>
                                    </RunConfiguration>
                                   </RunSettings>";
        var runsettingsPath = Path.Combine(TempDirectory.Path, "mtp_env_" + System.Guid.NewGuid() + ".runsettings");
        File.WriteAllText(runsettingsPath, runsettingsXml);

        var arguments = PrepareArguments(
            GetAssetFullPath(MtpApp),
            testAdapterPath: null,
            runSettings: runsettingsPath,
            FrameworkArgValue,
            runnerInfo.InIsolationValue,
            resultsDirectory: TempDirectory.Path);

        var env = new Dictionary<string, string?>
        {
            ["CHECK_RUNSETTINGS_VAR"] = "1",
        };

        InvokeVsTest(arguments, env);

        // The guarded test passes only if MTP_FROM_RUNSETTINGS reached the host with the runsettings value.
        ValidateSummaryStatus(3, 1, 1);
    }

    [TestMethod]
    // A test's captured standard output/error must be surfaced to the loggers. The MTP node carries the
    // per-test standardOutput/standardError; MtpTestNodeConverter now maps them onto the vstest result so
    // the console and TRX show them. Before this the markers appeared nowhere. TestPassesToo writes the
    // markers; the run produces a TRX that must contain them.
    [TestMatrix(testHost: Target.Net)]
    public void RunMtpApplicationSurfacesPerTestStandardOutput(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var trxFileName = "stdout.trx";
        var arguments = PrepareArguments(
            GetAssetFullPath(MtpApp),
            testAdapterPath: null,
            runSettings: string.Empty,
            FrameworkArgValue,
            runnerInfo.InIsolationValue,
            resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, $" /logger:trx;LogFileName={trxFileName}");

        InvokeVsTest(arguments);

        // MtpMSTestProject has five test cases: three pass, one fails, one is skipped.
        ValidateSummaryStatus(3, 1, 1);

        var trxPath = Path.Combine(TempDirectory.Path, trxFileName);
        Assert.IsTrue(File.Exists(trxPath), "Expected a TRX at '{0}'.", trxPath);
        var trx = File.ReadAllText(trxPath);
        Assert.Contains("MTP_STDOUT_MARKER", trx, "Expected the test's standard output to be surfaced into the TRX.");
        Assert.Contains("MTP_STDERR_MARKER", trx, "Expected the test's standard error to be surfaced into the TRX.");
    }

    [TestMethod]
    // A generic out-of-process data collector (SampleDataCollector) subscribes to per-test-case
    // start/end events, emits per-test-case attachments and reports the launched test-host PID. In the
    // classic path the testhost drives all of that; under MTP there is no testhost, so vstest.console
    // owns the datacollector lifecycle and forwards the per-test-case events itself. Blame is the
    // most visible collector that needs this, but the wiring must work for ANY out-of-process
    // collector (Blame, Event Log, custom ones). This guards that a generic collector on the MTP path
    // completes the run (no shutdown hang), reports the collector lifecycle (SessionStarted,
    // TestHostLaunched, per-test-case events, SessionEnded) and produces its attachments.
    [TestMatrix(testHost: Target.Net)]
    public void RunMtpApplicationWithGenericOutOfProcDataCollectorCompletesRun(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var extensionsPath = Path.GetDirectoryName(GetTestDllForFramework("OutOfProcDataCollector.dll", "netstandard2.0"));
        var arguments = PrepareArguments(
            GetAssetFullPath(MtpApp),
            testAdapterPath: null,
            runSettings: string.Empty,
            FrameworkArgValue,
            runnerInfo.InIsolationValue,
            resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " /Collect:SampleDataCollector", $" /TestAdapterPath:{extensionsPath}");

        // The collector writes its per-test-case source files here and then hands them to the sink with
        // deleteFile:true, so the sink moves them into the results directory as attachments. Keep this
        // source directory separate from the results directory so a leftover source file (e.g. if the sink
        // ever failed to delete it) cannot be mistaken for a produced attachment.
        var collectorSourceDirectory = Path.Combine(TempDirectory.Path, "collector-source");
        Directory.CreateDirectory(collectorSourceDirectory);
        var env = new Dictionary<string, string?>
        {
            ["TEST_ASSET_SAMPLE_COLLECTOR_PATH"] = collectorSourceDirectory,
        };

        InvokeVsTest(arguments, env);

        // The run must complete with the usual summary rather than hang at shutdown. MtpMSTestProject has
        // five test cases: three pass, one fails, one is skipped.
        ValidateSummaryStatus(3, 1, 1);

        // The datacollector lifecycle must be driven end to end even though there is no testhost: the
        // session events, the launched-process notification and the forwarded per-test-case events all
        // have to reach the out-of-process collector. Match the full datacollector message prefix so the
        // assertions cannot be satisfied by unrelated console output.
        StdOutputContains("Data collector 'SampleDataCollector' message: SessionStarted");
        StdOutputContains("Data collector 'SampleDataCollector' message: TestHostLaunched");
        StdOutputContains("Data collector 'SampleDataCollector' message: SessionEnded");
        StdOutputContains("Data collector 'SampleDataCollector' message: TestCaseStarted");
        StdOutputContains("Data collector 'SampleDataCollector' message: TestCaseEnded");

        // The collector emits one attachment per started test case through the forwarded TestCaseStart
        // events. All five MtpMSTestProject test cases surface a start on this path (the skipped one still
        // reports a TestCaseStart), so five attachments must land in the results directory. Exclude the
        // collector's own source directory so only the moved attachments are counted.
        var collectorSourceDirectoryPrefix = collectorSourceDirectory + Path.DirectorySeparatorChar;
        var testCaseAttachments = Directory
            .GetFiles(TempDirectory.Path, "testcasefilename*.txt", SearchOption.AllDirectories)
            .Where(file => !file.StartsWith(collectorSourceDirectoryPrefix, StringComparison.OrdinalIgnoreCase))
            .ToList();
        Assert.HasCount(5, testCaseAttachments, "Expected one per-test-case attachment for each started MtpMSTestProject test case forwarded on the MTP path.");
    }
}
