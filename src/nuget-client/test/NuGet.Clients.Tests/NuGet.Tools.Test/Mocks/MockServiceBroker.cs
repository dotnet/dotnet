// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceHub.Framework;

namespace NuGet.Tools.Test
{
    // This mock is necessary because Moq-ing value type parameters with default value fails.
    // https://github.com/dotnet/runtime/issues/24589
    internal sealed class MockServiceBroker : IServiceBroker
    {
        public event EventHandler<BrokeredServicesChangedEventArgs> AvailabilityChanged { add { } remove { } }

        public ValueTask<IDuplexPipe> GetPipeAsync(
            ServiceMoniker serviceMoniker,
            ServiceActivationOptions options = default,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<T> GetProxyAsync<T>(
            ServiceRpcDescriptor serviceDescriptor,
            ServiceActivationOptions options = default,
            CancellationToken cancellationToken = default)
            where T : class
        {
            throw new NotImplementedException();
        }
    }
}
