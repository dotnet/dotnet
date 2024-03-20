// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Api;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.ServiceHub.Framework;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed class RemoteClientInitializationService : RazorServiceBase, IRemoteClientInitializationService
{
    internal RemoteClientInitializationService(IServiceBroker serviceBroker)
        : base(serviceBroker)
    {
    }

    public async ValueTask InitializeAsync(RemoteClientInitializationOptions options, CancellationToken cancellationToken)
    {
        await RazorBrokeredServiceImplementation.RunServiceAsync(
            _ =>
            {
                RemoteLanguageServerFeatureOptions.SetOptions(options);
                return default;
            },
            cancellationToken);
    }
}
