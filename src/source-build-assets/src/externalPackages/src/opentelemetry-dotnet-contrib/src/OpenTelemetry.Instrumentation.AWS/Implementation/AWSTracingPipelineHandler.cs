// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Telemetry;
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
        var activity = this.ProcessBeginRequest(executionContext);
        base.InvokeSync(executionContext);
        this.ProcessEndRequest(activity, executionContext);
    }

    public override async Task<T> InvokeAsync<T>(IExecutionContext executionContext)
    {
        var activity = this.ProcessBeginRequest(executionContext);
        var ret = await base.InvokeAsync<T>(executionContext).ConfigureAwait(false);

        this.ProcessEndRequest(activity, executionContext);

        return ret;
    }

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

        if (AWSServiceHelper.ServiceResponseParameterMap.TryGetValue(service, out var parameters))
        {
            var response = responseContext.Response;

            foreach (var parameter in parameters)
            {
                try
                {
                    // for bedrock agent, extract attribute from object in response.
                    if (AWSServiceType.IsBedrockAgentService(service))
                    {
                        var operationName = Utils.RemoveSuffix(response.GetType().Name, "Response");
                        if (AWSServiceHelper.OperationNameToResourceMap()[operationName] == parameter)
                        {
                            this.AddBedrockAgentResponseAttribute(activity, response, parameter);
                        }
                    }

                    var property = response.GetType().GetProperty(parameter);
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
        var responseObject = response.GetType().GetProperty(Utils.RemoveSuffix(parameter, "Id"));
        if (responseObject != null)
        {
            var attributeObject = responseObject.GetValue(response);
            if (attributeObject != null)
            {
                var property = attributeObject.GetType().GetProperty(parameter);
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

            foreach (var parameter in parameters)
            {
                try
                {
                    // for bedrock agent, we only extract one attribute based on the operation.
                    if (AWSServiceType.IsBedrockAgentService(service))
                    {
                        if (AWSServiceHelper.OperationNameToResourceMap()[AWSServiceHelper.GetAWSOperationName(requestContext)] != parameter)
                        {
                            continue;
                        }
                    }

                    var property = request.GetType().GetProperty(parameter);
                    if (property != null)
                    {
                        if (this.awsServiceHelper.ParameterAttributeMap.TryGetValue(parameter, out var attribute))
                        {
                            activity.SetTag(attribute, property.GetValue(request));
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
        if (this.options.SuppressDownstreamInstrumentation)
        {
            SuppressInstrumentationScope.Enter();
        }

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
}
