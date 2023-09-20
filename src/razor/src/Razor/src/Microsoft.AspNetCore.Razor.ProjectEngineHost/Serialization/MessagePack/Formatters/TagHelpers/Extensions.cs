// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using MessagePack;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters.TagHelpers;

internal static class Extensions
{
    public static string? ReadString(this ref MessagePackReader reader, TagHelperSerializationCache? cache)
    {
        if (cache is null)
        {
            return reader.ReadString();
        }

        if (reader.NextMessagePackType == MessagePackType.Integer)
        {
            var referenceId = reader.ReadInt32();
            return cache.Strings.GetValue(referenceId);
        }

        var result = reader.ReadString();

        if (result is not null)
        {
            cache.Strings.Add(result);
        }

        return result;
    }

    public static void Write(this ref MessagePackWriter writer, string? value, TagHelperSerializationCache? cache)
    {
        if (cache is null)
        {
            writer.Write(value);
            return;
        }

        if (value is null)
        {
            writer.WriteNil();
        }
        else if (cache.Strings.TryGetReferenceId(value, out var referenceId))
        {
            writer.Write(referenceId);
        }
        else
        {
            writer.Write(value);
            cache.Strings.Add(value);
        }
    }
}
