// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Execution;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

namespace Microsoft.TestPlatform.Extensions.BlameDataCollector;

public class ProcDumpDumper : ICrashDumper, IHangDumper
{
    private static readonly IEnumerable<string> ProcDumpExceptionsList = new List<string>()
    {
        "STACK_OVERFLOW",
        "ACCESS_VIOLATION"
    };

    private readonly IProcessHelper _processHelper;
    private readonly IFileHelper _fileHelper;
    private readonly IEnvironment _environment;
    private readonly IEnvironmentVariableHelper _environmentVariableHelper;
    private string? _procDumpPath;
    private Process? _procDumpProcess;
    private string? _tempDirectory;
    private string? _dumpFileName;
    private bool _collectAlways;
    private string? _outputDirectory;
    private Process? _process;
    private string? _outputFilePrefix;
    private bool _isCrashDumpInProgress;
    private readonly int _timeout = EnvironmentHelper.GetConnectionTimeout() * 1000;
    private readonly ProcDumpExecutableHelper _procDumpExecutableHelper;

    public ProcDumpDumper()
        : this(new ProcessHelper(), new FileHelper(), new PlatformEnvironment(), new EnvironmentVariableHelper())
    {
    }

    public ProcDumpDumper(IProcessHelper processHelper, IFileHelper fileHelper, IEnvironment environment) :
        this(processHelper, fileHelper, environment, new EnvironmentVariableHelper())
    {
        _processHelper = processHelper;
        _fileHelper = fileHelper;
        _environment = environment;
    }

    internal ProcDumpDumper(IProcessHelper processHelper, IFileHelper fileHelper, IEnvironment environment, IEnvironmentVariableHelper environmentVariableHelper)
    {
        _processHelper = processHelper;
        _fileHelper = fileHelper;
        _environment = environment;
        _environmentVariableHelper = environmentVariableHelper;
        _procDumpExecutableHelper = new ProcDumpExecutableHelper(processHelper, fileHelper, environment, environmentVariableHelper);
    }

    protected Action<object?, string?> OutputReceivedCallback => (process, data) =>
    {
        EqtTrace.Info($"ProcDumpDumper.OutputReceivedCallback: Output received from procdump process: {data ?? "<null>"}");

        // This is what procdump writes to the output when it is creating a crash dump. When hangdump triggers while we are writing a crash dump
        // we probably don't want to cancel, because that crashdump probably has the more interesting info.
        // [16:06:59] Dump 1 initiated: <path>
        // [16:07:00] Dump 1 writing: Estimated dump file size is 11034 MB.
        // [16:07:09] Dump 1 complete: 11034 MB written in 10.1 seconds
        // We also want to know when we completed writing a dump (and not just set _isCrashDumpInProgress once), because dumpcount larger than 1
        // can be provided externally and then the first dump would prevent hangdump forever from stopping the process, but the not every dump is crashing the process
        // so we would run forever.
        //
        // Yes the two ifs below depend on the content being in english, and containg those words (which is the case for procdump from 2017 till 2023 at least),
        // if we get different language it should not break us, we will just cancel more aggressively (unfortunately).
        if (data != null && data.Contains("initiated"))
        {
            EqtTrace.Info($"ProcDumpDumper.OutputReceivedCallback: Output received from procdump process contains 'initiated', crashdump is being written. Don't cancel procdump right now.");
            _isCrashDumpInProgress = true;
        }

        if (data != null && data.Contains("complete"))
        {
            EqtTrace.Info($"ProcDumpDumper.OutputReceivedCallback: Output received from procdump process contains 'complete' dump is finished, you can cancel procdump if you need.");
            _isCrashDumpInProgress = false;
        }
    };

    internal static Action<object?, string?> ErrorReceivedCallback => (process, data) =>
        EqtTrace.Error($"ProcDumpDumper.ErrorReceivedCallback: Error received from procdump process: {data ?? "<null>"}");

    /// <inheritdoc/>
    public void WaitForDumpToFinish()
    {
        if (_procDumpProcess == null)
        {
            EqtTrace.Info($"ProcDumpDumper.WaitForDumpToFinish: ProcDump was not previously attached, this might indicate error during setup, look for ProcDumpDumper.AttachToTargetProcess.");
            return;
        }

        _processHelper.WaitForProcessExit(_procDumpProcess);
    }

