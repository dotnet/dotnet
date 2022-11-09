using Microsoft.Build.Framework;
using Microsoft.DotNet.Helix.Client;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Helix.Sdk
{
    public class CancelHelixJobs : HelixTask
    {
        /// <summary>
        /// An array of Helix Jobs to cancel
        /// Jobs are expected to have a "CancellationToken" metadata item.
        /// If they lack this, we'll make an attempt to cancel using the Access Token from HelixTask, if supplied.
        /// Note token-based job cancellation requires the token to be the same as the user who started the job.
        /// </summary>
        [Required]
        public ITaskItem[] Jobs { get; set; }

        protected override async Task ExecuteCore(CancellationToken cancellationToken)
        {
            var api = HelixApi;

            Log.LogMessage($"Attempting to cancel {Jobs.Count()} Helix jobs");

            foreach (ITaskItem job in Jobs)
            {
                string correlationId = job.GetMetadata("Identity");

                Log.LogMessage(MessageImportance.High, $"Cancelling Helix Job {correlationId}");

                try
                {
                    // Any ITaskItem describing a job started in the same build will have this metadata.
                    // However, this standalone task is designed to be used to work around issues such as described in:
                    // https://developercommunity.visualstudio.com/t/ado-pipeline-timeouts-dont-cancel-the-same-as-when/1371617?from=email
                    // and ensure that anyone sufficiently motivated can still cancel Helix jobs after MSBuild is rapidly killed.
                    if (job.TryGetMetadata("HelixJobCancellationToken", out string helixCancellationToken))
                    {
                        await api.Job.CancelAsync(correlationId, helixCancellationToken, cancellationToken);
                        Log.LogMessage(MessageImportance.High, $"Successfully cancelled Helix Job {correlationId} via cancellation token.");
                    }
                    // Cancellation via token is preferred as these values are single-use (only work for one job) secrets and don't matter to leak.
                    else if (!string.IsNullOrEmpty(AccessToken))
                    {
                        Log.LogMessage(MessageImportance.High, "'HelixJobCancellationToken' metadata not supplied, will attempt to cancel using Access token. (Token must match user id that started the work)");
                        await api.Job.CancelAsync(correlationId, null, cancellationToken);
                        Log.LogMessage(MessageImportance.High, $"Successfully cancelled Helix Job {correlationId} via access token.");
                    }
                    else
                    {
                        Log.LogError($"Cannot cancel job '{job}'; please supply either the Job's cancellation token or the job creator's access token");
                    }
                }
                catch (RestApiException e) when (e.Response.Status == 304)
                {
                    // Helix Cancel API's "Not Modified" == Already cancelled; not really an error case
                    Log.LogMessage($"Job '{correlationId}' was already cancelled.");
                }
                catch (RestApiException e) when (e.Response.Status == 404)
                {
                    // Not found can indicate calling very close to job creation or accidentally mixing and matching anonymous/authenticated.
                    // Try to be helpful with an error message.
                    Log.LogError($"Job '{correlationId}' was not found. Check if you are mixing and matching authenticated and anonymous access, or accessing instantly after job creation");
                }
                catch (Exception toLog)
                {
                    Log.LogErrorFromException(toLog, false);
                }
            }

            if (!Log.HasLoggedErrors)
            {
                Log.LogMessage(MessageImportance.High, $"Successfully cancelled {Jobs.Count()} Helix jobs");
            }
        }
    }
}
