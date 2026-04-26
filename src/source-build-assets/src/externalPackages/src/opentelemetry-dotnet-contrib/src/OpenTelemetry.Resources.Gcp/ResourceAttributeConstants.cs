// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Resources.Gcp;

internal sealed class ResourceAttributeConstants
{
    // GCP resource attributes constant values
    internal const string GcpCloudProviderValue = "gcp";
    internal const string GcpGcePlatformValue = "gcp_compute_engine";
    internal const string GcpGaePlatformValue = "gcp_app_engine";
    internal const string GcpCloudRunPlatformValue = "gcp_cloud_run";
    internal const string GcpGkePlatformValue = "gcp_kubernetes_engine";
}
