// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.ContentModel.Infrastructure;

namespace NuGet.ContentModel
{
    /// <summary>
    /// A set of patterns that can be used to query a set of file paths for items matching a provided criteria.
    /// </summary>
    public class PatternSet
    {
        public PatternSet(IReadOnlyDictionary<string, ContentPropertyDefinition> properties, IEnumerable<PatternDefinition> groupPatterns, IEnumerable<PatternDefinition> pathPatterns)
        {
            if (properties == null) throw new ArgumentNullException(nameof(properties));
            if (groupPatterns == null) throw new ArgumentNullException(nameof(groupPatterns));
            if (pathPatterns == null) throw new ArgumentNullException(nameof(pathPatterns));

            var groupPatternsArray = groupPatterns as PatternDefinition[] ?? groupPatterns.ToArray();
            var pathPatternsArray = pathPatterns as PatternDefinition[] ?? pathPatterns.ToArray();

            GroupPatterns = groupPatternsArray;
            PathPatterns = pathPatternsArray;
            PropertyDefinitions = properties;
            GroupExpressions = CreatePatternExpressions(groupPatternsArray);
            PathExpressions = CreatePatternExpressions(pathPatternsArray);
        }

        private static PatternExpression[] CreatePatternExpressions(PatternDefinition[] patternDefinitions)
        {
            PatternExpression[] patternExpressions = new PatternExpression[patternDefinitions.Length];
            for (int i = 0; i < patternDefinitions.Length; i++)
            {
                patternExpressions[i] = new PatternExpression(patternDefinitions[i]);
            }

            return patternExpressions;
        }

        /// <summary>
        /// Patterns used to select a group of items that matches the criteria
        /// </summary>
        public IEnumerable<PatternDefinition> GroupPatterns { get; }

        /// <summary>
        /// Pattern expressions.
        /// </summary>
        internal PatternExpression[] GroupExpressions { get; }

        /// <summary>
        /// Patterns used to select individual items that match the criteria
        /// </summary>
        public IEnumerable<PatternDefinition> PathPatterns { get; }

        /// <summary>
        /// Path expressions.
        /// </summary>
        internal PatternExpression[] PathExpressions { get; }

        /// <summary>
        /// Property definitions used for matching patterns
        /// </summary>
        public IReadOnlyDictionary<string, ContentPropertyDefinition> PropertyDefinitions { get; set; }
    }

    /// <summary>
    /// A pattern that can be used to match file paths given a provided criteria.
    /// </summary>
    /// <remarks>
    /// The pattern is defined as a sequence of literal path strings that must match exactly and property
    /// references,
    /// wrapped in {} characters, which are tested for compatibility with the consumer-provided criteria.
    /// <seealso cref="ContentPropertyDefinition" />
    /// </remarks>
    public class PatternDefinition
    {
        public string Pattern { get; }
        public IReadOnlyDictionary<string, object> Defaults { get; }

        /// <summary>
        /// Replacement token table.
        /// </summary>
        public PatternTable? Table { get; }

        internal bool PreserveRawValues { get; init; }

        public PatternDefinition(string pattern)
            : this(pattern, table: null, defaults: Enumerable.Empty<KeyValuePair<string, object>>())
        {
        }

        public PatternDefinition(string pattern, PatternTable? table)
            : this(pattern, table, Enumerable.Empty<KeyValuePair<string, object>>())
        {
        }

        public PatternDefinition(
            string pattern,
            PatternTable? table,
            IEnumerable<KeyValuePair<string, object>> defaults)
        {
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));
            if (defaults == null) throw new ArgumentNullException(nameof(defaults));
            Pattern = pattern;
            Table = table;
            Defaults = defaults.ToDictionary(p => p.Key, p => p.Value);
        }

        public static implicit operator PatternDefinition(string pattern)
        {
            return new PatternDefinition(pattern);
        }
    }
}
