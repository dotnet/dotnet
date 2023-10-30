// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace XliffTasks
{
    internal static class StringExtensions
    {
        /// <summary>
        /// Attempts to match formatting placeholders as documented at
        /// https://docs.microsoft.com/en-us/dotnet/standard/base-types/composite-formatting.
        ///
        /// Explanation:
        ///
        /// \{
        ///    A placeholder starts with an open curly brace. Since curly braces are used in
        ///    regex syntax we escape it to be clear that we mean a literal {.
        ///
        /// (\d+)
        ///    The "index" component; one or more decimal digits. This is captured in a group
        ///    to facilitate extracting the numeric value.
        ///
        /// (\,\-?\d+)?
        ///    The optional "alignment" component. This is a comma, followed by an optional
        ///    minus sign, followed by one or more decimal digits.
        ///
        /// (\:[^\}]+)?
        ///    The optional "format string" component. This is a colon, followed by one or more
        ///    characters that aren't close curly braces.
        ///
        /// \}
        ///    The close curly brace indicates the end of the placeholder.
        /// </summary>
        private static readonly Regex s_placeholderRegex = new(@"\{(\d+)(\,\-?\d+)?(\:[^\}]+)?\}", RegexOptions.Compiled);

        /// <summary>
        /// Returns the number of replacement strings needed to properly format the given text.
        /// </summary>
        public static int GetReplacementCount(this string text)
        {
            int replacementCount = 0;

            foreach (Match placeholder in s_placeholderRegex.Matches(text))
            {
                int index = int.Parse(placeholder.Groups[1].Value);
                replacementCount = Math.Max(replacementCount, index + 1);
            }

            return replacementCount;
        }
    }
}
