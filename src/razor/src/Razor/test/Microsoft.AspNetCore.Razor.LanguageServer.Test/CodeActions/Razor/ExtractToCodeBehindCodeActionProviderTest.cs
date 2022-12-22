// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

public class ExtractToCodeBehindCodeActionProviderTest : LanguageServerTestBase
{
    public ExtractToCodeBehindCodeActionProviderTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public async Task Handle_InvalidFileKind()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = "@page \"/test\"\n@code {}";
        var request = new CodeActionParams()
        {
            TextDocument = new TextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = new Range(),
        };

        var location = new SourceLocation(contents.IndexOf("code", StringComparison.Ordinal), -1, -1);
        var context = CreateRazorCodeActionContext(request, location, documentPath, contents);
        context.CodeDocument.SetFileKind(FileKinds.Legacy);

        var provider = new ExtractToCodeBehindCodeActionProvider();

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, default);

        // Assert
        Assert.Null(commandOrCodeActionContainer);
    }

    [Fact]
    public async Task Handle_OutsideCodeDirective()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = "@page \"/test\"\n@code {}";
        var request = new CodeActionParams()
        {
            TextDocument = new TextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = new Range(),
        };

        var location = new SourceLocation(contents.IndexOf("test", StringComparison.Ordinal), -1, -1);
        var context = CreateRazorCodeActionContext(request, location, documentPath, contents);

        var provider = new ExtractToCodeBehindCodeActionProvider();

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, default);

        // Assert
        Assert.Null(commandOrCodeActionContainer);
    }

    [Fact]
    public async Task Handle_InCodeDirectiveBlock_ReturnsNull()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = "@page \"/test\"\n@code {}";
        var request = new CodeActionParams()
        {
            TextDocument = new TextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = new Range(),
        };

        var location = new SourceLocation(contents.IndexOf("code", StringComparison.Ordinal) + 6, -1, -1);
        var context = CreateRazorCodeActionContext(request, location, documentPath, contents);

        var provider = new ExtractToCodeBehindCodeActionProvider();

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, default);

        // Assert
        Assert.Null(commandOrCodeActionContainer);
    }

    [Fact]
    public async Task Handle_InCodeDirectiveMalformed_ReturnsNull()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = "@page \"/test\"\n@code";
        var request = new CodeActionParams()
        {
            TextDocument = new TextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = new Range(),
        };

        var location = new SourceLocation(contents.IndexOf("code", StringComparison.Ordinal), -1, -1);
        var context = CreateRazorCodeActionContext(request, location, documentPath, contents);

        var provider = new ExtractToCodeBehindCodeActionProvider();

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, default);

        // Assert
        Assert.Null(commandOrCodeActionContainer);
    }

    [Fact]
    public async Task Handle_InCodeDirectiveWithMarkup_ReturnsNull()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = "@page \"/test\"\n@code { void Test() { <h1>Hello, world!</h1> } }";
        var request = new CodeActionParams()
        {
            TextDocument = new TextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = new Range(),
        };

        var location = new SourceLocation(contents.IndexOf("code", StringComparison.Ordinal), -1, -1);
        var context = CreateRazorCodeActionContext(request, location, documentPath, contents);

        var provider = new ExtractToCodeBehindCodeActionProvider();

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, default);

        // Assert
        Assert.Null(commandOrCodeActionContainer);
    }

    [Fact]
    public async Task Handle_InCodeDirective_SupportsFileCreationTrue_ReturnsResult()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = "@page \"/test\"\n@code { private var x = 1; }";
        var request = new CodeActionParams()
        {
            TextDocument = new TextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = new Range(),
        };

        var location = new SourceLocation(contents.IndexOf("code", StringComparison.Ordinal), -1, -1);
        var context = CreateRazorCodeActionContext(request, location, documentPath, contents, supportsFileCreation: true);

        var provider = new ExtractToCodeBehindCodeActionProvider();

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, default);

        // Assert
        Assert.NotNull(commandOrCodeActionContainer);
        var codeAction = Assert.Single(commandOrCodeActionContainer);
        var razorCodeActionResolutionParams = ((JObject)codeAction.Data!).ToObject<RazorCodeActionResolutionParams>();
        Assert.NotNull(razorCodeActionResolutionParams);
        var actionParams = ((JObject)razorCodeActionResolutionParams.Data).ToObject<ExtractToCodeBehindCodeActionParams>();
        Assert.NotNull(actionParams);
        Assert.Equal(14, actionParams.RemoveStart);
        Assert.Equal(19, actionParams.ExtractStart);
        Assert.Equal(42, actionParams.ExtractEnd);
        Assert.Equal(42, actionParams.RemoveEnd);
    }

    [Fact]
    public async Task Handle_InCodeDirective_SupportsFileCreationFalse_ReturnsNull()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = "@page \"/test\"\n@code { private var x = 1; }";
        var request = new CodeActionParams()
        {
            TextDocument = new TextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = new Range(),
        };

        var location = new SourceLocation(contents.IndexOf("code", StringComparison.Ordinal), -1, -1);
        var context = CreateRazorCodeActionContext(request, location, documentPath, contents, supportsFileCreation: false);

        var provider = new ExtractToCodeBehindCodeActionProvider();

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, default);

        // Assert
        Assert.Null(commandOrCodeActionContainer);
    }

    [Fact]
    public async Task Handle_InFunctionsDirective_SupportsFileCreationTrue_ReturnsResult()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = "@page \"/test\"\n@functions { private var x = 1; }";
        var request = new CodeActionParams()
        {
            TextDocument = new TextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = new Range(),
        };

        var location = new SourceLocation(contents.IndexOf("functions", StringComparison.Ordinal), -1, -1);
        var context = CreateRazorCodeActionContext(request, location, documentPath, contents);

        var provider = new ExtractToCodeBehindCodeActionProvider();

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, default);

        // Assert
        Assert.NotNull(commandOrCodeActionContainer);
        var codeAction = Assert.Single(commandOrCodeActionContainer);
        var razorCodeActionResolutionParams = ((JObject)codeAction.Data!).ToObject<RazorCodeActionResolutionParams>();
        Assert.NotNull(razorCodeActionResolutionParams);
        var actionParams = ((JObject)razorCodeActionResolutionParams.Data).ToObject<ExtractToCodeBehindCodeActionParams>();
        Assert.NotNull(actionParams);
        Assert.Equal(14, actionParams.RemoveStart);
        Assert.Equal(24, actionParams.ExtractStart);
        Assert.Equal(47, actionParams.ExtractEnd);
        Assert.Equal(47, actionParams.RemoveEnd);
    }

    [Fact]
    public async Task Handle_NullRelativePath_ReturnsNull()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = "@page \"/test\"\n@code { private var x = 1; }";
        var request = new CodeActionParams()
        {
            TextDocument = new TextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = new Range(),
        };

        var location = new SourceLocation(contents.IndexOf("code", StringComparison.Ordinal), -1, -1);
        var context = CreateRazorCodeActionContext(request, location, documentPath, contents, relativePath: null);

        var provider = new ExtractToCodeBehindCodeActionProvider();

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, default);

        // Assert
        Assert.Null(commandOrCodeActionContainer);
    }

    private static RazorCodeActionContext CreateRazorCodeActionContext(CodeActionParams request, SourceLocation location, string filePath, string text, bool supportsFileCreation = true)
        => CreateRazorCodeActionContext(request, location, filePath, text, relativePath: filePath, supportsFileCreation: supportsFileCreation);

    private static RazorCodeActionContext CreateRazorCodeActionContext(CodeActionParams request, SourceLocation location, string filePath, string text, string? relativePath, bool supportsFileCreation = true)
    {
        var sourceDocument = RazorSourceDocument.Create(text, new RazorSourceDocumentProperties(filePath, relativePath));
        var options = RazorParserOptions.Create(o =>
        {
            o.Directives.Add(ComponentCodeDirective.Directive);
            o.Directives.Add(FunctionsDirective.Directive);
        });
        var syntaxTree = RazorSyntaxTree.Parse(sourceDocument, options);

        var codeDocument = TestRazorCodeDocument.Create(sourceDocument, Array.Empty<RazorSourceDocument>());
        codeDocument.SetFileKind(FileKinds.Component);
        codeDocument.SetCodeGenerationOptions(RazorCodeGenerationOptions.Create(o =>
        {
            o.RootNamespace = "ExtractToCodeBehindTest";
        }));
        codeDocument.SetSyntaxTree(syntaxTree);

        var documentSnapshot = Mock.Of<DocumentSnapshot>(document =>
            document.GetGeneratedOutputAsync() == Task.FromResult(codeDocument) &&
            document.GetTextAsync() == Task.FromResult(codeDocument.GetSourceText()), MockBehavior.Strict);

        var sourceText = SourceText.From(text);

        var context = new RazorCodeActionContext(request, documentSnapshot, codeDocument, location, sourceText, supportsFileCreation, supportsCodeActionResolve: true);

        return context;
    }
}
