// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.Common.Exceptions;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.Common.Logging;
using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;

using CommonResources = Microsoft.VisualStudio.TestPlatform.Common.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client;

/// <summary>
/// Responsible for managing logger extensions and broadcasting results
/// and error/warning/informational messages to them.
/// </summary>
internal class TestLoggerManager : ITestLoggerManager
{
    /// <summary>
    /// Keeps track if we are disposed.
    /// </summary>
    private bool _isDisposed;

    /// <summary>
    /// Used to keep track of which loggers have been initialized.
    /// </summary>
    private readonly HashSet<Type> _initializedLoggers = new();

    /// <summary>
    /// Test run directory.
    /// </summary>
    private string? _testRunDirectory;

    /// <summary>
    /// Target framework.
    /// </summary>
    private string? _targetFramework;

    /// <summary>
    /// TreatNoTestsAsError value;
    /// </summary>
    private bool _treatNoTestsAsError;

    /// <summary>
    /// Test Logger Events instance which will be passed to loggers when they are initialized.
    /// </summary>
    private readonly InternalTestLoggerEvents _loggerEvents;

    /// <summary>
    /// Message logger.
    /// </summary>
    private readonly IMessageLogger _messageLogger;

    /// <summary>
    /// Request data.
    /// </summary>
    private readonly IRequestData _requestData;

    /// <summary>
    /// Logger extension manager.
    /// </summary>
    private TestLoggerExtensionManager? _testLoggerExtensionManager;

    /// <summary>
    /// AssemblyLoadContext for current platform
    /// </summary>
    private readonly IAssemblyLoadContext _assemblyLoadContext;

    /// <summary>
    /// Test logger manager.
    /// </summary>
    /// <param name="requestData">Request Data for Providing Common Services/Data for Discovery and Execution.</param>
    /// <param name="messageLogger">Message Logger.</param>
    /// <param name="loggerEvents">Logger events.</param>
    public TestLoggerManager(IRequestData requestData, IMessageLogger messageLogger, InternalTestLoggerEvents loggerEvents) : this(requestData, messageLogger, loggerEvents, new PlatformAssemblyLoadContext())
    {
    }

    /// <summary>
    /// Test logger manager.
    /// </summary>
    /// <param name="requestData"></param>
    /// <param name="messageLogger"></param>
    /// <param name="loggerEvents"></param>
    /// <param name="assemblyLoadContext"></param>
    internal TestLoggerManager(IRequestData requestData, IMessageLogger messageLogger,
        InternalTestLoggerEvents loggerEvents, IAssemblyLoadContext assemblyLoadContext)
    {
        _requestData = requestData;
        _messageLogger = messageLogger;
        _testLoggerExtensionManager = null;
        _loggerEvents = loggerEvents;
        _assemblyLoadContext = assemblyLoadContext;
    }

    /// <summary>
    /// Loggers initialized flag.
    /// </summary>
    public bool LoggersInitialized => _initializedLoggers.Count != 0;

    private TestLoggerExtensionManager TestLoggerExtensionManager
    {
        get
        {
            _testLoggerExtensionManager ??= TestLoggerExtensionManager.Create(_messageLogger);

            return _testLoggerExtensionManager;
        }
    }

