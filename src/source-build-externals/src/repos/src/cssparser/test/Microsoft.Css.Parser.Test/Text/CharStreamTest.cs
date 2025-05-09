// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.Text
{
    [TestClass]
    public class CharStreamTest : CssUnitTestBase
    {
        [TestMethod]
        public void TextIterator_Simple()
        {
            CharacterStream ti = new CharacterStream(new StringTextProvider("abcd"));

            Assert.AreEqual(4, ti.TextProvider.Length);
            Assert.AreEqual(0, ti.Position);
            Assert.AreEqual('a', ti.CurrentChar);
            Assert.AreEqual(new DecodedChar('a', 1), TextHelpers.DecodeCurrentChar(ti));

            Assert.IsTrue(ti.TextProvider.CompareTo(ti.Position, "ab", ignoreCase: false));
            Assert.IsFalse(ti.TextProvider.CompareTo(ti.Position, "abcde", ignoreCase: false));

            Assert.IsTrue(TextHelpers.CompareCurrentDecodedString(ti, "ab", ignoreCase: false, matchLength: out int matchLength));
            Assert.AreEqual(2, matchLength);
            Assert.IsFalse(TextHelpers.CompareCurrentDecodedString(ti, "abcde", ignoreCase: false, matchLength: out _));

            Assert.IsFalse(ti.IsAtEnd);
            Assert.IsTrue(ti.Advance(1));
            Assert.AreEqual(1, ti.Position);
            Assert.AreEqual('b', ti.CurrentChar);
            Assert.AreEqual('a', ti.Peek(-1));
            Assert.AreEqual('c', ti.Peek(1));
            Assert.AreEqual('d', ti.Peek(2));
            Assert.AreEqual(0, ti.Peek(3));
            Assert.AreEqual(0, ti.Peek(4));

            Assert.IsTrue(ti.Advance(3));
            Assert.IsTrue(ti.IsAtEnd);

            Assert.IsFalse(ti.Advance(1));
        }
    }
}
