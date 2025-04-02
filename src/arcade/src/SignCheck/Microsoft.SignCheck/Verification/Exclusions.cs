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
        private static readonly char[] _wildCards = new char[] { '*', '?' };
        private List<Exclusion> _exclusions = new List<Exclusion>();

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

        private bool IsExcluded(string path, string parent, string virtualPath, string containerPath, IEnumerable<Exclusion> exclusions)
        {
            foreach (Exclusion e in exclusions)
            {
                // 1. The file/container path matches a file part of the exclusion and the parent matches the parent part of the exclusion.
                //    Example: bar.dll;*.zip --> Exclude any occurence of bar.dll that is in a zip file
                //             bar.dll;foo.zip --> Exclude bar.dll only if it is contained inside foo.zip
                //             foo.exe;; --> Exclude any occurance of foo.exe and ignore the parent
                if (IsMatch(e.FilePatterns, Path.GetFileName(containerPath)) ||
                    IsMatch(e.FilePatterns, containerPath) ||
                    IsMatch(e.FilePatterns, Path.GetFileName(path)) ||
                    IsMatch(e.FilePatterns, path) ||
                    IsMatch(e.FilePatterns, Path.GetFileName(virtualPath)) ||
                    IsMatch(e.FilePatterns, virtualPath))
                {
                    if ((e.ParentFiles.Length == 0) || (e.ParentFiles.All(pf => String.IsNullOrEmpty(pf))) || IsParentExcluded(parent))
                    {
                        return true;
                    }
                }

                // 2. The file/container path matches the file part of the exclusion and there is no parent exclusion. 
                //    Example: *.dll;; --> Exclude any file with a .dll extension
                if ((IsMatch(e.FilePatterns, path) || IsMatch(e.FilePatterns, containerPath)) && (e.ParentFiles.All(pf => String.IsNullOrEmpty(pf))))
                {
                    return true;
                }
            }

            // 3. There is no file exclusion, but a parent exclusion matches.
            //    Example: ;foo.zip; --> Exclude any file in foo.zip. This is similar to using *;foo.zip;
            return exclusions.Any(e => (e.FilePatterns.All(fp => String.IsNullOrEmpty(fp)) && IsParentExcluded(parent)));
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
            IEnumerable<Exclusion> exclusions = _exclusions.Where(e => !e.Comment.Contains("IGNORE-STRONG-NAME"));
            return IsExcluded(path, parent, virtualPath, containerPath, exclusions);
        }

        /// <summary>
        /// Returns true if the file pattern matches the file and the exclusion comment contains DO-NOT-SIGN.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool IsDoNotSign(string path, string parent, string virtualPath, string containerPath)
        {
            // Get all the exclusions with DO-NOT-SIGN markers and check only against those
            IEnumerable<Exclusion> doNotSignExclusions = _exclusions.Where(e => e.Comment.Contains("DO-NOT-SIGN")).ToArray();

            return (doNotSignExclusions.Count() > 0) && (IsExcluded(path, parent, virtualPath, containerPath, doNotSignExclusions));
        }

        public bool IsIgnoreStrongName(string path, string parent, string virtualPath, string containerPath)
        {
            // Get all the exclusions with NO-STRONG-NAME markers and check only against those
            IEnumerable<Exclusion> noStrongNameExclusions = _exclusions.Where(e => e.Comment.Contains("IGNORE-STRONG-NAME"));

            return (noStrongNameExclusions.Count() > 0) && (IsExcluded(path, parent, virtualPath, containerPath, noStrongNameExclusions));
        }

        /// <summary>
        /// Returns true if any <see cref="Exclusion.FilePatterns"/> matches the value of <paramref name="path"/>.
        /// </summary>
        /// <param name="path">The value to match against any <see cref="Exclusion.FilePatterns"/>.</param>
        /// <returns></returns>
        public bool IsFileExcluded(string path)
        {
            return _exclusions.Any(e => IsMatch(e.FilePatterns, path));
        }

        /// <summary>
        /// Returns true if any <see cref="Exclusion.ParentFiles"/> matches the value of <paramref name="parent"/>.
        /// </summary>
        /// <param name="parent">The value to match against any <see cref="Exclusion.ParentFiles"/>.</param>
        /// <returns></returns>
        public bool IsParentExcluded(string parent)
        {
            return _exclusions.Any(e => IsMatch(e.ParentFiles.Select(pattern => !string.IsNullOrEmpty(pattern) ? $"*{pattern}*" : pattern).ToArray(), parent));
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
                string regexPattern = Utils.ConvertToRegexPattern(pattern);
                return Regex.IsMatch(value, regexPattern, RegexOptions.IgnoreCase);
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
    }
}
