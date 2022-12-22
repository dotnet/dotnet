// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace NuGet.Packaging.Licenses
{
    internal class LicenseExpressionTokenizer
    {
        private readonly string _value;

        /// <summary>
        /// A tokenizer for a license expression.
        /// This implementation assumes that the input has been sanitized and that there are no invalid characters.
        /// </summary>
        /// <param name="value">value to be tokenized</param>
        /// <exception cref="ArgumentException">If the string is null or whitespace.</exception>
        internal LicenseExpressionTokenizer(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.ArgumentCannotBeNullOrEmpty, nameof(value)));
            }
            _value = value.Trim();
        }

        /// <summary>
        /// The valid characters for a license identifier are a-zA-Z0-9.-+
        /// The valid characters for a license expression are the above whitespace and ().
        /// </summary>
        /// <returns>Whether the value has valid characters.</returns>
        internal bool HasValidCharacters()
        {
            var regex = new Regex("^[a-zA-Z0-9\\.\\-\\s\\+\\(\\)]+$", RegexOptions.CultureInvariant);
            return regex.IsMatch(_value);
        }

        /// <summary>
        /// Given a string, tokenizes by space into operators and values. The considered operators are, AND, OR, WITH, (, and ). 
        /// </summary>
        /// <returns>tokens, <see cref="LicenseExpressionToken"/>/></returns>
        internal IEnumerable<LicenseExpressionToken> Tokenize()
        {
            var potentialTokens = _value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var token in potentialTokens)
            {
                var processingToken = token;

                IList<LicenseExpressionToken> tokensAfterValue = null;

                while (processingToken.Length > 0 && (processingToken[0] == '(' || processingToken[0] == ')'))
                {
                    yield return ParseBracket(processingToken[0]);

                    processingToken = processingToken.Substring(1);
                }

                while (processingToken.Length > 0 && (processingToken[processingToken.Length - 1] == '(' || processingToken[processingToken.Length - 1] == ')'))
                {
                    if (tokensAfterValue == null)
                    {
                        tokensAfterValue = new List<LicenseExpressionToken>();
                    }

                    tokensAfterValue.Add(ParseBracket(processingToken[processingToken.Length - 1]));
                    processingToken = processingToken.Substring(0, processingToken.Length - 1);
                }

                if (!string.IsNullOrEmpty(processingToken))
                {
                    yield return ParseTokenType(processingToken);
                }

                if (tokensAfterValue != null)
                {
                    foreach (var tokenAfterValue in tokensAfterValue)
                    {
                        yield return tokenAfterValue;
                    }
                }

            }
        }

        private LicenseExpressionToken ParseBracket(char bracket)
        {
            if (bracket == '(')
            {
                return new LicenseExpressionToken(bracket.ToString(CultureInfo.CurrentCulture), LicenseTokenType.OPENING_BRACKET);
            }
            if (bracket == ')')
            {
                return new LicenseExpressionToken(bracket.ToString(CultureInfo.CurrentCulture), LicenseTokenType.CLOSING_BRACKET);
            }
            return null;
        }

        /// <summary>
        /// Parses a token type given a string.
        /// This method assumes that the brackets have been parsed out. 
        /// </summary>
        /// <param name="token">The token</param>
        /// <returns>A parsed token, operator or value.</returns>
        /// <remarks>This method assumes the brackets have already been parsed.</remarks>
        private LicenseExpressionToken ParseTokenType(string token)
        {
            var expressionToken = Enum.TryParse(value: token, result: out LicenseTokenType result);

            if (expressionToken && result.IsOperator())
            {
                return new LicenseExpressionToken(token, result);
            }
            else // We already covered the brackets earlier, so it has to be a value.
            {
                return new LicenseExpressionToken(token, LicenseTokenType.IDENTIFIER);

            }
        }
    }
}
