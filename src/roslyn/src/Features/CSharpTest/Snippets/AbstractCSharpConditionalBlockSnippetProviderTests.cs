﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.CSharp.UnitTests.Snippets;

[Trait(Traits.Feature, Traits.Features.Snippets)]
public abstract class AbstractCSharpConditionalBlockSnippetProviderTests : AbstractCSharpSnippetProviderTests
{
    [Fact]
    public Task InsertSnippetInMethodTest()
        => VerifySnippetAsync("""
            class Program
            {
                public void Method()
                {
                    $$
                }
            }
            """, $$"""
            class Program
            {
                public void Method()
                {
                    {{SnippetIdentifier}} ({|0:true|})
                    {
                        $$
                    }
                }
            }
            """);

    [Fact]
    public Task InsertSnippetInGlobalContextTest()
        => VerifySnippetAsync("""
            $$
            """, $$"""
            {{SnippetIdentifier}} ({|0:true|})
            {
                $$
            }
            """);

    [Fact]
    public Task NoSnippetInBlockNamespaceTest()
        => VerifySnippetIsAbsentAsync("""
            namespace Namespace
            {
                $$
            }
            """);

    [Fact]
    public Task NoSnippetInFileScopedNamespaceTest()
        => VerifySnippetIsAbsentAsync("""
            namespace Namespace;

            $$
            """);

    [Fact]
    public Task InsertSnippetInConstructorTest()
        => VerifySnippetAsync("""
            class Program
            {
                public Program()
                {
                    var x = 5;
                    $$
                }
            }
            """, $$"""
            class Program
            {
                public Program()
                {
                    var x = 5;
                    {{SnippetIdentifier}} ({|0:true|})
                    {
                        $$
                    }
                }
            }
            """);

    [Fact]
    public Task InsertSnippetInLocalFunctionTest()
        => VerifySnippetAsync("""
            class Program
            {
                public void Method()
                {
                    var x = 5;
                    void LocalMethod()
                    {
                        $$
                    }
                }
            }
            """, $$"""
            class Program
            {
                public void Method()
                {
                    var x = 5;
                    void LocalMethod()
                    {
                        {{SnippetIdentifier}} ({|0:true|})
                        {
                            $$
                        }
                    }
                }
            }
            """);

    [Fact]
    public Task InsertSnippetInAnonymousFunctionTest()
        => VerifySnippetAsync("""
            public delegate void Print(int value);

            static void Main(string[] args)
            {
                Print print = delegate(int val)
                {
                    $$
                };

            }
            """, $$"""
            public delegate void Print(int value);

            static void Main(string[] args)
            {
                Print print = delegate(int val)
                {
                    {{SnippetIdentifier}} ({|0:true|})
                    {
                        $$
                    }
                };

            }
            """);

    [Fact]
    public Task InsertSnippetInParenthesizedLambdaExpressionTest()
        => VerifySnippetAsync("""
            using System;

            Func<int, int, bool> testForEquality = (x, y) =>
            {
                $$
                return x == y;
            };
            """, $$"""
            using System;

            Func<int, int, bool> testForEquality = (x, y) =>
            {
                {{SnippetIdentifier}} ({|0:true|})
                {
                    $$
                }
                return x == y;
            };
            """);

    [Fact]
    public Task NoSnippetInSwitchExpression()
        => VerifySnippetIsAbsentAsync("""
            class Program
            {
                public void Method()
                {
                    var operation = 2;
  
                    var result = operation switch
                    {
                        $$
                        1 => "Case 1",
                        2 => "Case 2",
                        3 => "Case 3",
                        4 => "Case 4",
                    };
                }
            }
            """);

    [Fact]
    public Task NoSnippetInSingleLambdaExpression()
        => VerifySnippetIsAbsentAsync("""
            using System;

            class Program
            {
                public void Method()
                {
                    Func<int, int> f = x => $$;
                }
            }
            """);

