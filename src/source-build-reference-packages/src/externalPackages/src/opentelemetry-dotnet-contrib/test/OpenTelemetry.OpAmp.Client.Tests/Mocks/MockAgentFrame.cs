// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Google.Protobuf;
using OpAmp.Proto.V1;

namespace OpenTelemetry.OpAmp.Client.Tests.Mocks;

internal class MockAgentFrame
{
    public MockAgentFrame(ByteString uid, AgentToServer frame)
    {
        this.Uid = uid;
        this.Frame = frame;
    }

    public ByteString Uid { get; set; }

    public AgentToServer Frame { get; set; }
}
