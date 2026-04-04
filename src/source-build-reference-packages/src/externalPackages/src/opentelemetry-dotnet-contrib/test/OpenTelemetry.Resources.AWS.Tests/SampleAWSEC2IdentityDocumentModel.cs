// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Resources.AWS.Models;

namespace OpenTelemetry.Resources.AWS.Tests;

internal class SampleAWSEC2IdentityDocumentModel : AWSEC2IdentityDocumentModel
{
    public SampleAWSEC2IdentityDocumentModel()
    {
        this.AccountId = "Test account id";
        this.AvailabilityZone = "Test availability zone";
        this.Region = "Test aws region";
        this.InstanceId = "Test instance id";
        this.InstanceType = "Test instance type";
    }
}
