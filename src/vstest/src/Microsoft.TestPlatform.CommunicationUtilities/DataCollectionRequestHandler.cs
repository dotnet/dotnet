// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollector;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollector.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.DataCollection.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

using CommunicationUtilitiesResources = Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources.Resources;
using CoreUtilitiesConstants = Microsoft.VisualStudio.TestPlatform.CoreUtilities.Constants;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.DataCollection;

/// <summary>
/// Handles test session events received from vstest console process.
/// </summary>
internal class DataCollectionRequestHandler : IDataCollectionRequestHandler, IDisposable
{
    private static readonly object SyncObject = new();

    private readonly ICommunicationManager _communicationManager;
    private readonly IMessageSink _messageSink;
    private readonly IDataCollectionManager _dataCollectionManager;
    private readonly IDataCollectionTestCaseEventHandler _dataCollectionTestCaseEventHandler;
    private readonly IDataSerializer _dataSerializer;
    private readonly IFileHelper _fileHelper;
    private readonly IRequestData _requestData;

    private Task? _testCaseEventMonitorTask;

    /// <summary>
    /// Use to cancel data collection test case events monitoring if test run is canceled.
    /// </summary>
    private readonly CancellationTokenSource _cancellationTokenSource;

    /// <summary>
    /// Initializes a new instance of the <see cref="DataCollectionRequestHandler"/> class.
    /// </summary>
    /// <param name="communicationManager">
    /// The communication manager.
    /// </param>
    /// <param name="messageSink">
    /// The message sink.
    /// </param>
    /// <param name="dataCollectionManager">
    /// The data collection manager.
    /// </param>
    /// <param name="dataCollectionTestCaseEventHandler">
    /// The data collection test case event handler.
    /// </param>
    /// <param name="dataSerializer">
    /// Serializer for serialization and deserialization of the messages.
    /// </param>
    /// <param name="fileHelper">
    /// File Helper
    /// </param>
    /// <param name="requestData">
    /// Request data
    /// </param>
    protected DataCollectionRequestHandler(
        ICommunicationManager communicationManager,
        IMessageSink messageSink,
        IDataCollectionManager dataCollectionManager,
        IDataCollectionTestCaseEventHandler dataCollectionTestCaseEventHandler,
        IDataSerializer dataSerializer,
        IFileHelper fileHelper,
        IRequestData requestData)
    {
        _communicationManager = communicationManager;
        _messageSink = messageSink;
        _dataCollectionManager = dataCollectionManager;
        _dataSerializer = dataSerializer;
        _dataCollectionTestCaseEventHandler = dataCollectionTestCaseEventHandler;
        _cancellationTokenSource = new CancellationTokenSource();
        _fileHelper = fileHelper;
        _requestData = requestData;
    }

    /// <summary>
    /// Gets the singleton instance of DataCollectionRequestHandler.
    /// </summary>
    public static DataCollectionRequestHandler? Instance { get; private set; }

    /// <summary>
    /// Creates singleton instance of DataCollectionRequestHandler.
    /// </summary>
    /// <param name="communicationManager">
    /// Handles socket communication.
    /// </param>
    /// <param name="messageSink">
    /// Message sink for sending messages to execution process.
    /// </param>
    /// <returns>
    /// The instance of <see cref="DataCollectionRequestHandler"/>.
    /// </returns>
    public static DataCollectionRequestHandler Create(
        ICommunicationManager communicationManager,
        IMessageSink messageSink)
    {
        ValidateArg.NotNull(communicationManager, nameof(communicationManager));
        ValidateArg.NotNull(messageSink, nameof(messageSink));

        // TODO: The MessageSink and DataCollectionRequestHandler have circular dependency.
        // Message sink is injected into this Create method and then into constructor
        // and into the constructor of DataCollectionRequestHandler. Data collection manager
        // is then assigned to .Instace (which unlike many other .Instance is not populated
        // directly in that property, but is created here). And then MessageSink depends on
        // the .Instance. This is a very complicated way of solving the circular dependency,
        // and should be replaced by adding a property to Message and assigning it.
        // .Instance can then be removed.
        if (Instance == null)
        {
            lock (SyncObject)
            {
                if (Instance == null)
                {
                    var requestData = new RequestData();
                    var telemetryReporter = new TelemetryReporter(requestData, communicationManager, JsonDataSerializer.Instance);

                    Instance = new DataCollectionRequestHandler(
                        communicationManager,
                        messageSink,
                        DataCollectionManager.Create(messageSink, requestData, telemetryReporter),
                        new DataCollectionTestCaseEventHandler(messageSink),
                        JsonDataSerializer.Instance,
                        new FileHelper(),
                        requestData);
                }
            }
        }

        return Instance;
    }

