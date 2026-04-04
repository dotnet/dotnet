#nullable enable

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.LinuxTracepoints.Provider;

using System;

/// <summary>
/// Flags used when registering a user_events tracepoint.
/// From LINUX/include/uapi/linux/user_events.h enum user_reg_flag.
/// </summary>
[Flags]
internal enum PerfUserEventReg : UInt16
{
    /// <summary>
    /// No flags set (default).
    /// </summary>
    None,

    /// <summary>
    /// USER_EVENT_REG_PERSIST:
    /// Event will not delete upon last reference closing.
    /// </summary>
    Persist = 1 << 0,

    /// <summary>
    /// USER_EVENT_REG_MULTI_FORMAT:
    /// Event will be allowed to have multiple formats.
    /// </summary>
    MultiFormat = 1 << 1,
}

/// <summary>
/// Direct access to a user_events tracepoint -- caller is responsible for the registration
/// string and for packing/marshalling event data.
/// <br/>
/// For more information, see <a href="https://docs.kernel.org/trace/user_events.html">user_events</a>.
/// <br/>
/// The tracepoint is registered by the constructor.
/// You'll generally construct all of your application's Tracepoint objects at application
/// start or component initialization.
/// <br/>
/// You'll use the IsEnabled property to determine
/// whether any sessions are collecting the tracecpoint, and you'll use the Write method
/// to write events.
/// <br/>
/// Normal usage:
/// <code>
/// Tracepoint tp = new PerfTracepoint("MyEventName int MyField1; int MyField2");
///
/// // To log an event where preparing the data is very simple:
/// tp.Write(data...);
///
/// // To log an event where preparing the data is expensive:
/// if (tp.IsEnabled) // Skip preparing data and calling Write if the tracepoint is not enabled.
/// {
///     var data = ...; // Prepare data that needs to be logged.
///     tp.Write(data...);
/// }
/// </code>
/// Note that tracepoint registration can fail, and Write operations can also fail.
/// The RegisterResult property and the error code returned by the Write method are provided
/// for debugging and diagnostics, but you'll usually ignore these in normal operation (most
/// applications should continue to work even if tracing isn't working).
/// </summary>
internal class PerfTracepoint : IDisposable
{
    private readonly TracepointHandle handle;

    /// <summary>
    /// As a performance optimization, avoid one level of indirection during calls to IsEnabled
    /// by caching the enablement array. The array always has Length == 1. The contents of this
    /// array should be considered read-only and MUST NOT be modified (the array data is updated
    /// by the Linux kernel when the tracepoint is enabled or disabled).
    /// <br/>
    /// When handle.IsInvalid, one array is shared by all invalid handles and is a normal allocation.
    /// When !handle.IsInvalid, the array is unique for for each handle and is a pinned allocation.
    /// </summary>
    private readonly Int32[] enablementArray;

    /// <summary>
    /// Given a user_events command string, attempts to register a user_events tracepoint.
    /// <br/>
    /// If registration succeeds, the new tracepoint will be valid and active:
    /// IsEnabled is dynamic, RegisterResult == 0, Write sends data to the kernel.
    /// <br/>
    /// If registration fails, the new tracepoint will be invalid and inactive:
    /// IsEnabled == false, RegisterResult != 0, Write does nothing and immediately returns EBADF.
    /// </summary>
    /// <param name="nameArgs">
    /// <a href="https://docs.kernel.org/trace/user_events.html#registering">user_events command string</a>,
    /// e.g. "MyEventName int arg1; u32 arg2". String should use only Latin-1 characters (each
    /// char in the string should have value 255 or below).
    /// </param>
    /// <param name="flags">
    /// <a href="https://docs.kernel.org/trace/user_events.html#registering">user_reg flags</a>,
    /// e.g. USER_EVENT_REG_PERSIST or USER_EVENT_REG_MULTI_FORMAT.
    /// </param>
    public PerfTracepoint(ReadOnlySpan<char> nameArgs, PerfUserEventReg flags = 0)
    {
        var h = TracepointHandle.Register(nameArgs, flags);
        this.handle = h;
        this.enablementArray = h.DangerousGetEnablementArray();
    }

