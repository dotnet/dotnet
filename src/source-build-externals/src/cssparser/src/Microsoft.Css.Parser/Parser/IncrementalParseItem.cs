// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;

namespace Microsoft.Css.Parser.Parser
{
    /// <summary>
    /// Items that support incremental parsing will implement this
    /// </summary>
    internal interface IIncrementalParseItem
    {
        ParseItem CreateNextChild(ParseItem previousChild, ItemFactory itemFactory, ITextProvider text, TokenStream tokens);

        void UpdateCachedChildren();
        bool UpdateParseErrors();
    }
}
