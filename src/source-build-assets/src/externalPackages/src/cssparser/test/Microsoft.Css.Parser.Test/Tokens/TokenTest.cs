// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.Tokens
{
    [TestClass]
    public class CssTokenTest : CssUnitTestBase
    {
        [TestMethod]
        public void CssToken_EndOfFileTokenTest()
        {
            CssToken eof = CssToken.EndOfFileToken();
            Assert.AreEqual(0, eof.Start);
            Assert.AreEqual(0, eof.Length);
            Assert.AreEqual(CssTokenType.EndOfFile, eof.TokenType);

            eof = CssToken.EndOfFileToken(new StringTextProvider("abc"));
            Assert.AreEqual(3, eof.Start);
            Assert.AreEqual(0, eof.Length);
            Assert.AreEqual(CssTokenType.EndOfFile, eof.TokenType);
        }

        [TestMethod]
        public void CssToken_Simple()
        {
            ITextProvider tp = new StringTextProvider("foo*+bar");
            CssToken token = new CssToken(CssTokenType.Asterisk, 3, 0);

            Assert.AreEqual(3, token.Start);
            Assert.AreEqual(0, token.Length);
            Assert.AreEqual(3, token.AfterEnd);
            Assert.AreEqual(string.Empty, tp.GetText(token.Start, token.Length));
            Assert.AreEqual(CssTokenType.Asterisk, token.TokenType);
        }

        [TestMethod]
        public void CssToken_Compare()
        {
            ITextProvider text = new StringTextProvider("abcdef");
            CssToken t1 = new CssToken(CssTokenType.DoubleColon, 1, 2);
            CssToken t2 = new CssToken(CssTokenType.DoubleColon, 0, 2);

            Assert.IsTrue(CssToken.CompareTokens(t1, t2, text, text));

            t2.TokenType = CssTokenType.Colon;
            Assert.IsFalse(CssToken.CompareTokens(t1, t2, text, text));

            t1.TokenType = CssTokenType.Identifier;
            t2.TokenType = CssTokenType.Identifier;
            Assert.IsFalse(CssToken.CompareTokens(t1, t2, text, text));

            t2 = new CssToken(t1.TokenType, t1.Start, t1.Length);
            Assert.IsTrue(CssToken.CompareTokens(t1, t2, text, text));
        }
    }
}
