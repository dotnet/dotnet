namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.Net;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    internal class TransmissionProcessedEventArgs : EventArgs
    {
        public TransmissionProcessedEventArgs(Transmission transmission, Exception exception = null, HttpWebResponseWrapper response = null)
        {
            this.Transmission = transmission;
            this.Exception = exception;
            this.Response = response;
        }

        public Transmission Transmission { get; }

        public Exception Exception { get; }

        public HttpWebResponseWrapper Response { get; protected set; }
    }
}