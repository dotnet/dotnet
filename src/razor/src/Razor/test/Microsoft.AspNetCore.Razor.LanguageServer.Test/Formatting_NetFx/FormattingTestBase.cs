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
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
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

public class FormattingTestBase : RazorToolingIntegrationTestBase
{
    public FormattingTestBase(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        ITestOnlyLoggerExtensions.TestOnlyLoggingEnabled = true;
    }

    private protected async Task RunFormattingTestAsync(
        string input,
        string expected,
        int tabSize = 4,
        bool insertSpaces = true,
        string? fileKind = null,
        ImmutableArray<TagHelperDescriptor> tagHelpers = default,
        bool allowDiagnostics = false,
        RazorLSPOptions? razorLSPOptions = null,
        bool inGlobalNamespace = false)
    {
        // Run with and without forceRuntimeCodeGeneration
        await RunFormattingTestAsync(input, expected, tabSize, insertSpaces, fileKind, tagHelpers, allowDiagnostics, razorLSPOptions, inGlobalNamespace, forceRuntimeCodeGeneration: true);
        await RunFormattingTestAsync(input, expected, tabSize, insertSpaces, fileKind, tagHelpers, allowDiagnostics, razorLSPOptions, inGlobalNamespace, forceRuntimeCodeGeneration: false);
    }

