// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

public class FormattingDiagnosticValidationPassTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    [Fact]
    public async Task ExecuteAsync_NonDestructiveEdit_Allowed()
    {
        // Arrange
        TestCode source = """
            @code {
            [||]public class Foo { }
            }
            """;
        var context = CreateFormattingContext(source);
        var edits = ImmutableArray.Create(new TextChange(source.Span, "    "));
        var input = edits;
        var pass = GetPass();

        // Act
        var result = await pass.ExecuteAsync(context, input, DisposalToken);

        // Assert
        Assert.Equal(input, result);
    }

    [Fact]
    public async Task ExecuteAsync_DestructiveEdit_Rejected()
    {
        // Arrange
        // Arrange
        TestCode source = """
            [||]@code {
            public class Foo { }
            }
            """;
        var context = CreateFormattingContext(source);
        var badEdit = new TextChange(source.Span, "@ "); // Creates a diagnostic
        var pass = GetPass();

        // Act
        var result = await pass.ExecuteAsync(context, [badEdit], DisposalToken);

        // Assert
        Assert.Empty(result);
    }

    private FormattingDiagnosticValidationPass GetPass()
    {
        var pass = new FormattingDiagnosticValidationPass(LoggerFactory)
        {
            DebugAssertsEnabled = false
        };

        return pass;
    }

    private static FormattingContext CreateFormattingContext(TestCode input, int tabSize = 4, bool insertSpaces = true, string? fileKind = null)
    {
        var source = SourceText.From(input.Text);
        var path = "file:///path/to/document.razor";
        var uri = new Uri(path);
        var (codeDocument, documentSnapshot) = CreateCodeDocumentAndSnapshot(source, uri.AbsolutePath, fileKind: fileKind);
        var options = new RazorFormattingOptions()
        {
            TabSize = tabSize,
            InsertSpaces = insertSpaces,
        };

        var context = FormattingContext.Create(
            documentSnapshot,
            codeDocument,
            options);
        return context;
    }

    private static (RazorCodeDocument, IDocumentSnapshot) CreateCodeDocumentAndSnapshot(SourceText text, string path, ImmutableArray<TagHelperDescriptor> tagHelpers = default, string? fileKind = null)
    {
        fileKind ??= FileKinds.Component;
        tagHelpers = tagHelpers.NullToEmpty();
        var sourceDocument = RazorSourceDocument.Create(text, RazorSourceDocumentProperties.Create(path, path));
        var projectEngine = RazorProjectEngine.Create(builder =>
        {
            builder.SetRootNamespace("Test");

            builder.ConfigureParserOptions(builder =>
            {
                builder.UseRoslynTokenizer = true;
            });
        });
        var codeDocument = projectEngine.ProcessDesignTime(sourceDocument, fileKind, importSources: default, tagHelpers);

        var documentSnapshot = FormattingTestBase.CreateDocumentSnapshot(
            path, fileKind, codeDocument, projectEngine, imports: [], importDocuments: [], tagHelpers, inGlobalNamespace: false);

        return (codeDocument, documentSnapshot);
    }
}
