// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.ServiceModel.Channels;

namespace OpenTelemetry.Instrumentation.Wcf.Implementation;

internal class InstrumentedChannel<T> : InstrumentedCommunicationObject<T>, IChannel
    where T : IChannel
{
    public InstrumentedChannel(T inner)
        : base(inner)
    {
    }

    TProperty IChannel.GetProperty<TProperty>()
        where TProperty : class
    {
        return this.Inner.GetProperty<TProperty>();
    }
}
