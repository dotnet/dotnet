// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Remote.Razor.Logging;
using Microsoft.ServiceHub.Framework;
using Microsoft.ServiceHub.Framework.Services;
using Nerdbank.Streams;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal abstract partial class RazorBrokeredServiceBase
{
    /// <remarks>
    /// Implementors of <see cref="IServiceHubServiceFactory" /> (and thus this class) MUST provide a parameterless constructor
    /// or ServiceHub will fail to construct them.
    /// </remarks>
    internal abstract class FactoryBase<TService> : IServiceHubServiceFactory
        where TService : class
    {
        protected abstract TService CreateService(in ServiceArgs args);

        public Task<object> CreateAsync(
            Stream stream,
            IServiceProvider hostProvidedServices,
            ServiceActivationOptions serviceActivationOptions,
            IServiceBroker serviceBroker,
            AuthorizationServiceClient? authorizationServiceClient)
        {
            // Dispose the AuthorizationServiceClient since we won't be using it
            authorizationServiceClient?.Dispose();

#if NET
            // So that we can control assembly loading, we re-load ourselves in the shared Razor ALC and perform the creation there.
            // That ensures that the service type we return is in the Razor ALC and any dependencies it needs will be handled by the
            // Razor ALC dependency loading rather than the default ServiceHub ALC that we're in right now.
            var assemblyInRazorAlc = RazorAssemblyLoadContext.Instance.LoadFromAssemblyName(GetType().Assembly.GetName());
            var thisInRazorAlc = assemblyInRazorAlc.CreateInstance(GetType().FullName!)!;

            var createInternalAsyncFunc = thisInRazorAlc.GetType().GetMethod(nameof(CreateInternalAsync), System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!;
            var result = (Task<object>)createInternalAsyncFunc.Invoke(thisInRazorAlc, [stream, hostProvidedServices, serviceBroker])!;
            return result;
#else
            return CreateInternalAsync(stream, hostProvidedServices, serviceBroker);
#endif
        }

        protected async Task<object> CreateInternalAsync(
            Stream stream,
            IServiceProvider hostProvidedServices,
            IServiceBroker serviceBroker)
        {
            var traceSource = (TraceSource?)hostProvidedServices.GetService(typeof(TraceSource));

            // RazorBrokeredServiceData is a hook that can be provided for different host scenarios, such as testing.
            var brokeredServiceData = (RazorBrokeredServiceData?)hostProvidedServices.GetService(typeof(RazorBrokeredServiceData));

            var exportProvider = brokeredServiceData?.ExportProvider
                ?? await RemoteMefComposition.GetSharedExportProviderAsync(CancellationToken.None).ConfigureAwait(false);

            // There are three logging cases:
            //
            // 1. We've been provided an ILoggerFactory from the host.
            // 2. We've been provided a TraceSource and create an ILoggerFactory for that.
            // 3. We don't have anything and just use the empty ILoggerFactory.

            var targetLoggerFactory = brokeredServiceData?.LoggerFactory
                ?? (traceSource is not null
                    ? new TraceSourceLoggerFactory(traceSource)
                    : EmptyLoggerFactory.Instance);

            // Update the MEF composition's ILoggerFactory to the target ILoggerFactory.
            // Note that this means that the first non-empty ILoggerFactory that we use
            // will be used for MEF component logging for the lifetime of all services.
            var remoteLoggerFactory = exportProvider.GetExportedValue<RemoteLoggerFactory>();
            remoteLoggerFactory.SetTargetLoggerFactory(targetLoggerFactory);

            var pipe = stream.UsePipe();

            var descriptor = typeof(IRemoteJsonService).IsAssignableFrom(typeof(TService))
                ? RazorServices.JsonDescriptors.GetDescriptorForServiceFactory(typeof(TService))
                : RazorServices.Descriptors.GetDescriptorForServiceFactory(typeof(TService));
            var serverConnection = descriptor.WithTraceSource(traceSource).ConstructRpcConnection(pipe);

            var args = new ServiceArgs(serviceBroker, exportProvider, targetLoggerFactory, serverConnection, brokeredServiceData?.Interceptor);

            var service = CreateService(in args);

            serverConnection.AddLocalRpcTarget(service);
            serverConnection.StartListening();

            return service;
        }
    }
}
