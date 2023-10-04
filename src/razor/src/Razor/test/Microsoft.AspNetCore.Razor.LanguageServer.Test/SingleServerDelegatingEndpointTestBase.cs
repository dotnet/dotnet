// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Diagnostics;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Folding;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.LanguageServer.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Mef;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;
using DefinitionResult = Microsoft.VisualStudio.LanguageServer.Protocol.SumType<
    Microsoft.VisualStudio.LanguageServer.Protocol.VSInternalLocation,
    Microsoft.VisualStudio.LanguageServer.Protocol.VSInternalLocation[],
    Microsoft.VisualStudio.LanguageServer.Protocol.DocumentLink[]>;
using ImplementationResult = Microsoft.VisualStudio.LanguageServer.Protocol.SumType<
    Microsoft.VisualStudio.LanguageServer.Protocol.Location[],
    Microsoft.VisualStudio.LanguageServer.Protocol.VSInternalReferenceItem[]>;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

[UseExportProvider]
public abstract class SingleServerDelegatingEndpointTestBase : LanguageServerTestBase
{
    internal DocumentContextFactory DocumentContextFactory { get; private set; }
    internal LanguageServerFeatureOptions LanguageServerFeatureOptions { get; private set; }
    internal TestLanguageServer LanguageServer { get; private set; }
    internal IRazorDocumentMappingService DocumentMappingService { get; private set; }

