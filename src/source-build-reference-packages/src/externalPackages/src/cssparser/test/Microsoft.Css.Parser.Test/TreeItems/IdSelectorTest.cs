// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems.Selectors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.TreeItems
{
    [TestClass]
    public class IdSelectorTest : CssUnitTestBase
    {
        [TestMethod]
        public void IdSelector_ParseValid()
        {
            string[] tests = new string[]
            {
                "#foo",
                "#1234",
                "#-foo-",
                "#_foo_",
            };

            foreach (string test in tests)
            {
                ITextProvider tp = new StringTextProvider(test);
                TokenStream tokens = Helpers.MakeTokenStream(tp);
                IdSelector s = new IdSelector();

                Assert.IsTrue(s.Parse(new ItemFactory(tp, null), tp, tokens));
                bool hasId = s.TryGetIdRange(out _, out int idLength);
                Assert.IsTrue(hasId);
                Assert.IsNotNull(s.HashName);
                Assert.AreEqual(test.Length, s.HashName.Length);
                Assert.AreEqual(test.Length - 1, idLength);
            }
        }

        [TestMethod]
        public void IdSelector_ParseInvalid()
        {
            string[] tests = new string[]
            {
                "#",
                "# ",
                "#*+1>",
                "#{+|}",
                "+",
                "{+|}",
                "123",
                "abc",
            };

            foreach (string test in tests)
            {
                ITextProvider tp = new StringTextProvider(test);
                TokenStream tokens = Helpers.MakeTokenStream(tp);
                IdSelector s = new IdSelector();

                if (test[0] == '#')
                {
                    Assert.IsTrue(s.Parse(new ItemFactory(tp, null), tp, tokens));
                    Assert.IsFalse(s.HasParseErrors);
                    Assert.IsTrue(s.HashName.HasParseErrors);
                }
                else
                {
                    Assert.IsFalse(s.Parse(new ItemFactory(tp, null), tp, tokens));
                }
            }
        }
    }
}
