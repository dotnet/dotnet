// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.Css.Parser.Document;
using Microsoft.Css.Parser.Test;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems;
using Microsoft.Css.Parser.TreeItems.AtDirectives;
using Microsoft.Css.Parser.TreeItems.Comments;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Parser
{
    [TestClass]
    public class ComplexItemTest : CssUnitTestBase
    {
        [TestMethod]
        public void ComplexItemFromRangeTest()
        {
            CssParser p = new CssParser();
            StyleSheet s = p.Parse("@charset \"foo\"; .a { color: red; } moo boo", true);
            Assert.IsNull(s.ComplexItemFromRange(0, 0));
            Assert.IsTrue(s.ComplexItemFromRange(0, 2) is CharsetDirective);
            Assert.AreSame(s, s.ComplexItemFromRange(16, 0));
            Assert.IsTrue(s.ComplexItemFromRange(16, 4) is RuleSet);
            Assert.IsTrue(s.ComplexItemFromRange(20, 0) is RuleBlock);
            Assert.IsTrue(s.ComplexItemFromRange(20, 5) is RuleBlock);
            Assert.IsTrue(s.ComplexItemFromRange(33, 1) is RuleBlock);
            Assert.AreSame(s, s.ComplexItemFromRange(34, 0));
            Assert.IsTrue(s.ComplexItemFromRange(s.Start, s.Length) is StyleSheet);
            Assert.IsTrue(s.ComplexItemFromRange(s.AfterEnd - 1, 0) is ItemName);
            Assert.IsTrue(s.ComplexItemFromRange(s.AfterEnd - 2, 1) is ItemName);
        }

        [TestMethod]
        public void ItemBeforePositionTest()
        {
            CssParser p = new CssParser();
            StyleSheet s = p.Parse("@charset \"foo\"; .a { color: red; } moo boo", true);
            Assert.IsNull(s.ItemBeforePosition(0));

            TokenItem tokenItem = s.ItemBeforePosition(s.AfterEnd - 9) as TokenItem;
            Assert.IsNotNull(tokenItem);
            Assert.AreEqual(CssTokenType.Semicolon, tokenItem.TokenType);
            Assert.IsInstanceOfType(tokenItem.Parent, typeof(Declaration));

            // In the last token
            tokenItem = s.ItemBeforePosition(s.AfterEnd - 1) as TokenItem;
            Assert.IsNotNull(tokenItem);
            Assert.AreEqual(CssTokenType.Identifier, tokenItem.TokenType);
            Assert.AreEqual("boo", tokenItem.Text);
            Assert.IsInstanceOfType(tokenItem.Parent, typeof(ItemName));

            // After the last token
            tokenItem = s.ItemBeforePosition(s.AfterEnd + 9) as TokenItem;
            Assert.IsNotNull(tokenItem);
            Assert.AreEqual(CssTokenType.Identifier, tokenItem.TokenType);
            Assert.AreEqual("boo", tokenItem.Text);
            Assert.IsInstanceOfType(tokenItem.Parent, typeof(ItemName));

            // Between the first two tokens
            tokenItem = s.ItemBeforePosition(1) as TokenItem;
            Assert.IsNotNull(tokenItem);
            Assert.AreEqual(CssTokenType.At, tokenItem.TokenType);
            Assert.IsInstanceOfType(tokenItem.Parent, typeof(CharsetDirective));

            // Before the semicolon in the @charset
            tokenItem = s.ItemBeforePosition(14) as TokenItem;
            Assert.IsNotNull(tokenItem);
            Assert.AreEqual(CssTokenType.String, tokenItem.TokenType);
            Assert.IsInstanceOfType(tokenItem.Parent, typeof(CharsetDirective));
        }

        [TestMethod]
        public void ItemAfterPositionTest()
        {
            CssParser p = new CssParser();
            StyleSheet s = p.Parse("@charset \"foo\"; .a { color: red; } moo boo", true);
            Assert.IsNull(s.ItemAfterPosition(s.AfterEnd));

            TokenItem tokenItem = s.ItemAfterPosition(s.AfterEnd - 9) as TokenItem;
            Assert.IsNotNull(tokenItem);
            Assert.AreEqual(CssTokenType.CloseCurlyBrace, tokenItem.TokenType);

            // In the last token
            tokenItem = s.ItemAfterPosition(s.AfterEnd - 1) as TokenItem;
            Assert.IsNull(tokenItem);

            // Before the first token
            tokenItem = s.ItemAfterPosition(-8) as TokenItem;
            Assert.IsNotNull(tokenItem);
            Assert.AreEqual(CssTokenType.At, tokenItem.TokenType);

            // Between the first two tokens
            tokenItem = s.ItemAfterPosition(1) as TokenItem;
            Assert.IsNotNull(tokenItem);
            Assert.AreEqual(CssTokenType.Identifier, tokenItem.TokenType);
            Assert.AreEqual("charset", tokenItem.Text);

            // Before the semicolon in the @charset
            tokenItem = s.ItemAfterPosition(14) as TokenItem;
            Assert.IsNotNull(tokenItem);
            Assert.AreEqual(CssTokenType.Semicolon, tokenItem.TokenType);
        }

        [TestMethod]
        public void ItemFromRangeTest()
        {
            CssParser p = new CssParser();
            StyleSheet s = p.Parse("@charset \"foo\"; .a { color: red; } /* */ foo bar", true);
            Assert.IsNull(s.ItemFromRange(0, 0));
            Assert.AreEqual(0, s.ItemFromRange(0, 1).Start);
            Assert.AreEqual(1, s.ItemFromRange(1, 3).Start);
            Assert.AreEqual(9, s.ItemFromRange(11, 1).Start);
        }

        [TestMethod]
        public void ComplexItem_InsertChildTest()
        {
            CssParser p = new CssParser();
            StyleSheet s1 = p.Parse("@charset \"foo\"; .a { } .b { }        /* */", true);
            StyleSheet s2 = p.Parse("@charset \"foo\"; .a { } .b { } .c { } /* */ .d { }", true);

            s1.InsertChildIntoSubtree(s2.Children[3]);
            Assert.AreEqual(typeof(RuleSet), s1.Children[3].GetType());
            s1.InsertChildIntoSubtree(s2.Children[s2.Children.Count - 1]);
            Assert.AreEqual(typeof(RuleSet), s1.Children[s1.Children.Count - 1].GetType());
        }

        [TestMethod]
        public void ComplexItem_InsertCommentsTest()
        {
            string text = "@charset \"foo\"/* c1 */; .a { } .b { /* c2 */ }        /* c3 */";
            ITextProvider textProvider = new StringTextProvider(text);

            TokenList tokens = Helpers.MakeTokens(text);
            TokenList filteredTokens = new TokenList();

            CssParser parser = new CssParser();
            IList<Comment> comments = parser.ExtractComments(textProvider, tokens, 0, tokens.Count);

            foreach (CssToken token in tokens)
            {
                if (!token.IsComment)
                {
                    filteredTokens.Add(token);
                }
            }

            StyleSheet s = parser.Parse(textProvider, filteredTokens, insertComments: false);

            Assert.AreEqual(3, s.Children.Count);
            Assert.AreEqual(3, comments.Count);

            foreach (ParseItem comment in comments)
            {
                s.InsertChildIntoSubtree(comment);
            }

            Assert.AreEqual(4, s.Children.Count);
            Assert.IsTrue(s.Children[s.Children.Count - 1] is CComment);

            CharsetDirective cd = s.Children[0] as CharsetDirective;
            Assert.AreEqual(5, cd.Children.Count);
            Assert.IsTrue(cd.Children[3] is CComment);

            RuleSet rs = s.Children[2] as RuleSet;
            RuleBlock rb = rs.Block;
            Assert.AreEqual(3, rb.Children.Count);
            Assert.IsTrue(rb.Children[1] is CComment);
        }

        [TestMethod]
        public void ComplexItem_ItemFromPositionTest()
        {
            CssParser p = new CssParser();
            StyleSheet s = p.Parse("@charset \"foo\"; .a { color: red; } /* */", true);
            TokenItem ti = s.ItemFromRange(0, 1) as TokenItem;
            Assert.AreEqual(CssTokenType.At, ti.TokenType);
            ti = s.ItemFromRange(s.AfterEnd - 1, 0) as TokenItem;
            Assert.AreEqual(CssTokenType.CloseCComment, ti.TokenType);
            ti = s.ItemFromRange(16, 1) as TokenItem;
            Assert.AreEqual(CssTokenType.Dot, ti.TokenType);
        }

        [TestMethod]
        public void ComplexItem_NextPreviousChildTest()
        {
            CssTree doc = new CssTree(null)
            {
                TextProvider = new StringTextProvider("a{} b{} c{} d{}")
            };

            Assert.AreEqual(4, doc.StyleSheet.RuleSets.Count);

            Assert.IsNull(doc.StyleSheet.NextChild(null));
            Assert.IsNull(doc.StyleSheet.PreviousChild(null));

            Assert.IsNull(doc.StyleSheet.NextChild(doc.StyleSheet.RuleSets[3]));
            Assert.IsNull(doc.StyleSheet.PreviousChild(doc.StyleSheet.RuleSets[0]));

            Assert.AreEqual(doc.StyleSheet.RuleSets[3], doc.StyleSheet.NextChild(doc.StyleSheet.RuleSets[2]));
            Assert.AreEqual(doc.StyleSheet.RuleSets[2], doc.StyleSheet.PreviousChild(doc.StyleSheet.RuleSets[3]));
        }

        [TestMethod]
        public void ComplexItem_IsUnclosedTest()
        {
            CssTree doc = new CssTree(null)
            {
                TextProvider = new StringTextProvider("a{foo:bar} b{")
            };

            Assert.AreEqual(2, doc.StyleSheet.RuleSets.Count);
            Assert.AreEqual(1, doc.StyleSheet.RuleSets[0].Block.Declarations.Count);

            Assert.IsFalse(doc.StyleSheet.RuleSets[0].IsUnclosed);
            Assert.IsFalse(doc.StyleSheet.RuleSets[0].Selectors[0].IsUnclosed);
            Assert.IsFalse(doc.StyleSheet.RuleSets[0].Block.IsUnclosed);
            Assert.IsTrue(doc.StyleSheet.RuleSets[0].Block.Declarations[0].IsUnclosed);

            Assert.IsTrue(doc.StyleSheet.RuleSets[1].IsUnclosed);
            Assert.IsFalse(doc.StyleSheet.RuleSets[1].Selectors[0].IsUnclosed);
            Assert.IsTrue(doc.StyleSheet.RuleSets[1].Block.IsUnclosed);
        }

        [TestMethod]
        public void ComplexItem_IsParentTest()
        {
            StarHackPropertyName item1 = new StarHackPropertyName();
            StarHackPropertyName item2 = new StarHackPropertyName();
            item1.Children.Add(item2);

            Assert.IsFalse(item1.IsParentOf(null));
            Assert.IsFalse(item1.IsParentOf(item1));
            Assert.IsTrue(item1.IsParentOf(item2));
            Assert.IsFalse(item2.IsParentOf(item1));
        }
    }
}
