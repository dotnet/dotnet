// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NuGet.Packaging.Licenses.Test
{
    public class LicenseExpressionTokenizerTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]

        public void LicenseExpressionTokenizer_ThrowsForInvalidLicenseStrings(string value)
        {
            Assert.Throws<ArgumentException>(() => new LicenseExpressionTokenizer(value));
        }

        [Theory]
        [InlineData("MIT")]
        [InlineData("value with invalid characters _ * ! ? :)")]
        public void LicenseExpressionTokenizer_DoesNotThrowForNonEmptyStrings(string value)
        {
            var tokenizer = new LicenseExpressionTokenizer(value);
        }

        [Theory]
        [InlineData("MIT", "IDENTIFIER")]
        [InlineData("(", "OPENING_BRACKET")]
        [InlineData(")", "CLOSING_BRACKET")]
        [InlineData("AND", "AND")]
        [InlineData("OR", "OR")]
        [InlineData("WITH", "WITH")]
        [InlineData("with", "IDENTIFIER")]
        [InlineData("Or", "IDENTIFIER")]
        [InlineData("aND", "IDENTIFIER")]
        public void LicenseExpressionTokenizer_TokenizesSimpleExpressionCorrectly(string value, string type)
        {
            Enum.TryParse(type, out LicenseTokenType tokenType);

            var tokenizer = new LicenseExpressionTokenizer(value);

            var tokens = tokenizer.Tokenize().ToArray();
            Assert.Equal(1, tokens.Length);
            var token = tokens[0];
            Assert.Equal(value, token.Value);
            Assert.Equal(tokenType, token.TokenType);
        }

        [Theory]
        [MemberData(nameof(ComplexSPDXExpressionData))]
        public void LicenseExpressionTokenizer_TokenizesComplexExpressions(string[] values)
        {
            var expression = values[0];
            var tokenizer = new LicenseExpressionTokenizer(expression);
            var tokens = tokenizer.Tokenize().ToArray();
            Assert.Equal(values.Length - 1, tokens.Length);
            for (var i = 0; i < tokens.Length; i++)
            {
                Assert.Equal(tokens[i].Value, values[i + 1]);
            }
        }

        public static IEnumerable<object[]> ComplexSPDXExpressionData()
        {
            yield return new object[] { new string[] { "MIT                or              LPL-1.0                 ", "MIT", "or", "LPL-1.0" } };
            yield return new object[] { new string[] { "MIT or LPL-1.0", "MIT", "or", "LPL-1.0" } };
            yield return new object[] { new string[] { "MIT and LPL-1.0", "MIT", "and", "LPL-1.0" } };
            yield return new object[] { new string[] { "MIT AND LPL-1.0", "MIT", "AND", "LPL-1.0" } };
            yield return new object[] { new string[] { "MIT WITH LPL-1.0", "MIT", "WITH", "LPL-1.0" } };
            yield return new object[] { new string[] { "MIT OR (GPL-1.0 WITH E8)", "MIT", "OR", "(", "GPL-1.0", "WITH", "E8", ")" } };
            yield return new object[] { new string[] { "MIT OR ( GPL-1.0 WITH E8 )", "MIT", "OR", "(", "GPL-1.0", "WITH", "E8", ")" } };
            yield return new object[] { new string[] { "MIT OR ((GPL-1.0 WITH E8) AND APACHE-2.0)", "MIT", "OR", "(", "(", "GPL-1.0", "WITH", "E8", ")", "AND", "APACHE-2.0", ")" } };
            yield return new object[] { new string[] { "MIT )OR LPL-1.0", "MIT", ")", "OR", "LPL-1.0" } };
            yield return new object[] { new string[] { "MIT OR( LPL-1.0", "MIT", "OR", "(", "LPL-1.0" } };
            yield return new object[] { new string[] { "(())", "(", "(", ")", ")" } };
            yield return new object[] { new string[] { "((", "(", "(" } };
            yield return new object[] { new string[] { "))", ")", ")" } };
            yield return new object[] { new string[] { ")(", ")", "(" } };
        }

        [Theory]
        [MemberData(nameof(ComplexSPDXExpressionWithInvalidGrammarData))]
        public void LicenseExpressionTokenizer_TokenizesComplexButInvalidSPDXGrammarExpressions(string[] values)
        {
            var expression = values[0];
            var tokenizer = new LicenseExpressionTokenizer(expression);
            var tokens = tokenizer.Tokenize().ToArray();
            Assert.Equal(values.Length - 1, tokens.Length);
            for (var i = 0; i < tokens.Length; i++)
            {
                Assert.Equal(tokens[i].Value, values[i + 1]);
            }
        }

        public static IEnumerable<object[]> ComplexSPDXExpressionWithInvalidGrammarData()
        {
            yield return new object[] { new string[] { "          MIT         AND OR               LPL-1.0           ", "MIT", "AND", "OR", "LPL-1.0" } };
            yield return new object[] { new string[] { "MIT AND OR LPL-1.0", "MIT", "AND", "OR", "LPL-1.0" } };
            yield return new object[] { new string[] { "MIT and LPL-1.0", "MIT", "and", "LPL-1.0" } };
            yield return new object[] { new string[] { "MIT () AND OR LPL-1.0", "MIT", "(", ")", "AND", "OR", "LPL-1.0" } };
        }

        [Theory]
        [InlineData("(GPL-1.0+ WITH E8) OR MIT!", false)]
        [InlineData("(GPL-1.0+ WITH E8) OR MIT@", false)]
        [InlineData("(GPL-1.0+ WITH E8) OR MIT/", false)]
        [InlineData("(GPL-1.0+ WITH E8) OR MIT[]", false)]
        [InlineData("(GPL-1.0+ WITH E8) OR MIT", true)]
        [InlineData("MIT", true)]
        [InlineData("(MIT)", true)]
        [InlineData("MIT+", true)]
        [InlineData("MIT-1.0", true)]
        [InlineData("GPL-1.0+ WITH E8 OR MIT", true)]
        [InlineData("(", true)]
        [InlineData(")", true)]
        [InlineData("+-.)(dklamkdsa dkajsmdkjasn md asdna s", true)]

        public void LicenseExpressionTokenizer_ReturnsFalseForExpressionsWithInvalidCharacters(string infix, bool match)
        {
            var tokenizer = new LicenseExpressionTokenizer(infix);
            Assert.Equal(match, tokenizer.HasValidCharacters());
        }
    }
}
