// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if INSTRUMENTATION_AWSLAMBDA
using OpenTelemetry.Instrumentation.AWSLambda;
#elif INSTRUMENTATION_AWS
using OpenTelemetry.Instrumentation.AWS;
#elif RESOURCES_AWS
using OpenTelemetry.Resources.AWS;
#pragma warning disable SA1629
#endif

namespace OpenTelemetry.AWS;

// disable Style Warnings to improve readability of this specific file.
#pragma warning disable SA1124
#pragma warning disable SA1005
#pragma warning disable SA1514
#pragma warning disable SA1201
#pragma warning disable SA1623
#pragma warning disable SA1629
#pragma warning disable CS1570 // XML comment has badly formed XML

internal partial class AWSSemanticConventions
{
    /// <summary>
    /// Defines all Semantic Conventions used by AWS extension projects.
    ///
    /// All values default to <c>string.Empty</c> and are then is only defined
    /// in the first version specific class (ie <see cref="AWSSemanticConventions_V1_28_0"/>)
    /// to use it.  This helps ensure the attribute doesn't get used if the user has specified
    /// a specific <see cref="SemanticConventionVersion"/>.
    ///
    /// See <see cref="GetSemanticConventionVersion"/> for details.
    /// </summary>
    private abstract class AWSSemanticConventionsBase
    {
        #region CLOUD Attributes
        /// <summary>
        /// The cloud account ID the resource is assigned to.
        /// </summary>
        /// <remarks>
        /// CloudAttributes.AttributeCloudAccountId
        /// </remarks>
        public virtual string AttributeCloudAccountID => string.Empty;

        /// <summary>
        /// Cloud regions often have multiple, isolated locations known as zones to increase availability. Availability zone represents the zone where the resource is running.
        /// </summary>
        /// <remarks>
        /// Availability zones are called "zones" on Alibaba Cloud and Google Cloud.
        /// </remarks>
        /// <remarks>
        /// CloudAttributes.AttributeCloudAvailabilityZone
        /// </remarks>
        public virtual string AttributeCloudAvailabilityZone => string.Empty;

        /// <summary>
        /// The cloud platform in use.
        /// </summary>
        /// <remarks>
        /// The prefix of the service SHOULD match the one specified in <c>cloud.provider</c>.
        /// </remarks>
        /// <remarks>
        /// CloudAttributes.AttributeCloudPlatform
        /// </remarks>
        public virtual string AttributeCloudPlatform => string.Empty;

        /// <summary>
        /// Name of the cloud provider.
        /// </summary>
        /// <remarks>
        /// CloudAttributes.AttributeCloudProvider
        /// </remarks>
        public virtual string AttributeCloudProvider => string.Empty;

        /// <summary>
        /// The geographical region the resource is running.
        /// </summary>
        /// <remarks>
        /// Refer to your provider's docs to see the available regions, for example <a href="https://www.alibabacloud.com/help/doc-detail/40654.htm">Alibaba Cloud regions</a>, <a href="https://aws.amazon.com/about-aws/global-infrastructure/regions_az/">AWS regions</a>, <a href="https://azure.microsoft.com/global-infrastructure/geographies/">Azure regions</a>, <a href="https://cloud.google.com/about/locations">Google Cloud regions</a>, or <a href="https://www.tencentcloud.com/document/product/213/6091">Tencent Cloud regions</a>.
        /// </remarks>
        /// <remarks>
        /// CloudAttributes.AttributeCloudRegion
        /// </remarks>
        public virtual string AttributeCloudRegion => string.Empty;

