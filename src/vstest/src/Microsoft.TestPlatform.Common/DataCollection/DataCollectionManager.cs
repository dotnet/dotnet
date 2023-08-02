// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.Common.DataCollector.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.Common.Logging;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;

namespace Microsoft.VisualStudio.TestPlatform.Common.DataCollector;

/// <summary>
/// Manages data collection.
/// </summary>
internal class DataCollectionManager : IDataCollectionManager
{
    private static readonly object SyncObject = new();
    private const string CodeCoverageFriendlyName = "Code Coverage";

    /// <summary>
    /// Value indicating whether data collection is currently enabled.
    /// </summary>
    private bool _isDataCollectionEnabled;

    /// <summary>
    /// Data collection environment context.
    /// </summary>
    private DataCollectionEnvironmentContext? _dataCollectionEnvironmentContext;

    /// <summary>
    /// Attachment manager for performing file transfers for datacollectors.
    /// </summary>
    private readonly IDataCollectionAttachmentManager _attachmentManager;

    /// <summary>
    /// Message sink for sending data collection messages to client..
    /// </summary>
    private readonly IMessageSink _messageSink;

    /// <summary>
    /// Events that can be subscribed by datacollectors.
    /// </summary>
    private readonly TestPlatformDataCollectionEvents _events;

    /// <summary>
    /// Telemetry reporter
    /// </summary>
    private readonly ITelemetryReporter _telemetryReporter;

    /// <summary>
    /// Specifies whether the object is disposed or not.
    /// </summary>
    private bool _isDisposed;

    /// <summary>
    /// Extension manager for data collectors.
    /// </summary>
    private DataCollectorExtensionManager? _dataCollectorExtensionManager;

    /// <summary>
    /// Request data
    /// </summary>
    private readonly IDataCollectionTelemetryManager _dataCollectionTelemetryManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="DataCollectionManager"/> class.
    /// </summary>
    /// <param name="messageSink">
    /// The message Sink.
    /// </param>
    internal DataCollectionManager(IMessageSink messageSink, IRequestData requestData, ITelemetryReporter telemetryReporter) : this(new DataCollectionAttachmentManager(), messageSink, new DataCollectionTelemetryManager(requestData), telemetryReporter)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataCollectionManager"/> class.
    /// </summary>
    /// <param name="datacollectionAttachmentManager">
    /// The datacollection Attachment Manager.
    /// </param>
    /// <param name="messageSink">
    /// The message Sink.
    /// </param>
    /// <remarks>
    /// The constructor is not public because the factory method should be used to get instances of this class.
    /// </remarks>
    protected DataCollectionManager(IDataCollectionAttachmentManager datacollectionAttachmentManager, IMessageSink messageSink, IDataCollectionTelemetryManager dataCollectionTelemetryManager, ITelemetryReporter telemetryReporter)
    {
        _attachmentManager = datacollectionAttachmentManager;
        _messageSink = messageSink;
        _events = new TestPlatformDataCollectionEvents();
        _dataCollectorExtensionManager = null;
        RunDataCollectors = new Dictionary<Type, DataCollectorInformation>();
        _dataCollectionTelemetryManager = dataCollectionTelemetryManager;
        _telemetryReporter = telemetryReporter;
    }

    /// <summary>
    /// Gets the instance of DataCollectionManager.
    /// </summary>
    public static DataCollectionManager? Instance { get; private set; }

    /// <summary>
    /// Gets cache of data collectors associated with the run.
    /// </summary>
    internal Dictionary<Type, DataCollectorInformation> RunDataCollectors { get; private set; }

    /// <summary>
    /// Gets the data collector extension manager.
    /// </summary>
    private DataCollectorExtensionManager DataCollectorExtensionManager
    {
        get
        {
            // TODO : change IMessageSink and use IMessageLogger instead.
            _dataCollectorExtensionManager ??= DataCollectorExtensionManager.Create(TestSessionMessageLogger.Instance);

            return _dataCollectorExtensionManager;
        }
    }

