// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.Tokens
{
    [TestClass]
    public class TokenListTest : CssUnitTestBase
    {
        [TestMethod]
        public void TokenList_Simple()
        {
            TokenList tokens = new TokenList();
            Assert.AreEqual(0, tokens.Count);

            CssToken[] expect = new CssToken[]
            {
                new CssToken(CssTokenType.Asterisk, 10, 1),
                new CssToken(CssTokenType.Asterisk, 20, 1),
                new CssToken(CssTokenType.Asterisk, 30, 1),
                new CssToken(CssTokenType.Asterisk, 40, 1),
                new CssToken(CssTokenType.Asterisk, 50, 1),
                new CssToken(CssTokenType.Asterisk, 60, 1),
            };

            // The token collection is supposed to automatically sort its contents,
            // so add the tokens in a weird order.

            tokens.Add(expect[3]);
            tokens.Add(expect[0]);
            tokens.Add(expect[1]);
            tokens.Add(expect[4]);
            tokens.Add(expect[2]);
            tokens.Insert(tokens.Count, expect[5]);

            Assert.AreEqual(expect.Length, tokens.Count);

            for (int i = 0; i < expect.Length; i++)
            {
                Assert.AreEqual(expect[i], tokens[i]);
                Assert.IsTrue(tokens.Contains(expect[i]));
                Assert.AreEqual(i, tokens.IndexOf(expect[i]));
            }

            // Test the binary search for the token collection

            Assert.AreEqual(0, tokens.FindInsertIndex(0, beforeExisting: true));
            Assert.AreEqual(0, tokens.FindInsertIndex(10, beforeExisting: true));
            Assert.AreEqual(1, tokens.FindInsertIndex(10, beforeExisting: false));
            Assert.AreEqual(3, tokens.FindInsertIndex(35, beforeExisting: true));
            Assert.AreEqual(3, tokens.FindInsertIndex(35, beforeExisting: false));
            Assert.AreEqual(4, tokens.FindInsertIndex(50, beforeExisting: true));
            Assert.AreEqual(5, tokens.FindInsertIndex(50, beforeExisting: false));
            Assert.AreEqual(6, tokens.FindInsertIndex(61, beforeExisting: true));
            Assert.AreEqual(6, tokens.FindInsertIndex(61, beforeExisting: false));
            Assert.AreEqual(6, tokens.FindInsertIndex(100, beforeExisting: true));
            Assert.AreEqual(6, tokens.FindInsertIndex(100, beforeExisting: false));

            Assert.IsTrue(tokens.Remove(expect[2]));
            Assert.AreEqual(expect.Length - 1, tokens.Count);
            Assert.AreEqual(expect[3], tokens[2]);

            tokens.Clear();
            Assert.AreEqual(0, tokens.Count);
        }
    }
}
