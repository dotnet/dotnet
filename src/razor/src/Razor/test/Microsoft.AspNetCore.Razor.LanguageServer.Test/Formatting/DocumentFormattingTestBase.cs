// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Basic.Reference.Assemblies;
using Microsoft.AspNetCore.Mvc.Razor.Extensions;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.IntegrationTests;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.AspNetCore.Razor.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Features;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.ProjectEngineHost;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Settings;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Moq;
using Roslyn.Test.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

public abstract class DocumentFormattingTestBase(ITestOutputHelper testOutput) : RazorToolingIntegrationTestBase(testOutput)
{
    private static readonly AsyncLazy<TagHelperCollection> s_standardTagHelpers = AsyncLazy.Create(GetStandardTagHelpersAsync);

    internal sealed override bool UseTwoPhaseCompilation => true;

    private protected async Task RunFormattingTestAsync(
        string input,
        string htmlFormatted,
        string expected,
        int tabSize = 4,
        bool insertSpaces = true,
        RazorFileKind? fileKind = null,
        TagHelperCollection? tagHelpers = null,
        bool allowDiagnostics = false,
        bool codeBlockBraceOnNextLine = false,
        AttributeIndentStyle attributeIndentStyle = AttributeIndentStyle.AlignWithFirst,
        bool inGlobalNamespace = false,
        bool debugAssertsEnabled = true,
        bool validateHtmlFormattedMatchesWebTools = true,
        RazorCSharpSyntaxFormattingOptions? csharpSyntaxFormattingOptions = null)
    {
        var razorLSPOptions = RazorLSPOptions.Default with
        {
            CodeBlockBraceOnNextLine = codeBlockBraceOnNextLine,
            AttributeIndentStyle = attributeIndentStyle,
        };

        csharpSyntaxFormattingOptions ??= RazorCSharpSyntaxFormattingOptions.Default;

        await RunFormattingTestInternalAsync(input, htmlFormatted, expected, tabSize, insertSpaces, fileKind, tagHelpers, allowDiagnostics, razorLSPOptions, inGlobalNamespace, debugAssertsEnabled, validateHtmlFormattedMatchesWebTools, csharpSyntaxFormattingOptions);
    }

