﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeStyle;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.CodeStyle;
using Microsoft.CodeAnalysis.CSharp.InitializeParameter;
using Microsoft.CodeAnalysis.Editor.UnitTests.CodeActions;
using Microsoft.CodeAnalysis.Test.Utilities;
using Microsoft.CodeAnalysis.Testing;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.InitializeParameter;

using VerifyCS = CSharpCodeRefactoringVerifier<CSharpAddParameterCheckCodeRefactoringProvider>;

[UseExportProvider]
[Trait(Traits.Feature, Traits.Features.CodeActionsInitializeParameter)]
public sealed class AddParameterCheckTests
{
    [Fact]
    public async Task TestEmptyFile()
    {
        var code = @"[||]";

        await VerifyCS.VerifyRefactoringAsync(code, code);
    }

    [Fact]
    public Task TestSimpleReferenceType_AlreadyNullChecked1()
        => new VerifyCS.Test
        {
            LanguageVersion = LanguageVersion.CSharp11,
            TestCode = """
            using System;

            class C
            {
                public C([||]string s)
                {
                    if (s is null)
                    {
                        throw new ArgumentNullException(nameof(s));
                    }
                }
            }
            """
        }.RunAsync();

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/61181")]
    public Task TestSimpleReferenceType_AlreadyNullChecked2()
        => new VerifyCS.Test
        {
            TestCode = """
                using System;

                class C
                {
                    public C([||]string s)
                    {
                        ArgumentNullException.ThrowIfNull(s);
                    }
                }
                """,
            LanguageVersion = LanguageVersion.CSharp11,
            ReferenceAssemblies = ReferenceAssemblies.Net.Net60,
        }.RunAsync();

    [Fact]
    public Task TestSimpleReferenceType()
        => VerifyCS.VerifyRefactoringAsync(
            """
            using System;

            class C
            {
                public C([||]string s)
                {
                }
            }
            """,
            """
            using System;

            class C
            {
                public C(string s)
                {
                    if (s is null)
                    {
                        throw new ArgumentNullException(nameof(s));
                    }
                }
            }
            """);

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/61181")]
    public Task TestSimpleReferenceType_ThrowIfNull()
        => new VerifyCS.Test
        {
            TestCode = """
                using System;

                class C
                {
                    public C([||]string s)
                    {
                    }
                }
                """,
            FixedCode = """
                using System;

                class C
                {
                    public C(string s)
                    {
                        ArgumentNullException.ThrowIfNull(s);
                    }
                }
                """,
            ReferenceAssemblies = ReferenceAssemblies.Net.Net60,
        }.RunAsync();

    [Fact]
    public Task TestSimpleReferenceType_CSharp6()
        => new VerifyCS.Test
        {
            LanguageVersion = LanguageVersion.CSharp6,
            TestCode = """
            using System;

            class C
            {
                public C([||]string s)
                {
                }
            }
            """,
            FixedCode = """
            using System;

            class C
            {
                public C(string s)
                {
                    if (s == null)
                    {
                        throw new ArgumentNullException(nameof(s));
                    }
                }
            }
            """
        }.RunAsync();

    [Fact]
    public Task TestNullable()
        => VerifyCS.VerifyRefactoringAsync(
            """
            using System;

            class C
            {
                public C([||]int? i)
                {
                }
            }
            """,
            """
            using System;

            class C
            {
                public C(int? i)
                {
                    if (i is null)
                    {
                        throw new ArgumentNullException(nameof(i));
                    }
                }
            }
            """);

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/47030")]
    public async Task TestNotOnOutParameter()
    {
        var code = """
            class C
            {
                public C([||]out string s)
                {
                    s = "";
                }
            }
            """;
        await VerifyCS.VerifyRefactoringAsync(code, code);
    }

    [Fact]
    public async Task TestNotOnValueType()
    {
        var code = """
            using System;

            class C
            {
                public C([||]DateTime d)
                {
                }
            }
            """;

        await VerifyCS.VerifyRefactoringAsync(code, code);
    }

    [Fact]
    public async Task TestNotOnInterfaceParameter()
    {
        var code = """
            using System;

            interface I
            {
                void M([||]string s);
            }
            """;

        await VerifyCS.VerifyRefactoringAsync(code, code);
    }

    [Fact]
    public async Task TestNotOnNullableParameter()
    {
        var code = """
            #nullable enable

            using System;

            class C
            {
                void M([||]string? s)
                {
                }
            }
            """;

        await VerifyCS.VerifyRefactoringAsync(code, code);
    }

    [Fact]
    public async Task TestNotOnAbstractParameter()
    {
        var code = """
            using System;

            abstract class C
            {
                public abstract void M([||]string s);
            }
            """;
        await VerifyCS.VerifyRefactoringAsync(code, code);
    }

    [Fact]
    public async Task TestNotOnExternParameter()
    {
        var code = """
            using System;

            class C
            {
                extern void M([||]string s);
            }
            """;
        await VerifyCS.VerifyRefactoringAsync(code, code);
    }

    [Theory]
    [InlineData(LanguageVersion.CSharp11)]
    [InlineData(LanguageVersion.CSharp8)]
    public Task TestNotOnPartialMethodDefinition1(LanguageVersion languageVersion)
        => new VerifyCS.Test
        {
            LanguageVersion = languageVersion,
            TestCode = """
            using System;

            partial class C
            {
                partial void M([||]string s);

                partial void M(string s)
                {
                }
            }
            """
        }.RunAsync();

    [Fact]
    public Task TestNotOnExtendedPartialMethodDefinition1()
        => new VerifyCS.Test
        {
            LanguageVersion = LanguageVersion.CSharp9,
            TestCode = """
            using System;

            partial class C
            {
                public partial void M([||]string s);

                public partial void M(string s)
                {
                }
            }
            """
        }.RunAsync();

    [Theory]
    [InlineData(LanguageVersion.CSharp11)]
    [InlineData(LanguageVersion.CSharp8)]
    public Task TestNotOnPartialMethodDefinition2(LanguageVersion languageVersion)
        => new VerifyCS.Test
        {
            LanguageVersion = languageVersion,
            TestCode = """
            using System;

            partial class C
            {
                partial void M(string s)
                {
                }

                partial void M([||]string s);
            }
            """
        }.RunAsync();

    [Fact]
    public Task TestNotOnExtendedPartialMethodDefinition2()
        => new VerifyCS.Test
        {
            LanguageVersion = LanguageVersion.CSharp9,
            TestCode = """
            using System;

            partial class C
            {
                public partial void M(string s)
                {
                }

                public partial void M([||]string s);
            }
            """
        }.RunAsync();

    [Fact]
    public Task TestOnPartialMethodImplementation1()
        => VerifyCS.VerifyRefactoringAsync(
            """
            using System;

            partial class C
            {
                partial void M(string s);

                partial void M([||]string s)
                {
                }
            }
            """,
            """
            using System;

            partial class C
            {
                partial void M(string s);

                partial void M(string s)
                {
                    if (s is null)
                    {
                        throw new ArgumentNullException(nameof(s));
                    }
                }
            }
            """);

    [Fact]
    public Task TestOnExtendedPartialMethodImplementation1()
        => new VerifyCS.Test
        {
            LanguageVersion = LanguageVersion.CSharp9,
            TestCode = """
            using System;

            partial class C
            {
                public partial void M(string s);

                public partial void M([||]string s)
                {
                }
            }
            """,
            FixedCode = """
            using System;

            partial class C
            {
                public partial void M(string s);

                public partial void M(string s)
                {
                    if (s is null)
                    {
                        throw new ArgumentNullException(nameof(s));
                    }
                }
            }
            """
        }.RunAsync();

    [Fact]
    public Task TestOnPartialMethodImplementation2()
        => VerifyCS.VerifyRefactoringAsync(
            """
            using System;

            partial class C
            {
                partial void M([||]string s)
                {
                }

                partial void M(string s);
            }
            """,
            """
            using System;

            partial class C
            {
                partial void M(string s)
                {
                    if (s is null)
                    {
                        throw new ArgumentNullException(nameof(s));
                    }
                }

                partial void M(string s);
            }
            """);

    [Fact]
    public Task TestOnExtendedPartialMethodImplementation2()
        => new VerifyCS.Test
        {
            LanguageVersion = LanguageVersion.CSharp9,
            TestCode = """
            using System;

            partial class C
            {
                public partial void M([||]string s)
                {
                }

                public partial void M(string s);
            }
            """,
            FixedCode = """
            using System;

            partial class C
            {
                public partial void M(string s)
                {
                    if (s is null)
                    {
                        throw new ArgumentNullException(nameof(s));
                    }
                }

                public partial void M(string s);
            }
            """
        }.RunAsync();

    [Fact]
    public Task TestUpdateExistingFieldAssignment()
        => VerifyCS.VerifyRefactoringAsync(
            """
            using System;

            class C
            {
                private string _s;

                public C([||]string s)
                {
                    _s = s;
                }
            }
            """,
            """
            using System;

            class C
            {
                private string _s;

                public C(string s)
                {
                    _s = s ?? throw new ArgumentNullException(nameof(s));
                }
            }
            """);

