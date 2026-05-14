// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.DotNet.Helix.Client;

namespace Microsoft.DotNet.Helix.Sdk
{
    public class WaitForHelixJobCompletion : HelixTask
    {
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

            int iterationCount = 0;
            try
            {
                for (; ; await Task.Delay(20000, cancellationToken).ConfigureAwait(false)) // delay every time this loop repeats
                {
                    // On first try, and ~ every 12 checks (~4 minutes) after, check the job details for errors.
                    // Jobs with any job-level errors will never finish and we want to investigate these.
                    if (iterationCount++ % 12 == 0)
                    {
                        var jd = await HelixApi.Job.DetailsAsync(jobName, cancellationToken).ConfigureAwait(false);
                        if (jd.Errors.Count() > 0)
                        {
                            string errorMsgs = string.Join(",", jd.Errors.Select(d => d.Message));
                            Log.LogError($"Helix encountered job-level error(s) for this job ({errorMsgs}).  Please contact dnceng with this information.");
                            return;
                        }
                    }

                    cancellationToken.ThrowIfCancellationRequested();
                    var pf = await HelixApi.Job.PassFailAsync(jobName, cancellationToken).ConfigureAwait(false);
                    if (pf.Working == 0 && pf.Total != 0)
                    {
                        Log.LogMessage(MessageImportance.High, $"Job {jobName} on {queueName} is completed with {pf.Total} finished work items.");
                        return;
                    }

                    Log.LogMessage($"Job {jobName} on {queueName} is not yet completed with Pending: {pf.Working}, Finished: {pf.Total - pf.Working}");
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
    }
}
