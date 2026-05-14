// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading;

namespace NuGet.Protocol.Plugins.Tests
{
    internal sealed class SimulatedIpc : IDisposable
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private bool _isDisposed;

        internal SimulatedStreamWriter RemoteStandardInputForLocal { get; }
        internal SimulatedStreamReader RemoteStandardInputForRemote { get; }
        internal SimulatedStreamWriter RemoteStandardOutputForRemote { get; }
        internal SimulatedStreamReader RemoteStandardOutputForLocal { get; }

        private SimulatedIpc(
            SimulatedStreamWriter remoteStandardInputForLocal,
            SimulatedStreamReader remoteStandardInputForRemote,
            SimulatedStreamWriter remoteStandardOutputForRemote,
            SimulatedStreamReader remoteStandardOutputForLocal,
            CancellationTokenSource cancellationTokenSource)
        {
            RemoteStandardInputForLocal = remoteStandardInputForLocal;
            RemoteStandardInputForRemote = remoteStandardInputForRemote;
            RemoteStandardOutputForRemote = remoteStandardOutputForRemote;
            RemoteStandardOutputForLocal = remoteStandardOutputForLocal;
            _cancellationTokenSource = cancellationTokenSource;
        }

        internal static SimulatedIpc Create(CancellationToken cancellationToken)
        {
            SimulatedStreamWriter remoteStandardInputForLocal;
            SimulatedStreamReader remoteStandardInputForRemote;
            SimulatedStreamWriter remoteStandardOutputForRemote;
            SimulatedStreamReader remoteStandardOutputForLocal;

            var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            CreateSimulatedIpcChannel(cancellationTokenSource.Token, out remoteStandardInputForLocal, out remoteStandardInputForRemote);
            CreateSimulatedIpcChannel(cancellationTokenSource.Token, out remoteStandardOutputForRemote, out remoteStandardOutputForLocal);

            return new SimulatedIpc(
                remoteStandardInputForLocal,
                remoteStandardInputForRemote,
                remoteStandardOutputForRemote,
                remoteStandardOutputForLocal,
                cancellationTokenSource);
        }

        private static void CreateSimulatedIpcChannel(CancellationToken cancellationToken, out SimulatedStreamWriter streamWriter, out SimulatedStreamReader streamReader)
        {
            var memoryStream = new MemoryStream();
            var readWriteSemaphore = new SemaphoreSlim(initialCount: 1, maxCount: 1);
            var dataWrittenEvent = new ManualResetEventSlim(initialState: false);

            var inboundStream = new SimulatedReadOnlyFileStream(memoryStream, readWriteSemaphore, dataWrittenEvent, cancellationToken);
            var outboundStream = new SimulatedWriteOnlyFileStream(memoryStream, readWriteSemaphore, dataWrittenEvent, cancellationToken);

            streamWriter = new SimulatedStreamWriter(outboundStream);
            streamReader = new SimulatedStreamReader(inboundStream);
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            using (_cancellationTokenSource)
            {
                _cancellationTokenSource.Cancel();
            }

            RemoteStandardInputForLocal.Dispose();
            RemoteStandardInputForRemote.Dispose();
            RemoteStandardOutputForRemote.Dispose();
            RemoteStandardOutputForLocal.Dispose();

            GC.SuppressFinalize(this);

            _isDisposed = true;
        }
    }
}
