// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TestResult = Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult;

namespace Microsoft.TestPlatform.AcceptanceTests;

/// <summary>
/// Integration tests verifying serialization compatibility between STJ-based (System.Text.Json)
/// and Newtonsoft.Json-based versions of vstest.console and testhost.
///
/// After the STJ migration, the new vstest.console must be able to communicate with older
/// testhosts that use Newtonsoft for JSON serialization, and vice versa. These tests exercise
/// that cross-version communication through real discovery and execution flows.
///
/// If serialization is incompatible between versions, these tests will fail with zero
/// discovered tests or zero results — the JSON messages won't deserialize on the receiving side.
/// </summary>
[TestClass]
[TestCategory("Compatibility")]
public class SerializationCompatibilityTests : AcceptanceTestBase
{
    #region Discovery Tests

    /// <summary>
    /// Latest runner (STJ) discovers tests hosted by various testhost versions (Newtonsoft).
    /// Verifies that discovery request/response messages serialize correctly across the version boundary.
    /// </summary>
    [TestMethod]
    [TestCategory("Windows-Review")]
    [RunnerCompatibilityDataSource()]
    public void DiscoverTests_LatestRunner_WithOlderTesthosts(RunnerInfo runnerInfo)
    {
#pragma warning disable RS0030 // Do not use banned APIs
        Environment.SetEnvironmentVariable(EnvironmentHelper.VstestConnectionTimeout, "10");
#pragma warning restore RS0030 // Do not use banned APIs
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var wrapper = GetVsTestConsoleWrapper();
        try
        {
            var discoveryHandler = new CompatibilityDiscoveryEventHandler();

            wrapper.DiscoverTests(
                GetTestDlls("MSTestProject1.dll"),
                GetDefaultRunSettings(),
                discoveryHandler);

            Assert.IsNotEmpty(discoveryHandler.DiscoveredTestCases,
                $"Expected discovered tests but got 0. " +
                $"Runner={runnerInfo.VSTestConsoleInfo}, TestHost={runnerInfo.TestHostInfo}. " +
                $"This may indicate a serialization incompatibility between STJ and Newtonsoft.");
        }
        finally
        {
            wrapper.EndSession();
        }
    }

    /// <summary>
    /// Various older runner versions (Newtonsoft) discover tests hosted by the latest testhost (STJ).
    /// Verifies that older runners can understand discovery responses from the new STJ-based testhost.
    /// </summary>
    [TestMethod]
    [TestCategory("Windows-Review")]
    [TestHostCompatibilityDataSource]
    public void DiscoverTests_OlderRunners_WithLatestTesthost(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var wrapper = GetVsTestConsoleWrapper();
        try
        {
            var discoveryHandler = new CompatibilityDiscoveryEventHandler();

            wrapper.DiscoverTests(
                GetTestDlls("MSTestProject1.dll"),
                GetDefaultRunSettings(),
                discoveryHandler);

            Assert.IsNotEmpty(discoveryHandler.DiscoveredTestCases,
                $"Expected discovered tests but got 0. " +
                $"Runner={runnerInfo.VSTestConsoleInfo}, TestHost={runnerInfo.TestHostInfo}. " +
                $"This may indicate a serialization incompatibility between Newtonsoft and STJ.");
        }
        finally
        {
            wrapper.EndSession();
        }
    }

    #endregion

    #region Execution Tests

    /// <summary>
    /// Latest runner (STJ) executes tests hosted by various testhost versions (Newtonsoft).
    /// Verifies that test run messages (start, result, complete) serialize correctly across versions.
    /// </summary>
    [TestMethod]
    [TestCategory("Windows-Review")]
    [RunnerCompatibilityDataSource]
    public void RunTests_LatestRunner_WithOlderTesthosts(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var wrapper = GetVsTestConsoleWrapper();
        try
        {
            var runHandler = new CompatibilityRunEventHandler();

            wrapper.RunTests(
                GetTestDlls("MSTestProject1.dll"),
                GetDefaultRunSettings(),
                runHandler);

            Assert.IsNotEmpty(runHandler.TestResults,
                $"Expected test results but got 0. " +
                $"Runner={runnerInfo.VSTestConsoleInfo}, TestHost={runnerInfo.TestHostInfo}. " +
                $"This may indicate a serialization incompatibility between STJ and Newtonsoft.");

            // Verify we get a mix of outcomes — confirms full result fidelity across versions.
            #pragma warning disable MSTEST0037
            Assert.IsTrue(
                runHandler.TestResults.Any(r => r.Outcome == TestOutcome.Passed),
                "Expected at least one passed test result.");
#pragma warning restore MSTEST0037
        }
        finally
        {
            wrapper.EndSession();
        }
    }