    [Fact]
    public Task TestMultiNullableParameters()
        => new VerifyCS.Test
        {
            TestCode = """
            using System;

            class C
            {
                public C([||]string a, string b, string c)
                {
                }
            }
            """,
            FixedCode = $$"""
            using System;

            class C
            {
                public C(string a, string b, string c)
                {
                    if (string.IsNullOrEmpty(a))
                    {
                        throw new ArgumentException($"{{string.Format(FeaturesResources._0_cannot_be_null_or_empty, "{nameof(a)}").Replace("""
                               "
                               """, """
                               \"
                               """)}}", nameof(a));
                    }

                    if (string.IsNullOrEmpty(b))
                    {
                        throw new ArgumentException($"{{string.Format(FeaturesResources._0_cannot_be_null_or_empty, "{nameof(b)}").Replace("""
                                        "
                                        """, """
                                        \"
                                        """)}}", nameof(b));
                    }

                    if (string.IsNullOrEmpty(c))
                    {
                        throw new ArgumentException($"{{string.Format(FeaturesResources._0_cannot_be_null_or_empty, "{nameof(c)}").Replace("""
                                                 "
                                                 """, """
                                                 \"
                                                 """)}}", nameof(c));
                    }
                }
            }
            """,
            CodeActionIndex = 3,
            CodeActionEquivalenceKey = nameof(FeaturesResources.Add_null_checks_for_all_parameters)
        }.RunAsync();

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/61181")]
    public Task TestMultiNullableParameters_Net7()
        => new VerifyCS.Test
        {
            TestCode = """
            using System;

            class C
            {
                public C([||]string a, string b, string c)
                {
                }
            }
            """,
            FixedCode = $$"""
            using System;

            class C
            {
                public C(string a, string b, string c)
                {
                    ArgumentException.ThrowIfNullOrEmpty(a);
                    ArgumentException.ThrowIfNullOrEmpty(b);
                    ArgumentException.ThrowIfNullOrEmpty(c);
                }
            }
            """,
            CodeActionIndex = 3,
            CodeActionEquivalenceKey = nameof(FeaturesResources.Add_null_checks_for_all_parameters),
            ReferenceAssemblies = ReferenceAssemblies.Net.Net70,
        }.RunAsync();

    [Fact]
    public Task TestMultiNullableParametersSomeNullableReferenceTypes()
        => new VerifyCS.Test
        {
            TestCode = """
            #nullable enable

            using System;

            class C
            {
                public C([||]string a, string b, string? c)
                {
                }
            }
            """,
            FixedCode = $$"""
            #nullable enable

            using System;

            class C
            {
                public C(string a, string b, string? c)
                {
                    if (string.IsNullOrEmpty(a))
                    {
                        throw new ArgumentException($"{{string.Format(FeaturesResources._0_cannot_be_null_or_empty, "{nameof(a)}").Replace("""
                               "
                               """, """
                               \"
                               """)}}", nameof(a));
                    }

                    if (string.IsNullOrEmpty(b))
                    {
                        throw new ArgumentException($"{{string.Format(FeaturesResources._0_cannot_be_null_or_empty, "{nameof(b)}").Replace("""
                                        "
                                        """, """
                                        \"
                                        """)}}", nameof(b));
                    }
                }
            }
            """,
            CodeActionIndex = 3,
            CodeActionEquivalenceKey = nameof(FeaturesResources.Add_null_checks_for_all_parameters)
        }.RunAsync();

    [Fact]
    public async Task TestCursorNotOnParameters()
    {
        var code = """
            using System;

            class C
            {
                public C(string a[|,|] string b, string c)
                {
                }
            }
            """;
        await VerifyCS.VerifyRefactoringAsync(code, code);
    }

    [Fact]
    public Task TestMultiNullableWithCursorOnNonNullable()
        => new VerifyCS.Test
        {
            TestCode = """
            using System;

            class C
            {
                public C(string a, [||]bool b, string c)
                {
                }
            }
            """,
            FixedCode = $$"""
            using System;

            class C
            {
                public C(string a, bool b, string c)
                {
                    if (string.IsNullOrEmpty(a))
                    {
                        throw new ArgumentException($"{{string.Format(FeaturesResources._0_cannot_be_null_or_empty, "{nameof(a)}").Replace("""
                               "
                               """, """
                               \"
                               """)}}", nameof(a));
                    }

                    if (string.IsNullOrEmpty(c))
                    {
                        throw new ArgumentException($"{{string.Format(FeaturesResources._0_cannot_be_null_or_empty, "{nameof(c)}").Replace("""
                                        "
                                        """, """
                                        \"
                                        """)}}", nameof(c));
                    }
                }
            }
            """,
            CodeActionEquivalenceKey = nameof(FeaturesResources.Add_null_checks_for_all_parameters)
        }.RunAsync();

    [Fact]
    public Task TestMultiNullableNonNullable()
        => new VerifyCS.Test
        {
            TestCode = """
            using System;

            class C
            {
                public C([||]string a, bool b, string c)
                {
                }
            }
            """,
            FixedCode = $$"""
            using System;

            class C
            {
                public C(string a, bool b, string c)
                {
                    if (string.IsNullOrEmpty(a))
                    {
                        throw new ArgumentException($"{{string.Format(FeaturesResources._0_cannot_be_null_or_empty, "{nameof(a)}").Replace("""
                               "
                               """, """
                               \"
                               """)}}", nameof(a));
                    }

                    if (string.IsNullOrEmpty(c))
                    {
                        throw new ArgumentException($"{{string.Format(FeaturesResources._0_cannot_be_null_or_empty, "{nameof(c)}").Replace("""
                                        "
                                        """, """
                                        \"
                                        """)}}", nameof(c));
                    }
                }
            }
            """,
            CodeActionIndex = 3,
            CodeActionEquivalenceKey = nameof(FeaturesResources.Add_null_checks_for_all_parameters)
        }.RunAsync();

    [Fact]
    public Task TestMultiNullableStringsAndObjects()
        => new VerifyCS.Test
        {
            TestCode = """
            using System;

            class C
            {
                public C([||]string a, object b, string c)
                {
                }
            }
            """,
            FixedCode = $$"""
            using System;

            class C
            {
                public C(string a, object b, string c)
                {
                    if (string.IsNullOrEmpty(a))
                    {
                        throw new ArgumentException($"{{string.Format(FeaturesResources._0_cannot_be_null_or_empty, "{nameof(a)}").Replace("""
                               "
                               """, """
                               \"
                               """)}}", nameof(a));
                    }

                    if (b is null)
                    {
                        throw new ArgumentNullException(nameof(b));
                    }

                    if (string.IsNullOrEmpty(c))
                    {
                        throw new ArgumentException($"{{string.Format(FeaturesResources._0_cannot_be_null_or_empty, "{nameof(c)}").Replace("""
                                        "
                                        """, """
                                        \"
                                        """)}}", nameof(c));
                    }
                }
            }
            """,
            CodeActionIndex = 3,
            CodeActionEquivalenceKey = nameof(FeaturesResources.Add_null_checks_for_all_parameters)
        }.RunAsync();

    [Fact]
    public Task TestMultiNullableObjects()
        => new VerifyCS.Test
        {
            TestCode = """
            using System;

            class C
            {
                public C([||]object a, object b, object c)
                {
                }
            }
            """,
            FixedCode = """
            using System;

            class C
            {
                public C(object a, object b, object c)
                {
                    if (a is null)
                    {
                        throw new ArgumentNullException(nameof(a));
                    }

                    if (b is null)
                    {
                        throw new ArgumentNullException(nameof(b));
                    }

                    if (c is null)
                    {
                        throw new ArgumentNullException(nameof(c));
                    }
                }
            }
            """,
            CodeActionIndex = 1,
            CodeActionEquivalenceKey = nameof(FeaturesResources.Add_null_checks_for_all_parameters)
        }.RunAsync();

    [Fact]
    public Task TestMultiNullableStructs()
        => new VerifyCS.Test
        {
            TestCode = """
            using System;

            class C
            {
                public C([||]int ? a, bool ? b, double ? c)
                {
                }
            }
            """,
            FixedCode = """
            using System;

            class C
            {
                public C(int ? a, bool ? b, double ? c)
                {
                    if (a is null)
                    {
                        throw new ArgumentNullException(nameof(a));
                    }

                    if (b is null)
                    {
                        throw new ArgumentNullException(nameof(b));
                    }

                    if (c is null)
                    {
                        throw new ArgumentNullException(nameof(c));
                    }
                }
            }
            """,
            CodeActionIndex = 1,
            CodeActionEquivalenceKey = nameof(FeaturesResources.Add_null_checks_for_all_parameters)
        }.RunAsync();

    [Fact]
    public Task TestUpdateExistingPropertyAssignment()
        => VerifyCS.VerifyRefactoringAsync(
            """
            using System;

            class C
            {
                private string S;

                public C([||]string s)
                {
                    S = s;
                }
            }
            """,
            """
            using System;

            class C
            {
                private string S;

                public C(string s)
                {
                    S = s ?? throw new ArgumentNullException(nameof(s));
                }
            }
            """);

    [Fact]
    public Task DoNotUseThrowExpressionBeforeCSharp7()
        => new VerifyCS.Test
        {
            LanguageVersion = LanguageVersion.CSharp6,
            TestCode = """
            using System;

            class C
            {
                private string S;

                public C([||]string s)
                {
                    S = s;
                }
            }
            """,
            FixedCode = """
            using System;

            class C
            {
                private string S;

                public C(string s)
                {
                    if (s == null)
                    {
                        throw new ArgumentNullException(nameof(s));
                    }

                    S = s;
                }
            }
            """
        }.RunAsync();

    [Fact]
    public Task RespectUseThrowExpressionOption()
        => new VerifyCS.Test
        {
            TestCode = """
            using System;

            class C
            {
                private string S;

                public C([||]string s)
                {
                    S = s;
                }
            }
            """,
            FixedCode = """
            using System;

            class C
            {
                private string S;

                public C(string s)
                {
                    if (s is null)
                    {
                        throw new ArgumentNullException(nameof(s));
                    }

                    S = s;
                }
            }
            """,
            Options =
            {
                { CSharpCodeStyleOptions.PreferThrowExpression, false, NotificationOption2.Silent }
            }
        }.RunAsync();

