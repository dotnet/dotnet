using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.DotNet.Helix.Client.Models;

namespace Microsoft.DotNet.Helix.Client
{
    partial class Job
    {
        public async Task<JobPassFail> WaitForJobAsync(string jobCorrelationId, int pollingIntervalMs = 10000, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(jobCorrelationId)) throw new ArgumentNullException(nameof(jobCorrelationId));
            if (pollingIntervalMs < 5000) throw new ArgumentOutOfRangeException(nameof(pollingIntervalMs), pollingIntervalMs, "The polling interval cannot be less than 5000.");

            cancellationToken.ThrowIfCancellationRequested();

            for (;; await Task.Delay(pollingIntervalMs, cancellationToken).ConfigureAwait(false)) // delay every time this loop repeats
            {
                var pf = await PassFailAsync(jobCorrelationId, cancellationToken).ConfigureAwait(false);
                if (pf.Working == 0 && pf.Total != 0)
                {
                    return pf;
                }

                cancellationToken.ThrowIfCancellationRequested();
            }
        }
    }
}
