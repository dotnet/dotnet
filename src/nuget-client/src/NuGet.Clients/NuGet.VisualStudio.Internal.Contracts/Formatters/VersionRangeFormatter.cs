// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MessagePack;
using MessagePack.Formatters;
using Microsoft;
using NuGet.Versioning;

namespace NuGet.VisualStudio.Internal.Contracts
{
    internal sealed class VersionRangeFormatter : NuGetMessagePackFormatter<VersionRange>
    {
        private const string FloatRangePropertyName = "floatrange";
        private const string IsMaxInclusivePropertyName = "ismaxinclusive";
        private const string IsMinInclusivePropertyName = "ismininclusive";
        private const string MaxVersionPropertyName = "maxversion";
        private const string MinVersionPropertyName = "minversion";
        private const string OriginalStringPropertyName = "originalstring";

        internal static readonly IMessagePackFormatter<VersionRange?> Instance = new VersionRangeFormatter();

        private VersionRangeFormatter()
        {
        }

        protected override VersionRange? DeserializeCore(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            FloatRange? floatRange = null;
            bool? isMaxInclusive = null;
            bool? isMinInclusive = null;
            NuGetVersion? maxVersion = null;
            NuGetVersion? minVersion = null;
            string? originalString = null;

            int propertyCount = reader.ReadMapHeader();

            for (var propertyIndex = 0; propertyIndex < propertyCount; ++propertyIndex)
            {
                switch (reader.ReadString())
                {
                    case FloatRangePropertyName:
                        floatRange = FloatRangeFormatter.Instance.Deserialize(ref reader, options);
                        break;

                    case IsMaxInclusivePropertyName:
                        isMaxInclusive = reader.ReadBoolean();
                        break;

                    case IsMinInclusivePropertyName:
                        isMinInclusive = reader.ReadBoolean();
                        break;

                    case MaxVersionPropertyName:
                        maxVersion = NuGetVersionFormatter.Instance.Deserialize(ref reader, options);
                        break;

                    case MinVersionPropertyName:
                        minVersion = NuGetVersionFormatter.Instance.Deserialize(ref reader, options);
                        break;

                    case OriginalStringPropertyName:
                        originalString = reader.ReadString();
                        break;

                    default:
                        reader.Skip();
                        break;
                }
            }

            Assumes.True(isMinInclusive.HasValue);
            Assumes.True(isMaxInclusive.HasValue);

            return new VersionRange(
                minVersion,
                isMinInclusive.Value,
                maxVersion,
                isMaxInclusive.Value,
                floatRange,
                originalString);
        }

        protected override void SerializeCore(ref MessagePackWriter writer, VersionRange value, MessagePackSerializerOptions options)
        {
            writer.WriteMapHeader(count: 6);
            writer.Write(MinVersionPropertyName);
            NuGetVersionFormatter.Instance.Serialize(ref writer, value.MinVersion, options);
            writer.Write(IsMinInclusivePropertyName);
            writer.Write(value.IsMinInclusive);

            writer.Write(MaxVersionPropertyName);
            NuGetVersionFormatter.Instance.Serialize(ref writer, value.MaxVersion, options);
            writer.Write(IsMaxInclusivePropertyName);
            writer.Write(value.IsMaxInclusive);

            writer.Write(FloatRangePropertyName);
            FloatRangeFormatter.Instance.Serialize(ref writer, value.Float, options);
            writer.Write(OriginalStringPropertyName);
            writer.Write(value.OriginalString);
        }
    }
}
