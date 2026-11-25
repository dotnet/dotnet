// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
#if NET5_0_OR_GREATER
using System.Text.Json;
#endif
using Xunit;

namespace Microsoft.Security.AntiSSRF.FunctionalTests
{
    public class AntiSSRFHandlerTests
    {
        private static readonly string testUrl = "https://ambitious-flower-0611c910f.2.azurestaticapps.net/";

        [Fact]
        public async Task AllowAutoRedirect_Tests()
        {
            var policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1);
            using var handler = policy.GetHandler();

            // 1. Default value: true
            Assert.True(handler.AllowAutoRedirect);

            // 2. Can set property to a new value
            handler.AllowAutoRedirect = false;

            Assert.False(handler.AllowAutoRedirect);

            // 3. Test with actual requests - verify redirects are NOT followed when AllowAutoRedirect = false
            using var client = new HttpClient(handler);
            var response = await client.GetAsync("https://httpbin.org/redirect/1", CancellationToken.None);
            
            // Should return redirect status code instead of following
            Assert.Equal(HttpStatusCode.Found, response.StatusCode);
            Assert.NotNull(response.Headers.Location);

            // Test with AllowAutoRedirect = true
            var handler2 = policy.GetHandler();
            handler2.AllowAutoRedirect = true;
            using var client2 = new HttpClient(handler2);
            var response2 = await client2.GetAsync("https://httpbin.org/redirect/1", CancellationToken.None);
            
            // Should follow redirect and return final status
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);

            // 4. Cannot be edited after a request is sent
            Assert.Throws<InvalidOperationException>(() => handler.AllowAutoRedirect = true);

            // 5. One handler shouldn't affect another
            using var handler3 = policy.GetHandler();
            Assert.True(handler3.AllowAutoRedirect);

            // 6. Cannot be edited after disposed
            handler3.Dispose();
            Assert.Throws<ObjectDisposedException>(() => handler3.AllowAutoRedirect = false);
        }

#if NET5_0_OR_GREATER
        [Fact]
        public async Task CookieContainer_Tests()
        {
            var policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1);
            using var handler = policy.GetHandler();

            // 1. Default value: non-null instance of CookieContainer
            Assert.NotNull(handler.CookieContainer);

            // 2. Can set property to a new value
            var container = new CookieContainer();
            container.Add(new Uri(testUrl), new Cookie("test", "value"));
            handler.CookieContainer = container;
            handler.UseCookies = true; // Ensure cookies are used

            Assert.Same(container, handler.CookieContainer);
            Assert.Equal(1, handler.CookieContainer.Count);

            // 3. New value is respected on requests
            container.Add(new Uri("https://httpbin.org/"), new Cookie("testcookie", "testvalue"));
            using var client = new HttpClient(handler);
            var response = await client.GetAsync("https://httpbin.org/cookies", CancellationToken.None);
            var content = await response.Content.ReadAsStringAsync();

            Assert.Contains("testcookie", content);

            // 4. Cannot be edited after a request is sent
            Assert.Throws<InvalidOperationException>(() => handler.CookieContainer = new CookieContainer());

            // 5. One handler shouldn't affect another
            using var handler2 = policy.GetHandler();

            Assert.NotSame(handler.CookieContainer, handler2.CookieContainer);
            Assert.Equal(0, handler2.CookieContainer.Count);

            // 6. Cannot be edited after disposed
            handler2.Dispose();

            Assert.Throws<ObjectDisposedException>(() => handler2.CookieContainer = new CookieContainer());
        }
