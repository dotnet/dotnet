// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Instrumentation.Wcf.Implementation;

/// <summary>
/// An <see cref="IClientMessageInspector"/> implementation which adds telemetry to outgoing requests.
/// </summary>
internal class TelemetryClientMessageInspector : IClientMessageInspector
{
    private readonly IDictionary<string, ActionMetadata> actionMappings;

    internal TelemetryClientMessageInspector(IDictionary<string, ActionMetadata> actionMappings)
    {
        Guard.ThrowIfNull(actionMappings);

        this.actionMappings = actionMappings;
    }

    /// <inheritdoc/>
    public object? BeforeSendRequest(ref Message request, IClientChannel channel)
    {
        Guard.ThrowIfNull(request);

        request.Properties.Add(TelemetryContextMessageProperty.Name, new TelemetryContextMessageProperty(
            actionMappings: this.actionMappings));
        return null;
    }

    /// <inheritdoc/>
    public void AfterReceiveReply(ref Message reply, object? correlationState)
    {
    }
}
