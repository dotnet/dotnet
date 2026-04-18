// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Globalization;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.ApplicationLoadBalancerEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using Amazon.Lambda.SQSEvents;
using OpenTelemetry.AWS;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Extensions.AWS.Trace;

namespace OpenTelemetry.Instrumentation.AWSLambda.Implementation;

/// <summary>
/// Class for getting AWS Lambda related attributes.
/// </summary>
internal class AWSLambdaUtils
{
    private const string AWSRegion = "AWS_REGION";
    private const string AWSXRayTraceHeaderKey = "X-Amzn-Trace-Id";
    private const string FunctionName = "AWS_LAMBDA_FUNCTION_NAME";
    private const string FunctionVersion = "AWS_LAMBDA_FUNCTION_VERSION";
    private const string FunctionMaxMemory = "AWS_LAMBDA_FUNCTION_MEMORY_SIZE";
    private const string FunctionLogStreamName = "AWS_LAMBDA_LOG_STREAM_NAME";
    private const string AWSXRayLambdaTraceHeaderKey = "_X_AMZN_TRACE_ID";

    private static readonly Func<IDictionary<string, string>, string, IEnumerable<string>> Getter = (headers, name) =>
    {
        return headers.TryGetValue(name, out var value) ? [value] : [];
    };

    // Added volatile for abundance of caution because in theory, particular in using Lambda's Mananged Instances,
    // during initial startup multiple invocations invoking GetXRayParentContext() at the same time before the first
    // TraceProviderIsolated.CurrentTraceId is called.
    private static volatile bool traceProviderIsolatedFailed;

    private readonly AWSSemanticConventions semanticConventionBuilder;

    public AWSLambdaUtils(AWSSemanticConventions semanticConventionBuilder)
    {
        this.semanticConventionBuilder = semanticConventionBuilder;
    }

    internal static ActivityContext GetXRayParentContext()
    {
        string? tracerHeaderValue;
        try
        {
            if (!traceProviderIsolatedFailed)
            {
                // Prefer the TraceProviderIsolated.CurrentTraceId over the environment variable to support
                // AWS Lambda's Managed Instances feature. With Managed Instances there are multiple invocations
                // per execution and environment variable is not set since there isn't a single value to set to.
                tracerHeaderValue = TraceProviderIsolated.CurrentTraceId;
            }
            else
            {
                // If accessing TraceProviderIsolated failed before it will always fail, so skip trying to access it again.
                tracerHeaderValue = Environment.GetEnvironmentVariable(AWSXRayLambdaTraceHeaderKey);
            }
        }
        catch (TypeLoadException)
        {
            traceProviderIsolatedFailed = true;
            tracerHeaderValue = Environment.GetEnvironmentVariable(AWSXRayLambdaTraceHeaderKey);
        }

        if (tracerHeaderValue == null)
        {
            return default;
        }

        var activityContext = ParseXRayTraceHeader(tracerHeaderValue);
        return activityContext;
    }

    internal static (ActivityContext ParentContext, IEnumerable<ActivityLink>? Links) ExtractParentContext<TInput>(TInput input)
    {
        PropagationContext parentContext = default;
        IEnumerable<ActivityLink>? links = null;
        switch (input)
        {
            case APIGatewayProxyRequest apiGatewayProxyRequest:
                parentContext = Propagators.DefaultTextMapPropagator.Extract(default, apiGatewayProxyRequest, GetHeaderValues);
                break;
            case APIGatewayHttpApiV2ProxyRequest apiGatewayHttpApiV2ProxyRequest:
                parentContext = Propagators.DefaultTextMapPropagator.Extract(default, apiGatewayHttpApiV2ProxyRequest, GetHeaderValues);
                break;
            case ApplicationLoadBalancerRequest applicationLoadBalancerRequest:
                parentContext = Propagators.DefaultTextMapPropagator.Extract(default, applicationLoadBalancerRequest, GetHeaderValues);
                break;
            case SQSEvent sqsEvent:
                (parentContext, links) = AWSMessagingUtils.ExtractParentContext(sqsEvent);
                break;
            case SQSEvent.SQSMessage sqsMessage:
                parentContext = AWSMessagingUtils.ExtractParentContext(sqsMessage);
                break;
            case SNSEvent snsEvent:
                parentContext = AWSMessagingUtils.ExtractParentContext(snsEvent);
                break;
            case SNSEvent.SNSRecord snsRecord:
                parentContext = AWSMessagingUtils.ExtractParentContext(snsRecord);
                break;
            default:
                break;
        }

        return (parentContext.ActivityContext, links);
    }

    internal static string? GetAWSRegion()
    {
        return Environment.GetEnvironmentVariable(AWSRegion);
    }

    internal static string? GetFunctionName(ILambdaContext? context = null)
    {
        return context?.FunctionName ?? Environment.GetEnvironmentVariable(FunctionName);
    }

    internal static string? GetFunctionVersion()
    {
        return Environment.GetEnvironmentVariable(FunctionVersion);
    }