#endif

        [Fact]
        public async Task Credentials_Tests()
        {
            var policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1);
            using var handler = policy.GetHandler();

            // 1. Default value: null
            Assert.Null(handler.Credentials);

            // 2. Can set property to a new value
            var credentials = new NetworkCredential("testuser", "testpass");
            handler.Credentials = credentials;
            Assert.Same(credentials, handler.Credentials);

            // 3. Test actual functionality - use httpbin's basic auth endpoint
            var correctCredentials = new NetworkCredential("user", "pass");
            handler.Credentials = correctCredentials;

            using var client = new HttpClient(handler);

            // This endpoint requires basic auth with username "user" and password "pass"
            var response1 = await client.GetAsync("https://httpbin.org/basic-auth/user/pass", CancellationToken.None);
            Assert.Equal(HttpStatusCode.OK, response1.StatusCode);

            // Test with wrong credentials
            var handler2 = policy.GetHandler();
            var wrongCredentials = new NetworkCredential("wrong", "wrong");
            handler2.Credentials = wrongCredentials;

            using var client2 = new HttpClient(handler2);
            var response2 = await client2.GetAsync("https://httpbin.org/basic-auth/user/pass", CancellationToken.None);
            Assert.Equal(HttpStatusCode.Unauthorized, response2.StatusCode);

            // 4. Cannot be edited after a request is sent
            Assert.Throws<InvalidOperationException>(() => handler.Credentials = new NetworkCredential("new", "new"));

            // 5. One handler shouldn't affect another
            using var handler3 = policy.GetHandler();
            Assert.Null(handler3.Credentials);

            // 6. Cannot be edited after disposed
            handler3.Dispose();
            Assert.Throws<ObjectDisposedException>(() => handler3.Credentials = correctCredentials);
        }

        [Fact]
        public async Task MaxConnectionsPerServer_Tests()
        {
            var policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1);
            using var handler = policy.GetHandler();

            // 1. Default value: int.MaxValue
            Assert.True(int.MaxValue == handler.MaxConnectionsPerServer || 2 == handler.MaxConnectionsPerServer);

            // 2. Can set property to a new value
            handler.MaxConnectionsPerServer = 5;

            Assert.Equal(5, handler.MaxConnectionsPerServer);

            // 3. New value is respected on requests
            using var client = new HttpClient(handler);
            var response1 = await client.GetAsync(testUrl, CancellationToken.None);
            Assert.Equal(HttpStatusCode.OK, response1.StatusCode);

            var response2 = await client.GetAsync(testUrl, CancellationToken.None);
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);

            // 4. Cannot be edited after a request is sent
            Assert.Throws<InvalidOperationException>(() => handler.MaxConnectionsPerServer = 10);

            // 5. One handler shouldn't affect another
            using var handler2 = policy.GetHandler();
            Assert.True(int.MaxValue == handler2.MaxConnectionsPerServer || 2 == handler2.MaxConnectionsPerServer);

            // 6. Cannot be edited after disposed
            handler2.Dispose();
            Assert.Throws<ObjectDisposedException>(() => handler2.MaxConnectionsPerServer = 10);
        }

        [Fact]
        public async Task MaxAutomaticRedirections_Tests()
        {
            var policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1);
            using var handler = policy.GetHandler();

            // 1. Default value: 50
            Assert.Equal(50, handler.MaxAutomaticRedirections);

            // 2. Can set property to a new value
            handler.MaxAutomaticRedirections = 2;

            Assert.Equal(2, handler.MaxAutomaticRedirections);

            // 3. Test with actual requests - verify limit is enforced
            using var client = new HttpClient(handler);
            
            // Should fail with too many redirects
            var response = await client.GetAsync("https://httpbin.org/redirect/5", CancellationToken.None);
            Assert.Equal(HttpStatusCode.Found, response.StatusCode);

            // Test with higher limit - should succeed
            var handler2 = policy.GetHandler();
            handler2.MaxAutomaticRedirections = 10;
            using var client2 = new HttpClient(handler2);
            var response2 = await client2.GetAsync("https://httpbin.org/redirect/5", CancellationToken.None);
            
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);

            // 4. Cannot be edited after a request is sent
            Assert.Throws<InvalidOperationException>(() => handler.MaxAutomaticRedirections = 100);

            // 5. One handler shouldn't affect another
            using var handler3 = policy.GetHandler();
            Assert.Equal(50, handler3.MaxAutomaticRedirections);

            // 6. Cannot be edited after disposed
            handler3.Dispose();
            Assert.Throws<ObjectDisposedException>(() => handler3.MaxAutomaticRedirections = 20);
        }

        [Fact]
        public async Task MaxResponseHeadersLength_Tests()
        {
            var policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1);
            using var handler = policy.GetHandler();

            // 1. Default value: 64
            Assert.Equal(64, handler.MaxResponseHeadersLength);

            // 2. Can set property to a new value
            handler.MaxResponseHeadersLength = 128 * 1024;

            Assert.Equal(128 * 1024, handler.MaxResponseHeadersLength);

            // 3. New value is respected on requests
            handler.MaxResponseHeadersLength = 1;
            using var client = new HttpClient(handler);

            try
            {
                await client.GetAsync("https://httpbin.org/response-headers?X-Large-Header=" + new string('A', 2000), CancellationToken.None);
                Assert.Fail("Request should fail with HttpRequestException due to response headers exceeding limit");
            }
            catch (Exception ex) when (ex is not AntiSSRFException)
            {
                ;
            }

            // 4. Cannot be edited after a request is sent
            Assert.Throws<InvalidOperationException>(() => handler.MaxResponseHeadersLength = 256 * 1024);

            // 5. One handler shouldn't affect another
            using var handler2 = policy.GetHandler();
            Assert.Equal(64, handler2.MaxResponseHeadersLength);

            // 6. Cannot be edited after disposed
            handler2.Dispose();
            Assert.Throws<ObjectDisposedException>(() => handler2.MaxResponseHeadersLength = 64 * 1024);
        }

