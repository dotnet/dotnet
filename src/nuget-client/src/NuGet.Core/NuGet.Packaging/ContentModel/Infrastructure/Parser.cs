// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace NuGet.ContentModel.Infrastructure
{
    public class PatternExpression
    {
        private readonly List<Segment> _segments = new List<Segment>();
        private readonly Dictionary<string, object> _defaults;
        private readonly PatternTable _table;

        public PatternExpression(PatternDefinition pattern)
        {
            _table = pattern.Table;
            _defaults = pattern.Defaults.ToDictionary(p => p.Key, p => p.Value);
            Initialize(pattern.Pattern);
        }

        private void Initialize(string pattern)
        {
            for (var scanIndex = 0; scanIndex < pattern.Length;)
            {
                var beginToken = pattern.Length;
                var endToken = pattern.Length;
                for (var i = scanIndex; i < pattern.Length; i++)
                {
                    var ch = pattern[i];
                    if (beginToken == pattern.Length)
                    {
                        if (ch == '{')
                        {
                            beginToken = i;
                        }
                    }
                    else if (ch == '}')
                    {
                        endToken = i;
                        break;
                    }
                }

                if (scanIndex != beginToken)
                {
                    _segments.Add(new LiteralSegment(pattern, scanIndex, beginToken - scanIndex));
                }
                if (beginToken != endToken)
                {
                    var delimiter = endToken + 1 < pattern.Length ? pattern[endToken + 1] : '\0';
                    var matchOnly = pattern[endToken - 1] == '?';

                    var beginName = beginToken + 1;
                    var endName = endToken - (matchOnly ? 1 : 0);

                    var tokenName = pattern.Substring(beginName, endName - beginName);
                    _segments.Add(new TokenSegment(tokenName, delimiter, matchOnly, _table));
                }
                scanIndex = endToken + 1;
            }
        }

        public ContentItem Match(string path, IReadOnlyDictionary<string, ContentPropertyDefinition> propertyDefinitions)
        {
            ContentItem item = null;
            var startIndex = 0;
            foreach (var segment in _segments)
            {
                int endIndex;
                if (segment.TryMatch(ref item, path, propertyDefinitions, startIndex, out endIndex))
                {
                    startIndex = endIndex;
                    continue;
                }
                return null;
            }

            if (startIndex == path.Length)
            {
                // Successful match!
                // Apply defaults from the pattern
                if (item == null)
                {
                    // item not created, use shared defaults
                    item = new ContentItem
                    {
                        Path = path,
                        Properties = _defaults
                    };
                }
                else
                {
                    // item already created, append defaults
                    foreach (var pair in _defaults)
                    {
                        item.Properties[pair.Key] = pair.Value;
                    }
                }
                return item;
            }
            return null;
        }

        private abstract class Segment
        {
            internal abstract bool TryMatch(ref ContentItem item, string path, IReadOnlyDictionary<string, ContentPropertyDefinition> propertyDefinitions, int startIndex, out int endIndex);
        }

        [DebuggerDisplay("{_pattern.Substring(_start, _length)}")]
        private class LiteralSegment : Segment
        {
            private readonly string _pattern;
            private readonly int _start;
            private readonly int _length;

            public LiteralSegment(string pattern, int start, int length)
            {
                _pattern = pattern;
                _start = start;
                _length = length;
            }

            internal override bool TryMatch(
                ref ContentItem item,
                string path,
                IReadOnlyDictionary<string, ContentPropertyDefinition> propertyDefinitions,
                int startIndex,
                out int endIndex)
            {
                if (path.Length >= startIndex + _length)
                {
                    if (string.Compare(path, startIndex, _pattern, _start, _length, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        endIndex = startIndex + _length;
                        return true;
                    }
                }
                endIndex = startIndex;
                return false;
            }
        }

        [DebuggerDisplay("Token = {_token}, Delimiter = {_delimiter}, MatchOnly = {_matchOnly}")]
        private class TokenSegment : Segment
        {
            private readonly string _token;
            private readonly char _delimiter;
            private readonly bool _matchOnly;
            private readonly PatternTable _table;

            public TokenSegment(string token, char delimiter, bool matchOnly, PatternTable table)
            {
                _token = token;
                _delimiter = delimiter;
                _matchOnly = matchOnly;
                _table = table;
            }

            internal override bool TryMatch(
                ref ContentItem item,
                string path,
                IReadOnlyDictionary<string, ContentPropertyDefinition> propertyDefinitions,
                int startIndex,
                out int endIndex)
            {
                ContentPropertyDefinition propertyDefinition;
                if (!propertyDefinitions.TryGetValue(_token, out propertyDefinition))
                {
                    throw new Exception(string.Format(CultureInfo.CurrentCulture, "Unable to find property definition for {{{0}}}", _token));
                }

                for (var scanIndex = startIndex; scanIndex != path.Length;)
                {
                    var delimiterIndex = path.Length;
                    for (var i = scanIndex + 1; i < path.Length; i++)
                    {
                        if (path[i] == _delimiter)
                        {
                            delimiterIndex = i;
                            break;
                        }
                    }

                    if (delimiterIndex == path.Length
                        && _delimiter != '\0')
                    {
                        break;
                    }
                    var substring = path.Substring(startIndex, delimiterIndex - startIndex);
                    object value;
                    if (propertyDefinition.TryLookup(substring, _table, out value))
                    {
                        if (!_matchOnly)
                        {
                            // Adding property, create item if not already created
                            if (item == null)
                            {
                                item = new ContentItem
                                {
                                    Path = path
                                };
                            }
                            if (StringComparer.Ordinal.Equals(_token, "tfm"))
                            {
                                item.Properties.Add("tfm_raw", substring);
                            }
                            item.Properties.Add(_token, value);
                        }
                        endIndex = delimiterIndex;
                        return true;
                    }
                    scanIndex = delimiterIndex;
                }
                endIndex = startIndex;
                return false;
            }
        }
    }
}
