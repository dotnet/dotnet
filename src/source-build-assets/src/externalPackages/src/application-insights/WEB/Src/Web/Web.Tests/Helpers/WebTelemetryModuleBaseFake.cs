namespace Microsoft.ApplicationInsights.Web.Helpers
{
    using System.Web;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Web.Implementation;

    internal class WebTelemetryModuleBaseFake : WebTelemetryModuleBase
    {
        public bool OnBeginRequestCalled { get; set; }

        public bool OnEndRequestCalled { get; set; }

        public bool OnErrorCalled { get; set; }

        public override void OnBeginRequest(
            RequestTelemetry requestTelemetry,
            HttpContext platformContext)
        {
            this.OnBeginRequestCalled = true;
        }

        public override void OnEndRequest(
            RequestTelemetry requestTelemetry,
            HttpContext platformContext)
        {
            this.OnEndRequestCalled = true;
        }

        public override void OnError(
            RequestTelemetry requestTelemetry,
            HttpContext platformContext)
        {
            this.OnErrorCalled = true;
        }
    }
}
