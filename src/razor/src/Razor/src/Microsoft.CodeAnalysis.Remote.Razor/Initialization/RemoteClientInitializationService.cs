// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
                return default;
            },
            cancellationToken);
}
