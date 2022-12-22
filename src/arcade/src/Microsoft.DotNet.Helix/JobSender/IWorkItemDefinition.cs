using System;

namespace Microsoft.DotNet.Helix.Client
{
    /// <summary>
    /// Work Item definition with all required information already specified
    /// that can either completed by calling <see cref="AttachToJob"/> or further extended.
    /// </summary>
    public interface IWorkItemDefinition
    {
        string WorkItemName { get; }

        string Command { get; }

        /// <summary>
        /// Specifies timeout for the work item to finish inside of Helix.
        /// </summary>
        IWorkItemDefinition WithTimeout(TimeSpan timeout);

        /// <summary>
        /// Complete work item and return to specification of the overarching job.
        /// </summary>
        /// <returns>Fluent job builder.</returns>
        IJobDefinition AttachToJob();
    }
}
