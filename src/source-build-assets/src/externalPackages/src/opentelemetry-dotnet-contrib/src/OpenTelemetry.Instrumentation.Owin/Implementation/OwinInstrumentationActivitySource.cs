// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Reflection;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Instrumentation.Owin;

internal static class OwinInstrumentationActivitySource
{
    internal static readonly Assembly Assembly = typeof(OwinInstrumentationActivitySource).Assembly;
    internal static readonly AssemblyName AssemblyName = Assembly.GetName();
    internal static readonly string ActivitySourceName = AssemblyName.Name;
    internal static readonly string IncomingRequestActivityName = ActivitySourceName + ".IncomingRequest";

    public static ActivitySource ActivitySource { get; } = new(ActivitySourceName, Assembly.GetPackageVersion());

    public static OwinInstrumentationOptions? Options { get; set; }
}
