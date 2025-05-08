namespace Microsoft.ApplicationInsights.Channel
{
    using System;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    /// <summary>
    /// Event argument to track response from ingestion endpoint.
    /// </summary>
    public class TransmissionStatusEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransmissionStatusEventArgs"/> class.
        /// </summary>
        /// <param name="response">Response from ingestion endpoint.</param>
        [ObsoleteAttribute("This constructor is deprecated. Please use a constructor that accepts response and responseDurationInMs instead.", false)]
        public TransmissionStatusEventArgs(HttpWebResponseWrapper response) : this(response, default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransmissionStatusEventArgs"/> class.
        /// </summary>
        /// <param name="response">Response from ingestion endpoint.</param>
        /// <param name="responseDurationInMs">Response duration in milliseconds.</param>
        public TransmissionStatusEventArgs(HttpWebResponseWrapper response, long responseDurationInMs)
        {
            this.Response = response;
            this.ResponseDurationInMs = responseDurationInMs;
        }

        /// <summary>
        /// Gets the response from ingestion endpoint.
        /// </summary>
        public HttpWebResponseWrapper Response { get; }

        /// <summary>
        /// Gets response duration in milliseconds.
        /// </summary>
        public long ResponseDurationInMs { get; }
    }
}
