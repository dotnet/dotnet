#nullable enable

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.LinuxTracepoints.Provider;

using System;
using System.IO;
using System.Runtime.InteropServices;
using Debug = System.Diagnostics.Debug;
using Interlocked = System.Threading.Interlocked;

/// <summary>
/// Low-level owner of a tracepoint registration.
/// Handle is the tracepoint's write_index.
/// Uses a pinned allocation for the tracepoint's is-enabled buffer.
/// </summary>
internal sealed class TracepointHandle : SafeHandle
{
    /// <summary>
    /// The error to return from Write to a disabled event = EBADF = 9.
    /// </summary>
    public const int DisabledEventError = 9;

    /// <summary>
    /// The error to return from Write of an event that is too big = E2BIG = 7.
    /// </summary>
    public const int EventTooBigError = 7;

    private const byte EnableSize = sizeof(UInt32);

    private const int EBADF = 9;
    private const int UnknownErrno = int.MaxValue;
    private const int UnregisteredWriteIndex = -1;

    private const int IOC_NRSHIFT = 0;
    private const int IOC_TYPESHIFT = IOC_NRSHIFT + 8; // 8 = IOC_NRBITS
    private const int IOC_SIZESHIFT = IOC_TYPESHIFT + 8; // 8 = IOC_TYPEBITS
    private const int IOC_DIRSHIFT = IOC_SIZESHIFT + 14; // 14 = IOC_SIZEBITS
    private const uint IOC_WRITE = 1;
    private const uint IOC_READ = 2;
    private const uint DIAG_IOC_MAGIC = '*';

    private static RawFileHandle? userEventsDataStatic;
    private static Int32[]? emptyEnabledPinned; // From normal heap.

    /// <summary>
    /// When this.IsInvalid, enabledPinned is a shared emptyEnabledPinned from the normal heap.
    /// When !this.IsInvalid, enabledPinned is a unique allocation from the pinned object heap.
    /// </summary>
    private Int32[] enabledPinned;

    /// <summary>
    /// Initializes a new handle that is invalid.
    /// </summary>
#pragma warning disable CA1419 // Provide a parameterless constructor. (We don't need the runtime to create instances of TracepointHandle.)
    private TracepointHandle()
#pragma warning restore CA1419
        : base((nint)UnregisteredWriteIndex, true)
    {
        var enabledPinned = emptyEnabledPinned ??
            Utility.InterlockedInitSingleton(ref emptyEnabledPinned, new Int32[1]);
        Debug.Assert(0 == enabledPinned[0]);
        this.enabledPinned = enabledPinned;
    }

    /// <summary>
    /// If registration succeeded, returns 0.
    /// If registration failed, returns the errno from the failed open/ioctl.
    /// </summary>
    public int RegisterResult { get; private set; } = UnknownErrno;

    /// <summary>
    /// Returns true if this tracepoint is enabled, false otherwise.
    /// Value may change at any time while the tracepoint is registered.
    /// </summary>
    public bool IsEnabled => 0 != this.enabledPinned[0];

    /// <summary>
    /// Returns true if registration was successful.
    /// (Remains true even after handle is closed/disposed.)
    /// </summary>
    public override bool IsInvalid => (nint)UnregisteredWriteIndex == this.handle;

    /// <summary>
    /// Given an all-Latin1 user_events command string (no characters with value > 255),
    /// attempts to register it. Returns a TracepointHandle with the result.
    /// Syntax for nameArgs is given here:
    /// https://docs.kernel.org/trace/user_events.html#command-format,
    /// e.g. "MyEventName int arg1; u32 arg2".
    /// <br/>
    /// If registration succeeds, the returned handle will be valid and active:
    /// IsInvalid == false, IsEnabled is dynamic, RegisterResult == 0,
    /// Write is meaningful.
    /// <br/>
    /// If registration fails, the returned handle will be invalid and inactive:
    /// IsInvalid == true, IsEnabled == false, RegisterResult != 0,
    /// Write will always return EBADF.
    /// </summary>
    public static TracepointHandle Register(ReadOnlySpan<char> nameArgs, PerfUserEventReg flags = 0)
    {
        Span<byte> nulTerminatedNameArgs = stackalloc byte[nameArgs.Length + 1];
        for (var i = 0; i < nameArgs.Length; i += 1)
        {
            nulTerminatedNameArgs[i] = unchecked((byte)nameArgs[i]);
        }

        nulTerminatedNameArgs[nameArgs.Length] = 0;
        return Register(nulTerminatedNameArgs, flags);
    }

