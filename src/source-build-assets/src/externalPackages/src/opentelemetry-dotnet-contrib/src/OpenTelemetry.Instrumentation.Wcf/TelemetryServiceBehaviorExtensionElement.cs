// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK
using System.ServiceModel.Configuration;

namespace OpenTelemetry.Instrumentation.Wcf;

/// <summary>
/// A <see cref="BehaviorExtensionElement"/> for registering <see cref="TelemetryServiceBehavior"/> on a service through configuration.
/// </summary>
public class TelemetryServiceBehaviorExtensionElement : BehaviorExtensionElement
{
    /// <inheritdoc/>
    public override Type BehaviorType => typeof(TelemetryServiceBehavior);

    /// <inheritdoc/>
    protected override object CreateBehavior()
    {
        return new TelemetryServiceBehavior();
    }
}

#endif
