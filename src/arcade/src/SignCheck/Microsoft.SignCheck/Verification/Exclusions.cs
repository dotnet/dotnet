// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.SignCheck.Verification
{
    public class Exclusions
    {
        /// <summary>
        /// Cache for regex exclusions.
        /// Helps avoid recompiling the regex for the same pattern multiple times.
        /// </summary>
        private Dictionary<string, Regex> _regexCache = new Dictionary<string, Regex>();
        private static readonly char[] _wildCards = new char[] { '*', '?' };
        private List<Exclusion> _exclusions = new List<Exclusion>();

        private const string DoNotSign = "DO-NOT-SIGN";
        private const string General = "GENERAL";
        private const string IgnoreStrongName = "IGNORE-STRONG-NAME";
        private const string DoNotUnpack = "DO-NOT-UNPACK";

        public int Count
        {
            get
            {
                return _exclusions.Count;
            }
        }

        public Exclusions()
        {

        }

        /// <summary>
        /// Creates a collection of <see cref="Exclusion"/>s from a text file. 
        /// </summary>
        /// <param name="path">Path to a file that contains exclusion entries.</param>
        public Exclusions(string path)
        {
            if (File.Exists(path))
            {
                using (StreamReader fileReader = File.OpenText(path))
                {
                    string line = fileReader.ReadLine();

                    while (line != null)
                    {
                        if (!String.IsNullOrEmpty(line))
                        {
                            Add(new Exclusion(line));
                        }
                        line = fileReader.ReadLine();
                    }
                }
            }
        }

        public void Add(Exclusion exclusion)
        {
            if (!_exclusions.Contains(exclusion))
            {
                _exclusions.Add(exclusion);
            }
        }

        public void Clear()
        {
            _exclusions.Clear();
        }

        public bool Contains(Exclusion exclusion)
        {
            return _exclusions.Contains(exclusion);
        }

        private bool IsExcluded(string path, string parent, string virtualPath, string containerPath, string exclusionClassification, IEnumerable<Exclusion> exclusions)
        {
            foreach (var exclusion in exclusions)
            {
                // 1. The file/container path matches a file part of the exclusion and the parent matches the parent part of the exclusion.
                //    Example: bar.dll;*.zip --> Exclude any occurence of bar.dll that is in a zip file
                //             bar.dll;foo.zip --> Exclude bar.dll only if it is contained inside foo.zip
                //             foo.exe;; --> Exclude any occurance of foo.exe and ignore the parent
                if (IsFileExcluded(exclusion, path, containerPath, virtualPath, exclusionClassification) &&
                    (!exclusion.HasParentFiles || IsParentExcluded(exclusion, parent, exclusionClassification)))
                {
                    return true;
                }

                // 2. There is no file exclusion, but a parent exclusion matches.
                //    Example: ;foo.zip; --> Exclude any file in foo.zip. This is similar to using *;foo.zip;
                if (!exclusion.HasFilePatterns && IsParentExcluded(exclusion, parent, exclusionClassification))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Return true if an exclusion matches the file path, parent file container or the path in the container
        /// </summary>
        /// <param name="path">The path of the file on disk.</param>
        /// <param name="parent">The parent (container) of the file.</param>
        /// <param name="virtualPath">The full path of the parent (container).</param>
        /// <param name="containerPath">The path of the file in the container. May be null if the file is not embedded in a container.</param>
        /// <returns></returns>
        public bool IsExcluded(string path, string parent, string virtualPath, string containerPath)
        {
            IEnumerable<Exclusion> exclusions = _exclusions.Where(e => !e.Comment.Contains(IgnoreStrongName) && !e.Comment.Contains(DoNotUnpack));
            return IsExcluded(path, parent, virtualPath, containerPath, General, exclusions);
        }

        /// <summary>
        /// Returns true if the file pattern matches the file and the exclusion comment contains DO-NOT-SIGN.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool IsDoNotSign(string path, string parent, string virtualPath, string containerPath)
        {
            // Get all the exclusions with DO-NOT-SIGN markers and check only against those
            IEnumerable<Exclusion> doNotSignExclusions = _exclusions.Where(e => e.Comment.Contains(DoNotSign)).ToArray();

            return (doNotSignExclusions.Count() > 0) && (IsExcluded(path, parent, virtualPath, containerPath, DoNotSign, doNotSignExclusions));
        }

        public bool IsIgnoreStrongName(string path, string parent, string virtualPath, string containerPath)
        {
            // Get all the exclusions with NO-STRONG-NAME markers and check only against those
            IEnumerable<Exclusion> noStrongNameExclusions = _exclusions.Where(e => e.Comment.Contains(IgnoreStrongName));

            return (noStrongNameExclusions.Count() > 0) && (IsExcluded(path, parent, virtualPath, containerPath, IgnoreStrongName, noStrongNameExclusions));
        }

        public bool IsDoNotUnpack(string path, string parent, string virtualPath, string containerPath)
        {
            // Get all the exclusions with DO-NOT-UNPACK markers and check only against those
            IEnumerable<Exclusion> doNotUnpackExclusions = _exclusions.Where(e => e.Comment.Contains(DoNotUnpack));

            return (doNotUnpackExclusions.Count() > 0) && (IsExcluded(path, parent, virtualPath, containerPath, DoNotUnpack, doNotUnpackExclusions));
        }

        /// <summary>
        /// Returns true if any <see cref="Exclusion.FilePatterns"/> matches the value of
        /// <paramref name="path"/>, <paramref name="containerPath"/> or <paramref name="virtualPath"/>.
        /// </summary>
        /// <param name="path">The value to match against <see cref="Exclusion.FilePatterns"/>.</param>
        /// <param name="containerPath">The value to match against <see cref="Exclusion.FilePatterns"/>.</param>
        /// <param name="virtualPath">The value to match against <see cref="Exclusion.FilePatterns"/>.</param>
        /// <returns></returns>
        public bool IsFileExcluded(Exclusion exclusion, string path, string containerPath, string virtualPath, string exclusionsClassification)
        {
            var values = new[] { path, containerPath, virtualPath, Path.GetFileName(path), Path.GetFileName(containerPath), Path.GetFileName(virtualPath) };

            if(!exclusion.TryGetIsFileExcluded(exclusionsClassification, values, out bool isExcluded))
            {
                isExcluded = values.Any(v => IsMatch(exclusion.FilePatterns, v));
                exclusion.AddToFileCache(exclusionsClassification, values, isExcluded);
            }

            return isExcluded;
        }

        /// <summary>
        /// Returns true if any <see cref="Exclusion.ParentFiles"/> matches the value of <paramref name="parent"/>.
        /// </summary>
        /// <param name="parent">The value to match against <see cref="Exclusion.ParentFiles"/>.</param>
        /// <returns></returns>
        public bool IsParentExcluded(Exclusion exclusion, string parent, string exclusionsClassification)
        {
            if(!exclusion.TryGetIsParentExcluded(exclusionsClassification, parent, out bool isExcluded))
            {
                isExcluded = IsMatch(exclusion.ParentFiles, parent);
                exclusion.AddToParentCache(exclusionsClassification, parent, isExcluded);
            }
    
            return isExcluded;
        }

        private bool IsMatch(string[] patterns, string value)
        {
            return patterns.Any(p => IsMatch(p, value));
        }

        private bool IsMatch(string pattern, string value)
        {
            if (String.IsNullOrEmpty(pattern) || String.IsNullOrEmpty(value))
            {
                return false;
            }

            if (pattern.IndexOfAny(_wildCards) > -1)
            {
                var regex = GetRegex(pattern);
                return regex.IsMatch(value);
            }
            else
            {
                return String.Equals(pattern, value, StringComparison.OrdinalIgnoreCase);
            }
        }

        public bool Remove(Exclusion exclusion)
        {
            return false;
        }

        private Regex GetRegex(string pattern)
        {
            if (!_regexCache.ContainsKey(pattern))
            {
                string regexPattern = Utils.ConvertToRegexPattern(pattern);
                _regexCache[pattern] = new Regex(regexPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            }
            return _regexCache[pattern];
        }
    }
}
