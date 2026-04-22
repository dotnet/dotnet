// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Classify;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;

namespace Microsoft.Css.Parser.TreeItems.Selectors
{
    /// <summary>
    /// CSS attribute selector like [att=value]
    /// </summary>
    public class AttributeSelector : ComplexItem
    {
        internal TokenItem OpenBracket { get; private set; }
        internal ItemName AttributeName { get; private set; } // namespace may be missing as in [att] { color: green }
        internal TokenItem Operation { get; private set; } // =, ~=, |= , ...
        internal TokenItem OperationModifier { get; private set; } // i
        internal TokenItem AttributeValue { get; private set; }
        internal TokenItem CloseBracket { get; private set; }

        public AttributeSelector()
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public override bool Parse(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            if (tokens.CurrentToken.TokenType != CssTokenType.OpenSquareBracket)
            {
                return false;
            }

            // Collect each piece of "[ name = value ]", any of which could be missing after the '['

            OpenBracket = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.SquareBracket);

            if (ItemName.IsAtItemName(tokens))
            {
                AttributeName = itemFactory.CreateSpecific<ItemName>(this);
                AttributeName.Parse(itemFactory, text, tokens);
                AttributeName.Context = CssClassifierContextCache.FromTypeEnum(CssClassifierContextType.ElementAttribute);
                Children.Add(AttributeName);
            }
            else
            {
                OpenBracket.AddParseError(ParseErrorType.AttributeSelectorElementMissing, ParseErrorLocation.AfterItem);
            }

            switch (tokens.CurrentToken.TokenType)
            {
                case CssTokenType.BeginsWith:       // ^=
                case CssTokenType.EndsWith:         // $=
                case CssTokenType.OneOf:            // ~=
                case CssTokenType.ListBeginsWith:   // |=
                case CssTokenType.ContainsString:   // *=
                case CssTokenType.Equals:           // =
                    Operation = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.SelectorOperator);
                    break;
            }

            if (tokens.CurrentToken.TokenType == CssTokenType.Identifier)
            {
                AttributeValue = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.ElementAttributeValue);
            }
            else if (tokens.CurrentToken.IsString())
            {
                AttributeValue = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.ElementAttributeValue);
            }

            if (AttributeValue != null && Operation == null)
            {
                AttributeValue.AddParseError(ParseErrorType.AttributeSelectorOperationMissing, ParseErrorLocation.BeforeItem);
            }
            else if (AttributeValue == null && Operation != null)
            {
                Operation.AddParseError(ParseErrorType.AttributeSelectorValueMissing, ParseErrorLocation.AfterItem);
            }

            if (AttributeValue != null && tokens.CurrentToken.TokenType == CssTokenType.Identifier)
            {
                OperationModifier = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.SelectorOperator);

                if (OperationModifier.Length != 1 || text[OperationModifier.Start] != 'i')
                {
                    OperationModifier.AddParseError(ParseErrorType.UnexpectedToken, ParseErrorLocation.WholeItem);
                }
            }

            if (tokens.CurrentToken.TokenType == CssTokenType.CloseSquareBracket)
            {
                CloseBracket = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.SquareBracket);
            }
            else
            {
                AddParseError(ParseErrorType.AttributeSelectorCloseBracketMissing, ParseErrorLocation.AfterItem);
            }

            return Children.Count > 0;
        }

        public override bool IsValid
        {
            get
            {
                return OpenBracket != null &&
                    CloseBracket != null &&
                    AttributeName != null &&
                    ((AttributeValue == null) == (Operation == null)) && // both must be set or unset
                    base.IsValid;
            }
        }
    }
}
