// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AwesomeAssertions;
using Microsoft.DotNet.Helix.JobMonitor;
using Microsoft.DotNet.Helix.Sdk.Tests.Fakes;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Microsoft.DotNet.Helix.Sdk.Tests
{
    public class AzureDevOpsServiceTests
    {
        private const string HelixJobGuid = "f79561f8-6c13-4e86-9c6f-0527cd707a54";
        private const string HelixJobTag = "helixjobf79561f86c134e869c6f0527cd707a54";

        [Fact]
        public async Task CreateTestRunAsync_UsesPlainNameAndDoesNotTag()
        {
            var handler = new RecordingHttpMessageHandler(_ =>
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(@"{""id"":123}")
                });
            using var service = new AzureDevOpsService(CreateOptions(), NullLogger.Instance, new HttpClient(handler));

            int testRunId = await service.CreateTestRunAsync("Test Run", CancellationToken.None);

            testRunId.Should().Be(123);
            HttpRequestMessage request = handler.Requests.Should().ContainSingle().Subject;
            request.Method.Should().Be(HttpMethod.Post);
            request.RequestUri.ToString().Should().Be("https://dev.azure.com/dnceng-public/public/_apis/test/runs?api-version=7.1");

            JObject body = JObject.Parse(handler.Bodies.Should().ContainSingle().Subject);
            // The run name is the plain name; the Helix job name is recorded as a tag at completion,
            // not embedded in the name and not posted at creation time.
            body.Value<string>("name").Should().Be("Test Run");
            body.Value<string>("state").Should().Be("InProgress");
            body["build"]?.Value<string>("id").Should().Be("1403994");
            body["tags"].Should().BeNull();
        }

        [Fact]
        public async Task CompleteTestRunAsync_SendsCompletedStateAndHelixJobTag()
        {
            var handler = new RecordingHttpMessageHandler(_ =>
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{}")
                });
            using var service = new AzureDevOpsService(CreateOptions(), NullLogger.Instance, new HttpClient(handler));

            await service.CompleteTestRunAsync(123, HelixJobGuid, CancellationToken.None);

            HttpRequestMessage request = handler.Requests.Should().ContainSingle().Subject;
            request.Method.Should().Be(new HttpMethod("PATCH"));
            request.RequestUri.ToString().Should().Be("https://dev.azure.com/dnceng-public/public/_apis/test/runs/123?api-version=7.1");

            JObject body = JObject.Parse(handler.Bodies.Should().ContainSingle().Subject);
            body.Value<string>("state").Should().Be("Completed");
            // Tags must be posted as objects ([{ "name": "..." }]) — the string form is dropped by
            // Azure DevOps.
            var tags = body["tags"].Should().BeOfType<JArray>().Subject;
            tags.Should().ContainSingle();
            tags[0].Value<string>("name").Should().Be(HelixJobTag);
        }

        [Fact]
        public async Task GetProcessedHelixJobNamesAsync_RecoversFromTags()
        {
            // The build-scoped test results tags endpoint returns the union of tags across the build.
            // Each Helix job name is encoded as an alphanumeric "helixjob{guidWithoutDashes}" tag.
            var handler = new RoutingHttpMessageHandler
            {
                TagsResponse = @"{""count"":3,""value"":[
                    {""name"":""helixjobf79561f86c134e869c6f0527cd707a54""},
                    {""name"":""helixjob0b66c2e3ff384227a779810236d37238""},
                    {""name"":""someothertag""}
                ]}"
            };
            using var service = new AzureDevOpsService(CreateOptions(), NullLogger.Instance, new HttpClient(handler));

            IReadOnlySet<string> processed = await service.GetProcessedHelixJobNamesAsync(CancellationToken.None);

            processed.Should().BeEquivalentTo(
            [
                "f79561f8-6c13-4e86-9c6f-0527cd707a54",
                "0b66c2e3-ff38-4227-a779-810236d37238"
            ]);

            // The tags endpoint is served from the vstmr host and is build-scoped.
            handler.TagsRequests.Should().ContainSingle().Which.RequestUri.ToString()
                .Should().Be("https://vstmr.dev.azure.com/dnceng-public/public/_apis/testresults/tags?buildId=1403994&api-version=7.1-preview.1");
        }

        [Fact]
        public async Task TagRoundTrip_ThroughBuildTagsEndpoint()
        {
            // End-to-end against the fake that models the real Azure DevOps tag behavior: the run
            // is created with a plain name, completed with a tag, and the Helix job name is then
            // recovered via the build tags endpoint (NOT via the run name or inline run tags).
            using var handler = new FakeAzureDevOpsTestRunsHandler();
            using var service = new AzureDevOpsService(CreateOptions(), NullLogger.Instance, new HttpClient(handler));

            int runId = await service.CreateTestRunAsync("Test Run", CancellationToken.None);
            await service.CompleteTestRunAsync(runId, HelixJobGuid, CancellationToken.None);

            IReadOnlySet<string> processed = await service.GetProcessedHelixJobNamesAsync(CancellationToken.None);

            processed.Should().Contain(HelixJobGuid);
            handler.Runs[runId].Name.Should().Be("Test Run");
            handler.Runs[runId].State.Should().Be("Completed");
            handler.Runs[runId].Tags.Should().ContainSingle().Which.Should().Be(HelixJobTag);
        }

        [Theory]
        [InlineData("f79561f8-6c13-4e86-9c6f-0527cd707a54", "helixjobf79561f86c134e869c6f0527cd707a54")]
        [InlineData("0B66C2E3-FF38-4227-A779-810236D37238", "helixjob0b66c2e3ff384227a779810236d37238")]
        public void EncodeHelixJobTag_ProducesAlphanumericTagWithinLengthLimit(string jobName, string expectedTag)
        {
            string tag = AzureDevOpsService.EncodeHelixJobTag(jobName);

            tag.Should().Be(expectedTag);
            tag.Length.Should().BeLessThanOrEqualTo(50, "Azure DevOps limits test run tags to 50 characters");
            tag.Should().MatchRegex("^[a-zA-Z0-9]+$", "Azure DevOps only allows alphanumeric test run tags");

            // The tag round-trips back to the canonical (dashed, lower-case) GUID form.
            AzureDevOpsService.DecodeHelixJobTag(tag).Should().Be(jobName.ToLowerInvariant());
        }

        [Fact]
        public void EncodeHelixJobTag_ReturnsNull_ForNonGuidJobName()
        {
            AzureDevOpsService.EncodeHelixJobTag("not-a-guid").Should().BeNull();
        }

        [Theory]
        [InlineData("someothertag")]
        [InlineData("helixjobnotavalidguid")]
        [InlineData("")]
        [InlineData(null)]
        public void DecodeHelixJobTag_ReturnsNull_ForNonHelixJobTags(string tag)
        {
            AzureDevOpsService.DecodeHelixJobTag(tag).Should().BeNull();
        }

        [Theory]
        [InlineData("https://dev.azure.com/dnceng-public/", "https://vstmr.dev.azure.com/dnceng-public/")]
        [InlineData("https://contoso.visualstudio.com/", "https://contoso.vstmr.visualstudio.com/")]
        public void ToVstmrCollectionUri_RewritesHost(string collectionUri, string expected)
        {
            AzureDevOpsService.ToVstmrCollectionUri(collectionUri).Should().Be(expected);
        }

        private static JobMonitorOptions CreateOptions()
            => new()
            {
                BuildId = "1403994",
                CollectionUri = "https://dev.azure.com/dnceng-public/",
                TeamProject = "public",
                SystemAccessToken = "token",
            };

        private sealed class RecordingHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> respond) : HttpMessageHandler
        {
            public List<HttpRequestMessage> Requests { get; } = [];

            public List<string> Bodies { get; } = [];

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                Requests.Add(request);
                if (request.Content != null)
                {
                    Bodies.Add(await request.Content.ReadAsStringAsync(cancellationToken));
                }

                return respond(request);
            }
        }

        // Serves the build-scoped test results tags endpoint used by GetProcessedHelixJobNamesAsync.
        private sealed class RoutingHttpMessageHandler : HttpMessageHandler
        {
            public string TagsResponse { get; set; } = @"{""count"":0,""value"":[]}";

            public List<HttpRequestMessage> TagsRequests { get; } = [];

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                string path = request.RequestUri.AbsolutePath;
                string content;
                if (path.EndsWith("/_apis/testresults/tags", StringComparison.OrdinalIgnoreCase))
                {
                    TagsRequests.Add(request);
                    content = TagsResponse;
                }
                else
                {
                    content = @"{""value"":[]}";
                }

                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(content)
                });
            }
        }
    }
}