        /// <summary>
        /// Cloud provider-specific native identifier of the monitored cloud resource (e.g. an <a href="https://docs.aws.amazon.com/general/latest/gr/aws-arns-and-namespaces.html">ARN</a> on AWS, a <a href="https://learn.microsoft.com/rest/api/resources/resources/get-by-id">fully qualified resource ID</a> on Azure, a <a href="https://cloud.google.com/apis/design/resource_names#full_resource_name">full resource name</a> on GCP).
        /// </summary>
        /// <remarks>
        /// On some cloud providers, it may not be possible to determine the full ID at startup,
        /// so it may be necessary to set <c>cloud.resource_id</c> as a span attribute instead.
        /// <p>
        /// The exact value to use for <c>cloud.resource_id</c> depends on the cloud provider.
        /// The following well-known definitions MUST be used if you set this attribute and they apply:
        /// <p>
        /// <ul>
        ///   <li><strong>AWS Lambda:</strong> The function <a href="https://docs.aws.amazon.com/general/latest/gr/aws-arns-and-namespaces.html">ARN</a>.
        /// Take care not to use the "invoked ARN" directly but replace any
        /// <a href="https://docs.aws.amazon.com/lambda/latest/dg/configuration-aliases.html">alias suffix</a>
        /// with the resolved function version, as the same runtime instance may be invocable with
        /// multiple different aliases.</li>
        ///   <li><strong>GCP:</strong> The <a href="https://cloud.google.com/iam/docs/full-resource-names">URI of the resource</a></li>
        ///   <li><strong>Azure:</strong> The <a href="https://docs.microsoft.com/rest/api/resources/resources/get-by-id">Fully Qualified Resource ID</a> of the invoked function,
        /// <em>not</em> the function app, having the form
        /// <c>/subscriptions/<SUBSCRIPTION_GUID>/resourceGroups/<RG>/providers/Microsoft.Web/sites/<FUNCAPP>/functions/<FUNC></c>.
        /// This means that a span attribute MUST be used, as an Azure function app can host multiple functions that would usually share
        /// a TracerProvider.</li>
        /// </ul>
        /// </remarks>
        /// <remarks>
        /// CloudAttributes.AttributeCloudResourceId
        /// </remarks>
        public virtual string AttributeCloudResourceId => string.Empty;

        /// <summary>
        /// AWS Elastic Compute Cloud.
        /// </summary>
        /// <remarks>
        /// CloudAttributes.CloudPlatformValues.AwsEc2
        /// </remarks>
        public virtual string CloudPlatformValuesAwsEc2 => string.Empty;

        /// <summary>
        /// AWS Elastic Container Service.
        /// </summary>
        /// <remarks>
        /// CloudAttributes.CloudPlatformValues.AwsEcs
        /// </remarks>
        public virtual string CloudPlatformValuesAwsEcs => string.Empty;

        /// <summary>
        /// AWS Elastic Kubernetes Service.
        /// </summary>
        /// <remarks>
        /// CloudAttributes.CloudPlatformValues.AwsEks
        /// </remarks>
        public virtual string CloudPlatformValuesAwsEks => string.Empty;

        /// <summary>
        /// AWS Elastic Beanstalk.
        /// </summary>
        /// <remarks>
        /// CloudAttributes.CloudPlatformValues.AwsElasticBeanstalk
        /// </remarks>
        public virtual string CloudPlatformValuesAwsElasticBeanstalk => string.Empty;

        /// <summary>
        /// Amazon Web Services.
        /// </summary>
        /// <remarks>
        /// CloudAttributes.CloudProviderValues.Aws
        /// </remarks>
        public virtual string CloudProviderValuesAws => string.Empty;
        #endregion

        #region CONTAINER Attributes

        /// <summary>
        /// Container ID. Usually a UUID, as for example used to <a href="https://docs.docker.com/engine/containers/run/#container-identification">identify Docker containers</a>. The UUID might be abbreviated.
        /// </summary>
        /// <remarks>
        /// ContainerAttributes.AttributeContainerId
        /// </remarks>
        public virtual string AttributeContainerID => string.Empty;
        #endregion

        #region DB Attributes

        /// <summary>
        /// The database management system (DBMS) product as identified by the client instrumentation.
        /// </summary>
        /// <remarks>
        /// The actual DBMS may differ from the one identified by the client. For example, when using PostgreSQL client libraries to connect to a CockroachDB, the <c>db.system</c> is set to <c>postgresql</c> based on the instrumentation's best knowledge.
        /// This attribute has stability level RELEASE CANDIDATE.
        /// </remarks>
        /// <remarks>
        /// DbAttributes.AttributeDbSystem
        /// </remarks>
        public virtual string AttributeDbSystem => string.Empty;

        /// <summary>
        /// Amazon DynamoDB.
        /// </summary>
        /// <remarks>
        /// DbAttributes.DbSystemValues.Dynamodb
        /// </remarks>
        public virtual string AttributeDynamoDb => string.Empty;
        #endregion

        #region AWS Attributes

