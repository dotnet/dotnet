// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems;
using Microsoft.Css.Parser.TreeItems.Comments;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.TreeItems
{
    [TestClass]
    public class CCommentTest : CssUnitTestBase
    {
        [TestMethod]
        public void CComment_ParseTest()
        {
            string text = "/* */";
            ITextProvider tp = new StringTextProvider(text);
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            CComment cc = new CComment();
            Assert.IsTrue(cc.Parse(new ItemFactory(tp, null), tp, tokens));
            Assert.AreEqual(2, cc.Children.Count);
            Assert.IsNull(cc.CommentText);
            Assert.AreEqual(CssTokenType.OpenCComment, ((TokenItem)cc.Children[0]).TokenType);
            Assert.AreEqual(CssTokenType.CloseCComment, ((TokenItem)cc.Children[1]).TokenType);

            text = "/* <!-- --> */";
            tp = new StringTextProvider(text);
            tokens = Helpers.MakeTokenStream(tp);
            cc = new CComment();
            Assert.IsTrue(cc.Parse(new ItemFactory(tp, null), tp, tokens));
            Assert.AreEqual(3, cc.Children.Count);
            Assert.AreEqual(CssTokenType.OpenCComment, ((TokenItem)cc.Children[0]).TokenType);
            Assert.AreEqual(CssTokenType.CommentText, ((TokenItem)cc.Children[1]).TokenType);
            Assert.AreEqual(CssTokenType.CloseCComment, ((TokenItem)cc.Children[2]).TokenType);

            text = "/* ";
            tp = new StringTextProvider(text);
            tokens = Helpers.MakeTokenStream(tp);
            cc = new CComment();
            Assert.IsTrue(cc.Parse(new ItemFactory(tp, null), tp, tokens));
            Assert.AreEqual(1, cc.Children.Count);
            Assert.IsNull(cc.CommentText);
            Assert.AreEqual(CssTokenType.OpenCComment, ((TokenItem)cc.Children[0]).TokenType);
            Assert.IsTrue(cc.HasParseErrors);
            Assert.AreEqual(ParseErrorType.CloseCommentMissing, cc.ParseErrors[0].ErrorType);

            text = "/*";
            tp = new StringTextProvider(text);
            tokens = Helpers.MakeTokenStream(tp);
            cc = new CComment();
            Assert.IsTrue(cc.Parse(new ItemFactory(tp, null), tp, tokens));
            Assert.AreEqual(1, cc.Children.Count);
            Assert.IsNull(cc.CommentText);
            Assert.IsTrue(cc.HasParseErrors);
            Assert.AreEqual(CssTokenType.OpenCComment, ((TokenItem)cc.Children[0]).TokenType);
            Assert.AreEqual(ParseErrorType.CloseCommentMissing, cc.ParseErrors[0].ErrorType);
        }
    }
}
