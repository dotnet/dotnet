// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Test;
using Microsoft.Css.Parser.TreeItems;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Parser
{
    [TestClass]
    public class TreeVisitorTest : CssUnitTestBase
    {
        [TestMethod]
        [DeploymentItem(@"Files\090\91.css")]
        public void TreeVisitor_VisitTest1()
        {
            string text = Helpers.LoadFileAsString(@"Files\090\91.css");
            CssParser p = new CssParser();
            StyleSheet s = p.Parse(text, true);

            SimpleTreeVisitor simpleVisitor = new SimpleTreeVisitor();

            s.Accept(simpleVisitor);

            Assert.AreEqual(12913, simpleVisitor.CountCalls);
        }

        [TestMethod]
        public void TreeVisitor_VisitTest2()
        {
            string text = @"
                <!--
                @charset 'utf-8';
                @foo { }
                @font-face { foo: bar }
                @namespace foo bar;
                @import url('foo');
                @page { @margin { } }
                @media not screen { .foo { } }
                @keyframes foo { from { } }
                .foo[foo] { }
                -->
                ";
            CssParser p = new CssParser();
            StyleSheet s = p.Parse(text, true);

            SimpleTreeVisitor simpleVisitor = new SimpleTreeVisitor();

            s.Accept(simpleVisitor);

            Assert.AreEqual(102, simpleVisitor.CountCalls);
        }
    }

    internal sealed class SimpleTreeVisitor : ICssSimpleTreeVisitor
    {
        public int CountCalls = 0;

        public VisitItemResult Visit(ParseItem parseItem)
        {
            Assert.IsNotNull(parseItem);
            CountCalls++;

            return VisitItemResult.Continue;
        }
    }
}
