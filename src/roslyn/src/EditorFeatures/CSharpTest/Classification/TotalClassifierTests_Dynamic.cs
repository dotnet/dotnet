﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Remote.Testing;
using Roslyn.Test.Utilities;
using Xunit;
using static Microsoft.CodeAnalysis.Editor.UnitTests.Classification.FormattedClassifications;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Classification;

public sealed partial class TotalClassifierTests
{
    [Theory, CombinatorialData]
    public Task DynamicAsParamTypeAndDefault(TestHost testHost)
        => TestInClassAsync(@"void M(dynamic d = default(dynamic",
            testHost,
            Keyword("void"),
            Method("M"),
            Punctuation.OpenParen,
            Keyword("dynamic"),
            Parameter("d"),
            Operators.Equals,
            Keyword("default"),
            Punctuation.OpenParen,
            Keyword("dynamic"));

    [Theory, CombinatorialData]
    public Task DynamicExplicitConversion(TestHost testHost)
        => TestInMethodAsync(
@"dynamic d = (dynamic)a;",
            testHost,
            Keyword("dynamic"),
            Local("d"),
            Operators.Equals,
            Punctuation.OpenParen,
            Keyword("dynamic"),
            Punctuation.CloseParen,
            Identifier("a"),
            Punctuation.Semicolon);

    [Theory, CombinatorialData]
    public Task DynamicMethodCall(TestHost testHost)
        => TestInMethodAsync(@"dynamic.Equals(1, 1);",
            testHost,
            Identifier("dynamic"),
            Operators.Dot,
            Identifier("Equals"),
            Punctuation.OpenParen,
            Number("1"),
            Punctuation.Comma,
            Number("1"),
            Punctuation.CloseParen,
            Punctuation.Semicolon);

    [Theory, CombinatorialData]
    public Task DynamicNullable(TestHost testHost)
        => TestInMethodAsync(@"dynamic? a",
            testHost,
            Keyword("dynamic"),
            Operators.QuestionMark,
            Local("a"));

    [Theory, CombinatorialData]
    public Task DynamicAsUsingAliasForClass(TestHost testHost)
        => TestAsync(
@"using dynamic = System.EventArgs;",
            testHost,
            Keyword("using"),
            Class("dynamic"),
            Operators.Equals,
            Namespace("System"),
            Operators.Dot,
            Class("EventArgs"),
            Punctuation.Semicolon);

    [Theory, CombinatorialData]
    public Task DynamicAsUsingAliasForDelegate(TestHost testHost)
        => TestAsync(
@"using dynamic = System.Action;",
            testHost,
            Keyword("using"),
            Delegate("dynamic"),
            Operators.Equals,
            Namespace("System"),
            Operators.Dot,
            Delegate("Action"),
            Punctuation.Semicolon);

    [Theory, CombinatorialData]
    public Task DynamicAsUsingAliasForStruct(TestHost testHost)
        => TestAsync(
@"using dynamic = System.DateTime;",
            testHost,
            Keyword("using"),
            Struct("dynamic"),
            Operators.Equals,
            Namespace("System"),
            Operators.Dot,
            Struct("DateTime"),
            Punctuation.Semicolon);

    [Theory, CombinatorialData]
    public Task DynamicAsUsingAliasForEnum(TestHost testHost)
        => TestAsync(
@"using dynamic = System.DayOfWeek;",
            testHost,
            Keyword("using"),
            Enum("dynamic"),
            Operators.Equals,
            Namespace("System"),
            Operators.Dot,
            Enum("DayOfWeek"),
            Punctuation.Semicolon);

    [Theory, CombinatorialData]
    public Task DynamicAsUsingAliasForInterface(TestHost testHost)
        => TestAsync(
@"using dynamic = System.IDisposable;",
            testHost,
            Keyword("using"),
            Interface("dynamic"),
            Operators.Equals,
            Namespace("System"),
            Operators.Dot,
            Interface("IDisposable"),
            Punctuation.Semicolon);

