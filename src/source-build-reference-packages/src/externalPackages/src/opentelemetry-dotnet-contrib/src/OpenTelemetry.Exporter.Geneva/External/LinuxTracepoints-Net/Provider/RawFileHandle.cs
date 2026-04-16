#nullable enable

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.LinuxTracepoints.Provider;

using System;
using System.Runtime.InteropServices;

/// <summary>
/// Wraps a raw Posix file descriptor returned from "open".
/// Non-negative handle is a valid descriptor.
/// Negative handle is a negative errno with the result of the "open" operation.
/// </summary>
internal sealed class RawFileHandle
    : SafeHandle
{
    private const int UnknownErrno = int.MaxValue;

    /// <summary>
    /// Initializes a new handle that is invalid.
    /// Do not use this constructor. To create a valid handle, call the static Open method.
    /// </summary>
    public RawFileHandle()
        : base(new IntPtr(-UnknownErrno), true)
    {
        return;
    }

    /// <summary>
    /// Returns true if handle is negative (stores a negative errno).
    /// </summary>
    public override bool IsInvalid => (nint)this.handle < 0;

    /// <summary>
    /// If open succeeded, returns 0. Otherwise returns the errno from open.
    /// </summary>
    public int OpenResult
    {
        get
        {
            var h = (nint)this.handle;
            return h >= 0 ? 0 : -(int)h;
        }
    }

    /// <summary>
    /// Calls "open" with the given path name and with flags = O_WRONLY.
    /// On success, returns a valid handle. On failure, returns an invalid handle
    /// (check OpenResult for the errno).
    /// </summary>
    public static RawFileHandle OpenWRONLY(ReadOnlySpan<byte> nulTerminatedPathName)
    {
        var result = new RawFileHandle();

        // Need a finally block to make sure the thread is not interrupted
        // between the open and the handle assignment.
        try
        {
            // Nothing.
        }
        finally
        {
            unsafe
            {
                fixed (byte* pathNamePtr = nulTerminatedPathName)
                {
                    const int O_WRONLY = 0x0001;
                    result.handle = (nint)NativeMethods.open(pathNamePtr, O_WRONLY, 0);
                }
            }
        }

        if (0 > (nint)result.handle)
        {
            var errno = Marshal.GetLastWin32Error();
            if (errno <= 0)
            {
                errno = UnknownErrno;
            }

            result.handle = new IntPtr(-errno);
        }

        return result;
    }

    /// <summary>
    /// Calls "writev" with the given data.
    /// On success, returns the number of bytes written.
    /// On error, returns -1 (check Marshal.GetLastWin32Error() for the errno).
    /// </summary>
    public nint WriteV(ReadOnlySpan<DataSegment> iovecs)
    {
        var needRelease = false;
        try
        {
            this.DangerousAddRef(ref needRelease);
            unsafe
            {
                fixed (DataSegment* iovecsPtr = iovecs)
                {
                    return NativeMethods.writev((Int32)(nint)this.handle, iovecsPtr, iovecs.Length);
                }
            }
        }
        finally
        {
            if (needRelease)
            {
                this.DangerousRelease();
            }
        }
    }

    /// <summary>
    /// Calls "ioctl" with the given request and data.
    /// On error, returns -1 (check Marshal.GetLastWin32Error() for the errno).
    /// </summary>
    public int Ioctl<T>(uint request, ref T data)
        where T : unmanaged
    {
        var needRelease = false;
        try
        {
            this.DangerousAddRef(ref needRelease);
            unsafe
            {
                fixed (void* dataPtr = &data)
                {
                    return NativeMethods.ioctl((Int32)(nint)this.handle, new UIntPtr(request), dataPtr);
                }
            }
        }
        finally
        {
            if (needRelease)
            {
                this.DangerousRelease();
            }
        }
    }

    protected override bool ReleaseHandle()
    {
        var h = unchecked((Int32)(nint)this.handle);
        return 0 <= NativeMethods.close(h);
    }

    private unsafe static class NativeMethods
    {
        [DllImport("libc", SetLastError = true)]
        public static extern int close(Int32 fd);

        [DllImport("libc", SetLastError = true)]
        public static extern int open(byte* pathname, Int32 flags, Int32 mode);

        [DllImport("libc", SetLastError = true)]
        public static extern int ioctl(Int32 fd, UIntPtr request, void* data);

        [DllImport("libc", SetLastError = true)]
        public static extern IntPtr writev(Int32 fd, DataSegment* iovec, Int32 iovecCount);
    }
}
