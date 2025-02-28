// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.LanguageServer.Client;
using Microsoft.VisualStudio.Razor.LanguageClient.Endpoints;
using Microsoft.VisualStudio.Razor.LanguageClient.ProjectSystem;
using Microsoft.VisualStudio.Razor.Logging;
using Microsoft.VisualStudio.Razor.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using Microsoft.VisualStudio.Utilities;
using Nerdbank.Streams;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.Razor.LanguageClient;

[Export(typeof(ILanguageClient))]
[ContentType(RazorConstants.RazorLSPContentTypeName)]
[method: ImportingConstructor]
internal class RazorLanguageServerClient(
    RazorCustomMessageTarget customTarget,
    ProjectSnapshotManager projectManager,
    ILoggerFactory loggerFactory,
    RazorLogHubTraceProvider traceProvider,
    LanguageServerFeatureOptions languageServerFeatureOptions,
    ILanguageClientBroker languageClientBroker,
    ILanguageServiceBroker2 languageServiceBroker,
    ITelemetryReporter telemetryReporter,
    IClientSettingsManager clientSettingsManager,
    ILspServerActivationTracker lspServerActivationTracker,
    VisualStudioHostServicesProvider vsHostServicesProvider)
    : ILanguageClient, ILanguageClientCustomMessage2, ILanguageClientPriority, IPropertyOwner
{
    private readonly ILanguageClientBroker _languageClientBroker = languageClientBroker;
    private readonly ILanguageServiceBroker2 _languageServiceBroker = languageServiceBroker;
    private readonly ITelemetryReporter _telemetryReporter = telemetryReporter;
    private readonly IClientSettingsManager _clientSettingsManager = clientSettingsManager;
    private readonly ILspServerActivationTracker _lspServerActivationTracker = lspServerActivationTracker;
    private readonly RazorCustomMessageTarget _customMessageTarget = customTarget;
    private readonly ProjectSnapshotManager _projectManager = projectManager;
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions = languageServerFeatureOptions;
    private readonly VisualStudioHostServicesProvider _vsHostServicesProvider = vsHostServicesProvider;
    private readonly ILoggerFactory _loggerFactory = loggerFactory;
    private readonly RazorLogHubTraceProvider _traceProvider = traceProvider;

    private RazorLanguageServerHost? _host;

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

    public object? MiddleLayer => null;

    public object CustomMessageTarget => _customMessageTarget;

    public bool IsOverriding => false;

    // We set a priority to ensure that our Razor language server is always chosen if there's a conflict for which language server to prefer.
    public int Priority => 10;

    public bool ShowNotificationOnInitializeFailed => true;

    public PropertyCollection Properties { get; } = CreateStjPropertyCollection();

    private static PropertyCollection CreateStjPropertyCollection()
    {
        // Opt in to System.Text.Json serialization on the client
        var collection = new PropertyCollection();
        collection.AddProperty("lsp-serialization", "stj");
        return collection;
    }

    public async Task<Connection?> ActivateAsync(CancellationToken token)
    {
        // Swap to background thread, nothing below needs to be done on the UI thread.
        await TaskScheduler.Default;

        var (clientStream, serverStream) = FullDuplexStream.CreatePair();

        await EnsureCleanedUpServerAsync().ConfigureAwait(false);

        _traceProvider.TryGetTraceSource(out var traceSource);

        var lspOptions = RazorLSPOptions.From(_clientSettingsManager.GetClientSettings());

        _host = RazorLanguageServerHost.Create(
            serverStream,
            serverStream,
            _loggerFactory,
            _telemetryReporter,
            ConfigureServices,
            _languageServerFeatureOptions,
            lspOptions,
            _lspServerActivationTracker,
            traceSource);

        // This must not happen on an RPC endpoint due to UIThread concerns, so ActivateAsync was chosen.
        await EnsureContainedLanguageServersInitializedAsync();

        return new Connection(clientStream, clientStream);

        void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHostServicesProvider>(new HostServicesProviderAdapter(_vsHostServicesProvider));

            var projectInfoDriver = new RazorProjectInfoDriver(_projectManager, _loggerFactory);
            services.AddSingleton<IRazorProjectInfoDriver>(projectInfoDriver);
        }
    }

    private async Task EnsureCleanedUpServerAsync()
    {
        if (_host is null)
        {
            // Server was already cleaned up
            return;
        }

        if (_host is not null)
        {
            // Server still hasn't shutdown, wait for it to shutdown
            await _host.WaitForExitAsync().ConfigureAwait(false);

            _host.Dispose();
            _host = null;
        }
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
        // If the user has turned on the Cohost server, then disable the Razor server
        if (_languageServerFeatureOptions.UseRazorCohostServer)
        {
            return Task.CompletedTask;
        }

        return StartAsync.InvokeAsync(this, EventArgs.Empty);
    }

    public Task OnServerInitializedAsync()
        => Task.CompletedTask;

    private sealed class HostServicesProviderAdapter(VisualStudioHostServicesProvider vsHostServicesProvider) : IHostServicesProvider
    {
        private readonly VisualStudioHostServicesProvider _vsHostServicesProvider = vsHostServicesProvider;

        public HostServices GetServices() => _vsHostServicesProvider.GetServices();
    }
}
