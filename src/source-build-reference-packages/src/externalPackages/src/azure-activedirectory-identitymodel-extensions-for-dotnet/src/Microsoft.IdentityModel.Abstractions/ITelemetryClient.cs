// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Microsoft.IdentityModel.Abstractions
{
    /// <summary>
    /// Interface for Telemetry tracking.
    /// </summary>
    public interface ITelemetryClient
    {
        /// <summary>
        /// Gets or sets the application or client ID that telemetry is being sent for.
        /// </summary>
        string ClientId { get; set; }

        /// <summary>
        /// Perform any necessary bootstrapping for the telemetry client.
        /// </summary>
        /// <remarks>
        /// The expectation is that this should only be called once for the lifetime of the object however the
        /// implementation should be idempotent.
        /// </remarks>
        void Initialize();

        /// <summary>
        /// Checks to see if telemetry is enabled all up.
        /// </summary>
        /// <returns>
        /// Returns <see langword="true"/> if telemetry should be sent; <see langword="false"/> otherwise.
        /// </returns>
        /// <remarks>
        /// This check should be used to gate any resource intensive operations to generate telemetry as well.
        /// </remarks>
        bool IsEnabled();

        /// <summary>
        /// Checks to see if telemetry is enabled for the named event.
        /// </summary>
        /// <param name="eventName">Name of the event to check.</param>
        /// <returns>
        /// Returns <see langword="true"/> if telemetry should be sent for <paramref name="eventName"/>;
        /// <see langword="false"/> otherwise.
        /// </returns>
        /// <remarks>
        /// This check should be used to gate any resource intensive operations to generate telemetry as well.
        /// </remarks>
        bool IsEnabled(string eventName);

        /// <summary>
        /// Tracks an instance of a named event.
        /// </summary>
        /// <param name="eventDetails">Details of the event to track.</param>
        void TrackEvent(
            TelemetryEventDetails eventDetails);

        /// <summary>
        /// Tracks an instance of a named event.
        /// </summary>
        /// <param name="eventName">Name of the event to track. Should be unique per scenario.</param>
        /// <param name="stringProperties">Key value pair of strings to long with the event.</param>
        /// <param name="longProperties">Key value pair of longs to long with the event.</param>
        /// <param name="boolProperties">Key value pair of bools to long with the event.</param>
        /// <param name="dateTimeProperties">Key value pair of DateTimes to long with the event.</param>
        /// <param name="doubleProperties">Key value pair of doubles to long with the event.</param>
        /// <param name="guidProperties">Key value pair of Guids to long with the event.</param>
        void TrackEvent(
            string eventName,
            IDictionary<string, string>? stringProperties = null,
            IDictionary<string, long>? longProperties = null,
            IDictionary<string, bool>? boolProperties = null,
            IDictionary<string, DateTime>? dateTimeProperties = null,
            IDictionary<string, double>? doubleProperties = null,
            IDictionary<string, Guid>? guidProperties = null);
    }
}
