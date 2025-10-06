// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.TreeItems;
using Microsoft.Css.Parser.TreeItems.AtDirectives;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.TreeItems.AtDirectives
{
    [TestClass]
    public class AtBlockDirectiveTest : CssUnitTestBase
    {
        [TestMethod]
        public void AtDirective_Parse()
        {
            string text =
                @"@keyframes { foo } " +
                @"@-ms-keyframes { foo } " +
                @"@-moz-keyframes { foo } " +
                @"@-webkit-keyframes { foo } " +
                @"@keyframes foo { from, to { top: 50% } 0% { left: 100px } } " +
                @"@font-face { } " +
                @"@variables { foo: bar } " +
                @"@page { @top-left-corner { foo } }" +
                @"@counter { list-style-type: decimal; }" +
                @"@ ";

            StyleSheet ss = new StyleSheet();
            ITextProvider tp = new StringTextProvider(text);
            Assert.IsTrue(ss.Parse(new ItemFactory(tp, null), tp, Helpers.MakeTokenStream(tp)));

            Assert.IsInstanceOfType(ss.Children[0], typeof(KeyFramesDirective));
            Assert.IsInstanceOfType(ss.Children[1], typeof(KeyFramesDirective));
            Assert.IsInstanceOfType(ss.Children[2], typeof(KeyFramesDirective));
            Assert.IsInstanceOfType(ss.Children[3], typeof(KeyFramesDirective));
            Assert.IsInstanceOfType(ss.Children[4], typeof(KeyFramesDirective));
            Assert.IsInstanceOfType(ss.Children[5], typeof(FontFaceDirective));
            Assert.IsInstanceOfType(ss.Children[6], typeof(UnknownDirective));
            Assert.IsInstanceOfType(ss.Children[7], typeof(PageDirective));
            Assert.IsInstanceOfType(ss.Children[8], typeof(CounterDirective));
        }

        [TestMethod]
        public void AtDirective_PageDirectiveChildren()
        {
            string[] tests = new string[]
            {
                "@page { @top-left-corner { foo } }",
                "@page { @top-left { foo } }",
                "@page { @top-center { foo } }",
                "@page { @top-right { foo } }",
                "@page { @top-right-corner { foo } }",
                "@page { @bottom-left-corner { foo } }",
                "@page { @bottom-left { foo } }",
                "@page { @bottom-center { foo } }",
                "@page { @bottom-right { foo } }",
                "@page { @bottom-right-corner { foo } }",
                "@page { @left-top { foo } }",
                "@page { @left-middle { foo } }",
                "@page { @left-bottom { foo } }",
                "@page { @right-top { foo } }",
                "@page { @right-middle { foo } }",
                "@page { @right-bottom { foo } }",
                "@page { @RIGHT-bottom { foo } }",
            };

            MarginDirectiveType[] types = new MarginDirectiveType[]
            {
                MarginDirectiveType.TopLeftCorner,
                MarginDirectiveType.TopLeft,
                MarginDirectiveType.TopCenter,
                MarginDirectiveType.TopRight,
                MarginDirectiveType.TopRightCorner,
                MarginDirectiveType.BottomLeftCorner,
                MarginDirectiveType.BottomLeft,
                MarginDirectiveType.BottomCenter,
                MarginDirectiveType.BottomRight,
                MarginDirectiveType.BottomRightCorner,
                MarginDirectiveType.LeftTop,
                MarginDirectiveType.LeftMiddle,
                MarginDirectiveType.LeftBottom,
                MarginDirectiveType.RightTop,
                MarginDirectiveType.RightMiddle,
                MarginDirectiveType.RightBottom,
                MarginDirectiveType.Unknown,
            };

            for (int i = 0; i < tests.Length; i++)
            {
                PageDirective s = new PageDirective();
                ITextProvider tp = new StringTextProvider(tests[i]);
                Assert.IsTrue(s.Parse(new ItemFactory(tp, null), tp, Helpers.MakeTokenStream(tp)));
                Assert.AreEqual(1, s.PageDirectiveBlock.Margins.Count);
                Assert.AreEqual(types[i], s.PageDirectiveBlock.Margins[0].DirectiveType);
            }
        }
    }
}
