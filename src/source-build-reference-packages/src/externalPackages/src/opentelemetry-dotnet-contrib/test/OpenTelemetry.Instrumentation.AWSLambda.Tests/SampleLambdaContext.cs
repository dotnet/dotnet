// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Amazon.Lambda.Core;

namespace OpenTelemetry.Instrumentation.AWSLambda.Tests;

internal class SampleLambdaContext : ILambdaContext
{
    public string AwsRequestId { get; set; } = "testrequestid";

    public IClientContext? ClientContext { get; set; }

    public string FunctionName { get; set; } = "testfunction";

    public string FunctionVersion { get; set; } = "latest";

    public ICognitoIdentity? Identity { get; set; }

    public string InvokedFunctionArn { get; set; } = "arn:aws:lambda:us-east-1:111111111111:function:testfunction";

    public ILambdaLogger? Logger { get; set; }

    public string? LogGroupName { get; set; }

    public string? LogStreamName { get; set; } = "2025/07/21/[$LATEST]c53dd774881747b29e48cc58a46720e5";

    public int MemoryLimitInMB { get; set; } = 256;

    public TimeSpan RemainingTime { get; set; }
}
