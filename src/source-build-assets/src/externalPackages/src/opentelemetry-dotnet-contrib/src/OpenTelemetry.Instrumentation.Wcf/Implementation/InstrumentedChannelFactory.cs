// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.ServiceModel;
using System.ServiceModel.Channels;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Instrumentation.Wcf.Implementation;

internal sealed class InstrumentedChannelFactory<TChannel> : InstrumentedChannelFactoryBase<IChannelFactory<TChannel>>, IChannelFactory<TChannel>
{
    private readonly CustomBinding binding;

    public InstrumentedChannelFactory(IChannelFactory<TChannel> inner, CustomBinding binding)
        : base(inner)
    {
        this.binding = binding;
    }

    TChannel IChannelFactory<TChannel>.CreateChannel(EndpointAddress to, Uri via)
        => this.WrapChannel(this.Inner.CreateChannel(to, via));

    TChannel IChannelFactory<TChannel>.CreateChannel(EndpointAddress to)
        => this.WrapChannel(this.Inner.CreateChannel(to));

    private TChannel WrapChannel(TChannel innerChannel)
    {
        Guard.ThrowIfNull(innerChannel);

        return typeof(TChannel) == typeof(IRequestChannel) || typeof(TChannel) == typeof(IRequestSessionChannel)
            ? (TChannel)(IRequestChannel)new InstrumentedRequestChannel((IRequestChannel)innerChannel)
            : typeof(TChannel) == typeof(IDuplexChannel) || typeof(TChannel) == typeof(IDuplexSessionChannel)
            ? (TChannel)(IDuplexChannel)new InstrumentedDuplexChannel((IDuplexChannel)innerChannel, this.binding.SendTimeout)
            : throw new NotImplementedException();
    }
}
