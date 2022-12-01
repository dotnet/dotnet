// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LanguageServer.ContainedLanguage;

public class DefaultFallbackCapabilitiesFilterResolverTest : TestBase
{
    private static readonly DefaultFallbackCapabilitiesFilterResolver s_resolver = new();

    public DefaultFallbackCapabilitiesFilterResolverTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public void Resolve_Implementation_ReturnsTrue()
    {
        // Arrange
        var methodName = Methods.TextDocumentImplementationName;
        var capabilities = new ServerCapabilities()
        {
            ImplementationProvider = true,
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Resolve_ImplementationOptions_ReturnsTrue()
    {
        // Arrange
        var methodName = Methods.TextDocumentImplementationName;
        var capabilities = new ServerCapabilities()
        {
            ImplementationProvider = new ImplementationOptions(),
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Resolve_TypeDefinition_ReturnsTrue()
    {
        // Arrange
        var methodName = Methods.TextDocumentTypeDefinitionName;
        var capabilities = new ServerCapabilities()
        {
            TypeDefinitionProvider = true,
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Resolve_TypeDefinitionOptions_ReturnsTrue()
    {
        // Arrange
        var methodName = Methods.TextDocumentTypeDefinitionName;
        var capabilities = new ServerCapabilities()
        {
            TypeDefinitionProvider = new TypeDefinitionOptions(),
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Resolve_Reference_ReturnsTrue()
    {
        // Arrange
        var methodName = Methods.TextDocumentReferencesName;
        var capabilities = new ServerCapabilities()
        {
            ReferencesProvider = true,
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Resolve_ReferenceOptions_ReturnsTrue()
    {
        // Arrange
        var methodName = Methods.TextDocumentReferencesName;
        var capabilities = new ServerCapabilities()
        {
            ReferencesProvider = new ReferenceOptions(),
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Resolve_Rename_ReturnsTrue()
    {
        // Arrange
        var methodName = Methods.TextDocumentRenameName;
        var capabilities = new ServerCapabilities()
        {
            RenameProvider = true,
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Resolve_RenameOptions_ReturnsTrue()
    {
        // Arrange
        var methodName = Methods.TextDocumentRenameName;
        var capabilities = new ServerCapabilities()
        {
            RenameProvider = new RenameOptions(),
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Resolve_SignatureHelp_ReturnsTrue()
    {
        // Arrange
        var methodName = Methods.TextDocumentSignatureHelpName;
        var capabilities = new ServerCapabilities()
        {
            SignatureHelpProvider = new SignatureHelpOptions(),
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Resolve_WillSave_ReturnsTrue()
    {
        // Arrange
        var methodName = Methods.TextDocumentWillSaveName;
        var capabilities = new ServerCapabilities()
        {
            TextDocumentSync = new TextDocumentSyncOptions()
            {
                WillSave = true,
            },
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Resolve_WillSaveWaitUntil_ReturnsTrue()
    {
        // Arrange
        var methodName = Methods.TextDocumentWillSaveWaitUntilName;
        var capabilities = new ServerCapabilities()
        {
            TextDocumentSync = new TextDocumentSyncOptions()
            {
                WillSaveWaitUntil = true,
            },
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Resolve_RangeFormatting_ReturnsTrue()
    {
        // Arrange
        var methodName = Methods.TextDocumentRangeFormattingName;
        var capabilities = new ServerCapabilities()
        {
            DocumentRangeFormattingProvider = true,
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Resolve_RangeFormattingOptions_ReturnsTrue()
    {
        // Arrange
        var methodName = Methods.TextDocumentRangeFormattingName;
        var capabilities = new ServerCapabilities()
        {
            DocumentRangeFormattingProvider = new DocumentRangeFormattingOptions(),
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Resolve_WorkspaceSymbol_ReturnsTrue()
    {
        // Arrange
        var methodName = Methods.WorkspaceSymbolName;
        var capabilities = new ServerCapabilities()
        {
            WorkspaceSymbolProvider = true,
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Resolve_WorkspaceSymbolOptions_ReturnsTrue()
    {
        // Arrange
        var methodName = Methods.WorkspaceSymbolName;
        var capabilities = new ServerCapabilities()
        {
            WorkspaceSymbolProvider = new WorkspaceSymbolOptions(),
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Resolve_OnTypeFormatting_ReturnsTrue()
    {
        // Arrange
        var methodName = Methods.TextDocumentOnTypeFormattingName;
        var capabilities = new ServerCapabilities()
        {
            DocumentOnTypeFormattingProvider = new DocumentOnTypeFormattingOptions(),
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Resolve_Formatting_ReturnsTrue()
    {
        // Arrange
        var methodName = Methods.TextDocumentFormattingName;
        var capabilities = new ServerCapabilities()
        {
            DocumentFormattingProvider = true,
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Resolve_FormattingOptions_ReturnsTrue()
    {
        // Arrange
        var methodName = Methods.TextDocumentFormattingName;
        var capabilities = new ServerCapabilities()
        {
            DocumentFormattingProvider = new DocumentFormattingOptions(),
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Resolve_Hover_ReturnsTrue()
    {
        // Arrange
        var methodName = Methods.TextDocumentHoverName;
        var capabilities = new ServerCapabilities()
        {
            HoverProvider = true,
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Resolve_HoverOptions_ReturnsTrue()
    {
        // Arrange
        var methodName = Methods.TextDocumentHoverName;
        var capabilities = new ServerCapabilities()
        {
            HoverProvider = new HoverOptions(),
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Resolve_CodeAction_ReturnsTrue()
    {
        // Arrange
        var methodName = Methods.TextDocumentCodeActionName;
        var capabilities = new ServerCapabilities()
        {
            CodeActionProvider = true,
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Resolve_CodeActionOptions_ReturnsTrue()
    {
        // Arrange
        var methodName = Methods.TextDocumentCodeActionName;
        var capabilities = new ServerCapabilities()
        {
            CodeActionProvider = new CodeActionOptions(),
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Resolve_CodeLens_ReturnsTrue()
    {
        // Arrange
        var methodName = Methods.CodeLensResolveName;
        var capabilities = new ServerCapabilities()
        {
            CodeLensProvider = new CodeLensOptions(),
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Resolve_DocumentColor_ReturnsTrue()
    {
        // Arrange
        var methodName = Methods.TextDocumentDocumentColorName;
        var capabilities = new ServerCapabilities()
        {
            DocumentColorProvider = true,
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Resolve_DocumentColorOptions_ReturnsTrue()
    {
        // Arrange
        var methodName = Methods.TextDocumentDocumentColorName;
        var capabilities = new ServerCapabilities()
        {
            DocumentColorProvider = new DocumentColorOptions(),
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Resolve_Completion_ReturnsTrue()
    {
        // Arrange
        var methodName = Methods.TextDocumentCompletionName;
        var capabilities = new ServerCapabilities()
        {
            CompletionProvider = new CompletionOptions(),
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Resolve_CompletionResolve_ReturnsTrue()
    {
        // Arrange
        var methodName = Methods.TextDocumentCompletionResolveName;
        var capabilities = new ServerCapabilities()
        {
            CompletionProvider = new CompletionOptions()
            {
                ResolveProvider = true,
            },
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Resolve_Definition_ReturnsTrue()
    {
        // Arrange
        var methodName = Methods.TextDocumentDefinitionName;
        var capabilities = new ServerCapabilities()
        {
            DefinitionProvider = true,
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Resolve_DefinitionOptions_ReturnsTrue()
    {
        // Arrange
        var methodName = Methods.TextDocumentDefinitionName;
        var capabilities = new ServerCapabilities()
        {
            DefinitionProvider = new DefinitionOptions(),
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Resolve_Highlight_ReturnsTrue()
    {
        // Arrange
        var methodName = Methods.TextDocumentDocumentHighlightName;
        var capabilities = new ServerCapabilities()
        {
            DocumentHighlightProvider = true,
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Resolve_HighlightOptions_ReturnsTrue()
    {
        // Arrange
        var methodName = Methods.TextDocumentDocumentHighlightName;
        var capabilities = new ServerCapabilities()
        {
            DocumentHighlightProvider = new DocumentHighlightOptions(),
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Resolve_MSReference_ReturnsTrue()
    {
        // Arrange
        var methodName = VSInternalMethods.DocumentReferencesName;
        var capabilities = new VSInternalServerCapabilities()
        {
            MSReferencesProvider = true,
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Resolve_ProjectContext_ReturnsTrue()
    {
        // Arrange
        var methodName = VSMethods.GetProjectContextsName;
        var capabilities = new VSServerCapabilities()
        {
            ProjectContextProvider = true,
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Resolve_CodeActionResolve_ReturnsTrue()
    {
        // Arrange
        var methodName = Methods.CodeActionResolveName;
        var capabilities = new ServerCapabilities()
        {
            CodeActionProvider = new CodeActionOptions()
            {
                ResolveProvider = true,
            },
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Resolve_OnAutoInsert_ReturnsTrue()
    {
        // Arrange
        var methodName = VSInternalMethods.OnAutoInsertName;
        var capabilities = new VSInternalServerCapabilities()
        {
            OnAutoInsertProvider = new VSInternalDocumentOnAutoInsertOptions(),
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Resolve_PullDiagnostics_ReturnsTrue()
    {
        // Arrange
        var methodName = VSInternalMethods.DocumentPullDiagnosticName;
        var capabilities = new VSInternalServerCapabilities()
        {
            SupportsDiagnosticRequests = true,
        };
        var jobjectCapabilities = JObject.FromObject(capabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(Methods.TextDocumentImplementationName)]
    [InlineData(Methods.TextDocumentTypeDefinitionName)]
    [InlineData(Methods.TextDocumentReferencesName)]
    [InlineData(Methods.TextDocumentRenameName)]
    [InlineData(Methods.TextDocumentSignatureHelpName)]
    [InlineData(Methods.TextDocumentWillSaveName)]
    [InlineData(Methods.TextDocumentWillSaveWaitUntilName)]
    [InlineData(Methods.TextDocumentRangeFormattingName)]
    [InlineData(Methods.WorkspaceSymbolName)]
    [InlineData(Methods.TextDocumentOnTypeFormattingName)]
    [InlineData(Methods.TextDocumentFormattingName)]
    [InlineData(Methods.TextDocumentHoverName)]
    [InlineData(Methods.TextDocumentCodeActionName)]
    [InlineData(Methods.TextDocumentCodeLensName)]
    [InlineData(Methods.TextDocumentDocumentColorName)]
    [InlineData(Methods.TextDocumentCompletionName)]
    [InlineData(Methods.TextDocumentCompletionResolveName)]
    [InlineData(Methods.TextDocumentDefinitionName)]
    [InlineData(Methods.TextDocumentDocumentHighlightName)]
    [InlineData(Methods.CodeActionResolveName)]
    [InlineData(VSMethods.GetProjectContextsName)]
    [InlineData(VSInternalMethods.DocumentReferencesName)]
    [InlineData(VSInternalMethods.OnAutoInsertName)]
    [InlineData(VSInternalMethods.DocumentPullDiagnosticName)]
    [InlineData(VSInternalMethods.WorkspacePullDiagnosticName)]
    public void Resolve_NotPresent_ReturnsFalse(string methodName)
    {
        // Arrange
        var emptyCapabilities = new VSServerCapabilities();
        var jobjectCapabilities = JObject.FromObject(emptyCapabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Resolve_UnknownMethod_ReturnsTrue()
    {
        // Arrange
        var methodName = "razor/languageQuery";
        var emptyCapabilities = new VSServerCapabilities();
        var jobjectCapabilities = JObject.FromObject(emptyCapabilities);
        var filter = s_resolver.Resolve(methodName);

        // Act
        var result = filter(jobjectCapabilities);

        // Assert
        Assert.True(result);
    }
}