    /// <summary>
    /// Initializes all the loggers passed by user
    /// </summary>
    public void Initialize(string? runSettings)
    {
        // Enable logger events
        EnableLogging();

        // Store test run directory. This runsettings is the final runsettings merging CLI args and runsettings.
        _testRunDirectory = GetResultsDirectory(runSettings);
        _targetFramework = GetTargetFramework(runSettings)?.Name;
        _treatNoTestsAsError = GetTreatNoTestsAsError(runSettings);

        var loggers = XmlRunSettingsUtilities.GetLoggerRunSettings(runSettings);

        foreach (var logger in loggers?.LoggerSettingsList ?? Enumerable.Empty<LoggerSettings>())
        {
            // Don't add logger if its disabled.
            if (!logger.IsEnabled)
            {
                continue;
            }

            var parameters = GetParametersFromConfigurationElement(logger.Configuration);
            var loggerInitialized = false;

            // Try initializing logger by type.
            if (!StringUtils.IsNullOrWhiteSpace(logger.AssemblyQualifiedName))
            {
                loggerInitialized = InitializeLoggerByType(logger.AssemblyQualifiedName, logger.CodeBase!, parameters);
            }

            // Try initializing logger by uri.
            if (!loggerInitialized &&
                !StringUtils.IsNullOrWhiteSpace(logger.Uri?.ToString()))
            {
                loggerInitialized = InitializeLoggerByUri(logger.Uri, parameters);
            }

            // Try initializing logger by friendly name.
            if (!loggerInitialized &&
                TryGetUriFromFriendlyName(logger.FriendlyName, out var loggerUri) &&
                loggerUri != null)
            {
                loggerInitialized = InitializeLoggerByUri(loggerUri, parameters);
            }

            // Output error if logger is not initialized.
            if (!loggerInitialized)
            {
                var value = !StringUtils.IsNullOrWhiteSpace(logger.AssemblyQualifiedName)
                    ? logger.AssemblyQualifiedName
                    : !StringUtils.IsNullOrWhiteSpace(logger.Uri?.ToString())
                        ? logger.Uri.ToString()
                        : logger.FriendlyName;

                throw new InvalidLoggerException(
                    string.Format(CultureInfo.CurrentCulture, CommonResources.LoggerNotFound, value));
            }
        }

        _requestData.MetricsCollection.Add(TelemetryDataConstants.LoggerUsed, string.Join<Type>(",", _initializedLoggers.ToArray()));
    }

    /// <summary>
    /// Handles test run message event.
    /// </summary>
    /// <param name="e">TestRunMessage event args.</param>
    public void HandleTestRunMessage(TestRunMessageEventArgs e)
    {
        if (_isDisposed)
        {
            EqtTrace.Warning("TestLoggerManager.HandleTestRunMessage: Ignoring as the object is disposed.");
            return;
        }

        _loggerEvents.RaiseTestRunMessage(e);
    }

    /// <summary>
    /// Handle test run stats change event.
    /// </summary>
    /// <param name="e">TestRunChanged event args.</param>
    public void HandleTestRunStatsChange(TestRunChangedEventArgs e)
    {
        if (_isDisposed)
        {
            EqtTrace.Warning("TestLoggerManager.HandleTestRunStatsChange: Ignoring as the object is disposed.");
            return;
        }

        foreach (TestResult result in e.NewTestResults!)
        {
            _loggerEvents.RaiseTestResult(new TestResultEventArgs(result));
        }
    }

    /// <summary>
    /// Handles test run start event.
    /// </summary>
    /// <param name="e">TestRunStart event args.</param>
    public void HandleTestRunStart(TestRunStartEventArgs e)
    {
        if (_isDisposed)
        {
            EqtTrace.Warning("TestLoggerManager.HandleTestRunStart: Ignoring as the object is disposed.");
            return;
        }

        _loggerEvents.RaiseTestRunStart(e);
    }

    /// <summary>
    /// Handles test run complete.
    /// </summary>
    /// <param name="e">TestRunComplete event args.</param>
    public void HandleTestRunComplete(TestRunCompleteEventArgs e)
    {
        if (!_isDisposed)
        {
            try
            {
                _loggerEvents.CompleteTestRun(e.TestRunStatistics, e.IsCanceled, e.IsAborted, e.Error,
                    e.AttachmentSets, e.InvokedDataCollectors, e.ElapsedTimeInRunningTests);
            }
            finally
            {
                Dispose();
            }
        }
        else
        {
            // Note: We are not raising warning in case testLoggerManager is disposed as HandleRawMessage and HandleTestRunComplete both can call HandleTestRunComplete.
            EqtTrace.Verbose("TestLoggerManager.HandleTestRunComplete: Ignoring as the object is disposed.");
        }
    }

