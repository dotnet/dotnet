// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// A data structure providing efficient lookup for known directory containing a file with any nesting level.
    /// Inspired by prefix string search trie.
    /// </summary>
    /// <typeparam name="TValue">A data type stored at leaf nodes. For instance, package ID, GUID, etc.</typeparam>
    public sealed class PathLookupTrie<TValue>
    {
        private static readonly char[] PathSeparators = new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };

        private readonly PathSegmentTrieNode _root = new PathSegmentTrieNode(string.Empty);

        /// <summary>
        /// Indexer setting/retrieving custom data from a leaf node.
        /// </summary>
        /// <param name="path">Directory or file path</param>
        /// <returns>Value associated with leaf node if found.</returns>
        public TValue this[string path]
        {
            get
            {
                if (string.IsNullOrEmpty(path))
                {
                    throw new ArgumentNullException(nameof(path));
                }

                var segments = SplitPath(path);

                var current = _root;
                foreach (var segment in segments)
                {
                    current = current.FindChildNode(segment);
                    if (current == null)
                    {
                        throw new KeyNotFoundException();
                    }

                    if (current.IsTerminal)
                    {
                        return current.Value;
                    }
                }

                throw new KeyNotFoundException();
            }

            set
            {
                if (string.IsNullOrEmpty(path))
                {
                    throw new ArgumentNullException(nameof(path));
                }

                var segments = SplitPath(path);

                var current = _root;
                foreach (var segment in segments)
                {
                    current = current.GetOrCreateChildNode(segment);
                }

                current.Value = value;
            }
        }

        private static IEnumerable<string> SplitPath(string path)
        {
            return path.Split(PathSeparators, StringSplitOptions.RemoveEmptyEntries);
        }

        private class PathSegmentTrieNode
        {
            private readonly string _pathSegment;
            private readonly LinkedList<PathSegmentTrieNode> _children;
            private TValue _value;

            public TValue Value { get { return _value; } set { IsTerminal = true; _value = value; } }

            public bool IsTerminal { get; private set; }

            public PathSegmentTrieNode(string pathSegment)
            {
                _children = new LinkedList<PathSegmentTrieNode>();
                _pathSegment = pathSegment;
            }

            public PathSegmentTrieNode GetOrCreateChildNode(string pathSegment)
            {
                var found = FindChildNode(pathSegment);
                if (found == null)
                {
                    found = new PathSegmentTrieNode(pathSegment);
                    _children.AddLast(found);
                }

                return found;
            }

            public PathSegmentTrieNode FindChildNode(string pathSegment)
            {
                return _children
                    .FirstOrDefault(n => StringComparer.OrdinalIgnoreCase.Equals(n._pathSegment, pathSegment));
            }
        }
    }
}
