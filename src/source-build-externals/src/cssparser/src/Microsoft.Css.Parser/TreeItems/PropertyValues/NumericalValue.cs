// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Diagnostics;
using Microsoft.Css.Parser.Classify;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;

namespace Microsoft.Css.Parser.TreeItems.PropertyValues
{
    /// <summary>
    /// Numerical property value
    /// </summary>
    public class NumericalValue : ComplexItem
    {
        public TokenItem Number { get; private set; }

        internal override bool TreatAsWord => true;

        public NumericalValue()
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        internal static ParseItem ParseNumber(ComplexItem parent, ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            ParseItem pv;

            if (tokens.Peek(1).TokenType == CssTokenType.Units)
            {
                pv = itemFactory.Create<UnitValue>(parent);
            }
            else
            {
                pv = itemFactory.Create<NumericalValue>(parent);
            }

            if (pv != null && !pv.Parse(itemFactory, text, tokens))
            {
                pv = null;
            }

            return pv;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public override bool Parse(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            Debug.Assert(tokens.CurrentToken.TokenType == CssTokenType.Number);

            if (tokens.CurrentToken.TokenType != CssTokenType.Number)
            {
                return false;
            }

            Number = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.Number);

            return Children.Count > 0;
        }
    }
}
