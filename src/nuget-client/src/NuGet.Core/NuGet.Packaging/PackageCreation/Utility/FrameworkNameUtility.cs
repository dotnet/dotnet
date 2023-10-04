// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;

using NuGet.Frameworks;

namespace NuGet.Packaging
{
    public static class FrameworkNameUtility
    {
        public static FrameworkName ParseFrameworkNameFromFilePath(string filePath, out string effectivePath)
        {
            foreach (string knownFolder in PackagingConstants.Folders.Known)
            {
                string folderPrefix = knownFolder + System.IO.Path.DirectorySeparatorChar;
                if (filePath.Length > folderPrefix.Length &&
                    filePath.StartsWith(folderPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    string frameworkPart = filePath.Substring(folderPrefix.Length);

                    try
                    {
                        return FrameworkNameUtility.ParseFrameworkFolderName(
                            frameworkPart,
                            strictParsing: knownFolder == PackagingConstants.Folders.Lib,
                            effectivePath: out effectivePath);
                    }
                    catch (ArgumentException)
                    {
                        // if the parsing fails, we treat it as if this file
                        // doesn't have target framework.
                        effectivePath = frameworkPart;
                        return null;
                    }
                }

            }

            effectivePath = filePath;
            return null;
        }

        /// <summary>
        /// Parses the specified string into FrameworkName object.
        /// </summary>
        /// <param name="path">The string to be parse.</param>
        /// <param name="strictParsing">if set to <see langword="true" />, parse the first folder of path even if it is unrecognized framework.</param>
        /// <param name="effectivePath">returns the path after the parsed target framework</param>
        /// <returns></returns>
        public static FrameworkName ParseFrameworkFolderName(string path, bool strictParsing, out string effectivePath)
        {
            // The path for a reference might look like this for assembly foo.dll:
            // foo.dll
            // sub\foo.dll
            // {FrameworkName}{Version}\foo.dll
            // {FrameworkName}{Version}\sub1\foo.dll
            // {FrameworkName}{Version}\sub1\sub2\foo.dll

            // Get the target framework string if specified
            string targetFrameworkString = Path.GetDirectoryName(path).Split(Path.DirectorySeparatorChar).First();

            effectivePath = path;

            if (string.IsNullOrEmpty(targetFrameworkString))
            {
                return null;
            }

            var nugetFramework = NuGetFramework.ParseFolder(targetFrameworkString);

            if (strictParsing || nugetFramework.IsSpecificFramework)
            {
                // skip past the framework folder and the character \
                effectivePath = path.Substring(targetFrameworkString.Length + 1);
                return new FrameworkName(nugetFramework.DotNetFrameworkName);
            }

            return null;
        }

        public static NuGetFramework ParseNuGetFrameworkFromFilePath(string filePath, out string effectivePath)
        {
            foreach (string knownFolder in PackagingConstants.Folders.Known)
            {
                string folderPrefix = knownFolder + System.IO.Path.DirectorySeparatorChar;
                if (filePath.Length > folderPrefix.Length &&
                    filePath.StartsWith(folderPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    string frameworkPart = filePath.Substring(folderPrefix.Length);

                    try
                    {
                        return FrameworkNameUtility.ParseNuGetFrameworkFolderName(
                            frameworkPart,
                            strictParsing: knownFolder == PackagingConstants.Folders.Lib,
                            effectivePath: out effectivePath);
                    }
                    catch (ArgumentException)
                    {
                        // if the parsing fails, we treat it as if this file
                        // doesn't have target framework.
                        effectivePath = frameworkPart;
                        return null;
                    }
                }

            }

            effectivePath = filePath;
            return null;
        }

        /// <summary>
        /// Parses the specified string into FrameworkName object.
        /// </summary>
        /// <param name="path">The string to be parse.</param>
        /// <param name="strictParsing">if set to <see langword="true" />, parse the first folder of path even if it is unrecognized framework.</param>
        /// <param name="effectivePath">returns the path after the parsed target framework</param>
        /// <returns></returns>
        public static NuGetFramework ParseNuGetFrameworkFolderName(string path, bool strictParsing, out string effectivePath)
        {
            // The path for a reference might look like this for assembly foo.dll:
            // foo.dll
            // sub\foo.dll
            // {FrameworkName}{Version}\foo.dll
            // {FrameworkName}{Version}\sub1\foo.dll
            // {FrameworkName}{Version}\sub1\sub2\foo.dll

            // Get the target framework string if specified
            string targetFrameworkString = Path.GetDirectoryName(path).Split(Path.DirectorySeparatorChar).First();

            effectivePath = path;

            if (string.IsNullOrEmpty(targetFrameworkString))
            {
                return null;
            }

            var nugetFramework = NuGetFramework.ParseFolder(targetFrameworkString);

            if (strictParsing || nugetFramework.IsSpecificFramework)
            {
                // skip past the framework folder and the character \
                effectivePath = path.Substring(targetFrameworkString.Length + 1);
                return nugetFramework;
            }

            return null;
        }
    }
}