    [Fact]
    public Task NoSnippetInStringTest()
        => VerifySnippetIsAbsentAsync("""
            class Program
            {
                public void Method()
                {
                    var str = "$$";
                }
            }
            """);

    [Fact]
    public Task NoSnippetInConstructorArgumentsTest()
        => VerifySnippetIsAbsentAsync("""
            class Program
            {
                public void Method()
                {
                    var test = new Test($$);
                }
            }

            class Test
            {
                public Test(string val)
                {
                }
            }
            """);

    [Fact]
    public Task NoSnippetInParameterListTest()
        => VerifySnippetIsAbsentAsync("""
            class Program
            {
                public void Method(int x, $$)
                {
                }
            }
            """);

    [Fact]
    public Task NoSnippetInRecordDeclarationTest()
        => VerifySnippetIsAbsentAsync("""
            public record Person
            {
                $$
                public string FirstName { get; init; }
                public string LastName { get; init; }
            };
            """);

    [Fact]
    public Task NoSnippetInVariableDeclarationTest()
        => VerifySnippetIsAbsentAsync("""
            class Program
            {
                public void Method()
                {
                    var x = $$
                }
            }
            """);

    [Fact]
    public Task InsertInlineSnippetForCorrectTypeTest()
        => VerifySnippetAsync("""
            class Program
            {
                void M(bool arg)
                {
                    arg.$$
                }
            }
            """, $$"""
            class Program
            {
                void M(bool arg)
                {
                    {{SnippetIdentifier}} (arg)
                    {
                        $$
                    }
                }
            }
            """);

    [Fact]
    public Task NoInlineSnippetForIncorrectTypeTest()
        => VerifySnippetIsAbsentAsync("""
            class Program
            {
                void M(int arg)
                {
                    arg.$$
                }
            }
            """);

    [Fact]
    public Task NoInlineSnippetWhenNotDirectlyExpressionStatementTest()
        => VerifySnippetIsAbsentAsync("""
            class Program
            {
                void M(bool arg)
                {
                    System.Console.WriteLine(arg.$$);
                }
            }
            """);

    [Theory]
    [InlineData("// comment")]
    [InlineData("/* comment */")]
    [InlineData("#region test")]
    public Task CorrectlyDealWithLeadingTriviaInInlineSnippetInMethodTest1(string trivia)
        => VerifySnippetAsync($$"""
            class Program
            {
                void M(bool arg)
                {
                    {{trivia}}
                    arg.$$
                }
            }
            """, $$"""
            class Program
            {
                void M(bool arg)
                {
                    {{trivia}}
                    {{SnippetIdentifier}} (arg)
                    {
                        $$
                    }
                }
            }
            """);

    [Theory]
    [InlineData("#if true")]
    [InlineData("#pragma warning disable CS0108")]
    [InlineData("#nullable enable")]
    public Task CorrectlyDealWithLeadingTriviaInInlineSnippetInMethodTest2(string trivia)
        => VerifySnippetAsync($$"""
            class Program
            {
                void M(bool arg)
                {
            {{trivia}}
                    arg.$$
                }
            }
            """, $$"""
            class Program
            {
                void M(bool arg)
                {
            {{trivia}}
                    {{SnippetIdentifier}} (arg)
                    {
                        $$
                    }
                }
            }
            """);

    [Theory]
    [InlineData("// comment")]
    [InlineData("/* comment */")]
    public Task CorrectlyDealWithLeadingTriviaInInlineSnippetInGlobalStatementTest1(string trivia)
        => VerifySnippetAsync($"""
            {trivia}
            true.$$
            """, $$"""
            {{trivia}}
            {{SnippetIdentifier}} (true)
            {
                $$
            }
            """);

