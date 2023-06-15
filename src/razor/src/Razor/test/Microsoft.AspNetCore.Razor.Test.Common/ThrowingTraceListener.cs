// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Microsoft.AspNetCore.Razor.Test.Common;

public sealed class ThrowingTraceListener : TraceListener
{
    private static bool s_throwingTraceListenerAdded;
    private static readonly object s_gate = new();

    public static void AddToListeners()
    {
        lock (s_gate)
        {
            if (s_throwingTraceListenerAdded)
            {
                return;
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
                listeners.Add(new ThrowingTraceListener());
            }

            s_throwingTraceListenerAdded = true;
        }
    }

    public override void Fail(string? message, string? detailMessage)
    {
        throw new InvalidOperationException(
            (string.IsNullOrEmpty(message) ? "Assertion failed" : message) +
            (string.IsNullOrEmpty(detailMessage) ? "" : Environment.NewLine + detailMessage));
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
