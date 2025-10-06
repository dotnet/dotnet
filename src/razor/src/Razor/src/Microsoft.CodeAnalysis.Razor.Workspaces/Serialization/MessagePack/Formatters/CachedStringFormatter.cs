// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using MessagePack;

namespace Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Formatters;

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