    /// <summary>
    /// Given a NUL-terminated Latin1-encoded user_events command string, attempts to
    /// register a user_events tracepoint.
    /// <br/>
    /// If registration succeeds, the new tracepoint will be valid and active:
    /// IsEnabled is dynamic, RegisterResult == 0, Write sends data to the kernel.
    /// <br/>
    /// If registration fails, the new tracepoint will be invalid and inactive:
    /// IsEnabled == false, RegisterResult != 0, Write does nothing and immediately returns EBADF.
    /// </summary>
    /// <param name="nulTerminatedNameArgs">
    /// <a href="https://docs.kernel.org/trace/user_events.html#registering">user_events command string</a>,
    /// Latin-1 encoded, NUL-terminated, e.g. "MyEventName int arg1; u32 arg2\0".
    /// </param>
    /// <param name="flags">
    /// <a href="https://docs.kernel.org/trace/user_events.html#registering">user_reg flags</a>,
    /// e.g. USER_EVENT_REG_PERSIST or USER_EVENT_REG_MULTI_FORMAT.
    /// </param>
    /// <exception cref="ArgumentException">
    /// nulTerminatedNameArgs does not contain any NUL termination (no 0 bytes).
    /// </exception>
    public PerfTracepoint(ReadOnlySpan<byte> nulTerminatedNameArgs, PerfUserEventReg flags = 0)
    {
        if (0 > nulTerminatedNameArgs.LastIndexOf((byte)0))
        {
            throw new ArgumentException(
                nameof(nulTerminatedNameArgs) + " must be NUL-terminated, i.e. must contain (byte)0.",
                nameof(nulTerminatedNameArgs));
        }

        var h = TracepointHandle.Register(nulTerminatedNameArgs, flags);
        this.handle = h;
        this.enablementArray = h.DangerousGetEnablementArray();
    }

    /// <summary>
    /// Returns true if this tracepoint is registered and one or more tracepoint collection sessions
    /// are collecting this tracepoint.
    /// <br/>
    /// Returns false if this tracepoint is unregistered or if there are no tracepoint collection
    /// sessions that are collecting this tracepoint. The tracepoint will be unregistered if
    /// registration failed or if the tracepoint has been disposed.
    /// <br/>
    /// Note that this property is provided to support performance optimization, but use of this
    /// property is optional. It's ok to call Write even if IsEnabled returns false. If your
    /// tracepoint is not being collected, the Write method will do nothing and will immediately
    /// return EBADF. This property is provided so that you can efficiently skip preparing your data
    /// and calling the Write method if your tracepoint is not being collected.
    /// </summary>
    public bool IsEnabled => this.enablementArray[0] != 0;

    /// <summary>
    /// If tracepoint registration succeeded, returns 0.
    /// Otherwise, returns an errno indicating the error.
    /// <br/>
    /// This property is for diagnostic purposes and should usually be ignored for normal
    /// operation -- most programs should continue to operate even if trace registration
    /// fails.
    /// </summary>
    public int RegisterResult => this.handle.RegisterResult;

    /// <summary>
    /// Unregisters the tracepoint. After calling Dispose(), IsEnabled will return false and
    /// Write will do nothing and immediately return EBADF.
    /// </summary>
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// If !IsEnabled, immediately returns EBADF.
    /// Otherwise, writes an event with no data.
    /// </summary>
    /// <returns>
    /// 0 if event was written, errno otherwise.
    /// Typically returns EBADF if no data collection sessions are listening for the tracepoint.
    /// The return value is for debugging/diagnostic purposes and is usually ignored in normal operation
    /// since most programs should continue to function even when tracing is not configured.
    /// </returns>
    public int Write()
    {
        if (this.enablementArray[0] == 0)
        {
            return TracepointHandle.DisabledEventError;
        }

        return this.handle.Write(stackalloc DataSegment[] {
            default, // segment[0] is used for headers.
        });
    }

    /// <summary>
    /// If !IsEnabled, immediately returns EBADF.
    /// Otherwise, writes an event with 1 chunk of data.
    /// </summary>
    /// <returns>
    /// 0 if event was written, errno otherwise.
    /// Typically returns EBADF if no data collection sessions are listening for the tracepoint.
    /// The return value is for debugging/diagnostic purposes and is usually ignored in normal operation
    /// since most programs should continue to function even when tracing is not configured.
    /// </returns>
    public int Write<T1>(
        ReadOnlySpan<T1> v1)
        where T1 : unmanaged
    {
        if (this.enablementArray[0] == 0)
        {
            return TracepointHandle.DisabledEventError;
        }

        unsafe
        {
            fixed (void*
                p1 = v1)
            {
                return this.handle.Write(stackalloc DataSegment[] {
                    default, // segment[0] is used for headers.
                    new DataSegment(p1, (uint)v1.Length * (uint)sizeof(T1)),
                });
            }
        }
    }

    /// <summary>
    /// If !IsEnabled, immediately returns EBADF.
    /// Otherwise, writes an event with 2 chunks of data.
    /// </summary>
    /// <returns>
    /// 0 if event was written, errno otherwise.
    /// Typically returns EBADF if no data collection sessions are listening for the tracepoint.
    /// The return value is for debugging/diagnostic purposes and is usually ignored in normal operation
    /// since most programs should continue to function even when tracing is not configured.
    /// </returns>
    public int Write<T1, T2>(
        ReadOnlySpan<T1> v1,
        ReadOnlySpan<T2> v2)
        where T1 : unmanaged
        where T2 : unmanaged
    {
        if (this.enablementArray[0] == 0)
        {
            return TracepointHandle.DisabledEventError;
        }

        unsafe
        {
            fixed (void*
                p1 = v1,
                p2 = v2)
            {
                return this.handle.Write(stackalloc DataSegment[] {
                    default, // segment[0] is used for headers.
                    new DataSegment(p1, (uint)v1.Length * (uint)sizeof(T1)),
                    new DataSegment(p2, (uint)v2.Length * (uint)sizeof(T2)),
                });
            }
        }
    }

