// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Classify;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;

namespace Microsoft.Css.Parser.TreeItems.Selectors
{
    /// <summary>
    /// http://www.w3.org/TR/selectors4/#subject
    /// Chooses the subject of a selector, like the bang in: "E! > F" or "!E > F"
    /// The CSS 4 spec hasn't decided if before or after is correct.
    /// </summary>
    internal sealed class SubjectSelector : ComplexItem
    {
        internal ParseItem Bang { get; private set; }

        public SubjectSelector()
        {
        }

        public override bool IsValid
        {
            get
            {
                if (Parent == null || Parent.Children.Count == 0)
                {
                    return false;
                }

                // I should be the first or last child of my parent
                bool isFirstChild = Parent.Children[0] == this;
                bool isLastChild = Parent.Children[Parent.Children.Count - 1] == this;

                if (!isFirstChild && !isLastChild)
                {
                    // but it's possible (and most common) to have a later child that is a combine operator
                    if (Parent is SimpleSelector parent)
                    {
                        for (int i = Parent.Children.Count - 1; i >= 0; i--)
                        {
                            if (parent.Children[i] == this)
                            {
                                break;
                            }

                            if (parent.Children[i] != parent.SelectorCombineOperator)
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }

                return base.IsValid;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public override bool Parse(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            if (tokens.CurrentToken.TokenType == CssTokenType.Bang)
            {
                Bang = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.SelectorOperator);
            }

            return Children.Count > 0;
        }
    }
}
