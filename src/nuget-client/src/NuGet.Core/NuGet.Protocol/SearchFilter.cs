// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NuGet.Protocol.Core.Types
{
    /// <summary>
    /// A search filter context that represents the UI settings
    /// </summary>
    public class SearchFilter
    {
        /// <summary>
        /// Initializes an instance of a <see cref="SearchFilter"/> and validates required parameters.
        /// </summary>
        /// <param name="includePrerelease">Whether or not to allow prerelease results.</param>
        public SearchFilter(bool includePrerelease) : this(
            includePrerelease,
            includePrerelease ? SearchFilterType.IsAbsoluteLatestVersion : SearchFilterType.IsLatestVersion)
        {
        }

        /// <summary>
        /// Initializes an instance of a <see cref="SearchFilter"/> and validates required parameters.
        /// </summary>
        /// <param name="includePrerelease">Whether or not to allow prerelease results.</param>
        /// <param name="filter">The filter to apply to the results.</param>
        public SearchFilter(bool includePrerelease, SearchFilterType? filter)
        {
            Debug.Assert(
                (includePrerelease && filter == SearchFilterType.IsAbsoluteLatestVersion) ||
                (!includePrerelease && filter == SearchFilterType.IsLatestVersion) ||
                (filter == null),
                "Invalid combination of prerelease and filter parameters.");

            IncludePrerelease = includePrerelease;
            Filter = filter;
        }

        /// <summary>
        /// Filter to only the list of packages compatible with these frameworks.
        /// </summary>
        public IEnumerable<string> SupportedFrameworks { get; set; } = Enumerable.Empty<string>();

        /// <summary>
        /// Include prerelease packages in search
        /// </summary>
        public bool IncludePrerelease { get; } = false;

        /// <summary>
        /// Include unlisted packages in search
        /// </summary>
        public bool IncludeDelisted { get; set; } = false;

        /// <summary>
        /// Restrict the search to certain package types.
        /// </summary>
        public IEnumerable<string> PackageTypes { get; set; } = Enumerable.Empty<string>();

        /// <summary>
        /// The optional filter type. Absense of this value indicates that all versions should be returned.
        /// </summary>
        public SearchFilterType? Filter { get; } = null;

        /// <summary>
        /// The optional order by. Absense of this value indicates that search results should be ordered by relevance.
        /// </summary>
        public SearchOrderBy? OrderBy { get; set; }
    }
}
