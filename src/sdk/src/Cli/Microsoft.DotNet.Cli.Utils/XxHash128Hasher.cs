// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NET

using System.Diagnostics;
using System.IO.Hashing;

namespace Microsoft.DotNet.Cli.Utils;

public static class XxHash128Hasher
{
    public static string HashWithNormalizedCasing(string text)
    {
        text = text.ToUpperInvariant();
        var bytes = Encoding.UTF8.GetBytes(text);
        Span<byte> hash = stackalloc byte[sizeof(ulong) * 2];
        int bytesWritten = XxHash128.Hash(bytes.AsSpan(), hash);
        Debug.Assert(bytesWritten == hash.Length);
        return Convert.ToHexStringLower(hash);
    }
}

#endif
