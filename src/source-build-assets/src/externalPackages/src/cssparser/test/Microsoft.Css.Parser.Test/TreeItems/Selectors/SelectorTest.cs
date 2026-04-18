// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems;
using Microsoft.Css.Parser.TreeItems.Selectors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.TreeItems.Selectors
{
    [TestClass]
    public class SelectorTest : CssUnitTestBase
    {
        [TestMethod]
        public void Selector_ParseTest()
        {
            string text = "A+B C > D body + html ~ * foo {}";
            ITextProvider tp = new StringTextProvider(text);
            TokenStream ts = Helpers.MakeTokenStream(tp);

            Selector s = new Selector();

            Assert.IsTrue(s.Parse(new ItemFactory(tp, null), tp, ts));
            Assert.AreEqual(8, s.Children.Count);
            Assert.AreEqual(8, s.SimpleSelectors.Count);

            Assert.IsTrue(tp.CompareTo(s.SimpleSelectors[0].Name.Start, "A", ignoreCase: false));
            Assert.IsNotNull(s.SimpleSelectors[0].SelectorCombineOperator);
            Assert.AreEqual(CssTokenType.Plus, ((TokenItem)s.SimpleSelectors[0].SelectorCombineOperator).TokenType);

            Assert.IsTrue(tp.CompareTo(s.SimpleSelectors[1].Name.Start, "B", ignoreCase: false));
            Assert.IsNull(s.SimpleSelectors[1].SelectorCombineOperator);

            Assert.IsTrue(tp.CompareTo(s.SimpleSelectors[2].Name.Start, "C", ignoreCase: false));
            Assert.IsNotNull(s.SimpleSelectors[2].SelectorCombineOperator);
            Assert.IsTrue(tp.CompareTo(s.SimpleSelectors[2].SelectorCombineOperator.Start, ">", ignoreCase: false));

            Assert.IsTrue(tp.CompareTo(s.SimpleSelectors[3].Name.Start, "D", ignoreCase: false));
            Assert.IsNull(s.SimpleSelectors[3].SelectorCombineOperator);

            Assert.IsTrue(tp.CompareTo(s.SimpleSelectors[4].Name.Start, "body", ignoreCase: false));
            Assert.IsNotNull(s.SimpleSelectors[4].SelectorCombineOperator);
            Assert.IsTrue(tp.CompareTo(s.SimpleSelectors[4].SelectorCombineOperator.Start, "+", ignoreCase: false));

            Assert.IsTrue(tp.CompareTo(s.SimpleSelectors[5].Name.Start, "html", ignoreCase: false));
            Assert.IsNotNull(s.SimpleSelectors[5].SelectorCombineOperator);
            Assert.IsTrue(tp.CompareTo(s.SimpleSelectors[5].SelectorCombineOperator.Start, "~", ignoreCase: false));

            Assert.IsTrue(tp.CompareTo(s.SimpleSelectors[6].Name.Start, "*", ignoreCase: false));
            Assert.IsNull(s.SimpleSelectors[6].SelectorCombineOperator);

            Assert.IsTrue(tp.CompareTo(s.SimpleSelectors[7].Name.Start, "foo", ignoreCase: false));
            Assert.IsNull(s.SimpleSelectors[7].SelectorCombineOperator);

            Assert.IsTrue(s.IsValid);
        }

        [TestMethod]
        public void Combinator2SelectorTest()
        {
            string[] tests = new string[]
            {
                "foo bar {}",
                "foo                bar{}",
                "foo    bar {}",
                "foo > bar{}",
                "foo~bar{}",
                "foo ~ bar{}",
                "foo + bar {}",
                "foo + bar {}    ",
                "foo h1.bar {}",
                "c1.foo                bar{}",
                "c1#foo    bar {}",
                "foo > h1.bar{}",
                "h2.foo ~ bar{}",
                "foo + h1.bar {}",
                "h2.foo + bar {}    ",
            };

            foreach (string test in tests)
            {
                ITextProvider tp = new StringTextProvider(test);
                TokenStream ts = Helpers.MakeTokenStream(tp);
                Selector s = new Selector();

                Assert.IsTrue(s.Parse(new ItemFactory(tp, null), tp, ts), "Valid CSS '{0}' parsed incorrectly", test);
                Assert.IsTrue(s.IsValid);
                Assert.AreEqual(s.SimpleSelectors.Count, 2, "We should have only 2 SimpleSelectors for CSS '{0}'", test);
            }
        }

        [TestMethod]
        public void Combinator3SelectorTest()
        {
            string[] tests = new string[]
            {
                "foo *   bar {}",
                "foo * bar{}",
                "foo bar real {}",
                "div p *[href]",
            };

            foreach (string test in tests)
            {
                ITextProvider tp = new StringTextProvider(test);
                TokenStream ts = Helpers.MakeTokenStream(tp);
                Selector s = new Selector();

                Assert.IsTrue(s.Parse(new ItemFactory(tp, null), tp, ts), "Valid CSS '{0}' parsed incorrectly", test);
                Assert.IsTrue(s.IsValid);
                Assert.AreEqual(s.SimpleSelectors.Count, 3, "We should have only 3 SimpleSelectors for CSS '{0}'", test);
            }
        }

        [TestMethod]
        public void Combinator4SelectorTest()
        {
            string[] tests = new string[]
            {
                "foo bar > gone wild {}",
                "foo bar>gone wild {}",
                "foo bar>   gone wild {}",
                "foo+bar>gone wild {}",
            };

            foreach (string test in tests)
            {
                ITextProvider tp = new StringTextProvider(test);
                TokenStream ts = Helpers.MakeTokenStream(tp);
                Selector s = new Selector();

                Assert.IsTrue(s.Parse(new ItemFactory(tp, null), tp, ts), "Valid CSS '{0}' parsed incorrectly", test);
                Assert.IsTrue(s.IsValid);
                Assert.AreEqual(s.SimpleSelectors.Count, 4, "We should have only 4 SimpleSelectors for CSS '{0}'", test);
            }
        }

        [TestMethod]
        public void Combinator5SelectorTest()
        {
            string[] tests = new string[]
            {
                "foo+bar > gone * wild {}",
                "foo * bar>   gone ~ wild {}",
                "foo bar >   gone ~ wild bar {}",
            };

            foreach (string test in tests)
            {
                ITextProvider tp = new StringTextProvider(test);
                TokenStream ts = Helpers.MakeTokenStream(tp);
                Selector s = new Selector();

                Assert.IsTrue(s.Parse(new ItemFactory(tp, null), tp, ts), "Valid CSS '{0}' parsed incorrectly", test);
                Assert.IsTrue(s.IsValid);
                Assert.AreEqual(s.SimpleSelectors.Count, 5, "We should have only 5 SimpleSelectors for CSS '{0}'", test);
            }
        }

        [TestMethod]
        public void Combinator6SelectorTest()
        {
            string text = "A+B C >     D * foo {}";
            ITextProvider tp = new StringTextProvider(text);
            TokenStream ts = Helpers.MakeTokenStream(tp);

            Selector s = new Selector();
            Assert.IsTrue(s.Parse(new ItemFactory(tp, null), tp, ts), "Valid CSS '{0}' parsed incorrectly", text);

            Assert.AreEqual(s.SimpleSelectors.Count, 6, "We should have only 6 SimpleSelectors for CSS '{0}'", text);

            Assert.IsTrue(tp.CompareTo(s.SimpleSelectors[0].Name.Start, "A", ignoreCase: false));
            Assert.IsNotNull(s.SimpleSelectors[0].SelectorCombineOperator);
            Assert.IsTrue(tp.CompareTo(s.SimpleSelectors[0].SelectorCombineOperator.Start, "+", ignoreCase: false));

            Assert.IsTrue(tp.CompareTo(s.SimpleSelectors[1].Name.Start, "B", ignoreCase: false));
            Assert.IsNull(s.SimpleSelectors[1].SelectorCombineOperator);

            Assert.IsTrue(tp.CompareTo(s.SimpleSelectors[2].Name.Start, "C", ignoreCase: false), "SimpleSelector C not obtained for CSS '{0}'", text);
            Assert.IsNotNull(s.SimpleSelectors[2].SelectorCombineOperator);
            Assert.IsTrue(tp.CompareTo(s.SimpleSelectors[2].SelectorCombineOperator.Start, ">", ignoreCase: false), "SelectorCombineOperator \">\" not obtained for CSS '{0}'", text);

            Assert.IsTrue(tp.CompareTo(s.SimpleSelectors[3].Name.Start, "D", ignoreCase: false), "SimpleSelector D not obtained for CSS '{0}'", text);
            Assert.IsNull(s.SimpleSelectors[3].SelectorCombineOperator);

            Assert.IsTrue(tp.CompareTo(s.SimpleSelectors[4].Name.Start, "*", ignoreCase: false));
            Assert.IsNull(s.SimpleSelectors[4].SelectorCombineOperator);

            Assert.IsTrue(tp.CompareTo(s.SimpleSelectors[5].Name.Start, "foo", ignoreCase: false));
            Assert.IsNull(s.SimpleSelectors[5].SelectorCombineOperator);

            Assert.IsTrue(s.IsValid);
        }

        [TestMethod]
        public void CombinatorComplexSelectorTest()
        {
            string text = "A+B C > D * body + html ~ foo {}";
            ITextProvider tp = new StringTextProvider(text);
            TokenStream ts = Helpers.MakeTokenStream(tp);

            Selector s = new Selector();
            Assert.IsTrue(s.Parse(new ItemFactory(tp, null), tp, ts), "Valid CSS '{0}' parsed incorrectly", text);

            Assert.AreEqual(s.SimpleSelectors.Count, 8, "We should have only 8 SimpleSelectors for CSS '{0}'", text);

            Assert.IsTrue(tp.CompareTo(s.SimpleSelectors[0].Name.Start, "A", ignoreCase: false));
            Assert.IsNotNull(s.SimpleSelectors[0].SelectorCombineOperator);

            Assert.IsTrue(tp.CompareTo(s.SimpleSelectors[1].Name.Start, "B", ignoreCase: false));
            Assert.IsNull(s.SimpleSelectors[1].SelectorCombineOperator);

            Assert.IsTrue(tp.CompareTo(s.SimpleSelectors[2].Name.Start, "C", ignoreCase: false));
            Assert.IsNotNull(s.SimpleSelectors[2].SelectorCombineOperator);

            Assert.IsTrue(tp.CompareTo(s.SimpleSelectors[3].Name.Start, "D", ignoreCase: false));
            Assert.IsNull(s.SimpleSelectors[3].SelectorCombineOperator);

            Assert.IsTrue(tp.CompareTo(s.SimpleSelectors[4].Name.Start, "*", ignoreCase: false));
            Assert.IsNull(s.SimpleSelectors[4].SelectorCombineOperator);

            Assert.IsTrue(tp.CompareTo(s.SimpleSelectors[5].Name.Start, "body", ignoreCase: false), "SimpleSelector \"body\" not obtained for CSS '{0}'", text);
            Assert.IsNotNull(s.SimpleSelectors[5].SelectorCombineOperator);
            Assert.IsTrue(tp.CompareTo(s.SimpleSelectors[5].SelectorCombineOperator.Start, "+", ignoreCase: false), "SelectorCombineOperator \"+\" not obtained for CSS '{0}'", text);

            Assert.IsTrue(tp.CompareTo(s.SimpleSelectors[6].Name.Start, "html", ignoreCase: false));
            Assert.IsNotNull(s.SimpleSelectors[6].SelectorCombineOperator);
            Assert.IsTrue(tp.CompareTo(s.SimpleSelectors[6].SelectorCombineOperator.Start, "~", ignoreCase: false));

            Assert.IsTrue(tp.CompareTo(s.SimpleSelectors[7].Name.Start, "foo", ignoreCase: false));
            Assert.IsNull(s.SimpleSelectors[7].SelectorCombineOperator);

            Assert.IsTrue(s.IsValid);
        }

        [TestMethod]
        public void CombinatorSelectorInvalidTest()
        {
            string[] tests = new string[]
            {
                "+ {}",
                "> {}",
                "~ {}",
                ", {}",
            };

            foreach (string test in tests)
            {
                ITextProvider tp = new StringTextProvider(test);
                TokenStream ts = Helpers.MakeTokenStream(tp);
                Selector s = new Selector();
                Assert.IsTrue(s.Parse(new ItemFactory(tp, null), tp, ts));
                Assert.IsFalse(s.IsValid);
            }
        }

        [TestMethod]
        public void Selector_InvalidMissingNames()
        {
            string[] tests = new string[]
            {
                "..foo:nth-child(odd) {}",
                "##foo:nth-child(even) {}",
                ".# foo:nth-child(-1){}",
                "foo. bar:nth-child(2n+1) {}"
            };

            foreach (string test in tests)
            {
                ITextProvider tp = new StringTextProvider(test);
                TokenStream ts = Helpers.MakeTokenStream(tp);
                Selector s = new Selector();
                Assert.IsTrue(s.Parse(new ItemFactory(tp, null), tp, ts));
                Assert.IsFalse(s.IsValid);
            }
        }
    }
}
