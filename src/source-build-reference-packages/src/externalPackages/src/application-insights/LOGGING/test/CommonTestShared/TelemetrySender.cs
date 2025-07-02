namespace Microsoft.ApplicationInsights.CommonTestShared
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    internal static class TelemetrySender
    {
        /// <summary>
        /// Upload item to Validate endpoint.
        /// </summary>
        /// <param name="telemetryItem">Telemetry item to validate.</param>
        /// <returns>Empty string if no errors found. Response if validation failed.</returns>
        public static string ValidateEndpointSend(ITelemetry telemetryItem)
        {
            telemetryItem.Context.InstrumentationKey = "fafa4b10-03d3-4bb0-98f4-364f0bdf5df8";

            string response = null;

            string json = Encoding.UTF8.GetString(JsonSerializer.Serialize(new List<ITelemetry> { telemetryItem }, false));

            HttpClient client = new HttpClient();
            var result = client.PostAsync(
                new Uri("https://dc.services.visualstudio.com/v2/validate"),
                new ByteArrayContent(Encoding.UTF8.GetBytes(json))).GetAwaiter().GetResult();

            if (result.StatusCode != HttpStatusCode.OK)
            {
                response = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                Trace.WriteLine(response);
            }

            return response;
        }
    }
}
