// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.DotNet.Helix.Client.Models;

namespace Microsoft.DotNet.Helix.JobMonitor
{
    internal static class WorkItemExtensions
    {
        extension(WorkItemSummary workItem)
        {
            /// <summary>
            /// A Helix work item is considered failed if its exit code is non-zero
            /// or its state is not the terminal success state ("Finished").
            /// </summary>
            public bool IsFailed => workItem.ExitCode != 0
                || !workItem.State.Equals("Finished", StringComparison.OrdinalIgnoreCase);

            /// <summary>
            /// True while the work item is still in flight (no exit code yet and no
            /// terminal Helix state).
            /// </summary>
            public bool IsUnfinished => !workItem.ExitCode.HasValue
                && !string.Equals(workItem.State, "Finished", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(workItem.State, "Failed", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(workItem.State, "TimedOut", StringComparison.OrdinalIgnoreCase);

            /// <summary>
            /// True when the work item is failed and not still in flight. This is the
            /// "worth reporting eagerly" signal used by the status logger.
            /// </summary>
            public bool IsFailedAndTerminal => workItem.IsFailed && !workItem.IsUnfinished;

            public string FormattedState
            {
                get
                {
                    string exitCode = workItem.ExitCode.HasValue
                        ? $", exit code {workItem.ExitCode.Value}"
                        : string.Empty;
                    return $"{workItem.State}{exitCode}";
                }
            }
        }
    }
}