    /// <summary>
    /// Given a NUL-terminated Latin1 user_events command string, attempts to register it.
    /// Returns a TracepointHandle with the result.
    /// Syntax for nameArgs is given here:
    /// https://docs.kernel.org/trace/user_events.html#command-format,
    /// e.g. "MyEventName int arg1; u32 arg2".
    /// <br/>
    /// If registration succeeds, the returned handle will be valid and active:
    /// IsInvalid == false, IsEnabled is meaningful, RegisterResult == 0,
    /// Write is meaningful.
    /// <br/>
    /// If registration fails, the returned handle will be invalid and inactive:
    /// IsInvalid == true, IsEnabled == false, RegisterResult != 0,
    /// Write will always return EBADF.
    /// </summary>
    public static TracepointHandle Register(ReadOnlySpan<byte> nulTerminatedNameArgs, PerfUserEventReg flags = 0)
    {
        Debug.Assert(0 <= nulTerminatedNameArgs.LastIndexOf((byte)0));

        var tracepoint = new TracepointHandle();

        var userEventsData = userEventsDataStatic ?? InitUserEventsDataStatic();
        if (userEventsData.OpenResult != 0)
        {
            tracepoint.InitFailed(userEventsData.OpenResult);
        }
        else
        {
            var enabledPinned = GC.AllocateArray<Int32>(1, pinned: true);
            Debug.Assert(0 == enabledPinned[0]);

            int ioctlResult;
            unsafe
            {
                fixed (Int32* enabledPtr = enabledPinned)
                {
                    fixed (byte* nameArgsPtr = nulTerminatedNameArgs)
                    {
                        var reg = new user_reg
                        {
                            size = user_reg.SizeOfStruct,
                            enable_size = EnableSize,
                            flags = flags,
                            enable_addr = (nuint)enabledPtr,
                            name_args = (nuint)nameArgsPtr,
                        };

                        // Need a finally block to make sure the thread is not interrupted
                        // between the ioctl and the SetHandle.
                        try
                        {
                            // Nothing.
                        }
                        finally
                        {
                            var DIAG_IOCSREG =
                                ((IOC_WRITE | IOC_READ) << IOC_DIRSHIFT) |
                                (DIAG_IOC_MAGIC << IOC_TYPESHIFT) |
                                (0u << IOC_NRSHIFT) |
                                ((uint)IntPtr.Size << IOC_SIZESHIFT);
                            ioctlResult = userEventsData.Ioctl(DIAG_IOCSREG, ref reg);
                            if (ioctlResult >= 0)
                            {
                                tracepoint.enabledPinned = enabledPinned;
                                tracepoint.SetHandle((nint)reg.write_index);
                            }
                        }
                    }
                }
            }

            if (ioctlResult >= 0)
            {
                tracepoint.InitSucceeded();
            }
            else
            {
                var errno = Marshal.GetLastWin32Error();
                if (errno <= 0)
                {
                    errno = UnknownErrno;
                }

                tracepoint.InitFailed(errno);
            }
        }

        // All code paths should have called either InitSucceeded or InitFailed.
        return tracepoint;
    }

    /// <summary>
    /// Sends tracepoint data to the user_events_data file. Uses data[0] for headers.
    /// Returns EBADF if closed. Does NOT check IsEnabled (caller should do that).
    /// <br/>
    /// Requires: data[0].Length == 0 (data[0] will be used for headers).
    /// </summary>
    public int Write(Span<DataSegment> data)
    {
        // Precondition: slot for write_index in data[0].
        Debug.Assert(data[0].Length == 0);

        var userEventsData = userEventsDataStatic;
        Debug.Assert(userEventsData != null); // Otherwise there would be no TracepointHandle instance.
        Debug.Assert(userEventsData.OpenResult == 0); // Otherwise Enabled would be false.

        var writeIndex = new WriteIndexPlus { WriteIndex = (Int32)(nint)this.handle, Padding = 0 };
        unsafe
        {
            // Workaround: On old kernels, events with 0 bytes of data don't get written.
            // If event has 0 bytes of data, add a byte to avoid the problem.
            data[0] = new DataSegment(&writeIndex, sizeof(Int32) + (data.Length == 1 ? 1u : 0u));
        }

        if (this.IsClosed)
        {
            return DisabledEventError;
        }

        // Ignore race condition: if we're disposed between checking IsClosed and calling WriteV,
        // we will write the data using a write_index that doesn't belong to us. That's not great,
        // but it's not fatal and probably not worth degrading performance to avoid it. The
        // write_index is unlikely to be recycled during the race condition, and even if it is
        // recycled, the worst consequence would be a garbage event in a trace. Could avoid with:
        // try { AddRef; WriteV; } catch { return EBADF; } finally { Release; }.

        var writevResult = userEventsData.WriteV(data);
        return writevResult >= 0 ? 0 : Marshal.GetLastWin32Error();
    }

