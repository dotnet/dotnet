#nullable enable

namespace Microsoft.LinuxTracepoints.Provider
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    /// <summary>
    /// 16-byte raw value storage for IPv6, UUID, or any other 16-byte value
    /// that needs to be used as a field in an EventHeader event.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct EventHeaderValue128
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public byte Byte0;
        public byte Byte1;
        public byte Byte2;
        public byte Byte3;
        public byte Byte4;
        public byte Byte5;
        public byte Byte6;
        public byte Byte7;
        public byte Byte8;
        public byte Byte9;
        public byte Byte10;
        public byte Byte11;
        public byte Byte12;
        public byte Byte13;
        public byte Byte14;
        public byte Byte15;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// Casts the specified byte span to an EventHeaderValue128.
        /// </summary>
        /// <param name="source">A span of 16 or more bytes.</param>
        /// <exception cref="ArgumentOutOfRangeException">source.Length is less than 16.</exception>
        public EventHeaderValue128(ReadOnlySpan<byte> source)
        {
            if (source.Length < 16)
            {
                throw new ArgumentOutOfRangeException(nameof(source), nameof(source) + ".Length < 16");
            }

            Unsafe.CopyBlockUnaligned(ref this.Byte0, ref MemoryMarshal.GetReference(source), 16);
        }
    }
}
