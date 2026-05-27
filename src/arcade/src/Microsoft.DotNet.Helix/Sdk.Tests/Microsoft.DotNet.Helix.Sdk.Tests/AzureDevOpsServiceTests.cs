// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.DotNet.Helix.JobMonitor;
using Microsoft.DotNet.Helix.Sdk.Tests.Fakes;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Microsoft.DotNet.Helix.Sdk.Tests
{
    public class AzureDevOpsServiceTests
    {
        [Fact]
        public async Task CreateTestRunAsync_EmbedsHelixJobNameInRunName()
        {
            var handler = new RecordingHttpMessageHandler(_ =>
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(@"{""id"":123}")
                });
            using var service = new AzureDevOpsService(CreateOptions(), NullLogger.Instance, new HttpClient(handler));

            int testRunId = await service.CreateTestRunAsync("Test Run", "helix-job-1", CancellationToken.None);

            Assert.Equal(123, testRunId);
            HttpRequestMessage request = Assert.Single(handler.Requests);
            Assert.Equal(HttpMethod.Post, request.Method);
            Assert.Equal("https://dev.azure.com/dnceng-public/public/_apis/test/runs?api-version=7.1", request.RequestUri.ToString());

            JObject body = JObject.Parse(Assert.Single(handler.Bodies));
            Assert.Equal("Test Run [HelixJob:helix-job-1]", body.Value<string>("name"));
            Assert.Equal("InProgress", body.Value<string>("state"));
            Assert.Equal("1403994", body["build"]?.Value<string>("id"));
            Assert.Null(body.Value<string>("comment"));
            // We deliberately do not send tags: Azure DevOps silently drops them on
            // POST /test/runs (verified empirically), so the helix job name is round-tripped
            // via the run name marker.
            Assert.Null(body["tags"]);
        }

        [Fact]
        public async Task GetProcessedHelixJobNamesAsync_RecoversFromTestRunNameMarker()
        {
            // Real Azure DevOps drops the "tags" property on POST /test/runs. The monitor must
            // therefore recover the Helix job name from the run name marker so test results are
            // not re-uploaded on a retry of the monitor task.
            var handler = new RecordingHttpMessageHandler(_ =>
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(@"{""value"":[
                        {""id"":10,""state"":""Completed"",""name"":""Linux Build_Debug [HelixJob:helix-linux]""},
                        {""id"":11,""state"":""Completed"",""name"":""Windows Build_Release [HelixJob:helix-windows]""},
                        {""id"":12,""state"":""InProgress"",""name"":""Pending [HelixJob:helix-pending]""},
                        {""id"":13,""state"":""Completed"",""name"":""Unrelated test run""}
                    ]}")
                });
            using var service = new AzureDevOpsService(CreateOptions(), NullLogger.Instance, new HttpClient(handler));

            IReadOnlySet<string> processed = await service.GetProcessedHelixJobNamesAsync(CancellationToken.None);

            Assert.Equal(["helix-linux", "helix-windows"], processed.OrderBy(static name => name));
            // Single list call, no per-run detail fetches: the marker is in the run "name"
            // which is always part of the list response.
            HttpRequestMessage request = Assert.Single(handler.Requests);
            Assert.EndsWith("/_apis/test/runs?buildUri=vstfs%3A%2F%2F%2FBuild%2FBuild%2F1403994&$top=1000&api-version=7.1", request.RequestUri.ToString());
        }

        [Fact]
        public async Task CompleteTestRunAsync_SendsCompletedState()
        {
            var handler = new RecordingHttpMessageHandler(_ =>
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{}")
                });
            using var service = new AzureDevOpsService(CreateOptions(), NullLogger.Instance, new HttpClient(handler));

            await service.CompleteTestRunAsync(123, CancellationToken.None);

            HttpRequestMessage request = Assert.Single(handler.Requests);
            Assert.Equal(new HttpMethod("PATCH"), request.Method);
            Assert.Equal("https://dev.azure.com/dnceng-public/public/_apis/test/runs/123?api-version=7.1", request.RequestUri.ToString());

            JObject body = JObject.Parse(Assert.Single(handler.Bodies));
            Assert.Equal("Completed", body.Value<string>("state"));
        }

        [Fact]
        public async Task TagsArePostedButNeverObservableViaGet_DocumentsAzdoBehavior()
        {
            // Regression guard: even if a future change accidentally posts tags
            // when creating a test run, Azure DevOps drops them server-side and
            // GET /_apis/test/runs will not return them. The FakeAzureDevOpsTestRunsHandler
            // models this quirk (see its class-level note); this test ensures that
            // model is exercised so a regression to tag-based dedup would fail here.
            using var handler = new FakeAzureDevOpsTestRunsHandler();
            using var service = new AzureDevOpsService(CreateOptions(), NullLogger.Instance, new HttpClient(handler));

            int runId = await service.CreateTestRunAsync("Test Run", "helix-job-1", CancellationToken.None);
            await service.CompleteTestRunAsync(runId, CancellationToken.None);

            IReadOnlySet<string> processed = await service.GetProcessedHelixJobNamesAsync(CancellationToken.None);

            // Helix job name round-trips via the run "name" suffix, NOT via tags.
            Assert.Contains("helix-job-1", processed);
            Assert.Equal("Test Run [HelixJob:helix-job-1]", handler.Runs[runId].Name);
            Assert.Equal("Completed", handler.Runs[runId].State);
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
    }
}
