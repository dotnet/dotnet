// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost.CodeActions;

public class AddUsingTests(ITestOutputHelper testOutputHelper) : CohostCodeActionsEndpointTestBase(testOutputHelper)
{
    [Fact(Skip = "Need to map uri back to source generated document")]
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

        await VerifyCodeActionAsync(input, expected, "System.Text.StringBuilder");
    }

    [Fact(Skip = "Need to map uri back to source generated document")]
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
            codeActionName: "Fully qualify 'StringBuilder'",
            childActionIndex: 0);
    }

    [Fact(Skip = "Need to map uri back to source generated document")]
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

    [Fact(Skip = "Need to map uri back to source generated document")]
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

    [Fact(Skip = "Need to map uri back to source generated document")]
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
