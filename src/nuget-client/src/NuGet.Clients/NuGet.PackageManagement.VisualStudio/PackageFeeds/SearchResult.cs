// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// Generic search result as returned by a feed including actual items and current search
    /// state including but not limited to continuation token and refresh token.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class SearchResult<T> : IEnumerable<T>
    {
        public IReadOnlyList<T> Items { get; set; }

        public ContinuationToken NextToken { get; set; }

        public RefreshToken RefreshToken { get; set; }

        public Guid? OperationId { get; set; }

        public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();

        public IDictionary<string, LoadingStatus> SourceSearchStatus { get; set; } = new Dictionary<string, LoadingStatus>();

        public IDictionary<string, Exception> SourceSearchException { get; set; } = new Dictionary<string, Exception>();

        // total number of unmerged items found
        public int RawItemsCount { get; set; }

        public TimeSpan Duration { get; set; }
    }

    /// <summary>
    /// Helper class providing shortcuts to create new result instance
    /// </summary>
    public static class SearchResult
    {
        public static SearchResult<T> FromItems<T>(params T[] items) => new SearchResult<T>
        {
            Items = items,
            RawItemsCount = items.Length
        };

        public static SearchResult<T> FromItems<T>(IReadOnlyList<T> items) => new SearchResult<T>
        {
            Items = items,
            RawItemsCount = items.Count
        };

        public static SearchResult<T> Empty<T>() => new SearchResult<T>
        {
            Items = Array.Empty<T>()
        };
    }
}
