// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Remote.Razor.SemanticTokens;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed class RemoteClientInitializationService(in ServiceArgs args) : RazorBrokeredServiceBase(in args), IRemoteClientInitializationService
{
    internal sealed class Factory : FactoryBase<IRemoteClientInitializationService>
    {
        protected override IRemoteClientInitializationService CreateService(in ServiceArgs args)
            => new RemoteClientInitializationService(in args);
    }

    private readonly RemoteClientCapabilitiesService _remoteClientCapabilitiesService = args.ExportProvider.GetExportedValue<RemoteClientCapabilitiesService>();
    private readonly RemoteLanguageServerFeatureOptions _remoteLanguageServerFeatureOptions = args.ExportProvider.GetExportedValue<RemoteLanguageServerFeatureOptions>();
    private readonly RemoteSemanticTokensLegendService _remoteSemanticTokensLegendService = args.ExportProvider.GetExportedValue<RemoteSemanticTokensLegendService>();

    public ValueTask InitializeAsync(RemoteClientInitializationOptions options, CancellationToken cancellationToken)
        => RunServiceAsync(ct =>
            {
                _remoteLanguageServerFeatureOptions.SetOptions(options);
                return default;
            },
            cancellationToken);

    public ValueTask InitializeLSPAsync(RemoteClientLSPInitializationOptions options, CancellationToken cancellationToken)
        => RunServiceAsync(ct =>
            {
                _remoteSemanticTokensLegendService.SetLegend(options.TokenTypes, options.TokenModifiers);
                _remoteClientCapabilitiesService.SetCapabilities(options.ClientCapabilities);
                return default;
            },
            cancellationToken);
}
