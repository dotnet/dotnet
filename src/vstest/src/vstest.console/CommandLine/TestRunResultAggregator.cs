// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine;

/// <summary>
/// Aggregates test messages and test results received to determine the test run result.
/// </summary>
internal class TestRunResultAggregator
{
    private static TestRunResultAggregator? s_instance;

    /// <summary>
    /// Initializes the TestRunResultAggregator
    /// </summary>
    /// <remarks>Constructor is private since the factory method should be used to get the instance.</remarks>
    protected internal TestRunResultAggregator()
    {
        // Outcome is passed until we see a failure.
        Outcome = TestOutcome.Passed;
    }

    /// <summary>
    /// Gets the instance of the test run result aggregator.
    /// </summary>
    /// <returns>Instance of the test run result aggregator.</returns>
    public static TestRunResultAggregator Instance
        => s_instance ??= new TestRunResultAggregator();

    /// <summary>
    /// The current test run outcome.
    /// </summary>
    public TestOutcome Outcome { get; private set; }

    /// <summary>
    /// Registers to receive events from the provided test run request.
    /// These events will then be broadcast to any registered loggers.
    /// </summary>
    /// <param name="testRunRequest">The run request to register for events on.</param>
    public void RegisterTestRunEvents(ITestRunRequest testRunRequest)
    {
        ValidateArg.NotNull(testRunRequest, nameof(testRunRequest));
        // Register for the events.
        testRunRequest.TestRunMessage += TestRunMessageHandler;
        testRunRequest.OnRunCompletion += TestRunCompletionHandler;
    }

    /// <summary>
    /// Unregisters from the provided test run request to stop receiving events.
    /// </summary>
    /// <param name="testRunRequest">The run request from which events should be unregistered.</param>
    public void UnregisterTestRunEvents(ITestRunRequest testRunRequest)
    {
        ValidateArg.NotNull(testRunRequest, nameof(testRunRequest));
        testRunRequest.TestRunMessage -= TestRunMessageHandler;
        testRunRequest.OnRunCompletion -= TestRunCompletionHandler;
    }

    /// <summary>
    /// To mark the test run as failed.
    /// </summary>
    public void MarkTestRunFailed()
    {
        Outcome = TestOutcome.Failed;
    }

    /// <summary>
    /// Resets the outcome to default state i.e. Passed
    /// </summary>
    public void Reset()
    {
        Outcome = TestOutcome.Passed;
    }

    /// <summary>
    /// Called when a test run is complete.
    /// </summary>
    private void TestRunCompletionHandler(object? sender, TestRunCompleteEventArgs e)
    {
        if (e.TestRunStatistics == null || e.IsCanceled || e.IsAborted)
        {
            Outcome = TestOutcome.Failed;
        }
        else if (e.TestRunStatistics[TestOutcome.Failed] > 0)
        {
            Outcome = TestOutcome.Failed;
        }
    }

    /// <summary>
    /// Called when a test run message is sent.
    /// </summary>
    private void TestRunMessageHandler(object? sender, TestRunMessageEventArgs e)
    {
        if (e.Level == TestMessageLevel.Error)
        {
            Outcome = TestOutcome.Failed;
        }
    }

}
