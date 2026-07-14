// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Instrumentation.AWS.Implementation;

internal class AWSServiceType
{
    internal const string DynamoDbService = "DynamoDB";
    internal const string SQSService = "SQS";
    internal const string SNSService = "SNS";
    internal const string S3Service = "S3";
    internal const string BedrockService = "Bedrock";
    internal const string BedrockAgentService = "Bedrock Agent";
    internal const string BedrockAgentRuntimeService = "Bedrock Agent Runtime";
    internal const string BedrockRuntimeService = "Bedrock Runtime";

    internal static bool IsDynamoDbService(string service)
        => string.Equals(DynamoDbService, service, StringComparison.OrdinalIgnoreCase);

    internal static bool IsSqsService(string service)
        => string.Equals(SQSService, service, StringComparison.OrdinalIgnoreCase);

    internal static bool IsSnsService(string service)
        => string.Equals(SNSService, service, StringComparison.OrdinalIgnoreCase);

    internal static bool IsBedrockService(string service)
        => string.Equals(BedrockService, service, StringComparison.OrdinalIgnoreCase);

    internal static bool IsBedrockAgentService(string service)
        => string.Equals(BedrockAgentService, service, StringComparison.OrdinalIgnoreCase);

    internal static bool IsBedrockAgentRuntimeService(string service)
        => string.Equals(BedrockAgentRuntimeService, service, StringComparison.OrdinalIgnoreCase);

    internal static bool IsBedrockRuntimeService(string service)
        => string.Equals(BedrockRuntimeService, service, StringComparison.OrdinalIgnoreCase);
}