    /// <summary>
    /// Creates an instance of the TestLoggerExtensionManager.
    /// </summary>
    /// <param name="messageSink">
    /// The message sink.
    /// </param>
    /// <returns>
    /// The <see cref="DataCollectionManager"/>.
    /// </returns>
    public static DataCollectionManager Create(IMessageSink messageSink, IRequestData requestData, ITelemetryReporter telemetryReporter)
    {
        if (Instance == null)
        {
            lock (SyncObject)
            {
                Instance ??= new DataCollectionManager(messageSink, requestData, telemetryReporter);
            }
        }

        return Instance;
    }

    /// <inheritdoc/>
    public IDictionary<string, string?> InitializeDataCollectors(string settingsXml)
    {
        ValidateArg.NotNull(settingsXml, nameof(settingsXml));
        if (settingsXml.Length == 0)
        {
            EqtTrace.Info("DataCollectionManager.InitializeDataCollectors: Runsettings is empty.");
        }

        var sessionId = new SessionId(Guid.NewGuid());
        var dataCollectionContext = new DataCollectionContext(sessionId);
        _dataCollectionEnvironmentContext = DataCollectionEnvironmentContext.CreateForLocalEnvironment(dataCollectionContext);

        var runConfiguration = XmlRunSettingsUtilities.GetRunConfigurationNode(settingsXml);
        var resultsDirectory = RunSettingsUtilities.GetTestResultsDirectory(runConfiguration);

        _attachmentManager.Initialize(sessionId, resultsDirectory, _messageSink);

        // Environment variables are passed to testhost process, through ProcessStartInfo.EnvironmentVariables, which handles the key in a case-insensitive manner, which is translated to lowercase.
        // Therefore, using StringComparer.OrdinalIgnoreCase so that same keys with different cases are treated as same.
        var executionEnvironmentVariables = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

        var dataCollectionRunSettings = XmlRunSettingsUtilities.GetDataCollectionRunSettings(settingsXml);

        _isDataCollectionEnabled = dataCollectionRunSettings?.IsCollectionEnabled ?? false;

        // If dataCollectionRunSettings is null, that means datacollectors are not configured.
        if (dataCollectionRunSettings == null || !dataCollectionRunSettings.IsCollectionEnabled)
        {
            return executionEnvironmentVariables;
        }

        // Get settings for each data collector, load and initialize the data collectors.
        var enabledDataCollectorsSettings = GetDataCollectorsEnabledForRun(dataCollectionRunSettings);
        if (enabledDataCollectorsSettings == null || enabledDataCollectorsSettings.Count == 0)
        {
            return executionEnvironmentVariables;
        }

        foreach (var dataCollectorSettings in enabledDataCollectorsSettings)
        {
            LoadAndInitialize(dataCollectorSettings, settingsXml);
        }

        // Once all data collectors have been initialized, query for environment variables
        var dataCollectorEnvironmentVariables = GetEnvironmentVariables(out _);

        foreach (var variable in dataCollectorEnvironmentVariables.Values)
        {
            executionEnvironmentVariables.Add(variable.Name, variable.Value);
        }

        return executionEnvironmentVariables;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);

        // Use SupressFinalize in case a subclass
        // of this type implements a finalizer.
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    public Collection<AttachmentSet> SessionEnded(bool isCancelled = false)
    {
        // Return null if datacollection is not enabled.
        if (!_isDataCollectionEnabled)
        {
            return new Collection<AttachmentSet>();
        }

        if (isCancelled)
        {
            _attachmentManager.Cancel();
            return new Collection<AttachmentSet>();
        }

        TPDebug.Assert(_dataCollectionEnvironmentContext is not null, "_dataCollectionEnvironmentContext is null");
        var endEvent = new SessionEndEventArgs(_dataCollectionEnvironmentContext.SessionDataCollectionContext);
        SendEvent(endEvent);

        var result = new List<AttachmentSet>();
        try
        {
            result = _attachmentManager.GetAttachments(endEvent.Context);
        }
        catch (Exception ex)
        {
            EqtTrace.Error("DataCollectionManager.SessionEnded: Failed to get attachments : {0}", ex);

            return new Collection<AttachmentSet>(result);
        }

        if (EqtTrace.IsVerboseEnabled)
        {
            LogAttachments(result);
        }

        return new Collection<AttachmentSet>(result);
    }

