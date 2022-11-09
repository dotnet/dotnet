// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace NuGet.VisualStudio
{
    public static class PathHelper
    {
        public static string SmartTruncate(string path, int maxWidth)
        {
            if (maxWidth < 6)
            {
                string message = String.Format(CultureInfo.CurrentCulture, Resources.Argument_Must_Be_GreaterThanOrEqualTo, 6);
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
            string folder = path.Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault() ?? String.Empty;
            // surround the folder name with the pair of \ characters.
            folder = Path.DirectorySeparatorChar + folder + Path.DirectorySeparatorChar;

            string root = Path.GetPathRoot(path);
            int remainingWidth = maxWidth - root.Length - 3; // 3 = length(ellipsis)

            // is the directory name too big? 
            if (folder.Length >= remainingWidth)
            {
                // yes drop leading backslash and eat into name
                return String.Format(
                    CultureInfo.InvariantCulture,
                    "{0}...{1}",
                    root,
                    folder.Substring(folder.Length - remainingWidth));
            }
            // no, show like VS solution explorer (drive+ellipsis+end)
            return String.Format(
                CultureInfo.InvariantCulture,
                "{0}...{1}",
                root,
                folder);
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
    }
}
