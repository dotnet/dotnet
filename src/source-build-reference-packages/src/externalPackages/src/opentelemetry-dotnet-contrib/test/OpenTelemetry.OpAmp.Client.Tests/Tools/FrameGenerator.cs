// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Google.Protobuf;
using OpAmp.Proto.V1;
using OpenTelemetry.OpAmp.Client.Internal.Utils;
using OpenTelemetry.OpAmp.Client.Tests.Mocks;

namespace OpenTelemetry.OpAmp.Client.Tests.Tools;

internal class FrameGenerator
{
    public static MockAgentFrame GenerateMockAgentFrame(bool useSmallPackets = true)
    {
        var content = "This is a mock agent frame for testing purposes.";
        if (!useSmallPackets)
        {
            var padding = new string('A', 100_000);
            content = string.Concat(content, "__", padding);
        }

        var uid = ByteString.CopyFrom(Guid.NewGuid().ToByteArray());
        var frame = new AgentToServer()
        {
            InstanceUid = uid,
            CustomMessage = new CustomMessage()
            {
                Data = ByteString.CopyFromUtf8(content),
                Type = "Utf8String",
            },
        };

        return new MockAgentFrame(uid, frame);
    }

    public static MockServerFrame GenerateMockServerFrame(ByteString? instanceUid = null, bool useSmallPackets = true, bool addHeader = false)
    {
        var content = "This is a mock server frame for testing purposes.";
        if (!useSmallPackets)
        {
            var padding = new string('A', 100_000);
            content = string.Concat(content, "__", padding);
        }

        ByteString uid = instanceUid ?? ByteString.CopyFrom(Guid.NewGuid().ToByteArray());

        var frame = new ServerToAgent
        {
            InstanceUid = uid,
            CustomMessage = new CustomMessage()
            {
                Data = ByteString.CopyFromUtf8(content),
                Type = "Utf8String",
            },
        };
        var size = frame.CalculateSize();

        var responseBuffer = addHeader
            ? new byte[size + OpAmpWsHeaderHelper.MaxHeaderLength]
            : new byte[size];
        ArraySegment<byte> responseSegment;

        if (addHeader)
        {
            var headerSize = OpAmpWsHeaderHelper.WriteHeader(new ArraySegment<byte>(responseBuffer));
            var segment = new ArraySegment<byte>(responseBuffer, headerSize, size);
            frame.WriteTo(segment);
            responseSegment = new ArraySegment<byte>(responseBuffer, 0, size + headerSize);
        }
        else
        {
            frame.WriteTo(responseBuffer);
            responseSegment = new ArraySegment<byte>(responseBuffer, 0, size);
        }

        return new MockServerFrame
        {
            Frame = responseSegment,
            Size = size,
            HasHeader = addHeader,
            ExptectedContent = content,
        };
    }
}