    /// <inheritdoc/>
    public void AttachToTargetProcess(int processId, string outputDirectory, DumpTypeOption dumpType, bool collectAlways, Action<string> logWarning)
    {
        _collectAlways = collectAlways;
        _outputDirectory = outputDirectory;
        _process = Process.GetProcessById(processId);
        _outputFilePrefix = $"{_process.ProcessName}_{_process.Id}_{DateTime.Now:yyyyMMddTHHmmss}_crashdump";
        var outputFile = Path.Combine(outputDirectory, $"{_outputFilePrefix}.dmp");
        EqtTrace.Info($"ProcDumpDumper.AttachToTargetProcess: Attaching to process '{processId}' to dump into '{outputFile}'.");

        // Procdump will append .dmp at the end of the dump file. We generate this internally so it is rather a safety check.
        if (!outputFile.EndsWith(".dmp", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Procdump crash dump file must end with .dmp extension.");
        }

        if (!_procDumpExecutableHelper.TryGetProcDumpExecutable(processId, out var procDumpPath))
        {
            var procdumpNotFound = string.Format(CultureInfo.CurrentCulture, Resources.Resources.ProcDumpNotFound, procDumpPath);
            logWarning(procdumpNotFound);
            EqtTrace.Warning($"ProcDumpDumper.AttachToTargetProcess: {procdumpNotFound}");
            return;
        }

        _tempDirectory = Path.GetDirectoryName(outputFile);
        _dumpFileName = Path.GetFileNameWithoutExtension(outputFile);

        string procDumpArgs = new ProcDumpArgsBuilder().BuildTriggerBasedProcDumpArgs(
            processId,
            _dumpFileName,
            ProcDumpExceptionsList,
            isFullDump: dumpType == DumpTypeOption.Full);

        EqtTrace.Info($"ProcDumpDumper.AttachToTargetProcess: Running ProcDump with arguments: '{procDumpArgs}'.");
        _procDumpPath = procDumpPath;
        _procDumpProcess = (Process)_processHelper.LaunchProcess(
            procDumpPath,
            procDumpArgs,
            _tempDirectory,
            null,
            ErrorReceivedCallback,
            null,
            OutputReceivedCallback);

        EqtTrace.Info($"ProcDumpDumper.AttachToTargetProcess: ProcDump started as process with id '{_procDumpProcess.Id}'.");
    }

    /// <inheritdoc/>
    public void DetachFromTargetProcess(int targetProcessId)
    {
        if (_procDumpProcess == null || _procDumpPath == null)
        {
            EqtTrace.Info($"ProcDumpDumper.DetachFromTargetProcess: ProcDump was not previously attached, this might indicate error during setup, look for ProcDumpDumper.AttachToTargetProcess.");
            return;
        }

        if (_procDumpProcess.HasExited)
        {
            EqtTrace.Info($"ProcDumpDumper.DetachFromTargetProcess: ProcDump already exited, returning.");
            return;
        }

        try
        {
            if (_isCrashDumpInProgress)
            {
                EqtTrace.Info($"ProcDumpDumper.DetachFromTargetProcess: ProcDump is currently dumping process '{targetProcessId}', wait at most {_timeout} ms for it to finish so we get the crashdump.");
                var procDumpExit = Task.Run(() => _procDumpProcess.WaitForExit(_timeout));
                // Could do this better with completion source, but we have nothing better to do in this process anyway,
                // than wait for the crashdump to finish.
                while (_isCrashDumpInProgress && !procDumpExit.IsCompleted)
                {
                    // The timeout is driven by VSTEST_CONNECTION_TIMEOUT which is specified in seconds so it cannot be less than 1000ms.
                    // (Technically it can be 0, but that will fail way before we ever reach here.)
                    Thread.Sleep(500);
                    EqtTrace.Verbose($"ProcDumpDumper.DetachFromTargetProcess: Waiting for procdump to finish dumping the process.");
                }

                if (procDumpExit.IsCompleted && procDumpExit.Result == false)
                {
                    EqtTrace.Verbose($"ProcDumpDumper.DetachFromTargetProcess: Procdump did not exit after {_timeout} ms.");
                }
            }

            if (_procDumpProcess.HasExited)
            {
                EqtTrace.Info($"ProcDumpDumper.DetachFromTargetProcess: ProcDump already exited, returning.");
                return;
            }

            EqtTrace.Info($"ProcDumpDumper.DetachFromTargetProcess: ProcDump detaching from target process '{targetProcessId}'.");
            // Alternative to sending this event is calling Procdump -cancel <targetProcessId> (the dumped process id, not the existing Procdump.exe process id).
            // But not all versions of procdump have that parameter (definitely not the one we are getting from the Procdump 0.0.1 nuget package), and it works reliably.
            // What was not reliable before was that we sent the message and immediately killed procdump, that caused testhost to crash occasionally, because procdump was not detached,
            // and killing the process when it is not detached takes the observed process with it.
            var eventName = $"ProcDump-{targetProcessId}";
            new Win32NamedEvent(eventName).Set();
            EqtTrace.Info($"ProcDumpDumper.DetachFromTargetProcess: Cancel event '{eventName}' was sent to Procdump.");

            var sw = Stopwatch.StartNew();
            var exited = _procDumpProcess.WaitForExit(_timeout);
            if (exited)
            {
                EqtTrace.Info($"ProcDumpDumper.DetachFromTargetProcess: ProcDump cancelled after {sw.ElapsedMilliseconds} ms.");
            }
            else
            {
                EqtTrace.Info($"ProcDumpDumper.DetachFromTargetProcess: ProcDump cancellation timed out, after {sw.ElapsedMilliseconds} ms.");
            }
        }
        finally
        {
            try
            {
                if (!_procDumpProcess.HasExited)
                {
                    EqtTrace.Info("ProcDumpDumper.DetachFromTargetProcess: Procdump process is still running after cancellation, force killing it. This will probably take down the process it is attached to as well.");
                    _processHelper.TerminateProcess(_procDumpProcess);
                }
            }
            catch (Exception e)
            {
                EqtTrace.Warning($"ProcDumpDumper.DetachFromTargetProcess: Failed to kill procdump process with exception {e}");
            }
        }
    }

    public IEnumerable<string> GetDumpFiles(bool processCrashed)
    {
        var allDumps = _fileHelper.DirectoryExists(_outputDirectory)
            ? _fileHelper.GetFiles(_outputDirectory, "*_crashdump*.dmp", SearchOption.AllDirectories)
            : Array.Empty<string>();

        // We are always collecting dump on exit even when collectAlways option is false, to make sure we collect
        // dump for Environment.FailFast. So there always can be a dump if the process already exited. In most cases
        // this was just a normal process exit that was not caused by an exception and user is not interested in getting that
        // dump because it only pollutes their CI.
        // The hangdumps and crash dumps actually end up in the same folder, but we can distinguish them based on the _crashdump suffix.
        if (_collectAlways)
        {
            return allDumps;
        }

        if (processCrashed)
        {
            return allDumps;
        }

        // There can be more dumps in the crash folder from the child processes that were .NET5 or newer and crashed
        // get only the ones that match the path we provide to procdump. And get the last one created.
        var allTargetProcessDumps = allDumps
            .Where(dump => Path.GetFileNameWithoutExtension(dump).StartsWith(_outputFilePrefix ?? string.Empty))
            .Select(dump => new FileInfo(dump))
            .OrderBy(dump => dump.LastWriteTime).ThenBy(dump => dump.Name)
            .ToList();

        var dumpToRemove = allTargetProcessDumps.LastOrDefault();

        if (dumpToRemove != null)
        {
            EqtTrace.Verbose($"ProcDumpDumper.GetDumpFiles: Found {allTargetProcessDumps.Count} dumps for the target process, removing {dumpToRemove.Name} because we always collect a dump, even if there is no crash. But the process did not crash and user did not specify CollectAlways=true.");
            try
            {
                File.Delete(dumpToRemove.FullName);
            }
            catch (Exception ex)
            {
                EqtTrace.Error($"ProcDumpDumper.GetDumpFiles: Removing dump failed with: {ex}");
                EqtTrace.Error(ex);
            }
        }

        return allTargetProcessDumps.Take(allTargetProcessDumps.Count - 1).Select(dump => dump.FullName).ToList();
    }

    // Hang dumps the process using procdump.
    public void Dump(int processId, string outputDirectory, DumpTypeOption dumpType)
    {
        var process = Process.GetProcessById(processId);
        var outputFile = Path.Combine(outputDirectory, $"{process.ProcessName}_{processId}_{DateTime.Now:yyyyMMddTHHmmss}_hangdump.dmp");
        EqtTrace.Info($"ProcDumpDumper.Dump: Hang dumping process '{processId}' to dump into '{outputFile}'.");

        // Procdump will append .dmp at the end of the dump file. We generate this internally so it is rather a safety check.
        if (!outputFile.EndsWith(".dmp", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Procdump crash dump file must end with .dmp extension.");
        }

        if (!_procDumpExecutableHelper.TryGetProcDumpExecutable(processId, out var procDumpPath))
        {
            var err = $"{procDumpPath} could not be found, please set PROCDUMP_PATH environment variable to a directory that contains {procDumpPath} executable, or make sure that the executable is available on PATH.";
            ConsoleOutput.Instance.Warning(false, err);
            EqtTrace.Error($"ProcDumpDumper.Dump: {err}");
            return;
        }

        var tempDirectory = Path.GetDirectoryName(outputFile);
        var dumpFileName = Path.GetFileNameWithoutExtension(outputFile);

        string procDumpArgs = new ProcDumpArgsBuilder().BuildHangBasedProcDumpArgs(
            processId,
            dumpFileName,
            isFullDump: dumpType == DumpTypeOption.Full);

        EqtTrace.Info($"ProcDumpDumper.Dump: Running ProcDump with arguments: '{procDumpArgs}'.");
        var procDumpProcess = (Process)_processHelper.LaunchProcess(
            procDumpPath,
            procDumpArgs,
            tempDirectory,
            null,
            ErrorReceivedCallback,
            null,
            OutputReceivedCallback);

        EqtTrace.Info($"ProcDumpDumper.Dump: ProcDump started as process with id '{procDumpProcess.Id}'.");

        _processHelper?.WaitForProcessExit(procDumpProcess);

        EqtTrace.Info($"ProcDumpDumper.Dump: ProcDump finished hang dumping process with id '{processId}'.");
    }
}
