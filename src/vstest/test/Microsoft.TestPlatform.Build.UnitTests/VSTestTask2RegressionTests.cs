// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.TestPlatform.Build.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.Build.UnitTests;

/// <summary>
/// Regression tests for VSTestTask2 message parsing and formatting.
/// </summary>
[TestClass]
public class VSTestTask2RegressionTests
{
    // Regression test for #5115 — Write dll instead of target on abort, rename errors
    // Regression test for #5113 — Error output as info in terminal logger
    // Regression test for #5084 — Handle ANSI escape in terminal logger reporter

    [TestMethod]
    public void GetFormattedDurationString_ZeroDuration_ShouldReturnNull()
    {
        // Regression test for #4894 — Time is reported incorrectly
        string? result = VSTestTask2.GetFormattedDurationString(TimeSpan.Zero);
        Assert.IsNull(result);
    }

    [TestMethod]
    public void GetFormattedDurationString_SubMillisecond_ShouldReturnLessThan1ms()
    {
        // Regression test for #4894
        string? result = VSTestTask2.GetFormattedDurationString(TimeSpan.FromTicks(1));
        Assert.AreEqual("< 1ms", result);
    }

    [TestMethod]
    public void GetFormattedDurationString_Milliseconds_ShouldFormatCorrectly()
    {
        // Regression test for #4894
        string? result = VSTestTask2.GetFormattedDurationString(TimeSpan.FromMilliseconds(500));
        Assert.AreEqual("500ms", result);
    }

    [TestMethod]
    public void GetFormattedDurationString_Seconds_ShouldFormatCorrectly()
    {
        // Regression test for #4894
        string? result = VSTestTask2.GetFormattedDurationString(TimeSpan.FromSeconds(5));
        Assert.AreEqual("5s", result);
    }

    [TestMethod]
    public void GetFormattedDurationString_SecondsAndMilliseconds_ShouldFormatCorrectly()
    {
        // Regression test for #4894
        string? result = VSTestTask2.GetFormattedDurationString(TimeSpan.FromMilliseconds(5500));
        Assert.AreEqual("5s 500ms", result);
    }

    [TestMethod]
    public void GetFormattedDurationString_Minutes_ShouldOmitMilliseconds()
    {
        // Regression test for #4894
        string? result = VSTestTask2.GetFormattedDurationString(TimeSpan.FromMinutes(2) + TimeSpan.FromSeconds(30));
        Assert.AreEqual("2m 30s", result);
    }

    [TestMethod]
    public void GetFormattedDurationString_Hours_ShouldOmitSecondsAndMilliseconds()
    {
        // Regression test for #4894
        string? result = VSTestTask2.GetFormattedDurationString(TimeSpan.FromHours(1) + TimeSpan.FromMinutes(15));
        Assert.AreEqual("1h 15m", result);
    }

    [TestMethod]
    public void GetFormattedDurationString_MoreThanOneDay_ShouldReturnGreaterThan1d()
    {
        // Regression test for #4894
        string? result = VSTestTask2.GetFormattedDurationString(TimeSpan.FromDays(2));
        Assert.AreEqual("> 1d", result);
    }

    [TestMethod]
    public void LogEventsFromTextOutput_NonSplitterMessage_ShouldNotThrow()
    {
        // Regression test for #5113 — Regular output should be forwarded without error
        var task = CreateVSTestTask2();
        var engine = (RecordingBuildEngine)task.BuildEngine;

        // Call protected method via helper
        task.TestLogEventsFromTextOutput("Microsoft (R) Test Execution Command Line Tool", MessageImportance.High);

        // Should have logged an output message, not an error
        Assert.IsEmpty(engine.Errors);
    }

    [TestMethod]
    public void LogEventsFromTextOutput_OutputError_ShouldLogAsInfo()
    {
        // Regression test for #5113 — Error output from testhost should not be logged as MSBuild error
        var task = CreateVSTestTask2();
        var engine = (RecordingBuildEngine)task.BuildEngine;

        task.TestLogEventsFromTextOutput("||||output-error||||Some stderr output from test", MessageImportance.High);

        // Should NOT log as error
        Assert.IsEmpty(engine.Errors);
        // Should log as message (info)
        Assert.IsNotEmpty(engine.Messages);

    }

