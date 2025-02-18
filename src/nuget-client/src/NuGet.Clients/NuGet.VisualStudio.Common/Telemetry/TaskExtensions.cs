// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace NuGet.VisualStudio.Telemetry
{
    public static class TaskExtensions
    {
        /// <summary> Records error information when the given task faults. </summary>
        /// <param name="task"> Task to execute. </param>
        /// <param name="callerClassName"> Caller class name. </param>
        /// <param name="callerMemberName"> Caller member name. </param>
        public static void PostOnFailure(this Task task, string callerClassName, [CallerMemberName] string callerMemberName = null)
        {
            _ = Task.Run(async () =>
            {
#pragma warning disable CA1031 // Do not catch general exception types
                try
                {
#pragma warning disable VSTHRD003 // Avoid awaiting foreign Tasks
                    await task.ConfigureAwait(false);
#pragma warning restore VSTHRD003 // Avoid awaiting foreign Tasks
                }
                catch (OperationCanceledException)
                {
                    //We ignore this exception as it is expected to be thrown when the task is cancelled.
                }
                catch (Exception e)
                {
                    await TelemetryUtility.PostFaultAsync(e, callerClassName, callerMemberName);
                }
#pragma warning restore CA1031 // Do not catch general exception types
            });
        }

        /// <summary> Records error information when the given task faults. </summary>
        /// <param name="task"> Task to execute. </param>
        /// <param name="callerClassName"> Caller class name. </param>
        /// <param name="callerMemberName"> Caller member name. </param>
        /// <returns>True, if the task did now throw an unhandled exception</returns>
        public static async Task<bool> PostOnFailureAsync(this Task task, string callerClassName, [CallerMemberName] string callerMemberName = null)
        {
            var success = true;
#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
#pragma warning disable VSTHRD003 // Avoid awaiting foreign Tasks
                await task.ConfigureAwait(false);
#pragma warning restore VSTHRD003 // Avoid awaiting foreign Tasks
            }
            catch (OperationCanceledException)
            {
                //We ignore this exception as it is expected to be thrown when the task is cancelled.
            }
            catch (Exception e)
            {
                await TelemetryUtility.PostFaultAsync(e, callerClassName, callerMemberName);
                success = false;
            }
#pragma warning restore CA1031 // Do not catch general exception types
            return success;
        }
    }
}
