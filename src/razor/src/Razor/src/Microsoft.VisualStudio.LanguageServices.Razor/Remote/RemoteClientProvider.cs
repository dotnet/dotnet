// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Remote.Razor;

namespace Microsoft.VisualStudio.LanguageServices.Razor.Remote;

[Export(typeof(IRemoteClientProvider))]
[method: ImportingConstructor]
internal sealed class RemoteClientProvider(
    IWorkspaceProvider workspaceProvider,
    LanguageServerFeatureOptions languageServerFeatureOptions)
    : IRemoteClientProvider
{
    private readonly IWorkspaceProvider _workspaceProvider = workspaceProvider;
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions = languageServerFeatureOptions;
    private bool _isInitialized;

    public async Task<RazorRemoteHostClient?> TryGetClientAsync(CancellationToken cancellationToken)
    {
       var workspace = _workspaceProvider.GetWorkspace();

        var remoteClient = await RazorRemoteHostClient.TryGetClientAsync(
            workspace.Services,
            RazorServices.Descriptors,
            RazorRemoteServiceCallbackDispatcherRegistry.Empty,
            cancellationToken);

        if (remoteClient is null)
        {
            return null;
        }

        await InitializeRemoteClientAsync(remoteClient, cancellationToken);

        return remoteClient;
    }

    private async Task InitializeRemoteClientAsync(RazorRemoteHostClient remoteClient, CancellationToken cancellationToken)
    {
        if (_isInitialized)
        {
            return;
        }

        var initParams = new RemoteClientInitializationOptions
        {
            UseRazorCohostServer = _languageServerFeatureOptions.UseRazorCohostServer,
            UsePreciseSemanticTokenRanges = _languageServerFeatureOptions.UsePreciseSemanticTokenRanges,
        };

        await remoteClient.TryInvokeAsync<IRemoteClientInitializationService>(
            (s, ct) => s.InitializeAsync(initParams, ct),
            cancellationToken);

        _isInitialized = true;
    }
}