    [Theory]
    [InlineData("#region test")]
    [InlineData("#if true")]
    [InlineData("#pragma warning disable CS0108")]
    [InlineData("#nullable enable")]
    public Task CorrectlyDealWithLeadingTriviaInInlineSnippetInGlobalStatementTest2(string trivia)
        => VerifySnippetAsync($"""
            {trivia}
            true.$$
            """, $$"""

            {{trivia}}
            {{SnippetIdentifier}} (true)
            {
                $$
            }
            """);

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/69598")]
    public Task InsertInlineSnippetWhenDottingBeforeContextualKeywordTest1()
        => VerifySnippetAsync("""
            class C
            {
                void M(bool flag)
                {
                    flag.$$
                    var a = 0;
                }
            }
            """, $$"""
            class C
            {
                void M(bool flag)
                {
                    {{SnippetIdentifier}} (flag)
                    {
                        $$
                    }
                    var a = 0;
                }
            }
            """);

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/69598")]
    public Task InsertInlineSnippetWhenDottingBeforeContextualKeywordTest2()
        => VerifySnippetAsync("""
            class C
            {
                async void M(bool flag, Task t)
                {
                    flag.$$
                    await t;
                }
            }
            """, $$"""
            class C
            {
                async void M(bool flag, Task t)
                {
                    {{SnippetIdentifier}} (flag)
                    {
                        $$
                    }
                    await t;
                }
            }
            """);

    [Theory, WorkItem("https://github.com/dotnet/roslyn/issues/69598")]
    [InlineData("Task")]
    [InlineData("Task<int>")]
    [InlineData("System.Threading.Tasks.Task<int>")]
    public Task InsertInlineSnippetWhenDottingBeforeNameSyntaxTest(string nameSyntax)
        => VerifySnippetAsync($$"""
            using System.Threading.Tasks;

            class C
            {
                void M(bool flag)
                {
                    flag.$$
                    {{nameSyntax}} t = null;
                }
            }
            """, $$"""
            using System.Threading.Tasks;

            class C
            {
                void M(bool flag)
                {
                    {{SnippetIdentifier}} (flag)
                    {
                        $$
                    }
                    {{nameSyntax}} t = null;
                }
            }
            """);

    [Fact]
    public Task InsertInlineSnippetWhenDottingBeforeMemberAccessExpressionOnTheNextLineTest()
        => VerifySnippetAsync("""
            using System;

            class C
            {
                void M(bool flag)
                {
                    flag.$$
                    Console.WriteLine();
                }
            }
            """, $$"""
            using System;

            class C
            {
                void M(bool flag)
                {
                    {{SnippetIdentifier}} (flag)
                    {
                        $$
                    }
                    Console.WriteLine();
                }
            }
            """);

    [Fact]
    public Task NoInlineSnippetWhenDottingBeforeMemberAccessExpressionOnTheSameLineTest()
        => VerifySnippetIsAbsentAsync("""
            class C
            {
                void M(bool flag)
                {
                    flag.$$ToString();
                }
            }
            """);

    [Fact]
    public Task NoInlineSnippetWhenDottingBeforeContextualKeywordOnTheSameLineTest()
        => VerifySnippetIsAbsentAsync("""
            class C
            {
                void M(bool flag)
                {
                    flag.$$var a = 0;
                }
            }
            """);

    [Fact]
    public Task NoInlineSnippetForTypeItselfTest()
        => VerifySnippetIsAbsentAsync("""
            class C
            {
                void M()
                {
                    bool.$$
                }
            }
            """);

    [Fact]
    public Task NoInlineSnippetForTypeItselfTest_Parenthesized()
        => VerifySnippetIsAbsentAsync("""
            class C
            {
                void M()
                {
                    (bool).$$
                }
            }
            """);

    [Fact]
    public Task NoInlineSnippetForTypeItselfTest_BeforeContextualKeyword()
        => VerifySnippetIsAbsentAsync("""
            using System.Threading.Tasks;

            class C
            {
                async void M()
                {
                    bool.$$
                    await Task.Delay(10);
                }
            }
            """);
}
