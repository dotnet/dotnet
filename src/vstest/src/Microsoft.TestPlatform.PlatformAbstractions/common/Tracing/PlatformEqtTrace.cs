// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;

using Microsoft.TestPlatform.PlatformAbstractions;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

/// <summary>
/// Wrapper class for tracing.
///     - Shortcut-methods for Error, Warning, Info, Verbose.
///     - Adds additional information to the trace: calling process name, PID, ThreadID, Time.
///     - Uses custom switch <c>EqtTraceLevel</c> from .config file.
///     - By default tracing if OFF.
///     - Our build environment always sets the /d:TRACE so this class is always enabled,
///       the Debug class is enabled only in debug builds (/d:DEBUG).
///     - We ignore exceptions thrown by underlying TraceSwitch (e.g. due to config file error).
///       We log ignored exceptions to system Application log.
///       We pass through exceptions thrown due to incorrect arguments to <c>EqtTrace</c> methods.
/// Usage: <c>EqtTrace.Info("Here's how to trace info");</c>
/// </summary>
public partial class PlatformEqtTrace : IPlatformEqtTrace
{
    /// <summary>
    /// Name of the trace listener.
    /// </summary>
    private const string ListenerName = "TptTraceListener";

    /// <summary>
    /// Create static maps for TraceLevel to SourceLevels. The APIs need to provide TraceLevel
    /// for backward compatibility with older versions of Object Model.
    /// </summary>
    private static readonly Dictionary<TraceLevel, SourceLevels> TraceSourceLevelsMap =
        new()
        {
            { TraceLevel.Error, SourceLevels.Error },
            { TraceLevel.Info, SourceLevels.Information },
            { TraceLevel.Off, SourceLevels.Off },
            { TraceLevel.Verbose, SourceLevels.Verbose },
            { TraceLevel.Warning, SourceLevels.Warning }
        };

    /// <summary>
    /// Create static maps for SourceLevels to TraceLevel. The APIs need to provide TraceLevel
    /// for backward compatibility with older versions of Object Model.
    /// </summary>
    private static readonly Dictionary<SourceLevels, TraceLevel> SourceTraceLevelsMap =
        new()
        {
            { SourceLevels.Error, TraceLevel.Error },
            { SourceLevels.Information, TraceLevel.Info },
            { SourceLevels.Off, TraceLevel.Off },
            { SourceLevels.Verbose, TraceLevel.Verbose },
            { SourceLevels.Warning, TraceLevel.Warning },
            { SourceLevels.All, TraceLevel.Verbose }
        };

    /// <summary>
    /// Create static maps for SourceLevels to TraceLevel. The APIs need to provide TraceLevel
    /// for backward compatibility with older versions of Object Model.
    /// </summary>
    private static readonly Dictionary<TraceLevel, TraceEventType> TraceLevelEventTypeMap =
        new()
        {
            { TraceLevel.Error, TraceEventType.Error },
            { TraceLevel.Info, TraceEventType.Information },
            { TraceLevel.Verbose, TraceEventType.Verbose },
            { TraceLevel.Warning, TraceEventType.Warning }
        };

    // Current process name/id that called trace so that it's easier to read logs.
    // We cache them for performance reason.
    private static readonly string ProcessName = GetProcessName();

    private static readonly int ProcessId = GetProcessId();

    private static readonly object LockObject = new();

    private static TraceSource? s_traceSource;

    /// <summary>
    /// Specifies whether the trace is initialized or not
    /// </summary>
    private static bool s_isInitialized;

    /// <summary>
    /// Lock over initialization
    /// </summary>
    private static readonly object IsInitializationLock = new();

    private static int s_traceFileSize;
    private static readonly int DefaultTraceFileSize = 10240; // 10Mb.

    public static string? LogFile
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets or sets the trace level.
    /// </summary>
    public static TraceLevel TraceLevel
    {
        get
        {
            return SourceTraceLevelsMap[Source.Switch.Level];
        }

        set
        {
            try
            {
                Source.Switch.Level = TraceSourceLevelsMap[value];
            }
            catch (ArgumentException e)
            {
                LogIgnoredException(e);
            }
        }
    }

    public static string? ErrorOnInitialization
    {
        get;
        set;
    }

    // This is added to ensure that traceSource should not be instantiated in when creating appdomains if EqtTrace is not enabled.
    public bool DoNotInitialize
    {
        get;
        set;
    }

