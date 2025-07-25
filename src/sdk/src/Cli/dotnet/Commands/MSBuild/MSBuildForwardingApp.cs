﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Reflection;
using Microsoft.DotNet.Cli.Utils;
using Microsoft.DotNet.Cli.Utils.Extensions;

namespace Microsoft.DotNet.Cli.Commands.MSBuild;

public class MSBuildForwardingApp : CommandBase
{
    internal const string TelemetrySessionIdEnvironmentVariableName = "DOTNET_CLI_TELEMETRY_SESSIONID";

    private readonly MSBuildForwardingAppWithoutLogging _forwardingAppWithoutLogging;

    private static MSBuildArgs ConcatTelemetryLogger(MSBuildArgs msbuildArgs)
    {
        if (Telemetry.Telemetry.CurrentSessionId != null)
        {
            try
            {
                Type loggerType = typeof(MSBuildLogger);
                Type forwardingLoggerType = typeof(MSBuildForwardingLogger);

                msbuildArgs.OtherMSBuildArgs.Add($"-distributedlogger:{loggerType.FullName},{loggerType.GetTypeInfo().Assembly.Location}*{forwardingLoggerType.FullName},{forwardingLoggerType.GetTypeInfo().Assembly.Location}");
                return msbuildArgs;
            }
            catch (Exception)
            {
                // Exceptions during telemetry shouldn't cause anything else to fail
            }
        }
        return msbuildArgs;
    }

    /// <summary>
    /// Mostly intended for quick/one-shot usage - most 'core' SDK commands should do more hands-on parsing.
    /// </summary>
    public MSBuildForwardingApp(IEnumerable<string> rawMSBuildArgs, string? msbuildPath = null) : this(
        MSBuildArgs.AnalyzeMSBuildArguments(rawMSBuildArgs.ToArray(), CommonOptions.PropertiesOption, CommonOptions.RestorePropertiesOption, CommonOptions.MSBuildTargetOption(), CommonOptions.VerbosityOption()),
        msbuildPath)
    {
    }

    public MSBuildForwardingApp(MSBuildArgs msBuildArgs, string? msbuildPath = null, bool includeLogo = false)
    {
        _forwardingAppWithoutLogging = new MSBuildForwardingAppWithoutLogging(
            ConcatTelemetryLogger(msBuildArgs),
            msbuildPath: msbuildPath,
            includeLogo: includeLogo);

        // Add the performance log location to the environment of the target process.
        if (PerformanceLogManager.Instance != null && !string.IsNullOrEmpty(PerformanceLogManager.Instance.CurrentLogDirectory))
        {
            EnvironmentVariable(PerformanceLogManager.PerfLogDirEnvVar, PerformanceLogManager.Instance.CurrentLogDirectory);
        }
    }

    public IEnumerable<string> MSBuildArguments { get { return _forwardingAppWithoutLogging.GetAllArguments(); } }

    public void EnvironmentVariable(string name, string? value)
    {
        _forwardingAppWithoutLogging.EnvironmentVariable(name, value);
    }

    public ProcessStartInfo GetProcessStartInfo()
    {
        InitializeRequiredEnvironmentVariables();

        return _forwardingAppWithoutLogging.GetProcessStartInfo();
    }

    private void InitializeRequiredEnvironmentVariables()
    {
        EnvironmentVariable(TelemetrySessionIdEnvironmentVariableName, Telemetry.Telemetry.CurrentSessionId);
    }

    /// <summary>
    /// Test hook returning concatenated and escaped command line arguments that would be passed to MSBuild.
    /// </summary>
    internal string GetArgumentsToMSBuild() => ArgumentEscaper.EscapeAndConcatenateArgArrayForProcessStart(GetArgumentTokensToMSBuild());

    internal string[] GetArgumentTokensToMSBuild() => _forwardingAppWithoutLogging.GetAllArguments();

    public override int Execute()
    {
        // Ignore Ctrl-C for the remainder of the command's execution
        // Forwarding commands will just spawn the child process and exit
        Console.CancelKeyPress += (sender, e) => { e.Cancel = true; };

        int exitCode;
        if (_forwardingAppWithoutLogging.ExecuteMSBuildOutOfProc)
        {
            ProcessStartInfo startInfo = GetProcessStartInfo();

            PerformanceLogEventSource.Log.LogMSBuildStart(startInfo.FileName, startInfo.Arguments);
            exitCode = startInfo.Execute();
            PerformanceLogEventSource.Log.MSBuildStop(exitCode);
        }
        else
        {
            InitializeRequiredEnvironmentVariables();
            string[] arguments = _forwardingAppWithoutLogging.GetAllArguments();
            if (PerformanceLogEventSource.Log.IsEnabled())
            {
                PerformanceLogEventSource.Log.LogMSBuildStart(
                    _forwardingAppWithoutLogging.MSBuildPath,
                    ArgumentEscaper.EscapeAndConcatenateArgArrayForProcessStart(arguments));
            }
            exitCode = _forwardingAppWithoutLogging.ExecuteInProc(arguments);
            PerformanceLogEventSource.Log.MSBuildStop(exitCode);
        }

        return exitCode;
    }
}
