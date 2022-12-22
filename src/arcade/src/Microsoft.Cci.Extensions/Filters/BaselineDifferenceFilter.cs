// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Cci.Differs;

namespace Microsoft.Cci.Filters
{
    public class BaselineDifferenceFilter : IDifferenceFilter
    {
        private readonly Dictionary<string, bool> _ignoreDifferences = new Dictionary<string, bool>();
        private readonly IDifferenceFilter _filter;
        private readonly bool _treatUnusedDifferencesAsIssues;

        public BaselineDifferenceFilter(IDifferenceFilter filter, bool treatUnusedDifferencesAsIssues)
        {
            _filter = filter;
            _treatUnusedDifferencesAsIssues = treatUnusedDifferencesAsIssues;
        }

        public void AddBaselineFile(string baselineFile)
        {
            if (!File.Exists(baselineFile))
                return;

            foreach (var line in File.ReadAllLines(baselineFile))
            {
                string filteredLine = line;
                int index = filteredLine.IndexOf('#');

                if (index >= 0)
                    filteredLine = filteredLine.Substring(0, index);

                filteredLine = filteredLine.Trim();
                if (string.IsNullOrWhiteSpace(filteredLine))
                    continue;

                if (filteredLine.StartsWith("Compat issues with assembly", StringComparison.OrdinalIgnoreCase) || 
                    filteredLine.StartsWith("Total Issues"))
                    continue;

                _ignoreDifferences[filteredLine] = false;
            }
        }

        public bool Include(Difference difference)
        {
            // Is the entire rule ignored?
            if (_ignoreDifferences.ContainsKey(difference.Id))
            {
                _ignoreDifferences[difference.Id] = true;
                return false;
            }

            // Is the specific violation of the rule ignored?
            var diff = difference.ToString();
            if (_ignoreDifferences.ContainsKey(diff))
            {
                _ignoreDifferences[diff] = true;
                return false;
            }

            return _filter.Include(difference);
        }

        public IEnumerable<string> GetUnusedBaselineDifferences()
        {
            if (!_treatUnusedDifferencesAsIssues)
                return Enumerable.Empty<string>();

            return _ignoreDifferences.Where(i => !i.Value).Select(i => i.Key);
        }
    }
}
