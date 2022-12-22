// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.LanguageServer.Client;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Threading;
using Newtonsoft.Json.Linq;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;

internal class TestLanguageServiceBroker : ILanguageServiceBroker2
{
    private readonly Action<string> _callback;

#pragma warning disable CS0067 // The event is never used
    public event EventHandler<LanguageClientLoadedEventArgs> LanguageClientLoaded;
    public event AsyncEventHandler<LanguageClientNotifyEventArgs> ClientNotifyAsync;
#pragma warning restore CS0067 // The event is never used

    public IEnumerable<ILanguageClientInstance> ActiveLanguageClients => throw new NotImplementedException();

    public IStreamingRequestBroker<CompletionParams, CompletionList> CompletionBroker => throw new NotImplementedException();

    public IRequestBroker<CompletionItem, CompletionItem> CompletionResolveBroker => throw new NotImplementedException();

    public IStreamingRequestBroker<ReferenceParams, object[]> ReferencesBroker => throw new NotImplementedException();

    public IRequestBroker<TextDocumentPositionParams, object[]> ImplementationBroker => throw new NotImplementedException();

    public IRequestBroker<TextDocumentPositionParams, object[]> TypeDefinitionBroker => throw new NotImplementedException();

    public IRequestBroker<TextDocumentPositionParams, object[]> DefinitionBroker => throw new NotImplementedException();

    public IRequestBroker<TextDocumentPositionParams, Hover> HoverBroker => throw new NotImplementedException();

    public IRequestBroker<RenameParams, WorkspaceEdit> RenameBroker => throw new NotImplementedException();

    public IRequestBroker<DocumentFormattingParams, TextEdit[]> DocumentFormattingBroker => throw new NotImplementedException();

    public IRequestBroker<DocumentRangeFormattingParams, TextEdit[]> RangeFormattingBroker => throw new NotImplementedException();

    public IRequestBroker<DocumentOnTypeFormattingParams, TextEdit[]> OnTypeFormattingBroker => throw new NotImplementedException();

    public IRequestBroker<ExecuteCommandParams, object> ExecuteCommandBroker => throw new NotImplementedException();

    public IRequestBroker<CodeActionParams, SumType<Command, CodeAction>[]> CodeActionsBroker => throw new NotImplementedException();

    public IStreamingRequestBroker<DocumentHighlightParams, DocumentHighlight[]> DocumentHighlightBroker => throw new NotImplementedException();

    public IRequestBroker<SignatureHelpParams, SignatureHelp> SignatureHelpBroker => throw new NotImplementedException();

    public IRequestBroker<DocumentSymbolParams, SymbolInformation[]> DocumentSymbolBroker => throw new NotImplementedException();

    public IStreamingRequestBroker<WorkspaceSymbolParams, SymbolInformation[]> WorkspaceSymbolBroker => throw new NotImplementedException();

    public IRequestBroker<FoldingRangeParams, FoldingRange[]> FoldingRangeBroker => throw new NotImplementedException();

    public IEnumerable<Lazy<ILanguageClient, IContentTypeMetadata>> FactoryLanguageClients => throw new NotImplementedException();

    public IEnumerable<Lazy<ILanguageClient, IContentTypeMetadata>> LanguageClients => throw new NotImplementedException();

    IRequestBroker<VSInternalKindAndModifier, VSInternalIconMapping> ILanguageServiceBroker.KindDescriptionResolveBroker => throw new NotImplementedException();

    public TestLanguageServiceBroker(Action<string> callback)
    {
        _callback = callback;
    }

    public Task LoadAsync(ILanguageClientMetadata metadata, ILanguageClient client)
    {
        throw new NotImplementedException();
    }

    public Task<(ILanguageClient, JToken)> RequestAsync(
        string[] contentTypes,
        Func<JToken, bool> capabilitiesFilter,
        string method,
        JToken parameters,
        CancellationToken cancellationToken)
    {
        _callback?.Invoke(method);

        return Task.FromResult<(ILanguageClient, JToken)>((null, null));
    }

    public Task<(ILanguageClient, JToken)> RequestAsync(string[] contentTypes, Func<JToken, bool> capabilitiesFilter, string clientName, string method, JToken parameters, CancellationToken cancellationToken)
    {
        _callback?.Invoke(method);

        return Task.FromResult<(ILanguageClient, JToken)>((null, null));
    }

    public IEnumerable<(Uri, JToken)> GetAllDiagnostics()
    {
        throw new NotImplementedException();
    }

    public JToken GetDiagnostics(Uri uri)
    {
        throw new NotImplementedException();
    }

    public Task<JToken> RequestAsync(ILanguageClient languageClient, string method, JToken parameters, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<(ILanguageClient, JToken)>> RequestMultipleAsync(string[] contentTypes, Func<JToken, bool> capabilitiesFilter, string method, JToken parameters, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public void AddCustomBufferContentTypes(IEnumerable<string> contentTypes)
    {
        throw new NotImplementedException();
    }

    public void RemoveCustomBufferContentTypes(IEnumerable<string> contentTypes)
    {
        throw new NotImplementedException();
    }

    public void AddLanguageClients(IEnumerable<Lazy<ILanguageClient, IContentTypeMetadata>> items)
    {
        throw new NotImplementedException();
    }

    public void RemoveLanguageClients(IEnumerable<Lazy<ILanguageClient, IContentTypeMetadata>> items)
    {
        throw new NotImplementedException();
    }

    public Task LoadAsync(IContentTypeMetadata contentType, ILanguageClient client)
    {
        throw new NotImplementedException();
    }

    public Task OnDidOpenTextDocumentAsync(ITextSnapshot snapShot)
    {
        throw new NotImplementedException();
    }

    public Task OnDidCloseTextDocumentAsync(ITextSnapshot snapShot)
    {
        throw new NotImplementedException();
    }

    public Task OnDidChangeTextDocumentAsync(ITextSnapshot before, ITextSnapshot after, IEnumerable<ITextChange> textChanges)
    {
        throw new NotImplementedException();
    }

    public Task OnDidSaveTextDocumentAsync(ITextDocument document)
    {
        throw new NotImplementedException();
    }

    public Task<(ILanguageClient, TOut)> RequestAsync<TIn, TOut>(string[] contentTypes, Func<ServerCapabilities, bool> capabilitiesFilter, LspRequest<TIn, TOut> method, TIn parameters, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<TOut> RequestAsync<TIn, TOut>(ILanguageClient languageClient, LspRequest<TIn, TOut> method, TIn parameters, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<(ILanguageClient, TOut)>> RequestMultipleAsync<TIn, TOut>(string[] contentTypes, Func<ServerCapabilities, bool> capabilitiesFilter, LspRequest<TIn, TOut> method, TIn parameters, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ManualInvocationResponse> RequestAsync(ITextBuffer textBuffer, Func<JToken, bool> capabilitiesFilter, string languageServerName, string method, Func<ITextSnapshot, JToken> parameterFactory, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<ManualInvocationResponse> RequestMultipleAsync(ITextBuffer textBuffer, Func<JToken, bool> capabilitiesFilter, string method, Func<ITextSnapshot, JToken> parameterFactory, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
