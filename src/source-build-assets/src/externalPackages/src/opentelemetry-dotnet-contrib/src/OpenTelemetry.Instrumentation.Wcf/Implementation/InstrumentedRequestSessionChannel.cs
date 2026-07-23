// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.ServiceModel.Channels;

namespace OpenTelemetry.Instrumentation.Wcf.Implementation;

internal sealed class InstrumentedRequestSessionChannel : InstrumentedRequestChannel, IRequestSessionChannel
{
    private readonly IRequestSessionChannel inner;

    public InstrumentedRequestSessionChannel(IRequestSessionChannel inner)
        : base(inner)
    {
        this.inner = inner;
    }

    IOutputSession ISessionChannel<IOutputSession>.Session => this.inner.Session;
}
