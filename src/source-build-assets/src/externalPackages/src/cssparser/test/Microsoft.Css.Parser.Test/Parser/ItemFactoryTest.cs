// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Microsoft.Css.Parser.Test;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Parser
{
    [TestClass]
    public class ItemFactoryTest : CssUnitTestBase
    {
        private class TestDeclaration : Declaration
        {
        }

        private class TestFactory : ICssItemFactory
        {
            public ParseItem CreateItem(ItemFactory itemFactory, ITextProvider text, TokenStream tokens, ComplexItem parent, Type type)
            {
                Assert.AreSame(itemFactory.TextProvider, text);
                Assert.AreSame(itemFactory.TokenStream, tokens);

                if (type == typeof(Declaration))
                {
                    Assert.IsInstanceOfType(parent, typeof(RuleBlock));
                    return new TestDeclaration();
                }

                return null;
            }
        }

        [TestMethod]
        public void ItemFactory_BasicTest()
        {
            CssParser parser = new CssParser(null, new TestFactory());
            StyleSheet sheet = parser.Parse(@".foo#bar { color: blue; color: invisible }", insertComments: true);

            Assert.AreEqual(2, sheet.RuleSets[0].Block.Declarations.Count);
            foreach (Declaration decl in sheet.RuleSets[0].Block.Declarations)
            {
                Assert.IsInstanceOfType(decl, typeof(TestDeclaration));
                Assert.AreEqual("color", decl.PropertyNameText);
            }
        }
    }
}