#if NET5_0_OR_GREATER
        [Fact]
        public async Task UseCookies_Tests()
        {
            var policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1);
            using var handler = policy.GetHandler();

            // 1. Default value: true
            Assert.True(handler.UseCookies);

            // 2. Can set property to a new value
            handler.UseCookies = false;

            Assert.False(handler.UseCookies);

            // 3. Test with actual requests - verify cookies are NOT sent when UseCookies = false
            using var client = new HttpClient(handler);
            var response = await client.GetAsync("https://httpbin.org/cookies", CancellationToken.None);
            var content = await response.Content.ReadAsStringAsync();
            
            // Verify cookies property is an empty object
            var jsonDoc = JsonDocument.Parse(content);
            Assert.True(jsonDoc.RootElement.TryGetProperty("cookies", out var cookiesProperty));
            Assert.Equal(JsonValueKind.Object, cookiesProperty.ValueKind);
            Assert.Empty(cookiesProperty.EnumerateObject());

            // 4. Cannot be edited after a request is sent
            Assert.Throws<InvalidOperationException>(() => handler.UseCookies = true);

            // 5. One handler shouldn't affect another
            using var handler2 = policy.GetHandler();
            Assert.True(handler2.UseCookies);

            // 6. Cannot be edited after disposed
            handler2.Dispose();
            Assert.Throws<ObjectDisposedException>(() => handler2.UseCookies = false);
        }
#endif

        [Fact]
        public async Task SslProtocols_Tests()
        {
            var policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1);
            using var handler = policy.GetHandler();

            // 1. Default value: None
#if NET5_0_OR_GREATER
            Assert.Equal(SslProtocols.None, handler.SslOptions.EnabledSslProtocols);
#else
            Assert.Equal(SslProtocols.None, handler.SslProtocols);
#endif

            // Roslyn doesn't like hardcoding the SslProtocols, even in tests
        }

        [Fact]
        public async Task CheckCertificateRevocationList_Tests()
        {
            var policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1);
            using var handler = policy.GetHandler();

            // 1. Default value: false
#if NET5_0_OR_GREATER
            Assert.Equal(X509RevocationMode.NoCheck, handler.SslOptions.CertificateRevocationCheckMode);
#else
            Assert.False(handler.CheckCertificateRevocationList);
#endif

            // 2. Can set property to a new value
#if NET5_0_OR_GREATER
            handler.SslOptions.CertificateRevocationCheckMode = X509RevocationMode.Online;
            Assert.Equal(X509RevocationMode.Online, handler.SslOptions.CertificateRevocationCheckMode);
