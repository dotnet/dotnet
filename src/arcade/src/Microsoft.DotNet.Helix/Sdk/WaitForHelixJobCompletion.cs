// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.DotNet.Helix.Client;
using Microsoft.DotNet.Helix.Client.Models;

namespace Microsoft.DotNet.Helix.Sdk
{
    public class WaitForHelixJobCompletion : HelixTask
    {
        internal const string HelixControllerWorkQueueingWorkItemName = "HelixController Work Queueing";
        private static readonly TimeSpan HelixJobCompletionPollingInterval = TimeSpan.FromSeconds(20);

        /// <summary>
        /// An array of Helix Jobs to be waited on
        /// </summary>
        [Required]
        public ITaskItem[] Jobs { get; set; }

        public bool CancelHelixJobsOnTaskCancellation { get; set; } = true;

        protected override async Task ExecuteCore(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            // Wait 1 second to allow helix to register the job creation
            await Task.Delay(1000, cancellationToken);

            List<(string jobName, string queueName, string jobCancellationToken)> jobNames = Jobs.Select(j => (j.GetMetadata("Identity"), j.GetMetadata("HelixTargetQueue"), j.GetMetadata("HelixJobCancellationToken"))).ToList();

            cancellationToken.ThrowIfCancellationRequested();
            await Task.WhenAll(jobNames.Select(n => WaitForHelixJobAsync(n.jobName, n.queueName, n.jobCancellationToken, cancellationToken)));
        }

        private async Task WaitForHelixJobAsync(string jobName, string queueName, string helixJobCancellationToken, CancellationToken cancellationToken)
        {
            await Task.Yield();
            cancellationToken.ThrowIfCancellationRequested();

            // Only include the link to the Helix API "Details" page if we're using anonymous access.
            // If a user must manually edit the URL with an Access Token, there is limited value in including it in build logging.
            string detailsUrlWhereApplicable = HelixApi.Options.Credentials == null ? $" (Details: {HelixApi.Options.BaseUri}api/jobs/{jobName}/details?api-version=2019-06-17 )" : string.Empty;

            Log.LogMessage(MessageImportance.High, $"Waiting for completion of job {jobName} on {queueName}{detailsUrlWhereApplicable}");

            try
            {
                for (; ; await Task.Delay(HelixJobCompletionPollingInterval, cancellationToken).ConfigureAwait(false)) // delay every time this loop repeats
                {
                    JobDetails jd = await HelixApi.Job.DetailsAsync(jobName, cancellationToken).ConfigureAwait(false);
                    if (jd.Errors?.Any() == true)
                    {
                        string errorMsgs = string.Join(",", jd.Errors.Select(d => d.Message));
                        Log.LogError($"Helix encountered job-level error(s) for this job ({errorMsgs}).  Please contact dnceng with this information.");
                        return;
                    }

                    cancellationToken.ThrowIfCancellationRequested();
                    IReadOnlyCollection<WorkItemSummary> realWorkItems = GetRealWorkItems(await HelixApi.WorkItem.ListAsync(jobName, cancellationToken).ConfigureAwait(false));
                    int finishedWorkItems = realWorkItems.Count(wi => wi.ExitCode.HasValue);

                    if (IsJobComplete(jd, realWorkItems))
                    {
                        Log.LogMessage(MessageImportance.High, $"Job {jobName} on {queueName} is completed with {finishedWorkItems} finished work items.");
                        return;
                    }

                    string expected = jd.InitialWorkItemCount is > 0 ? jd.InitialWorkItemCount.Value.ToString() : "unknown";
                    Log.LogMessage($"Job {jobName} on {queueName} is not yet completed with Expected: {expected}, Observed: {realWorkItems.Count}, Finished: {finishedWorkItems}");
                    cancellationToken.ThrowIfCancellationRequested();
                }
            }
            catch (OperationCanceledException)
            {
                // Just in case the cancellation was an Http client timeout from the API, check our token was the one that caused the exception
                if (CancelHelixJobsOnTaskCancellation && cancellationToken.IsCancellationRequested)
                {
                    if (string.IsNullOrEmpty(helixJobCancellationToken))
                    {
                        Log.LogWarning($"{nameof(CancelHelixJobsOnTaskCancellation)} is set to 'true', but no value was provided for {nameof(helixJobCancellationToken)}");
                        return;
                    }
                    Log.LogWarning($"Build task was cancelled while waiting on job '{jobName}'.  Attempting to cancel this job in Helix...");
                    try
                    {
                        await HelixApi.Job.CancelAsync(jobName, helixJobCancellationToken);
                        Log.LogWarning($"Successfully cancelled job '{jobName}'");
                    }
                    catch (RestApiException checkIfAlreadyCancelled) when (checkIfAlreadyCancelled.Response.Status == 304)
                    {
                        // Already cancelled
                        Log.LogWarning($"Job '{jobName}' was already cancelled.");
                    }
                }
            }
        }

        internal static IReadOnlyCollection<WorkItemSummary> GetRealWorkItems(IEnumerable<WorkItemSummary> workItems)
        {
            return workItems
                .Where(w => w.Name != HelixControllerWorkQueueingWorkItemName)
                .ToList();
        }

        internal static bool AreAllExpectedWorkItemsTerminal(JobDetails jobDetails, IReadOnlyCollection<WorkItemSummary> realWorkItems)
        {
            return jobDetails.InitialWorkItemCount is > 0
                && realWorkItems.Count >= jobDetails.InitialWorkItemCount.Value
                && realWorkItems.All(wi => wi.ExitCode.HasValue);
        }

        internal static bool IsJobComplete(JobDetails jobDetails, IReadOnlyCollection<WorkItemSummary> realWorkItems)
        {
            return jobDetails.Finished != null || AreAllExpectedWorkItemsTerminal(jobDetails, realWorkItems);
        }
    }
}
