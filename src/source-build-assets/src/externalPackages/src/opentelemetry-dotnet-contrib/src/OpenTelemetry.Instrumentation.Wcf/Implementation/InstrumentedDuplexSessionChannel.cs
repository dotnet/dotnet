// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.ServiceModel.Channels;

namespace OpenTelemetry.Instrumentation.Wcf.Implementation;

internal sealed class InstrumentedDuplexSessionChannel : InstrumentedDuplexChannel, IDuplexSessionChannel
{
    private readonly IDuplexSessionChannel inner;

    public InstrumentedDuplexSessionChannel(IDuplexSessionChannel inner, TimeSpan telemetryTimeout)
        : base(inner, telemetryTimeout)
    {
        this.inner = inner;
    }

    public IDuplexSession Session => this.inner.Session;
}