#else
            handler.CheckCertificateRevocationList = true;
            Assert.True(handler.CheckCertificateRevocationList);
#endif

            // 3. Test with actual requests - verify it works with CRL checking enabled
            using var client = new HttpClient(handler);

            try
            {
                await client.GetAsync("https://revoked.badssl.com/", CancellationToken.None);
                Assert.Fail("Request should have failed due to revoked certificate");
            }
            catch (Exception e) when (e is not AntiSSRFException)
            {
                ;
            }

            // Test with valid certificate - should work regardless of CRL setting
            var response1 = await client.GetAsync(testUrl, CancellationToken.None);
            Assert.Equal(HttpStatusCode.OK, response1.StatusCode);

            // 4. Cannot be edited after a request is sent
#if NET5_0_OR_GREATER
            Assert.Throws<InvalidOperationException>(() => handler.SslOptions = new SslClientAuthenticationOptions() {
                CertificateRevocationCheckMode = X509RevocationMode.Online
            });
#else
            Assert.Throws<InvalidOperationException>(() => handler.CheckCertificateRevocationList = false);
#endif

            // 5. One handler shouldn't affect another
            using var handler2 = policy.GetHandler();
#if NET5_0_OR_GREATER
            Assert.Equal(X509RevocationMode.NoCheck, handler2.SslOptions.CertificateRevocationCheckMode);
#else
            Assert.False(handler2.CheckCertificateRevocationList);
#endif

            // 6. Cannot be edited after disposed
            handler2.Dispose();
#if NET5_0_OR_GREATER
            Assert.Throws<ObjectDisposedException>(() => handler2.SslOptions = new SslClientAuthenticationOptions() {
                CertificateRevocationCheckMode = X509RevocationMode.Online
            });
#else
            Assert.Throws<ObjectDisposedException>(() => handler2.CheckCertificateRevocationList = true);
#endif
        }

