// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using MessagePack;
using MessagePack.Formatters;
using Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Resolvers;

internal sealed class TagHelperDeltaResultResolver : IFormatterResolver
{
    public static readonly TagHelperDeltaResultResolver Instance = new();

    private TagHelperDeltaResultResolver()
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
            Formatter = (IMessagePackFormatter<T>?)TypeToFormatterMap.GetFormatter(typeof(T));
        }
    }

    private static class TypeToFormatterMap
    {
        private static readonly Dictionary<Type, object> s_map = new()
        {
            TagHelperDeltaResultFormatter.Instance,
            ChecksumFormatter.Instance
        };

        public static object? GetFormatter(Type t)
        {
            if (s_map.TryGetValue(t, out var formatter))
            {
                return formatter;
            }

            return null;
        }
    }
}