    protected SingleServerDelegatingEndpointTestBase(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    protected async Task CreateLanguageServerAsync(RazorCodeDocument codeDocument, string razorFilePath, IEnumerable<(string, string)> additionalRazorDocuments = null)
    {
        var projectKey = TestProjectKey.Create("");
        var csharpSourceText = codeDocument.GetCSharpSourceText();
        var csharpDocumentUri = new Uri(FilePathService.GetRazorCSharpFilePath(projectKey, razorFilePath));

        var csharpFiles = new List<(Uri, SourceText)>();
        csharpFiles.Add((csharpDocumentUri, csharpSourceText));
        if (additionalRazorDocuments is not null)
        {
            foreach ((var filePath, var contents) in additionalRazorDocuments)
            {
                var additionalDocument = CreateCodeDocument(contents, filePath: filePath);
                var additionalDocumentSourceText = additionalDocument.GetCSharpSourceText();
                var additionalDocumentUri = new Uri(FilePathService.GetRazorCSharpFilePath(projectKey, "C:/path/to/" + filePath));

                csharpFiles.Add((additionalDocumentUri, additionalDocumentSourceText));
            }
        }

        var csharpServer = await CSharpTestLspServerHelpers.CreateCSharpLspServerAsync(
            csharpFiles,
            new VSInternalServerCapabilities
            {
                SupportsDiagnosticRequests = true,
            },
            razorSpanMappingService: null,
            DisposalToken);

        AddDisposable(csharpServer);

        await csharpServer.OpenDocumentAsync(csharpDocumentUri, csharpSourceText.ToString()).ConfigureAwait(false);

        DocumentContextFactory = new TestDocumentContextFactory(razorFilePath, codeDocument, version: 1337);
        LanguageServerFeatureOptions = Mock.Of<LanguageServerFeatureOptions>(options =>
            options.SupportsFileManipulation == true &&
            options.SingleServerSupport == true &&
            options.CSharpVirtualDocumentSuffix == DefaultLanguageServerFeatureOptions.DefaultCSharpVirtualDocumentSuffix &&
            options.HtmlVirtualDocumentSuffix == DefaultLanguageServerFeatureOptions.DefaultHtmlVirtualDocumentSuffix,
            MockBehavior.Strict);
        LanguageServer = new TestLanguageServer(csharpServer, csharpDocumentUri, DisposalToken);
        DocumentMappingService = new RazorDocumentMappingService(FilePathService, DocumentContextFactory, LoggerFactory);
    }

    internal class TestLanguageServer : ClientNotifierServiceBase
    {
        private readonly CSharpTestLspServer _csharpServer;
        private readonly Uri _csharpDocumentUri;
        private readonly CancellationToken _cancellationToken;

        public int RequestCount;

        public TestLanguageServer(
            CSharpTestLspServer csharpServer,
            Uri csharpDocumentUri,
            CancellationToken cancellationToken)
        {
            _csharpServer = csharpServer;
            _csharpDocumentUri = csharpDocumentUri;
            _cancellationToken = cancellationToken;
        }

        public override Task OnInitializedAsync(VSInternalClientCapabilities clientCapabilities, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async override Task<TResponse> SendRequestAsync<TParams, TResponse>(string method, TParams @params, CancellationToken cancellationToken)
        {
            RequestCount++;
            object result = method switch
            {
                CustomMessageNames.RazorDefinitionEndpointName => await HandleDefinitionAsync(@params),
                CustomMessageNames.RazorImplementationEndpointName => await HandleImplementationAsync(@params),
                CustomMessageNames.RazorSignatureHelpEndpointName => await HandleSignatureHelpAsync(@params),
                CustomMessageNames.RazorRenameEndpointName => await HandleRenameAsync(@params),
                CustomMessageNames.RazorOnAutoInsertEndpointName => await HandleOnAutoInsertAsync(@params),
                CustomMessageNames.RazorValidateBreakpointRangeName => await HandleValidateBreakpointRangeAsync(@params),
                CustomMessageNames.RazorReferencesEndpointName => await HandleReferencesAsync(@params),
                CustomMessageNames.RazorProvideCodeActionsEndpoint => await HandleProvideCodeActionsAsync(@params),
                CustomMessageNames.RazorResolveCodeActionsEndpoint => await HandleResolveCodeActionsAsync(@params),
                CustomMessageNames.RazorPullDiagnosticEndpointName => await HandlePullDiagnosticsAsync(@params),
                CustomMessageNames.RazorFoldingRangeEndpoint => await HandleFoldingRangeAsync(),
                CustomMessageNames.RazorSpellCheckEndpoint => await HandleSpellCheckAsync(@params),
                CustomMessageNames.RazorDocumentSymbolEndpoint => await HandleDocumentSymbolAsync(@params),
                CustomMessageNames.RazorProjectContextsEndpoint => await HandleProjectContextsAsync(@params),
                CustomMessageNames.RazorSimplifyMethodEndpointName => HandleSimplifyMethod(@params),
                _ => throw new NotImplementedException($"I don't know how to handle the '{method}' method.")
            };

            return (TResponse)result;
        }

        private static TextEdit[] HandleSimplifyMethod<TParams>(TParams @params)
        {
            Assert.IsType<DelegatedSimplifyMethodParams>(@params);
            return null;
        }

        private async Task<VSProjectContextList> HandleProjectContextsAsync<TParams>(TParams @params)
        {
            Assert.IsType<DelegatedProjectContextsParams>(@params);

            var delegatedRequest = new VSGetProjectContextsParams
            {
                TextDocument = new TextDocumentItem
                {
                    Uri = _csharpDocumentUri,
                },
            };

            var result = await _csharpServer.ExecuteRequestAsync<VSGetProjectContextsParams, VSProjectContextList>(
                VSMethods.GetProjectContextsName,
                delegatedRequest,
                _cancellationToken);

            return result;
        }

        private async Task<SymbolInformation[]> HandleDocumentSymbolAsync<TParams>(TParams @params)
        {
            Assert.IsType<DelegatedDocumentSymbolParams>(@params);

            var delegatedRequest = new DocumentSymbolParams
            {
                TextDocument = new TextDocumentIdentifier
                {
                    Uri = _csharpDocumentUri,
                },
            };

            var result = await _csharpServer.ExecuteRequestAsync<DocumentSymbolParams, SymbolInformation[]>(
                Methods.TextDocumentDocumentSymbolName,
                delegatedRequest,
                _cancellationToken);

            return result;
        }

        private async Task<VSInternalSpellCheckableRangeReport[]> HandleSpellCheckAsync<TParams>(TParams @params)
        {
            var delegatedParams = Assert.IsType<DelegatedSpellCheckParams>(@params);

            var delegatedRequest = new VSInternalDocumentSpellCheckableParams
            {
                TextDocument = new VSTextDocumentIdentifier
                {
                    Uri = _csharpDocumentUri,
                    ProjectContext = delegatedParams.Identifier.TextDocumentIdentifier.GetProjectContext(),
                },
            };

            var result = await _csharpServer.ExecuteRequestAsync<VSInternalDocumentSpellCheckableParams, VSInternalSpellCheckableRangeReport[]>(
                VSInternalMethods.TextDocumentSpellCheckableRangesName,
                delegatedRequest,
                _cancellationToken);

            return result;
        }

        private async Task<RazorPullDiagnosticResponse> HandlePullDiagnosticsAsync<TParams>(TParams @params)
        {
            var delegatedParams = Assert.IsType<DelegatedDiagnosticParams>(@params);

            var delegatedRequest = new VSInternalDocumentDiagnosticsParams
            {
                TextDocument = new VSTextDocumentIdentifier
                {
                    Uri = _csharpDocumentUri,
                    ProjectContext = delegatedParams.Identifier.TextDocumentIdentifier.GetProjectContext(),
                },
            };

            var result = await _csharpServer.ExecuteRequestAsync<VSInternalDocumentDiagnosticsParams, VSInternalDiagnosticReport[]>(
                VSInternalMethods.DocumentPullDiagnosticName,
                delegatedRequest,
                _cancellationToken);

            return new RazorPullDiagnosticResponse(result, Array.Empty<VSInternalDiagnosticReport>());
        }

        private Task<RazorFoldingRangeResponse> HandleFoldingRangeAsync()
        {
            return Task.FromResult(new RazorFoldingRangeResponse(ImmutableArray<FoldingRange>.Empty, ImmutableArray<FoldingRange>.Empty));
        }

        private async Task<VSInternalCodeAction> HandleResolveCodeActionsAsync<TParams>(TParams @params)
        {
            var delegatedParams = Assert.IsType<RazorResolveCodeActionParams>(@params);

            var delegatedRequest = delegatedParams.CodeAction;

            var result = await _csharpServer.ExecuteRequestAsync<CodeAction, VSInternalCodeAction>(
                Methods.CodeActionResolveName,
                delegatedRequest,
                _cancellationToken);

            return result;
        }

        private async Task<RazorVSInternalCodeAction[]> HandleProvideCodeActionsAsync<TParams>(TParams @params)
        {
            var delegatedParams = Assert.IsType<DelegatedCodeActionParams>(@params);

            var delegatedRequest = delegatedParams.CodeActionParams;
            delegatedRequest.TextDocument.Uri = _csharpDocumentUri;

            var result = await _csharpServer.ExecuteRequestAsync<VSCodeActionParams, RazorVSInternalCodeAction[]>(
                Methods.TextDocumentCodeActionName,
                delegatedRequest,
                _cancellationToken);

            return result;
        }

        private async Task<VSInternalReferenceItem[]> HandleReferencesAsync<TParams>(TParams @params)
        {
            var delegatedParams = Assert.IsType<DelegatedPositionParams>(@params);
            var delegatedRequest = new TextDocumentPositionParams()
            {
                TextDocument = new VSTextDocumentIdentifier()
                {
                    Uri = _csharpDocumentUri,
                    ProjectContext = delegatedParams.Identifier.TextDocumentIdentifier.GetProjectContext(),
                },
                Position = delegatedParams.ProjectedPosition
            };

            var result = await _csharpServer.ExecuteRequestAsync<TextDocumentPositionParams, VSInternalReferenceItem[]>(
                Methods.TextDocumentReferencesName,
                delegatedRequest,
                _cancellationToken);

            return result;
        }

        private async Task<DefinitionResult?> HandleDefinitionAsync<T>(T @params)
        {
            var delegatedParams = Assert.IsType<DelegatedPositionParams>(@params);
            var delegatedRequest = new TextDocumentPositionParams()
            {
                TextDocument = new VSTextDocumentIdentifier()
                {
                    Uri = _csharpDocumentUri,
                    ProjectContext = delegatedParams.Identifier.TextDocumentIdentifier.GetProjectContext(),
                },
                Position = delegatedParams.ProjectedPosition
            };

            var result = await _csharpServer.ExecuteRequestAsync<TextDocumentPositionParams, DefinitionResult?>(
                Methods.TextDocumentDefinitionName,
                delegatedRequest,
                _cancellationToken);

            return result;
        }

        private async Task<ImplementationResult> HandleImplementationAsync<T>(T @params)
        {
            var delegatedParams = Assert.IsType<DelegatedPositionParams>(@params);
            var delegatedRequest = new TextDocumentPositionParams()
            {
                TextDocument = new VSTextDocumentIdentifier()
                {
                    Uri = _csharpDocumentUri,
                    ProjectContext = delegatedParams.Identifier.TextDocumentIdentifier.GetProjectContext(),
                },
                Position = delegatedParams.ProjectedPosition
            };

            var result = await _csharpServer.ExecuteRequestAsync<TextDocumentPositionParams, ImplementationResult>(
                Methods.TextDocumentImplementationName,
                delegatedRequest,
                _cancellationToken);

            return result;
        }

        private async Task<VisualStudio.LanguageServer.Protocol.SignatureHelp> HandleSignatureHelpAsync<T>(T @params)
        {
            var delegatedParams = Assert.IsType<DelegatedPositionParams>(@params);
            var delegatedRequest = new SignatureHelpParams()
            {
                TextDocument = new VSTextDocumentIdentifier()
                {
                    Uri = _csharpDocumentUri,
                    ProjectContext = delegatedParams.Identifier.TextDocumentIdentifier.GetProjectContext(),
                },
                Position = delegatedParams.ProjectedPosition,
            };

            var result = await _csharpServer.ExecuteRequestAsync<SignatureHelpParams, VisualStudio.LanguageServer.Protocol.SignatureHelp>(
                Methods.TextDocumentSignatureHelpName,
                delegatedRequest,
                _cancellationToken);

            return result;
        }

        private async Task<WorkspaceEdit> HandleRenameAsync<T>(T @params)
        {
            var delegatedParams = Assert.IsType<DelegatedRenameParams>(@params);
            var delegatedRequest = new RenameParams()
            {
                TextDocument = new VSTextDocumentIdentifier()
                {
                    Uri = _csharpDocumentUri,
                    ProjectContext = delegatedParams.Identifier.TextDocumentIdentifier.GetProjectContext(),
                },
                Position = delegatedParams.ProjectedPosition,
                NewName = delegatedParams.NewName,
            };

            var result = await _csharpServer.ExecuteRequestAsync<RenameParams, WorkspaceEdit>(
                Methods.TextDocumentRenameName,
                delegatedRequest,
                _cancellationToken);

            return result;
        }

        private async Task<VSInternalDocumentOnAutoInsertResponseItem> HandleOnAutoInsertAsync<T>(T @params)
        {
            var delegatedParams = Assert.IsType<DelegatedOnAutoInsertParams>(@params);
            var delegatedRequest = new VSInternalDocumentOnAutoInsertParams()
            {
                TextDocument = new VSTextDocumentIdentifier()
                {
                    Uri = _csharpDocumentUri,
                    ProjectContext = delegatedParams.Identifier.TextDocumentIdentifier.GetProjectContext()
                },
                Position = delegatedParams.ProjectedPosition,
                Character = delegatedParams.Character,
                Options = delegatedParams.Options
            };

            var result = await _csharpServer.ExecuteRequestAsync<VSInternalDocumentOnAutoInsertParams, VSInternalDocumentOnAutoInsertResponseItem>(
                VSInternalMethods.OnAutoInsertName,
                delegatedRequest,
                _cancellationToken);

            return result;
        }

        public override Task SendNotificationAsync<TParams>(string method, TParams @params, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task SendNotificationAsync(string method, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task<Range> HandleValidateBreakpointRangeAsync<T>(T @params)
        {
            var delegatedParams = Assert.IsType<DelegatedValidateBreakpointRangeParams>(@params);
            var delegatedRequest = new VSInternalValidateBreakableRangeParams()
            {
                TextDocument = new VSTextDocumentIdentifier()
                {
                    Uri = _csharpDocumentUri,
                    ProjectContext = delegatedParams.Identifier.TextDocumentIdentifier.GetProjectContext(),
                },
                Range = delegatedParams.ProjectedRange,
            };

            var result = await _csharpServer.ExecuteRequestAsync<VSInternalValidateBreakableRangeParams, Range>(
                VSInternalMethods.TextDocumentValidateBreakableRangeName, delegatedRequest, _cancellationToken);

            return result;
        }
    }
}