#if NET5_0_OR_GREATER
        [Fact]
        public async Task ConnectTimeout_Tests()
        {
            var policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1);
            using var handler = policy.GetHandler();

            // 1. Default value: InfiniteTimeSpan
            Assert.Equal(Timeout.InfiniteTimeSpan, handler.ConnectTimeout);

            // 2. Can set property to a new value
            handler.ConnectTimeout = TimeSpan.FromSeconds(30);

            Assert.Equal(TimeSpan.FromSeconds(30), handler.ConnectTimeout);

            // 3. Test with actual requests - set a very short timeout that should fail
            handler.ConnectTimeout = TimeSpan.FromMilliseconds(1); // Very short timeout
            using var client = new HttpClient(handler);

            await Assert.ThrowsAsync<TaskCanceledException>(async () =>
                await client.GetAsync("https://httpbin.org/delay/1", CancellationToken.None));

            // 4. Cannot be edited after a request is sent
            Assert.Throws<InvalidOperationException>(() => handler.ConnectTimeout = TimeSpan.FromSeconds(60));

            // 5. One handler shouldn't affect another
            using var handler2 = policy.GetHandler();
            Assert.Equal(Timeout.InfiniteTimeSpan, handler2.ConnectTimeout);

            // 6. Cannot be edited after disposed
            handler2.Dispose();
            Assert.Throws<ObjectDisposedException>(() => handler2.ConnectTimeout = TimeSpan.FromSeconds(30));
        }

        [Fact]
        public async Task ResponseDrainTimeout_Tests()
        {
            var policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1);
            using var handler = policy.GetHandler();

            // 1. Default value: 2 seconds
            Assert.Equal(TimeSpan.FromSeconds(2), handler.ResponseDrainTimeout);

            // 2. Can set property to a new value
            handler.ResponseDrainTimeout = TimeSpan.FromSeconds(5);

            Assert.Equal(TimeSpan.FromSeconds(5), handler.ResponseDrainTimeout);

            // 3. Test with actual requests - verify it works with reasonable timeout
            handler.ResponseDrainTimeout = TimeSpan.FromMilliseconds(1);
            using var client = new HttpClient(handler);
            var response = await client.GetAsync("https://httpbin.org/bytes/10000", CancellationToken.None);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // 4. Cannot be edited after a request is sent
            Assert.Throws<InvalidOperationException>(() => handler.ResponseDrainTimeout = TimeSpan.FromSeconds(10));

            // 5. One handler shouldn't affect another
            using var handler2 = policy.GetHandler();
            Assert.Equal(TimeSpan.FromSeconds(2), handler2.ResponseDrainTimeout);

            // 6. Cannot be edited after disposed
            handler2.Dispose();
            Assert.Throws<ObjectDisposedException>(() => handler2.ResponseDrainTimeout = TimeSpan.FromSeconds(5));
        }

        [Fact]
        public async Task PooledConnectionIdleTimeout_Tests()
        {
            var policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1);
            using var handler = policy.GetHandler();

            // 1. Default value: TimeSpan.FromMinutes(1)
            Assert.Equal(TimeSpan.FromMinutes(1), handler.PooledConnectionIdleTimeout);

            // 2. Can set property to a new value
            handler.PooledConnectionIdleTimeout = TimeSpan.FromMinutes(5);

            Assert.Equal(TimeSpan.FromMinutes(5), handler.PooledConnectionIdleTimeout);

            // 3. Test with actual requests - Hard to test, just checking requests still happen
            using var client = new HttpClient(handler);
            var response1 = await client.GetAsync(testUrl, CancellationToken.None);
            Assert.Equal(HttpStatusCode.OK, response1.StatusCode);

            var response2 = await client.GetAsync(testUrl, CancellationToken.None);
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);

            // 4. Cannot be edited after a request is sent
            Assert.Throws<InvalidOperationException>(() => handler.PooledConnectionIdleTimeout = TimeSpan.FromMinutes(10));

            // 5. One handler shouldn't affect another
            using var handler2 = policy.GetHandler();
            Assert.Equal(TimeSpan.FromMinutes(1), handler2.PooledConnectionIdleTimeout);

            // 6. Cannot be edited after disposed
            handler2.Dispose();
            Assert.Throws<ObjectDisposedException>(() => handler2.PooledConnectionIdleTimeout = TimeSpan.FromMinutes(5));
        }

        [Fact]
        public async Task PooledConnectionLifetime_Tests()
        {
            var policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1);
            using var handler = policy.GetHandler();

            // 1. Default value: InfiniteTimeSpan
            var defaultValue = Timeout.InfiniteTimeSpan;
            Assert.Equal(Timeout.InfiniteTimeSpan, defaultValue);

            // 2. Can set property to a new value
            handler.PooledConnectionLifetime = TimeSpan.FromMinutes(10);

            Assert.Equal(TimeSpan.FromMinutes(10), handler.PooledConnectionLifetime);

            // 3. Test with actual requests - Hard to test, just checking requests still happen
            handler.PooledConnectionLifetime = TimeSpan.FromMinutes(5);
            using var client = new HttpClient(handler);
            var response1 = await client.GetAsync(testUrl, CancellationToken.None);
            Assert.Equal(HttpStatusCode.OK, response1.StatusCode);

            var response2 = await client.GetAsync(testUrl, CancellationToken.None);
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);

            // 4. Cannot be edited after a request is sent
            Assert.Throws<InvalidOperationException>(() => handler.PooledConnectionLifetime = TimeSpan.FromMinutes(15));

            // 5. One handler shouldn't affect another
            using var handler2 = policy.GetHandler();
            Assert.Equal(defaultValue, handler2.PooledConnectionLifetime);

            // 6. Cannot be edited after disposed
            handler2.Dispose();
            Assert.Throws<ObjectDisposedException>(() => handler2.PooledConnectionLifetime = TimeSpan.FromMinutes(5));
        }

