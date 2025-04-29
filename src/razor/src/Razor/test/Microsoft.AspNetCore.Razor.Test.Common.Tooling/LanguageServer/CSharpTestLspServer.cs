// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.VisualStudio.Composition;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Nerdbank.Streams;
using StreamJsonRpc;
using Range = Microsoft.VisualStudio.LanguageServer.Protocol.Range;

namespace Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;

public sealed class CSharpTestLspServer : IAsyncDisposable
{
    private readonly AdhocWorkspace _testWorkspace;
    private readonly ExportProvider _exportProvider;

    private readonly JsonRpc _clientRpc;
    private readonly JsonRpc _serverRpc;

    private readonly SystemTextJsonFormatter _clientMessageFormatter;
    private readonly SystemTextJsonFormatter _serverMessageFormatter;
    private readonly HeaderDelimitedMessageHandler _clientMessageHandler;
    private readonly HeaderDelimitedMessageHandler _serverMessageHandler;

    private readonly CancellationToken _cancellationToken;

    private CSharpTestLspServer(
        AdhocWorkspace testWorkspace,
        ExportProvider exportProvider,
        VSInternalServerCapabilities serverCapabilities,
        CancellationToken cancellationToken)
    {
        _testWorkspace = testWorkspace;
        _exportProvider = exportProvider;
        _cancellationToken = cancellationToken;

        var (clientStream, serverStream) = FullDuplexStream.CreatePair();

        var languageServerFactory = exportProvider.GetExportedValue<AbstractRazorLanguageServerFactoryWrapper>();

        _serverMessageFormatter = CreateSystemTextJsonMessageFormatter(languageServerFactory);
        _serverMessageHandler = new HeaderDelimitedMessageHandler(serverStream, serverStream, _serverMessageFormatter);
        _serverRpc = new JsonRpc(_serverMessageHandler)
        {
            ExceptionStrategy = ExceptionProcessing.ISerializable,
        };

        _clientMessageFormatter = CreateSystemTextJsonMessageFormatter(languageServerFactory);
        _clientMessageHandler = new HeaderDelimitedMessageHandler(clientStream, clientStream, _clientMessageFormatter);
        _clientRpc = new JsonRpc(_clientMessageHandler)
        {
            ExceptionStrategy = ExceptionProcessing.ISerializable,
        };

        // Roslyn will call back to us to get configuration options when the server is initialized, so this is how we configure
        // what it options we need
        _clientRpc.AddLocalRpcTarget(new WorkspaceConfigurationHandler());

        _clientRpc.StartListening();

        _ = CreateLanguageServer(_serverRpc, _serverMessageFormatter.JsonSerializerOptions, testWorkspace, languageServerFactory, exportProvider, serverCapabilities);

        static SystemTextJsonFormatter CreateSystemTextJsonMessageFormatter(AbstractRazorLanguageServerFactoryWrapper languageServerFactory)
        {
            var messageFormatter = new SystemTextJsonFormatter();

            // Roslyn has its own converters since it doesn't use MS.VS.LS.Protocol
            languageServerFactory.AddJsonConverters(messageFormatter.JsonSerializerOptions);

            JsonHelpers.AddVSInternalExtensionConverters(messageFormatter.JsonSerializerOptions);

            return messageFormatter;
        }

        static IRazorLanguageServerTarget CreateLanguageServer(
            JsonRpc serverRpc,
            JsonSerializerOptions options,
            Workspace workspace,
            AbstractRazorLanguageServerFactoryWrapper languageServerFactory,
            ExportProvider exportProvider,
            VSInternalServerCapabilities serverCapabilities)
        {
            var capabilitiesProvider = new RazorTestCapabilitiesProvider(serverCapabilities, options);

            var registrationService = exportProvider.GetExportedValue<RazorTestWorkspaceRegistrationService>();
            registrationService.Register(workspace);

            var hostServices = workspace.Services.HostServices;
            var languageServer = languageServerFactory.CreateLanguageServer(serverRpc, options, capabilitiesProvider, hostServices);

            serverRpc.StartListening();
            return languageServer;
        }
    }

    internal static async Task<CSharpTestLspServer> CreateAsync(
        AdhocWorkspace testWorkspace,
        ExportProvider exportProvider,
        ClientCapabilities clientCapabilities,
        VSInternalServerCapabilities serverCapabilities,
        CancellationToken cancellationToken)
    {
        var server = new CSharpTestLspServer(testWorkspace, exportProvider, serverCapabilities, cancellationToken);

        await server.ExecuteRequestAsync<InitializeParams, InitializeResult>(
            Methods.InitializeName,
            new InitializeParams
            {
                Capabilities = clientCapabilities,
            },
            cancellationToken);

        await server.ExecuteRequestAsync(Methods.InitializedName, new InitializedParams(), cancellationToken);

        return server;
    }

    internal Task ExecuteRequestAsync<RequestType>(
        string methodName,
        RequestType request,
        CancellationToken cancellationToken) where RequestType : class
        => _clientRpc.InvokeWithParameterObjectAsync(
            methodName,
            request,
            cancellationToken);

