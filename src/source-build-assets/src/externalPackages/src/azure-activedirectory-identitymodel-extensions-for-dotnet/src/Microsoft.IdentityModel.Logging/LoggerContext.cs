// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.IdentityModel.Logging
{
    /// <summary>
    /// A context class that can be used to store work per request to aid with debugging.
    /// </summary>
    public class LoggerContext
    {
        /// <summary>
        /// Instantiates a new <see cref="LoggerContext"/> with the default activityId.
        /// </summary>
        public LoggerContext()
        {
        }

        /// <summary>
        /// Instantiates a new <see cref="LoggerContext"/> with an activityId.
        /// </summary>
        /// <param name="activityId"></param>
        public LoggerContext(Guid activityId)
        {
            ActivityId = activityId;
        }

        /// <summary>
        /// Gets or set a <see cref="Guid"/> that will be used in the call to EventSource.SetCurrentThreadActivityId before logging.
        /// </summary>
        public Guid ActivityId { get; set; } = Guid.Empty;

        /// <summary>
        /// Gets or sets a boolean controlling if logs are written into the context.
        /// Useful when debugging.
        /// </summary>
        public bool CaptureLogs { get; set; } = false;

        /// <summary>
        /// Gets or sets a string that helps with setting breakpoints when debugging.
        /// </summary>
        public virtual string DebugId { get; set; } = string.Empty;

        /// <summary>
        /// The collection of logs associated with a request. Use <see cref="CaptureLogs"/> to control capture.
        /// </summary>
        public ICollection<string> Logs { get; private set; } = new Collection<string>();

        /// <summary>
        /// Gets or sets an <see cref="IDictionary{String, Object}"/> that enables custom extensibility scenarios.
        /// </summary>
        public IDictionary<string, object> PropertyBag { get; set; }
    }
}