        /// <summary>
        /// The Amazon Resource Name (ARN) of an <a href="https://docs.aws.amazon.com/AmazonECS/latest/developerguide/ECS_instances.html">ECS container instance</a>.
        /// </summary>
        /// <remarks>
        /// AwsAttributes.AttributeAwsEcsContainerArn
        /// </remarks>
        public virtual string AttributeEcsContainerArn => string.Empty;

        /// <summary>
        /// The ARN of an <a href="https://docs.aws.amazon.com/AmazonECS/latest/developerguide/clusters.html">ECS cluster</a>.
        /// </summary>
        /// <remarks>
        /// AwsAttributes.AttributeAwsEcsClusterArn
        /// </remarks>
        public virtual string AttributeEcsClusterArn => string.Empty;

        /// <summary>
        /// The <a href="https://docs.aws.amazon.com/AmazonECS/latest/developerguide/launch_types.html">launch type</a> for an ECS task.
        /// </summary>
        /// <remarks>
        /// AwsAttributes.AttributeAwsEcsLaunchtype
        /// </remarks>
        public virtual string AttributeEcsLaunchtype => string.Empty;

        /// <summary>
        /// ec2.
        /// </summary>
        /// <remarks>
        /// AwsAttributes.AwsEcsLaunchtypeValues.Ec2
        /// </remarks>
        public virtual string ValueEcsLaunchTypeEc2 => string.Empty;

        /// <summary>
        /// fargate.
        /// </summary>
        /// <remarks>
        /// AwsAttributes.AwsEcsLaunchtypeValues.Fargate
        /// </remarks>
        public virtual string ValueEcsLaunchTypeFargate => string.Empty;

        /// <summary>
        /// The ARN of a running <a href="https://docs.aws.amazon.com/AmazonECS/latest/developerguide/ecs-account-settings.html#ecs-resource-ids">ECS task</a>.
        /// </summary>
        /// <remarks>
        /// AwsAttributes.AttributeAwsEcsTaskArn
        /// </remarks>
        public virtual string AttributeEcsTaskArn => string.Empty;

        /// <summary>
        /// The family name of the <a href="https://docs.aws.amazon.com/AmazonECS/latest/developerguide/task_definitions.html">ECS task definition</a> used to create the ECS task.
        /// </summary>
        /// <remarks>
        /// AwsAttributes.AttributeAwsEcsTaskFamily
        /// </remarks>
        public virtual string AttributeEcsTaskFamily => string.Empty;

        /// <summary>
        /// The revision for the task definition used to create the ECS task.
        /// </summary>
        /// <remarks>
        /// AwsAttributes.AttributeAwsEcsTaskRevision
        /// </remarks>
        public virtual string AttributeEcsTaskRevision => string.Empty;

        /// <summary>
        /// The name(s) of the AWS log group(s) an application is writing to.
        /// </summary>
        /// <remarks>
        /// Multiple log groups must be supported for cases like multi-container applications, where a single application has sidecar containers, and each write to their own log group.
        /// </remarks>
        /// <remarks>
        /// AwsAttributes.AttributeAwsLogGroupNames
        /// </remarks>
        public virtual string AttributeLogGroupNames => string.Empty;

        /// <summary>
        /// The Amazon Resource Name(s) (ARN) of the AWS log group(s).
        /// </summary>
        /// <remarks>
        /// See the <a href="https://docs.aws.amazon.com/AmazonCloudWatch/latest/logs/iam-access-control-overview-cwl.html#CWL_ARN_Format">log group ARN format documentation</a>.
        /// </remarks>
        /// <remarks>
        /// AwsAttributes.AttributeAwsLogGroupArns
        /// </remarks>
        public virtual string AttributeLogGroupArns => string.Empty;

        /// <summary>
        /// The name(s) of the AWS log stream(s) an application is writing to.
        /// </summary>
        /// <remarks>
        /// AwsAttributes.AttributeAwsLogStreamArns
        /// </remarks>
        public virtual string AttributeLogStreamNames => string.Empty;

        /// <summary>
        /// The ARN(s) of the AWS log stream(s).
        /// </summary>
        /// <remarks>
        /// See the <a href="https://docs.aws.amazon.com/AmazonCloudWatch/latest/logs/iam-access-control-overview-cwl.html#CWL_ARN_Format">log stream ARN format documentation</a>. One log group can contain several log streams, so these ARNs necessarily identify both a log group and a log stream.
        /// </remarks>
        /// <remarks>
        /// AwsAttributes.AttributeAwsLogStreamNames
        /// </remarks>
        public virtual string AttributeLogStreamArns => string.Empty;

