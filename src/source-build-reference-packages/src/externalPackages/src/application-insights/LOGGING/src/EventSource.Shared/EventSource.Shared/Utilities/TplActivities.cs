//-----------------------------------------------------------------------
// <copyright file="TplActivities.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.TraceEvent.Shared.Utilities
{
    using System;

    /// <summary>
    /// Provides well-known values for working with Task Parallel Library (TPL) EventSource.
    /// </summary>
    public static class TplActivities
    {
        /// <summary>
        /// Gets the GUID of the TPL EventSource.
        /// </summary>
        public static readonly Guid TplEventSourceGuid = new Guid("2e5dba47-a3d2-4d16-8ee0-6671ffdcd7b5");

        /// <summary>
        /// Gets the keyword that enables hierarchical activity IDs.
        /// </summary>
        public static readonly ulong TaskFlowActivityIdsKeyword = 0x80;
    }
}
