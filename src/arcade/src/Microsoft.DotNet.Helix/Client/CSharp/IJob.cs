using System.Threading;
using System.Threading.Tasks;
using Microsoft.DotNet.Helix.Client.Models;

namespace Microsoft.DotNet.Helix.Client
{
    partial interface IJob
    {
        Task<JobPassFail> WaitForJobAsync(string jobCorrelationId, int pollingIntervalMs = 10000, CancellationToken cancellationToken = default);
    }
}
