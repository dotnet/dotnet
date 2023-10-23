// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Language;

internal static class ChecksumUtilities
{
    public static string BytesToString(ImmutableArray<byte> bytes)
    {
        if (bytes.IsDefault)
        {
            throw new ArgumentNullException(nameof(bytes));
        }

        using var _ = StringBuilderPool.GetPooledObject(out var builder);
        builder.EnsureCapacity(bytes.Length);

        foreach (var b in bytes)
        {
            // The x2 format means lowercase hex, where each byte is a 2-character string.
            builder.Append(b.ToString("x2", CultureInfo.InvariantCulture));
        }

        return builder.ToString();
    }
}
