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
    public class SimpleSelectorTest : CssUnitTestBase
    {
        [TestMethod]
        public void SimpleSelector_Valid()
        {
            string[] tests = new string[]
            {
                "foo|h1 { color: blue }",
                " foo|* { color: yellow }",
                " |h1 { color: red }",
                " h1 { color: green }",
                "*[hreflang|=en]",
                "E[foo]",
                "E[foo=\"warning\"]",
                "E[foo~=\"warning\"]",
                "E[lang|=\"en\"]",
                "span[class=\"example\"]",
                "span[hello=\"Cleveland\"][goodbye=\"Columbus\"]",
                "object[type^=\"image/\"]",
                "a[href$=\".html\"]",
                "p[title*=\"hello\"]",
                "*.pastoral { color: green }",
                ".pastoral { color: green }",
                "H1.pastoral { color: green }",
                "p.pastoral.marine { color: green }",
                "DIV.warning",
                "h1#chapter1",
                "#chapter1",
                "*#myid",
                "E:first-child",
                "E:link",
                "E:visited",
                "E:active",
                "E:hover",
                "E:focus",
                "E:lang(c)",
                "[*|att] { color: yellow }",
                "a.external:not(:visited)",
                "a.external:not(::foo)",
                "a.external:not(.class)",
                "a.external:not(#id)",
                "*:target::before { content : url(target.png) }",
                "*:lang(fr-be) > q",
                "p:nth-child(4n+3) { color: maroon; }",
                "h2:not(:first-of-type):not(:last-of-type)",
                " *|*:not(:hover)",
                "button:not([DISABLED])",
                "p::first-letter { color: green; font-size: 200% }",
                "p.note:target",
                "li:last-child",
                "!ul > li",
                "ul! > li",
                "col.selected || td",
                "foo ||",
            };

            foreach (string test in tests)
            {
                ITextProvider tp = new StringTextProvider(test);
                TokenStream ts = Helpers.MakeTokenStream(tp);
                SimpleSelector s = new SimpleSelector();
                Assert.IsTrue(s.Parse(new ItemFactory(tp, null), tp, ts));
                Assert.IsTrue(s.IsValid);
            }
        }

        [TestMethod]
        public void SimpleSelector_ParseFailure()
        {
            string[] tests = new string[]
            {
                " { color: blue }",
            };

            foreach (string test in tests)
            {
                ITextProvider tp = new StringTextProvider(test);
                TokenStream ts = Helpers.MakeTokenStream(tp);
                SimpleSelector s = new SimpleSelector();
                Assert.IsFalse(s.Parse(new ItemFactory(tp, null), tp, ts));
            }
        }

        [TestMethod]
        public void SimpleSelector_Invalid()
        {
            string[] tests = new string[]
            {
                "||{}",
                "**{}",
                "h2..foo{}",
                ":",
                "::",
                ":::",
                ".",
                "..",
                ".#:foo",
                "#",
                "##",
                "#:foo",
                "!!ul > li",
                "ul!! > li",
                "!ul! > li",
                "a.external:not(::::)",
                "a.external:not(.#)",
                "|| foo",
            };

            foreach (string test in tests)
            {
                ITextProvider tp = new StringTextProvider(test);
                TokenStream ts = Helpers.MakeTokenStream(tp);
                SimpleSelector s = new SimpleSelector();
                Assert.IsTrue(s.Parse(new ItemFactory(tp, null), tp, ts));
                Assert.IsFalse(s.IsValid);
            }
        }
    }
}
