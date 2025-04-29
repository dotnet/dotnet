// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Api;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.ServiceHub.Framework;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal abstract partial class RazorBrokeredServiceBase : IDisposable
{
    private readonly ServiceBrokerClient _serviceBrokerClient;
    private readonly ServiceRpcDescriptor.RpcConnection? _serverConnection;
    private readonly IRazorBrokeredServiceInterceptor? _interceptor;

    protected readonly RemoteSnapshotManager SnapshotManager;
    protected readonly ILogger Logger;

    protected RazorBrokeredServiceBase(in ServiceArgs args)
    {
        _serviceBrokerClient = new ServiceBrokerClient(args.ServiceBroker, joinableTaskFactory: null);
        _serverConnection = args.ServerConnection;
        _interceptor = args.Interceptor;
        SnapshotManager = args.ExportProvider.GetExportedValue<RemoteSnapshotManager>();

        Logger = args.ServiceLoggerFactory.GetOrCreateLogger(GetType());
    }

    protected ValueTask RunServiceAsync(Func<CancellationToken, ValueTask> implementation, CancellationToken cancellationToken)
        => _interceptor is not null
            ? _interceptor.RunServiceAsync(implementation, cancellationToken)
            : RazorBrokeredServiceImplementation.RunServiceAsync(implementation, cancellationToken);

    protected ValueTask<T> RunServiceAsync<T>(RazorPinnedSolutionInfoWrapper solutionInfo, Func<Solution, ValueTask<T>> implementation, CancellationToken cancellationToken)
        => _interceptor is not null
            ? _interceptor.RunServiceAsync(solutionInfo, implementation, cancellationToken)
            : RazorBrokeredServiceImplementation.RunServiceAsync(solutionInfo, _serviceBrokerClient, implementation, cancellationToken);

    public void Dispose()
    {
        _serviceBrokerClient.Dispose();
        _serverConnection?.Dispose();
    }
}
