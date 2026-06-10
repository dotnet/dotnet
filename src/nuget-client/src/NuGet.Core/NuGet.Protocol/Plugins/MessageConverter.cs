// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NuGet.Protocol.Plugins
{
    internal sealed class MessageConverter : JsonConverter<Message>
    {
        private static readonly Dictionary<(MessageMethod, MessageType), Func<JsonElement, object?>> _read = new()
        {
            [(MessageMethod.Handshake, MessageType.Request)] = e => e.Deserialize(PluginJsonContext.Default.HandshakeRequest),
            [(MessageMethod.Handshake, MessageType.Response)] = e => e.Deserialize(PluginJsonContext.Default.HandshakeResponse),

            [(MessageMethod.Initialize, MessageType.Request)] = e => e.Deserialize(PluginJsonContext.Default.InitializeRequest),
            [(MessageMethod.Initialize, MessageType.Response)] = e => e.Deserialize(PluginJsonContext.Default.InitializeResponse),

            [(MessageMethod.GetOperationClaims, MessageType.Request)] = e => e.Deserialize(PluginJsonContext.Default.GetOperationClaimsRequest),
            [(MessageMethod.GetOperationClaims, MessageType.Response)] = e => e.Deserialize(PluginJsonContext.Default.GetOperationClaimsResponse),

            [(MessageMethod.GetServiceIndex, MessageType.Request)] = e => e.Deserialize(PluginJsonContext.Default.GetServiceIndexRequest),
            [(MessageMethod.GetServiceIndex, MessageType.Response)] = e => e.Deserialize(PluginJsonContext.Default.GetServiceIndexResponse),

            [(MessageMethod.GetAuthenticationCredentials, MessageType.Request)] = e => e.Deserialize(PluginJsonContext.Default.GetAuthenticationCredentialsRequest),
            [(MessageMethod.GetAuthenticationCredentials, MessageType.Response)] = e => e.Deserialize(PluginJsonContext.Default.GetAuthenticationCredentialsResponse),

            [(MessageMethod.GetCredentials, MessageType.Request)] = e => e.Deserialize(PluginJsonContext.Default.GetCredentialsRequest),
            [(MessageMethod.GetCredentials, MessageType.Response)] = e => e.Deserialize(PluginJsonContext.Default.GetCredentialsResponse),

            [(MessageMethod.Log, MessageType.Request)] = e => e.Deserialize(PluginJsonContext.Default.LogRequest),
            [(MessageMethod.Log, MessageType.Response)] = e => e.Deserialize(PluginJsonContext.Default.LogResponse),

            [(MessageMethod.SetLogLevel, MessageType.Request)] = e => e.Deserialize(PluginJsonContext.Default.SetLogLevelRequest),
            [(MessageMethod.SetLogLevel, MessageType.Response)] = e => e.Deserialize(PluginJsonContext.Default.SetLogLevelResponse),

            [(MessageMethod.SetCredentials, MessageType.Request)] = e => e.Deserialize(PluginJsonContext.Default.SetCredentialsRequest),
            [(MessageMethod.SetCredentials, MessageType.Response)] = e => e.Deserialize(PluginJsonContext.Default.SetCredentialsResponse),

            [(MessageMethod.MonitorNuGetProcessExit, MessageType.Request)] = e => e.Deserialize(PluginJsonContext.Default.MonitorNuGetProcessExitRequest),
            [(MessageMethod.MonitorNuGetProcessExit, MessageType.Response)] = e => e.Deserialize(PluginJsonContext.Default.MonitorNuGetProcessExitResponse),

            [(MessageMethod.CopyFilesInPackage, MessageType.Request)] = e => e.Deserialize(PluginJsonContext.Default.CopyFilesInPackageRequest),
            [(MessageMethod.CopyFilesInPackage, MessageType.Response)] = e => e.Deserialize(PluginJsonContext.Default.CopyFilesInPackageResponse),

            [(MessageMethod.CopyNupkgFile, MessageType.Request)] = e => e.Deserialize(PluginJsonContext.Default.CopyNupkgFileRequest),
            [(MessageMethod.CopyNupkgFile, MessageType.Response)] = e => e.Deserialize(PluginJsonContext.Default.CopyNupkgFileResponse),

            [(MessageMethod.GetFilesInPackage, MessageType.Request)] = e => e.Deserialize(PluginJsonContext.Default.GetFilesInPackageRequest),
            [(MessageMethod.GetFilesInPackage, MessageType.Response)] = e => e.Deserialize(PluginJsonContext.Default.GetFilesInPackageResponse),

            [(MessageMethod.GetPackageHash, MessageType.Request)] = e => e.Deserialize(PluginJsonContext.Default.GetPackageHashRequest),
            [(MessageMethod.GetPackageHash, MessageType.Response)] = e => e.Deserialize(PluginJsonContext.Default.GetPackageHashResponse),

            [(MessageMethod.GetPackageVersions, MessageType.Request)] = e => e.Deserialize(PluginJsonContext.Default.GetPackageVersionsRequest),
            [(MessageMethod.GetPackageVersions, MessageType.Response)] = e => e.Deserialize(PluginJsonContext.Default.GetPackageVersionsResponse),

            [(MessageMethod.PrefetchPackage, MessageType.Request)] = e => e.Deserialize(PluginJsonContext.Default.PrefetchPackageRequest),
            [(MessageMethod.PrefetchPackage, MessageType.Response)] = e => e.Deserialize(PluginJsonContext.Default.PrefetchPackageResponse),
        };

        private static readonly Dictionary<Type, Action<Utf8JsonWriter, object>> _write = new()
        {
            [typeof(HandshakeRequest)] = (w, p) => JsonSerializer.Serialize(w, (HandshakeRequest)p, PluginJsonContext.Default.HandshakeRequest),
            [typeof(HandshakeResponse)] = (w, p) => JsonSerializer.Serialize(w, (HandshakeResponse)p, PluginJsonContext.Default.HandshakeResponse),
            [typeof(InitializeRequest)] = (w, p) => JsonSerializer.Serialize(w, (InitializeRequest)p, PluginJsonContext.Default.InitializeRequest),
            [typeof(InitializeResponse)] = (w, p) => JsonSerializer.Serialize(w, (InitializeResponse)p, PluginJsonContext.Default.InitializeResponse),
            [typeof(GetOperationClaimsRequest)] = (w, p) => JsonSerializer.Serialize(w, (GetOperationClaimsRequest)p, PluginJsonContext.Default.GetOperationClaimsRequest),
            [typeof(GetOperationClaimsResponse)] = (w, p) => JsonSerializer.Serialize(w, (GetOperationClaimsResponse)p, PluginJsonContext.Default.GetOperationClaimsResponse),
            [typeof(GetServiceIndexRequest)] = (w, p) => JsonSerializer.Serialize(w, (GetServiceIndexRequest)p, PluginJsonContext.Default.GetServiceIndexRequest),
            [typeof(GetServiceIndexResponse)] = (w, p) => JsonSerializer.Serialize(w, (GetServiceIndexResponse)p, PluginJsonContext.Default.GetServiceIndexResponse),
            [typeof(GetAuthenticationCredentialsRequest)] = (w, p) => JsonSerializer.Serialize(w, (GetAuthenticationCredentialsRequest)p, PluginJsonContext.Default.GetAuthenticationCredentialsRequest),
            [typeof(GetAuthenticationCredentialsResponse)] = (w, p) => JsonSerializer.Serialize(w, (GetAuthenticationCredentialsResponse)p, PluginJsonContext.Default.GetAuthenticationCredentialsResponse),
            [typeof(GetCredentialsRequest)] = (w, p) => JsonSerializer.Serialize(w, (GetCredentialsRequest)p, PluginJsonContext.Default.GetCredentialsRequest),
            [typeof(GetCredentialsResponse)] = (w, p) => JsonSerializer.Serialize(w, (GetCredentialsResponse)p, PluginJsonContext.Default.GetCredentialsResponse),
            [typeof(LogRequest)] = (w, p) => JsonSerializer.Serialize(w, (LogRequest)p, PluginJsonContext.Default.LogRequest),
            [typeof(LogResponse)] = (w, p) => JsonSerializer.Serialize(w, (LogResponse)p, PluginJsonContext.Default.LogResponse),
            [typeof(SetLogLevelRequest)] = (w, p) => JsonSerializer.Serialize(w, (SetLogLevelRequest)p, PluginJsonContext.Default.SetLogLevelRequest),
            [typeof(SetLogLevelResponse)] = (w, p) => JsonSerializer.Serialize(w, (SetLogLevelResponse)p, PluginJsonContext.Default.SetLogLevelResponse),
            [typeof(SetCredentialsRequest)] = (w, p) => JsonSerializer.Serialize(w, (SetCredentialsRequest)p, PluginJsonContext.Default.SetCredentialsRequest),
            [typeof(SetCredentialsResponse)] = (w, p) => JsonSerializer.Serialize(w, (SetCredentialsResponse)p, PluginJsonContext.Default.SetCredentialsResponse),
            [typeof(MonitorNuGetProcessExitRequest)] = (w, p) => JsonSerializer.Serialize(w, (MonitorNuGetProcessExitRequest)p, PluginJsonContext.Default.MonitorNuGetProcessExitRequest),
            [typeof(MonitorNuGetProcessExitResponse)] = (w, p) => JsonSerializer.Serialize(w, (MonitorNuGetProcessExitResponse)p, PluginJsonContext.Default.MonitorNuGetProcessExitResponse),
            [typeof(CopyFilesInPackageRequest)] = (w, p) => JsonSerializer.Serialize(w, (CopyFilesInPackageRequest)p, PluginJsonContext.Default.CopyFilesInPackageRequest),
            [typeof(CopyFilesInPackageResponse)] = (w, p) => JsonSerializer.Serialize(w, (CopyFilesInPackageResponse)p, PluginJsonContext.Default.CopyFilesInPackageResponse),
            [typeof(CopyNupkgFileRequest)] = (w, p) => JsonSerializer.Serialize(w, (CopyNupkgFileRequest)p, PluginJsonContext.Default.CopyNupkgFileRequest),
            [typeof(CopyNupkgFileResponse)] = (w, p) => JsonSerializer.Serialize(w, (CopyNupkgFileResponse)p, PluginJsonContext.Default.CopyNupkgFileResponse),
            [typeof(GetFilesInPackageRequest)] = (w, p) => JsonSerializer.Serialize(w, (GetFilesInPackageRequest)p, PluginJsonContext.Default.GetFilesInPackageRequest),
            [typeof(GetFilesInPackageResponse)] = (w, p) => JsonSerializer.Serialize(w, (GetFilesInPackageResponse)p, PluginJsonContext.Default.GetFilesInPackageResponse),
            [typeof(GetPackageHashRequest)] = (w, p) => JsonSerializer.Serialize(w, (GetPackageHashRequest)p, PluginJsonContext.Default.GetPackageHashRequest),
            [typeof(GetPackageHashResponse)] = (w, p) => JsonSerializer.Serialize(w, (GetPackageHashResponse)p, PluginJsonContext.Default.GetPackageHashResponse),
            [typeof(GetPackageVersionsRequest)] = (w, p) => JsonSerializer.Serialize(w, (GetPackageVersionsRequest)p, PluginJsonContext.Default.GetPackageVersionsRequest),
            [typeof(GetPackageVersionsResponse)] = (w, p) => JsonSerializer.Serialize(w, (GetPackageVersionsResponse)p, PluginJsonContext.Default.GetPackageVersionsResponse),
            [typeof(PrefetchPackageRequest)] = (w, p) => JsonSerializer.Serialize(w, (PrefetchPackageRequest)p, PluginJsonContext.Default.PrefetchPackageRequest),
            [typeof(PrefetchPackageResponse)] = (w, p) => JsonSerializer.Serialize(w, (PrefetchPackageResponse)p, PluginJsonContext.Default.PrefetchPackageResponse),
            [typeof(Fault)] = (w, p) => JsonSerializer.Serialize(w, (Fault)p, PluginJsonContext.Default.Fault),
            [typeof(Progress)] = (w, p) => JsonSerializer.Serialize(w, (Progress)p, PluginJsonContext.Default.Progress),
        };

        public override Message Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;

            if (!root.TryGetProperty(nameof(Message.RequestId), out var requestIdProp))
            {
                throw new JsonException(string.Format(CultureInfo.CurrentCulture, Strings.Error_RequiredJsonPropertyMissing, nameof(Message.RequestId)));
            }

            var requestId = requestIdProp.GetString();
            if (string.IsNullOrEmpty(requestId))
            {
                throw new JsonException(string.Format(CultureInfo.CurrentCulture, Strings.ArgumentCannotBeNullOrEmpty, nameof(Message.RequestId)));
            }

            if (!root.TryGetProperty(nameof(Message.Type), out var typeProp))
            {
                throw new JsonException(string.Format(CultureInfo.CurrentCulture, Strings.Error_RequiredJsonPropertyMissing, nameof(Message.Type)));
            }

            var typeStr = typeProp.GetString();
            if (!Enum.TryParse<MessageType>(typeStr, out var messageType) || !Enum.IsDefined(typeof(MessageType), messageType))
            {
                throw new JsonException(string.Format(CultureInfo.CurrentCulture, Strings.Plugin_UnrecognizedEnumValue, typeStr));
            }

            if (!root.TryGetProperty(nameof(Message.Method), out var methodProp))
            {
                throw new JsonException(string.Format(CultureInfo.CurrentCulture, Strings.Error_RequiredJsonPropertyMissing, nameof(Message.Method)));
            }

            var methodStr = methodProp.GetString();
            if (!Enum.TryParse<MessageMethod>(methodStr, out var messageMethod) || !Enum.IsDefined(typeof(MessageMethod), messageMethod))
            {
                throw new JsonException(string.Format(CultureInfo.CurrentCulture, Strings.Plugin_UnrecognizedEnumValue, methodStr));
            }

            object? payload = null;
            if (root.TryGetProperty("Payload", out var payloadProp) && payloadProp.ValueKind != JsonValueKind.Null)
            {
                if (payloadProp.ValueKind != JsonValueKind.Object)
                {
                    throw new JsonException(string.Format(CultureInfo.CurrentCulture, Strings.Error_UnexpectedPayloadTokenType, payloadProp.ValueKind));
                }

                if (messageType is MessageType.Fault)
                {
                    payload = payloadProp.Deserialize(PluginJsonContext.Default.Fault);
                }
                else if (messageType is MessageType.Progress)
                {
                    payload = payloadProp.Deserialize(PluginJsonContext.Default.Progress);
                }
                else if (_read.TryGetValue((messageMethod, messageType), out var deserialize))
                {
                    payload = deserialize(payloadProp);
                }
                else
                {
                    throw new JsonException(string.Format(CultureInfo.CurrentCulture, Strings.Plugin_UnrecognizedEnumValue, $"{messageMethod}/{messageType}"));
                }
            }

            return new Message(requestId!, messageType, messageMethod, payload);
        }

        public override void Write(Utf8JsonWriter writer, Message value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString(nameof(Message.RequestId), value.RequestId);
            writer.WriteString(nameof(Message.Type), value.Type.ToString());
            writer.WriteString(nameof(Message.Method), value.Method.ToString());

            if (value.PayloadObject is not null)
            {
                if (!_write.TryGetValue(value.PayloadObject.GetType(), out var serialize))
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.Error_UnsupportedPayloadType, value.PayloadObject.GetType()));
                }

                writer.WritePropertyName("Payload");
                serialize(writer, value.PayloadObject);
            }
            writer.WriteEndObject();
        }
    }
}