    /// <summary>
    /// If !IsEnabled, immediately returns EBADF.
    /// Otherwise, writes an event with 3 chunks of data.
    /// </summary>
    /// <returns>
    /// 0 if event was written, errno otherwise.
    /// Typically returns EBADF if no data collection sessions are listening for the tracepoint.
    /// The return value is for debugging/diagnostic purposes and is usually ignored in normal operation
    /// since most programs should continue to function even when tracing is not configured.
    /// </returns>
    public int Write<T1, T2, T3>(
        ReadOnlySpan<T1> v1,
        ReadOnlySpan<T2> v2,
        ReadOnlySpan<T3> v3)
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
    {
        if (this.enablementArray[0] == 0)
        {
            return TracepointHandle.DisabledEventError;
        }

        unsafe
        {
            fixed (void*
                p1 = v1,
                p2 = v2,
                p3 = v3)
            {
                return this.handle.Write(stackalloc DataSegment[] {
                    default, // segment[0] is used for headers.
                    new DataSegment(p1, (uint)v1.Length * (uint)sizeof(T1)),
                    new DataSegment(p2, (uint)v2.Length * (uint)sizeof(T2)),
                    new DataSegment(p3, (uint)v3.Length * (uint)sizeof(T3)),
                });
            }
        }
    }

    /// <summary>
    /// If !IsEnabled, immediately returns EBADF.
    /// Otherwise, writes an event with 4 chunks of data.
    /// </summary>
    /// <returns>
    /// 0 if event was written, errno otherwise.
    /// Typically returns EBADF if no data collection sessions are listening for the tracepoint.
    /// The return value is for debugging/diagnostic purposes and is usually ignored in normal operation
    /// since most programs should continue to function even when tracing is not configured.
    /// </returns>
    public int Write<T1, T2, T3, T4>(
        ReadOnlySpan<T1> v1,
        ReadOnlySpan<T2> v2,
        ReadOnlySpan<T3> v3,
        ReadOnlySpan<T4> v4)
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
    {
        if (this.enablementArray[0] == 0)
        {
            return TracepointHandle.DisabledEventError;
        }

        unsafe
        {
            fixed (void*
                p1 = v1,
                p2 = v2,
                p3 = v3,
                p4 = v4)
            {
                return this.handle.Write(stackalloc DataSegment[] {
                    default, // segment[0] is used for headers.
                    new DataSegment(p1, (uint)v1.Length * (uint)sizeof(T1)),
                    new DataSegment(p2, (uint)v2.Length * (uint)sizeof(T2)),
                    new DataSegment(p3, (uint)v3.Length * (uint)sizeof(T3)),
                    new DataSegment(p4, (uint)v4.Length * (uint)sizeof(T4)),
                });
            }
        }
    }

    /// <summary>
    /// If !IsEnabled, immediately returns EBADF.
    /// Otherwise, writes an event with 5 chunks of data.
    /// </summary>
    /// <returns>
    /// 0 if event was written, errno otherwise.
    /// Typically returns EBADF if no data collection sessions are listening for the tracepoint.
    /// The return value is for debugging/diagnostic purposes and is usually ignored in normal operation
    /// since most programs should continue to function even when tracing is not configured.
    /// </returns>
    public int Write<T1, T2, T3, T4, T5>(
        ReadOnlySpan<T1> v1,
        ReadOnlySpan<T2> v2,
        ReadOnlySpan<T3> v3,
        ReadOnlySpan<T4> v4,
        ReadOnlySpan<T5> v5)
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
        where T5 : unmanaged
    {
        if (this.enablementArray[0] == 0)
        {
            return TracepointHandle.DisabledEventError;
        }

        unsafe
        {
            fixed (void*
                p1 = v1,
                p2 = v2,
                p3 = v3,
                p4 = v4,
                p5 = v5)
            {
                return this.handle.Write(stackalloc DataSegment[] {
                    default, // segment[0] is used for headers.
                    new DataSegment(p1, (uint)v1.Length * (uint)sizeof(T1)),
                    new DataSegment(p2, (uint)v2.Length * (uint)sizeof(T2)),
                    new DataSegment(p3, (uint)v3.Length * (uint)sizeof(T3)),
                    new DataSegment(p4, (uint)v4.Length * (uint)sizeof(T4)),
                    new DataSegment(p5, (uint)v5.Length * (uint)sizeof(T5)),
                });
            }
        }
    }

    /// <summary>
    /// If disposing is true, calls this.handle.Dispose().
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.handle.Dispose();
        }
    }
}
