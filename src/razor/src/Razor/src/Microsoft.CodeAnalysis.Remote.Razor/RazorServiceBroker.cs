// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Api;
using Microsoft.ServiceHub.Framework;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal class RazorServiceBroker : IRazorServiceBroker
{
    private readonly ServiceBrokerClient _serviceBrokerClient;

    public RazorServiceBroker(IServiceBroker serviceBroker)
    {
        _serviceBrokerClient = new ServiceBrokerClient(serviceBroker, joinableTaskFactory: null);
    }

    public ValueTask RunServiceAsync(Func<CancellationToken, ValueTask> implementation, CancellationToken cancellationToken)
        => RazorBrokeredServiceImplementation.RunServiceAsync(implementation, cancellationToken);

    public ValueTask<T> RunServiceAsync<T>(RazorPinnedSolutionInfoWrapper solutionInfo, Func<Solution, ValueTask<T>> implementation, CancellationToken cancellationToken)
        => RazorBrokeredServiceImplementation.RunServiceAsync(solutionInfo, _serviceBrokerClient, implementation, cancellationToken);

    public void Dispose()
    {
        _serviceBrokerClient.Dispose();
    }
}