        /// <summary>
        /// The keys in the <c>RequestItems</c> object field.
        /// </summary>
        /// <remarks>
        /// AwsAttributes.AttributeAwsDynamodbTableNames
        /// </remarks>
        public virtual string AttributeAWSDynamoTableName => string.Empty;

        /// <summary>
        /// Not yet incorporated in Semantic Conventions repository.
        /// </summary>
        public virtual string AttributeAWSSQSQueueUrl => string.Empty;

        /// <summary>
        /// Not yet incorporated in Semantic Conventions repository.
        /// </summary>
        public virtual string AttributeAWSBedrockAgentId => string.Empty;

        /// <summary>
        /// Not yet incorporated in Semantic Conventions repository.
        /// </summary>
        public virtual string AttributeAWSBedrockDataSourceId => string.Empty;

        /// <summary>
        /// Not yet incorporated in Semantic Conventions repository.
        /// </summary>
        public virtual string AttributeAWSBedrockGuardrailId => string.Empty;

        /// <summary>
        /// Not yet incorporated in Semantic Conventions repository.
        /// </summary>
        public virtual string AttributeAWSBedrockKnowledgeBaseId => string.Empty;

        /// <summary>
        /// Not yet incorporated in Semantic Conventions repository.
        /// </summary>
        public virtual string AttributeAWSBedrock => string.Empty;

        #endregion

        #region FAAS Attributes

        /// <summary>
        /// Cloud provider-specific native identifier of the monitored cloud resource (e.g. an <a href="https://docs.aws.amazon.com/general/latest/gr/aws-arns-and-namespaces.html">ARN</a> on AWS, a <a href="https://learn.microsoft.com/rest/api/resources/resources/get-by-id">fully qualified resource ID</a> on Azure, a <a href="https://cloud.google.com/apis/design/resource_names#full_resource_name">full resource name</a> on GCP).
        /// </summary>
        /// <remarks>
        /// On some cloud providers, it may not be possible to determine the full ID at startup,
        /// so it may be necessary to set <c>cloud.resource_id</c> as a span attribute instead.
        /// <p>
        /// The exact value to use for <c>cloud.resource_id</c> depends on the cloud provider.
        /// The following well-known definitions MUST be used if you set this attribute and they apply:
        /// <p>
        /// <ul>
        ///   <li><strong>AWS Lambda:</strong> The function <a href="https://docs.aws.amazon.com/general/latest/gr/aws-arns-and-namespaces.html">ARN</a>.
        /// Take care not to use the "invoked ARN" directly but replace any
        /// <a href="https://docs.aws.amazon.com/lambda/latest/dg/configuration-aliases.html">alias suffix</a>
        /// with the resolved function version, as the same runtime instance may be invocable with
        /// multiple different aliases.</li>
        ///   <li><strong>GCP:</strong> The <a href="https://cloud.google.com/iam/docs/full-resource-names">URI of the resource</a></li>
        ///   <li><strong>Azure:</strong> The <a href="https://docs.microsoft.com/rest/api/resources/resources/get-by-id">Fully Qualified Resource ID</a> of the invoked function,
        /// <em>not</em> the function app, having the form
        /// <c>/subscriptions/<SUBSCRIPTION_GUID>/resourceGroups/<RG>/providers/Microsoft.Web/sites/<FUNCAPP>/functions/<FUNC></c>.
        /// This means that a span attribute MUST be used, as an Azure function app can host multiple functions that would usually share
        /// a TracerProvider.</li>
        /// </ul>
        /// </remarks>
        /// <remarks>
        /// CloudAttributes.AttributeCloudResourceId
        /// </remarks>
        public virtual string AttributeFaasID => string.Empty;

        /// <summary>
        /// The invocation ID of the current function invocation.
        /// </summary>
        /// <remarks>
        /// FaasAttributes.AttributeFaasInvocationId
        /// </remarks>
        public virtual string AttributeFaasExecution => string.Empty;

