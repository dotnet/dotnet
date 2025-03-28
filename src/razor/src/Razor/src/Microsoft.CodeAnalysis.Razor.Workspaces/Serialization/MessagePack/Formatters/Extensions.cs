// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using MessagePack;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters;

internal static class Extensions
{
    public static void ReadArrayHeaderAndVerify(this ref MessagePackReader reader, int expectedCount)
    {
        if (reader.NextMessagePackType != MessagePackType.Array)
        {
            throw new MessagePackSerializationException($"Expected next type to be {MessagePackType.Array}, but it was {reader.NextMessagePackType}");
        }

        var count = reader.ReadArrayHeader();

        if (count != expectedCount)
        {
            throw new MessagePackSerializationException($"Expected {expectedCount} values, but there were {count}");
        }
    }

    public static void Serialize<T>(this ref MessagePackWriter writer, T? value, MessagePackSerializerOptions options)
        where T : class
    {
        if (value is null)
        {
            writer.WriteNil();
            return;
        }

        options.Resolver.GetFormatterWithVerify<T>().Serialize(ref writer, value, options);
    }

    public static T Deserialize<T>(this ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        return options.Resolver.GetFormatterWithVerify<T>().Deserialize(ref reader, options);
    }

    public static T? DeserializeOrNull<T>(this ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        if (reader.TryReadNil())
        {
            return default;
        }

        return options.Resolver.GetFormatterWithVerify<T>().Deserialize(ref reader, options);
    }
}

internal static class ExtraExtensions
{
    // C# allows extension method overloads to differ only by generic constraints, but they must be declared on
    // different classes, since they'll have the same signature.
    public static void Serialize<T>(this ref MessagePackWriter writer, T value, MessagePackSerializerOptions options)
        where T : struct
    {
        options.Resolver.GetFormatterWithVerify<T>().Serialize(ref writer, value, options);
    }
}