    /// <summary>
    /// Sends tracepoint with EventHeader to the user_events_data file. Uses data[0] for headers.
    /// Returns EBADF if closed. Does NOT check IsEnabled (caller should do that).
    /// <br/>
    /// Fills in data[0] with writeIndex + eventHeader + activityIdBlock? + metadataHeader?. Sets the extension
    /// block's flags based on metaLength.
    /// <br/>
    /// Requires: data[0].Length == 0 (data[0] will be used for headers).
    /// <br/>
    /// Requires: relatedId cannot be present unless activityId is present.
    /// <br/>
    /// Requires: If activityId is present or metaLength != 0 then
    /// eventHeader.Flags must equal DefaultWithExtension.
    /// <br/>
    /// Requires: If metaLength != 0 then data[1] starts with metadata extension block data.
    /// </summary>
    public unsafe int WriteEventHeader(
        EventHeader eventHeader,
        Guid* activityId,
        Guid* relatedId,
        ushort metaLength,
        Span<DataSegment> data)
    {
        // Precondition: slot for write_index in data[0].
        Debug.Assert(data[0].Length == 0);

        // Precondition: relatedId cannot be present unless activityId is present.
        Debug.Assert(relatedId == null || activityId != null);

        // Precondition: eventHeader.Flags must match up with presence of first extension.
        Debug.Assert((activityId == null && metaLength == 0) ||
            eventHeader.Flags == (EventHeader.DefaultFlags | EventHeaderFlags.Extension));

        // Precondition: metaLength implies metadata extension block data.
        Debug.Assert(metaLength == 0 || data.Length > 1);

        var userEventsData = userEventsDataStatic;
        Debug.Assert(userEventsData != null); // Otherwise there would be no TracepointHandle instance.
        Debug.Assert(userEventsData.OpenResult == 0); // Otherwise Enabled would be false.

        const byte HeadersMax = sizeof(Int32)               // writeIndex
            + EventHeader.SizeOfStruct                      // eventHeader
            + EventHeaderExtension.SizeOfStruct + 16 + 16   // activityId header + activityId + relatedId
            + EventHeaderExtension.SizeOfStruct;            // metadata header
        var writeIndex = (Int32)(nint)this.handle;
        unsafe
        {
            uint* headersUInt32 = stackalloc UInt32[HeadersMax / sizeof(UInt32)]; // Ensure 4-byte alignment.
            byte* headers = (byte*)headersUInt32;
            uint pos = 0;

            *(Int32*)&headers[pos] = (Int32)(nint)this.handle;
            pos += sizeof(Int32);

            *(EventHeader*)&headers[pos] = eventHeader;
            pos += EventHeader.SizeOfStruct;

            if (activityId != null)
            {
                var kind = EventHeaderExtensionKind.ActivityId | (metaLength == 0 ? 0 : EventHeaderExtensionKind.ChainFlag);
                if (relatedId != null)
                {
                    *(EventHeaderExtension*)&headers[pos] = new EventHeaderExtension { Kind = kind, Size = 32 };
                    pos += EventHeaderExtension.SizeOfStruct;
                    Utility.WriteGuidBigEndian(new Span<byte>(&headers[pos], 16), *activityId);
                    pos += 16;
                    Utility.WriteGuidBigEndian(new Span<byte>(&headers[pos], 16), *relatedId);
                    pos += 16;
                }
                else
                {
                    *(EventHeaderExtension*)&headers[pos] = new EventHeaderExtension { Kind = kind, Size = 16 };
                    pos += EventHeaderExtension.SizeOfStruct;
                    Utility.WriteGuidBigEndian(new Span<byte>(&headers[pos], 16), *activityId);
                    pos += 16;
                }
            }

            if (metaLength != 0)
            {
                *(EventHeaderExtension*)&headers[pos] = new EventHeaderExtension
                {
                    Kind = EventHeaderExtensionKind.Metadata, // Last one, so no chain flag.
                    Size = metaLength,
                };
                pos += EventHeaderExtension.SizeOfStruct;
            }

            data[0] = new DataSegment(headers, pos);
        }

        if (this.IsClosed)
        {
            return DisabledEventError;
        }

        // Ignore race condition: if we're disposed between checking IsClosed and calling WriteV,
        // we will write the data using a write_index that doesn't belong to us. That's not great,
        // but it's not fatal and probably not worth degrading performance to avoid it. The
        // write_index is unlikely to be recycled during the race condition, and even if it is
        // recycled, the worst consequence would be a garbage event in a trace. Could avoid with:
        // try { AddRef; WriteV; } catch { return EBADF; } finally { Release; }.

        var writevResult = userEventsData.WriteV(data);
        return writevResult >= 0 ? 0 : Marshal.GetLastWin32Error();
    }

