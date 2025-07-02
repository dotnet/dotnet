// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.Tokens
{
    [TestClass]
    public class TokenStreamTest : CssUnitTestBase
    {
        [TestMethod]
        public void TokenStream_Simple()
        {
            TokenStream iter = Helpers.MakeTokenStream(".foo");
            Assert.AreEqual(CssTokenType.Dot, iter.CurrentToken.TokenType);
            Assert.AreEqual(CssTokenType.Identifier, iter.Peek(1).TokenType);
            Assert.AreEqual(CssTokenType.EndOfFile, iter.Peek(2).TokenType);

            Assert.AreEqual(0, iter.Position);
            Assert.AreEqual(3, iter.Tokens.Count);

            Assert.AreEqual(iter.Tokens[0], iter.Advance(1));
            Assert.AreEqual(1, iter.Position);
            Assert.AreEqual(CssTokenType.Identifier, iter.CurrentToken.TokenType);

            iter.Advance(-1);
            Assert.AreEqual(0, iter.Position);

            iter.Advance(2);
            Assert.AreEqual(CssTokenType.EndOfFile, iter.CurrentToken.TokenType);
        }

        [TestMethod]
        public void TokenStream_Whitespace()
        {
            TokenStream iter = Helpers.MakeTokenStream("* .foo");

            Assert.IsFalse(iter.IsWhiteSpaceBeforeCurrentToken());
            Assert.IsTrue(iter.IsWhiteSpaceAfterCurrentToken());

            iter.Advance(1);
            Assert.IsTrue(iter.IsWhiteSpaceBeforeCurrentToken());
            Assert.IsFalse(iter.IsWhiteSpaceAfterCurrentToken());

            iter.Advance(1);
            Assert.IsFalse(iter.IsWhiteSpaceBeforeCurrentToken());
            Assert.IsFalse(iter.IsWhiteSpaceAfterCurrentToken());
        }
    }
}
