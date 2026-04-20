namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.Mocks
{
    using System.Collections.Generic;
    using System.Globalization;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule;
    using TestFramework;

    internal class DiagnosticsSenderMock : IDiagnosticsSender
    {
        public IList<string> Messages = new List<string>();

        public void Send(TraceEvent eventData)
        {
            var message = eventData.Payload != null && eventData.Payload.Length > 0 ?
                string.Format(CultureInfo.InvariantCulture, eventData.MetaData.MessageFormat, eventData.Payload) :
                eventData.MetaData.MessageFormat;

            this.Messages.Add(message);
        }
    }
}
