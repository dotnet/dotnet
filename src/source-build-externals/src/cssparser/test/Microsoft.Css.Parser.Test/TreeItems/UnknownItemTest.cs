// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.TreeItems;
using Microsoft.Css.Parser.TreeItems.Functions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.TreeItems
{
    [TestClass]
    public class UnknownItemTest : CssUnitTestBase
    {
        [TestMethod]
        public void ParseUnknownTest()
        {
            string text = "}";
            ITextProvider tp = new StringTextProvider(text);
            {
                ParseItem pi = UnknownItem.ParseUnknown(
                    null, new ItemFactory(tp, null), tp,
                    Helpers.MakeTokenStream(tp),
                    ParseErrorType.OpenCurlyBraceMissingForRule);

                Assert.IsNotNull(pi);
                Assert.IsTrue(pi.HasParseErrors);
                Assert.AreEqual(ParseErrorType.OpenCurlyBraceMissingForRule, pi.ParseErrors[0].ErrorType);
                Assert.AreEqual(ParseErrorLocation.WholeItem, pi.ParseErrors[0].Location);
            }

            // Try a URL
            {
                text = "url('foo.jpg')";
                tp = new StringTextProvider(text);

                UrlItem pi = UnknownItem.ParseUnknown(
                    null, new ItemFactory(tp, null), tp,
                    Helpers.MakeTokenStream(tp)) as UrlItem;

                Assert.IsNotNull(pi);
            }
        }
    }
}