        /// <summary>
        /// The name of the single function that this runtime instance executes.
        /// </summary>
        /// <remarks>
        /// This is the name of the function as configured/deployed on the FaaS
        /// platform and is usually different from the name of the callback
        /// function (which may be stored in the
        /// <a href="/docs/general/attributes.md#source-code-attributes"><c>code.namespace</c>/<c>code.function</c></a>
        /// span attributes).
        /// <p>
        /// For some cloud providers, the above definition is ambiguous. The following
        /// definition of function name MUST be used for this attribute
        /// (and consequently the span name) for the listed cloud providers/products:
        /// <p>
        /// <ul>
        ///   <li><strong>Azure:</strong>  The full name <c><FUNCAPP>/<FUNC></c>, i.e., function app name
        /// followed by a forward slash followed by the function name (this form
        /// can also be seen in the resource JSON for the function).
        /// This means that a span attribute MUST be used, as an Azure function
        /// app can host multiple functions that would usually share
        /// a TracerProvider (see also the <c>cloud.resource_id</c> attribute).</li>
        /// </ul>
        /// </remarks>
        /// <remarks>
        /// FaasAttributes.AttributeFaasName
        /// </remarks>
        public virtual string AttributeFaasName => string.Empty;

        /// <summary>
        /// The immutable version of the function being executed.
        /// </summary>
        /// <remarks>
        /// Depending on the cloud provider and platform, use:
        /// <p>
        /// <ul>
        ///   <li><strong>AWS Lambda:</strong> The <a href="https://docs.aws.amazon.com/lambda/latest/dg/configuration-versions.html">function version</a>
        /// (an integer represented as a decimal string).</li>
        ///   <li><strong>Google Cloud Run (Services):</strong> The <a href="https://cloud.google.com/run/docs/managing/revisions">revision</a>
        /// (i.e., the function name plus the revision suffix).</li>
        ///   <li><strong>Google Cloud Functions:</strong> The value of the
        /// <a href="https://cloud.google.com/functions/docs/env-var#runtime_environment_variables_set_automatically"><c>K_REVISION</c> environment variable</a>.</li>
        ///   <li><strong>Azure Functions:</strong> Not applicable. Do not set this attribute.</li>
        /// </ul>
        /// </remarks>
        /// <remarks>
        /// FaasAttributes.AttributeFaasVersion
        /// </remarks>
        public virtual string AttributeFaasVersion => string.Empty;

        /// <summary>
        /// Type of the trigger which caused this function invocation.
        /// </summary>
        /// <remarks>
        /// FaasAttributes.AttributeFaasTrigger
        /// </remarks>
        public virtual string AttributeFaasTrigger => string.Empty;

        /// <summary>
        /// A boolean that is true if the serverless function is executed for the first time (aka cold-start).
        /// </summary>
        /// <remarks>
        /// FaasAttributes.AttributeFaasColdstart
        /// </remarks>
        public virtual string AttributeFaasColdStart => string.Empty;

        /// <summary>
        /// The execution environment ID as a string, that will be potentially reused for other invocations to the same function/function version.
        /// </summary>
        /// <remarks>
        /// FaasAttributes.AttributeFaasInstance
        /// </remarks>
        public virtual string AttributeFaasInstance => string.Empty;

        /// <summary>
        /// The amount of memory available to the serverless function converted to Bytes.
        /// </summary>
        /// <remarks>
        /// FaasAttributes.AttributeFaasMaxMemory
        /// </remarks>
        public virtual string AttributeFaasMaxMemory => string.Empty;
        #endregion

        #region GEN AI Attributes
        /// <summary>
        /// The name of the GenAI model a request is being made to.
        /// </summary>
        /// <remarks>
        /// GenAiAttributes.AttributeGenAiRequestModel
        /// </remarks>
        public virtual string AttributeGenAiModelId => string.Empty;

        /// <summary>
        /// The Generative AI product as identified by the client or server instrumentation.
        /// </summary>
        /// <remarks>
        /// The <c>gen_ai.system</c> describes a family of GenAI models with specific model identified
        /// by <c>gen_ai.request.model</c> and <c>gen_ai.response.model</c> attributes.
        /// <p>
        /// The actual GenAI product may differ from the one identified by the client.
        /// For example, when using OpenAI client libraries to communicate with Mistral, the <c>gen_ai.system</c>
        /// is set to <c>openai</c> based on the instrumentation's best knowledge.
        /// <p>
        /// For custom model, a custom friendly name SHOULD be used.
        /// If none of these options apply, the <c>gen_ai.system</c> SHOULD be set to <c>_OTHER</c>.
        /// </remarks>
        /// <remarks>
        /// GenAiAttributes.AttributeGenAiSystem
        /// </remarks>
        public virtual string AttributeGenAiSystem => string.Empty;

