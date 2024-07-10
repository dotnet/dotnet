// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Remote.Razor;
using Microsoft.ServiceHub.Framework;

namespace Microsoft.VisualStudio.LanguageServices.Razor.Test.Cohost;

internal class InterceptingServiceBroker(Solution solution) : IServiceBroker, IBrokeredServiceInterceptor
{
    public event EventHandler<BrokeredServicesChangedEventArgs>? AvailabilityChanged { add { } remove { } }

    public ValueTask<IDuplexPipe?> GetPipeAsync(ServiceMoniker serviceMoniker, ServiceActivationOptions options = default, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask<T?> GetProxyAsync<T>(ServiceRpcDescriptor serviceDescriptor, ServiceActivationOptions options = default, CancellationToken cancellationToken = default)
        where T : class
    {
        throw new NotImplementedException();
    }

    public ValueTask RunServiceAsync(Func<CancellationToken, ValueTask> implementation, CancellationToken cancellationToken)
    {
        return implementation(cancellationToken);
    }

    public ValueTask<T> RunServiceAsync<T>(RazorPinnedSolutionInfoWrapper solutionInfo, Func<Solution, ValueTask<T>> implementation, CancellationToken cancellationToken)
    {
        return implementation(solution);
    }
}