    internal static int? GetFunctionMemorySize(ILambdaContext? context = null)
    {
        int? memoryLimitInMB = context?.MemoryLimitInMB;

        if (!memoryLimitInMB.HasValue)
        {
            var value = Environment.GetEnvironmentVariable(FunctionMaxMemory);

            if (!string.IsNullOrWhiteSpace(value) && int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int valueMB))
            {
                memoryLimitInMB = valueMB;
            }
        }

        if (memoryLimitInMB.HasValue)
        {
            // Convert to bytes to match semantic conventions (e.g. 128 to 134217728)
            return memoryLimitInMB.Value * 1024 * 1024;
        }

        return null;
    }

    internal static string? GetFunctionInstance(ILambdaContext? context = null)
    {
        return context?.LogStreamName ?? Environment.GetEnvironmentVariable(FunctionLogStreamName);
    }

    internal static IEnumerable<string>? GetHeaderValues(APIGatewayProxyRequest request, string name)
    {
        var multiValueHeader = request.MultiValueHeaders?.GetValueByKeyIgnoringCase(name);
        if (multiValueHeader != null)
        {
            return multiValueHeader;
        }

        var headerValue = request.Headers?.GetValueByKeyIgnoringCase(name);

        return headerValue != null ? new[] { headerValue } : null;
    }

    internal static IEnumerable<string>? GetHeaderValues(APIGatewayHttpApiV2ProxyRequest request, string name)
    {
        var headerValue = GetHeaderValue(request, name);

        // Multiple values for the same header will be separated by a comma.
        return headerValue?.Split(',');
    }

    internal static IEnumerable<string>? GetHeaderValues(ApplicationLoadBalancerRequest request, string name)
    {
        var multiValueHeader = request.MultiValueHeaders?.GetValueByKeyIgnoringCase(name);
        if (multiValueHeader != null)
        {
            return multiValueHeader;
        }

        var headerValue = request.Headers?.GetValueByKeyIgnoringCase(name);

        return headerValue != null ? new[] { headerValue } : null;
    }

    internal IEnumerable<KeyValuePair<string, object>> GetFunctionTags<TInput>(TInput input, ILambdaContext context, bool isColdStart)
    {
        var functionArn = context.InvokedFunctionArn;

        var tags =
            this.semanticConventionBuilder
                .AttributeBuilder
                .AddAttributeFaasTrigger(GetFaasTrigger(input))
                .AddAttributeFaasColdStart(isColdStart)
                .AddAttributeFaasName(GetFunctionName(context))
                .AddAttributeFaasExecution(context.AwsRequestId)
                .AddAttributeFaasID(GetFaasId(functionArn))
                .AddAttributeFaasInstance(GetFunctionInstance(context))
                .AddAttributeFaasMaxMemory(GetFunctionMemorySize(context))
                .AddAttributeCloudAccountID(GetAccountId(functionArn))
                .Build();

        return tags;
    }

    private static string? GetHeaderValue(APIGatewayHttpApiV2ProxyRequest request, string name) =>
        request.Headers?.GetValueByKeyIgnoringCase(name);

    private static string? GetAccountId(string? functionArn)
    {
        if (string.IsNullOrEmpty(functionArn))
        {
            return null;
        }

        // The fifth item of function arn: https://github.com/open-telemetry/opentelemetry-specification/blob/86aeab1e0a7e6c67be09c7f15ff25063ee6d2b5c/specification/trace/semantic_conventions/instrumentation/aws-lambda.md#all-triggers
        // Function arn format - arn:aws:lambda:<region>:<account-id>:function:<function-name>

        var items = functionArn!.Split(':');
        return items.Length >= 5 ? items[4] : null;
    }

    private static string? GetFaasId(string? functionArn)
    {
        if (string.IsNullOrEmpty(functionArn))
        {
            return null;
        }

        var faasId = functionArn;

        // According to faas.id description https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/trace/semantic_conventions/instrumentation/aws-lambda.md#all-triggers
        // the 8th part of arn (function version or alias, see https://docs.aws.amazon.com/lambda/latest/dg/lambda-api-permissions-ref.html)
        // should not be included into faas.id
        var items = functionArn!.Split(':');
        if (items.Length >= 8)
        {
            faasId = string.Join(":", items.Take(7));
        }

        return faasId;
    }

    private static string GetFaasTrigger<TInput>(TInput input) =>
        IsHttpRequest(input) ? "http" : "other";

    private static bool IsHttpRequest<TInput>(TInput input) =>
        input is APIGatewayProxyRequest or APIGatewayHttpApiV2ProxyRequest or ApplicationLoadBalancerRequest;

    private static ActivityContext ParseXRayTraceHeader(string rawHeader)
    {
        var xrayPropagator = new AWSXRayPropagator();

        var carrier = new Dictionary<string, string>()
        {
            { AWSXRayTraceHeaderKey, rawHeader },
        };

        var propagationContext = xrayPropagator.Extract(default, carrier, Getter);
        return propagationContext.ActivityContext;
    }
}
