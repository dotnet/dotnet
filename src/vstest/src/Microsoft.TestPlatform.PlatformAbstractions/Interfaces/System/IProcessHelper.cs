// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;

/// <summary>
/// Interface for any process related functionality. This is needed for clean unit-testing.
/// </summary>
public interface IProcessHelper
{
    /// <summary>
    /// Launches the process with the given arguments.
    /// </summary>
    /// <param name="processPath">The full file name of the process.</param>
    /// <param name="arguments">The command-line arguments.</param>
    /// <param name="workingDirectory">The working directory for this process.</param>
    /// <param name="environmentVariables">Environment variables to set while bootstrapping the process.</param>
    /// <param name="errorCallback">Call back for to read error stream data</param>
    /// <param name="exitCallBack">Call back for on process exit</param>
    /// <param name="outputCallback">Call back for on process output</param>
    /// <returns>The process created.</returns>
    object LaunchProcess(string processPath, string? arguments, string? workingDirectory, IDictionary<string, string?>? envVariables, Action<object?, string?>? errorCallback, Action<object?>? exitCallBack, Action<object?, string?>? outputCallBack);

    /// <summary>
    /// Gets the current process file path.
    /// </summary>
    /// <returns>The current process file path.</returns>
    string? GetCurrentProcessFileName();

    /// <summary>
    /// Gets the current process location.
    /// </summary>
    /// <returns>The current process location.</returns>
    string GetCurrentProcessLocation();

    /// <summary>
    /// Gets the location of test engine.
    /// </summary>
    /// <returns>Location of test engine.</returns>
    string? GetTestEngineDirectory();

    /// <summary>
    /// Gets the location of native dll's, depending on current process architecture..
    /// </summary>
    /// <returns>Location of native dll's</returns>
    string GetNativeDllDirectory();

    /// <summary>
    /// Gets current process architecture
    /// </summary>
    /// <returns>Process Architecture</returns>
    PlatformArchitecture GetCurrentProcessArchitecture();

    /// <summary>
    /// Gets process architecture
    /// </summary>
    /// <returns>Process Architecture</returns>
    PlatformArchitecture GetProcessArchitecture(int processId);

    /// <summary>
    /// Gets the process id of test engine.
    /// </summary>
    /// <returns>process id of test engine.</returns>
    int GetCurrentProcessId();

    /// <summary>
    /// Gets the process id of input process.
    /// </summary>
    /// <param name="process">process parameter</param>
    /// <returns>process id.</returns>
    int GetProcessId(object? process);

    /// <summary>
    /// Gets the process name for given process id.
    /// </summary>
    /// <param name="processId">process id</param>
    /// <returns>Name of process</returns>
    string GetProcessName(int processId);

    /// <summary>
    /// False if process has not exited, True otherwise. Set exitCode only if process has exited.
    /// </summary>
    /// <param name="process">process parameter</param>
    /// <param name="exitCode">return value of exitCode</param>
    /// <returns>False if process has not exited, True otherwise</returns>
    bool TryGetExitCode(object? process, out int exitCode);

    /// <summary>
    /// Sets the process exit callback.
    /// </summary>
    /// <param name="processId">
    /// The process id.
    /// </param>
    /// <param name="callbackAction">
    /// Callback on process exit.
    /// </param>
    void SetExitCallback(int processId, Action<object?>? callbackAction);

    /// <summary>
    /// Terminates a process.
    /// </summary>
    /// <param name="process">Reference of process to terminate.</param>
    void TerminateProcess(object? process);

    /// <summary>
    /// Wait for process to exit
    /// </summary>
    /// <param name="process">Reference to process</param>
    void WaitForProcessExit(object? process);

    /// <summary>
    /// Gets the process handle for given process Id.
    /// </summary>
    /// <param name="processId">process id</param>
    /// <returns>Process Handle</returns>
    nint GetProcessHandle(int processId);
}
