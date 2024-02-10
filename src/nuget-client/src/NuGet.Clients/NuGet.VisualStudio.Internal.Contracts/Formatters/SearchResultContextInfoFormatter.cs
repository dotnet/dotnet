// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using MessagePack;
using MessagePack.Formatters;
using Microsoft;

namespace NuGet.VisualStudio.Internal.Contracts
{
    internal sealed class SearchResultContextInfoFormatter : NuGetMessagePackFormatter<SearchResultContextInfo>
    {
        private const string HasMoreItemsPropertyName = "hasmoreitems";
        private const string SourceLoadingStatusPropertyName = "sourceloadingstatus";
        private const string PackageSearchItemsPropertyName = "packagesearchitems";
        private const string OperationIdPropertyName = "operationguid";

        internal static readonly IMessagePackFormatter<SearchResultContextInfo?> Instance = new SearchResultContextInfoFormatter();

        private SearchResultContextInfoFormatter()
        {
        }

        protected override SearchResultContextInfo? DeserializeCore(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            Guid? operationId = null;
            bool hasMoreItems = false;
            IReadOnlyCollection<PackageSearchMetadataContextInfo>? packageSearchItems = null;
            IReadOnlyDictionary<string, LoadingStatus>? sourceLoadingStatus = null;

            int propertyCount = reader.ReadMapHeader();
            for (int propertyIndex = 0; propertyIndex < propertyCount; propertyIndex++)
            {
                switch (reader.ReadString())
                {
                    case HasMoreItemsPropertyName:
                        hasMoreItems = reader.ReadBoolean();
                        break;
                    case PackageSearchItemsPropertyName:
                        packageSearchItems = options.Resolver.GetFormatter<IReadOnlyCollection<PackageSearchMetadataContextInfo>>().Deserialize(ref reader, options);
                        break;
                    case SourceLoadingStatusPropertyName:
                        sourceLoadingStatus = options.Resolver.GetFormatter<IReadOnlyDictionary<string, LoadingStatus>>().Deserialize(ref reader, options);
                        break;
                    case OperationIdPropertyName:
                        if (!reader.TryReadNil())
                        {
                            string? guidString = reader.ReadString();
                            if (Guid.TryParse(guidString, out Guid operationIdGuid))
                            {
                                operationId = operationIdGuid;
                            }
                        }
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            Assumes.NotNull(packageSearchItems);
            Assumes.NotNull(sourceLoadingStatus);

            return new SearchResultContextInfo(packageSearchItems, sourceLoadingStatus, hasMoreItems, operationId);
        }

        protected override void SerializeCore(ref MessagePackWriter writer, SearchResultContextInfo value, MessagePackSerializerOptions options)
        {
            writer.WriteMapHeader(count: 4);
            writer.Write(HasMoreItemsPropertyName);
            writer.Write(value.HasMoreItems);
            writer.Write(SourceLoadingStatusPropertyName);
            options.Resolver.GetFormatter<IReadOnlyDictionary<string, LoadingStatus>>().Serialize(ref writer, value.SourceLoadingStatus, options);
            writer.Write(OperationIdPropertyName);
            if (value.OperationId.HasValue)
            {
                writer.Write(value.OperationId.Value.ToString());
            }
            else
            {
                writer.WriteNil();
            }

            writer.Write(PackageSearchItemsPropertyName);
            options.Resolver.GetFormatter<IReadOnlyCollection<PackageSearchMetadataContextInfo>>().Serialize(ref writer, value.PackageSearchItems, options);
        }
    }
}