    private async Task RunFormattingTestAsync(string input, string expected, int tabSize, bool insertSpaces, string? fileKind, ImmutableArray<TagHelperDescriptor> tagHelpers, bool allowDiagnostics, RazorLSPOptions? razorLSPOptions, bool inGlobalNamespace, bool forceRuntimeCodeGeneration)
    {
        // Arrange
        fileKind ??= FileKinds.Component;
        tagHelpers = tagHelpers.NullToEmpty();

        TestFileMarkupParser.GetSpans(input, out input, out ImmutableArray<TextSpan> spans);

        var source = SourceText.From(input);
        var range = spans.IsEmpty
            ? null
            : source.GetRange(spans.Single());

        var path = "file:///path/to/Document." + fileKind;
        var uri = new Uri(path);
        var (codeDocument, documentSnapshot) = CreateCodeDocumentAndSnapshot(source, uri.AbsolutePath, tagHelpers, fileKind, allowDiagnostics, inGlobalNamespace, forceRuntimeCodeGeneration);
        var options = new FormattingOptions()
        {
            TabSize = tabSize,
            InsertSpaces = insertSpaces,
        };

        var formattingService = await TestRazorFormattingService.CreateWithFullSupportAsync(LoggerFactory, codeDocument, razorLSPOptions);
        var documentContext = new DocumentContext(uri, documentSnapshot, projectContext: null);

        // Act
        var edits = await formattingService.FormatAsync(documentContext, range, options, DisposalToken);

        // Assert
        var edited = ApplyEdits(source, edits);
        var actual = edited.ToString();

        AssertEx.EqualOrDiff(expected, actual);

        if (input.Equals(expected))
        {
            Assert.Empty(edits);
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
        // Arrange
        fileKind ??= FileKinds.Component;

        TestFileMarkupParser.GetPosition(input, out input, out var positionAfterTrigger);

        var razorSourceText = SourceText.From(input);
        var path = "file:///path/to/Document.razor";
        var uri = new Uri(path);
        var (codeDocument, documentSnapshot) = CreateCodeDocumentAndSnapshot(razorSourceText, uri.AbsolutePath, fileKind: fileKind, inGlobalNamespace: inGlobalNamespace);

        var filePathService = new LSPFilePathService(TestLanguageServerFeatureOptions.Instance);
        var mappingService = new LspDocumentMappingService(
            filePathService, new TestDocumentContextFactory(), LoggerFactory);
        var languageKind = mappingService.GetLanguageKind(codeDocument, positionAfterTrigger, rightAssociative: false);

        var formattingService = await TestRazorFormattingService.CreateWithFullSupportAsync(LoggerFactory, codeDocument, razorLSPOptions);
        var options = new FormattingOptions()
        {
            TabSize = tabSize,
            InsertSpaces = insertSpaces,
        };
        var documentContext = new DocumentContext(uri, documentSnapshot, projectContext: null);

        // Act
        var edits = await formattingService.FormatOnTypeAsync(documentContext, languageKind, Array.Empty<TextEdit>(), options, hostDocumentIndex: positionAfterTrigger, triggerCharacter: triggerCharacter, DisposalToken);

        // Assert
        var edited = ApplyEdits(razorSourceText, edits);
        var actual = edited.ToString();

        AssertEx.EqualOrDiff(expected, actual);

        if (input.Equals(expected))
        {
            Assert.Empty(edits);
        }

        if (expectedChangedLines is not null)
        {
            var firstLine = edits.Min(e => e.Range.Start.Line);
            var lastLine = edits.Max(e => e.Range.End.Line);
            var delta = lastLine - firstLine + edits.Count(e => e.NewText.Contains(Environment.NewLine));
            Assert.Equal(expectedChangedLines.Value, delta + 1);
        }
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
        if (codeActionEdits is null)
        {
            throw new NotImplementedException("Code action formatting must provide edits.");
        }

        // Arrange
        fileKind ??= FileKinds.Component;

        TestFileMarkupParser.GetPosition(input, out input, out var positionAfterTrigger);

        var razorSourceText = SourceText.From(input);
        var path = "file:///path/to/Document.razor";
        var uri = new Uri(path);
        var (codeDocument, documentSnapshot) = CreateCodeDocumentAndSnapshot(razorSourceText, uri.AbsolutePath, fileKind: fileKind, inGlobalNamespace: inGlobalNamespace);

        var filePathService = new LSPFilePathService(TestLanguageServerFeatureOptions.Instance);
        var mappingService = new LspDocumentMappingService(filePathService, new TestDocumentContextFactory(), LoggerFactory);
        var languageKind = mappingService.GetLanguageKind(codeDocument, positionAfterTrigger, rightAssociative: false);
        if (languageKind == RazorLanguageKind.Html)
        {
            throw new NotImplementedException("Code action formatting is not yet supported for HTML in Razor.");
        }

        if (!mappingService.TryMapToGeneratedDocumentPosition(codeDocument.GetCSharpDocument(), positionAfterTrigger, out _, out var _))
        {
            throw new InvalidOperationException("Could not map from Razor document to generated document");
        }

        var formattingService = await TestRazorFormattingService.CreateWithFullSupportAsync(LoggerFactory, codeDocument);
        var options = new FormattingOptions()
        {
            TabSize = tabSize,
            InsertSpaces = insertSpaces,
        };
        var documentContext = new DocumentContext(uri, documentSnapshot, projectContext: null);

        // Act
        var edits = await formattingService.FormatCodeActionAsync(documentContext, languageKind, codeActionEdits, options, DisposalToken);

        // Assert
        var edited = ApplyEdits(razorSourceText, edits);
        var actual = edited.ToString();

        AssertEx.EqualOrDiff(expected, actual);
    }

    protected static TextEdit Edit(int startLine, int startChar, int endLine, int endChar, string newText)
        => VsLspFactory.CreateTextEdit(startLine, startChar, endLine, endChar, newText);

    private static SourceText ApplyEdits(SourceText source, TextEdit[] edits)
    {
        var changes = edits.Select(source.GetTextChange);
        return source.WithChanges(changes);
    }

    private static (RazorCodeDocument, IDocumentSnapshot) CreateCodeDocumentAndSnapshot(SourceText text, string path, ImmutableArray<TagHelperDescriptor> tagHelpers = default, string? fileKind = default, bool allowDiagnostics = false, bool inGlobalNamespace = false, bool forceRuntimeCodeGeneration = false)
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

        var importsPath = new Uri("file:///path/to/_Imports.razor").AbsolutePath;
        var importsSourceText = SourceText.From(DefaultImports);
        var importsDocument = RazorSourceDocument.Create(importsSourceText, RazorSourceDocumentProperties.Create(importsPath, importsPath));
        var importsSnapshot = new Mock<IDocumentSnapshot>(MockBehavior.Strict);
        importsSnapshot
            .Setup(d => d.GetTextAsync())
            .ReturnsAsync(importsSourceText);
        importsSnapshot
            .Setup(d => d.FilePath)
            .Returns(importsPath);
        importsSnapshot
            .Setup(d => d.TargetPath)
            .Returns(importsPath);

        var projectFileSystem = new TestRazorProjectFileSystem([
            new TestRazorProjectItem(path, fileKind: fileKind),
            new TestRazorProjectItem(importsPath, fileKind: FileKinds.ComponentImport),
            ]);

        var projectEngine = RazorProjectEngine.Create(
            new RazorConfiguration(RazorLanguageVersion.Latest, "TestConfiguration", ImmutableArray<RazorExtension>.Empty, new LanguageServerFlags(forceRuntimeCodeGeneration)),
            projectFileSystem,
            builder =>
            {
                builder.SetRootNamespace(inGlobalNamespace ? string.Empty : "Test");
                builder.Features.Add(new DefaultTypeNameFeature());
                RazorExtensions.Register(builder);
            });

        var codeDocument = projectEngine.ProcessDesignTime(sourceDocument, fileKind, ImmutableArray.Create(importsDocument), tagHelpers);

        if (!allowDiagnostics)
        {
            Assert.False(codeDocument.GetCSharpDocument().Diagnostics.Any(), "Error creating document:" + Environment.NewLine + string.Join(Environment.NewLine, codeDocument.GetCSharpDocument().Diagnostics));
        }

        var imports = ImmutableArray.Create(importsSnapshot.Object);
        var importsDocuments = ImmutableArray.Create(importsDocument);
        var documentSnapshot = CreateDocumentSnapshot(path, tagHelpers, fileKind, importsDocuments, imports, projectEngine, codeDocument, inGlobalNamespace: inGlobalNamespace);

        return (codeDocument, documentSnapshot);
    }

