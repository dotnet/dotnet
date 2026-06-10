// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using NuGet.Common;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    using SemanticVersion = Versioning.SemanticVersion;

    /// <summary>
    /// Verifies that NSJ and STJ serialization/deserialization of <see cref="Message"/> produce identical
    /// wire format and correctly round-trip every payload type.
    /// </summary>
    public class MessageConverterTests
    {
        private static readonly SemanticVersion V100 = new SemanticVersion(1, 0, 0);
        private static readonly SemanticVersion V200 = new SemanticVersion(2, 0, 0);
        private static readonly string ServiceIndexJson = "{\"version\":\"3.0.0\",\"resources\":[]}";

        private static string SerializeWithStj(Message message) =>
            System.Text.Json.JsonSerializer.Serialize(message, PluginJsonContext.Default.Message);

        private static string SerializeWithNsj(Message message)
        {
            using var sw = new StringWriter();
            using var jw = new Newtonsoft.Json.JsonTextWriter(sw) { CloseOutput = false };
            JsonSerializationUtilities.Serialize(jw, message);
            return sw.ToString();
        }

        private static Message DeserializeWithStj(string json) =>
            System.Text.Json.JsonSerializer.Deserialize(json, PluginJsonContext.Default.Message)!;

        private static Message DeserializeWithNsj(string json) =>
            JsonSerializationUtilities.Deserialize<Message>(json);

        private static object[] Msg(MessageType type, MessageMethod method, object payload) =>
            new object[] { MessageUtilities.Create("test-id", type, method, payload) };

        public static IEnumerable<object[]> AllMessages()
        {
            // Fault
            yield return Msg(MessageType.Fault, MessageMethod.None, new Fault("something went wrong"));

            // Progress — with and without percentage
            yield return Msg(MessageType.Progress, MessageMethod.None, new Progress(0.5));
            yield return Msg(MessageType.Progress, MessageMethod.None, new Progress());

            // Handshake
            yield return Msg(MessageType.Request, MessageMethod.Handshake, new HandshakeRequest(V100, V100));
            yield return Msg(MessageType.Request, MessageMethod.Handshake, new HandshakeRequest(V200, V100));
            yield return Msg(MessageType.Response, MessageMethod.Handshake, new HandshakeResponse(MessageResponseCode.Success, V100));
            // Non-success response must have null version
            yield return Msg(MessageType.Response, MessageMethod.Handshake, new HandshakeResponse(MessageResponseCode.Error, null));

            // Initialize
            yield return Msg(MessageType.Request, MessageMethod.Initialize, new InitializeRequest("1.0.0", "en-US", TimeSpan.FromSeconds(30)));
            yield return Msg(MessageType.Response, MessageMethod.Initialize, new InitializeResponse(MessageResponseCode.Success));

            // GetOperationClaims — both fields can be null (source-agnostic)
            yield return Msg(MessageType.Request, MessageMethod.GetOperationClaims,
                new GetOperationClaimsRequest("https://contoso.test", ServiceIndexJson));
            yield return Msg(MessageType.Request, MessageMethod.GetOperationClaims,
                new GetOperationClaimsRequest((string?)null, (string?)null));
            yield return Msg(MessageType.Response, MessageMethod.GetOperationClaims,
                new GetOperationClaimsResponse(new[] { OperationClaim.DownloadPackage }));
            yield return Msg(MessageType.Response, MessageMethod.GetOperationClaims,
                new GetOperationClaimsResponse(new[] { OperationClaim.DownloadPackage, OperationClaim.Authentication }));

            // GetServiceIndex
            yield return Msg(MessageType.Request, MessageMethod.GetServiceIndex,
                new GetServiceIndexRequest("https://contoso.test"));
            yield return Msg(MessageType.Response, MessageMethod.GetServiceIndex,
                new GetServiceIndexResponse(MessageResponseCode.Success, ServiceIndexJson));

            // GetAuthenticationCredentials
            yield return Msg(MessageType.Request, MessageMethod.GetAuthenticationCredentials,
                new GetAuthenticationCredentialsRequest(new Uri("https://contoso.test"), isRetry: false, isNonInteractive: false, canShowDialog: true));
            yield return Msg(MessageType.Request, MessageMethod.GetAuthenticationCredentials,
                new GetAuthenticationCredentialsRequest(new Uri("https://contoso.test"), isRetry: true, isNonInteractive: true, canShowDialog: false));
            // Response — all fields populated
            yield return Msg(MessageType.Response, MessageMethod.GetAuthenticationCredentials,
                new GetAuthenticationCredentialsResponse("user", "pass", "ok", new[] { "basic", "digest" }, MessageResponseCode.Success));
            // Response — optional fields null
            yield return Msg(MessageType.Response, MessageMethod.GetAuthenticationCredentials,
                new GetAuthenticationCredentialsResponse(null, null, null, null, MessageResponseCode.Error));

            // GetCredentials
            yield return Msg(MessageType.Request, MessageMethod.GetCredentials,
                new GetCredentialsRequest("https://contoso.test", HttpStatusCode.Unauthorized));
            yield return Msg(MessageType.Response, MessageMethod.GetCredentials,
                new GetCredentialsResponse(MessageResponseCode.Success, "user", "pass", new[] { "basic" }));
            // Response — null optional fields
            yield return Msg(MessageType.Response, MessageMethod.GetCredentials,
                new GetCredentialsResponse(MessageResponseCode.Error, null, null, null));

            // Log
            yield return Msg(MessageType.Request, MessageMethod.Log,
                new LogRequest(LogLevel.Information, "something happened"));
            yield return Msg(MessageType.Request, MessageMethod.Log,
                new LogRequest(LogLevel.Error, "something failed"));
            yield return Msg(MessageType.Response, MessageMethod.Log,
                new LogResponse(MessageResponseCode.Success));

            // SetLogLevel
            yield return Msg(MessageType.Request, MessageMethod.SetLogLevel,
                new SetLogLevelRequest(LogLevel.Debug));
            yield return Msg(MessageType.Request, MessageMethod.SetLogLevel,
                new SetLogLevelRequest(LogLevel.Minimal));
            yield return Msg(MessageType.Response, MessageMethod.SetLogLevel,
                new SetLogLevelResponse(MessageResponseCode.Success));

            // SetCredentials — all optional credential fields populated
            yield return Msg(MessageType.Request, MessageMethod.SetCredentials,
                new SetCredentialsRequest("https://contoso.test", "proxyUser", "proxyPass", "user", "pass"));
            // SetCredentials — optional fields null
            yield return Msg(MessageType.Request, MessageMethod.SetCredentials,
                new SetCredentialsRequest("https://contoso.test", null, null, null, null));
            yield return Msg(MessageType.Response, MessageMethod.SetCredentials,
                new SetCredentialsResponse(MessageResponseCode.Success));

            // MonitorNuGetProcessExit
            yield return Msg(MessageType.Request, MessageMethod.MonitorNuGetProcessExit,
                new MonitorNuGetProcessExitRequest(1234));
            yield return Msg(MessageType.Response, MessageMethod.MonitorNuGetProcessExit,
                new MonitorNuGetProcessExitResponse(MessageResponseCode.Success));

            // CopyFilesInPackage
            yield return Msg(MessageType.Request, MessageMethod.CopyFilesInPackage,
                new CopyFilesInPackageRequest("https://source.contoso.test", "pkg", "1.0.0", new[] { "lib/net472/pkg.dll", "lib/netstandard2.0/pkg.dll" }, "C:\\dest"));
            yield return Msg(MessageType.Response, MessageMethod.CopyFilesInPackage,
                new CopyFilesInPackageResponse(MessageResponseCode.Success, new[] { "lib/net472/pkg.dll" }));
            yield return Msg(MessageType.Response, MessageMethod.CopyFilesInPackage,
                new CopyFilesInPackageResponse(MessageResponseCode.Error, Array.Empty<string>()));

            // CopyNupkgFile
            yield return Msg(MessageType.Request, MessageMethod.CopyNupkgFile,
                new CopyNupkgFileRequest("https://source.contoso.test", "pkg", "1.0.0", "C:\\dest\\pkg.nupkg"));
            yield return Msg(MessageType.Response, MessageMethod.CopyNupkgFile,
                new CopyNupkgFileResponse(MessageResponseCode.Success));

            // GetFilesInPackage
            yield return Msg(MessageType.Request, MessageMethod.GetFilesInPackage,
                new GetFilesInPackageRequest("https://source.contoso.test", "pkg", "1.0.0"));
            yield return Msg(MessageType.Response, MessageMethod.GetFilesInPackage,
                new GetFilesInPackageResponse(MessageResponseCode.Success, new[] { "lib/net472/pkg.dll" }));

            // GetPackageHash
            yield return Msg(MessageType.Request, MessageMethod.GetPackageHash,
                new GetPackageHashRequest("https://source.contoso.test", "pkg", "1.0.0", "SHA512"));
            yield return Msg(MessageType.Response, MessageMethod.GetPackageHash,
                new GetPackageHashResponse(MessageResponseCode.Success, "abc123=="));

            // GetPackageVersions
            yield return Msg(MessageType.Request, MessageMethod.GetPackageVersions,
                new GetPackageVersionsRequest("https://source.contoso.test", "pkg"));
            yield return Msg(MessageType.Response, MessageMethod.GetPackageVersions,
                new GetPackageVersionsResponse(MessageResponseCode.Success, new[] { "1.0.0", "2.0.0", "3.0.0-beta" }));

            // PrefetchPackage
            yield return Msg(MessageType.Request, MessageMethod.PrefetchPackage,
                new PrefetchPackageRequest("https://source.contoso.test", "pkg", "1.0.0"));
            yield return Msg(MessageType.Response, MessageMethod.PrefetchPackage,
                new PrefetchPackageResponse(MessageResponseCode.Success));

            // Null payload — no Payload field emitted
            yield return new object[] { MessageUtilities.Create("test-id", MessageType.Cancel, MessageMethod.None) };
        }

        public static IEnumerable<object[]> MalformedJsonCases()
        {
            yield return new object[] { "{\"Type\":\"Request\",\"Method\":\"Handshake\",\"Payload\":{\"ProtocolVersion\":\"1.0.0\",\"MinimumProtocolVersion\":\"1.0.0\"}}", false, typeof(ArgumentException) };
            yield return new object[] { "{\"Type\":\"Request\",\"Method\":\"Handshake\",\"Payload\":{\"ProtocolVersion\":\"1.0.0\",\"MinimumProtocolVersion\":\"1.0.0\"}}", true, typeof(System.Text.Json.JsonException) };
            yield return new object[] { "{\"RequestId\":\"id\",\"Method\":\"Handshake\"}", false, typeof(Newtonsoft.Json.JsonSerializationException) };
            yield return new object[] { "{\"RequestId\":\"id\",\"Method\":\"Handshake\"}", true, typeof(System.Text.Json.JsonException) };
            yield return new object[] { "{\"RequestId\":\"id\",\"Type\":\"NotAType\",\"Method\":\"Handshake\"}", false, typeof(Newtonsoft.Json.JsonSerializationException) };
            yield return new object[] { "{\"RequestId\":\"id\",\"Type\":\"NotAType\",\"Method\":\"Handshake\"}", true, typeof(System.Text.Json.JsonException) };
            yield return new object[] { "{\"RequestId\":\"id\",\"Type\":\"Request\"}", false, typeof(Newtonsoft.Json.JsonSerializationException) };
            yield return new object[] { "{\"RequestId\":\"id\",\"Type\":\"Request\"}", true, typeof(System.Text.Json.JsonException) };
            yield return new object[] { "{\"RequestId\":\"id\",\"Type\":\"Request\",\"Method\":\"NotAMethod\"}", false, typeof(Newtonsoft.Json.JsonSerializationException) };
            yield return new object[] { "{\"RequestId\":\"id\",\"Type\":\"Request\",\"Method\":\"NotAMethod\"}", true, typeof(System.Text.Json.JsonException) };
        }

        [Theory]
        [MemberData(nameof(AllMessages))]
        public void Serialize_AnyMessage_NsjAndStjProduceSameJson(Message message)
        {
            // Act
            var nsjJson = SerializeWithNsj(message);
            var stjJson = SerializeWithStj(message);

            // Assert
            Assert.Equal(nsjJson, stjJson);
        }

        [Theory]
        [MemberData(nameof(AllMessages))]
        public void Roundtrip_WithStj_ProducesIdenticalJson(Message message)
        {
            // Act
            var json = SerializeWithStj(message);
            var deserialized = DeserializeWithStj(json);
            var json2 = SerializeWithStj(deserialized);

            // Assert
            Assert.Equal(json, json2);
        }

        [Theory]
        [MemberData(nameof(AllMessages))]
        public void Roundtrip_WithNsj_ProducesIdenticalJson(Message message)
        {
            // Act
            var json = SerializeWithNsj(message);
            var deserialized = DeserializeWithNsj(json);
            var json2 = SerializeWithNsj(deserialized);

            // Assert
            Assert.Equal(json, json2);
        }

        [Theory]
        [MemberData(nameof(AllMessages))]
        public void Serialize_WithStj_CanBeRoundtrippedByNsj(Message message)
        {
            // Act
            var json = SerializeWithStj(message);
            var deserialized = DeserializeWithNsj(json);
            var json2 = SerializeWithNsj(deserialized);

            // Assert
            Assert.Equal(json, json2);
        }

        [Theory]
        [MemberData(nameof(AllMessages))]
        public void Serialize_WithNsj_CanBeRoundtrippedByStj(Message message)
        {
            // Act
            var json = SerializeWithNsj(message);
            var deserialized = DeserializeWithStj(json);
            var json2 = SerializeWithStj(deserialized);

            // Assert
            Assert.Equal(json, json2);
        }

        [Theory]
        [MemberData(nameof(MalformedJsonCases))]
        public void Deserialize_MalformedJson_Throws(string json, bool useStj, Type expectedExceptionType)
        {
            // Act & Assert
            Assert.Throws(expectedExceptionType, () => Deserialize(json, useStj));
        }

        [Fact]
        public void Serialize_UnsupportedPayloadType_Throws()
        {
            // Arrange
            var message = MessageUtilities.Create("id", MessageType.Request, MessageMethod.Handshake, new UnknownPayload());

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => SerializeWithStj(message));
        }

        private sealed class UnknownPayload { }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_HandshakeRequest_ReturnsExpectedProperties(bool useStj)
        {
            // Arrange
            var message = MessageUtilities.Create("id", MessageType.Request, MessageMethod.Handshake, new HandshakeRequest(V200, V100));

            // Act
            var payload = RoundtripPayload<HandshakeRequest>(message, useStj);

            // Assert
            Assert.Equal(V200, payload.ProtocolVersion);
            Assert.Equal(V100, payload.MinimumProtocolVersion);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_HandshakeResponse_ReturnsExpectedProperties(bool useStj)
        {
            // Arrange
            var message = MessageUtilities.Create("id", MessageType.Response, MessageMethod.Handshake, new HandshakeResponse(MessageResponseCode.Success, V100));

            // Act
            var payload = RoundtripPayload<HandshakeResponse>(message, useStj);

            // Assert
            Assert.Equal(MessageResponseCode.Success, payload.ResponseCode);
            Assert.Equal(V100, payload.ProtocolVersion);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_InitializeRequest_ReturnsExpectedProperties(bool useStj)
        {
            // Arrange
            var timeout = TimeSpan.FromSeconds(30);
            var message = MessageUtilities.Create("id", MessageType.Request, MessageMethod.Initialize,
                new InitializeRequest("1.0.0", "en-US", timeout));

            // Act
            var payload = RoundtripPayload<InitializeRequest>(message, useStj);

            // Assert
            Assert.Equal("1.0.0", payload.ClientVersion);
            Assert.Equal("en-US", payload.Culture);
            Assert.Equal(timeout, payload.RequestTimeout);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_InitializeResponse_ReturnsExpectedProperties(bool useStj)
        {
            // Arrange
            var message = MessageUtilities.Create("id", MessageType.Response, MessageMethod.Initialize, new InitializeResponse(MessageResponseCode.Success));

            // Act
            var payload = RoundtripPayload<InitializeResponse>(message, useStj);

            // Assert
            Assert.Equal(MessageResponseCode.Success, payload.ResponseCode);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_Fault_ReturnsExpectedProperties(bool useStj)
        {
            // Arrange
            var message = MessageUtilities.Create("id", MessageType.Fault, MessageMethod.None, new Fault("oops"));

            // Act
            var payload = RoundtripPayload<Fault>(message, useStj);

            // Assert
            Assert.Equal("oops", payload.Message);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_ProgressWithPercentage_ReturnsExpectedProperties(bool useStj)
        {
            // Arrange
            var message = MessageUtilities.Create("id", MessageType.Progress, MessageMethod.None, new Progress(0.75));

            // Act
            var payload = RoundtripPayload<Progress>(message, useStj);

            // Assert
            Assert.Equal(0.75, payload.Percentage);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_ProgressWithNullPercentage_ReturnsNullPercentage(bool useStj)
        {
            // Arrange
            var message = MessageUtilities.Create("id", MessageType.Progress, MessageMethod.None, new Progress());

            // Act
            var payload = RoundtripPayload<Progress>(message, useStj);

            // Assert
            Assert.Null(payload.Percentage);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_GetOperationClaimsRequestWithNullFields_ReturnsNullProperties(bool useStj)
        {
            // Arrange
            var message = MessageUtilities.Create("id", MessageType.Request, MessageMethod.GetOperationClaims,
                new GetOperationClaimsRequest((string?)null, (string?)null));

            // Act
            var payload = RoundtripPayload<GetOperationClaimsRequest>(message, useStj);

            // Assert
            Assert.Null(payload.PackageSourceRepository);
            Assert.Null(payload.ServiceIndexJson);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_GetOperationClaimsRequest_ReturnsExpectedProperties(bool useStj)
        {
            // Arrange
            var message = MessageUtilities.Create("id", MessageType.Request, MessageMethod.GetOperationClaims,
                new GetOperationClaimsRequest("https://contoso.test", ServiceIndexJson));

            // Act
            var payload = RoundtripPayload<GetOperationClaimsRequest>(message, useStj);

            // Assert
            Assert.Equal("https://contoso.test", payload.PackageSourceRepository);
            Assert.Equal(ServiceIndexJson, payload.ServiceIndexJson);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_GetOperationClaimsResponse_ReturnsExpectedProperties(bool useStj)
        {
            // Arrange
            var message = MessageUtilities.Create("id", MessageType.Response, MessageMethod.GetOperationClaims,
                new GetOperationClaimsResponse(new[] { OperationClaim.DownloadPackage, OperationClaim.Authentication }));

            // Act
            var payload = RoundtripPayload<GetOperationClaimsResponse>(message, useStj);

            // Assert
            Assert.Equal(2, payload.Claims.Count);
            Assert.Contains(OperationClaim.DownloadPackage, payload.Claims);
            Assert.Contains(OperationClaim.Authentication, payload.Claims);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_GetServiceIndexRequest_ReturnsExpectedProperties(bool useStj)
        {
            // Arrange
            var message = MessageUtilities.Create("id", MessageType.Request, MessageMethod.GetServiceIndex,
                new GetServiceIndexRequest("https://contoso.test"));

            // Act
            var payload = RoundtripPayload<GetServiceIndexRequest>(message, useStj);

            // Assert
            Assert.Equal("https://contoso.test", payload.PackageSourceRepository);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_GetServiceIndexResponse_RawJsonPreserved(bool useStj)
        {
            // Arrange
            var message = MessageUtilities.Create("id", MessageType.Response, MessageMethod.GetServiceIndex,
                new GetServiceIndexResponse(MessageResponseCode.Success, ServiceIndexJson));

            // Act
            var payload = RoundtripPayload<GetServiceIndexResponse>(message, useStj);

            // Assert
            Assert.Equal(ServiceIndexJson, payload.ServiceIndexJson);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_GetAuthenticationCredentialsRequest_ReturnsExpectedProperties(bool useStj)
        {
            // Arrange
            var uri = new Uri("https://contoso.test");
            var message = MessageUtilities.Create("id", MessageType.Request, MessageMethod.GetAuthenticationCredentials,
                new GetAuthenticationCredentialsRequest(uri, isRetry: true, isNonInteractive: true, canShowDialog: false));

            // Act
            var payload = RoundtripPayload<GetAuthenticationCredentialsRequest>(message, useStj);

            // Assert
            Assert.Equal(uri, payload.Uri);
            Assert.True(payload.IsRetry);
            Assert.True(payload.IsNonInteractive);
            Assert.False(payload.CanShowDialog);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_GetAuthenticationCredentialsResponse_ReturnsExpectedProperties(bool useStj)
        {
            // Arrange
            var message = MessageUtilities.Create("id", MessageType.Response, MessageMethod.GetAuthenticationCredentials,
                new GetAuthenticationCredentialsResponse("user", "pass", "ok", new[] { "basic" }, MessageResponseCode.Success));

            // Act
            var payload = RoundtripPayload<GetAuthenticationCredentialsResponse>(message, useStj);

            // Assert
            Assert.Equal("user", payload.Username);
            Assert.Equal("pass", payload.Password);
            Assert.Equal("ok", payload.Message);
            Assert.Equal(new[] { "basic" }, payload.AuthenticationTypes);
            Assert.Equal(MessageResponseCode.Success, payload.ResponseCode);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_GetCredentialsRequest_ReturnsExpectedProperties(bool useStj)
        {
            // Arrange
            var message = MessageUtilities.Create("id", MessageType.Request, MessageMethod.GetCredentials,
                new GetCredentialsRequest("https://contoso.test", HttpStatusCode.Unauthorized));

            // Act
            var payload = RoundtripPayload<GetCredentialsRequest>(message, useStj);

            // Assert
            Assert.Equal("https://contoso.test", payload.PackageSourceRepository);
            Assert.Equal(HttpStatusCode.Unauthorized, payload.StatusCode);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_GetCredentialsResponseWithNullAuthTypes_ReturnsNullAuthTypes(bool useStj)
        {
            // Arrange
            var message = MessageUtilities.Create("id", MessageType.Response, MessageMethod.GetCredentials,
                new GetCredentialsResponse(MessageResponseCode.Success, "user", "pass", null));

            // Act
            var payload = RoundtripPayload<GetCredentialsResponse>(message, useStj);

            // Assert
            Assert.Equal("user", payload.Username);
            Assert.Equal("pass", payload.Password);
            Assert.Null(payload.AuthenticationTypes);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_LogRequest_ReturnsExpectedProperties(bool useStj)
        {
            // Arrange
            var message = MessageUtilities.Create("id", MessageType.Request, MessageMethod.Log,
                new LogRequest(LogLevel.Warning, "watch out"));

            // Act
            var payload = RoundtripPayload<LogRequest>(message, useStj);

            // Assert
            Assert.Equal(LogLevel.Warning, payload.LogLevel);
            Assert.Equal("watch out", payload.Message);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_LogResponse_ReturnsExpectedProperties(bool useStj)
        {
            // Arrange
            var message = MessageUtilities.Create("id", MessageType.Response, MessageMethod.Log, new LogResponse(MessageResponseCode.Success));

            // Act
            var payload = RoundtripPayload<LogResponse>(message, useStj);

            // Assert
            Assert.Equal(MessageResponseCode.Success, payload.ResponseCode);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_SetLogLevelRequest_ReturnsExpectedProperties(bool useStj)
        {
            // Arrange
            var message = MessageUtilities.Create("id", MessageType.Request, MessageMethod.SetLogLevel,
                new SetLogLevelRequest(LogLevel.Verbose));

            // Act
            var payload = RoundtripPayload<SetLogLevelRequest>(message, useStj);

            // Assert
            Assert.Equal(LogLevel.Verbose, payload.LogLevel);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_SetLogLevelResponse_ReturnsExpectedProperties(bool useStj)
        {
            // Arrange
            var message = MessageUtilities.Create("id", MessageType.Response, MessageMethod.SetLogLevel, new SetLogLevelResponse(MessageResponseCode.Success));

            // Act
            var payload = RoundtripPayload<SetLogLevelResponse>(message, useStj);

            // Assert
            Assert.Equal(MessageResponseCode.Success, payload.ResponseCode);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_SetCredentialsRequestWithCredentials_ReturnsExpectedProperties(bool useStj)
        {
            // Arrange
            var message = MessageUtilities.Create("id", MessageType.Request, MessageMethod.SetCredentials,
                new SetCredentialsRequest("https://contoso.test", "pUser", "pPass", "user", "pass"));

            // Act
            var payload = RoundtripPayload<SetCredentialsRequest>(message, useStj);

            // Assert
            Assert.Equal("https://contoso.test", payload.PackageSourceRepository);
            Assert.Equal("pUser", payload.ProxyUsername);
            Assert.Equal("pPass", payload.ProxyPassword);
            Assert.Equal("user", payload.Username);
            Assert.Equal("pass", payload.Password);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_SetCredentialsRequestWithNullFields_ReturnsNullProperties(bool useStj)
        {
            // Arrange
            var message = MessageUtilities.Create("id", MessageType.Request, MessageMethod.SetCredentials,
                new SetCredentialsRequest("https://contoso.test", null, null, null, null));

            // Act
            var payload = RoundtripPayload<SetCredentialsRequest>(message, useStj);

            // Assert
            Assert.Equal("https://contoso.test", payload.PackageSourceRepository);
            Assert.Null(payload.ProxyUsername);
            Assert.Null(payload.ProxyPassword);
            Assert.Null(payload.Username);
            Assert.Null(payload.Password);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_SetCredentialsResponse_ReturnsExpectedProperties(bool useStj)
        {
            // Arrange
            var message = MessageUtilities.Create("id", MessageType.Response, MessageMethod.SetCredentials, new SetCredentialsResponse(MessageResponseCode.Success));

            // Act
            var payload = RoundtripPayload<SetCredentialsResponse>(message, useStj);

            // Assert
            Assert.Equal(MessageResponseCode.Success, payload.ResponseCode);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_MonitorNuGetProcessExitRequest_ReturnsExpectedProperties(bool useStj)
        {
            // Arrange
            var message = MessageUtilities.Create("id", MessageType.Request, MessageMethod.MonitorNuGetProcessExit,
                new MonitorNuGetProcessExitRequest(9999));

            // Act
            var payload = RoundtripPayload<MonitorNuGetProcessExitRequest>(message, useStj);

            // Assert
            Assert.Equal(9999, payload.ProcessId);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_MonitorNuGetProcessExitResponse_ReturnsExpectedProperties(bool useStj)
        {
            // Arrange
            var message = MessageUtilities.Create("id", MessageType.Response, MessageMethod.MonitorNuGetProcessExit, new MonitorNuGetProcessExitResponse(MessageResponseCode.Success));

            // Act
            var payload = RoundtripPayload<MonitorNuGetProcessExitResponse>(message, useStj);

            // Assert
            Assert.Equal(MessageResponseCode.Success, payload.ResponseCode);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_CopyFilesInPackageRequest_ReturnsExpectedProperties(bool useStj)
        {
            // Arrange
            var files = new[] { "lib/net472/pkg.dll", "lib/netstandard2.0/pkg.dll" };
            var message = MessageUtilities.Create("id", MessageType.Request, MessageMethod.CopyFilesInPackage,
                new CopyFilesInPackageRequest("https://source.contoso.test", "pkg", "1.0.0", files, "C:\\dest"));

            // Act
            var payload = RoundtripPayload<CopyFilesInPackageRequest>(message, useStj);

            // Assert
            Assert.Equal("https://source.contoso.test", payload.PackageSourceRepository);
            Assert.Equal("pkg", payload.PackageId);
            Assert.Equal("1.0.0", payload.PackageVersion);
            Assert.Equal(files, payload.FilesInPackage);
            Assert.Equal("C:\\dest", payload.DestinationFolderPath);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_CopyFilesInPackageResponse_ReturnsExpectedProperties(bool useStj)
        {
            // Arrange
            var copied = new[] { "lib/net472/pkg.dll" };
            var message = MessageUtilities.Create("id", MessageType.Response, MessageMethod.CopyFilesInPackage,
                new CopyFilesInPackageResponse(MessageResponseCode.Success, copied));

            // Act
            var payload = RoundtripPayload<CopyFilesInPackageResponse>(message, useStj);

            // Assert
            Assert.Equal(MessageResponseCode.Success, payload.ResponseCode);
            Assert.Equal(copied, payload.CopiedFiles);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_CopyNupkgFileRequest_ReturnsExpectedProperties(bool useStj)
        {
            // Arrange
            var message = MessageUtilities.Create("id", MessageType.Request, MessageMethod.CopyNupkgFile,
                new CopyNupkgFileRequest("https://source.contoso.test", "pkg", "1.0.0", "C:\\dest\\pkg.nupkg"));

            // Act
            var payload = RoundtripPayload<CopyNupkgFileRequest>(message, useStj);

            // Assert
            Assert.Equal("https://source.contoso.test", payload.PackageSourceRepository);
            Assert.Equal("pkg", payload.PackageId);
            Assert.Equal("1.0.0", payload.PackageVersion);
            Assert.Equal("C:\\dest\\pkg.nupkg", payload.DestinationFilePath);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_CopyNupkgFileResponse_ReturnsExpectedProperties(bool useStj)
        {
            // Arrange
            var message = MessageUtilities.Create("id", MessageType.Response, MessageMethod.CopyNupkgFile, new CopyNupkgFileResponse(MessageResponseCode.Success));

            // Act
            var payload = RoundtripPayload<CopyNupkgFileResponse>(message, useStj);

            // Assert
            Assert.Equal(MessageResponseCode.Success, payload.ResponseCode);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_GetFilesInPackageRequest_ReturnsExpectedProperties(bool useStj)
        {
            // Arrange
            var message = MessageUtilities.Create("id", MessageType.Request, MessageMethod.GetFilesInPackage,
                new GetFilesInPackageRequest("https://source.contoso.test", "pkg", "1.0.0"));

            // Act
            var payload = RoundtripPayload<GetFilesInPackageRequest>(message, useStj);

            // Assert
            Assert.Equal("https://source.contoso.test", payload.PackageSourceRepository);
            Assert.Equal("pkg", payload.PackageId);
            Assert.Equal("1.0.0", payload.PackageVersion);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_GetFilesInPackageResponse_ReturnsExpectedProperties(bool useStj)
        {
            // Arrange
            var files = new[] { "lib/net472/pkg.dll" };
            var message = MessageUtilities.Create("id", MessageType.Response, MessageMethod.GetFilesInPackage,
                new GetFilesInPackageResponse(MessageResponseCode.Success, files));

            // Act
            var payload = RoundtripPayload<GetFilesInPackageResponse>(message, useStj);

            // Assert
            Assert.Equal(MessageResponseCode.Success, payload.ResponseCode);
            Assert.Equal(files, payload.Files);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_GetPackageHashRequest_ReturnsExpectedProperties(bool useStj)
        {
            // Arrange
            var message = MessageUtilities.Create("id", MessageType.Request, MessageMethod.GetPackageHash,
                new GetPackageHashRequest("https://source.contoso.test", "pkg", "1.0.0", "SHA512"));

            // Act
            var payload = RoundtripPayload<GetPackageHashRequest>(message, useStj);

            // Assert
            Assert.Equal("https://source.contoso.test", payload.PackageSourceRepository);
            Assert.Equal("pkg", payload.PackageId);
            Assert.Equal("1.0.0", payload.PackageVersion);
            Assert.Equal("SHA512", payload.HashAlgorithm);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_GetPackageHashResponse_ReturnsExpectedProperties(bool useStj)
        {
            // Arrange
            var message = MessageUtilities.Create("id", MessageType.Response, MessageMethod.GetPackageHash,
                new GetPackageHashResponse(MessageResponseCode.Success, "abc123=="));

            // Act
            var payload = RoundtripPayload<GetPackageHashResponse>(message, useStj);

            // Assert
            Assert.Equal(MessageResponseCode.Success, payload.ResponseCode);
            Assert.Equal("abc123==", payload.Hash);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_GetPackageVersionsRequest_ReturnsExpectedProperties(bool useStj)
        {
            // Arrange
            var message = MessageUtilities.Create("id", MessageType.Request, MessageMethod.GetPackageVersions,
                new GetPackageVersionsRequest("https://source.contoso.test", "pkg"));

            // Act
            var payload = RoundtripPayload<GetPackageVersionsRequest>(message, useStj);

            // Assert
            Assert.Equal("https://source.contoso.test", payload.PackageSourceRepository);
            Assert.Equal("pkg", payload.PackageId);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_GetPackageVersionsResponse_ReturnsExpectedProperties(bool useStj)
        {
            // Arrange
            var versions = new[] { "1.0.0", "2.0.0" };
            var message = MessageUtilities.Create("id", MessageType.Response, MessageMethod.GetPackageVersions,
                new GetPackageVersionsResponse(MessageResponseCode.Success, versions));

            // Act
            var payload = RoundtripPayload<GetPackageVersionsResponse>(message, useStj);

            // Assert
            Assert.Equal(MessageResponseCode.Success, payload.ResponseCode);
            Assert.Equal(versions, payload.Versions);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_PrefetchPackageRequest_ReturnsExpectedProperties(bool useStj)
        {
            // Arrange
            var message = MessageUtilities.Create("id", MessageType.Request, MessageMethod.PrefetchPackage,
                new PrefetchPackageRequest("https://source.contoso.test", "pkg", "1.0.0"));

            // Act
            var payload = RoundtripPayload<PrefetchPackageRequest>(message, useStj);

            // Assert
            Assert.Equal("https://source.contoso.test", payload.PackageSourceRepository);
            Assert.Equal("pkg", payload.PackageId);
            Assert.Equal("1.0.0", payload.PackageVersion);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_PrefetchPackageResponse_ReturnsExpectedProperties(bool useStj)
        {
            // Arrange
            var message = MessageUtilities.Create("id", MessageType.Response, MessageMethod.PrefetchPackage, new PrefetchPackageResponse(MessageResponseCode.Success));

            // Act
            var payload = RoundtripPayload<PrefetchPackageResponse>(message, useStj);

            // Assert
            Assert.Equal(MessageResponseCode.Success, payload.ResponseCode);
        }

        private static string Serialize(Message message, bool useStj) =>
            useStj ? SerializeWithStj(message) : SerializeWithNsj(message);

        private static Message Deserialize(string json, bool useStj) =>
            useStj ? DeserializeWithStj(json) : DeserializeWithNsj(json);

        private static T GetPayload<T>(Message message) where T : class =>
            MessageUtilities.DeserializePayload<T>(message)!;

        private static T RoundtripPayload<T>(Message message, bool useStj) where T : class
        {
            var json = Serialize(message, useStj);
            var deserialized = Deserialize(json, useStj);
            return GetPayload<T>(deserialized);
        }
    }
}