    /// <inheritdoc/>
    public Collection<InvokedDataCollector> GetInvokedDataCollectors()
    {
        List<InvokedDataCollector> invokedDataCollector = new();
        foreach (DataCollectorInformation dataCollectorInformation in RunDataCollectors.Values)
        {
            invokedDataCollector.Add(new InvokedDataCollector(dataCollectorInformation.DataCollectorConfig.TypeUri!,
                dataCollectorInformation.DataCollectorConfig.FriendlyName,
                dataCollectorInformation.DataCollectorConfig.DataCollectorType.AssemblyQualifiedName!,
                dataCollectorInformation.DataCollectorConfig.FilePath!,
                dataCollectorInformation.DataCollectorConfig.HasAttachmentsProcessor()));
        }

        return new Collection<InvokedDataCollector>(invokedDataCollector);
    }

    /// <inheritdoc/>
    public void TestHostLaunched(int processId)
    {
        if (!_isDataCollectionEnabled)
        {
            return;
        }

        TPDebug.Assert(_dataCollectionEnvironmentContext is not null, "_dataCollectionEnvironmentContext is null");
        var testHostLaunchedEventArgs = new TestHostLaunchedEventArgs(_dataCollectionEnvironmentContext.SessionDataCollectionContext, processId);

        SendEvent(testHostLaunchedEventArgs);
    }

    /// <inheritdoc/>
    public bool SessionStarted(SessionStartEventArgs sessionStartEventArgs)
    {
        // If datacollectors are not configured or datacollection is not enabled, return false.
        if (!_isDataCollectionEnabled || RunDataCollectors.Count == 0)
        {
            return false;
        }

        TPDebug.Assert(_dataCollectionEnvironmentContext is not null, "_dataCollectionEnvironmentContext is null");
        sessionStartEventArgs.Context = new DataCollectionContext(_dataCollectionEnvironmentContext.SessionDataCollectionContext.SessionId);
        SendEvent(sessionStartEventArgs);

        return _events.AreTestCaseEventsSubscribed();
    }

    /// <inheritdoc/>
    public void TestCaseStarted(TestCaseStartEventArgs testCaseStartEventArgs)
    {
        if (!_isDataCollectionEnabled)
        {
            return;
        }

        TPDebug.Assert(_dataCollectionEnvironmentContext is not null, "_dataCollectionEnvironmentContext is null");
        TPDebug.Assert(testCaseStartEventArgs.TestElement is not null, "testCaseStartEventArgs.TestElement is null");
        var context = new DataCollectionContext(_dataCollectionEnvironmentContext.SessionDataCollectionContext.SessionId, testCaseStartEventArgs.TestElement);
        testCaseStartEventArgs.Context = context;

        SendEvent(testCaseStartEventArgs);
    }

    /// <inheritdoc/>
    public Collection<AttachmentSet> TestCaseEnded(TestCaseEndEventArgs testCaseEndEventArgs)
    {
        if (!_isDataCollectionEnabled)
        {
            return new Collection<AttachmentSet>();
        }

        TPDebug.Assert(_dataCollectionEnvironmentContext is not null, "_dataCollectionEnvironmentContext is null");
        TPDebug.Assert(testCaseEndEventArgs.TestElement is not null, "testCaseEndEventArgs.TestElement is null");
        var context = new DataCollectionContext(_dataCollectionEnvironmentContext.SessionDataCollectionContext.SessionId, testCaseEndEventArgs.TestElement);
        testCaseEndEventArgs.Context = context;

        SendEvent(testCaseEndEventArgs);

        List<AttachmentSet>? result = null;
        try
        {
            result = _attachmentManager.GetAttachments(testCaseEndEventArgs.Context);
        }
        catch (Exception ex)
        {
            EqtTrace.Error("DataCollectionManager.TestCaseEnded: Failed to get attachments: {0}", ex);
            // TODO: It's possible we throw ArgumentNullException from catch, is it expected?
            return new Collection<AttachmentSet>(result!);
        }

        if (EqtTrace.IsVerboseEnabled)
        {
            LogAttachments(result);
        }

        return new Collection<AttachmentSet>(result);
    }

