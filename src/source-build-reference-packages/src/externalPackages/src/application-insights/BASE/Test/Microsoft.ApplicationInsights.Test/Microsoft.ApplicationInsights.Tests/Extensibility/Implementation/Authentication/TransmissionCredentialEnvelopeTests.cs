#if !NET452 && !NET46 && !REDFIELD
namespace Microsoft.ApplicationInsights.TestFramework.Extensibility.Implementation.Authentication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Authentication;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// These tests verify that <see cref="Transmission"/> can receive and store an instance of <see cref="Azure.Core.TokenCredential"/>.
    /// </summary>
    /// <remarks>
    /// These tests do not run in NET452 OR NET46. 
    /// In these cases, the test runner is NET452 or NET46 and Azure.Core.TokenCredential is NOT SUPPORTED in these frameworks.
    /// This does not affect the end user because we REQUIRE the end user to create their own instance of TokenCredential.
    /// This ensures that the end user is consuming the AI SDK in one of the newer frameworks.
    /// </remarks>
    [TestClass]
    [TestCategory("AAD")]
    public class TransmissionCredentialEnvelopeTests
    {
        private readonly Uri testUri = new Uri("https://127.0.0.1/");

        [TestMethod]
        public async Task VerifyTransmissionSendAsync_Default()
        {
            var handler = new HandlerForFakeHttpClient
            {
                InnerHandler = new HttpClientHandler(),
                OnSendAsync = (req, cancellationToken) =>
                {
                    // VALIDATE
                    Assert.IsNull(req.Headers.Authorization);

                    return Task.FromResult<HttpResponseMessage>(new HttpResponseMessage());
                }
            };

            using (var fakeHttpClient = new HttpClient(handler))
            {
                var expectedContentType = "content/type";
                var expectedContentEncoding = "contentEncoding";
                var items = new List<ITelemetry> { new EventTelemetry() };

                // Instantiate Transmission with the mock HttpClient
                var transmission = new Transmission(testUri, new byte[] { 1, 2, 3, 4, 5 }, fakeHttpClient, expectedContentType, expectedContentEncoding);

                var result = await transmission.SendAsync();
            }
        }

        [TestMethod]
        public async Task VerifyTransmissionSendAsync_WithCredential_SetsAuthHeader()
        {
            var credentialEnvelope = new ReflectionCredentialEnvelope(new MockCredential());
            var authToken = credentialEnvelope.GetToken();

            var handler = new HandlerForFakeHttpClient
            {
                InnerHandler = new HttpClientHandler(),
                OnSendAsync = (req, cancellationToken) =>
                {
                    // VALIDATE
                    Assert.AreEqual(AuthConstants.AuthorizationTokenPrefix.Trim(), req.Headers.Authorization.Scheme);
                    Assert.AreEqual(authToken.Token, req.Headers.Authorization.Parameter);
                    
                    return Task.FromResult<HttpResponseMessage>(new HttpResponseMessage());
                }
            };

            using (var fakeHttpClient = new HttpClient(handler))
            {
                var expectedContentType = "content/type";
                var expectedContentEncoding = "contentEncoding";
                var items = new List<ITelemetry> { new EventTelemetry() };

                // Instantiate Transmission with the mock HttpClient
                var transmission = new Transmission(testUri, new byte[] { 1, 2, 3, 4, 5 }, fakeHttpClient, expectedContentType, expectedContentEncoding);
                transmission.CredentialEnvelope = credentialEnvelope;

                var result = await transmission.SendAsync();
            }
        }
    }
}
#endif
