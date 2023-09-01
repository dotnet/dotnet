// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.AspNetCore.Razor.Test.Common;

public sealed class ThrowingTraceListener : TraceListener
{
    private static bool s_throwingTraceListenerAdded;
    private static readonly object s_gate = new();

    public static ThrowingTraceListener? AddToListeners()
    {
        ThrowingTraceListener? result = null;
        lock (s_gate)
        {
            if (s_throwingTraceListenerAdded)
            {
                return result;
            }

            var sawThrowingTraceListener = false;
            var listeners = Trace.Listeners;
            for (var i = listeners.Count - 1; i >= 0; i--)
            {
                switch (listeners[i])
                {
                    case DefaultTraceListener listener:
                        // Don't show assert UI during test runs
                        listener.AssertUiEnabled = false;
                        break;
                    case ThrowingTraceListener:
                        sawThrowingTraceListener = true;
                        break;
                }
            }

            if (!sawThrowingTraceListener)
            {
                // Add an instance of the ThrowingTraceListener so that Debug.Assert and Debug.Fail
                // throw exceptions during test runs.
                result = new ThrowingTraceListener();
                listeners.Add(result);
            }

            s_throwingTraceListenerAdded = true;
        }

        return result;
    }

    private readonly List<string> _fails = new();

    public string[] Fails => _fails.ToArray();

    public override void Fail(string? message, string? detailMessage)
    {
        var stackTrace = new StackTrace(fNeedFileInfo: true);
        var logMessage = (string.IsNullOrEmpty(message) ? "Assertion failed" : message) +
            (string.IsNullOrEmpty(detailMessage) ? "" : Environment.NewLine + detailMessage);

        _fails.Add($"{logMessage}{Environment.NewLine}{stackTrace}");

        throw new InvalidOperationException(logMessage);
    }

    public override void Write(object? o)
    {
    }

    public override void Write(object? o, string? category)
    {
    }

    public override void Write(string? message)
    {
    }

    public override void Write(string? message, string? category)
    {
    }

    public override void WriteLine(object? o)
    {
    }

    public override void WriteLine(object? o, string? category)
    {
    }

    public override void WriteLine(string? message)
    {
    }

    public override void WriteLine(string? message, string? category)
    {
    }
}
