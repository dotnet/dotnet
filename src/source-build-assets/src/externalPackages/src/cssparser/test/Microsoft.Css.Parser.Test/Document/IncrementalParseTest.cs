// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Document;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.TreeItems;
using Microsoft.Css.Parser.TreeItems.AtDirectives;
using Microsoft.Css.Parser.TreeItems.Comments;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.Document
{
    [TestClass]
    public class IncrementalParseTest : CssUnitTestBase
    {
        [TestMethod]
        public void IncrementalParse_DestructiveChangeTest()
        {
            CssTree doc = new CssTree(new DefaultParserFactory());

            string text = "@import 'list.css' .a {color:red} z{} y{} x{}";
            string changedText = "/* comment */ .foo {hello:goodbye} z{} y{} x{}";
            doc.TextProvider = new StringTextProvider(text);

            bool fullTreeUpdateFired = false;
            CssItemsChangedEventArgs itemChangedEventArgs = null;

            doc.TreeUpdated += (object sender, CssTreeUpdateEventArgs eventArgs) =>
            {
                fullTreeUpdateFired = true;
            };

            doc.ItemsChanged += (object sender, CssItemsChangedEventArgs eventArgs) =>
            {
                itemChangedEventArgs = eventArgs;
            };

            doc.OnTextChange(new StringTextProvider(changedText), 0, text.Length - 12, changedText.Length - 12);

            Assert.IsFalse(fullTreeUpdateFired);
            Assert.IsNotNull(itemChangedEventArgs);
            Assert.AreEqual(2, itemChangedEventArgs.InsertedItems.Count);
            Assert.AreEqual(2, itemChangedEventArgs.DeletedItems.Count);
            Assert.AreEqual(0, itemChangedEventArgs.ErrorsChangedItems.Count);
            Assert.AreEqual(doc.StyleSheet, itemChangedEventArgs.InsertedItems[0].Parent);
        }

        [TestMethod]
        public void IncrementalParse_SimpleChangeTest()
        {
            CssTree doc = new CssTree(new DefaultParserFactory());
            ITextProvider text1 = new StringTextProvider(".foo { color: red } .bar { /* comment */ color: rgb(100 }");
            doc.TextProvider = text1;

            bool fullTreeUpdateFired = false;
            CssItemsChangedEventArgs changeEventArgs = null;

            doc.TreeUpdated += (object sender, CssTreeUpdateEventArgs eventArgs) =>
            {
                fullTreeUpdateFired = true;
            };

            doc.ItemsChanged += (object sender, CssItemsChangedEventArgs eventArgs) =>
            {
                changeEventArgs = eventArgs;
            };

            ITextProvider text2 = new StringTextProvider(".foo { color: BLUE; } .bar { /* comment */ color: rgb(100 }");
            doc.OnTextChange(text2, 14, 3, 5);

            Assert.IsFalse(fullTreeUpdateFired);
            Assert.IsNotNull(changeEventArgs);
            Assert.AreEqual(1, changeEventArgs.DeletedItems.Count);
            Assert.AreEqual(1, changeEventArgs.InsertedItems.Count);

            Declaration oldDecl = changeEventArgs.DeletedItems[0] as Declaration;
            Declaration newDecl = changeEventArgs.InsertedItems[0] as Declaration;

            Assert.AreEqual("color: red", text1.GetText(oldDecl.Start, oldDecl.Length));
            Assert.AreEqual("color: BLUE;", text2.GetText(newDecl.Start, newDecl.Length));
        }

        [TestMethod]
        public void IncrementalParse_InsertDeleteAndAppendRule()
        {
            CssTree doc = new CssTree(null);

            Assert.IsNull(doc.StyleSheet);
            doc.OnTextChange(new StringTextProvider("foo"), 0, 0, 3);
            // Changes are ignored before the initial parse
            Assert.IsNull(doc.StyleSheet);

            // Start with two rules
            doc.TextProvider = new StringTextProvider("a { } b { }");
            Assert.AreEqual("a { }", doc.StyleSheet.RuleSets[0].Text);
            Assert.AreEqual("b { }", doc.StyleSheet.RuleSets[1].Text);

            // Insert a rule "c"
            doc.OnTextChange(new StringTextProvider("a { } c { } b { }"), 6, 0, 6);
            Assert.AreEqual("a { }", doc.StyleSheet.RuleSets[0].Text);
            Assert.AreEqual("c { }", doc.StyleSheet.RuleSets[1].Text);
            Assert.AreEqual("b { }", doc.StyleSheet.RuleSets[2].Text);

            // Delete the rule "c" (and rename "b")
            doc.OnTextChange(new StringTextProvider("a { } bb { }"), 6, 6, 1);
            Assert.AreEqual("a { }", doc.StyleSheet.RuleSets[0].Text);
            Assert.AreEqual("bb { }", doc.StyleSheet.RuleSets[1].Text);

            // Append a rule "c"
            doc.OnTextChange(new StringTextProvider("a { } bb { } c { }"), 11, 1, 7);
            Assert.AreEqual("a { }", doc.StyleSheet.RuleSets[0].Text);
            Assert.AreEqual("bb { }", doc.StyleSheet.RuleSets[1].Text);
            Assert.AreEqual("c { }", doc.StyleSheet.RuleSets[2].Text);
        }

        [TestMethod]
        public void IncrementalParse_Comments()
        {
            CssTree doc = new CssTree(null)
            {
                // Start with some rules
                TextProvider = new StringTextProvider("a { } b { }")
            };

            Assert.AreEqual("a { }", doc.StyleSheet.RuleSets[0].Text);
            Assert.AreEqual("b { }", doc.StyleSheet.RuleSets[1].Text);

            // Start to comment them out
            doc.OnTextChange(new StringTextProvider("/* a { } b { }"), 0, 0, 3);
            Assert.AreEqual(1, doc.StyleSheet.Children.Count);
            Assert.IsInstanceOfType(doc.StyleSheet.Children[0], typeof(CComment));

            // Finish commenting them out
            doc.OnTextChange(new StringTextProvider("/* a { } b { } */"), 14, 0, 3);
            Assert.AreEqual(1, doc.StyleSheet.Children.Count);
            Assert.IsInstanceOfType(doc.StyleSheet.Children[0], typeof(CComment));

            // Start to uncomment
            doc.OnTextChange(new StringTextProvider("* a { } b { } */"), 0, 1, 0);
            Assert.AreEqual(3, doc.StyleSheet.Children.Count);
            Assert.AreEqual("* a { }", doc.StyleSheet.Children[0].Text);
            Assert.AreEqual("b { }", doc.StyleSheet.Children[1].Text);
            Assert.AreEqual("*/", doc.StyleSheet.Children[2].Text);
            Assert.IsInstanceOfType(doc.StyleSheet.Children[0], typeof(RuleSet));
            Assert.IsInstanceOfType(doc.StyleSheet.Children[1], typeof(RuleSet));
            Assert.IsInstanceOfType(doc.StyleSheet.Children[2], typeof(RuleSet));

            // Finish uncommenting (and delete "b")
            doc.OnTextChange(new StringTextProvider("* a { }"), 7, 9, 0);
            Assert.AreEqual(1, doc.StyleSheet.Children.Count);
            Assert.AreEqual("* a { }", doc.StyleSheet.Children[0].Text);
            Assert.IsInstanceOfType(doc.StyleSheet.Children[0], typeof(RuleSet));

            // Start over
            doc.TextProvider = new StringTextProvider("a { } /* foo */ b { }");
            Assert.AreEqual("a { }", doc.StyleSheet.Children[0].Text);
            Assert.AreEqual("/* foo */", doc.StyleSheet.Children[1].Text);
            Assert.AreEqual("b { }", doc.StyleSheet.Children[2].Text);
            Assert.IsInstanceOfType(doc.StyleSheet.Children[0], typeof(RuleSet));
            Assert.IsInstanceOfType(doc.StyleSheet.Children[1], typeof(CComment));
            Assert.IsInstanceOfType(doc.StyleSheet.Children[2], typeof(RuleSet));

            // Add text in the comment
            doc.OnTextChange(new StringTextProvider("a { } /* barfoo */ b { }"), 9, 3, 6);
            Assert.AreEqual("a { }", doc.StyleSheet.Children[0].Text);
            Assert.AreEqual("/* barfoo */", doc.StyleSheet.Children[1].Text);
            Assert.AreEqual("b { }", doc.StyleSheet.Children[2].Text);
            Assert.IsInstanceOfType(doc.StyleSheet.Children[0], typeof(RuleSet));
            Assert.IsInstanceOfType(doc.StyleSheet.Children[1], typeof(CComment));
            Assert.IsInstanceOfType(doc.StyleSheet.Children[2], typeof(RuleSet));
        }

        [TestMethod]
        public void IncrementalParse_Whitespace()
        {
            CssTree doc = new CssTree(null);
            DebugWriter writer = new DebugWriter();

            // Start with some rules
            doc.TextProvider = new StringTextProvider("a { } b { }");
            Assert.AreEqual(2, doc.StyleSheet.Children.Count);
            string origTree = writer.Serialize(doc.TextProvider, doc.StyleSheet);

            // Add space between them
            doc.OnTextChange(new StringTextProvider("a { } \r\n b { }"), 6, 0, 3);
            Assert.AreEqual(2, doc.StyleSheet.Children.Count);
            Assert.AreEqual(origTree, writer.Serialize(doc.TextProvider, doc.StyleSheet));

            // Delete spaces between them
            doc.OnTextChange(new StringTextProvider("a { } b { }"), 6, 3, 0);
            Assert.AreEqual(2, doc.StyleSheet.Children.Count);
            Assert.AreEqual(origTree, writer.Serialize(doc.TextProvider, doc.StyleSheet));

            // Change all the text, but only really add one more space
            doc.OnTextChange(new StringTextProvider("a { } b {  }"), 0, 11, 12);
            Assert.AreEqual(2, doc.StyleSheet.Children.Count);
            Assert.AreEqual(origTree, writer.Serialize(doc.TextProvider, doc.StyleSheet));
        }

        [TestMethod]
        public void IncrementalParse_EmptyComment()
        {
            CssTree doc = new CssTree(null)
            {
                TextProvider = new StringTextProvider("/**/")
            };

            Assert.AreEqual(1, doc.StyleSheet.Children.Count);
            Assert.IsInstanceOfType(doc.StyleSheet.Children[0], typeof(CComment));
            Assert.IsNull(((CComment)doc.StyleSheet.Children[0]).CommentText);
            Assert.AreEqual(4, doc.StyleSheet.Children[0].Length);

            // Delete the last slash
            doc.OnTextChange(new StringTextProvider("/**"), 3, 1, 0);
            Assert.AreEqual(1, doc.StyleSheet.Children.Count);
            Assert.IsInstanceOfType(doc.StyleSheet.Children[0], typeof(CComment));
            Assert.AreEqual("*", ((CComment)doc.StyleSheet.Children[0]).CommentText.Text);
            Assert.AreEqual(3, doc.StyleSheet.Children[0].Length);

            // Delete the last star
            doc.OnTextChange(new StringTextProvider("/*"), 2, 1, 0);
            Assert.AreEqual(1, doc.StyleSheet.Children.Count);
            Assert.IsInstanceOfType(doc.StyleSheet.Children[0], typeof(CComment));
            Assert.IsNull(((CComment)doc.StyleSheet.Children[0]).CommentText);
            Assert.AreEqual(2, doc.StyleSheet.Children[0].Length);

            // Delete the last star
            doc.OnTextChange(new StringTextProvider("/"), 1, 1, 0);
            Assert.AreEqual(1, doc.StyleSheet.Children.Count);
            Assert.IsInstanceOfType(doc.StyleSheet.Children[0], typeof(RuleSet));
            Assert.AreEqual(1, doc.StyleSheet.Children[0].Length);

            // Delete the last slash
            doc.OnTextChange(new StringTextProvider(""), 0, 1, 0);
            Assert.AreEqual(0, doc.StyleSheet.Children.Count);
            Assert.AreEqual(0, doc.StyleSheet.Length);
        }

        [TestMethod]
        public void IncrementalParse_UnclosedRule()
        {
            CssTree doc = new CssTree(null)
            {
                // Make an unclosed rule and look for the right parse error
                TextProvider = new StringTextProvider("p { color: red; /* Comment }")
            };

            Assert.AreEqual(1, doc.StyleSheet.Children.Count);
            RuleSet rs = doc.StyleSheet.Children[0] as RuleSet;
            Assert.IsNotNull(rs);
            Assert.IsTrue(rs.IsUnclosed);

            Assert.IsFalse(rs.HasParseErrors);
            Assert.IsTrue(rs.Block.HasParseErrors);
            Assert.IsTrue(rs.Block.ParseErrors[0].ErrorType == ParseErrorType.CloseCurlyBraceMissing);
            Assert.IsTrue(rs.Block.ParseErrors[0].Location == ParseErrorLocation.AfterItem);

            Assert.IsInstanceOfType(doc.StyleSheet.ItemFromRange(16, 10), typeof(CComment));
            Assert.IsInstanceOfType(doc.StyleSheet.ItemFromRange(16, 10).Parent, typeof(RuleBlock));

            // Close the rule (make sure the error goes away)

            void changedHandler(object sender, CssItemsChangedEventArgs eventArgs)
            {
                // The change is crafted in such a way that the incremental parse is
                // confined to the rule block that has a missing curly brace, but now
                // the curly brace is detected and the parse error goes away.

                Assert.AreEqual(2, eventArgs.DeletedItems.Count);
                Assert.IsInstanceOfType(eventArgs.DeletedItems[0], typeof(Declaration));
                Assert.IsInstanceOfType(eventArgs.DeletedItems[1], typeof(CComment));

                Assert.AreEqual(3, eventArgs.InsertedItems.Count);
                Assert.IsInstanceOfType(eventArgs.InsertedItems[0], typeof(Declaration));
                Assert.IsInstanceOfType(eventArgs.InsertedItems[1], typeof(CComment));
                Assert.IsInstanceOfType(eventArgs.InsertedItems[2], typeof(TokenItem));

                Assert.AreEqual(1, eventArgs.ErrorsChangedItems.Count);
                Assert.AreSame(rs.Block, eventArgs.ErrorsChangedItems[0]);
            }

            doc.ItemsChanged += changedHandler;
            doc.OnTextChange(new StringTextProvider("p { color: red; /* Comment */ }"), 27, 0, 3);
            doc.ItemsChanged -= changedHandler;

            Assert.AreEqual(1, doc.StyleSheet.Children.Count);
            Assert.IsFalse(rs.IsUnclosed);
            Assert.IsFalse(rs.HasParseErrors);
            Assert.IsFalse(rs.Block.HasParseErrors);

            Assert.IsInstanceOfType(doc.StyleSheet.ItemFromRange(16, 10), typeof(CComment));
            Assert.IsInstanceOfType(doc.StyleSheet.ItemFromRange(16, 10).Parent, typeof(RuleBlock));

            // Open the rule again

            doc.OnTextChange(new StringTextProvider("p { color: red; /* Comment"), 26, 5, 0);
            Assert.AreEqual(1, doc.StyleSheet.Children.Count);
            rs = doc.StyleSheet.Children[0] as RuleSet;
            Assert.IsNotNull(rs);
            Assert.IsTrue(rs.IsUnclosed);

            Assert.IsFalse(rs.HasParseErrors);
            Assert.IsTrue(rs.Block.HasParseErrors);
            Assert.IsTrue(rs.Block.ParseErrors[0].ErrorType == ParseErrorType.CloseCurlyBraceMissing);
            Assert.IsTrue(rs.Block.ParseErrors[0].Location == ParseErrorLocation.AfterItem);

            CComment comment = doc.StyleSheet.ItemFromRange(16, 10) as CComment;
            Assert.IsNotNull(comment);
            Assert.IsInstanceOfType(comment.Parent, typeof(RuleBlock));
            Assert.IsTrue(comment.IsUnclosed);
        }

        [TestMethod]
        public void IncrementalParse_UnclosedMedia()
        {
            // http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems?id=331654: Closing a comment didn't allow the exposed curly brace
            // to close the prevoius @media block.

            CssTree doc = new CssTree(null)
            {
                TextProvider = new StringTextProvider("@media { /*foo*/ /* }")
            };

            Assert.AreEqual(1, doc.StyleSheet.Children.Count);
            MediaDirective md = doc.StyleSheet.Children[0] as MediaDirective;
            Assert.IsNotNull(md);
            Assert.IsTrue(md.IsUnclosed); // this was the bug, directives never said they were unclosed
            Assert.IsTrue(md.Block.HasParseErrors);

            doc.OnTextChange(new StringTextProvider("@media { /*foo*/ /**/ }"), 19, 0, 2);

            Assert.AreEqual(1, doc.StyleSheet.Children.Count);
            Assert.IsFalse(md.IsUnclosed);
            Assert.IsFalse(md.Block.HasParseErrors);
            Assert.AreEqual(7, md.Block.Start);
            Assert.AreEqual(23, md.Block.AfterEnd);
        }
    }
}
