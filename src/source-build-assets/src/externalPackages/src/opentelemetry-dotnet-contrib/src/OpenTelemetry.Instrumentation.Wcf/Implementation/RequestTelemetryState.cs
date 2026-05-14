// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;

namespace OpenTelemetry.Instrumentation.Wcf.Implementation;

internal sealed class RequestTelemetryState
{
    public IDisposable? SuppressionScope { get; set; }

    public Activity? Activity { get; set; }
}
