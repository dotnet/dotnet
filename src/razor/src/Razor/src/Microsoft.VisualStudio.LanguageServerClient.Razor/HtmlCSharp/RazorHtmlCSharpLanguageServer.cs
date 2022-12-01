// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts.WrapWithTag;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Logging;
using Newtonsoft.Json.Linq;
using StreamJsonRpc;
using ImplementationResult = Microsoft.VisualStudio.LanguageServer.Protocol.SumType<
    Microsoft.VisualStudio.LanguageServer.Protocol.Location[],
    Microsoft.VisualStudio.LanguageServer.Protocol.VSInternalReferenceItem[]>;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;

internal class RazorHtmlCSharpLanguageServer : IDisposable
{
    private readonly JsonRpc _jsonRpc;
    private readonly ImmutableDictionary<string, Lazy<IRequestHandler, IRequestHandlerMetadata>> _requestHandlers;
    private VSInternalClientCapabilities? _clientCapabilities;

    private RazorHtmlCSharpLanguageServer(
        Stream inputStream,
        Stream outputStream,
        IEnumerable<Lazy<IRequestHandler, IRequestHandlerMetadata>> requestHandlers,
        HTMLCSharpLanguageServerLogHubLoggerProvider loggerProvider) : this(requestHandlers)
    {
        _jsonRpc = CreateJsonRpc(outputStream, inputStream, target: this);

        // Facilitates activity based tracing for structured logging within LogHub
        var traceSource = loggerProvider.GetTraceSource();
        _jsonRpc.ActivityTracingStrategy = new CorrelationManagerTracingStrategy
        {
            TraceSource = traceSource
        };
        _jsonRpc.TraceSource = traceSource;

        _jsonRpc.StartListening();
    }

    private VSInternalClientCapabilities ClientCapabilities
    {
        get
        {
            if (_clientCapabilities is null)
            {
                throw new InvalidOperationException("Client capabilities have not been provided prior to request");
            }

            return _clientCapabilities;
        }
    }

    public static async Task<RazorHtmlCSharpLanguageServer> CreateAsync(
        Stream inputStream,
        Stream outputStream,
        IEnumerable<Lazy<IRequestHandler, IRequestHandlerMetadata>> requestHandlers,
        HTMLCSharpLanguageServerLogHubLoggerProvider loggerProvider,
        CancellationToken cancellationToken)
    {
        if (inputStream is null)
        {
            throw new ArgumentNullException(nameof(inputStream));
        }

        if (outputStream is null)
        {
            throw new ArgumentNullException(nameof(outputStream));
        }

        if (loggerProvider is null)
        {
            throw new ArgumentNullException(nameof(loggerProvider));
        }

        // Wait for logging infrastructure to initialize. This must be completed
        // before we can start listening via Json RPC (as we must link the log hub
        // trace source with Json RPC to facilitate structured logging / activity tracing).
        await loggerProvider.InitializeLoggerAsync(cancellationToken).ConfigureAwait(false);

        return new RazorHtmlCSharpLanguageServer(inputStream, outputStream, requestHandlers, loggerProvider);
    }

    // Test constructor
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    internal RazorHtmlCSharpLanguageServer(IEnumerable<Lazy<IRequestHandler, IRequestHandlerMetadata>> requestHandlers)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        if (requestHandlers is null)
        {
            throw new ArgumentNullException(nameof(requestHandlers));
        }