        #endregion

        #region HOST Attributes

        /// <summary>
        /// Unique host ID. For Cloud, this must be the instance_id assigned by the cloud provider. For non-containerized systems, this should be the <c>machine-id</c>. See the table below for the sources to use to determine the <c>machine-id</c> based on operating system.
        /// </summary>
        /// <remarks>
        /// HostAttributes.AttributeHostId
        /// </remarks>
        public virtual string AttributeHostID => string.Empty;

        /// <summary>
        /// Type of host. For Cloud, this must be the machine type.
        /// </summary>
        /// <remarks>
        /// HostAttributes.AttributeHostType
        /// </remarks>
        public virtual string AttributeHostType => string.Empty;

        /// <summary>
        /// Name of the host. On Unix systems, it may contain what the hostname command returns, or the fully qualified hostname, or another name specified by the user.
        /// </summary>
        /// <remarks>
        /// HostAttributes.AttributeHostName
        /// </remarks>
        public virtual string AttributeHostName => string.Empty;

        #endregion

        #region HTTP Attributes

        /// <summary>
        /// Deprecated, use <c>http.response.status_code</c> instead.
        /// </summary>
        /// <remarks>
        /// HttpAttributes.AttributeHttpStatusCode
        /// </remarks>
        [Obsolete("Replaced by <c>http.response.status_code</c>.")]
        public virtual string AttributeHttpStatusCode => string.Empty;

        /// <summary>
        /// <a href="https://tools.ietf.org/html/rfc7231#section-6">HTTP response status code</a>.
        /// </summary>
        /// <remarks>
        /// HttpAttributes.AttributeHttpResponseStatusCode
        /// </remarks>
        public virtual string AttributeHttpResponseStatusCode => string.Empty;

        /// <summary>
        /// Deprecated, use <c>url.scheme</c> instead.
        /// </summary>
        /// <remarks>
        /// HttpAttributes.AttributeHttpScheme
        /// </remarks>
        [Obsolete("Replaced by <c>url.scheme</c> instead.")]
        public virtual string AttributeHttpScheme => string.Empty;

        /// <summary>
        /// Deprecated, use <c>url.path</c> and <c>url.query</c> instead.
        /// </summary>
        /// <remarks>
        /// HttpAttributes.AttributeHttpTarget
        /// </remarks>
        [Obsolete("Split to <c>url.path</c> and `url.query.")]
        public virtual string AttributeHttpTarget => string.Empty;

        /// <summary>
        /// Deprecated, use <c>http.request.method</c> instead.
        /// </summary>
        /// <remarks>
        /// HttpAttributes.AttributeHttpMethod
        /// </remarks>
        [Obsolete("Replaced by <c>http.request.method</c>.")]
        public virtual string AttributeHttpMethod => string.Empty;

        /// <summary>
        /// HTTP request method.
        /// </summary>
        /// <remarks>
        /// HTTP request method value SHOULD be "known" to the instrumentation.
        /// By default, this convention defines "known" methods as the ones listed in <a href="https://www.rfc-editor.org/rfc/rfc9110.html#name-methods">RFC9110</a>
        /// and the PATCH method defined in <a href="https://www.rfc-editor.org/rfc/rfc5789.html">RFC5789</a>.
        /// <p>
        /// If the HTTP request method is not known to instrumentation, it MUST set the <c>http.request.method</c> attribute to <c>_OTHER</c>.
        /// <p>
        /// If the HTTP instrumentation could end up converting valid HTTP request methods to <c>_OTHER</c>, then it MUST provide a way to override
        /// the list of known HTTP methods. If this override is done via environment variable, then the environment variable MUST be named
        /// OTEL_INSTRUMENTATION_HTTP_KNOWN_METHODS and support a comma-separated list of case-sensitive known HTTP methods
        /// (this list MUST be a full override of the default known method, it is not a list of known methods in addition to the defaults).
        /// <p>
        /// HTTP method names are case-sensitive and <c>http.request.method</c> attribute value MUST match a known HTTP method name exactly.
        /// Instrumentations for specific web frameworks that consider HTTP methods to be case insensitive, SHOULD populate a canonical equivalent.
        /// Tracing instrumentations that do so, MUST also set <c>http.request.method_original</c> to the original value.
        /// </remarks>
        /// <remarks>
        /// HttpAttributes.AttributeHttpRequestMethod
        /// </remarks>
        public virtual string AttributeHttpRequestMethod => string.Empty;

