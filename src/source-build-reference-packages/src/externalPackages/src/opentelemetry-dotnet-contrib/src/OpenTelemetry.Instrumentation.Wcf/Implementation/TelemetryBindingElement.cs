// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.ServiceModel.Channels;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Instrumentation.Wcf.Implementation;

/// <summary>
/// A <see cref="BindingElement"/> that can be used to instrument WCF clients.
/// </summary>
internal sealed class TelemetryBindingElement : BindingElement
{
    /// <inheritdoc/>
    public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context)
    {
        Guard.ThrowIfNull(context);

        return new InstrumentedChannelFactory<TChannel>(context.BuildInnerChannelFactory<TChannel>(), context.Binding);
    }

    /// <inheritdoc/>
    public override bool CanBuildChannelFactory<TChannel>(BindingContext context)
    {
        Guard.ThrowIfNull(context);

        var supportedByInstrumentation =
            typeof(TChannel) == typeof(IRequestChannel) ||
            typeof(TChannel) == typeof(IRequestSessionChannel) ||
            typeof(TChannel) == typeof(IDuplexChannel) ||
            typeof(TChannel) == typeof(IDuplexSessionChannel);

        return supportedByInstrumentation && base.CanBuildChannelFactory<TChannel>(context);
    }

    /// <inheritdoc/>
    public override BindingElement Clone()
    {
        return new TelemetryBindingElement();
    }

    /// <inheritdoc/>
    public override T GetProperty<T>(BindingContext context)
    {
        Guard.ThrowIfNull(context);

        return context.GetInnerProperty<T>();
    }
}
