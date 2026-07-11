// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.AWS;

// disable Style Warnings to improve readability of this specific file.
#pragma warning disable SA1124
#pragma warning disable SA1005
#pragma warning disable SA1514
#pragma warning disable SA1201
#pragma warning disable SA1516

internal partial class AWSSemanticConventions
{
    /// <summary>
    /// Open Telemetry Semantic Conventions as of 1.40.0:
    /// https://github.com/open-telemetry/semantic-conventions/releases/tag/v1.40.0.
    /// </summary>
    private class AWSSemanticConventions_V1_40_0 : AWSSemanticConventions_V1_29_0
    {
        // CLOUD Attributes
        public override string AttributeCloudRegion => "cloud.region";

        // AWS Attributes
        public override string AttributeAWSExtendedRequestId => "aws.extended_request_id";
        public override string AttributeAWSSNS => "aws.sns";
        public override string AttributeAWSSNSTopicArn => "aws.sns.topic.arn";
        public override string AttributeAWSSQS => "aws_sqs";
        public override string AttributeAWSSQSQueueUrl => "aws.sqs.queue.url";

        // MESSAGING Attributes
        public override string AttributeMessagingDestinationName => "messaging.destination.name";
        public override string AttributeMessagingMessageId => "messaging.message.id";
        public override string AttributeMessagingOperationName => "messaging.operation.name";
        public override string AttributeMessagingOperationType => "messaging.operation.type";
        public override string AttributeMessagingSystem => "messaging.system";

        // RPC Attributes
        public override string AttributeRpcService => "rpc.method";
        public override string AttributeRpcSystemName => "rpc.system.name";
    }
}
