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
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.LanguageServer.Test.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Serialization;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

// Sets the FileName static variable.
// Finds the test method name using reflection, and uses
// that to find the expected input/output test files in the file system.
[IntializeTestFile]

// These tests must be run serially due to the test specific FileName static var.
[Collection("FormattingTestSerialRuns")]
public class FormattingTestBase : RazorIntegrationTestBase
{
    private static readonly AsyncLocal<string> s_fileName = new AsyncLocal<string>();
    private static readonly IReadOnlyList<TagHelperDescriptor> s_defaultComponents = GetDefaultRuntimeComponents();

    public FormattingTestBase(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        TestProjectPath = GetProjectDirectory();

        ILoggerExtensions.TestOnlyLoggingEnabled = true;
    }

    public static string? TestProjectPath { get; private set; }

    // Used by the test framework to set the 'base' name for test files.
    public static string FileName
    {
        get { return s_fileName.Value!; }
        set { s_fileName.Value = value; }
    }

    protected async Task RunFormattingTestAsync(
        string input,
        string expected,
        int tabSize = 4,
        bool insertSpaces = true,
        string? fileKind = null,
        IReadOnlyList<TagHelperDescriptor>? tagHelpers = null,
        bool allowDiagnostics = false)
    {
        // Arrange
        fileKind ??= FileKinds.Component;

        TestFileMarkupParser.GetSpans(input, out input, out ImmutableArray<TextSpan> spans);

        var source = SourceText.From(input);
        var range = spans.IsEmpty
            ? null
            : spans.Single().AsRange(source);

        var path = "file:///path/to/Document." + fileKind;
        var uri = new Uri(path);
        var (codeDocument, documentSnapshot) = CreateCodeDocumentAndSnapshot(source, uri.AbsolutePath, tagHelpers, fileKind, allowDiagnostics);
        var options = new FormattingOptions()
        {
            TabSize = tabSize,
            InsertSpaces = insertSpaces,
        };

        var formattingService = await TestRazorFormattingService.CreateWithFullSupportAsync(codeDocument, documentSnapshot, LoggerFactory);
        var documentContext = new VersionedDocumentContext(uri, documentSnapshot, version: 1);

        // Act
        var edits = await formattingService.FormatAsync(documentContext, range, options, DisposalToken);

        // Assert
        var edited = ApplyEdits(source, edits);
        var actual = edited.ToString();

        new XUnitVerifier().EqualOrDiff(expected, actual);

        if (input.Equals(expected))
        {
            Assert.Empty(edits);
        }
    }

    protected async Task RunOnTypeFormattingTestAsync(
        string input,
        string expected,
        char triggerCharacter,
        int tabSize = 4,
        bool insertSpaces = true,
        string? fileKind = null,
        int? expectedChangedLines = null)
    {
        // Arrange
        fileKind ??= FileKinds.Component;

        TestFileMarkupParser.GetPosition(input, out input, out var positionAfterTrigger);

        var razorSourceText = SourceText.From(input);
        var path = "file:///path/to/Document.razor";
        var uri = new Uri(path);
        var (codeDocument, documentSnapshot) = CreateCodeDocumentAndSnapshot(razorSourceText, uri.AbsolutePath, fileKind: fileKind);

        var mappingService = new DefaultRazorDocumentMappingService(
            TestLanguageServerFeatureOptions.Instance, new TestDocumentContextFactory(), LoggerFactory);
        var languageKind = mappingService.GetLanguageKind(codeDocument, positionAfterTrigger, rightAssociative: false);

        var formattingService = await TestRazorFormattingService.CreateWithFullSupportAsync(codeDocument, documentSnapshot, LoggerFactory);
        var options = new FormattingOptions()
        {
            TabSize = tabSize,
            InsertSpaces = insertSpaces,
        };
        var documentContext = new VersionedDocumentContext(uri, documentSnapshot, version: 1);

        // Act
        var edits = await formattingService.FormatOnTypeAsync(documentContext, languageKind, Array.Empty<TextEdit>(), options, hostDocumentIndex: positionAfterTrigger, triggerCharacter: triggerCharacter, DisposalToken);

        // Assert
        var edited = ApplyEdits(razorSourceText, edits);
        var actual = edited.ToString();

        new XUnitVerifier().EqualOrDiff(expected, actual);

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
        string? fileKind = null)
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
        var (codeDocument, documentSnapshot) = CreateCodeDocumentAndSnapshot(razorSourceText, uri.AbsolutePath, fileKind: fileKind);

#pragma warning disable CS0618 // Type or member is obsolete
        var mappingService = new DefaultRazorDocumentMappingService();
#pragma warning restore CS0618 // Type or member is obsolete
        var languageKind = mappingService.GetLanguageKind(codeDocument, positionAfterTrigger, rightAssociative: false);
        if (languageKind == RazorLanguageKind.Html)
        {
            throw new NotImplementedException("Code action formatting is not yet supported for HTML in Razor.");
        }

        if (!mappingService.TryMapToProjectedDocumentPosition(codeDocument, positionAfterTrigger, out _, out var _))
        {
            throw new InvalidOperationException("Could not map from Razor document to generated document");
        }

        var formattingService = await TestRazorFormattingService.CreateWithFullSupportAsync(codeDocument);
        var options = new FormattingOptions()
        {
            TabSize = tabSize,
            InsertSpaces = insertSpaces,
        };
        var documentContext = new VersionedDocumentContext(uri, documentSnapshot, version: 1);

        // Act
        var edits = await formattingService.FormatCodeActionAsync(documentContext, languageKind, codeActionEdits, options, DisposalToken);

