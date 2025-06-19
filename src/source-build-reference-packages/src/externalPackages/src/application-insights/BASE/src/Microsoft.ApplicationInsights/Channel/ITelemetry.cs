namespace Microsoft.ApplicationInsights.Channel
{
    using System;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;

    /// <summary>
    /// The base telemetry type for application insights.
    /// </summary>
    public interface ITelemetry
    {
        /// <summary>
        /// Gets or sets date and time when telemetry was recorded.
        /// </summary>
        DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// Gets the context associated with this telemetry instance.
        /// </summary>
        TelemetryContext Context { get; }

        /// <summary>
        /// Gets or sets gets the extension used to extend this telemetry instance using new strongly
        /// typed object.
        /// </summary>
        IExtension Extension { get; set; }

        /// <summary>
        /// Gets or sets the value that defines absolute order of the telemetry item.
        /// </summary>
        /// <remarks>
        /// The sequence is used to track absolute order of uploaded telemetry items. It is a two-part value that includes 
        /// a stable identifier for the current boot session and an incrementing identifier for each event added to the upload queue:
        /// For UTC this would increment for all events across the system.
        /// For Persistence this would increment for all events emitted from the hosting process.    
        /// The Sequence helps track how many events were fired and how many events were uploaded and enables identification 
        /// of data lost during upload and de-duplication of events on the ingress server.
        /// From <a href="https://microsoft.sharepoint.com/teams/CommonSchema/Shared%20Documents/Schema%20Specs/Common%20Schema%202%20-%20Language%20Specification.docx"/>.
        /// </remarks>
        string Sequence { get; set; }

        /// <summary>
        /// Sanitizes the properties of the telemetry item based on DP constraints.
        /// </summary>
        void Sanitize();

        /// <summary>
        /// Clones the telemetry object deeply, so that the original object and the clone share no state 
        /// and can be modified independently.
        /// </summary>
        /// <returns>The cloned object.</returns>
        ITelemetry DeepClone();
        
        /// <summary>
        /// Writes serialization info about the data class of the implementing type using the given <see cref="ISerializationWriter"/>.
        /// </summary>
        void SerializeData(ISerializationWriter serializationWriter);
    }
}
