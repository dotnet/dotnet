// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Extensions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;

namespace Microsoft.TestPlatform.TestHostProvider.Hosting;

internal class TestHostManagerCallbacks
{
    private const int E_HANDLE = unchecked((int)0x80070006);
    private readonly bool _forwardOutput;
    private readonly IMessageLogger? _messageLogger;

    public TestHostManagerCallbacks(bool forwardOutput, IMessageLogger? logger)
    {
        if (forwardOutput)
        {
            string? but = null;
            if (logger == null)
            {
                but = " But logger is null, so it won't forward any output.";
            }
            EqtTrace.Verbose($"TestHostManagerCallbacks.ctor: Forwarding output is enabled.{but}");
        }
        else
        {
            EqtTrace.Verbose($"TestHostManagerCallbacks.ctor: Forwarding output is disabled.");
        }
        _forwardOutput = forwardOutput;
        _messageLogger = logger;
    }

    public void StandardOutputReceivedCallback(StringBuilder testHostProcessStdOut, string? data)
    {
        EqtTrace.Verbose("TestHostManagerCallbacks.StandardOutputReceivedCallback Test host standard output line: {0}", data);
        testHostProcessStdOut.AppendSafeWithNewLine(data);
        if (_forwardOutput && _messageLogger != null && !StringUtils.IsNullOrWhiteSpace(data))
        {
            _messageLogger.SendMessage(TestMessageLevel.Informational, data);
        }
    }

    public void ErrorReceivedCallback(StringBuilder testHostProcessStdError, string? data)
    {
        // Log all standard error message because on too much data we ignore starting part.
        // This is helpful in abnormal failure of testhost.
        EqtTrace.Warning("TestHostManagerCallbacks.ErrorReceivedCallback Test host standard error line: {0}", data);

        testHostProcessStdError.AppendSafeWithNewLine(data);
        if (_forwardOutput && _messageLogger != null && !StringUtils.IsNullOrWhiteSpace(data))
        {
            // Forward the error output, but DO NOT forward it as error. Until now it was only written into logs,
            // and applications love to write Debug messages into error stream. Which we do not want to fail the test run.
            _messageLogger.SendMessage(TestMessageLevel.Informational, data);
        }
    }

    public static void ExitCallBack(
        IProcessHelper processHelper,
        object? process,
        StringBuilder testHostProcessStdError,
        Action<HostProviderEventArgs> onHostExited)
    {
        EqtTrace.Verbose("TestHostProvider.ExitCallBack: Host exited starting callback.");
        var testHostProcessStdErrorStr = testHostProcessStdError.ToString();

        int exitCode;
        try
        {
            processHelper.TryGetExitCode(process, out exitCode);
        }
        catch (COMException ex) when (ex.HResult == E_HANDLE)
        {
            exitCode = -1;
            EqtTrace.Error("TestHostProvider.ExitCallBack: Invalid process handle we cannot get the error code, error {0}.", ex);
        }

        int procId = -1;
        try
        {
            if (process is Process p)
            {
                procId = p.Id;
            }
        }
        catch (InvalidOperationException ex)
        {
            EqtTrace.Error("TestHostProvider.ExitCallBack: could not get proccess id from process, error: {0}.", ex);
        }

        if (exitCode != 0)
        {
            EqtTrace.Error("TestHostManagerCallbacks.ExitCallBack: Testhost processId: {0} exited with exitcode: {1} error: '{2}'", procId, exitCode, testHostProcessStdErrorStr);
        }
        else
        {
            EqtTrace.Info("TestHostManagerCallbacks.ExitCallBack: Testhost processId: {0} exited with exitcode: 0 error: '{1}'", procId, testHostProcessStdErrorStr);
        }

        onHostExited(new HostProviderEventArgs(testHostProcessStdErrorStr, exitCode, procId));
    }
}
