// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems.Selectors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.TreeItems.Selectors
{
    [TestClass]
    public class ClassSelectorTest : CssUnitTestBase
    {
        [TestMethod]
        public void ClassSelector_ParseTest()
        {
            string text1 = ".a";
            ITextProvider tp = new StringTextProvider(text1);
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            ClassSelector d = new ClassSelector();
            Assert.IsTrue(d.Parse(new ItemFactory(tp, null), tp, tokens));
            Assert.IsNotNull(d.ClassName);
            Assert.IsNotNull(d.Dot);

            text1 = ". a";
            tp = new StringTextProvider(text1);
            tokens = Helpers.MakeTokenStream(tp);
            d = new ClassSelector();
            Assert.IsTrue(d.Parse(new ItemFactory(tp, null), tp, tokens));
            Assert.IsNull(d.ClassName);
            Assert.IsNotNull(d.Dot);

            text1 = "a";
            tp = new StringTextProvider(text1);
            tokens = Helpers.MakeTokenStream(tp);
            d = new ClassSelector();
            Assert.IsFalse(d.Parse(new ItemFactory(tp, null), tp, tokens));

            text1 = ".a[b=c]";
            tp = new StringTextProvider(text1);
            tokens = Helpers.MakeTokenStream(tp);
            d = new ClassSelector();
            Assert.IsTrue(d.Parse(new ItemFactory(tp, null), tp, tokens));
            Assert.IsNotNull(d.ClassName);
            Assert.IsNotNull(d.Dot);
        }

        [TestMethod]
        public void ClassSelectorValidTest()
        {
            string[] tests = new string[]
            {
                ".a",
                ".a[b=c]",
                ".*",
            };

            foreach (string test in tests)
            {
                ITextProvider tp = new StringTextProvider(test);
                TokenStream ts = Helpers.MakeTokenStream(tp);
                ClassSelector classSelector = new ClassSelector();
                Assert.IsTrue(classSelector.Parse(new ItemFactory(tp, null), tp, ts), "Valid CSS '{0}' parsed incorrectly", test);
                Assert.IsNotNull(classSelector.ClassName, "ClassName not parsed correct for CSS '{0}'", test);
                Assert.IsNotNull(classSelector.Dot, "ClassSelector.Dot not parsed correct for CSS '{0}'", test);
            }
        }

        [TestMethod]
        public void ClassSelectorInvalidClassNameTest()
        {
            string[] tests = new string[]
            {
                ".  a",
                ".#",
            };

            foreach (string test in tests)
            {
                ITextProvider tp = new StringTextProvider(test);
                TokenStream ts = Helpers.MakeTokenStream(tp);
                ClassSelector classSelector = new ClassSelector();
                Assert.IsTrue(classSelector.Parse(new ItemFactory(tp, null), tp, ts));
                Assert.IsNull(classSelector.ClassName, "Invalid ClassName parsed correctly for CSS '{0}'", test);
                Assert.IsNotNull(classSelector.Dot);
            }
        }

        [TestMethod]
        public void ClassSelectorInvalidTest()
        {
            string[] tests = new string[]
            {
                "a",
                "#abc",
                "*", "+", ">",
                "|foo",
            };

            foreach (string test in tests)
            {
                ITextProvider tp = new StringTextProvider(test);
                TokenStream ts = Helpers.MakeTokenStream(tp);
                ClassSelector classSelector = new ClassSelector();
                Assert.IsFalse(classSelector.Parse(new ItemFactory(tp, null), tp, ts), "Invalid CSS '{0}' parsed correctly", test);
            }
        }
    }
}
