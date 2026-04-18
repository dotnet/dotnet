// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Linq;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems;
using Microsoft.Css.Parser.TreeItems.AtDirectives;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.TreeItems
{
    [TestClass]
    public class RuleBlockTest : CssUnitTestBase
    {
        [TestMethod]
        public void RuleBlock_ParseTest()
        {
            string text = "{ color:red; }";
            ITextProvider tp = new StringTextProvider(text);
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            RuleBlock rb = new RuleBlock();

            Assert.IsTrue(rb.Parse(new ItemFactory(tp, null), tp, tokens));

            Assert.AreEqual(0, rb.Start);
            Assert.AreEqual(14, rb.AfterEnd);

            Assert.AreEqual(CssTokenType.OpenCurlyBrace, rb.OpenCurlyBrace.TokenType);
            Assert.AreEqual(0, rb.OpenCurlyBrace.Start);
            Assert.AreEqual(1, rb.OpenCurlyBrace.Length);

            Assert.AreEqual(CssTokenType.CloseCurlyBrace, rb.CloseCurlyBrace.TokenType);
            Assert.AreEqual(13, rb.CloseCurlyBrace.Start);
            Assert.AreEqual(1, rb.CloseCurlyBrace.Length);

            Assert.AreEqual(3, rb.Children.Count);
            Assert.AreEqual(typeof(Declaration), rb.Children[1].GetType());
        }

        [TestMethod]
        public void RuleBlock_ContainsAtDirective()
        {
            string text = "{ color:red; @media screen { background: bad } }";
            ITextProvider tp = new StringTextProvider(text);
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            RuleBlock rb = new RuleBlock();

            Assert.IsTrue(rb.Parse(new ItemFactory(tp, null), tp, tokens));

            Assert.AreEqual(0, rb.Start);
            Assert.AreEqual(48, rb.AfterEnd);

            Assert.AreEqual(4, rb.Children.Count);
            Assert.IsInstanceOfType(rb.Children[2], typeof(MediaDirective));
        }

        [TestMethod]
        public void RuleBlock_ContainsCustomDeclaration()
        {
            string text = "{ color:red; --color: green; }";
            ITextProvider tp = new StringTextProvider(text);
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            RuleBlock rb = new RuleBlock();

            Assert.IsTrue(rb.Parse(new ItemFactory(tp, null), tp, tokens));

            Assert.AreEqual(0, rb.Start);
            Assert.AreEqual(30, rb.AfterEnd);

            Assert.AreEqual(4, rb.Children.Count);
            Assert.IsInstanceOfType(rb.Children[1], typeof(Declaration));
            Assert.IsFalse(((Declaration)rb.Children[1]).IsCustomProperty);
            Assert.IsInstanceOfType(rb.Children[2], typeof(Declaration));
            Assert.IsTrue(((Declaration)rb.Children[2]).IsCustomProperty);

            Assert.AreEqual(1, rb.CustomProperties.Count());
            Assert.AreEqual(rb.Children[2].Start, rb.CustomProperties.First().Start);
            Assert.AreEqual(rb.Children[2].AfterEnd, rb.CustomProperties.First().AfterEnd);
        }
    }
}
