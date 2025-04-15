// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

namespace Microsoft.SignCheck.Verification
{
    /// <summary>
    /// Represents an exclusion describing
    /// </summary>
    public class Exclusion
    {
        private string[] _exclusionParts;
        private const int FilePatternsIndex = 0;
        private const int ParentFilesIndex = 1;
        private const int CommentIndex = 2;
        private Dictionary<string, HashSet<string>> _fileExcludedCache = new();
        private Dictionary<string, HashSet<string>> _fileNotExcludedCache = new();
        private Dictionary<string, HashSet<string>> _parentExcludedCache = new();
        private Dictionary<string, HashSet<string>> _parentNotExcludedCache = new();

        /// <summary>
        /// Creates a new <see cref="Exclusion"/>.
        /// </summary>
        /// <param name="exclusion">A string representation of a file exclusion. An exclusion contains a number of fields, separated by
        /// a ';'. The entry is formated as FILE_PATTERNS;PARENT_FILES;COMMENT. Additional fields are ignored and fields may be left
        /// empty, e.g. ";B.txt" indicates an exclusion with no file patterns and one parent file.
        ///
        /// The FILE_PATTERNS and PARENT_FILES fields may contain multiple values separated by a '|'.
        ///
        /// For example: "A.txt|C:\Dir1\B.txt;C.zip;" indicates an exclusion with two file patterns ("A.txt" and "C:\Dir1\B.txt") and one
        /// parent file ("C.zip").
        /// </param>
        public Exclusion(string exclusion)
        {
            if (!String.IsNullOrEmpty(exclusion))
            {
                _exclusionParts = exclusion.Split(';');
            }
        }

        public string Comment
        {
            get
            {
                return GetExclusionPart(CommentIndex, defaultValue: SignCheckResources.NA);
            }
        }

        /// <summary>
        /// Returns an array of file patterns or null if there are no entries. Each file pattern is separated by '|'.
        /// </summary>
        public string[] FilePatterns
        {
            get
            {
                return GetExclusionPart(FilePatternsIndex).Split('|');
            }
        }

        /// <summary>
        /// Returns an array of parent files or null if there are no entries. Each parent file is separated by '|'.
        /// </summary>
        public string[] ParentFiles
        {
            get
            {
                return GetExclusionPart(ParentFilesIndex)
                    .Split('|')
                    .Select(e => e.Trim())
                    .Select(e => !string.IsNullOrEmpty(e) ? $"{e}*" : e)
                    .ToArray();
            }
        }

        /// <summary>
        /// Returns true if the exclusion contains file patterns.
        /// </summary>
        public bool HasFilePatterns
        {
            get
            {
                return FilePatterns.Length != 0 && !FilePatterns.All(fp => String.IsNullOrEmpty(fp));
            }
        }

        /// <summary>
        /// Returns true if the exclusion contains parent patterns.
        /// </summary>
        public bool HasParentFiles
        {
            get
            {
                return ParentFiles.Length != 0 && !ParentFiles.All(pf => String.IsNullOrEmpty(pf));
            }
        }

        public override string ToString()
        {
            return String.Format("FilePattern: {0} | Parent: {1} | Comment: {2}", FilePatterns, ParentFiles, Comment);
        }

        private string GetExclusionPart(int index)
        {
            return GetExclusionPart(index, defaultValue: String.Empty);
        }

        private string GetExclusionPart(int index, string defaultValue)
        {
            if ((_exclusionParts != null) && (index >= 0) && (index < _exclusionParts.Length))
            {
                return _exclusionParts[index];
            }

            return defaultValue;
        }

        public bool TryGetIsFileExcluded(string exclusionsClassification, IEnumerable<string> values, out bool isExcluded)
            => TryGetIsExcluded(_fileExcludedCache, _fileNotExcludedCache, exclusionsClassification, values, out isExcluded); 

        public bool TryGetIsParentExcluded(string exclusionsClassification, string parent, out bool isExcluded)
            => TryGetIsExcluded(_parentExcludedCache, _parentNotExcludedCache, exclusionsClassification, new[] { parent }, out isExcluded);

        public void AddToFileCache(string exclusionsClassification, IEnumerable<string> values, bool isExcluded)
        {
            Dictionary<string, HashSet<string>> cache = isExcluded ? _fileExcludedCache : _fileNotExcludedCache;
            AddToCache(cache, exclusionsClassification, values);
        }

        public void AddToParentCache(string exclusionsClassification, string parent, bool isExcluded)
        {
            Dictionary<string, HashSet<string>> cache = isExcluded ? _parentExcludedCache : _parentNotExcludedCache;
            AddToCache(cache, exclusionsClassification, new[] { parent });
        }

        private bool TryGetIsExcluded(
            Dictionary<string, HashSet<string>> excludedCache,
            Dictionary<string, HashSet<string>> notExcludedCache,
            string exclusionClassification,
            IEnumerable<string> values,
            out bool isExcluded)
        {
            if (IsInCache(excludedCache, exclusionClassification, values))
            {
                isExcluded = true;
                return true;
            }

            if (IsInCache(notExcludedCache, exclusionClassification, values))
            {
                isExcluded = false;
                return true;
            }

            isExcluded = false;
            return false;
        }

        private void AddToCache(Dictionary<string, HashSet<string>> cache, string key, IEnumerable<string> values)
        {
            if (!cache.ContainsKey(key))
            {
                cache[key] = new HashSet<string>();
            }

            foreach (var value in values)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    cache[key].Add(value);
                }
            }
        }

        private bool IsInCache(Dictionary<string, HashSet<string>> cache, string key, IEnumerable<string> values)
            => cache.TryGetValue(key, out var cachedValues) && values.Any(cachedValues.Contains);
    }
}
