// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

namespace NuGet.LibraryModel
{
    /// <summary>
    /// Helper methods for dealing with LibraryDependencyTarget strings.
    /// </summary>
    public static class LibraryDependencyTargetUtils
    {
        /// <summary>
        /// Convert flag string into a LibraryTypeFlag.
        /// </summary>
        public static LibraryDependencyTarget Parse(string flag)
        {
            // If the LibraryDependency does not have a flag value it is considered all
            if (string.IsNullOrEmpty(flag))
            {
                return LibraryDependencyTarget.All;
            }

            var flagEnd = flag.IndexOf(',');
            if (flagEnd == -1)
            {
                var segment = StringSegment.CreateTrimmed(flag, 0, flag.Length - 1);
                if (segment.Length == 0)
                {
                    return LibraryDependencyTarget.All;
                }

                return ParseSingleFlag(segment);
            }
            else
            {
                return ParseMultiFlag(flag, flagEnd);
            }
        }

        private static LibraryDependencyTarget ParseMultiFlag(string flag, int end)
        {
            var result = LibraryDependencyTarget.None;
            var flagsAdded = 0;
            var start = 0;
            do
            {
                var segment = StringSegment.CreateTrimmed(flag, start, end - 1);
                if (segment.Length > 0)
                {
                    flagsAdded++;
                    result |= ParseSingleFlag(segment);
                }

                start = end + 1;
                if (start >= flag.Length)
                {
                    // Reached end, don't look for next comma
                    break;
                }

                end = flag.IndexOf(',', start);
                if (end == -1)
                {
                    end = flag.Length;
                }
            } while (true);

            return flagsAdded > 0 ? result : LibraryDependencyTarget.All;
        }

        private static LibraryDependencyTarget ParseSingleFlag(StringSegment flag)
        {
            var result = LibraryDependencyTarget.None;

            if ("package".Length == flag.Length && string.Compare(flag.String, flag.Start, "package", 0, flag.Length, StringComparison.OrdinalIgnoreCase) == 0)
            {
                result = LibraryDependencyTarget.Package;
            }
            else if ("project".Length == flag.Length && string.Compare(flag.String, flag.Start, "project", 0, flag.Length, StringComparison.OrdinalIgnoreCase) == 0)
            {
                result = LibraryDependencyTarget.Project;
            }
            else if ("externalproject".Length == flag.Length && string.Compare(flag.String, flag.Start, "externalproject", 0, flag.Length, StringComparison.OrdinalIgnoreCase) == 0)
            {
                result = LibraryDependencyTarget.ExternalProject;
            }
            else if ("reference".Length == flag.Length && string.Compare(flag.String, flag.Start, "reference", 0, flag.Length, StringComparison.OrdinalIgnoreCase) == 0)
            {
                result = LibraryDependencyTarget.Reference;
            }
            else if ("assembly".Length == flag.Length && string.Compare(flag.String, flag.Start, "assembly", 0, flag.Length, StringComparison.OrdinalIgnoreCase) == 0)
            {
                result = LibraryDependencyTarget.Assembly;
            }
            else if ("winmd".Length == flag.Length && string.Compare(flag.String, flag.Start, "winmd", 0, flag.Length, StringComparison.OrdinalIgnoreCase) == 0)
            {
                result = LibraryDependencyTarget.WinMD;
            }
            else if ("all".Length == flag.Length && string.Compare(flag.String, flag.Start, "all", 0, flag.Length, StringComparison.OrdinalIgnoreCase) == 0)
            {
                result = LibraryDependencyTarget.All;
            }

            return result;
        }

        /// <summary>
        /// Convert type flags to a friendly string.
        /// </summary>
        public static string GetFlagString(LibraryDependencyTarget flags)
        {
            if (flags == LibraryDependencyTarget.None)
            {
                return "none";
            }

            if (flags == LibraryDependencyTarget.All)
            {
                return "all";
            }

            var flagStrings = new List<string>();

            foreach (LibraryDependencyTarget value in Enum.GetValues(typeof(LibraryDependencyTarget)))
            {
                if (value != LibraryDependencyTarget.None && flags.HasFlag(value))
                {
                    flagStrings.Add(value.ToString().ToLowerInvariant());
                }
            }

            return string.Join(",", flagStrings);
        }

        private static readonly ConcurrentDictionary<LibraryDependencyTarget, string> LibraryDependencyTargetCache = new();

        /// <summary>
        /// Efficiently converts <see cref="LibraryDependencyTarget"/> to it's <see cref="string"/> representation.
        /// </summary>
        /// <param name="includeFlags">The <see cref="LibraryDependencyTarget"/> instance to get the <see cref="string"/> representation for.</param>
        /// <returns>The <see cref="string"/> representation of <paramref name="includeFlags"/>.</returns>
        public static string AsString(this LibraryDependencyTarget includeFlags)
        {
            if (!LibraryDependencyTargetCache.TryGetValue(includeFlags, out string enumAsString))
            {
                enumAsString = includeFlags.ToString();
                LibraryDependencyTargetCache.TryAdd(includeFlags, enumAsString);
            }

            return enumAsString;
        }

        [DebuggerDisplay("{String.Substring(Start, Length)}")]
        private struct StringSegment
        {
            public static StringSegment CreateTrimmed(string String, int start, int end)
            {
                for (; start <= end; start++)
                {
                    if (!char.IsWhiteSpace(String[start])) break;
                }

                for (; end >= start; end--)
                {
                    if (!char.IsWhiteSpace(String[end])) break;
                }

                return new StringSegment
                {
                    Start = start,
                    Length = end - start + 1,
                    String = String
                };
            }

            public int Start;
            public int Length;
            public string String;
        }
    }
}
