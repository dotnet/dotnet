// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Telemetry;
using Amazon.Util;
using OpenTelemetry.AWS;
using OpenTelemetry.Context.Propagation;

namespace OpenTelemetry.Instrumentation.AWS.Implementation;

/// <summary>
/// Adds additional request and response tags depending on the target AWS Service.
/// <para />
/// This <see cref="PipelineHandler"/> must execute early in the AWS SDK pipeline
/// in order to manipulate outgoing requests objects before they are marshalled (ie serialized).
/// </summary>
internal sealed class AWSTracingPipelineHandler : PipelineHandler
{
    // Caches reflected properties keyed by (declaring type, property name) to avoid repeated
    // GetType().GetProperty(...) lookups on the hot request/response tagging path. The set of
    // AWS request/response types and parameter names is small and bounded, so growth is limited.
    private static readonly ConcurrentDictionary<(Type Type, string PropertyName), PropertyInfo?> PropertyCache = new();

    private readonly AWSClientInstrumentationOptions options;
    private readonly AWSSemanticConventions awsSemanticConventions;
    private readonly AWSServiceHelper awsServiceHelper;

    public AWSTracingPipelineHandler(AWSClientInstrumentationOptions options)
    {
        this.options = options;
        this.awsSemanticConventions = new AWSSemanticConventions(options.SemanticConventionVersion);
        this.awsServiceHelper = new AWSServiceHelper(this.awsSemanticConventions);
    }

    public override void InvokeSync(IExecutionContext executionContext)
    {
        using var scope = this.SuppressDownstreamInstrumentation();
        var activity = this.ProcessBeginRequest(executionContext);
        base.InvokeSync(executionContext);
        this.ProcessEndRequest(activity, executionContext);
    }

    public override async Task<T> InvokeAsync<T>(IExecutionContext executionContext)
    {
        using var scope = this.SuppressDownstreamInstrumentation();
        var activity = this.ProcessBeginRequest(executionContext);
        var ret = await base.InvokeAsync<T>(executionContext).ConfigureAwait(false);

        this.ProcessEndRequest(activity, executionContext);

        return ret;
    }

#if NET
    [System.Diagnostics.CodeAnalysis.UnconditionalSuppressMessage(
        "Trimming",
        "IL2080",
        Justification = "The reflected properties were already used by the AWS SDK's marshallers so the properties could not have been trimmed.")]
#endif
    private static PropertyInfo? GetCachedProperty(Type type, string propertyName)
        => PropertyCache.GetOrAdd((type, propertyName), static key => key.Type.GetProperty(key.PropertyName));

