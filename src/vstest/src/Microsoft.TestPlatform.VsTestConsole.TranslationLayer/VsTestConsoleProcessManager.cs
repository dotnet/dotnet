// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;

using Microsoft.TestPlatform.VsTestConsole.TranslationLayer.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Helpers;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestPlatform.VsTestConsole.TranslationLayer.Resources;

using Resources = Microsoft.VisualStudio.TestPlatform.VsTestConsole.TranslationLayer.Resources.Resources;

namespace Microsoft.TestPlatform.VsTestConsole.TranslationLayer;

/// <summary>
/// Vstest.console process manager
/// </summary>
internal sealed class VsTestConsoleProcessManager : IProcessManager, IDisposable
{
    /// <summary>
    /// Port number for communicating with Vstest CLI
    /// </summary>
    private const string PortArgument = "/port:{0}";

    /// <summary>
    /// Process Id of the Current Process which is launching Vstest CLI
    /// Helps Vstest CLI in auto-exit if current process dies without notifying it
    /// </summary>
    private const string ParentProcessidArgument = "/parentprocessid:{0}";

    /// <summary>
    /// Diagnostics argument for Vstest CLI
    /// Enables Diagnostic logging for Vstest CLI and TestHost - Optional
    /// </summary>
    private const string DiagArgument = "/diag:{0};tracelevel={1}";

    /// <summary>
    /// EndSession timeout
    /// </summary>
    private const int Endsessiontimeout = 1000;

    private readonly string _vstestConsolePath;
    private readonly object _syncObject = new();
    private readonly bool _isNetCoreRunner;
    private readonly string? _dotnetExePath;
    private readonly ManualResetEvent _processExitedEvent = new(false);
    private Process? _process;
    private bool _vstestConsoleStarted;
    private bool _vstestConsoleExited;
    private bool _isDisposed;

    internal IFileHelper FileHelper { get; set; }

    /// <inheritdoc/>
    public event EventHandler? ProcessExited;

    /// <summary>
    /// Creates an instance of VsTestConsoleProcessManager class.
    /// </summary>
    /// <param name="vstestConsolePath">The full path to vstest.console</param>
    public VsTestConsoleProcessManager(string vstestConsolePath)
    {
        FileHelper = new FileHelper();
        if (!FileHelper.Exists(vstestConsolePath))
        {
            EqtTrace.Error("Invalid File Path: {0}", vstestConsolePath);
            throw new Exception(string.Format(CultureInfo.CurrentCulture, Resources.InvalidFilePath, vstestConsolePath));
        }
        _vstestConsolePath = vstestConsolePath;
        _isNetCoreRunner = vstestConsolePath.EndsWith(".dll");
    }

    public VsTestConsoleProcessManager(string vstestConsolePath, string dotnetExePath) : this(vstestConsolePath)
    {
        _dotnetExePath = dotnetExePath;
    }

    /// <summary>
    /// Checks if the process has been initialized.
    /// </summary>
    /// <returns>True if process is successfully initialized</returns>
    public bool IsProcessInitialized()
    {
        lock (_syncObject)
        {
            return _vstestConsoleStarted && !_vstestConsoleExited && _process != null;
        }
    }

