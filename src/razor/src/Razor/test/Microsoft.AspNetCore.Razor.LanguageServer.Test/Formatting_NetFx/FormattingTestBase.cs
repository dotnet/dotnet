// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor.Extensions;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.IntegrationTests;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Roslyn.Test.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

public abstract class FormattingTestBase : RazorToolingIntegrationTestBase
{
    private readonly HtmlFormattingService _htmlFormattingService;
    private readonly FormattingTestContext _context;

    internal sealed override bool UseTwoPhaseCompilation => true;

    internal sealed override bool DesignTime => true;

    private protected FormattingTestBase(FormattingTestContext context, HtmlFormattingService htmlFormattingService, ITestOutputHelper testOutput)
        : base(testOutput)
    {
        ITestOnlyLoggerExtensions.TestOnlyLoggingEnabled = true;

        _htmlFormattingService = htmlFormattingService;
        _context = context;
    }

    private protected async Task RunFormattingTestAsync(
        string input,
        string expected,
        int tabSize = 4,
        bool insertSpaces = true,
        string? fileKind = null,
        ImmutableArray<TagHelperDescriptor> tagHelpers = default,
        bool allowDiagnostics = false,
        bool codeBlockBraceOnNextLine = false,
        bool inGlobalNamespace = false)
    {
        (input, expected) = ProcessFormattingContext(input, expected);

        var razorLSPOptions = RazorLSPOptions.Default with { CodeBlockBraceOnNextLine = codeBlockBraceOnNextLine };

        await RunFormattingTestInternalAsync(input, expected, tabSize, insertSpaces, fileKind, tagHelpers, allowDiagnostics, razorLSPOptions, inGlobalNamespace);
    }

    private async Task RunFormattingTestInternalAsync(string input, string expected, int tabSize, bool insertSpaces, string? fileKind, ImmutableArray<TagHelperDescriptor> tagHelpers, bool allowDiagnostics, RazorLSPOptions? razorLSPOptions, bool inGlobalNamespace)
    {
        // Arrange
        fileKind ??= FileKinds.Component;
        tagHelpers = tagHelpers.NullToEmpty();

        TestFileMarkupParser.GetSpans(input, out input, out ImmutableArray<TextSpan> spans);

        var source = SourceText.From(input);
        LinePositionSpan? range = spans.IsEmpty
            ? null
            : source.GetLinePositionSpan(spans.Single());

        var path = "file:///path/to/Document." + fileKind;
        var uri = new Uri(path);
        var (codeDocument, documentSnapshot) = CreateCodeDocumentAndSnapshot(source, uri.AbsolutePath, tagHelpers, fileKind, allowDiagnostics, inGlobalNamespace);
        var options = new FormattingOptions()
        {
            TabSize = tabSize,
            InsertSpaces = insertSpaces,
        };
        var razorOptions = RazorFormattingOptions.From(options, codeBlockBraceOnNextLine: razorLSPOptions?.CodeBlockBraceOnNextLine ?? false);

        var languageServerFeatureOptions = new TestLanguageServerFeatureOptions(useNewFormattingEngine: _context.UseNewFormattingEngine);

        var formattingService = await TestRazorFormattingService.CreateWithFullSupportAsync(LoggerFactory, codeDocument, razorLSPOptions, languageServerFeatureOptions);
        var documentContext = new DocumentContext(uri, documentSnapshot, projectContext: null);

        var client = new FormattingLanguageServerClient(_htmlFormattingService, LoggerFactory);
        client.AddCodeDocument(codeDocument);

        var htmlFormatter = new HtmlFormatter(client);
        var htmlChanges = await htmlFormatter.GetDocumentFormattingEditsAsync(documentSnapshot, uri, options, DisposalToken);

        // Act
        var changes = await formattingService.GetDocumentFormattingChangesAsync(documentContext, htmlChanges, range, razorOptions, DisposalToken);

        // Assert
        var edited = source.WithChanges(changes);
        var actual = edited.ToString();

        AssertEx.EqualOrDiff(expected, actual);

        if (input.Equals(expected))
        {
            Assert.Empty(changes);
        }
    }

