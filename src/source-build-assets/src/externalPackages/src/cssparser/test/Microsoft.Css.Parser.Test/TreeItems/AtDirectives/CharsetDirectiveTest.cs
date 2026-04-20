// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems.AtDirectives;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.TreeItems.AtDirectives
{
    [TestClass]
    public class CharsetDirectiveTest : CssUnitTestBase
    {
        [TestMethod]
        public void CharsetDirective_ParseTest()
        {
            ITextProvider tp = new StringTextProvider("@charset 'foo';");
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            CharsetDirective d = new CharsetDirective();
            Assert.IsTrue(d.Parse(new ItemFactory(tp, null), tp, tokens));
            Assert.IsNotNull(d.CharacterSet);
            Assert.IsTrue(tp.CompareTo(d.CharacterSet.Start, "'foo'", ignoreCase: false));
            Assert.IsNotNull(d.Keyword);
            Assert.IsNotNull(d.Semicolon);

            tp = new StringTextProvider("@charset ;");
            tokens = Helpers.MakeTokenStream(tp);
            d = new CharsetDirective();
            Assert.IsTrue(d.Parse(new ItemFactory(tp, null), tp, tokens));
            Assert.IsNotNull(d.Keyword);
            Assert.IsNull(d.CharacterSet);
            Assert.IsNotNull(d.Semicolon);

            tp = new StringTextProvider("@charset");
            tokens = Helpers.MakeTokenStream(tp);
            d = new CharsetDirective();
            Assert.IsTrue(d.Parse(new ItemFactory(tp, null), tp, tokens));
            Assert.IsNotNull(d.Keyword);
            Assert.IsNull(d.CharacterSet);
            Assert.IsNull(d.Semicolon);
        }
    }
}
