// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Document;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.TreeItems;
using Microsoft.Css.Parser.TreeItems.AtDirectives;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.Document
{
    [TestClass]
    public class CssDocumentTest : CssUnitTestBase
    {
        [TestMethod]
        public void CssDocument_TextTest()
        {
            CssTree tree = new CssTree(new DefaultParserFactory());
            Assert.IsNull(tree.TextProvider);

            string text = "@import 'list.css' .a {color:red}";
            tree.TextProvider = new StringTextProvider(text);
            Assert.AreEqual(text, tree.TextProvider.GetText(0, tree.TextProvider.Length));
            Assert.IsNotNull(tree.StyleSheet);
            Assert.AreEqual(2, tree.StyleSheet.Children.Count);
            Assert.IsTrue(tree.StyleSheet.Children[0] is ImportDirective);

            text = ".a {color:red}";
            tree.TextProvider = new StringTextProvider(text);
            Assert.AreEqual(text, tree.TextProvider.GetText(0, tree.TextProvider.Length));
            Assert.IsNotNull(tree.StyleSheet);
            Assert.AreEqual(1, tree.StyleSheet.Children.Count);
            Assert.IsTrue(tree.StyleSheet.Children[0] is RuleSet);
        }
    }
}
