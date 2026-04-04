// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Xunit;

namespace OpenTelemetry.OpAmp.Client.Tests.Messages;

public class CustomCapabilitiesMessageTests
{
    private const string Capability1Name = "Capability1";
    private const string Capability2Name = "Capability2";

    [Fact]
    public void Constructor_WithValidCapabilities_InitializesCapabilities()
    {
        // Arrange
        var customCapabilities = this.CreateCustomCapabilities();

        // Act
        var customCapabilitiesMessage = new Client.Messages.CustomCapabilitiesMessage(customCapabilities);

        // Assert
        Assert.Equal(2, customCapabilitiesMessage.Capabilities.Count);
        Assert.True(customCapabilitiesMessage.Capabilities.Contains(Capability1Name));
        Assert.True(customCapabilitiesMessage.Capabilities.Contains(Capability2Name));
    }

    [Fact]
    public void Constructor_WithEmptyCapabilities_InitializesEmptyCapabilitiesCollection()
    {
        // Arrange
        var customCapabilities = new global::OpAmp.Proto.V1.CustomCapabilities();

        // Act
        var customCapabilitiesMessage = new Client.Messages.CustomCapabilitiesMessage(customCapabilities);

        // Assert
        Assert.Empty(customCapabilitiesMessage.Capabilities);
    }

    private global::OpAmp.Proto.V1.CustomCapabilities CreateCustomCapabilities()
    {
        var capabilities = new global::OpAmp.Proto.V1.CustomCapabilities();
        capabilities.Capabilities.Add(Capability1Name);
        capabilities.Capabilities.Add(Capability2Name);

        return capabilities;
    }
}
