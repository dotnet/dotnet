//-----------------------------------------------------------------------
// <copyright file="EtwListeningRequest.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.EtwCollector
{
    using System;
    using Microsoft.Diagnostics.Tracing;

    /// <summary>
    /// Represents a request to listen to specific ETW provider.
    /// </summary>
    public class EtwListeningRequest
    {
        /// <summary>
        /// Gets or sets the provider name to listen to.
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// Gets or sets the guid of the provider to listen to.
        /// </summary>
        public Guid ProviderGuid { get; set; }

        /// <summary>
        /// Gets or sets the minimum level of an event that will be traced. 
        /// </summary>
        /// <remarks>
        /// Events with level lower than the specified level will be silently discarded.
        /// </remarks>
        public TraceEventLevel Level { get; set; }

        /// <summary>
        /// Gets or sets the keywords that must be set on an event to be included in tracing.
        /// </summary>
        public ulong Keywords { get; set; }

        /// <summary>
        /// Verify this request is valid.
        /// </summary>
        /// <param name="errorMessage">Error message to display in case the request is invalid.</param>
        /// <returns>True if the request is valid, otherwise false.</returns>
        /// <exception cref="ArgumentNullException">Throws when the object is not valid.</exception>
        public bool Validate(out string errorMessage)
        {
            if (this.ProviderGuid == Guid.Empty && string.IsNullOrEmpty(this.ProviderName))
            {
                errorMessage = "ProviderGuid and ProviderName can't be null at the same time.";
                return false;
            }

            errorMessage = null;
            return true;
        }
    }
}
