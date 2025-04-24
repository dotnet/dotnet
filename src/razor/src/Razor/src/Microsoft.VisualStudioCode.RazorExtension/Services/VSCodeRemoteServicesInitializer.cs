// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
    ILoggerFactory loggerFactory) : IRazorCohostStartupService
{
    private readonly LanguageServerFeatureOptions _featureOptions = featureOptions;
    private readonly ISemanticTokensLegendService _semanticTokensLegendService = semanticTokensLegendService;
    private readonly ILoggerFactory _loggerFactory = loggerFactory;

    public async Task StartupAsync(VSInternalClientCapabilities clientCapabilities, RazorCohostRequestContext requestContext, CancellationToken cancellationToken)
    {
        // Normal remote service invoker logic requires a solution, but we don't have one here. Fortunately we don't need one, and since
        // we know this is VS Code specific, its all just smoke and mirrors anyway. We can avoid the smoke :)
        var serviceInterceptor = new VSCodeBrokeredServiceInterceptor();
        var service = await InProcServiceFactory.CreateServiceAsync<IRemoteClientInitializationService>(serviceInterceptor, _loggerFactory).ConfigureAwait(false);

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
