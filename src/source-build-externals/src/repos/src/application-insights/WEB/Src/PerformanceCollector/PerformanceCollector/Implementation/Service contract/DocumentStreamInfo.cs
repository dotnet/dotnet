namespace Microsoft.ApplicationInsights.Extensibility.Filtering
{
    using System.Runtime.Serialization;

    [DataContract]
    internal class DocumentStreamInfo
    {
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets an OR-connected collection of filter groups.
        /// </summary>
        /// <remarks>
        /// Each DocumentFilterConjunctionGroupInfo has a TelemetryType.
        /// Telemetry types that are not mentioned in this array will NOT be included in the stream.
        /// Telemetry types that are mentioned in this array once or more will be included if any of the mentioning DocumentFilterConjunctionGroupInfo's pass.
        /// </remarks>
        [DataMember]
        public DocumentFilterConjunctionGroupInfo[] DocumentFilterGroups { get; set; }
    }
}