    private static void AddPropagationDataToRequest(Activity activity, IRequestContext requestContext)
    {
        var service = requestContext.ServiceMetaData.ServiceId;

        if (AWSServiceType.IsSqsService(service))
        {
            SqsRequestContextHelper.AddAttributes(
                requestContext, AWSMessagingUtils.InjectIntoDictionary(new PropagationContext(activity.Context, Baggage.Current)));
        }
        else if (AWSServiceType.IsSnsService(service))
        {
            SnsRequestContextHelper.AddAttributes(
                requestContext, AWSMessagingUtils.InjectIntoDictionary(new PropagationContext(activity.Context, Baggage.Current)));
        }
    }

#if NET
    [System.Diagnostics.CodeAnalysis.UnconditionalSuppressMessage(
        "Trimming",
        "IL2075",
        Justification = "The reflected properties were already used by the AWS SDK's marshallers so the properties could not have been trimmed.")]
#endif
    private void AddResponseSpecificInformation(Activity activity, IExecutionContext executionContext)
    {
        var service = executionContext.RequestContext.ServiceMetaData.ServiceId;
        var responseContext = executionContext.ResponseContext;

        if (responseContext.HttpResponse.IsHeaderPresent(HeaderKeys.XAmzId2Header))
        {
            this.awsSemanticConventions.TagBuilder.SetTagAttributeAWSExtendedRequestId(activity, responseContext.HttpResponse.GetHeaderValue(HeaderKeys.XAmzId2Header));
        }

        if (AWSServiceHelper.ServiceResponseParameterMap.TryGetValue(service, out var parameters))
        {
            var response = responseContext.Response;
            var responseType = response.GetType();

            var isBedrockAgentService = AWSServiceType.IsBedrockAgentService(service);
            string? bedrockAgentResource = null;
            if (isBedrockAgentService)
            {
                var operationName = Utils.RemoveSuffix(responseType.Name, "Response");
                _ = AWSServiceHelper.OperationNameToResourceMap.TryGetValue(operationName, out bedrockAgentResource);
            }

            foreach (var parameter in parameters)
            {
                try
                {
                    // for bedrock agent, extract attribute from object in response.
                    if (isBedrockAgentService)
                    {
                        // Skip parameters for operations that are not mapped to a resource.
                        if (bedrockAgentResource == null)
                        {
                            continue;
                        }

                        if (bedrockAgentResource == parameter)
                        {
                            this.AddBedrockAgentResponseAttribute(activity, response, parameter);
                        }
                    }

                    var property = GetCachedProperty(responseType, parameter);
                    if (property != null)
                    {
                        if (this.awsServiceHelper.ParameterAttributeMap.TryGetValue(parameter, out var attribute))
                        {
                            activity.SetTag(attribute, property.GetValue(response));
                        }
                    }
                }
                catch (Exception)
                {
                    // Guard against any reflection-related exceptions when running in AoT.
                    // See https://github.com/open-telemetry/opentelemetry-dotnet-contrib/issues/1543#issuecomment-1907667722.
                }
            }
        }
    }

#if NET
    [System.Diagnostics.CodeAnalysis.UnconditionalSuppressMessage(
        "Trimming",
        "IL2075",
        Justification = "The reflected properties were already used by the AWS SDK's marshallers so the properties could not have been trimmed.")]
#endif
    private void AddBedrockAgentResponseAttribute(Activity activity, AmazonWebServiceResponse response, string parameter)
    {
        var responseObject = GetCachedProperty(response.GetType(), Utils.RemoveSuffix(parameter, "Id"));
        if (responseObject != null)
        {
            var attributeObject = responseObject.GetValue(response);
            if (attributeObject != null)
            {
                var property = GetCachedProperty(attributeObject.GetType(), parameter);
                if (property != null)
                {
                    if (this.awsServiceHelper.ParameterAttributeMap.TryGetValue(parameter, out var attribute))
                    {
                        activity.SetTag(attribute, property.GetValue(attributeObject));
                    }
                }
            }
        }
    }

#if NET
    [System.Diagnostics.CodeAnalysis.UnconditionalSuppressMessage(
        "Trimming",
        "IL2075",
        Justification = "The reflected properties were already used by the AWS SDK's marshallers so the properties could not have been trimmed.")]
#endif
    private void AddRequestSpecificInformation(Activity activity, IRequestContext requestContext)
    {
        var service = requestContext.ServiceMetaData.ServiceId;

        if (AWSServiceHelper.ServiceRequestParameterMap.TryGetValue(service, out var parameters))
        {
            var request = requestContext.OriginalRequest;
            var requestType = request.GetType();

            var isBedrockAgentService = AWSServiceType.IsBedrockAgentService(service);
            string? bedrockAgentResource = null;
            if (isBedrockAgentService)
            {
                AWSServiceHelper.OperationNameToResourceMap.TryGetValue(AWSServiceHelper.GetAWSOperationName(requestContext), out bedrockAgentResource);
            }

            foreach (var parameter in parameters)
            {
                try
                {
                    // for bedrock agent, we only extract one attribute based on the operation.
                    if (isBedrockAgentService && bedrockAgentResource != parameter)
                    {
                        continue;
                    }

                    var property = GetCachedProperty(requestType, parameter);
                    if (property != null)
                    {
                        if (this.awsServiceHelper.ParameterAttributeMap.TryGetValue(parameter, out var attribute))
                        {
                            var value = property.GetValue(request);

                            if (value is string stringValue && this.awsServiceHelper.ArrayValueAttributeNames.Contains(attribute))
                            {
                                activity.SetTag(attribute, new string[] { stringValue });
                            }
                            else
                            {
                                activity.SetTag(attribute, value);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    // Guard against any reflection-related exceptions when running in AoT.
                    // See https://github.com/open-telemetry/opentelemetry-dotnet-contrib/issues/1543#issuecomment-1907667722.
                }
            }
        }

        if (AWSServiceType.IsDynamoDbService(service))
        {
            this.awsSemanticConventions.TagBuilder.SetTagAttributeDbSystemToDynamoDb(activity);
        }
        else if (AWSServiceType.IsBedrockRuntimeService(service))
        {
            this.awsSemanticConventions.TagBuilder.SetTagAttributeGenAiSystemToBedrock(activity);
        }
        else if (AWSServiceType.IsSnsService(service))
        {
            // See https://github.com/open-telemetry/semantic-conventions/blob/v1.40.0/docs/messaging/sns.md
            this.awsSemanticConventions.TagBuilder.SetTagAttributeMessagingSystemToSns(activity);

            var topicArn = activity.GetTagItem("aws.sns.topic.arn");

            if (topicArn is string arn && TryGetLastSplitItem(arn, ':', out var topicName))
            {
#if NET
                this.awsSemanticConventions.TagBuilder.SetTagAttributeMessagingDestinationName(activity, topicName);
#else
                this.awsSemanticConventions.TagBuilder.SetTagAttributeMessagingDestinationName(activity, topicName!);
#endif
            }

            var operationName = AWSServiceHelper.GetAWSOperationName(requestContext);
            this.awsSemanticConventions.TagBuilder.SetTagAttributeMessagingOperationName(activity, operationName);

            if (AWSServiceHelper.MessagingOperationTypeMap.TryGetValue(AWSServiceType.SNSService, out var map) &&
                map.TryGetValue(operationName, out var operationType))
            {
                this.awsSemanticConventions.TagBuilder.SetTagAttributeMessagingOperationType(activity, operationType);
            }
        }
        else if (AWSServiceType.IsSqsService(service))
        {
            // See https://github.com/open-telemetry/semantic-conventions/blob/v1.40.0/docs/messaging/sqs.md
            this.awsSemanticConventions.TagBuilder.SetTagAttributeMessagingSystemToSqs(activity);

            var queueUrl = activity.GetTagItem("aws.sqs.queue.url");

            if (queueUrl is string url && Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                activity.SetTag("server.address", uri.Host);

                if (uri.GetLeftPart(UriPartial.Path) is { Length: > 0 } path && TryGetLastSplitItem(path, '/', out var queueName))
                {
#if NET
                    this.awsSemanticConventions.TagBuilder.SetTagAttributeMessagingDestinationName(activity, queueName);
#else
                    this.awsSemanticConventions.TagBuilder.SetTagAttributeMessagingDestinationName(activity, queueName!);
#endif
                }
            }

            var operationName = AWSServiceHelper.GetAWSOperationName(requestContext);
            this.awsSemanticConventions.TagBuilder.SetTagAttributeMessagingOperationName(activity, operationName);

            if (AWSServiceHelper.MessagingOperationTypeMap.TryGetValue(AWSServiceType.SQSService, out var map) &&
                map.TryGetValue(operationName, out var operationType))
            {
                this.awsSemanticConventions.TagBuilder.SetTagAttributeMessagingOperationType(activity, operationType);
            }
        }

        var region = requestContext.ClientConfig?.RegionEndpoint?.SystemName;
        if (!string.IsNullOrEmpty(region))
        {
            this.awsSemanticConventions.TagBuilder.SetTagAttributeCloudRegion(activity, region);
        }

        this.awsSemanticConventions.TagBuilder.SetTagAttributeRpcSystemName(activity);

        static bool TryGetLastSplitItem(
            string value,
            char delimiter,
#if NET
            [System.Diagnostics.CodeAnalysis.NotNullWhen(true)]
#endif
            out string? lastItem)
        {
            lastItem = null;
            var result = false;

            var index = value.LastIndexOf(delimiter);

            if (index > -1 && index < value.Length - 1)
            {
#if NET
                lastItem = value[(index + 1)..];
#else
                lastItem = value.Substring(index + 1);
#endif

                result = true;
            }

            return result;
        }
    }

    private void ProcessEndRequest(Activity? activity, IExecutionContext executionContext)
    {
        if (activity == null || !activity.IsAllDataRequested)
        {
            return;
        }

        this.AddResponseSpecificInformation(activity, executionContext);
    }

    private Activity? ProcessBeginRequest(IExecutionContext executionContext)
    {
        var currentActivity = Activity.Current;

        if (currentActivity == null)
        {
            return null;
        }

        if (currentActivity.IsAllDataRequested
            && currentActivity.Source.Name.StartsWith(TelemetryConstants.TelemetryScopePrefix, StringComparison.Ordinal))
        {
            this.AddRequestSpecificInformation(currentActivity, executionContext.RequestContext);
        }

        // Context propagation should always happen regardless of sampling decision (which affects Activity.IsAllDataRequested and Activity.Source).
        // Otherwise, downstream services can make inconsistent sampling decisions and create incomplete traces.
        AddPropagationDataToRequest(currentActivity, executionContext.RequestContext);

        return currentActivity;
    }

    private IDisposable? SuppressDownstreamInstrumentation() =>
        this.options.SuppressDownstreamInstrumentation
            ? SuppressInstrumentationScope.Begin()
            : null;
}
