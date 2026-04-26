// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;

namespace Microsoft.DotNet.SourceBuild.Tasks
{
    internal class TargetFrameworkRegexFilter
    {
        private const char TargetFrameworkDelimiter = ';';
        private readonly Regex? _includeTargetFrameworks;
        private readonly Regex? _excludeTargetFrameworks;
        private readonly HashSet<string> _foundExcludedTargetFrameworks = new();

        /// <summary>
        /// The list of found excluded target frameworks.
        /// </summary>
        public IReadOnlySet<string> FoundExcludedTargetFrameworks => _foundExcludedTargetFrameworks;

        public TargetFrameworkRegexFilter(string? includeTargetFrameworks,
            string? excludeTargetFrameworks)
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

        private static Regex? TransformPatternsToRegexList(string? patterns)
        {
            if (string.IsNullOrWhiteSpace(patterns))
                return null;

            string pattern = patterns.Split(TargetFrameworkDelimiter, StringSplitOptions.RemoveEmptyEntries)
                .Select(p => Regex.Escape(p).Replace("\\*", ".*"))
                .Aggregate((p1, p2) => p1 + "|" + p2);
            pattern = $"^(?:{pattern})$";

            return new Regex(pattern, RegexOptions.NonBacktracking | RegexOptions.Compiled);
        }
    }
}