    [Fact]
    public Task TestUpdateExpressionBody1()
        => VerifyCS.VerifyRefactoringAsync(
            """
            using System;

            class C
            {
                private string S;

                public C([||]string s)
                    => S = s;
            }
            """,
            """
            using System;

            class C
            {
                private string S;

                public C(string s)
                    => S = s ?? throw new ArgumentNullException(nameof(s));
            }
            """);

    [Fact]
    public Task TestUpdateExpressionBody2()
        => VerifyCS.VerifyRefactoringAsync(
            """
            using System;

            class C
            {
                public C([||]string s)
                    => Init();

                private void Init()
                {
                }
            }
            """,
            """
            using System;

            class C
            {
                public C(string s)
                {
                    if (s is null)
                    {
                        throw new ArgumentNullException(nameof(s));
                    }

                    Init();
                }

                private void Init()
                {
                }
            }
            """);

    [Fact]
    public Task TestUpdateExpressionBody3()
        => new VerifyCS.Test
        {
            TestCode = """
            using System;

            class C
            {
                public C([||]string s)
                    => Init();

                private void Init()
                {
                }
            }
            """,
            FixedCode = """
            using System;

            class C
            {
                public C(string s)
                {
                    if (s is null)
                    {
                        throw new ArgumentNullException(nameof(s));
                    }

                    Init();
                }

                private void Init()
                {
                }
            }
            """,
            Options =
            {
                { CSharpCodeStyleOptions.PreferExpressionBodiedConstructors, CSharpCodeStyleOptions.WhenPossibleWithSuggestionEnforcement }
            }
        }.RunAsync();

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/20983")]
    public Task TestUpdateLocalFunctionExpressionBody_NonVoid()
        => VerifyCS.VerifyRefactoringAsync(
            """
            using System;

            class C
            {
                void M()
                {
                    int F([||]string s) => Init();
                }

                private int Init() => 1;
            }
            """,
            """
            using System;

            class C
            {
                void M()
                {
                    int F(string s)
                    {
                        if (s is null)
                        {
                            throw new ArgumentNullException(nameof(s));
                        }

                        return Init();
                    }
                }

                private int Init() => 1;
            }
            """);

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/20983")]
    public Task TestUpdateLocalFunctionExpressionBody_Void()
        => VerifyCS.VerifyRefactoringAsync(
            """
            using System;

            class C
            {
                void M()
                {
                    void F([||]string s) => Init();
                }

                private int Init() => 1;
            }
            """,
            """
            using System;

            class C
            {
                void M()
                {
                    void F(string s)
                    {
                        if (s is null)
                        {
                            throw new ArgumentNullException(nameof(s));
                        }

                        Init();
                    }
                }

                private int Init() => 1;
            }
            """);

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/20983")]
    public Task TestUpdateLambdaExpressionBody_NonVoid()
        => VerifyCS.VerifyRefactoringAsync(
            """
            using System;

            class C
            {
                void M()
                {
                    Func<string, int> f = [||]s => GetValue();

                    int GetValue() => 0;
                }
            }
            """,
            """
            using System;

            class C
            {
                void M()
                {
                    Func<string, int> f = s =>
                    {
                        if (s is null)
                        {
                            throw new ArgumentNullException(nameof(s));
                        }

                        return GetValue();
                    };

                    int GetValue() => 0;
                }
            }
            """);

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/20983")]
    public Task TestUpdateLambdaExpressionBody_Void()
        => VerifyCS.VerifyRefactoringAsync(
            """
            using System;

            class C
            {
                void M()
                {
                    Action<string> f = [||]s => NoValue();

                    void NoValue() { }
                }
            }
            """,
            """
            using System;

            class C
            {
                void M()
                {
                    Action<string> f = s =>
                    {
                        if (s is null)
                        {
                            throw new ArgumentNullException(nameof(s));
                        }

                        NoValue();
                    };

                    void NoValue() { }
                }
            }
            """);

    [Fact]
    public Task TestInsertAfterExistingNullCheck1()
        => VerifyCS.VerifyRefactoringAsync(
            """
            using System;

            class C
            {
                public C(string a, [||]string s)
                {
                    if (a == null)
                    {
                    }
                }
            }
            """,
            """
            using System;

            class C
            {
                public C(string a, string s)
                {
                    if (a == null)
                    {
                    }

                    if (s is null)
                    {
                        throw new ArgumentNullException(nameof(s));
                    }
                }
            }
            """);

    [Fact]
    public Task TestInsertBeforeExistingNullCheck1()
        => VerifyCS.VerifyRefactoringAsync(
            """
            using System;

            class C
            {
                public C(string [||]a, string s)
                {
                    if (s == null)
                    {
                    }
                }
            }
            """,
            """
            using System;

            class C
            {
                public C(string a, string s)
                {
                    if (a is null)
                    {
                        throw new ArgumentNullException(nameof(a));
                    }

                    if (s == null)
                    {
                    }
                }
            }
            """);

    [Fact]
    public async Task TestMissingWithExistingNullCheck1()
    {
        var code = """
            using System;

            class C
            {
                public C([||]string s)
                {
                    if (s == null)
                    {
                        throw new ArgumentNullException();
                    }
                }
            }
            """;
        await VerifyCS.VerifyRefactoringAsync(code, code);
    }

    [Fact]
    public async Task TestMissingWithExistingNullCheck2()
    {
        var code = """
            using System;

            class C
            {
                private string _s;

                public C([||]string s)
                {
                    _s = s ?? throw new ArgumentNullException();
                }
            }
            """;
        await VerifyCS.VerifyRefactoringAsync(code, code);
    }

    [Fact]
    public async Task TestMissingWithExistingNullCheck3()
    {
        var code = """
            using System;

            class C
            {
                public C([||]string s)
                {
                    if (string.IsNullOrEmpty(s))
                    {
                    }
                }
            }
            """;
        await VerifyCS.VerifyRefactoringAsync(code, code);
    }

    [Fact]
    public async Task TestMissingWithExistingNullCheck4()
    {
        var code = """
            using System;

            class C
            {
                public C([||]string s)
                {
                    if (string.IsNullOrWhiteSpace(s))
                    {
                    }
                }
            }
            """;
        await VerifyCS.VerifyRefactoringAsync(code, code);
    }

    [Fact]
    public async Task TestMissingWithExistingNullCheck5()
    {
        var code = """
            using System;

            class C
            {
                public C([||]string s)
                {
                    if (null == s)
                    {
                        throw new ArgumentNullException();
                    }
                }
            }
            """;
        await VerifyCS.VerifyRefactoringAsync(code, code);
    }

    [Fact]
    public async Task TestMissingWithExistingNullCheck6()
    {
        var code = """
            using System;

            class C
            {
                public C([||]string s)
                {
                    if (s is null)
                    {
                        throw new ArgumentNullException();
                    }
                }
            }
            """;
        await VerifyCS.VerifyRefactoringAsync(code, code);
    }

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/20983")]
    public async Task TestMissingWithExistingNullCheckInLocalFunction()
    {
        var code = """
            using System;

            class C
            {
                public C()
                {
                    void F([||]string s)
                    {
                        if (s == null)
                        {
                            throw new ArgumentNullException();
                        }
                    }
                }
            }
            """;
        await VerifyCS.VerifyRefactoringAsync(code, code);
    }

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/20983")]
    public async Task TestMissingWithExistingNullCheckInLambda()
    {
        var code = """
            using System;

            class C
            {
                public C()
                {
                    Action<string> f = ([||]string s) => { if (s == null) { throw new ArgumentNullException(nameof(s)); } };
                }
            }
            """;
        await VerifyCS.VerifyRefactoringAsync(code, code);
    }

    [Fact]
    public async Task TestMissingWithoutParameterName()
    {
        var code = """
            using System;

            class C
            {
                public C([||]string{|CS1001:)|}
                {
                }
            }
            """;
        await VerifyCS.VerifyRefactoringAsync(code, code);
    }

    [Fact]
    public Task TestInMethod()
        => VerifyCS.VerifyRefactoringAsync(
            """
            using System;

            class C
            {
                void F([||]string s)
                {
                }
            }
            """,
            """
            using System;

            class C
            {
                void F(string s)
                {
                    if (s is null)
                    {
                        throw new ArgumentNullException(nameof(s));
                    }
                }
            }
            """);

