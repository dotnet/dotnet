﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable disable

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.CodeStyle;
using Microsoft.CodeAnalysis.Editor.UnitTests.CodeActions;
using Microsoft.CodeAnalysis.GenerateComparisonOperators;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.GenerateComparisonOperators;

using VerifyCS = CSharpCodeRefactoringVerifier<
    GenerateComparisonOperatorsCodeRefactoringProvider>;

[UseExportProvider]
[Trait(Traits.Feature, Traits.Features.CodeActionsGenerateComparisonOperators)]
public sealed class GenerateComparisonOperatorsTests
{
    [Fact]
    public Task TestClass()
        => VerifyCS.VerifyRefactoringAsync(
            """
            using System;

            [||]class C : IComparable<C>
            {
                public int CompareTo(C c) => 0;
            }
            """,
            """
            using System;

            class C : IComparable<C>
            {
                public int CompareTo(C c) => 0;

                public static bool operator <(C left, C right)
                {
                    return left.CompareTo(right) < 0;
                }

                public static bool operator >(C left, C right)
                {
                    return left.CompareTo(right) > 0;
                }

                public static bool operator <=(C left, C right)
                {
                    return left.CompareTo(right) <= 0;
                }

                public static bool operator >=(C left, C right)
                {
                    return left.CompareTo(right) >= 0;
                }
            }
            """);

    [Fact]
    public Task TestPreferExpressionBodies()
        => new VerifyCS.Test
        {
            TestCode =
            """
            using System;

            [||]class C : IComparable<C>
            {
                public int CompareTo(C c) => 0;
            }
            """,
            FixedCode =
            """
            using System;

            class C : IComparable<C>
            {
                public int CompareTo(C c) => 0;

                public static bool operator <(C left, C right) => left.CompareTo(right) < 0;
                public static bool operator >(C left, C right) => left.CompareTo(right) > 0;
                public static bool operator <=(C left, C right) => left.CompareTo(right) <= 0;
                public static bool operator >=(C left, C right) => left.CompareTo(right) >= 0;
            }
            """,
            EditorConfig = CodeFixVerifierHelper.GetEditorConfigText(
                new OptionsCollection(LanguageNames.CSharp)
                {
                    { CSharpCodeStyleOptions.PreferExpressionBodiedOperators, CSharpCodeStyleOptions.WhenPossibleWithSuggestionEnforcement },
                }),
        }.RunAsync();

    [Fact]
    public Task TestExplicitImpl()
        => VerifyCS.VerifyRefactoringAsync(
            """
            using System;

            [||]class C : IComparable<C>
            {
                int IComparable<C>.CompareTo(C c) => 0;
            }
            """,
            """
            using System;

            class C : IComparable<C>
            {
                int IComparable<C>.CompareTo(C c) => 0;

                public static bool operator <(C left, C right)
                {
                    return ((IComparable<C>)left).CompareTo(right) < 0;
                }

                public static bool operator >(C left, C right)
                {
                    return ((IComparable<C>)left).CompareTo(right) > 0;
                }

                public static bool operator <=(C left, C right)
                {
                    return ((IComparable<C>)left).CompareTo(right) <= 0;
                }

                public static bool operator >=(C left, C right)
                {
                    return ((IComparable<C>)left).CompareTo(right) >= 0;
                }
            }
            """);

    [Fact]
    public Task TestOnInterface()
        => VerifyCS.VerifyRefactoringAsync(
            """
            using System;

            class C : [||]IComparable<C>
            {
                public int CompareTo(C c) => 0;
            }
            """,
            """
            using System;

            class C : IComparable<C>
            {
                public int CompareTo(C c) => 0;

                public static bool operator <(C left, C right)
                {
                    return left.CompareTo(right) < 0;
                }

                public static bool operator >(C left, C right)
                {
                    return left.CompareTo(right) > 0;
                }

                public static bool operator <=(C left, C right)
                {
                    return left.CompareTo(right) <= 0;
                }

                public static bool operator >=(C left, C right)
                {
                    return left.CompareTo(right) >= 0;
                }
            }
            """);

    [Fact]
    public Task TestAtEndOfInterface()
        => VerifyCS.VerifyRefactoringAsync(
            """
            using System;

            class C : IComparable<C>[||]
            {
                public int CompareTo(C c) => 0;
            }
            """,
            """
            using System;

            class C : IComparable<C>
            {
                public int CompareTo(C c) => 0;

                public static bool operator <(C left, C right)
                {
                    return left.CompareTo(right) < 0;
                }

                public static bool operator >(C left, C right)
                {
                    return left.CompareTo(right) > 0;
                }

                public static bool operator <=(C left, C right)
                {
                    return left.CompareTo(right) <= 0;
                }

                public static bool operator >=(C left, C right)
                {
                    return left.CompareTo(right) >= 0;
                }
            }
            """);

