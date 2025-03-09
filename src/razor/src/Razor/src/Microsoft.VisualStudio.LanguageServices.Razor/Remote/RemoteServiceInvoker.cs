// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.AspNetCore.Razor.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.SemanticTokens;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.VisualStudio.Razor.Remote;

[Export(typeof(IRemoteServiceInvoker))]
[method: ImportingConstructor]
internal sealed class RemoteServiceInvoker(
    IWorkspaceProvider workspaceProvider,
    LanguageServerFeatureOptions languageServerFeatureOptions,
    IClientCapabilitiesService clientCapabilitiesService,
    ISemanticTokensLegendService semanticTokensLegendService,
    ITelemetryReporter telemetryReporter,
    ILoggerFactory loggerFactory) : IRemoteServiceInvoker, IDisposable
{
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions = languageServerFeatureOptions;
    private readonly IClientCapabilitiesService _clientCapabilitiesService = clientCapabilitiesService;
    private readonly ISemanticTokensLegendService _semanticTokensLegendService = semanticTokensLegendService;
    private readonly ITelemetryReporter _telemetryReporter = telemetryReporter;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<RemoteServiceInvoker>();

    private readonly CancellationTokenSource _disposeTokenSource = new();

    private readonly AsyncLazy<RazorRemoteHostClient> _lazyMessagePackClient = AsyncLazy.Create(GetMessagePackClientAsync, workspaceProvider);
    private readonly AsyncLazy<RazorRemoteHostClient> _lazyJsonClient = AsyncLazy.Create(GetJsonClientAsync, workspaceProvider);

    private readonly object _gate = new();
    private Task? _initializeOOPTask;
    private Task? _initializeLspTask;

    public void Dispose()
    {
        if (_disposeTokenSource.IsCancellationRequested)
        {
            return;
        }

        _disposeTokenSource.Cancel();
        _disposeTokenSource.Dispose();
    }

    public async ValueTask<TResult?> TryInvokeAsync<TService, TResult>(
        Solution solution,
        Func<TService, RazorPinnedSolutionInfoWrapper, CancellationToken, ValueTask<TResult>> invocation,
        CancellationToken cancellationToken,
        [CallerFilePath] string? callerFilePath = null,
        [CallerMemberName] string? callerMemberName = null)
        where TService : class
    {
        await InitializeAsync().ConfigureAwait(false);

        var client = await GetClientAsync<TService>(cancellationToken).ConfigureAwait(false);

        if (cancellationToken.IsCancellationRequested)
        {
            return default;
        }

        try
        {
            var result = await client.TryInvokeAsync(solution, invocation, cancellationToken).ConfigureAwait(false);

            return result.Value;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            var approximateCallingClassName = Path.GetFileNameWithoutExtension(callerFilePath);
            _logger.LogError(ex, $"Error calling remote method for {typeof(TService).Name} service, invocation: {approximateCallingClassName}.{callerMemberName}");
            _telemetryReporter.ReportFault(ex, "Exception calling remote method for {service}, invocation: {class}.{method}", typeof(TService).FullName, approximateCallingClassName, callerMemberName);
            return default;
        }
    }

    private Task<RazorRemoteHostClient> GetClientAsync<TService>(CancellationToken cancellationToken)
        where TService : class
        => typeof(IRemoteJsonService).IsAssignableFrom(typeof(TService))
            ? _lazyJsonClient.GetValueAsync(cancellationToken)
            : _lazyMessagePackClient.GetValueAsync(cancellationToken);

    private async static Task<RazorRemoteHostClient> GetMessagePackClientAsync(IWorkspaceProvider workspaceProvider, CancellationToken cancellationToken)
    {
        var workspace = workspaceProvider.GetWorkspace();

        var remoteClient = await RazorRemoteHostClient
            .TryGetClientAsync(
                workspace.Services,
                RazorServices.Descriptors,
                RazorRemoteServiceCallbackDispatcherRegistry.Empty,
                cancellationToken)
            .ConfigureAwait(false);

        return remoteClient
            ?? throw new InvalidOperationException($"Couldn't retrieve {nameof(RazorRemoteHostClient)} for MessagePack serialization.");
    }

    private async static Task<RazorRemoteHostClient> GetJsonClientAsync(IWorkspaceProvider workspaceProvider, CancellationToken cancellationToken)
    {
        var workspace = workspaceProvider.GetWorkspace();

        var remoteClient = await RazorRemoteHostClient
            .TryGetClientAsync(
                workspace.Services,
                RazorServices.JsonDescriptors,
                RazorRemoteServiceCallbackDispatcherRegistry.Empty,
                cancellationToken)
            .ConfigureAwait(false);

        return remoteClient
            ?? throw new InvalidOperationException($"Couldn't retrieve {nameof(RazorRemoteHostClient)} for JSON serialization.");
    }

    private ValueTask InitializeAsync()
    {
        var oopInitialized = _initializeOOPTask is { Status: TaskStatus.RanToCompletion };
        var lspInitialized = _initializeLspTask is { Status: TaskStatus.RanToCompletion };

        // Note: Since InitializeAsync will be called for each remote service call, we provide a synchronous path
        // to exit quickly when initialized and avoid creating an unnecessary async state machine.
        return oopInitialized && lspInitialized
            ? default
            : new(InitializeCoreAsync(oopInitialized, lspInitialized));

        async Task InitializeCoreAsync(bool oopInitialized, bool lspInitialized)
        {
            // Note: IRemoteClientInitializationService is an IRemoteJsonService, so we always need the JSON client.
            var remoteClient = await _lazyJsonClient
                .GetValueAsync(_disposeTokenSource.Token)
                .ConfigureAwait(false);

            if (!oopInitialized)
            {
                lock (_gate)
                {
                    _initializeOOPTask ??= InitializeOOPAsync(remoteClient);
                }

                await _initializeOOPTask.ConfigureAwait(false);
            }

            if (!lspInitialized && _clientCapabilitiesService.CanGetClientCapabilities)
            {
                lock (_gate)
                {
                    _initializeLspTask ??= InitializeLspAsync(remoteClient);
                }

                await _initializeLspTask.ConfigureAwait(false);
            }

            Task InitializeOOPAsync(RazorRemoteHostClient remoteClient)
            {
                var initParams = new RemoteClientInitializationOptions
                {
                    UseRazorCohostServer = _languageServerFeatureOptions.UseRazorCohostServer,
                    UsePreciseSemanticTokenRanges = _languageServerFeatureOptions.UsePreciseSemanticTokenRanges,
                    HtmlVirtualDocumentSuffix = _languageServerFeatureOptions.HtmlVirtualDocumentSuffix,
                    ReturnCodeActionAndRenamePathsWithPrefixedSlash = _languageServerFeatureOptions.ReturnCodeActionAndRenamePathsWithPrefixedSlash,
                    SupportsFileManipulation = _languageServerFeatureOptions.SupportsFileManipulation,
                    ShowAllCSharpCodeActions = _languageServerFeatureOptions.ShowAllCSharpCodeActions,
                    SupportsSoftSelectionInCompletion = _languageServerFeatureOptions.SupportsSoftSelectionInCompletion,
                    UseVsCodeCompletionTriggerCharacters = _languageServerFeatureOptions.UseVsCodeCompletionTriggerCharacters,
                };

                _logger.LogDebug($"First OOP call, so initializing OOP service.");

                return remoteClient
                    .TryInvokeAsync<IRemoteClientInitializationService>(
                        (s, ct) => s.InitializeAsync(initParams, ct),
                        _disposeTokenSource.Token)
                    .AsTask();
            }

            Task InitializeLspAsync(RazorRemoteHostClient remoteClient)
            {
                var initParams = new RemoteClientLSPInitializationOptions
                {
                    ClientCapabilities = _clientCapabilitiesService.ClientCapabilities,
                    TokenTypes = _semanticTokensLegendService.TokenTypes.All,
                    TokenModifiers = _semanticTokensLegendService.TokenModifiers.All,
                };

                _logger.LogDebug($"LSP server has started since last OOP call, so initializing OOP service with LSP info.");

                return remoteClient
                    .TryInvokeAsync<IRemoteClientInitializationService>(
                        (s, ct) => s.InitializeLSPAsync(initParams, ct),
                        _disposeTokenSource.Token)
                    .AsTask();
            }
        }
    }
}