        // Assert
        var edited = ApplyEdits(razorSourceText, edits);
        var actual = edited.ToString();

        new XUnitVerifier().EqualOrDiff(expected, actual);
    }

    protected static TextEdit Edit(int startLine, int startChar, int endLine, int endChar, string newText)
        => new TextEdit()
        {
            Range = new VisualStudio.LanguageServer.Protocol.Range
            {
                Start = new Position(startLine, startChar),
                End = new Position(endLine, endChar),
            },
            NewText = newText,
        };

    private static SourceText ApplyEdits(SourceText source, TextEdit[] edits)
    {
        var changes = edits.Select(e => e.AsTextChange(source));
        return source.WithChanges(changes);
    }

    private static (RazorCodeDocument, DocumentSnapshot) CreateCodeDocumentAndSnapshot(SourceText text, string path, IReadOnlyList<TagHelperDescriptor>? tagHelpers = null, string? fileKind = default, bool allowDiagnostics = false)
    {
        fileKind ??= FileKinds.Component;
        tagHelpers ??= Array.Empty<TagHelperDescriptor>();
        if (fileKind == FileKinds.Component)
        {
            tagHelpers = tagHelpers.Concat(s_defaultComponents).ToArray();
        }

        var sourceDocument = text.GetRazorSourceDocument(path, path);

        // Yes I know "BlazorServer_31 is weird, but thats what is in the taghelpers.json file
        const string DefaultImports = """
                @using BlazorServer_31
                @using BlazorServer_31.Pages
                @using BlazorServer_31.Shared
                @using Microsoft.AspNetCore.Components
                @using Microsoft.AspNetCore.Components.Authorization
                @using Microsoft.AspNetCore.Components.Routing
                @using Microsoft.AspNetCore.Components.Web
                """;

        var importsPath = new Uri("file:///path/to/_Imports.razor").AbsolutePath;
        var importsSourceText = SourceText.From(DefaultImports);
        var importsDocument = importsSourceText.GetRazorSourceDocument(importsPath, importsPath);
        var importsSnapshot = new Mock<DocumentSnapshot>(MockBehavior.Strict);
        importsSnapshot
            .Setup(d => d.GetTextAsync())
            .ReturnsAsync(importsSourceText);
        importsSnapshot
            .Setup(d => d.FilePath)
            .Returns(importsPath);
        importsSnapshot
            .Setup(d => d.TargetPath)
            .Returns(importsPath);

        var projectEngine = RazorProjectEngine.Create(builder =>
        {
            builder.SetRootNamespace("Test");
            builder.Features.Add(new DefaultTypeNameFeature());
            RazorExtensions.Register(builder);
        });
        var codeDocument = projectEngine.ProcessDesignTime(sourceDocument, fileKind, new[] { importsDocument }, tagHelpers);

        if (!allowDiagnostics)
        {
            Assert.False(codeDocument.GetCSharpDocument().Diagnostics.Any(), "Error creating document:" + Environment.NewLine + string.Join(Environment.NewLine, codeDocument.GetCSharpDocument().Diagnostics));
        }

        var documentSnapshot = new Mock<DocumentSnapshot>(MockBehavior.Strict);
        documentSnapshot
            .Setup(d => d.GetGeneratedOutputAsync())
            .ReturnsAsync(codeDocument);
        documentSnapshot
            .Setup(d => d.GetImports())
            .Returns(new[] { importsSnapshot.Object });
        documentSnapshot
            .Setup(d => d.Project.GetProjectEngine())
            .Returns(projectEngine);
        documentSnapshot
            .Setup(d => d.FilePath)
            .Returns(path);
        documentSnapshot
            .Setup(d => d.TargetPath)
            .Returns(path);
        documentSnapshot
            .Setup(d => d.Project.TagHelpers)
            .Returns(tagHelpers);
        documentSnapshot
            .Setup(d => d.FileKind)
            .Returns(fileKind);

        return (codeDocument, documentSnapshot.Object);
    }

    private static string GetProjectDirectory()
    {
        var repoRoot = SearchUp(AppContext.BaseDirectory, "global.json");
        if (repoRoot is null)
        {
            repoRoot = AppContext.BaseDirectory;
        }

        var assemblyName = typeof(FormattingTestBase).Assembly.GetName().Name;
        var projectDirectory = Path.Combine(repoRoot, "src", "Razor", "test", assemblyName!);

        return projectDirectory;
    }

    private static string? SearchUp(string baseDirectory, string fileName)
    {
        var directoryInfo = new DirectoryInfo(baseDirectory);
        do
        {
            var fileInfo = new FileInfo(Path.Combine(directoryInfo.FullName, fileName));
            if (fileInfo.Exists)
            {
                return fileInfo.DirectoryName;
            }

            directoryInfo = directoryInfo.Parent;
        }
        while (directoryInfo?.Parent != null);

        return null;
    }

    private static IReadOnlyList<TagHelperDescriptor> GetDefaultRuntimeComponents()
    {
        var testFileName = "test.taghelpers.json";
        var current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current != null && !File.Exists(Path.Combine(current.FullName, testFileName)))
        {
            current = current.Parent;
        }

        var tagHelperFilePath = Path.Combine(current!.FullName, testFileName);
        var buffer = File.ReadAllBytes(tagHelperFilePath);
        var serializer = new JsonSerializer();
        serializer.Converters.Add(new TagHelperDescriptorJsonConverter());

        using var stream = new MemoryStream(buffer);
        using var streamReader = new StreamReader(stream);
        using var reader = new JsonTextReader(streamReader);

        return serializer.Deserialize<IReadOnlyList<TagHelperDescriptor>>(reader)!;
    }
}
