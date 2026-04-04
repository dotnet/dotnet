// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpAmp.Proto.V1;
using OpenTelemetry.OpAmp.Client.Internal;
using OpenTelemetry.OpAmp.Client.Internal.Services.Heartbeat;
using Xunit;

namespace OpenTelemetry.OpAmp.Client.Tests.DataGenerators;

internal class FrameBuilderTestData
    : TheoryData<Func<IFrameBuilder, IFrameBuilder>, Func<AgentToServer, object>>
{
    public FrameBuilderTestData()
    {
        this.Add(fb => fb.AddAgentDescription(), m => m.AgentDescription);

        this.Add(fb => fb.AddHealth(new HealthReport { IsHealthy = true }), m => m.Health);

        this.Add(fb => fb.AddAgentDisconnect(), m => m.AgentDisconnect);

        this.Add(fb => fb.AddCapabilities(), m => m.Capabilities);
    }
}
