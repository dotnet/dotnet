// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems.AtDirectives;
using Microsoft.Css.Parser.TreeItems.Functions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.TreeItems
{
    [TestClass]
    public class FontFaceRuleTest : CssUnitTestBase
    {
        [TestMethod]
        public void FontFaceRule_ParseTest()
        {
            string text = "@font-face {  font-family: Headline; src: local(Futura-Medium), url(fonts.svg#MyGeometricModern) format(\"svg\", 'opentype'); unicode-range: U+3000-9FFF, U+ff??; }";
            ITextProvider tp = new StringTextProvider(text);
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            FontFaceDirective ff = new FontFaceDirective();
            Assert.IsTrue(ff.Parse(new ItemFactory(tp, null), tp, tokens));

            Assert.IsTrue(tp.CompareTo(ff.Keyword.Start, "font-face", ignoreCase: false));
            Assert.IsNotNull(ff.Block.OpenCurlyBrace);
            Assert.IsNotNull(ff.Block.CloseCurlyBrace);

            Assert.AreEqual(3, ff.RuleBlock.Declarations.Count);

            Assert.IsTrue(tp.CompareTo(ff.RuleBlock.Declarations[0].PropertyName.Start, "font-family", ignoreCase: false));

            Assert.IsTrue(tp.CompareTo(ff.RuleBlock.Declarations[1].PropertyName.Start, "src", ignoreCase: false));
            Assert.IsTrue(ff.RuleBlock.Declarations[1].Values[0] is FunctionLocal);
            Assert.IsTrue(ff.RuleBlock.Declarations[1].Values[2] is UrlItem);
            Assert.IsTrue(ff.RuleBlock.Declarations[1].Values[3] is FunctionFormat);
            FunctionFormat func = ff.RuleBlock.Declarations[1].Values[3] as FunctionFormat;

            FunctionArgument arg = func.Arguments[0] as FunctionArgument;
            Assert.AreEqual("\"svg\",", tp.GetText(arg.Start, arg.Length));

            arg = func.Arguments[1] as FunctionArgument;
            Assert.AreEqual("'opentype'", tp.GetText(arg.Start, arg.Length));

            Assert.IsTrue(tp.CompareTo(ff.RuleBlock.Declarations[2].PropertyName.Start, "unicode-range", ignoreCase: false));
        }
    }
}
