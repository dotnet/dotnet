// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Text;
using Google.Protobuf;
using Xunit;

namespace OpenTelemetry.OpAmp.Client.Tests.Messages;

public class RemoteConfigMessageTests
{
    private const string JsonString = "{ \"myKey\": \"this is a value\" }";
    private const string YamlString = "enabled: true";
    private const string HashString = "dummy-hash";
    private const string Config1Name = "config1";
    private const string Config2Name = "config2";
    private const string JsonContentType = "application/json";
    private const string YamlContentType = "application/yaml";

    [Fact]
    public void Constructor_WithValidConfig_InitializesConfigMap()
    {
        // Arrange
        var agentRemoteConfig = this.CreateAgentRemoteConfig();

        // Act
        var remoteConfigMessage = new Client.Messages.RemoteConfigMessage(agentRemoteConfig);

        // Assert
        Assert.Equal(2, remoteConfigMessage.AgentConfigMap.Count);
        Assert.True(remoteConfigMessage.AgentConfigMap.ContainsKey(Config1Name));
        Assert.True(remoteConfigMessage.AgentConfigMap.ContainsKey(Config2Name));
    }

    [Fact]
    public void Constructor_WithEmptyConfigMap_InitializesEmptyDictionary()
    {
        // Arrange
        var agentRemoteConfig = new global::OpAmp.Proto.V1.AgentRemoteConfig
        {
            Config = new global::OpAmp.Proto.V1.AgentConfigMap(),
            ConfigHash = ByteString.CopyFromUtf8(HashString),
        };

        // Act
        var remoteConfigMessage = new Client.Messages.RemoteConfigMessage(agentRemoteConfig);

        // Assert
        Assert.Empty(remoteConfigMessage.AgentConfigMap);
    }

    [Fact]
    public void ConfigHash_WithValidHash_ReturnsExpectedBytes()
    {
        // Arrange
        var agentRemoteConfig = this.CreateAgentRemoteConfig();
        var remoteConfigMessage = new Client.Messages.RemoteConfigMessage(agentRemoteConfig);

        // Act
        var hash = remoteConfigMessage.ConfigHash;

        // Assert
        Assert.Equal(Encoding.UTF8.GetByteCount(HashString), hash.Length);
        Assert.Equal(HashString, Encoding.UTF8.GetString(hash.ToArray()));
    }

    [Fact]
    public void ConfigHash_WithEmptyHash_ReturnsEmptySpan()
    {
        // Arrange
        var agentRemoteConfig = new global::OpAmp.Proto.V1.AgentRemoteConfig
        {
            Config = new global::OpAmp.Proto.V1.AgentConfigMap(),
            ConfigHash = ByteString.Empty,
        };
        var remoteConfigMessage = new Client.Messages.RemoteConfigMessage(agentRemoteConfig);

        // Act
        var hashSpan = remoteConfigMessage.ConfigHash;

        // Assert
        Assert.Equal(0, hashSpan.Length);
        Assert.True(hashSpan.IsEmpty);
    }

    [Theory]
    [InlineData(Config1Name, JsonContentType, JsonString)]
    [InlineData(Config2Name, YamlContentType, YamlString)]
    public void AgentConfigFile_Properties_ReturnExpectedValues(string configName, string contentType, string bodyContent)
    {
        // Arrange
        var agentRemoteConfig = this.CreateAgentRemoteConfig();
        var remoteConfigMessage = new Client.Messages.RemoteConfigMessage(agentRemoteConfig);

        // Act
        var configFile = remoteConfigMessage.AgentConfigMap[configName];

        // Assert
        Assert.Equal(configName, configFile.Name);
        Assert.Equal(contentType, configFile.ContentType);
        Assert.Equal(bodyContent, Encoding.UTF8.GetString(configFile.Body.ToArray()));
    }

    [Fact]
    public void AgentConfigFile_Body_WithEmptyBody_ReturnsEmptySpan()
    {
        // Arrange
        var configMap = new global::OpAmp.Proto.V1.AgentConfigMap();
        configMap.ConfigMap.Add("empty-config", new global::OpAmp.Proto.V1.AgentConfigFile
        {
            Body = ByteString.Empty,
            ContentType = JsonContentType,
        });

        var agentRemoteConfig = new global::OpAmp.Proto.V1.AgentRemoteConfig
        {
            Config = configMap,
            ConfigHash = ByteString.CopyFromUtf8(HashString),
        };

        var remoteConfigMessage = new Client.Messages.RemoteConfigMessage(agentRemoteConfig);
        var configFile = remoteConfigMessage.AgentConfigMap["empty-config"];

        // Act
        var bodySpan = configFile.Body;

        // Assert
        Assert.Equal(0, bodySpan.Length);
        Assert.True(bodySpan.IsEmpty);
    }