    [Theory, CombinatorialData]
    public Task DynamicAsExternAlias(TestHost testHost)
        => TestAsync(
            """
            extern alias dynamic;

            class C
            {
                dynamic::Goo a;
            }
            """,
            testHost,
            Keyword("extern"),
            Keyword("alias"),
            Namespace("dynamic"),
            Punctuation.Semicolon,
            Keyword("class"),
            Class("C"),
            Punctuation.OpenCurly,
            Namespace("dynamic"),
            Operators.ColonColon,
            Identifier("Goo"),
            Field("a"),
            Punctuation.Semicolon,
            Punctuation.CloseCurly);

    [Theory, CombinatorialData]
    public Task DynamicAsDelegateType(TestHost testHost)
        => TestAsync(@"delegate void dynamic()",
            testHost,
            Keyword("delegate"),
            Keyword("void"),
            Delegate("dynamic"),
            Punctuation.OpenParen,
            Punctuation.CloseParen);

    [Theory, CombinatorialData]
    public Task DynamicAsDelegateReturnTypeAndParam(TestHost testHost)
        => TestAsync(@"delegate dynamic MyDelegate (dynamic d)",
            testHost,
            Keyword("delegate"),
            Keyword("dynamic"),
            Delegate("MyDelegate"),
            Punctuation.OpenParen,
            Keyword("dynamic"),
            Parameter("d"),
            Punctuation.CloseParen);

    [Theory, CombinatorialData]
    public Task DynamicAsDelegateLocalVariable(TestHost testHost)
        => TestInMethodAsync(
            """
            Func<string> f = delegate
            {
                int dynamic = 10;
                return dynamic.ToString();
            };
            """,
            testHost,
            Identifier("Func"),
            Punctuation.OpenAngle,
            Keyword("string"),
            Punctuation.CloseAngle,
            Local("f"),
            Operators.Equals,
            Keyword("delegate"),
            Punctuation.OpenCurly,
            Keyword("int"),
            Local("dynamic"),
            Operators.Equals,
            Number("10"),
            Punctuation.Semicolon,
            ControlKeyword("return"),
            Local("dynamic"),
            Operators.Dot,
            Method("ToString"),
            Punctuation.OpenParen,
            Punctuation.CloseParen,
            Punctuation.Semicolon,
            Punctuation.CloseCurly,
            Punctuation.Semicolon);

    [Theory, CombinatorialData]
    public Task DynamicAsGenericTypeName(TestHost testHost)
        => TestAsync(
            """
            partial class dynamic<T>
            {
            }

            class C
            {
                dynamic<int> d;
            }
            """,
            testHost,
            Keyword("partial"),
            Keyword("class"),
            Class("dynamic"),
            Punctuation.OpenAngle,
            TypeParameter("T"),
            Punctuation.CloseAngle,
            Punctuation.OpenCurly,
            Punctuation.CloseCurly,
            Keyword("class"),
            Class("C"),
            Punctuation.OpenCurly,
            Class("dynamic"),
            Punctuation.OpenAngle,
            Keyword("int"),
            Punctuation.CloseAngle,
            Field("d"),
            Punctuation.Semicolon,
            Punctuation.CloseCurly);

    [Theory, CombinatorialData]
    public Task DynamicAsGenericField(TestHost testHost)
        => TestAsync(
            """
            class A<T>
            {
                T dynamic;
            }
            """,
            testHost,
            Keyword("class"),
            Class("A"),
            Punctuation.OpenAngle,
            TypeParameter("T"),
            Punctuation.CloseAngle,
            Punctuation.OpenCurly,
            TypeParameter("T"),
            Field("dynamic"),
            Punctuation.Semicolon,
            Punctuation.CloseCurly);

    [Theory, CombinatorialData]
    public Task DynamicAsIndexerTypeAndParameter(TestHost testHost)
        => TestInClassAsync(@"dynamic this[dynamic i]",
            testHost,
            Keyword("dynamic"),
            Keyword("this"),
            Punctuation.OpenBracket,
            Keyword("dynamic"),
            Parameter("i"),
            Punctuation.CloseBracket);

    [Theory, CombinatorialData]
    public Task DynamicAsOperatorTypeAndParameter(TestHost testHost)
        => TestInClassAsync(@"static dynamic operator +(dynamic d1)",
            testHost,
            Keyword("static"),
            Keyword("dynamic"),
            Keyword("operator"),
            Operators.Plus,
            Punctuation.OpenParen,
            Keyword("dynamic"),
            Parameter("d1"),
            Punctuation.CloseParen);

