// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MessagePack;
using MessagePack.Formatters;

namespace NuGet.VisualStudio.Internal.Contracts
{
    internal abstract class NuGetMessagePackFormatter<T> : IMessagePackFormatter<T?>
        where T : class
    {
        public T? Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            // stack overflow mitigation - see https://github.com/neuecc/MessagePack-CSharp/security/advisories/GHSA-7q36-4xx7-xcxf
            options.Security.DepthStep(ref reader);

            try
            {
                return DeserializeCore(ref reader, options);
            }
            finally
            {
                // stack overflow mitigation - see https://github.com/neuecc/MessagePack-CSharp/security/advisories/GHSA-7q36-4xx7-xcxf
                reader.Depth--;
            }
        }

        public void Serialize(ref MessagePackWriter writer, T? value, MessagePackSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNil();
                return;
            }

            SerializeCore(ref writer, value, options);
        }

        protected abstract T? DeserializeCore(ref MessagePackReader reader, MessagePackSerializerOptions options);

        protected abstract void SerializeCore(ref MessagePackWriter writer, T value, MessagePackSerializerOptions options);
    }
}
