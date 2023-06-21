// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace NuGet.Common
{
    public static class PathUtility
    {
        private static readonly Lazy<bool> _isFileSystemCaseInsensitive = new Lazy<bool>(CheckIfFileSystemIsCaseInsensitive);

        /// <summary>
        /// Returns OrdinalIgnoreCase Windows and Mac. Ordinal for Linux. (Do not use with package paths)
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// This method should not be used with package paths.
        /// Package paths are always case sensitive, using this with package paths leads to bugs like https://github.com/NuGet/Home/issues/9817.
        /// </remarks>
        public static StringComparer GetStringComparerBasedOnOS()
        {
            if (IsFileSystemCaseInsensitive)
            {
                return StringComparer.OrdinalIgnoreCase;
            }

            return StringComparer.Ordinal;
        }

        /// <summary>
        /// Returns OrdinalIgnoreCase Windows and Mac. Ordinal for Linux. (Do not use with package paths)
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// This method should not be used with package paths.
        /// Package paths are always case sensitive, using this with package paths leads to bugs like https://github.com/NuGet/Home/issues/9817.
        /// </remarks>
        public static StringComparison GetStringComparisonBasedOnOS()
        {
            if (IsFileSystemCaseInsensitive)
            {
                return StringComparison.OrdinalIgnoreCase;
            }

            return StringComparison.Ordinal;
        }

        /// <summary>
        /// Returns distinct orderd paths based on the file system case sensitivity.
        /// </summary>
        public static IEnumerable<string> GetUniquePathsBasedOnOS(IEnumerable<string> paths)
        {
            if (paths == null)
            {
                throw new ArgumentNullException(nameof(paths));
            }

            var unique = new HashSet<string>(GetStringComparerBasedOnOS());

            foreach (var path in paths)
            {
                if (unique.Add(path))
                {
                    yield return path;
                }
            }

            yield break;
        }

        /// <summary>
        /// Replace all back slashes with forward slashes.
        /// If the path does not contain a back slash
        /// the original string is returned.
        /// </summary>
        public static string GetPathWithForwardSlashes(string path)
        {
            if (path != null && path.IndexOf('\\') > -1)
            {
                return path.Replace('\\', '/');
            }

            // Leave the path unchanged.
            return path;
        }

        public static string EnsureTrailingSlash(string path)
        {
            return EnsureTrailingCharacter(path, Path.DirectorySeparatorChar);
        }

        public static string EnsureTrailingForwardSlash(string path)
        {
            return EnsureTrailingCharacter(path, '/');
        }

        private static string EnsureTrailingCharacter(string path, char trailingCharacter)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            // if the path is empty, we want to return the original string instead of a single trailing character.
            if (path.Length == 0
                || path[path.Length - 1] == trailingCharacter)
            {
                return path;
            }
            // This condition checks if there is a different valid path separator than the one requested for.
            // In that case we replace this path separator.
            else if (HasTrailingDirectorySeparator(path))
            {
                return path.Substring(0, path.Length - 1) + trailingCharacter;
            }

            return path + trailingCharacter;
        }

        public static bool IsChildOfDirectory(string dir, string candidate)
        {
            if (dir == null)
            {
                throw new ArgumentNullException(nameof(dir));
            }
            if (candidate == null)
            {
                throw new ArgumentNullException(nameof(candidate));
            }
            dir = Path.GetFullPath(dir);
            dir = EnsureTrailingSlash(dir);
            candidate = Path.GetFullPath(candidate);
            return candidate.StartsWith(dir, StringComparison.OrdinalIgnoreCase);
        }

        public static bool HasTrailingDirectorySeparator(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }
            else
            {
                return IsDirectorySeparatorChar(path[path.Length - 1]);
            }
        }

        public static bool IsDirectorySeparatorChar(char ch)
        {
            if (RuntimeEnvironmentHelper.IsWindows)
            {
                // Windows has both '/' and '\' as valid directory separators.
                return (ch == Path.DirectorySeparatorChar ||
                        ch == Path.AltDirectorySeparatorChar);
            }
            else
            {
                return ch == Path.DirectorySeparatorChar;
            }
        }

        public static void EnsureParentDirectory(string filePath)
        {
            string directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        /// <summary>
        /// Returns path2 relative to path1, with Path.DirectorySeparatorChar as separator
        /// </summary>
        public static string GetRelativePath(string path1, string path2)
        {
            return GetRelativePath(path1, path2, Path.DirectorySeparatorChar);
        }

        /// <summary>
        /// Returns path2 relative to path1, with given path separator
        /// </summary>
        public static string GetRelativePath(string path1, string path2, char separator)
        {
            if (string.IsNullOrEmpty(path1))
            {
                throw new ArgumentException("Path must have a value", nameof(path1));
            }

            if (string.IsNullOrEmpty(path2))
            {
                throw new ArgumentException("Path must have a value", nameof(path2));
            }

            StringComparison compare;
            if (RuntimeEnvironmentHelper.IsWindows)
            {
                compare = StringComparison.OrdinalIgnoreCase;
                // check if paths are on the same volume
                if (!string.Equals(Path.GetPathRoot(path1), Path.GetPathRoot(path2), compare))
                {
                    // on different volumes, "relative" path is just path2
                    return path2;
                }
            }
            else
            {
                compare = StringComparison.Ordinal;
            }

            var index = 0;
            var path1Segments = path1.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            var path2Segments = path2.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            // if path1 does not end with / it is assumed the end is not a directory
            // we will assume that is isn't a directory by ignoring the last split
            var len1 = path1Segments.Length - 1;
            var len2 = path2Segments.Length;

            // find largest common absolute path between both paths
            var min = Math.Min(len1, len2);
            while (min > index)
            {
                if (!string.Equals(path1Segments[index], path2Segments[index], compare))
                {
                    break;
                }
                // Handle scenarios where folder and file have same name (only if os supports same name for file and directory)
                // e.g. /file/name /file/name/app
                else if ((len1 == index && len2 > index + 1) || (len1 > index && len2 == index + 1))
                {
                    break;
                }
                ++index;
            }

            // check if path2 ends with a non-directory separator and if path1 has the same non-directory at the end
            if (len1 + 1 == len2 && !string.IsNullOrEmpty(path1Segments[index]) &&
                string.Equals(path1Segments[index], path2Segments[index], compare))
            {
                return string.Empty;
            }

            var path = StringBuilderPool.Shared.Rent(256);

            const string twoDots = "..";
            for (var i = index; len1 > i; ++i)
            {
                path.Append(twoDots);
                path.Append(separator);
            }

            for (var i = index; len2 - 1 > i; ++i)
            {
                path.Append(path2Segments[i]);
                path.Append(separator);
            }

            // if path2 doesn't end with an empty string it means it ended with a non-directory name, so we add it back
            if (!string.IsNullOrEmpty(path2Segments[len2 - 1]))
            {
                path.Append(path2Segments[len2 - 1]);
            }

            return StringBuilderPool.Shared.ToStringAndReturn(path);
        }

        public static string GetAbsolutePath(string basePath, string relativePath)
        {
            if (basePath == null)
            {
                throw new ArgumentNullException(nameof(basePath));
            }

            if (relativePath == null)
            {
                throw new ArgumentNullException(nameof(relativePath));
            }

            Uri resultUri = new Uri(new Uri(basePath), new Uri(relativePath, UriKind.Relative));
            return resultUri.LocalPath;
        }

        public static string GetDirectoryName(string path)
        {
            path = path.TrimEnd(Path.DirectorySeparatorChar);
            return path.Substring(Path.GetDirectoryName(path).Length).Trim(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        public static string GetPathWithBackSlashes(string path)
        {
            return path.Replace('/', '\\');
        }

        public static string GetPathWithDirectorySeparator(string path)
        {
            if (Path.DirectorySeparatorChar == '/')
            {
                return GetPathWithForwardSlashes(path);
            }
            else
            {
                return GetPathWithBackSlashes(path);
            }
        }

        public static string GetPath(Uri uri)
        {
            var path = uri.OriginalString;
            if (path.StartsWith("/", StringComparison.Ordinal))
            {
                path = path.Substring(1);
            }

            // Bug 483: We need the unescaped uri string to ensure that all characters are valid for a path.
            // Change the direction of the slashes to match the filesystem.
            return Uri.UnescapeDataString(path.Replace('/', Path.DirectorySeparatorChar));
        }

        public static string EscapePSPath(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            // The and [ the ] characters are interpreted as wildcard delimiters. Escape them first.
            path = path.Replace("[", "`[").Replace("]", "`]");

            if (path.Contains("'"))
            {
                // If the path has an apostrophe, then use double quotes to enclose it.
                // However, in that case, if the path also has $ characters in it, they
                // will be interpreted as variables. Thus we escape the $ characters.
                return "\"" + path.Replace("$", "`$") + "\"";
            }
            // if the path doesn't have apostrophe, then it's safe to enclose it with apostrophes
            return "'" + path + "'";
        }

        public static string SmartTruncate(string path, int maxWidth)
        {
            if (maxWidth < 6)
            {
                var message = string.Format(CultureInfo.CurrentCulture, Strings.Argument_Must_Be_GreaterThanOrEqualTo, 6);
                throw new ArgumentOutOfRangeException(nameof(maxWidth), message);
            }

            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (path.Length <= maxWidth)
            {
                return path;
            }

            // get the leaf folder name of this directory path
            // e.g. if the path is C:\documents\projects\visualstudio\, we want to get the 'visualstudio' part.
            var folder = path.Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault() ?? string.Empty;
            // surround the folder name with the pair of \ characters.
            folder = Path.DirectorySeparatorChar + folder + Path.DirectorySeparatorChar;

            var root = Path.GetPathRoot(path);
            var remainingWidth = maxWidth - root.Length - 3; // 3 = length(ellipsis)

            // is the directory name too big? 
            if (folder.Length >= remainingWidth)
            {
                // yes drop leading backslash and eat into name
                return string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}...{1}",
                    root,
                    folder.Substring(folder.Length - remainingWidth));
            }
            // no, show like VS solution explorer (drive+ellipsis+end)
            return string.Format(
                CultureInfo.InvariantCulture,
                "{0}...{1}",
                root,
                folder);
        }

        public static bool IsSubdirectory(string basePath, string path)
        {
            if (basePath == null)
            {
                throw new ArgumentNullException(nameof(basePath));
            }
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            basePath = basePath.TrimEnd(Path.DirectorySeparatorChar);
            return path.StartsWith(basePath, StringComparison.OrdinalIgnoreCase);
        }

        public static string ReplaceAltDirSeparatorWithDirSeparator(string path)
        {
            return path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
        }

        public static string ReplaceDirSeparatorWithAltDirSeparator(string path)
        {
            return path.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        public static ZipArchiveEntry GetEntry(ZipArchive archive, string path)
        {
            return archive.Entries.SingleOrDefault(
                    z => string.Equals(
                        Uri.UnescapeDataString(z.FullName),
                        ReplaceDirSeparatorWithAltDirSeparator(path),
                        StringComparison.OrdinalIgnoreCase));
        }

        public static bool IsFileSystemCaseInsensitive
        {
            get { return _isFileSystemCaseInsensitive.Value; }
        }

        private static bool CheckIfFileSystemIsCaseInsensitive()
        {
            if (RuntimeEnvironmentHelper.IsWindows)
            {
                return true;
            }
            else
            {
                var listOfPathsToCheck = new string[]
                {
                    NuGetEnvironment.GetFolderPath(NuGetFolderPath.NuGetHome),
                    NuGetEnvironment.GetFolderPath(NuGetFolderPath.Temp),
                    Directory.GetCurrentDirectory()
                };

                var isCaseInsensitive = true;
                foreach (var path in listOfPathsToCheck)
                {
                    bool ignore;
                    var result = CheckCaseSenstivityRecursivelyTillDirectoryExists(path, out ignore);
                    if (!ignore)
                    {
                        isCaseInsensitive &= result;
                    }
                }
                return isCaseInsensitive;
            }
        }

        private static bool CheckCaseSenstivityRecursivelyTillDirectoryExists(string path, out bool ignoreResult)
        {
            var parentDirectoryFound = true;
            path = Path.GetFullPath(path);
            ignoreResult = true;
            while (true)
            {
                if (path.Length <= 1)
                {
                    ignoreResult = true;
                    parentDirectoryFound = false;
                    break;
                }
                if (Directory.Exists(path))
                {
                    ignoreResult = false;
                    break;
                }
                path = Path.GetDirectoryName(path);
            }

            if (parentDirectoryFound)
            {
                return Directory.Exists(path.ToLowerInvariant()) && Directory.Exists(path.ToUpperInvariant());
            }
            return false;
        }

        public static string StripLeadingDirectorySeparators(string filename)
        {
            filename = GetPathWithForwardSlashes(filename);
            var currentDirectoryPath = $"./";

            if (filename.StartsWith(currentDirectoryPath, PathUtility.GetStringComparisonBasedOnOS()))
            {
                filename = filename.Substring(currentDirectoryPath.Length);
            }
            return filename.TrimStart('/');
        }
    }
}
