// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.Security.AntiSSRF.FunctionalTests
{
    public class AntiSSRFPolicy_HeaderTests
    {
        private static readonly string TestDomain = "ambitious-flower-0611c910f.2.azurestaticapps.net";

        [Fact]
        public void BadInputs()
        {
            var policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1);

            // Invalid arrays
            Assert.Throws<ArgumentNullException>(() => policy.AddDeniedHeaders(null));
            Assert.Throws<ArgumentNullException>(() => policy.AddRequiredHeaders(null));

            // Invalid array elements
            Assert.Throws<ArgumentException>(() => policy.AddDeniedHeaders([""]));
            Assert.Throws<ArgumentException>(() => policy.AddRequiredHeaders([""]));
            Assert.Throws<ArgumentNullException>(() => policy.AddDeniedHeaders(["X-Valid-Header", null!, "Another-Header"]));
            Assert.Throws<ArgumentNullException>(() => policy.AddRequiredHeaders([null!, "X-Test-Header"]));
        }

        [Fact]
        public void CheckDefaults()
        {
            var policy = new AntiSSRFPolicy(PolicyConfigOptions.InternalOnly);
            var policy2 = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1);
            var policy3 = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyLatest);
            var policy4 = new AntiSSRFPolicy(PolicyConfigOptions.None);
            Assert.Empty(policy.RequiredHeaders);
            Assert.Empty(policy2.RequiredHeaders);
            Assert.Empty(policy3.RequiredHeaders);
            Assert.Empty(policy4.RequiredHeaders);
            Assert.Empty(policy.DeniedHeaders);
            Assert.Empty(policy2.DeniedHeaders);
            Assert.Empty(policy3.DeniedHeaders);
            Assert.Empty(policy4.DeniedHeaders);
        }

        [Fact]
        public void AddedHeaders_AppearInDeniedHeadersAndRequiredHeaders()
        {
            AntiSSRFPolicy policy = new(PolicyConfigOptions.None);

            policy.AddDeniedHeaders(["X-Denied-Header", "X-Another-Denied-Header"]);
            policy.AddRequiredHeaders(["X-Required-Header", "X-Another-Required-Header"]);

            IReadOnlyList<string> deniedHeaders = policy.DeniedHeaders;
            IReadOnlyList<string> requiredHeaders = policy.RequiredHeaders;

            Assert.Contains("X-Denied-Header", deniedHeaders);
            Assert.Contains("X-Another-Denied-Header", deniedHeaders);
            Assert.Contains("X-Required-Header", requiredHeaders);
            Assert.Contains("X-Another-Required-Header", requiredHeaders);
        }

        [Fact]
        public async Task RequiredHeader()
        {
            AntiSSRFPolicy policy = new(PolicyConfigOptions.None);
            policy.AddRequiredHeaders(["X-Test-Header"]);
            using HttpClient client = new(policy.GetHandler());
            string Url = $"https://{TestDomain}/api/header-check?header=X-Test-Header";

            using HttpRequestMessage request = new(HttpMethod.Get, Url);
            request.Headers.Add("Not-X-Test-Header", "test-value");
            await Assert.ThrowsAsync<AntiSSRFException>(() => client.SendAsync(request, CancellationToken.None));

            using HttpRequestMessage request2 = new(HttpMethod.Get, Url);
            request2.Headers.Add("X-Test-Header", "test-value");
            var response = await client.SendAsync(request2, CancellationToken.None);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeniedHeader()
        {
            AntiSSRFPolicy policy = new(PolicyConfigOptions.None);
            policy.AddDeniedHeaders(["X-Test-Header"]);
            using HttpClient client = new(policy.GetHandler());
            string url = $"https://{TestDomain}/";

            using HttpRequestMessage request = new(HttpMethod.Get, url);
            request.Headers.Add("X-Test-Header", "test-value");
            await Assert.ThrowsAsync<AntiSSRFException>(() => client.SendAsync(request, CancellationToken.None));

            using HttpRequestMessage request2 = new(HttpMethod.Get, url);
            request2.Headers.Add("Not-X-Test-Header", "test-value");
            var response = await client.SendAsync(request2, CancellationToken.None);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Headers_AreCaseInsensitive()
        {
            string url = $"https://{TestDomain}/api/header-check?header=x-test-header";

            AntiSSRFPolicy requiredPolicy = new(PolicyConfigOptions.None);
            requiredPolicy.AddRequiredHeaders(["X-Test-Header"]);
            using HttpClient requiredClient = new(requiredPolicy.GetHandler());

            using HttpRequestMessage requiredRequest = new(HttpMethod.Get, url);
            requiredRequest.Headers.Add("x-test-header", "test-value");
            var requiredResponse = await requiredClient.SendAsync(requiredRequest, CancellationToken.None);
            Assert.Equal(HttpStatusCode.OK, requiredResponse.StatusCode);

            AntiSSRFPolicy deniedPolicy = new(PolicyConfigOptions.None);
            deniedPolicy.AddDeniedHeaders(["X-Test-Header"]);
            using HttpClient deniedClient = new(deniedPolicy.GetHandler());

            using HttpRequestMessage deniedRequest = new(HttpMethod.Get, $"https://{TestDomain}/");
            deniedRequest.Headers.Add("x-test-header", "test-value");
            await Assert.ThrowsAsync<AntiSSRFException>(() => deniedClient.SendAsync(deniedRequest, CancellationToken.None));
        }

        [Fact]
        public async Task BothRequiredAndDenied()
        {
            AntiSSRFPolicy policy = new(PolicyConfigOptions.None);
            policy.AddRequiredHeaders(["X-Test-Header"]);
            policy.AddDeniedHeaders(["X-Test-Header"]);
            HttpClient client = new(policy.GetHandler());
            string Url = $"https://{TestDomain}/api/header-check?header=X-Test-Header";

            using HttpRequestMessage request = new(HttpMethod.Get, Url);
            request.Headers.Add("X-Test-Header", "test-value");
            await Assert.ThrowsAsync<AntiSSRFException>(() => client.SendAsync(request, CancellationToken.None));

            using HttpRequestMessage request2 = new(HttpMethod.Get, Url);
            request2.Headers.Add("Not-X-Test-Header", "test-value");
            await Assert.ThrowsAsync<AntiSSRFException>(() => client.SendAsync(request2, CancellationToken.None));
        }

        [Fact]
        public async Task WithAddXFFHeader()
        {
            string Url = $"https://{TestDomain}/api/header-check?header=X-Forwarded-For";

            AntiSSRFPolicy policy = new(PolicyConfigOptions.None)
            {
                AddXFFHeader = true
            };
            policy.AddRequiredHeaders(["X-Forwarded-For", "X-Test-Header"]);
            HttpClient client = new(policy.GetHandler());

            using HttpRequestMessage request = new(HttpMethod.Get, Url);
            request.Headers.Add("X-Test-Header", "test-value");
            var response = await client.SendAsync(request, CancellationToken.None);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            AntiSSRFPolicy policy2 = new(PolicyConfigOptions.None)
            {
                AddXFFHeader = true
            };
            policy2.AddDeniedHeaders(["X-Forwarded-For", "Not-X-Test-Header"]);
            HttpClient client2 = new(policy2.GetHandler());

            using HttpRequestMessage request2 = new(HttpMethod.Get, Url);
            request2.Headers.Add("X-Test-Header", "test-value");
            await Assert.ThrowsAsync<AntiSSRFException>(() => client2.SendAsync(request2, CancellationToken.None));
        }

        [Fact]
        public async Task HoldsOnRedirect()
        {
            var policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
            policy.AddRequiredHeaders(["X-Required-Header"]);
            HttpClient client = new(policy.GetHandler());

            using HttpRequestMessage req = new(HttpMethod.Get, $"https://{TestDomain}/api/redirect?num=3");
            req.Headers.Add("X-Required-Header", "test-value");

            // Should succeed since the required header is present and maintained through redirects
            var response = await client.SendAsync(req, CancellationToken.None);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task RequiredHeaderDroppedOnRedirect()
        {
            var policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
            policy.AddRequiredHeaders(["Authorization"]);
            HttpClient client = new(policy.GetHandler());

            using HttpRequestMessage req = new(HttpMethod.Get, $"https://{TestDomain}/api/redirect?num=3");
            req.Headers.Add("Authorization", "Bearer test-token");

            // Should fail because Authorization header gets dropped by HttpClient during redirects
            await Assert.ThrowsAsync<AntiSSRFException>(() => client.SendAsync(req, CancellationToken.None));
        }

        [Fact]
        public void NoEditsAfterHandler()
        {
            var policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1);
            policy.GetHandler();

            Assert.Throws<AntiSSRFException>(() => policy.AddRequiredHeaders(["X-Test-Header"]));
            Assert.Throws<AntiSSRFException>(() => policy.AddDeniedHeaders(["X-Test-Header"]));
        }
    }
}