    /// <summary>
    /// Gets a custom trace source. This doesn't pollute the default tracing for user applications.
    /// </summary>
    private static TraceSource Source
    {
        get
        {
            if (s_traceSource == null)
            {
                lock (LockObject)
                {
                    s_traceSource ??= new TraceSource("TpTrace", SourceLevels.Off);
                }
            }

            return s_traceSource;
        }
    }

    /// <inheritdoc/>
    public bool InitializeVerboseTrace(string? customLogFile)
    {
        return InitializeTrace(customLogFile, PlatformTraceLevel.Verbose);
    }

    /// <inheritdoc/>
    public bool InitializeTrace(string? customLogFile, PlatformTraceLevel platformTraceLevel)
    {
        s_isInitialized = false;

        LogFile = customLogFile;
        TraceLevel = MapPlatformTraceToTrace(platformTraceLevel);
        Source.Switch.Level = TraceSourceLevelsMap[TraceLevel];

        // Ensure trace is initialized
        return EnsureTraceIsInitialized();
    }

    /// <inheritdoc/>
    public bool ShouldTrace(PlatformTraceLevel traceLevel)
    {
        if (DoNotInitialize)
        {
            return false;
        }

        switch (traceLevel)
        {
            case PlatformTraceLevel.Off:
                return false;
            case PlatformTraceLevel.Error:
                return Source.Switch.ShouldTrace(TraceEventType.Error);
            case PlatformTraceLevel.Warning:
                return Source.Switch.ShouldTrace(TraceEventType.Warning);
            case PlatformTraceLevel.Info:
                return Source.Switch.ShouldTrace(TraceEventType.Information);
            case PlatformTraceLevel.Verbose:
                return Source.Switch.ShouldTrace(TraceEventType.Verbose);
            default:
                Debug.Fail("Should never get here!");
                return false;
        }
    }

    /// <inheritdoc/>
    public string? GetLogFile()
    {
        return LogFile;
    }

    /// <inheritdoc/>
    public void WriteLine(PlatformTraceLevel level, string? message)
    {
        TPDebug.Assert(message != null, "message != null");
        TPDebug.Assert(!string.IsNullOrEmpty(ProcessName), "!string.IsNullOrEmpty(ProcessName)");

        if (EnsureTraceIsInitialized())
        {
            // The format below is a CSV so that Excel could be used easily to
            // view/filter the logs.
            var log = string.Format(
                CultureInfo.InvariantCulture,
                "{0}, {1}, {2:yyyy}/{2:MM}/{2:dd}, {2:HH}:{2:mm}:{2:ss}.{2:fff}, {5}, {3}, {4}",
                ProcessId,
                Environment.CurrentManagedThreadId,
                DateTime.Now,
                ProcessName,
                message,
                Stopwatch.GetTimestamp());

            try
            {
                Source.TraceEvent(TraceLevelEventTypeMap[MapPlatformTraceToTrace(level)], 0, log);
                Source.Flush();
            }
            catch (Exception e)
            {
                // valid suppress
                // Log exception from tracing into event viewer.
                LogIgnoredException(e);
            }
        }
    }

    /// <inheritdoc/>
    public void SetTraceLevel(PlatformTraceLevel value)
    {
        Source.Switch.Level = TraceSourceLevelsMap[MapPlatformTraceToTrace(value)];
    }

    public PlatformTraceLevel GetTraceLevel()
    {
        return (PlatformTraceLevel)SourceTraceLevelsMap[Source.Switch.Level];
    }

    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Part of the public API")]
    public TraceLevel MapPlatformTraceToTrace(PlatformTraceLevel traceLevel)
    {
        switch (traceLevel)
        {
            case PlatformTraceLevel.Off:
                return TraceLevel.Off;
            case PlatformTraceLevel.Error:
                return TraceLevel.Error;
            case PlatformTraceLevel.Warning:
                return TraceLevel.Warning;
            case PlatformTraceLevel.Info:
                return TraceLevel.Info;
            case PlatformTraceLevel.Verbose:
                return TraceLevel.Verbose;
            default:
                Debug.Fail("Should never get here!");
                return TraceLevel.Verbose;
        }
    }