    private protected async Task RunOnTypeFormattingTestAsync(
        string input,
        string expected,
        char triggerCharacter,
        int tabSize = 4,
        bool insertSpaces = true,
        string? fileKind = null,
        int? expectedChangedLines = null,
        RazorLSPOptions? razorLSPOptions = null,
        bool inGlobalNamespace = false)
    {
        (input, expected) = ProcessFormattingContext(input, expected);

        // Arrange
        fileKind ??= FileKinds.Component;

        TestFileMarkupParser.GetPosition(input, out input, out var positionAfterTrigger);

        var razorSourceText = SourceText.From(input);
        var path = "file:///path/to/Document.razor";
        var uri = new Uri(path);
        var (codeDocument, documentSnapshot) = CreateCodeDocumentAndSnapshot(razorSourceText, uri.AbsolutePath, fileKind: fileKind, inGlobalNamespace: inGlobalNamespace);

        var languageServerFeatureOptions = new TestLanguageServerFeatureOptions(useNewFormattingEngine: _context.UseNewFormattingEngine);

        var filePathService = new LSPFilePathService(languageServerFeatureOptions);
        var mappingService = new LspDocumentMappingService(
            filePathService, new TestDocumentContextFactory(), LoggerFactory);
        var languageKind = codeDocument.GetLanguageKind(positionAfterTrigger, rightAssociative: false);

        var formattingService = await TestRazorFormattingService.CreateWithFullSupportAsync(LoggerFactory, codeDocument, razorLSPOptions, languageServerFeatureOptions);
        var options = new FormattingOptions()
        {
            TabSize = tabSize,
            InsertSpaces = insertSpaces,
        };
        var razorOptions = RazorFormattingOptions.From(options, codeBlockBraceOnNextLine: razorLSPOptions?.CodeBlockBraceOnNextLine ?? false);

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
            changes = await formattingService.GetHtmlOnTypeFormattingChangesAsync(documentContext, htmlChanges, razorOptions, hostDocumentIndex: positionAfterTrigger, triggerCharacter: triggerCharacter, DisposalToken);
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

    private (string input, string expected) ProcessFormattingContext(string input, string expected)
    {
        Assert.True(_context.CreatedByFormattingDiscoverer, "Test class is using FormattingTestContext, but not using [FormattingTestFact] or [FormattingTestTheory]");
        Assert.False(_context.ForceRuntimeCodeGeneration, "ForceRuntimeGeneration does not currently work in the language server. Creating tests for it is a false positive");

        if (_context.ShouldFlipLineEndings)
        {
            // flip the line endings of the stings (LF to CRLF and vice versa) and run again
            input = _context.FlipLineEndings(input);
            expected = _context.FlipLineEndings(expected);
        }

        return (input, expected);
    }

    protected async Task RunCodeActionFormattingTestAsync(
        string input,
        TextEdit[] codeActionEdits,
        string expected,
        int tabSize = 4,
        bool insertSpaces = true,
        string? fileKind = null,
        bool inGlobalNamespace = false)
    {
        // Arrange
        fileKind ??= FileKinds.Component;

        TestFileMarkupParser.GetPosition(input, out input, out var positionAfterTrigger);

        var razorSourceText = SourceText.From(input);
        var path = "file:///path/to/Document.razor";
        var uri = new Uri(path);
        var (codeDocument, documentSnapshot) = CreateCodeDocumentAndSnapshot(razorSourceText, uri.AbsolutePath, fileKind: fileKind, inGlobalNamespace: inGlobalNamespace);

        var filePathService = new LSPFilePathService(TestLanguageServerFeatureOptions.Instance);
        var mappingService = new LspDocumentMappingService(filePathService, new TestDocumentContextFactory(), LoggerFactory);
        var languageKind = codeDocument.GetLanguageKind(positionAfterTrigger, rightAssociative: false);
        if (languageKind == RazorLanguageKind.Html)
        {
            throw new NotImplementedException("Code action formatting is not yet supported for HTML in Razor.");
        }

        if (!mappingService.TryMapToGeneratedDocumentPosition(codeDocument.GetCSharpDocument(), positionAfterTrigger, out _, out var _))
        {
            throw new InvalidOperationException("Could not map from Razor document to generated document");
        }

        var formattingService = await TestRazorFormattingService.CreateWithFullSupportAsync(LoggerFactory, codeDocument);
        var options = new RazorFormattingOptions()
        {
            TabSize = tabSize,
            InsertSpaces = insertSpaces,
        };
        var documentContext = new DocumentContext(uri, documentSnapshot, projectContext: null);

        // Act
        var csharpSourceText = codeDocument.GetCSharpSourceText();
        var changes = codeActionEdits.SelectAsArray(csharpSourceText.GetTextChange);
        var edit = await formattingService.TryGetCSharpCodeActionEditAsync(documentContext, changes, options, DisposalToken);

        // Assert
        var edited = razorSourceText.WithChanges(edit.Value);
        var actual = edited.ToString();

        AssertEx.EqualOrDiff(expected, actual);
    }

    protected static TextEdit Edit(int startLine, int startChar, int endLine, int endChar, string newText)
        => VsLspFactory.CreateTextEdit(startLine, startChar, endLine, endChar, newText);

    private static (RazorCodeDocument, IDocumentSnapshot) CreateCodeDocumentAndSnapshot(SourceText text, string path, ImmutableArray<TagHelperDescriptor> tagHelpers = default, string? fileKind = null, bool allowDiagnostics = false, bool inGlobalNamespace = false)
    {
        fileKind ??= FileKinds.Component;
        tagHelpers = tagHelpers.NullToEmpty();

        if (fileKind == FileKinds.Component)
        {
            tagHelpers = tagHelpers.AddRange(RazorTestResources.BlazorServerAppTagHelpers);
        }

        var sourceDocument = RazorSourceDocument.Create(text, RazorSourceDocumentProperties.Create(
            filePath: path,
            relativePath: inGlobalNamespace ? Path.GetFileName(path) : path));

        const string DefaultImports = """
            @using BlazorApp1
            @using BlazorApp1.Pages
            @using BlazorApp1.Shared
            @using Microsoft.AspNetCore.Components
            @using Microsoft.AspNetCore.Components.Authorization
            @using Microsoft.AspNetCore.Components.Routing
            @using Microsoft.AspNetCore.Components.Web
            """;

        var importPath = new Uri("file:///path/to/_Imports.razor").AbsolutePath;
        var importText = SourceText.From(DefaultImports);
        var importSource = RazorSourceDocument.Create(importText, RazorSourceDocumentProperties.Create(importPath, importPath));
        var importSnapshotMock = new StrictMock<IDocumentSnapshot>();
        importSnapshotMock
            .Setup(d => d.GetTextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(importText);
        importSnapshotMock
            .Setup(d => d.FilePath)
            .Returns(importPath);
        importSnapshotMock
            .Setup(d => d.TargetPath)
            .Returns(importPath);

        var projectFileSystem = new TestRazorProjectFileSystem([
            new TestRazorProjectItem(path, fileKind: fileKind),
            new TestRazorProjectItem(importPath, fileKind: FileKinds.ComponentImport)]);

        var projectEngine = RazorProjectEngine.Create(
            new RazorConfiguration(RazorLanguageVersion.Latest, "TestConfiguration", Extensions: []),
            projectFileSystem,
            builder =>
            {
                builder.SetRootNamespace(inGlobalNamespace ? string.Empty : "Test");
                builder.Features.Add(new DefaultTypeNameFeature());

                builder.ConfigureParserOptions(builder =>
                {
                    builder.UseRoslynTokenizer = true;
                });

                RazorExtensions.Register(builder);
            });

        var codeDocument = projectEngine.ProcessDesignTime(sourceDocument, fileKind, [importSource], tagHelpers);

        if (!allowDiagnostics)
        {
            Assert.False(codeDocument.GetCSharpDocument().Diagnostics.Any(), "Error creating document:" + Environment.NewLine + string.Join(Environment.NewLine, codeDocument.GetCSharpDocument().Diagnostics));
        }

        var documentSnapshot = CreateDocumentSnapshot(
            path, fileKind, codeDocument, projectEngine, [importSnapshotMock.Object], [importSource], tagHelpers, inGlobalNamespace);

        return (codeDocument, documentSnapshot);
    }

    internal static IDocumentSnapshot CreateDocumentSnapshot(
        string path,
        string fileKind,
        RazorCodeDocument codeDocument,
        RazorProjectEngine projectEngine,
        ImmutableArray<IDocumentSnapshot> imports,
        ImmutableArray<RazorSourceDocument> importDocuments,
        ImmutableArray<TagHelperDescriptor> tagHelpers,
        bool inGlobalNamespace)
    {
        var snapshotMock = new StrictMock<IDocumentSnapshot>();

        snapshotMock
            .Setup(d => d.GetGeneratedOutputAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(codeDocument);
        snapshotMock
            .Setup(d => d.FilePath)
            .Returns(path);
        snapshotMock
            .Setup(d => d.Project.Key)
            .Returns(TestProjectKey.Create("/obj"));
        snapshotMock
            .Setup(d => d.TargetPath)
            .Returns(path);
        snapshotMock
            .Setup(d => d.GetTextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(codeDocument.Source.Text);
        snapshotMock
            .Setup(d => d.Project.GetTagHelpersAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(tagHelpers);
        snapshotMock
            .Setup(d => d.FileKind)
            .Returns(fileKind);
        snapshotMock
            .Setup(d => d.Version)
            .Returns(1);
        snapshotMock
            .Setup(d => d.WithText(It.IsAny<SourceText>()))
            .Returns<SourceText>(text =>
            {
                var source = RazorSourceDocument.Create(text, RazorSourceDocumentProperties.Create(
                    filePath: path,
                    relativePath: inGlobalNamespace ? Path.GetFileName(path) : path));

                var codeDocument = projectEngine.ProcessDesignTime(source, fileKind, importDocuments, tagHelpers);

                return CreateDocumentSnapshot(
                    path, fileKind, codeDocument, projectEngine, imports, importDocuments, tagHelpers, inGlobalNamespace);
            });

#if !FORMAT_FUSE
        var generatorMock = snapshotMock.As<IDesignTimeCodeGenerator>();
        generatorMock
            .Setup(x => x.GenerateDesignTimeOutputAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(codeDocument);
#endif

        return snapshotMock.Object;
    }
}