        _requestHandlers = CreateMethodToHandlerMap(requestHandlers);
    }

    [JsonRpcMethod(Methods.InitializeName)]
    public Task<InitializeResult?> InitializeAsync(JToken input, CancellationToken cancellationToken)
    {
        if (input is null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        // InitializeParams only references ClientCapabilities, but the VS LSP client
        // sends additional VS specific capabilities, so directly deserialize them into the VSInternalClientCapabilities
        // to avoid losing them.
        _clientCapabilities = input["capabilities"]?.ToObject<VSInternalClientCapabilities>();
        if (_clientCapabilities is null)
        {
            throw new InvalidOperationException("Client capabilities failed to deserialize");
        }

        var initializeParams = input.ToObject<InitializeParams>();
        if (initializeParams is null)
        {
            throw new InvalidOperationException("Initialize params failed to deserialize");
        }

        return ExecuteRequestAsync<InitializeParams, InitializeResult>(Methods.InitializeName, initializeParams, ClientCapabilities, cancellationToken);
    }

    [JsonRpcMethod(Methods.ShutdownName)]
    public Task ShutdownAsync(CancellationToken _)
    {
        // Nothing to detatch to yet.

        return Task.CompletedTask;
    }

    [JsonRpcMethod(Methods.ExitName)]
    public Task ExitAsync(CancellationToken _)
    {
        Dispose();

        return Task.CompletedTask;
    }

    [JsonRpcMethod(Methods.TextDocumentCompletionName, UseSingleObjectParameterDeserialization = true)]
    public Task<SumType<CompletionItem[], CompletionList>?> ProvideCompletionsAsync(CompletionParams completionParams, CancellationToken cancellationToken)
    {
        if (completionParams is null)
        {
            throw new ArgumentNullException(nameof(completionParams));
        }

        return ExecuteRequestAsync<CompletionParams, SumType<CompletionItem[], CompletionList>?>(Methods.TextDocumentCompletionName, completionParams, ClientCapabilities, cancellationToken);
    }

    [JsonRpcMethod(Methods.TextDocumentHoverName, UseSingleObjectParameterDeserialization = true)]
    public Task<Hover?> ProvideHoverAsync(TextDocumentPositionParams positionParams, CancellationToken cancellationToken)
    {
        if (positionParams is null)
        {
            throw new ArgumentNullException(nameof(positionParams));
        }

        return ExecuteRequestAsync<TextDocumentPositionParams, Hover>(Methods.TextDocumentHoverName, positionParams, ClientCapabilities, cancellationToken);
    }

    [JsonRpcMethod(Methods.TextDocumentCompletionResolveName, UseSingleObjectParameterDeserialization = true)]
    public Task<CompletionItem?> ResolveCompletionAsync(CompletionItem request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        return ExecuteRequestAsync<CompletionItem, CompletionItem>(Methods.TextDocumentCompletionResolveName, request, ClientCapabilities, cancellationToken);
    }

    [JsonRpcMethod(VSInternalMethods.OnAutoInsertName, UseSingleObjectParameterDeserialization = true)]
    public Task<VSInternalDocumentOnAutoInsertResponseItem?> OnAutoInsertAsync(VSInternalDocumentOnAutoInsertParams request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        return ExecuteRequestAsync<VSInternalDocumentOnAutoInsertParams, VSInternalDocumentOnAutoInsertResponseItem?>(VSInternalMethods.OnAutoInsertName, request, ClientCapabilities, cancellationToken);
    }

    [JsonRpcMethod(Methods.TextDocumentOnTypeFormattingName, UseSingleObjectParameterDeserialization = true)]
    public Task<TextEdit[]?> OnTypeFormattingAsync(DocumentOnTypeFormattingParams request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        return ExecuteRequestAsync<DocumentOnTypeFormattingParams, TextEdit[]>(Methods.TextDocumentOnTypeFormattingName, request, ClientCapabilities, cancellationToken);
    }

    [JsonRpcMethod(Methods.TextDocumentLinkedEditingRangeName, UseSingleObjectParameterDeserialization = true)]
    public Task<LinkedEditingRanges?> OnLinkedEditingRangeAsync(LinkedEditingRangeParams request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        return ExecuteRequestAsync<LinkedEditingRangeParams, LinkedEditingRanges>(Methods.TextDocumentLinkedEditingRangeName, request, ClientCapabilities, cancellationToken);
    }

    [JsonRpcMethod(Methods.TextDocumentDefinitionName, UseSingleObjectParameterDeserialization = true)]
    public Task<Location[]?> GoToDefinitionAsync(TextDocumentPositionParams positionParams, CancellationToken cancellationToken)
    {
        if (positionParams is null)
        {
            throw new ArgumentNullException(nameof(positionParams));
        }

        return ExecuteRequestAsync<TextDocumentPositionParams, Location[]>(Methods.TextDocumentDefinitionName, positionParams, ClientCapabilities, cancellationToken);
    }

    [JsonRpcMethod(Methods.TextDocumentReferencesName, UseSingleObjectParameterDeserialization = true)]
    public Task<VSInternalReferenceItem[]?> FindAllReferencesAsync(VSInternalReferenceParams referenceParams, CancellationToken cancellationToken)
    {
        if (referenceParams is null)
        {
            throw new ArgumentNullException(nameof(referenceParams));
        }

        return ExecuteRequestAsync<ReferenceParams, VSInternalReferenceItem[]>(Methods.TextDocumentReferencesName, referenceParams, ClientCapabilities, cancellationToken);
    }

    [JsonRpcMethod(Methods.TextDocumentSignatureHelpName, UseSingleObjectParameterDeserialization = true)]
    public Task<SignatureHelp?> SignatureHelpAsync(TextDocumentPositionParams positionParams, CancellationToken cancellationToken)
    {
        if (positionParams is null)
        {
            throw new ArgumentNullException(nameof(positionParams));
        }

        return ExecuteRequestAsync<TextDocumentPositionParams, SignatureHelp>(Methods.TextDocumentSignatureHelpName, positionParams, ClientCapabilities, cancellationToken);
    }

    [JsonRpcMethod(Methods.TextDocumentDocumentHighlightName, UseSingleObjectParameterDeserialization = true)]
    public Task<DocumentHighlight[]?> HighlightDocumentAsync(DocumentHighlightParams documentHighlightParams, CancellationToken cancellationToken)
    {
        if (documentHighlightParams is null)
        {
            throw new ArgumentNullException(nameof(documentHighlightParams));
        }

        return ExecuteRequestAsync<DocumentHighlightParams, DocumentHighlight[]>(Methods.TextDocumentDocumentHighlightName, documentHighlightParams, ClientCapabilities, cancellationToken);
    }

    [JsonRpcMethod(Methods.TextDocumentRenameName, UseSingleObjectParameterDeserialization = true)]
    public Task<WorkspaceEdit?> RenameAsync(RenameParams renameParams, CancellationToken cancellationToken)
    {
        if (renameParams is null)
        {
            throw new ArgumentNullException(nameof(renameParams));
        }

        return ExecuteRequestAsync<RenameParams, WorkspaceEdit?>(Methods.TextDocumentRenameName, renameParams, ClientCapabilities, cancellationToken);
    }

    [JsonRpcMethod(Methods.TextDocumentImplementationName, UseSingleObjectParameterDeserialization = true)]
    public Task<ImplementationResult> GoToImplementationAsync(TextDocumentPositionParams positionParams, CancellationToken cancellationToken)
    {
        if (positionParams is null)
        {
            throw new ArgumentNullException(nameof(positionParams));
        }

        return ExecuteRequestAsync<TextDocumentPositionParams, ImplementationResult>(Methods.TextDocumentImplementationName, positionParams, ClientCapabilities, cancellationToken);
    }

    [JsonRpcMethod(VSInternalMethods.DocumentPullDiagnosticName, UseSingleObjectParameterDeserialization = true)]
    public Task<IReadOnlyList<VSInternalDiagnosticReport>?> DocumentPullDiagnosticsAsync(VSInternalDocumentDiagnosticsParams documentDiagnosticsParams, CancellationToken cancellationToken)
    {
        if (documentDiagnosticsParams is null)
        {
            throw new ArgumentNullException(nameof(documentDiagnosticsParams));
        }

        return ExecuteRequestAsync<VSInternalDocumentDiagnosticsParams, IReadOnlyList<VSInternalDiagnosticReport>>(VSInternalMethods.DocumentPullDiagnosticName, documentDiagnosticsParams, ClientCapabilities, cancellationToken);
    }

#pragma warning disable IDE0060 // Remove unused parameter
    // Razor tooling doesn't utilize workspace pull diagnostics as it doesn't really make sense for our use case.
    // However, without the workspace pull diagnostics endpoint, a bunch of unnecessary exceptions are
    // triggered. Thus we add the following no-op handler until a server capability is available.
    // Having a server capability would reduce overhead of sending/receiving the request and the
    // associated serialization/deserialization.
    [JsonRpcMethod(VSInternalMethods.WorkspacePullDiagnosticName, UseSingleObjectParameterDeserialization = true)]
    public static Task<VSInternalWorkspaceDiagnosticReport?> WorkspacePullDiagnosticsAsync(VSInternalWorkspaceDiagnosticsParams workspaceDiagnosticsParams, CancellationToken cancellationToken)
    {
        return Task.FromResult<VSInternalWorkspaceDiagnosticReport?>(null);
    }

    // Workaround for https://devdiv.visualstudio.com/DevDiv/_workitems/edit/1445500
    // The Web Tools WrapWithTag handler sends messages to all of the LSP servers attached to the buffer
    // and we respond correctly from the RazorLanguageServer, but unless we implement a handler here, the
    // platform will get a "Request Method Not Found" exception, and throw away the real result from the Razor
    // server entirely.
    [JsonRpcMethod(LanguageServerConstants.RazorWrapWithTagEndpoint, UseSingleObjectParameterDeserialization = true)]
    public static Task<WrapWithTagResponse?> WrapWithTagAsync(WrapWithTagParams workspaceDiagnosticsParams, CancellationToken cancellationToken)
    {
        return Task.FromResult<WrapWithTagResponse?>(null);
    }
#pragma warning restore IDE0060 // Remove unused parameter

    // Internal for testing
    internal Task<ResponseType?> ExecuteRequestAsync<RequestType, ResponseType>(
        string methodName,
        RequestType request,
        ClientCapabilities clientCapabilities,
        CancellationToken cancellationToken) where RequestType : class
    {
        if (string.IsNullOrEmpty(methodName))
        {
            throw new ArgumentException("Invalid method name", nameof(methodName));
        }

        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (!_requestHandlers.TryGetValue(methodName, out var lazyHandler))
        {
            throw new InvalidOperationException($"Request handler not found for method {methodName}");
        }

        var handler = (IRequestHandler<RequestType, ResponseType>)lazyHandler.Value;
        return handler.HandleRequestAsync(request, clientCapabilities, cancellationToken);
    }

    private static JsonRpc CreateJsonRpc(Stream outputStream, Stream inputStream, object target)
    {
#pragma warning disable CA2000 // Dispose objects before losing scope
        var messageFormatter = new JsonMessageFormatter();
#pragma warning restore CA2000 // Dispose objects before losing scope

        var serializer = messageFormatter.JsonSerializer;
        serializer.AddVSInternalExtensionConverters();

#pragma warning disable CA2000 // Dispose objects before losing scope
        var messageHandler = new HeaderDelimitedMessageHandler(outputStream, inputStream, messageFormatter);
#pragma warning restore CA2000 // Dispose objects before losing scope

        // The JsonRpc object owns disposing the message handler which disposes the formatter.
        var jsonRpc = new JsonRpc(messageHandler, target);
        return jsonRpc;
    }

    private static ImmutableDictionary<string, Lazy<IRequestHandler, IRequestHandlerMetadata>> CreateMethodToHandlerMap(IEnumerable<Lazy<IRequestHandler, IRequestHandlerMetadata>> requestHandlers)
    {
        var requestHandlerDictionary = ImmutableDictionary.CreateBuilder<string, Lazy<IRequestHandler, IRequestHandlerMetadata>>();
        foreach (var lazyHandler in requestHandlers)
        {
            requestHandlerDictionary.Add(lazyHandler.Metadata.MethodName, lazyHandler);
        }

        return requestHandlerDictionary.ToImmutable();
    }

    public void Dispose()
    {
        try
        {
            if (!_jsonRpc.IsDisposed)
            {
                _jsonRpc.Dispose();
            }
        }
        catch (Exception)
        {
            // Swallow exceptions thrown by disposing our JsonRpc object. Disconnected events can potentially throw their own exceptions so
            // we purposefully ignore all of those exceptions in an effort to shutdown gracefully.
        }
    }
}