    /// <summary>
    /// Call vstest.console with the parameters previously specified
    /// </summary>
    public void StartProcess(ConsoleParameters consoleParameters)
    {
        var consoleRunnerPath = GetConsoleRunner();

        // The console runner path we retrieve might have been escaped so we need to remove the
        // extra double quotes before testing whether the file exists.
        if (!File.Exists(consoleRunnerPath.Trim('"')))
        {
            throw new FileNotFoundException(string.Format(CultureInfo.CurrentCulture, InternalResources.CannotFindConsoleRunner, consoleRunnerPath), consoleRunnerPath);
        }

        var arguments = string.Join(" ", BuildArguments(consoleParameters));
        var info = new ProcessStartInfo(consoleRunnerPath, arguments)
        {
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        };

        EqtTrace.Verbose("VsTestCommandLineWrapper.StartProcess: Process Start Info {0} {1}", info.FileName, info.Arguments);

        if (!consoleParameters.InheritEnvironmentVariables)
        {
            EqtTrace.Verbose("VsTestCommandLineWrapper.StartProcess: Clearing all environment variables.");

            info.EnvironmentVariables.Clear();
        }

        if (consoleParameters.EnvironmentVariables != null)
        {
            foreach (var envVariable in consoleParameters.EnvironmentVariables)
            {
                if (envVariable.Key != null)
                {
                    // Not printing the value on purpose, env variables can contain secrets and we don't need to know the values
                    // most of the time.
                    EqtTrace.Verbose("VsTestCommandLineWrapper.StartProcess: Setting environment variable: {0}", envVariable.Key);
                    info.EnvironmentVariables[envVariable.Key] = envVariable.Value?.ToString();
                }
            }
        }

        try
        {
            _process = Process.Start(info);
        }
        catch (Win32Exception ex)
        {
            throw new Exception(string.Format(CultureInfo.CurrentCulture, InternalResources.ProcessStartWin32Failure, consoleRunnerPath, arguments), ex);
        }

        lock (_syncObject)
        {
            _vstestConsoleExited = false;
            _vstestConsoleStarted = true;
        }

        _process!.EnableRaisingEvents = true;
        _process.Exited += Process_Exited;

        _process.OutputDataReceived += Process_OutputDataReceived;
        _process.ErrorDataReceived += Process_ErrorDataReceived;
        _process.BeginOutputReadLine();
        _process.BeginErrorReadLine();
        _processExitedEvent.Reset();
    }

    /// <summary>
    /// Shutdown the vstest.console process
    /// </summary>
    public void ShutdownProcess()
    {
        // Ideally process should die by itself
        if (!_processExitedEvent.WaitOne(Endsessiontimeout) && IsProcessInitialized())
        {
            EqtTrace.Info($"VsTestConsoleProcessManager.ShutDownProcess : Terminating vstest.console process after waiting for {Endsessiontimeout} milliseconds.");
            _vstestConsoleExited = true;
            if (_process is not null)
            {
                _process.OutputDataReceived -= Process_OutputDataReceived;
                _process.ErrorDataReceived -= Process_ErrorDataReceived;
                SafelyTerminateProcess();
                _process.Dispose();
                _process = null;
            }
        }
    }

    private void SafelyTerminateProcess()
    {
        try
        {
            if (_process != null && !_process.HasExited)
            {
                _process.Kill();
            }
        }
        catch (InvalidOperationException ex)
        {
            EqtTrace.Info("VsTestCommandLineWrapper: Error While Terminating Process {0} ", ex.Message);
        }
    }

    private void Process_Exited(object? sender, EventArgs e)
    {
        lock (_syncObject)
        {
            _processExitedEvent.Set();
            _vstestConsoleExited = true;
            ProcessExited?.Invoke(sender, e);
        }
    }

    private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (e.Data != null)
        {
            EqtTrace.Error(e.Data);
        }
    }

    private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (e.Data != null)
        {
            EqtTrace.Verbose(e.Data);
        }
    }

    internal string[] BuildArguments(ConsoleParameters parameters)
    {
        var args = new List<string>
        {
            // Start Vstest.console with args: --parentProcessId|/parentprocessid:<ppid> --port|/port:<port>
            string.Format(CultureInfo.InvariantCulture, ParentProcessidArgument, parameters.ParentProcessId),
            string.Format(CultureInfo.InvariantCulture, PortArgument, parameters.PortNumber)
        };

        if (!parameters.LogFilePath.IsNullOrEmpty())
        {
            // Extra args: --diag|/diag:<PathToLogFile>;tracelevel=<tracelevel>
            args.Add(string.Format(CultureInfo.InvariantCulture, DiagArgument, parameters.LogFilePath, parameters.TraceLevel));
        }

        if (_isNetCoreRunner)
        {
            args.Insert(0, GetEscapeSequencedPath(_vstestConsolePath));
        }

        return args.ToArray();
    }

    private string GetConsoleRunner()
        => _isNetCoreRunner
            ? _dotnetExePath.IsNullOrEmpty()
                ? new DotnetHostHelper().GetDotnetPath()
                : _dotnetExePath
            : GetEscapeSequencedPath(_vstestConsolePath);

    private static string GetEscapeSequencedPath(string path)
        => path.IsNullOrEmpty() ? path : $"\"{path.Trim('"')}\"";

    public void Dispose()
    {
        if (!_isDisposed)
        {
            _processExitedEvent.Dispose();
            _process?.Dispose();
            _isDisposed = true;
        }
    }
}
