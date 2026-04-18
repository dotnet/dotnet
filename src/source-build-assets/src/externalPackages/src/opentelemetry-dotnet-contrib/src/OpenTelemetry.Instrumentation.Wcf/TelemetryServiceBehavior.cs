// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using OpenTelemetry.Instrumentation.Wcf.Implementation;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Instrumentation.Wcf;

/// <summary>
/// An <see cref="IServiceBehavior"/> implementation to add the telemetry to service operations.
/// </summary>
public class TelemetryServiceBehavior : IServiceBehavior
{
    /// <inheritdoc />
    public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
    {
    }

    /// <inheritdoc/>
    public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
    {
        Guard.ThrowIfNull(serviceHostBase);

        foreach (var channelDispatcherBase in serviceHostBase.ChannelDispatchers)
        {
            var channelDispatcher = (ChannelDispatcher)channelDispatcherBase;

            channelDispatcher.ErrorHandlers.Add(new TracingErrorHandler());

            foreach (var endpointDispatcher in channelDispatcher.Endpoints)
            {
                TelemetryEndpointBehavior.ApplyDispatchBehaviorToEndpoint(endpointDispatcher);
            }
        }
    }

    /// <inheritdoc/>
    public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
    {
    }
}
#endif