    [TestMethod]
    public void LogEventsFromTextOutput_RunCancel_ShouldLogErrorWithTestRunCancelCode()
    {
        // Regression test for #5115 — Write dll instead of target on abort
        var task = CreateVSTestTask2();
        var engine = (RecordingBuildEngine)task.BuildEngine;

        task.TestLogEventsFromTextOutput("||||run-cancel||||Test run was canceled.", MessageImportance.High);

        Assert.HasCount(1, engine.Errors);
        Assert.AreEqual("TESTRUNCANCEL", engine.Errors[0].Code);
    }

    [TestMethod]
    public void LogEventsFromTextOutput_RunAbort_ShouldLogErrorWithTestRunAbortCode()
    {
        // Regression test for #5115 — Write dll instead of target on abort
        var task = CreateVSTestTask2();
        var engine = (RecordingBuildEngine)task.BuildEngine;

        task.TestLogEventsFromTextOutput("||||run-abort||||Test run was aborted.", MessageImportance.High);

        Assert.HasCount(1, engine.Errors);
        Assert.AreEqual("TESTRUNABORT", engine.Errors[0].Code);
    }

    [TestMethod]
    public void LogEventsFromTextOutput_TestFailed_ShouldLogErrorWithTestErrorCode()
    {
        // Regression test for #5115
        var task = CreateVSTestTask2();
        var engine = (RecordingBuildEngine)task.BuildEngine;

        task.TestLogEventsFromTextOutput("||||test-failed||||Test failed: expected 1 but was 2||||TestFile.cs||||42", MessageImportance.High);

        Assert.HasCount(1, engine.Errors);
        Assert.AreEqual("TESTERROR", engine.Errors[0].Code);
    }

    [TestMethod]
    public void LogEventsFromTextOutput_OutputWarning_ShouldLogAsWarning()
    {
        var task = CreateVSTestTask2();
        var engine = (RecordingBuildEngine)task.BuildEngine;

        task.TestLogEventsFromTextOutput("||||output-warning||||Some warning message", MessageImportance.High);

        Assert.HasCount(1, engine.Warnings);
    }

    [TestMethod]
    public void LogEventsFromTextOutput_NullOutput_ShouldNotThrow()
    {
        // Regression test for #5113 — LogMSBuildOutputMessage null-safe
        var task = CreateVSTestTask2();

        // Should not throw
        task.TestLogEventsFromTextOutput("||||output-info||||", MessageImportance.High);
    }

    private static TestableVSTestTask2 CreateVSTestTask2()
    {
        var engine = new RecordingBuildEngine();
        var task = new TestableVSTestTask2
        {
            BuildEngine = engine,
            TestFileFullPath = new TaskItem(@"C:\path\to\test.dll"),
            VSTestConsolePath = new TaskItem(@"C:\path\to\vstest.console.dll"),
        };
        return task;
    }
}

/// <summary>
/// Testable wrapper that exposes LogEventsFromTextOutput for testing.
/// </summary>
internal class TestableVSTestTask2 : VSTestTask2
{
    public void TestLogEventsFromTextOutput(string singleLine, MessageImportance messageImportance)
    {
        LogEventsFromTextOutput(singleLine, messageImportance);
    }
}

/// <summary>
/// A build engine that records errors, warnings, and messages for test verification.
/// </summary>
internal class RecordingBuildEngine : IBuildEngine
{
    public List<BuildErrorEventArgs> Errors { get; } = new();
    public List<BuildWarningEventArgs> Warnings { get; } = new();
    public List<BuildMessageEventArgs> Messages { get; } = new();

    public bool ContinueOnError => false;
    public int LineNumberOfTaskNode => 0;
    public int ColumnNumberOfTaskNode => 0;
    public string ProjectFileOfTaskNode => string.Empty;

    public bool BuildProjectFile(string projectFileName, string[] targetNames, System.Collections.IDictionary globalProperties, System.Collections.IDictionary targetOutputs) => false;

    public void LogCustomEvent(CustomBuildEventArgs e) { }

    public void LogErrorEvent(BuildErrorEventArgs e) => Errors.Add(e);

    public void LogMessageEvent(BuildMessageEventArgs e) => Messages.Add(e);

    public void LogWarningEvent(BuildWarningEventArgs e) => Warnings.Add(e);
}
