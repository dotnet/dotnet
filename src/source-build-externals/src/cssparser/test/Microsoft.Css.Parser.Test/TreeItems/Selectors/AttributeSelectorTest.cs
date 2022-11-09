// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems.Selectors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.TreeItems.Selectors
{
    [TestClass]
    public class AttributeSelectorTest : CssUnitTestBase
    {
        [TestMethod]
        public void AttributeSelector_AttributeOnly()
        {
            string test = "[title]";
            ITextProvider tp = new StringTextProvider(test);
            TokenStream ts = Helpers.MakeTokenStream(tp);
            AttributeSelector attributeSelector = new AttributeSelector();
            Assert.IsTrue(attributeSelector.Parse(new ItemFactory(tp, null), tp, ts), "Valid CSS '{0}' parsed incorrectly", test);
            Assert.IsNotNull(attributeSelector.OpenBracket, "OpenBracket not parsed for CSS : '{0}'", test);
            Assert.IsNotNull(attributeSelector.CloseBracket, "CloseBracket not parsed for CSS : '{0}'", test);
            Assert.IsTrue(tp.CompareTo(attributeSelector.AttributeName.Start, "title", ignoreCase: false), "AttributeName not parsed for CSS : '{0}'", test);
            Assert.IsNull(attributeSelector.Operation);
            Assert.IsTrue(attributeSelector.IsValid);
        }

        [TestMethod]
        public void AttributeSelectorAttributeOneOfValue()
        {
            string test = "[rel ~= 'copyright']";
            ITextProvider tp = new StringTextProvider(test);
            TokenStream ts = Helpers.MakeTokenStream(tp);
            AttributeSelector attributeSelector = new AttributeSelector();
            Assert.IsTrue(attributeSelector.Parse(new ItemFactory(tp, null), tp, ts), "Valid CSS '{0}' parsed incorrectly", test);
            Assert.IsNotNull(attributeSelector.OpenBracket, "OpenBracket not parsed for CSS : '{0}'", test);
            Assert.IsNotNull(attributeSelector.CloseBracket, "CloseBracket not parsed for CSS : '{0}'", test);
            Assert.IsTrue(tp.CompareTo(attributeSelector.AttributeName.Start, "rel", ignoreCase: false), "AttributeName not parsed for CSS : '{0}'", test);
            Assert.AreEqual(CssTokenType.OneOf, attributeSelector.Operation.TokenType, "CssTokenType.OneOf not parsed for CSS : '{0}'", test);
            Assert.IsTrue(attributeSelector.IsValid);
        }

        [TestMethod]
        public void AttributeSelector_AttributeListBegins()
        {
            string test = "[hreflang |=\t\"en\"]";
            ITextProvider tp = new StringTextProvider(test);
            TokenStream ts = Helpers.MakeTokenStream(tp);
            AttributeSelector attributeSelector = new AttributeSelector();
            Assert.IsTrue(attributeSelector.Parse(new ItemFactory(tp, null), tp, ts), "Valid CSS '{0}' parsed incorrectly", test);
            Assert.IsNotNull(attributeSelector.OpenBracket, "OpenBracket not parsed for CSS : '{0}'", test);
            Assert.IsNotNull(attributeSelector.CloseBracket, "CloseBracket not parsed for CSS : '{0}'", test);
            Assert.IsTrue(tp.CompareTo(attributeSelector.AttributeName.Start, "hreflang", ignoreCase: false), "AttributeName not parsed for CSS : '{0}'", test);
            Assert.AreEqual(CssTokenType.ListBeginsWith, attributeSelector.Operation.TokenType, "CssTokenType.ListBeginsWith not parsed for CSS : '{0}'", test);
            Assert.IsTrue(attributeSelector.IsValid);
        }


        [TestMethod]
        public void AttributeSelector_AttributeBeginsWith()
        {
            string test = "[type^=\"image/\"]";
            ITextProvider tp = new StringTextProvider(test);
            TokenStream ts = Helpers.MakeTokenStream(tp);
            AttributeSelector attributeSelector = new AttributeSelector();
            Assert.IsTrue(attributeSelector.Parse(new ItemFactory(tp, null), tp, ts), "Valid CSS '{0}' parsed incorrectly", test);
            Assert.IsNotNull(attributeSelector.OpenBracket, "OpenBracket not parsed for CSS : '{0}'", test);
            Assert.IsNotNull(attributeSelector.CloseBracket, "CloseBracket not parsed for CSS : '{0}'", test);
            Assert.IsTrue(tp.CompareTo(attributeSelector.AttributeName.Start, "type", ignoreCase: false), "AttributeName not parsed for CSS : '{0}'", test);
            Assert.AreEqual(CssTokenType.BeginsWith, attributeSelector.Operation.TokenType, "CssTokenType.BeginsWith not parsed for CSS : '{0}'", test);
            Assert.IsTrue(attributeSelector.IsValid);
        }

        [TestMethod]
        public void AttributeSelector_AttributeEndsWith()
        {
            string test = "[href$=\".html\"][ignore=this]";
            ITextProvider tp = new StringTextProvider(test);
            TokenStream ts = Helpers.MakeTokenStream(tp);
            AttributeSelector attributeSelector = new AttributeSelector();
            Assert.IsTrue(attributeSelector.Parse(new ItemFactory(tp, null), tp, ts), "Valid CSS '{0}' parsed incorrectly", test);
            Assert.IsNotNull(attributeSelector.OpenBracket, "OpenBracket not parsed for CSS : '{0}'", test);
            Assert.IsNotNull(attributeSelector.CloseBracket, "CloseBracket not parsed for CSS : '{0}'", test);
            Assert.IsTrue(tp.CompareTo(attributeSelector.AttributeName.Start, "href", ignoreCase: false), "AttributeName not parsed for CSS : '{0}'", test);
            Assert.AreEqual(CssTokenType.EndsWith, attributeSelector.Operation.TokenType);
            Assert.IsTrue(attributeSelector.IsValid);
        }

        [TestMethod]
        public void AttributeSelector_AttributeContainsString()
        {
            string test = "[  title  *=  \"hello\"  ]:ignore";
            ITextProvider tp = new StringTextProvider(test);
            TokenStream ts = Helpers.MakeTokenStream(tp);
            AttributeSelector attributeSelector = new AttributeSelector();
            Assert.IsTrue(attributeSelector.Parse(new ItemFactory(tp, null), tp, ts), "Valid CSS '{0}' parsed incorrectly", test);
            Assert.IsNotNull(attributeSelector.OpenBracket, "OpenBracket not parsed for CSS : '{0}'", test);
            Assert.IsNotNull(attributeSelector.CloseBracket, "CloseBracket not parsed for CSS : '{0}'", test);
            Assert.IsTrue(tp.CompareTo(attributeSelector.AttributeName.Start, "title", ignoreCase: false), "AttributeName not parsed for CSS : '{0}'", test);
            Assert.AreEqual(CssTokenType.ContainsString, attributeSelector.Operation.TokenType);
            Assert.IsTrue(attributeSelector.IsValid);
        }

        [TestMethod]
        public void AttributeSelector_AttributeOrSeparator()
        {
            string test = "[foo|att=val]";
            ITextProvider tp = new StringTextProvider(test);
            TokenStream ts = Helpers.MakeTokenStream(tp);
            AttributeSelector attributeSelector = new AttributeSelector();
            Assert.IsTrue(attributeSelector.Parse(new ItemFactory(tp, null), tp, ts), "Valid CSS '{0}' parsed incorrectly", test);
            Assert.IsTrue(attributeSelector.IsValid);
            Assert.IsNotNull(attributeSelector.OpenBracket, "OpenBracket not parsed for CSS : '{0}'", test);
            Assert.IsNotNull(attributeSelector.CloseBracket, "CloseBracket not parsed for CSS : '{0}'", test);

            Assert.IsNotNull(attributeSelector.AttributeName.Namespace, test, "AttributeName.Namespace not parsed for CSS : '{0}'", test);
            Assert.IsNotNull(attributeSelector.AttributeName.Separator, test, "AttributeName.Separator  not parsed for CSS : '{0}'", test);

            Assert.IsTrue(tp.CompareTo(attributeSelector.AttributeName.Namespace.Start, "foo", ignoreCase: false), "AttributeName.Namespace not parsed for CSS : '{0}'", test);
            Assert.IsTrue(tp.CompareTo(attributeSelector.AttributeName.Name.Start, "att", ignoreCase: false), "AttributeName.Name not parsed for CSS : '{0}'", test);
        }

        [TestMethod]
        public void AttributeSelector_AttributeOrSeparatorUniversalNS()
        {
            string test = "[*|att=val]";
            ITextProvider tp = new StringTextProvider(test);
            TokenStream ts = Helpers.MakeTokenStream(tp);
            AttributeSelector attributeSelector = new AttributeSelector();
            Assert.IsTrue(attributeSelector.Parse(new ItemFactory(tp, null), tp, ts), "Valid CSS '{0}' parsed incorrectly", test);
            Assert.IsTrue(attributeSelector.IsValid);
            Assert.IsNotNull(attributeSelector.OpenBracket, "OpenBracket not parsed for CSS : '{0}'", test);
            Assert.IsNotNull(attributeSelector.CloseBracket, "CloseBracket not parsed for CSS : '{0}'", test);

            Assert.IsNotNull(attributeSelector.AttributeName.Namespace, test, "AttributeName.Namespace not parsed for CSS : '{0}'", test);
            Assert.IsNotNull(attributeSelector.AttributeName.Separator, test, "AttributeName.Separator  not parsed for CSS : '{0}'", test);

            Assert.IsTrue(tp.CompareTo(attributeSelector.AttributeName.Namespace.Start, "*", ignoreCase: false), "AttributeName.Namespace not parsed for CSS : '{0}'", test);
            Assert.IsTrue(tp.CompareTo(attributeSelector.AttributeName.Name.Start, "att", ignoreCase: false), "AttributeName.Name not parsed for CSS : '{0}'", test);
        }


        [TestMethod]
        public void AttributeSelector_AttributeOrSeparatorNoNS()
        {
            string test = "[|att=val]";
            ITextProvider tp = new StringTextProvider(test);
            TokenStream ts = Helpers.MakeTokenStream(tp);
            AttributeSelector attributeSelector = new AttributeSelector();
            Assert.IsTrue(attributeSelector.Parse(new ItemFactory(tp, null), tp, ts), "Valid CSS '{0}' parsed incorrectly", test);
            Assert.IsTrue(attributeSelector.IsValid);
            Assert.IsNotNull(attributeSelector.OpenBracket, "OpenBracket not parsed for CSS : '{0}'", test);
            Assert.IsNotNull(attributeSelector.CloseBracket, "CloseBracket not parsed for CSS : '{0}'", test);

            Assert.IsNotNull(attributeSelector.AttributeName.Separator, test, "AttributeName.Separator  not parsed for CSS : '{0}'", test);
            Assert.IsTrue(tp.CompareTo(attributeSelector.AttributeName.Name.Start, "att", ignoreCase: false), "AttributeName not parsed for CSS : '{0}'", test);
            Assert.IsTrue(tp.CompareTo(attributeSelector.AttributeValue.Start, "val", ignoreCase: false), "Value not parsed for CSS : '{0}'", test);
        }

        public void AttributeSelector_MissingEndBracket()
        {
            string test = "[foo=";
            ITextProvider tp = new StringTextProvider(test);
            TokenStream ts = Helpers.MakeTokenStream(tp);
            AttributeSelector attributeSelector = new AttributeSelector();
            Assert.IsTrue(attributeSelector.Parse(new ItemFactory(tp, null), tp, ts));
            Assert.IsNotNull(attributeSelector.OpenBracket);
            Assert.IsNull(attributeSelector.CloseBracket);
        }

        [TestMethod]
        public void AttributeSelector_Invalid()
        {
            string[] tests = new string[]
            {
                ".a",
                "#123",
                "a",
                "*",
                "+",
                ">",
                "|foo",
                "foo]"
            };

            foreach (string test in tests)
            {
                ITextProvider tp = new StringTextProvider(test);
                TokenStream ts = Helpers.MakeTokenStream(tp);
                AttributeSelector attributeSelector = new AttributeSelector();
                Assert.IsFalse(attributeSelector.Parse(new ItemFactory(tp, null), tp, ts));
            }
        }
    }
}
