// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.Build.Framework;

namespace Microsoft.DotNet.Build.Tasks.Workloads
{
    public static class StringExtensions
    {
        /// <summary>
        /// Removes the leading occurence of a string from the current string.
        /// </summary>
        /// <param name="str">The current string.</param>
        /// <param name="trimString">The string to remove.</param>
        /// <returns>The string that remains after the leading occurence of <paramref name="trimString"/> was removed.</returns>
        public static string TrimStart(this string str, string trimString)
        {
            return str.StartsWith(trimString) ? str.Remove(0, trimString.Length) : str;
        }

        /// <summary>
        /// Removes the leading occrence of a string from the current string.
        /// </summary>
        /// <param name="str">The current string.</param>
        /// <param name="trimString">The string to remove.</param>
        /// <param name="comparisonType">Specifies the comparison rules to use when searching for the string to remove.</param>
        /// <returns>The string that remains after the leading occurence of <paramref name="trimString"/> was removed.</returns>
        public static string TrimStart(this string str, string trimString, StringComparison comparisonType)
        {
            return str.StartsWith(trimString, comparisonType) ? str.Remove(0, trimString.Length) : str;
        }

        /// <summary>
        /// Replace multiple substrings using task items.
        /// </summary>
        /// <param name="str">The current string.</param>
        /// <param name="replacementStrings">An array of task items containing substrings and replacement strings.</param>
        /// <returns>A string with all instances of the specified strings have been replaced.</returns>
        public static string Replace(this string str, ITaskItem[] replacementStrings)
        {
            if ((replacementStrings is not null) && (replacementStrings.Length > 0))
            {
                foreach (ITaskItem item in replacementStrings)
                {
                    str = str.Replace(item.ItemSpec, item.GetMetadata(Metadata.Replacement));
                }
            }

            return str;
        }
    }
}
