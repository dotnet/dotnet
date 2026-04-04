// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Xunit;

namespace OpenTelemetry.Instrumentation.AWSLambda.Tests;

[Collection("TracerProviderDependent")]
public class AWSLambdaWrapperTests : IDisposable
{
    private const string TraceId = "5759e988bd862e3fe1be46a994272793";
    private const string XRayParentId = "53995c3f42cd8ad8";
    private const string CustomParentId = "11195c3f42cd8222";

    private readonly SampleHandlers sampleHandlers;
    private readonly SampleLambdaContext sampleLambdaContext;
    private readonly IDisposable environmentScope;

    public AWSLambdaWrapperTests()
    {
        this.sampleHandlers = new SampleHandlers();
        this.sampleLambdaContext = new SampleLambdaContext();
        this.environmentScope = EnvironmentVariableScope.Create(
            ("_X_AMZN_TRACE_ID", $"Root=1-5759e988-bd862e3fe1be46a994272793;Parent={XRayParentId};Sampled=1"),
            ("AWS_REGION", "us-east-1"),
            ("AWS_LAMBDA_FUNCTION_NAME", "testfunction"),
            ("AWS_LAMBDA_FUNCTION_VERSION", "latest"),
            ("AWS_LAMBDA_FUNCTION_MEMORY_SIZE", "128"),
            ("AWS_LAMBDA_LOG_STREAM_NAME", "2025/07/21/[$LATEST]7b176c212e954e62adfb9b5451cb5374"));
    }

