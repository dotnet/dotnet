// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
#if !NET5_0_OR_GREATER
using System.Buffers;
#endif
using System.Buffers.Binary;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.Extensions.ObjectPool;

// PERFORMANCE: Care has been taken to avoid using IncrementalHash on .NET Framework, which can cause
// threadpool starvation. Essentially, on .NET Framework, IncrementalHash ends up using the OS implementation
// of SHA-256, which creates several finalizable objects in the form of safe handles. By creating instances
// of SHA256 for .NET Framework (and netstandard2.0), we get the managed code version of SHA-256, which
// doesn't have the overhead of using the OS implementations.
//
// See https://github.com/dotnet/roslyn/issues/67995 for more detail.

#if NET5_0_OR_GREATER
using HashingType = System.Security.Cryptography.IncrementalHash;
#else
using HashingType = System.Security.Cryptography.SHA256;
#endif

namespace Microsoft.AspNetCore.Razor.Utilities;

internal sealed partial record Checksum
{
    internal readonly ref partial struct Builder
    {
        private static readonly ObjectPool<HashingType> s_hashPool = DefaultPool.Create(Policy.Instance);

        private enum TypeKind : byte
        {
            Null,
            Bool,
            Int32,
            Int64,
            String,
            Checksum,
            Byte,
            Char,
        }

        // Small, per-thread array to use as a buffer for appending primitive values to the hash.
        [ThreadStatic]
        private static byte[]? s_buffer;

        private readonly HashingType _hash;

        public Builder()
        {
            _hash = s_hashPool.Get();

#if !NET5_0_OR_GREATER
            _hash.Initialize();
#endif
        }

        static byte[] GetBuffer()
            => s_buffer ??= new byte[8];

        public Checksum FreeAndGetChecksum()
        {
#if NET5_0_OR_GREATER
            Span<byte> hash = stackalloc byte[HashSize];
            _hash.GetHashAndReset(hash);
            var result = From(hash);
#else
            _hash.TransformFinalBlock(inputBuffer: [], inputOffset: 0, inputCount: 0);
            var result = From(_hash.Hash);
#endif

            s_hashPool.Return(_hash);
            return result;
        }

        private void AppendBuffer(int count)
        {
            Debug.Assert(s_buffer is not null);

#if NET5_0_OR_GREATER
            _hash.AppendData(s_buffer, offset: 0, count);
#else
            _hash.TransformBlock(s_buffer, inputOffset: 0, inputCount: count, outputBuffer: null, outputOffset: 0);
#endif
        }

        private void AppendTypeKind(TypeKind kind)
        {
            var buffer = GetBuffer();
            buffer[0] = (byte)kind;
            AppendBuffer(count: 1);
        }

        private void AppendBoolValue(bool value)
        {
            AppendByteValue((byte)(value ? 1 : 0));
        }

        private void AppendByteValue(byte value)
        {
            var buffer = GetBuffer();
            buffer[0] = value;
            AppendBuffer(count: sizeof(byte));
        }

        private void AppendCharValue(char value)
        {
            var buffer = GetBuffer();
            BinaryPrimitives.WriteUInt16LittleEndian(buffer.AsSpan(0, sizeof(char)), value);
            AppendBuffer(count: sizeof(char));
        }

        private void AppendInt32Value(int value)
        {
            var buffer = GetBuffer();
            BinaryPrimitives.WriteInt32LittleEndian(buffer.AsSpan(0, sizeof(int)), value);
            AppendBuffer(count: sizeof(int));
        }

        private void AppendInt64Value(long value)
        {
            var buffer = GetBuffer();
            BinaryPrimitives.WriteInt64LittleEndian(buffer.AsSpan(0, sizeof(long)), value);
            AppendBuffer(count: sizeof(long));
        }

        private void AppendStringValue(string value)
        {
#if NET5_0_OR_GREATER
            _hash.AppendData(MemoryMarshal.AsBytes(value.AsSpan()));
            _hash.AppendData(MemoryMarshal.AsBytes("\0".AsSpan()));
#else
            using var _ = ArrayPool<byte>.Shared.GetPooledArray(4 * 1024, out var buffer);

            AppendData(_hash, buffer, value);
            AppendData(_hash, buffer, "\0");

            static void AppendData(HashingType hash, byte[] buffer, string value)
            {
                var stringBytes = MemoryMarshal.AsBytes(value.AsSpan());
                Debug.Assert(stringBytes.Length == value.Length * 2);

                var index = 0;
                while (index < stringBytes.Length)
                {
                    var remaining = stringBytes.Length - index;
                    var toCopy = Math.Min(remaining, buffer.Length);

                    stringBytes.Slice(index, toCopy).CopyTo(buffer);
                    hash.TransformBlock(buffer, inputOffset: 0, inputCount: toCopy, outputBuffer: null, outputOffset: 0);

                    index += toCopy;
                }
            }
#endif
        }
        private void AppendHashDataValue(HashData value)
        {
            AppendInt64Value(value.Data1);
            AppendInt64Value(value.Data2);
            AppendInt64Value(value.Data3);
            AppendInt64Value(value.Data4);
        }

        public void AppendNull()
        {
            AppendTypeKind(TypeKind.Null);
        }

        public void AppendData(bool value)
        {
            AppendTypeKind(TypeKind.Bool);
            AppendBoolValue(value);
        }

        public void AppendData(byte value)
        {
            AppendTypeKind(TypeKind.Byte);
            AppendByteValue(value);
        }

        public void AppendData(char value)
        {
            AppendTypeKind(TypeKind.Char);
            AppendCharValue(value);
        }

        public void AppendData(int value)
        {
            AppendTypeKind(TypeKind.Int32);
            AppendInt32Value(value);
        }

        public void AppendData(long value)
        {
            AppendTypeKind(TypeKind.Int64);
            AppendInt64Value(value);
        }

        public void AppendData(string? value)
        {
            if (value is null)
            {
                AppendNull();
                return;
            }

            AppendTypeKind(TypeKind.String);
            AppendStringValue(value);
        }

        public void AppendData(Checksum value)
        {
            AppendTypeKind(TypeKind.Checksum);
            AppendHashDataValue(value.Data);
        }

        public void AppendData(object? value)
        {
            switch (value)
            {
                case null:
                    AppendNull();
                    break;

                case string s:
                    AppendData(s);
                    break;

                case Checksum c:
                    AppendData(c);
                    break;

                case bool b:
                    AppendData(b);
                    break;

                case int i:
                    AppendData(i);
                    break;

                case long l:
                    AppendData(l);
                    break;

                case byte b:
                    AppendData(b);
                    break;

                case char c:
                    AppendData(c);
                    break;

                default:
                    throw new ArgumentException(
                        SR.FormatUnsupported_type_0(value.GetType().FullName), nameof(value));
            }
        }
    }
}
