// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.Tokens
{
    [TestClass]
    public class TokenizerTest : CssUnitTestBase
    {
        [TestMethod]
        public void TokenizerTest_EmptyString()
        {
            TokenList tokens = Helpers.MakeTokens(string.Empty);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(CssTokenType.EndOfFile, tokens[0].TokenType);
        }

        [TestMethod]
        public void TokenizerTest_AtDirective()
        {
            ITextProvider text = new StringTextProvider("@charset \"iso-1234\";");

            CssToken[] expected = new CssToken[]
            {
                new CssToken(CssTokenType.At, 0, 1),
                new CssToken(CssTokenType.Identifier, 1, 7),
                new CssToken(CssTokenType.String, 9, 10),
                new CssToken(CssTokenType.Semicolon, 19, 1),
                CssToken.EndOfFileToken(text)
            };

            TokenList actual = Helpers.MakeTokens(text);
            TokenizeFilesTest.CompareTokenArrays(expected, actual);
        }

        [TestMethod]
        public void IdentifierTokenizerTest_HyphenValid()
        {
            string[] inputs =
            {
                "-a",
                "-a9",
                "-Z-",
                "-a_",
                "-_",
                "-\u00A1",
                "-\\00A1",
                "-\\abc def",
            };

            RunTest(inputs, CssTokenType.Identifier);
        }

        [TestMethod]
        public void IdentifierTokenizerTest_Valid()
        {
            string[] inputs =
            {
                "_",
                "_Z",
                "_-a1",
                char.ConvertFromUtf32(0x10FFFF)
            };

            RunTest(inputs, CssTokenType.Identifier);
        }

        [TestMethod]
        public void AtKeywordTokenizerTest_Valid()
        {
            string[] inputs =
            {
                "@",
            };

            RunTest(inputs, CssTokenType.At);
        }

        [TestMethod]
        public void StringTokenizerTest_Valid()
        {
            string[] inputs =
            {
                "\"'\"",
                "'\"'",
                "\"!~ \\\\t \\\\n \\\\r\\n \\\\r \\\\f\"",
                "'!~ \\\\t \\\\n \\\\r\\n \\\\r \\\\f'"
            };

            RunTest(inputs, CssTokenType.String);
        }

        [TestMethod]
        public void StringTokenizerTest_Invalid()
        {
            string[] inputs =
            {
                "'~!\"",
                "\"~!'",
                "\"text",
                "'text",
            };

            RunTest(inputs, CssTokenType.InvalidString);
        }

        [TestMethod]
        public void HashNameTokenizerTest_Valid()
        {
            string[] inputs =
            {
                "#_",
                "#0",
                "#a9",
                "#-A-Z",
            };

            RunTest(inputs, CssTokenType.HashName);
        }

        [TestMethod]
        public void NumberTokenizerTest_Valid()
        {
            string[] inputs =
            {
                "0",
                "9.0",
                "-0.9",
            };

            RunTest(inputs, CssTokenType.Number);
        }

        [TestMethod]
        public void SelectorMatchTokenizerTest()
        {
            // Create more complex tests that input strings containing this
            string input;

            input = "~=";
            RunTest(input, CreateToken(input, CssTokenType.OneOf));

            input = "|=";
            RunTest(input, CreateToken(input, CssTokenType.ListBeginsWith));

            input = "^=";
            RunTest(input, CreateToken(input, CssTokenType.BeginsWith));

            input = "$=";
            RunTest(input, CreateToken(input, CssTokenType.EndsWith));

            input = "*=";
            RunTest(input, CreateToken(input, CssTokenType.ContainsString));

            input = "||";
            RunTest(input, CreateToken(input, CssTokenType.DoublePipe));
        }

        [TestMethod]
        public void BOMTokenizerTest()
        {
            // The BOM isn't treated specially, so it ends up as an identifier
            string input = "\uFEFE";
            RunTest(input, CreateToken(input, CssTokenType.Identifier));
        }

        [TestMethod]
        public void UnicodeRangeTest()
        {
            string[] inputs =
            {
                "u+012345-6789ab",
                "U+?",
                "U+F",
                "U+??00??",
                "U+??-00",
                "u+?0-abcdef",
            };

            RunTest(inputs, CssTokenType.UnicodeRange);
        }

        [TestMethod]
        public void TokenizerTest_EncodedUrl()
        {
            CssTokenizer tokenizer = new CssTokenizer();

            // Base case: No encoding:
            ITextProvider text = new StringTextProvider(@"url('foo.jpg')");
            TokenList actual = Helpers.MakeTokens(text);

            TokenizeFilesTest.CompareTokenArrays(
                new CssToken[]
                {
                    new CssToken(CssTokenType.Url, 0, 4),
                    new CssToken(CssTokenType.String, 4, 9),
                    new CssToken(CssTokenType.CloseFunctionBrace, 13, 1),
                    CssToken.EndOfFileToken(text)
                },
                actual);

#if SUPPORT_ENCODED_CSS
            // Escape characters:
            text = new StringTextProvider(@"\u\r\l('foo.jpg')");
            actual = Helpers.MakeTokens(text.GetText(0, text.Length));

            TokenizeFilesTest.CompareTokenArrays(
                new CssToken[]
                {
                    new CssToken(CssTokenType.Url, 0, 7),
                    new CssToken(CssTokenType.String, 7, 9),
                    new CssToken(CssTokenType.CloseFunctionBrace, 16, 1),
                    CssToken.EndOfFileToken(text)
                },
                actual);

            // Unicode encode and escape characters:
            text = new StringTextProvider(@"u\52 \l(foo)");
            actual = Helpers.MakeTokens(text.GetText(0, text.Length));

            TokenizeFilesTest.CompareTokenArrays(
                new CssToken[]
                {
                    new CssToken(CssTokenType.Url, 0, 8),
                    new CssToken(CssTokenType.UnquotedUrlString, 8, 3),
                    new CssToken(CssTokenType.CloseFunctionBrace, 11, 1),
                    CssToken.EndOfFileToken(text)
                },
                actual);
#endif
        }

        private void RunTest(string[] inputs, CssTokenType tokenType)
        {
            foreach (string input in inputs)
            {
                RunTest(input, CreateToken(input, tokenType));
            }
        }

        private void RunTest(string input, CssToken token)
        {
            List<CssToken> list = new List<CssToken>() { token };
            RunTest(input, list);
        }

        private void RunTest(string input, IList<CssToken> expectedTokens)
        {
            expectedTokens.Add(CssToken.EndOfFileToken(new StringTextProvider(input)));
            VerifyTokens(input, expectedTokens);
        }

        private CssToken CreateToken(string input, CssTokenType tokenType)
        {
            CssToken token = new CssToken(tokenType, 0, input.Length);
#if DEBUG
            token.DebugText = input;
#endif
            return token;
        }

        private static void VerifyTokens(string input, IList<CssToken> expectedTokens)
        {
            TokenList actualTokens = Helpers.MakeTokens(input);

            Assert.AreEqual(actualTokens.Count, expectedTokens.Count, "Verify number of tokens");

            for (int i = 0; i < expectedTokens.Count; i++)
            {
                VerifyToken(actualTokens[i], expectedTokens[i]);
            }
        }

        private static void VerifyToken(CssToken actual, CssToken expected)
        {
#if DEBUG
            if (!string.IsNullOrEmpty(expected.DebugText))
            {
                Assert.AreEqual(actual.DebugText, expected.DebugText);
            }
#endif
            Assert.AreEqual(expected.TokenType, actual.TokenType);
            Assert.AreEqual(expected.Start, actual.Start);
            Assert.AreEqual(expected.Length, actual.Length);
        }
    }
}
