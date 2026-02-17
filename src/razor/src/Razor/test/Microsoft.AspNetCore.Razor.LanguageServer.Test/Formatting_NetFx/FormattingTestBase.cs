// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Settings;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Roslyn.Test.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

public abstract class FormattingTestBase : DocumentFormattingTestBase
{
    private readonly FormattingTestContext _context;
    private readonly HtmlFormattingService _htmlFormattingService;

    private protected FormattingTestBase(FormattingTestContext context, HtmlFormattingService htmlFormattingService, ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _context = context;
        _htmlFormattingService = htmlFormattingService;
    }

    private protected async Task RunOnTypeFormattingTestAsync(
        string input,
        string expected,
        char triggerCharacter,
        int tabSize = 4,
        bool insertSpaces = true,
        RazorFileKind? fileKind = null,
        int? expectedChangedLines = null,
        RazorLSPOptions? razorLSPOptions = null,
        bool inGlobalNamespace = false)
    {
        (input, _, expected) = ProcessFormattingContext(input, "", expected);

        // Arrange
        var fileKindValue = fileKind ?? RazorFileKind.Component;

        TestFileMarkupParser.GetPosition(input, out input, out var positionAfterTrigger);

        var tagHelpers = await GetStandardTagHelpersAsync(DisposalToken);

        var razorSourceText = SourceText.From(input);
        var path = "file:///path/to/Document.razor";
        var uri = new Uri(path);
        var (codeDocument, documentSnapshot) = DocumentFormattingTestBase.CreateCodeDocumentAndSnapshot(razorSourceText, uri.AbsolutePath, tagHelpers, fileKind: fileKindValue, inGlobalNamespace: inGlobalNamespace);

        var languageServerFeatureOptions = new TestLanguageServerFeatureOptions();

        var filePathService = new LSPFilePathService(languageServerFeatureOptions);
        var mappingService = new LspDocumentMappingService(
            filePathService, new TestDocumentContextFactory(), LoggerFactory);
        var languageKind = codeDocument.GetLanguageKind(positionAfterTrigger, rightAssociative: false);

        var formattingService = await TestRazorFormattingService.CreateWithFullSupportAsync(LoggerFactory, TestOutputHelper, codeDocument, razorLSPOptions, languageServerFeatureOptions, debugAssertsEnabled: true);
        var options = new FormattingOptions()
        {
            TabSize = tabSize,
            InsertSpaces = insertSpaces,
        };
        var razorOptions = RazorFormattingOptions.From(options, codeBlockBraceOnNextLine: razorLSPOptions?.CodeBlockBraceOnNextLine ?? false, razorLSPOptions?.AttributeIndentStyle ?? AttributeIndentStyle.AlignWithFirst);

        var documentContext = new DocumentContext(uri, documentSnapshot, projectContext: null);

        // Act
        ImmutableArray<TextChange> changes;
        if (languageKind == RazorLanguageKind.CSharp)
        {
            changes = await formattingService.GetCSharpOnTypeFormattingChangesAsync(documentContext, razorOptions, hostDocumentIndex: positionAfterTrigger, triggerCharacter: triggerCharacter, DisposalToken);
        }
        else
        {
            var client = new FormattingLanguageServerClient(_htmlFormattingService, LoggerFactory);
            client.AddCodeDocument(codeDocument);

            var htmlFormatter = new HtmlFormatter(client);
            var htmlChanges = await htmlFormatter.GetDocumentFormattingEditsAsync(documentSnapshot, uri, options, DisposalToken);
            changes = await formattingService.GetHtmlOnTypeFormattingChangesAsync(documentContext, htmlChanges.AssumeNotNull(), razorOptions, hostDocumentIndex: positionAfterTrigger, triggerCharacter: triggerCharacter, DisposalToken);
        }

        // Assert
        var edited = razorSourceText.WithChanges(changes);
        var actual = edited.ToString();

        AssertEx.EqualOrDiff(expected, actual);

        if (input.Equals(expected))
        {
            Assert.Empty(changes);
        }

        if (expectedChangedLines is not null)
        {
            var firstLine = changes.Min(e => razorSourceText.GetLinePositionSpan(e.Span).Start.Line);
            var lastLine = changes.Max(e => razorSourceText.GetLinePositionSpan(e.Span).End.Line);
            var delta = lastLine - firstLine + changes.Count(e => e.NewText.Contains(Environment.NewLine));
            Assert.Equal(expectedChangedLines.Value, delta + 1);
        }
    }

    private (string input, string htmlFormatted, string expected) ProcessFormattingContext(string input, string htmlFormatted, string expected)
    {
        Assert.True(_context.CreatedByFormattingDiscoverer, "Test class is using FormattingTestContext, but not using [FormattingTestFact] or [FormattingTestTheory]");

        if (_context.ShouldFlipLineEndings)
        {
            // flip the line endings of the stings (LF to CRLF and vice versa) and run again
            input = FormattingTestContext.FlipLineEndings(input);
            expected = FormattingTestContext.FlipLineEndings(expected);
            htmlFormatted = FormattingTestContext.FlipLineEndings(htmlFormatted);
        }

        return (input, htmlFormatted, expected);
    }
}