    [Fact]
    public Task TestInBody()
        => VerifyCS.VerifyRefactoringAsync(
            """
            using System;

            class C : IComparable<C>
            {
                public int CompareTo(C c) => 0;

            [||]
            }
            """,
            """
            using System;

            class C : IComparable<C>
            {
                public int CompareTo(C c) => 0;

                public static bool operator <(C left, C right)
                {
                    return left.CompareTo(right) < 0;
                }

                public static bool operator >(C left, C right)
                {
                    return left.CompareTo(right) > 0;
                }

                public static bool operator <=(C left, C right)
                {
                    return left.CompareTo(right) <= 0;
                }

                public static bool operator >=(C left, C right)
                {
                    return left.CompareTo(right) >= 0;
                }
            }
            """);

    [Fact]
    public async Task TestMissingWithoutCompareMethod()
    {
        var code = """
            using System;

            class C : {|CS0535:IComparable<C>|}
            {
            [||]
            }
            """;

        await VerifyCS.VerifyRefactoringAsync(code, code);
    }

    [Fact]
    public async Task TestMissingWithUnknownType()
    {
        var code = """
            using System;

            class C : IComparable<{|CS0246:Goo|}>
            {
                public int CompareTo({|CS0246:Goo|} g) => 0;

            [||]
            }
            """;

        await VerifyCS.VerifyRefactoringAsync(code, code);
    }

    [Fact]
    public async Task TestMissingWithAllExistingOperators()
    {
        var code =
            """
            using System;

            class C : IComparable<C>
            {
                public int CompareTo(C c) => 0;

                public static bool operator <(C left, C right)
                {
                    return left.CompareTo(right) < 0;
                }

                public static bool operator >(C left, C right)
                {
                    return left.CompareTo(right) > 0;
                }

                public static bool operator <=(C left, C right)
                {
                    return left.CompareTo(right) <= 0;
                }

                public static bool operator >=(C left, C right)
                {
                    return left.CompareTo(right) >= 0;
                }

            [||]
            }
            """;

        await VerifyCS.VerifyRefactoringAsync(code, code);
    }

    [Fact]
    public Task TestWithExistingOperator()
        => VerifyCS.VerifyRefactoringAsync(
            """
            using System;

            class C : IComparable<C>
            {
                public int CompareTo(C c) => 0;

                public static bool operator {|CS0216:<|}(C left, C right)
                {
                    return left.CompareTo(right) < 0;
                }

            [||]
            }
            """,
            """
            using System;

            class C : IComparable<C>
            {
                public int CompareTo(C c) => 0;

                public static bool operator <(C left, C right)
                {
                    return left.CompareTo(right) < 0;
                }

                public static bool operator >(C left, C right)
                {
                    return left.CompareTo(right) > 0;
                }

                public static bool operator <=(C left, C right)
                {
                    return left.CompareTo(right) <= 0;
                }

                public static bool operator >=(C left, C right)
                {
                    return left.CompareTo(right) >= 0;
                }
            }
            """);

    [Fact]
    public async Task TestMultipleInterfaces()
    {
        var code =
            """
            using System;

            class C : IComparable<C>, IComparable<int>
            {
                public int CompareTo(C c) => 0;
                public int CompareTo(int c) => 0;

            [||]
            }
            """;
        static string GetFixedCode(string type)
            => $$"""
            using System;

            class C : IComparable<C>, IComparable<int>
            {
                public int CompareTo(C c) => 0;
                public int CompareTo(int c) => 0;

                public static bool operator <(C left, {{type}} right)
                {
                    return left.CompareTo(right) < 0;
                }

                public static bool operator >(C left, {{type}} right)
                {
                    return left.CompareTo(right) > 0;
                }

                public static bool operator <=(C left, {{type}} right)
                {
                    return left.CompareTo(right) <= 0;
                }

                public static bool operator >=(C left, {{type}} right)
                {
                    return left.CompareTo(right) >= 0;
                }
            }
            """;

        await new VerifyCS.Test
        {
            TestCode = code,
            FixedCode = GetFixedCode("C"),
            CodeActionEquivalenceKey = "Generate_for_0_C",
        }.RunAsync();

        await new VerifyCS.Test
        {
            TestCode = code,
            FixedCode = GetFixedCode("int"),
            CodeActionIndex = 1,
            CodeActionEquivalenceKey = "Generate_for_0_int",
        }.RunAsync();
    }

    // TODO: Enable test on .NET Core
    // https://github.com/dotnet/roslyn/issues/71625
    [ConditionalFact(typeof(DesktopOnly))]
    public Task TestInInterfaceWithDefaultImpl()
        => VerifyCS.VerifyRefactoringAsync(
            """
            using System;

            interface C : IComparable<C>
            {
                int IComparable<C>.{|CS8701:CompareTo|}(C c) => 0;

            [||]
            }
            """,
            """
            using System;

            interface C : IComparable<C>
            {
                int IComparable<C>.{|CS8701:CompareTo|}(C c) => 0;

                public static bool operator {|CS8701:<|}(C left, C right)
                {
                    return left.CompareTo(right) < 0;
                }

                public static bool operator {|CS8701:>|}(C left, C right)
                {
                    return left.CompareTo(right) > 0;
                }

                public static bool operator {|CS8701:<=|}(C left, C right)
                {
                    return left.CompareTo(right) <= 0;
                }

                public static bool operator {|CS8701:>=|}(C left, C right)
                {
                    return left.CompareTo(right) >= 0;
                }
            }
            """);
}

