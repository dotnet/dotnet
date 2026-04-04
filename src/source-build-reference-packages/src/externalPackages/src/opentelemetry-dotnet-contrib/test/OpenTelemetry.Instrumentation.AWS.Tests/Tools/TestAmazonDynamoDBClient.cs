// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Runtime;

namespace OpenTelemetry.Instrumentation.AWS.Tests.Tools;

internal class TestAmazonDynamoDBClient : AmazonDynamoDBClient
{
    public TestAmazonDynamoDBClient(AWSCredentials credentials, RegionEndpoint region)
        : base(credentials, region)
    {
    }
}
