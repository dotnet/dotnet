// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Amazon.Runtime;
using OpenTelemetry.AWS;

namespace OpenTelemetry.Instrumentation.AWS.Implementation;

internal class AWSServiceHelper
{
    public AWSServiceHelper(AWSSemanticConventions semanticConventions)
    {
        this.ParameterAttributeMap =
            semanticConventions
                .ParameterMappingBuilder
                .AddAttributeAWSDynamoTableName("TableName")
                .AddAttributeAWSSNSTopicArn("TopicArn")
                .AddAttributeAWSSQSQueueUrl("QueueUrl")
                .AddAttributeGenAiModelId("ModelId")
                .AddAttributeAWSBedrockAgentId("AgentId")
                .AddAttributeAWSBedrockDataSourceId("DataSourceId")
                .AddAttributeAWSBedrockGuardrailId("GuardrailId")
                .AddAttributeAWSBedrockKnowledgeBaseId("KnowledgeBaseId")
                .AddAttributeAWSS3BucketName("BucketName")
                .AddAttributeAWSS3Key("Key")
                .AddAttributeMessageId("MessageId")
                .Build();

        this.ArrayValueAttributeNames = new(semanticConventions.GetArrayValueAttributeNames(), StringComparer.Ordinal);
    }

    internal static IReadOnlyDictionary<string, List<string>> ServiceRequestParameterMap { get; } = new Dictionary<string, List<string>>(7)
    {
        { AWSServiceType.DynamoDbService, ["TableName"] },
        { AWSServiceType.S3Service, ["BucketName", "Key"] },
        { AWSServiceType.SNSService, ["TopicArn"] },
        { AWSServiceType.SQSService, ["QueueUrl"] },
        { AWSServiceType.BedrockAgentService, ["AgentId", "KnowledgeBaseId", "DataSourceId"] },
        { AWSServiceType.BedrockAgentRuntimeService, ["AgentId", "KnowledgeBaseId"] },
        { AWSServiceType.BedrockRuntimeService, ["ModelId"] },
    };

    internal static IReadOnlyDictionary<string, List<string>> ServiceResponseParameterMap { get; } = new Dictionary<string, List<string>>(4)
    {
        { AWSServiceType.BedrockService, ["GuardrailId"] },
        { AWSServiceType.BedrockAgentService, ["AgentId", "DataSourceId"] },
        { AWSServiceType.SNSService, ["MessageId", "TopicArn"] },
        { AWSServiceType.SQSService, ["MessageId", "QueueUrl"] },
    };

    internal static IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> MessagingOperationTypeMap { get; } = new Dictionary<string, IReadOnlyDictionary<string, string>>(2)
    {
        // See https://github.com/open-telemetry/semantic-conventions/blob/v1.40.0/docs/registry/attributes/messaging.md#messaging-operation-type
        [AWSServiceType.SNSService] = new Dictionary<string, string>(2)
        {
            ["Publish"] = "send",
            ["PublishBatch"] = "send",
        },
        [AWSServiceType.SQSService] = new Dictionary<string, string>(3)
        {
            ["ReceiveMessage"] = "receive",
            ["SendMessage"] = "send",
            ["SendMessageBatch"] = "send",
        },
    };

    // for Bedrock Agent operations, we map each supported operation to one resource: Agent, DataSource, or KnowledgeBase
    internal static List<string> BedrockAgentAgentOps { get; } =
    [
        "CreateAgentActionGroup",
        "CreateAgentAlias",
        "DeleteAgentActionGroup",
        "DeleteAgentAlias",
        "DeleteAgent",
        "DeleteAgentVersion",
        "GetAgentActionGroup",
        "GetAgentAlias",
        "GetAgent",
        "GetAgentVersion",
        "ListAgentActionGroups",
        "ListAgentAliases",
        "ListAgentKnowledgeBases",
        "ListAgentVersions",
        "PrepareAgent",
        "UpdateAgentActionGroup",
        "UpdateAgentAlias",
        "UpdateAgent"
    ];

    internal static List<string> BedrockAgentKnowledgeBaseOps { get; } =
    [
        "AssociateAgentKnowledgeBase",
        "CreateDataSource",
        "DeleteKnowledgeBase",
        "DisassociateAgentKnowledgeBase",
        "GetAgentKnowledgeBase",
        "GetKnowledgeBase",
        "ListDataSources",
        "UpdateAgentKnowledgeBase"
    ];

    internal static List<string> BedrockAgentDataSourceOps { get; } =
    [
        "DeleteDataSource",
        "GetDataSource",
        "UpdateDataSource"
    ];

    internal static IReadOnlyDictionary<string, string> OperationNameToResourceMap { get; } = BuildOperationNameToResourceMap();

    internal IDictionary<string, string> ParameterAttributeMap { get; }

    internal HashSet<string> ArrayValueAttributeNames { get; }

    internal static string GetAWSServiceName(IRequestContext requestContext)
        => Utils.RemoveAmazonPrefixFromServiceName(requestContext.ServiceMetaData.ServiceId);

    internal static string GetAWSOperationName(IRequestContext requestContext)
    {
        var completeRequestName = requestContext.OriginalRequest.GetType().Name;
        var suffix = "Request";
        var operationName = Utils.RemoveSuffix(completeRequestName, suffix);
        return operationName;
    }

    private static Dictionary<string, string> BuildOperationNameToResourceMap()
    {
        var operationClassMap = new Dictionary<string, string>(StringComparer.Ordinal);

        foreach (var op in BedrockAgentKnowledgeBaseOps)
        {
            operationClassMap[op] = "KnowledgeBaseId";
        }

        foreach (var op in BedrockAgentDataSourceOps)
        {
            operationClassMap[op] = "DataSourceId";
        }

        foreach (var op in BedrockAgentAgentOps)
        {
            operationClassMap[op] = "AgentId";
        }

        return operationClassMap;
    }
}