    /// <summary>
    /// Returns an array of length 1 that contains the value that will be updated by the
    /// kernel when the tracepoint is enabled or disabled. Caller MUST NOT modify the contents
    /// of the array.
    /// <br/>
    /// When this.IsInvalid, the array is shared by all other invalid handles and is a normal allocation.
    /// When !this.IsInvalid, there is a separate array for each handle and is a pinned allocation.
    /// </summary>
    public Int32[] DangerousGetEnablementArray()
    {
        return this.enabledPinned;
    }

    protected override bool ReleaseHandle()
    {
        var userEventsData = userEventsDataStatic;
        Debug.Assert(userEventsData != null); // Otherwise there would be no TracepointHandle instance.

        var enabledPinned = this.enabledPinned;
        int ioctlResult;
        unsafe
        {
            Debug.Assert(!ReferenceEquals(enabledPinned, emptyEnabledPinned));
            fixed (Int32* enabledPtr = enabledPinned)
            {
                var unreg = new user_unreg
                {
                    size = user_unreg.SizeOfStruct,
                    disable_addr = (nuint)enabledPtr,
                };

                var DIAG_IOCSUNREG =
                    (IOC_WRITE << IOC_DIRSHIFT) |
                    (DIAG_IOC_MAGIC << IOC_TYPESHIFT) |
                    (2u << IOC_NRSHIFT) |
                    ((uint)IntPtr.Size << IOC_SIZESHIFT);
                ioctlResult = userEventsData.Ioctl(DIAG_IOCSUNREG, ref unreg);
            }
        }

        enabledPinned[0] = 0; // Force IsEnabled = false.
        return 0 <= ioctlResult;
    }

