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
/// An <see cref="IEndpointBehavior"/> implementation which adds the <see
/// cref="TelemetryBindingElement"/> to client endpoints and the
/// <see cref="TelemetryDispatchMessageInspector"/> to service endpoints.
/// </summary>
#else
/// <summary>
/// An <see cref="IEndpointBehavior"/> implementation which adds the <see
/// cref="TelemetryBindingElement"/> to client endpoints.
/// </summary>
#endif
public class TelemetryEndpointBehavior : IEndpointBehavior
{
    /// <inheritdoc/>
    public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
    {
    }

    /// <inheritdoc/>
    public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
    {
        Guard.ThrowIfNull(endpoint);
        Guard.ThrowIfNull(clientRuntime);
        ApplyClientBehaviorToClientRuntime(clientRuntime);
        ApplyBindingElementToServiceEndpoint(endpoint);
    }

    /// <inheritdoc/>
    public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
    {
#if NETFRAMEWORK
        Guard.ThrowIfNull(endpointDispatcher);
        endpointDispatcher.ChannelDispatcher.ErrorHandlers.Add(new TracingErrorHandler());
        ApplyDispatchBehaviorToEndpoint(endpointDispatcher);
#endif
    }

    /// <inheritdoc/>
    public void Validate(ServiceEndpoint endpoint)
    {
    }

    internal static void ApplyBindingElementToServiceEndpoint(ServiceEndpoint endpoint)
    {
#if NETFRAMEWORK
        if (endpoint.IsSystemEndpoint)
        {
            return;
        }
#endif

        if (endpoint.Binding is CustomBinding customBinding && customBinding.Elements.Find<TelemetryBindingElement>() != null)
        {
            return;
        }

        var newBinding = new CustomBinding(endpoint.Binding);
        newBinding.Elements.Insert(0, new TelemetryBindingElement());
        endpoint.Binding = newBinding;
    }

    internal static void ApplyClientBehaviorToClientRuntime(ClientRuntime clientRuntime)
    {
        if (clientRuntime.ClientMessageInspectors.Any(cmi => cmi is TelemetryClientMessageInspector))
        {
            return;
        }

        var actionMappings = new Dictionary<string, ActionMetadata>(StringComparer.OrdinalIgnoreCase);

        foreach (var clientOperation in clientRuntime.ClientOperations)
        {
            actionMappings[clientOperation.Action] = new ActionMetadata(
                contractName: $"{clientRuntime.ContractNamespace}{clientRuntime.ContractName}",
                operationName: clientOperation.Name);
        }

        clientRuntime.ClientMessageInspectors.Add(new TelemetryClientMessageInspector(actionMappings));
    }

#if NETFRAMEWORK
    internal static void ApplyDispatchBehaviorToEndpoint(EndpointDispatcher endpointDispatcher)
    {
        var actionMappings = new Dictionary<string, ActionMetadata>(StringComparer.OrdinalIgnoreCase);

        foreach (var dispatchOperation in endpointDispatcher.DispatchRuntime.Operations)
        {
            actionMappings[dispatchOperation.Action] = new ActionMetadata(
                contractName: $"{endpointDispatcher.ContractNamespace}{endpointDispatcher.ContractName}",
                operationName: dispatchOperation.Name);
        }

        endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new TelemetryDispatchMessageInspector(actionMappings));
    }
#endif
}
