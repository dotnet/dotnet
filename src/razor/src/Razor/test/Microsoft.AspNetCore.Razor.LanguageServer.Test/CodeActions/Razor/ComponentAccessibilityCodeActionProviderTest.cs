// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis.Razor.CodeActions;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.CodeActions;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Testing;
using Moq;
using Xunit;
using Xunit.Abstractions;

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
            TextDocument = new VSTextDocumentIdentifier { DocumentUri = new(new Uri(documentPath)) },
            Range = LspFactory.CreateZeroWidthRange(0, 1),
            Context = new VSInternalCodeActionContext()
        };

        var context = CreateRazorCodeActionContext(request, cursorPosition, documentPath, contents, new SourceSpan(0, 1));

        var provider = new ComponentAccessibilityCodeActionProvider(new FileSystem());

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, DisposalToken);

        // Assert
        Assert.Empty(commandOrCodeActionContainer);
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
            TextDocument = new VSTextDocumentIdentifier { DocumentUri = new(new Uri(documentPath)) },
            Range = LspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
        };

        var context = CreateRazorCodeActionContext(
            request,
            cursorPosition,
            documentPath,
            contents,
            new SourceSpan(0, 0),
            fileKind: RazorFileKind.Legacy);

        var provider = new ComponentAccessibilityCodeActionProvider(new FileSystem());

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, DisposalToken);

        // Assert
        Assert.Empty(commandOrCodeActionContainer);
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
            TextDocument = new VSTextDocumentIdentifier { DocumentUri = new(new Uri(documentPath)) },
            Range = LspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
        };

        var context = CreateRazorCodeActionContext(request, cursorPosition, documentPath, contents, new SourceSpan(contents.IndexOf("Component", StringComparison.Ordinal), 9));

        var provider = new ComponentAccessibilityCodeActionProvider(new FileSystem());

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, DisposalToken);

        // Assert
        Assert.Empty(commandOrCodeActionContainer);
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
            TextDocument = new VSTextDocumentIdentifier { DocumentUri = new(new Uri(documentPath)) },
            Range = LspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
        };

        var context = CreateRazorCodeActionContext(request, cursorPosition, documentPath, contents, new SourceSpan(contents.IndexOf("Component", StringComparison.Ordinal), 9), supportsFileCreation: true);

        var provider = new ComponentAccessibilityCodeActionProvider(new FileSystem());

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, DisposalToken);

        // Assert
        Assert.Collection(commandOrCodeActionContainer,
            e =>
            {
                Assert.Equal(LanguageServerConstants.CodeActions.AddUsing, e.Name);
                Assert.NotNull(e.Data);
                Assert.Null(e.Edit);
            },
            e =>
            {
                Assert.Equal(LanguageServerConstants.CodeActions.FullyQualify, e.Name);
                Assert.NotNull(e.Edit);
                Assert.NotNull(e.Edit.DocumentChanges);
                Assert.Null(e.Data);
            },
            e =>
            {
                Assert.Equal(LanguageServerConstants.CodeActions.CreateComponentFromTag, e.Name);
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
            TextDocument = new VSTextDocumentIdentifier { DocumentUri = new(new Uri(documentPath)) },
            Range = LspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
        };

        var context = CreateRazorCodeActionContext(request, cursorPosition, documentPath, contents, new SourceSpan(contents.IndexOf("CompOnent", StringComparison.Ordinal), 9), supportsFileCreation: true);

        var provider = new ComponentAccessibilityCodeActionProvider(new FileSystem());

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, DisposalToken);

        // Assert
        Assert.Collection(commandOrCodeActionContainer,
            e =>
            {
                Assert.Equal(LanguageServerConstants.CodeActions.AddUsing, e.Name);
                Assert.NotNull(e.Data);
                Assert.Null(e.Edit);
            },
            e =>
            {
                Assert.Equal(LanguageServerConstants.CodeActions.FullyQualify, e.Name);
                Assert.NotNull(e.Edit);
                Assert.NotNull(e.Edit.DocumentChanges);
                Assert.Null(e.Data);
            },
            e =>
            {
                Assert.Equal(LanguageServerConstants.CodeActions.CreateComponentFromTag, e.Name);
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
            TextDocument = new VSTextDocumentIdentifier { DocumentUri = new(new Uri(documentPath)) },
            Range = LspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
        };

        var context = CreateRazorCodeActionContext(request, cursorPosition, documentPath, contents, new SourceSpan(contents.IndexOf("CompOnent", StringComparison.Ordinal), 9), supportsFileCreation: true);

        var provider = new ComponentAccessibilityCodeActionProvider(new FileSystem());

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, DisposalToken);

        // Assert
        Assert.Collection(commandOrCodeActionContainer,
            e =>
            {
                Assert.Equal(LanguageServerConstants.CodeActions.FullyQualify, e.Name);
                Assert.NotNull(e.Edit);
                Assert.NotNull(e.Edit.DocumentChanges);
                Assert.Null(e.Data);
            },
            e =>
            {
                Assert.Equal(LanguageServerConstants.CodeActions.CreateComponentFromTag, e.Name);
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
            TextDocument = new VSTextDocumentIdentifier { DocumentUri = new(new Uri(documentPath)) },
            Range = LspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
        };

        var context = CreateRazorCodeActionContext(request, cursorPosition, documentPath, contents, new SourceSpan(contents.IndexOf("GenericComponent", StringComparison.Ordinal), "GenericComponent".Length), supportsFileCreation: true);

        var provider = new ComponentAccessibilityCodeActionProvider(new FileSystem());

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, DisposalToken);

        // Assert
        Assert.Collection(commandOrCodeActionContainer,
            e =>
            {
                Assert.Equal(LanguageServerConstants.CodeActions.AddUsing, e.Name);
                Assert.NotNull(e.Data);
                Assert.Null(e.Edit);
            },
            e =>
            {
                Assert.Equal(LanguageServerConstants.CodeActions.FullyQualify, e.Name);
                Assert.NotNull(e.Edit);
                Assert.NotNull(e.Edit.DocumentChanges);
                Assert.Null(e.Data);
            },
            e =>
            {
                Assert.Equal(LanguageServerConstants.CodeActions.CreateComponentFromTag, e.Name);
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
            TextDocument = new VSTextDocumentIdentifier { DocumentUri = new(new Uri(documentPath)) },
            Range = LspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
        };

        var context = CreateRazorCodeActionContext(request, cursorPosition, documentPath, contents, new SourceSpan(contents.IndexOf("Component", StringComparison.Ordinal), 9), supportsFileCreation: true);

        var provider = new ComponentAccessibilityCodeActionProvider(new FileSystem());

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, DisposalToken);

        // Assert
        var command = Assert.Single(commandOrCodeActionContainer);
        Assert.Equal(LanguageServerConstants.CodeActions.CreateComponentFromTag, command.Name);
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
            TextDocument = new VSTextDocumentIdentifier { DocumentUri = new(new Uri(documentPath)) },
            Range = LspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
        };

        var context = CreateRazorCodeActionContext(request, cursorPosition, documentPath, contents, new SourceSpan(contents.IndexOf("Component", StringComparison.Ordinal), 9), supportsFileCreation: true);

        var provider = new ComponentAccessibilityCodeActionProvider(new FileSystem());

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, DisposalToken);

        // Assert
        var command = Assert.Single(commandOrCodeActionContainer);
        Assert.Equal(LanguageServerConstants.CodeActions.CreateComponentFromTag, command.Name);
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
            TextDocument = new VSTextDocumentIdentifier { DocumentUri = new(new Uri(documentPath)) },
            Range = LspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
        };

        var context = CreateRazorCodeActionContext(request, cursorPosition, documentPath, contents, new SourceSpan(contents.IndexOf("Component", StringComparison.Ordinal), 9), supportsFileCreation: false);

        var provider = new ComponentAccessibilityCodeActionProvider(new FileSystem());

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, DisposalToken);

        // Assert
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
            TextDocument = new VSTextDocumentIdentifier { DocumentUri = new(new Uri(documentPath)) },
            Range = LspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
        };

        var context = CreateRazorCodeActionContext(request, cursorPosition, documentPath, contents, new SourceSpan(contents.IndexOf("Component", StringComparison.Ordinal), 9), supportsFileCreation: false);

        var provider = new ComponentAccessibilityCodeActionProvider(new FileSystem());

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, DisposalToken);

        // Assert
        Assert.Collection(commandOrCodeActionContainer,
            e =>
            {
                Assert.Equal(LanguageServerConstants.CodeActions.AddUsing, e.Name);
                Assert.NotNull(e.Data);
                Assert.Null(e.Edit);
            },
            e =>
            {
                Assert.Equal(LanguageServerConstants.CodeActions.FullyQualify, e.Name);
                Assert.NotNull(e.Edit);
                Assert.NotNull(e.Edit.DocumentChanges);
                Assert.Null(e.Data);
            });
    }

    private static RazorCodeActionContext CreateRazorCodeActionContext(
        VSCodeActionParams request,
        int absoluteIndex,
        string filePath,
        string text,
        SourceSpan componentSourceSpan,
        RazorFileKind? fileKind = null,
        bool supportsFileCreation = true)
    {
        var shortComponent = TagHelperDescriptorBuilder.CreateComponent("Fully.Qualified.Component", "TestAssembly");
        shortComponent.SetTypeName(
            fullName: "Fully.Qualified.Component",
            typeNamespace: "Fully.Qualified",
            typeNameIdentifier: "Component");
        shortComponent.CaseSensitive = true;
        shortComponent.TagMatchingRule(rule => rule.TagName = "Component");

        var fullyQualifiedComponent = TagHelperDescriptorBuilder.CreateComponent("Fully.Qualified.Component", "TestAssembly");
        fullyQualifiedComponent.SetTypeName(
            fullName: "Fully.Qualified.Component",
            typeNamespace: "Fully.Qualified",
            typeNameIdentifier: "Component");
        fullyQualifiedComponent.CaseSensitive = true;
        fullyQualifiedComponent.TagMatchingRule(rule => rule.TagName = "Fully.Qualified.Component");

        var shortGenericComponent = TagHelperDescriptorBuilder.CreateComponent("Fully.Qualified.GenericComponent<T>", "TestAssembly");
        shortGenericComponent.SetTypeName(
            fullName: "Fully.Qualified.GenericComponent<T>",
            typeNamespace: "Fully.Qualified",
            typeNameIdentifier: "GenericComponent");
        shortGenericComponent.CaseSensitive = true;
        shortGenericComponent.TagMatchingRule(rule => rule.TagName = "GenericComponent");

        var fullyQualifiedGenericComponent = TagHelperDescriptorBuilder.CreateComponent("Fully.Qualified.GenericComponent<T>", "TestAssembly");
        fullyQualifiedGenericComponent.SetTypeName(
            fullName: "Fully.Qualified.GenericComponent<T>",
            typeNamespace: "Fully.Qualified",
            typeNameIdentifier: "GenericComponent");
        fullyQualifiedGenericComponent.CaseSensitive = true;
        fullyQualifiedGenericComponent.TagMatchingRule(rule => rule.TagName = "Fully.Qualified.GenericComponent");

        TagHelperCollection tagHelpers = [shortComponent.Build(), fullyQualifiedComponent.Build(), shortGenericComponent.Build(), fullyQualifiedGenericComponent.Build()];

        var sourceDocument = TestRazorSourceDocument.Create(text, filePath: filePath, relativePath: filePath);
        var projectEngine = RazorProjectEngine.Create(builder =>
        {
            builder.SetTagHelpers(tagHelpers);

            builder.ConfigureParserOptions(builder =>
            {
                builder.UseRoslynTokenizer = true;
            });
        });

        var fileKindValue = fileKind ?? RazorFileKind.Component;

        var codeDocument = projectEngine.Process(sourceDocument, fileKindValue, importSources: default, tagHelpers);

        var csharpDocument = codeDocument.GetRequiredCSharpDocument();
        var diagnosticDescriptor = new RazorDiagnosticDescriptor("RZ10012", "diagnostic", RazorDiagnosticSeverity.Error);
        var diagnostic = RazorDiagnostic.Create(diagnosticDescriptor, componentSourceSpan);
        var csharpDocumentWithDiagnostic = new RazorCSharpDocument(codeDocument, csharpDocument.Text, [diagnostic]);
        codeDocument.SetCSharpDocument(csharpDocumentWithDiagnostic);

        var documentSnapshotMock = new StrictMock<IDocumentSnapshot>();
        documentSnapshotMock
            .Setup(x => x.GetGeneratedOutputAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(codeDocument);
        documentSnapshotMock
            .Setup(x => x.GetTextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(codeDocument.Source.Text);
        documentSnapshotMock
            .Setup(x => x.Project.GetTagHelpersAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(tagHelpers);

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
