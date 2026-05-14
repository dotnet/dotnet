// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Test;
using Microsoft.Css.Parser.TreeItems;
using Microsoft.Css.Parser.TreeItems.AtDirectives;
using Microsoft.Css.Parser.TreeItems.Comments;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Parser
{
    [TestClass]
    public class ParseItemCollectionTest : CssUnitTestBase
    {
        [TestMethod]
        public void ParseItemCollection_ConstructorTest()
        {
            ParseItemList pc = new ParseItemList();

            Assert.AreEqual(0, pc.TextStart);
            Assert.AreEqual(0, pc.TextLength);
            Assert.AreEqual(0, pc.Count);
            Assert.AreEqual(0, pc.FindInsertIndex(-10, beforeExisting: true));
            Assert.AreEqual(0, pc.FindInsertIndex(10, beforeExisting: false));
            Assert.AreEqual(-1, pc.IndexOf(new RuleSet()));
        }

        [TestMethod]
        public void ParseItemCollection_AddTest()
        {
            CssParser p = new CssParser();
            StyleSheet s = p.Parse("@charset \"foo\"; .a { } .b { } /* */", true);

            ParseItemList pc = new ParseItemList
            {
                s.Children[0]
            };

            Assert.AreSame(pc[0], s.Children[0]);
            Assert.AreEqual(typeof(CharsetDirective), pc[0].GetType());

            pc.Add(s.Children[s.Children.Count - 1]);
            Assert.AreEqual(typeof(CComment), pc[1].GetType());
            Assert.AreEqual(2, pc.Count);

            pc.Add(s.Children[1]);
            Assert.AreEqual(typeof(CharsetDirective), pc[0].GetType());
            Assert.AreEqual(typeof(RuleSet), pc[1].GetType());
            Assert.AreEqual(typeof(CComment), pc[2].GetType());
            Assert.AreEqual(3, pc.Count);

            Assert.AreEqual(0, pc.FindInsertIndex(0, beforeExisting: true));
            Assert.AreEqual(1, pc.FindInsertIndex(0, beforeExisting: false));

            Assert.AreEqual(0, pc.TextStart);
            Assert.AreEqual(s.Length, pc.TextLength);
        }

        [TestMethod]
        public void ParseItemCollection_InsertTest()
        {
            CssParser p = new CssParser();
            StyleSheet s = p.Parse("@charset \"foo\"; .a { } .b { } /* */", true);

            ParseItemList pc = new ParseItemList();

            pc.Insert(0, s.Children[0]);
            Assert.AreSame(pc[0], s.Children[0]);
            Assert.AreEqual(typeof(CharsetDirective), pc[0].GetType());

            pc.Insert(1, s.Children[s.Children.Count - 1]);
            Assert.AreEqual(typeof(CComment), pc[1].GetType());
            Assert.AreEqual(2, pc.Count);

            pc.Insert(1, s.Children[1]);
            Assert.AreEqual(typeof(CharsetDirective), pc[0].GetType());
            Assert.AreEqual(typeof(RuleSet), pc[1].GetType());
            Assert.AreEqual(typeof(CComment), pc[2].GetType());
            Assert.AreEqual(3, pc.Count);

            Assert.AreEqual(0, pc.TextStart);
            Assert.AreEqual(s.Length, pc.TextLength);
        }

        [TestMethod]
        public void ParseItemCollection_RemoveTest()
        {
            CssParser p = new CssParser();
            StyleSheet s = p.Parse("@charset \"foo\"; .a { } .b { } /* */", true);

            ParseItemList pc = new ParseItemList
            {
                s.Children[0]
            };

            Assert.IsTrue(pc.Remove(pc[0]));
            Assert.AreEqual(0, pc.Count);

            pc.Add(s.Children[0]);
            pc.Add(s.Children[s.Children.Count - 1]);
            Assert.AreEqual(2, pc.Count);
            Assert.AreEqual(typeof(CComment), pc[1].GetType());

            Assert.IsTrue(pc.Remove(s.Children[s.Children.Count - 1]));
            Assert.AreEqual(1, pc.Count);
            Assert.AreEqual(s.Children[0], pc[0]);
            Assert.AreEqual(typeof(CharsetDirective), pc[0].GetType());

            Assert.IsTrue(pc.Remove(s.Children[0]));
            Assert.AreEqual(0, pc.Count);

            Assert.IsFalse(pc.Remove(s.Children[0]));
            Assert.AreEqual(0, pc.Count);
        }
    }
}
