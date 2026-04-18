// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using OpenTelemetry.Instrumentation.Wcf.Implementation;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Instrumentation.Wcf;

#if NETFRAMEWORK
/// <summary>
/// An <see cref="IContractBehavior"/> <see cref="Attribute"/> to add the
/// <see cref="TelemetryDispatchMessageInspector"/> to service operations,
/// and <see cref="TelemetryClientMessageInspector"/> and
/// TelemetryBindingElement to client operations programmatically.
/// </summary>
#else
/// <summary>
/// An <see cref="IContractBehavior"/> <see cref="Attribute"/> to add the
/// <see cref="TelemetryClientMessageInspector"/> and
/// TelemetryBindingElement to client operations programmatically.
/// </summary>
#endif
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
public sealed class TelemetryContractBehaviorAttribute : Attribute, IContractBehavior
{
    /// <inheritdoc />
    public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
    {
    }

    /// <inheritdoc />
    public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
    {
        Guard.ThrowIfNull(clientRuntime);
        Guard.ThrowIfNull(endpoint);
        TelemetryEndpointBehavior.ApplyClientBehaviorToClientRuntime(clientRuntime);
        TelemetryEndpointBehavior.ApplyBindingElementToServiceEndpoint(endpoint);
    }

    /// <inheritdoc />
    public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
    {
#if NETFRAMEWORK
        Guard.ThrowIfNull(dispatchRuntime);
        dispatchRuntime.ChannelDispatcher.ErrorHandlers.Add(new TracingErrorHandler());
        TelemetryEndpointBehavior.ApplyDispatchBehaviorToEndpoint(dispatchRuntime.EndpointDispatcher);
#endif
    }

    /// <inheritdoc />
    public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
    {
    }
}
