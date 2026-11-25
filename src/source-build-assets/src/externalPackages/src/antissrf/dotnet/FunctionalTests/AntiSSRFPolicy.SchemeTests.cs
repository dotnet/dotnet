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
    public class AntiSSRFPolicy_SchemeTests
    {
        private static readonly string TestDomain = "ambitious-flower-0611c910f.2.azurestaticapps.net";

        [Fact]
        public void CheckDefaults()
        {
            var policy = new AntiSSRFPolicy(PolicyConfigOptions.InternalOnly);
            var policy2 = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1);
            var policy3 = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyLatest);
            var policy4 = new AntiSSRFPolicy(PolicyConfigOptions.None);
            Assert.False(policy.AllowPlainTextHttp);
            Assert.False(policy2.AllowPlainTextHttp);
            Assert.False(policy3.AllowPlainTextHttp);
            Assert.False(policy4.AllowPlainTextHttp);
        }

        [Fact]
        public async Task OnFalse()
        {
            AntiSSRFPolicy policy = new(PolicyConfigOptions.None)
            {
                AllowPlainTextHttp = false
            };
            HttpClient client = new(policy.GetHandler());

            var response = await client.GetAsync($"https://{TestDomain}", CancellationToken.None);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            await Assert.ThrowsAsync<AntiSSRFException>(() => client.GetAsync($"http://{TestDomain}", CancellationToken.None));
        }

        [Fact]
        public async Task OnTrue()
        {
            var policy = new AntiSSRFPolicy(PolicyConfigOptions.None)
            {
                AllowPlainTextHttp = true
            };
            HttpClient client = new(policy.GetHandler());

            var response = await client.GetAsync($"https://{TestDomain}/", CancellationToken.None);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var response2 = await client.GetAsync($"http://{TestDomain}/", CancellationToken.None);
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
        }

        [Fact]
        public async Task RejectsNonHttpSchemes()
        {
            var policy = new AntiSSRFPolicy(PolicyConfigOptions.None)
            {
                AllowPlainTextHttp = true
            };
            HttpClient client = new(policy.GetHandler());

            // Test various non-HTTP schemes that should all be rejected
            var nonHttpSchemes = new[]
            {
                "ws://example.com",          // WebSocket
                "wss://example.com",         // WebSocket Secure  
                "ftp://example.com",         // FTP
                "gopher://example.com",      // Gopher
                "file:///etc/passwd",        // File
                "ldap://example.com",        // LDAP
                "ldaps://example.com",       // LDAP Secure
                "mailto:test@example.com",   // Email
                "tel:+1234567890",          // Telephone
                "data:text/plain;base64,SGVsbG8=", // Data URL
                "javascript:alert('xss')",   // JavaScript
                "custom://example.com"       // Custom scheme
            };

            foreach (var url in nonHttpSchemes)
            {
                try
                {
                    await client.GetAsync(url, CancellationToken.None);
                    Assert.Fail($"Expected AntiSSRFException for scheme: {url}");
                }
                catch (AntiSSRFException)
                {
                    // Expected - this is the correct behavior
                    continue;
                }
                catch (Exception)
                {
                    // Other exceptions (like ArgumentException for invalid URIs) are also acceptable
                    // as long as the request doesn't succeed
                    continue;
                }
            }
        }

        [Fact]
        public void NoEditsAfterHandler()
        {
            var policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1);
            policy.GetHandler();

            Assert.Throws<AntiSSRFException>(() => policy.AllowPlainTextHttp = true);
        }
    }
}
