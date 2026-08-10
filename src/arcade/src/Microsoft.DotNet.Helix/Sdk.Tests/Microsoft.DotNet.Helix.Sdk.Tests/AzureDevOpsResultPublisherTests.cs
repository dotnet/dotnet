// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using Microsoft.DotNet.Helix.AzureDevOpsTestPublisher;
using Microsoft.DotNet.Helix.AzureDevOpsTestPublisher.Model;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Microsoft.DotNet.Helix.Sdk.Tests
{
    public class AzureDevOpsResultPublisherTests
    {
        [Fact]
        public void Constructor_ConfiguresHttpClientTimeoutForLongUploads()
        {
            using var publisher = new AzureDevOpsResultPublisher(
                new AzureDevOpsReportingParameters(
                    new Uri("https://dev.azure.com/dnceng-public/"),
                    "public",
                    "123",
                    "token"),
                NullLogger.Instance);

            FieldInfo field = typeof(AzureDevOpsResultPublisher).GetField("_httpClient", BindingFlags.Instance | BindingFlags.NonPublic);
            var client = Assert.IsType<HttpClient>(field.GetValue(publisher));

            Assert.Equal(TimeSpan.FromMinutes(5), client.Timeout);
        }

        [Theory]
        [InlineData("Passed", true)]
        [InlineData("NotExecuted", true)]
        [InlineData("Inconclusive", true)]
        [InlineData("Failed", false)]
        [InlineData("None", false)]
        public void ComputeAllPassed_SingleResult_OnlyFailedAndNoneCountAsFailure(string result, bool expectedAllPassed)
        {
            var results = new[] { new AggregatedResult(AggregationType.Single, "Test1", 1, result) };

            Assert.Equal(expectedAllPassed, AzureDevOpsResultPublisher.ComputeAllPassed(results));
        }

        [Fact]
        public void ComputeAllPassed_InconclusiveDataDrivenRollup_DoesNotFailTheWorkItem()
        {
            // Mirrors the rollup the aggregator produces for a theory with some passing and some
            // skipped data rows: no data row failed, but the mix isn't a clean pass or skip either.
            var results = new[]
            {
                new AggregatedResult(AggregationType.Single, "Test1", 1, "Passed"),
                new AggregatedResult(AggregationType.DataDriven, "Test2", 1, "Inconclusive"),
            };

            Assert.True(AzureDevOpsResultPublisher.ComputeAllPassed(results));
        }

        [Fact]
        public void ComputeAllPassed_AnyFailedResult_FailsTheWorkItem()
        {
            var results = new[]
            {
                new AggregatedResult(AggregationType.Single, "Test1", 1, "Passed"),
                new AggregatedResult(AggregationType.DataDriven, "Test2", 1, "Failed"),
            };

            Assert.False(AzureDevOpsResultPublisher.ComputeAllPassed(results));
        }

        [Fact]
        public void HttpClientTimeoutIsTransient()
        {
            Assert.True(AzureDevOpsResultPublisher.IsTransientException(
                new OperationCanceledException("The request timed out.", new TimeoutException()),
                CancellationToken.None));
        }

        [Fact]
        public void CallerCancellationIsNotTransient()
        {
            using var cancellation = new CancellationTokenSource();
            cancellation.Cancel();

            Assert.False(AzureDevOpsResultPublisher.IsTransientException(
                new OperationCanceledException("The request timed out.", new TimeoutException()),
                cancellation.Token));
        }

        [Fact]
        public void CancellationWithoutTimeoutIsNotTransient()
        {
            Assert.False(AzureDevOpsResultPublisher.IsTransientException(
                new OperationCanceledException(),
                CancellationToken.None));
        }

    }
}
