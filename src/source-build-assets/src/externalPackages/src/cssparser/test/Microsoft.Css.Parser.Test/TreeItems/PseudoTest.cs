// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems.Selectors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.TreeItems
{
    [TestClass]
    public class PseudoClassTest : CssUnitTestBase
    {
        /// <summary>
        /// Helps test the parsing of a pseudo-class
        /// </summary>
        private class PseudoTest
        {
            public PseudoTest(string text, Type type, bool valid, ParseErrorType error)
            {
                SelectorText = text;
                ParseType = type;
                IsValid = valid;
                Error = error;
            }

            public string SelectorText { get; set; } // selector to parse
            public Type ParseType { get; set; } // expected type of first simple selector
            public bool IsValid { get; set; } // validity of first simple selector
            public ParseErrorType Error { get; set; } // expected error for invalid selector
        }

        private static void RunPseudoTest(IEnumerable<PseudoTest> tests)
        {
            foreach (PseudoTest test in tests)
            {
                ITextProvider text = new StringTextProvider(test.SelectorText);
                TokenStream tokens = Helpers.MakeTokenStream(text);
                Selector selector = new Selector();

                Assert.IsTrue(selector.Parse(new ItemFactory(text, null), text, tokens));
                Assert.IsTrue(selector.SimpleSelectors.Count > 0);

                ParseItemList subs = selector.SimpleSelectors[0].SubSelectors;
                Assert.IsTrue(subs.Count > 0);
                Assert.IsInstanceOfType(subs[0], test.ParseType);

                Assert.AreEqual(test.IsValid, selector.IsValid);
                Assert.AreEqual(test.IsValid, subs[0].IsValid);

                if (!test.IsValid)
                {
                    int errorCount = 0;

                    Assert.IsTrue(subs[0].ContainsParseErrors);

                    IEnumerable<ParseError> aggregateErrors = subs[0].HasParseErrors
                                                                ? subs[0].ParseErrors 
                                                                : ((ComplexItem)subs[0]).Children.SelectMany(s => s.ParseErrors);

                    foreach (ParseError error in aggregateErrors)
                    {
                        errorCount++;
                        Assert.AreEqual(test.Error, error.ErrorType);
                    }

                    Assert.AreEqual(1, errorCount);
                }
            }
        }

        [TestMethod]
        public void PseudoClass_ParseTest()
        {
            PseudoTest[] tests = new PseudoTest[]
            {
                // Classes
                new PseudoTest(":", typeof(PseudoClassSelector), false, ParseErrorType.PseudoClassNameMissing),
                new PseudoTest(":foo", typeof(PseudoClassSelector), true, ParseErrorType.UnexpectedParseError),
                new PseudoTest(": foo", typeof(PseudoClassSelector), false, ParseErrorType.PseudoClassNameMissing),
                new PseudoTest(":(bar)", typeof(PseudoClassSelector), false, ParseErrorType.PseudoClassNameMissing),
                new PseudoTest(":foo(bar)", typeof(PseudoClassFunctionSelector), true, ParseErrorType.UnexpectedParseError),
                new PseudoTest(": foo(bar)", typeof(PseudoClassFunctionSelector), false, ParseErrorType.PseudoClassNameMissing),

                new PseudoTest(":-foo", typeof(PseudoClassSelector), true, ParseErrorType.UnexpectedParseError),
                new PseudoTest(":-foo()", typeof(PseudoClassFunctionSelector), true, ParseErrorType.UnexpectedParseError),
                new PseudoTest(":-", typeof(PseudoClassSelector), true, ParseErrorType.UnexpectedParseError),

                // Elements
                new PseudoTest("::", typeof(PseudoElementSelector), false, ParseErrorType.PseudoElementNameMissing),
                new PseudoTest("::foo", typeof(PseudoElementSelector), true, ParseErrorType.UnexpectedParseError),
                new PseudoTest(":: foo", typeof(PseudoElementSelector), false, ParseErrorType.PseudoElementNameMissing),
                new PseudoTest("::(bar)", typeof(PseudoElementSelector), false, ParseErrorType.PseudoElementNameMissing),
                new PseudoTest("::foo(bar)", typeof(PseudoElementFunctionSelector), true, ParseErrorType.UnexpectedParseError),
                new PseudoTest(":: foo(bar)", typeof(PseudoElementFunctionSelector), false, ParseErrorType.PseudoElementNameMissing),

                new PseudoTest("::-foo", typeof(PseudoElementSelector), true, ParseErrorType.UnexpectedParseError),
                new PseudoTest("::-foo()", typeof(PseudoElementFunctionSelector), true, ParseErrorType.UnexpectedParseError),
                new PseudoTest("::-", typeof(PseudoElementSelector), true, ParseErrorType.UnexpectedParseError),
            };

            RunPseudoTest(tests);
        }
    }
}
