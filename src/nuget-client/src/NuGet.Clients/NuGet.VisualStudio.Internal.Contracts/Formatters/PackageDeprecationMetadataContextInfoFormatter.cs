// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using MessagePack;
using MessagePack.Formatters;

namespace NuGet.VisualStudio.Internal.Contracts
{
    internal sealed class PackageDeprecationMetadataContextInfoFormatter : NuGetMessagePackFormatter<PackageDeprecationMetadataContextInfo>
    {
        private const string MessagePropertyName = "message";
        private const string ReasonsPropertyName = "reasons";
        private const string AlternatePackageMetadataPropertyName = "alternatepackage";

        internal static readonly IMessagePackFormatter<PackageDeprecationMetadataContextInfo?> Instance = new PackageDeprecationMetadataContextInfoFormatter();

        private PackageDeprecationMetadataContextInfoFormatter()
        {
        }

        protected override PackageDeprecationMetadataContextInfo? DeserializeCore(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            string? message = null;
            IReadOnlyCollection<string>? reasons = null;
            AlternatePackageMetadataContextInfo? alternatePackageMetadataContextInfo = null;

            int propertyCount = reader.ReadMapHeader();
            for (int propertyIndex = 0; propertyIndex < propertyCount; propertyIndex++)
            {
                switch (reader.ReadString())
                {
                    case MessagePropertyName:
                        message = reader.ReadString();
                        break;
                    case ReasonsPropertyName:
                        reasons = reader.TryReadNil() ? null : options.Resolver.GetFormatter<IReadOnlyCollection<string>>()!.Deserialize(ref reader, options);
                        break;
                    case AlternatePackageMetadataPropertyName:
                        alternatePackageMetadataContextInfo = reader.TryReadNil() ? null : AlternatePackageMetadataContextInfoFormatter.Instance.Deserialize(ref reader, options);
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            return new PackageDeprecationMetadataContextInfo(message, reasons, alternatePackageMetadataContextInfo);
        }

        protected override void SerializeCore(ref MessagePackWriter writer, PackageDeprecationMetadataContextInfo value, MessagePackSerializerOptions options)
        {
            writer.WriteMapHeader(count: 3);
            writer.Write(MessagePropertyName);
            writer.Write(value.Message);
            writer.Write(ReasonsPropertyName);
            if (value.Reasons == null)
            {
                writer.WriteNil();
            }
            else
            {
                options.Resolver.GetFormatter<IReadOnlyCollection<string>>()!.Serialize(ref writer, value.Reasons, options);
            }

            writer.Write(AlternatePackageMetadataPropertyName);
            if (value.AlternatePackage == null)
            {
                writer.WriteNil();
            }
            else
            {
                AlternatePackageMetadataContextInfoFormatter.Instance.Serialize(ref writer, value.AlternatePackage, options);
            }
        }
    }
}
