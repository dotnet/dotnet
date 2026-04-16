// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.TreeItems
{
    [TestClass]
    public class RulesetTest : CssUnitTestBase
    {
        [TestMethod]
        public void Ruleset_ParseTest()
        {
            RuleSet target = new RuleSet();
            string text = ".a { }";
            ITextProvider tp = new StringTextProvider(text);
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            bool actual = target.Parse(new ItemFactory(tp, null), tp, tokens);
            Assert.AreEqual(true, actual);
        }
    }
}
