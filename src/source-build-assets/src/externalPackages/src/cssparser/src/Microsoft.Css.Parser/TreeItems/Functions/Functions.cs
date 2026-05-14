// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.Css.Parser.Classify;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;

namespace Microsoft.Css.Parser.TreeItems.Functions
{
    /// <summary>
    /// CSS function, like rgb(), rgba(), hsl(), counter(), local(), etc
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", Justification = "Public API")]
    public class Function : ComplexItem
    {
        /// <summary>
        /// List of function arguments
        /// </summary>
        internal ParseItemList Arguments { get; private set; }

        /// <summary>
        /// Function name
        /// </summary>
        public TokenItem FunctionName { get; private set; }

        /// <summary>
        /// Token of the closing brace
        /// </summary>
        internal TokenItem CloseBrace { get; private set; }

        public Function()
        {
            Arguments = new ParseItemList();
            Context = CssClassifierContextCache.FromTypeEnum(CssClassifierContextType.FunctionName);
        }

        internal override bool IsUnclosed
        {
            get { return CloseBrace == null; }
        }

        // functional_pseudo : FUNCTION S* expression ')'
        // expression :
        //  In CSS3, the expressions are identifiers, strings, or of the form "an+b"
        // : [ [ PLUS | '-' | DIMENSION | NUMBER | STRING | IDENT ] S* ]+
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        internal static Function ParseFunction(ComplexItem parent, ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            Function fn;

            if (TextRange.CompareDecoded(tokens.CurrentToken.Start, tokens.CurrentToken.Length, text, "rgb(", ignoreCase: true))
            {
                fn = itemFactory.CreateSpecific<FunctionColor>(parent);
                ((FunctionColor)fn).ColorFunction = ColorFunctionType.Rgb;
            }
            else if (TextRange.CompareDecoded(tokens.CurrentToken.Start, tokens.CurrentToken.Length, text, "rgba(", ignoreCase: true))
            {
                fn = itemFactory.CreateSpecific<FunctionColor>(parent);
                ((FunctionColor)fn).ColorFunction = ColorFunctionType.Rgba;
            }
            else if (TextRange.CompareDecoded(tokens.CurrentToken.Start, tokens.CurrentToken.Length, text, "hsl(", ignoreCase: true))
            {
                fn = itemFactory.CreateSpecific<FunctionColor>(parent);
                ((FunctionColor)fn).ColorFunction = ColorFunctionType.Hsl;
            }
            else if (TextRange.CompareDecoded(tokens.CurrentToken.Start, tokens.CurrentToken.Length, text, "hsla(", ignoreCase: true))
            {
                fn = itemFactory.CreateSpecific<FunctionColor>(parent);
                ((FunctionColor)fn).ColorFunction = ColorFunctionType.Hsla;
            }
            else if (TextRange.CompareDecoded(tokens.CurrentToken.Start, tokens.CurrentToken.Length, text, "attr(", ignoreCase: true))
            {
                fn = itemFactory.CreateSpecific<FunctionAttr>(parent);
            }
            else if (TextRange.CompareDecoded(tokens.CurrentToken.Start, tokens.CurrentToken.Length, text, "calc(", ignoreCase: true))
            {
                fn = itemFactory.CreateSpecific<FunctionCalc>(parent);
            }
            else if (TextRange.CompareDecoded(tokens.CurrentToken.Start, tokens.CurrentToken.Length, text, "counter(", ignoreCase: true))
            {
                fn = itemFactory.CreateSpecific<FunctionCounter>(parent);
            }
            else if (TextRange.CompareDecoded(tokens.CurrentToken.Start, tokens.CurrentToken.Length, text, "expression(", ignoreCase: true))
            {
                fn = itemFactory.CreateSpecific<FunctionExpression>(parent);
            }
            else if (TextRange.CompareDecoded(tokens.CurrentToken.Start, tokens.CurrentToken.Length, text, "format(", ignoreCase: true))
            {
                fn = itemFactory.CreateSpecific<FunctionFormat>(parent);
            }
            else if (TextRange.CompareDecoded(tokens.CurrentToken.Start, tokens.CurrentToken.Length, text, "local(", ignoreCase: true))
            {
                fn = itemFactory.CreateSpecific<FunctionLocal>(parent);
            }
            else if (TextRange.CompareDecoded(tokens.CurrentToken.Start, tokens.CurrentToken.Length, text, "var(", ignoreCase: true))
            {
                fn = itemFactory.CreateSpecific<FunctionVar>(parent);
            }
            else
            {
                fn = itemFactory.CreateSpecific<Function>(parent);
            }

            if (fn != null && !fn.Parse(itemFactory, text, tokens))
            {
                fn = null;
            }

            return fn;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public override bool Parse(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            FunctionName = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.FunctionName);
            ParseArguments(itemFactory, text, tokens);
            CheckCloseFunctionBrace(tokens);

            return Children.Count > 0;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        protected virtual bool ParseArguments(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            if (tokens.CurrentToken.IsFunctionTerminator())
            {
                // There could be functions without arguments (like "filter: mask();")
                return false;
            }

            while (!tokens.CurrentToken.IsFunctionTerminator())
            {
                ParseItem fa = CreateArgumentObject(this, itemFactory, Arguments.Count);

                if (fa.Parse(itemFactory, text, tokens))
                {
                    fa.Context = GetArgumentContext(Arguments.Count);
                    Children.Add(fa);
                    Arguments.Add(fa);
                }
                else
                {
                    // Don't know what this is
                    Children.AddUnknownAndAdvance(itemFactory, text, tokens, ParseErrorType.UnexpectedToken);
                }
            }

            FunctionArgument lastArgument = (Arguments.Count > 0) ? Arguments[Arguments.Count - 1] as FunctionArgument : null;

            if (lastArgument != null && lastArgument.Comma != null && lastArgument == Children[Children.Count - 1])
            {
                Children.AddParseError(ParseErrorType.FunctionArgumentMissing);
            }

            return true;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        protected bool CheckCloseFunctionBrace(TokenStream tokens)
        {
            if (tokens.CurrentToken.TokenType == CssTokenType.CloseFunctionBrace)
            {
                CloseBrace = Children.AddCurrentAndAdvance(tokens, null);
            }
            else
            {
                FunctionName.AddParseError(ParseErrorType.CloseFunctionBraceMissing, ParseErrorLocation.AfterItem);
            }

            return CloseBrace != null;
        }

        protected virtual ParseItem CreateArgumentObject(ComplexItem parent, ItemFactory itemFactory, int argumentNumber)
        {
            return itemFactory.Create<FunctionArgument>(this);
        }

        protected virtual IClassifierContext GetArgumentContext(int argumentNumber)
        {
            return CssClassifierContextCache.FromTypeEnum(CssClassifierContextType.FunctionArgument);
        }
    }
}
