// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.Css.Parser.Utilities;

namespace Microsoft.Css.Parser.Tokens
{
    /// <summary>
    /// This is used to store a sorted list of tokens.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "Public API")]
    public class TokenList : SortedRangeList<CssToken>
    {
        internal TokenList()
        {
        }

        internal TokenList(int capacity)
            : base(capacity)
        {
        }
    }
}
