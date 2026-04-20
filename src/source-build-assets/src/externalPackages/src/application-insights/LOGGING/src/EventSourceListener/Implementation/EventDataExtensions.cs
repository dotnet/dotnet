//-----------------------------------------------------------------------
// <copyright file="EventDataExtensions.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.EventSourceListener.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Tracing;
    using System.Globalization;
    using System.Linq;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.TraceEvent.Shared.Utilities;

    /// <summary>
    /// Extension methods to convert EventSource structures to Application Insights telemetry.
    /// </summary>
    public static class EventDataExtensions
    {
        private const string ProviderNameProperty = "ProviderName";
        private const string ProviderGuidProperty = "ProviderGuid";

        private static Lazy<Random> random = new Lazy<Random>();

        private static SeverityLevel[] eventLevelToSeverityLevel = new SeverityLevel[]
        {
            SeverityLevel.Critical,     // EventLevel.LogAlways == 0
            SeverityLevel.Critical,     // EventLevel.Critical == 1
            SeverityLevel.Error,        // EventLevel.Error == 2
            SeverityLevel.Warning,      // EventLevel.Warning == 3
            SeverityLevel.Information,  // EventLevel.Informational == 4
            SeverityLevel.Verbose,      // EventLevel.Verbose == 5
        };

        /// <summary>
        /// Creates a TraceTelemetry out of an EventSource event.
        /// </summary>
        /// <param name="eventSourceEvent">The source for the telemetry data.</param>
        public static TraceTelemetry CreateTraceTelementry(this EventWrittenEventArgs eventSourceEvent)
        {
            if (eventSourceEvent == null)
            {
                throw new ArgumentNullException(nameof(eventSourceEvent));
            }

            string formattedMessage = null;
            if (eventSourceEvent.Message != null)
            {
                try
                {
                    // If the event has a badly formatted manifest, message formatting might fail
                    formattedMessage = string.Format(CultureInfo.InvariantCulture, eventSourceEvent.Message, eventSourceEvent.Payload.ToArray());
                }
                catch
                {
                }
            }

            return new TraceTelemetry(
                formattedMessage,
                eventLevelToSeverityLevel[(int)eventSourceEvent.Level]);
        }

        /// <summary>
        /// Populates a standard set of properties on the <see cref="TraceTelemetry"/> with values from the a given EventSource event.
        /// </summary>
        /// <param name="telemetry">Telemetry item to populate with properties.</param>
        /// <param name="eventSourceEvent">Event to extract values from.</param>
        public static TraceTelemetry PopulateStandardProperties(this TraceTelemetry telemetry, EventWrittenEventArgs eventSourceEvent)
        {
            if (telemetry == null)
            {
                throw new ArgumentNullException(nameof(telemetry));
            }

            if (eventSourceEvent == null)
            {
                throw new ArgumentNullException(nameof(eventSourceEvent));
            }

            if (!string.IsNullOrWhiteSpace(eventSourceEvent.EventSource.Name))
            {
                telemetry.AddProperty(ProviderNameProperty, eventSourceEvent.EventSource.Name);
            }

            if (eventSourceEvent.EventSource.Guid != Guid.Empty)
            {
                telemetry.AddProperty(ProviderGuidProperty, eventSourceEvent.EventSource.Guid.ToString());
            }

            telemetry.AddProperty(nameof(EventWrittenEventArgs.EventId), eventSourceEvent.EventId.ToString(CultureInfo.InvariantCulture));
            telemetry.AddProperty(nameof(EventWrittenEventArgs.EventName), eventSourceEvent.EventName);
            if (eventSourceEvent.ActivityId != default(Guid))
            {
                telemetry.AddProperty(nameof(EventWrittenEventArgs.ActivityId), ActivityPathDecoder.GetActivityPathString(eventSourceEvent.ActivityId));
            }

            if (eventSourceEvent.RelatedActivityId != default(Guid))
            {
                telemetry.AddProperty(nameof(EventWrittenEventArgs.RelatedActivityId), ActivityPathDecoder.GetActivityPathString(eventSourceEvent.RelatedActivityId));
            }

            telemetry.AddProperty(nameof(EventWrittenEventArgs.Channel), eventSourceEvent.Channel.GetChannelName());
            telemetry.AddProperty(nameof(EventWrittenEventArgs.Keywords), GetHexRepresentation((long)eventSourceEvent.Keywords));
            telemetry.AddProperty(nameof(EventWrittenEventArgs.Opcode), eventSourceEvent.Opcode.GetOpcodeName());
            if (eventSourceEvent.Tags != EventTags.None)
            {
                telemetry.AddProperty(nameof(EventWrittenEventArgs.Tags), GetHexRepresentation((int)eventSourceEvent.Tags));
            }

            if (eventSourceEvent.Task != EventTask.None)
            {
                telemetry.AddProperty(nameof(EventWrittenEventArgs.Task), GetHexRepresentation((int)eventSourceEvent.Task));
            }

            return telemetry;
        }

        /// <summary>
        /// Populates properties on the <see cref="TraceTelemetry"/> with values from the Payload of a given EventSource event.
        /// </summary>
        /// <param name="telemetry">Telemetry item to populate with properties.</param>
        /// <param name="eventSourceEvent">Event to extract values from.</param>
        public static TraceTelemetry PopulatePayloadProperties(this TraceTelemetry telemetry, EventWrittenEventArgs eventSourceEvent)
        {
            if (eventSourceEvent == null)
            {
                throw new ArgumentNullException(nameof(eventSourceEvent));
            }

            if (telemetry == null)
            {
                throw new ArgumentNullException(nameof(telemetry));
            }

            if (eventSourceEvent.Payload == null || eventSourceEvent.PayloadNames == null)
            {
                return telemetry;
            }

            IDictionary<string, string> payloadData = telemetry.Properties;

            IEnumerator<object> payloadEnumerator = eventSourceEvent.Payload.GetEnumerator();
            IEnumerator<string> payloadNamesEnunmerator = eventSourceEvent.PayloadNames.GetEnumerator();
            while (payloadEnumerator.MoveNext())
            {
                payloadNamesEnunmerator.MoveNext();
                if (payloadEnumerator.Current != null)
                {
                    payloadData.Add(payloadNamesEnunmerator.Current, payloadEnumerator.Current.ToString());
                }
            }

            return telemetry;
        }

        /// <summary>
        /// Creates a TraceTelemetry out of an EventSource event and tracks it using the supplied client.
        /// </summary>
        /// <param name="eventSourceEvent">The source for the telemetry data.</param>
        /// <param name="client">Client to track the data with.</param>
        internal static void Track(this EventWrittenEventArgs eventSourceEvent, TelemetryClient client)
        {
            Debug.Assert(client != null, "Should always receive a valid client");

            var telemetry = eventSourceEvent.CreateTraceTelementry()
                .PopulatePayloadProperties(eventSourceEvent)
                .PopulateStandardProperties(eventSourceEvent);

            client.Track(telemetry);
        }

        /// <summary>
        /// Adds a property to a telemetry item.
        /// </summary>
        /// <param name="telemetry">Telemetry item that receives a new property.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Property value.</param>
        /// <remarks>There is a potential of naming conflicts between standard ETW properties (like Keywords, Channel)
        /// and properties that are part of EventSource event payload. Because both end up in the same ITelemetry.Properties dictionary,
        /// we need some sort of conflict resolution. In this implementation we err on the side of preserving names that are part of EventSource event payload
        /// because they are usually the "interesting" properties, specific to the application. If there is a conflict with standard properties,
        /// we make the standard property name unique by appending a random numeric suffix.</remarks>
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
                newKey += EventDataExtensions.random.Value.Next(0, 10).ToString(CultureInfo.InvariantCulture);
            }
            while (properties.ContainsKey(newKey));

            properties.Add(newKey, value);
        }

        /// <summary>
        /// Returns a string representation of an EventChannel.
        /// </summary>
        /// <param name="channel">The channel to get a name for.</param>
        /// <returns>Name of the channel (or a numeric string, if standard name is not known).</returns>
        /// <remarks>Enum.GetName() could be used but it is using reflection and because of that it is an order of magnitude less efficient.</remarks>
        private static string GetChannelName(this EventChannel channel)
        {
            switch (channel)
            {
                case EventChannel.None: return nameof(EventChannel.None);
                case EventChannel.Admin: return nameof(EventChannel.Admin);
                case EventChannel.Operational: return nameof(EventChannel.Operational);
                case EventChannel.Analytic: return nameof(EventChannel.Analytic);
                case EventChannel.Debug: return nameof(EventChannel.Debug);
                default: return channel.ToString();
            }
        }

        /// <summary>
        /// Returns a string representation of an operation code.
        /// </summary>
        /// <param name="opcode">The operation code to get a name for.</param>
        /// <returns>Name of the operation code (or a numeric string, if standard name is not known).</returns>
        /// <remarks>Enum.GetName() could be used but it is using reflection and because of that it is an order of magnitude less efficient.</remarks>
        private static string GetOpcodeName(this EventOpcode opcode)
        {
            switch (opcode)
            {
                case EventOpcode.Info: return nameof(EventOpcode.Info);
                case EventOpcode.Start: return nameof(EventOpcode.Start);
                case EventOpcode.Stop: return nameof(EventOpcode.Stop);
                case EventOpcode.DataCollectionStart: return nameof(EventOpcode.DataCollectionStart);
                case EventOpcode.DataCollectionStop: return nameof(EventOpcode.DataCollectionStop);
                case EventOpcode.Extension: return nameof(EventOpcode.Extension);
                case EventOpcode.Reply: return nameof(EventOpcode.Reply);
                case EventOpcode.Resume: return nameof(EventOpcode.Resume);
                case EventOpcode.Suspend: return nameof(EventOpcode.Suspend);
                case EventOpcode.Send: return nameof(EventOpcode.Send);
                case EventOpcode.Receive: return nameof(EventOpcode.Receive);
                default: return opcode.ToString();
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
    }
}