// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.Helix.JobMonitor;

namespace Microsoft.DotNet.Helix.AzureDevOpsTestPublisher;

internal interface ITestResultProcessor
{
    Task<PreparedTestResults> PrepareAsync(
        WorkItemTestResults results,
        CancellationToken cancellationToken);
}