    /// <summary>
    /// Handles discovery message event.
    /// </summary>
    /// <param name="e">TestRunMessage event args.</param>
    public void HandleDiscoveryMessage(TestRunMessageEventArgs e)
    {
        if (_isDisposed)
        {
            EqtTrace.Warning("TestLoggerManager.HandleDiscoveryMessage: Ignoring as the object is disposed.");
            return;
        }

        _loggerEvents.RaiseDiscoveryMessage(e);
    }

    /// <summary>
    /// Handle discovered tests.
    /// </summary>
    /// <param name="e">DiscoveredTests event args.</param>
    public void HandleDiscoveredTests(DiscoveredTestsEventArgs e)
    {
        if (_isDisposed)
        {
            EqtTrace.Warning("TestLoggerManager.HandleDiscoveredTests: Ignoring as the object is disposed.");
            return;
        }

        _loggerEvents.RaiseDiscoveredTests(e);
    }

    /// <summary>
    /// Handles discovery complete event.
    /// </summary>
    /// <param name="e">DiscoveryComplete event args.</param>
    public void HandleDiscoveryComplete(DiscoveryCompleteEventArgs e)
    {
        if (!_isDisposed)
        {
            try
            {
                _loggerEvents.RaiseDiscoveryComplete(e);
            }
            finally
            {
                Dispose();
            }
        }
        else
        {
            // Note: We are not raising warning in case testLoggerManager is disposed as HandleRawMessage and HandleDiscoveryComplete both can call HandleDiscoveryComplete.
            EqtTrace.Verbose("TestLoggerManager.HandleDiscoveryComplete: Ignoring as the object is disposed.");
        }
    }

    /// <summary>
    /// Handles discovery start event.
    /// </summary>
    /// <param name="e">DiscoveryStart event args.</param>
    public void HandleDiscoveryStart(DiscoveryStartEventArgs e)
    {
        if (_isDisposed)
        {
            EqtTrace.Warning("TestLoggerManager.HandleDiscoveryStart: Ignoring as the object is disposed.");
            return;
        }

        _loggerEvents.RaiseDiscoveryStart(e);
    }

    /// <summary>
    /// Ensure that all pending messages are sent to the loggers.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);

        // Use SupressFinalize in case a subclass
        // of this type implements a finalizer.
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Initializes logger with the specified URI and parameters.
    /// For ex. TfsPublisher takes parameters such as  Platform, Flavor etc.
    /// </summary>
    /// <param name="uri">URI of the logger to add.</param>
    /// <param name="parameters">Logger parameters.</param>
    /// <returns>Logger Initialized flag.</returns>
    internal bool InitializeLoggerByUri(Uri uri, Dictionary<string, string?>? parameters)
    {
        ValidateArg.NotNull(uri, nameof(uri));
        CheckDisposed();

        // Look up the extension and initialize it if one is found.
        var extensionManager = TestLoggerExtensionManager;
        var logger = extensionManager.TryGetTestExtension(uri.AbsoluteUri);

        if (logger == null)
        {
            return false;
        }

        // If the logger has already been initialized just return.
        if (_initializedLoggers.Contains(logger.Value.GetType()))
        {
            EqtTrace.Verbose("TestLoggerManager: Skipping duplicate logger initialization: {0}", logger.Value.GetType());
            return true;
        }

        // Initialize logger.
        var initialized = InitializeLogger(logger.Value, logger.Metadata.ExtensionUri, parameters);

        // Add logger in initializedLoggers list.
        if (initialized)
        {
            _initializedLoggers.Add(logger.Value.GetType());
        }

        return initialized;
    }

