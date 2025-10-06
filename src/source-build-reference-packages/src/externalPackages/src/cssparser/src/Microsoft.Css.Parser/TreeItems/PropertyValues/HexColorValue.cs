// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Classify;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;

namespace Microsoft.Css.Parser.TreeItems.PropertyValues
{
    /// <summary>
    /// Hex color property value
    /// </summary>
    public class HexColorValue : ComplexItem
    {
        internal TokenItem HashName { get; private set; }

        internal override bool TreatAsWord => true;

        public HexColorValue()
        {
        }

        internal bool TryGetNumberRange(out int start, out int length)
        {
            if (HashName == null)
            {
                start = 0;
                length = 0;
                return false;
            }

            start = HashName.Start + 1;
            length = HashName.Length - 1;
            return true;
        }

        public override bool Parse(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            HashName = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.HexColor);

            return Children.Count > 0;
        }
    }
}
