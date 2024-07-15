// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Remote.Razor.Logging;
using Microsoft.ServiceHub.Framework;
using Microsoft.ServiceHub.Framework.Services;
using Microsoft.VisualStudio.Composition;
using Nerdbank.Streams;

namespace Microsoft.CodeAnalysis.Remote.Razor;

/// <summary>
/// </summary>
/// <typeparam name="TService"></typeparam>
/// <remarks>
/// Implementors of <see cref="IServiceHubServiceFactory" /> (and thus this class) MUST provide a parameterless constructor or ServiceHub will fail to construct them.
/// </remarks>
internal abstract class RazorServiceFactoryBase<TService> : IServiceHubServiceFactory where TService : class
{
    private readonly RazorServiceDescriptorsWrapper _razorServiceDescriptors;

    /// <summary>
    /// </summary>
    /// <param name="razorServiceDescriptors"></param>
    /// <remarks>
    /// Implementors of <see cref="IServiceHubServiceFactory" /> (and thus this class) MUST provide a parameterless constructor or ServiceHub will fail to construct them.
    /// </remarks>
    public RazorServiceFactoryBase(RazorServiceDescriptorsWrapper razorServiceDescriptors)
    {
        _razorServiceDescriptors = razorServiceDescriptors;
    }

    public async Task<object> CreateAsync(
        Stream stream,
        IServiceProvider hostProvidedServices,
        ServiceActivationOptions serviceActivationOptions,
        IServiceBroker serviceBroker,
        AuthorizationServiceClient? authorizationServiceClient)
    {
        // Dispose the AuthorizationServiceClient since we won't be using it
        authorizationServiceClient?.Dispose();

        var traceSource = RemoteLoggerFactory.Initialize(hostProvidedServices);

        var pipe = stream.UsePipe();

        var descriptor = _razorServiceDescriptors.GetDescriptorForServiceFactory(typeof(TService));
        var serverConnection = descriptor.WithTraceSource(traceSource).ConstructRpcConnection(pipe);

        var exportProvider = await RemoteMefComposition.GetExportProviderAsync().ConfigureAwait(false);

        var razorServiceBroker = new RazorServiceBroker(serviceBroker);

        var service = CreateService(razorServiceBroker, exportProvider);

        serverConnection.AddLocalRpcTarget(service);
        serverConnection.StartListening();

        return service;
    }

    protected abstract TService CreateService(IRazorServiceBroker serviceBroker, ExportProvider exportProvider);

    internal TestAccessor GetTestAccessor()
    {
        return new TestAccessor(this);
    }

    internal readonly struct TestAccessor(RazorServiceFactoryBase<TService> instance)
    {
        public TService CreateService(IRazorServiceBroker serviceBroker, ExportProvider exportProvider)
            => instance.CreateService(serviceBroker, exportProvider);
    }
}
