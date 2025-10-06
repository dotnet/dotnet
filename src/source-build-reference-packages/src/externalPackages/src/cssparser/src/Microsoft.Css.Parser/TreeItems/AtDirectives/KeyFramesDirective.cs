// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Classify;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;

namespace Microsoft.Css.Parser.TreeItems.AtDirectives
{
    // http://www.w3.org/TR/2009/WD-css3-animations-20090320/
    //  keyframes-rule: '@keyframes' IDENT '{' keyframes-blocks '}';
    //  keyframes-blocks: [ keyframe-selectors block ]* ;
    //  keyframe-selectors: [ 'from' | 'to' | PERCENTAGE ] [ ',' [ 'from' | 'to' | PERCENTAGE ] ]*;
    //
    //   @keyframes bounce {
    //      from {
    //          top: 100px;
    //          animation-timing-function: ease-out;
    //      }
    //      25% {
    //          top: 50px;
    //          animation-timing-function: ease-in;
    //      }
    //      50% {
    //          top: 100px;
    //          animation-timing-function: ease-out;
    //      }
    //      75% {
    //          top: 75px;
    //          animation-timing-function: ease-in;
    //      }
    //      to {
    //          top: 100px;
    //      }
    //  }

    internal class KeyFramesDirective : AtBlockDirective
    {
        internal KeyFramesBlock KeyFramesBlock { get; private set; }
        internal ParseItem Name { get; private set; }

        public KeyFramesDirective()
        {
        }

        internal override BlockItem Block
        {
            get { return KeyFramesBlock; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public override bool Parse(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            ParseAtAndKeyword(itemFactory, text, tokens);
            Name = ParseName(itemFactory, text, tokens);

            KeyFramesBlock = itemFactory.CreateSpecific<KeyFramesBlock>(this);
            if (!ParseBlock(KeyFramesBlock, itemFactory, text, tokens))
            {
                KeyFramesBlock = null;
            }

            return Children.Count > 0;
        }

        protected virtual ParseItem ParseName(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            if (tokens.CurrentToken.TokenType == CssTokenType.Identifier)
            {
                return Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.AtDirectiveKeyword);
            }
            else
            {
                Children.AddParseError(ParseErrorType.KeyFrameBlockNameMissing);
                return null;
            }
        }
    }
}