    /// <summary>
    /// The dispose.
    /// </summary>
    /// <param name="disposing">
    /// The disposing.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                CleanupPlugins();
            }

            _isDisposed = true;
        }
    }

    private void CleanupPlugins()
    {
        EqtTrace.Info("DataCollectionManager.CleanupPlugins: CleanupPlugins called");

        if (!_isDataCollectionEnabled)
        {
            return;
        }

        EqtTrace.Verbose("DataCollectionManager.CleanupPlugins: Cleaning up {0} plugins", RunDataCollectors.Count);

        RemoveDataCollectors(new List<DataCollectorInformation>(RunDataCollectors.Values));

        EqtTrace.Info("DataCollectionManager.CleanupPlugins: CleanupPlugins finished");
    }

    /// <summary>
    /// Tries to get uri of the data collector corresponding to the friendly name. If no such data collector exists return null.
    /// </summary>
    /// <param name="friendlyName">The friendly Name.</param>
    /// <param name="dataCollectorUri">The data collector Uri.</param>
    /// <returns><see cref="bool"/></returns>
    protected virtual bool TryGetUriFromFriendlyName(string? friendlyName, [NotNullWhen(true)] out string? dataCollectorUri)
    {
        TPDebug.Assert(_dataCollectorExtensionManager is not null, "_dataCollectorExtensionManager is null");
        foreach (var extension in _dataCollectorExtensionManager.TestExtensions)
        {
            if (string.Equals(friendlyName, extension.Metadata.FriendlyName, StringComparison.OrdinalIgnoreCase))
            {
                dataCollectorUri = extension.Metadata.ExtensionUri;
                return true;
            }
        }

        dataCollectorUri = null;
        return false;
    }

    /// <summary>
    /// Gets the DataCollectorConfig using uri.
    /// </summary>
    /// <param name="extensionUri">
    /// The extension uri.
    /// </param>
    /// <returns>
    /// The <see cref="DataCollectorConfig"/>.
    /// </returns>
    protected virtual DataCollectorConfig? TryGetDataCollectorConfig(string extensionUri)
    {
        TPDebug.Assert(_dataCollectorExtensionManager is not null, "_dataCollectorExtensionManager is null");
        foreach (var extension in _dataCollectorExtensionManager.TestExtensions)
        {
            if (string.Equals(extension.TestPluginInfo?.IdentifierData, extensionUri, StringComparison.OrdinalIgnoreCase))
            {
                return (DataCollectorConfig)extension.TestPluginInfo!;
            }
        }

        return null;
    }

    protected virtual bool IsUriValid(string? uri)
    {
        if (uri.IsNullOrEmpty())
        {
            return false;
        }

        TPDebug.Assert(_dataCollectorExtensionManager is not null, "_dataCollectorExtensionManager is null");
        foreach (var extension in _dataCollectorExtensionManager.TestExtensions)
        {
            if (string.Equals(uri, extension.Metadata.ExtensionUri, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Gets the extension using uri.
    /// </summary>
    /// <param name="extensionUri">
    /// The extension uri.
    /// </param>
    /// <returns>
    /// The <see cref="DataCollector"/>.
    /// </returns>
    protected virtual ObjectModel.DataCollection.DataCollector TryGetTestExtension(string extensionUri)
    {
        var extension = DataCollectorExtensionManager.TryGetTestExtension(extensionUri);
        TPDebug.Assert(extension is not null, "extension is null");
        return extension.Value;
    }

    /// <summary>
    /// Loads and initializes data collector using data collector settings.
    /// </summary>
    /// <param name="dataCollectorSettings">
    /// The data collector settings.
    /// </param>
    /// <param name="settingsXml"> runsettings Xml</param>
    private void LoadAndInitialize(DataCollectorSettings dataCollectorSettings, string settingsXml)
    {
        DataCollectorInformation dataCollectorInfo;
        DataCollectorConfig? dataCollectorConfig;

        try
        {
            // Look up the extension and initialize it if one is found.
            var extensionManager = DataCollectorExtensionManager;
            var dataCollectorUri = dataCollectorSettings.Uri?.ToString();

            if (!IsUriValid(dataCollectorUri) && !TryGetUriFromFriendlyName(dataCollectorSettings.FriendlyName, out dataCollectorUri))
            {
                LogWarning(string.Format(CultureInfo.CurrentCulture, Resources.Resources.UnableToFetchUriString, dataCollectorSettings.FriendlyName));
            }

            ObjectModel.DataCollection.DataCollector? dataCollector = null;
            if (!dataCollectorUri.IsNullOrWhiteSpace())
            {
                dataCollector = TryGetTestExtension(dataCollectorUri);
            }

            if (dataCollector == null)
            {
                LogWarning(string.Format(CultureInfo.CurrentCulture, Resources.Resources.DataCollectorNotFound, dataCollectorSettings.FriendlyName));
                return;
            }

            if (RunDataCollectors.ContainsKey(dataCollector.GetType()))
            {
                // Collector is already loaded (may be configured twice). Ignore duplicates and return.
                return;
            }

            dataCollectorConfig = TryGetDataCollectorConfig(dataCollectorUri!);
            TPDebug.Assert(dataCollectorConfig is not null, "dataCollectorConfig is null");
            TPDebug.Assert(_dataCollectionEnvironmentContext is not null, "_dataCollectionEnvironmentContext is null");

            // Attempt to get the data collector information verifying that all of the required metadata for the collector is available.
            dataCollectorInfo = new DataCollectorInformation(
                dataCollector,
                dataCollectorSettings.Configuration,
                dataCollectorConfig,
                _dataCollectionEnvironmentContext,
                _attachmentManager,
                _events,
                _messageSink,
                settingsXml);
        }
        catch (Exception ex)
        {
            EqtTrace.Error("DataCollectionManager.LoadAndInitialize: exception while creating data collector {0} : {1}", dataCollectorSettings.FriendlyName, ex);

            // No data collector info, so send the error with no direct association to the collector.
            LogWarning(string.Format(CultureInfo.CurrentCulture, Resources.Resources.DataCollectorInitializationError, dataCollectorSettings.FriendlyName, ex));
            return;
        }

        try
        {
            dataCollectorInfo.InitializeDataCollector(_telemetryReporter);
            TPDebug.Assert(dataCollectorConfig is not null, "dataCollectorConfig is null");
            lock (RunDataCollectors)
            {
                // Add data collectors to run cache.
                RunDataCollectors[dataCollectorConfig.DataCollectorType] = dataCollectorInfo;
            }
        }
        catch (Exception ex)
        {
            EqtTrace.Error("DataCollectionManager.LoadAndInitialize: exception while initializing data collector {0} : {1}", dataCollectorSettings.FriendlyName, ex);

            // Log error.
            TPDebug.Assert(_dataCollectionEnvironmentContext is not null, "_dataCollectionEnvironmentContext is null");
            TPDebug.Assert(dataCollectorConfig is not null, "dataCollectorConfig is null");
            dataCollectorInfo.Logger.LogError(_dataCollectionEnvironmentContext.SessionDataCollectionContext, string.Format(CultureInfo.CurrentCulture, Resources.Resources.DataCollectorInitializationError, dataCollectorConfig.FriendlyName, ex));

            // Dispose datacollector.
            dataCollectorInfo.DisposeDataCollector();
        }
    }

    /// <summary>
    /// Finds data collector enabled for the run in data collection settings.
    /// </summary>
    /// <param name="dataCollectionSettings">data collection settings</param>
    /// <returns>List of enabled data collectors</returns>
    private List<DataCollectorSettings> GetDataCollectorsEnabledForRun(DataCollectionRunSettings dataCollectionSettings)
    {
        var runEnabledDataCollectors = new List<DataCollectorSettings>();
        foreach (var settings in dataCollectionSettings.DataCollectorSettingsList)
        {
            if (settings.IsEnabled)
            {
                if (runEnabledDataCollectors.Any(dcSettings => string.Equals(dcSettings.FriendlyName, settings.FriendlyName, StringComparison.OrdinalIgnoreCase)))
                {
                    // If Uri or assembly qualified type name is repeated, consider data collector as duplicate and ignore it.
                    LogWarning(string.Format(CultureInfo.CurrentCulture, Resources.Resources.IgnoredDuplicateConfiguration, settings.FriendlyName));
                    continue;
                }

                runEnabledDataCollectors.Add(settings);
            }
        }

        return runEnabledDataCollectors;
    }

    /// <summary>
    /// Sends a warning message against the session which is not associated with a data collector.
    /// </summary>
    /// <remarks>
    /// This should only be used when we do not have the data collector info yet.  After we have the data
    /// collector info we can use the data collectors logger for errors.
    /// </remarks>
    /// <param name="warningMessage">The message to be logged.</param>
    private void LogWarning(string warningMessage)
    {
        _messageSink.SendMessage(new DataCollectionMessageEventArgs(TestMessageLevel.Warning, warningMessage));
    }

    /// <summary>
    /// Sends the event to all data collectors and fires a callback on the sender, letting it
    /// know when all plugins have completed processing the event
    /// </summary>
    /// <param name="args">The context information for the event</param>
    private void SendEvent(DataCollectionEventArgs args)
    {
        ValidateArg.NotNull(args, nameof(args));
        if (!_isDataCollectionEnabled)
        {
            EqtTrace.Error("DataCollectionManger:SendEvent: SendEvent called when no collection is enabled.");
            return;
        }

        // do not send events multiple times
        _events.RaiseEvent(args);
    }

    /// <summary>
    /// The get environment variables.
    /// </summary>
    /// <param name="unloadedAnyCollector">
    /// The unloaded any collector.
    /// </param>
    /// <returns>
    /// Dictionary of variable name as key and collector requested environment variable as value.
    /// </returns>
    private Dictionary<string, DataCollectionEnvironmentVariable> GetEnvironmentVariables(out bool unloadedAnyCollector)
    {
        var failedCollectors = new List<DataCollectorInformation>();
        unloadedAnyCollector = false;
        var dataCollectorEnvironmentVariable = new Dictionary<string, DataCollectionEnvironmentVariable>(StringComparer.OrdinalIgnoreCase);

        // Ordering here is temporary to enable Fakes + Code Coverage integration in scenarios when Fakes decides to instrument code using
        // CLR Instrumentation Engine. This code will be cleaned when both Fakes and Code Coverage will fully switch to CLR Instrumentation Engine.
        foreach (var dataCollectorInfo in RunDataCollectors.Values.
                     OrderBy(rdc => rdc.DataCollectorConfig.FriendlyName.Equals(CodeCoverageFriendlyName, StringComparison.OrdinalIgnoreCase) ? 1 : 0))
        {
            try
            {
                dataCollectorInfo.SetTestExecutionEnvironmentVariables();
                AddCollectorEnvironmentVariables(dataCollectorInfo, dataCollectorEnvironmentVariable);
            }
            catch (Exception ex)
            {
                unloadedAnyCollector = true;

                var friendlyName = dataCollectorInfo.DataCollectorConfig.FriendlyName;
                failedCollectors.Add(dataCollectorInfo);
                TPDebug.Assert(_dataCollectionEnvironmentContext is not null, "_dataCollectionEnvironmentContext is null");
                dataCollectorInfo.Logger.LogError(
                    _dataCollectionEnvironmentContext.SessionDataCollectionContext,
                    string.Format(CultureInfo.CurrentCulture, Resources.Resources.DataCollectorErrorOnGetVariable, friendlyName, ex));

                EqtTrace.Error("DataCollectionManager.GetEnvironmentVariables: Failed to get variable for Collector '{0}': {1}", friendlyName, ex);
            }
        }

        RemoveDataCollectors(failedCollectors);
        return dataCollectorEnvironmentVariable;
    }

    /// <summary>
    /// Collects environment variable to be set in test process by avoiding duplicates
    /// and detecting override of variable value by multiple adapters.
    /// </summary>
    /// <param name="dataCollectionWrapper">
    /// The data Collection Wrapper.
    /// </param>
    /// <param name="dataCollectorEnvironmentVariables">
    /// Environment variables required for already loaded plugin.
    /// </param>
    private void AddCollectorEnvironmentVariables(
        DataCollectorInformation dataCollectionWrapper,
        Dictionary<string, DataCollectionEnvironmentVariable> dataCollectorEnvironmentVariables)
    {
        if (dataCollectionWrapper.TestExecutionEnvironmentVariables == null)
        {
            return;
        }

        var collectorFriendlyName = dataCollectionWrapper.DataCollectorConfig.FriendlyName;
        foreach (var namevaluepair in dataCollectionWrapper.TestExecutionEnvironmentVariables)
        {
            if (dataCollectorEnvironmentVariables.TryGetValue(namevaluepair.Key, out var alreadyRequestedVariable))
            {
                // Dev10 behavior is to consider environment variables values as case sensitive.
                if (string.Equals(namevaluepair.Value, alreadyRequestedVariable.Value, StringComparison.Ordinal))
                {
                    alreadyRequestedVariable.AddRequestingDataCollector(collectorFriendlyName);
                }
                else
                {
                    // Data collector is overriding an already requested variable, possibly an error.
                    var message = string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.Resources.DataCollectorRequestedDuplicateEnvironmentVariable,
                        collectorFriendlyName,
                        namevaluepair.Key,
                        namevaluepair.Value,
                        alreadyRequestedVariable.FirstDataCollectorThatRequested,
                        alreadyRequestedVariable.Value);

                    if (collectorFriendlyName.Equals(CodeCoverageFriendlyName, StringComparison.OrdinalIgnoreCase))
                    {
                        // Do not treat this as error for Code Coverage Data Collector. This is expected in some Fakes integration scenarios
                        EqtTrace.Info(message);
                    }
                    else
                    {
                        TPDebug.Assert(_dataCollectionEnvironmentContext is not null, "_dataCollectionEnvironmentContext is null");
                        dataCollectionWrapper.Logger.LogError(_dataCollectionEnvironmentContext.SessionDataCollectionContext, message);
                    }
                }

                _dataCollectionTelemetryManager.RecordEnvironmentVariableConflict(dataCollectionWrapper, namevaluepair.Key, namevaluepair.Value, alreadyRequestedVariable.Value);
            }
            else
            {
                // new variable, add to the list.
                EqtTrace.Verbose("DataCollectionManager.AddCollectionEnvironmentVariables: Adding Environment variable '{0}' value '{1}'", namevaluepair.Key, namevaluepair.Value);

                dataCollectorEnvironmentVariables.Add(
                    namevaluepair.Key,
                    new DataCollectionEnvironmentVariable(namevaluepair, collectorFriendlyName));

                _dataCollectionTelemetryManager.RecordEnvironmentVariableAddition(dataCollectionWrapper, namevaluepair.Key, namevaluepair.Value);
            }
        }
    }

    /// <summary>
    /// The remove data collectors.
    /// </summary>
    /// <param name="dataCollectorsToRemove">
    /// The data collectors to remove.
    /// </param>
    private void RemoveDataCollectors(IReadOnlyCollection<DataCollectorInformation> dataCollectorsToRemove)
    {
        if (dataCollectorsToRemove == null || dataCollectorsToRemove.Count == 0)
        {
            return;
        }

        lock (RunDataCollectors)
        {
            foreach (var dataCollectorToRemove in dataCollectorsToRemove)
            {
                dataCollectorToRemove.DisposeDataCollector();
                RunDataCollectors.Remove(dataCollectorToRemove.DataCollector.GetType());
            }

            if (RunDataCollectors.Count == 0)
            {
                _isDataCollectionEnabled = false;
            }
        }
    }

    private static void LogAttachments(List<AttachmentSet> attachmentSets)
    {
        if (attachmentSets is null)
        {
            EqtTrace.Error("DataCollectionManager.LogAttachments: Unexpected null attachmentSets.");
            return;
        }

        foreach (var entry in attachmentSets)
        {
            if (entry is null)
            {
                EqtTrace.Error("DataCollectionManager.LogAttachments: Unexpected null entry inside attachmentSets.");
                continue;
            }

            foreach (var file in entry.Attachments)
            {
                if (file is null)
                {
                    EqtTrace.Error("DataCollectionManager.LogAttachments: Unexpected null file inside entry attachments.");
                    continue;
                }

                EqtTrace.Verbose(
                    "Test Attachment Description: Collector:'{0}'  Uri:'{1}'  Description:'{2}' Uri:'{3}' ",
                    entry.DisplayName,
                    entry.Uri,
                    file.Description,
                    file.Uri);
            }
        }
    }
}
