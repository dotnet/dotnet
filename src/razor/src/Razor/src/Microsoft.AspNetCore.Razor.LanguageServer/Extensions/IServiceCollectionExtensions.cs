// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Razor;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Completion;
using Microsoft.AspNetCore.Razor.LanguageServer.Completion.Delegation;
using Microsoft.AspNetCore.Razor.LanguageServer.Diagnostics;
using Microsoft.AspNetCore.Razor.LanguageServer.DocumentPresentation;
using Microsoft.AspNetCore.Razor.LanguageServer.DocumentSynchronization;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.LanguageServer.Hover;
using Microsoft.AspNetCore.Razor.LanguageServer.InlineCompletion;
using Microsoft.AspNetCore.Razor.LanguageServer.Mapping;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic;
using Microsoft.AspNetCore.Razor.LanguageServer.SpellCheck;
using Microsoft.AspNetCore.Razor.LanguageServer.Tooltip;
using Microsoft.AspNetCore.Razor.ProjectEngineHost;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.Diagnostics;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.SemanticTokens;
using Microsoft.CodeAnalysis.Razor.SpellCheck;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Extensions;

internal static class IServiceCollectionExtensions
{
    public static void AddLifeCycleServices(this IServiceCollection services, RazorLanguageServer razorLanguageServer, ClientConnection clientConnection, ILspServerActivationTracker? lspServerActivationTracker)
    {
        services.AddHandler<RazorInitializeEndpoint>();
        services.AddHandler<RazorInitializedEndpoint>();

        var razorLifeCycleManager = new RazorLifeCycleManager(razorLanguageServer, lspServerActivationTracker);
        services.AddSingleton<ILifeCycleManager>(razorLifeCycleManager);
        services.AddSingleton<RazorLifeCycleManager>(razorLifeCycleManager);
        services.AddSingleton<CapabilitiesManager>();
        services.AddSingleton<IInitializeManager<InitializeParams, InitializeResult>, CapabilitiesManager>(sp => sp.GetRequiredService<CapabilitiesManager>());
        services.AddSingleton<IClientCapabilitiesService>(sp => sp.GetRequiredService<CapabilitiesManager>());
        services.AddSingleton<IWorkspaceRootPathProvider>(sp => sp.GetRequiredService<CapabilitiesManager>());
        services.AddSingleton<AbstractRequestContextFactory<RazorRequestContext>, RazorRequestContextFactory>();

        services.AddSingleton<ICapabilitiesProvider, RazorLanguageServerCapability>();

        services.AddSingleton<IOnInitialized>(clientConnection);
    }

    public static void AddFormattingServices(this IServiceCollection services, LanguageServerFeatureOptions featureOptions)
    {
        // Formatting
        services.AddSingleton<IRazorFormattingService, RazorFormattingService>();

        if (!featureOptions.UseRazorCohostServer)
        {
            services.AddSingleton<IHtmlFormatter, HtmlFormatter>();

            services.AddHandlerWithCapabilities<DocumentFormattingEndpoint>();
            services.AddHandlerWithCapabilities<DocumentOnTypeFormattingEndpoint>();
            services.AddHandlerWithCapabilities<DocumentRangeFormattingEndpoint>();
        }
    }

    public static void AddCompletionServices(this IServiceCollection services)
    {
        services.AddHandlerWithCapabilities<InlineCompletionEndpoint>();
        services.AddHandlerWithCapabilities<RazorCompletionEndpoint>();
        services.AddHandlerWithCapabilities<RazorCompletionResolveEndpoint>();
        services.AddSingleton<CompletionListCache>();
        services.AddSingleton<CompletionListProvider>();
        services.AddSingleton<DelegatedCompletionListProvider>();
        services.AddSingleton<RazorCompletionListProvider>();
        services.AddSingleton<DelegatedCompletionResponseRewriter, TextEditResponseRewriter>();
        services.AddSingleton<DelegatedCompletionResponseRewriter, DesignTimeHelperResponseRewriter>();
        services.AddSingleton<DelegatedCompletionResponseRewriter, HtmlCommitCharacterResponseRewriter>();
        services.AddSingleton<DelegatedCompletionResponseRewriter, SnippetResponseRewriter>();

        services.AddSingleton<AggregateCompletionItemResolver>();
        services.AddSingleton<CompletionItemResolver, RazorCompletionItemResolver>();
        services.AddSingleton<CompletionItemResolver, DelegatedCompletionItemResolver>();
        services.AddSingleton<ITagHelperCompletionService, LspTagHelperCompletionService>();
        services.AddSingleton<IRazorCompletionFactsService, LspRazorCompletionFactsService>();
        services.AddSingleton<IRazorCompletionItemProvider, DirectiveCompletionItemProvider>();
        services.AddSingleton<IRazorCompletionItemProvider, DirectiveAttributeCompletionItemProvider>();
        services.AddSingleton<IRazorCompletionItemProvider, DirectiveAttributeParameterCompletionItemProvider>();
        services.AddSingleton<IRazorCompletionItemProvider, DirectiveAttributeTransitionCompletionItemProvider>();
        services.AddSingleton<IRazorCompletionItemProvider, MarkupTransitionCompletionItemProvider>();
        services.AddSingleton<IRazorCompletionItemProvider, TagHelperCompletionProvider>();
    }