    [Fact]
    public Task TestInOperator()
        => VerifyCS.VerifyRefactoringAsync(
            """
            using System;

            class C
            {
                public static C operator +(C c1, [||]string s)
                {
                    return null;
                }
            }
            """,
            """
            using System;

            class C
            {
                public static C operator +(C c1, [||]string s)
                {
                    if (s is null)
                    {
                        throw new ArgumentNullException(nameof(s));
                    }

                    return null;
                }
            }
            """);

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/20983")]
    public Task TestOnSimpleLambdaParameter()
        => VerifyCS.VerifyRefactoringAsync(
            """
            using System;

            class C
            {
                public C()
                {
                    Func<string, int> f = [||]s => { return 0; };
                }
            }
            """,
            """
            using System;

            class C
            {
                public C()
                {
                    Func<string, int> f = s =>
                    {
                        if (s is null)
                        {
                            throw new ArgumentNullException(nameof(s));
                        }

                        return 0;
                    };
                }
            }
            """);

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/20983")]
    public Task TestOnSimpleLambdaParameter_EmptyBlock()
        => VerifyCS.VerifyRefactoringAsync(
            """
            using System;

            class C
            {
                public C()
                {
                    Action<string> f = [||]s => { };
                }
            }
            """,
            """
            using System;

            class C
            {
                public C()
                {
                    Action<string> f = s =>
                    {
                        if (s is null)
                        {
                            throw new ArgumentNullException(nameof(s));
                        }
                    };
                }
            }
            """);

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/20983")]
    public Task TestOnParenthesizedLambdaParameter()
        => VerifyCS.VerifyRefactoringAsync(
            """
            using System;

            class C
            {
                public C()
                {
                    Func<string, int> f = ([||]string s) => { return 0; };
                }
            }
            """,
            """
            using System;

            class C
            {
                public C()
                {
                    Func<string, int> f = (string s) =>
                    {
                        if (s is null)
                        {
                            throw new ArgumentNullException(nameof(s));
                        }

                        return 0;
                    };
                }
            }
            """);

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/20983")]
    public Task TestOnDiscardLambdaParameter1()
        => new VerifyCS.Test
        {
            LanguageVersion = LanguageVersion.CSharp11,
            TestCode = """
            using System;

            class C
            {
                public C()
                {
                    Func<string, int> f = ([||]_) => { return 0; };
                }
            }
            """,
            FixedCode = """
            using System;

            class C
            {
                public C()
                {
                    Func<string, int> f = (_) =>
                    {
                        if (_ is null)
                        {
                            throw new ArgumentNullException(nameof(_));
                        }

                        return 0;
                    };
                }
            }
            """
        }.RunAsync();

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/20983")]
    public Task TestOnDiscardLambdaParameter2()
        => new VerifyCS.Test
        {
            LanguageVersion = LanguageVersion.CSharp11,
            TestCode = """
            using System;

            class C
            {
                public C()
                {
                    Func<string, string, int> f = ([||]_, _) => { return 0; };
                }
            }
            """
        }.RunAsync();

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/20983")]
    public Task TestOnAnonymousMethodParameter()
        => VerifyCS.VerifyRefactoringAsync(
            """
            using System;

            class C
            {
                public C()
                {
                    Func<string, int> f = delegate ([||]string s) { return 0; };
                }
            }
            """,
            """
            using System;

            class C
            {
                public C()
                {
                    Func<string, int> f = delegate (string s)
                    {
                        if (s is null)
                        {
                            throw new ArgumentNullException(nameof(s));
                        }

                        return 0;
                    };
                }
            }
            """);

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/20983")]
    public Task TestOnLocalFunctionParameter()
        => VerifyCS.VerifyRefactoringAsync(
            """
            using System;

            class C
            {
                public C()
                {
                    void F([||]string s)
                    {
                    }
                }
            }
            """,
            """
            using System;

            class C
            {
                public C()
                {
                    void F(string s)
                    {
                        if (s is null)
                        {
                            throw new ArgumentNullException(nameof(s));
                        }
                    }
                }
            }
            """);

    [Fact]
    public async Task TestNotOnIndexerParameter()
    {
        var code = """
            class C
            {
                int this[[||]string s]
                {
                    get
                    {
                        return 0;
                    }
                }
            }
            """;

        await VerifyCS.VerifyRefactoringAsync(code, code);
    }

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/63307")]
    public Task TestNotOnIndexerParameterInRecordWithParameter()
        => new VerifyCS.Test
        {
            TestCode = """
            record R(string S)
            {
                int this[[||]string s]
                {
                    get
                    {
                        return 0;
                    }
                }
            }
            """,
            LanguageVersion = LanguageVersion.CSharp11,
            ReferenceAssemblies = ReferenceAssemblies.Net.Net50,
        }.RunAsync();

    [Fact]
    public async Task TestNotOnIndexerParameters()
    {
        var code = """
            class C
            {
                int this[[|object a|], object b, object c]
                {
                    get
                    {
                        return 0;
                    }
                }
            }
            """;

        await VerifyCS.VerifyRefactoringAsync(code, code);
    }

    [Fact]
    public Task TestSpecialStringCheck1()
        => new VerifyCS.Test
        {
            TestCode = """
            using System;

            class C
            {
                public C([||]string s)
                {
                }
            }
            """,
            FixedCode = $$"""
            using System;

            class C
            {
                public C(string s)
                {
                    if (string.IsNullOrEmpty(s))
                    {
                        throw new ArgumentException($"{{string.Format(FeaturesResources._0_cannot_be_null_or_empty, "{nameof(s)}").Replace("""
                               "
                               """, """
                               \"
                               """)}}", nameof(s));
                    }
                }
            }
            """,
            CodeActionIndex = 1,
            CodeActionEquivalenceKey = "Add_string_IsNullOrEmpty_check"
        }.RunAsync();

    [Fact]
    public Task TestSpecialStringCheck2()
        => new VerifyCS.Test
        {
            TestCode = """
            using System;

            class C
            {
                public C([||]string s)
                {
                }
            }
            """,
            FixedCode = $$"""
            using System;

            class C
            {
                public C(string s)
                {
                    if (string.IsNullOrWhiteSpace(s))
                    {
                        throw new ArgumentException($"{{string.Format(FeaturesResources._0_cannot_be_null_or_whitespace, "{nameof(s)}").Replace("""
                               "
                               """, """
                               \"
                               """)}}", nameof(s));
                    }
                }
            }
            """,
            CodeActionIndex = 2,
            CodeActionEquivalenceKey = "Add_string_IsNullOrWhiteSpace_check"
        }.RunAsync();

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/61181")]
    public Task TestSpecialStringCheck2_Net8()
        => new VerifyCS.Test
        {
            TestCode = """
            using System;

            class C
            {
                public C([||]string s)
                {
                }
            }
            """,
            FixedCode = $$"""
            using System;

            class C
            {
                public C(string s)
                {
                    ArgumentException.ThrowIfNullOrWhiteSpace(s);
                }
            }
            """,
            CodeActionIndex = 2,
            CodeActionEquivalenceKey = "Add_string_IsNullOrWhiteSpace_check",
            ReferenceAssemblies = ReferenceAssemblies.Net.Net80,
        }.RunAsync();

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/51338")]
    [UseCulture("de-DE", "de-DE")]
    public Task TestSpecialStringCheck3()
        => new VerifyCS.Test
        {
            TestCode = """
            using System;

            class C
            {
                public C([||]string s)
                {
                }
            }
            """,
            FixedCode = $$"""
            using System;

            class C
            {
                public C(string s)
                {
                    if (string.IsNullOrEmpty(s))
                    {
                        throw new ArgumentException($"{{string.Format(FeaturesResources._0_cannot_be_null_or_empty, "{nameof(s)}").Replace("""
                               "
                               """, """
                               \"
                               """)}}", nameof(s));
                    }
                }
            }
            """,
            CodeActionIndex = 1,
            CodeActionEquivalenceKey = "Add_string_IsNullOrEmpty_check"
        }.RunAsync();

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/19173")]
    public async Task TestMissingOnUnboundTypeWithExistingNullCheck()
    {
        var code = """
            class C
            {
                public C(string [||]s)
                {
                    if (s == null)
                    {
                        throw new System.Exception();
                    }
                }
            }
            """;
        await VerifyCS.VerifyRefactoringAsync(code, code);
    }

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/19174")]
    public Task TestRespectPredefinedTypePreferences()
        => new VerifyCS.Test
        {
            TestCode = """
            using System;

            class Program
            {
                static void Main([||]String bar)
                {
                }
            }
            """,
            FixedCode = $$"""
            using System;

            class Program
            {
                static void Main(String bar)
                {
                    if (String.IsNullOrEmpty(bar))
                    {
                        throw new ArgumentException($"{{string.Format(FeaturesResources._0_cannot_be_null_or_empty, "{nameof(bar)}").Replace("""
                               "
                               """, """
                               \"
                               """)}}", nameof(bar));
                    }
                }
            }
            """,
            CodeActionIndex = 1,
            CodeActionEquivalenceKey = "Add_string_IsNullOrEmpty_check",
            Options =
            {
                { CodeStyleOptions2.PreferIntrinsicPredefinedTypeKeywordInMemberAccess, CodeStyleOption2.FalseWithSuggestionEnforcement }
            }
        }.RunAsync();

