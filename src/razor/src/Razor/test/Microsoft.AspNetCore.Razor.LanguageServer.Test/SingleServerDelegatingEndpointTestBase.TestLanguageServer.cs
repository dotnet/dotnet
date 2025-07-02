// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis.Razor.CodeActions.Models;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.CodeActions;
using Microsoft.CodeAnalysis.Razor.Protocol.Diagnostics;
using Microsoft.CodeAnalysis.Razor.Protocol.Folding;
using Xunit;
using DefinitionResult = Roslyn.LanguageServer.Protocol.SumType<
    Roslyn.LanguageServer.Protocol.Location,
    Roslyn.LanguageServer.Protocol.VSInternalLocation,
    Roslyn.LanguageServer.Protocol.VSInternalLocation[],
    Roslyn.LanguageServer.Protocol.DocumentLink[]>;
using ImplementationResult = Roslyn.LanguageServer.Protocol.SumType<
    Roslyn.LanguageServer.Protocol.Location[],
    Roslyn.LanguageServer.Protocol.VSInternalReferenceItem[]>;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public abstract partial class SingleServerDelegatingEndpointTestBase
{
    private protected class TestLanguageServer(
        CSharpTestLspServer csharpServer,
        Uri csharpDocumentUri)
        : IClientConnection, IAsyncDisposable
    {
        private readonly CSharpTestLspServer _csharpServer = csharpServer;
        private readonly Uri _csharpDocumentUri = csharpDocumentUri;
        private readonly CancellationTokenSource _disposeTokenSource = new();

        private int _requestCount;

        public int RequestCount => _requestCount;

        public async ValueTask DisposeAsync()
        {
            if (_disposeTokenSource.IsCancellationRequested)
            {
                return;
            }

            _disposeTokenSource.Cancel();
            _disposeTokenSource.Dispose();

            await _csharpServer.DisposeAsync().ConfigureAwait(false);
        }

        public async Task<TResponse> SendRequestAsync<TParams, TResponse>(string method, TParams @params, CancellationToken cancellationToken)
        {
            _requestCount++;

            object? result = method switch
            {
                CustomMessageNames.RazorDefinitionEndpointName => await HandleDefinitionAsync(@params, cancellationToken),
                CustomMessageNames.RazorImplementationEndpointName => await HandleImplementationAsync(@params, cancellationToken),
                CustomMessageNames.RazorSignatureHelpEndpointName => await HandleSignatureHelpAsync(@params, cancellationToken),
                CustomMessageNames.RazorRenameEndpointName => await HandleRenameAsync(@params, cancellationToken),
                CustomMessageNames.RazorOnAutoInsertEndpointName => await HandleOnAutoInsertAsync(@params, cancellationToken),
                CustomMessageNames.RazorValidateBreakpointRangeName => await HandleValidateBreakpointRangeAsync(@params, cancellationToken),
                CustomMessageNames.RazorDataTipRangeName => await HandleDataTipRangeAsync(@params, cancellationToken),
                CustomMessageNames.RazorReferencesEndpointName => await HandleReferencesAsync(@params, cancellationToken),
                CustomMessageNames.RazorProvideCodeActionsEndpoint => await HandleProvideCodeActionsAsync(@params, cancellationToken),
                CustomMessageNames.RazorResolveCodeActionsEndpoint => await HandleResolveCodeActionsAsync(@params, cancellationToken),
                CustomMessageNames.RazorPullDiagnosticEndpointName => await HandlePullDiagnosticsAsync(@params, cancellationToken),
                CustomMessageNames.RazorFoldingRangeEndpoint => await HandleFoldingRangeAsync(),
                CustomMessageNames.RazorSpellCheckEndpoint => await HandleSpellCheckAsync(@params, cancellationToken),
                CustomMessageNames.RazorDocumentSymbolEndpoint => await HandleDocumentSymbolAsync(@params, cancellationToken),
                CustomMessageNames.RazorProjectContextsEndpoint => await HandleProjectContextsAsync(@params, cancellationToken),
                CustomMessageNames.RazorSimplifyMethodEndpointName => HandleSimplifyMethod(@params),
                CustomMessageNames.RazorInlayHintEndpoint => await HandleInlayHintAsync(@params, cancellationToken),
                CustomMessageNames.RazorInlayHintResolveEndpoint => await HandleInlayHintResolveAsync(@params, cancellationToken),
                CustomMessageNames.RazorCSharpPullDiagnosticsEndpointName => await HandleCSharpDiagnosticsAsync(@params, cancellationToken),

                _ => throw new NotSupportedException($"I don't know how to handle the '{method}' method.")
            };

            return (TResponse)result!;
        }

        private Task<SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?> HandleCSharpDiagnosticsAsync<TParams>(TParams @params, CancellationToken cancellationToken)
        {
            Assert.IsType<DelegatedDiagnosticParams>(@params);
            var actualParams = new DocumentDiagnosticParams()
            {
                TextDocument = new TextDocumentIdentifier { DocumentUri = new(_csharpDocumentUri) }
            };

            return _csharpServer.ExecuteRequestAsync<DocumentDiagnosticParams, SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?>(
                Methods.TextDocumentDiagnosticName,
                actualParams,
                cancellationToken);
        }

        private static TextEdit[]? HandleSimplifyMethod<TParams>(TParams @params)
        {
            Assert.IsType<DelegatedSimplifyMethodParams>(@params);
            return null;
        }

        private Task<VSProjectContextList> HandleProjectContextsAsync<TParams>(TParams @params, CancellationToken cancellationToken)
        {
            Assert.IsType<DelegatedProjectContextsParams>(@params);

            var delegatedRequest = new VSGetProjectContextsParams
            {
                TextDocument = new TextDocumentItem
                {
                    DocumentUri = new(_csharpDocumentUri),
                },
            };

            return _csharpServer.ExecuteRequestAsync<VSGetProjectContextsParams, VSProjectContextList>(
                VSMethods.GetProjectContextsName,
                delegatedRequest,
                cancellationToken);
        }

        private Task<InlayHint[]> HandleInlayHintAsync<TParams>(TParams @params, CancellationToken cancellationToken)
        {
            var delegatedParams = Assert.IsType<DelegatedInlayHintParams>(@params);

            var delegatedRequest = new InlayHintParams
            {
                TextDocument = new TextDocumentIdentifier
                {
                    DocumentUri = new(_csharpDocumentUri),
                },
                Range = delegatedParams.ProjectedRange
            };

            return _csharpServer.ExecuteRequestAsync<InlayHintParams, InlayHint[]>(
                Methods.TextDocumentInlayHintName,
                delegatedRequest,
                cancellationToken);
        }

        private Task<InlayHint> HandleInlayHintResolveAsync<TParams>(TParams @params, CancellationToken cancellationToken)
        {
            var delegatedParams = Assert.IsType<DelegatedInlayHintResolveParams>(@params);

            var delegatedRequest = delegatedParams.InlayHint;

            return _csharpServer.ExecuteRequestAsync<InlayHint, InlayHint>(
                Methods.InlayHintResolveName,
                delegatedRequest,
                cancellationToken);
        }

        private Task<SumType<DocumentSymbol[], SymbolInformation[]>?> HandleDocumentSymbolAsync<TParams>(TParams @params, CancellationToken cancellationToken)
        {
            Assert.IsType<DelegatedDocumentSymbolParams>(@params);

            var delegatedRequest = new DocumentSymbolParams
            {
                TextDocument = new TextDocumentIdentifier
                {
                    DocumentUri = new(_csharpDocumentUri),
                },
            };

            return _csharpServer.ExecuteRequestAsync<DocumentSymbolParams, SumType<DocumentSymbol[], SymbolInformation[]>?>(
                Methods.TextDocumentDocumentSymbolName,
                delegatedRequest,
                cancellationToken);
        }

        private Task<VSInternalSpellCheckableRangeReport[]> HandleSpellCheckAsync<TParams>(TParams @params, CancellationToken cancellationToken)
        {
            var delegatedParams = Assert.IsType<DelegatedSpellCheckParams>(@params);

            var delegatedRequest = new VSInternalDocumentSpellCheckableParams
            {
                TextDocument = new VSTextDocumentIdentifier
                {
                    DocumentUri = new(_csharpDocumentUri),
                    ProjectContext = delegatedParams.Identifier.TextDocumentIdentifier.GetProjectContext(),
                },
            };

            return _csharpServer.ExecuteRequestAsync<VSInternalDocumentSpellCheckableParams, VSInternalSpellCheckableRangeReport[]>(
                VSInternalMethods.TextDocumentSpellCheckableRangesName,
                delegatedRequest,
                cancellationToken);
        }

        private async Task<RazorPullDiagnosticResponse> HandlePullDiagnosticsAsync<TParams>(TParams @params, CancellationToken cancellationToken)
        {
            var delegatedParams = Assert.IsType<DelegatedDiagnosticParams>(@params);

            var delegatedRequest = new VSInternalDocumentDiagnosticsParams
            {
                TextDocument = new VSTextDocumentIdentifier
                {
                    DocumentUri = new(_csharpDocumentUri),
                    ProjectContext = delegatedParams.Identifier.TextDocumentIdentifier.GetProjectContext(),
                },
            };

            var result = await _csharpServer.ExecuteRequestAsync<VSInternalDocumentDiagnosticsParams, VSInternalDiagnosticReport[]>(
                VSInternalMethods.DocumentPullDiagnosticName,
                delegatedRequest,
                cancellationToken);

            return new RazorPullDiagnosticResponse(result, []);
        }

        private static Task<RazorFoldingRangeResponse> HandleFoldingRangeAsync()
        {
            return Task.FromResult(RazorFoldingRangeResponse.Empty);
        }

        private Task<VSInternalCodeAction> HandleResolveCodeActionsAsync<TParams>(TParams @params, CancellationToken cancellationToken)
        {
            var delegatedParams = Assert.IsType<RazorResolveCodeActionParams>(@params);

            var delegatedRequest = delegatedParams.CodeAction;

            return _csharpServer.ExecuteRequestAsync<CodeAction, VSInternalCodeAction>(
                Methods.CodeActionResolveName,
                delegatedRequest,
                cancellationToken);
        }

        private Task<RazorVSInternalCodeAction[]> HandleProvideCodeActionsAsync<TParams>(TParams @params, CancellationToken cancellationToken)
        {
            var delegatedParams = Assert.IsType<DelegatedCodeActionParams>(@params);

            var delegatedRequest = delegatedParams.CodeActionParams;
            delegatedRequest.TextDocument.DocumentUri = new(_csharpDocumentUri);

            return _csharpServer.ExecuteRequestAsync<VSCodeActionParams, RazorVSInternalCodeAction[]>(
                Methods.TextDocumentCodeActionName,
                delegatedRequest,
                cancellationToken);
        }

        private Task<VSInternalReferenceItem[]> HandleReferencesAsync<TParams>(TParams @params, CancellationToken cancellationToken)
        {
            var delegatedParams = Assert.IsType<DelegatedPositionParams>(@params);
            var delegatedRequest = new ReferenceParams()
            {
                TextDocument = new VSTextDocumentIdentifier()
                {
                    DocumentUri = new(_csharpDocumentUri),
                    ProjectContext = delegatedParams.Identifier.TextDocumentIdentifier.GetProjectContext(),
                },
                Position = delegatedParams.ProjectedPosition,
                Context = new ReferenceContext()
            };

            return _csharpServer.ExecuteRequestAsync<ReferenceParams, VSInternalReferenceItem[]>(
                Methods.TextDocumentReferencesName,
                delegatedRequest,
                cancellationToken);
        }

        private Task<DefinitionResult?> HandleDefinitionAsync<T>(T @params, CancellationToken cancellationToken)
        {
            var delegatedParams = Assert.IsType<DelegatedPositionParams>(@params);
            var delegatedRequest = new TextDocumentPositionParams()
            {
                TextDocument = new VSTextDocumentIdentifier()
                {
                    DocumentUri = new(_csharpDocumentUri),
                    ProjectContext = delegatedParams.Identifier.TextDocumentIdentifier.GetProjectContext(),
                },
                Position = delegatedParams.ProjectedPosition
            };

            return _csharpServer.ExecuteRequestAsync<TextDocumentPositionParams, DefinitionResult?>(
                Methods.TextDocumentDefinitionName,
                delegatedRequest,
                cancellationToken);
        }

        private Task<ImplementationResult> HandleImplementationAsync<T>(T @params, CancellationToken cancellationToken)
        {
            var delegatedParams = Assert.IsType<DelegatedPositionParams>(@params);
            var delegatedRequest = new TextDocumentPositionParams()
            {
                TextDocument = new VSTextDocumentIdentifier()
                {
                    DocumentUri = new(_csharpDocumentUri),
                    ProjectContext = delegatedParams.Identifier.TextDocumentIdentifier.GetProjectContext(),
                },
                Position = delegatedParams.ProjectedPosition
            };

            return _csharpServer.ExecuteRequestAsync<TextDocumentPositionParams, ImplementationResult>(
                Methods.TextDocumentImplementationName,
                delegatedRequest,
                cancellationToken);
        }

        private Task<LspSignatureHelp> HandleSignatureHelpAsync<T>(T @params, CancellationToken cancellationToken)
        {
            var delegatedParams = Assert.IsType<DelegatedPositionParams>(@params);
            var delegatedRequest = new SignatureHelpParams()
            {
                TextDocument = new VSTextDocumentIdentifier()
                {
                    DocumentUri = new(_csharpDocumentUri),
                    ProjectContext = delegatedParams.Identifier.TextDocumentIdentifier.GetProjectContext(),
                },
                Position = delegatedParams.ProjectedPosition,
            };

            return _csharpServer.ExecuteRequestAsync<SignatureHelpParams, LspSignatureHelp>(
                Methods.TextDocumentSignatureHelpName,
                delegatedRequest,
                cancellationToken);
        }

        private Task<WorkspaceEdit> HandleRenameAsync<T>(T @params, CancellationToken cancellationToken)
        {
            var delegatedParams = Assert.IsType<DelegatedRenameParams>(@params);
            var delegatedRequest = new RenameParams()
            {
                TextDocument = new VSTextDocumentIdentifier()
                {
                    DocumentUri = new(_csharpDocumentUri),
                    ProjectContext = delegatedParams.Identifier.TextDocumentIdentifier.GetProjectContext(),
                },
                Position = delegatedParams.ProjectedPosition,
                NewName = delegatedParams.NewName,
            };

            return _csharpServer.ExecuteRequestAsync<RenameParams, WorkspaceEdit>(
                Methods.TextDocumentRenameName,
                delegatedRequest,
                cancellationToken);
        }

        private Task<VSInternalDocumentOnAutoInsertResponseItem> HandleOnAutoInsertAsync<T>(T @params, CancellationToken cancellationToken)
        {
            var delegatedParams = Assert.IsType<DelegatedOnAutoInsertParams>(@params);
            var delegatedRequest = new VSInternalDocumentOnAutoInsertParams()
            {
                TextDocument = new VSTextDocumentIdentifier()
                {
                    DocumentUri = new(_csharpDocumentUri),
                    ProjectContext = delegatedParams.Identifier.TextDocumentIdentifier.GetProjectContext()
                },
                Position = delegatedParams.ProjectedPosition,
                Character = delegatedParams.Character,
                Options = delegatedParams.Options
            };

            return _csharpServer.ExecuteRequestAsync<VSInternalDocumentOnAutoInsertParams, VSInternalDocumentOnAutoInsertResponseItem>(
                VSInternalMethods.OnAutoInsertName,
                delegatedRequest,
                cancellationToken);
        }

        public Task SendNotificationAsync<TParams>(string method, TParams @params, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SendNotificationAsync(string method, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private Task<LspRange> HandleValidateBreakpointRangeAsync<T>(T @params, CancellationToken cancellationToken)
        {
            var delegatedParams = Assert.IsType<DelegatedValidateBreakpointRangeParams>(@params);
            var delegatedRequest = new VSInternalValidateBreakableRangeParams()
            {
                TextDocument = new VSTextDocumentIdentifier()
                {
                    DocumentUri = new(_csharpDocumentUri),
                    ProjectContext = delegatedParams.Identifier.TextDocumentIdentifier.GetProjectContext(),
                },
                Range = delegatedParams.ProjectedRange,
            };

            return _csharpServer.ExecuteRequestAsync<VSInternalValidateBreakableRangeParams, LspRange>(
                VSInternalMethods.TextDocumentValidateBreakableRangeName, delegatedRequest, cancellationToken);
        }

        private Task<VSInternalDataTip> HandleDataTipRangeAsync<T>(T @params, CancellationToken cancellationToken)
        {
            var delegatedParams = Assert.IsType<DelegatedPositionParams>(@params);
            var delegatedRequest = new TextDocumentPositionParams()
            {
                TextDocument = new VSTextDocumentIdentifier()
                {
                    DocumentUri = new(_csharpDocumentUri),
                    ProjectContext = delegatedParams.Identifier.TextDocumentIdentifier.GetProjectContext(),
                },
                Position = delegatedParams.ProjectedPosition,
            };

            return _csharpServer.ExecuteRequestAsync<TextDocumentPositionParams, VSInternalDataTip>(
                VSInternalMethods.TextDocumentDataTipRangeName, delegatedRequest, cancellationToken);
        }
    }
}
