// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.ClientProtocol;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.TesthostProtocol;

/// <summary>
/// Orchestrates test execution related functionality for the engine communicating with the test host process.
/// </summary>
public interface IExecutionManager
{
    /// <summary>
    /// Initializes the execution manager.
    /// </summary>
    /// <param name="pathToAdditionalExtensions"> The path to additional extensions. </param>
    void Initialize(IEnumerable<string> pathToAdditionalExtensions, ITestMessageEventHandler? testMessageEventsHandler);

    /// <summary>
    /// Starts the test run with sources.
    /// </summary>
    /// <param name="adapterSourceMap"> The adapter Source Map.  </param>
    /// <param name="package"> The packages which actually contain sources. A testhost can at max execute for one package at time
    /// Package can be null if test source, and package are same
    /// </param>
    /// <param name="runSettings"> The run Settings.  </param>
    /// <param name="testExecutionContext"> The test Execution Context. </param>
    /// <param name="testCaseEvents"> EventHandler for handling test cases level events from Engine. </param>
    /// <param name="eventHandler"> EventHandler for handling execution events from Engine. </param>
    void StartTestRun(Dictionary<string, IEnumerable<string>> adapterSourceMap, string? package, string? runSettings, TestExecutionContext testExecutionContext, ITestCaseEventsHandler? testCaseEvents, IInternalTestRunEventsHandler eventHandler);

    /// <summary>
    /// Starts the test run with tests.
    /// </summary>
    /// <param name="tests"> The test list. </param>
    /// <param name="package"> The packages which actually contain sources. A testhost can at max execute for one package at time
    /// Package can be null if test source, and package are same
    /// </param>
    /// <param name="runSettings"> The run Settings.  </param>
    /// <param name="testExecutionContext"> The test Execution Context. </param>
    /// /// <param name="testCaseEvents"> EventHandler for handling test cases level events from Engine. </param>
    /// <param name="eventHandler"> EventHandler for handling execution events from Engine. </param>
    void StartTestRun(IEnumerable<TestCase> tests, string? package, string? runSettings, TestExecutionContext testExecutionContext, ITestCaseEventsHandler? testCaseEvents, IInternalTestRunEventsHandler eventHandler);

    /// <summary>
    /// Cancel the test execution.
    /// </summary>
    /// <param name="eventHandler"> EventHandler for handling execution events from Engine. </param>
    void Cancel(IInternalTestRunEventsHandler testRunEventsHandler);

    /// <summary>
    /// Aborts the test execution.
    /// </summary>
    /// <param name="eventHandler"> EventHandler for handling execution events from Engine. </param>
    void Abort(IInternalTestRunEventsHandler testRunEventsHandler);
}
