// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if !NETFRAMEWORK

using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace OpenTelemetry.Resources.AWS.Tests;

public class AWSECSDetectorTests
{
    private const string AWSECSMetadataFilePath = "SampleMetadataFiles/testcgroup";
    private const string AWSECSMetadataURLKey = "ECS_CONTAINER_METADATA_URI";
    private const string AWSECSMetadataURLV4Key = "ECS_CONTAINER_METADATA_URI_V4";

    [Fact]
    public void TestNotOnEcs()
    {
        var ecsResourceDetector = new AWSECSDetector(
            new OpenTelemetry.AWS.AWSSemanticConventions(
                SemanticConventionVersion.Latest));

        var resourceAttributes = ecsResourceDetector.Detect();

        Assert.NotNull(resourceAttributes);
        Assert.Empty(resourceAttributes.Attributes);
    }

    [Fact]
    public void TestGetECSContainerId()
    {
        Assert.Equal("a4d00c9dd675d67f866c786181419e1b44832d4696780152e61afd44a3e02856", AWSECSDetector.GetECSContainerId(AWSECSMetadataFilePath));
    }

    [Fact]
    public void TestEcsMetadataV3()
    {
        using (EnvironmentVariableScope.Create(AWSECSMetadataURLKey, "TestECSURIKey"))
        {
            var ecsResourceDetector = new AWSECSDetector(
                new OpenTelemetry.AWS.AWSSemanticConventions(
                    SemanticConventionVersion.Latest));

            var resourceAttributes = ecsResourceDetector.Detect().Attributes.ToDictionary(x => x.Key, x => x.Value);

            Assert.Equal(resourceAttributes[ExpectedSemanticConventions.AttributeCloudProvider], "aws");
            Assert.Equal(resourceAttributes[ExpectedSemanticConventions.AttributeCloudPlatform], "aws_ecs");
        }
    }

    [Fact]
    public async Task TestEcsMetadataV4Ec2()
    {
        var source = new CancellationTokenSource();
        var token = source.Token;

        await using (var metadataEndpoint = new MockEcsMetadataEndpoint("ecs_metadata/metadatav4-response-container-ec2.json", "ecs_metadata/metadatav4-response-task-ec2.json"))
        {
            using (EnvironmentVariableScope.Create(AWSECSMetadataURLV4Key, metadataEndpoint.Address.ToString()))
            {
                var ecsResourceDetector = new AWSECSDetector(
                    new OpenTelemetry.AWS.AWSSemanticConventions(
                        SemanticConventionVersion.Latest));

                var resourceAttributes = ecsResourceDetector.Detect().Attributes.ToDictionary(x => x.Key, x => x.Value);

                Assert.Equal(resourceAttributes[ExpectedSemanticConventions.AttributeCloudProvider], "aws");
                Assert.Equal(resourceAttributes[ExpectedSemanticConventions.AttributeCloudPlatform], "aws_ecs");
                Assert.Equal(resourceAttributes[ExpectedSemanticConventions.AttributeCloudAccountID], "111122223333");
                Assert.Equal(resourceAttributes[ExpectedSemanticConventions.AttributeCloudAvailabilityZone], "us-west-2d");
                Assert.Equal(resourceAttributes[ExpectedSemanticConventions.AttributeCloudRegion], "us-west-2");
                Assert.Equal(resourceAttributes[ExpectedSemanticConventions.AttributeCloudResourceId], "arn:aws:ecs:us-west-2:111122223333:container/0206b271-b33f-47ab-86c6-a0ba208a70a9");
                Assert.Equal(resourceAttributes[ExpectedSemanticConventions.AttributeEcsContainerArn], "arn:aws:ecs:us-west-2:111122223333:container/0206b271-b33f-47ab-86c6-a0ba208a70a9");
                Assert.Equal(resourceAttributes[ExpectedSemanticConventions.AttributeEcsLaunchtype], "ec2");
                Assert.Equal(resourceAttributes[ExpectedSemanticConventions.AttributeEcsTaskArn], "arn:aws:ecs:us-west-2:111122223333:task/default/158d1c8083dd49d6b527399fd6414f5c");
                Assert.Equal(resourceAttributes[ExpectedSemanticConventions.AttributeEcsTaskFamily], "curltest");
                Assert.Equal(resourceAttributes[ExpectedSemanticConventions.AttributeEcsTaskRevision], "26");
#pragma warning disable CA1861 // Avoid constant arrays as arguments
                Assert.NotStrictEqual(resourceAttributes[ExpectedSemanticConventions.AttributeLogGroupNames], new string[] { "/ecs/metadata" });
                Assert.NotStrictEqual(resourceAttributes[ExpectedSemanticConventions.AttributeLogGroupArns], new string[] { "arn:aws:logs:us-west-2:111122223333:log-group:/ecs/metadata" });
                Assert.NotStrictEqual(resourceAttributes[ExpectedSemanticConventions.AttributeLogStreamNames], new string[] { "ecs/curl/8f03e41243824aea923aca126495f665" });
                Assert.NotStrictEqual(resourceAttributes[ExpectedSemanticConventions.AttributeLogStreamArns], new string[] { "arn:aws:logs:us-west-2:111122223333:log-group:/ecs/metadata:log-stream:ecs/curl/8f03e41243824aea923aca126495f665" });
#pragma warning restore CA1861 // Avoid constant arrays as arguments
            }
        }
    }

