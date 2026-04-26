// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Text.Json.Serialization;

namespace OpenTelemetry.Resources.AWS.Models;

internal class AWSEBSMetadataModel
{
    [JsonPropertyName("deployment_id")]
    public string? DeploymentId { get; set; }

    [JsonPropertyName("environment_name")]
    public string? EnvironmentName { get; set; }

    [JsonPropertyName("version_label")]
    public string? VersionLabel { get; set; }
}
