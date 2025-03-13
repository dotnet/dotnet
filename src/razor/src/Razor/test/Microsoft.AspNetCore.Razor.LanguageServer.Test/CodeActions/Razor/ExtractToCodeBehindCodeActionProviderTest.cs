// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis.Razor.CodeActions;
using Microsoft.CodeAnalysis.Razor.CodeActions.Models;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.CodeActions;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

public class ExtractToCodeBehindCodeActionProviderTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    [Fact]
    public async Task Handle_InvalidFileKind()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = """
            @page "/test"
            @$$code {}
            """;
        TestFileMarkupParser.GetPosition(contents, out contents, out var cursorPosition);

        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = VsLspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
        };

        var context = CreateRazorCodeActionContext(request, cursorPosition, documentPath, contents, fileKind: FileKinds.Legacy);

        var provider = new ExtractToCodeBehindCodeActionProvider(LoggerFactory);

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, DisposalToken);

        // Assert
        Assert.Empty(commandOrCodeActionContainer);
    }

    [Fact]
    public async Task Handle_OutsideCodeDirective()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = """
            @page "/$$test"
            @code {}
            """;
        TestFileMarkupParser.GetPosition(contents, out contents, out var cursorPosition);

        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = VsLspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
        };

        var context = CreateRazorCodeActionContext(request, cursorPosition, documentPath, contents);

        var provider = new ExtractToCodeBehindCodeActionProvider(LoggerFactory);

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, DisposalToken);

        // Assert
        Assert.Empty(commandOrCodeActionContainer);
    }

    [Fact]
    public async Task Handle_InCodeDirectiveBlock_ReturnsNull()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = """
            @page "/test"
            @code {$$}
            """;
        TestFileMarkupParser.GetPosition(contents, out contents, out var cursorPosition);

        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = VsLspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
        };

        var context = CreateRazorCodeActionContext(request, cursorPosition, documentPath, contents);

        var provider = new ExtractToCodeBehindCodeActionProvider(LoggerFactory);

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, DisposalToken);

        // Assert
        Assert.Empty(commandOrCodeActionContainer);
    }

    [Fact]
    public async Task Handle_InCodeDirectiveMalformed_ReturnsNull()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = """
            @page "/test"
            @$$code
            """;
        TestFileMarkupParser.GetPosition(contents, out contents, out var cursorPosition);

        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = VsLspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
        };

        var context = CreateRazorCodeActionContext(request, cursorPosition, documentPath, contents);

        var provider = new ExtractToCodeBehindCodeActionProvider(LoggerFactory);

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, DisposalToken);

        // Assert
        Assert.Empty(commandOrCodeActionContainer);
    }

    [Fact]
    public async Task Handle_InCodeDirectiveWithMarkup_ReturnsNull()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = """
            @page "/test"
            @$$code {
                void Test()
                {
                    <h1>Hello, world!</h1>
                }
            }
            """;
        TestFileMarkupParser.GetPosition(contents, out contents, out var cursorPosition);

        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = VsLspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
        };

        var context = CreateRazorCodeActionContext(request, cursorPosition, documentPath, contents);

        var provider = new ExtractToCodeBehindCodeActionProvider(LoggerFactory);

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, DisposalToken);

        // Assert
        Assert.Empty(commandOrCodeActionContainer);
    }

    [Theory]
    [InlineData("@$$code")]
    [InlineData("@c$$ode")]
    [InlineData("@co$$de")]
    [InlineData("@cod$$e")]
    [InlineData("@code$$")]
    public async Task Handle_InCodeDirective_SupportsFileCreationTrue_ReturnsResult(string codeDirective)
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = $$"""
            @page "/test"
            {|remove:{{codeDirective}}{|extract: { private var x = 1; }|}|}
            """;

        TestFileMarkupParser.GetPositionAndSpans(
            contents, out contents, out int cursorPosition,
            out ImmutableDictionary<string, ImmutableArray<TextSpan>> namedSpans);

        var extractSpan = namedSpans["extract"].Single();
        var removeSpan = namedSpans["remove"].Single();

        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = VsLspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
        };

        var context = CreateRazorCodeActionContext(request, cursorPosition, documentPath, contents, supportsFileCreation: true);

        var provider = new ExtractToCodeBehindCodeActionProvider(LoggerFactory);

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, DisposalToken);

        // Assert
        var codeAction = Assert.Single(commandOrCodeActionContainer);
        var razorCodeActionResolutionParams = ((JsonElement)codeAction.Data!).Deserialize<RazorCodeActionResolutionParams>();
        Assert.NotNull(razorCodeActionResolutionParams);
        var actionParams = ((JsonElement)razorCodeActionResolutionParams.Data!).Deserialize<ExtractToCodeBehindCodeActionParams>();
        Assert.NotNull(actionParams);

        Assert.Equal(removeSpan, TextSpan.FromBounds(actionParams.RemoveStart, actionParams.RemoveEnd));
        Assert.Equal(extractSpan, TextSpan.FromBounds(actionParams.ExtractStart, actionParams.ExtractEnd));
    }

    [Fact]
    public async Task Handle_AtEndOfCodeDirectiveWithNoSpace_ReturnsResult()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = """
            @page "/test"
            {|remove:@code$${|extract:{ private var x = 1; }|}|}
            """;

        TestFileMarkupParser.GetPositionAndSpans(
            contents, out contents, out int cursorPosition,
            out ImmutableDictionary<string, ImmutableArray<TextSpan>> namedSpans);

        var extractSpan = namedSpans["extract"].Single();
        var removeSpan = namedSpans["remove"].Single();

        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = VsLspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
        };

        var context = CreateRazorCodeActionContext(request, cursorPosition, documentPath, contents, supportsFileCreation: true);

        var provider = new ExtractToCodeBehindCodeActionProvider(LoggerFactory);

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, DisposalToken);

        // Assert
        var codeAction = Assert.Single(commandOrCodeActionContainer);
        var razorCodeActionResolutionParams = ((JsonElement)codeAction.Data!).Deserialize<RazorCodeActionResolutionParams>();
        Assert.NotNull(razorCodeActionResolutionParams);
        var actionParams = ((JsonElement)razorCodeActionResolutionParams.Data!).Deserialize<ExtractToCodeBehindCodeActionParams>();
        Assert.NotNull(actionParams);

        Assert.Equal(removeSpan, TextSpan.FromBounds(actionParams.RemoveStart, actionParams.RemoveEnd));
        Assert.Equal(extractSpan, TextSpan.FromBounds(actionParams.ExtractStart, actionParams.ExtractEnd));
    }

    [Fact]
    public async Task Handle_InCodeDirective_SupportsFileCreationFalse_ReturnsNull()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = """
            @page "/test"
            @$$code { private var x = 1; }
            """;
        TestFileMarkupParser.GetPosition(contents, out contents, out var cursorPosition);

        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = VsLspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
        };

        var context = CreateRazorCodeActionContext(request, cursorPosition, documentPath, contents, supportsFileCreation: false);

        var provider = new ExtractToCodeBehindCodeActionProvider(LoggerFactory);

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, DisposalToken);

        // Assert
        Assert.Empty(commandOrCodeActionContainer);
    }

    [Fact]
    public async Task Handle_InFunctionsDirective_SupportsFileCreationTrue_ReturnsResult()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = """
            @page "/test"
            {|remove:@$$functions{|extract: { private var x = 1; }|}|}
            """;

        TestFileMarkupParser.GetPositionAndSpans(
            contents, out contents, out int cursorPosition,
            out ImmutableDictionary<string, ImmutableArray<TextSpan>> namedSpans);

        var extractSpan = namedSpans["extract"].Single();
        var removeSpan = namedSpans["remove"].Single();

        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = VsLspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
        };

        var context = CreateRazorCodeActionContext(request, cursorPosition, documentPath, contents);

        var provider = new ExtractToCodeBehindCodeActionProvider(LoggerFactory);

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, DisposalToken);

        // Assert
        var codeAction = Assert.Single(commandOrCodeActionContainer);
        var razorCodeActionResolutionParams = ((JsonElement)codeAction.Data!).Deserialize<RazorCodeActionResolutionParams>();
        Assert.NotNull(razorCodeActionResolutionParams);
        var actionParams = ((JsonElement)razorCodeActionResolutionParams.Data!).Deserialize<ExtractToCodeBehindCodeActionParams>();
        Assert.NotNull(actionParams);

        Assert.Equal(removeSpan, TextSpan.FromBounds(actionParams.RemoveStart, actionParams.RemoveEnd));
        Assert.Equal(extractSpan, TextSpan.FromBounds(actionParams.ExtractStart, actionParams.ExtractEnd));
    }

    [Fact]
    public async Task Handle_NullRelativePath_ReturnsNull()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = """
            @page "/test"
            @$$code { private var x = 1; }
            """;
        TestFileMarkupParser.GetPosition(contents, out contents, out var cursorPosition);

        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = VsLspFactory.DefaultRange,
            Context = null!
        };

        var context = CreateRazorCodeActionContext(request, cursorPosition, documentPath, contents, relativePath: null);

        var provider = new ExtractToCodeBehindCodeActionProvider(LoggerFactory);

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, DisposalToken);

        // Assert
        Assert.Empty(commandOrCodeActionContainer);
    }

    private static RazorCodeActionContext CreateRazorCodeActionContext(
        VSCodeActionParams request,
        int absoluteIndex,
        string filePath,
        string text,
        string? fileKind = null,
        bool supportsFileCreation = true)
        => CreateRazorCodeActionContext(
            request, absoluteIndex, filePath, text, relativePath: filePath, fileKind, supportsFileCreation: supportsFileCreation);

    private static RazorCodeActionContext CreateRazorCodeActionContext(
        VSCodeActionParams request,
        int absoluteIndex,
        string filePath,
        string text,
        string? relativePath,
        string? fileKind = null,
        bool supportsFileCreation = true)
    {
        var source = RazorSourceDocument.Create(text, RazorSourceDocumentProperties.Create(filePath, relativePath));

        fileKind ??= FileKinds.Component;

        var codeDocument = RazorCodeDocument.Create(
            source,
            parserOptions: RazorParserOptions.Create(RazorLanguageVersion.Latest, fileKind, builder =>
            {
                builder.Directives = [ComponentCodeDirective.Directive, FunctionsDirective.Directive];
            }),
            codeGenerationOptions: RazorCodeGenerationOptions.Default.WithRootNamespace("ExtractToCodeBehindTest"));

        var syntaxTree = RazorSyntaxTree.Parse(source, codeDocument.ParserOptions);

        codeDocument.SetSyntaxTree(syntaxTree);

        var documentSnapshotMock = new StrictMock<IDocumentSnapshot>();
        documentSnapshotMock
            .Setup(x => x.GetGeneratedOutputAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(codeDocument);
        documentSnapshotMock
            .Setup(x => x.GetTextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(codeDocument.Source.Text);

        return new RazorCodeActionContext(
            request,
            documentSnapshotMock.Object,
            codeDocument,
            DelegatedDocumentUri: null,
            StartAbsoluteIndex: absoluteIndex,
            EndAbsoluteIndex: absoluteIndex,
            RazorLanguageKind.Razor,
            codeDocument.Source.Text,
            supportsFileCreation,
            SupportsCodeActionResolve: true);
    }
}