    /// <inheritdoc />
    public void InitializeCommunication(int port)
    {
        _communicationManager.SetupClientAsync(new IPEndPoint(IPAddress.Loopback, port));
    }

    /// <inheritdoc />
    public bool WaitForRequestSenderConnection(int connectionTimeout)
    {
        return _communicationManager.WaitForServerConnection(connectionTimeout);
    }

    /// <summary>
    /// Process requests.
    /// </summary>
    public void ProcessRequests()
    {
        var isSessionEnded = false;

        do
        {
            var message = _communicationManager.ReceiveMessage();

            EqtTrace.Info("DataCollectionRequestHandler.ProcessRequests: Datacollector received message: {0}", message);

            switch (message?.MessageType)
            {
                case MessageType.BeforeTestRunStart:
                    HandleBeforeTestRunStart(message);
                    break;

                case MessageType.AfterTestRunEnd:
                    HandleAfterTestRunEnd(message);
                    isSessionEnded = true;
                    break;

                case MessageType.TestHostLaunched:
                    var testHostLaunchedPayload = _dataSerializer.DeserializePayload<TestHostLaunchedPayload>(message);
                    TPDebug.Assert(testHostLaunchedPayload is not null, "testHostLaunchedPayload is null");
                    _dataCollectionManager.TestHostLaunched(testHostLaunchedPayload.ProcessId);
                    break;

                default:
                    EqtTrace.Error("DataCollectionRequestHandler.ProcessRequests : Invalid Message types: {0}", message?.MessageType);
                    break;
            }
        }
        while (!isSessionEnded);
    }

    /// <summary>
    /// Sends datacollection message.
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    public void SendDataCollectionMessage(DataCollectionMessageEventArgs args)
    {
        _communicationManager.SendMessage(MessageType.DataCollectionMessage, args);
    }

    /// <summary>
    /// The dispose.
    /// </summary>
    public void Dispose()
    {
        _communicationManager?.StopClient();
    }

    /// <summary>
    /// Closes the connection
    /// </summary>
    public void Close()
    {
        Dispose();
        EqtTrace.Info("Closing the connection !");
    }

    /// <summary>
    /// Update the test adapter paths provided through run settings to be used by the test plugin cache.
    /// </summary>
    /// <param name="payload">
    /// The before test run start payload
    /// </param>
    private void AddExtensionAssemblies(BeforeTestRunStartPayload payload)
    {
        try
        {
            TPDebug.Assert(payload is not null, "payload is null");

            if (payload.Sources is null)
            {
                EqtTrace.Verbose("DataCollectionRequestHandler.AddExtensionAssemblies: No sources provided");
                return;
            }

            // In case of dotnet vstest with code coverage, data collector needs to be picked up from publish folder.
            // Therefore, adding source dll folders to search datacollectors in these.
            var datacollectorSearchPaths = new HashSet<string>();
            foreach (var source in payload.Sources)
            {
                datacollectorSearchPaths.Add(Path.GetDirectoryName(source)!);
            }

            var customTestAdaptersPaths = RunSettingsUtilities.GetTestAdaptersPaths(payload.SettingsXml);
            if (customTestAdaptersPaths != null)
            {
                datacollectorSearchPaths.UnionWith(customTestAdaptersPaths);
            }

            List<string> extensionAssemblies = new();
            foreach (var datacollectorSearchPath in datacollectorSearchPaths)
            {
                var adapterPath =
                    Path.GetFullPath(Environment.ExpandEnvironmentVariables(datacollectorSearchPath));
                if (!_fileHelper.DirectoryExists(adapterPath))
                {
                    EqtTrace.Warning($"AdapterPath Not Found: {adapterPath}");
                    continue;
                }

                extensionAssemblies.AddRange(
                    _fileHelper.EnumerateFiles(
                        adapterPath,
                        SearchOption.AllDirectories,
                        TestPlatformConstants.DataCollectorEndsWithPattern));
            }

            if (extensionAssemblies.Count > 0)
            {
                TestPluginCache.Instance.UpdateExtensions(extensionAssemblies, skipExtensionFilters: false);
            }
        }
        catch (Exception e)
        {
            // If any exception is thrown while updating additional assemblies, log the exception in eqt trace.
            EqtTrace.Error("DataCollectionRequestHandler.AddExtensionAssemblies: Exception occurred: {0}", e);
        }
    }

