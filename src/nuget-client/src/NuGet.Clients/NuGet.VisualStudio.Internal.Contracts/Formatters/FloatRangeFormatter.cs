// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MessagePack;
using MessagePack.Formatters;
using Microsoft;
using NuGet.Versioning;

namespace NuGet.VisualStudio.Internal.Contracts
{
    internal sealed class FloatRangeFormatter : NuGetMessagePackFormatter<FloatRange>
    {
        private const string FloatBehaviorPropertyName = "floatbehavior";
        private const string MinVersionPropertyName = "minversion";
        private const string ReleasePrefixPropertyName = "releaseprefix";

        internal static readonly IMessagePackFormatter<FloatRange?> Instance = new FloatRangeFormatter();

        private FloatRangeFormatter()
        {
        }

        protected override FloatRange? DeserializeCore(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            NuGetVersionFloatBehavior? floatBehavior = null;
            NuGetVersion? minVersion = null;
            string? releasePrefix = null;

            int propertyCount = reader.ReadMapHeader();

            for (var propertyIndex = 0; propertyIndex < propertyCount; ++propertyIndex)
            {
                switch (reader.ReadString())
                {
                    case FloatBehaviorPropertyName:
                        floatBehavior = options.Resolver.GetFormatter<NuGetVersionFloatBehavior>().Deserialize(ref reader, options);
                        break;

                    case MinVersionPropertyName:
                        minVersion = NuGetVersionFormatter.Instance.Deserialize(ref reader, options);
                        break;

                    case ReleasePrefixPropertyName:
                        releasePrefix = reader.ReadString();
                        break;

                    default:
                        reader.Skip();
                        break;
                }
            }

            Assumes.True(floatBehavior.HasValue);
            Assumes.NotNull(minVersion);

            return new FloatRange(floatBehavior.Value, minVersion!, releasePrefix);
        }

        protected override void SerializeCore(ref MessagePackWriter writer, FloatRange value, MessagePackSerializerOptions options)
        {
            writer.WriteMapHeader(count: 3);
            writer.Write(MinVersionPropertyName);
            NuGetVersionFormatter.Instance.Serialize(ref writer, value.MinVersion, options);
            writer.Write(FloatBehaviorPropertyName);
            options.Resolver.GetFormatter<NuGetVersionFloatBehavior>().Serialize(ref writer, value.FloatBehavior, options);
            writer.Write(ReleasePrefixPropertyName);
            writer.Write(value.OriginalReleasePrefix);
        }
    }
}
