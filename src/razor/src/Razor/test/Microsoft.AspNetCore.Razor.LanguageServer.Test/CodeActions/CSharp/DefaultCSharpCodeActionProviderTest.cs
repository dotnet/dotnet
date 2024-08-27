// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol.CodeActions;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

public class DefaultCSharpCodeActionProviderTest : LanguageServerTestBase
{
    private readonly ImmutableArray<RazorVSInternalCodeAction> _supportedCodeActions;
    private readonly ImmutableArray<RazorVSInternalCodeAction> _supportedImplicitExpressionCodeActions;

    public DefaultCSharpCodeActionProviderTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _supportedCodeActions = DefaultCSharpCodeActionProvider
            .SupportedDefaultCodeActionNames
            .Select(name => new RazorVSInternalCodeAction { Name = name })
            .ToImmutableArray();

        _supportedImplicitExpressionCodeActions = DefaultCSharpCodeActionProvider
            .SupportedImplicitExpressionCodeActionNames
            .Select(name => new RazorVSInternalCodeAction { Name = name })
            .ToImmutableArray();
    }

    [Fact]
    public async Task ProvideAsync_ValidCodeActions_ReturnsProvidedCodeAction()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = "@code { $$Path; }";
        TestFileMarkupParser.GetPosition(contents, out contents, out var cursorPosition);

        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = VsLspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
        };

        var location = new SourceLocation(cursorPosition, -1, -1);
        var context = CreateRazorCodeActionContext(request, location, documentPath, contents, new SourceSpan(8, 4));
        context.CodeDocument.SetFileKind(FileKinds.Legacy);

        var provider = new DefaultCSharpCodeActionProvider(TestLanguageServerFeatureOptions.Instance);

        // Act
        var providedCodeActions = await provider.ProvideAsync(context, _supportedCodeActions, default);

        // Assert
        Assert.Equal(_supportedCodeActions.Length, providedCodeActions.Length);
        var providedNames = providedCodeActions.Select(action => action.Name);
        var expectedNames = _supportedCodeActions.Select(action => action.Name);
        Assert.Equal(expectedNames, providedNames);
    }

    [Fact]
    public async Task ProvideAsync_SupportsCodeActionResolveFalse_ValidCodeActions_ReturnsEmpty()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = "@code { $$Path; }";
        TestFileMarkupParser.GetPosition(contents, out contents, out var cursorPosition);

        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = VsLspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
        };

        var location = new SourceLocation(cursorPosition, -1, -1);
        var context = CreateRazorCodeActionContext(request, location, documentPath, contents, new SourceSpan(8, 4), supportsCodeActionResolve: false);
        context.CodeDocument.SetFileKind(FileKinds.Legacy);

        var provider = new DefaultCSharpCodeActionProvider(TestLanguageServerFeatureOptions.Instance);

        // Act
        var providedCodeActions = await provider.ProvideAsync(context, _supportedCodeActions, default);

        // Assert
        Assert.Empty(providedCodeActions);
    }

    [Fact]
    public async Task ProvideAsync_FunctionsBlock_SingleLine_ValidCodeActions_ReturnsProvidedCodeAction()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = "@functions { $$Path; }";
        TestFileMarkupParser.GetPosition(contents, out contents, out var cursorPosition);

        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = VsLspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
        };

        var location = new SourceLocation(cursorPosition, -1, -1);
        var context = CreateRazorCodeActionContext(request, location, documentPath, contents, new SourceSpan(13, 4));
        context.CodeDocument.SetFileKind(FileKinds.Legacy);

        var provider = new DefaultCSharpCodeActionProvider(TestLanguageServerFeatureOptions.Instance);

        // Act
        var providedCodeActions = await provider.ProvideAsync(context, _supportedCodeActions, default);

        // Assert
        Assert.Equal(_supportedCodeActions.Length, providedCodeActions.Length);
        var providedNames = providedCodeActions.Select(action => action.Name);
        var expectedNames = _supportedCodeActions.Select(action => action.Name);
        Assert.Equal(expectedNames, providedNames);
    }

    [Fact]
    public async Task ProvideAsync_FunctionsBlock_OpenBraceSameLine_ValidCodeActions_ReturnsProvidedCodeAction()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = @"@functions {
