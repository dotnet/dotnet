// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Utilities;
using System;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.DotNet.Build.Tasks.VersionTools
{
    public static class TraceListenerCollectionExtensions
    {
        /// <summary>
        /// Adds listeners to Trace that pass output into the given msbuild logger, invokes the
        /// action, then cleans up the listeners. This makes Trace calls visible in build output.
        /// VersionTools logs using Trace.
        /// </summary>
        public static void MsBuildListenedInvoke(
            this TraceListenerCollection listenerCollection,
            TaskLoggingHelper log,
            Action action,
            TraceEventType eventTypeFlags =
                TraceEventType.Error |
                TraceEventType.Warning |
                TraceEventType.Critical |
                TraceEventType.Information |
                TraceEventType.Verbose)
        {
            MsBuildTraceListener[] listeners = Enum.GetValues(typeof(TraceEventType))
                .Cast<TraceEventType>()
                .Where(type => (type & eventTypeFlags) != 0)
                .Select(type => new MsBuildTraceListener(log, type))
                .ToArray();

            listenerCollection.AddRange(listeners);
            try
            {
                action();
            }
            finally
            {
                foreach (MsBuildTraceListener listener in listeners)
                {
                    /// Call flush on each listener in case the last call was Write.
                    listenerCollection.Remove(listener);
                    listener.Flush();
                }
            }
        }
    }
}
