// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Reflection;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.DataCollection;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.DataCollection.Interfaces;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;

using Microsoft.VisualStudio.TestPlatform.Execution;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;

using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;

using CommunicationUtilitiesResources = Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources.Resources;
using CoreUtilitiesConstants = Microsoft.VisualStudio.TestPlatform.CoreUtilities.Constants;

namespace Microsoft.VisualStudio.TestPlatform.DataCollector;

public class DataCollectorMain
{
    /// <summary>
    /// Port number used to communicate with test runner process.
    /// </summary>
    private const string PortArgument = "--port";

    /// <summary>
    /// Parent process Id argument to monitor parent process.
    /// </summary>
    private const string ParentProcessArgument = "--parentprocessid";

    /// <summary>
    /// Log file for writing eqt trace logs.
    /// </summary>
    private const string LogFileArgument = "--diag";

    /// <summary>
    /// Trace level for logs.
    /// </summary>
    private const string TraceLevelArgument = "--tracelevel";

    private readonly IProcessHelper _processHelper;

    private readonly IEnvironment _environment;
    private readonly IDataCollectionRequestHandler _requestHandler;
    private readonly UiLanguageOverride _uiLanguageOverride;

    public DataCollectorMain() :
        this(
            new ProcessHelper(),
            new PlatformEnvironment(),
            DataCollectionRequestHandler.Create(new SocketCommunicationManager(), new MessageSink()),
            new UiLanguageOverride()
        )
    {
    }

    internal DataCollectorMain(IProcessHelper processHelper, IEnvironment environment, IDataCollectionRequestHandler requestHandler, UiLanguageOverride uiLanguageOverride)
    {
        _processHelper = processHelper;
        _environment = environment;
        _requestHandler = requestHandler;
        _uiLanguageOverride = uiLanguageOverride;
    }

    public void Run(string[]? args)
    {
        DebuggerBreakpoint.AttachVisualStudioDebugger(WellKnownDebugEnvironmentVariables.VSTEST_DATACOLLECTOR_DEBUG_ATTACHVS);
        DebuggerBreakpoint.WaitForDebugger(WellKnownDebugEnvironmentVariables.VSTEST_DATACOLLECTOR_DEBUG);

        var argsDictionary = CommandLineArgumentsHelper.GetArgumentsDictionary(args);

        // Setup logging if enabled
        if (argsDictionary.TryGetValue(LogFileArgument, out var logFile))
        {
            var traceLevelInt = CommandLineArgumentsHelper.GetIntArgFromDict(argsDictionary, TraceLevelArgument);
            var isTraceLevelArgValid = Enum.IsDefined(typeof(PlatformTraceLevel), traceLevelInt);

            // In case traceLevelInt is not defined in PlatfromTraceLevel, default it to verbose.
            var traceLevel = isTraceLevelArgValid ? (PlatformTraceLevel)traceLevelInt : PlatformTraceLevel.Verbose;

            // Initialize trace.
            EqtTrace.InitializeTrace(logFile, traceLevel);


            // Log warning in case tracelevel passed in arg is invalid
            if (!isTraceLevelArgValid)
            {
                EqtTrace.Warning("DataCollectorMain.Run: Invalid trace level: {0}, defaulting to verbose tracelevel.", traceLevelInt);
            }
        }
        else
        {
            EqtTrace.DoNotInitailize = true;
        }

        if (EqtTrace.IsVerboseEnabled)
        {
            var version = typeof(DataCollectorMain)
                .GetTypeInfo()
                .Assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            EqtTrace.Verbose($"Version: {version}");
        }

        _uiLanguageOverride.SetCultureSpecifiedByUser();

        EqtTrace.Info("DataCollectorMain.Run: Starting data collector run with args: {0}", args != null ? string.Join(",", args) : "null");

        // Attach to exit of parent process
        var parentProcessId = CommandLineArgumentsHelper.GetIntArgFromDict(argsDictionary, ParentProcessArgument);
        EqtTrace.Info("DataCollector: Monitoring parent process with id: '{0}'", parentProcessId);

        _processHelper.SetExitCallback(
            parentProcessId,
            (obj) =>
            {
                EqtTrace.Info("DataCollector: ParentProcess '{0}' Exited.", parentProcessId);
                _environment.Exit(1);
            });

        // Get server port and initialize communication.
        int port = argsDictionary.TryGetValue(PortArgument, out var portValue)
            && int.TryParse(portValue, NumberStyles.Integer, CultureInfo.CurrentCulture, out var p)
            ? p
            : 0;

        if (port <= 0)
        {
            throw new ArgumentException("Incorrect/No Port number");
        }

        _requestHandler.InitializeCommunication(port);

        // Can only do this after InitializeCommunication because datacollector cannot "Send Log" unless communications are initialized
        if (!string.IsNullOrEmpty(EqtTrace.LogFile))
        {
            (_requestHandler as DataCollectionRequestHandler)?.SendDataCollectionMessage(new DataCollectionMessageEventArgs(TestMessageLevel.Informational, $"Logging DataCollector Diagnostics in file: {EqtTrace.LogFile}"));
        }

        // Start processing async in a different task
        EqtTrace.Info("DataCollector: Start Request Processing.");
        StartProcessing();
    }


    private void StartProcessing()
    {
        var timeout = EnvironmentHelper.GetConnectionTimeout();

        // Wait for the connection to the sender and start processing requests from sender
        if (_requestHandler.WaitForRequestSenderConnection(timeout * 1000))
        {
            _requestHandler.ProcessRequests();
        }
        else
        {
            EqtTrace.Error(
                "DataCollectorMain.StartProcessing: RequestHandler timed out while connecting to the Sender, timeout: {0} seconds.",
                timeout);

            _requestHandler.Close();

            throw new TestPlatformException(
                string.Format(
                    CultureInfo.CurrentCulture,
                    CommunicationUtilitiesResources.ConnectionTimeoutErrorMessage,
                    CoreUtilitiesConstants.DatacollectorProcessName,
                    CoreUtilitiesConstants.VstestConsoleProcessName,
                    timeout,
                    EnvironmentHelper.VstestConnectionTimeout)
            );
        }
    }
}
