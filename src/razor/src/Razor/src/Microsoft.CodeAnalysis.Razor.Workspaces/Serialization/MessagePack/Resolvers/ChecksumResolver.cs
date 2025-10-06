// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using MessagePack;
using MessagePack.Formatters;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Formatters;

namespace Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Resolvers;

internal sealed class ChecksumResolver : IFormatterResolver
{
    public static readonly ChecksumResolver Instance = new();

    private ChecksumResolver()
    {
    }

    public IMessagePackFormatter<T>? GetFormatter<T>()
    {
        return Cache<T>.Formatter;
    }

    private static class Cache<T>
    {
        public static readonly IMessagePackFormatter<T>? Formatter;

        static Cache()
        {
            if (typeof(T) == typeof(Checksum))
            {
                Formatter = (IMessagePackFormatter<T>)ChecksumFormatter.Instance;
            }
        }
    }
}
