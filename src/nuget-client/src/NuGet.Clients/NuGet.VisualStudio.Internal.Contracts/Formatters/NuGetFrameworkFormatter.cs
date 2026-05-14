// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MessagePack;
using MessagePack.Formatters;
using NuGet.Frameworks;

namespace NuGet.VisualStudio.Internal.Contracts
{
    internal sealed class NuGetFrameworkFormatter : NuGetMessagePackFormatter<NuGetFramework>
    {
        private const string DotNetFrameworkNamePropertyName = "dotnetframeworkname";
        private const string DotNetPlatformNamePropertyName = "dotnetplatformname";

        internal static readonly IMessagePackFormatter<NuGetFramework?> Instance = new NuGetFrameworkFormatter();

        private NuGetFrameworkFormatter()
        {
        }

        protected override NuGetFramework? DeserializeCore(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            string? frameworkName = null;
            string? platformName = null;

            int propertyCount = reader.ReadMapHeader();

            for (int propertyIndex = 0; propertyIndex < propertyCount; propertyIndex++)
            {
                switch (reader.ReadString())
                {
                    case DotNetFrameworkNamePropertyName:
                        frameworkName = reader.ReadString();
                        break;

                    case DotNetPlatformNamePropertyName:
                        platformName = reader.ReadString();
                        break;

                    default:
                        reader.Skip();
                        break;
                }
            }

            return NuGetFramework.ParseComponents(frameworkName!, platformName);
        }

        protected override void SerializeCore(ref MessagePackWriter writer, NuGetFramework value, MessagePackSerializerOptions options)
        {
            writer.WriteMapHeader(count: 2);
            writer.Write(DotNetFrameworkNamePropertyName);
            writer.Write(value.DotNetFrameworkName);
            writer.Write(DotNetPlatformNamePropertyName);
            writer.Write(value.DotNetPlatformName);
        }
    }
}