    /// <summary>
    /// Setup trace listeners. It should be called when setting trace listener for child domain.
    /// </summary>
    /// <param name="listener">New listener.</param>
    internal static void SetupRemoteListeners(TraceListener? listener)
    {
        lock (IsInitializationLock)
        {
            // Add new listeners.
            if (listener != null)
            {
                Source.Listeners.Add(listener);
            }

            s_isInitialized = true;
        }
    }

    /// <summary>
    /// Auxiliary method: logs the exception that is being ignored.
    /// </summary>
    /// <param name="e">The exception to log.</param>
    private static void LogIgnoredException(Exception e)
    {
        TPDebug.Assert(e != null, "e != null");

        try
        {
            EnsureTraceIsInitialized();

            // Note: Debug.WriteLine may throw if there is a problem in .config file.
            Debug.WriteLine("Ignore exception: " + e);
        }
        catch
        {
            // Ignore everything at this point.
        }
    }

    /// <summary>
    /// Ensure the trace is initialized
    /// </summary>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    private static bool EnsureTraceIsInitialized()
    {
        if (s_isInitialized)
        {
            return true;
        }

        lock (IsInitializationLock)
        {
            if (s_isInitialized)
            {
                return true;
            }

            using var process = Process.GetCurrentProcess();
            string runnerLogFileName = $"{Path.GetFileNameWithoutExtension(process.MainModule!.FileName)}_{process.Id}.{DateTime.Now:yy-MM-dd_HH-mm-ss_fffff}.diag";
            string logsDirectory = Path.GetTempPath();

            // Set the trace level and add the trace listener
            LogFile ??= Path.Combine(logsDirectory, runnerLogFileName);

            // Add a default listener
            s_traceFileSize = DefaultTraceFileSize;
            try
            {
                // If we point to a folder we create default filename
                if (LogFile.EndsWith(@"\") || LogFile.EndsWith("/"))
                {
                    if (!Directory.Exists(LogFile))
                    {
                        Directory.CreateDirectory(LogFile);
                    }

                    runnerLogFileName = Path.Combine(LogFile, runnerLogFileName);
                    LogFile = Path.Combine(LogFile, $"{Path.GetFileNameWithoutExtension(process.MainModule.FileName)}_{process.Id}.diag");
                }
                else
                {
                    runnerLogFileName = LogFile;
                }

                var runnerLogFileInfo = new FileInfo(runnerLogFileName);
                if (!Directory.Exists(runnerLogFileInfo.DirectoryName))
                {
                    Directory.CreateDirectory(runnerLogFileInfo.DirectoryName!);
                }

                Source.Listeners.Add(new RollingFileTraceListener(runnerLogFileName, ListenerName, s_traceFileSize));
            }
            catch (Exception e)
            {
                UnInitializeTrace();
                ErrorOnInitialization = e.ToString();
                return false;
            }

            s_isInitialized = true;
            return true;
        }
    }

    /// <summary>
    /// Get the process name. Note: we cache it, use m_processName.
    /// </summary>
    /// <returns>Name of the process.</returns>
    private static string GetProcessName()
    {
        try
        {
            string? processName = null;

            string[] args = Environment.GetCommandLineArgs();

            if (args != null && args.Length != 0)
            {
                // Leave the extension if specified, otherwise don't add it (e.g. case a.exe.exe).
                // It seems that if .exe suffix is not specified Framework adds .EXE to args[0].
                processName = Path.GetFileName(args[0]);
            }

            // If we still have not got process name from command line - use the slow way.
            // This should never happen unless the process is called from execv with empty cmdline.
            if (processName.IsNullOrEmpty())
            {
                Debug.Fail("Could not get process name from command line, will try to use the slow way.");
                using var process = Process.GetCurrentProcess();
                processName = process.ProcessName;
            }

            return processName;
        }
        catch (Exception e)
        {
            // valid suppress
            Debug.Fail("Could not get process name: " + e);
            LogIgnoredException(e);
            return e.Message;
        }
    }

    private static int GetProcessId()
    {
        try
        {
            using var process = Process.GetCurrentProcess();
            return process.Id;
        }
        catch (InvalidOperationException e)
        {
            Debug.Fail("Could not get process id: " + e);
            LogIgnoredException(e);
            return -1;
        }
    }

    private static void UnInitializeTrace()
    {
        s_isInitialized = false;

        LogFile = null;
        TraceLevel = TraceLevel.Off;
        Source.Switch.Level = SourceLevels.Off;
    }
}

#endif
