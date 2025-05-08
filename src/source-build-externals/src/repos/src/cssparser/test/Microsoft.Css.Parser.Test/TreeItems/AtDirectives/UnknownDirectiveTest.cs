// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.TreeItems.AtDirectives;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.TreeItems.AtDirectives
{
    [TestClass]
    public class UnknownDirectiveTest : CssUnitTestBase
    {
        [TestMethod]
        public void UnknownDirective_ParseTest()
        {
            string text = "@foo \"bar\";";
            ITextProvider tp = new StringTextProvider(text);
            UnknownDirective ud = new UnknownDirective();
            Assert.IsTrue(ud.Parse(new ItemFactory(tp, null), tp, Helpers.MakeTokenStream(tp)));
            Assert.AreEqual("@", text.Substring(ud.At.Start, ud.At.Length));
            Assert.AreEqual("foo", text.Substring(ud.Keyword.Start, ud.Keyword.Length));
        }
    }
}
