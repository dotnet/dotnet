// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Security.Cryptography;

namespace Microsoft.VisualStudio.SolutionPersistence.Utilities;

/// <summary>
/// Helper for turning unique string identifiers into guid identifiers.
/// </summary>
internal static class DefaultIdGenerator
{
    internal static Guid CreateIdFrom(string uniqueName)
    {
        byte[] bytes = StringToBytes(uniqueName);
        return MakeId(bytes, null);
    }

    internal static Guid CreateIdFrom(Guid parentItemId, string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return Guid.Empty;
        }

        byte[] parentData = parentItemId.ToByteArray();
        byte[] itemData = StringToBytes(name);
        return MakeId(parentData, itemData);
    }

    private static byte[] StringToBytes(string text)
    {
        return Encoding.UTF8.GetBytes(text.Replace('\\', '/').ToUpperInvariant());
    }

    private static Guid MakeId(byte[] data1, byte[]? data2)
    {
        if (data1.IsNullOrEmpty() && data2.IsNullOrEmpty())
        {
            return Guid.Empty;
        }

#if NETFRAMEWORK
        byte[] allData = data2 is null ? data1 : [.. data1, .. data2];

        using (SHA256 hash = SHA256.Create())
        {
            byte[] hashBytes = hash.ComputeHash(allData);
            byte[] guidBytes = new byte[16];
            Array.Copy(hashBytes, guidBytes, 16);
            return new Guid(guidBytes);
        }
#else
        ReadOnlySpan<byte> allData = data2 is null ? data1 : [.. data1, .. data2];

        Span<byte> hashBytes = stackalloc byte[32];
        if (!SHA256.TryHashData(allData, hashBytes, out int bytesWritten) || bytesWritten <= 16)
        {
            throw new InvalidOperationException();
        }

        return new Guid(hashBytes.Slice(0, 16));
#endif
    }
}
