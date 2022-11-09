// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Text;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Extensions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;

namespace Microsoft.TestPlatform.TestHostProvider.Hosting;

internal class TestHostManagerCallbacks
{
    public static void ErrorReceivedCallback(StringBuilder testHostProcessStdError, string? data)
    {
        // Log all standard error message because on too much data we ignore starting part.
        // This is helpful in abnormal failure of testhost.
        EqtTrace.Warning("TestHostManagerCallbacks.ErrorReceivedCallback Test host standard error line: {0}", data);

        testHostProcessStdError.AppendSafeWithNewLine(data);
    }

    public static void ExitCallBack(
        IProcessHelper processHelper,
        object? process,
        StringBuilder testHostProcessStdError,
        Action<HostProviderEventArgs> onHostExited)
    {
        EqtTrace.Verbose("TestHostProvider.ExitCallBack: Host exited starting callback.");
        var testHostProcessStdErrorStr = testHostProcessStdError.ToString();

        processHelper.TryGetExitCode(process, out int exitCode);

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
