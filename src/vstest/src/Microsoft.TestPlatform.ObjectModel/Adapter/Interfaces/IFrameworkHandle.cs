// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

/// <summary>
/// Handle to the framework which is passed to the test executors.
/// </summary>
public interface IFrameworkHandle : ITestExecutionRecorder, IMessageLogger
{
    /// <summary>
    /// Gets or sets a value indicating whether the execution framework enables the shutdown of execution process after the test run is complete. This should be used only in out of process test runs when IRunContext.KeepAlive is true
    /// and should be used only when absolutely required as using it degrades the performance of the subsequent run.
    /// It throws InvalidOperationException when it is attempted to be enabled when keepAlive is false.
    /// </summary>
    bool EnableShutdownAfterTestRun { get; set; }

    /// <summary>
    /// Launch the specified process with the debugger attached.
    /// </summary>
    /// <param name="filePath">File path to the exe to launch.</param>
    /// <param name="workingDirectory">Working directory that process should use. If null, the current directory will be used.</param>
    /// <param name="arguments">Command line arguments the process should be launched with.</param>
    /// <param name="environmentVariables">Environment variables to be set in target process</param>
    /// <returns>Process ID of the started process.</returns>
    int LaunchProcessWithDebuggerAttached(string filePath, string? workingDirectory, string? arguments, IDictionary<string, string?>? environmentVariables);
}
