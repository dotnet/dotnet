// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.ServiceModel.Channels;

namespace OpenTelemetry.Instrumentation.Wcf.Implementation;

internal class InstrumentedChannelFactoryBase<T> : InstrumentedCommunicationObject<T>, IChannelFactory
    where T : IChannelFactory
{
    public InstrumentedChannelFactoryBase(T inner)
        : base(inner)
    {
    }

    TProperty IChannelFactory.GetProperty<TProperty>()
        where TProperty : class
    {
        return this.Inner.GetProperty<TProperty>();
    }
}
