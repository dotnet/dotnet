// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion.Delegation;

public class DelegatedCompletionListProviderTest : CompletionTestBase
{
    private static readonly RazorCompletionOptions s_defaultRazorCompletionOptions = new(
        SnippetsSupported: true,
        AutoInsertAttributeQuotes: true,
        CommitElementsWithSpace: true);

    private static readonly VSInternalClientCapabilities s_clientCapabilities = new();

    private readonly DelegatedCompletionListProvider _provider;
    private DelegatedCompletionParams? _delegatedParams;

    public DelegatedCompletionListProviderTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        var clientConnection = CreateClientConnectionForCompletion(response: null, processParams: @params =>
        {
            _delegatedParams = @params;
        });

        _provider = CreateDelegatedCompletionListProvider(clientConnection);
    }

    [Fact]
    public async Task HtmlDelegation_Invoked()
    {
        // Arrange
        var completionContext = new VSInternalCompletionContext() { TriggerKind = CompletionTriggerKind.Invoked };
        var codeDocument = CreateCodeDocument("<");
        var documentContext = TestDocumentContext.Create("C:/path/to/file.cshtml", codeDocument);

        // Act
        await _provider.GetCompletionListAsync(
            codeDocument,
            absoluteIndex: 1,
            completionContext,
            documentContext,
            s_clientCapabilities,
            s_defaultRazorCompletionOptions,
            correlationId: Guid.Empty,
            cancellationToken: DisposalToken);

        // Assert
        Assert.NotNull(_delegatedParams);
        Assert.Equal(RazorLanguageKind.Html, _delegatedParams.ProjectedKind);
        Assert.Equal(LspFactory.CreatePosition(0, 1), _delegatedParams.ProjectedPosition);
        Assert.Equal(CompletionTriggerKind.Invoked, _delegatedParams.Context.TriggerKind);
        Assert.Equal(1, _delegatedParams.Identifier.Version);
        Assert.Null(_delegatedParams.ProvisionalTextEdit);
    }

    [Fact]
    public async Task HtmlDelegation_TriggerCharacter()
    {
        // Arrange
        var completionContext = new VSInternalCompletionContext()
        {
            InvokeKind = VSInternalCompletionInvokeKind.Typing,
            TriggerKind = CompletionTriggerKind.TriggerCharacter,
            TriggerCharacter = "<",
        };
        var codeDocument = CreateCodeDocument("<");
        var documentContext = TestDocumentContext.Create("C:/path/to/file.cshtml", codeDocument);

        // Act
        await _provider.GetCompletionListAsync(
            codeDocument,
            absoluteIndex: 1,
            completionContext,
            documentContext,
            s_clientCapabilities,
            s_defaultRazorCompletionOptions,
            correlationId: Guid.Empty,
            cancellationToken: DisposalToken);

        // Assert
        Assert.NotNull(_delegatedParams);
        Assert.Equal(RazorLanguageKind.Html, _delegatedParams.ProjectedKind);
        Assert.Equal(LspFactory.CreatePosition(0, 1), _delegatedParams.ProjectedPosition);
        Assert.Equal(CompletionTriggerKind.TriggerCharacter, _delegatedParams.Context.TriggerKind);
        Assert.Equal(VSInternalCompletionInvokeKind.Typing, _delegatedParams.Context.InvokeKind);
        Assert.Equal(1, _delegatedParams.Identifier.Version);
        Assert.Null(_delegatedParams.ProvisionalTextEdit);
    }

    [Fact]
    public async Task HtmlDelegation_UnsupportedTriggerCharacter_ReturnsNull()
    {
        // Arrange
        var completionContext = new VSInternalCompletionContext()
        {
            InvokeKind = VSInternalCompletionInvokeKind.Typing,
            TriggerKind = CompletionTriggerKind.TriggerCharacter,
            TriggerCharacter = "|",
        };
        var codeDocument = CreateCodeDocument("|");
        var documentContext = TestDocumentContext.Create("C:/path/to/file.cshtml", codeDocument);

        // Act
        await _provider.GetCompletionListAsync(
            codeDocument,
            absoluteIndex: 1,
            completionContext,
            documentContext,
            s_clientCapabilities,
            s_defaultRazorCompletionOptions,
            correlationId: Guid.Empty,
            cancellationToken: DisposalToken);

        // Assert
        Assert.Null(_delegatedParams);
    }

    [Fact]
    public async Task Delegation_NullResult_ToIncompleteResult()
    {
        // Arrange
        var completionContext = new VSInternalCompletionContext()
        {
            InvokeKind = VSInternalCompletionInvokeKind.Typing,
            TriggerKind = CompletionTriggerKind.TriggerCharacter,
            TriggerCharacter = "<",
        };

        var codeDocument = CreateCodeDocument("<");
        var documentContext = TestDocumentContext.Create("C:/path/to/file.cshtml", codeDocument);

        var clientConnection = CreateClientConnectionForCompletionWithNullResponse();

        var provider = CreateDelegatedCompletionListProvider(clientConnection);

        // Act
        var delegatedCompletionList = await provider.GetCompletionListAsync(
            codeDocument,
            absoluteIndex: 1,
            completionContext,
            documentContext,
            s_clientCapabilities,
            s_defaultRazorCompletionOptions,
            correlationId: Guid.Empty,
            cancellationToken: DisposalToken);

        // Assert
        Assert.NotNull(delegatedCompletionList);
        Assert.True(delegatedCompletionList.IsIncomplete);
    }

    [Fact]
    public async Task CSharp_Invoked()
    {
        // Arrange & Act
        var completionList = await GetCompletionListAsync("@$$", CompletionTriggerKind.Invoked);

        // Assert
        Assert.NotNull(completionList);
        Assert.Contains(completionList.Items, item => item.Label == "System");
    }

    [Fact]
    public async Task CSharp_At_TranslatesToInvoked_Triggered()
    {
        // Arrange & Act
        var completionList = await GetCompletionListAsync("@$$", CompletionTriggerKind.TriggerCharacter);

        // Assert
        Assert.NotNull(completionList);
        Assert.Contains(completionList.Items, item => item.Label == "System");
    }

    [Fact]
    public async Task CSharp_Operator_Triggered()
    {
        // Arrange & Act
        var completionList = await GetCompletionListAsync("@(DateTime.$$)", CompletionTriggerKind.TriggerCharacter);

        // Assert
        Assert.NotNull(completionList);
        Assert.Contains(completionList.Items, item => item.Label == "Now");
    }

    [Fact]
    public async Task RazorDelegation_Noop()
    {
        // Arrange
        var completionContext = new VSInternalCompletionContext() { TriggerKind = CompletionTriggerKind.Invoked };
        var codeDocument = CreateCodeDocument("@functions ");
        var documentContext = TestDocumentContext.Create("C:/path/to/file.razor", codeDocument);

        // Act
        var completionList = await _provider.GetCompletionListAsync(
            codeDocument,
            absoluteIndex: 11,
            completionContext,
            documentContext,
            s_clientCapabilities,
            s_defaultRazorCompletionOptions,
            correlationId: Guid.Empty,
            cancellationToken: DisposalToken);

        // Assert
        Assert.Null(completionList);
        Assert.Null(_delegatedParams);
    }

    [Fact]
    public async Task ProvisionalCompletion_TranslatesToCSharpWithProvisionalTextEdit()
    {
        // Arrange
        var completionContext = new VSInternalCompletionContext()
        {
            InvokeKind = VSInternalCompletionInvokeKind.Typing,
            TriggerKind = CompletionTriggerKind.TriggerCharacter,
            TriggerCharacter = ".",
        };
        var codeDocument = CreateCodeDocument("@DateTime.");
        var documentContext = TestDocumentContext.Create("C:/path/to/file.cshtml", codeDocument);

        // Act
        await _provider.GetCompletionListAsync(
            codeDocument,
            absoluteIndex: 10,
            completionContext,
            documentContext,
            s_clientCapabilities,
            s_defaultRazorCompletionOptions,
            correlationId: Guid.Empty,
            cancellationToken: DisposalToken);

        // Assert
        Assert.NotNull(_delegatedParams);
        Assert.Equal(RazorLanguageKind.CSharp, _delegatedParams.ProjectedKind);

        // Just validating that we're generating code in a way that's different from the top-level document. Don't need to be specific.
        Assert.True(_delegatedParams.ProjectedPosition.Line > 2);
        Assert.Equal(CompletionTriggerKind.TriggerCharacter, _delegatedParams.Context.TriggerKind);
        Assert.Equal(VSInternalCompletionInvokeKind.Typing, _delegatedParams.Context.InvokeKind);
        Assert.Equal(1, _delegatedParams.Identifier.Version);
        Assert.NotNull(_delegatedParams.ProvisionalTextEdit);
    }

    [Fact]
    public async Task DotTriggerInMiddleOfCSharpImplicitExpressionNotTreatedAsProvisional()
    {
        // Arrange
        var completionContext = new VSInternalCompletionContext()
        {
            InvokeKind = VSInternalCompletionInvokeKind.Typing,
            TriggerKind = CompletionTriggerKind.TriggerCharacter,
            TriggerCharacter = ".",
        };
        var codeDocument = CreateCodeDocument("@DateTime.Now");
        var documentContext = TestDocumentContext.Create("C:/path/to/file.cshtml", codeDocument);

        // Act
        await _provider.GetCompletionListAsync(
            codeDocument,
            absoluteIndex: 10,
            completionContext,
            documentContext,
            s_clientCapabilities,
            s_defaultRazorCompletionOptions,
            correlationId: Guid.Empty,
            cancellationToken: DisposalToken);

        // Assert
        Assert.NotNull(_delegatedParams);
        Assert.Equal(RazorLanguageKind.CSharp, _delegatedParams.ProjectedKind);

        // Just validating that we're generating code in a way that's different from the top-level document. Don't need to be specific.
        Assert.True(_delegatedParams.ProjectedPosition.Line > 2);
        Assert.Equal(CompletionTriggerKind.TriggerCharacter, _delegatedParams.Context.TriggerKind);
        Assert.Equal(VSInternalCompletionInvokeKind.Typing, _delegatedParams.Context.InvokeKind);
        Assert.Equal(1, _delegatedParams.Identifier.Version);
        Assert.Null(_delegatedParams.ProvisionalTextEdit);
    }

    [Theory]
    [InlineData("$$", true)]
    [InlineData("<$$", false)]
    [InlineData(">$$", true)]
    [InlineData("$$<", true)]
    [InlineData("$$>", false)] // This is the only case that returns false but should return true. It's unlikely a user will type this, but it's complex to solve. Consider this a known and acceptable bug.
    [InlineData("<div>$$</div>", true)]
    [InlineData("$$<div></div>", true)]
    [InlineData("<div></div>$$", true)]
    [InlineData("<$$div></div>", false)]
    [InlineData("<div$$></div>", false)]
    [InlineData("<div class=\"$$\"></div>", false)]
    [InlineData("<div><$$/div>", false)]
    [InlineData("<div></div$$>", false)]
    public async Task ShouldIncludeSnippets(string input, bool shouldIncludeSnippets)
    {
        var requestSent = false;

        var clientConnection = TestClientConnection.Create(builder =>
        {
            builder.AddFactory<DelegatedCompletionParams, RazorVSInternalCompletionList?>(
                LanguageServerConstants.RazorCompletionEndpointName,
                (_, @params, _) =>
            {
                requestSent = true;
                Assert.Equal(shouldIncludeSnippets, @params.ShouldIncludeSnippets);

                return Task.FromResult((RazorVSInternalCompletionList?)null);
            });
        });

        TestFileMarkupParser.GetPosition(input, out var code, out var cursorPosition);
        var codeDocument = CreateCodeDocument(code);
        var documentContext = TestDocumentContext.Create("C:/path/to/file.cshtml", codeDocument);

        var generatedPosition = new LinePosition(0, cursorPosition);

        var documentMappingServiceMock = new StrictMock<IDocumentMappingService>();
        documentMappingServiceMock
            .Setup(x => x.TryMapToCSharpDocumentPosition(It.IsAny<RazorCSharpDocument>(), It.IsAny<int>(), out generatedPosition, out It.Ref<int>.IsAny))
            .Returns(true);

        var completionProvider = CreateDelegatedCompletionListProvider(clientConnection, documentMappingServiceMock.Object);

        var completionContext = new VSInternalCompletionContext()
        {
            InvokeKind = VSInternalCompletionInvokeKind.Typing,
            TriggerKind = CompletionTriggerKind.TriggerCharacter,
            TriggerCharacter = ".",
        };

        _ = await completionProvider.GetCompletionListAsync(
            codeDocument,
            cursorPosition,
            completionContext,
            documentContext,
            s_clientCapabilities,
            s_defaultRazorCompletionOptions,
            correlationId: Guid.Empty,
            DisposalToken);

        Assert.True(requestSent);
    }

    private async Task<RazorVSInternalCompletionList?> GetCompletionListAsync(string content, CompletionTriggerKind triggerKind)
    {
        TestFileMarkupParser.GetPosition(content, out var output, out var cursorPosition);
        var codeDocument = CreateCodeDocument(output);
        var csharpSourceText = codeDocument.GetCSharpSourceText();
        var csharpDocumentUri = new Uri("C:/path/to/file.razor__virtual.g.cs");
        var serverCapabilities = new VSInternalServerCapabilities()
        {
            CompletionProvider = new CompletionOptions
            {
                ResolveProvider = true,
                TriggerCharacters = [" ", "(", "=", "#", ".", "<", "[", "{", "\"", "/", ":", "~"]
            }
        };

        await using var csharpServer = await CSharpTestLspServerHelpers.CreateCSharpLspServerAsync(
            csharpSourceText, csharpDocumentUri, serverCapabilities, DisposalToken);

        await csharpServer.OpenDocumentAsync(csharpDocumentUri, csharpSourceText.ToString(), DisposalToken);

        var triggerCharacter = triggerKind == CompletionTriggerKind.TriggerCharacter ? output[cursorPosition - 1].ToString() : null;
        var invocationKind = triggerKind == CompletionTriggerKind.TriggerCharacter ? VSInternalCompletionInvokeKind.Typing : VSInternalCompletionInvokeKind.Explicit;

        var completionContext = new VSInternalCompletionContext()
        {
            TriggerKind = triggerKind,
            TriggerCharacter = triggerCharacter,
            InvokeKind = invocationKind,
        };

        var documentContext = TestDocumentContext.Create("C:/path/to/file.razor", codeDocument);
        var clientConnection = CreateClientConnectionForCompletion(csharpServer);
        var provider = CreateDelegatedCompletionListProvider(clientConnection);

        var completionList = await provider.GetCompletionListAsync(
            codeDocument,
            absoluteIndex: cursorPosition,
            completionContext,
            documentContext,
            s_clientCapabilities,
            s_defaultRazorCompletionOptions,
            correlationId: Guid.Empty,
            cancellationToken: DisposalToken);

        return completionList;
    }
}
