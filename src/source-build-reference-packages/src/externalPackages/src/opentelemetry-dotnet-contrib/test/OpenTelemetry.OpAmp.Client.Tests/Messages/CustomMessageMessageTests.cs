// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Text;
using Google.Protobuf;
using Xunit;

namespace OpenTelemetry.OpAmp.Client.Tests.Messages;

public class CustomMessageMessageTests
{
    private const string CapabilityName = "Capability1";
    private const string TypeName = "CustomMessageType";
    private const string DataString = "Custom Message Data Example";

    [Fact]
    public void Constructor_WithValidMessage_InitializesCustomMessage()
    {
        // Arrange
        var customMessage = this.CreateCustomMessagesMessage();

        // Act
        var customMessageMessage = new Client.Messages.CustomMessageMessage(customMessage);

        // Assert
        Assert.Equal(CapabilityName, customMessageMessage.Capability);
        Assert.Equal(TypeName, customMessageMessage.Type);
#if NET
        Assert.Equal(DataString, Encoding.UTF8.GetString(customMessageMessage.Data));
#else
        Assert.Equal(DataString, Encoding.UTF8.GetString([.. customMessageMessage.Data]));
#endif
    }

    private global::OpAmp.Proto.V1.CustomMessage CreateCustomMessagesMessage()
    {
        return new global::OpAmp.Proto.V1.CustomMessage
        {
            Capability = CapabilityName,
            Type = TypeName,
            Data = ByteString.CopyFromUtf8(DataString),
        };
    }
}
