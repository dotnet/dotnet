// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.ServiceHub.Framework;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal abstract class RazorServiceBase : IDisposable
{
    protected readonly ServiceBrokerClient ServiceBrokerClient;

    public RazorServiceBase(IServiceBroker serviceBroker)
    {
#pragma warning disable VSTHRD012 // Provide JoinableTaskFactory where allowed
        ServiceBrokerClient = new ServiceBrokerClient(serviceBroker);
#pragma warning restore
    }

    public void Dispose()
    {
        ServiceBrokerClient.Dispose();
    }
}