    [Fact]
    public async Task TestEcsMetadataV4Fargate()
    {
        var source = new CancellationTokenSource();
        var token = source.Token;

        await using (var metadataEndpoint = new MockEcsMetadataEndpoint("ecs_metadata/metadatav4-response-container-fargate.json", "ecs_metadata/metadatav4-response-task-fargate.json"))
        {
            using (EnvironmentVariableScope.Create(AWSECSMetadataURLV4Key, metadataEndpoint.Address.ToString()))
            {
                var ecsResourceDetector = new AWSECSDetector(
                    new OpenTelemetry.AWS.AWSSemanticConventions(
                        SemanticConventionVersion.Latest));

                var resourceAttributes = ecsResourceDetector.Detect().Attributes.ToDictionary(x => x.Key, x => x.Value);

                Assert.Equal(resourceAttributes[ExpectedSemanticConventions.AttributeCloudProvider], "aws");
                Assert.Equal(resourceAttributes[ExpectedSemanticConventions.AttributeCloudPlatform], "aws_ecs");
                Assert.Equal(resourceAttributes[ExpectedSemanticConventions.AttributeCloudAccountID], "111122223333");
                Assert.Equal(resourceAttributes[ExpectedSemanticConventions.AttributeCloudAvailabilityZone], "us-west-2a");
                Assert.Equal(resourceAttributes[ExpectedSemanticConventions.AttributeCloudRegion], "us-west-2");
                Assert.Equal(resourceAttributes[ExpectedSemanticConventions.AttributeCloudResourceId], "arn:aws:ecs:us-west-2:111122223333:container/05966557-f16c-49cb-9352-24b3a0dcd0e1");
                Assert.Equal(resourceAttributes[ExpectedSemanticConventions.AttributeEcsContainerArn], "arn:aws:ecs:us-west-2:111122223333:container/05966557-f16c-49cb-9352-24b3a0dcd0e1");
                Assert.Equal(resourceAttributes[ExpectedSemanticConventions.AttributeEcsLaunchtype], "fargate");
                Assert.Equal(resourceAttributes[ExpectedSemanticConventions.AttributeEcsTaskArn], "arn:aws:ecs:us-west-2:111122223333:task/default/e9028f8d5d8e4f258373e7b93ce9a3c3");
                Assert.Equal(resourceAttributes[ExpectedSemanticConventions.AttributeEcsTaskFamily], "curltest");
                Assert.Equal(resourceAttributes[ExpectedSemanticConventions.AttributeEcsTaskRevision], "3");
#pragma warning disable CA1861 // Avoid constant arrays as arguments
                Assert.NotStrictEqual(resourceAttributes[ExpectedSemanticConventions.AttributeLogGroupNames], new string[] { "/ecs/containerlogs" });
                Assert.NotStrictEqual(resourceAttributes[ExpectedSemanticConventions.AttributeLogGroupArns], new string[] { "arn:aws:logs:us-west-2:111122223333:log-group:/ecs/containerlogs" });
                Assert.NotStrictEqual(resourceAttributes[ExpectedSemanticConventions.AttributeLogStreamNames], new string[] { "ecs/curl/cd189a933e5849daa93386466019ab50" });
                Assert.NotStrictEqual(resourceAttributes[ExpectedSemanticConventions.AttributeLogStreamArns], new string[] { "arn:aws:logs:us-west-2:111122223333:log-group:/ecs/containerlogs:log-stream:ecs/curl/cd189a933e5849daa93386466019ab50" });
#pragma warning restore CA1861 // Avoid constant arrays as arguments
            }
        }
    }

