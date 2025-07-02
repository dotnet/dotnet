// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Classify;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems.PropertyValues;

namespace Microsoft.Css.Parser.TreeItems
{
    // declaration  : property ':' S* expr prio?
    // prio : IMPORTANT_SYM S*
    // expr : term [ operator? term ]*
    //
    // term : unary_operator? [ NUMBER S* | PERCENTAGE S* | LENGTH S* | EMS S* | EXS S* | ANGLE S* | TIME S* | FREQ S* ]
    // | STRING S* | IDENT S* | URI S* | hexcolor | function
    //
    // operator : '/' S* | ',' S*
    // unary_operator  : '-' | '+'
    // function : FUNCTION S* expr ')' S*
    //
    // "!"({w}|{comment})*{I}{M}{P}{O}{R}{T}{A}{N}{T} {return IMPORTANT_SYM;}

    /// <summary>
    /// CSS declaration, i.g. property name : property value; sequence
    /// </summary>
    public class Declaration : ComplexItem
    {
        public ParseItemList Values { get; private set; }
        public ParseItem PropertyName { get; private set; }
        public TokenItem Colon { get; private set; }
        internal TokenItem Bang { get; private set; }
        public TokenItem Important { get; private set; }
        public TokenItem Semicolon { get; private set; }

        public Declaration()
        {
            Context = CssClassifierContextCache.FromTypeEnum(CssClassifierContextType.PropertyDeclaration);
            Values = new ParseItemList();
        }

        /// <summary>
        /// True if declaration is not terminated by semicolon
        /// </summary>
        internal override bool IsUnclosed
        {
            get { return Semicolon == null; }
        }

        internal virtual bool AllowSemicolon
        {
            get { return true; }
        }

        /// <summary>
        /// Allows derived classes to decide if property names are valid CSS names
        /// (and therefore can be looked up in the schema)
        /// </summary>
        internal virtual bool PropertyNameIsValid
        {
            get { return PropertyName != null ? PropertyName.IsValid : false; }
        }

        /// <summary>
        /// Allows derived classes to map custom syntax to valid CSS property names
        /// </summary>
        public virtual string PropertyNameText
        {
            get { return PropertyName != null ? PropertyName.Text : string.Empty; }
        }

        /// <summary>
        /// Returns true if PropertyNameText is a standard CSS property name that can be
        /// looked up in the schema.
        /// </summary>
        internal virtual bool PropertyNameTextIsStandard
        {
            get { return !IsCustomProperty; }
        }

        /// <summary>
        /// Returns true if the PropertyNameText starts with '--', marking a custom property.
        /// See http://www.w3.org/TR/css-variables-1/ for more details
        /// </summary>
        internal bool IsCustomProperty { get; private set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public override bool Parse(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            // foo : bar rgb(1,2,3) "baz" url(quux) / moo goo ! important ;
            // First item is property name. Allow -webkit and *foo - the latter is used for 'commenting out'

            PropertyName = ParseName(itemFactory, text, tokens);

            if (tokens.CurrentToken.TokenType == CssTokenType.Colon ||
                tokens.CurrentToken.TokenType == CssTokenType.DoubleColon)
            {
                Colon = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.Punctuation);

                if (Colon.TokenType != CssTokenType.Colon)
                {
                    Colon.AddParseError(ParseErrorType.UnexpectedToken, ParseErrorLocation.WholeItem);
                }
            }

            // Parse values
            while (KeepParsingValues(tokens))
            {
                switch (tokens.CurrentToken.TokenType)
                {
                    case CssTokenType.Bang:
                        ParseBang(itemFactory, text, tokens);
                        break;

                    default:
                        ParseDefaultChild(itemFactory, text, tokens);
                        break;
                }
            }

            ParseAfterValues(itemFactory, text, tokens);

            // Add appropriate errors

            if (PropertyName == null)
            {
                AddParseError(ParseErrorType.PropertyNameMissing, ParseErrorLocation.BeforeItem);
            }
            else if (Colon != null)
            {
                int semiColonIndex = (Semicolon != null) ? Children.IndexOf(Semicolon) : Children.Count;

                if (Children.IndexOf(Colon) + 1 == semiColonIndex)
                {
                    Colon.AddParseError(ParseErrorType.PropertyValueMissing, ParseErrorLocation.AfterItem);
                }
            }
            else
            {
                AddColonMissingError();
            }

            return Children.Count > 0;
        }

        private bool KeepParsingValues(TokenStream tokens)
        {
            // For custom propeties, we should allow a block item (with curly braces) in the value
            return !tokens.CurrentToken.IsDeclarationTerminator()
                || (IsCustomProperty && tokens.CurrentToken.TokenType == CssTokenType.OpenCurlyBrace);
        }

        protected virtual void ParseDefaultChild(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            if (Colon != null)
            {
                ParsePropertyValue(itemFactory, text, tokens);
            }
            else
            {
                // Not an error because the missing colon is the real error
                Children.AddUnknownAndAdvance(itemFactory, text, tokens);
            }
        }

        protected virtual void ParsePropertyValue(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            ParseItem item = PropertyValueHelpers.ParsePropertyValue(this, itemFactory, text, tokens);
            if (item != null)
            {
                Values.Add(item);
                Children.Add(item);
            }
            else
            {
                Children.AddUnknownAndAdvance(itemFactory, text, tokens, ParseErrorType.PropertyValueExpected);
            }
        }

        protected virtual void ParseBang(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            if (Bang == null)
            {
                Bang = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.Important);

                if (TextRange.CompareDecoded(tokens.CurrentToken.Start, tokens.CurrentToken.Length, text, "important", ignoreCase: true))
                {
                    Important = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.Important);
                }
                else
                {
                    Children.AddParseError(ParseErrorType.ImportantMissing);
                }
            }
            else
            {
                Children.AddUnknownAndAdvance(itemFactory, text, tokens, ParseErrorType.UnexpectedBangInProperty);
            }
        }

        protected virtual ParseItem ParseName(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            ParseItem name = null;

            if (tokens.CurrentToken.TokenType == CssTokenType.Asterisk)
            {
                StarHackPropertyName customName = new StarHackPropertyName();
                if (customName.Parse(itemFactory, text, tokens))
                {
                    name = customName;
                    Children.Add(customName);
                }
            }
            else if (tokens.CurrentToken.TokenType == CssTokenType.Identifier)
            {
                CssClassifierContextType contextType = CssClassifierContextType.PropertyName;
                if (text.CompareTo(tokens.CurrentToken.Start, "--", true))
                {
                    IsCustomProperty = true;
                    contextType = CssClassifierContextType.CustomPropertyName;
                }

                name = Children.AddCurrentAndAdvance(tokens, contextType);
            }

            return name;
        }

        protected virtual void ParseAfterValues(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            if (tokens.CurrentToken.TokenType == CssTokenType.Semicolon)
            {
                Semicolon = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.Punctuation);
            }
        }

        /// <summary>
        /// Add the error for missing colon.  In extension languages, this may be placed differently.
        /// </summary>
        protected virtual void AddColonMissingError()
        {
            PropertyName.AddParseError(ParseErrorType.ColonMissingInDeclaration, ParseErrorLocation.AfterItem);
        }
    }
}
