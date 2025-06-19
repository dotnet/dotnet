// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETCOREAPP

using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.TestHost;

internal class TestHostTraceListener : DefaultTraceListener
{
    public static void Setup()
    {
        EqtTrace.Info("Setting up debug trace listener.");
        // in the majority of cases there will be only a single DefaultTraceListener in this collection
        // and we will replace that with our listener, in case there are listeners of different types we keep
        // them as is
        for (var i = 0; i < Trace.Listeners.Count; i++)
        {
            var listener = Trace.Listeners[i];
            if (listener is DefaultTraceListener)
            {
                EqtTrace.Verbose($"TestPlatformTraceListener.Setup: Replacing listener {Trace.Listeners[i]} with {nameof(TestHostTraceListener)}.");
                Trace.Listeners[i] = new TestHostTraceListener();
            }
        }

        EqtTrace.Verbose("TestPlatformTraceListener.Setup: Added test platform trace listener.");
    }

    public override void Fail(string? message)
    {
        throw GetException(message);
    }

    public override void Fail(string? message, string? detailMessage)
    {
        throw GetException((message + Environment.NewLine + detailMessage));
    }

    private static DebugAssertException GetException(string? message)
    {
        var stack = new StackTrace(true);
        var debugMethodFound = false;
        var frameCount = 0;
        MethodBase? method = null;
        var frames = stack.GetFrames();
        foreach (var f in frames)
        {
            var m = f?.GetMethod();
            var declaringType = m?.DeclaringType;
            if (!debugMethodFound && (declaringType == typeof(Debug) || declaringType == typeof(Trace)))
            {
                method = m;
                debugMethodFound = true;
            }

            if (debugMethodFound)
            {
                frameCount++;
            }
        }

        var stackTrace = new StackTrace(frames.TakeLast(frameCount)).ToString();
        var methodName = method != null ? $"{method.DeclaringType?.Name}.{method.Name}" : "<method>";
        var wholeMessage = $"Method {methodName} failed with '{message}', and was translated to {typeof(DebugAssertException).FullName} to avoid terminating the process hosting the test.";

        return new DebugAssertException(wholeMessage, stackTrace);
    }
}

#endif