$$Path;
}";
        TestFileMarkupParser.GetPosition(contents, out contents, out var cursorPosition);

        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = VsLspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
        };

        var location = new SourceLocation(cursorPosition, -1, -1);
        var context = CreateRazorCodeActionContext(request, location, documentPath, contents, new SourceSpan(13, 4));
        context.CodeDocument.SetFileKind(FileKinds.Legacy);

        var provider = new DefaultCSharpCodeActionProvider(TestLanguageServerFeatureOptions.Instance);

        // Act
        var providedCodeActions = await provider.ProvideAsync(context, _supportedCodeActions, default);

        // Assert
        Assert.Equal(_supportedCodeActions.Length, providedCodeActions.Length);
        var providedNames = providedCodeActions.Select(action => action.Name);
        var expectedNames = _supportedCodeActions.Select(action => action.Name);
        Assert.Equal(expectedNames, providedNames);
    }

    [Fact]
    public async Task ProvideAsync_FunctionsBlock_OpenBraceNextLine_ValidCodeActions_ReturnsProvidedCodeAction()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = @"@functions
{
$$Path;
}";
        TestFileMarkupParser.GetPosition(contents, out contents, out var cursorPosition);

        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = VsLspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
        };

        var location = new SourceLocation(cursorPosition, -1, -1);
        var context = CreateRazorCodeActionContext(request, location, documentPath, contents, new SourceSpan(13, 4));
        context.CodeDocument.SetFileKind(FileKinds.Legacy);

        var provider = new DefaultCSharpCodeActionProvider(TestLanguageServerFeatureOptions.Instance);

        // Act
        var providedCodeActions = await provider.ProvideAsync(context, _supportedCodeActions, default);

        // Assert
        Assert.Equal(_supportedCodeActions.Length, providedCodeActions.Length);
        var providedNames = providedCodeActions.Select(action => action.Name);
        var expectedNames = _supportedCodeActions.Select(action => action.Name);
        Assert.Equal(expectedNames, providedNames);
    }

    [Fact]
    public async Task ProvideAsync_InvalidCodeActions_ReturnsNoCodeActions()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = "@code { $$Path; }";
        TestFileMarkupParser.GetPosition(contents, out contents, out var cursorPosition);

        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = VsLspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
        };

        var location = new SourceLocation(cursorPosition, -1, -1);
        var context = CreateRazorCodeActionContext(request, location, documentPath, contents, new SourceSpan(8, 4));
        context.CodeDocument.SetFileKind(FileKinds.Legacy);

        var provider = new DefaultCSharpCodeActionProvider(TestLanguageServerFeatureOptions.Instance);

        ImmutableArray<RazorVSInternalCodeAction> codeActions =
        [
           new RazorVSInternalCodeAction()
           {
               Title = "Do something not really supported in razor",
               Name = "Non-existant name"
           }
        ];

        // Act
        var providedCodeActions = await provider.ProvideAsync(context, codeActions, default);

        // Assert
        Assert.Empty(providedCodeActions);
    }

    [Fact]
    public async Task ProvideAsync_InvalidCodeActions_ShowAllFeatureFlagOn_ReturnsCodeActions()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = "@code { $$Path; }";
        TestFileMarkupParser.GetPosition(contents, out contents, out var cursorPosition);

        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = VsLspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
        };

        var location = new SourceLocation(cursorPosition, -1, -1);
        var context = CreateRazorCodeActionContext(request, location, documentPath, contents, new SourceSpan(8, 4));
        context.CodeDocument.SetFileKind(FileKinds.Legacy);

        var options = new ConfigurableLanguageServerFeatureOptions(new[] { $"--{nameof(ConfigurableLanguageServerFeatureOptions.ShowAllCSharpCodeActions)}" });
        var provider = new DefaultCSharpCodeActionProvider(options);

        ImmutableArray<RazorVSInternalCodeAction> codeActions =
        [
           new RazorVSInternalCodeAction()
           {
               Title = "Do something not really supported in razor",
               Name = "Non-existant name"
           }
        ];

        // Act
        var providedCodeActions = await provider.ProvideAsync(context, codeActions, default);

        // Assert
        Assert.NotEmpty(providedCodeActions);
    }

    [Fact]
    public async Task ProvideAsync_ImplicitExpression_ReturnsProvidedCodeAction()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = """
                @page "/dates"

                @DateTi$$

                @code {
                    public DateTime Goo { get; set; }
                }
                """;
        TestFileMarkupParser.GetPosition(contents, out contents, out var cursorPosition);

        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = VsLspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
        };

        var location = new SourceLocation(cursorPosition, -1, -1);
        var context = CreateRazorCodeActionContext(request, location, documentPath, contents, new SourceSpan(8, 4));
        context.CodeDocument.SetFileKind(FileKinds.Legacy);

        var provider = new DefaultCSharpCodeActionProvider(TestLanguageServerFeatureOptions.Instance);

        // Act
        var providedCodeActions = await provider.ProvideAsync(context, _supportedCodeActions, default);

        // Assert
        Assert.Equal(_supportedImplicitExpressionCodeActions.Length, providedCodeActions.Length);
        var providedNames = providedCodeActions.Select(action => action.Name);
        var expectedNames = _supportedImplicitExpressionCodeActions.Select(action => action.Name);
        Assert.Equal(expectedNames, providedNames);
    }

    private static RazorCodeActionContext CreateRazorCodeActionContext(
        VSCodeActionParams request,
        SourceLocation location,
        string filePath,
        string text,
        SourceSpan componentSourceSpan,
        bool supportsFileCreation = true,
        bool supportsCodeActionResolve = true)
    {
        var tagHelpers = ImmutableArray<TagHelperDescriptor>.Empty;
        var sourceDocument = TestRazorSourceDocument.Create(text, filePath: filePath, relativePath: filePath);
        var projectEngine = RazorProjectEngine.Create(builder => builder.AddTagHelpers(tagHelpers));
        var codeDocument = projectEngine.ProcessDesignTime(sourceDocument, FileKinds.Component, importSources: default, tagHelpers);

        var csharpDocument = codeDocument.GetCSharpDocument();
        var diagnosticDescriptor = new RazorDiagnosticDescriptor("RZ10012", "diagnostic", RazorDiagnosticSeverity.Error);
        var diagnostic = RazorDiagnostic.Create(diagnosticDescriptor, componentSourceSpan);
        var csharpDocumentWithDiagnostic = new RazorCSharpDocument(codeDocument, csharpDocument.GeneratedCode, csharpDocument.Options, [diagnostic]);
        codeDocument.SetCSharpDocument(csharpDocumentWithDiagnostic);

        var documentSnapshot = Mock.Of<IDocumentSnapshot>(document =>
            document.GetGeneratedOutputAsync() == Task.FromResult(codeDocument) &&
            document.GetTextAsync() == Task.FromResult(codeDocument.Source.Text) &&
            document.Project.GetTagHelpersAsync(It.IsAny<CancellationToken>()) == new ValueTask<ImmutableArray<TagHelperDescriptor>>(tagHelpers), MockBehavior.Strict);

        var sourceText = SourceText.From(text);

        var context = new RazorCodeActionContext(request, documentSnapshot, codeDocument, location, sourceText, supportsFileCreation, supportsCodeActionResolve);

        return context;
    }
}