    private async Task RunFormattingTestInternalAsync(
        string input,
        string htmlFormatted,
        string expected,
        int tabSize,
        bool insertSpaces,
        RazorFileKind? fileKind,
        TagHelperCollection? tagHelpers,
        bool allowDiagnostics,
        RazorLSPOptions? razorLSPOptions,
        bool inGlobalNamespace,
        bool debugAssertsEnabled,
        bool validateHtmlFormattedMatchesWebTools,
        RazorCSharpSyntaxFormattingOptions csharpSyntaxFormattingOptions)
    {
        // Arrange
        var fileKindValue = fileKind ?? RazorFileKind.Component;
        tagHelpers ??= [];

        TestFileMarkupParser.GetSpans(input, out input, out ImmutableArray<TextSpan> spans);

        var source = SourceText.From(input);
        LinePositionSpan? range = spans.IsEmpty
            ? null
            : source.GetLinePositionSpan(spans.Single());

        tagHelpers = TagHelperCollection.Merge(tagHelpers, await s_standardTagHelpers.GetValueAsync(DisposalToken));

        var path = "file:///path/to/Document." + fileKindValue.ToString();
        var uri = new Uri(path);
        var (codeDocument, documentSnapshot) = DocumentFormattingTestBase.CreateCodeDocumentAndSnapshot(source, uri.AbsolutePath, tagHelpers, fileKindValue, allowDiagnostics, inGlobalNamespace);
        var options = new FormattingOptions()
        {
            TabSize = tabSize,
            InsertSpaces = insertSpaces,
        };
        var razorOptions = RazorFormattingOptions.From(options, codeBlockBraceOnNextLine: razorLSPOptions?.CodeBlockBraceOnNextLine ?? false, razorLSPOptions?.AttributeIndentStyle ?? AttributeIndentStyle.AlignWithFirst, csharpSyntaxFormattingOptions);

        var languageServerFeatureOptions = new TestLanguageServerFeatureOptions();

        var formattingService = await TestRazorFormattingService.CreateWithFullSupportAsync(LoggerFactory, TestOutputHelper, codeDocument, razorLSPOptions, languageServerFeatureOptions, debugAssertsEnabled);
        var documentContext = new DocumentContext(uri, documentSnapshot, projectContext: null);

        var htmlChanges = SourceText.From(htmlFormatted).GetTextChangesArray(source);

        if (validateHtmlFormattedMatchesWebTools)
        {
#if NETFRAMEWORK
            var htmlFormattingService = new HtmlFormattingService();
            var client = new FormattingLanguageServerClient(htmlFormattingService, LoggerFactory);
            client.AddCodeDocument(codeDocument);

            var htmlFormatter = new HtmlFormatter(client);
            var htmlEdited = source.WithChanges(htmlChanges);
            var htmlEditedLegacy = source.WithChanges(await htmlFormatter.GetDocumentFormattingEditsAsync(documentSnapshot, uri, options, DisposalToken) ?? []);
            Assert.Equal(htmlEdited.ToString(), htmlEditedLegacy.ToString());
            Assert.Equal(htmlFormatted, htmlEditedLegacy.ToString());
            AssertEx.EqualOrDiff(htmlFormatted, htmlEdited.ToString());
#endif
        }

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

    private protected static (RazorCodeDocument, IDocumentSnapshot) CreateCodeDocumentAndSnapshot(
        SourceText text,
        string path,
        TagHelperCollection tagHelpers,
        RazorFileKind? fileKind = null,
        bool allowDiagnostics = false,
        bool inGlobalNamespace = false)
    {
        var fileKindValue = fileKind ?? RazorFileKind.Component;

        var sourceDocument = RazorSourceDocument.Create(text, RazorSourceDocumentProperties.Create(
            filePath: path,
            relativePath: inGlobalNamespace ? Path.GetFileName(path) : path));

        const string DefaultImports = """
            @using Microsoft.AspNetCore.Components
            @using Microsoft.AspNetCore.Components.Authorization
            @using Microsoft.AspNetCore.Components.Forms
            @using Microsoft.AspNetCore.Components.Routing
            @using Microsoft.AspNetCore.Components.Web

            @addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
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
            new TestRazorProjectItem(path, fileKind: fileKindValue),
            new TestRazorProjectItem(importPath, fileKind: RazorFileKind.ComponentImport)]);

        var projectEngine = RazorProjectEngine.Create(
            new RazorConfiguration(RazorLanguageVersion.Latest, "TestConfiguration", Extensions: []),
            projectFileSystem,
            builder =>
            {
                builder.SetRootNamespace(inGlobalNamespace ? string.Empty : "Test");

                builder.ConfigureParserOptions(builder =>
                {
                    builder.UseRoslynTokenizer = true;
                });

                RazorExtensions.Register(builder);
            });

        var codeDocument = projectEngine.Process(sourceDocument, fileKindValue, [importSource], tagHelpers);

        if (!allowDiagnostics)
        {
            Assert.False(codeDocument.GetRequiredCSharpDocument().Diagnostics.Any(), "Error creating document:" + Environment.NewLine + string.Join(Environment.NewLine, codeDocument.GetRequiredCSharpDocument().Diagnostics));
        }

        var documentSnapshot = CreateDocumentSnapshot(
            path, fileKindValue, codeDocument, projectEngine, [importSnapshotMock.Object], [importSource], tagHelpers, inGlobalNamespace);

        return (codeDocument, documentSnapshot);
    }

    internal static IDocumentSnapshot CreateDocumentSnapshot(
        string path,
        RazorFileKind fileKind,
        RazorCodeDocument codeDocument,
        RazorProjectEngine projectEngine,
        ImmutableArray<IDocumentSnapshot> imports,
        ImmutableArray<RazorSourceDocument> importDocuments,
        TagHelperCollection tagHelpers,
        bool inGlobalNamespace)
    {
        var projectKey = new ProjectKey(Path.Combine(path, "obj"));

        var snapshotMock = new StrictMock<IDocumentSnapshot>();

        snapshotMock
            .Setup(d => d.GetGeneratedOutputAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(codeDocument);
        snapshotMock
            .Setup(d => d.FilePath)
            .Returns(path);
        snapshotMock
            .Setup(d => d.Project.Key)
            .Returns(projectKey);
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

                var codeDocument = projectEngine.Process(source, fileKind, importDocuments, tagHelpers);

                return CreateDocumentSnapshot(
                    path, fileKind, codeDocument, projectEngine, imports, importDocuments, tagHelpers, inGlobalNamespace);
            });
        snapshotMock
            .Setup(d => d.GetCSharpSyntaxTreeAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(codeDocument.GetOrParseCSharpSyntaxTree(CancellationToken.None));

        return snapshotMock.Object;
    }

    private protected static async Task<TagHelperCollection> GetStandardTagHelpersAsync(CancellationToken cancellationToken)
    {
        var projectId = ProjectId.CreateNewId();
        var projectInfo = ProjectInfo
            .Create(
                projectId,
                VersionStamp.Create(),
                name: TestProjectData.SomeProject.FilePath,
                assemblyName: TestProjectData.SomeProject.FilePath,
                LanguageNames.CSharp,
                TestProjectData.SomeProject.FilePath,
                compilationOptions: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
            .WithMetadataReferences(AspNet80.ReferenceInfos.All.Select(r => r.Reference))
            .WithDefaultNamespace(TestProjectData.SomeProject.RootNamespace);

        using var workspace = new AdhocWorkspace();
        var project = workspace.CurrentSolution.AddProject(projectInfo).GetRequiredProject(projectId);

        var configuration = new RazorConfiguration(
            RazorLanguageVersion.Experimental,
            "MVC-3.0",
            Extensions: [],
            CSharpLanguageVersion: CSharpParseOptions.Default.LanguageVersion,
            UseConsolidatedMvcViews: true,
            SuppressAddComponentParameter: false,
            UseRoslynTokenizer: false,
            PreprocessorSymbols: []);

        var fileSystem = RazorProjectFileSystem.Create(TestProjectData.SomeProject.FilePath);

        var engineFactory = ProjectEngineFactories.DefaultProvider.GetFactory(configuration);

        var engine = engineFactory.Create(
            configuration,
            fileSystem,
            configure: null);

        var tagHelpers = await project.GetTagHelpersAsync(engine, cancellationToken).ConfigureAwait(false);
        Assert.NotEmpty(tagHelpers);

        return tagHelpers;
    }
}
