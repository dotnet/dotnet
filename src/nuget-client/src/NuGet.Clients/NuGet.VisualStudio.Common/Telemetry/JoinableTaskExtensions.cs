// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.Threading;

namespace NuGet.VisualStudio.Telemetry
{
    public static class JoinableTaskExtensions
    {
        /// <summary> Records error information when the given task faults. </summary>
        /// <param name="joinableTask"> Joinable task to execute. </param>
        /// <param name="callerClassName"> Caller class name. </param>
        /// <param name="callerMemberName"> Caller member name. </param>
        public static void PostOnFailure(this JoinableTask joinableTask, string callerClassName, [CallerMemberName] string callerMemberName = null)
        {
            JoinableTask forget = NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                try
                {
#pragma warning disable VSTHRD003 // Avoid awaiting foreign Tasks - As a fire-and-forget continuation, deadlocks can't happen.
                    await joinableTask.Task.ConfigureAwait(false);
#pragma warning restore VSTHRD003
                }
                catch (Exception e)
                {
                    await TelemetryUtility.PostFaultAsync(e, callerClassName, callerMemberName);
                }
            });
        }
    }
}
