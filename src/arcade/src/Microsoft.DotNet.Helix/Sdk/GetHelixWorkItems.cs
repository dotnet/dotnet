// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Helix.Client.Models;
using Newtonsoft.Json;
using Task = System.Threading.Tasks.Task;

namespace Microsoft.DotNet.Helix.Sdk
{
    public class GetHelixWorkItems : HelixTask
    {
        public const int DelayBetweenHelixApiCallsInMs = 500;

        /// <summary>
        /// An array of Helix Jobs for which to get status
        /// </summary>
        [Required]
        public ITaskItem[] Jobs { get; set; }

        [Output]
        public ITaskItem[] WorkItems { get; set; }

        protected override async Task ExecuteCore(CancellationToken cancellationToken)
        {
            WorkItems = (await Task.WhenAll(Jobs.Select(j => GetWorkItemsAsync(j, cancellationToken))).ConfigureAwait(false)).SelectMany(r => r).ToArray();
        }

        private async Task<IEnumerable<ITaskItem>> GetWorkItemsAsync(ITaskItem job, CancellationToken cancellationToken)
        {
            var jobName = job.GetMetadata("Identity");

            Log.LogMessage($"Getting status of job {jobName}");

            var status = await HelixApi.Job.PassFailAsync(jobName, cancellationToken).ConfigureAwait(false);

            if (status.Working > 0)
            {
                Log.LogError(
                    FailureCategory.Build,
                    $"This task can only be used on completed jobs. There are {status.Working} of {status.Total} unfinished work items.");
                return Array.Empty<ITaskItem>();
            }

            var workItems = new List<ITaskItem>();

            IDictionary<string, string> CreateWorkItemMetadata(string name)
            {
                var metadata = job.CloneCustomMetadata() as IDictionary<string, string>;
                metadata["JobName"] = jobName;
                metadata["WorkItemName"] = name;
                var consoleUri = HelixApi.Options.BaseUri.AbsoluteUri.TrimEnd('/') + $"/api/2019-06-17/jobs/{jobName}/workitems/{Uri.EscapeDataString(name)}/console";
                metadata["ConsoleOutputUri"] = consoleUri;

                return metadata;
            }

            ITaskItem2 CreateTaskItem(string workItemName, IDictionary<string, string> metadata)
            {
                ITaskItem2 workItem = new TaskItem(workItemName);

                foreach(KeyValuePair<string, string> entry in metadata)
                {
                    workItem.SetMetadataValueLiteral(entry.Key, entry.Value);
                }

                return workItem;
            }

            foreach (string name in status.Passed)
            {
                string wi = Helpers.CleanWorkItemName(name);

                var metadata = CreateWorkItemMetadata(wi);
                metadata["Failed"] = "false";

                workItems.Add(CreateTaskItem($"{jobName}/{wi}", metadata));
            }

            foreach (string name in status.Failed)
            {
                string wi = Helpers.CleanWorkItemName(name);

                var metadata = CreateWorkItemMetadata(wi);
                metadata["Failed"] = "true";

                try
                {
                    // Do this serially with a delay because total failure can hit throttling
                    // latestOnly parameter is set false here to download all possible files
                    var files = await HelixApi.WorkItem.ListFilesAsync(wi, jobName, false, cancellationToken).ConfigureAwait(false);

                    if (!string.IsNullOrEmpty(AccessToken))
                    {
                        // Add AccessToken to all file links because the api requires auth if we submitted the job with auth
                        files = files
                                .Select(file => new UploadedFile(file.Name, file.Link + "?access_token=" + AccessToken))
                                .ToImmutableList();
                    }

                    metadata["UploadedFiles"] = JsonConvert.SerializeObject(files);
                }
                catch (Exception ex)
                {
                    Log.LogWarningFromException(ex);
                }

                workItems.Add(CreateTaskItem($"{jobName}/{wi}", metadata));
                await Task.Delay(DelayBetweenHelixApiCallsInMs, cancellationToken);
            }

            return workItems;
        }
    }
}