    [Theory, WorkItem("https://github.com/dotnet/roslyn/issues/19172")]
    [InlineData((int)PreferBracesPreference.None)]
    [InlineData((int)PreferBracesPreference.WhenMultiline)]
    public Task TestPreferNoBlock(int preferBraces)
        => new VerifyCS.Test
        {
            TestCode = """
            using System;

            class C
            {
                public C([||]string s)
                {
                }
            }
            """,
            FixedCode = """
            using System;

            class C
            {
                public C(string s)
                {
                    if (s is null) throw new ArgumentNullException(nameof(s));
                }
            }
            """,
            Options =
            {
                { CSharpCodeStyleOptions.PreferBraces, new CodeStyleOption2<PreferBracesPreference>((PreferBracesPreference)preferBraces, NotificationOption2.Silent) },
            }
        }.RunAsync();

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/19956")]
    public Task TestNoBlock()
        => new VerifyCS.Test
        {
            TestCode = """
            using System;

            class C
            {
                public C(string s[||])
            }
            """,
            ExpectedDiagnostics = {
                // /0/Test0.cs(6,12): error CS0501: 'C.C(string)' must declare a body because it is not marked abstract, extern, or partial
                DiagnosticResult.CompilerError("CS0501").WithLocation(5, 12).WithArguments("C.C(string)"),
                // /0/Test0.cs(6,23): error CS1002: ; expected
                DiagnosticResult.CompilerError("CS1002").WithLocation(5, 23),
            },
            FixedState =
            {
                Sources = { """
                    using System;

                    class C
                    {
                        public C(string s)
                        {
                            if (s is null)
                            {
                                throw new ArgumentNullException(nameof(s));
                            }
                        }
                    }
                    """ },
                InheritanceMode = StateInheritanceMode.Explicit
            }
        }.RunAsync();

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/21501")]
    public Task TestInArrowExpression1()
        => VerifyCS.VerifyRefactoringAsync(
            """
            using System;
            using System.Linq;

            class C
            {
                public int Foo(int[] array[||]) =>
                    array.Where(x => x > 3)
                        .OrderBy(x => x)
                        .Count();
            }
            """,
            """
            using System;
            using System.Linq;

            class C
            {
                public int Foo(int[] array)
                {
                    if (array is null)
                    {
                        throw new ArgumentNullException(nameof(array));
                    }

                    return array.Where(x => x > 3)
                        .OrderBy(x => x)
                        .Count();
                }
            }
            """);

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/21501")]
    public Task TestInArrowExpression2()
        => VerifyCS.VerifyRefactoringAsync(
            """
            using System;
            using System.Linq;

            class C
            {
                public int Foo(int[] array[||]) /* Bar */ => /* Bar */
                    array.Where(x => x > 3)
                        .OrderBy(x => x)
                        .Count(); /* Bar */
            }
            """,
            """
            using System;
            using System.Linq;

            class C
            {
                public int Foo(int[] array) /* Bar */
                {
                    if (array is null)
                    {
                        throw new ArgumentNullException(nameof(array));
                    }
                    /* Bar */
                    return array.Where(x => x > 3)
                        .OrderBy(x => x)
                        .Count(); /* Bar */
                }
            }
            """);

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/21501")]
    public async Task TestMissingInArrowExpression1()
    {
        var code = """
            using System;
            using System.Linq;

            class C
            {
                public void Foo(string bar[||]) =>
            #if DEBUG
                    Console.WriteLine("debug" + bar);
            #else
                    Console.WriteLine("release" + bar);
            #endif
            }
            """;
        await VerifyCS.VerifyRefactoringAsync(code, code);
    }

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/21501")]
    public async Task TestMissingInArrowExpression2()
    {
        var code = """
            using System;
            using System.Linq;

            class C
            {
                public int Foo(int[] array[||]) =>
            #if DEBUG
                    array.Where(x => x > 3)
                        .OrderBy(x => x)
                        .Count();
            #else
                    array.Where(x => x > 3)
                        .OrderBy(x => x)
                        .Count();
            #endif
            }
            """;
        await VerifyCS.VerifyRefactoringAsync(code, code);
    }

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/21501")]
    public Task TestInArrowExpression3()
        => VerifyCS.VerifyRefactoringAsync(
            """
            using System;
            using System.Linq;

            class C
            {
                public void Foo(int[] array[||]) =>
                    array.Where(x => x > 3)
                        .OrderBy(x => x)
                        .Count();
            }
            """,
            """
            using System;
            using System.Linq;

            class C
            {
                public void Foo(int[] array)
                {
                    if (array is null)
                    {
                        throw new ArgumentNullException(nameof(array));
                    }

                    array.Where(x => x > 3)
                        .OrderBy(x => x)
                        .Count();
                }
            }
            """);

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/29190")]
    public Task TestSimpleReferenceTypeWithParameterNameSelected1()
        => VerifyCS.VerifyRefactoringAsync(
            """
            using System;

            class C
            {
                public C(string [|s|])
                {
                }
            }
            """,
            """
            using System;

            class C
            {
                public C(string s)
                {
                    if (s is null)
                    {
                        throw new ArgumentNullException(nameof(s));
                    }
                }
            }
            """);

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/29333")]
    public async Task TestLambdaWithIncorrectNumberOfParameters()
    {
        var code = """
            using System;

            class C
            {
                void M(Action<int, int> a)
                {
                    M((x[||]
                }
            }
            """;
        await VerifyCS.VerifyRefactoringAsync(code,
            [
                // /0/Test0.cs(7,12): error CS0103: The name 'x' does not exist in the current context
                DiagnosticResult.CompilerError("CS0103").WithSpan(7, 12, 7, 13).WithArguments("x"),
                // /0/Test0.cs(7,13): error CS1002: ; expected
                DiagnosticResult.CompilerError("CS1002").WithSpan(7, 13, 7, 13),
                // /0/Test0.cs(7,13): error CS1026: ) expected
                DiagnosticResult.CompilerError("CS1026").WithSpan(7, 13, 7, 13),
                // /0/Test0.cs(7,13): error CS1026: ) expected
                DiagnosticResult.CompilerError("CS1026").WithSpan(7, 13, 7, 13),
            ], code);
    }

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/41824")]
    public async Task TestMissingInArgList()
    {
        var code = """
            class C
            {
                private static void M()
                {
                    M2(__arglist(1, 2, 3, 5, 6));
                }

                public static void M2([||]__arglist)
                {
                }
            }
            """;
        await VerifyCS.VerifyRefactoringAsync(code, code);
    }

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/52383")]
    public Task TestImportSystem()
        => VerifyCS.VerifyRefactoringAsync(
            """
            class C
            {
                public C([||]string s)
                {
                }
            }
            """,
            """
            using System;

            class C
            {
                public C(string s)
                {
                    if (s is null)
                    {
                        throw new ArgumentNullException(nameof(s));
                    }
                }
            }
            """);

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/52385")]
    public Task SingleLineStatement_NullCheck_BracesNone_SameLineFalse()
        => new VerifyCS.Test
        {
            TestCode = """
            using System;

            class C
            {
                public C($$object o)
                {
                }
            }
            """,
            FixedCode = """
            using System;

            class C
            {
                public C(object o)
                {
                    if (o is null)
                        throw new ArgumentNullException(nameof(o));
                }
            }
            """,
            Options =
            {
                { CSharpCodeStyleOptions.PreferThrowExpression, false },
                { CSharpCodeStyleOptions.PreferBraces, PreferBracesPreference.None },
                { CSharpCodeStyleOptions.AllowEmbeddedStatementsOnSameLine, false },
            }
        }.RunAsync();

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/52385")]
    public Task SingleLineStatement_NullCheck_BracesWhenMultiline_SameLineFalse()
        => new VerifyCS.Test
        {
            TestCode = """
            using System;

            class C
            {
                public C($$object o)
                {
                }
            }
            """,
            FixedCode = """
            using System;

            class C
            {
                public C(object o)
                {
                    if (o is null)
                        throw new ArgumentNullException(nameof(o));
                }
            }
            """,
            Options =
            {
                { CSharpCodeStyleOptions.PreferThrowExpression, false },
                { CSharpCodeStyleOptions.PreferBraces, PreferBracesPreference.WhenMultiline },
                { CSharpCodeStyleOptions.AllowEmbeddedStatementsOnSameLine, false },
            }
        }.RunAsync();

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/52385")]
    public Task SingleLineStatement_NullCheck_BracesAlways_SameLineFalse()
        => new VerifyCS.Test
        {
            TestCode = """
            using System;

            class C
            {
                public C($$object o)
                {
                }
            }
            """,
            FixedCode = """
            using System;

            class C
            {
                public C(object o)
                {
                    if (o is null)
                    {
                        throw new ArgumentNullException(nameof(o));
                    }
                }
            }
            """,
            Options =
            {
                { CSharpCodeStyleOptions.PreferThrowExpression, false },
                { CSharpCodeStyleOptions.PreferBraces, PreferBracesPreference.Always },
                { CSharpCodeStyleOptions.AllowEmbeddedStatementsOnSameLine, false },
            }
        }.RunAsync();

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/52385")]
    public Task SingleLineStatement_NullCheck_BracesNone_SameLineTrue()
        => new VerifyCS.Test
        {
            TestCode = """
            using System;

            class C
            {
                public C($$object o)
                {
                }
            }
            """,
            FixedCode = """
            using System;

            class C
            {
                public C(object o)
                {
                    if (o is null) throw new ArgumentNullException(nameof(o));
                }
            }
            """,
            Options =
            {
                { CSharpCodeStyleOptions.PreferThrowExpression, false },
                { CSharpCodeStyleOptions.PreferBraces, PreferBracesPreference.None },
                { CSharpCodeStyleOptions.AllowEmbeddedStatementsOnSameLine, true },
            }
        }.RunAsync();

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/52385")]
    public Task SingleLineStatement_NullCheck_BracesWhenMultiline_SameLineTrue()
        => new VerifyCS.Test
        {
            TestCode = """
            using System;

            class C
            {
                public C($$object o)
                {
                }
            }
            """,
            FixedCode = """
            using System;

            class C
            {
                public C(object o)
                {
                    if (o is null) throw new ArgumentNullException(nameof(o));
                }
            }
            """,
            Options =
            {
                { CSharpCodeStyleOptions.PreferThrowExpression, false },
                { CSharpCodeStyleOptions.PreferBraces, PreferBracesPreference.WhenMultiline },
                { CSharpCodeStyleOptions.AllowEmbeddedStatementsOnSameLine, true },
            }
        }.RunAsync();

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/52385")]
    public Task SingleLineStatement_NullCheck_BracesAlways_SameLineTrue()
        => new VerifyCS.Test
        {
            TestCode = """
            using System;

            class C
            {
                public C($$object o)
                {
                }
            }
            """,
            FixedCode = """
            using System;

            class C
            {
                public C(object o)
                {
                    if (o is null)
                    {
                        throw new ArgumentNullException(nameof(o));
                    }
                }
            }
            """,
            Options =
            {
                { CSharpCodeStyleOptions.PreferThrowExpression, false },
                { CSharpCodeStyleOptions.PreferBraces, PreferBracesPreference.Always },
                { CSharpCodeStyleOptions.AllowEmbeddedStatementsOnSameLine, true },
            }
        }.RunAsync();

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/52385")]
    public Task SingleLineStatement_StringIsNullOrEmpty_BracesNone_SameLineFalse()
        => new VerifyCS.Test
        {
            TestCode = """
            using System;

            class C
            {
                public C($$string s)
                {
                }
            }
            """,
            FixedCode = $$"""
            using System;

            class C
            {
                public C(string s)
                {
                    if (string.IsNullOrEmpty(s))
                        throw new ArgumentException($"{{string.Format(FeaturesResources._0_cannot_be_null_or_empty, "{nameof(s)}").Replace("""
                               "
                               """, """
                               \"
                               """)}}", nameof(s));
                }
            }
            """,
            Options =
            {
                { CSharpCodeStyleOptions.PreferThrowExpression, false },
                { CSharpCodeStyleOptions.PreferBraces, PreferBracesPreference.None },
                { CSharpCodeStyleOptions.AllowEmbeddedStatementsOnSameLine, false },
            },
            CodeActionIndex = 1,
            CodeActionEquivalenceKey = "Add_string_IsNullOrEmpty_check"
        }.RunAsync();

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/52385")]
    public Task SingleLineStatement_StringIsNullOrEmpty_BracesWhenMultiline_SameLineFalse()
        => new VerifyCS.Test
        {
            TestCode = """
            using System;

            class C
            {
                public C($$string s)
                {
                }
            }
            """,
            FixedCode = $$"""
            using System;

            class C
            {
                public C(string s)
                {
                    if (string.IsNullOrEmpty(s))
                        throw new ArgumentException($"{{string.Format(FeaturesResources._0_cannot_be_null_or_empty, "{nameof(s)}").Replace("""
                               "
                               """, """
                               \"
                               """)}}", nameof(s));
                }
            }
            """,
            Options =
            {
                { CSharpCodeStyleOptions.PreferThrowExpression, false },
                { CSharpCodeStyleOptions.PreferBraces, PreferBracesPreference.WhenMultiline},
                { CSharpCodeStyleOptions.AllowEmbeddedStatementsOnSameLine, false },
            },
            CodeActionIndex = 1,
            CodeActionEquivalenceKey = "Add_string_IsNullOrEmpty_check"
        }.RunAsync();

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/52385")]
    public Task SingleLineStatement_StringIsNullOrEmpty_BracesAlways_SameLineFalse()
        => new VerifyCS.Test
        {
            TestCode = """
            using System;

            class C
            {
                public C($$string s)
                {
                }
            }
            """,
            FixedCode = $$"""
            using System;

            class C
            {
                public C(string s)
                {
                    if (string.IsNullOrEmpty(s))
                    {
                        throw new ArgumentException($"{{string.Format(FeaturesResources._0_cannot_be_null_or_empty, "{nameof(s)}").Replace("""
                               "
                               """, """
                               \"
                               """)}}", nameof(s));
                    }
                }
            }
            """,
            Options =
            {
                { CSharpCodeStyleOptions.PreferThrowExpression, false },
                { CSharpCodeStyleOptions.PreferBraces, PreferBracesPreference.Always },
                { CSharpCodeStyleOptions.AllowEmbeddedStatementsOnSameLine, false },
            },
            CodeActionIndex = 1,
            CodeActionEquivalenceKey = "Add_string_IsNullOrEmpty_check"
        }.RunAsync();

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/52385")]
    public Task SingleLineStatement_StringIsNullOrEmpty_BracesNone_SameLineTrue()
        => new VerifyCS.Test
        {
            TestCode = """
            using System;

            class C
            {
                public C($$string s)
                {
                }
            }
            """,
            FixedCode = $$"""
            using System;

            class C
            {
                public C(string s)
                {
                    if (string.IsNullOrEmpty(s)) throw new ArgumentException($"{{string.Format(FeaturesResources._0_cannot_be_null_or_empty, "{nameof(s)}").Replace("""
                               "
                               """, """
                               \"
                               """)}}", nameof(s));
                }
            }
            """,
            Options =
            {
                { CSharpCodeStyleOptions.PreferThrowExpression, false },
                { CSharpCodeStyleOptions.PreferBraces, PreferBracesPreference.None },
                { CSharpCodeStyleOptions.AllowEmbeddedStatementsOnSameLine, true },
            },
            CodeActionIndex = 1,
            CodeActionEquivalenceKey = "Add_string_IsNullOrEmpty_check"
        }.RunAsync();

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/52385")]
    public Task SingleLineStatement_StringIsNullOrEmpty_BracesWhenMultiline_SameLineTrue()
        => new VerifyCS.Test
        {
            TestCode = """
            using System;

            class C
            {
                public C($$string s)
                {
                }
            }
            """,
            FixedCode = $$"""
            using System;

            class C
            {
                public C(string s)
                {
                    if (string.IsNullOrEmpty(s)) throw new ArgumentException($"{{string.Format(FeaturesResources._0_cannot_be_null_or_empty, "{nameof(s)}").Replace("""
                               "
                               """, """
                               \"
                               """)}}", nameof(s));
                }
            }
            """,
            Options =
            {
                { CSharpCodeStyleOptions.PreferThrowExpression, false },
                { CSharpCodeStyleOptions.PreferBraces, PreferBracesPreference.WhenMultiline },
                { CSharpCodeStyleOptions.AllowEmbeddedStatementsOnSameLine, true },
            },
            CodeActionIndex = 1,
            CodeActionEquivalenceKey = "Add_string_IsNullOrEmpty_check"
        }.RunAsync();

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/52385")]
    public Task SingleLineStatement_StringIsNullOrEmpty_BracesAlways_SameLineTrue()
        => new VerifyCS.Test
        {
            TestCode = """
            using System;

            class C
            {
                public C($$string s)
                {
                }
            }
            """,
            FixedCode = $$"""
            using System;

            class C
            {
                public C(string s)
                {
                    if (string.IsNullOrEmpty(s))
                    {
                        throw new ArgumentException($"{{string.Format(FeaturesResources._0_cannot_be_null_or_empty, "{nameof(s)}").Replace("""
                               "
                               """, """
                               \"
                               """)}}", nameof(s));
                    }
                }
            }
            """,
            Options =
            {
                { CSharpCodeStyleOptions.PreferThrowExpression, false },
                { CSharpCodeStyleOptions.PreferBraces, PreferBracesPreference.Always },
                { CSharpCodeStyleOptions.AllowEmbeddedStatementsOnSameLine, true },
            },
            CodeActionIndex = 1,
            CodeActionEquivalenceKey = "Add_string_IsNullOrEmpty_check"
        }.RunAsync();

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/52385")]
    public Task SingleLineStatement_NullCheck_AllParameters()
        => new VerifyCS.Test
        {
            TestCode = """
            using System;

            class C
            {
                public C([||]object a, object b, object c)
                {
                }
            }
            """,
            FixedCode = """
            using System;

            class C
            {
                public C(object a, object b, object c)
                {
                    if (a is null) throw new ArgumentNullException(nameof(a));
                    if (b is null) throw new ArgumentNullException(nameof(b));
                    if (c is null) throw new ArgumentNullException(nameof(c));
                }
            }
            """,
            Options =
            {
                { CSharpCodeStyleOptions.PreferThrowExpression, false },
                { CSharpCodeStyleOptions.PreferBraces, PreferBracesPreference.None },
                { CSharpCodeStyleOptions.AllowEmbeddedStatementsOnSameLine, true },
            },
            CodeActionIndex = 1,
            CodeActionEquivalenceKey = nameof(FeaturesResources.Add_null_checks_for_all_parameters)
        }.RunAsync();

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/58811")]
    public async Task TestMissingParameter1()
    {
        var source = """
            using System;

            class C
            {
                public C(string s,[||]{|CS1031:{|CS1001:)|}|}
                {
                }
            }
            """;
        await VerifyCS.VerifyRefactoringAsync(source, source);
    }

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/58811")]
    public async Task TestMissingParameter2()
    {
        var source = """
            using System;

            class C
            {
                public C(string s,[||] {|CS1031:{|CS1001:)|}|}
                {
                }
            }
            """;
        await VerifyCS.VerifyRefactoringAsync(source, source);
    }

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/58811")]
    public async Task TestMissingParameter3()
    {
        var source = """
            using System;

            class C
            {
                public C(string s, [||]{|CS1031:{|CS1001:)|}|}
                {
                }
            }
            """;
        await VerifyCS.VerifyRefactoringAsync(source, source);
    }

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/58811")]
    public async Task TestMissingParameter4()
    {
        var source = """
            using System;

            class C
            {
                public C(string s, [||] {|CS1031:{|CS1001:)|}|}
                {
                }
            }
            """;
        await VerifyCS.VerifyRefactoringAsync(source, source);
    }

