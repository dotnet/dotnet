// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Trace;

namespace OpenTelemetry.Instrumentation.Wcf.Tests.Tools;

internal static class DownstreamInstrumentationExtensions
{
    public static TracerProviderBuilder AddDownstreamInstrumentation(this TracerProviderBuilder builder) =>
        builder.AddSource(DownstreamInstrumentationChannel.DownstreamInstrumentationSourceName);
}
