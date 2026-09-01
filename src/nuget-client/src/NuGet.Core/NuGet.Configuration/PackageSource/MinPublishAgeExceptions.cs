// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.Configuration
{
    /// <summary>
    /// Matches package IDs against package-specific minimum publish age exception patterns.
    /// </summary>
    public sealed class MinPublishAgeExceptions
    {
        private readonly SearchTree _searchTree;
        private readonly IReadOnlyDictionary<string, MinPublishAgeExceptionItem> _itemsByPattern;

        /// <summary>
        /// Gets a value indicating whether at least one exception pattern is configured.
        /// </summary>
        public bool IsEnabled { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MinPublishAgeExceptions"/> class.
        /// </summary>
        /// <param name="items">The exception items to use for matching.</param>
        public MinPublishAgeExceptions(IReadOnlyList<MinPublishAgeExceptionItem> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            IsEnabled = items.Count > 0;
            var itemsByPattern = new Dictionary<string, MinPublishAgeExceptionItem>(items.Count, StringComparer.OrdinalIgnoreCase);
            var patterns = new string[items.Count];
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (item == null)
                {
                    throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(items));
                }

                string pattern = item.Pattern;
                itemsByPattern[pattern.Trim()] = item;
                patterns[i] = pattern;
            }

            var packageSourceMapping = new PackageSourceMapping(new Dictionary<string, IReadOnlyList<string>>
            {
                ["minPublishAgeExceptions"] = patterns
            });

            _itemsByPattern = itemsByPattern;
            _searchTree = new SearchTree(packageSourceMapping);
        }

        /// <summary>
        /// Finds the exception that matches a package ID.
        /// </summary>
        /// <param name="packageId">The package ID to match.</param>
        /// <returns>The matching exception, or <see langword="null"/> if no exception matches.</returns>
        public MinPublishAgeExceptionItem? FindException(string packageId)
        {
            string? matchingPattern = _searchTree.SearchForPattern(packageId);
            return matchingPattern != null && _itemsByPattern.TryGetValue(matchingPattern, out var item)
                ? item
                : null;
        }
    }
}
