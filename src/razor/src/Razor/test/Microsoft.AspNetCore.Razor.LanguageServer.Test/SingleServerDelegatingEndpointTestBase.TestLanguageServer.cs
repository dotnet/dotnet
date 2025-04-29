// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Diagnostics;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis.Razor.CodeActions.Models;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.CodeActions;
using Microsoft.CodeAnalysis.Razor.Protocol.Diagnostics;
using Microsoft.CodeAnalysis.Razor.Protocol.Folding;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using DefinitionResult = Microsoft.VisualStudio.LanguageServer.Protocol.SumType<
    Microsoft.VisualStudio.LanguageServer.Protocol.Location,
    Microsoft.VisualStudio.LanguageServer.Protocol.Location[],
    Microsoft.VisualStudio.LanguageServer.Protocol.DocumentLink[]>;
using ImplementationResult = Microsoft.VisualStudio.LanguageServer.Protocol.SumType<
    Microsoft.VisualStudio.LanguageServer.Protocol.Location[],
    Microsoft.VisualStudio.LanguageServer.Protocol.VSInternalReferenceItem[]>;
using Range = Microsoft.VisualStudio.LanguageServer.Protocol.Range;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public abstract partial class SingleServerDelegatingEndpointTestBase
{
    private protected class TestLanguageServer(
        CSharpTestLspServer csharpServer,
        Uri csharpDocumentUri,
        CancellationToken cancellationToken) : IClientConnection
    {
        private readonly CSharpTestLspServer _csharpServer = csharpServer;
        private readonly Uri _csharpDocumentUri = csharpDocumentUri;
        private readonly CancellationToken _cancellationToken = cancellationToken;

        private int _requestCount;

        public int RequestCount => _requestCount;

        public async Task<TResponse> SendRequestAsync<TParams, TResponse>(string method, TParams @params, CancellationToken cancellationToken)
        {
            _requestCount++;

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
                CustomMessageNames.RazorInlayHintEndpoint => await HandleInlayHintAsync(@params),
                CustomMessageNames.RazorInlayHintResolveEndpoint => await HandleInlayHintResolveAsync(@params),
                CustomMessageNames.RazorCSharpPullDiagnosticsEndpointName => await HandleCSharpDiagnosticsAsync(@params),
                _ => throw new NotImplementedException($"I don't know how to handle the '{method}' method.")
            };

            return (TResponse)result;
        }

        private Task<SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?> HandleCSharpDiagnosticsAsync<TParams>(TParams @params)
        {
            Assert.IsType<DelegatedDiagnosticParams>(@params);
            var actualParams = new DocumentDiagnosticParams()
            {
                TextDocument = new TextDocumentIdentifier { Uri = _csharpDocumentUri }
            };

            return _csharpServer.ExecuteRequestAsync<DocumentDiagnosticParams, SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?>(
                Methods.TextDocumentDiagnosticName,
                actualParams,
                _cancellationToken);
        }

        private static TextEdit[] HandleSimplifyMethod<TParams>(TParams @params)
        {
            Assert.IsType<DelegatedSimplifyMethodParams>(@params);
            return null;
        }

        private Task<VSProjectContextList> HandleProjectContextsAsync<TParams>(TParams @params)
        {
            Assert.IsType<DelegatedProjectContextsParams>(@params);

            var delegatedRequest = new VSGetProjectContextsParams
            {
                TextDocument = new TextDocumentItem
                {
                    Uri = _csharpDocumentUri,
                },
            };

            return _csharpServer.ExecuteRequestAsync<VSGetProjectContextsParams, VSProjectContextList>(
                VSMethods.GetProjectContextsName,
                delegatedRequest,
                _cancellationToken);
        }

        private Task<InlayHint[]> HandleInlayHintAsync<TParams>(TParams @params)
        {
            var delegatedParams = Assert.IsType<DelegatedInlayHintParams>(@params);

            var delegatedRequest = new InlayHintParams
            {
                TextDocument = new TextDocumentIdentifier
                {
                    Uri = _csharpDocumentUri,
                },
                Range = delegatedParams.ProjectedRange
            };

            return _csharpServer.ExecuteRequestAsync<InlayHintParams, InlayHint[]>(
                Methods.TextDocumentInlayHintName,
                delegatedRequest,
                _cancellationToken);
        }

        private Task<InlayHint> HandleInlayHintResolveAsync<TParams>(TParams @params)
        {
            var delegatedParams = Assert.IsType<DelegatedInlayHintResolveParams>(@params);

            var delegatedRequest = delegatedParams.InlayHint;

            return _csharpServer.ExecuteRequestAsync<InlayHint, InlayHint>(
                Methods.InlayHintResolveName,
                delegatedRequest,
                _cancellationToken);
        }

        private Task<SumType<DocumentSymbol[], SymbolInformation[]>?> HandleDocumentSymbolAsync<TParams>(TParams @params)
        {
            Assert.IsType<DelegatedDocumentSymbolParams>(@params);

            var delegatedRequest = new DocumentSymbolParams
            {
                TextDocument = new TextDocumentIdentifier
                {
                    Uri = _csharpDocumentUri,
                },
            };

            return _csharpServer.ExecuteRequestAsync<DocumentSymbolParams, SumType<DocumentSymbol[], SymbolInformation[]>?>(
                Methods.TextDocumentDocumentSymbolName,
                delegatedRequest,
                _cancellationToken);
        }

        private Task<VSInternalSpellCheckableRangeReport[]> HandleSpellCheckAsync<TParams>(TParams @params)
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

            return _csharpServer.ExecuteRequestAsync<VSInternalDocumentSpellCheckableParams, VSInternalSpellCheckableRangeReport[]>(
                VSInternalMethods.TextDocumentSpellCheckableRangesName,
                delegatedRequest,
                _cancellationToken);
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

            return new RazorPullDiagnosticResponse(result, []);
        }

        private static Task<RazorFoldingRangeResponse> HandleFoldingRangeAsync()
        {
            return Task.FromResult(RazorFoldingRangeResponse.Empty);
        }

        private Task<VSInternalCodeAction> HandleResolveCodeActionsAsync<TParams>(TParams @params)
        {
            var delegatedParams = Assert.IsType<RazorResolveCodeActionParams>(@params);

            var delegatedRequest = delegatedParams.CodeAction;

            return _csharpServer.ExecuteRequestAsync<CodeAction, VSInternalCodeAction>(
                Methods.CodeActionResolveName,
                delegatedRequest,
                _cancellationToken);
        }

        private Task<RazorVSInternalCodeAction[]> HandleProvideCodeActionsAsync<TParams>(TParams @params)
        {
            var delegatedParams = Assert.IsType<DelegatedCodeActionParams>(@params);

            var delegatedRequest = delegatedParams.CodeActionParams;
            delegatedRequest.TextDocument.Uri = _csharpDocumentUri;

            return _csharpServer.ExecuteRequestAsync<VSCodeActionParams, RazorVSInternalCodeAction[]>(
                Methods.TextDocumentCodeActionName,
                delegatedRequest,
                _cancellationToken);
        }

        private Task<VSInternalReferenceItem[]> HandleReferencesAsync<TParams>(TParams @params)
        {
            var delegatedParams = Assert.IsType<DelegatedPositionParams>(@params);
            var delegatedRequest = new ReferenceParams()
            {
                TextDocument = new VSTextDocumentIdentifier()
                {
                    Uri = _csharpDocumentUri,
                    ProjectContext = delegatedParams.Identifier.TextDocumentIdentifier.GetProjectContext(),
                },
                Position = delegatedParams.ProjectedPosition,
                Context = new ReferenceContext()
            };

            return _csharpServer.ExecuteRequestAsync<ReferenceParams, VSInternalReferenceItem[]>(
                Methods.TextDocumentReferencesName,
                delegatedRequest,
                _cancellationToken);
        }

        private Task<DefinitionResult?> HandleDefinitionAsync<T>(T @params)
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

            return _csharpServer.ExecuteRequestAsync<TextDocumentPositionParams, DefinitionResult?>(
                Methods.TextDocumentDefinitionName,
                delegatedRequest,
                _cancellationToken);
        }

        private Task<ImplementationResult> HandleImplementationAsync<T>(T @params)
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

            return _csharpServer.ExecuteRequestAsync<TextDocumentPositionParams, ImplementationResult>(
                Methods.TextDocumentImplementationName,
                delegatedRequest,
                _cancellationToken);
        }

        private Task<VisualStudio.LanguageServer.Protocol.SignatureHelp> HandleSignatureHelpAsync<T>(T @params)
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

            return _csharpServer.ExecuteRequestAsync<SignatureHelpParams, VisualStudio.LanguageServer.Protocol.SignatureHelp>(
                Methods.TextDocumentSignatureHelpName,
                delegatedRequest,
                _cancellationToken);
        }

        private Task<WorkspaceEdit> HandleRenameAsync<T>(T @params)
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

            return _csharpServer.ExecuteRequestAsync<RenameParams, WorkspaceEdit>(
                Methods.TextDocumentRenameName,
                delegatedRequest,
                _cancellationToken);
        }

        private Task<VSInternalDocumentOnAutoInsertResponseItem> HandleOnAutoInsertAsync<T>(T @params)
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

            return _csharpServer.ExecuteRequestAsync<VSInternalDocumentOnAutoInsertParams, VSInternalDocumentOnAutoInsertResponseItem>(
                VSInternalMethods.OnAutoInsertName,
                delegatedRequest,
                _cancellationToken);
        }

        public Task SendNotificationAsync<TParams>(string method, TParams @params, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SendNotificationAsync(string method, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private Task<Range> HandleValidateBreakpointRangeAsync<T>(T @params)
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

            return _csharpServer.ExecuteRequestAsync<VSInternalValidateBreakableRangeParams, Range>(
                VSInternalMethods.TextDocumentValidateBreakableRangeName, delegatedRequest, _cancellationToken);
        }
    }
}
