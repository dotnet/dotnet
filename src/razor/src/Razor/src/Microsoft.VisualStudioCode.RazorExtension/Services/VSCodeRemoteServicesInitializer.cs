// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.SemanticTokens;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

namespace Microsoft.VisualStudioCode.RazorExtension.Services;

[Shared]
[Export(typeof(IRazorCohostStartupService))]
[method: ImportingConstructor]
internal class VSCodeRemoteServicesInitializer(
    LanguageServerFeatureOptions featureOptions,
    ISemanticTokensLegendService semanticTokensLegendService,
    IWorkspaceProvider workspaceProvider,
    ILoggerFactory loggerFactory) : IRazorCohostStartupService
{
    private readonly LanguageServerFeatureOptions _featureOptions = featureOptions;
    private readonly ISemanticTokensLegendService _semanticTokensLegendService = semanticTokensLegendService;
    private readonly IWorkspaceProvider _workspaceProvider = workspaceProvider;
    private readonly ILoggerFactory _loggerFactory = loggerFactory;

    public int Order => WellKnownStartupOrder.RemoteServices;

    public async Task StartupAsync(VSInternalClientCapabilities clientCapabilities, RazorCohostRequestContext requestContext, CancellationToken cancellationToken)
    {
        // Initializing remote services will create a MEF composition, but if cohost is not on we don't need it
        if (!_featureOptions.UseRazorCohostServer)
        {
            return;
        }

        // Normal remote service invoker logic requires a solution, but we don't have one here. Fortunately we don't need one, and since
        // we know this is VS Code specific, its all just smoke and mirrors anyway. We can avoid the smoke :)
        var serviceInterceptor = new VSCodeBrokeredServiceInterceptor();

        var logger = _loggerFactory.GetOrCreateLogger<VSCodeRemoteServicesInitializer>();
        logger.LogDebug("Initializing remote services.");
        var service = await InProcServiceFactory.CreateServiceAsync<IRemoteClientInitializationService>(serviceInterceptor, _workspaceProvider, _loggerFactory).ConfigureAwait(false);
        logger.LogDebug("Initialized remote services.");

        await service.InitializeAsync(new RemoteClientInitializationOptions
        {
            UseRazorCohostServer = _featureOptions.UseRazorCohostServer,
            UsePreciseSemanticTokenRanges = _featureOptions.UsePreciseSemanticTokenRanges,
            HtmlVirtualDocumentSuffix = _featureOptions.HtmlVirtualDocumentSuffix,
            ReturnCodeActionAndRenamePathsWithPrefixedSlash = _featureOptions.ReturnCodeActionAndRenamePathsWithPrefixedSlash,
            SupportsFileManipulation = _featureOptions.SupportsFileManipulation,
            ShowAllCSharpCodeActions = _featureOptions.ShowAllCSharpCodeActions,
            SupportsSoftSelectionInCompletion = _featureOptions.SupportsSoftSelectionInCompletion,
            UseVsCodeCompletionTriggerCharacters = _featureOptions.UseVsCodeCompletionTriggerCharacters,
        }, cancellationToken).ConfigureAwait(false);

        await service.InitializeLSPAsync(new RemoteClientLSPInitializationOptions
        {
            ClientCapabilities = clientCapabilities,
            TokenTypes = _semanticTokensLegendService.TokenTypes.All,
            TokenModifiers = _semanticTokensLegendService.TokenModifiers.All,
        }, cancellationToken).ConfigureAwait(false);
    }
}
