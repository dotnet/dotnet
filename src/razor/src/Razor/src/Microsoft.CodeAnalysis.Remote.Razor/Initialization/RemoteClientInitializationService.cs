// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Remote.Razor.SemanticTokens;
using Microsoft.ServiceHub.Framework;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed class RemoteClientInitializationService(
    IServiceBroker serviceBroker)
    : RazorServiceBase(serviceBroker), IRemoteClientInitializationService
{
    public ValueTask InitializeAsync(RemoteClientInitializationOptions options, CancellationToken cancellationToken)
        => RunServiceAsync(ct =>
            {
                RemoteLanguageServerFeatureOptions.SetOptions(options);
                return default;
            },
            cancellationToken);

    public ValueTask InitializeLSPAsync(RemoteClientLSPInitializationOptions options, CancellationToken cancellationToken)
        => RunServiceAsync(ct =>
            {
                RemoteSemanticTokensLegendService.SetLegend(options.TokenTypes, options.TokenModifiers);
                return default;
            },
            cancellationToken);
}
