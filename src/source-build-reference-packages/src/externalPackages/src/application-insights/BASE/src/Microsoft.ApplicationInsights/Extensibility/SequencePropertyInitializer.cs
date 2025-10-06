namespace Microsoft.ApplicationInsights.Extensibility
{
    using System;
    using System.Globalization;
    using System.Threading;
    using Microsoft.ApplicationInsights.Channel;

    /// <summary>
    /// An <see cref="ITelemetryInitializer"/> that populates <see cref="ITelemetry.Sequence"/> property for 
    /// the Microsoft internal telemetry sent to the Vortex endpoint.
    /// </summary>
    public sealed class SequencePropertyInitializer : ITelemetryInitializer
    {
        // https://microsoft.sharepoint.com/teams/CommonSchema/Shared%20Documents/Schema%20Specs/Common%20Schema%202%20-%20Language%20Specification.docx
        private readonly string stablePrefix = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).TrimEnd('=') + ":";
        private long currentNumber;

        /// <summary>
        /// Populates <see cref="ITelemetry.Sequence"/> with unique ID and sequential number.
        /// </summary>
        public void Initialize(ITelemetry telemetry)
        {
            if (telemetry == null)
            {
                throw new ArgumentNullException(nameof(telemetry));
            }

            if (string.IsNullOrEmpty(telemetry.Sequence))
            {
                telemetry.Sequence = this.stablePrefix + Interlocked.Increment(ref this.currentNumber).ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}
