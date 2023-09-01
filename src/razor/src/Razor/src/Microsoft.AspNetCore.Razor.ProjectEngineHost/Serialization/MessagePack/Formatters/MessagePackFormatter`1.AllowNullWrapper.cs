// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using MessagePack;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters;

internal abstract partial class MessagePackFormatter<T>
{
    public readonly ref struct AllowNullWrapper(MessagePackFormatter<T> formatter)
    {
        private readonly MessagePackFormatter<T> _formatter = formatter;

        public T? Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
            => reader.TryReadNil() ? default : _formatter.Deserialize(ref reader, options);

        public void Serialize(ref MessagePackWriter writer, T? value, MessagePackSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNil();
            }
            else
            {
                _formatter.Serialize(ref writer, value, options);
            }
        }

        public string? DeserializeString(ref MessagePackReader reader, MessagePackSerializerOptions options)
            => s_stringFormatter.Deserialize(ref reader, options);
    }
}
