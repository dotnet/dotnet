// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
#if NETCOREAPP2_0_OR_GREATER || NETFRAMEWORK
using System.IO;
#endif
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.TesthostProtocol;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;

using CommunicationUtilitiesResources =
    Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources.Resources;
using CoreUtilitiesConstants = Microsoft.VisualStudio.TestPlatform.CoreUtilities.Constants;

namespace Microsoft.VisualStudio.TestPlatform.TestHost;

internal class DefaultEngineInvoker :
#if NETFRAMEWORK
    MarshalByRefObject,
#endif
    IEngineInvoker
{
    /// <summary>
    /// The timeout for the client to connect to the server.
    /// Increasing Timeout to allow client to connect, not always the client can connect within 5 seconds
    /// </summary>
    private const int ClientListenTimeOut = Timeout.Infinite;

    private const string EndpointArgument = "--endpoint";

    private const string RoleArgument = "--role";

    private const string ParentProcessIdArgument = "--parentprocessid";

    private const string LogFileArgument = "--diag";

    private const string TraceLevelArgument = "--tracelevel";

    private const string DataCollectionPortArgument = "--datacollectionport";

    private const string TelemetryOptedIn = "--telemetryoptedin";

    // this path is where the sources were originally located on the source system
    // we are in a testhost that runs on the remote system, so this local path is
    // actually remote for us and the remote path is local for us.
    private const string LocalPath = "--local-path";

    private const string RemotePath = "--remote-path";

    private static readonly bool EnableParentProcessQuery =
        AppContext.TryGetSwitch(
            switchName: "MSTest.EnableParentProcessQuery",
            isEnabled: out bool value)
        ? value : true;

    private readonly ITestRequestHandler _requestHandler;

    private readonly IDataCollectionTestCaseEventSender _dataCollectionTestCaseEventSender;

    private readonly IProcessHelper _processHelper;

    public DefaultEngineInvoker() : this(new TestRequestHandler(), DataCollectionTestCaseEventSender.Create(), new ProcessHelper())
    {
    }

    internal DefaultEngineInvoker(ITestRequestHandler requestHandler,
        IDataCollectionTestCaseEventSender dataCollectionTestCaseEventSender, IProcessHelper processHelper)
    {
        _processHelper = processHelper;
        _requestHandler = requestHandler;
        _dataCollectionTestCaseEventSender = dataCollectionTestCaseEventSender;
    }

    public void Invoke(IDictionary<string, string?> argsDictionary)
    {
        InitializeEqtTrace(argsDictionary);

        // We don't have a way to pass these values to TestRequestHandler directly
        // beacuse of it's public interface, we work around that by making it implement a second interface
        if (_requestHandler is IDeploymentAwareTestRequestHandler deployedHandler)
        {
            if (argsDictionary.TryGetValue(RemotePath, out var remotePath) && remotePath != null)
            {
                deployedHandler.RemotePath = remotePath;
            }

            if (argsDictionary.TryGetValue(LocalPath, out var localPath) && localPath != null)
            {
                deployedHandler.LocalPath = localPath;
            }
        }

        if (EqtTrace.IsVerboseEnabled)
        {
            var version = typeof(DefaultEngineInvoker)
                .Assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            EqtTrace.Verbose($"Version: {version} Current process architecture: {_processHelper.GetCurrentProcessArchitecture()}");
#if NETCOREAPP2_0_OR_GREATER || NETFRAMEWORK
            // https://docs.microsoft.com/en-us/dotnet/api/system.reflection.assembly.location?view=net-6.0#remarks
            // In .NET 5 and later versions, for bundled assemblies, the value returned is an empty string.
            string objectTypeLocation = typeof(object).Assembly.Location;
            if (!objectTypeLocation.IsNullOrEmpty())
            {
                EqtTrace.Verbose($"Runtime location: {Path.GetDirectoryName(objectTypeLocation)}");
            }
#endif
        }

        if (EqtTrace.IsInfoEnabled)
        {
            EqtTrace.Info("DefaultEngineInvoker.Invoke: Testhost process started with args :{0}",
                string.Join(",", argsDictionary));
#if NETFRAMEWORK
            var appConfigText =
                System.IO.File.ReadAllText(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            EqtTrace.Info("DefaultEngineInvoker: Using Application Configuration: '{0}'", appConfigText);
#endif
        }

#if NETCOREAPP
        TestHostTraceListener.Setup();
#endif

        SetParentProcessExitCallback(argsDictionary);

        _requestHandler.ConnectionInfo = GetConnectionInfo(argsDictionary);

        // Initialize Communication with vstest.console
        _requestHandler.InitializeCommunication();

        // skipping because 0 is the default value, and also the value the the callers use when they
        // call with the parameter specified, but without providing an actual port
        var dcPort = CommandLineArgumentsHelper.GetIntArgFromDict(argsDictionary, DataCollectionPortArgument);
        if (dcPort > 0)
        {
            ConnectToDatacollector(dcPort);
        }

        var telemetryOptedIn = GetTelemetryStatusFromArgs(argsDictionary);

        // Start processing async in a different task
        EqtTrace.Info("DefaultEngineInvoker.Invoke: Start Request Processing.");
        try
        {
            StartProcessingAsync(_requestHandler, new TestHostManagerFactory(telemetryOptedIn)).Wait();
        }
        finally
        {
            if (dcPort > 0)
            {
                // Close datacollector communication.
                _dataCollectionTestCaseEventSender.Close();
            }

            _requestHandler.Dispose();
        }
    }

    private static bool GetTelemetryStatusFromArgs(IDictionary<string, string?> argsDictionary)
    {
        // Checks if telemetry is opted in in the command line arguments.
        // By default opting out in testhost to handle the scenario when the user is running an old
        // version of vstest.console.
        var telemetryStatus = CommandLineArgumentsHelper.GetStringArgFromDict(argsDictionary, TelemetryOptedIn);

        return string.Equals(telemetryStatus, "true", StringComparison.Ordinal);
    }

    private void ConnectToDatacollector(int dcPort)
    {
        EqtTrace.Info("DefaultEngineInvoker.ConnectToDatacollector: Connecting to datacollector, port: {0}",
            dcPort);
        _dataCollectionTestCaseEventSender.InitializeCommunication(dcPort);

        // It's possible that connection to vstest.console happens, but to datacollector fails, why?
        // DataCollector keeps the server alive for testhost only for 15secs(increased to 90 now),
        // if somehow(on slower machines, with Profiler Enabled) testhost can take considerable time to launch,
        // in such scenario dc.exe would have killed the server, but testhost will wait infinitely to connect to it,
        // hence do not wait to connect to datacollector process infinitely, as it will cause process hang.
        var timeout = EnvironmentHelper.GetConnectionTimeout();
        if (!_dataCollectionTestCaseEventSender.WaitForRequestSenderConnection(timeout * 1000))
        {
            EqtTrace.Error(
                "DefaultEngineInvoker.ConnectToDatacollector: Connection to DataCollector failed: '{0}', DataCollection will not happen in this session",
                dcPort);
            throw new TestPlatformException(
                string.Format(
                    CultureInfo.CurrentCulture,
                    CommunicationUtilitiesResources.ConnectionTimeoutErrorMessage,
                    CoreUtilitiesConstants.TesthostProcessName,
                    CoreUtilitiesConstants.DatacollectorProcessName,
                    timeout,
                    EnvironmentHelper.VstestConnectionTimeout)
            );
        }
    }

    private void SetParentProcessExitCallback(IDictionary<string, string?> argsDictionary)
    {
        // Attach to exit of parent process
        var hasParentProcessArgument = CommandLineArgumentsHelper.TryGetIntArgFromDict(argsDictionary, ParentProcessIdArgument, out var parentProcessId);

        if (!hasParentProcessArgument)
        {
            throw new ArgumentException($"Argument {ParentProcessIdArgument} was not specified.");
        }

        if (!EnableParentProcessQuery)
        {
            EqtTrace.Info("DefaultEngineInvoker.SetParentProcessExitCallback: Skipping querying parent process with id: '{0}'", parentProcessId);
            return;
        }

        EqtTrace.Info("DefaultEngineInvoker.SetParentProcessExitCallback: Monitoring parent process with id: '{0}'", parentProcessId);

        if (parentProcessId == -1)
        {
            // In remote scenario we cannot monitor parent process, so we expect user to pass parentProcessId as -1
            return;
        }

        if (parentProcessId == 0)
        {
            //TODO: should there be a warning / error in this case, on windows and linux we are most likely not started by this PID 0, because it's Idle process on Windows, and Swapper on Linux, and similarly in docker
            // Trying to attach to 0 will cause access denied error on Windows
        }

        _processHelper.SetExitCallback(
            parentProcessId,
            (obj) =>
            {
                EqtTrace.Info("DefaultEngineInvoker.SetParentProcessExitCallback: ParentProcess '{0}' Exited.",
                    parentProcessId);
                new PlatformEnvironment().Exit(1);
            });
    }

    private static TestHostConnectionInfo GetConnectionInfo(IDictionary<string, string?> argsDictionary)
    {
        // vstest.console < 15.5 won't send endpoint and role arguments.
        // So derive endpoint from port argument and Make connectionRole as Client.
        var endpoint = CommandLineArgumentsHelper.GetStringArgFromDict(argsDictionary, EndpointArgument);
        if (string.IsNullOrWhiteSpace(endpoint))
        {
            var port = CommandLineArgumentsHelper.GetIntArgFromDict(argsDictionary, "--port");
            endpoint = IPAddress.Loopback + ":" + port;
        }

        EqtTrace.Info("DefaultEngineInvoker.GetConnectionInfo: Initialize communication on endpoint address: '{0}'", endpoint);

        var connectionRole = ConnectionRole.Client;
        string? role = CommandLineArgumentsHelper.GetStringArgFromDict(argsDictionary, RoleArgument);
        if (string.Equals(role, "host", StringComparison.OrdinalIgnoreCase))
        {
            connectionRole = ConnectionRole.Host;
        }

        // Start Processing of requests
        var connectionInfo = new TestHostConnectionInfo
        {
            Endpoint = endpoint!,
            Role = connectionRole,
            Transport = Transport.Sockets
        };

        return connectionInfo;
    }

    private static void InitializeEqtTrace(IDictionary<string, string?> argsDictionary)
    {
        // Setup logging if enabled
        if (argsDictionary.TryGetValue(LogFileArgument, out var logFile))
        {
            var traceLevelInt = CommandLineArgumentsHelper.GetIntArgFromDict(argsDictionary, TraceLevelArgument);

            // In case traceLevelInt is not defined in PlatfromTraceLevel, default it to verbose.
            var traceLevel = Enum.IsDefined(typeof(PlatformTraceLevel), traceLevelInt) ?
                (PlatformTraceLevel)traceLevelInt :
                PlatformTraceLevel.Verbose;

            EqtTrace.InitializeTrace(logFile, traceLevel);
        }
        else
        {
            EqtTrace.DoNotInitailize = true;
        }
    }

    private static Task StartProcessingAsync(ITestRequestHandler requestHandler, ITestHostManagerFactory managerFactory)
    {
        var task = new Task(
            () =>
            {
                // Wait for the connection to the sender and start processing requests from sender
                // Note that we are waiting here infinitely to connect to vstest.console, but at the same time vstest.console doesn't wait infinitely.
                // It has a default timeout of 60secs(which is configurable), & then it kills testhost.exe
                // The reason to wait infinitely, was remote debugging scenarios of UWP app,
                // in such cases after the app gets launched, VS debugger takes control of it, & causes a lot of delay, which frequently causes timeout with vstest.console.
                // One fix would be just double this timeout, but there is no telling how much time it can actually take.
                // Hence we are waiting here indefinitely, to avoid such guessed timeouts, & letting user kill the debugging if they feel it is taking too much time.
                // In other cases if vstest.console's timeout exceeds it will definitely such down the app.
                if (requestHandler.WaitForRequestSenderConnection(ClientListenTimeOut))
                {
                    EqtTrace.Info("DefaultEngineInvoker.StartProcessingAsync: Connected to vstest.console, Starting process requests.");
                    requestHandler.ProcessRequests(managerFactory);
                }
                else
                {
                    EqtTrace.Info(
                        "DefaultEngineInvoker.StartProcessingAsync: RequestHandler timed out while connecting to the Sender.");
                    throw new TimeoutException();
                }
            },
            TaskCreationOptions.LongRunning);

        task.Start();
        return task;
    }
}