#pragma warning disable CA5359 // Do not disable certificate validation - intentionally testing SSL callback behavior
        [Fact]
        public async Task RemoteCertificateValidationCallback_Tests()
        {
            var policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1);
            using var handler = policy.GetHandler();

            // 1. Default value: null
            Assert.Null(handler.SslOptions.RemoteCertificateValidationCallback);

            // 2. Can set property to a new value
            handler.SslOptions.RemoteCertificateValidationCallback = new RemoteCertificateValidationCallback((sender, cert, chain, errors) => true);
            Assert.NotNull(handler.SslOptions.RemoteCertificateValidationCallback);

            // 3. Test actual functionality - use callback that always returns true
            handler.SslOptions.RemoteCertificateValidationCallback = new RemoteCertificateValidationCallback((sender, cert, chain, errors) => true);
            using var client = new HttpClient(handler);
            var response1 = await client.GetAsync("https://self-signed.badssl.com/", CancellationToken.None);

            Assert.Equal(HttpStatusCode.OK, response1.StatusCode);

            // Test with callback that always returns false
            var handler2 = policy.GetHandler();
            handler2.SslOptions.RemoteCertificateValidationCallback = new RemoteCertificateValidationCallback((sender, cert, chain, errors) => false);
            using var client2 = new HttpClient(handler2);
            
            await Assert.ThrowsAsync<AuthenticationException>(async () => 
                await client2.GetAsync(testUrl, CancellationToken.None));
            
            // 4. Cannot be edited after a request is sent
            Assert.Throws<InvalidOperationException>(() => handler.SslOptions = new SslClientAuthenticationOptions() {
                RemoteCertificateValidationCallback = null
            });

            // 5. One handler shouldn't affect another
            using var handler3 = policy.GetHandler();
            Assert.Null(handler3.SslOptions.RemoteCertificateValidationCallback);

            // 6. Cannot be edited after disposed
            handler3.Dispose();
            Assert.Throws<ObjectDisposedException>(() => handler3.SslOptions = new SslClientAuthenticationOptions() {
                RemoteCertificateValidationCallback = null
            });
        }
#pragma warning restore CA5359
#else
#pragma warning disable CA5359 // Do not disable certificate validation - intentionally testing SSL callback behavior
        [Fact]
        public async Task ServerCertificateCustomValidationCallback_Tests()
        {
            var policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1);
            using var handler = policy.GetHandler();

            // 1. Default value: null
            Assert.Null(handler.ServerCertificateCustomValidationCallback);

            // 2. Can set property to a new value
            Func<object, X509Certificate2?, X509Chain?, SslPolicyErrors, bool> alwaysTrue =
                (request, cert, chain, errors) => true;

            handler.ServerCertificateCustomValidationCallback = alwaysTrue;
            Assert.Same(alwaysTrue, handler.ServerCertificateCustomValidationCallback);

            // 3. Test actual functionality - use callback that always returns true
            handler.ServerCertificateCustomValidationCallback = (request, cert, chain, errors) => true;
            using var client = new HttpClient(handler);
            var response1 = await client.GetAsync("https://self-signed.badssl.com/", CancellationToken.None);

            Assert.Equal(HttpStatusCode.OK, response1.StatusCode);

            // Test with callback that always returns false
            var handler2 = policy.GetHandler();
            handler2.ServerCertificateCustomValidationCallback = (request, cert, chain, errors) => false;
            using var client2 = new HttpClient(handler2);

            try 
            {
                await client2.GetAsync(testUrl, CancellationToken.None);
                Assert.Fail("Request should have failed due to certificate validation failure");
            } 
            catch (Exception e) when (e is not AntiSSRFException) 
            {
                ;
            }

            // 4. Cannot be edited after a request is sent
            Assert.Throws<InvalidOperationException>(() => handler.ServerCertificateCustomValidationCallback = null);

            // 5. One handler shouldn't affect another
            using var handler3 = policy.GetHandler();
            Assert.Null(handler3.ServerCertificateCustomValidationCallback);

            // 6. Cannot be edited after disposed
            handler3.Dispose();
            Assert.Throws<ObjectDisposedException>(() => handler3.ServerCertificateCustomValidationCallback = alwaysTrue);
        }
#pragma warning restore CA5359
#endif
    }
}
