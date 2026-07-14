// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.ServiceModel.Channels;
using OpenTelemetry.Instrumentation.Wcf.Implementation;

namespace OpenTelemetry.Instrumentation.Wcf;

/// <summary>
/// WCF instrumentation.
/// </summary>
internal static class WcfInstrumentationActivitySource
{
    internal static readonly Version SemanticConventionsVersion = new(1, 23, 0);
    internal static readonly ActivitySource ActivitySource = Trace.ActivitySourceFactory.Create(typeof(WcfInstrumentationActivitySource), SemanticConventionsVersion);

    internal static readonly Version SemanticConventionsVersionNew = new(1, 42, 0);
    internal static readonly ActivitySource ActivitySourceNew = Trace.ActivitySourceFactory.Create(typeof(WcfInstrumentationActivitySource), SemanticConventionsVersionNew);

    internal static readonly ActivitySource ActivitySourceBoth = Trace.ActivitySourceFactory.Create(typeof(WcfInstrumentationActivitySource), null);

    internal static readonly string IncomingRequestActivityName = ActivitySource.Name + ".IncomingRequest";
    internal static readonly string OutgoingRequestActivityName = ActivitySource.Name + ".OutgoingRequest";
    internal static readonly string UnassociatedExceptionActivityName = ActivitySource.Name + ".Exception";

    public static WcfInstrumentationOptions? Options { get; set; }

    public static IEnumerable<string>? MessageHeaderValuesGetter(Message request, string name)
        => TelemetryPropagationReader.Default(request, name);

    public static void MessageHeaderValueSetter(Message request, string name, string value)
        => TelemetryPropagationWriter.Default(request, name, value);

    public static ActivitySource Get(WcfInstrumentationOptions? options) =>
        options == null || !options.EmitNewRpcAttributes
        ? ActivitySource
        : options.EmitOldRpcAttributes ? ActivitySourceBoth : ActivitySourceNew;
}
