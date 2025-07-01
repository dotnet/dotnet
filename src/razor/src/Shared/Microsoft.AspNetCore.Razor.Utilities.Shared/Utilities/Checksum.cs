// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
#if NETCOREAPP && !NET8_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif
using System.Runtime.InteropServices;

namespace Microsoft.AspNetCore.Razor.Utilities;

internal sealed partial record Checksum
{
    // Size of SHA-256
    private const int HashSize = 256 / 8;

    public static readonly Checksum Null = new(default(HashData));

    public readonly HashData Data;

    public Checksum(HashData data)
        => Data = data;

    public static Checksum From(ReadOnlySpan<byte> source)
    {
        if (source.Length == 0)
        {
            return Null;
        }

        if (source.Length != HashSize)
        {
            throw new ArgumentException($"{nameof(source)} size must be equal to {HashSize}", nameof(source));
        }

        if (!MemoryMarshal.TryRead(source, out HashData hash))
        {
            throw new InvalidOperationException("Could not read hash data");
        }

        return new Checksum(hash);
    }

    public string ToBase64String()
    {
#if NETCOREAPP
        Span<byte> bytes = stackalloc byte[HashSize];
#if NET8_0_OR_GREATER
        MemoryMarshal.TryWrite(bytes, in Data);
#else
        MemoryMarshal.TryWrite(bytes, ref Unsafe.AsRef(in Data));
#endif

        return Convert.ToBase64String(bytes);
#else
        unsafe
        {
            var data = new byte[HashSize];
            fixed (byte* dataPtr = data)
            {
                *(HashData*)dataPtr = Data;
            }

            return Convert.ToBase64String(data, offset: 0, length: HashSize);
        }
#endif
    }

    public static Checksum FromBase64String(string value)
        => value is null
            ? Null
            : From(Convert.FromBase64String(value));

    public override string ToString()
        => ToBase64String();
}
