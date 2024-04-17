// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using MessagePack;
using MessagePack.Formatters;
using NuGet.Protocol.Core.Types;

namespace NuGet.VisualStudio.Internal.Contracts
{
    internal sealed class SearchFilterFormatter : NuGetMessagePackFormatter<SearchFilter>
    {
        private const string IncludePrereleasePropertyName = "includeprerelease";
        private const string IncludeDelistedPropertyName = "includedelisted";
        private const string PackageTypesPropertyName = "packagetypes";
        private const string FilterPropertyName = "filter";
        private const string OrderByPropertyName = "orderby";
        private const string SupportedFrameworksPropertyName = "supportedframeworks";

        internal static readonly IMessagePackFormatter<SearchFilter?> Instance = new SearchFilterFormatter();

        private SearchFilterFormatter()
        {
        }

        protected override SearchFilter? DeserializeCore(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            bool includePrerelease = false;
            bool includeDelisted = false;
            SearchFilterType? filterType = null;
            SearchOrderBy? searchOrderBy = null;
            IEnumerable<string>? supportedFrameworks = null;
            IEnumerable<string>? packageTypes = null;

            int propertyCount = reader.ReadMapHeader();
            for (int propertyIndex = 0; propertyIndex < propertyCount; propertyIndex++)
            {
                switch (reader.ReadString())
                {
                    case IncludePrereleasePropertyName:
                        includePrerelease = reader.ReadBoolean();
                        break;
                    case IncludeDelistedPropertyName:
                        includeDelisted = reader.ReadBoolean();
                        break;
                    case PackageTypesPropertyName:
                        packageTypes = options.Resolver.GetFormatter<IEnumerable<string>>()!.Deserialize(ref reader, options);
                        break;
                    case FilterPropertyName:
                        filterType = options.Resolver.GetFormatter<SearchFilterType?>()!.Deserialize(ref reader, options);
                        break;
                    case OrderByPropertyName:
                        searchOrderBy = options.Resolver.GetFormatter<SearchOrderBy?>()!.Deserialize(ref reader, options);
                        break;
                    case SupportedFrameworksPropertyName:
                        supportedFrameworks = options.Resolver.GetFormatter<IEnumerable<string>>()!.Deserialize(ref reader, options);
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            return new SearchFilter(includePrerelease, filterType)
            {
                SupportedFrameworks = supportedFrameworks,
                OrderBy = searchOrderBy,
                PackageTypes = packageTypes,
                IncludeDelisted = includeDelisted,
            };
        }

        protected override void SerializeCore(ref MessagePackWriter writer, SearchFilter value, MessagePackSerializerOptions options)
        {
            writer.WriteMapHeader(count: 6);
            writer.Write(IncludePrereleasePropertyName);
            writer.Write(value.IncludePrerelease);
            writer.Write(IncludeDelistedPropertyName);
            writer.Write(value.IncludeDelisted);
            writer.Write(PackageTypesPropertyName);
            options.Resolver.GetFormatter<IEnumerable<string>>()!.Serialize(ref writer, value.PackageTypes, options);
            writer.Write(FilterPropertyName);
            options.Resolver.GetFormatter<SearchFilterType?>()!.Serialize(ref writer, value.Filter, options);
            writer.Write(OrderByPropertyName);
            options.Resolver.GetFormatter<SearchOrderBy?>()!.Serialize(ref writer, value.OrderBy, options);
            writer.Write(SupportedFrameworksPropertyName);
            options.Resolver.GetFormatter<IEnumerable<string>>()!.Serialize(ref writer, value.SupportedFrameworks, options);
        }
    }
}
