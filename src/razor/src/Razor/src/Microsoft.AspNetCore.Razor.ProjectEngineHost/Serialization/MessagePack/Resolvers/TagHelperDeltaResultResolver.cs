// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
            if (typeof(T) == typeof(TagHelperDeltaResult))
            {
                Formatter = (IMessagePackFormatter<T>)TagHelperDeltaResultFormatter.Instance;
            }
        }
    }
}
