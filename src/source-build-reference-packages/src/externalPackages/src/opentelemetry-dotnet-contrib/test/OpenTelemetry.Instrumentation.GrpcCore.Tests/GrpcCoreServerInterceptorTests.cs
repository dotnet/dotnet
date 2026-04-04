// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Grpc.Core;
using OpenTelemetry.Context.Propagation;
using Xunit;

namespace OpenTelemetry.Instrumentation.GrpcCore.Tests;

/// <summary>
/// Grpc Core server interceptor tests.
/// </summary>
public class GrpcCoreServerInterceptorTests
{
    /// <summary>
    /// Validates a successful UnaryServerHandler call.
    /// </summary>
    /// <returns>A task.</returns>
    [Fact]
    public async Task UnaryServerHandlerSuccess()
    {
        await TestHandlerSuccess(FoobarService.MakeUnaryAsyncRequest);
    }

    /// <summary>
    /// Validates a failed UnaryServerHandler call.
    /// </summary>
    /// <returns>A task.</returns>
    [Fact]
    public async Task UnaryServerHandlerFail()
    {
        await TestHandlerFailure(FoobarService.MakeUnaryAsyncRequest);
    }

    /// <summary>
    /// Validates a successful ClientStreamingServerHandler call.
    /// </summary>
    /// <returns>A task.</returns>
    [Fact]
    public async Task ClientStreamingServerHandlerSuccess()
    {
        await TestHandlerSuccess(FoobarService.MakeClientStreamingRequest);
    }

    /// <summary>
    /// Validates a failed ClientStreamingServerHandler call.
    /// </summary>
    /// <returns>A task.</returns>
    [Fact]
    public async Task ClientStreamingServerHandlerFail()
    {
        await TestHandlerFailure(FoobarService.MakeClientStreamingRequest);
    }

    /// <summary>
    /// Validates a successful ServerStreamingServerHandler call.
    /// </summary>
    /// <returns>A task.</returns>
    [Fact]
    public async Task ServerStreamingServerHandlerSuccess()
    {
        await TestHandlerSuccess(FoobarService.MakeServerStreamingRequest);
    }

    /// <summary>
    /// Validates a failed ServerStreamingServerHandler call.
    /// </summary>
    /// <returns>A task.</returns>
    [Fact]
    public async Task ServerStreamingServerHandlerFail()
    {
        await TestHandlerFailure(FoobarService.MakeServerStreamingRequest);
    }

    /// <summary>
    /// Validates a successful DuplexStreamingServerHandler call.
    /// </summary>
    /// <returns>A task.</returns>
    [Fact]
    public async Task DuplexStreamingServerHandlerSuccess()
    {
        await TestHandlerSuccess(FoobarService.MakeDuplexStreamingRequest);
    }

    /// <summary>
    /// Validates a failed DuplexStreamingServerHandler call.
    /// </summary>
    /// <returns>A task.</returns>
    [Fact]
    public async Task DuplexStreamingServerHandlerFail()
    {
        await TestHandlerFailure(FoobarService.MakeDuplexStreamingRequest);
    }

    /// <summary>
    /// A common method to test server interceptor handler success.
    /// </summary>
    /// <param name="clientRequestFunc">The specific client request function.</param>
    /// <param name="additionalMetadata">The additional metadata, if any.</param>
    /// <returns>A Task.</returns>
    private static async Task TestHandlerSuccess(Func<Foobar.FoobarClient, Metadata?, Task> clientRequestFunc, Metadata? additionalMetadata = null)
    {
        // starts the server with the server interceptor
        var testTags = new TestActivityTags();
        var interceptorOptions = new ServerTracingInterceptorOptions { Propagator = new TraceContextPropagator(), RecordMessageEvents = true, AdditionalTags = testTags.Tags };
        using var server = FoobarService.Start(new ServerTracingInterceptor(interceptorOptions));

        // No parent Activity, no context from header
        using (var activityListener = new InterceptorActivityListener(testTags))
        {
            var client = FoobarService.ConstructRpcClient(server.UriString);
            await clientRequestFunc(client, additionalMetadata);

            var activity = activityListener.Activity;
            GrpcCoreClientInterceptorTests.ValidateCommonActivityTags(activity, StatusCode.OK, interceptorOptions.RecordMessageEvents);
            Assert.NotNull(activity);
            Assert.Equal(default, activity.ParentSpanId);
        }

        // No parent Activity, context from header
        using (var activityListener = new InterceptorActivityListener(testTags))
        {
            var client = FoobarService.ConstructRpcClient(
                server.UriString,
                additionalMetadata: [new Metadata.Entry("traceparent", FoobarService.DefaultTraceparentWithSampling)]);

            await clientRequestFunc(client, additionalMetadata).ConfigureAwait(false);

            var activity = activityListener.Activity;
            GrpcCoreClientInterceptorTests.ValidateCommonActivityTags(activity, StatusCode.OK, interceptorOptions.RecordMessageEvents);
            Assert.NotNull(activity);
            Assert.Equal(FoobarService.DefaultParentFromTraceparentHeader.SpanId, activity.ParentSpanId);
        }
    }

    /// <summary>
    /// A common method to test server interceptor handler failure.
    /// </summary>
    /// <param name="clientRequestFunc">The specific client request function.</param>
    /// <param name="additionalMetadata">The additional metadata, if any.</param>
    /// <returns>A Task.</returns>
    private static async Task TestHandlerFailure(Func<Foobar.FoobarClient, Metadata?, Task> clientRequestFunc, Metadata? additionalMetadata = null)
    {
        // starts the server with the server interceptor
        var testTags = new TestActivityTags();
        var interceptorOptions = new ServerTracingInterceptorOptions { Propagator = new TraceContextPropagator(), AdditionalTags = testTags.Tags, RecordException = true };
        using var server = FoobarService.Start(new ServerTracingInterceptor(interceptorOptions));

        using var activityListener = new InterceptorActivityListener(testTags);
        var client = FoobarService.ConstructRpcClient(
            server.UriString,
            additionalMetadata:
            [
                new Metadata.Entry("traceparent", FoobarService.DefaultTraceparentWithSampling),
                new Metadata.Entry(FoobarService.RequestHeaderFailWithStatusCode, StatusCode.ResourceExhausted.ToString()),
                new Metadata.Entry(FoobarService.RequestHeaderErrorDescription, "fubar"),
            ]);

        await Assert.ThrowsAsync<RpcException>(async () => await clientRequestFunc(client, additionalMetadata).ConfigureAwait(false));

        var activity = activityListener.Activity;
        GrpcCoreClientInterceptorTests.ValidateCommonActivityTags(activity, StatusCode.ResourceExhausted, interceptorOptions.RecordMessageEvents, interceptorOptions.RecordException);
        Assert.NotNull(activity);
        Assert.Equal(FoobarService.DefaultParentFromTraceparentHeader.SpanId, activity.ParentSpanId);
    }
}
