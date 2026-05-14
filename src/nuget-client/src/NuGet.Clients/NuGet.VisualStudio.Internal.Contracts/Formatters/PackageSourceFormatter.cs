// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MessagePack;
using MessagePack.Formatters;
using NuGet.Configuration;

namespace NuGet.VisualStudio.Internal.Contracts
{
    internal sealed class PackageSourceFormatter : NuGetMessagePackFormatter<PackageSource>
    {
        private const string NamePropertyName = "name";
        private const string SourcePropertyName = "source";
        private const string IsEnabledPropertyName = "isenabled";
        private const string IsMachineWidePropertyName = "ismachinewide";

        internal static readonly IMessagePackFormatter<PackageSource?> Instance = new PackageSourceFormatter();

        private PackageSourceFormatter()
        {
        }

        protected override PackageSource? DeserializeCore(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            string? name = null;
            string? source = null;
            bool isEnabled = false;
            bool isMachineWide = false;

            int propertyCount = reader.ReadMapHeader();
            for (int propertyIndex = 0; propertyIndex < propertyCount; propertyIndex++)
            {
                switch (reader.ReadString())
                {
                    case NamePropertyName:
                        name = reader.ReadString();
                        break;
                    case SourcePropertyName:
                        source = reader.ReadString();
                        break;
                    case IsEnabledPropertyName:
                        isEnabled = reader.ReadBoolean();
                        break;
                    case IsMachineWidePropertyName:
                        isMachineWide = reader.ReadBoolean();
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            return new PackageSource(source!, name!, isEnabled)
            {
                IsMachineWide = isMachineWide
            };
        }

        protected override void SerializeCore(ref MessagePackWriter writer, PackageSource value, MessagePackSerializerOptions options)
        {
            writer.WriteMapHeader(count: 4);
            writer.Write(NamePropertyName);
            writer.Write(value.Name);
            writer.Write(SourcePropertyName);
            writer.Write(value.Source);
            writer.Write(IsEnabledPropertyName);
            writer.Write(value.IsEnabled);
            writer.Write(IsMachineWidePropertyName);
            writer.Write(value.IsMachineWide);
        }
    }
}
