// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;

/// <summary>
/// Orchestrates test execution related functionality for the engine communicating with the client.
/// </summary>
public interface IProxyExecutionManager
{
    /// <summary>
    /// Gets whether current Execution Manager is initialized or not
    /// </summary>
    bool IsInitialized { get; }

    /// <summary>
    /// Initializes test execution. Create the test host, setup channel and initialize extensions.
    /// <param name="skipDefaultAdapters">Skip default adapters flag.</param>
    /// </summary>
    void Initialize(bool skipDefaultAdapters);

    /// <summary>
    /// Initializes test execution. Create the test host, setup channel and initialize extensions.
    /// </summary>
    /// 
    /// <param name="testRunCriteria">The settings/options for the test run.</param>
    /// <param name="eventHandler">EventHandler for handling execution events from Engine.</param>
    void InitializeTestRun(TestRunCriteria testRunCriteria, IInternalTestRunEventsHandler eventHandler);

    /// <summary>
    /// Starts the test run.
    /// </summary>
    /// <param name="testRunCriteria">The settings/options for the test run.</param>
    /// <param name="eventHandler">EventHandler for handling execution events from Engine.</param>
    /// <returns>The process id of the runner executing tests.</returns>
    int StartTestRun(TestRunCriteria testRunCriteria, IInternalTestRunEventsHandler eventHandler);

    /// <summary>
    /// Cancels the test run. On the test host, this will send a message to adapters.
    /// </summary>
    // <param name="eventHandler"> EventHandler for handling execution events from Engine. </param>
    void Cancel(IInternalTestRunEventsHandler eventHandler);

    /// <summary>
    /// Aborts the test operation. This will forcefully terminate the test host.
    /// </summary>
    // <param name="eventHandler"> EventHandler for handling execution events from Engine. </param>
    void Abort(IInternalTestRunEventsHandler eventHandler);

    /// <summary>
    /// Closes the current test operation by sending a end session message.
    /// Terminates the test host.
    /// </summary>
    void Close();
}
