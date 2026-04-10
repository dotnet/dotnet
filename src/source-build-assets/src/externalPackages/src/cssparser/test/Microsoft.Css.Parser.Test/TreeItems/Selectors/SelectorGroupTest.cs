// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.TreeItems.Selectors
{
    [TestClass]
    public class SelectorGroupTest : CssUnitTestBase
    {
        [TestMethod]
        public void SelectorGroup_ParseTest()
        {
            string text = "A+B, C > D, body + html ~ * foo {}";
            ITextProvider tp = new StringTextProvider(text);
            TokenStream ts = Helpers.MakeTokenStream(tp);

            RuleSet s = new RuleSet();

            Assert.IsTrue(s.Parse(new ItemFactory(tp, null), tp, ts));
            Assert.AreEqual(4, s.Children.Count);
            Assert.AreEqual(3, s.Selectors.Count);

            Assert.IsTrue(tp.CompareTo(s.Selectors[0].SimpleSelectors[0].Name.Start, "A", ignoreCase: false));
            Assert.IsNotNull(s.Selectors[0].SimpleSelectors[0].SelectorCombineOperator);

            Assert.IsTrue(tp.CompareTo(s.Selectors[0].SimpleSelectors[1].Name.Start, "B", ignoreCase: false));
            Assert.IsNull(s.Selectors[0].SimpleSelectors[1].SelectorCombineOperator);

            Assert.IsTrue(tp.CompareTo(s.Selectors[1].SimpleSelectors[0].Name.Start, "C", ignoreCase: false));
            Assert.IsNotNull(s.Selectors[1].SimpleSelectors[0].SelectorCombineOperator);
            Assert.IsTrue(tp.CompareTo(s.Selectors[1].SimpleSelectors[0].SelectorCombineOperator.Start, ">", ignoreCase: false));

            Assert.IsTrue(tp.CompareTo(s.Selectors[1].SimpleSelectors[1].Name.Start, "D", ignoreCase: false));
            Assert.IsNull(s.Selectors[1].SimpleSelectors[1].SelectorCombineOperator);

            Assert.IsTrue(tp.CompareTo(s.Selectors[2].SimpleSelectors[0].Name.Start, "body", ignoreCase: false));
            Assert.IsNotNull(s.Selectors[2].SimpleSelectors[0].SelectorCombineOperator);
            Assert.IsTrue(tp.CompareTo(s.Selectors[2].SimpleSelectors[0].SelectorCombineOperator.Start, "+", ignoreCase: false));

            Assert.IsTrue(tp.CompareTo(s.Selectors[2].SimpleSelectors[1].Name.Start, "html", ignoreCase: false));
            Assert.IsNotNull(s.Selectors[2].SimpleSelectors[1].SelectorCombineOperator);
            Assert.IsTrue(tp.CompareTo(s.Selectors[2].SimpleSelectors[1].SelectorCombineOperator.Start, "~", ignoreCase: false));

            Assert.IsTrue(tp.CompareTo(s.Selectors[2].SimpleSelectors[2].Name.Start, "*", ignoreCase: false));
            Assert.IsNull(s.Selectors[2].SimpleSelectors[2].SelectorCombineOperator);
        }

        [TestMethod]
        public void SelectorGroup_ParseTest2()
        {
            string text = "A B, C ~ D, body + html foo, E+F, body {}";
            ITextProvider tp = new StringTextProvider(text);
            TokenStream ts = Helpers.MakeTokenStream(tp);

            RuleSet ruleSet = new RuleSet();

            Assert.IsTrue(ruleSet.Parse(new ItemFactory(tp, null), tp, ts), "Valid CSS '{0}' parsed incorrectly", text);
            Assert.AreEqual(ruleSet.Selectors.Count, 5, "Selectors.Count not obtained correctly for CSS '{0}'", text);

            // Selector A B
            Assert.IsTrue(tp.CompareTo(ruleSet.Selectors[0].SimpleSelectors[0].Name.Start, "A", ignoreCase: false));
            Assert.IsNull(ruleSet.Selectors[0].SimpleSelectors[0].SelectorCombineOperator);

            Assert.IsTrue(tp.CompareTo(ruleSet.Selectors[0].SimpleSelectors[1].Name.Start, "B", ignoreCase: false));
            Assert.IsNull(ruleSet.Selectors[0].SimpleSelectors[1].SelectorCombineOperator);

            // Selector C ~ D
            Assert.IsTrue(tp.CompareTo(ruleSet.Selectors[1].SimpleSelectors[0].Name.Start, "C", ignoreCase: false), "Selector C not parsed for CSS '{0}'", text);
            Assert.IsNotNull(ruleSet.Selectors[1].SimpleSelectors[0].SelectorCombineOperator, "SelectorCombineOperator not parsed for CSS '{0}'", text);
            Assert.IsTrue(tp.CompareTo(ruleSet.Selectors[1].SimpleSelectors[0].SelectorCombineOperator.Start, "~", ignoreCase: false), "SelectorCombineOperator \"~\" not parsed for CSS '{0}'", text);

            Assert.IsTrue(tp.CompareTo(ruleSet.Selectors[1].SimpleSelectors[1].Name.Start, "D", ignoreCase: false), "Selector D not parsed for CSS '{0}'", text);
            Assert.IsNull(ruleSet.Selectors[1].SimpleSelectors[1].SelectorCombineOperator, "SelectorCombineOperator after D found, when not present for CSS '{0}'", text);

            // body + html foo
            Assert.IsTrue(tp.CompareTo(ruleSet.Selectors[2].SimpleSelectors[0].Name.Start, "body", ignoreCase: false));
            Assert.IsNotNull(ruleSet.Selectors[2].SimpleSelectors[0].SelectorCombineOperator);
            Assert.IsTrue(tp.CompareTo(ruleSet.Selectors[2].SimpleSelectors[0].SelectorCombineOperator.Start, "+", ignoreCase: false));

            Assert.IsTrue(tp.CompareTo(ruleSet.Selectors[2].SimpleSelectors[1].Name.Start, "html", ignoreCase: false));
            Assert.IsNull(ruleSet.Selectors[2].SimpleSelectors[1].SelectorCombineOperator);

            Assert.IsTrue(tp.CompareTo(ruleSet.Selectors[2].SimpleSelectors[2].Name.Start, "foo", ignoreCase: false));
            Assert.IsNull(ruleSet.Selectors[2].SimpleSelectors[2].SelectorCombineOperator);

            // Selector E+F
            Assert.IsTrue(tp.CompareTo(ruleSet.Selectors[3].SimpleSelectors[0].Name.Start, "E", ignoreCase: false));
            Assert.IsNotNull(ruleSet.Selectors[3].SimpleSelectors[0].SelectorCombineOperator);
            Assert.IsTrue(tp.CompareTo(ruleSet.Selectors[3].SimpleSelectors[0].SelectorCombineOperator.Start, "+", ignoreCase: false));

            Assert.IsTrue(tp.CompareTo(ruleSet.Selectors[3].SimpleSelectors[1].Name.Start, "F", ignoreCase: false));
            Assert.IsNull(ruleSet.Selectors[3].SimpleSelectors[1].SelectorCombineOperator);

            // Selector body
            Assert.IsTrue(tp.CompareTo(ruleSet.Selectors[4].SimpleSelectors[0].Name.Start, "body", ignoreCase: false));
            Assert.IsNull(ruleSet.Selectors[4].SimpleSelectors[0].SelectorCombineOperator);
        }

        [TestMethod]
        public void SelectorGroupInvalidSelectorTest()
        {
            string[] tests = new string[]
            {
                "+,+ {}",
                ">,~ {}",
            };

            foreach (string test in tests)
            {
                ITextProvider tp = new StringTextProvider(test);
                TokenStream ts = Helpers.MakeTokenStream(tp);
                RuleSet ruleSet = new RuleSet();
                Assert.IsTrue(ruleSet.Parse(new ItemFactory(tp, null), tp, ts));
                Assert.IsFalse(ruleSet.IsValid);
            }
        }
    }
}
