// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;
using LanguageServerSR = Microsoft.AspNetCore.Razor.LanguageServer.Resources.SR;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

public class ComponentAccessibilityCodeActionProviderTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    [Fact]
    public async Task Handle_NoTagName_DoesNotProvideLightBulb()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = """
            <$$
            """;
        TestFileMarkupParser.GetPosition(contents, out contents, out var cursorPosition);

        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = new Range { Start = new Position(0, 1), End = new Position(0, 1), },
            Context = new VSInternalCodeActionContext()
        };

        var location = new SourceLocation(cursorPosition, -1, -1);
        var context = CreateRazorCodeActionContext(request, location, documentPath, contents, new SourceSpan(0, 1));

        var provider = new ComponentAccessibilityCodeActionProvider(new TagHelperFactsService());

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, default);

        // Assert
        Assert.Null(commandOrCodeActionContainer);
    }

    [Fact]
    public async Task Handle_InvalidSyntaxTree_NoStartNode()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = """
            $$
            """;
        TestFileMarkupParser.GetPosition(contents, out contents, out var cursorPosition);

        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = new Range(),
            Context = new VSInternalCodeActionContext()
        };

        var location = new SourceLocation(cursorPosition, -1, -1);
        var context = CreateRazorCodeActionContext(request, location, documentPath, contents, new SourceSpan(0, 0));
        context.CodeDocument.SetFileKind(FileKinds.Legacy);

        var provider = new ComponentAccessibilityCodeActionProvider(new TagHelperFactsService());

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, default);

        // Assert
        Assert.Null(commandOrCodeActionContainer);
    }

    [Fact]
    public async Task Handle_CursorOutsideComponent()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = """
            $$ <Component></Component>
            """;
        TestFileMarkupParser.GetPosition(contents, out contents, out var cursorPosition);

        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = new Range { Start = new Position(0, 0), End = new Position(0, 0) },
            Context = new VSInternalCodeActionContext()
        };

        var location = new SourceLocation(cursorPosition, -1, -1);
        var context = CreateRazorCodeActionContext(request, location, documentPath, contents, new SourceSpan(contents.IndexOf("Component", StringComparison.Ordinal), 9));

        var provider = new ComponentAccessibilityCodeActionProvider(new TagHelperFactsService());

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, default);

        // Assert
        Assert.Null(commandOrCodeActionContainer);
    }

    [Fact]
    public async Task Handle_ExistingComponent_SupportsFileCreationTrue_ReturnsResults()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = """
            <$$Component></Component>
            """;
        TestFileMarkupParser.GetPosition(contents, out contents, out var cursorPosition);

        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = new Range { Start = new Position(0, 0), End = new Position(0, 0) },
            Context = new VSInternalCodeActionContext()
        };

        var location = new SourceLocation(cursorPosition, -1, -1);
        var context = CreateRazorCodeActionContext(request, location, documentPath, contents, new SourceSpan(contents.IndexOf("Component", StringComparison.Ordinal), 9), supportsFileCreation: true);

        var provider = new ComponentAccessibilityCodeActionProvider(new TagHelperFactsService());

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, default);

        // Assert
        Assert.NotNull(commandOrCodeActionContainer);
        Assert.Collection(commandOrCodeActionContainer,
            e =>
            {
                Assert.Equal("@using Fully.Qualified", e.Title);
                Assert.NotNull(e.Data);
                Assert.Null(e.Edit);
            },
            e =>
            {
                Assert.Equal("Fully.Qualified.Component", e.Title);
                Assert.NotNull(e.Edit);
                Assert.NotNull(e.Edit.DocumentChanges);
                Assert.Null(e.Data);
            },
            e =>
            {
                Assert.Equal(LanguageServerSR.Create_Component_FromTag_Title, e.Title);
                Assert.NotNull(e.Data);
                Assert.Null(e.Edit);
            });
    }

    [Fact]
    public async Task Handle_ExistingComponent_CaseIncorrect_ReturnsResults()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = """
            <$$CompOnent></CompOnent>
            """;
        TestFileMarkupParser.GetPosition(contents, out contents, out var cursorPosition);

        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = new Range { Start = new Position(0, 0), End = new Position(0, 0) },
            Context = new VSInternalCodeActionContext()
        };

        var location = new SourceLocation(cursorPosition, -1, -1);
        var context = CreateRazorCodeActionContext(request, location, documentPath, contents, new SourceSpan(contents.IndexOf("CompOnent", StringComparison.Ordinal), 9), supportsFileCreation: true);

        var provider = new ComponentAccessibilityCodeActionProvider(new TagHelperFactsService());

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, default);

        // Assert
        Assert.NotNull(commandOrCodeActionContainer);
        Assert.Collection(commandOrCodeActionContainer,
            e =>
            {
                Assert.Equal("Component - @using Fully.Qualified", e.Title);
                Assert.NotNull(e.Data);
                Assert.Null(e.Edit);
            },
            e =>
            {
                Assert.Equal("Fully.Qualified.Component", e.Title);
                Assert.NotNull(e.Edit);
                Assert.NotNull(e.Edit.DocumentChanges);
                Assert.Null(e.Data);
            },
            e =>
            {
                Assert.Equal(LanguageServerSR.Create_Component_FromTag_Title, e.Title);
                Assert.NotNull(e.Data);
                Assert.Null(e.Edit);
            });
    }

    [Fact]
    public async Task Handle_ExistingComponent_CaseIncorrect_WithUsing_ReturnsResults()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = """
            @using Fully.Qualified

            <$$CompOnent></CompOnent>
            """;
        TestFileMarkupParser.GetPosition(contents, out contents, out var cursorPosition);

        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = new Range { Start = new Position(0, 0), End = new Position(0, 0) },
            Context = new VSInternalCodeActionContext()
        };

        var location = new SourceLocation(cursorPosition, -1, -1);
        var context = CreateRazorCodeActionContext(request, location, documentPath, contents, new SourceSpan(contents.IndexOf("CompOnent", StringComparison.Ordinal), 9), supportsFileCreation: true);

        var provider = new ComponentAccessibilityCodeActionProvider(new TagHelperFactsService());

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, default);

        // Assert
        Assert.NotNull(commandOrCodeActionContainer);
        Assert.Collection(commandOrCodeActionContainer,
            e =>
            {
                Assert.Equal("Component", e.Title);
                Assert.NotNull(e.Edit);
                Assert.NotNull(e.Edit.DocumentChanges);
                Assert.Null(e.Data);
            },
            e =>
            {
                Assert.Equal(LanguageServerSR.Create_Component_FromTag_Title, e.Title);
                Assert.NotNull(e.Data);
                Assert.Null(e.Edit);
            });
    }

    [Fact]
    public async Task Handle_ExistingGenericComponent_SupportsFileCreationTrue_ReturnsResults()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = """
            <$$GenericComponent></GenericComponent>
            """;
        TestFileMarkupParser.GetPosition(contents, out contents, out var cursorPosition);

        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = new Range { Start = new Position(0, 0), End = new Position(0, 0) },
            Context = new VSInternalCodeActionContext()
        };

        var location = new SourceLocation(cursorPosition, -1, -1);
        var context = CreateRazorCodeActionContext(request, location, documentPath, contents, new SourceSpan(contents.IndexOf("GenericComponent", StringComparison.Ordinal), "GenericComponent".Length), supportsFileCreation: true);

        var provider = new ComponentAccessibilityCodeActionProvider(new TagHelperFactsService());

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, default);

        // Assert
        Assert.NotNull(commandOrCodeActionContainer);
        Assert.Collection(commandOrCodeActionContainer,
            e =>
            {
                Assert.Equal("@using Fully.Qualified", e.Title);
                Assert.NotNull(e.Data);
                Assert.Null(e.Edit);
            },
            e =>
            {
                Assert.Equal("Fully.Qualified.GenericComponent", e.Title);
                Assert.NotNull(e.Edit);
                Assert.NotNull(e.Edit.DocumentChanges);
                Assert.Null(e.Data);
            },
            e =>
            {
                Assert.Equal(LanguageServerSR.Create_Component_FromTag_Title, e.Title);
                Assert.NotNull(e.Data);
                Assert.Null(e.Edit);
            });
    }

    [Fact]
    public async Task Handle_NewComponent_SupportsFileCreationTrue_ReturnsResult()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = """
            <$$NewComponent></NewComponent>
            """;
        TestFileMarkupParser.GetPosition(contents, out contents, out var cursorPosition);

        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = new Range { Start = new Position(0, 0), End = new Position(0, 0) },
            Context = new VSInternalCodeActionContext()
        };

        var location = new SourceLocation(cursorPosition, -1, -1);
        var context = CreateRazorCodeActionContext(request, location, documentPath, contents, new SourceSpan(contents.IndexOf("Component", StringComparison.Ordinal), 9), supportsFileCreation: true);

        var provider = new ComponentAccessibilityCodeActionProvider(new TagHelperFactsService());

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, default);

        // Assert
        Assert.NotNull(commandOrCodeActionContainer);
        var command = Assert.Single(commandOrCodeActionContainer);
        Assert.Equal(LanguageServerSR.Create_Component_FromTag_Title, command.Title);
        Assert.NotNull(command.Data);
    }

    [Fact]
    public async Task Handle_NewComponent_CaretInAttribute_ReturnsResult()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = """
            <NewComponent checked $$goo="blah"></NewComponent>
            """;
        TestFileMarkupParser.GetPosition(contents, out contents, out var cursorPosition);

        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = new Range { Start = new Position(0, 0), End = new Position(0, 0) },
            Context = new VSInternalCodeActionContext()
        };

        var location = new SourceLocation(cursorPosition, -1, -1);
        var context = CreateRazorCodeActionContext(request, location, documentPath, contents, new SourceSpan(contents.IndexOf("Component", StringComparison.Ordinal), 9), supportsFileCreation: true);

        var provider = new ComponentAccessibilityCodeActionProvider(new TagHelperFactsService());

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, default);

        // Assert
        Assert.NotNull(commandOrCodeActionContainer);
        var command = Assert.Single(commandOrCodeActionContainer);
        Assert.Equal(LanguageServerSR.Create_Component_FromTag_Title, command.Title);
        Assert.NotNull(command.Data);
    }

    [Fact]
    public async Task Handle_NewComponent_SupportsFileCreationFalse_ReturnsEmpty()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = """
            <$$NewComponent></NewComponent>
            """;
        TestFileMarkupParser.GetPosition(contents, out contents, out var cursorPosition);

        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = new Range { Start = new Position(0, 0), End = new Position(0, 0) },
            Context = new VSInternalCodeActionContext()
        };

        var location = new SourceLocation(cursorPosition, -1, -1);
        var context = CreateRazorCodeActionContext(request, location, documentPath, contents, new SourceSpan(contents.IndexOf("Component", StringComparison.Ordinal), 9), supportsFileCreation: false);

        var provider = new ComponentAccessibilityCodeActionProvider(new TagHelperFactsService());

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, default);

        // Assert
        Assert.NotNull(commandOrCodeActionContainer);
        Assert.Empty(commandOrCodeActionContainer);
    }

    [Fact]
    public async Task Handle_ExistingComponent_SupportsFileCreationFalse_ReturnsResults()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = """
            <$$Component></Component>
            """;
        TestFileMarkupParser.GetPosition(contents, out contents, out var cursorPosition);

        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = new Range { Start = new Position(0, 0), End = new Position(0, 0) },
            Context = new VSInternalCodeActionContext()
        };

        var location = new SourceLocation(cursorPosition, -1, -1);
        var context = CreateRazorCodeActionContext(request, location, documentPath, contents, new SourceSpan(contents.IndexOf("Component", StringComparison.Ordinal), 9), supportsFileCreation: false);

        var provider = new ComponentAccessibilityCodeActionProvider(new TagHelperFactsService());

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, default);

        // Assert
        Assert.NotNull(commandOrCodeActionContainer);
        Assert.Collection(commandOrCodeActionContainer,
            e =>
            {
                Assert.Equal("@using Fully.Qualified", e.Title);
                Assert.NotNull(e.Data);
                Assert.Null(e.Edit);
            },
            e =>
            {
                Assert.Equal("Fully.Qualified.Component", e.Title);
                Assert.NotNull(e.Edit);
                Assert.NotNull(e.Edit.DocumentChanges);
                Assert.Null(e.Data);
            });
    }

    private static RazorCodeActionContext CreateRazorCodeActionContext(VSCodeActionParams request, SourceLocation location, string filePath, string text, SourceSpan componentSourceSpan, bool supportsFileCreation = true)
    {
        var shortComponent = TagHelperDescriptorBuilder.Create(ComponentMetadata.Component.TagHelperKind, "Fully.Qualified.Component", "TestAssembly");
        shortComponent.CaseSensitive = true;
        shortComponent.TagMatchingRule(rule => rule.TagName = "Component");
        var fullyQualifiedComponent = TagHelperDescriptorBuilder.Create(ComponentMetadata.Component.TagHelperKind, "Fully.Qualified.Component", "TestAssembly");
        fullyQualifiedComponent.CaseSensitive = true;
        fullyQualifiedComponent.TagMatchingRule(rule => rule.TagName = "Fully.Qualified.Component");

        var shortGenericComponent = TagHelperDescriptorBuilder.Create(ComponentMetadata.Component.TagHelperKind, "Fully.Qualified.GenericComponent<T>", "TestAssembly");
        shortGenericComponent.CaseSensitive = true;
        shortGenericComponent.TagMatchingRule(rule => rule.TagName = "GenericComponent");
        var fullyQualifiedGenericComponent = TagHelperDescriptorBuilder.Create(ComponentMetadata.Component.TagHelperKind, "Fully.Qualified.GenericComponent<T>", "TestAssembly");
        fullyQualifiedGenericComponent.CaseSensitive = true;
        fullyQualifiedGenericComponent.TagMatchingRule(rule => rule.TagName = "Fully.Qualified.GenericComponent");

        var tagHelpers = ImmutableArray.Create(shortComponent.Build(), fullyQualifiedComponent.Build(), shortGenericComponent.Build(), fullyQualifiedGenericComponent.Build());

        var sourceDocument = TestRazorSourceDocument.Create(text, filePath: filePath, relativePath: filePath);
        var projectEngine = RazorProjectEngine.Create(builder => builder.AddTagHelpers(tagHelpers));
        var codeDocument = projectEngine.ProcessDesignTime(sourceDocument, FileKinds.Component, importSources: default, tagHelpers);

        var cSharpDocument = codeDocument.GetCSharpDocument();
        var diagnosticDescriptor = new RazorDiagnosticDescriptor("RZ10012", "diagnostic", RazorDiagnosticSeverity.Error);
        var diagnostic = RazorDiagnostic.Create(diagnosticDescriptor, componentSourceSpan);
        var cSharpDocumentWithDiagnostic = RazorCSharpDocument.Create(codeDocument, cSharpDocument.GeneratedCode, cSharpDocument.Options, new[] { diagnostic });
        codeDocument.SetCSharpDocument(cSharpDocumentWithDiagnostic);

        var documentSnapshot = Mock.Of<IDocumentSnapshot>(document =>
            document.GetGeneratedOutputAsync() == Task.FromResult(codeDocument) &&
            document.GetTextAsync() == Task.FromResult(codeDocument.GetSourceText()) &&
            document.Project.TagHelpers == tagHelpers, MockBehavior.Strict);

        var sourceText = SourceText.From(text);

        var context = new RazorCodeActionContext(request, documentSnapshot, codeDocument, location, sourceText, supportsFileCreation, supportsCodeActionResolve: true);

        return context;
    }
}
