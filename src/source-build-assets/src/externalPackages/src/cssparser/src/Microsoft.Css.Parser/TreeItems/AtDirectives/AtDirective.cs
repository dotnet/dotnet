// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Diagnostics;
using Microsoft.Css.Parser.Classify;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;

namespace Microsoft.Css.Parser.TreeItems.AtDirectives
{
    // http://www.w3.org/TR/CSS21/syndata.html#at-rules
    // "At-rules start with an at-Keyword, an '@' character followed immediately by an identifier (for example, '@import', '@page').
    // An at-rule consists of everything up to and including the next semicolon (;) or the next block, whichever comes first."
    //
    // http://www.w3.org/TR/CSS21/syndata.html#block
    // A block starts with a left curly brace ({) and ends with the matching right curly brace (}).
    // In between there may be any tokens, except that parentheses (( )), brackets ([ ]), and braces ({ })
    // must always occur in matching pairs and may be nested. Single (') and double quotes (") must also
    // occur in matching pairs, and characters between them are parsed as a string.
    //
    // effectively, if semicolon is missing and next comes selector block, selector will be considered
    // to be a part of the @ directive. For example '@import "foo.css" .a { color: red; }' is not a directive
    // followed by a selector, but rather a single import block. This, however, makes validation difficult.
    // In typical cases @charset is followed by charset name and @import specifies file name, so we'll
    // terminate parsing by EOL so we can parse following selector correctly and give validator an opportunity
    // to report that semicolon is missing. Basically each directive type requires a bit of custom
    // parsing since typical mistakes (and parser heuristics) are different. For example, not many
    // would write .a {...} on the same line after @import, in most cases selector will start at a new line.

    public abstract class AtDirective : ComplexItem
    {
        public TokenItem At { get; protected set; }
        public TokenItem Keyword { get; protected set; }

        protected AtDirective()
        {
        }

        /// <summary>
        /// Extensions can return false for directives that are known by the language
        /// and shouldn't be checked against the standard CSS schema.
        /// </summary>
        internal virtual bool IsStandardDirective
        {
            get { return true; }
        }

        // abstract factory
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        internal static ParseItem ParseDirective(ComplexItem parent, ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            Debug.Assert(tokens.CurrentToken.TokenType == CssTokenType.At);

            CssToken atToken = tokens.CurrentToken;
            CssToken nameToken = tokens.Peek(1);
            ParseItem pi = null;

            if (nameToken.TokenType == CssTokenType.At && AllowDoubleAt(text))
            {
                // Ignore the first @ in @@directive
                atToken = nameToken;
                nameToken = tokens.Peek(2);
            }

            if (nameToken.TokenType == CssTokenType.Identifier &&
                nameToken.Start == atToken.AfterEnd)
            {
                if ("charset".Length == nameToken.Length && text.CompareTo(nameToken.Start, "charset", ignoreCase: false))
                {
                    // must be lowercase, and not encoded
                    pi = itemFactory.Create<CharsetDirective>(parent);
                }
                else if ("import".Length == nameToken.Length && text.CompareTo(nameToken.Start, "import", ignoreCase: true))
                {
                    pi = itemFactory.Create<ImportDirective>(parent);
                }
                else if ("page".Length == nameToken.Length && text.CompareTo(nameToken.Start, "page", ignoreCase: true))
                {
                    pi = itemFactory.Create<PageDirective>(parent);
                }
                else if ("media".Length == nameToken.Length && text.CompareTo(nameToken.Start, "media", ignoreCase: true))
                {
                    pi = itemFactory.Create<MediaDirective>(parent);
                }
                else if ("namespace".Length == nameToken.Length && text.CompareTo(nameToken.Start, "namespace", ignoreCase: true))
                {
                    // CSS3 Namespaces
                    // http://www.w3.org/TR/css3-namespace/
                    pi = itemFactory.Create<NamespaceDirective>(parent);
                }
                else if (TextRange.CompareDecoded(nameToken.Start, nameToken.Length, text, "keyframes", ignoreCase: true) ||
                    TextRange.CompareDecoded(nameToken.Start, nameToken.Length, text, "-moz-keyframes", ignoreCase: true) ||
                    TextRange.CompareDecoded(nameToken.Start, nameToken.Length, text, "-ms-keyframes", ignoreCase: true) ||
                    TextRange.CompareDecoded(nameToken.Start, nameToken.Length, text, "-o-keyframes", ignoreCase: true) ||
                    TextRange.CompareDecoded(nameToken.Start, nameToken.Length, text, "-webkit-keyframes", ignoreCase: true))
                {
                    // CSS3 Animations
                    // http://www.w3.org/TR/2009/WD-css3-animations-20090320/
                    pi = itemFactory.Create<KeyFramesDirective>(parent);
                }
                else if (TextRange.CompareDecoded(nameToken.Start, nameToken.Length, text, "font-face", ignoreCase: true))
                {
                    // CSS3 Webfonts
                    // http://www.w3.org/TR/2002/WD-css3-webfonts-20020802/#font-descriptions
                    pi = itemFactory.Create<FontFaceDirective>(parent);
                }
                else if (TextRange.CompareDecoded(nameToken.Start, nameToken.Length, text, "counter", ignoreCase: true))
                {
                    pi = itemFactory.Create<CounterDirective>(parent);
                }
                else if (TextRange.CompareDecoded(nameToken.Start, nameToken.Length, text, "viewport", ignoreCase: true))
                {
                    pi = itemFactory.Create<ViewportDirective>(parent);
                }
            }

            if (pi == null)
            {
                // some other stuff, like @top-center/left/top/middle... or @footnoote in CSS3
                pi = itemFactory.Create<UnknownDirective>(parent);
            }

            pi.Parse(itemFactory, text, tokens);

            return pi;
        }

        /// <summary>
        /// Is @@directive allowed?
        /// </summary>
        private static bool AllowDoubleAt(ITextProvider text)
        {
            return text is ITextTypeProvider textTypes && (textTypes.TextTypes & TextTypes.Razor) != 0;
        }

        /// <summary>
        /// This must be called at the start of any @directive's Parse() function.
        /// It fills in the "At" and "Keyword" variables.
        /// "Keyword" could end up being null, but "At" is always set.
        /// </summary>
        protected virtual void ParseAtAndKeyword(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            Debug.Assert(tokens.CurrentToken.TokenType == CssTokenType.At);

            At = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.AtDirectiveName);

            if (tokens.CurrentToken.Start == At.AfterEnd &&
                tokens.CurrentToken.TokenType == CssTokenType.At &&
                AllowDoubleAt(text))
            {
                // Two @'s in a row (@@directive). Ignore the first one for Razor support.

                At = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.AtDirectiveName);
            }

            // Check for the directive name
            if (tokens.CurrentToken.Start == At.AfterEnd &&
                tokens.CurrentToken.TokenType == CssTokenType.Identifier)
            {
                Keyword = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.AtDirectiveName);
            }
            else
            {
                At.AddParseError(ParseErrorType.AtDirectiveNameMissing, ParseErrorLocation.AfterItem);
            }
        }
    }
}
