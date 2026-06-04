// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text.Json.Serialization;

namespace NuGet.Protocol.Plugins
{
    [JsonSourceGenerationOptions(
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        UseStringEnumConverter = true)]
    [JsonSerializable(typeof(Message))]
    [JsonSerializable(typeof(CopyFilesInPackageRequest))]
    [JsonSerializable(typeof(CopyFilesInPackageResponse))]
    [JsonSerializable(typeof(CopyNupkgFileRequest))]
    [JsonSerializable(typeof(CopyNupkgFileResponse))]
    [JsonSerializable(typeof(Fault))]
    [JsonSerializable(typeof(GetAuthenticationCredentialsRequest))]
    [JsonSerializable(typeof(GetAuthenticationCredentialsResponse))]
    [JsonSerializable(typeof(GetCredentialsRequest))]
    [JsonSerializable(typeof(GetCredentialsResponse))]
    [JsonSerializable(typeof(GetFilesInPackageRequest))]
    [JsonSerializable(typeof(GetFilesInPackageResponse))]
    [JsonSerializable(typeof(GetOperationClaimsRequest))]
    [JsonSerializable(typeof(GetOperationClaimsResponse))]
    [JsonSerializable(typeof(GetPackageHashRequest))]
    [JsonSerializable(typeof(GetPackageHashResponse))]
    [JsonSerializable(typeof(GetPackageVersionsRequest))]
    [JsonSerializable(typeof(GetPackageVersionsResponse))]
    [JsonSerializable(typeof(GetServiceIndexRequest))]
    [JsonSerializable(typeof(GetServiceIndexResponse))]
    [JsonSerializable(typeof(HandshakeRequest))]
    [JsonSerializable(typeof(HandshakeResponse))]
    [JsonSerializable(typeof(InitializeRequest))]
    [JsonSerializable(typeof(InitializeResponse))]
    [JsonSerializable(typeof(LogRequest))]
    [JsonSerializable(typeof(LogResponse))]
    [JsonSerializable(typeof(MonitorNuGetProcessExitRequest))]
    [JsonSerializable(typeof(MonitorNuGetProcessExitResponse))]
    [JsonSerializable(typeof(PrefetchPackageRequest))]
    [JsonSerializable(typeof(PrefetchPackageResponse))]
    [JsonSerializable(typeof(Progress))]
    [JsonSerializable(typeof(SetCredentialsRequest))]
    [JsonSerializable(typeof(SetCredentialsResponse))]
    [JsonSerializable(typeof(SetLogLevelRequest))]
    [JsonSerializable(typeof(SetLogLevelResponse))]
    internal partial class PluginJsonContext : JsonSerializerContext { }
}