    internal Task<ResponseType> ExecuteRequestAsync<RequestType, ResponseType>(
        string methodName,
        RequestType request,
        CancellationToken cancellationToken)
        => _clientRpc.InvokeWithParameterObjectAsync<ResponseType>(
            methodName,
            request,
            cancellationToken);

    public async ValueTask DisposeAsync()
    {
        _testWorkspace.Dispose();
        _exportProvider.Dispose();

        _clientRpc.Dispose();
        _clientMessageFormatter.Dispose();
        await _clientMessageHandler.DisposeAsync();

        _serverRpc.Dispose();
        _serverMessageFormatter.Dispose();
        await _serverMessageHandler.DisposeAsync();
    }

    #region Document Change Methods

    public async Task OpenDocumentAsync(Uri documentUri, string documentText)
    {
        var didOpenParams = CreateDidOpenTextDocumentParams(documentUri, documentText);
        await ExecuteRequestAsync<DidOpenTextDocumentParams, object>(Methods.TextDocumentDidOpenName, didOpenParams, _cancellationToken);

        static DidOpenTextDocumentParams CreateDidOpenTextDocumentParams(Uri uri, string source)
            => new()
            {
                TextDocument = new TextDocumentItem
                {
                    Text = source,
                    Uri = uri
                }
            };
    }

    public async Task ReplaceTextAsync(Uri documentUri, params (Range Range, string Text)[] changes)
    {
        var didChangeParams = CreateDidChangeTextDocumentParams(
            documentUri,
            changes.Select(change => (change.Range, change.Text)).ToImmutableArray());

        await ExecuteRequestAsync<DidChangeTextDocumentParams, object>(Methods.TextDocumentDidChangeName, didChangeParams, _cancellationToken);

        static DidChangeTextDocumentParams CreateDidChangeTextDocumentParams(Uri documentUri, ImmutableArray<(Range Range, string Text)> changes)
        {
            var changeEvents = changes.Select(change => new TextDocumentContentChangeEvent
            {
                Text = change.Text,
                Range = change.Range,
            }).ToArray();

            return new DidChangeTextDocumentParams()
            {
                TextDocument = new VersionedTextDocumentIdentifier
                {
                    Uri = documentUri
                },
                ContentChanges = changeEvents
            };
        }
    }

    #endregion

    private class RazorTestCapabilitiesProvider(VSInternalServerCapabilities serverCapabilities, JsonSerializerOptions options) : IRazorTestCapabilitiesProvider
    {
        private readonly VSInternalServerCapabilities _serverCapabilities = serverCapabilities;
        private readonly JsonSerializerOptions _options = options;

        public string GetServerCapabilitiesJson(string clientCapabilitiesJson)
        {
            // To avoid exposing types from VS.LSP.Protocol across the Razor <-> Roslyn API boundary, and therefore
            // requiring us to agree on dependency versions, we use JSON as a transport mechanism.
            return JsonSerializer.Serialize(_serverCapabilities, _options);
        }
    }

    private class WorkspaceConfigurationHandler
    {
        [JsonRpcMethod(Methods.WorkspaceConfigurationName, UseSingleObjectParameterDeserialization = true)]
        public string[]? GetConfigurationOptions(ConfigurationParams configurationParams)
        {
            using var _ = ListPool<string>.GetPooledObject(out var values);
            values.SetCapacityIfLarger(configurationParams.Items.Length);

            foreach (var item in configurationParams.Items)
            {
                values.Add(item.Section switch
                {
                    "csharp|inlay_hints.dotnet_enable_inlay_hints_for_parameters" => "true",
                    "csharp|inlay_hints.dotnet_enable_inlay_hints_for_literal_parameters" => "true",
                    "csharp|inlay_hints.dotnet_enable_inlay_hints_for_indexer_parameters" => "true",
                    "csharp|inlay_hints.dotnet_enable_inlay_hints_for_object_creation_parameters" => "true",
                    "csharp|inlay_hints.dotnet_enable_inlay_hints_for_other_parameters" => "true",
                    "csharp|inlay_hints.dotnet_suppress_inlay_hints_for_parameters_that_differ_only_by_suffix" => "false",
                    "csharp|inlay_hints.dotnet_suppress_inlay_hints_for_parameters_that_match_method_intent" => "false",
                    "csharp|inlay_hints.dotnet_suppress_inlay_hints_for_parameters_that_match_argument_name" => "false",
                    "csharp|inlay_hints.csharp_enable_inlay_hints_for_types" => "true",
                    "csharp|inlay_hints.csharp_enable_inlay_hints_for_implicit_variable_types" => "true",
                    "csharp|inlay_hints.csharp_enable_inlay_hints_for_lambda_parameter_types" => "true",
                    "csharp|inlay_hints.csharp_enable_inlay_hints_for_implicit_object_creation" => "true",
                    _ => ""
                });
            }

            return values.ToArray();
        }
    }
}