    public static void AddDiagnosticServices(this IServiceCollection services)
    {
        services.AddHandlerWithCapabilities<DocumentPullDiagnosticsEndpoint>();
        services.AddSingleton<RazorTranslateDiagnosticsService>();
        services.AddSingleton(sp => new Lazy<RazorTranslateDiagnosticsService>(sp.GetRequiredService<RazorTranslateDiagnosticsService>));
    }

    public static void AddHoverServices(this IServiceCollection services)
    {
        services.AddHandlerWithCapabilities<HoverEndpoint>();

        services.AddSingleton<IHoverService, HoverService>();
    }

    public static void AddSemanticTokensServices(this IServiceCollection services, LanguageServerFeatureOptions featureOptions)
    {
        if (!featureOptions.UseRazorCohostServer)
        {
            services.AddHandlerWithCapabilities<SemanticTokensRangeEndpoint>();
            // Ensure that we don't add the default service if something else has added one.
            services.TryAddSingleton<IRazorSemanticTokensInfoService, RazorSemanticTokensInfoService>();
            services.AddSingleton<ICSharpSemanticTokensProvider, LSPCSharpSemanticTokensProvider>();

            services.AddSingleton<ISemanticTokensLegendService, RazorSemanticTokensLegendService>();
        }

        services.AddHandler<RazorSemanticTokensRefreshEndpoint>();

        services.AddSingleton<IWorkspaceSemanticTokensRefreshNotifier, WorkspaceSemanticTokensRefreshNotifier>();
        services.AddSingleton<IRazorStartupService, WorkspaceSemanticTokensRefreshTrigger>();
    }

    public static void AddCodeActionsServices(this IServiceCollection services)
    {
        services.AddHandlerWithCapabilities<CodeActionEndpoint>();
        services.AddHandler<CodeActionResolveEndpoint>();

        // CSharp Code actions
        services.AddSingleton<ICSharpCodeActionProvider, TypeAccessibilityCodeActionProvider>();
        services.AddSingleton<ICSharpCodeActionProvider, DefaultCSharpCodeActionProvider>();
        services.AddSingleton<CSharpCodeActionResolver, DefaultCSharpCodeActionResolver>();
        services.AddSingleton<CSharpCodeActionResolver, UnformattedRemappingCSharpCodeActionResolver>();

        // Razor Code actions
        services.AddSingleton<IRazorCodeActionProvider, ExtractToCodeBehindCodeActionProvider>();
        services.AddSingleton<IRazorCodeActionResolver, ExtractToCodeBehindCodeActionResolver>();
        services.AddSingleton<IRazorCodeActionProvider, ComponentAccessibilityCodeActionProvider>();
        services.AddSingleton<IRazorCodeActionResolver, CreateComponentCodeActionResolver>();
        services.AddSingleton<IRazorCodeActionResolver, AddUsingsCodeActionResolver>();
        services.AddSingleton<IRazorCodeActionProvider, GenerateMethodCodeActionProvider>();
        services.AddSingleton<IRazorCodeActionResolver, GenerateMethodCodeActionResolver>();

        // Html Code actions
        services.AddSingleton<IHtmlCodeActionProvider, DefaultHtmlCodeActionProvider>();
        services.AddSingleton<HtmlCodeActionResolver, DefaultHtmlCodeActionResolver>();
    }

