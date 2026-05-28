// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.DotNet.SourceBuild.Tasks
{
    internal class TargetFrameworkRegexFilter
    {
        private readonly Regex? _includeTargetFrameworks;
        private readonly Regex? _excludeTargetFrameworks;
        private readonly HashSet<string> _foundExcludedTargetFrameworks = new();

        /// <summary>
        /// The list of found excluded target frameworks.
        /// </summary>
        public IReadOnlySet<string> FoundExcludedTargetFrameworks => _foundExcludedTargetFrameworks;

        public TargetFrameworkRegexFilter(IEnumerable<string>? includeTargetFrameworks,
            IEnumerable<string>? excludeTargetFrameworks)
        {
            _includeTargetFrameworks = TransformPatternsToRegexList(includeTargetFrameworks);
            _excludeTargetFrameworks = TransformPatternsToRegexList(excludeTargetFrameworks);
        }

        /// <summary>
        /// Skip target frameworks that aren't included in the IncludeTargetFrameworks filter and that
        /// are excluded in the ExcludeTargetFrameworks filter.
        /// </summary>
        public bool IsIncludedAndNotExcluded(string? targetFramework)
        {
            // Skip empty target frameworks.
            if (string.IsNullOrWhiteSpace(targetFramework))
                return false;

            // Skip target frameworks that aren't included in the IncludeTargetFrameworks filter.
            if (_includeTargetFrameworks is not null && !_includeTargetFrameworks.IsMatch(targetFramework))
                return false;

            // Skip target frameworks that are excluded.
            if (_excludeTargetFrameworks is not null && _excludeTargetFrameworks.IsMatch(targetFramework))
            {
                _foundExcludedTargetFrameworks.Add(targetFramework);
                return false;
            }

            return true;
        }

        private static Regex? TransformPatternsToRegexList(IEnumerable<string>? patterns)
        {
            if (patterns is null)
                return null;

            string[] patternArray = patterns
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .ToArray();

            if (patternArray.Length == 0)
                return null;

            string pattern = patternArray
                .Select(p => Regex.Escape(p).Replace("\\*", ".*"))
                .Aggregate((p1, p2) => p1 + "|" + p2);
            pattern = $"^(?:{pattern})$";

            return new Regex(pattern, RegexOptions.NonBacktracking | RegexOptions.Compiled);
        }
    }
}
