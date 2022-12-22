// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Globalization;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems.PropertyValues;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.TreeItems
{
    [TestClass]
    public class HexColorValueTest : CssUnitTestBase
    {
        [TestMethod]
        public void HexColorValue_ParseTest1()
        {
            string text = "#f0e0d0";
            ITextProvider tp = new StringTextProvider(text);
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            HexColorValue h = new HexColorValue();

            Assert.IsTrue(h.Parse(new ItemFactory(tp, null), tp, tokens));
            bool isNumber = h.TryGetNumberRange(out int numberStart, out int numberLength);
            Assert.IsTrue(isNumber);
            Assert.AreEqual(1, numberStart);
            Assert.AreEqual(text.Length, numberStart + numberLength);

            int n = int.Parse(text.Substring(numberStart, numberLength), NumberStyles.HexNumber);
            Assert.AreEqual(0xf0e0d0, n);
        }

        [TestMethod]
        public void HexColorValue_ParseTest2()
        {
            string text = "#0;";
            ITextProvider tp = new StringTextProvider(text);
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            HexColorValue h = new HexColorValue();

            Assert.IsTrue(h.Parse(new ItemFactory(tp, null), tp, tokens));
            bool isNumber = h.TryGetNumberRange(out int numberStart, out int numberLength);
            Assert.IsTrue(isNumber);
            Assert.AreEqual(1, numberStart);
            Assert.AreEqual(text.Length - 1, numberStart + numberLength);

            int n = int.Parse(text.Substring(numberStart, numberLength), NumberStyles.HexNumber);
            Assert.AreEqual(0, n);
        }
    }
}
