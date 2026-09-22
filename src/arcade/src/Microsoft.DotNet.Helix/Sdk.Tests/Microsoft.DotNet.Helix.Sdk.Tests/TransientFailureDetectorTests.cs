// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Azure;
using Azure.Core;
using Azure.Core.Pipeline;
using Microsoft.DotNet.Helix.Client;
using Microsoft.DotNet.Helix.JobMonitor;
using Moq;
using Xunit;

namespace Microsoft.DotNet.Helix.Sdk.Tests;

public class TransientFailureDetectorTests
{
    [Theory]
    [InlineData(400, false)]
    [InlineData(408, true)]
    [InlineData(429, true)]
    [InlineData(500, true)]
    [InlineData(503, true)]
    public void ClassifiesRestApiExceptionByStatusCode(int statusCode, bool expected)
    {
        RestApiException exception = CreateRestApiException(statusCode);

        Assert.Equal(expected, TransientFailureDetector.IsTransient(exception));
    }

    private static RestApiException CreateRestApiException(int statusCode)
    {
        HttpPipeline pipeline = HttpPipelineBuilder.Build(new TestClientOptions());
        using Request request = pipeline.CreateRequest();
        request.Method = RequestMethod.Get;
        request.Uri.Reset(new Uri("https://helix.dot.net/api/jobs/test"));

        var response = new Mock<Response>();
        response.SetupGet(r => r.Status).Returns(statusCode);
        response.SetupGet(r => r.ReasonPhrase).Returns("Injected response");

        return new RestApiException(request, response.Object, "");
    }

    private sealed class TestClientOptions : ClientOptions
    {
    }
}
