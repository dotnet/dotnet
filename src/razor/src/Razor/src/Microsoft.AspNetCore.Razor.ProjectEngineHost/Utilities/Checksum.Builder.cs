// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.Utilities;

internal sealed partial record Checksum
{
    private static readonly ObjectPool<IncrementalHash> s_incrementalHashPool = DefaultPool.Create(IncrementalHashPoolPolicy.Instance);

    internal readonly ref struct Builder
    {
        private enum TypeKind : byte
        {
            Null,
            Bool,
            Int32,
            Int64,
            String,
            Checksum,
        }

        // Small, per-thread array to use as a buffer for appending primitive values to the hash.
        [ThreadStatic]
        private static byte[]? s_buffer;

        private readonly IncrementalHash _hash;

        public Builder()
        {
            _hash = s_incrementalHashPool.Get();
        }

        static byte[] GetBuffer()
            => s_buffer ??= new byte[8];

        public Checksum FreeAndGetChecksum()
        {
            var result = From(_hash.GetHashAndReset());
            s_incrementalHashPool.Return(_hash);
            return result;
        }

        private static void AppendTypeKind(IncrementalHash hash, TypeKind kind)
        {
            var buffer = GetBuffer();
            buffer[0] = (byte)kind;
            hash.AppendData(buffer, offset: 0, count: 1);
        }

        private static void AppendBoolValue(IncrementalHash hash, bool value)
        {
            var buffer = GetBuffer();
            buffer[0] = (byte)(value ? 1 : 0);
            hash.AppendData(buffer, offset: 0, count: sizeof(bool));
        }

        private static void AppendInt32Value(IncrementalHash hash, int value)
        {
            var buffer = GetBuffer();
            BinaryPrimitives.WriteInt32LittleEndian(buffer.AsSpan(0, sizeof(int)), value);
            hash.AppendData(buffer, offset: 0, count: sizeof(int));
        }

        private static void AppendInt64Value(IncrementalHash hash, long value)
        {
            var buffer = GetBuffer();
            BinaryPrimitives.WriteInt64LittleEndian(buffer.AsSpan(0, sizeof(long)), value);
            hash.AppendData(buffer, offset: 0, count: sizeof(long));
        }

        private static void AppendStringValue(IncrementalHash hash, string value)
        {
            var stringBytes = MemoryMarshal.AsBytes(value.AsSpan());
            Debug.Assert(stringBytes.Length == value.Length * 2);

            var buffer = ArrayPool<byte>.Shared.Rent(4 * 1024);
            try
            {
                var index = 0;
                while (index < stringBytes.Length)
                {
                    var remaining = stringBytes.Length - index;
                    var toCopy = Math.Min(remaining, buffer.Length);
                    stringBytes.Slice(index, toCopy).CopyTo(buffer);
                    hash.AppendData(buffer, 0, toCopy);

                    index += toCopy;
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        private static void AppendHashDataValue(IncrementalHash hash, HashData value)
        {
            AppendInt64Value(hash, value.Data1);
            AppendInt64Value(hash, value.Data2);
            AppendInt64Value(hash, value.Data3);
            AppendInt64Value(hash, value.Data4);
        }

        public void AppendNull()
        {
            AppendTypeKind(_hash, TypeKind.Null);
        }

        public void AppendData(bool value)
        {
            AppendTypeKind(_hash, TypeKind.Bool);
            AppendBoolValue(_hash, value);
        }

        public void AppendData(int value)
        {
            AppendTypeKind(_hash, TypeKind.Int32);
            AppendInt32Value(_hash, value);
        }

        public void AppendData(long value)
        {
            AppendTypeKind(_hash, TypeKind.Int64);
            AppendInt64Value(_hash, value);
        }

        public void AppendData(string? value)
        {
            if (value is null)
            {
                AppendNull();
                return;
            }

            AppendTypeKind(_hash, TypeKind.String);
            AppendStringValue(_hash, value);
        }

        public void AppendData(Checksum value)
        {
            AppendTypeKind(_hash, TypeKind.Checksum);
            AppendHashDataValue(_hash, value.Data);
        }
    }
}