    [Theory, CombinatorialData]
    public Task DynamicAsOperatorName(TestHost testHost)
        => TestInClassAsync(@"static explicit operator dynamic(dynamic s)",
            testHost,
            Keyword("static"),
            Keyword("explicit"),
            Keyword("operator"),
            Keyword("dynamic"),
            Punctuation.OpenParen,
            Keyword("dynamic"),
            Parameter("s"),
            Punctuation.CloseParen);

    [Theory, CombinatorialData]
    public Task DynamicAsPropertyTypeAndName(TestHost testHost)
        => TestInClassAsync(@"dynamic dynamic { get; set; }",
            testHost,
            Keyword("dynamic"),
            Property("dynamic"),
            Punctuation.OpenCurly,
            Keyword("get"),
            Punctuation.Semicolon,
            Keyword("set"),
            Punctuation.Semicolon,
            Punctuation.CloseCurly);

    [Theory, CombinatorialData]
    public Task DynamicAsEventName(TestHost testHost)
        => TestInClassAsync(@"event Action dynamic",
            testHost,
            Keyword("event"),
            Identifier("Action"),
            Event("dynamic"));

    [Theory, CombinatorialData]
    public Task DynamicAsLinqLocalVariable(TestHost testHost)
        => TestInMethodAsync(@"var v = from dynamic in names",
            testHost,
            Keyword("var"),
            Local("v"),
            Operators.Equals,
            Keyword("from"),
            Identifier("dynamic"),
            Keyword("in"),
            Identifier("names"));

    [Theory, CombinatorialData]
    public Task DynamicAsAnonymousTypePropertyName(TestHost testHost)
        => TestInMethodAsync(
            """
            var v = from dynamic in names
                    select new { dynamic = dynamic };
            """,
            testHost,
            Keyword("var"),
            Local("v"),
            Operators.Equals,
            Keyword("from"),
            Identifier("dynamic"),
            Keyword("in"),
            Identifier("names"),
            Keyword("select"),
            Keyword("new"),
            Punctuation.OpenCurly,
            Property("dynamic"),
            Operators.Equals,
            Identifier("dynamic"),
            Punctuation.CloseCurly,
            Punctuation.Semicolon);

    [Theory, CombinatorialData]
    public Task DynamicAsArgumentToLambdaExpression(TestHost testHost)
        => TestInMethodAsync(
@"var p = names.Select(dynamic => dynamic.Length);",
            testHost,
            Keyword("var"),
            Local("p"),
            Operators.Equals,
            Identifier("names"),
            Operators.Dot,
            Identifier("Select"),
            Punctuation.OpenParen,
            Parameter("dynamic"),
            Operators.EqualsGreaterThan,
            Parameter("dynamic"),
            Operators.Dot,
            Identifier("Length"),
            Punctuation.CloseParen,
            Punctuation.Semicolon);

    [Theory, CombinatorialData]
    public Task DynamicAsAnonymousMethodLocalVariable(TestHost testHost)
        => TestInMethodAsync(
            """
            D f = delegate
            {
                string dynamic = "a";
                return dynamic.Length;
            };
            """,
            testHost,
            Identifier("D"),
            Local("f"),
            Operators.Equals,
            Keyword("delegate"),
            Punctuation.OpenCurly,
            Keyword("string"),
            Local("dynamic"),
            Operators.Equals,
            String("""
                "a"
                """),
            Punctuation.Semicolon,
            ControlKeyword("return"),
            Local("dynamic"),
            Operators.Dot,
            Property("Length"),
            Punctuation.Semicolon,
            Punctuation.CloseCurly,
            Punctuation.Semicolon);

    [Theory, CombinatorialData]
    public Task DynamicAsMethodName(TestHost testHost)
        => TestInClassAsync(
            """
            dynamic dynamic()
            {
            }
            """,
            testHost,
            Keyword("dynamic"),
            Method("dynamic"),
            Punctuation.OpenParen,
            Punctuation.CloseParen,
            Punctuation.OpenCurly,
            Punctuation.CloseCurly);