    [Theory]
    [WorkItem("https://github.com/dotnet/roslyn/issues/58779")]
    [InlineData(LanguageVersion.CSharp10)]
    [InlineData(LanguageVersion.CSharp11)]
    public Task TestNotInRecord(LanguageVersion version)
        => new VerifyCS.Test
        {
            LanguageVersion = version,
            TestCode = """
            record C([||]string s) { public string s; }
            """,
        }.RunAsync();

    [Fact]
    public Task TestNotInClass()
        => new VerifyCS.Test
        {
            LanguageVersion = LanguageVersion.CSharp12,
            TestCode = """
            class C([||]string s) { public string s; }
            """,
        }.RunAsync();

    [Fact]
    public Task TestNotInStruct()
        => new VerifyCS.Test
        {
            LanguageVersion = LanguageVersion.CSharp12,
            TestCode = """
            struct C([||]string s) { public string s; }
            """,
        }.RunAsync();

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/38093")]
    public Task TestReadBeforeAssignment()
        => VerifyCS.VerifyRefactoringAsync("""
            using System;
            using System.IO;

            class Program
            {
                public Program([||]Stream output)
                {
                    if (!output.CanWrite) throw new ArgumentException();
                    OutStream = output;
                }

                public Stream OutStream { get; }
            }
            """, """
            using System;
            using System.IO;

            class Program
            {
                public Program([||]Stream output)
                {
                    if (output is null)
                    {
                        throw new ArgumentNullException(nameof(output));
                    }

                    if (!output.CanWrite) throw new ArgumentException();
                    OutStream = output;
                }

                public Stream OutStream { get; }
            }
            """);

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/41140")]
    public Task TestAfterComma1()
        => VerifyCS.VerifyRefactoringAsync("""
            using System;

            class C
            {
                // should generate for 'b'
                void M(string a,$$ string b, string c)
                {

                }
            }
            """, """
            using System;

            class C
            {
                // should generate for 'b'
                void M(string a, string b, string c)
                {
                    if (b is null)
                    {
                        throw new ArgumentNullException(nameof(b));
                    }
                }
            }
            """);

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/41140")]
    public Task TestAfterComma2()
        => VerifyCS.VerifyRefactoringAsync("""
            using System;

            class C
            {
                // should generate for 'a'
                void M(string a,$$
                    string b, string c)
                {

                }
            }
            """, """
            using System;

            class C
            {
                // should generate for 'a'
                void M(string a,
                    string b, string c)
                {
                    if (a is null)
                    {
                        throw new ArgumentNullException(nameof(a));
                    }
                }
            }
            """);

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/66327")]
    public Task TestSimpleEnumIsDefinedCheck_ModernEnumIsDefinedOverload()
        => new VerifyCS.Test()
        {
            TestCode = """
                using System;

                class C
                {
                    void M(DayOfWeek [|dayOfWeek|])
                    {
                    }
                }
                """,
            FixedCode = """
                using System;
            
                class C
                {
                    void M(DayOfWeek dayOfWeek)
                    {
                        if (!Enum.IsDefined(dayOfWeek))
                        {
                            throw new System.ComponentModel.InvalidEnumArgumentException(nameof(dayOfWeek), (int)dayOfWeek, typeof(DayOfWeek));
                        }
                    }
                }
                """,
            ReferenceAssemblies = ReferenceAssemblies.Net.Net80
        }.RunAsync();

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/66327")]
    public Task TestSimpleEnumIsDefinedCheck_OldEnumIsDefinedOverload()
        => new VerifyCS.Test()
        {
            TestCode = """
                using System;
                using System.ComponentModel;

                class C
                {
                    void M(DayOfWeek [|dayOfWeek|])
                    {
                    }
                }
                """,
            FixedCode = """
                using System;
                using System.ComponentModel;
            
                class C
                {
                    void M(DayOfWeek dayOfWeek)
                    {
                        if (!Enum.IsDefined(typeof(DayOfWeek), dayOfWeek))
                        {
                            throw new InvalidEnumArgumentException(nameof(dayOfWeek), (int)dayOfWeek, typeof(DayOfWeek));
                        }
                    }
                }
                """,
            ReferenceAssemblies = ReferenceAssemblies.NetStandard.NetStandard20
        }.RunAsync();

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/66327")]
    public Task TestNoEnumIsDefinedCheckForOutEnumParameter()
        => VerifyCS.VerifyRefactoringAsync("""
            using System;
            using System.ComponentModel;
            
            class C
            {
                void M(out DayOfWeek [|result|])
                {
                    result = default;
                }
            }
            """);

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/66327")]
    public Task TestNoEnumIsDefinedCheckForFlagsEnumParameter()
        => VerifyCS.VerifyRefactoringAsync("""
            using System;
            using System.ComponentModel;
            
            class C
            {
                void M(MyFlags [|myFlags|])
                {
                }
            }

            [Flags]
            enum MyFlags
            {
                A = 1,
                B = 2
            }
            """);

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/66327")]
    public async Task TestNoEnumIsDefinedCheckIfAlreadyExist_ModernEnumIsDefinedOverload()
    {
        var code = """
            using System;
            using System.ComponentModel;
            
            class C
            {
                void M(DayOfWeek [|dayOfWeek|])
                {
                    if (!Enum.IsDefined(dayOfWeek))
                    {
                        throw new InvalidEnumArgumentException(nameof(dayOfWeek), (int)dayOfWeek, typeof(DayOfWeek));
                    }
                }
            }
            """;

        await new VerifyCS.Test()
        {
            TestCode = code,
            FixedCode = code,
            ReferenceAssemblies = ReferenceAssemblies.Net.Net80
        }.RunAsync();
    }

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/66327")]
    public async Task TestNoEnumIsDefinedCheckIfAlreadyExist_OldEnumIsDefinedOverload()
    {
        var code = """
            using System;
            using System.ComponentModel;
            
            class C
            {
                void M(DayOfWeek [|dayOfWeek|])
                {
                    if (!Enum.IsDefined(typeof(DayOfWeek), dayOfWeek))
                    {
                        throw new InvalidEnumArgumentException(nameof(dayOfWeek), (int)dayOfWeek, typeof(DayOfWeek));
                    }
                }
            }
            """;

        await new VerifyCS.Test()
        {
            TestCode = code,
            FixedCode = code,
            ReferenceAssemblies = ReferenceAssemblies.Net.Net80
        }.RunAsync();
    }

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/66327")]
    public Task TestEnumIsDefinedCheckAfterAnotherEnumIsDefinedCheck()
        => new VerifyCS.Test()
        {
            TestCode = """
                using System;
                using System.ComponentModel;

                class C
                {
                    void M(DayOfWeek dayOfWeek1, DayOfWeek [|dayOfWeek2|])
                    {
                        if (!Enum.IsDefined(dayOfWeek1))
                        {
                            throw new InvalidEnumArgumentException(nameof(dayOfWeek1), (int)dayOfWeek1, typeof(DayOfWeek));
                        }
                    }
                }
                """,
            FixedCode = """
                using System;
                using System.ComponentModel;
                
                class C
                {
                    void M(DayOfWeek dayOfWeek1, DayOfWeek dayOfWeek2)
                    {
                        if (!Enum.IsDefined(dayOfWeek1))
                        {
                            throw new InvalidEnumArgumentException(nameof(dayOfWeek1), (int)dayOfWeek1, typeof(DayOfWeek));
                        }

                        if (!Enum.IsDefined(dayOfWeek2))
                        {
                            throw new InvalidEnumArgumentException(nameof(dayOfWeek2), (int)dayOfWeek2, typeof(DayOfWeek));
                        }
                    }
                }
                """,
            ReferenceAssemblies = ReferenceAssemblies.Net.Net80
        }.RunAsync();

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/66327")]
    public Task TestEnumIsDefinedCheckBeforeAnotherEnumIsDefinedCheck()
        => new VerifyCS.Test()
        {
            TestCode = """
                using System;
                using System.ComponentModel;

                class C
                {
                    void M(DayOfWeek [|dayOfWeek1|], DayOfWeek dayOfWeek2)
                    {
                        if (!Enum.IsDefined(dayOfWeek2))
                        {
                            throw new InvalidEnumArgumentException(nameof(dayOfWeek2), (int)dayOfWeek2, typeof(DayOfWeek));
                        }
                    }
                }
                """,
            FixedCode = """
                using System;
                using System.ComponentModel;
                
                class C
                {
                    void M(DayOfWeek dayOfWeek1, DayOfWeek dayOfWeek2)
                    {
                        if (!Enum.IsDefined(dayOfWeek1))
                        {
                            throw new InvalidEnumArgumentException(nameof(dayOfWeek1), (int)dayOfWeek1, typeof(DayOfWeek));
                        }

                        if (!Enum.IsDefined(dayOfWeek2))
                        {
                            throw new InvalidEnumArgumentException(nameof(dayOfWeek2), (int)dayOfWeek2, typeof(DayOfWeek));
                        }
                    }
                }
                """,
            ReferenceAssemblies = ReferenceAssemblies.Net.Net80
        }.RunAsync();

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/66327")]
    public Task TestEnumIsDefinedCheckAfterNullCheck()
        => new VerifyCS.Test()
        {
            TestCode = """
                using System;
                using System.ComponentModel;

                class C
                {
                    void M(string s, DayOfWeek [|dayOfWeek|])
                    {
                        ArgumentNullException.ThrowIfNull(s);
                    }
                }
                """,
            FixedCode = """
                using System;
                using System.ComponentModel;
                
                class C
                {
                    void M(string s, DayOfWeek dayOfWeek)
                    {
                        ArgumentNullException.ThrowIfNull(s);
                        if (!Enum.IsDefined(dayOfWeek))
                        {
                            throw new InvalidEnumArgumentException(nameof(dayOfWeek), (int)dayOfWeek, typeof(DayOfWeek));
                        }
                    }
                }
                """,
            ReferenceAssemblies = ReferenceAssemblies.Net.Net80
        }.RunAsync();

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/66327")]
    public Task TestEnumIsDefinedCheckBeforeNullCheck()
        => new VerifyCS.Test()
        {
            TestCode = """
                using System;
                using System.ComponentModel;

                class C
                {
                    void M(DayOfWeek [|dayOfWeek|], object o)
                    {
                        if (o is null)
                        {
                            throw new ArgumentNullException(nameof(o));
                        }
                    }
                }
                """,
            FixedCode = """
                using System;
                using System.ComponentModel;
                
                class C
                {
                    void M(DayOfWeek dayOfWeek, object o)
                    {
                        if (!Enum.IsDefined(dayOfWeek))
                        {
                            throw new InvalidEnumArgumentException(nameof(dayOfWeek), (int)dayOfWeek, typeof(DayOfWeek));
                        }

                        if (o is null)
                        {
                            throw new ArgumentNullException(nameof(o));
                        }
                    }
                }
                """,
            ReferenceAssemblies = ReferenceAssemblies.Net.Net80
        }.RunAsync();

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/66327")]
    public Task TestEnumIsDefinedCheckInBetweenChecks()
        => new VerifyCS.Test()
        {
            TestCode = """
                using System;
                using System.ComponentModel;

                class C
                {
                    void M(object o, DayOfWeek [|dayOfWeek|], string s)
                    {
                        if (o is null)
                        {
                            throw new ArgumentNullException(nameof(o));
                        }

                        ArgumentException.ThrowIfNullOrEmpty(s);
                    }
                }
                """,
            FixedCode = """
                using System;
                using System.ComponentModel;
                
                class C
                {
                    void M(object o, DayOfWeek dayOfWeek, string s)
                    {
                        if (o is null)
                        {
                            throw new ArgumentNullException(nameof(o));
                        }

                        if (!Enum.IsDefined(dayOfWeek))
                        {
                            throw new InvalidEnumArgumentException(nameof(dayOfWeek), (int)dayOfWeek, typeof(DayOfWeek));
                        }
                
                        ArgumentException.ThrowIfNullOrEmpty(s);
                    }
                }
                """,
            ReferenceAssemblies = ReferenceAssemblies.Net.Net80
        }.RunAsync();

