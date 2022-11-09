// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;
using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.DataCollection;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.DataCollection.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Extensions;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Utilities;

using CommunicationUtilitiesResources = Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources.Resources;
using CoreUtilitiesConstants = Microsoft.VisualStudio.TestPlatform.CoreUtilities.Constants;
using CrossPlatEngineResources = Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection;

/// <summary>
/// Managed datacollector interaction from runner process.
/// </summary>
internal class ProxyDataCollectionManager : IProxyDataCollectionManager
{
    private const string PortOption = "--port";
    private const string DiagOption = "--diag";
    private const string ParentProcessIdOption = "--parentprocessid";
    private const string TraceLevelOption = "--tracelevel";
    public const string DebugEnvironmentVaribleName = "VSTEST_DATACOLLECTOR_DEBUG";

    private readonly IDataCollectionRequestSender _dataCollectionRequestSender;
    private readonly IDataCollectionLauncher _dataCollectionLauncher;
    private readonly IProcessHelper _processHelper;
    private readonly IRequestData _requestData;
    private int _dataCollectionPort;
    private int _dataCollectionProcessId;

    /// <summary>
    /// The settings xml
    /// </summary>
    public string? SettingsXml { get; }

    /// <summary>
    /// List of test sources
    /// </summary>
    public IEnumerable<string> Sources { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProxyDataCollectionManager"/> class.
    /// </summary>
    /// <param name="requestData">
    /// Request Data providing common execution/discovery services.
    /// </param>
    /// <param name="settingsXml">
    ///     Runsettings that contains the datacollector related configuration.
    /// </param>
    /// <param name="sources">
    ///     Test Run sources
    /// </param>
    public ProxyDataCollectionManager(IRequestData requestData, string? settingsXml, IEnumerable<string> sources)
        : this(requestData, settingsXml, sources, new ProcessHelper())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProxyDataCollectionManager"/> class.
    /// </summary>
    /// <param name="requestData">
    ///     Request Data providing common execution/discovery services.
    /// </param>
    /// <param name="settingsXml">
    ///     The settings xml.
    /// </param>
    /// <param name="sources">
    ///     Test Run sources
    /// </param>
    /// <param name="processHelper">
    ///     The process helper.
    /// </param>
    internal ProxyDataCollectionManager(IRequestData requestData, string? settingsXml, IEnumerable<string> sources, IProcessHelper processHelper)
        : this(requestData, settingsXml, sources, new DataCollectionRequestSender(), processHelper, DataCollectionLauncherFactory.GetDataCollectorLauncher(processHelper, settingsXml))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProxyDataCollectionManager"/> class.
    /// </summary>
    /// <param name="requestData">
    ///     Request Data providing common execution/discovery services.
    /// </param>
    /// <param name="settingsXml">
    ///     Runsettings that contains the datacollector related configuration.
    /// </param>
    /// <param name="sources">
    ///     Test Run sources
    /// </param>
    /// <param name="dataCollectionRequestSender">
    ///     Handles communication with datacollector process.
    /// </param>
    /// <param name="processHelper">
    ///     The process Helper.
    /// </param>
    /// <param name="dataCollectionLauncher">
    ///     Launches datacollector process.
    /// </param>
    internal ProxyDataCollectionManager(IRequestData requestData, string? settingsXml,
        IEnumerable<string> sources,
        IDataCollectionRequestSender dataCollectionRequestSender, IProcessHelper processHelper,
        IDataCollectionLauncher dataCollectionLauncher)
    {
        // DataCollector process needs the information of the Extensions folder
        // Add the Extensions folder path to runsettings.
        SettingsXml = UpdateExtensionsFolderInRunSettings(settingsXml);
        Sources = sources;
        _requestData = requestData;

        _dataCollectionRequestSender = dataCollectionRequestSender;
        _dataCollectionLauncher = dataCollectionLauncher;
        _processHelper = processHelper;
        LogEnabledDataCollectors();
    }

    /// <summary>
    /// Invoked after ending of test run
    /// </summary>
    /// <param name="isCanceled">
    /// The is Canceled.
    /// </param>
    /// <param name="runEventsHandler">
    /// The run Events Handler.
    /// </param>
    /// <returns>
    /// The <see cref="DataCollectionResult"/>.
    /// </returns>
    public DataCollectionResult AfterTestRunEnd(bool isCanceled, ITestMessageEventHandler? runEventsHandler)
    {
        AfterTestRunEndResult? afterTestRunEnd = null;
        InvokeDataCollectionServiceAction(
            () =>
            {
                EqtTrace.Info("ProxyDataCollectionManager.AfterTestRunEnd: Get attachment set and invoked data collectors processId: {0} port: {1}", _dataCollectionProcessId, _dataCollectionPort);
                afterTestRunEnd = _dataCollectionRequestSender.SendAfterTestRunEndAndGetResult(runEventsHandler, isCanceled);
            },
            runEventsHandler);

        if (_requestData.IsTelemetryOptedIn && afterTestRunEnd?.Metrics != null)
        {
            foreach (var metric in afterTestRunEnd.Metrics)
            {
                _requestData.MetricsCollection.Add(metric.Key, metric.Value);
            }
        }

        return new DataCollectionResult(afterTestRunEnd?.AttachmentSets, afterTestRunEnd?.InvokedDataCollectors);
    }

    /// <summary>
    /// Invoked before starting of test run
    /// </summary>
    /// <param name="resetDataCollectors">
    /// The reset Data Collectors.
    /// </param>
    /// <param name="isRunStartingNow">
    /// The is Run Starting Now.
    /// </param>
    /// <param name="runEventsHandler">
    /// The run Events Handler.
    /// </param>
    /// <returns>
    /// BeforeTestRunStartResult object
    /// </returns>
    public DataCollectionParameters BeforeTestRunStart(
        bool resetDataCollectors,
        bool isRunStartingNow,
        ITestMessageEventHandler? runEventsHandler)
    {
        var areTestCaseLevelEventsRequired = false;
        IDictionary<string, string?> environmentVariables = new Dictionary<string, string?>();

        var dataCollectionEventsPort = 0;
        InvokeDataCollectionServiceAction(
            () =>
            {
                EqtTrace.Info("ProxyDataCollectionManager.BeforeTestRunStart: Get environment variable and port for datacollector processId: {0} port: {1}", _dataCollectionProcessId, _dataCollectionPort);
                var result = _dataCollectionRequestSender.SendBeforeTestRunStartAndGetResult(SettingsXml, Sources, _requestData.IsTelemetryOptedIn, runEventsHandler);
                TPDebug.Assert(result is not null, "result is null");
                environmentVariables = result.EnvironmentVariables;
                dataCollectionEventsPort = result.DataCollectionEventsPort;

                EqtTrace.Info(
                    "ProxyDataCollectionManager.BeforeTestRunStart: SendBeforeTestRunStartAndGetResult successful, env variable from datacollector: {0}  and testhost port: {1}",
                    string.Join(";", environmentVariables),
                    dataCollectionEventsPort);
            },
            runEventsHandler);
        return new DataCollectionParameters(
            areTestCaseLevelEventsRequired,
            environmentVariables,
            dataCollectionEventsPort);
    }

    /// <inheritdoc />
    public void TestHostLaunched(int processId)
    {
        var payload = new TestHostLaunchedPayload();
        payload.ProcessId = processId;

        _dataCollectionRequestSender.SendTestHostLaunched(payload);
    }

    /// <summary>
    /// The dispose.
    /// </summary>
    public void Dispose()
    {
        EqtTrace.Info("ProxyDataCollectionManager.Dispose: calling dispose for datacollector processId: {0} port: {1}", _dataCollectionProcessId, _dataCollectionPort);
        _dataCollectionRequestSender.Close();
    }

    /// <inheritdoc />
    public void Initialize()
    {
        _dataCollectionPort = _dataCollectionRequestSender.InitializeCommunication();

        // Warn the user that execution will wait for debugger attach.
        _dataCollectionProcessId = _dataCollectionLauncher.LaunchDataCollector(null, GetCommandLineArguments(_dataCollectionPort));
        EqtTrace.Info("ProxyDataCollectionManager.Initialize: Launched datacollector processId: {0} port: {1}", _dataCollectionProcessId, _dataCollectionPort);

        var connectionTimeout = GetConnectionTimeout(_dataCollectionProcessId);

        EqtTrace.Info("ProxyDataCollectionManager.Initialize: waiting for connection with timeout: {0} seconds", connectionTimeout);

        var connected = _dataCollectionRequestSender.WaitForRequestHandlerConnection(connectionTimeout * 1000);
        if (connected == false)
        {
            EqtTrace.Error("ProxyDataCollectionManager.Initialize: failed to connect to datacollector process, processId: {0} port: {1}", _dataCollectionProcessId, _dataCollectionPort);
            throw new TestPlatformException(
                string.Format(
                    CultureInfo.CurrentCulture,
                    CommunicationUtilitiesResources.ConnectionTimeoutErrorMessage,
                    CoreUtilitiesConstants.VstestConsoleProcessName,
                    CoreUtilitiesConstants.DatacollectorProcessName,
                    connectionTimeout,
                    EnvironmentHelper.VstestConnectionTimeout)
            );
        }
    }

    private int GetConnectionTimeout(int processId)
    {
        var connectionTimeout = EnvironmentHelper.GetConnectionTimeout();

        // Increase connection timeout when debugging is enabled.
        var dataCollectorDebugEnabled = Environment.GetEnvironmentVariable(DebugEnvironmentVaribleName);
        if (!StringUtils.IsNullOrEmpty(dataCollectorDebugEnabled) &&
            dataCollectorDebugEnabled.Equals("1", StringComparison.Ordinal))
        {
            ConsoleOutput.Instance.WriteLine(CrossPlatEngineResources.DataCollectorDebuggerWarning, OutputLevel.Warning);
            ConsoleOutput.Instance.WriteLine(
                string.Format(CultureInfo.InvariantCulture, "Process Id: {0}, Name: {1}", processId, _processHelper.GetProcessName(processId)),
                OutputLevel.Information);

            // Increase connection timeout when debugging is enabled.
            connectionTimeout *= 5;
        }

        return connectionTimeout;
    }

    private static void InvokeDataCollectionServiceAction(Action action, ITestMessageEventHandler? runEventsHandler)
    {
        try
        {
            EqtTrace.Verbose("ProxyDataCollectionManager.InvokeDataCollectionServiceAction: Starting.");
            action();
            EqtTrace.Info("ProxyDataCollectionManager.InvokeDataCollectionServiceAction: Completed.");
        }
        catch (Exception ex)
        {
            EqtTrace.Warning("ProxyDataCollectionManager.InvokeDataCollectionServiceAction: TestPlatformException = {0}.", ex);
            HandleExceptionMessage(runEventsHandler, ex);
        }
    }

    private static void HandleExceptionMessage(ITestMessageEventHandler? runEventsHandler, Exception exception)
    {
        EqtTrace.Error(exception);
        runEventsHandler?.HandleLogMessage(ObjectModel.Logging.TestMessageLevel.Error, exception.ToString());
    }

    private IList<string> GetCommandLineArguments(int portNumber)
    {
        var commandlineArguments = new List<string>
        {
            PortOption,
            portNumber.ToString(CultureInfo.CurrentCulture),

            ParentProcessIdOption,
            _processHelper.GetCurrentProcessId().ToString(CultureInfo.CurrentCulture)
        };

        if (!StringUtils.IsNullOrEmpty(EqtTrace.LogFile))
        {
            commandlineArguments.Add(DiagOption);
            commandlineArguments.Add(GetTimestampedLogFile(EqtTrace.LogFile));

            commandlineArguments.Add(TraceLevelOption);
            commandlineArguments.Add(((int)EqtTrace.TraceLevel).ToString(CultureInfo.CurrentCulture));
        }

        return commandlineArguments;
    }

    private static string GetTimestampedLogFile(string logFile)
    {
        return Path.ChangeExtension(
            logFile,
            string.Format(
                CultureInfo.InvariantCulture,
                "datacollector.{0}_{1}{2}",
                DateTime.Now.ToString("yy-MM-dd_HH-mm-ss_fffff", CultureInfo.CurrentCulture),
                new PlatformEnvironment().GetCurrentManagedThreadId(),
                Path.GetExtension(logFile))).AddDoubleQuote();
    }

    /// <summary>
    /// Update Extensions path folder in test adapters paths in runsettings.
    /// </summary>
    /// <param name="settingsXml"></param>
    private static string? UpdateExtensionsFolderInRunSettings(string? settingsXml)
    {
        if (settingsXml.IsNullOrWhiteSpace())
        {
            return settingsXml;
        }

        var extensionsFolder = Path.Combine(Path.GetDirectoryName(typeof(ITestPlatform).GetTypeInfo().Assembly.GetAssemblyLocation())!, "Extensions");

        using var stream = new StringReader(settingsXml);
        using var reader = XmlReader.Create(stream, XmlRunSettingsUtilities.ReaderSettings);
        var document = new XmlDocument();
        document.Load(reader);

        var tapNode = RunSettingsProviderExtensions.GetXmlNode(document, "RunConfiguration.TestAdaptersPaths");

        if (tapNode != null && !StringUtils.IsNullOrWhiteSpace(tapNode.InnerText))
        {
            extensionsFolder = string.Concat(tapNode.InnerText, ';', extensionsFolder);
        }

        RunSettingsProviderExtensions.UpdateRunSettingsXmlDocumentInnerText(document, "RunConfiguration.TestAdaptersPaths", extensionsFolder);

        return document.OuterXml;
    }

    /// <summary>
    /// Log Enabled Data Collectors
    /// </summary>
    private void LogEnabledDataCollectors()
    {
        if (!_requestData.IsTelemetryOptedIn)
        {
            return;
        }

        var dataCollectionSettings = XmlRunSettingsUtilities.GetDataCollectionRunSettings(SettingsXml);

        if (dataCollectionSettings == null || !dataCollectionSettings.IsCollectionEnabled)
        {
            return;
        }

        var enabledDataCollectors = new List<DataCollectorSettings>();
        foreach (var settings in dataCollectionSettings.DataCollectorSettingsList)
        {
            if (settings.IsEnabled)
            {
                if (enabledDataCollectors.Any(dcSettings => string.Equals(dcSettings.FriendlyName, settings.FriendlyName, StringComparison.OrdinalIgnoreCase)))
                {
                    // If Uri or assembly qualified type name is repeated, consider data collector as duplicate and ignore it.
                    continue;
                }

                enabledDataCollectors.Add(settings);
            }
        }

        var dataCollectors = enabledDataCollectors.Select(x => new { x.FriendlyName, x.Uri }.ToString());
        _requestData.MetricsCollection.Add(TelemetryDataConstants.DataCollectorsEnabled, string.Join(",", dataCollectors.ToArray()));
    }
}