    [Theory, CombinatorialData]
    public Task DynamicAsStaticMethodTypeAndParams(TestHost testHost)
        => TestInClassAsync(
            """
            static dynamic dynamic(params dynamic[] dynamic)
            {
            }
            """,
            testHost,
            Keyword("static"),
            Keyword("dynamic"),
            Method("dynamic"),
            Static("dynamic"),
            Punctuation.OpenParen,
            Keyword("params"),
            Keyword("dynamic"),
            Punctuation.OpenBracket,
            Punctuation.CloseBracket,
            Parameter("dynamic"),
            Punctuation.CloseParen,
            Punctuation.OpenCurly,
            Punctuation.CloseCurly);

    [Theory, CombinatorialData]
    public Task DynamicArraysInMethodSignature(TestHost testHost)
        => TestInClassAsync(
            """
            dynamic[] M(dynamic[] p, params dynamic[] pa)
            {
            }
            """,
            testHost,
            Keyword("dynamic"),
            Punctuation.OpenBracket,
            Punctuation.CloseBracket,
            Method("M"),
            Punctuation.OpenParen,
            Keyword("dynamic"),
            Punctuation.OpenBracket,
            Punctuation.CloseBracket,
            Parameter("p"),
            Punctuation.Comma,
            Keyword("params"),
            Keyword("dynamic"),
            Punctuation.OpenBracket,
            Punctuation.CloseBracket,
            Parameter("pa"),
            Punctuation.CloseParen,
            Punctuation.OpenCurly,
            Punctuation.CloseCurly);

    [Theory, CombinatorialData]
    public Task DynamicInPartialMethods(TestHost testHost)
        => TestInClassAsync(
            """
            partial void F(dynamic d);

            partial void F(dynamic d)
            {
            }
            """,
            testHost,
            Keyword("partial"),
            Keyword("void"),
            Method("F"),
            Punctuation.OpenParen,
            Keyword("dynamic"),
            Parameter("d"),
            Punctuation.CloseParen,
            Punctuation.Semicolon,
            Keyword("partial"),
            Keyword("void"),
            Method("F"),
            Punctuation.OpenParen,
            Keyword("dynamic"),
            Parameter("d"),
            Punctuation.CloseParen,
            Punctuation.OpenCurly,
            Punctuation.CloseCurly);

    [Theory, CombinatorialData]
    public Task DynamicRefAndOutParameters(TestHost testHost)
        => TestInClassAsync(
            """
            void F(ref dynamic r, out dynamic o)
            {
            }
            """,
            testHost,
            Keyword("void"),
            Method("F"),
            Punctuation.OpenParen,
            Keyword("ref"),
            Keyword("dynamic"),
            Parameter("r"),
            Punctuation.Comma,
            Keyword("out"),
            Keyword("dynamic"),
            Parameter("o"),
            Punctuation.CloseParen,
            Punctuation.OpenCurly,
            Punctuation.CloseCurly);

    [Theory, CombinatorialData]
    public Task DynamicInExtensionMethod(TestHost testHost)
        => TestInClassAsync(
            """
            dynamic F(this dynamic self, dynamic p)
            {
            }
            """,
            testHost,
            Keyword("dynamic"),
            ExtensionMethod("F"),
            Punctuation.OpenParen,
            Keyword("this"),
            Keyword("dynamic"),
            Parameter("self"),
            Punctuation.Comma,
            Keyword("dynamic"),
            Parameter("p"),
            Punctuation.CloseParen,
            Punctuation.OpenCurly,
            Punctuation.CloseCurly);

    [Theory, CombinatorialData]
    public Task DynamicAsBaseClass(TestHost testHost)
        => TestAsync(
            """
            class C : dynamic
            {
            }
            """,
            testHost,
            Keyword("class"),
            Class("C"),
            Punctuation.Colon,
            Keyword("dynamic"),
            Punctuation.OpenCurly,
            Punctuation.CloseCurly);

    [Theory, CombinatorialData]
    public Task DynamicAsGenericConstraint(TestHost testHost)
        => TestAsync(
            """
            class C<T> where T : dynamic
            {
            }
            """,
            testHost,
            Keyword("class"),
            Class("C"),
            Punctuation.OpenAngle,
            TypeParameter("T"),
            Punctuation.CloseAngle,
            Keyword("where"),
            TypeParameter("T"),
            Punctuation.Colon,
            Keyword("dynamic"),
            Punctuation.OpenCurly,
            Punctuation.CloseCurly);

