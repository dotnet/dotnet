// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.TestPlatform.Build.Trace;
using Microsoft.TestPlatform.Build.Utils;

namespace Microsoft.TestPlatform.Build.Tasks;

public class VSTestForwardingApp
{
    private const string HostExe = "dotnet";
    private readonly List<string> _allArgs = new();
    private int _activeProcessId;

    public VSTestForwardingApp(string vsTestExePath, IEnumerable<string> argsToForward)
    {
        _allArgs.Add("exec");

        // Ensure that path to vstest.console is whitespace friendly. User may install
        // dotnet-cli to any folder containing whitespace (e.g. VS installs to program files).
        // Arguments are already whitespace friendly.
        _allArgs.Add(ArgumentEscaper.HandleEscapeSequenceInArgForProcessStart(vsTestExePath));
        _allArgs.AddRange(argsToForward);
    }

    public int Execute()
    {
        var processInfo = new ProcessStartInfo
        {
            FileName = HostExe,
            Arguments = string.Join(" ", _allArgs),
            UseShellExecute = false,
        };

        Tracing.Trace("VSTest: Starting vstest.console...");
        Tracing.Trace("VSTest: Arguments: " + processInfo.FileName + " " + processInfo.Arguments);

        using var activeProcess = new Process { StartInfo = processInfo };
        activeProcess.Start();
        _activeProcessId = activeProcess.Id;

        activeProcess.WaitForExit();
        Tracing.Trace("VSTest: Exit code: " + activeProcess.ExitCode);
        return activeProcess.ExitCode;
    }

    public void Cancel()
    {
        try
        {
            Process.GetProcessById(_activeProcessId).Kill();
        }
        catch (ArgumentException ex)
        {
            Tracing.Trace($"VSTest: Killing process throws ArgumentException with the following message {ex}. It may be that process is not running");
        }
    }
}