    /// <summary>
    /// Locates and opens the user_events_data file.
    /// First call to this will update userEventsDataStatic with the result.
    /// </summary>
    /// <returns>The new value of userEventsDataStatic (never null).</returns>
    private static RawFileHandle InitUserEventsDataStatic()
    {
        RawFileHandle? resultHandle = null;
        RawFileHandle? newHandle = null;
        try
        {
            newHandle = RawFileHandle.OpenWRONLY("/sys/kernel/tracing/user_events_data\0"u8);
            if (newHandle.OpenResult == 0)
            {
                // Success.
            }
            else if (!File.Exists("/proc/mounts"))
            {
                // Give up.
            }
            else
            {
                Span<byte> path = stackalloc byte[274]; // 256 + sizeof("/user_events_data\0")
                FileStream? mounts = null;
                try
                {
                    mounts = File.OpenRead("/proc/mounts");

                    Span<byte> line = stackalloc byte[4096];
                    bool eof = false;
                    while (!eof)
                    {
                        // ~fgets
                        int lineEnd;
                        for (lineEnd = 0; lineEnd < line.Length; lineEnd += 1)
                        {
                            var b = mounts.ReadByte();
                            if (b < 0)
                            {
                                eof = true;
                                break;
                            }
                            else if (b == '\n')
                            {
                                break;
                            }
                            else
                            {
                                line[lineEnd] = (byte)b;
                            }
                        }

                        // line is "device_name mount_point file_system other_stuff..."

                        int linePos = 0;

                        // device_name
                        while (linePos < lineEnd && IsNonspaceByte(line[linePos]))
                        {
                            linePos += 1;
                        }

                        // whitespace
                        while (linePos < lineEnd && IsSpaceByte(line[linePos]))
                        {
                            linePos += 1;
                        }

                        // mount_point
                        var mountBegin = linePos;
                        while (linePos < lineEnd && IsNonspaceByte(line[linePos]))
                        {
                            linePos += 1;
                        }

                        var mountEnd = linePos;

                        // whitespace
                        while (linePos < lineEnd && IsSpaceByte(line[linePos]))
                        {
                            linePos += 1;
                        }

                        // file_system
                        var fsBegin = linePos;
                        while (linePos < lineEnd && IsNonspaceByte(line[linePos]))
                        {
                            linePos += 1;
                        }

                        var fsEnd = linePos;

                        if (linePos == lineEnd || !IsSpaceByte(line[linePos]))
                        {
                            // Ignore line if no whitespace after file_system.
                            continue;
                        }

                        bool foundTraceFS;
                        var fs = line.Slice(fsBegin, fsEnd - fsBegin);
                        if (fs.SequenceEqual("tracefs"u8))
                        {
                            // "tracefsMountPoint/user_events_data"
                            foundTraceFS = true;
                        }
                        else if (path[0] == 0 && fs.SequenceEqual("debugfs"u8))
                        {
                            // "debugfsMountPoint/tracing/user_events_data"
                            foundTraceFS = false;
                        }
                        else
                        {
                            continue;
                        }

                        var pathSuffix0 = foundTraceFS
                            ? "/user_events_data\0"u8
                            : "/tracing/user_events_data\0"u8;

                        var mountLen = mountEnd - mountBegin;
                        var pathLen = mountLen + pathSuffix0.Length; // Includes NUL
                        if (pathLen > path.Length)
                        {
                            continue;
                        }

                        // path = mountpoint + suffix
                        line.Slice(mountBegin, mountLen).CopyTo(path);
                        pathSuffix0.CopyTo(path.Slice(mountLen)); // Includes NUL

                        if (foundTraceFS)
                        {
                            // Found a match, and it's tracefs, so stop looking.
                            break;
                        }
                        else
                        {
                            // Found a match, but it's debugfs. We prefer tracefs, so keep looking.
                        }
                    }
                }
                catch (ArgumentException) { }
                catch (IOException) { }
                catch (NotSupportedException) { }
                catch (UnauthorizedAccessException) { }
                finally
                {
                    mounts?.Dispose();
                }

                if (path[0] != 0)
                {
                    newHandle.Dispose();
                    newHandle = RawFileHandle.OpenWRONLY(path);
                }
            }

            var oldHandle = Interlocked.CompareExchange(ref userEventsDataStatic, newHandle, null);
            if (oldHandle != null)
            {
                resultHandle = oldHandle;
            }
            else
            {
                resultHandle = newHandle;
            }
        }
        finally
        {
            if (newHandle != null && newHandle != resultHandle)
            {
                newHandle.Dispose();
            }
        }

        return resultHandle;
    }

    private static bool IsSpaceByte(byte b)
    {
        return b == ' ' || b == '\t';
    }

    private static bool IsNonspaceByte(byte b)
    {
        return b != '\0' && !IsSpaceByte(b);
    }

    private void InitSucceeded()
    {
        this.RegisterResult = 0;
        Debug.Assert(1 >= this.enabledPinned[0]);
        Debug.Assert(!ReferenceEquals(this.enabledPinned, emptyEnabledPinned));
        Debug.Assert(!this.IsClosed);
        Debug.Assert(!this.IsInvalid);
    }

    private void InitFailed(int registerResult)
    {
        this.SetHandleAsInvalid();
        this.RegisterResult = registerResult;
        Debug.Assert(0 == this.enabledPinned[0]);
        Debug.Assert(ReferenceEquals(this.enabledPinned, emptyEnabledPinned));
        Debug.Assert(this.IsClosed);
        Debug.Assert(this.IsInvalid);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private struct user_reg
    {
        public const int SizeOfStruct = 28;
        public UInt32 size;
        public byte enable_bit;
        public byte enable_size;
        public PerfUserEventReg flags;
        public UInt64 enable_addr;
        public UInt64 name_args;
        public Int32 write_index;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private struct user_unreg
    {
        public const int SizeOfStruct = 16;
        public UInt32 size;
        public byte disable_bit;
        public byte reserved;
        public UInt16 reserved2;
        public UInt64 disable_addr;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct WriteIndexPlus
    {
        public Int32 WriteIndex;
        public UInt32 Padding;
    }
}
