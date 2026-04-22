# AWS OTel .NET SDK for Lambda

| Status | |
| ------ | --- |
| Stability | [Stable](../../README.md#stable) |
| Code Owners | [@rypdal](https://github.com/rypdal), [@Oberon00](https://github.com/Oberon00), [@normj](https://github.com/normj), [@lukeina2z](https://github.com/lukeina2z) |

[![NuGet version badge](https://img.shields.io/nuget/v/OpenTelemetry.Instrumentation.AWSLambda)](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.AWSLambda)
[![NuGet download count badge](https://img.shields.io/nuget/dt/OpenTelemetry.Instrumentation.AWSLambda)](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.AWSLambda)
[![codecov.io](https://codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib/branch/main/graphs/badge.svg?flag=unittests-Instrumentation.AWSLambda)](https://app.codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib?flags[0]=unittests-Instrumentation.AWSLambda)

This repo contains SDK to instrument Lambda handler to create incoming span.

## Installation

```shell
dotnet add package OpenTelemetry.Instrumentation.AWSLambda
```

## Configuration

Add `AddAWSLambdaConfigurations()` to `TracerProvider`.

```csharp
TracerProvider tracerProvider = Sdk.CreateTracerProviderBuilder()
        // add other instrumentations
        .AddAWSLambdaConfigurations(options => options.DisableAwsXRayContextExtraction = true)
        .Build();
```

### AWSLambdaInstrumentationOptions

`AWSLambdaInstrumentationOptions` contains various properties to configure
AWS lambda instrumentation:

* [`DisableAwsXRayContextExtraction`](/src/OpenTelemetry.Instrumentation.AWSLambda/AWSLambdaInstrumentationOptions.cs)
* [`SetParentFromBatch`](/src/OpenTelemetry.Instrumentation.AWSLambda/AWSLambdaInstrumentationOptions.cs)

## Instrumentation

`AWSLambdaWrapper` contains tracing methods covering different types of
function handler method signatures. `AWSLambdaWrapper.Trace()` and
`AWSLambdaWrapper.TraceAsync()` are used for wrapping synchronous
and asynchronous function handlers respectively. The `ActivityContext parentContext`
parameter is optional and used to pass a custom parent context. If the parent
is not passed explicitly then it's either extracted from the
input parameter or uses AWS X-Ray headers if AWS X-Ray context extraction is
enabled (see configuration property `DisableAwsXRayContextExtraction`).

> [!NOTE]
> If tracing is required when AWS X-Ray is disabled,
> the `DisableAwsXRayContextExtraction` property needs to be set to `true`.
> This will instruct the instrumentation to ignore the "not sampled"
> trace header automatically sent when AWS X-Ray is disabled.

The sequence of the parent extraction:
`explicit parent` -> `parent from input parameter` -> `AWS X-Ray headers` -> `default`
The parent extraction is supported for the input types listed in the table below:

| Type | Parent extraction source |
| ---- | ------------------------ |
| `APIGatewayProxyRequest, APIGatewayHttpApiV2ProxyRequest`, `ApplicationLoadBalancerRequest` | HTTP headers of the request |
| `SQSEvent` | Attributes of the last `SQSMessage` (if `SetParentFromMessageBatch` is `true`) |
| `SNSEvent` | Attributes of the last `SNSRecord` |

### Lambda Function

1. Create a wrapper function with the same signature as the original Lambda
function but an added ILambdaContext parameter if it was not already present.
Call `AWSLambdaWrapper.Trace()` or `AWSLambdaWrapper.TraceAsync()` API and pass
`TracerProvider`, original Lambda function and its parameters.

2. Set the wrapper function as the Lambda handler input.

```csharp
// new Lambda function handler passed in
public string TracingFunctionHandler(JObject input, ILambdaContext context)
=> AWSLambdaWrapper.Trace(tracerProvider, OriginalFunctionHandler, input, context);

public string OriginalFunctionHandler(JObject input, ILambdaContext context)
{
    var S3Client = new AmazonS3Client();
    var httpClient = new HttpClient();
    _ = S3Client.ListBucketsAsync().Result;
    _ = httpClient.GetAsync("https://aws.amazon.com").Result;
    return input?.ToString();
}
```

### Lambda Function - Asp.Net Core

For using base classes from package [Amazon.Lambda.AspNetCoreServer](https://github.com/aws/aws-lambda-dotnet/tree/master/Libraries/src/Amazon.Lambda.AspNetCoreServer#amazonlambdaaspnetcoreserver),
override the `FunctionHandlerAsync` function in `LambdaEntryPoint.cs` file. Call
`AWSLambdaWrapper.TraceAsync()` API and pass `TracerProvider`, original Lambda function
and its inputs as parameters. Below is an example if using `APIGatewayProxyFunction`
as base class.

```csharp
public override async Task<APIGatewayProxyResponse> FunctionHandlerAsync(
    APIGatewayProxyRequest request, ILambdaContext lambdaContext)
=> await AWSLambdaWrapper.TraceAsync(tracerProvider, base.FunctionHandlerAsync,
    request, lambdaContext);
```

## Example

A more detailed Lambda function example that takes a JObject and ILambdaContext
as inputs.

```csharp
public class Function
{
    public static TracerProvider tracerProvider;

    static Function()
    {
        tracerProvider = Sdk.CreateTracerProviderBuilder()
                .AddHttpClientInstrumentation()
                .AddAWSInstrumentation()
                .AddOtlpExporter()
                .AddAWSLambdaConfigurations(options => options.DisableAwsXRayContextExtraction = true)
                .Build();
    }

    public string TracingFunctionHandler(JObject input, ILambdaContext context)
    => AWSLambdaWrapper.Trace(tracerProvider, FunctionHandler, input, context);

    /// <summary>
    /// A simple function that takes a JObject input and sends downstream http
    /// request to aws.amazon.com and aws request to S3.
    /// </summary>
    public string FunctionHandler(JObject input, ILambdaContext context)
    {
        var S3Client = new AmazonS3Client();
        var httpClient = new HttpClient();
        _ = S3Client.ListBucketsAsync().Result;
        _ = httpClient.GetAsync("https://aws.amazon.com").Result;
        return input?.ToString();
    }
}
```

## Semantic Conventions

_For an overview on Semantic Conventions, see
[Open Telemetery - Semantic Conventions](https://opentelemetry.io/docs/concepts/semantic-conventions/)_.

While this library is intended for production use, it relies on several
Semantic Conventions that are still considered Experimental, meaning
they may undergo additional changes before becoming Stable.  This can impact
the aggregation and analysis of telemetry signals in environments with
multiple applications or microservices.

For example, a microservice using an older version of the Semantic Conventions
for Http Attributes may emit `"http.method"` with a value of GET, while a
different microservice, using a new version of Semantic Convention may instead
emit the GET as `"http.request.method"`.

Future versions of OpenTelemetry.Instrumentation.AWSLambda library will include
updates to the Semantic Convention, which may break compatibility with a
previous version.

The default will remain as `V1_28_0` until the next major version bump.

To opt in to automatic upgrades, you can use `SemanticConventionVersion.Latest`
or you can specify a specific version:

```csharp
 using (var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddAWSLambdaConfigurations(opt =>
    {
        // pin to a specific Semantic Convention version
        opt.SemanticConventionVersion = SemanticConventionVersion.V1_29_0;
    })
    .Build()!);
```

**NOTE:** Once a Semantic Convention becomes Stable,
OpenTelemetry.Instrumentation.AWSLambda will remain on that version until
the next major version bump.

## Reference

* [OpenTelemetry Project](https://opentelemetry.io/)
* [OpenTelemetry Lambda](https://github.com/open-telemetry/opentelemetry-lambda)
* [AWS OpenTelemetry Lambda](https://github.com/aws-observability/aws-otel-lambda)
