// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Microsoft.Css.Parser.Utilities
{
    internal static class RangeExtensions
    {
        /// <summary>
        /// Does this item contain a specific single character position?
        /// </summary>
        internal static bool Contains(this IRange range, int pos, bool inclusiveStart, bool inclusiveEnd)
        {
            //If the position is outside of the range entirely...
            //If the position is just prior to the opening character of the item and the check is start-edge exclusive...
            //If the position is just after the closing character of the item and the check is end-edge exclusive...
            if ((pos < range.Start || pos > range.AfterEnd)
                || !inclusiveStart && pos == range.Start
                || !inclusiveEnd && pos == range.AfterEnd)
            {
                return false;
            }

            return true;
        }
    }
}
