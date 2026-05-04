// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Reflection;
using System.ServiceModel.Channels;
using OpenTelemetry.Instrumentation.Wcf.Implementation;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Instrumentation.Wcf;

/// <summary>
/// WCF instrumentation.
/// </summary>
internal static class WcfInstrumentationActivitySource
{
    internal static readonly Assembly Assembly = typeof(WcfInstrumentationActivitySource).Assembly;
    internal static readonly AssemblyName AssemblyName = Assembly.GetName();
    internal static readonly string ActivitySourceName = AssemblyName.Name!;
    internal static readonly string IncomingRequestActivityName = ActivitySourceName + ".IncomingRequest";
    internal static readonly string OutgoingRequestActivityName = ActivitySourceName + ".OutgoingRequest";
    internal static readonly string UnassociatedExceptionActivityName = ActivitySourceName + ".Exception";

    public static ActivitySource ActivitySource { get; } = new(ActivitySourceName, Assembly.GetPackageVersion());

    public static WcfInstrumentationOptions? Options { get; set; }

    public static IEnumerable<string>? MessageHeaderValuesGetter(Message request, string name)
    {
        return TelemetryPropagationReader.Default(request, name);
    }

    public static void MessageHeaderValueSetter(Message request, string name, string value)
    {
        TelemetryPropagationWriter.Default(request, name, value);
    }
}
