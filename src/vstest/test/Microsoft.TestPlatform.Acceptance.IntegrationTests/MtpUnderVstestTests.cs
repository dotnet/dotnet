// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;

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
    // MtpMSTestProject is an MSTest project built as an MTP application (EnableMSTestRunner): two tests
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

        ValidateSummaryStatus(2, 1, 1);
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

        // Classic 1/1/1 + MTP 2/1/1 aggregated into one run summary.
        ValidateSummaryStatus(3, 2, 2);
    }

    [TestMethod]
    // Prove a TRX logger aggregates results from both the classic and the MTP source in a mixed run into a
    // single .trx with all seven tests.
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

        ValidateSummaryStatus(3, 2, 2);

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

        ValidateSummaryStatus(2, 1, 1);
    }
}
