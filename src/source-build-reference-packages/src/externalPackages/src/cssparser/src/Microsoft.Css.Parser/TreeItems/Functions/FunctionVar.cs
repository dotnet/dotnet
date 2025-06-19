// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;

namespace Microsoft.Css.Parser.TreeItems.Functions
{
    /// <summary>
    /// CSS var() function
    /// var( <custom-property-name> [, <declaration-value> ]? )
    /// </summary>
    internal sealed class FunctionVar : Function
    {
        internal ParseItem CustomPropertyName
        {
            get
            {
                TokenItem customPropertyIdentifier = null;
                if (Arguments.Count > 0)
                {
                    if (Arguments[0] is FunctionArgument firstArg && firstArg.ArgumentItems.Count > 0)
                    {
                        customPropertyIdentifier = firstArg.ArgumentItems[0] as TokenItem;
                    }
                }

                return customPropertyIdentifier;
            }
        }

        public FunctionVar()
        {
        }

        public override bool Parse(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            bool result = base.Parse(itemFactory, text, tokens);

            if (CloseBrace != null)
            {
                CheckCustomPropertyName(text);
            }

            return result;
        }

        private void CheckCustomPropertyName(ITextProvider text)
        {
            if (CustomPropertyName == null)
            {
                CloseBrace.AddParseError(ParseErrorType.FunctionArgumentMissing, ParseErrorLocation.BeforeItem);
            }
            else
            {
                TokenItem customPropertyToken = CustomPropertyName as TokenItem;
                if (customPropertyToken == null || !text.GetText(customPropertyToken.Start, customPropertyToken.Length).StartsWith("--", StringComparison.Ordinal))
                {
                    CustomPropertyName.AddParseError(ParseErrorType.CustomPropertyNameExpected, ParseErrorLocation.WholeItem);
                }
            }
        }
    }
}
