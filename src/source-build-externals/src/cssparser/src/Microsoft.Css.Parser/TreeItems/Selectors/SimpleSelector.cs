// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Classify;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;

namespace Microsoft.Css.Parser.TreeItems.Selectors
{
    // http://www.w3.org/TR/css3-selectors/#simple-selectors
    //
    // simple_selector_sequence : [ type_selector | universal ]
    //              [ HASH | class | attrib | pseudo | negation ]* | [ HASH | class | attrib | pseudo | negation ]+
    // type_selector : [ namespace_prefix ]? element_name
    // namespace_prefix : [ IDENT | '*' ]? '|'
    // element_name : IDENT
    // universal : [ namespace_prefix ]? '*'

    /// <summary>
    /// CSS simple selector where simple_selector_sequence : [ type_selector | universal ]
    /// [ HASH | class | attrib | pseudo | negation ]* | [ HASH | class | attrib | pseudo | negation ]+
    /// </summary>
    public class SimpleSelector : ComplexItem
    {
        public ParseItemList SubSelectors { get; private set; }
        public ItemName Name { get; protected set; } // foo|bar, *|, *, ...,
        public ParseItem SelectorCombineOperator { get; protected set; } // +, >, ~, /id/

        public SimpleSelector()
        {
            SubSelectors = new ParseItemList();
        }

        protected virtual bool IsAtSelectorTerminator(TokenStream tokens)
        {
            return tokens.CurrentToken.IsSelectorTerminator();
        }

        protected virtual bool IsAtSelectorCombineOperator(ITextProvider text, TokenStream tokens)
        {
            return tokens.CurrentToken.IsSimpleSelectorCombineOperator() ||
                IdReferenceOperator.IsAtIdReferenceOperator(tokens);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public override bool Parse(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            bool hasSubject = false;
            // Allow a bang before the tag name.
            while (tokens.CurrentToken.TokenType == CssTokenType.Bang)
            {
                ParseItem item = itemFactory.Create<SubjectSelector>(this);
                if (item.Parse(itemFactory, text, tokens))
                {
                    Children.Add(item);

                    if (hasSubject)
                    {
                        item.AddParseError(ParseErrorType.UnexpectedBangInSelector, ParseErrorLocation.WholeItem);
                    }

                    hasSubject = true;
                }

                if (tokens.IsWhiteSpaceBeforeCurrentToken())
                {
                    break;
                }
            }

            if (ItemName.IsAtItemName(tokens))
            {
                ItemName name = itemFactory.CreateSpecific<ItemName>(this);
                if (name.Parse(itemFactory, text, tokens))
                {
                    name.Context = CssClassifierContextCache.FromTypeEnum(CssClassifierContextType.ElementTagName);
                    Name = name;
                    Children.Add(name);
                }
            }

            ParseItem lastItem = (Children.Count > 0) ? Children[Children.Count - 1] : null;

            // Only continue parsing the simple selector if the name touches the next token.
            if (lastItem == null || lastItem.AfterEnd == tokens.CurrentToken.Start)
            {
                bool addedErrorItem = false;

                while (!IsAtSelectorTerminator(tokens))
                {
                    ParseItem pi = CreateNextAtomicPart(itemFactory, text, tokens);
                    if (pi == null)
                    {
                        // Only treat the first bad token as an error (don't need more than one in a row)

                        if (addedErrorItem)
                        {
                            pi = UnknownItem.ParseUnknown(this, itemFactory, text, tokens);
                        }
                        else
                        {
                            pi = UnknownItem.ParseUnknown(this, itemFactory, text, tokens, ParseErrorType.SimpleSelectorExpected);
                            addedErrorItem = true;
                        }
                    }
                    else if (!pi.Parse(itemFactory, text, tokens))
                    {
                        break;
                    }

                    if (pi is SubjectSelector)
                    {
                        if (hasSubject)
                        {
                            pi.AddParseError(ParseErrorType.UnexpectedBangInSelector, ParseErrorLocation.WholeItem);
                        }
                        hasSubject = true;
                    }

                    SubSelectors.Add(pi);
                    Children.Add(pi);

                    if (tokens.IsWhiteSpaceBeforeCurrentToken())
                    {
                        break;
                    }
                }
            }

            if (IsAtSelectorCombineOperator(text, tokens))
            {
                ParseSelectorCombineOperator(itemFactory, text, tokens);

                if (Name == null &&
                    SelectorCombineOperator != null &
                    SubSelectors.Count == 0 &&
                    !CanStartWithCombineOperator(SelectorCombineOperator))
                {
                    SelectorCombineOperator.AddParseError(ParseErrorType.SelectorBeforeCombineOperatorMissing, ParseErrorLocation.BeforeItem);
                }
            }

            return Children.Count > 0;
        }

        protected virtual void ParseSelectorCombineOperator(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            if (IdReferenceOperator.IsAtIdReferenceOperator(tokens))
            {
                ParseItem combineOperator = itemFactory.Create<IdReferenceOperator>(this);
                if (combineOperator.Parse(itemFactory, text, tokens))
                {
                    SelectorCombineOperator = combineOperator;
                    Children.Add(combineOperator);
                }
            }
            else
            {
                SelectorCombineOperator = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.SelectorCombineOperator);
            }
        }

