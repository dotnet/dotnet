// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MessagePack;
using MessagePack.Formatters;
using Microsoft;
using NuGet.Configuration;

namespace NuGet.VisualStudio.Internal.Contracts
{
    internal sealed class PackageSourceContextInfoFormatter : NuGetMessagePackFormatter<PackageSourceContextInfo>
    {
        private const string SourcePropertyName = "source";
        private const string IsEnabledPropertyName = "isenabled";
        private const string ProtocolVersionPropertyName = "protocolversion";
        private const string IsMachineWidePropertyName = "ismachinewide";
        private const string NamePropertyName = "name";
        private const string DescriptionPropertyName = "description";
        private const string OriginalHashCodePropertyName = "originalhashcode";

        internal static readonly IMessagePackFormatter<PackageSourceContextInfo?> Instance = new PackageSourceContextInfoFormatter();

        private PackageSourceContextInfoFormatter()
        {
        }

        protected override PackageSourceContextInfo? DeserializeCore(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            string? source = null;
            string? name = null;
            bool isMachineWide = false;
            bool isEnabled = true;
            string? description = null;
            int originalHashCode = 0;
            int protocolVersion = PackageSource.DefaultProtocolVersion;

            int propertyCount = reader.ReadMapHeader();
            for (int propertyIndex = 0; propertyIndex < propertyCount; propertyIndex++)
            {
                switch (reader.ReadString())
                {
                    case SourcePropertyName:
                        source = reader.ReadString();
                        break;
                    case IsEnabledPropertyName:
                        isEnabled = reader.ReadBoolean();
                        break;
                    case IsMachineWidePropertyName:
                        isMachineWide = reader.ReadBoolean();
                        break;
                    case NamePropertyName:
                        name = reader.ReadString();
                        break;
                    case DescriptionPropertyName:
                        description = reader.ReadString();
                        break;
                    case OriginalHashCodePropertyName:
                        originalHashCode = reader.ReadInt32();
                        break;
                    case ProtocolVersionPropertyName:
                        protocolVersion = reader.ReadInt32();
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            Assumes.NotNullOrEmpty(source);
            Assumes.NotNullOrEmpty(name);

            return new PackageSourceContextInfo(source, name, isEnabled, protocolVersion)
            {
                IsMachineWide = isMachineWide,
                Description = description,
                OriginalHashCode = originalHashCode,
            };
        }

        protected override void SerializeCore(ref MessagePackWriter writer, PackageSourceContextInfo value, MessagePackSerializerOptions options)
        {
            writer.WriteMapHeader(count: 7);
            writer.Write(SourcePropertyName);
            writer.Write(value.Source);
            writer.Write(ProtocolVersionPropertyName);
            writer.Write(value.ProtocolVersion);
            writer.Write(IsEnabledPropertyName);
            writer.Write(value.IsEnabled);
            writer.Write(IsMachineWidePropertyName);
            writer.Write(value.IsMachineWide);
            writer.Write(NamePropertyName);
            writer.Write(value.Name);
            writer.Write(OriginalHashCodePropertyName);
            writer.Write(value.OriginalHashCode);
            writer.Write(DescriptionPropertyName);
            writer.Write(value.Description);
        }
    }
}
