// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.DotNet.Helix.Sdk;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.DotNet.Helix.AzureDevOps
{
    public class CheckAzurePipelinesTestResults : AzureDevOpsTask
    {
        public int[] TestRunIds { get; set; }

        public ITaskItem[] ExpectedTestFailures { get; set; }

        public string EnableFlakyTestSupport { get; set; }

        [Required]
        public ITaskItem[] WorkItems { get; set; }

        protected override async Task ExecuteCoreAsync(HttpClient client)
        {
            if (ExpectedTestFailures?.Length > 0)
            {
                await ValidateExpectedTestFailuresAsync(client);
                return;
            }

            if (!string.IsNullOrEmpty(EnableFlakyTestSupport))
            {
                await CheckTestResultsWithFlakySupport(client);
                return;
            }


            await CheckTestResultsAsync(client);
        }

        private async Task CheckTestResultsAsync(HttpClient client)
        {
            foreach (int testRunId in TestRunIds)
            {
                bool runComplete = false;
                int triesToWait = 3;
                JObject data = null;

                do
                {
                    data = await RetryAsync(
                    async () =>
                    {
                        using var req = new HttpRequestMessage(
                            HttpMethod.Get,
                            $"{CollectionUri}{TeamProject}/_apis/test/runs/{testRunId}?api-version=6.0");
                        using HttpResponseMessage res = await client.SendAsync(req);
                        return await ParseResponseAsync(req, res);
                    });
                    // This retry does not use the RetryAsync() function as that one only retries for network/timeout issues
                    triesToWait--;
                    runComplete = CheckAzurePipelinesTestRunIsComplete(data);
                    if (!runComplete && triesToWait > 0)
                    {
                        Log.LogWarning($"Test run {testRunId} is not in completed state.  Will check back in 10 seconds.");
                        await Task.Delay(10000);
                    }
                }
                while (!runComplete && triesToWait > 0);

                if (data != null && data["runStatistics"] is JArray runStatistics)
                {
                    var failed = runStatistics.Children()
                        .FirstOrDefault(stat => stat["outcome"]?.ToString() == "Failed");
                    if (failed != null)
                    {
                        await LogErrorsForFailedRun(client, testRunId);
                    }
                    else
                    {
                        Log.LogMessage(MessageImportance.Low, $"Test run {testRunId} has not failed.");
                    }
                }
            }
        }

        private bool CheckAzurePipelinesTestRunIsComplete(JObject data)
        {
            // Context: https://github.com/dotnet/arcade/issues/11942
            // it seems it's possible if checking immediately after a run is closed to not see all results
            // Since we pass/fail build tasks based off failed test items, it's very important that we not miss this.
            // This check will add logging if /_apis/test/runs/ manages to get called while incomplete.
            if (data == null)
            {
                return false;
            }
            var stateCompleted = data["state"]?.Value<string>()?.Equals("Completed");
            var postProcessStateCompleted = data["postProcessState"]?.Value<string>()?.Equals("Complete");

            return (stateCompleted == true && postProcessStateCompleted == true);
        }

        private async Task LogErrorsForFailedRun(HttpClient client, int testRunId)
        {
            JObject data = await RetryAsync(
                async () =>
                {
                    using var req = new HttpRequestMessage(
                        HttpMethod.Get,
                        $"{CollectionUri}{TeamProject}/_apis/test/runs/{testRunId}/results?outcomes=Failed&$top=100&api-version=6.0");
                    using HttpResponseMessage res = await client.SendAsync(req);
                    return await ParseResponseAsync(req, res);
                });
            int count = data.Value<int>("count");
            IEnumerable<JObject> entries = data.Value<JArray>("value").Cast<JObject>();
            if (count == 0)
            {
                Log.LogError(FailureCategory.Test, $"Test run {testRunId} has one or more failing tests based on run statistics, but I couldn't find the failures.");
                return;
            }

            foreach (JObject result in entries)
            {
                string name = result.Value<string>("automatedTestName");
                string comment = result.Value<string>("comment");
                JObject helixData;
                try
                {
                    helixData = JObject.Parse(comment);
                }
                catch (JsonException)
                {
                    helixData = null;
                }
                string jobId = helixData?.Value<string>("HelixJobId");
                string workItemName = helixData?.Value<string>("HelixWorkItemName");
                ITaskItem workItem = null;
                if (helixData != null && !string.IsNullOrEmpty(jobId) && !string.IsNullOrEmpty(workItemName))
                {
                    workItem = WorkItems.FirstOrDefault(t =>
                        t.GetMetadata("JobName") == jobId && t.GetMetadata("WorkItemName") == workItemName);
                }

                if (workItem != null)
                {
                    Log.LogError(FailureCategory.Test, $"Test {name} has failed. Check the Test tab or this console log: {workItem.GetMetadata("ConsoleOutputUri")}");
                }
                else
                {
                    Log.LogError(FailureCategory.Test, $"Test {name} has failed. Check the Test tab for details.");
                }
            }
        }

        private async Task CheckTestResultsWithFlakySupport(HttpClient client)
        {
            JObject data = await RetryAsync(
                async () =>
                {
                    using (var req = new HttpRequestMessage(
                        HttpMethod.Get,
                        $"{CollectionUri}{TeamProject}/_apis/test/resultsummarybybuild?buildId={BuildId}&api-version=5.1-preview.2")
                    )
                    {
                        using (HttpResponseMessage res = await client.SendAsync(req))
                        {
                            return await ParseResponseAsync(req, res);
                        }
                    }
                });

            if (data != null && data["aggregatedResultsAnalysis"] is JObject aggregatedResultsAnalysis &&
                aggregatedResultsAnalysis["resultsByOutcome"] is JObject resultsByOutcome)
            {
                foreach (JProperty property in resultsByOutcome.Properties())
                {
                    string outcome = property.Name.ToLowerInvariant();
                    var outcomeResults = (JObject) property.Value;
                    int count = outcomeResults["count"].ToObject<int>();
                    var message = $"Build has {count} {outcome} tests.";
                    if (outcome == "failed")
                    {
                        Log.LogError(FailureCategory.Test, message);
                    }
                    else
                    {
                        Log.LogMessage(MessageImportance.High, message);
                    }
                }
            }
            else
            {
                Log.LogError(FailureCategory.Helix, "Unable to get test report from build.");
            }
        }

        private async Task ValidateExpectedTestFailuresAsync(HttpClient client)
        {
            foreach (var runId in TestRunIds)
            {
                JObject data = await RetryAsync(
                    async () =>
                    {
                        using (var req = new HttpRequestMessage(
                            HttpMethod.Get,
                            $"{CollectionUri}{TeamProject}/_apis/test/runs/{runId}/results?api-version=5.0&outcomes=Failed")
                        )
                        {
                            using (HttpResponseMessage res = await client.SendAsync(req))
                            {
                                return await ParseResponseAsync(req, res);
                            }
                        }
                    });

                if (data != null)
                {
                    var failedResults = (JArray)data["value"];
                    HashSet<string> expectedFailures = ExpectedTestFailures?.Select(i => i.GetMetadata("Identity")).ToHashSet() ?? new HashSet<string>();
                    foreach (var failedResult in failedResults)
                    {
                        var testName = (string)failedResult["automatedTestName"];
                        if (expectedFailures.Contains(testName))
                        {
                            expectedFailures.Remove(testName);
                            Log.LogMessage($"TestRun {runId}: Test {testName} has failed and was expected to fail.");
                        }
                        else
                        {
                            Log.LogError(FailureCategory.Test, $"TestRun {runId}: Test {testName} has failed and is not expected to fail.");
                        }
                    }

                    foreach (string expectedFailure in expectedFailures)
                    {
                        Log.LogError(FailureCategory.Test, $"TestRun {runId}: Test {expectedFailure} was expected to fail but did not fail.");
                    }
                }
            }
        }
    }
}