        protected virtual bool CanStartWithCombineOperator(ParseItem combineOperator)
        {
            return false;
        }

        // :not(:visited)
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        internal virtual bool ParseInFunction(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            if (ItemName.IsAtItemName(tokens))
            {
                ItemName name = itemFactory.CreateSpecific<ItemName>(this);
                if (name.Parse(itemFactory, text, tokens))
                {
                    name.Context = CssClassifierContextCache.FromTypeEnum(CssClassifierContextType.ElementTagName);
                    Name = name;
                    Children.Add(name);
                }
            }

            if (Name == null || Name.AfterEnd == tokens.CurrentToken.Start)
            {
                while (!IsAtSelectorTerminator(tokens) && tokens.CurrentToken.TokenType != CssTokenType.CloseFunctionBrace)
                {
                    ParseItem childItem = CreateNextAtomicPart(itemFactory, text, tokens);
                    if (childItem == null || !childItem.Parse(itemFactory, text, tokens))
                    {
                        childItem = UnknownItem.ParseUnknown(this, itemFactory, text, tokens, ParseErrorType.PseudoFunctionSelectorExpected);
                    }
                    else
                    {
                        SubSelectors.Add(childItem);
                    }

                    Children.Add(childItem);

                    if (tokens.IsWhiteSpaceBeforeCurrentToken())
                    {
                        break;
                    }
                }
            }

            return Children.Count > 0;
        }

        /// <summary>
        /// Creates an unparsed ParseItem for the next atomic part of the selector
        /// (class, ID, attribute, or pseudo-class)
        /// </summary>
        protected virtual ParseItem CreateNextAtomicPart(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            ParseItem pi = null;

            switch (tokens.CurrentToken.TokenType)
            {
                case CssTokenType.Dot:
                    pi = itemFactory.Create<ClassSelector>(this);
                    break;

                case CssTokenType.HashName:
                case CssTokenType.Hash: // An invalid ID, but let the IdSelector class create the parse error
                    pi = itemFactory.Create<IdSelector>(this);
                    break;

                case CssTokenType.OpenSquareBracket:
                    pi = itemFactory.Create<AttributeSelector>(this);
                    break;

                case CssTokenType.Colon:
                    switch (tokens.Peek(1).TokenType)
                    {
                        case CssTokenType.Function:
                            pi = itemFactory.Create<PseudoClassFunctionSelector>(this); // :nth-child(n)
                            break;

                        default:
                        case CssTokenType.Identifier:
                            pi = itemFactory.Create<PseudoClassSelector>(this); // :first-child
                            break;
                    }
                    break;

                case CssTokenType.DoubleColon: // CSS3 syntax
                    switch (tokens.Peek(1).TokenType)
                    {
                        case CssTokenType.Function:
                            pi = itemFactory.Create<PseudoElementFunctionSelector>(this); // ::slot(b)
                            break;

                        default:
                        case CssTokenType.Identifier:
                            pi = itemFactory.Create<PseudoElementSelector>(this); // ::after
                            break;
                    }
                    break;

                case CssTokenType.Bang:
                    pi = itemFactory.Create<SubjectSelector>(this); // E!
                    break;
            }

            return pi;
        }

        public override bool IsValid
        {
            get
            {
                foreach (ParseItem child in Children)
                {
                    if (!IsValidChild(child))
                    {
                        return false;
                    }
                }

                return base.IsValid;
            }
        }

        protected virtual bool IsValidChild(ParseItem child)
        {
            if (child == SelectorCombineOperator ||
                child is ItemName ||
                child is ClassSelector ||
                child is IdSelector ||
                child is AttributeSelector ||
                child is PseudoClassSelector ||
                child is PseudoClassFunctionSelector ||
                child is PseudoElementSelector ||
                child is PseudoElementFunctionSelector ||
                child is SubjectSelector)
            {
                if (child.IsValid)
                {
                    // an expected child
                    return true;
                }
            }

            return false;
        }
    }
}
