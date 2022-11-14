// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if USE_EXTERN_ALIAS
extern alias Abstraction;
#endif

using System;
using System.IO;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

#if USE_EXTERN_ALIAS
using Abstraction::Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Abstraction::Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
#else
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
#endif

using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.Execution;

internal class ProcDumpExecutableHelper
{
    private const string ProcdumpUnixProcess = "procdump";

    private readonly IProcessHelper _processHelper;
    private readonly IFileHelper _fileHelper;
    private readonly IEnvironment _environment;
    private readonly IEnvironmentVariableHelper _environmentVariableHelper;

    public ProcDumpExecutableHelper(IProcessHelper processHelper, IFileHelper fileHelper, IEnvironment environment, IEnvironmentVariableHelper environmentVariableHelper)
    {
        _processHelper = processHelper;
        _fileHelper = fileHelper;
        _environment = environment;
        _environmentVariableHelper = environmentVariableHelper;
    }

    public static string ProcDumpFileName(PlatformArchitecture architecture) =>
        architecture switch
        {
            PlatformArchitecture.X86 => "procdump.exe",
            PlatformArchitecture.ARM64 => "procdump64a.exe",
            _ => "procdump64.exe",
        };

    public bool TryGetProcDumpExecutable(out string path)
    {
        // Use machine architecture
        var targetProcessArchitecture = _environment.Architecture;
        return TryGetProcDumpExecutable(targetProcessArchitecture, out path);
    }

    public bool TryGetProcDumpExecutable(int processId, out string path)
    {
        // Launch proc dump according to process architecture
        var targetProcessArchitecture = _processHelper.GetProcessArchitecture(processId);
        return TryGetProcDumpExecutable(targetProcessArchitecture, out path);
    }

    public bool TryGetProcDumpExecutable(PlatformArchitecture architecture, out string path)
    {
        var procdumpDirectory = _environmentVariableHelper.GetEnvironmentVariable("PROCDUMP_PATH");
        var searchPath = false;
        if (procdumpDirectory.IsNullOrWhiteSpace())
        {
            EqtTrace.Verbose("ProcDumpExecutableHelper.GetProcDumpExecutable: PROCDUMP_PATH env variable is empty will try to run ProcDump from PATH.");
            searchPath = true;
        }
        else if (!_fileHelper.DirectoryExists(procdumpDirectory))
        {
            EqtTrace.Verbose($"ProcDumpExecutableHelper.GetProcDumpExecutable: PROCDUMP_PATH env variable '{procdumpDirectory}' is not a directory, or the directory does not exist. Will try to run ProcDump from PATH.");
            searchPath = true;
        }

        string filename = _environment.OperatingSystem == PlatformOperatingSystem.Windows
            ? ProcDumpFileName(architecture)
            : _environment.OperatingSystem is PlatformOperatingSystem.Unix or PlatformOperatingSystem.OSX
                ? ProcdumpUnixProcess
                : throw new NotSupportedException($"Not supported platform {_environment.OperatingSystem}");

        if (!searchPath)
        {
            var candidatePath = Path.Combine(procdumpDirectory!, filename);
            if (_fileHelper.Exists(candidatePath))
            {
                EqtTrace.Verbose($"ProcDumpExecutableHelper.GetProcDumpExecutable: Path to ProcDump '{candidatePath}' exists, using that.");
                path = candidatePath;
                return true;
            }

            EqtTrace.Verbose($"ProcDumpExecutableHelper.GetProcDumpExecutable: Path '{candidatePath}' does not exist will try to run {filename} from PATH.");
        }

        if (TryGetExecutablePath(filename, out var p))
        {
            EqtTrace.Verbose($"ProcDumpExecutableHelper.GetProcDumpExecutable: Resolved {filename} to {p} from PATH.");
            path = p;
            return true;
        }

        EqtTrace.Verbose($"ProcDumpExecutableHelper.GetProcDumpExecutable: Could not find {filename} on PATH.");
        path = filename;
        return false;
    }

    private bool TryGetExecutablePath(string executable, out string executablePath)
    {
        executablePath = string.Empty;
        var pathString = _environmentVariableHelper.GetEnvironmentVariable("PATH") ?? string.Empty;
        foreach (string path in pathString.Split(Path.PathSeparator))
        {
            string exeFullPath = Path.Combine(path.Trim(), executable);
            if (_fileHelper.Exists(exeFullPath))
            {
                executablePath = exeFullPath;
                return true;
            }
        }

        return false;
    }
}
