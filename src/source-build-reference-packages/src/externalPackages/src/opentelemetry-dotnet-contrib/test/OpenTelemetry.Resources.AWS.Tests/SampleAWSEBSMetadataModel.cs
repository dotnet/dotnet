// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Resources.AWS.Models;

namespace OpenTelemetry.Resources.AWS.Tests;

internal class SampleAWSEBSMetadataModel : AWSEBSMetadataModel
{
    public SampleAWSEBSMetadataModel()
    {
        this.EnvironmentName = "Test environment name";
        this.DeploymentId = "Test ID";
        this.VersionLabel = "Test version label";
    }
}
