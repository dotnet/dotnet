// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.AWS;

// disable Style Warnings to improve readability of this specific file.
#pragma warning disable SA1124
#pragma warning disable SA1005
#pragma warning disable SA1514
#pragma warning disable SA1516
#pragma warning disable SA1201

internal partial class AWSSemanticConventions
{
    /// <summary>
    /// The Open Telemetry Semantic Conventions used by OpenTelemetry.*.AWS libraries as of
    /// version 1.10.0-beta and represents a mixture of Semantic Conventions as these libraries had
    /// not yet been upgraded to use a specific Semantic Convention version.
    /// </summary>
    /// <remarks>
    /// Future version specific convention classes will only need to define new or changed attributes.
    /// </remarks>
    private abstract class AWSSemanticConventionsLegacy : AWSSemanticConventionsBase
    {
        // CLOUD Attributes
        public override string AttributeCloudAccountID => "cloud.account.id";
        public override string AttributeCloudAvailabilityZone => "cloud.availability_zone";
        public override string AttributeCloudPlatform => "cloud.platform";
        public override string AttributeCloudProvider => "cloud.provider";
        public override string AttributeCloudRegion => "cloud.region";
        public override string AttributeCloudResourceId => "cloud.resource_id";
        public override string CloudPlatformValuesAwsEc2 => "aws_ec2";
        public override string CloudPlatformValuesAwsEcs => "aws_ecs";
        public override string CloudPlatformValuesAwsEks => "aws_eks";
        public override string CloudPlatformValuesAwsElasticBeanstalk => "aws_elastic_beanstalk";
        public override string CloudProviderValuesAws => "aws";

        // CONTAINER Attributes
        public override string AttributeContainerID => "container.id";

        // DB Attributes
        public override string AttributeDbSystem => "db.system";
        public override string AttributeDynamoDb => "dynamodb";

        // AWS Attributes
        public override string AttributeEcsContainerArn => "aws.ecs.container.arn";
        public override string AttributeEcsClusterArn => "aws.ecs.cluster.arn";
        public override string AttributeEcsLaunchtype => "aws.ecs.launchtype";
        public override string ValueEcsLaunchTypeEc2 => "ec2";
        public override string ValueEcsLaunchTypeFargate => "fargate";
        public override string AttributeEcsTaskArn => "aws.ecs.task.arn";
        public override string AttributeEcsTaskFamily => "aws.ecs.task.family";
        public override string AttributeEcsTaskRevision => "aws.ecs.task.revision";
        public override string AttributeLogGroupNames => "aws.log.group.names";
        public override string AttributeLogGroupArns => "aws.log.group.arns";
        public override string AttributeLogStreamNames => "aws.log.stream.arns";
        public override string AttributeLogStreamArns => "aws.log.stream.names";
        public override string AttributeAWSDynamoTableName => "aws.table_name";
        public override string AttributeAWSSQSQueueUrl => "aws.queue_url";
        public override string AttributeAWSBedrockAgentId => "aws.bedrock.agent.id";
        public override string AttributeAWSBedrockDataSourceId => "aws.bedrock.data_source.id";
        public override string AttributeAWSBedrockGuardrailId => "aws.bedrock.guardrail.id";
        public override string AttributeAWSBedrockKnowledgeBaseId => "aws.bedrock.knowledge_base.id";
        public override string AttributeAWSBedrock => "aws_bedrock";

        // FAAS Attributes
        public override string AttributeFaasID => "faas.id";
        public override string AttributeFaasExecution => "faas.execution";
        public override string AttributeFaasName => "faas.name";
        public override string AttributeFaasVersion => "faas.version";
        public override string AttributeFaasTrigger => "faas.trigger";
        public override string AttributeFaasColdStart => "faas.coldstart";
        public override string AttributeFaasInstance => "faas.instance";
        public override string AttributeFaasMaxMemory => "faas.max_memory";

        // GEN AI Attributes
        public override string AttributeGenAiModelId => "gen_ai.request.model";
        public override string AttributeGenAiSystem => "gen_ai.system";

        // HOST Attributes
        public override string AttributeHostID => "host.id";
        public override string AttributeHostType => "host.type";
        public override string AttributeHostName => "host.name";

        // HTTP Attributes
        [Obsolete("Replaced by <c>http.response.status_code</c>.")]
        public override string AttributeHttpStatusCode => "http.status_code";
        [Obsolete("Replaced by <c>url.scheme</c> instead.")]
        public override string AttributeHttpScheme => "http.scheme";
        [Obsolete("Split to <c>url.path</c> and `url.query.")]
        public override string AttributeHttpTarget => "http.target";
        [Obsolete("Replaced by <c>http.request.method</c>.")]
        public override string AttributeHttpMethod => "http.method";

        // NET Attributes
        [Obsolete("Replaced by <c>server.address</c>.")]
        public override string AttributeNetHostName => "net.host.name";
        [Obsolete("Replaced by <c>server.port</c>.")]
        public override string AttributeNetHostPort => "net.host.port";

        // K8s Attributes
        public override string AttributeK8SClusterName => "k8s.cluster.name";

        // SERVICE Attributes
        public override string AttributeServiceName => "service.name";
        public override string AttributeServiceNamespace => "service.namespace";
        public override string AttributeServiceInstanceID => "service.instance.id";
        public override string AttributeServiceVersion => "service.version";
        public override string ServiceNameValuesAwsElasticBeanstalk => "aws_elastic_beanstalk";
    }
}
