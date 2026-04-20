// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.TreeItems;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.TreeItems
{
    [TestClass]
    public class StylesheetTest : CssUnitTestBase
    {
        [TestMethod]
        public void Stylesheet_ConstructorTest()
        {
            StyleSheet s = new StyleSheet();
            Assert.IsNotNull(s.Children);
            Assert.AreEqual(0, s.Children.Count);
        }

        [TestMethod]
        public void Stylesheet_EmptyTest()
        {
            CssParser parser = new CssParser();
            StyleSheet sheet = parser.Parse(null, null, insertComments: false);
            Assert.IsNull(sheet);

            sheet = parser.Parse(string.Empty, insertComments: false);
            Assert.IsNotNull(sheet);
            Assert.AreEqual(0, sheet.Children.Count);
        }

        [TestMethod]
        public void Stylesheet_ParseNotNestedTest()
        {
            // http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems?id=238231 - The root stylesheet must not cache child curly braces
            string text = ".foo } { } {";
            StyleSheet s = Helpers.MakeStyleSheet(text);

            Assert.IsNull(s.OpenCurlyBrace);
            Assert.IsNull(s.CloseCurlyBrace);
        }
    }
}