    [Theory, CombinatorialData]
    public Task DynamicSizeOf(TestHost testHost)
        => TestInClassAsync(
            """
            unsafe int M()
            {
                return sizeof(dynamic);
            }
            """,
            testHost,
            Keyword("unsafe"),
            Keyword("int"),
            Method("M"),
            Punctuation.OpenParen,
            Punctuation.CloseParen,
            Punctuation.OpenCurly,
            ControlKeyword("return"),
            Keyword("sizeof"),
            Punctuation.OpenParen,
            Keyword("dynamic"),
            Punctuation.CloseParen,
            Punctuation.Semicolon,
            Punctuation.CloseCurly);

    [Theory, CombinatorialData]
    public Task DynamicTypeOf(TestHost testHost)
        => TestInMethodAsync(@"typeof(dynamic)",
            testHost,
            Keyword("typeof"),
            Punctuation.OpenParen,
            Keyword("dynamic"),
            Punctuation.CloseParen);

    [Theory, WorkItem("https://github.com/dotnet/roslyn/issues/44423")]
    [CombinatorialData]
    public async Task DynamicAsArrayName(bool script, TestHost testHost)
    {
        var code =
            """
            int[] dynamic = {
                1
            };
            """;

        var parseOptions = script ? Options.Script : null;

        await TestAsync(
            code,
            code,
            testHost,
            parseOptions,
            Keyword("int"),
            Punctuation.OpenBracket,
            Punctuation.CloseBracket,
            script ? Field("dynamic") : Local("dynamic"),
            Operators.Equals,
            Punctuation.OpenCurly,
            Number("1"),
            Punctuation.CloseCurly,
            Punctuation.Semicolon);
    }

    [Theory, CombinatorialData]
    public Task DynamicInForeach(TestHost testHost)
        => TestInMethodAsync(@"foreach (dynamic dynamic in dynamic",
            testHost,
            ControlKeyword("foreach"),
            Punctuation.OpenParen,
            Keyword("dynamic"),
            Local("dynamic"),
            ControlKeyword("in"),
            Identifier("dynamic"));

    [Theory, CombinatorialData]
    public Task DynamicInUsing(TestHost testHost)
        => TestInMethodAsync(@"using(dynamic d",
            testHost,
            Keyword("using"),
            Punctuation.OpenParen,
            Keyword("dynamic"),
            Local("d"));

    [Theory, CombinatorialData]
    public Task DynamicAsLocalVariableName(TestHost testHost)
        => TestInMethodAsync(
@"dynamic dynamic;",
            testHost,
            Keyword("dynamic"),
            Local("dynamic"),
            Punctuation.Semicolon);

    [Theory, CombinatorialData]
    public Task DynamicAsNamespaceName(TestHost testHost)
        => TestAsync(
            """
            namespace dynamic
            {
            }
            """,
            testHost,
            Keyword("namespace"),
            Namespace("dynamic"),
            Punctuation.OpenCurly,
            Punctuation.CloseCurly);

    [Theory, CombinatorialData]
    public Task DynamicAsClassName(TestHost testHost)
        => TestAsync(
            """
            class dynamic
            {
            }
            """,
            testHost,
            Keyword("class"),
            Class("dynamic"),
            Punctuation.OpenCurly,
            Punctuation.CloseCurly);

    [Theory, CombinatorialData]
    public Task DynamicAsConstructorDeclarationName(TestHost testHost)
        => TestAsync(
            """
            class dynamic
            {
                dynamic()
                {
                }
            }
            """,
            testHost,
            Keyword("class"),
            Class("dynamic"),
            Punctuation.OpenCurly,
            Class("dynamic"),
            Punctuation.OpenParen,
            Punctuation.CloseParen,
            Punctuation.OpenCurly,
            Punctuation.CloseCurly,
            Punctuation.CloseCurly);

