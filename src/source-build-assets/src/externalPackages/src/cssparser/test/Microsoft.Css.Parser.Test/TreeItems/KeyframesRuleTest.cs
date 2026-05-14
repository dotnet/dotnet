// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems.AtDirectives;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.TreeItems
{
    [TestClass]
    public class KeyFramesRuleTest : CssUnitTestBase
    {
        [TestMethod]
        public void KeyFramesRule_ParseTest()
        {
            string text =
                @"@keyframes bounce {
                      from {
                          top: 100px;
                          animation-timing-function: ease-out;
                      }
                      25% {
                          top: 50px;
                          animation-timing-function: ease-in;
                      }
                      50% {
                          top: 100px;
                          animation-timing-function: ease-out;
                      }
                      75% {
                          top: 75px;
                          animation-timing-function: ease-in;
                      }
                      to {
                          top: 100px;
                      }
                }";

            ITextProvider tp = new StringTextProvider(text);
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            KeyFramesDirective kf = new KeyFramesDirective();

            Assert.IsTrue(kf.Parse(new ItemFactory(tp, null), tp, tokens));
            Assert.IsTrue(tp.CompareTo(kf.Keyword.Start, "keyframes", ignoreCase: false));
            Assert.IsTrue(tp.CompareTo(kf.Name.Start, "bounce", ignoreCase: false));
            Assert.IsNotNull(kf.Block.OpenCurlyBrace);
            Assert.IsNotNull(kf.Block.CloseCurlyBrace);

            KeyFrameSelectorType[] types = new KeyFrameSelectorType[]
            {
                KeyFrameSelectorType.From,
                KeyFrameSelectorType.Percentage,
                KeyFrameSelectorType.Percentage,
                KeyFrameSelectorType.Percentage,
                KeyFrameSelectorType.To
            };

            Assert.AreEqual(5, kf.KeyFramesBlock.KeyFrames.Count);
            for (int i = 0; i < kf.KeyFramesBlock.KeyFrames.Count; i++)
            {
                Assert.AreEqual(types[i], kf.KeyFramesBlock.KeyFrames[i].Selectors[0].SelectorType);
                Assert.IsNotNull(kf.KeyFramesBlock.KeyFrames[i].RuleBlock.OpenCurlyBrace);
                Assert.IsNotNull(kf.KeyFramesBlock.KeyFrames[i].RuleBlock.CloseCurlyBrace);
                Assert.IsTrue(kf.KeyFramesBlock.KeyFrames[i].RuleBlock.Declarations.Count > 0);
                Assert.IsTrue(tp.CompareTo(kf.KeyFramesBlock.KeyFrames[i].RuleBlock.Declarations[0].PropertyName.Start, "top", ignoreCase: false));
            }
        }
    }
}
