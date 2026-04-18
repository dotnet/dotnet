// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Classify;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems;
using Microsoft.Css.Parser.TreeItems.AtDirectives;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Parser
{
    [TestClass]
    public class ParseItemTest
    {
        [TestMethod]
        public void ParseItem_ContainsTest()
        {
            CssParser p = new CssParser();
            StyleSheet s = p.Parse("@charset \"foo\"; .a { } .b { } /* */", true);

            Assert.IsTrue(TextRange.ContainsChar(s.Start, s.Length, 0));
            Assert.IsTrue(TextRange.ContainsChar(s.Start, s.Length, s.Start));
            Assert.IsTrue(TextRange.ContainsChar(s.Start, s.Length, s.AfterEnd - 1));
            Assert.IsTrue(TextRange.ContainsChar(s.Start, s.Length, (s.Start + s.AfterEnd) / 2));
            Assert.IsFalse(TextRange.ContainsChar(s.Start, s.Length, s.AfterEnd));
            Assert.IsFalse(TextRange.ContainsChar(s.Start, s.Length, (s.Start - 1)));

            Assert.IsTrue(TextRange.Touches(s.Start, s.Length, s.Start));
            Assert.IsTrue(TextRange.Touches(s.Start, s.Length, s.AfterEnd));
            Assert.IsFalse(TextRange.Touches(s.Start, s.Length, s.Start - 1));
            Assert.IsFalse(TextRange.Touches(s.Start, s.Length, s.AfterEnd + 1));
        }

        [TestMethod]
        public void ParseItem_ClassifierContextTest()
        {
            CssParser p = new CssParser();
            StyleSheet s = p.Parse("@media { foo }", false);

            Assert.IsTrue(s.Children[0].Context.IsDefault());
            Assert.IsTrue(((ComplexItem)s.Children[0]).Children[0].Context.IsEqualTo((int)CssClassifierContextType.AtDirectiveName, typeof(CssClassifierContextType)));
            Assert.IsTrue(s.Children[0] is AtBlockDirective);

            s.Children[0].Context = CssClassifierContextCache.FromTypeEnum(CssClassifierContextType.PercentUnits);
            Assert.IsTrue(s.Children[0].Context.IsEqualTo((int)CssClassifierContextType.PercentUnits, typeof(CssClassifierContextType)));
            Assert.IsTrue(s.Children[0] is AtBlockDirective);
        }

        [TestMethod]
        public void ParseItem_IntersectTest()
        {
            CssParser p = new CssParser();
            StyleSheet s = p.Parse("@charset \"foo\"; .a { color: red; } /* */", true);
            Assert.IsTrue(TextRange.Intersects(s.Start, s.Length, 0, 2));
            Assert.IsTrue(TextRange.Intersects(s.Start, s.Length, s.Start, s.Length));
            Assert.IsFalse(TextRange.Intersects(s.Start, s.Length, -3, 2));
            Assert.IsFalse(TextRange.Intersects(s.Start, s.Length, s.AfterEnd, 4));
            Assert.IsTrue(TextRange.Intersects(s.Start, s.Length, s.Start - 1, 2));
            Assert.IsTrue(TextRange.Intersects(s.Start, s.Length, s.AfterEnd - 2, 2));
        }


        [TestMethod]
        public void ParseItem_EndTest()
        {
            CssParser p = new CssParser();
            StyleSheet s = p.Parse("", true);
            Assert.AreEqual(0, s.AfterEnd);
            s = p.Parse(".a{}", true);
            Assert.AreEqual(4, s.AfterEnd);
            s = p.Parse(".a{}    ", true);
            Assert.AreEqual(4, s.AfterEnd);
        }

        [TestMethod]
        public void ParseItem_LengthTest()
        {
            CssParser p = new CssParser();
            StyleSheet s = p.Parse("", true);
            Assert.AreEqual(0, s.Length);
            s = p.Parse(".a{}", true);
            Assert.AreEqual(4, s.Length);
            s = p.Parse(".a{}    ", true);
            Assert.AreEqual(4, s.Length);
        }

        [TestMethod]
        public void ParseItem_ParentTest()
        {
            CssParser p = new CssParser();
            StyleSheet s = p.Parse(".a{}", true);
            Assert.AreEqual(null, s.Parent);

            foreach (ComplexItem ci in s.Children)
            {
                Assert.AreEqual(s, ci.Parent);
            }
        }

        [TestMethod]
        public void ParseItem_StartTest()
        {
            CssParser p = new CssParser();
            StyleSheet s = p.Parse("", true);
            Assert.AreEqual(0, s.Start);
            s = p.Parse(".a{}", true);
            Assert.AreEqual(0, s.Start);
            s = p.Parse("    .a{}    ", true);
            Assert.AreEqual(4, s.Start);
        }

        [TestMethod]
        public void ParseItem_TextTest()
        {
            CssParser p = new CssParser();
#if SUPPORT_ENCODED_CSS
            string text = "@\\me\\64 ia { foo }";
#else
            string text = "@media { foo }";
#endif

            StyleSheet s = p.Parse(text, false);
            Assert.AreEqual(text, s.Children[0].Text);

            CharacterStream iter = new CharacterStream(s.TextProvider);
            Assert.IsTrue(TextHelpers.CompareCurrentDecodedString(iter, "@media", ignoreCase: false, matchLength: out int matchLength));

#if SUPPORT_ENCODED_CSS
            Assert.AreEqual(10, matchLength);
#else
            Assert.AreEqual(6, matchLength);
#endif
        }

        [TestMethod]
        public void ParseItem_AddParseErrorTest()
        {
            CharsetDirective cd = new CharsetDirective();

            Assert.IsFalse(cd.HasParseErrors);
            Assert.IsFalse(cd.ContainsParseErrors);
            Assert.AreEqual(0, cd.ParseErrors.Count);

            cd.Children.AddParseError(ParseErrorType.AtDirectiveSemicolonMissing);
            Assert.IsTrue(cd.HasParseErrors);
            Assert.IsTrue(cd.ContainsParseErrors);
            Assert.AreEqual(1, cd.ParseErrors.Count);
            Assert.AreEqual(ParseErrorType.AtDirectiveSemicolonMissing, cd.ParseErrors[0].ErrorType);
            Assert.AreEqual(ParseErrorLocation.BeforeItem, cd.ParseErrors[0].Location);

            cd.AddParseError(ParseErrorType.UnexpectedToken, ParseErrorLocation.WholeItem);
            Assert.AreEqual(2, cd.ParseErrors.Count);
            Assert.AreEqual(ParseErrorType.UnexpectedToken, cd.ParseErrors[1].ErrorType);
            Assert.AreEqual(ParseErrorLocation.WholeItem, cd.ParseErrors[1].Location);

            TokenItem ti = new TokenItem(new CssToken(CssTokenType.At, 0, 0), null);
            Assert.IsFalse(ti.HasParseErrors);
            Assert.AreEqual(0, ti.ParseErrors.Count);

            cd.Children.Add(ti);
            cd.Children.AddParseError(ParseErrorType.AtDirectiveNameMissing);
            Assert.AreEqual(2, cd.ParseErrors.Count);
            Assert.AreEqual(1, ti.ParseErrors.Count);
            Assert.AreEqual(ParseErrorType.AtDirectiveNameMissing, ti.ParseErrors[0].ErrorType);
            Assert.AreEqual(ParseErrorLocation.AfterItem, ti.ParseErrors[0].Location);
        }
    }
}
