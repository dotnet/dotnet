// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft;

namespace NuGet.VisualStudio.Internal.Contracts
{
    public sealed class SearchResultContextInfo
    {
        public SearchResultContextInfo(
            IReadOnlyCollection<PackageSearchMetadataContextInfo> packageSearchItems,
            IReadOnlyDictionary<string, LoadingStatus> sourceLoadingStatus,
            bool hasMoreItems)
            : this(packageSearchItems, sourceLoadingStatus, hasMoreItems, operationId: null)
        {
        }

        public SearchResultContextInfo(
            IReadOnlyCollection<PackageSearchMetadataContextInfo> packageSearchItems,
            IReadOnlyDictionary<string, LoadingStatus> sourceLoadingStatus,
            bool hasMoreItems,
            Guid? operationId)
        {
            Assumes.NotNull(packageSearchItems);
            Assumes.NotNull(sourceLoadingStatus);

            PackageSearchItems = packageSearchItems;
            SourceLoadingStatus = sourceLoadingStatus;
            HasMoreItems = hasMoreItems;
            OperationId = operationId;
        }

        public SearchResultContextInfo(Guid? operationId)
            : this(Array.Empty<PackageSearchMetadataContextInfo>(),
                ImmutableDictionary<string, LoadingStatus>.Empty,
                hasMoreItems: false,
                operationId: operationId)
        {
        }

        public SearchResultContextInfo()
            : this(operationId: null)
        {
        }

        public Guid? OperationId { get; }
        public bool HasMoreItems { get; }
        public IReadOnlyCollection<PackageSearchMetadataContextInfo> PackageSearchItems { get; }
        public IReadOnlyDictionary<string, LoadingStatus> SourceLoadingStatus { get; }
    }
}
