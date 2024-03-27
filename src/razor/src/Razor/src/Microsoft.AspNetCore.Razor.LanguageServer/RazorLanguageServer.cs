// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.LanguageServer.AutoInsert;
using Microsoft.AspNetCore.Razor.LanguageServer.ColorPresentation;
using Microsoft.AspNetCore.Razor.LanguageServer.Debugging;
using Microsoft.AspNetCore.Razor.LanguageServer.Definition;
using Microsoft.AspNetCore.Razor.LanguageServer.DocumentColor;
using Microsoft.AspNetCore.Razor.LanguageServer.DocumentHighlighting;
using Microsoft.AspNetCore.Razor.LanguageServer.DocumentSymbol;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.FindAllReferences;
using Microsoft.AspNetCore.Razor.LanguageServer.Folding;
using Microsoft.AspNetCore.Razor.LanguageServer.Implementation;
using Microsoft.AspNetCore.Razor.LanguageServer.InlayHints;
using Microsoft.AspNetCore.Razor.LanguageServer.LinkedEditingRange;
using Microsoft.AspNetCore.Razor.LanguageServer.MapCode;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectContexts;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.LanguageServer.Refactoring;
using Microsoft.AspNetCore.Razor.LanguageServer.SignatureHelp;
using Microsoft.AspNetCore.Razor.LanguageServer.WrapWithTag;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StreamJsonRpc;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal partial class RazorLanguageServer : AbstractLanguageServer<RazorRequestContext>
{
    private readonly JsonRpc _jsonRpc;
    private readonly IRazorLoggerFactory _loggerFactory;
    private readonly LanguageServerFeatureOptions? _featureOptions;
    private readonly ProjectSnapshotManagerDispatcher? _projectSnapshotManagerDispatcher;
    private readonly Action<IServiceCollection>? _configureServer;
    private readonly RazorLSPOptions _lspOptions;
    private readonly ILspServerActivationTracker? _lspServerActivationTracker;
    private readonly ITelemetryReporter _telemetryReporter;
    private readonly ClientConnection _clientConnection;

    // Cached for testing
    private AbstractHandlerProvider? _handlerProvider;

    public RazorLanguageServer(
        JsonRpc jsonRpc,
        IRazorLoggerFactory loggerFactory,
        ProjectSnapshotManagerDispatcher? projectSnapshotManagerDispatcher,
        LanguageServerFeatureOptions? featureOptions,
        Action<IServiceCollection>? configureServer,
        RazorLSPOptions? lspOptions,
        ILspServerActivationTracker? lspServerActivationTracker,
        ITelemetryReporter telemetryReporter)
        : base(jsonRpc, CreateILspLogger(loggerFactory, telemetryReporter))
    {
        _jsonRpc = jsonRpc;
        _loggerFactory = loggerFactory;
        _featureOptions = featureOptions;
        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
        _configureServer = configureServer;
        _lspOptions = lspOptions ?? RazorLSPOptions.Default;
        _lspServerActivationTracker = lspServerActivationTracker;
        _telemetryReporter = telemetryReporter;

        _clientConnection = new ClientConnection(_jsonRpc);

        Initialize();
    }

    private static ILspLogger CreateILspLogger(IRazorLoggerFactory loggerFactory, ITelemetryReporter telemetryReporter)
    {
        return new ClaspLoggingBridge(loggerFactory, telemetryReporter);
    }

    protected override IRequestExecutionQueue<RazorRequestContext> ConstructRequestExecutionQueue()
    {
        var handlerProvider = this.HandlerProvider;
        var queue = new RazorRequestExecutionQueue(this, _logger, handlerProvider);
        queue.Start();

        return queue;
    }

    protected override ILspServices ConstructLspServices()
    {
        var services = new ServiceCollection()
            .AddOptions();

        var loggerFactoryWrapper = new LoggerFactoryWrapper(_loggerFactory);
        // Wrap the logger factory so that we can add [LSP] to the start of all the categories
        services.AddSingleton<IRazorLoggerFactory>(loggerFactoryWrapper);

        if (_configureServer is not null)
        {
            _configureServer(services);
        }

        services.AddSingleton<IClientConnection>(_clientConnection);

        // Add the logger as a service in case anything in CLaSP pulls it out to do logging
        services.AddSingleton<ILspLogger>(_logger);

        services.AddSingleton<IErrorReporter, LanguageServerErrorReporter>();

        if (_projectSnapshotManagerDispatcher is null)
        {
            services.AddSingleton<ProjectSnapshotManagerDispatcher, LSPProjectSnapshotManagerDispatcher>();
        }
        else
        {
            services.AddSingleton<ProjectSnapshotManagerDispatcher>(_projectSnapshotManagerDispatcher);
        }

        services.AddSingleton<IAdhocWorkspaceFactory, AdhocWorkspaceFactory>();
        services.AddSingleton<IWorkspaceProvider, LspWorkspaceProvider>();

        var featureOptions = _featureOptions ?? new DefaultLanguageServerFeatureOptions();
        services.AddSingleton(featureOptions);

        services.AddSingleton<IFilePathService, LSPFilePathService>();

        services.AddLifeCycleServices(this, _clientConnection, _lspServerActivationTracker);

        services.AddDiagnosticServices();
        services.AddSemanticTokensServices(featureOptions);
        services.AddDocumentManagementServices(featureOptions);
        services.AddCompletionServices(featureOptions);
        services.AddFormattingServices();
        services.AddCodeActionsServices();
        services.AddOptionsServices(_lspOptions);
        services.AddHoverServices();
        services.AddTextDocumentServices();

        // Auto insert
        services.AddSingleton<IOnAutoInsertProvider, CloseTextTagOnAutoInsertProvider>();
        services.AddSingleton<IOnAutoInsertProvider, AutoClosingTagOnAutoInsertProvider>();

        // Folding Range Providers
        services.AddSingleton<IRazorFoldingRangeProvider, RazorCodeBlockFoldingProvider>();
        services.AddSingleton<IRazorFoldingRangeProvider, UsingsFoldingRangeProvider>();

        // Other
        services.AddSingleton<WorkspaceDirectoryPathResolver, DefaultWorkspaceDirectoryPathResolver>();
        services.AddSingleton<RazorComponentSearchEngine, DefaultRazorComponentSearchEngine>();

        // Get the DefaultSession for telemetry. This is set by VS with
        // TelemetryService.SetDefaultSession and provides the correct
        // appinsights keys etc
        services.AddSingleton<ITelemetryReporter>(_telemetryReporter);

        // Defaults: For when the caller hasn't provided them through the `configure` action.
        services.TryAddSingleton<HostServicesProvider, DefaultHostServicesProvider>();

        AddHandlers(services, featureOptions);

        var lspServices = new LspServices(services);

        return lspServices;

        static void AddHandlers(IServiceCollection services, LanguageServerFeatureOptions featureOptions)
        {
            // Not calling AddHandler because we want to register this endpoint as an IOnInitialized too
            services.AddSingleton<RazorConfigurationEndpoint>();
            services.AddSingleton<IMethodHandler, RazorConfigurationEndpoint>(s => s.GetRequiredService<RazorConfigurationEndpoint>());
            // Transient because it should only be used once and I'm hoping it doesn't stick around.
            services.AddTransient<IOnInitialized>(sp => sp.GetRequiredService<RazorConfigurationEndpoint>());

            services.AddHandlerWithCapabilities<ImplementationEndpoint>();
            services.AddHandlerWithCapabilities<SignatureHelpEndpoint>();
            services.AddHandlerWithCapabilities<DocumentHighlightEndpoint>();
            services.AddHandlerWithCapabilities<OnAutoInsertEndpoint>();
            services.AddHandler<MonitorProjectConfigurationFilePathEndpoint>();
            services.AddHandlerWithCapabilities<RenameEndpoint>();
            services.AddHandlerWithCapabilities<DefinitionEndpoint>();
            services.AddHandlerWithCapabilities<LinkedEditingRangeEndpoint>();
            services.AddHandler<WrapWithTagEndpoint>();
            services.AddHandler<RazorBreakpointSpanEndpoint>();
            services.AddHandler<RazorProximityExpressionsEndpoint>();

            services.AddHandlerWithCapabilities<DocumentColorEndpoint>();
            services.AddSingleton<IDocumentColorService, DocumentColorService>();

            services.AddHandler<ColorPresentationEndpoint>();
            services.AddHandlerWithCapabilities<FoldingRangeEndpoint>();
            services.AddHandlerWithCapabilities<ValidateBreakpointRangeEndpoint>();
            services.AddHandlerWithCapabilities<FindAllReferencesEndpoint>();
            services.AddHandlerWithCapabilities<ProjectContextsEndpoint>();
            services.AddHandlerWithCapabilities<DocumentSymbolEndpoint>();
            services.AddHandlerWithCapabilities<MapCodeEndpoint>();

            services.AddSingleton<IInlayHintService, InlayHintService>();

            services.AddHandlerWithCapabilities<InlayHintEndpoint>();
            services.AddHandler<InlayHintResolveEndpoint>();
        }
    }

    protected override AbstractHandlerProvider HandlerProvider
    {
        get
        {
            _handlerProvider ??= base.HandlerProvider;

            return _handlerProvider;
        }
    }

    internal T GetRequiredService<T>() where T : notnull
    {
        var lspServices = GetLspServices();

        return lspServices.GetRequiredService<T>();
    }

    // Internal for testing
    internal new TestAccessor GetTestAccessor()
    {
        return new TestAccessor(this);
    }

    internal new class TestAccessor
    {
        private RazorLanguageServer _server;

        public TestAccessor(RazorLanguageServer server)
        {
            _server = server;
        }

        public AbstractHandlerProvider HandlerProvider => _server.HandlerProvider;

        public RazorRequestExecutionQueue GetRequestExecutionQueue()
        {
            return (RazorRequestExecutionQueue)_server.GetRequestExecutionQueue();
        }
    }
}
