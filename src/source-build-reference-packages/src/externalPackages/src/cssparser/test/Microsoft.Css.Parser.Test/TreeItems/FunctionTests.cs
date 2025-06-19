// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Globalization;
using System.Threading;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems;
using Microsoft.Css.Parser.TreeItems.Functions;
using Microsoft.Css.Parser.TreeItems.PropertyValues;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.TreeItems
{
    [TestClass]
    public class FunctionTest : CssUnitTestBase
    {
        [TestMethod]
        public void Function_ParseTest1()
        {
            ITextProvider tp = new StringTextProvider("counter(a,b,c)");
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            Function f = Function.ParseFunction(null, new ItemFactory(tp, null), tp, tokens);
            Assert.AreEqual(typeof(FunctionCounter), f.GetType());
            Assert.AreEqual("counter(", tp.GetText(f.FunctionName.Start, f.FunctionName.Length));
        }

        [TestMethod]
        public void Function_ParseTest2()
        {
            ITextProvider tp = new StringTextProvider("local(x, 3)");
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            Function f = Function.ParseFunction(null, new ItemFactory(tp, null), tp, tokens);
            Assert.AreEqual(typeof(FunctionLocal), f.GetType());
            Assert.AreEqual("local(", tp.GetText(f.FunctionName.Start, f.FunctionName.Length));
        }

        [TestMethod]
        public void Function_ParseTest3()
        {
            ITextProvider tp = new StringTextProvider("counter(3%)");
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            Function f = Function.ParseFunction(null, new ItemFactory(tp, null), tp, tokens);
            Assert.AreEqual(typeof(FunctionCounter), f.GetType());
            Assert.AreEqual("counter(", tp.GetText(f.FunctionName.Start, f.FunctionName.Length));
        }

        [TestMethod]
        public void Function_ParseTest4()
        {
            ITextProvider tp = new StringTextProvider("foo(,)");
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            Function f = Function.ParseFunction(null, new ItemFactory(tp, null), tp, tokens);
            Assert.AreEqual(typeof(Function), f.GetType());
            Assert.AreEqual("foo(", tp.GetText(f.FunctionName.Start, f.FunctionName.Length));
        }

        [TestMethod]
        public void Function_ParseTest5()
        {
            ITextProvider tp = new StringTextProvider("foo(a,}");
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            Function f = Function.ParseFunction(null, new ItemFactory(tp, null), tp, tokens);
            Assert.AreEqual(typeof(Function), f.GetType());
            Assert.AreEqual("foo(", tp.GetText(f.FunctionName.Start, f.FunctionName.Length));
        }

        [TestMethod]
        public void Function_ParseTest6()
        {
            // Unknown token and block in a function argument

            StyleSheet ss = Helpers.MakeStyleSheet("p { width: calc(100% * (attr(value) - attr(min)) / (attr(max) - attr(min))); }");

            Function func = ss.RuleSets[0].Block.Declarations[0].Values[0] as Function;
            Assert.IsNotNull(func);
            Assert.IsInstanceOfType(func, typeof(FunctionCalc));

            Assert.AreEqual(1, func.Arguments.Count);
            Assert.IsInstanceOfType(func.Arguments[0], typeof(FunctionArgument));

            FunctionArgument arg = (FunctionArgument)func.Arguments[0];
            Assert.AreEqual(5, arg.ArgumentItems.Count);
            Assert.IsInstanceOfType(arg.ArgumentItems[0], typeof(UnitValue));
            Assert.IsInstanceOfType(arg.ArgumentItems[1], typeof(TokenItem));
            Assert.IsInstanceOfType(arg.ArgumentItems[2], typeof(UnknownBlock));
            Assert.IsInstanceOfType(arg.ArgumentItems[3], typeof(TokenItem));
            Assert.IsInstanceOfType(arg.ArgumentItems[4], typeof(UnknownBlock));

            Assert.AreEqual("100% * (attr(value) - attr(min)) / (attr(max) - attr(min))", arg.Text);
        }

        [TestMethod]
        public void Function_ParseTest7()
        {
            ITextProvider tp = new StringTextProvider("var(a,}");
            TokenStream tokens = Helpers.MakeTokenStream(tp);

            Function f = Function.ParseFunction(null, new ItemFactory(tp, null), tp, tokens);
            Assert.IsTrue(f.IsUnclosed);
            Assert.AreEqual(6, f.Length);
            Assert.IsInstanceOfType(f, typeof(FunctionVar));
            Assert.AreEqual("var(", tp.GetText(f.FunctionName.Start, f.FunctionName.Length));
        }

        [TestMethod]
        public void Function_ParseTest8()
        {
            // Functions within function arguments

            StyleSheet ss = Helpers.MakeStyleSheet("p { background: -webkit-gradient(linear, 0 0, 100% 0, from(#008ECF), to(#00AEEF)); }");

            Function func = ss.RuleSets[0].Block.Declarations[0].Values[0] as Function;
            Assert.IsNotNull(func);
            Assert.AreEqual("-webkit-gradient(", func.FunctionName.Text);

            Assert.AreEqual(5, func.Arguments.Count);
            Assert.IsInstanceOfType(func.Arguments[0], typeof(FunctionArgument));
            Assert.IsInstanceOfType(func.Arguments[1], typeof(FunctionArgument));
            Assert.IsInstanceOfType(func.Arguments[2], typeof(FunctionArgument));
            Assert.IsInstanceOfType(func.Arguments[3], typeof(FunctionArgument));
            Assert.IsInstanceOfType(func.Arguments[4], typeof(FunctionArgument));

            Assert.AreEqual(1, ((FunctionArgument)func.Arguments[0]).ArgumentItems.Count);
            Assert.AreEqual(2, ((FunctionArgument)func.Arguments[1]).ArgumentItems.Count);
            Assert.AreEqual(2, ((FunctionArgument)func.Arguments[2]).ArgumentItems.Count);
            Assert.AreEqual(1, ((FunctionArgument)func.Arguments[3]).ArgumentItems.Count);
            Assert.AreEqual(1, ((FunctionArgument)func.Arguments[4]).ArgumentItems.Count);

            Function func3 = ((FunctionArgument)func.Arguments[3]).ArgumentItems[0] as Function;
            Function func4 = ((FunctionArgument)func.Arguments[4]).ArgumentItems[0] as Function;
            Assert.IsNotNull(func3);
            Assert.IsNotNull(func4);

            Assert.AreEqual(1, func3.Arguments.Count);
            Assert.AreEqual(1, func4.Arguments.Count);
        }

        [TestMethod]
        public void FunctionRgb_ParseTest1()
        {
            ITextProvider tp = new StringTextProvider("rgb(25, 10%, 34)");
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            Function f = Function.ParseFunction(null, new ItemFactory(tp, null), tp, tokens);
            Assert.AreEqual(typeof(FunctionColor), f.GetType());
            Assert.AreEqual(3, f.Arguments.Count);

            Assert.IsInstanceOfType(f.Arguments[0], typeof(FunctionArgument));
            FunctionArgument fa = f.Arguments[0] as FunctionArgument;
            Assert.IsNotNull(fa.Comma);

            Assert.AreEqual(1, fa.ArgumentItems.Count);
            Assert.IsInstanceOfType(fa.ArgumentItems[0], typeof(NumericalValue));
            NumericalValue numericalValue = (NumericalValue)fa.ArgumentItems[0];
            Assert.IsInstanceOfType(numericalValue.Children[0], typeof(TokenItem));
            Assert.AreEqual(CssTokenType.Number, ((TokenItem)numericalValue.Children[0]).TokenType);

            Assert.IsInstanceOfType(f.Arguments[1], typeof(FunctionArgument));
            Assert.IsNotNull(fa.Comma);
            Assert.AreEqual(1, fa.ArgumentItems.Count);
            Assert.IsInstanceOfType(fa.ArgumentItems[0], typeof(NumericalValue));

            fa = f.Arguments[1] as FunctionArgument;
            Assert.IsInstanceOfType(fa.ArgumentItems[0], typeof(NumericalValue));
            numericalValue = (NumericalValue)fa.ArgumentItems[0];
            Assert.IsInstanceOfType(numericalValue.Children[1], typeof(TokenItem));
            Assert.IsInstanceOfType(numericalValue.Children[1], typeof(TokenItem));
            Assert.AreEqual(CssTokenType.Number, ((TokenItem)numericalValue.Children[0]).TokenType);
            Assert.AreEqual(CssTokenType.Units, ((TokenItem)numericalValue.Children[1]).TokenType);
        }

        [TestMethod]
        public void FunctionRgb_ParseTest2()
        {
            ITextProvider tp = new StringTextProvider("rgb(a b,b,1px)");
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            Function f = Function.ParseFunction(null, new ItemFactory(tp, null), tp, tokens);
            Assert.AreEqual(typeof(FunctionColor), f.GetType());
            Assert.AreEqual("rgb(", tp.GetText(f.FunctionName.Start, f.FunctionName.Length));
        }

        [TestMethod]
        public void FunctionFormat_Test()
        {
            ITextProvider tp = new StringTextProvider("format(opentype)");
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            Function f = Function.ParseFunction(null, new ItemFactory(tp, null), tp, tokens);
            Assert.AreEqual(typeof(FunctionFormat), f.GetType());
        }

        [TestMethod]
        public void FunctionAttr_Test()
        {
            ITextProvider tp = new StringTextProvider("attr(class)");
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            Function f = Function.ParseFunction(null, new ItemFactory(tp, null), tp, tokens);
            Assert.AreEqual(typeof(FunctionAttr), f.GetType());
        }

        [TestMethod]
        public void FunctionExpression_Test()
        {
            ITextProvider tp = new StringTextProvider(
                "expression(eval(document.compatMode && document.compatMode=='CSS1Compat') ? document.documentElement.scrollTop : document.body.scrollTop)");

            TokenStream tokens = Helpers.MakeTokenStream(tp);
            Function f = Function.ParseFunction(null, new ItemFactory(tp, null), tp, tokens);
            Assert.AreEqual(typeof(FunctionExpression), f.GetType());
            Assert.AreEqual(tp.Length, f.Length);
        }

        [TestMethod]
        public void FunctionColor_FloatParseTest()
        {
            StyleSheet ss = Helpers.MakeStyleSheet("p { color: rgba(1, 2, 3, 0.625); }");
            FunctionColor func = ss.RuleSets[0].Block.Declarations[0].Values[0] as FunctionColor;
            Assert.IsNotNull(func);

            Assert.IsTrue(func.GetColorArgumentValue(3, false, out float value));
            Assert.AreEqual(0.625f, value);

            CultureInfo oldCulture = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("ru-RU");
                Assert.IsTrue(func.GetColorArgumentValue(3, false, out value));
                Assert.AreEqual(0.625f, value);
            }
            finally
            {
                // Don't let the asserts cause the thread to stay in the wrong culture
                Thread.CurrentThread.CurrentCulture = oldCulture;
            }
        }

        [TestMethod]
        public void FunctionVar_ParseTest_NoCustomPropertyName()
        {
            ITextProvider tp = new StringTextProvider("var()");
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            Function f = Function.ParseFunction(null, new ItemFactory(tp, null), tp, tokens);

            Assert.IsInstanceOfType(f, typeof(FunctionVar));

            FunctionVar fv = f as FunctionVar;

            Assert.AreEqual(0, fv.Arguments.Count);
            Assert.IsNull(fv.CustomPropertyName);

            Assert.IsTrue(fv.ContainsParseErrors);
            Assert.AreEqual(1, fv.CloseBrace.ParseErrors.Count);
            Assert.AreEqual(ParseErrorType.FunctionArgumentMissing, fv.CloseBrace.ParseErrors[0].ErrorType);
            Assert.AreEqual(ParseErrorLocation.BeforeItem, fv.CloseBrace.ParseErrors[0].Location);
        }

        [TestMethod]
        public void FunctionVar_ParseTest_InvalidCustomPropertyName()
        {
            ITextProvider tp = new StringTextProvider("var(test)");
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            Function f = Function.ParseFunction(null, new ItemFactory(tp, null), tp, tokens);

            Assert.IsInstanceOfType(f, typeof(FunctionVar));

            FunctionVar fv = f as FunctionVar;

            Assert.AreEqual(1, fv.Arguments.Count);
            Assert.IsNotNull(fv.CustomPropertyName);

            Assert.IsTrue(fv.ContainsParseErrors);
            Assert.AreEqual(1, fv.CustomPropertyName.ParseErrors.Count);
            Assert.AreEqual(ParseErrorType.CustomPropertyNameExpected, fv.CustomPropertyName.ParseErrors[0].ErrorType);
            Assert.AreEqual(ParseErrorLocation.WholeItem, fv.CustomPropertyName.ParseErrors[0].Location);
        }

        [TestMethod]
        public void FunctionVar_ParseTest_CustomPropertyNameOnly()
        {
            ITextProvider tp = new StringTextProvider("var(--test)");
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            Function f = Function.ParseFunction(null, new ItemFactory(tp, null), tp, tokens);

            Assert.IsInstanceOfType(f, typeof(FunctionVar));

            FunctionVar fv = f as FunctionVar;

            Assert.AreEqual(1, fv.Arguments.Count);
            Assert.IsNotNull(fv.CustomPropertyName);
            Assert.AreEqual("--test", tp.GetText(fv.CustomPropertyName.Start, fv.CustomPropertyName.Length));
        }

        [TestMethod]
        public void FunctionVar_ParseTest_WithDeclarationValues()
        {
            ITextProvider tp = new StringTextProvider("var(--test, 20px 10px solid red )");
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            Function f = Function.ParseFunction(null, new ItemFactory(tp, null), tp, tokens);

            Assert.IsInstanceOfType(f, typeof(FunctionVar));

            FunctionVar fv = f as FunctionVar;

            Assert.AreEqual(2, fv.Arguments.Count);
            Assert.IsNotNull(fv.CustomPropertyName);
            Assert.AreEqual("--test", tp.GetText(fv.CustomPropertyName.Start, fv.CustomPropertyName.Length));
            Assert.AreEqual("20px 10px solid red", tp.GetText(fv.Arguments[1].Start, fv.Arguments[1].Length));
        }
    }
}
