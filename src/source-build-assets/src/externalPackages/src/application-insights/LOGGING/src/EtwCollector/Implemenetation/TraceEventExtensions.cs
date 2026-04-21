//-----------------------------------------------------------------------
// <copyright file="TraceEventExtensions.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.EtwCollector.Implemenetation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.TraceEvent.Shared.Utilities;
    using Microsoft.Diagnostics.Tracing;

    internal static class TraceEventExtensions
    {
        private static Lazy<Random> random = new Lazy<Random>();

        private static SeverityLevel[] traceEventLevelToSeverityLevel = new SeverityLevel[]
        {
            SeverityLevel.Critical,     // TraceEventLevel.Always == 0
            SeverityLevel.Critical,     // TraceEventLevel.Critical == 1
            SeverityLevel.Error,        // TraceEventLevel.Error == 2
            SeverityLevel.Warning,      // TraceEventLevel.Warning == 3
            SeverityLevel.Information,  // TraceEventLevel.Informational == 4
            SeverityLevel.Verbose, // TraceEventLevel.Verbose == 5
        };

        public static void Track(this TraceEvent traceEvent, TelemetryClient client)
        {
            Debug.Assert(client != null, "Should always receive a valid client");

            string formattedMessage = traceEvent.FormattedMessage;
            TraceTelemetry telemetry = new TraceTelemetry(formattedMessage, traceEventLevelToSeverityLevel[(int)traceEvent.Level]);

            telemetry.AddProperty("EventId", traceEvent.ID.ToString());
            telemetry.AddProperty(nameof(traceEvent.EventName), traceEvent.EventName);
            if (traceEvent.ActivityID != default(Guid))
            {
                telemetry.AddProperty(nameof(traceEvent.ActivityID), ActivityPathDecoder.GetActivityPathString(traceEvent.ActivityID));
            }

            if (traceEvent.RelatedActivityID != default(Guid))
            {
                telemetry.AddProperty(nameof(traceEvent.RelatedActivityID), traceEvent.RelatedActivityID.ToString());
            }

            telemetry.AddProperty(nameof(traceEvent.Channel), traceEvent.Channel.ToString());
            telemetry.AddProperty(nameof(traceEvent.Keywords), GetHexRepresentation((long)traceEvent.Keywords));
            telemetry.AddProperty(nameof(traceEvent.Opcode), traceEvent.Opcode.ToString());
            if (traceEvent.Task != TraceEventTask.Default)
            {
                telemetry.AddProperty(nameof(traceEvent.Task), GetHexRepresentation((int)traceEvent.Task));
            }

            // Make this the call after adding traceEvent properties that property name on trace event will take the priority in possible duplicated property scenario.
            traceEvent.ExtractPayloadData(telemetry);

            client.Track(telemetry);
        }

        private static void ExtractPayloadData(this TraceEvent traceEvent, TraceTelemetry telemetry)
        {
            Debug.Assert(telemetry != null, "Should have received a valid TraceTelemetry object");

            if (traceEvent.PayloadNames == null)
            {
                return;
            }

            foreach (string payloadName in traceEvent.PayloadNames)
            {
                telemetry.AddProperty(payloadName, traceEvent.PayloadByName(payloadName).ToString());
            }
        }

        private static string GetHexRepresentation(long l)
        {
            return "0x" + l.ToString("X16", CultureInfo.InvariantCulture);
        }

        private static string GetHexRepresentation(int i)
        {
            return "0x" + i.ToString("X8", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Adds a property to a telemetry item.
        /// </summary>
        /// <param name="telemetry">Telemetry item that receives a new property.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Property value.</param>
        /// <remarks>There is a potential of naming conflicts between standard ETW properties (like Keywords, Channel)
        /// and properties that are part of EventSource event payload. Because both end up in the same ITelemetry.Properties dictionary,
        /// we need some sort of conflict resolution. When calling into this method, property name will be suffixed with a random number when duplicated name exists.</remarks>
        private static void AddProperty(this TraceTelemetry telemetry, string name, string value)
        {
            Debug.Assert(!string.IsNullOrEmpty(name), "Property name should always be specified");

            IDictionary<string, string> properties = telemetry.Properties;
            if (!properties.ContainsKey(name))
            {
                properties.Add(name, value);
                return;
            }

            string newKey = name + "_";

            // Update property key till there is no such key in dict
            do
            {
                newKey += TraceEventExtensions.random.Value.Next(0, 10).ToString(CultureInfo.InvariantCulture);
            }
            while (properties.ContainsKey(newKey));

            properties.Add(newKey, value);
        }
    }
}
