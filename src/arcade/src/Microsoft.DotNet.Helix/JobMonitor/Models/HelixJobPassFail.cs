// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;

namespace Microsoft.DotNet.Helix.JobMonitor.Models
{
    /// <summary>
    /// Pass/fail breakdown for a completed Helix job based on work item exit codes.
    /// Decoupled from the Helix Client SDK's generated models.
    /// </summary>
    public sealed class HelixJobPassFail
    {
        public HelixJobPassFail(IReadOnlyList<string> passedWorkItems, IReadOnlyList<string> failedWorkItems)
        {
            PassedWorkItems = passedWorkItems ?? Array.Empty<string>();
            FailedWorkItems = failedWorkItems ?? Array.Empty<string>();
        }

        public IReadOnlyList<string> PassedWorkItems { get; }

        public IReadOnlyList<string> FailedWorkItems { get; }

        public bool HasFailures => FailedWorkItems.Count > 0;
    }
}
