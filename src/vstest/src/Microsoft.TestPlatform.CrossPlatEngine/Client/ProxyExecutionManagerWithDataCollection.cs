// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client;

/// <summary>
/// The proxy execution manager with data collection.
/// </summary>
internal class ProxyExecutionManagerWithDataCollection : ProxyExecutionManager
{
    private IDictionary<string, string?>? _dataCollectionEnvironmentVariables;
    private int _dataCollectionPort;
    private readonly IRequestData _requestData;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProxyExecutionManagerWithDataCollection"/> class.
    /// </summary>
    /// <param name="requestSender">
    /// Test request sender instance.
    /// </param>
    /// <param name="testHostManager">
    /// Test host manager for this operation.
    /// </param>
    /// <param name="proxyDataCollectionManager">
    /// The proxy Data Collection Manager.
    /// </param>
    /// <param name="requestData">
    /// The request data for providing execution services and data.
    /// </param>
    public ProxyExecutionManagerWithDataCollection(
        IRequestData requestData,
        ITestRequestSender requestSender,
        ITestRuntimeProvider testHostManager,
        Framework testHostManagerFramework,
        IProxyDataCollectionManager proxyDataCollectionManager)
        : base(
            requestData,
            requestSender,
            testHostManager,
            testHostManagerFramework)
    {
        ProxyDataCollectionManager = proxyDataCollectionManager;
        DataCollectionRunEventsHandler = new DataCollectionRunEventsHandler();
        _requestData = requestData;
        _dataCollectionEnvironmentVariables = new Dictionary<string, string?>();

        testHostManager.HostLaunched += TestHostLaunchedHandler;
    }

    private void TestHostLaunchedHandler(object? sender, HostProviderEventArgs e)
    {
        ProxyDataCollectionManager.TestHostLaunched(e.ProcessId);
    }

    /// <summary>
    /// Gets the data collection run events handler.
    /// </summary>
    internal DataCollectionRunEventsHandler DataCollectionRunEventsHandler
    {
        get; private set;
    }

    /// <summary>
    /// Gets the proxy data collection manager.
    /// </summary>
    internal IProxyDataCollectionManager ProxyDataCollectionManager
    {
        get; private set;
    }

    /// <summary>
    /// Gets the cancellation token for execution.
    /// </summary>
    internal CancellationToken CancellationToken => CancellationTokenSource.Token;

    /// <summary>
    /// Ensure that the Execution component of engine is ready for execution usually by loading extensions.
    /// <param name="skipDefaultAdapters">Skip default adapters flag.</param>
    /// </summary>
    public override void Initialize(bool skipDefaultAdapters)
    {
        ProxyDataCollectionManager.Initialize();

        try
        {
            var dataCollectionParameters = ProxyDataCollectionManager.BeforeTestRunStart(
                resetDataCollectors: true,
                isRunStartingNow: true,
                runEventsHandler: DataCollectionRunEventsHandler);

            if (dataCollectionParameters != null)
            {
                _dataCollectionEnvironmentVariables = dataCollectionParameters.EnvironmentVariables;
                _dataCollectionPort = dataCollectionParameters.DataCollectionEventsPort;
            }
        }
        catch (Exception)
        {
            // On failure in calling BeforeTestRunStart, call AfterTestRunEnd to end DataCollectionProcess
            ProxyDataCollectionManager.AfterTestRunEnd(isCanceled: true, runEventsHandler: DataCollectionRunEventsHandler);
            throw;
        }

        base.Initialize(skipDefaultAdapters);
    }