    [Theory, CombinatorialData]
    public Task DynamicAsNamespaceAlias(TestHost testHost)
        => TestInMethodAsync(
@"dynamic.FileInfo file;",
            testHost,
            Identifier("dynamic"),
            Operators.Dot,
            Identifier("FileInfo"),
            Local("file"),
            Punctuation.Semicolon);

    [Theory, CombinatorialData]
    public Task DynamicAsGotoLabel(TestHost testHost)
        => TestInMethodAsync(
            """
            dynamic: int i = 0;
                    goto dynamic;
            """,
            testHost,
            Label("dynamic"),
            Punctuation.Colon,
            Keyword("int"),
            Local("i"),
            Operators.Equals,
            Number("0"),
            Punctuation.Semicolon,
            ControlKeyword("goto"),
            Label("dynamic"),
            Punctuation.Semicolon);

    [Theory, CombinatorialData]
    public Task DynamicAsEnumField(TestHost testHost)
        => TestInMethodAsync(
@"A a = A.dynamic;",
            testHost,
            Identifier("A"),
            Local("a"),
            Operators.Equals,
            Identifier("A"),
            Operators.Dot,
            Identifier("dynamic"),
            Punctuation.Semicolon);

    [Theory, CombinatorialData]
    public Task DynamicAsEnumFieldDefinition(TestHost testHost)
        => TestAsync(
            """
            enum A
            {
                dynamic
            }
            """,
            testHost,
            Keyword("enum"),
            Enum("A"),
            Punctuation.OpenCurly,
            EnumMember("dynamic"),
            Punctuation.CloseCurly);

    [Theory, CombinatorialData]
    public Task DynamicAsEnumType(TestHost testHost)
        => TestAsync(
            """
            enum dynamic
            {
            }
            """,
            testHost,
            Keyword("enum"),
            Enum("dynamic"),
            Punctuation.OpenCurly,
            Punctuation.CloseCurly);

    [Theory, CombinatorialData]
    public Task DynamicAsGenericTypeParameter(TestHost testHost)
        => TestAsync(
            """
            class C<dynamic, T> where dynamic : T
            {
                dynamic d;
            }
            """,
            testHost,
            Keyword("class"),
            Class("C"),
            Punctuation.OpenAngle,
            TypeParameter("dynamic"),
            Punctuation.Comma,
            TypeParameter("T"),
            Punctuation.CloseAngle,
            Keyword("where"),
            TypeParameter("dynamic"),
            Punctuation.Colon,
            TypeParameter("T"),
            Punctuation.OpenCurly,
            TypeParameter("dynamic"),
            Field("d"),
            Punctuation.Semicolon,
            Punctuation.CloseCurly);

    [Theory, CombinatorialData]
    public Task DynamicAsFieldType(TestHost testHost)
        => TestInClassAsync(@"dynamic d",
            testHost,
            Keyword("dynamic"),
            Field("d"));

    [Theory, CombinatorialData]
    public Task DynamicAsStaticFieldType(TestHost testHost)
        => TestInClassAsync(@"static dynamic d",
            testHost,
            Keyword("static"),
            Keyword("dynamic"),
            Field("d"),
            Static("d"));

    [Theory, CombinatorialData]
    public Task DynamicAsLocalVariableType(TestHost testHost)
        => TestInMethodAsync(@"dynamic d",
            testHost,
            Keyword("dynamic"),
            Local("d"));

    [Theory, CombinatorialData]
    public Task DynamicAsArrayLocalVariableType(TestHost testHost)
        => TestInMethodAsync(@"dynamic[] d",
            testHost,
            Keyword("dynamic"),
            Punctuation.OpenBracket,
            Punctuation.CloseBracket,
            Local("d"));

    [Theory, CombinatorialData]
    public Task DynamicAsLambdaParameterType(TestHost testHost)
        => TestInMethodAsync(
@"var q = a.Where((dynamic d) => d == dynamic);",
            testHost,
            Keyword("var"),
            Local("q"),
            Operators.Equals,
            Identifier("a"),
            Operators.Dot,
            Identifier("Where"),
            Punctuation.OpenParen,
            Punctuation.OpenParen,
            Keyword("dynamic"),
            Parameter("d"),
            Punctuation.CloseParen,
            Operators.EqualsGreaterThan,
            Parameter("d"),
            Operators.EqualsEquals,
            Identifier("dynamic"),
            Punctuation.CloseParen,
            Punctuation.Semicolon);

