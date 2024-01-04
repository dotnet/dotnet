// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.DotNet.Helix.AzureDevOps
{
    public class StopAzurePipelinesTestRun : AzureDevOpsTask
    {
        [Required]
        public int TestRunId { get; set; }

        [Required]
        public string TestRunName { get; set; }

        protected override async Task ExecuteCoreAsync(HttpClient client)
        {
            await RetryAsync(
                async () =>
                {
                    Log.LogMessage(MessageImportance.High, $"Stopping Azure Pipelines Test Run {TestRunName} (Results: {CollectionUri}{TeamProject}/_build/results?buildId={BuildId}&view=ms.vss-test-web.build-test-results-tab )");

                    using (var req =
                        new HttpRequestMessage(
                            new HttpMethod("PATCH"),
                            $"{CollectionUri}{TeamProject}/_apis/test/runs/{TestRunId}?api-version=5.0")
                        {
                            Content = new StringContent(
                                JsonConvert.SerializeObject(new JObject { ["state"] = "Completed", }),
                                Encoding.UTF8,
                                "application/json"),
                        })
                    {
                        using (var res = await client.SendAsync(req))
                        {
                            res.EnsureSuccessStatusCode();
                        }
                    }
                });
        }
    }
}
