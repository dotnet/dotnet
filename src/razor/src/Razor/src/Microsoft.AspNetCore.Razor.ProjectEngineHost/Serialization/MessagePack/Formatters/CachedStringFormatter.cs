// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using MessagePack;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters;

internal sealed class CachedStringFormatter : ValueFormatter<string?>
{
    public static readonly ValueFormatter<string?> Instance = new CachedStringFormatter();

    private CachedStringFormatter()
    {
    }

    public override string? Deserialize(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        if (reader.NextMessagePackType == MessagePackType.Integer)
        {
            var referenceId = reader.ReadInt32();
            return options.Strings.GetValue(referenceId);
        }

        var result = reader.ReadString();

        if (result is not null)
        {
            options.Strings.Add(result);
        }

        return result;
    }

    public override void Serialize(ref MessagePackWriter writer, string? value, SerializerCachingOptions options)
    {
        if (value is null)
        {
            writer.WriteNil();
        }
        else if (options.Strings.TryGetReferenceId(value, out var referenceId))
        {
            writer.Write(referenceId);
        }
        else
        {
            writer.Write(value);
            options.Strings.Add(value);
        }
    }

    public override void Skim(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        if (reader.NextMessagePackType == MessagePackType.Integer)
        {
            reader.Skip(); // Reference Id
            return;
        }

        var result = reader.ReadString();

        if (result is not null)
        {
            options.Strings.Add(result);
        }
    }
}