    [Theory, WorkItem("https://github.com/dotnet/roslyn/issues/37653")]
    [InlineData("sbyte")]
    [InlineData("short")]
    [InlineData("int")]
    [InlineData("long")]
    public async Task TestSimpleNumericChecks_ModernOverloads(string validNumericType)
    {
        var code = $$"""
            using System;

            class C
            {
                void M({{validNumericType}} [|num|])
                {
                }
            }
            """;

        await new VerifyCS.Test()
        {
            TestCode = code,
            FixedCode = $$"""
                using System;
                
                class C
                {
                    void M({{validNumericType}} [|num|])
                    {
                        ArgumentOutOfRangeException.ThrowIfNegative(num);
                    }
                }
                """,
            ReferenceAssemblies = ReferenceAssemblies.Net.Net80,
            CodeActionIndex = 0,
        }.RunAsync();

        await new VerifyCS.Test()
        {
            TestCode = code,
            FixedCode = $$"""
                using System;
                
                class C
                {
                    void M({{validNumericType}} [|num|])
                    {
                        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(num);
                    }
                }
                """,
            ReferenceAssemblies = ReferenceAssemblies.Net.Net80,
            CodeActionIndex = 1,
        }.RunAsync();
    }

    [Theory, WorkItem("https://github.com/dotnet/roslyn/issues/37653")]
    [InlineData("sbyte")]
    [InlineData("short")]
    [InlineData("int")]
    [InlineData("long")]
    public async Task TestSimpleNumericChecks_OldStyleCheckStatement(string validNumericType)
    {
        var code = $$"""
            using System;

            class C
            {
                void M({{validNumericType}} [|num|])
                {
                }
            }
            """;

        await new VerifyCS.Test()
        {
            TestCode = code,
            FixedCode = $$"""
                using System;
                
                class C
                {
                    void M({{validNumericType}} [|num|])
                    {
                        if (num < 0)
                        {
                            throw new ArgumentOutOfRangeException(nameof(num), num, $"{{string.Format(FeaturesResources._0_cannot_be_negative, "{nameof(num)}").Replace("\"", "\\\"")}}");
                        }
                    }
                }
                """,
            ReferenceAssemblies = ReferenceAssemblies.NetStandard.NetStandard20,
            CodeActionIndex = 0,
        }.RunAsync();

        await new VerifyCS.Test()
        {
            TestCode = code,
            FixedCode = $$"""
                using System;
                
                class C
                {
                    void M({{validNumericType}} [|num|])
                    {
                        if (num <= 0)
                        {
                            throw new ArgumentOutOfRangeException(nameof(num), num, $"{{string.Format(FeaturesResources._0_cannot_be_negative_or_zero, "{nameof(num)}").Replace("\"", "\\\"")}}");
                        }
                    }
                }
                """,
            ReferenceAssemblies = ReferenceAssemblies.NetStandard.NetStandard20,
            CodeActionIndex = 1,
        }.RunAsync();
    }

