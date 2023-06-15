// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace NuGet.LibraryModel
{
    /// <summary>
    /// Helper methods for dealing with include/exclude flag strings.
    /// </summary>
    public static class LibraryIncludeFlagUtils
    {
        /// <summary>
        /// By default build, contentFiles, and analyzers do not flow transitively between projects.
        /// </summary>
        public static readonly LibraryIncludeFlags DefaultSuppressParent =
            (LibraryIncludeFlags.Build | LibraryIncludeFlags.ContentFiles | LibraryIncludeFlags.Analyzers);

        public static readonly LibraryIncludeFlags NoContent = LibraryIncludeFlags.All & ~LibraryIncludeFlags.ContentFiles;

        /// <summary>
        /// Convert set of flag strings into a LibraryIncludeFlags.
        /// </summary>
        public static LibraryIncludeFlags GetFlags(IEnumerable<string> flags)
        {
            if (flags == null)
            {
                throw new ArgumentNullException(nameof(flags));
            }

            var result = LibraryIncludeFlags.None;

            foreach (var flag in flags)
            {
                switch (flag.ToLowerInvariant())
                {
                    case "all":
                        result |= LibraryIncludeFlags.All;
                        break;
                    case "runtime":
                        result |= LibraryIncludeFlags.Runtime;
                        break;
                    case "compile":
                        result |= LibraryIncludeFlags.Compile;
                        break;
                    case "build":
                        result |= LibraryIncludeFlags.Build;
                        break;
                    case "contentfiles":
                        result |= LibraryIncludeFlags.ContentFiles;
                        break;
                    case "native":
                        result |= LibraryIncludeFlags.Native;
                        break;
                    case "analyzers":
                        result |= LibraryIncludeFlags.Analyzers;
                        break;
                    case "buildtransitive":
                        result |= LibraryIncludeFlags.BuildTransitive | LibraryIncludeFlags.Build;
                        break;

                        // None is a noop here
                }
            }

            return result;
        }

        /// <summary>
        /// Convert library flags to a friendly string.
        /// </summary>
        public static string GetFlagString(LibraryIncludeFlags flags)
        {
            if (flags == LibraryIncludeFlags.None)
            {
                return "none";
            }

            if (flags == LibraryIncludeFlags.All)
            {
                return "all";
            }

            var flagStrings = new List<string>();

            foreach (LibraryIncludeFlags value in Enum.GetValues(typeof(LibraryIncludeFlags)))
            {
                if (value != LibraryIncludeFlags.None && flags.HasFlag(value))
                {
                    flagStrings.Add(value.ToString().ToLowerInvariant());
                }
            }

            return string.Join(", ", flagStrings);
        }

        /// <summary>
        /// Convert set of flag strings into a LibraryIncludeFlags.
        /// </summary>
        public static LibraryIncludeFlags GetFlags(string flags, LibraryIncludeFlags defaultFlags)
        {
            var result = defaultFlags;

            if (!string.IsNullOrEmpty(flags))
            {
                var splitFlags = flags.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrEmpty(s))
                    .ToArray();

                if (splitFlags.Length > 0)
                {
                    result = GetFlags(splitFlags);
                }
            }

            return result;
        }

        private static readonly ConcurrentDictionary<LibraryIncludeFlags, string> LibraryIncludeFlagsCache = new();

        /// <summary>
        /// Efficiently converts <see cref="LibraryIncludeFlags"/> to it's <see cref="string"/> representation.
        /// </summary>
        /// <param name="includeFlags">The <see cref="LibraryIncludeFlags"/> instance to get the <see cref="string"/> representation for.</param>
        /// <returns>The <see cref="string"/> representation of <paramref name="includeFlags"/>.</returns>
        public static string AsString(this LibraryIncludeFlags includeFlags)
        {
            if (!LibraryIncludeFlagsCache.TryGetValue(includeFlags, out string enumAsString))
            {
                enumAsString = includeFlags.ToString();
                LibraryIncludeFlagsCache.TryAdd(includeFlags, enumAsString);
            }

            return enumAsString;
        }
    }
}
