// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Reflection;
using System.ServiceModel.Channels;

namespace OpenTelemetry.Instrumentation.Wcf.Tests.Tools;

public class DownstreamInstrumentationChannelFactory<TChannel> : DispatchProxy
    where TChannel : notnull
{
    public IChannelFactory<TChannel>? Target { get; set; }

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        var returnValue = targetMethod!.Invoke(this.Target, args);
        return targetMethod.Name == nameof(IChannelFactory<TChannel>.CreateChannel) ? DownstreamInstrumentationChannel.Create((TChannel)returnValue!) : returnValue;
    }
}