    /// <summary>
    /// Tries to get uri of the logger corresponding to the friendly name. If no such logger exists return null.
    /// </summary>
    /// <param name="friendlyName">The friendly Name.</param>
    /// <param name="loggerUri">The logger Uri.</param>
    /// <returns><see cref="bool"/></returns>
    internal bool TryGetUriFromFriendlyName(string? friendlyName, [NotNullWhen(true)] out Uri? loggerUri)
    {
        var extensionManager = TestLoggerExtensionManager;
        foreach (var extension in extensionManager.TestExtensions)
        {
            if (!string.Equals(friendlyName, extension.Metadata.FriendlyName, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            try
            {
                loggerUri = new Uri(extension.Metadata.ExtensionUri);
            }
            catch (UriFormatException)
            {
                loggerUri = null;

                throw new InvalidLoggerException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        CommonResources.LoggerUriInvalid,
                        extension.Metadata.ExtensionUri));
            }

            return true;
        }

        loggerUri = null;
        return false;
    }

    /// <summary>
    /// Gets the test results directory.
    /// </summary>
    /// <param name="runSettings">Test run settings.</param>
    /// <returns>Test results directory</returns>
    internal static string? GetResultsDirectory(string? runSettings)
    {
        string? resultsDirectory = null;
        if (runSettings != null)
        {
            try
            {
                RunConfiguration runConfiguration = XmlRunSettingsUtilities.GetRunConfigurationNode(runSettings);
                resultsDirectory = RunSettingsUtilities.GetTestResultsDirectory(runConfiguration);
            }
            catch (SettingsException se)
            {
                EqtTrace.Error("TestLoggerManager.GetResultsDirectory: Unable to get the test results directory: Error {0}", se);
            }
        }

        return resultsDirectory;
    }

    /// <summary>
    /// Gets the target framework of the test run.
    /// </summary>
    /// <param name="runSettings">Test run settings.</param>
    /// <returns>Target framework</returns>
    internal static Framework? GetTargetFramework(string? runSettings)
    {
        Framework? targetFramework = null;
        if (runSettings != null)
        {
            try
            {
                RunConfiguration runConfiguration = XmlRunSettingsUtilities.GetRunConfigurationNode(runSettings);
                targetFramework = RunSettingsUtilities.GetTargetFramework(runConfiguration);
            }
            catch (SettingsException se)
            {
                EqtTrace.Error("TestLoggerManager.GetResultsDirectory: Unable to get the target framework: Error {0}", se);
            }
        }

        return targetFramework;
    }

    /// <summary>
    /// Get TreatNoTestsAsError value of the test run
    /// </summary>
    /// <param name="runSettings"></param>
    /// <returns></returns>
    internal static bool GetTreatNoTestsAsError(string? runSettings)
    {
        return RunSettingsUtilities.GetTreatNoTestsAsError(runSettings);
    }

    /// <summary>
    /// Enables sending of events to the loggers which are registered.
    /// </summary>
    /// <remarks>
    /// By default events are disabled and will not be raised until this method is called.
    /// This is done because during logger initialization, errors could be sent and we do not
    /// want them broadcast out to the loggers until all loggers have been enabled.  Without this
    /// all loggers would not receive the errors which were sent prior to initialization finishing.
    /// </remarks>
    internal void EnableLogging()
    {
        CheckDisposed();
        _loggerEvents.EnableEvents();
    }