    internal static IDocumentSnapshot CreateDocumentSnapshot(string path, ImmutableArray<TagHelperDescriptor> tagHelpers, string? fileKind, ImmutableArray<RazorSourceDocument> importsDocuments, ImmutableArray<IDocumentSnapshot> imports, RazorProjectEngine projectEngine, RazorCodeDocument codeDocument, bool inGlobalNamespace = false)
    {
        var documentSnapshot = new Mock<IDocumentSnapshot>(MockBehavior.Strict);
        documentSnapshot
            .Setup(d => d.GetGeneratedOutputAsync())
            .ReturnsAsync(codeDocument);
        documentSnapshot
            .Setup(d => d.FilePath)
            .Returns(path);
        documentSnapshot
            .Setup(d => d.Project.Key)
            .Returns(TestProjectKey.Create("/obj"));
        documentSnapshot
            .Setup(d => d.TargetPath)
            .Returns(path);
        documentSnapshot
            .Setup(d => d.Project.Configuration)
            .Returns(projectEngine.Configuration);
        documentSnapshot
            .Setup(d => d.GetTextAsync())
            .ReturnsAsync(codeDocument.Source.Text);
        documentSnapshot
            .Setup(d => d.Project.GetTagHelpersAsync(It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<ImmutableArray<TagHelperDescriptor>>(tagHelpers));
        documentSnapshot
            .Setup(d => d.Project.GetProjectEngine())
            .Returns(projectEngine);
        documentSnapshot
            .Setup(d => d.FileKind)
            .Returns(fileKind);
        documentSnapshot
            .Setup(d => d.Version)
            .Returns(1);
        documentSnapshot
            .Setup(d => d.WithText(It.IsAny<SourceText>()))
            .Returns<SourceText>(text =>
            {
                var sourceDocument = RazorSourceDocument.Create(text, RazorSourceDocumentProperties.Create(
                    filePath: path,
                    relativePath: inGlobalNamespace ? Path.GetFileName(path) : path));
                var codeDocument = projectEngine.ProcessDesignTime(sourceDocument, fileKind, importsDocuments, tagHelpers);
                return CreateDocumentSnapshot(path, tagHelpers, fileKind, importsDocuments, imports, projectEngine, codeDocument, inGlobalNamespace: inGlobalNamespace);
            });
        return documentSnapshot.Object;
    }
}