    [Fact]
    public void AgentConfigFile_WithEmptyContentType_ReturnsEmptyString()
    {
        // Arrange
        var configMap = new global::OpAmp.Proto.V1.AgentConfigMap();
        configMap.ConfigMap.Add("empty-content-type", new global::OpAmp.Proto.V1.AgentConfigFile
        {
            Body = ByteString.CopyFromUtf8(JsonString),
            ContentType = string.Empty,
        });

        var agentRemoteConfig = new global::OpAmp.Proto.V1.AgentRemoteConfig
        {
            Config = configMap,
            ConfigHash = ByteString.CopyFromUtf8(HashString),
        };

        var remoteConfigMessage = new Client.Messages.RemoteConfigMessage(agentRemoteConfig);

        // Act
        var configFile = remoteConfigMessage.AgentConfigMap["empty-content-type"];

        // Assert
        Assert.Equal(string.Empty, configFile.ContentType);
    }

    [Fact]
    public void AgentConfigMap_IsReadOnly()
    {
        // Arrange
        var agentRemoteConfig = this.CreateAgentRemoteConfig();
        var remoteConfigMessage = new Client.Messages.RemoteConfigMessage(agentRemoteConfig);

        // Act & Assert
        Assert.IsType<IReadOnlyDictionary<string, Client.Messages.AgentConfigFile>>(
            remoteConfigMessage.AgentConfigMap, exactMatch: false);
    }

    [Fact]
    public void AgentConfigMap_UsesOrdinalComparison()
    {
        // Arrange
        var configMap = new global::OpAmp.Proto.V1.AgentConfigMap();
        configMap.ConfigMap.Add("Config", new global::OpAmp.Proto.V1.AgentConfigFile
        {
            Body = ByteString.CopyFromUtf8(JsonString),
            ContentType = JsonContentType,
        });

        var agentRemoteConfig = new global::OpAmp.Proto.V1.AgentRemoteConfig
        {
            Config = configMap,
            ConfigHash = ByteString.CopyFromUtf8(HashString),
        };

        var remoteConfigMessage = new Client.Messages.RemoteConfigMessage(agentRemoteConfig);

        // Act & Assert - case-sensitive check
        Assert.True(remoteConfigMessage.AgentConfigMap.ContainsKey("Config"));
        Assert.False(remoteConfigMessage.AgentConfigMap.ContainsKey("config"));
    }

    [Fact]
    public void ConfigHash_MultipleAccess_ReturnsSameSpan()
    {
        // Arrange
        var agentRemoteConfig = this.CreateAgentRemoteConfig();
        var remoteConfigMessage = new Client.Messages.RemoteConfigMessage(agentRemoteConfig);

        // Act
        var hash1 = remoteConfigMessage.ConfigHash;
        var hash2 = remoteConfigMessage.ConfigHash;

        // Assert
        Assert.True(hash1.SequenceEqual(hash2));
    }

    [Fact]
    public void AgentConfigFile_Body_MultipleAccess_ReturnsSameSpan()
    {
        // Arrange
        var agentRemoteConfig = this.CreateAgentRemoteConfig();
        var remoteConfigMessage = new Client.Messages.RemoteConfigMessage(agentRemoteConfig);
        var configFile = remoteConfigMessage.AgentConfigMap[Config1Name];

        // Act
        var body1 = configFile.Body;
        var body2 = configFile.Body;

        // Assert
        Assert.True(body1.SequenceEqual(body2));
    }

#if NET
    [Fact]
    public void AgentConfigFile_JsonContent_CanBeDeserialized()
    {
        // Arrange
        var agentRemoteConfig = this.CreateAgentRemoteConfig();
        var remoteConfigMessage = new Client.Messages.RemoteConfigMessage(agentRemoteConfig);
        var config1 = remoteConfigMessage.AgentConfigMap[Config1Name];

        // Act
        var config = System.Text.Json.JsonSerializer.Deserialize<Config>(config1.Body);

        // Assert
        Assert.NotNull(config);
        Assert.Equal("this is a value", config!.MyKey);
    }
#endif

    private global::OpAmp.Proto.V1.AgentRemoteConfig CreateAgentRemoteConfig()
    {
        var configMap = new global::OpAmp.Proto.V1.AgentConfigMap();

        configMap.ConfigMap.Add(Config1Name, new global::OpAmp.Proto.V1.AgentConfigFile
        {
            Body = ByteString.CopyFromUtf8(JsonString),
            ContentType = JsonContentType,
        });

        configMap.ConfigMap.Add(Config2Name, new global::OpAmp.Proto.V1.AgentConfigFile
        {
            Body = ByteString.CopyFromUtf8(YamlString),
            ContentType = YamlContentType,
        });

        return new global::OpAmp.Proto.V1.AgentRemoteConfig
        {
            Config = configMap,
            ConfigHash = ByteString.CopyFromUtf8(HashString),
        };
    }

#if NET
    private class Config
    {
        [System.Text.Json.Serialization.JsonPropertyName("myKey")]
        public string? MyKey { get; set; }
    }
#endif
}
