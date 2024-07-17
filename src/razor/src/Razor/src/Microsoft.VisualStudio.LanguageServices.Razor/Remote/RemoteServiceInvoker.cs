// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Telemetry;
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
    ILoggerFactory loggerFactory)
    : IRemoteServiceInvoker
{
    private readonly IWorkspaceProvider _workspaceProvider = workspaceProvider;
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions = languageServerFeatureOptions;
    private readonly IClientCapabilitiesService _clientCapabilitiesService = clientCapabilitiesService;
    private readonly ISemanticTokensLegendService _semanticTokensLegendService = semanticTokensLegendService;
    private readonly ITelemetryReporter _telemetryReporter = telemetryReporter;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<RemoteServiceInvoker>();

    private readonly object _gate = new();
    private ValueTask<bool>? _isInitializedTask;
    private ValueTask<bool>? _isLSPInitializedTask;
    private bool _fullyInitialized;

    public async ValueTask<TResult?> TryInvokeAsync<TService, TResult>(
        Solution solution,
        Func<TService, RazorPinnedSolutionInfoWrapper, CancellationToken, ValueTask<TResult>> invocation,
        CancellationToken cancellationToken,
        [CallerFilePath] string? callerFilePath = null,
        [CallerMemberName] string? callerMemberName = null)
        where TService : class
    {
        var client = await TryGetClientAsync(cancellationToken).ConfigureAwait(false);
        if (client is null)
        {
            _logger.LogError($"Couldn't get remote client for {typeof(TService).Name} service");
            _telemetryReporter.ReportEvent("OOPClientFailure", Severity.Normal, new Property("service", typeof(TService).FullName));
            return default;
        }

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

    private async Task<RazorRemoteHostClient?> TryGetClientAsync(CancellationToken cancellationToken)
    {
        var workspace = _workspaceProvider.GetWorkspace();

        var remoteClient = await RazorRemoteHostClient.TryGetClientAsync(
            workspace.Services,
            RazorServices.Descriptors,
            RazorRemoteServiceCallbackDispatcherRegistry.Empty,
            cancellationToken).ConfigureAwait(false);

        if (remoteClient is null)
        {
            return null;
        }

        await InitializeRemoteClientAsync(remoteClient, cancellationToken).ConfigureAwait(false);

        return remoteClient;
    }

    private async Task InitializeRemoteClientAsync(RazorRemoteHostClient remoteClient, CancellationToken cancellationToken)
    {
        if (_fullyInitialized)
        {
            return;
        }

        lock (_gate)
        {
            if (_isInitializedTask is null)
            {
                var initParams = new RemoteClientInitializationOptions
                {
                    UseRazorCohostServer = _languageServerFeatureOptions.UseRazorCohostServer,
                    UsePreciseSemanticTokenRanges = _languageServerFeatureOptions.UsePreciseSemanticTokenRanges,
                    CSharpVirtualDocumentSuffix = _languageServerFeatureOptions.CSharpVirtualDocumentSuffix,
                    HtmlVirtualDocumentSuffix = _languageServerFeatureOptions.HtmlVirtualDocumentSuffix,
                    IncludeProjectKeyInGeneratedFilePath = _languageServerFeatureOptions.IncludeProjectKeyInGeneratedFilePath,
                };

                _logger.LogDebug($"First OOP call, so initializing OOP service.");

                _isInitializedTask = remoteClient.TryInvokeAsync<IRemoteClientInitializationService>(
                    (s, ct) => s.InitializeAsync(initParams, ct),
                    cancellationToken);
            }
        }

        await _isInitializedTask.Value.ConfigureAwait(false);

        if (_clientCapabilitiesService.CanGetClientCapabilities)
        {
            lock (_gate)
            {
                if (_isLSPInitializedTask is null)
                {
                    var initParams = new RemoteClientLSPInitializationOptions
                    {
                        TokenTypes = _semanticTokensLegendService.TokenTypes.All,
                        TokenModifiers = _semanticTokensLegendService.TokenModifiers.All,
                    };

                    _logger.LogDebug($"LSP server has started since last OOP call, so initializing OOP service with LSP info.");

                    _isLSPInitializedTask = remoteClient.TryInvokeAsync<IRemoteClientInitializationService>(
                        (s, ct) => s.InitializeLSPAsync(initParams, ct),
                        cancellationToken);
                }
            }

            await _isLSPInitializedTask.Value.ConfigureAwait(false);

            _fullyInitialized = true;
        }
    }
}