        #endregion

        #region NET Attributes

        /// <summary>
        /// Deprecated, use <c>server.address</c>.
        /// </summary>
        /// <remarks>
        /// NetAttributes.AttributeNetHostName
        /// </remarks>
        [Obsolete("Replaced by <c>server.address</c>.")]
        public virtual string AttributeNetHostName => string.Empty;

        /// <summary>
        /// Deprecated, use <c>server.port</c>.
        /// </summary>
        /// <remarks>
        /// NetAttributes.AttributeNetHostPort
        /// </remarks>
        [Obsolete("Replaced by <c>server.port</c>.")]
        public virtual string AttributeNetHostPort => string.Empty;

        #endregion

        #region SERVER Attributes

        /// <summary>
        /// Server domain name if available without reverse DNS lookup; otherwise, IP address or Unix domain socket name.
        /// </summary>
        /// <remarks>
        /// When observed from the client side, and when communicating through an intermediary, <c>server.address</c> SHOULD represent the server address behind any intermediaries, for example proxies, if it's available.
        /// </remarks>
        /// <remarks>
        /// ServerAttributes.AttributeServerAddress
        /// </remarks>
        public virtual string AttributeServerAddress => string.Empty;

        /// <summary>
        /// Server port number.
        /// </summary>
        /// <remarks>
        /// When observed from the client side, and when communicating through an intermediary, <c>server.port</c> SHOULD represent the server port behind any intermediaries, for example proxies, if it's available.
        /// </remarks>
        /// <remarks>
        /// ServerAttributes.AttributeServerPort
        /// </remarks>
        public virtual string AttributeServerPort => string.Empty;

        // K8s Attributes

        /// <summary>
        /// The name of the cluster.
        /// </summary>
        /// <remarks>
        /// K8sAttributes.AttributeK8sClusterName
        /// </remarks>
        public virtual string AttributeK8SClusterName => string.Empty;

        #endregion

        #region SERVICE Attributes

        /// <summary>
        /// Logical name of the service.
        /// </summary>
        /// <remarks>
        /// MUST be the same for all instances of horizontally scaled services. If the value was not specified, SDKs MUST fallback to <c>unknown_service:</c> concatenated with <a href="process.md"><c>process.executable.name</c></a>, e.g. <c>unknown_service:bash</c>. If <c>process.executable.name</c> is not available, the value MUST be set to <c>unknown_service</c>.
        /// </remarks>
        /// <remarks>
        /// ServiceAttributes.AttributeServiceName
        /// </remarks>
        public virtual string AttributeServiceName => string.Empty;

        /// <summary>
        /// A namespace for <c>service.name</c>.
        /// </summary>
        /// <remarks>
        /// A string value having a meaning that helps to distinguish a group of services, for example the team name that owns a group of services. <c>service.name</c> is expected to be unique within the same namespace. If <c>service.namespace</c> is not specified in the Resource then <c>service.name</c> is expected to be unique for all services that have no explicit namespace defined (so the empty/unspecified namespace is simply one more valid namespace). Zero-length namespace string is assumed equal to unspecified namespace.
        /// </remarks>
        /// <remarks>
        /// ServiceAttributes.AttributeServiceNamespace
        /// </remarks>
        public virtual string AttributeServiceNamespace => string.Empty;