    /// <summary>
    /// Ensure that all pending messages are sent to the loggers.
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
                _loggerEvents.Dispose();
            }

            _isDisposed = true;
        }
    }

    /// <summary>
    /// Get parameters from configuration element.
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    private static Dictionary<string, string?> GetParametersFromConfigurationElement(XmlElement? configuration)
    {
        var configurationManager = new LoggerNameValueConfigurationManager(configuration);
        return configurationManager.NameValuePairs;
    }

    /// <summary>
    /// Initialize logger with the specified type and parameters.
    /// </summary>
    /// <param name="assemblyQualifiedName">Assembly qualified name.</param>
    /// <param name="codeBase">Code base.</param>
    /// <param name="parameters">Logger parameters.</param>
    /// <returns>Logger Initialized flag.</returns>
    private bool InitializeLoggerByType(string assemblyQualifiedName, string codeBase, Dictionary<string, string?> parameters)
    {
        CheckDisposed();
        try
        {
            // Load logger assembly.
            Assembly assembly = _assemblyLoadContext.LoadAssemblyFromPath(codeBase);
            var loggerType =
                assembly?.GetTypes()
                    .FirstOrDefault(x => string.Equals(x.AssemblyQualifiedName, assemblyQualifiedName));

            // Create logger instance
            var constructorInfo = loggerType?.GetConstructor(Type.EmptyTypes);
            var logger = constructorInfo?.Invoke(new object[] { });

            // Handle logger null scenario.
            if (logger == null)
            {
                return false;
            }

            // If the logger has already been initialized just return.
            if (_initializedLoggers.Contains(logger.GetType()))
            {
                EqtTrace.Verbose("TestLoggerManager: Skipping duplicate logger initialization: {0}", logger.GetType());
                return true;
            }

            // Get Logger instance and initialize.
            var initialized = InitializeLogger(logger, null, parameters);

            // Add logger in initializedLoggers list.
            if (initialized)
            {
                _initializedLoggers.Add(logger.GetType());
            }

            return initialized;
        }
        catch (Exception ex)
        {
            EqtTrace.Error(
                "TestLoggerManager: Error occurred while initializing the Logger assemblyQualifiedName : {0}, codeBase : {1} , Exception Details : {2}", assemblyQualifiedName, codeBase, ex);
            return false;
        }
    }

    private bool InitializeLogger(object? logger, string? extensionUri, Dictionary<string, string?>? parameters)
    {
        if (logger == null)
        {
            return false;
        }

        try
        {
            switch (logger)
            {
                case ITestLoggerWithParameters _:
                    ((ITestLoggerWithParameters)logger).Initialize(_loggerEvents, UpdateLoggerParameters(parameters));
                    break;

                case ITestLogger _:
                    ((ITestLogger)logger).Initialize(_loggerEvents, _testRunDirectory!);
                    break;

                default:
                    // If logger is of different type, then logger should not be initialized.
                    EqtTrace.Error(
                        "TestLoggerManager: Incorrect logger type: {0}", logger.GetType());
                    return false;
            }
        }
        catch (Exception ex)
        {
            var loggerUri = extensionUri.IsNullOrEmpty() ? logger.GetType().ToString() : extensionUri;
            EqtTrace.Error(
                "TestLoggerManager: Error while initializing logger: {0}, Exception details: {1}", loggerUri, ex);

            _messageLogger.SendMessage(
                TestMessageLevel.Error,
                string.Format(
                    CultureInfo.CurrentCulture,
                    CommonResources.LoggerInitializationError,
                    extensionUri.IsNullOrEmpty() ? "type" : "uri",
                    extensionUri.IsNullOrEmpty() ? logger.GetType().ToString() : extensionUri,
                    ex));

            throw new InvalidLoggerException($"Error while initializing logger: {loggerUri}, Exception details: {ex.Message}");
        }

        return true;
    }

    /// <summary>
    /// Populates user supplied and default logger parameters.
    /// </summary>
    private Dictionary<string, string?> UpdateLoggerParameters(Dictionary<string, string?>? parameters)
    {
        var loggerParams = parameters is not null
            ? new Dictionary<string, string?>(parameters, StringComparer.OrdinalIgnoreCase)
            : new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

        // Add default logger parameters...
        loggerParams[DefaultLoggerParameterNames.TestRunDirectory] = _testRunDirectory;
        loggerParams[DefaultLoggerParameterNames.TargetFramework] = _targetFramework;

        // Add custom logger parameters
        if (_treatNoTestsAsError)
        {
            loggerParams[ObjectModel.Constants.TreatNoTestsAsError] = _treatNoTestsAsError.ToString();
        }

        return loggerParams;
    }

    private void CheckDisposed()
    {
        if (_isDisposed)
        {
            throw new ObjectDisposedException(typeof(TestLoggerManager).FullName);
        }
    }
}
