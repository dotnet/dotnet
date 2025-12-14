// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
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
        var pass = GetPass();

        // Act
        var result = await pass.IsValidAsync(context, edits, DisposalToken);

        // Assert
        Assert.True(result);
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
        var result = await pass.IsValidAsync(context, [badEdit], DisposalToken);

        // Assert
        Assert.False(result);
    }

    private FormattingDiagnosticValidationPass GetPass()
    {
        var pass = new FormattingDiagnosticValidationPass(LoggerFactory)
        {
            DebugAssertsEnabled = false
        };

        return pass;
    }

    private static FormattingContext CreateFormattingContext(
        TestCode input,
        int tabSize = 4,
        bool insertSpaces = true,
        RazorFileKind? fileKind = null)
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
            options,
            logger: null);
        return context;
    }

    private static (RazorCodeDocument, IDocumentSnapshot) CreateCodeDocumentAndSnapshot(
        SourceText text,
        string path,
        TagHelperCollection? tagHelpers = null,
        RazorFileKind? fileKind = null)
    {
        var fileKindValue = fileKind ?? RazorFileKind.Component;
        tagHelpers ??= [];

        var sourceDocument = RazorSourceDocument.Create(text, RazorSourceDocumentProperties.Create(path, path));
        var projectEngine = RazorProjectEngine.Create(builder =>
        {
            builder.SetRootNamespace("Test");

            builder.ConfigureParserOptions(builder =>
            {
                builder.UseRoslynTokenizer = true;
            });
        });
        var codeDocument = projectEngine.Process(sourceDocument, fileKindValue, importSources: default, tagHelpers);

        var documentSnapshot = FormattingTestBase.CreateDocumentSnapshot(
            path, fileKindValue, codeDocument, projectEngine, imports: [], importDocuments: [], tagHelpers, inGlobalNamespace: false);

        return (codeDocument, documentSnapshot);
    }
}