        /// <summary>
        /// The string ID of the service instance.
        /// </summary>
        /// <remarks>
        /// MUST be unique for each instance of the same <c>service.namespace,service.name</c> pair (in other words
        /// <c>service.namespace,service.name,service.instance.id</c> triplet MUST be globally unique). The ID helps to
        /// distinguish instances of the same service that exist at the same time (e.g. instances of a horizontally scaled
        /// service).
        /// <p>
        /// Implementations, such as SDKs, are recommended to generate a random Version 1 or Version 4 <a href="https://www.ietf.org/rfc/rfc4122.txt">RFC
        /// 4122</a> UUID, but are free to use an inherent unique ID as the source of
        /// this value if stability is desirable. In that case, the ID SHOULD be used as source of a UUID Version 5 and
        /// SHOULD use the following UUID as the namespace: <c>4d63009a-8d0f-11ee-aad7-4c796ed8e320</c>.
        /// <p>
        /// UUIDs are typically recommended, as only an opaque value for the purposes of identifying a service instance is
        /// needed. Similar to what can be seen in the man page for the
        /// <a href="https://www.freedesktop.org/software/systemd/man/machine-id.html"><c>/etc/machine-id</c></a> file, the underlying
        /// data, such as pod name and namespace should be treated as confidential, being the user's choice to expose it
        /// or not via another resource attribute.
        /// <p>
        /// For applications running behind an application server (like unicorn), we do not recommend using one identifier
        /// for all processes participating in the application. Instead, it's recommended each division (e.g. a worker
        /// thread in unicorn) to have its own instance.id.
        /// <p>
        /// It's not recommended for a Collector to set <c>service.instance.id</c> if it can't unambiguously determine the
        /// service instance that is generating that telemetry. For instance, creating an UUID based on <c>pod.name</c> will
        /// likely be wrong, as the Collector might not know from which container within that pod the telemetry originated.
        /// However, Collectors can set the <c>service.instance.id</c> if they can unambiguously determine the service instance
        /// for that telemetry. This is typically the case for scraping receivers, as they know the target address and
        /// port.
        /// </remarks>
        /// <remarks>
        /// ServiceAttributes.AttributeServiceInstanceId
        /// </remarks>
        public virtual string AttributeServiceInstanceID => string.Empty;

        /// <summary>
        /// The version string of the service API or implementation. The format is not defined by these conventions.
        /// </summary>
        /// <remarks>
        /// ServiceAttributes.AttributeServiceVersion
        /// </remarks>
        public virtual string AttributeServiceVersion => string.Empty;

        /// <summary>
        /// Not yet incorporated in Semantic Conventions repository.
        /// </summary>
        public virtual string ServiceNameValuesAwsElasticBeanstalk => string.Empty;

        #endregion

        #region URL Attributes

        /// <summary>
        /// The <a href="https://www.rfc-editor.org/rfc/rfc3986#section-3.3">URI path</a> component.
        /// </summary>
        /// <remarks>
        /// Sensitive content provided in <c>url.path</c> SHOULD be scrubbed when instrumentations can identify it.
        /// </remarks>
        /// <remarks>
        /// UrlAttributes.AttributeUrlPath
        /// </remarks>
        public virtual string AttributeUrlPath => string.Empty;

        /// <summary>
        /// The <a href="https://www.rfc-editor.org/rfc/rfc3986#section-3.4">URI query</a> component.
        /// </summary>
        /// <remarks>
        /// Sensitive content provided in <c>url.query</c> SHOULD be scrubbed when instrumentations can identify it.
        /// <p>
        /// Query string values for the following keys SHOULD be redacted by default and replaced by the value <c>REDACTED</c>:
        /// <p>
        /// <ul>
        ///   <li><a href="https://docs.aws.amazon.com/AmazonS3/latest/userguide/RESTAuthentication.html#RESTAuthenticationQueryStringAuth"><c>AWSAccessKeyId</c></a></li>
        ///   <li><a href="https://docs.aws.amazon.com/AmazonS3/latest/userguide/RESTAuthentication.html#RESTAuthenticationQueryStringAuth"><c>Signature</c></a></li>
        ///   <li><a href="https://learn.microsoft.com/azure/storage/common/storage-sas-overview#sas-token"><c>sig</c></a></li>
        ///   <li><a href="https://cloud.google.com/storage/docs/access-control/signed-urls"><c>X-Goog-Signature</c></a></li>
        /// </ul>
        /// This list is subject to change over time.
        /// <p>
        /// When a query string value is redacted, the query string key SHOULD still be preserved, e.g.
        /// <c>q=OpenTelemetry&sig=REDACTED</c>.
        /// </remarks>
        /// <remarks>
        /// UrlAttributes.AttributeUrlQuery
        /// </remarks>
        public virtual string AttributeUrlQuery => string.Empty;

        /// <summary>
        /// The <a href="https://www.rfc-editor.org/rfc/rfc3986#section-3.1">URI scheme</a> component identifying the used protocol.
        /// </summary>
        /// <remarks>
        /// UrlAttributes.AttributeUrlScheme
        /// </remarks>
        public virtual string AttributeUrlScheme => string.Empty;

        #endregion
    }
}