    /// <summary>
    /// Starts the test run
    /// </summary>
    /// <param name="testRunCriteria"> The settings/options for the test run. </param>
    /// <param name="eventHandler"> EventHandler for handling execution events from Engine. </param>
    /// <returns> The process id of the runner executing tests. </returns>
    public override int StartTestRun(TestRunCriteria testRunCriteria, IInternalTestRunEventsHandler eventHandler)
    {
        var currentEventHandler = eventHandler;
        if (ProxyDataCollectionManager != null)
        {
            currentEventHandler = new DataCollectionTestRunEventsHandler(eventHandler, ProxyDataCollectionManager, CancellationTokenSource.Token);
        }

        // Log all the messages that are reported while initializing DataCollectionClient
        if (DataCollectionRunEventsHandler.Messages.Count > 0)
        {
            foreach (var message in DataCollectionRunEventsHandler.Messages)
            {
                currentEventHandler.HandleLogMessage(message.Item1, message.Item2);
            }

            DataCollectionRunEventsHandler.Messages.Clear();
        }

        // Push all raw messages
        if (DataCollectionRunEventsHandler.RawMessages.Count > 0)
        {
            foreach (var message in DataCollectionRunEventsHandler.RawMessages)
            {
                currentEventHandler.HandleRawMessage(message);
            }

            DataCollectionRunEventsHandler.RawMessages.Clear();
        }

        return base.StartTestRun(testRunCriteria, currentEventHandler);
    }

    public override int LaunchProcessWithDebuggerAttached(TestProcessStartInfo testProcessStartInfo)
    {
        if (_dataCollectionEnvironmentVariables != null)
        {
            testProcessStartInfo.EnvironmentVariables ??= new Dictionary<string, string?>();

            foreach (var envVariable in _dataCollectionEnvironmentVariables)
            {
                if (testProcessStartInfo.EnvironmentVariables.ContainsKey(envVariable.Key))
                {
                    testProcessStartInfo.EnvironmentVariables[envVariable.Key] = envVariable.Value;
                }
                else
                {
                    testProcessStartInfo.EnvironmentVariables.Add(envVariable.Key, envVariable.Value);
                }
            }
        }

        return base.LaunchProcessWithDebuggerAttached(testProcessStartInfo);
    }

    /// <inheritdoc />
    public override TestProcessStartInfo UpdateTestProcessStartInfo(TestProcessStartInfo testProcessStartInfo)
    {
        if (testProcessStartInfo.EnvironmentVariables == null)
        {
            testProcessStartInfo.EnvironmentVariables = _dataCollectionEnvironmentVariables;
        }
        else if (_dataCollectionEnvironmentVariables is not null)
        {
            foreach (var kvp in _dataCollectionEnvironmentVariables)
            {
                testProcessStartInfo.EnvironmentVariables[kvp.Key] = kvp.Value;
            }
        }

        // Update Telemetry Opt in status because by default in Test Host Telemetry is opted out
        var telemetryOptedIn = _requestData.IsTelemetryOptedIn ? "true" : "false";
        testProcessStartInfo.Arguments += $" --datacollectionport {_dataCollectionPort} --telemetryoptedin {telemetryOptedIn}";

        return testProcessStartInfo;
    }
}

/// <summary>
/// Handles Log and raw messages and stores them in list. Messages in the list will be logged after test execution begins.
/// </summary>
internal class DataCollectionRunEventsHandler : ITestMessageEventHandler
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataCollectionRunEventsHandler"/> class.
    /// </summary>
    public DataCollectionRunEventsHandler()
    {
        Messages = new List<Tuple<TestMessageLevel, string?>>();
        RawMessages = new List<string>();
    }

    /// <summary>
    /// Gets the cached messages.
    /// </summary>
    public List<Tuple<TestMessageLevel, string?>> Messages { get; private set; }

    /// <summary>
    /// Gets the cached raw messages.
    /// </summary>
    public List<string> RawMessages { get; private set; }

    /// <inheritdoc />
    public void HandleLogMessage(TestMessageLevel level, string? message)
    {
        Messages.Add(new Tuple<TestMessageLevel, string?>(level, message));
    }

    /// <inheritdoc />
    public void HandleRawMessage(string rawMessage)
    {
        RawMessages.Add(rawMessage);
    }
}
