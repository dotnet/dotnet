// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.TestPlatform.Extensions.BlameDataCollector;

public class Win32NamedEvent
{
    private const uint EventModifyState = 0x0002;

    private readonly string _eventName;

    /// <summary>
    /// Initializes a new instance of the <see cref="Win32NamedEvent"/> class.
    /// Create a NamedEvent object with the name of the event, and assume auto reset with
    /// an initial state of reset.
    /// </summary>
    /// <param name="eventName">The name of the win32 event</param>
    public Win32NamedEvent(string eventName)
    {
        _eventName = eventName;
    }

    /// <summary>
    /// Set the named event to a signaled state. The Wait() method will not block any
    /// thread as long as the event is in a signaled state.
    /// </summary>
    public void Set()
    {
        IntPtr handle = OpenEvent(EventModifyState, false, _eventName);
        SetEvent(handle);
        CloseHandle(handle);
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CloseHandle(IntPtr hObject);

    [DllImport("kernel32.dll")]
    private static extern bool SetEvent(IntPtr hEvent);

    [DllImport("Kernel32.dll", SetLastError = true)]
    private static extern IntPtr OpenEvent(uint dwDesiredAccess, bool bInheritHandle, string lpName);
}
