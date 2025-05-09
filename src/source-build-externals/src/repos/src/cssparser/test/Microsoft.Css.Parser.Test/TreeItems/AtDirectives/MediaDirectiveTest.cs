// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems;
using Microsoft.Css.Parser.TreeItems.AtDirectives;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.TreeItems.AtDirectives
{
    [TestClass]
    public class MediaDirectiveTest : CssUnitTestBase
    {
        [TestMethod]
        public void MediaDirective_ParseTest()
        {
            string text = "@media screen and (device-aspect-ratio: 16/9), projection and (color) { @page {margin: 3cm;} body { background:lime } }";
            ITextProvider tp = new StringTextProvider(text);
            TokenStream ts = Helpers.MakeTokenStream(tp);

            MediaDirective md = new MediaDirective();
            Assert.IsTrue(md.Parse(new ItemFactory(tp, null), tp, ts));
            Assert.IsTrue(tp.CompareTo(md.Keyword.Start, "media", ignoreCase: false));
            Assert.AreEqual(2, md.MediaQueries.Count);

            Assert.IsTrue(md.MediaQueries[0] is MediaQuery);
            MediaQuery mq = md.MediaQueries[0] as MediaQuery;
            Assert.IsTrue(tp.CompareTo(mq.MediaType.Start, "screen", ignoreCase: false));
            Assert.AreEqual(1, mq.Expressions.Count);
            Assert.IsTrue(tp.CompareTo(mq.Expressions[0].MediaCombineOperator.Start, "and", ignoreCase: false));
            Assert.IsTrue(tp.CompareTo(mq.Expressions[0].MediaFeature.Start, "device-aspect-ratio", ignoreCase: false));
            Assert.IsNotNull(mq.Expressions[0].Colon);
            Assert.IsNotNull(mq.Expressions[0].CloseFunctionBrace);
            Assert.IsNotNull(tp.CompareTo(mq.Expressions[0].Values[0].Start, "16/9", ignoreCase: false));

            Assert.IsTrue(md.MediaQueries[1] is MediaQuery);
            mq = md.MediaQueries[1] as MediaQuery;
            Assert.IsNotNull(mq.Comma);
            Assert.IsTrue(tp.CompareTo(mq.MediaType.Start, "projection", ignoreCase: false));
            Assert.AreEqual(1, mq.Expressions.Count);
            Assert.IsTrue(tp.CompareTo(mq.Expressions[0].MediaCombineOperator.Start, "and", ignoreCase: false));
            Assert.IsTrue(tp.CompareTo(mq.Expressions[0].MediaFeature.Start, "color", ignoreCase: false));
            Assert.IsNull(mq.Expressions[0].Colon);
            Assert.IsNotNull(mq.Expressions[0].CloseFunctionBrace);
            Assert.AreEqual(0, mq.Expressions[0].Values.Count);

            Assert.IsNotNull(md.MediaBlock);
            Assert.IsTrue(md.MediaBlock is StyleSheet);
            Assert.AreEqual(4, md.MediaBlock.Children.Count);
            Assert.IsInstanceOfType(md.MediaBlock.Children[1], typeof(PageDirective));
            Assert.IsInstanceOfType(md.MediaBlock.Children[2], typeof(RuleSet));
            RuleSet rs = md.MediaBlock.Children[2] as RuleSet;

            Assert.IsTrue(tp.CompareTo(rs.Selectors[0].SimpleSelectors[0].Name.Start, "body", ignoreCase: false));
            Assert.IsTrue(tp.CompareTo(rs.Block.Declarations[0].PropertyName.Start, "background", ignoreCase: false));
            Assert.IsTrue(tp.CompareTo(rs.Block.Declarations[0].Values[0].Start, "lime", ignoreCase: false));
        }

        [TestMethod]
        public void MediaDirective_ParseTest2()
        {
            string test = @"@media only screen, foo { }";
            ITextProvider tp = new StringTextProvider(test);
            MediaDirective s = new MediaDirective();
            Assert.IsTrue(s.Parse(new ItemFactory(tp, null), tp, Helpers.MakeTokenStream(tp)));
            Assert.AreEqual(2, s.MediaQueries.Count);

            test = @"@media not print { .foo[bar] { foo:bar; } }";
            tp = new StringTextProvider(test);
            s = new MediaDirective();
            Assert.IsTrue(s.Parse(new ItemFactory(tp, null), tp, Helpers.MakeTokenStream(tp)));
            Assert.AreEqual(1, s.MediaQueries.Count);
        }
    }
}