    internal class MockEcsMetadataEndpoint : IAsyncDisposable
    {
        public readonly Uri Address;
        private readonly IHost host;

        public MockEcsMetadataEndpoint(string containerJsonPath, string taskJsonPath)
        {
            this.host = new HostBuilder()
                .ConfigureWebHost(builder => builder
                .UseKestrel()
                .UseUrls("http://127.0.0.1:0") // Use random localhost port
                .Configure(app =>
            {
                app.Run(async context =>
                {
                    if (context.Request.Method == HttpMethods.Get && context.Request.Path == "/")
                    {
                        var content = await File.ReadAllTextAsync($"{Environment.CurrentDirectory}/{containerJsonPath}");
                        var data = Encoding.UTF8.GetBytes(content);
                        context.Response.ContentType = "application/json";
                        await context.Response.Body.WriteAsync(data);
                    }
                    else if (context.Request.Method == HttpMethods.Get && context.Request.Path == "/task")
                    {
                        var content = await File.ReadAllTextAsync($"{Environment.CurrentDirectory}/{taskJsonPath}");
                        var data = Encoding.UTF8.GetBytes(content);
                        context.Response.ContentType = "application/json";
                        await context.Response.Body.WriteAsync(data);
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status404NotFound;
                        await context.Response.WriteAsync("Not found");
                    }
                });
            })).Build();
            this.host.Start();

            var server = this.host.Services.GetRequiredService<IServer>();
            this.Address = new Uri(server.Features.Get<IServerAddressesFeature>()!.Addresses.First());
        }

        public async ValueTask DisposeAsync() =>
            await this.DisposeAsyncCore();

        protected virtual async ValueTask DisposeAsyncCore() =>
            await this.host.StopAsync();
    }

    private static class ExpectedSemanticConventions
    {
        public const string AttributeCloudProvider = "cloud.provider";
        public const string AttributeCloudPlatform = "cloud.platform";
        public const string AttributeCloudAccountID = "cloud.account.id";
        public const string AttributeCloudAvailabilityZone = "cloud.availability_zone";
        public const string AttributeCloudRegion = "cloud.region";
        public const string AttributeCloudResourceId = "cloud.resource_id";
        public const string AttributeEcsContainerArn = "aws.ecs.container.arn";
        public const string AttributeEcsLaunchtype = "aws.ecs.launchtype";
        public const string AttributeEcsTaskArn = "aws.ecs.task.arn";
        public const string AttributeEcsTaskFamily = "aws.ecs.task.family";
        public const string AttributeEcsTaskRevision = "aws.ecs.task.revision";
        public const string AttributeLogGroupArns = "aws.log.group.arns";
        public const string AttributeLogGroupNames = "aws.log.group.names";
        public const string AttributeLogStreamArns = "aws.log.stream.arns";
        public const string AttributeLogStreamNames = "aws.log.stream.names";
    }
}
#endif
