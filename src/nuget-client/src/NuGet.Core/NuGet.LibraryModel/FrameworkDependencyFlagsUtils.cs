// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace NuGet.LibraryModel
{
    public static class FrameworkDependencyFlagsUtils
    {
        public static readonly FrameworkDependencyFlags Default = FrameworkDependencyFlags.None;

        /// <summary>
        /// Convert set of flag strings into a FrameworkDependencyFlags.
        /// </summary>
        /// <param name="values">A list of values to generate the flags out of</param>
        public static FrameworkDependencyFlags GetFlags(IEnumerable<string>? values)
        {
            var result = FrameworkDependencyFlags.None;

            if (values != null)
            {
                foreach (var value in values)
                {
                    if (Enum.TryParse<FrameworkDependencyFlags>(value, ignoreCase: true, result: out var flag))
                    {
                        result |= flag;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Convert framework dependency flags to a friendly string.
        /// </summary>
        public static string GetFlagString(FrameworkDependencyFlags flags)
        {
            switch (flags)
            {
                case FrameworkDependencyFlags.All:
                    return "all";
                case FrameworkDependencyFlags.None:
                default:
                    return "none";
            }
        }

        /// <summary>
        /// Convert set of flag strings into a LibraryIncludeFlags.
        /// If the <paramref name="flags"/> is null, it returns the default value of <see cref="FrameworkDependencyFlags.None"/>
        /// </summary>
        public static FrameworkDependencyFlags GetFlags(string? flags)
        {
            var result = FrameworkDependencyFlags.None;

            if (!string.IsNullOrEmpty(flags))
            {
                var splitFlags = flags!.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
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
    }
}
