// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Reflection;
using System.ServiceModel.Channels;

namespace OpenTelemetry.Instrumentation.Wcf.Tests.Tools;

public class DownstreamInstrumentationChannel : DispatchProxy
{
    public const string DownstreamInstrumentationSourceName = "DownstreamInstrumentationSource";
    private static readonly ActivitySource DownstreamInstrumentationSource = new(DownstreamInstrumentationSourceName);
    private static bool failNextReceive;

    private object? Target { get; set; }

    public static TChannel Create<TChannel>(TChannel target)
        where TChannel : notnull
    {
        var proxy = (DownstreamInstrumentationChannel)(object)Create<TChannel, DownstreamInstrumentationChannel>();
        proxy.Target = target;
        return (TChannel)(object)proxy;
    }

    public static void FailNextReceive(bool shouldFail = true)
    {
        failNextReceive = shouldFail;
    }

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        var toInstrument = new[]
        {
            nameof(IRequestChannel.Request),
            nameof(IRequestChannel.BeginRequest),
            nameof(IOutputChannel.Send),
            nameof(IOutputChannel.BeginSend),
        };

        using var activity = toInstrument.Contains(targetMethod!.Name) ? DownstreamInstrumentationSource.StartActivity("DownstreamInstrumentation") : null;

        var receiveMethods = new[]
        {
            nameof(IInputChannel.Receive),
            nameof(IInputChannel.BeginReceive),
            nameof(IInputChannel.TryReceive),
            nameof(IInputChannel.BeginTryReceive),
        };

        if (failNextReceive && receiveMethods.Contains(targetMethod!.Name))
        {
            failNextReceive = false;
            throw new Exception();
        }

        return targetMethod.Invoke(this.Target, args);
    }
}
