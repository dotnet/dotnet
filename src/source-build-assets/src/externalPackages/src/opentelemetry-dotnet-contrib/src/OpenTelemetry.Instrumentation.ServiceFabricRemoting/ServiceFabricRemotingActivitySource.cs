// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Reflection;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Instrumentation.ServiceFabricRemoting;

internal class ServiceFabricRemotingActivitySource
{
    internal static readonly Assembly Assembly = typeof(ServiceFabricRemotingActivitySource).Assembly;
    internal static readonly AssemblyName AssemblyName = Assembly.GetName();
#pragma warning disable IDE0370 // Suppression is unnecessary
    internal static readonly string ActivitySourceName = AssemblyName.Name!;
#pragma warning restore IDE0370 // Suppression is unnecessary

    internal static readonly string IncomingRequestActivityName = ActivitySourceName + ".IncomingRequest";
    internal static readonly string OutgoingRequestActivityName = ActivitySourceName + ".OutgoingRequest";

    public static ActivitySource ActivitySource { get; } = new(ActivitySourceName, Assembly.GetPackageVersion());

    public static ServiceFabricRemotingInstrumentationOptions? Options { get; set; }
}
