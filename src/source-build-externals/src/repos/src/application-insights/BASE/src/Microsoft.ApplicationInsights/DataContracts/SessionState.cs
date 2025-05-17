namespace Microsoft.ApplicationInsights.DataContracts
{
#pragma warning disable CA1724 // "The type name conflicts with 'System.Web.SessionState'. This will go away in 3.0
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Contains values that identify state of a user session.
    /// </summary>
    [Obsolete("Session state events are no longer used.")]
    public enum SessionState
    {
        /// <summary>
        /// Indicates that a user session started.
        /// </summary>
        Start,

        /// <summary>
        /// Indicates that a user session ended.
        /// </summary>
        End,
    }
#pragma warning restore CA1724
}