    [Theory, WorkItem("https://github.com/dotnet/roslyn/issues/37653")]
    [InlineData("byte")]
    [InlineData("ushort")]
    [InlineData("uint")]
    [InlineData("ulong")]
    [InlineData("float")]
    [InlineData("double")]
    public Task TestNoNumericChecksForUnsignedAndFloatingPointNumericTypes(string invalidNumericType)
        => VerifyCS.VerifyRefactoringAsync($$"""
            using System;
            
            class C
            {
                void M({{invalidNumericType}} [|num|])
                {
                }
            }
            """);

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/37653")]
    public Task TestNoNumericChecksForOutParameter()
        => VerifyCS.VerifyRefactoringAsync("""
            using System;
            
            class C
            {
                void M(out int [|num|])
                {
                    num = 0;
                }
            }
            """);

    [Theory, WorkItem("https://github.com/dotnet/roslyn/issues/37653")]
    [InlineData("ThrowIfNegative(num)")]
    [InlineData("ThrowIfNegativeOrZero(num)")]
    [InlineData("ThrowIfEqual(num, 5)")]
    [InlineData("ThrowIfGreaterThan(num, 6)")]
    [InlineData("ThrowIfGreaterThanOrEqual(num, 1)")]
    [InlineData("ThrowIfLessThan(num, 2)")]
    [InlineData("ThrowIfLessThanOrEqual(num, 3)")]
    [InlineData("ThrowIfEqual(num, 15)")]
    [InlineData("ThrowIfNotEqual(num, 10)")]
    [InlineData("ThrowIfZero(num)")]
    public Task TestNoNumericChecksIfAlreadyExist_ModernOverloads(string methodInvocation)
        => new VerifyCS.Test()
        {
            TestCode = $$"""
                using System;
                
                class C
                {
                    void M(int [|num|])
                    {
                        ArgumentOutOfRangeException.{{methodInvocation}};
                    }
                }
                """,
            ReferenceAssemblies = ReferenceAssemblies.Net.Net80
        }.RunAsync();

    [Theory, WorkItem("https://github.com/dotnet/roslyn/issues/37653")]
    [InlineData("num < 0")]
    [InlineData("num <= 0")]
    [InlineData("num > 11")]
    [InlineData("num >= 12")]
    [InlineData("0 > num")]
    [InlineData("0 >= num")]
    [InlineData("14 < num")]
    [InlineData("22 <= num")]
    [InlineData("num is < 0")]
    [InlineData("num is <= 0")]
    [InlineData("num < 1")]
    [InlineData("num <= 39")]
    [InlineData("25 > num")]
    [InlineData("29 >= num")]
    [InlineData("num is < 8")]
    [InlineData("num is <= 18")]
    [InlineData("num is > 5")]
    [InlineData("num is >= 3")]
    public Task TestNoNumericChecksIfAlreadyExist_OldStyleCheckStatements(string numericCheck)
        => new VerifyCS.Test()
        {
            TestCode = $$"""
                using System;
                
                class C
                {
                    void M(int [|num|])
                    {
                        if ({{numericCheck}})
                        {
                            throw new ArgumentOutOfRangeException(nameof(num));
                        }
                    }
                }
                """,
            LanguageVersion = LanguageVersion.Latest,
            ReferenceAssemblies = ReferenceAssemblies.Net.Net80
        }.RunAsync();

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/37653")]
    public Task TestNumericChecksAfterAnotherNumericCheck()
        => new VerifyCS.Test()
        {
            TestCode = """
                using System;

                class C
                {
                    void M(sbyte a, short [|b|])
                    {
                        ArgumentOutOfRangeException.ThrowIfNegative(a);
                    }
                }
                """,
            FixedCode = """
                using System;
                
                class C
                {
                    void M(sbyte a, short [|b|])
                    {
                        ArgumentOutOfRangeException.ThrowIfNegative(a);
                        ArgumentOutOfRangeException.ThrowIfNegative(b);
                    }
                }
                """,
            ReferenceAssemblies = ReferenceAssemblies.Net.Net80,
        }.RunAsync();

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/37653")]
    public Task TestNumericChecksBeforeAnotherNumericCheck()
        => new VerifyCS.Test()
        {
            TestCode = """
                using System;

                class C
                {
                    void M(long [|a|], int b)
                    {
                        if (b < 0)
                        {
                            throw new ArgumentOutOfRangeException(nameof(b));
                        }
                    }
                }
                """,
            FixedCode = """
                using System;
                
                class C
                {
                    void M(long a, int b)
                    {
                        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(a);
                        if (b < 0)
                        {
                            throw new ArgumentOutOfRangeException(nameof(b));
                        }
                    }
                }
                """,
            ReferenceAssemblies = ReferenceAssemblies.Net.Net80,
            CodeActionIndex = 1
        }.RunAsync();

    [Fact, WorkItem("https://github.com/dotnet/roslyn/issues/37653")]
    public Task TestNumericChecksInBetweenDifferentChecks()
        => new VerifyCS.Test()
        {
            TestCode = """
                using System;

                class C
                {
                    void M(DayOfWeek day, int [|i|], string s)
                    {
                        if (!Enum.IsDefined(typeof(DayOfWeek), day))
                        {
                            throw new System.ComponentModel.InvalidEnumArgumentException(nameof(day), (int)day, typeof(DayOfWeek));
                        }

                        if (string.IsNullOrEmpty(s))
                        {
                            throw new ArgumentNullException(nameof(s));
                        }
                    }
                }
                """,
            FixedCode = $$"""
                using System;
                
                class C
                {
                    void M(DayOfWeek day, int i, string s)
                    {
                        if (!Enum.IsDefined(typeof(DayOfWeek), day))
                        {
                            throw new System.ComponentModel.InvalidEnumArgumentException(nameof(day), (int)day, typeof(DayOfWeek));
                        }

                        if (i < 0)
                        {
                            throw new ArgumentOutOfRangeException(nameof(i), i, $"{{string.Format(FeaturesResources._0_cannot_be_negative, "{nameof(i)}").Replace("\"", "\\\"")}}");
                        }

                        if (string.IsNullOrEmpty(s))
                        {
                            throw new ArgumentNullException(nameof(s));
                        }
                    }
                }
                """,
            ReferenceAssemblies = ReferenceAssemblies.NetStandard.NetStandard20
        }.RunAsync();
}
