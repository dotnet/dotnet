// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MessagePack;
using MessagePack.Formatters;
using NuGet.Versioning;

namespace NuGet.VisualStudio.Internal.Contracts
{
    internal sealed class NuGetVersionFormatter : NuGetMessagePackFormatter<NuGetVersion>
    {
        private const string OriginalStringOrToStringPropertyName = "originalstringortostring";

        internal static readonly IMessagePackFormatter<NuGetVersion?> Instance = new NuGetVersionFormatter();

        private NuGetVersionFormatter()
        {
        }

        protected override NuGetVersion? DeserializeCore(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            int propertyCount = reader.ReadMapHeader();

            string? version = null;

            for (int propertyIndex = 0; propertyIndex < propertyCount; propertyIndex++)
            {
                switch (reader.ReadString())
                {
                    case OriginalStringOrToStringPropertyName:
                        version = reader.ReadString();
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            return NuGetVersion.Parse(version!);
        }

        protected override void SerializeCore(ref MessagePackWriter writer, NuGetVersion value, MessagePackSerializerOptions options)
        {
            writer.WriteMapHeader(count: 1);
            writer.Write(OriginalStringOrToStringPropertyName);
            writer.Write(value.OriginalVersion ?? value.ToString());
        }
    }
}
