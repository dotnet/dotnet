namespace Microsoft.ApplicationInsights.DependencyCollector
{
    using System;
    using System.Collections.Concurrent;
    using System.Net;
    using System.Net.Http;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class OperationDetailsInitializer : ITelemetryInitializer
    {
        private readonly ConcurrentDictionary<DependencyTelemetry, Tuple<object, object, object>> operationDetails =
            new ConcurrentDictionary<DependencyTelemetry, Tuple<object, object, object>>();

        public void Initialize(ITelemetry telemetry)
        {
            if (telemetry is DependencyTelemetry dependency)
            {
                dependency.TryGetOperationDetail(OperationDetailConstants.HttpRequestOperationDetailName, out var request);
                dependency.TryGetOperationDetail(OperationDetailConstants.HttpResponseOperationDetailName, out var response);
                dependency.TryGetOperationDetail(OperationDetailConstants.HttpResponseHeadersOperationDetailName, out var responseHeaders);

                var newDetails = new Tuple<object, object, object>(request, response, responseHeaders);
                this.operationDetails.AddOrUpdate(dependency, newDetails, (d, o) => newDetails);
            }
        }

        public void ValidateOperationDetailsCore(DependencyTelemetry telemetry, bool responseExpected = true)
        {
            Assert.IsTrue(this.TryGetDetails(telemetry, out var request, out var response, out var responseHeaders));

            Assert.IsNotNull(request, "Request was not present and expected.");
            Assert.IsNotNull(request as HttpRequestMessage, "Request was not the expected type.");
            Assert.IsNull(responseHeaders, "Response headers were present and not expected.");
            if (responseExpected)
            {
                Assert.IsNotNull(response, "Response was not present and expected.");
                Assert.IsNotNull(response as HttpResponseMessage, "Response was not the expected type.");
            }
            else
            {
                Assert.IsNull(response, "Response was present and not expected.");
            }
        }

        public void ValidateOperationDetailsDesktop(DependencyTelemetry telemetry, bool responseExpected = true, bool headersExpected = false)
        {
            Assert.IsTrue(this.TryGetDetails(telemetry, out var request, out var response, out var responseHeaders));

            Assert.IsNotNull(request, "Request was not present and expected.");
            Assert.IsNotNull(request as HttpWebRequest, "Request was not the expected type.");

            if (responseExpected)
            {
                Assert.IsNotNull(response, "Response was not present and expected.");
                Assert.IsNotNull(response as HttpWebResponse, "Response was not the expected type.");
            }
            else
            {
                Assert.IsNull(response, "Response was present and not expected.");
            }

            if (headersExpected)
            {
                Assert.IsNotNull(responseHeaders, "Response headers were not present and expected.");
                Assert.IsNotNull(responseHeaders as WebHeaderCollection, "Response headers were not the expected type.");
            }
            else
            {
                Assert.IsNull(responseHeaders, "Response headers were present and not expected.");
            }
        }

        private bool TryGetDetails(DependencyTelemetry depednency, out object request, out object response,
            out object responseHeaders)
        {
            request = response = responseHeaders = null;
            if (this.operationDetails.TryGetValue(depednency, out var tuple))
            {
                request = tuple.Item1;
                response = tuple.Item2;
                responseHeaders = tuple.Item3;
                return true;
            }

            return false;
        }
    }
}