    public void Dispose()
    {
        this.environmentScope.Dispose();

        // reset Semantic Convention to default
        Sdk.CreateTracerProviderBuilder()
            .AddAWSLambdaConfigurations();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void TraceSyncWithInputAndReturn(bool setCustomParent)
    {
        var exportedItems = new List<Activity>();

        using (var tracerProvider = Sdk.CreateTracerProviderBuilder()
                   .AddAWSLambdaConfigurations(opt =>
                   {
                       opt.SemanticConventionVersion = SemanticConventionVersion.Latest;
                   })
                   .AddInMemoryExporter(exportedItems)
                   .Build()!)
        {
            var parentContext = setCustomParent ? CreateParentContext() : default;
            var result = AWSLambdaWrapper.Trace(tracerProvider, this.sampleHandlers.SampleHandlerSyncInputAndReturn, "TestStream", this.sampleLambdaContext, parentContext);
            var resource = tracerProvider.GetResource();
            this.AssertResourceAttributes(resource);
        }

        Assert.Single(exportedItems);

        var activity = exportedItems[0];
        this.AssertSpanProperties(activity, setCustomParent ? CustomParentId : XRayParentId);
        this.AssertSpanAttributes(activity);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void TraceSyncWithInputAndNoReturn(bool setCustomParent)
    {
        var exportedItems = new List<Activity>();

        using (var tracerProvider = Sdk.CreateTracerProviderBuilder()
                   .AddAWSLambdaConfigurations(opt =>
                   {
                       opt.SemanticConventionVersion = SemanticConventionVersion.Latest;
                   })
                   .AddInMemoryExporter(exportedItems)
                   .Build()!)
        {
            var parentContext = setCustomParent ? CreateParentContext() : default;
            AWSLambdaWrapper.Trace(tracerProvider, this.sampleHandlers.SampleHandlerSyncInputAndNoReturn, "TestStream", this.sampleLambdaContext, parentContext);
            var resource = tracerProvider.GetResource();
            this.AssertResourceAttributes(resource);
        }

        Assert.Single(exportedItems);

        var activity = exportedItems[0];
        this.AssertSpanProperties(activity, setCustomParent ? CustomParentId : XRayParentId);
        this.AssertSpanAttributes(activity);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task TraceAsyncWithInputAndReturn(bool setCustomParent)
    {
        var exportedItems = new List<Activity>();

        using (var tracerProvider = Sdk.CreateTracerProviderBuilder()
                   .AddAWSLambdaConfigurations(opt =>
                       {
                           opt.SemanticConventionVersion = SemanticConventionVersion.Latest;
                       })
                   .AddInMemoryExporter(exportedItems)
                   .Build()!)
        {
            var parentContext = setCustomParent ? CreateParentContext() : default;
            var result = await AWSLambdaWrapper.TraceAsync(tracerProvider, this.sampleHandlers.SampleHandlerAsyncInputAndReturn, "TestStream", this.sampleLambdaContext, parentContext);
            var resource = tracerProvider.GetResource();
            this.AssertResourceAttributes(resource);
        }

        Assert.Single(exportedItems);

        var activity = exportedItems[0];
        this.AssertSpanProperties(activity, setCustomParent ? CustomParentId : XRayParentId);
        this.AssertSpanAttributes(activity);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task TraceAsyncWithInputAndNoReturn(bool setCustomParent)
    {
        var exportedItems = new List<Activity>();

        using (var tracerProvider = Sdk.CreateTracerProviderBuilder()
                   .AddAWSLambdaConfigurations(opt =>
                   {
                       opt.SemanticConventionVersion = SemanticConventionVersion.Latest;
                   })
                   .AddInMemoryExporter(exportedItems)
                   .Build()!)
        {
            var parentContext = setCustomParent ? CreateParentContext() : default;
            await AWSLambdaWrapper.TraceAsync(tracerProvider, this.sampleHandlers.SampleHandlerAsyncInputAndNoReturn, "TestStream", this.sampleLambdaContext, parentContext);
            var resource = tracerProvider.GetResource();
            this.AssertResourceAttributes(resource);
        }

        Assert.Single(exportedItems);

        var activity = exportedItems[0];
        this.AssertSpanProperties(activity, setCustomParent ? CustomParentId : XRayParentId);
        this.AssertSpanAttributes(activity);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void TestLambdaHandlerException(bool setCustomParent)
    {
        var exportedItems = new List<Activity>();

        using (var tracerProvider = Sdk.CreateTracerProviderBuilder()
                   .AddAWSLambdaConfigurations(opt =>
                   {
                       opt.SemanticConventionVersion = SemanticConventionVersion.Latest;
                   })
                   .AddInMemoryExporter(exportedItems)
                   .Build()!)
        {
            try
            {
                var parentContext = setCustomParent ? CreateParentContext() : default;
                AWSLambdaWrapper.Trace(tracerProvider, this.sampleHandlers.SampleHandlerSyncNoReturnException, "TestException", this.sampleLambdaContext, parentContext);
            }
            catch
            {
                var resource = tracerProvider.GetResource();
                this.AssertResourceAttributes(resource);
            }
        }

        Assert.Single(exportedItems);

        var activity = exportedItems[0];
        this.AssertSpanProperties(activity, setCustomParent ? CustomParentId : XRayParentId);
        this.AssertSpanAttributes(activity);
        this.AssertSpanException(activity);
    }

    [Fact]
    public void TestLambdaHandlerNotSampled()
    {
        using (EnvironmentVariableScope.Create("_X_AMZN_TRACE_ID", "Root=1-5759e988-bd862e3fe1be46a994272793;Parent=53995c3f42cd8ad8;Sampled=0"))
        {
            var exportedItems = new List<Activity>();

            using (var tracerProvider = Sdk.CreateTracerProviderBuilder()
                       .AddAWSLambdaConfigurations(opt =>
                       {
                           opt.SemanticConventionVersion = SemanticConventionVersion.Latest;
                       })
                       .AddInMemoryExporter(exportedItems)
                       .Build()!)
            {
                AWSLambdaWrapper.Trace(tracerProvider, this.sampleHandlers.SampleHandlerSyncInputAndReturn, "TestStream", this.sampleLambdaContext);
                var resource = tracerProvider.GetResource();
                this.AssertResourceAttributes(resource);
            }

            Assert.Empty(exportedItems);
        }
    }

    [Fact]
    public void OnFunctionStart_NoParent_ActivityCreated()
    {
        using (EnvironmentVariableScope.Create("_X_AMZN_TRACE_ID", null))
        {
            Activity? activity = null;
            using (Sdk.CreateTracerProviderBuilder()
                      .AddAWSLambdaConfigurations(opt =>
                      {
                          opt.SemanticConventionVersion = SemanticConventionVersion.Latest;
                      })
                      .Build())
            {
                activity = AWSLambdaWrapper.OnFunctionStart("test-input", new SampleLambdaContext());
            }

            Assert.NotNull(activity);
        }
    }

    [Fact]
    public void OnFunctionStart_NoSampledAndAwsXRayContextExtractionDisabled_ActivityCreated()
    {
        using (EnvironmentVariableScope.Create("_X_AMZN_TRACE_ID", $"Root=1-5759e988-bd862e3fe1be46a994272793;Parent={XRayParentId};Sampled=0"))
        {
            Activity? activity = null;

            using (Sdk.CreateTracerProviderBuilder()
                      .AddAWSLambdaConfigurations(c => c.DisableAwsXRayContextExtraction = true)
                      .Build())
            {
                activity = AWSLambdaWrapper.OnFunctionStart("test-input", new SampleLambdaContext());
            }

            Assert.NotNull(activity);
        }
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void OnFunctionStart_ColdStart_ColdStartTagHasCorrectValue(int invocationsCount)
    {
        AWSLambdaWrapper.ResetColdStart();
        Activity? activity = null;

        using (Sdk.CreateTracerProviderBuilder()
                  .AddAWSLambdaConfigurations(c => c.DisableAwsXRayContextExtraction = true)
                  .Build())
        {
            for (var i = 1; i <= invocationsCount; i++)
            {
                activity = AWSLambdaWrapper.OnFunctionStart("test-input", new SampleLambdaContext());
            }
        }

        Assert.NotNull(activity);
        Assert.NotNull(activity.TagObjects);
        var expectedColdStartValue = invocationsCount == 1;
        Assert.Contains(activity.TagObjects, x => x.Key == ExpectedSemanticConventions.AttributeFaasColdStart && expectedColdStartValue.Equals(x.Value));
    }

    [Fact]
    public async Task TraceAsyncDoesNotCrashForEmptyLambdaContext()
    {
        var emptyLambdaContext = new SampleLambdaContext
        {
            AwsRequestId = null!,
            ClientContext = null,
            FunctionName = null!,
            FunctionVersion = null!,
            Identity = null!,
            InvokedFunctionArn = null!,
            Logger = null!,
            LogGroupName = null!,
            LogStreamName = null!,
            MemoryLimitInMB = 0,
            RemainingTime = TimeSpan.Zero,
        };

        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
                   .AddAWSLambdaConfigurations(opt =>
                   {
                       opt.SemanticConventionVersion = SemanticConventionVersion.Latest;
                   })
                   .Build();

        // We simply verify that no exception is thrown here.
        await AWSLambdaWrapper.TraceAsync(tracerProvider, this.sampleHandlers.SampleHandlerAsyncInputAndNoReturn, "TestStream", emptyLambdaContext);
    }

    private static ActivityContext CreateParentContext()
    {
        var traceId = ActivityTraceId.CreateFromString(TraceId.AsSpan());
        var parentId = ActivitySpanId.CreateFromString(CustomParentId.AsSpan());
        return new ActivityContext(traceId, parentId, ActivityTraceFlags.Recorded);
    }

    private void AssertSpanProperties(Activity activity, string parentId)
    {
        Assert.Equal(TraceId, activity.TraceId.ToHexString());
        Assert.Equal(parentId, activity.ParentSpanId.ToHexString());
        Assert.Equal(ActivityTraceFlags.Recorded, activity.ActivityTraceFlags);
        Assert.Equal(ActivityKind.Server, activity.Kind);
        Assert.Equal("testfunction", activity.DisplayName);
        Assert.Equal("OpenTelemetry.Instrumentation.AWSLambda", activity.Source.Name);

        // Version should consist of 3 decimals separated by dots followed by optional pre-release suffix
        Assert.Matches(@"^\d+(\.\d+){2}(-.+)?$", activity.Source.Version);
    }

    private void AssertResourceAttributes(Resource? resource)
    {
        Assert.NotNull(resource);

        var resourceAttributes = resource.Attributes.ToDictionary(x => x.Key, x => x.Value);
        Assert.Equal("aws", resourceAttributes[ExpectedSemanticConventions.AttributeCloudProvider]);
        Assert.Equal("us-east-1", resourceAttributes[ExpectedSemanticConventions.AttributeCloudRegion]);
        Assert.Equal("testfunction", resourceAttributes[ExpectedSemanticConventions.AttributeFaasName]);
        Assert.Equal("latest", resourceAttributes[ExpectedSemanticConventions.AttributeFaasVersion]);
        Assert.Equal("2025/07/21/[$LATEST]7b176c212e954e62adfb9b5451cb5374", resourceAttributes[ExpectedSemanticConventions.AttributeFaasInstance]);
        Assert.Equal(134217728L, resourceAttributes[ExpectedSemanticConventions.AttributeFaasMaxMemory]);
    }

    private void AssertSpanAttributes(Activity activity)
    {
        Assert.Equal(this.sampleLambdaContext.AwsRequestId, activity.GetTagValue(ExpectedSemanticConventions.AttributeFaasExecution));
        Assert.Equal(this.sampleLambdaContext.InvokedFunctionArn, activity.GetTagValue(ExpectedSemanticConventions.AttributeFaasID));
        Assert.Equal(this.sampleLambdaContext.FunctionName, activity.GetTagValue(ExpectedSemanticConventions.AttributeFaasName));
        Assert.Equal("other", activity.GetTagValue(ExpectedSemanticConventions.AttributeFaasTrigger));
        Assert.Equal("111111111111", activity.GetTagValue(ExpectedSemanticConventions.AttributeCloudAccountID));
        Assert.Equal(this.sampleLambdaContext.LogStreamName, activity.GetTagValue(ExpectedSemanticConventions.AttributeFaasInstance));
        Assert.Equal(this.sampleLambdaContext.MemoryLimitInMB * 1024 * 1024, activity.GetTagValue(ExpectedSemanticConventions.AttributeFaasMaxMemory));
    }

    private void AssertSpanException(Activity activity)
    {
        Assert.Equal(ActivityStatusCode.Error, activity.Status);
        Assert.Equal("TestException", activity.StatusDescription);
        var exception = Assert.Single(activity.Events);
        Assert.Equal("exception", exception.Name);
        Assert.Equal("TestException", exception.Tags.SingleOrDefault(t => t.Key.Equals("exception.message")).Value);
    }

    private static class ExpectedSemanticConventions
    {
        public const string AttributeCloudProvider = "cloud.provider";
        public const string AttributeCloudAccountID = "cloud.account.id";
        public const string AttributeCloudRegion = "cloud.region";
        public const string AttributeFaasColdStart = "faas.coldstart";
        public const string AttributeFaasName = "faas.name";
        public const string AttributeFaasExecution = "faas.invocation_id";
        public const string AttributeFaasID = "cloud.resource_id";
        public const string AttributeFaasTrigger = "faas.trigger";
        public const string AttributeFaasVersion = "faas.version";
        public const string AttributeFaasInstance = "faas.instance";
        public const string AttributeFaasMaxMemory = "faas.max_memory";
    }
}
