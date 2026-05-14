// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace NuGet.Common
{
    public static class PathResolver
    {
        /// <summary>
        /// Returns a collection of files from the source that matches the wildcard.
        /// </summary>
        /// <param name="source">The collection of files to match.</param>
        /// <param name="getPath">Function that returns the path to filter a package file </param>
        /// <param name="wildcards">The wildcards to apply to match the path with.</param>
        /// <returns></returns>
        public static IEnumerable<T> GetMatches<T>(
            IEnumerable<T> source,
            Func<T, string> getPath,
            IEnumerable<string> wildcards)
        {
            List<Regex> filters = wildcards.Select(WildcardToRegex).ToList();

            return source.AsParallel().Where(item =>
            {
                string path = getPath(item);
                return filters.Any(f => f.IsMatch(path));
            });
        }

        /// <summary>
        /// Removes files from the source that match any wildcard.
        /// </summary>
        public static void FilterPackageFiles<T>(
            ICollection<T> source,
            Func<T, string> getPath,
            IEnumerable<string> wildcards)
        {
            GetFilteredPackageFiles<T>(source, getPath, wildcards);
        }

        public static IEnumerable<T> GetFilteredPackageFiles<T>(
            ICollection<T> source,
            Func<T, string> getPath,
            IEnumerable<string> wildcards)
        {
            var matchedFiles = new HashSet<T>(GetMatches(source, getPath, wildcards));
            List<T> toRemove = source.Where(matchedFiles.Contains).ToList();

            foreach (T item in toRemove)
            {
                source.Remove(item);
            }

            return toRemove;
        }

        public static string NormalizeWildcardForExcludedFiles(string basePath, string wildcard)
        {
            if (wildcard.StartsWith("**", StringComparison.OrdinalIgnoreCase))
            {
                // Allow any path to match the first '**' segment, see issue 2891 for more details.
                return wildcard;
            }
            basePath = NormalizeBasePath(basePath, ref wildcard);
            return Path.Combine(basePath, wildcard);
        }

        private static Regex WildcardToRegex(string wildcard)
        {
            string pattern = Regex.Escape(wildcard);

            if (Path.DirectorySeparatorChar == '/')
            {
                // regex wildcard adjustments for *nix-style file systems
                pattern = pattern
                    .Replace(@"\.\*\*", @"\.[^/.]*") // .** should not match on ../file or ./file but will match .file
                    .Replace(@"\*\*/", "/?([^/]+/)*?") //For recursive wildcards /**/, include the current directory.
                    .Replace(@"\*\*", ".*") // For recursive wildcards that don't end in a slash e.g. **.txt would be treated as a .txt file at any depth
                    .Replace(@"\*", @"[^/]*(/)?") // For non recursive searches, limit it any character that is not a directory separator
                    .Replace(@"\?", "."); // ? translates to a single any character
            }
            else
            {
                // regex wildcard adjustments for Windows-style file systems
                pattern = pattern
                    .Replace("/", @"\\") // On Windows, / is treated the same as \.
                    .Replace(@"\.\*\*", @"\.[^\\.]*") // .** should not match on ../file or ./file but will match .file
                    .Replace(@"\*\*\\", @"(\\\\)?([^\\]+\\)*?") //For recursive wildcards \**\, include the current directory.
                    .Replace(@"\*\*", ".*") // For recursive wildcards that don't end in a slash e.g. **.txt would be treated as a .txt file at any depth
                    .Replace(@"\*", @"[^\\]*(\\)?") // For non recursive searches, limit it any character that is not a directory separator
                    .Replace(@"\?", "."); // ? translates to a single any character
            }

            return new Regex('^' + pattern + '$', RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant);
        }

        public static IEnumerable<string> PerformWildcardSearch(string basePath, string searchPath)
        {
            IEnumerable<SearchPathResult> searchResults = PerformWildcardSearch(
                basePath,
                searchPath,
                includeEmptyDirectories: false,
                normalizedBasePath: out string normalizedBasePath);

            return searchResults.Select(s => s.Path);
        }

        public static IEnumerable<SearchPathResult> PerformWildcardSearch(
            string basePath,
            string searchPath,
            bool includeEmptyDirectories,
            out string normalizedBasePath)
        {
            bool searchDirectory = false;

            // If the searchPath ends with \ or /, we treat searchPath as a directory,
            // and will include everything under it, recursively
            if (IsDirectoryPath(searchPath))
            {
                searchPath = searchPath + "**" + Path.DirectorySeparatorChar + "*";
                searchDirectory = true;
            }

            basePath = NormalizeBasePath(basePath, ref searchPath);
            normalizedBasePath = GetPathToEnumerateFrom(basePath, searchPath);

            // Append the basePath to searchPattern and get the search regex. We need to do this because the search regex is matched from line start.
            Regex searchRegex = WildcardToRegex(Path.Combine(basePath, searchPath));

            // This is a hack to prevent enumerating over the entire directory tree if the only wildcard characters are the ones in the file name. 
            // If the path portion of the search path does not contain any wildcard characters only iterate over the TopDirectory.
            SearchOption searchOption = SearchOption.AllDirectories;
            // (a) Path is not recursive search
            bool isRecursiveSearch = searchPath.IndexOf("**", StringComparison.OrdinalIgnoreCase) != -1;
            // (b) Path does not have any wildcards.
            bool isWildcardPath = Path.GetDirectoryName(searchPath)?.Contains('*') ?? false;
            if (!isRecursiveSearch && !isWildcardPath)
            {
                searchOption = SearchOption.TopDirectoryOnly;
            }

            // Starting from the base path, enumerate over all files and match it using the wildcard expression provided by the user.
            // Note: We use Directory.GetFiles() instead of Directory.EnumerateFiles() here to support Mono
            IEnumerable<SearchPathResult> matchedFiles = from file in Directory.GetFiles(normalizedBasePath, "*.*", searchOption).AsParallel()
                                                         where searchRegex.IsMatch(file)
                                                         select new SearchPathResult(file, isFile: true);

            if (!includeEmptyDirectories)
            {
                return matchedFiles;
            }

            // retrieve empty directories
            // Note: We use Directory.GetDirectories() instead of Directory.EnumerateDirectories() here to support Mono
            IEnumerable<SearchPathResult> matchedDirectories = from directory in Directory.GetDirectories(normalizedBasePath, "*.*", searchOption)
                                                               where searchRegex.IsMatch(directory) && IsEmptyDirectory(directory)
                                                               select new SearchPathResult(directory, isFile: false);

            if (searchDirectory && IsEmptyDirectory(normalizedBasePath))
            {
                matchedDirectories = matchedDirectories.Concat(new[] { new SearchPathResult(normalizedBasePath, isFile: false) });
            }

            return matchedFiles.Concat(matchedDirectories);
        }

        internal static string GetPathToEnumerateFrom(string basePath, string searchPath)
        {
            string basePathToEnumerate;
            int wildcardIndex = searchPath.IndexOf('*');
            if (wildcardIndex == -1)
            {
                // For paths without wildcard, we could either have base relative paths (such as lib\foo.dll) or paths outside the base path
                // (such as basePath: C:\packages and searchPath: D:\packages\foo.dll)
                // In this case, Path.Combine would pick up the right root to enumerate from.
                string? searchRoot = Path.GetDirectoryName(searchPath);
                if (searchRoot is null)
                {
                    throw new ArgumentException(paramName: nameof(searchPath), message: "Path.GetDirectoryName(searchPath) returned null");
                }
                basePathToEnumerate = Path.Combine(basePath, searchRoot);
            }
            else
            {
                // If not, find the first directory separator and use the path to the left of it as the base path to enumerate from.
                int directorySeparatoryIndex = searchPath.LastIndexOf(Path.DirectorySeparatorChar, wildcardIndex);
                if (directorySeparatoryIndex == -1)
                {
                    // We're looking at a path like "NuGet*.dll", NuGet*\bin\release\*.dll
                    // In this case, the basePath would continue to be the path to begin enumeration from.
                    basePathToEnumerate = basePath;
                }
                else
                {
                    string nonWildcardPortion = searchPath.Substring(0, directorySeparatoryIndex);
                    basePathToEnumerate = Path.Combine(basePath, nonWildcardPortion);
                }
            }

            return basePathToEnumerate;
        }

        internal static string NormalizeBasePath(string? basePath, ref string searchPath)
        {
            string parentDirectoryPath = $"..{Path.DirectorySeparatorChar}";
            string currentDirectoryPath = $".{Path.DirectorySeparatorChar}";

            // If no base path is provided, use the current directory.
            basePath = string.IsNullOrEmpty(basePath) ? currentDirectoryPath : basePath;

            // If the search path is relative, transfer the parent directory portion to the base path.
            // This needs to be done because the base path determines the root for our enumeration.
            while (searchPath.StartsWith(parentDirectoryPath, StringComparison.OrdinalIgnoreCase))
            {
                basePath = Path.Combine(basePath, parentDirectoryPath);
                searchPath = searchPath.Substring(parentDirectoryPath.Length);
            }

            return Path.GetFullPath(basePath);
        }

        /// <summary>
        /// Returns true if the path contains any wildcard characters.
        /// </summary>
        public static bool IsWildcardSearch(string filter)
        {
            return filter.IndexOf('*') != -1;
        }

        public static bool IsDirectoryPath(string? path)
        {
            return path != null && path.Length > 1 &&
                (path[path.Length - 1] == Path.DirectorySeparatorChar ||
                path[path.Length - 1] == Path.AltDirectorySeparatorChar);
        }

        private static bool IsEmptyDirectory(string directory)
        {
            return !Directory.EnumerateFileSystemEntries(directory).Any();
        }

        public struct SearchPathResult
        {
            public string Path { get; }
            public bool IsFile { get; }

            public SearchPathResult(string path, bool isFile)
            {
                Path = path;
                IsFile = isFile;
            }
        }
    }
}
