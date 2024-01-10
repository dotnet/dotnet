// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.Editor.Razor.Logging;
using Microsoft.VisualStudio.LanguageServer.Client;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.Threading;
using Microsoft.VisualStudio.Utilities;
using Nerdbank.Streams;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

[Export(typeof(ILanguageClient))]
[ContentType(RazorConstants.RazorLSPContentTypeName)]
[method: ImportingConstructor]
internal class RazorLanguageServerClient(
    RazorCustomMessageTarget customTarget,
    RazorLanguageClientMiddleLayer middleLayer,
    LSPRequestInvoker requestInvoker,
    ProjectConfigurationFilePathStore projectConfigurationFilePathStore,
    IRazorLoggerFactory razorLoggerFactory,
    RazorLogHubTraceProvider traceProvider,
    LanguageServerFeatureOptions languageServerFeatureOptions,
    ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
    ILanguageClientBroker languageClientBroker,
    ILanguageServiceBroker2 languageServiceBroker,
    ITelemetryReporter telemetryReporter,
    IClientSettingsManager clientSettingsManager,
    ILspServerActivationTracker lspServerActivationTracker,
    VisualStudioHostServicesProvider vsHostWorkspaceServicesProvider)
    : ILanguageClient, ILanguageClientCustomMessage2, ILanguageClientPriority
{
    private readonly ILanguageClientBroker _languageClientBroker = languageClientBroker ?? throw new ArgumentNullException(nameof(languageClientBroker));
    private readonly ILanguageServiceBroker2 _languageServiceBroker = languageServiceBroker ?? throw new ArgumentNullException(nameof(languageServiceBroker));
    private readonly ITelemetryReporter _telemetryReporter = telemetryReporter ?? throw new ArgumentNullException(nameof(telemetryReporter));
    private readonly IClientSettingsManager _clientSettingsManager = clientSettingsManager ?? throw new ArgumentNullException(nameof(clientSettingsManager));
    private readonly ILspServerActivationTracker _lspServerActivationTracker = lspServerActivationTracker ?? throw new ArgumentNullException(nameof(lspServerActivationTracker));
    private readonly RazorCustomMessageTarget _customMessageTarget = customTarget ?? throw new ArgumentNullException(nameof(customTarget));
    private readonly ILanguageClientMiddleLayer _middleLayer = middleLayer ?? throw new ArgumentNullException(nameof(middleLayer));
    private readonly LSPRequestInvoker _requestInvoker = requestInvoker ?? throw new ArgumentNullException(nameof(requestInvoker));
    private readonly ProjectConfigurationFilePathStore _projectConfigurationFilePathStore = projectConfigurationFilePathStore ?? throw new ArgumentNullException(nameof(projectConfigurationFilePathStore));
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions = languageServerFeatureOptions ?? throw new ArgumentNullException(nameof(languageServerFeatureOptions));
    private readonly VisualStudioHostServicesProvider _vsHostWorkspaceServicesProvider = vsHostWorkspaceServicesProvider ?? throw new ArgumentNullException(nameof(vsHostWorkspaceServicesProvider));
    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher ?? throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
    private readonly IRazorLoggerFactory _razorLoggerFactory = razorLoggerFactory ?? throw new ArgumentNullException(nameof(razorLoggerFactory));
    private readonly RazorLogHubTraceProvider _traceProvider = traceProvider ?? throw new ArgumentNullException(nameof(traceProvider));

    private RazorLanguageServerWrapper? _server;

    public event AsyncEventHandler<EventArgs>? StartAsync;
    public event AsyncEventHandler<EventArgs>? StopAsync
    {
        add { }
        remove { }
    }

    public string Name => RazorLSPConstants.RazorLanguageServerName;

    public IEnumerable<string>? ConfigurationSections => null;

    public object? InitializationOptions => null;

    public IEnumerable<string>? FilesToWatch => null;

    public object MiddleLayer => _middleLayer;

    public object CustomMessageTarget => _customMessageTarget;

    public bool IsOverriding => false;

    // We set a priority to ensure that our Razor language server is always chosen if there's a conflict for which language server to prefer.
    public int Priority => 10;

    public bool ShowNotificationOnInitializeFailed => true;

    public async Task<Connection?> ActivateAsync(CancellationToken token)
    {
        // Swap to background thread, nothing below needs to be done on the UI thread.
        await TaskScheduler.Default;

        var (clientStream, serverStream) = FullDuplexStream.CreatePair();

        await EnsureCleanedUpServerAsync().ConfigureAwait(false);

        var traceSource = _traceProvider.TryGetTraceSource();

        var lspOptions = RazorLSPOptions.From(_clientSettingsManager.GetClientSettings());

        _server = RazorLanguageServerWrapper.Create(
            serverStream,
            serverStream,
            _razorLoggerFactory,
            _telemetryReporter,
            _projectSnapshotManagerDispatcher,
            ConfigureLanguageServer,
            _languageServerFeatureOptions,
            lspOptions,
            _lspServerActivationTracker,
            traceSource);

        // This must not happen on an RPC endpoint due to UIThread concerns, so ActivateAsync was chosen.
        await EnsureContainedLanguageServersInitializedAsync();
        var connection = new Connection(clientStream, clientStream);
        return connection;
    }

    internal static IEnumerable<Lazy<ILanguageClient, LanguageServer.Client.IContentTypeMetadata>> GetRelevantContainedLanguageClientsAndMetadata(ILanguageServiceBroker2 languageServiceBroker)
    {
        var relevantClientAndMetadata = new List<Lazy<ILanguageClient, LanguageServer.Client.IContentTypeMetadata>>();

#pragma warning disable CS0618 // Type or member is obsolete
        foreach (var languageClientAndMetadata in languageServiceBroker.LanguageClients)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            if (languageClientAndMetadata.Metadata is not ILanguageClientMetadata metadata)
            {
                continue;
            }

            if (metadata is IIsUserExperienceDisabledMetadata userExperienceDisabledMetadata &&
                userExperienceDisabledMetadata.IsUserExperienceDisabled)
            {
                continue;
            }

            if (IsCSharpApplicable(metadata) ||
                metadata.ContentTypes.Contains(RazorLSPConstants.HtmlLSPDelegationContentTypeName))
            {
                relevantClientAndMetadata.Add(languageClientAndMetadata);
            }
        }

        return relevantClientAndMetadata;

        static bool IsCSharpApplicable(ILanguageClientMetadata metadata)
        {
            return metadata.ContentTypes.Contains(RazorLSPConstants.CSharpContentTypeName) &&
                metadata.ClientName == CSharpVirtualDocumentFactory.CSharpClientName;
        }
    }

    private async Task EnsureContainedLanguageServersInitializedAsync()
    {
        var relevantClientsAndMetadata = GetRelevantContainedLanguageClientsAndMetadata(_languageServiceBroker);

        var clientLoadTasks = new List<Task>();

        foreach (var languageClientAndMetadata in relevantClientsAndMetadata)
        {
            if (languageClientAndMetadata.Metadata is not ILanguageClientMetadata metadata)
            {
                continue;
            }

            var loadAsyncTask = _languageClientBroker.LoadAsync(metadata, languageClientAndMetadata.Value);
            clientLoadTasks.Add(loadAsyncTask);
        }

        await Task.WhenAll(clientLoadTasks).ConfigureAwait(false);

        // We only want to mark the server as activated after the delegated language servers have been initialized.
        _lspServerActivationTracker.Activated();
    }

    private void ConfigureLanguageServer(IServiceCollection serviceCollection)
    {
        if (_vsHostWorkspaceServicesProvider is not null)
        {
            var wrapper = new HostServicesProviderWrapper(_vsHostWorkspaceServicesProvider);
            serviceCollection.AddSingleton<HostServicesProvider>(wrapper);
        }
    }

    private async Task EnsureCleanedUpServerAsync()
    {
        if (_server is null)
        {
            // Server was already cleaned up
            return;
        }

        if (_server is not null)
        {
            _projectConfigurationFilePathStore.Changed -= ProjectConfigurationFilePathStore_Changed;
            // Server still hasn't shutdown, wait for it to shutdown
            await _server.WaitForExitAsync().ConfigureAwait(false);
        }
    }

    private void ProjectConfigurationFilePathStore_Changed(object sender, ProjectConfigurationFilePathChangedEventArgs args)
    {
        _ = ProjectConfigurationFilePathStore_ChangedAsync(args, CancellationToken.None);
    }

    private async Task ProjectConfigurationFilePathStore_ChangedAsync(ProjectConfigurationFilePathChangedEventArgs args, CancellationToken cancellationToken)
    {
        if (_languageServerFeatureOptions.DisableRazorLanguageServer)
        {
            return;
        }

        try
        {
            var parameter = new MonitorProjectConfigurationFilePathParams()
            {
                ProjectKeyId = args.ProjectKey.Id,
                ConfigurationFilePath = args.ConfigurationFilePath,
            };

            await _requestInvoker.ReinvokeRequestOnServerAsync<MonitorProjectConfigurationFilePathParams, object>(
                LanguageServerConstants.RazorMonitorProjectConfigurationFilePathEndpoint,
                RazorLSPConstants.RazorLanguageServerName,
                parameter,
                cancellationToken);
        }
        catch (Exception)
        {
            // We're fire and forgetting here, if the request fails we're ok with that.
            //
            // Note: When moving between solutions this can fail with a null reference exception because the underlying LSP platform's
            // JsonRpc object will be `null`. This can happen in two situations:
            //      1.  There's currently a race in the platform on shutting down/activating so we don't get the opportunity to properly detach
            //          from the configuration file path store changed event properly.
            //          Tracked by: https://github.com/dotnet/aspnetcore/issues/23819
            //      2.  The LSP platform failed to shutdown our language server properly due to a JsonRpc timeout. There's currently a limitation in
            //          the LSP platform APIs where we don't know if the LSP platform requested shutdown but our language server never saw it. Therefore,
            //          we will null-ref until our language server client boot-logic kicks back in and re-activates resulting in the old server being
            //          being cleaned up.
        }
    }

    public Task AttachForCustomMessageAsync(JsonRpc rpc) => Task.CompletedTask;

    public Task<InitializationFailureContext?> OnServerInitializeFailedAsync(ILanguageClientInitializationInfo initializationState)
    {
        _lspServerActivationTracker.Deactivated();

        var initializationFailureContext = new InitializationFailureContext
        {
            FailureMessage = string.Format(SR.LanguageServer_Initialization_Failed,
                Name, initializationState.StatusMessage, initializationState.InitializationException?.ToString())
        };
        return Task.FromResult<InitializationFailureContext?>(initializationFailureContext);
    }

    public Task OnLoadedAsync()
    {
        // If the user hasn't turned on the Cohost server, then don't disable the Razor server
        if (_languageServerFeatureOptions.DisableRazorLanguageServer && _languageServerFeatureOptions.UseRazorCohostServer)
        {
            return Task.CompletedTask;
        }

        return StartAsync.InvokeAsync(this, EventArgs.Empty);
    }

    public Task OnServerInitializedAsync()
    {
        ServerStarted();

        return Task.CompletedTask;
    }

    private void ServerStarted()
    {
        _projectConfigurationFilePathStore.Changed += ProjectConfigurationFilePathStore_Changed;

        var mappings = _projectConfigurationFilePathStore.GetMappings();
        foreach (var mapping in mappings)
        {
            var args = new ProjectConfigurationFilePathChangedEventArgs(mapping.Key, mapping.Value);
            ProjectConfigurationFilePathStore_Changed(this, args);
        }
    }

    private sealed class HostServicesProviderWrapper(VisualStudioHostServicesProvider vsHostServicesProvider) : HostServicesProvider
    {
        private readonly VisualStudioHostServicesProvider _vsHostServicesProvider = vsHostServicesProvider;

        public override HostServices GetServices() => _vsHostServicesProvider.GetServices();
    }
}
