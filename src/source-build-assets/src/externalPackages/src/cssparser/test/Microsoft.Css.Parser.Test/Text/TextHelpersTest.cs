// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.Text
{
    [TestClass]
    public class TextHelpersTest : CssUnitTestBase
    {
        [TestMethod]
        public void TextHelpers_Decode1()
        {
            // Try parsing a simple unicode char and escaped char

            string text = @"u\52 \l(foo.jpg)";
            CharacterStream cs = new CharacterStream(new StringTextProvider(text));

            Assert.IsFalse(TextHelpers.AtEscape(cs));
            Assert.IsFalse(TextHelpers.AtUnicodeEscape(cs));
            Assert.AreEqual(new DecodedChar('u', 1), TextHelpers.DecodeCurrentChar(cs));
            Assert.IsTrue(cs.Advance(1));

            Assert.IsTrue(TextHelpers.AtEscape(cs));
            Assert.IsTrue(TextHelpers.AtUnicodeEscape(cs));
            Assert.AreEqual('R', TextHelpers.DecodeCurrentChar(cs).Char);
            Assert.AreEqual(4, TextHelpers.DecodeCurrentChar(cs).EncodedLength);
            Assert.IsTrue(cs.Advance(4));

            Assert.IsTrue(TextHelpers.AtEscape(cs));
            Assert.IsFalse(TextHelpers.AtUnicodeEscape(cs));
            Assert.AreEqual('l', TextHelpers.DecodeCurrentChar(cs).Char);
            Assert.AreEqual(2, TextHelpers.DecodeCurrentChar(cs).EncodedLength);
            Assert.IsTrue(cs.Advance(2));

            Assert.IsFalse(TextHelpers.AtEscape(cs));
            Assert.IsFalse(TextHelpers.AtUnicodeEscape(cs));
            Assert.AreEqual(new DecodedChar('(', 1), TextHelpers.DecodeCurrentChar(cs));

            Assert.AreEqual(@"uRl(foo.jpg)", TextHelpers.DecodeText(cs.TextProvider, 0, text.Length, forStringToken: false));
        }

        [TestMethod]
        public void TextHelpers_Decode2()
        {
            // Try parsing a unicode char that's larger than 0xFFFF

            CharacterStream cs = new CharacterStream(new StringTextProvider(@"\abcd1234"));

            Assert.IsTrue(TextHelpers.AtEscape(cs));
            Assert.IsTrue(TextHelpers.AtUnicodeEscape(cs));

            DecodedChar dc = TextHelpers.DecodeCurrentChar(cs);
            Assert.AreEqual(7, dc.EncodedLength);
            Assert.IsTrue(dc.RequiresUtf32);
            Assert.AreEqual(0xABCD12, dc.CharUtf32);
            Assert.AreEqual('\0', dc.Char);
            Assert.IsTrue(cs.Advance(dc.EncodedLength));
            Assert.AreEqual('3', cs.CurrentChar);
        }

        [TestMethod]
        public void TextHelpers_Decode3()
        {
            Assert.AreEqual("PQ RST U",
                TextHelpers.DecodeText(new StringTextProvider("PQ \\52\\53 \\54\n \\U"),
                0, 17, forStringToken: false));

            Assert.AreEqual("PQ RST U",
                TextHelpers.DecodeText(new StringTextProvider("PQ RST U"),
                0, 8, forStringToken: false));

            Assert.AreEqual("R2\r2",
                TextHelpers.DecodeText(new StringTextProvider("\\0000522\\D \\32"),
                0, 14, forStringToken: false));
        }

        public void TextHelpers_DecodeStringToken()
        {
            Assert.AreEqual("Path1Path2Path3",
                TextHelpers.DecodeText(new StringTextProvider("\\\nPath1\\\r\nPath2\\\rPath3\\\f\\\r\n"),
                0, 21, forStringToken: true));

            Assert.AreEqual(@"include.css",
                TextHelpers.DecodeText(new StringTextProvider("include.cs\\73 "),
                0, 14, forStringToken: true));
        }

        [TestMethod]
        public void TextHelpers_DecodeOutOfRange()
        {
            Assert.AreEqual("\xFFFD\xD800\xDFFF\xFFFD",
                TextHelpers.DecodeText(new StringTextProvider(@"\110000 \D800 \DFFF \999999"),
                0, 27, forStringToken: false));
        }
    }
}
