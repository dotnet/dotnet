// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems.PropertyValues;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.TreeItems
{
    [TestClass]
    public class UnitValueTest : CssUnitTestBase
    {
        [TestMethod]
        public void ParseTest()
        {
            string[] samples1 = new string[]
            {
                "100px",
                "200%",
                "-3grad",
                "0.8em",
                "-0.5deg",
                "22s",
                "345.7Hz",
                "144dpi",
                "12dppx",
                "12st",
                "0.7vmin",
                "0vmax",
                "1.117gr",
                "-6.0dB",
                "+12dB",
            };

            UnitType[] types1 = new UnitType[]
            {
                UnitType.Length,
                UnitType.Percentage,
                UnitType.Angle,
                UnitType.Length,
                UnitType.Angle,
                UnitType.Time,
                UnitType.Frequency,
                UnitType.Resolution,
                UnitType.Resolution,
                UnitType.Semitones,
                UnitType.Viewport,
                UnitType.Viewport,
                UnitType.Grid,
                UnitType.Volume,
                UnitType.Volume,
            };

            int i = 0;

            foreach (string text in samples1)
            {
                ITextProvider tp = new StringTextProvider(text);
                TokenStream tokens = Helpers.MakeTokenStream(tp);
                tokens.Advance(1);

                UnitValue uv = new UnitValue();
                uv.Parse(new ItemFactory(tp, null), tp, tokens);

                Assert.AreEqual(types1[i++], uv.UnitType);
                Assert.AreEqual(CssTokenType.Units, uv.UnitToken.TokenType);
            }
        }
    }
}
