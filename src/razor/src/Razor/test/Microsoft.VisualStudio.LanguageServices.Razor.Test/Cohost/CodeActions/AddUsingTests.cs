// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost.CodeActions;

public class AddUsingTests(ITestOutputHelper testOutputHelper) : CohostCodeActionsEndpointTestBase(testOutputHelper)
{
    [Fact]
    public async Task FullyQualify()
    {
        var input = """
            @code
            {
                private [||]StringBuilder _x = new StringBuilder();
            }
            """;

        var expected = """
            @code
            {
                private System.Text.StringBuilder _x = new StringBuilder();
            }
            """;

        await VerifyCodeActionAsync(input, expected, LanguageServerConstants.CodeActions.FullyQualify);
    }

    [Fact]
    public async Task FullyQualify_Multiple()
    {
        await VerifyCodeActionAsync(
            input: """
                @code
                {
                    private [||]StringBuilder _x = new StringBuilder();
                }
                """,
            expected: """
                @code
                {
                    private System.Text.StringBuilder _x = new StringBuilder();
                }
                """,
            additionalFiles: [
                (FilePath("StringBuilder.cs"), """
                    namespace Not.Built.In;

                    public class StringBuilder
                    {
                    }
                    """)],
            codeActionName: LanguageServerConstants.CodeActions.FullyQualify,
            childActionIndex: 0);
    }

    [Fact]
    public async Task AddUsing()
    {
        var input = """
            @code
            {
                private [||]StringBuilder _x = new StringBuilder();
            }
            """;

        var expected = """
            @using System.Text
            @code
            {
                private StringBuilder _x = new StringBuilder();
            }
            """;

        await VerifyCodeActionAsync(input, expected, RazorPredefinedCodeFixProviderNames.AddImport);
    }

    [Fact]
    public async Task AddUsing_Typo()
    {
        var input = """
            @code
            {
                private [||]Stringbuilder _x = new Stringbuilder();
            }
            """;

        var expected = """
            @using System.Text
            @code
            {
                private StringBuilder _x = new Stringbuilder();
            }
            """;

        await VerifyCodeActionAsync(input, expected, RazorPredefinedCodeFixProviderNames.AddImport);
    }

    [Fact]
    public async Task AddUsing_WithExisting()
    {
        var input = """
            @using System
            @using System.Collections.Generic

            @code
            {
                private [||]StringBuilder _x = new StringBuilder();
            }
            """;

        var expected = """
            @using System
            @using System.Collections.Generic
            @using System.Text

            @code
            {
                private StringBuilder _x = new StringBuilder();
            }
            """;

        await VerifyCodeActionAsync(input, expected, RazorPredefinedCodeFixProviderNames.AddImport);
    }
}
