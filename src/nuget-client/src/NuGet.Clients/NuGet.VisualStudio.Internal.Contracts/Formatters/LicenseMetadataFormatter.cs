// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using MessagePack;
using MessagePack.Formatters;
using NuGet.Packaging;
using NuGet.Packaging.Licenses;

namespace NuGet.VisualStudio.Internal.Contracts
{
    internal sealed class LicenseMetadataFormatter : NuGetMessagePackFormatter<LicenseMetadata>
    {
        private const string TypePropertyName = "type";
        private const string LicensePropertyName = "license";
        private const string LicenseExpressionPropertyName = "licenseexpression";
        private const string WarningsAndErrorsPropertyName = "warningsanderrors";
        private const string VersionPropertyName = "version";

        internal static readonly IMessagePackFormatter<LicenseMetadata?> Instance = new LicenseMetadataFormatter();

        private LicenseMetadataFormatter()
        {
        }

        protected override LicenseMetadata? DeserializeCore(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            LicenseType type = LicenseType.File;
            string? license = null;
            NuGetLicenseExpression? licenseExpression = null;
            IReadOnlyList<string>? warningsAndErrors = null;
            Version? version = null;

            int propertyCount = reader.ReadMapHeader();
            for (int propertyIndex = 0; propertyIndex < propertyCount; propertyIndex++)
            {
                switch (reader.ReadString())
                {
                    case TypePropertyName:
                        if (!reader.TryReadNil())
                        {
                            type = options.Resolver.GetFormatter<LicenseType>()!.Deserialize(ref reader, options);
                        }
                        break;
                    case LicensePropertyName:
                        license = reader.ReadString();
                        break;
                    case LicenseExpressionPropertyName:
                        var expressionString = reader.ReadString();
                        if (expressionString != null)
                        {
                            licenseExpression = NuGetLicenseExpression.Parse(expressionString);
                        }
                        break;
                    case WarningsAndErrorsPropertyName:
                        if (!reader.TryReadNil())
                        {
                            warningsAndErrors = options.Resolver.GetFormatter<IReadOnlyList<string>>()!.Deserialize(ref reader, options);
                        }
                        break;
                    case VersionPropertyName:
                        version = options.Resolver.GetFormatter<Version>()!.Deserialize(ref reader, options);
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            return new LicenseMetadata(type,
                license,
                licenseExpression,
                warningsAndErrors,
                version);
        }

        protected override void SerializeCore(ref MessagePackWriter writer, LicenseMetadata value, MessagePackSerializerOptions options)
        {
            writer.WriteMapHeader(count: 5);
            writer.Write(TypePropertyName);
            options.Resolver.GetFormatter<LicenseType>()!.Serialize(ref writer, value.Type, options);

            writer.Write(LicensePropertyName);
            writer.Write(value.License);

            writer.Write(LicenseExpressionPropertyName);
            writer.Write(value.LicenseExpression?.ToString());

            writer.Write(WarningsAndErrorsPropertyName);
            if (value.WarningsAndErrors == null)
            {
                writer.WriteNil();
            }
            else
            {
                options.Resolver.GetFormatter<IReadOnlyList<string>>()!.Serialize(ref writer, value.WarningsAndErrors, options);
            }

            writer.Write(VersionPropertyName);
            options.Resolver.GetFormatter<Version>()!.Serialize(ref writer, value.Version, options);
        }
    }
}
