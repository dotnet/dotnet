// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems.Functions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.TreeItems
{
    [TestClass]
    public class UrlTest : CssUnitTestBase
    {
        [TestMethod]
        public void Url_ParseTest()
        {
            TokenStream tokens;
            string text = "url(www.microsoft.com)";
            ITextProvider tp = new StringTextProvider(text);
            tokens = Helpers.MakeTokenStream(tp);
            UrlItem u = new UrlItem();
            Assert.IsTrue(u.Parse(new ItemFactory(tp, null), tp, tokens));

            text = "url('www.microsoft.com')";
            tp = new StringTextProvider(text);
            tokens = Helpers.MakeTokenStream(tp);
            u = new UrlItem();
            Assert.IsTrue(u.Parse(new ItemFactory(tp, null), tp, tokens));

            text = "url()";
            tp = new StringTextProvider(text);
            tokens = Helpers.MakeTokenStream(tp);
            u = new UrlItem();
            Assert.IsTrue(u.Parse(new ItemFactory(tp, null), tp, tokens));
        }
    }
}