    public static void AddTextDocumentServices(this IServiceCollection services, LanguageServerFeatureOptions featureOptions)
    {
        if (!featureOptions.UseRazorCohostServer)
        {
            services.AddHandlerWithCapabilities<TextDocumentTextPresentationEndpoint>();
            services.AddHandlerWithCapabilities<TextDocumentUriPresentationEndpoint>();

            services.AddSingleton<ISpellCheckService, SpellCheckService>();
            services.AddSingleton<ICSharpSpellCheckRangeProvider, LspCSharpSpellCheckRangeProvider>();
            services.AddHandlerWithCapabilities<DocumentSpellCheckEndpoint>();
            services.AddHandler<WorkspaceSpellCheckEndpoint>();
        }

        services.AddHandlerWithCapabilities<DocumentDidChangeEndpoint>();
        services.AddHandler<DocumentDidCloseEndpoint>();
        services.AddHandler<DocumentDidOpenEndpoint>();
        services.AddHandler<DocumentDidSaveEndpoint>();

        services.AddHandler<RazorMapToDocumentRangesEndpoint>();
        services.AddHandler<RazorLanguageQueryEndpoint>();
    }

    public static void AddOptionsServices(this IServiceCollection services, RazorLSPOptions currentOptions)
    {
        services.AddSingleton<IConfigurationSyncService, DefaultRazorConfigurationService>();
        services.AddSingleton(s =>
        {
            return new RazorLSPOptionsMonitor(
                s.GetRequiredService<IConfigurationSyncService>(),
                currentOptions);
        });
    }

    public static void AddDocumentManagementServices(this IServiceCollection services, LanguageServerFeatureOptions featureOptions)
    {
        services.AddSingleton<IGeneratedDocumentPublisher, GeneratedDocumentPublisher>();
        services.AddSingleton<IRazorStartupService>((services) => (GeneratedDocumentPublisher)services.GetRequiredService<IGeneratedDocumentPublisher>());
        services.AddSingleton<IDocumentContextFactory, DocumentContextFactory>();
        services.AddSingleton(sp => new Lazy<IDocumentContextFactory>(sp.GetRequiredService<IDocumentContextFactory>));

        services.AddSingleton<RemoteTextLoaderFactory, DefaultRemoteTextLoaderFactory>();
        services.AddSingleton<IRazorProjectService, RazorProjectService>();
        services.AddSingleton<IRazorStartupService>((services) => (RazorProjectService)services.GetRequiredService<IRazorProjectService>());
        services.AddSingleton<IRazorStartupService, OpenDocumentGenerator>();
        services.AddSingleton<IDocumentMappingService, LspDocumentMappingService>();
        services.AddSingleton<IEditMappingService, LspEditMappingService>();
        services.AddSingleton<RazorFileChangeDetectorManager>();
        services.AddSingleton<IOnInitialized>(sp => sp.GetRequiredService<RazorFileChangeDetectorManager>());

        services.AddSingleton<IRazorFileChangeListener, RazorFileSynchronizer>();
        services.AddSingleton<IFileChangeDetector, RazorFileChangeDetector>();

        // Document processed listeners
        if (!featureOptions.SingleServerSupport)
        {
            // If single server is on, then we don't want to publish diagnostics, so best to just not hook up to any
            // events etc.
            services.AddSingleton<IDocumentProcessedListener, RazorDiagnosticsPublisher>();
        }

        services.AddSingleton<IDocumentProcessedListener, GeneratedDocumentSynchronizer>();
        services.AddSingleton<IDocumentProcessedListener, CodeDocumentReferenceHolder>();

        services.AddSingleton<LSPTagHelperTooltipFactory, DefaultLSPTagHelperTooltipFactory>();
        services.AddSingleton<VSLSPTagHelperTooltipFactory, DefaultVSLSPTagHelperTooltipFactory>();

        // Add project snapshot manager
        services.AddSingleton<IProjectEngineFactoryProvider, LspProjectEngineFactoryProvider>();
        services.AddSingleton<IProjectSnapshotManager, LspProjectSnapshotManager>();
        services.AddSingleton<IProjectCollectionResolver>(sp => (LspProjectSnapshotManager)sp.GetRequiredService<IProjectSnapshotManager>());
    }

    public static void AddHandlerWithCapabilities<T>(this IServiceCollection services)
        where T : class, IMethodHandler, ICapabilitiesProvider
    {
        services.AddSingleton<T>();
        services.AddSingleton<IMethodHandler, T>(s => s.GetRequiredService<T>());
        // Transient because it should only be used once and I'm hoping it doesn't stick around.
        services.AddTransient<ICapabilitiesProvider, T>(s => s.GetRequiredService<T>());
    }

    public static void AddHandler<T>(this IServiceCollection services)
        where T : class, IMethodHandler
    {
        if (typeof(ICapabilitiesProvider).IsAssignableFrom(typeof(T)))
        {
            throw new NotImplementedException($"{nameof(T)} is not using {nameof(AddHandlerWithCapabilities)} when it implements {nameof(ICapabilitiesProvider)}");
        }

        services.AddSingleton<IMethodHandler, T>();
    }
}