    [Theory, CombinatorialData]
    public Task DynamicArray(TestHost testHost)
        => TestInMethodAsync(
@"dynamic d = new dynamic[5];",
            testHost,
            Keyword("dynamic"),
            Local("d"),
            Operators.Equals,
            Keyword("new"),
            Keyword("dynamic"),
            Punctuation.OpenBracket,
            Number("5"),
            Punctuation.CloseBracket,
            Punctuation.Semicolon);

    [Theory, CombinatorialData]
    public Task DynamicConstructor(TestHost testHost)
        => TestInMethodAsync(
@"dynamic d = new dynamic();",
            testHost,
            Keyword("dynamic"),
            Local("d"),
            Operators.Equals,
            Keyword("new"),
            Keyword("dynamic"),
            Punctuation.OpenParen,
            Punctuation.CloseParen,
            Punctuation.Semicolon);

    [Theory, CombinatorialData]
    public Task DynamicAfterIs(TestHost testHost)
        => TestInMethodAsync(@"if (a is dynamic)",
            testHost,
            ControlKeyword("if"),
            Punctuation.OpenParen,
            Identifier("a"),
            Keyword("is"),
            Keyword("dynamic"),
            Punctuation.CloseParen);

    [Theory, CombinatorialData]
    public Task DynamicAfterAs(TestHost testHost)
        => TestInMethodAsync(@"a = a as dynamic",
            testHost,
            Identifier("a"),
            Operators.Equals,
            Identifier("a"),
            Keyword("as"),
            Keyword("dynamic"));

    [Theory, CombinatorialData]
    public Task DynamicAsGenericTypeArgument(TestHost testHost)
        => TestInMethodAsync(
@"List<dynamic> l = new List<dynamic>();",
            testHost,
            Identifier("List"),
            Punctuation.OpenAngle,
            Keyword("dynamic"),
            Punctuation.CloseAngle,
            Local("l"),
            Operators.Equals,
            Keyword("new"),
            Identifier("List"),
            Punctuation.OpenAngle,
            Keyword("dynamic"),
            Punctuation.CloseAngle,
            Punctuation.OpenParen,
            Punctuation.CloseParen,
            Punctuation.Semicolon);

    [Theory, CombinatorialData]
    public Task DynamicAsSecondGenericTypeArgument(TestHost testHost)
        => TestInMethodAsync(
@"KVP<string, dynamic> kvp;",
            testHost,
            Identifier("KVP"),
            Punctuation.OpenAngle,
            Keyword("string"),
            Punctuation.Comma,
            Keyword("dynamic"),
            Punctuation.CloseAngle,
            Local("kvp"),
            Punctuation.Semicolon);

    [Theory, CombinatorialData]
    public Task DynamicAsRegionLabel(TestHost testHost)
        => TestAsync("""
            #region dynamic
            #endregion
            """,
            testHost,
            PPKeyword("#"),
            PPKeyword("region"),
            PPText("dynamic"),
            PPKeyword("#"),
            PPKeyword("endregion"));

    [Theory, CombinatorialData]
    public Task DynamicAsInterfaceType(TestHost testHost)
        => TestAsync(
            """
            interface dynamic
            {
            }
            """,
            testHost,
            Keyword("interface"),
            Interface("dynamic"),
            Punctuation.OpenCurly,
            Punctuation.CloseCurly);

    [Theory, CombinatorialData]
    public Task DynamicAsStructType(TestHost testHost)
        => TestAsync(
            """
            struct dynamic
            {
            }
            """,
            testHost,
            Keyword("struct"),
            Struct("dynamic"),
            Punctuation.OpenCurly,
            Punctuation.CloseCurly);

    [Theory, CombinatorialData]
    public Task DynamicAsUndefinedGenericType(TestHost testHost)
        => TestInMethodAsync(
@"dynamic<int> d;",
            testHost,
            Identifier("dynamic"),
            Punctuation.OpenAngle,
            Keyword("int"),
            Punctuation.CloseAngle,
            Local("d"),
            Punctuation.Semicolon);
}