    /// <summary>
    /// Various older runner versions (Newtonsoft) execute tests hosted by the latest testhost (STJ).
    /// Verifies that older runners can process execution results from the new STJ-based testhost.
    /// </summary>
    [TestMethod]
    [TestCategory("Windows-Review")]
    [TestHostCompatibilityDataSource]
    public void RunTests_OlderRunners_WithLatestTesthost(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var wrapper = GetVsTestConsoleWrapper();
        try
        {
            var runHandler = new CompatibilityRunEventHandler();

            wrapper.RunTests(
                GetTestDlls("MSTestProject1.dll"),
                GetDefaultRunSettings(),
                runHandler);

            Assert.IsNotEmpty(runHandler.TestResults,
                $"Expected test results but got 0. " +
                $"Runner={runnerInfo.VSTestConsoleInfo}, TestHost={runnerInfo.TestHostInfo}. " +
                $"This may indicate a serialization incompatibility between Newtonsoft and STJ.");

            #pragma warning disable MSTEST0037
            Assert.IsTrue(
                runHandler.TestResults.Any(r => r.Outcome == TestOutcome.Passed),
                "Expected at least one passed test result.");
#pragma warning restore MSTEST0037
        }
        finally
        {
            wrapper.EndSession();
        }
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Minimal discovery event handler for compatibility tests.
    /// Collects discovered test cases and any error messages for diagnostics.
    /// </summary>
    private sealed class CompatibilityDiscoveryEventHandler : ITestDiscoveryEventsHandler
    {
        public List<TestCase> DiscoveredTestCases { get; } = new();

        public List<string> ErrorMessages { get; } = new();

        public void HandleDiscoveredTests(IEnumerable<TestCase>? discoveredTestCases)
        {
            if (discoveredTestCases != null)
            {
                DiscoveredTestCases.AddRange(discoveredTestCases);
            }
        }

        public void HandleDiscoveryComplete(long totalTests, IEnumerable<TestCase>? lastChunk, bool isAborted)
        {
            if (lastChunk != null)
            {
                DiscoveredTestCases.AddRange(lastChunk);
            }
        }

        public void HandleLogMessage(TestMessageLevel level, string? message)
        {
            if (level == TestMessageLevel.Error && message != null)
            {
                ErrorMessages.Add(message);
            }
        }

        public void HandleRawMessage(string rawMessage)
        {
        }
    }

    /// <summary>
    /// Minimal run event handler for compatibility tests.
    /// Collects test results and any error messages for diagnostics.
    /// </summary>
    private sealed class CompatibilityRunEventHandler : ITestRunEventsHandler
    {
        public List<TestResult> TestResults { get; } = new();

        public List<string> ErrorMessages { get; } = new();

        public void HandleTestRunStatsChange(TestRunChangedEventArgs? testRunChangedArgs)
        {
            if (testRunChangedArgs?.NewTestResults != null)
            {
                TestResults.AddRange(testRunChangedArgs.NewTestResults);
            }
        }

        public void HandleTestRunComplete(
            TestRunCompleteEventArgs testRunCompleteArgs,
            TestRunChangedEventArgs? lastChunkArgs,
            ICollection<AttachmentSet>? runContextAttachments,
            ICollection<string>? executorUris)
        {
            if (lastChunkArgs?.NewTestResults != null)
            {
                TestResults.AddRange(lastChunkArgs.NewTestResults);
            }

            if (testRunCompleteArgs.Error != null)
            {
                ErrorMessages.Add(testRunCompleteArgs.Error.ToString());
            }
        }

        public void HandleLogMessage(TestMessageLevel level, string? message)
        {
            if (level == TestMessageLevel.Error && message != null)
            {
                ErrorMessages.Add(message);
            }
        }

        public void HandleRawMessage(string rawMessage)
        {
        }

        public int LaunchProcessWithDebuggerAttached(TestProcessStartInfo testProcessStartInfo)
        {
            return -1;
        }
    }

    #endregion
}
