// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Microsoft.Css.Parser.TreeItems
{
    /// <summary>
    /// This is used for parsing the contents of a style attribute from an HTML file.
    /// It's just a rule block that doesn't have curly braces.
    /// </summary>
    internal sealed class InlineStyle : RuleBlock
    {
        internal InlineStyle()
        {
        }

        protected override bool IsInlineStyle
        {
            get { return true; }
        }
    }
}
