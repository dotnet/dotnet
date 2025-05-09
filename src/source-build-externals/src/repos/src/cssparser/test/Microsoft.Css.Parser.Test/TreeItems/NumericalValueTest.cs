// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems.PropertyValues;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.TreeItems
{
    [TestClass]
    public class NumericalValueTest : CssUnitTestBase
    {
        [TestMethod]
        public void NumericalValue_Parse()
        {
            string[] tests = new string[]
            {
                "100",
                "-100",
                "+100",
                "002.125",
                "-23.125",
                "+234.125",
            };

            foreach (string test in tests)
            {
                TokenStream tokens = Helpers.MakeTokenStream(test);
                ITextProvider tp = new StringTextProvider(test);
                NumericalValue s = new NumericalValue();
                Assert.IsTrue(s.Parse(new ItemFactory(tp, null), tp, tokens));
                Assert.IsNotNull(s.Number);
                Assert.AreEqual(test.Length, s.Number.Length);
            }
        }

        [TestMethod]
        public void NumericalValue_ParseFail()
        {
            string[] tests = new string[]
            {
                "a12",
                "-a12",
                "+a12",
            };

            foreach (string test in tests)
            {
                TokenStream tokens = Helpers.MakeTokenStream(test);
                Assert.AreNotEqual(CssTokenType.Number, tokens.CurrentToken.TokenType);
            }
        }
    }
}
