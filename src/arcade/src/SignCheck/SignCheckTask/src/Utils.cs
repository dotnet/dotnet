// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.IO;

namespace SignCheckTask
{
    public static class Utils
    {
        public static readonly char[] WildCards = new char[] { '*', '?' };

        public static string[] GetDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            if (path.IndexOfAny(WildCards) > -1)
            {
                var directoryPath = Path.GetDirectoryName(path);
                var directory = Path.GetFileName(path);

                var matchedDirectories = GetDirectories(directoryPath, directory, searchOption);
                var directories = new List<string>();

                if (searchPattern != null)
                {
                    foreach (var match in matchedDirectories)
                    {
                        directories.AddRange(Directory.GetDirectories(match, searchPattern, searchOption));
                    }

                    return directories.ToArray();
                }
                else
                {
                    return matchedDirectories;
                }
            }
            else
            {
                if (searchPattern != null)
                {
                    return Directory.GetDirectories(path, searchPattern, searchOption);
                }
            }

            return null;
        }
    }
}
