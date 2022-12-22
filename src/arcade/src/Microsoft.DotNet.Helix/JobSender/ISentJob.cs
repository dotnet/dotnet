using System.Threading;
using System.Threading.Tasks;
using Microsoft.DotNet.Helix.Client.Models;

namespace Microsoft.DotNet.Helix.Client
{
    /// <summary>
    /// Job that has been already sent to Helix, but is not
    /// necessarily evaluated yet by its agents.
    /// </summary>
    public interface ISentJob
    {
        /// <summary>
        /// The ID of the job assigned by Helix.
        /// </summary>
        string CorrelationId { get; }

        /// <summary>
        /// Token allowing cancellation of this specific job without other credentials
        /// </summary>
        string HelixCancellationToken { get; }

        /// <summary>
        /// URI for blob storage container with the results.
        /// </summary>
        string ResultsContainerUri { get; }

        /// <summary>
        /// Shared Access Signature for access to the container with results.
        /// Used for internal builds.
        /// </summary>
        string ResultsContainerReadSAS { get; }

        /// <summary>
        /// Poll for the job to actually finish inside Helix.
        /// </summary>
        Task<JobPassFail> WaitAsync(int pollingIntervalMs = 10000, CancellationToken cancellationToken = default);
    }
}
