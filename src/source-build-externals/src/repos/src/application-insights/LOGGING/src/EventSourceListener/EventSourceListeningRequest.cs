//-----------------------------------------------------------------------
// <copyright file="EventSourceListeningRequest.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.EventSourceListener
{
    using System.Diagnostics.Tracing;

    /// <summary>
    /// Represents a request to listen to specific EventSource.
    /// </summary>
    public class EventSourceListeningRequest : EventSourceListeningRequestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventSourceListeningRequest"/> class.
        /// </summary>
        /// <remarks>
        /// By default all events from an EventSource are traced. The set of events can be restricted using <see cref="Level"/> and <see cref="Keywords"/> properties.
        /// </remarks>
        public EventSourceListeningRequest()
        {
            this.Level = EventLevel.LogAlways;
            this.Keywords = (EventKeywords)~0;
        }

        /// <summary>
        /// Gets or sets the minimum level of an event that will be traced.
        /// </summary>
        /// <remarks>
        /// Events with level lower than the specified level will be silently discarded.
        /// </remarks>
        public EventLevel Level { get; set; }

        /// <summary>
        /// Gets or sets the keywords that must be set on an event to be included in tracing.
        /// </summary>
        public EventKeywords Keywords { get; set; }

        /// <summary>
        /// Tests for equality.
        /// </summary>
        /// <param name="obj">Object to compare with.</param>
        /// <returns>True if the supplied object is equal to "this", otherwise false.</returns>
        public override bool Equals(object obj)
        {
            var other = obj as EventSourceListeningRequest;
            if (other == null)
            {
                return false;
            }

            return base.Equals(obj) && this.Level == other.Level && this.Keywords == other.Keywords;
        }

        /// <summary>
        /// Gets the hash code for the current listening request.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode() ^ (int)this.Level ^ this.Keywords.GetHashCode();
        }
    }
}
