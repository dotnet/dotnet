// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.Text
{
    [TestClass]
    public class TextRangeTest : CssUnitTestBase
    {
        [TestMethod]
        public void CssTextRange_Simple()
        {
            ITextProvider textProvider = new StringTextProvider("01234567890123456789");

            Assert.AreEqual("01234", textProvider.GetText(10, 5));

            Assert.IsFalse(TextRange.Touches(10, 5, -1));
            Assert.IsFalse(TextRange.Touches(10, 5, 9));
            Assert.IsTrue(TextRange.Touches(10, 5, 10));
            Assert.IsTrue(TextRange.Touches(10, 5, 14));
            Assert.IsTrue(TextRange.Touches(10, 5, 15));
            Assert.IsFalse(TextRange.Touches(10, 5, 16));
            Assert.IsFalse(TextRange.Touches(10, 5, 100));

            Assert.IsFalse(TextRange.ContainsChar(10, 5, -1));
            Assert.IsFalse(TextRange.ContainsChar(10, 5, 9));
            Assert.IsTrue(TextRange.ContainsChar(10, 5, 10));
            Assert.IsTrue(TextRange.ContainsChar(10, 5, 14));
            Assert.IsFalse(TextRange.ContainsChar(10, 5, 15));
            Assert.IsFalse(TextRange.ContainsChar(10, 5, 16));
            Assert.IsFalse(TextRange.ContainsChar(10, 5, 100));

            Assert.IsFalse(TextRange.Intersects(10, 5, 0, 10));
            Assert.IsTrue(TextRange.Intersects(10, 5, 0, 11));
            Assert.IsFalse(TextRange.Intersects(10, 5, 10, 0));
            Assert.IsTrue(TextRange.Intersects(10, 5, 10, 1));
            Assert.IsTrue(TextRange.Intersects(10, 5, 11, 10));
            Assert.IsTrue(TextRange.Intersects(10, 5, 14, 1));
            Assert.IsTrue(TextRange.Intersects(10, 5, 14, 10));
            Assert.IsFalse(TextRange.Intersects(10, 5, 15, 0));
            Assert.IsFalse(TextRange.Intersects(10, 5, 15, 1));
            Assert.IsFalse(TextRange.Intersects(10, 5, 20, 10));
        }

        [TestMethod]
        public void CssTextRange_Equals()
        {
            ITextProvider tp = new StringTextProvider("@namespace foo \"www.foo.com\";");
            TokenList tokens = Helpers.MakeTokens(tp);

            Assert.IsTrue(tp.CompareTo(tokens[0].Start, "@", ignoreCase: false));
            Assert.IsTrue(tp.CompareTo(tokens[1].Start, "Namespace", ignoreCase: true));
            Assert.IsTrue(tp.CompareTo(tokens[2].Start, "foo", ignoreCase: false));
        }

        [TestMethod]
        public void CssTextRange_CompareDecoded()
        {
            for (int i = 0; i < 2; i++)
            {
#if !SUPPORT_ENCODED_CSS
                if (i == 0)
                {
                    continue;
                }
#endif
                // Try the same string with and without encoded/escaped chars
                string text = (i == 0)
                    ? @"@na\mes\70 ace \66 \o\o 'www.\'f\6F \o\'.com';"
                    : @"@namespace foo 'www.\'foo\'.com'";

                ITextProvider tp = new StringTextProvider(text);
                TokenList tokens = Helpers.MakeTokens(tp);

                Assert.AreEqual("@", TextRange.GetDecodedText(tokens[0].Start, tokens[0].Length, tp, forStringToken: false));
                Assert.AreEqual("namespace", TextRange.GetDecodedText(tokens[1].Start, tokens[1].Length, tp, forStringToken: false));
                Assert.AreEqual("foo", TextRange.GetDecodedText(tokens[2].Start, tokens[2].Length, tp, forStringToken: false));
                Assert.AreEqual(@"'www.'foo'.com'", TextRange.GetDecodedText(tokens[3].Start, tokens[3].Length, tp, forStringToken: false));

                Assert.IsTrue(TextRange.CompareDecoded(tokens[1].Start, tokens[1].Length, tp, "namespace", ignoreCase: false));
                Assert.IsFalse(TextRange.CompareDecoded(tokens[1].Start, tokens[1].Length, tp, "NAMEspace", ignoreCase: false));
                Assert.IsTrue(TextRange.CompareDecoded(tokens[1].Start, tokens[1].Length, tp, "NAMEspace", ignoreCase: true));
                Assert.IsFalse(TextRange.CompareDecoded(tokens[1].Start, tokens[1].Length, tp, "namespace-foobar", ignoreCase: true));
                Assert.IsFalse(TextRange.CompareDecoded(tokens[1].Start, tokens[1].Length, tp, "@namespace", ignoreCase: true));
                Assert.IsTrue(TextRange.CompareDecoded(tokens[2].Start, tokens[2].Length, tp, "foo", ignoreCase: false));

#if SUPPORT_ENCODED_CSS
                Assert.IsTrue(TextRange.CompareDecoded(tokens[3].Start, tokens[3].Length, tp, "'www.'FOO'.com'", ignoreCase: true));
#endif
            }
        }
    }
}
