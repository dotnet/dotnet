// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Instrumentation.AWS.Implementation;

internal class AWSServiceType
{
    internal const string DynamoDbService = "DynamoDB";
    internal const string SQSService = "SQS";
    internal const string SNSService = "SNS";
    internal const string BedrockService = "Bedrock";
    internal const string BedrockAgentService = "Bedrock Agent";
    internal const string BedrockAgentRuntimeService = "Bedrock Agent Runtime";
    internal const string BedrockRuntimeService = "Bedrock Runtime";

    internal static bool IsDynamoDbService(string service)
        => DynamoDbService.Equals(service, StringComparison.OrdinalIgnoreCase);

    internal static bool IsSqsService(string service)
        => SQSService.Equals(service, StringComparison.OrdinalIgnoreCase);

    internal static bool IsSnsService(string service)
        => SNSService.Equals(service, StringComparison.OrdinalIgnoreCase);

    internal static bool IsBedrockService(string service)
        => BedrockService.Equals(service, StringComparison.OrdinalIgnoreCase);

    internal static bool IsBedrockAgentService(string service)
        => BedrockAgentService.Equals(service, StringComparison.OrdinalIgnoreCase);

    internal static bool IsBedrockAgentRuntimeService(string service)
        => BedrockAgentRuntimeService.Equals(service, StringComparison.OrdinalIgnoreCase);

    internal static bool IsBedrockRuntimeService(string service)
        => BedrockRuntimeService.Equals(service, StringComparison.OrdinalIgnoreCase);
}