    private void HandleBeforeTestRunStart(Message message)
    {
        // Initialize datacollectors and get environment variables.
        var payload = _dataSerializer.DeserializePayload<BeforeTestRunStartPayload>(message);
        TPDebug.Assert(payload is not null, "payload is null");
        UpdateRequestData(payload.IsTelemetryOptedIn);
        AddExtensionAssemblies(payload);

        TPDebug.Assert(payload.SettingsXml is not null, "payload.SettingsXml is null");
        var envVariables = _dataCollectionManager.InitializeDataCollectors(payload.SettingsXml);

        var properties = new Dictionary<string, object?>
        {
            { CoreUtilitiesConstants.TestSourcesKeyName, payload.Sources }
        };
        var eventArgs = new SessionStartEventArgs(properties);

        var areTestCaseLevelEventsRequired = _dataCollectionManager.SessionStarted(eventArgs);

        // Open a socket communication port for test level events.
        var testCaseEventsPort = 0;
        if (areTestCaseLevelEventsRequired)
        {
            testCaseEventsPort = _dataCollectionTestCaseEventHandler.InitializeCommunication();

            _testCaseEventMonitorTask = Task.Factory.StartNew(
                () =>
                {
                    try
                    {
                        var timeout = EnvironmentHelper.GetConnectionTimeout();
                        if (_dataCollectionTestCaseEventHandler.WaitForRequestHandlerConnection(
                                timeout * 1000))
                        {
                            _dataCollectionTestCaseEventHandler.ProcessRequests();
                        }
                        else
                        {
                            EqtTrace.Error(
                                "DataCollectionRequestHandler.HandleBeforeTestRunStart: TestCaseEventHandler timed out while connecting to the Sender.");
                            _dataCollectionTestCaseEventHandler.Close();
                            throw new TestPlatformException(
                                string.Format(
                                    CultureInfo.CurrentCulture,
                                    CommunicationUtilitiesResources.ConnectionTimeoutErrorMessage,
                                    CoreUtilitiesConstants.DatacollectorProcessName,
                                    CoreUtilitiesConstants.TesthostProcessName,
                                    timeout,
                                    EnvironmentHelper.VstestConnectionTimeout));
                        }
                    }
                    catch (Exception e)
                    {
                        EqtTrace.Error("DataCollectionRequestHandler.HandleBeforeTestRunStart : Error occurred during test case events handling: {0}.", e);
                    }
                },
                _cancellationTokenSource.Token);
        }

        _communicationManager.SendMessage(
            MessageType.BeforeTestRunStartResult,
            new BeforeTestRunStartResult(envVariables, testCaseEventsPort));

        EqtTrace.Info("DataCollectionRequestHandler.ProcessRequests : DataCollection started.");
    }

    private void HandleAfterTestRunEnd(Message message)
    {
        var isCancelled = _dataSerializer.DeserializePayload<bool>(message);

        if (isCancelled)
        {
            _cancellationTokenSource.Cancel();
        }

        try
        {
            _testCaseEventMonitorTask?.Wait(_cancellationTokenSource.Token);
            _dataCollectionTestCaseEventHandler.Close();
        }
        catch (Exception ex)
        {
            EqtTrace.Error("DataCollectionRequestHandler.HandleAfterTestRunEnd : Error while processing event from testhost: {0}", ex.ToString());
        }

        var attachmentsets = _dataCollectionManager.SessionEnded(isCancelled);
        var invokedDataCollectors = _dataCollectionManager.GetInvokedDataCollectors();

        if (invokedDataCollectors != null && invokedDataCollectors.Count != 0)
        {
            // For the invoked collectors we report the same information as ProxyDataCollectionManager.cs line ~416
            var invokedDataCollectorsForMetrics = invokedDataCollectors.Select(x => new { x.Uri, x.FriendlyName, x.HasAttachmentProcessor }.ToString());
            _requestData.MetricsCollection.Add(TelemetryDataConstants.InvokedDataCollectors, string.Join(",", invokedDataCollectorsForMetrics.ToArray()));
        }

        var afterTestRunEndResult = new AfterTestRunEndResult(attachmentsets, invokedDataCollectors, _requestData.MetricsCollection.Metrics);

        // Dispose all datacollectors before sending attachments to vstest.console process.
        // As datacollector process exits itself on parent process(vstest.console) exits.
        _dataCollectionManager?.Dispose();

        _communicationManager.SendMessage(MessageType.AfterTestRunEndResult, afterTestRunEndResult);
        EqtTrace.Info("DataCollectionRequestHandler.ProcessRequests : Session End message received from server. Closing the connection.");

        Close();

        EqtTrace.Info("DataCollectionRequestHandler.ProcessRequests : DataCollection completed");
    }

    private void UpdateRequestData(bool isTelemetryOptedIn)
    {
        if (isTelemetryOptedIn != _requestData.IsTelemetryOptedIn)
        {
            _requestData.MetricsCollection = isTelemetryOptedIn ? (IMetricsCollection)new MetricsCollection() : new NoOpMetricsCollection();
            _requestData.IsTelemetryOptedIn = isTelemetryOptedIn;
        }
    }
}
