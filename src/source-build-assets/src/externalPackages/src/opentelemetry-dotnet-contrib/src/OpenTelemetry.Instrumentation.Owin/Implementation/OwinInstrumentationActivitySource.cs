// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;

namespace OpenTelemetry.Instrumentation.Owin;

internal static class OwinInstrumentationActivitySource
{
    internal static readonly Version SemanticConventionsVersion = new(1, 41, 0);
    internal static readonly ActivitySource ActivitySource = Trace.ActivitySourceFactory.Create(typeof(OwinInstrumentationActivitySource), SemanticConventionsVersion);
    internal static readonly string IncomingRequestActivityName = ActivitySource.Name + ".IncomingRequest";

    public static OwinInstrumentationOptions? Options { get; set; }
}
