// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK
using System.ServiceModel.Configuration;

namespace OpenTelemetry.Instrumentation.Wcf;

/// <summary>
/// A <see cref="BehaviorExtensionElement"/> for registering <see cref="TelemetryEndpointBehavior"/> on a service endpoint through configuration.
/// </summary>
public class TelemetryEndpointBehaviorExtensionElement : BehaviorExtensionElement
{
    /// <inheritdoc/>
    public override Type BehaviorType => typeof(TelemetryEndpointBehavior);

    /// <inheritdoc/>
    protected override object CreateBehavior()
    {
        return new TelemetryEndpointBehavior();
    }
}

#endif
