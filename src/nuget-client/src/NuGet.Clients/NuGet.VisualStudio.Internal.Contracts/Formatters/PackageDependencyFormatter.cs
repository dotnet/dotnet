// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using MessagePack;
using MessagePack.Formatters;
using Microsoft;
using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace NuGet.VisualStudio.Internal.Contracts
{
    internal sealed class PackageDependencyFormatter : NuGetMessagePackFormatter<PackageDependency>
    {
        private const string ExcludePropertyName = "exclude";
        private const string IdPropertyName = "id";
        private const string IncludePropertyName = "include";
        private const string VersionRangePropertyName = "versionrange";

        internal static readonly IMessagePackFormatter<PackageDependency?> Instance = new PackageDependencyFormatter();

        private PackageDependencyFormatter()
        {
        }

        protected override PackageDependency? DeserializeCore(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            string? id = null;
            VersionRange? versionRange = null;
            IReadOnlyList<string>? include = null;
            IReadOnlyList<string>? exclude = null;

            int propertyCount = reader.ReadMapHeader();

            for (var propertyIndex = 0; propertyIndex < propertyCount; ++propertyIndex)
            {
                switch (reader.ReadString())
                {
                    case ExcludePropertyName:
                        exclude = options.Resolver.GetFormatter<IReadOnlyList<string>>().Deserialize(ref reader, options);
                        break;

                    case IdPropertyName:
                        id = reader.ReadString();
                        break;

                    case IncludePropertyName:
                        include = options.Resolver.GetFormatter<IReadOnlyList<string>>().Deserialize(ref reader, options);
                        break;

                    case VersionRangePropertyName:
                        versionRange = VersionRangeFormatter.Instance.Deserialize(ref reader, options);
                        break;

                    default:
                        reader.Skip();
                        break;
                }
            }

            Assumes.NotNullOrEmpty(id);

            return new PackageDependency(id, versionRange, include, exclude);
        }

        protected override void SerializeCore(ref MessagePackWriter writer, PackageDependency value, MessagePackSerializerOptions options)
        {
            writer.WriteMapHeader(count: 4);
            writer.Write(IdPropertyName);
            writer.Write(value.Id);
            writer.Write(VersionRangePropertyName);
            VersionRangeFormatter.Instance.Serialize(ref writer, value.VersionRange, options);
            writer.Write(IncludePropertyName);
            options.Resolver.GetFormatter<IReadOnlyList<string>>().Serialize(ref writer, value.Include, options);
            writer.Write(ExcludePropertyName);
            options.Resolver.GetFormatter<IReadOnlyList<string>>().Serialize(ref writer, value.Exclude, options);
        }
    }
}
