// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems;
using Microsoft.Css.Parser.TreeItems.Functions;
using Microsoft.Css.Parser.TreeItems.PropertyValues;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.TreeItems
{
    [TestClass]
    public class DeclarationTest : CssUnitTestBase
    {
        [TestMethod]
        public void Declaration_ParseTest1()
        {
            string text = "color:red;";
            ITextProvider tp = new StringTextProvider(text);
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            Declaration d = new Declaration();

            Assert.IsTrue(d.Parse(new ItemFactory(tp, null), tp, tokens));

            Assert.AreEqual(0, d.Start);
            Assert.AreEqual(text.Length, d.AfterEnd);

            Assert.AreEqual(4, d.Children.Count);
            Assert.AreEqual(typeof(TokenItem), d.Semicolon.GetType());

            Assert.AreEqual(typeof(TokenItem), d.Children[0].GetType());
            Assert.AreEqual(CssTokenType.Identifier, ((TokenItem)d.Children[0]).Token.TokenType);

            Assert.AreEqual(typeof(TokenItem), d.Children[1].GetType());
            Assert.AreEqual(CssTokenType.Colon, ((TokenItem)d.Children[1]).Token.TokenType);

            Assert.AreEqual(typeof(TokenItem), d.Children[2].GetType());
            Assert.AreEqual(CssTokenType.Identifier, ((TokenItem)d.Children[2]).Token.TokenType);

            Assert.AreEqual(typeof(TokenItem), d.Children[d.Children.Count - 1].GetType());
            Assert.AreEqual(CssTokenType.Semicolon, ((TokenItem)d.Children[d.Children.Count - 1]).Token.TokenType);

            Assert.AreEqual(1, d.Values.Count);
            Assert.AreEqual(d.Children[2], d.Values[0]);
        }

        [TestMethod]
        public void Declaration_ParseTest2()
        {
            string text = "-moz-image: url(image.jpg) -25 bar baz";
            ITextProvider tp = new StringTextProvider(text);
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            Declaration d = new Declaration();

            Assert.IsTrue(d.Parse(new ItemFactory(tp, null), tp, tokens));

            Assert.AreEqual(0, d.Start);
            Assert.AreEqual(text.Length, d.AfterEnd);

            Assert.AreEqual(6, d.Children.Count);
            Assert.AreEqual(null, d.Semicolon);

            Assert.IsInstanceOfType(d.Children[0], typeof(TokenItem));
            TokenItem name = (TokenItem)d.Children[0];
            Assert.AreEqual(CssTokenType.Identifier, name.Token.TokenType);

            Assert.AreEqual(typeof(TokenItem), d.Children[1].GetType());
            Assert.AreEqual(CssTokenType.Colon, ((TokenItem)d.Children[1]).Token.TokenType);

            Assert.AreEqual(typeof(UrlItem), d.Children[2].GetType());
            Assert.AreEqual(3, ((UrlItem)d.Children[2]).Children.Count);

            Assert.AreEqual(typeof(NumericalValue), d.Children[3].GetType());
        }

        [TestMethod]
        public void Declaration_ParseTest3()
        {
            string text = "background-image:";
            ITextProvider tp = new StringTextProvider(text);
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            Declaration d = new Declaration();

            Assert.IsTrue(d.Parse(new ItemFactory(tp, null), tp, tokens));

            Assert.AreEqual(0, d.Start);
            Assert.AreEqual(text.Length, d.AfterEnd);

            Assert.AreEqual(2, d.Children.Count);
            Assert.AreEqual(null, d.Semicolon);

            Assert.AreEqual(typeof(TokenItem), d.Children[0].GetType());
            Assert.AreEqual(CssTokenType.Identifier, ((TokenItem)d.Children[0]).Token.TokenType);

            Assert.AreEqual(typeof(TokenItem), d.Children[1].GetType());
            Assert.AreEqual(CssTokenType.Colon, ((TokenItem)d.Children[1]).Token.TokenType);
        }

        [TestMethod]
        public void Declaration_ParseTest4()
        {
            string text = "background-image: }";
            ITextProvider tp = new StringTextProvider(text);
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            Declaration d = new Declaration();

            Assert.IsTrue(d.Parse(new ItemFactory(tp, null), tp, tokens));

            Assert.AreEqual(0, d.Start);
            Assert.AreEqual(text.Length - 2, d.AfterEnd);

            Assert.AreEqual(2, d.Children.Count);
            Assert.AreEqual(null, d.Semicolon);

            Assert.AreEqual(typeof(TokenItem), d.Children[0].GetType());
            Assert.AreEqual(CssTokenType.Identifier, ((TokenItem)d.Children[0]).Token.TokenType);

            Assert.AreEqual(typeof(TokenItem), d.Children[1].GetType());
            Assert.AreEqual(CssTokenType.Colon, ((TokenItem)d.Children[1]).Token.TokenType);
        }

        [TestMethod]
        public void Declaration_ParseTest5()
        {
            string text = "background-image: url(\"www.microsoft.com\" }";
            ITextProvider tp = new StringTextProvider(text);
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            Declaration d = new Declaration();

            Assert.IsTrue(d.Parse(new ItemFactory(tp, null), tp, tokens));

            Assert.AreEqual(0, d.Start);
            Assert.AreEqual(text.Length - 2, d.AfterEnd);

            Assert.AreEqual(3, d.Children.Count);
            Assert.AreEqual(null, d.Semicolon);

            Assert.AreEqual(typeof(TokenItem), d.Children[0].GetType());
            Assert.AreEqual(CssTokenType.Identifier, ((TokenItem)d.Children[0]).Token.TokenType);

            Assert.AreEqual(typeof(TokenItem), d.Children[1].GetType());
            Assert.AreEqual(CssTokenType.Colon, ((TokenItem)d.Children[1]).Token.TokenType);

            Assert.AreEqual(typeof(UrlItem), d.Children[2].GetType());
            Assert.AreEqual(2, ((UrlItem)d.Children[2]).Children.Count);
        }

        [TestMethod]
        public void Declaration_ParseTest6()
        {
            string text = "color: red !;";
            ITextProvider tp = new StringTextProvider(text);
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            Declaration d = new Declaration();

            Assert.IsTrue(d.Parse(new ItemFactory(tp, null), tp, tokens));

            Assert.AreEqual(0, d.Start);
            Assert.AreEqual(text.Length, d.AfterEnd);

            Assert.AreEqual(5, d.Children.Count);
            Assert.AreEqual(d.Children[d.Children.Count - 1], d.Semicolon);

            Assert.AreEqual(typeof(TokenItem), d.Children[0].GetType());
            Assert.AreEqual(CssTokenType.Identifier, ((TokenItem)d.Children[0]).Token.TokenType);

            Assert.AreEqual(typeof(TokenItem), d.Children[1].GetType());
            Assert.AreEqual(CssTokenType.Colon, ((TokenItem)d.Children[1]).Token.TokenType);
        }

        [TestMethod]
        public void Declaration_ParseTest7()
        {
            string text = "color: rgb(0, 50%, 124) !important";
            ITextProvider tp = new StringTextProvider(text);
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            Declaration d = new Declaration();

            Assert.IsTrue(d.Parse(new ItemFactory(tp, null), tp, tokens));

            Assert.AreEqual(0, d.Start);
            Assert.AreEqual(text.Length, d.AfterEnd);

            Assert.AreEqual(5, d.Children.Count);
            Assert.AreEqual(null, d.Semicolon);

            Assert.AreEqual(typeof(TokenItem), d.Children[0].GetType());
            Assert.AreEqual(CssTokenType.Identifier, ((TokenItem)d.Children[0]).Token.TokenType);

            Assert.AreEqual(typeof(TokenItem), d.Children[1].GetType());
            Assert.AreEqual(CssTokenType.Colon, ((TokenItem)d.Children[1]).Token.TokenType);
        }

        [TestMethod]
        public void Declaration_ParseTest8()
        {
            string text = "color: #f00 #010203 attr(foo) !important}";
            ITextProvider tp = new StringTextProvider(text);
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            Declaration d = new Declaration();

            Assert.IsTrue(d.Parse(new ItemFactory(tp, null), tp, tokens));

            Assert.AreEqual(0, d.Start);
            Assert.AreEqual(text.Length - 1, d.AfterEnd);

            Assert.AreEqual(7, d.Children.Count);
            Assert.AreEqual(null, d.Semicolon);

            Assert.AreEqual(typeof(TokenItem), d.Children[0].GetType());
            Assert.AreEqual(CssTokenType.Identifier, ((TokenItem)d.Children[0]).Token.TokenType);

            Assert.AreEqual(typeof(TokenItem), d.Children[1].GetType());
            Assert.AreEqual(CssTokenType.Colon, ((TokenItem)d.Children[1]).Token.TokenType);
        }

        [TestMethod]
        public void Declaration_ParseTest_CustomPropertyName()
        {
            string text = "--color: blue;";
            ITextProvider tp = new StringTextProvider(text);
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            Declaration d = new Declaration();

            Assert.IsTrue(d.Parse(new ItemFactory(tp, null), tp, tokens));
            Assert.IsTrue(d.IsCustomProperty);

            Assert.AreEqual(0, d.Start);
            Assert.AreEqual(text.Length, d.AfterEnd);

            Assert.AreEqual(4, d.Children.Count);
            Assert.AreEqual(typeof(TokenItem), d.Semicolon.GetType());

            Assert.AreEqual(typeof(TokenItem), d.Children[0].GetType());
            Assert.AreEqual(CssTokenType.Identifier, ((TokenItem)d.Children[0]).Token.TokenType);

            Assert.AreEqual(typeof(TokenItem), d.Children[1].GetType());
            Assert.AreEqual(CssTokenType.Colon, ((TokenItem)d.Children[1]).Token.TokenType);

            Assert.AreEqual(typeof(TokenItem), d.Children[2].GetType());
            Assert.AreEqual(CssTokenType.Identifier, ((TokenItem)d.Children[2]).Token.TokenType);

            Assert.AreEqual(typeof(TokenItem), d.Children[d.Children.Count - 1].GetType());
            Assert.AreEqual(CssTokenType.Semicolon, ((TokenItem)d.Children[d.Children.Count - 1]).Token.TokenType);

            Assert.AreEqual(1, d.Values.Count);
            Assert.AreEqual(d.Children[2], d.Values[0]);
        }

        [TestMethod]
        public void Declaration_ParseTest_CustomPropertyValue_WithBracedBlock()
        {
            string text = "--paper-toolbar: { border: none; };";
            ITextProvider tp = new StringTextProvider(text);
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            Declaration d = new Declaration();

            Assert.IsTrue(d.Parse(new ItemFactory(tp, null), tp, tokens));
            Assert.IsTrue(d.IsCustomProperty);

            Assert.IsFalse(d.HasParseErrors);

            Assert.AreEqual(0, d.Start);
            Assert.AreEqual(text.Length, d.AfterEnd);

            Assert.AreEqual(4, d.Children.Count);
            Assert.AreEqual(typeof(TokenItem), d.Semicolon.GetType());
            Assert.AreEqual(34, d.Semicolon.Start); // make sure we have the correct semicolon

            Assert.AreEqual(typeof(TokenItem), d.Children[0].GetType());
            Assert.AreEqual(CssTokenType.Identifier, ((TokenItem)d.Children[0]).Token.TokenType);

            Assert.AreEqual(typeof(TokenItem), d.Children[1].GetType());
            Assert.AreEqual(CssTokenType.Colon, ((TokenItem)d.Children[1]).Token.TokenType);

            Assert.IsInstanceOfType(d.Children[2], typeof(PropertyValueBlock));
            PropertyValueBlock pvb = (PropertyValueBlock)d.Children[2];

            #region Verifying property value block

            Assert.AreEqual(6, pvb.Children.Count);
            Assert.AreEqual(17, pvb.Start);
            Assert.AreEqual(34, pvb.AfterEnd);

            Assert.AreEqual(typeof(TokenItem), pvb.Children[0].GetType());
            Assert.AreEqual(CssTokenType.OpenCurlyBrace, ((TokenItem)pvb.Children[0]).TokenType);

            Assert.AreEqual(typeof(TokenItem), pvb.Children[1].GetType());
            Assert.AreEqual(CssTokenType.Identifier, ((TokenItem)pvb.Children[1]).TokenType);

            Assert.AreEqual(typeof(TokenItem), pvb.Children[2].GetType());
            Assert.AreEqual(CssTokenType.Colon, ((TokenItem)pvb.Children[2]).TokenType);

            Assert.AreEqual(typeof(TokenItem), pvb.Children[3].GetType());
            Assert.AreEqual(CssTokenType.Identifier, ((TokenItem)pvb.Children[3]).TokenType);

            Assert.AreEqual(typeof(TokenItem), pvb.Children[4].GetType());
            Assert.AreEqual(CssTokenType.Semicolon, ((TokenItem)pvb.Children[4]).TokenType);

            Assert.AreEqual(typeof(TokenItem), pvb.Children[5].GetType());
            Assert.AreEqual(CssTokenType.CloseCurlyBrace, ((TokenItem)pvb.Children[5]).TokenType);

            #endregion

            Assert.AreEqual(typeof(TokenItem), d.Children[d.Children.Count - 1].GetType());
            Assert.AreEqual(CssTokenType.Semicolon, ((TokenItem)d.Children[d.Children.Count - 1]).Token.TokenType);

            Assert.AreEqual(1, d.Values.Count);
            Assert.AreEqual(d.Children[2], d.Values[0]);
        }

        [TestMethod]
        public void Declaration_ParseTest_CustomPropertyValue_WithMultipleBracedBlocks()
        {
            string text = "--jsCode: if(condition) { trueAction(); } else { falseAction() };";
            ITextProvider tp = new StringTextProvider(text);
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            Declaration d = new Declaration();

            Assert.IsTrue(d.Parse(new ItemFactory(tp, null), tp, tokens));
            Assert.IsTrue(d.IsCustomProperty);

            Assert.IsFalse(d.HasParseErrors);

            Assert.AreEqual(0, d.Start);
            Assert.AreEqual(text.Length, d.AfterEnd);

            Assert.AreEqual(7, d.Children.Count);
            Assert.AreEqual(typeof(TokenItem), d.Semicolon.GetType());
            Assert.AreEqual(64, d.Semicolon.Start); // make sure we have the correct semicolon

            Assert.AreEqual(typeof(TokenItem), d.Children[0].GetType());
            Assert.AreEqual(CssTokenType.Identifier, ((TokenItem)d.Children[0]).Token.TokenType);

            Assert.AreEqual(typeof(TokenItem), d.Children[1].GetType());
            Assert.AreEqual(CssTokenType.Colon, ((TokenItem)d.Children[1]).Token.TokenType);

            Assert.IsInstanceOfType(d.Children[2], typeof(Function));
            Function ifFunction = (Function)d.Children[2];

            // asserts for: if(condition)
            {
                Assert.AreEqual(1, ifFunction.Arguments.Count);
                Assert.AreEqual("condition", tp.GetText(ifFunction.Arguments[0].Start, ifFunction.Arguments[0].Length));
            }

            Assert.IsInstanceOfType(d.Children[3], typeof(PropertyValueBlock));
            PropertyValueBlock pvb1 = (PropertyValueBlock)d.Children[3];

            // asserts for: { trueAction(); }
            {
                Assert.AreEqual(4, pvb1.Children.Count);

                Assert.IsInstanceOfType(pvb1.Children[0], typeof(TokenItem));
                Assert.AreEqual(CssTokenType.OpenCurlyBrace, ((TokenItem)pvb1.Children[0]).TokenType);

                Assert.IsInstanceOfType(pvb1.Children[1], typeof(Function));
                Assert.AreEqual(0, ((Function)pvb1.Children[1]).Arguments.Count);

                Assert.IsInstanceOfType(pvb1.Children[2], typeof(TokenItem));
                Assert.AreEqual(CssTokenType.Semicolon, ((TokenItem)pvb1.Children[2]).TokenType);

                Assert.IsInstanceOfType(pvb1.Children[3], typeof(TokenItem));
                Assert.AreEqual(CssTokenType.CloseCurlyBrace, ((TokenItem)pvb1.Children[3]).TokenType);
            }

            Assert.IsInstanceOfType(d.Children[4], typeof(TokenItem));
            Assert.AreEqual(CssTokenType.Identifier, ((TokenItem)d.Children[4]).TokenType);

            Assert.IsInstanceOfType(d.Children[5], typeof(PropertyValueBlock));
            PropertyValueBlock pvb2 = (PropertyValueBlock)d.Children[5];

            // asserts for: { falseAction() }
            // note: to differentiate between the block above, this one doesn't have a semicolon
            // after the method call
            {
                Assert.AreEqual(3, pvb2.Children.Count);

                Assert.IsInstanceOfType(pvb2.Children[0], typeof(TokenItem));
                Assert.AreEqual(CssTokenType.OpenCurlyBrace, ((TokenItem)pvb2.Children[0]).TokenType);

                Assert.IsInstanceOfType(pvb2.Children[1], typeof(Function));
                Assert.AreEqual(0, ((Function)pvb2.Children[1]).Arguments.Count);

                Assert.IsInstanceOfType(pvb2.Children[2], typeof(TokenItem));
                Assert.AreEqual(CssTokenType.CloseCurlyBrace, ((TokenItem)pvb2.Children[2]).TokenType);
            }

            Assert.AreEqual(typeof(TokenItem), d.Children[d.Children.Count - 1].GetType());
            Assert.AreEqual(CssTokenType.Semicolon, ((TokenItem)d.Children[d.Children.Count - 1]).Token.TokenType);

            Assert.AreEqual(4, d.Values.Count);
            Assert.AreEqual(d.Children[2], d.Values[0]);
            Assert.AreEqual(d.Children[3], d.Values[1]);
            Assert.AreEqual(d.Children[4], d.Values[2]);
            Assert.AreEqual(d.Children[5], d.Values[3]);
        }

        [TestMethod]
        public void Declaration_InvalidParse()
        {
            string[] rules = new string[]
            {
                ".foo { color: red color: blue; color: green; }",
                ".foo { : red; color: blue;  }",
                ".foo { color: (red] blue; color: green;  }",
                ".foo { color red: blue; color: green;  }",
                ".foo { *color: blue; color: green;  }",
                ".foo { **color: blue; color: green;  }",
            };

            foreach (string text in rules)
            {
                ITextProvider tp = new StringTextProvider(text);
                TokenStream tokens = Helpers.MakeTokenStream(tp);
                RuleSet rs = new RuleSet();

                Assert.IsTrue(rs.Parse(new ItemFactory(tp, tokens), tp, tokens));
                Assert.IsTrue(rs.IsValid);
                Assert.AreEqual(2, rs.Block.Declarations.Count);
                Assert.IsFalse(rs.Block.Declarations[0].IsValid);
                Assert.IsTrue(rs.Block.Declarations[1].IsValid);
            }
        }
    }
}
