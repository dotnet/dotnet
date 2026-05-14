// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems.AtDirectives;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.TreeItems.AtDirectives
{
    [TestClass]
    public class PageDirectiveTest : CssUnitTestBase
    {
        [TestMethod]
        public void PageDirective_ParseTest()
        {
            string text = "@page { }";
            ITextProvider tp = new StringTextProvider(text);
            TokenStream ts = Helpers.MakeTokenStream(tp);
            PageDirective pd = new PageDirective();
            Assert.IsTrue(pd.Parse(new ItemFactory(tp, null), tp, ts));

            text = "@page:left { }";
            tp = new StringTextProvider(text);
            ts = Helpers.MakeTokenStream(tp);
            pd = new PageDirective();
            Assert.IsTrue(pd.Parse(new ItemFactory(tp, null), tp, ts));
            Assert.IsTrue(tp.CompareTo(pd.Keyword.Start, "page", ignoreCase: false));
            Assert.AreEqual("left", tp.GetText(pd.PseudoPage.Start, pd.PseudoPage.Length));
            Assert.IsNotNull(pd.Block.OpenCurlyBrace);
            Assert.IsNotNull(pd.Block.CloseCurlyBrace);

            text = "@page :right{ }";
            tp = new StringTextProvider(text);
            ts = Helpers.MakeTokenStream(tp);
            pd = new PageDirective();
            Assert.IsTrue(pd.Parse(new ItemFactory(tp, null), tp, ts));
            Assert.IsTrue(tp.CompareTo(pd.Keyword.Start, "page", ignoreCase: false));
            Assert.AreEqual("right", tp.GetText(pd.PseudoPage.Start, pd.PseudoPage.Length));
            Assert.IsNotNull(pd.Block.OpenCurlyBrace);
            Assert.IsNotNull(pd.Block.CloseCurlyBrace);

            text = "@page foo:first{ }";
            tp = new StringTextProvider(text);
            ts = Helpers.MakeTokenStream(tp);
            pd = new PageDirective();
            Assert.IsTrue(pd.Parse(new ItemFactory(tp, null), tp, ts));
            Assert.IsTrue(tp.CompareTo(pd.Keyword.Start, "page", ignoreCase: false));
            Assert.IsTrue(tp.CompareTo(pd.Identifier.Start, "foo", ignoreCase: false));
            Assert.AreEqual("first", tp.GetText(pd.PseudoPage.Start, pd.PseudoPage.Length));
            Assert.IsNotNull(pd.Block.OpenCurlyBrace);
            Assert.IsNotNull(pd.Block.CloseCurlyBrace);

            text = "@page :foo{ @top-left { } }";
            tp = new StringTextProvider(text);
            ts = Helpers.MakeTokenStream(tp);
            pd = new PageDirective();
            Assert.IsTrue(pd.Parse(new ItemFactory(tp, null), tp, ts));
            Assert.AreEqual(1, pd.PageDirectiveBlock.Margins.Count);
            Assert.AreEqual(MarginDirectiveType.TopLeft, pd.PageDirectiveBlock.Margins[0].DirectiveType);
            Assert.IsTrue(tp.CompareTo(pd.PageDirectiveBlock.Margins[0].Keyword.Start, "top-left", ignoreCase: false));

            text = "@page";
            tp = new StringTextProvider(text);
            ts = Helpers.MakeTokenStream(tp);
            pd = new PageDirective();
            Assert.IsTrue(pd.Parse(new ItemFactory(tp, null), tp, ts));
            Assert.IsNull(pd.Block);

            text = "@page :right{ @top-left { }";
            tp = new StringTextProvider(text);
            ts = Helpers.MakeTokenStream(tp);
            pd = new PageDirective();
            Assert.IsTrue(pd.Parse(new ItemFactory(tp, null), tp, ts));
            Assert.AreEqual(1, pd.PageDirectiveBlock.Margins.Count);
            Assert.IsTrue(tp.CompareTo(pd.PageDirectiveBlock.Margins[0].Keyword.Start, "top-left", ignoreCase: false));
            Assert.IsNotNull(pd.PageDirectiveBlock.Margins[0].RuleBlock.OpenCurlyBrace);
            Assert.IsNotNull(pd.PageDirectiveBlock.Margins[0].RuleBlock.CloseCurlyBrace);
            Assert.IsNotNull(pd.Block.OpenCurlyBrace);
            Assert.IsNull(pd.Block.CloseCurlyBrace);
        }
    }
}
