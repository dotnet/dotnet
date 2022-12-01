// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.LanguageServer.AutoInsert;
using Microsoft.AspNetCore.Razor.LanguageServer.Debugging;
using Microsoft.AspNetCore.Razor.LanguageServer.Definition;
using Microsoft.AspNetCore.Razor.LanguageServer.DocumentColor;
using Microsoft.AspNetCore.Razor.LanguageServer.DocumentHighlighting;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Folding;
using Microsoft.AspNetCore.Razor.LanguageServer.Implementation;
using Microsoft.AspNetCore.Razor.LanguageServer.LinkedEditingRange;
using Microsoft.AspNetCore.Razor.LanguageServer.Refactoring;
using Microsoft.AspNetCore.Razor.LanguageServer.SignatureHelp;
using Microsoft.AspNetCore.Razor.LanguageServer.WrapWithTag;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.Telemetry;
using StreamJsonRpc;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class RazorLanguageServer : AbstractLanguageServer<RazorRequestContext>
{
    private readonly JsonRpc _jsonRpc;
    private readonly LanguageServerFeatureOptions? _featureOptions;
    private readonly ProjectSnapshotManagerDispatcher? _projectSnapshotManagerDispatcher;
    private readonly Action<IServiceCollection>? _configureServer;
    // Cached for testing
    private IHandlerProvider? _handlerProvider;

    public RazorLanguageServer(
        JsonRpc jsonRpc,
        ILspLogger logger,
        ProjectSnapshotManagerDispatcher? projectSnapshotManagerDispatcher,
        LanguageServerFeatureOptions? featureOptions,
        Action<IServiceCollection>? configureServer)
        : base(jsonRpc, logger)
    {
        _jsonRpc = jsonRpc;
        _featureOptions = featureOptions;
        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
        _configureServer = configureServer;

        Initialize();
    }

    protected override ILspServices ConstructLspServices()
    {
        var services = new ServiceCollection()
            .AddOptions()
            .AddLogging();

        if (_configureServer is not null)
        {
            _configureServer(services);
        }

        var serverManager = new DefaultClientNotifierService(_jsonRpc);
        services.AddSingleton<ClientNotifierServiceBase>(serverManager);
        if (_logger is LspLogger lspLogger)
        {
            lspLogger.Initialize(serverManager);
        }

        services.AddSingleton<ILspLogger>(_logger);
        if (_logger is ILogger ilogger)
        {
            services.AddSingleton<ILogger>(ilogger);
        }

        services.AddSingleton<ErrorReporter, LanguageServerErrorReporter>();

        if (_projectSnapshotManagerDispatcher is null)
        {
            services.AddSingleton<ProjectSnapshotManagerDispatcher, LSPProjectSnapshotManagerDispatcher>();
        }
        else
        {
            services.AddSingleton<ProjectSnapshotManagerDispatcher>(_projectSnapshotManagerDispatcher);
        }

        services.AddSingleton<AdhocWorkspaceFactory, DefaultAdhocWorkspaceFactory>();

        var featureOptions = _featureOptions ?? new DefaultLanguageServerFeatureOptions();
        services.AddSingleton(featureOptions);

        services.AddLifeCycleServices(this, serverManager);

        services.AddDiagnosticServices();
        services.AddSemanticTokensServices();
        services.AddDocumentManagmentServices();
        services.AddCompletionServices(featureOptions);
        services.AddFormattingServices();
        services.AddCodeActionsServices();
        services.AddOptionsServices();
        services.AddHoverServices();
        services.AddTextDocumentServices();

        // Auto insert
        services.AddSingleton<RazorOnAutoInsertProvider, CloseTextTagOnAutoInsertProvider>();
        services.AddSingleton<RazorOnAutoInsertProvider, AutoClosingTagOnAutoInsertProvider>();

        // Folding Range Providers
        services.AddSingleton<RazorFoldingRangeProvider, RazorCodeBlockFoldingProvider>();

        // Other
        services.AddSingleton<HtmlFactsService, DefaultHtmlFactsService>();
        services.AddSingleton<WorkspaceDirectoryPathResolver, DefaultWorkspaceDirectoryPathResolver>();
        services.AddSingleton<RazorComponentSearchEngine, DefaultRazorComponentSearchEngine>();

        // Folding Range Providers
        services.AddSingleton<RazorFoldingRangeProvider, RazorCodeBlockFoldingProvider>();

        // Get the DefaultSession for telemetry. This is set by VS with
        // TelemetryService.SetDefaultSession and provides the correct
        // appinsights keys etc
        services.AddSingleton<ITelemetryReporter>(provider =>
            new TelemetryReporter(ImmutableArray.Create(TelemetryService.DefaultSession), provider.GetRequiredService<ILoggerFactory>()));

        // Defaults: For when the caller hasn't provided them through the `configure` action.
        services.TryAddSingleton<HostServicesProvider, DefaultHostServicesProvider>();

        AddHandlers(services);

        var lspServices = new LspServices(services);

        return lspServices;

        static void AddHandlers(IServiceCollection services)
        {
            services.AddRegisteringHandler<ImplementationEndpoint>();
            services.AddRegisteringHandler<SignatureHelpEndpoint>();
            services.AddRegisteringHandler<DocumentHighlightEndpoint>();
            services.AddHandler<RazorConfigurationEndpoint>();
            services.AddRegisteringHandler<OnAutoInsertEndpoint>();
            services.AddHandler<MonitorProjectConfigurationFilePathEndpoint>();
            services.AddRegisteringHandler<RenameEndpoint>();
            services.AddRegisteringHandler<RazorDefinitionEndpoint>();
            services.AddRegisteringHandler<LinkedEditingRangeEndpoint>();
            services.AddHandler<WrapWithTagEndpoint>();
            services.AddHandler<RazorBreakpointSpanEndpoint>();
            services.AddHandler<RazorProximityExpressionsEndpoint>();
            services.AddRegisteringHandler<DocumentColorEndpoint>();
            services.AddRegisteringHandler<FoldingRangeEndpoint>();
            services.AddRegisteringHandler<ValidateBreakpointRangeEndpoint>();
        }
    }

    protected override IHandlerProvider GetHandlerProvider()
    {
        _handlerProvider ??= base.GetHandlerProvider();

        return _handlerProvider;
    }

    internal T GetRequiredService<T>() where T : notnull
    {
        var lspServices = GetLspServices();

        return lspServices.GetRequiredService<T>();
    }

    // Internal for testing
    internal TestAccessor GetTestAccessor()
    {
        return new TestAccessor(this);
    }

    internal class TestAccessor
    {
        private RazorLanguageServer _server;

        public TestAccessor(RazorLanguageServer server)
        {
            _server = server;
        }

        public IHandlerProvider GetHandlerProvider()
        {
            return _server.GetHandlerProvider();
        }
    }
}
