// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Microsoft.IdentityModel.Abstractions
{
    /// <summary>
    /// The default implementation of the <see cref="ITelemetryClient"/> interface which swallows all telemetry signals.
    /// </summary>
    public class NullTelemetryClient : ITelemetryClient
    {
        /// <inheritdoc />
        public string ClientId { get; set; }

        /// <summary>
        /// Singleton instance of <see cref="NullTelemetryClient"/>.
        /// </summary>
        public static NullTelemetryClient Instance { get; } = new NullTelemetryClient();

        /// <summary>
        /// Initializes an instance of <see cref="NullTelemetryClient"/>.
        /// </summary>
        /// <remarks>
        /// Private constructor to prevent the default constructor being exposed.
        /// </remarks>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private NullTelemetryClient() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        /// <inheritdoc />
        public bool IsEnabled() => false;

        /// <inheritdoc/>
        public bool IsEnabled(string eventName) => false;

        /// <inheritdoc/>
        public void Initialize()
        {
            // no-op
        }

        /// <inheritdoc/>
        public void TrackEvent(TelemetryEventDetails eventDetails)
        {
            // no-op
        }

        /// <inheritdoc/>
        public void TrackEvent(
            string eventName,
            IDictionary<string, string>? stringProperties = null,
            IDictionary<string, long>? longProperties = null,
            IDictionary<string, bool>? boolProperties = null,
            IDictionary<string, DateTime>? dateTimeProperties = null,
            IDictionary<string, double>? doubleProperties = null,
            IDictionary<string, Guid>? guidProperties = null)
        {
            // no-op
        }
    }
}
