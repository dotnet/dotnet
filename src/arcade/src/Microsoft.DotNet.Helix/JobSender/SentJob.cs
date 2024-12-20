// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.DotNet.Helix.Client.Models;

namespace Microsoft.DotNet.Helix.Client
{
    internal class SentJob : ISentJob
    {
        public SentJob(IJob jobApi, JobCreationResult newJob, string resultsContainerUri, string resultsContainerReadSAS)
        {
            JobApi = jobApi;
            CorrelationId = newJob.Name;
            HelixCancellationToken = newJob.CancellationToken;
            ResultsContainerUri = resultsContainerUri;
            ResultsContainerReadSAS = resultsContainerReadSAS;
        }

        public IJob JobApi { get; }
        public string CorrelationId { get; }
        public string HelixCancellationToken { get; }
        public string ResultsContainerUri { get; }
        public string ResultsContainerReadSAS { get; }

        public Task<JobPassFail> WaitAsync(int pollingIntervalMs = 10000, CancellationToken cancellationToken = default)
        {
            return JobApi.WaitForJobAsync(CorrelationId, pollingIntervalMs, cancellationToken);
        }
    }
}
