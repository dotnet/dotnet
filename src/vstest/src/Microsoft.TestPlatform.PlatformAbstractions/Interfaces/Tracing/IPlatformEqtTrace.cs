// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

/// <summary>
/// Specifies what messages to output for the System.Diagnostics.Debug, System.Diagnostics.Trace
/// and System.Diagnostics.TraceSwitch classes.
/// </summary>
public partial interface IPlatformEqtTrace
{
    // This is added to ensure that tracing for testhost/datacollector should not be instantiated if not enabled via --diag switch.
    bool DoNotInitialize
    {
        get;
        set;
    }

    /// <summary>
    /// Adds the message to the trace log.
    /// The line becomes:
    ///     [I, PID, ThreadID, 2003/06/11 11:56:07.445] CallingAssemblyName: message.
    /// </summary>
    /// <param name="level">Trace level.</param>
    /// <param name="message">The message to add to trace.</param>
    void WriteLine(PlatformTraceLevel level, string? message);

    /// <summary>
    /// Initializes the verbose tracing with custom log file
    /// And overrides if any trace is set before
    /// </summary>
    /// <param name="customLogFile">
    /// A custom log file for trace messages.
    /// </param>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    bool InitializeVerboseTrace(string? customLogFile);

    /// <summary>
    /// Initializes the tracing with custom log file and trace level.
    /// Overrides if any trace is set before.
    /// </summary>
    /// <param name="customLogFile">Custom log file for trace messages.</param>
    /// <param name="traceLevel">Trace level.</param>
    /// <returns>Trace initialized flag.</returns>
    bool InitializeTrace(string? customLogFile, PlatformTraceLevel traceLevel);

    /// <summary>
    /// Gets a value indicating if tracing is enabled for a trace level.
    /// </summary>
    /// <param name="traceLevel">Trace level.</param>
    /// <returns>True if tracing is enabled.</returns>
    bool ShouldTrace(PlatformTraceLevel traceLevel);

    /// <summary>
    /// Gets file path for trace log file.
    /// </summary>
    /// <returns>True if tracing is enabled.</returns>
    string? GetLogFile();

    /// <summary>
    /// Sets platform specific trace value for tracing verbosity.
    /// </summary>
    /// <param name="value">
    /// The value.
    /// </param>
    void SetTraceLevel(PlatformTraceLevel value);

    /// <summary>
    /// Gets platform specific trace value for tracing verbosity.
    /// </summary>
    /// <returns>
    /// The <see cref="PlatformTraceLevel"/>.
    /// </returns>
    PlatformTraceLevel GetTraceLevel();
}
