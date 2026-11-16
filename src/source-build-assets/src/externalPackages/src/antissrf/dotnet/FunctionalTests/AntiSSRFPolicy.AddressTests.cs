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
    public class AntiSSRFPolicy_AddressTests
    {
        private static readonly string TestDomain = "ambitious-flower-0611c910f.2.azurestaticapps.net";
        private static readonly uint BlockedByAzureFirewall = 470;

        [Fact]
        public void BadInputs()
        {
            AntiSSRFPolicy policy = new(PolicyConfigOptions.None)
            {
                DenyAllUnspecifiedIPs = true
            };
            Assert.Throws<AntiSSRFException>(() => policy.AddDeniedAddresses(["1.2.3.4"]));

            // Test null arrays
            AntiSSRFPolicy policy2 = new(PolicyConfigOptions.None);
            Assert.Throws<ArgumentNullException>(() => policy2.AddAllowedAddresses(null));
            Assert.Throws<ArgumentNullException>(() => policy2.AddDeniedAddresses(null));

            // Test empty arrays - these should be allowed (no-op)
            policy2.AddAllowedAddresses([]);
            policy2.AddDeniedAddresses([]);

            // Test invalid IP address formats
            Assert.Throws<FormatException>(() => policy2.AddDeniedAddresses(["invalid.ip.address"]));
            Assert.Throws<FormatException>(() => policy2.AddDeniedAddresses(["256.256.256.256/24"]));
            Assert.Throws<FormatException>(() => policy2.AddDeniedAddresses(["192.168.1.1/33"])); // Invalid subnet mask
            Assert.Throws<FormatException>(() => policy2.AddAllowedAddresses(["not-an-ip"]));

            // Invalid arrays should not partially mutate the policy.
            Assert.Throws<FormatException>(() => policy2.AddAllowedAddresses(["10.0.0.0/8", "not-an-ip"]));
            Assert.Empty(policy2.AllowedAddresses);

            Assert.Throws<FormatException>(() => policy2.AddDeniedAddresses(["192.168.1.0/24", "invalid.ip.address"]));
            Assert.Empty(policy2.DeniedAddresses);

            // Test array containing null addresses
            AntiSSRFPolicy policy3 = new(PolicyConfigOptions.None);
            Assert.Throws<ArgumentNullException>(() => policy3.AddDeniedAddresses(["192.168.1.0/24", null!, "10.0.0.0/8"]));
            Assert.Throws<ArgumentNullException>(() => policy3.AddAllowedAddresses([null!]));
        }

        [Fact]
        public void AddDeniedAddresses_FromIPAddressRanges_PopulatesDeniedAddresses()
        {
            AntiSSRFPolicy policy = new(PolicyConfigOptions.None);
            List<string> deniedRanges = new();
            deniedRanges.AddRange(IPAddressRanges.imds);
            deniedRanges.AddRange(IPAddressRanges.wireserver);
            deniedRanges.AddRange(IPAddressRanges.loopback);

            policy.AddDeniedAddresses(deniedRanges.ToArray());

            IReadOnlyList<string> deniedAddresses = policy.DeniedAddresses;
            Assert.Contains("169.254.169.254/32", deniedAddresses);
            Assert.Contains("168.63.129.16/32", deniedAddresses);
            Assert.Contains("127.0.0.0/8", deniedAddresses);
            Assert.Contains("::1/128", deniedAddresses);
        }

        [Fact]
        public void AddAllowedAddresses_FromIPAddressRanges_PopulatesAllowedAddresses()
        {
            AntiSSRFPolicy policy = new(PolicyConfigOptions.None)
            {
                DenyAllUnspecifiedIPs = true
            };
            List<string> allowedRanges = new();
            allowedRanges.AddRange(IPAddressRanges.privateUse);
            allowedRanges.AddRange(IPAddressRanges.documentation);

            policy.AddAllowedAddresses(allowedRanges.ToArray());

            IReadOnlyList<string> allowedAddresses = policy.AllowedAddresses;
            Assert.Contains("10.0.0.0/8", allowedAddresses);
            Assert.Contains("172.16.0.0/12", allowedAddresses);
            Assert.Contains("192.168.0.0/16", allowedAddresses);
            Assert.Contains("192.0.2.0/24", allowedAddresses);
            Assert.Contains("2001:db8::/32", allowedAddresses);
        }

        [Fact]
        public async Task CheckDefaults_IMDSAsync()
        {
            using HttpClient client = new(new AntiSSRFPolicy(PolicyConfigOptions.InternalOnly).GetHandler())
            {
                Timeout = TimeSpan.FromSeconds(3)
            };
            using HttpClient client2 = new(new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1).GetHandler())
            {
                Timeout = TimeSpan.FromSeconds(3)
            };
            using HttpClient client3 = new(new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyLatest).GetHandler())
            {
                Timeout = TimeSpan.FromSeconds(3)
            };
            using HttpClient client4 = new(new AntiSSRFPolicy(PolicyConfigOptions.None).GetHandler())
            {
                Timeout = TimeSpan.FromSeconds(3)
            };

            var ipUrl = "https://169.254.169.254/latest/meta-data/";
            using (var cts1 = new CancellationTokenSource(TimeSpan.FromSeconds(1)))
                await Assert.ThrowsAsync<AntiSSRFException>(async () => await client.GetAsync(ipUrl, cts1.Token));
            using (var cts2 = new CancellationTokenSource(TimeSpan.FromSeconds(1)))
                await Assert.ThrowsAsync<AntiSSRFException>(async () => await client2.GetAsync(ipUrl, cts2.Token));
            using (var cts3 = new CancellationTokenSource(TimeSpan.FromSeconds(1)))
                await Assert.ThrowsAsync<AntiSSRFException>(async () => await client3.GetAsync(ipUrl, cts3.Token));
            try
            {
                using var cts4 = new CancellationTokenSource(TimeSpan.FromMilliseconds(500));
                await client4.GetAsync(ipUrl, cts4.Token);
                // If we get here, the call succeeded (no AntiSSRFException thrown as expected)
                Assert.True(true, "Call completed successfully without throwing AntiSSRFException as expected");
            }
            catch (Exception ex) when (ex is not AntiSSRFException)
            {
                // Expected: timeout, socket exception, etc. - but not AntiSSRFException
                Assert.True(true, "Call failed with expected non-AntiSSRF exception: " + ex.GetType().Name);
            }
            catch (AntiSSRFException)
            {
                Assert.Fail("AntiSSRFException should not be thrown for localhost since it is not in the denied list");
            }

            var nonStandardIpUrl = "https://0xA9.0xFE.0xA9.0xFE/latest/meta-data/";
            using (var cts1 = new CancellationTokenSource(TimeSpan.FromSeconds(1)))
                await Assert.ThrowsAsync<AntiSSRFException>(() => client.GetAsync(nonStandardIpUrl, cts1.Token));
            using (var cts2 = new CancellationTokenSource(TimeSpan.FromSeconds(1)))
                await Assert.ThrowsAsync<AntiSSRFException>(() => client2.GetAsync(nonStandardIpUrl, cts2.Token));
            using (var cts3 = new CancellationTokenSource(TimeSpan.FromSeconds(1)))
                await Assert.ThrowsAsync<AntiSSRFException>(() => client3.GetAsync(nonStandardIpUrl, cts3.Token));
            try
            {
                using (var cts4 = new CancellationTokenSource(TimeSpan.FromSeconds(1)))
                    await client4.GetAsync(nonStandardIpUrl, cts4.Token);
            }
            catch (Exception ex) when (ex is not AntiSSRFException)
            {
                // Expected: timeout, socket exception, etc. - but not AntiSSRFException
            }
            catch (AntiSSRFException)
            {
                Assert.Fail("AntiSSRFException should not be thrown for localhost since it is not in the denied list");
            }

            var mappedIpUrl = "https://[::ffff:169.254.169.254]/latest/meta-data/";
            using (var cts5 = new CancellationTokenSource(TimeSpan.FromSeconds(1)))
                await Assert.ThrowsAsync<AntiSSRFException>(() => client.GetAsync(mappedIpUrl, cts5.Token));
            using (var cts6 = new CancellationTokenSource(TimeSpan.FromSeconds(1)))
                await Assert.ThrowsAsync<AntiSSRFException>(() => client2.GetAsync(mappedIpUrl, cts6.Token));
            using (var cts7 = new CancellationTokenSource(TimeSpan.FromSeconds(1)))
                await Assert.ThrowsAsync<AntiSSRFException>(() => client3.GetAsync(mappedIpUrl, cts7.Token));
            try
            {
                using (var cts8 = new CancellationTokenSource(TimeSpan.FromSeconds(1)))
                    await client4.GetAsync(mappedIpUrl, cts8.Token);
            }
            catch (Exception ex) when (ex is not AntiSSRFException)
            {
                // Expected: timeout, socket exception, etc. - but not AntiSSRFException
            }
            catch (AntiSSRFException)
            {
                Assert.Fail("AntiSSRFException should not be thrown for localhost since it is not in the denied list");
            }

            var mappedIpUrl2 = "https://[::ffff:A9FE:A9FE]/latest/meta-data/";
            await Assert.ThrowsAsync<AntiSSRFException>(() => client.GetAsync(mappedIpUrl2, CancellationToken.None));
            await Assert.ThrowsAsync<AntiSSRFException>(() => client2.GetAsync(mappedIpUrl2, CancellationToken.None));
            await Assert.ThrowsAsync<AntiSSRFException>(() => client3.GetAsync(mappedIpUrl2, CancellationToken.None));
            try
            {
                await client4.GetAsync(mappedIpUrl2, CancellationToken.None);
            }
            catch (Exception ex) when (ex is not AntiSSRFException)
            {
                // Expected: timeout, socket exception, etc. - but not AntiSSRFException
            }
            catch (AntiSSRFException)
            {
                Assert.Fail("AntiSSRFException should not be thrown for localhost since it is not in the denied list");
            }

            var redirectUrl = $"https://{TestDomain}/api/imds-ip?code=301";
            await Assert.ThrowsAsync<AntiSSRFException>(() => client.GetAsync(redirectUrl, CancellationToken.None));
            await Assert.ThrowsAsync<AntiSSRFException>(() => client2.GetAsync(redirectUrl, CancellationToken.None));
            await Assert.ThrowsAsync<AntiSSRFException>(() => client3.GetAsync(redirectUrl, CancellationToken.None));
            try
            {
                await client4.GetAsync(redirectUrl, CancellationToken.None);
            }
            catch (Exception ex) when (ex is not AntiSSRFException)
            {
                // Expected: timeout, socket exception, etc. - but not AntiSSRFException
            }
            catch (AntiSSRFException)
            {
                Assert.Fail("AntiSSRFException should not be thrown for localhost since it is not in the denied list");
            }

            var redirectUrl2 = $"https://{TestDomain}/api/imds-ip?code=302";
            await Assert.ThrowsAsync<AntiSSRFException>(() => client.GetAsync(redirectUrl2, CancellationToken.None));
            await Assert.ThrowsAsync<AntiSSRFException>(() => client2.GetAsync(redirectUrl2, CancellationToken.None));
            await Assert.ThrowsAsync<AntiSSRFException>(() => client3.GetAsync(redirectUrl2, CancellationToken.None));
            try
            {
                await client4.GetAsync(redirectUrl2, CancellationToken.None);
            }
            catch (Exception ex) when (ex is not AntiSSRFException)
            {
                // Expected: timeout, socket exception, etc. - but not AntiSSRFException
            }
            catch (AntiSSRFException)
            {
                Assert.Fail("AntiSSRFException should not be thrown for localhost since it is not in the denied list");
            }

            var redirectUrl3 = $"https://{TestDomain}/api/imds?redirectNum=3";
            await Assert.ThrowsAsync<AntiSSRFException>(() => client.GetAsync(redirectUrl3, CancellationToken.None));
            await Assert.ThrowsAsync<AntiSSRFException>(() => client2.GetAsync(redirectUrl3, CancellationToken.None));

            await Assert.ThrowsAsync<AntiSSRFException>(() => client3.GetAsync(redirectUrl3, CancellationToken.None));
            try
            {
                await client4.GetAsync(redirectUrl3, CancellationToken.None);
            }
            catch (Exception ex) when (ex is not AntiSSRFException)
            {
                // Expected: timeout, socket exception, etc. - but not AntiSSRFException
            }
            catch (AntiSSRFException)
            {
                Assert.Fail("AntiSSRFException should not be thrown for localhost since it is not in the denied list");
            }
        }

        [Fact]
        public async Task CheckDefaults_WireServerAsync()
        {
            using HttpClient client = new(new AntiSSRFPolicy(PolicyConfigOptions.InternalOnly)
            {
                AllowPlainTextHttp = true
            }.GetHandler())
            {
                Timeout = TimeSpan.FromSeconds(3)
            };
            using HttpClient client2 = new(new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1)
            {
                AllowPlainTextHttp = true
            }.GetHandler())
            {
                Timeout = TimeSpan.FromSeconds(3)
            };
            using HttpClient client3 = new(new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyLatest)
            {
                AllowPlainTextHttp = true
            }.GetHandler())
            {
                Timeout = TimeSpan.FromSeconds(3)
            };
            using HttpClient client4 = new(new AntiSSRFPolicy(PolicyConfigOptions.None)
            {
                AllowPlainTextHttp = true
            }.GetHandler())
            {
                Timeout = TimeSpan.FromSeconds(3)
            };

            var ipUrl = "http://168.63.129.16/";
            await Assert.ThrowsAsync<AntiSSRFException>(async () => await client.GetAsync(ipUrl, new CancellationTokenSource(TimeSpan.FromSeconds(1)).Token));
            await Assert.ThrowsAsync<AntiSSRFException>(async () => await client2.GetAsync(ipUrl, new CancellationTokenSource(TimeSpan.FromSeconds(1)).Token));
            await Assert.ThrowsAsync<AntiSSRFException>(async () => await client3.GetAsync(ipUrl, new CancellationTokenSource(TimeSpan.FromSeconds(1)).Token));
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
                await client4.GetAsync(ipUrl, cts.Token);
            }
            catch (Exception ex) when (ex is not AntiSSRFException)
            {
                // Expected: timeout, socket exception, etc. - but not AntiSSRFException
            }
            catch (AntiSSRFException)
            {
                Assert.Fail("AntiSSRFException should not be thrown for localhost since it is not in the denied list");
            }

            var nonStandardIpUrl = "http://0xA8.0x3F.0x81.0x10/";
            await Assert.ThrowsAsync<AntiSSRFException>(() => client.GetAsync(nonStandardIpUrl, new CancellationTokenSource(TimeSpan.FromSeconds(1)).Token));
            await Assert.ThrowsAsync<AntiSSRFException>(() => client2.GetAsync(nonStandardIpUrl, new CancellationTokenSource(TimeSpan.FromSeconds(1)).Token));
            await Assert.ThrowsAsync<AntiSSRFException>(() => client3.GetAsync(nonStandardIpUrl, new CancellationTokenSource(TimeSpan.FromSeconds(1)).Token));
            try
            {
                await client4.GetAsync(nonStandardIpUrl, new CancellationTokenSource(TimeSpan.FromSeconds(1)).Token);
            }
            catch (Exception ex) when (ex is not AntiSSRFException)
            {
                // Expected: timeout, socket exception, etc. - but not AntiSSRFException
            }
            catch (AntiSSRFException)
            {
                Assert.Fail("AntiSSRFException should not be thrown for localhost since it is not in the denied list");
            }


            var mappedIpUrl = "http://[::ffff:168.63.129.16]/";
            await Assert.ThrowsAsync<AntiSSRFException>(() => client.GetAsync(mappedIpUrl, new CancellationTokenSource(TimeSpan.FromSeconds(1)).Token));
            await Assert.ThrowsAsync<AntiSSRFException>(() => client2.GetAsync(mappedIpUrl, new CancellationTokenSource(TimeSpan.FromSeconds(1)).Token));
            await Assert.ThrowsAsync<AntiSSRFException>(() => client3.GetAsync(mappedIpUrl, new CancellationTokenSource(TimeSpan.FromSeconds(1)).Token));
            try
            {
                await client4.GetAsync(mappedIpUrl, new CancellationTokenSource(TimeSpan.FromSeconds(1)).Token);
            }
            catch (Exception ex) when (ex is not AntiSSRFException)
            {
                // Expected: timeout, socket exception, etc. - but not AntiSSRFException
            }
            catch (AntiSSRFException)
            {
                Assert.Fail("AntiSSRFException should not be thrown for localhost since it is not in the denied list");
            }


            var mappedIpUrl2 = "http://[::ffff:A83F:8110]/";
            await Assert.ThrowsAsync<AntiSSRFException>(() => client.GetAsync(mappedIpUrl2, new CancellationTokenSource(TimeSpan.FromSeconds(1)).Token));
            await Assert.ThrowsAsync<AntiSSRFException>(() => client2.GetAsync(mappedIpUrl2, new CancellationTokenSource(TimeSpan.FromSeconds(1)).Token));
            await Assert.ThrowsAsync<AntiSSRFException>(() => client3.GetAsync(mappedIpUrl2, new CancellationTokenSource(TimeSpan.FromSeconds(1)).Token));
            try
            {
                await client4.GetAsync(mappedIpUrl2, new CancellationTokenSource(TimeSpan.FromSeconds(1)).Token);
            }
            catch (Exception ex) when (ex is not AntiSSRFException)
            {
                // Expected: timeout, socket exception, etc. - but not AntiSSRFException
            }
            catch (AntiSSRFException)
            {
                Assert.Fail("AntiSSRFException should not be thrown for localhost since it is not in the denied list");
            }

            var redirectUrl = $"https://{TestDomain}/api/wireserver";
            await Assert.ThrowsAsync<AntiSSRFException>(() => client.GetAsync(redirectUrl, new CancellationTokenSource(TimeSpan.FromSeconds(1)).Token));
            await Assert.ThrowsAsync<AntiSSRFException>(() => client2.GetAsync(redirectUrl, new CancellationTokenSource(TimeSpan.FromSeconds(1)).Token));
            await Assert.ThrowsAsync<AntiSSRFException>(() => client3.GetAsync(redirectUrl, new CancellationTokenSource(TimeSpan.FromSeconds(1)).Token));
            try
            {
                await client4.GetAsync(redirectUrl, new CancellationTokenSource(TimeSpan.FromSeconds(1)).Token);
            }
            catch (Exception ex) when (ex is not AntiSSRFException)
            {
                // Expected: timeout, socket exception, etc. - but not AntiSSRFException
            }
            catch (AntiSSRFException)
            {
                Assert.Fail("AntiSSRFException should not be thrown for localhost since it is not in the denied list");
            }
        }

        [Fact]
        public async Task CheckDefaults_LocalHostAsync()
        {
            using HttpClient client = new(new AntiSSRFPolicy(PolicyConfigOptions.InternalOnly)
            {
                AllowPlainTextHttp = true
            }.GetHandler())
            {
                Timeout = TimeSpan.FromSeconds(3)
            };
            using HttpClient client2 = new(new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1)
            {
                AllowPlainTextHttp = true
            }.GetHandler())
            {
                Timeout = TimeSpan.FromSeconds(3)
            };
            using HttpClient client3 = new(new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyLatest)
            {
                AllowPlainTextHttp = true
            }.GetHandler())
            {
                Timeout = TimeSpan.FromSeconds(3)
            };
            using HttpClient client4 = new(new AntiSSRFPolicy(PolicyConfigOptions.None)
            {
                AllowPlainTextHttp = true
            }.GetHandler())
            {
                Timeout = TimeSpan.FromSeconds(3)
            };

            var ipUrl = "http://127.0.0.1/";
            await Assert.ThrowsAsync<AntiSSRFException>(async () => await client.GetAsync(ipUrl, CancellationToken.None));
            await Assert.ThrowsAsync<AntiSSRFException>(async () => await client2.GetAsync(ipUrl, CancellationToken.None));
            await Assert.ThrowsAsync<AntiSSRFException>(async () => await client3.GetAsync(ipUrl, CancellationToken.None));
            try
            {
                await client4.GetAsync(ipUrl, CancellationToken.None);
            }
            catch (Exception ex) when (ex is not AntiSSRFException)
            {
                // Expected: timeout, socket exception, etc. - but not AntiSSRFException
            }
            catch (AntiSSRFException)
            {
                Assert.Fail("AntiSSRFException should not be thrown for localhost since it is not in the denied list");
            }

            var nonStandardIpUrl = "http://0x7F.0x0.0x0.0x1/";
            await Assert.ThrowsAsync<AntiSSRFException>(() => client.GetAsync(nonStandardIpUrl, CancellationToken.None));
            await Assert.ThrowsAsync<AntiSSRFException>(() => client2.GetAsync(nonStandardIpUrl, CancellationToken.None));
            await Assert.ThrowsAsync<AntiSSRFException>(() => client3.GetAsync(nonStandardIpUrl, CancellationToken.None));
            try
            {
                await client4.GetAsync(nonStandardIpUrl, CancellationToken.None);
            }
            catch (Exception ex) when (ex is not AntiSSRFException)
            {
                // Expected: timeout, socket exception, etc. - but not AntiSSRFException
            }
            catch (AntiSSRFException)
            {
                Assert.Fail("AntiSSRFException should not be thrown for localhost since it is not in the denied list");
            }

            var mappedIpUrl = "http://[::ffff:127.0.0.1]/";
            await Assert.ThrowsAsync<AntiSSRFException>(() => client.GetAsync(mappedIpUrl, CancellationToken.None));
            await Assert.ThrowsAsync<AntiSSRFException>(() => client2.GetAsync(mappedIpUrl, CancellationToken.None));
            await Assert.ThrowsAsync<AntiSSRFException>(() => client3.GetAsync(mappedIpUrl, CancellationToken.None));
            try
            {
                await client4.GetAsync(mappedIpUrl, CancellationToken.None);
            }
            catch (Exception ex) when (ex is not AntiSSRFException)
            {
                // Expected: timeout, socket exception, etc. - but not AntiSSRFException
            }
            catch (AntiSSRFException)
            {
                Assert.Fail("AntiSSRFException should not be thrown for localhost since it is not in the denied list");
            }

            var mappedIpUrl2 = "http://[::ffff:7F00:1]/";
            await Assert.ThrowsAsync<AntiSSRFException>(() => client.GetAsync(mappedIpUrl2, CancellationToken.None));
            await Assert.ThrowsAsync<AntiSSRFException>(() => client2.GetAsync(mappedIpUrl2, CancellationToken.None));
            await Assert.ThrowsAsync<AntiSSRFException>(() => client3.GetAsync(mappedIpUrl2, CancellationToken.None));
            try
            {
                await client4.GetAsync(mappedIpUrl2, CancellationToken.None);
            }
            catch (Exception ex) when (ex is not AntiSSRFException)
            {
                // Expected: timeout, socket exception, etc. - but not AntiSSRFException
            }
            catch (AntiSSRFException)
            {
                Assert.Fail("AntiSSRFException should not be thrown for localhost since it is not in the denied list");
            }

            var redirectUrl = $"https://{TestDomain}/api/localhost";
            await Assert.ThrowsAsync<AntiSSRFException>(() => client.GetAsync(redirectUrl, CancellationToken.None));
            await Assert.ThrowsAsync<AntiSSRFException>(() => client2.GetAsync(redirectUrl, CancellationToken.None));
            await Assert.ThrowsAsync<AntiSSRFException>(() => client3.GetAsync(redirectUrl, CancellationToken.None));
            try
            {
                await client4.GetAsync(redirectUrl, CancellationToken.None);
            }
            catch (Exception ex) when (ex is not AntiSSRFException)
            {
                // Expected: timeout, socket exception, etc. - but not AntiSSRFException
            }
            catch (AntiSSRFException)
            {
                Assert.Fail("AntiSSRFException should not be thrown for localhost since it is not in the denied list");
            }

            var localhostUrl = "http://localhost/";
            await Assert.ThrowsAsync<AntiSSRFException>(() => client.GetAsync(localhostUrl, CancellationToken.None));
            await Assert.ThrowsAsync<AntiSSRFException>(() => client2.GetAsync(localhostUrl, CancellationToken.None));
            await Assert.ThrowsAsync<AntiSSRFException>(() => client3.GetAsync(localhostUrl, CancellationToken.None));
            try
            {
                await client4.GetAsync(localhostUrl, CancellationToken.None);
            }
            catch (Exception ex) when (ex is not AntiSSRFException)
            {
                // Expected: timeout, socket exception, etc. - but not AntiSSRFException
            }
            catch (AntiSSRFException)
            {
                Assert.Fail("AntiSSRFException should not be thrown for localhost since it is not in the denied list");
            }
        }

        [Fact]
        public async Task Allow_IPv4()
        {
            AntiSSRFPolicy policy = new(PolicyConfigOptions.None)
            {
                AllowPlainTextHttp = true,
                DenyAllUnspecifiedIPs = true
            };
            IPAddress[] testIpArr = Dns.GetHostAddresses(TestDomain);
            policy.AddAllowedAddresses(testIpArr.Select(ip => ip.ToString()).ToArray());
            using HttpClient client = new(policy.GetHandler())
            {
                Timeout = TimeSpan.FromSeconds(3)
            };

            // Allowed IPv4
            using var response = await client.GetAsync("http://" + testIpArr[0], CancellationToken.None);
            Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NotFound || (uint)response.StatusCode == BlockedByAzureFirewall, $"Request to IPv4 address {testIpArr[0]} should be allowed since it is in the allowed list, but got status code {response.StatusCode}");

            // Allowed IPv4-mapped IPv6
            try
            {
                using var response2 = await client.GetAsync("http://[" + testIpArr[0].MapToIPv6() + "]:80", CancellationToken.None);
                Assert.True(response2.StatusCode == HttpStatusCode.OK || response2.StatusCode == HttpStatusCode.NotFound || (uint)response2.StatusCode == BlockedByAzureFirewall, $"Request to IPv4-mapped IPv6 address {testIpArr[0].MapToIPv6()} should be allowed since it is in the allowed list, but got status code {response2.StatusCode}");
            }
            catch (Exception ex) when (ex is not AntiSSRFException)
            {
                // Pipeline struggles with some IPv6 addresses, so we catch exceptions that are not AntiSSRFException
            }
            catch (AntiSSRFException)
            {
                Assert.Fail("AntiSSRFException should not be thrown for IPv4-mapped IPv6 address since it is in the allowed list");
            }

            // Disallowed IPv4
            await Assert.ThrowsAsync<AntiSSRFException>(() => client.GetAsync("http://1.2.3.4", CancellationToken.None));

            // Disallowed IPv6
            await Assert.ThrowsAsync<AntiSSRFException>(() => client.GetAsync("http://[1:2:3:4:5:6:7:8]", CancellationToken.None));

            using var response3 = await client.GetAsync("http://" + TestDomain, CancellationToken.None);
            Assert.True(response3.StatusCode == HttpStatusCode.OK || response3.StatusCode == HttpStatusCode.NotFound || (uint)response3.StatusCode == BlockedByAzureFirewall, $"Request to domain {TestDomain} should be allowed since it is in the allowed list, but got status code {response3.StatusCode}");
        }

        [Fact]
        public async Task Allow_IPv4MappedIPv6()
        {
            AntiSSRFPolicy policy = new(PolicyConfigOptions.None)
            {
                AllowPlainTextHttp = true,
                DenyAllUnspecifiedIPs = true
            };
            IPAddress[] testIpArr = Dns.GetHostAddresses(TestDomain);
            policy.AddAllowedAddresses(testIpArr.Select(ip => ip.MapToIPv6().ToString()).ToArray());
            using HttpClient client = new(policy.GetHandler())
            {
                Timeout = TimeSpan.FromSeconds(3)
            };

            // Allowed IPv4
            using var response = await client.GetAsync("http://" + testIpArr[0], CancellationToken.None);
            Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NotFound || (uint)response.StatusCode == BlockedByAzureFirewall, $"Request to IPv4 address {testIpArr[0]} should be allowed since its IPv4-mapped IPv6 address is in the allowed list, but got status code {response.StatusCode}");

            // Allowed IPv4-mapped IPv6
            try
            {
                using var response2 = await client.GetAsync("http://[" + testIpArr[0].MapToIPv6() + "]:80", CancellationToken.None);
                Assert.True(response2.StatusCode == HttpStatusCode.OK || response2.StatusCode == HttpStatusCode.NotFound || (uint)response2.StatusCode == BlockedByAzureFirewall, $"Request to IPv4-mapped IPv6 address {testIpArr[0].MapToIPv6()} should be allowed since it is in the allowed list, but got status code {response2.StatusCode}");
            }
            catch (Exception ex) when (ex is not AntiSSRFException)
            {
                // Pipeline struggles with some IPv6 addresses, so we catch exceptions that are not AntiSSRFException
            }
            catch (AntiSSRFException)
            {
                Assert.Fail("AntiSSRFException should not be thrown for IPv4-mapped IPv6 address since it is in the allowed list");
            }

            using var response3 = await client.GetAsync("https://" + TestDomain, CancellationToken.None);
            Assert.True(response3.StatusCode == HttpStatusCode.OK || response3.StatusCode == HttpStatusCode.NotFound || (uint)response3.StatusCode == BlockedByAzureFirewall, $"Request to domain {TestDomain} should be allowed since its IPv4-mapped IPv6 address is in the allowed list, but got status code {response3.StatusCode}");

            // Disallowed IPv4
            await Assert.ThrowsAsync<AntiSSRFException>(() => client.GetAsync("http://1.2.3.4", CancellationToken.None));

            // Disallowed IPv6
            await Assert.ThrowsAsync<AntiSSRFException>(() => client.GetAsync("http://[1:2:3:4:5:6:7:8]", CancellationToken.None));
        }

        [Fact]
        public async Task Allow_IPv6()
        {
            AntiSSRFPolicy policy = new(PolicyConfigOptions.None)
            {
                AllowPlainTextHttp = true,
                DenyAllUnspecifiedIPs = true
            };
            string testIPv6 = "::1";
            policy.AddAllowedAddresses(new[] { testIPv6 });
            using HttpClient client = new(policy.GetHandler())
            {
                Timeout = TimeSpan.FromSeconds(3)
            };

            // Allowed IPv6
            try
            {
                await client.GetAsync($"http://[{testIPv6}]", CancellationToken.None);
            }
            catch (Exception ex) when (ex is not AntiSSRFException)
            {
                // Pipeline struggles with some IPv6 addresses, so we catch exceptions that are not AntiSSRFException
            }
            catch (AntiSSRFException)
            {
                Assert.Fail("AntiSSRFException should not be thrown for client3 since defaults are not used, but a timeout or socket exception may occur instead");
            }

            // Disallowed IPv4  
            await Assert.ThrowsAsync<AntiSSRFException>(() => client.GetAsync("https://1.2.3.4", CancellationToken.None));

            // Disallowed different IPv6
            await Assert.ThrowsAsync<AntiSSRFException>(() => client.GetAsync("https://[2606:4700:4700::1111]", CancellationToken.None));
        }

        [Fact]
        public async Task Deny_IPv4()
        {
            AntiSSRFPolicy policy = new(PolicyConfigOptions.None);
            IPAddress testIp = Dns.GetHostAddresses(TestDomain)[0];
            policy.AddDeniedAddresses(new[] { testIp.ToString() });
            using HttpClient client = new(policy.GetHandler())
            {
                Timeout = TimeSpan.FromSeconds(3)
            };

            // Denied IPv4
            await Assert.ThrowsAsync<AntiSSRFException>(() => client.GetAsync("https://" + testIp, CancellationToken.None));

            // Denied IPv4-mapped IPv6 (should also be denied since it's the same address)
            await Assert.ThrowsAsync<AntiSSRFException>(() => client.GetAsync("https://[" + testIp.MapToIPv6() + "]", CancellationToken.None));

            // Allowed different IPv4
            using var response = await client.GetAsync("https://github.com", CancellationToken.None);
            Assert.True(HttpStatusCode.OK == response.StatusCode || (uint)response.StatusCode == BlockedByAzureFirewall, $"Request to different IPv4 address should be allowed since only {testIp} is in the denied list, but got status code {response.StatusCode}");

            // Allowed IPv6
            try
            {
                using var response2 = await client.GetAsync("https://ipv6.google.com", CancellationToken.None);
                Assert.True(HttpStatusCode.OK == response2.StatusCode || (uint)response2.StatusCode == BlockedByAzureFirewall, $"Request to IPv6 address should be allowed since it is not in the denied list, but got status code {response2.StatusCode}");
            }
            catch (Exception ex) when (ex is not AntiSSRFException)
            {
                // Pipeline struggles with some IPv6 addresses
            }
            catch (AntiSSRFException)
            {
                Assert.Fail("AntiSSRFException should not be thrown for IPv6 address since it is not in the denied list");
            }
        }

        [Fact]
        public async Task Deny_IPv4MappedIPv6()
        {
            AntiSSRFPolicy policy = new(PolicyConfigOptions.None)
            {
                AllowPlainTextHttp = true
            };
            IPAddress testIp = Dns.GetHostAddresses(TestDomain)[0];
            policy.AddDeniedAddresses(new[] { testIp.MapToIPv6().ToString() });
            using HttpClient client = new(policy.GetHandler())
            {
                Timeout = TimeSpan.FromSeconds(3)
            };

            // Denied IPv4 (should be denied since IPv4-mapped IPv6 denies the underlying IPv4)
            await Assert.ThrowsAsync<AntiSSRFException>(() => client.GetAsync("http://" + testIp, CancellationToken.None));

            // Denied IPv4-mapped IPv6
            await Assert.ThrowsAsync<AntiSSRFException>(() => client.GetAsync("http://[" + testIp.MapToIPv6() + "]", CancellationToken.None));

            // Allowed different IPv4
            using var response = await client.GetAsync($"https://github.com", CancellationToken.None);
            Assert.True(HttpStatusCode.OK == response.StatusCode || (uint)response.StatusCode == BlockedByAzureFirewall, $"Request to different IPv4 address should be allowed since it is not in the denied list, but got status code {response.StatusCode}");

            // Allowed IPv6
            try
            {
                using var response2 = await client.GetAsync("https://ipv6.google.com", CancellationToken.None);
                Assert.True(HttpStatusCode.OK == response2.StatusCode || (uint)response2.StatusCode == BlockedByAzureFirewall, $"Request to IPv6 address should be allowed since it is not in the denied list, but got status code {response2.StatusCode}");
            }
            catch (Exception ex) when (ex is not AntiSSRFException)
            {
                // Pipeline struggles with some IPv6 addresses
            }
            catch (AntiSSRFException)
            {
                Assert.Fail("AntiSSRFException should not be thrown for IPv6 address since it is not in the denied list");
            }
        }

        [Fact]
        public async Task Deny_IPv6Async()
        {
            AntiSSRFPolicy policy = new(PolicyConfigOptions.None)
            {
                AllowPlainTextHttp = true
            };
            string testIPv6 = "2001:4860:4860::8888";
            policy.AddDeniedAddresses(new[] { testIPv6 });
            using HttpClient client = new(policy.GetHandler())
            {
                Timeout = TimeSpan.FromSeconds(3)
            };

            // Denied IPv6
            await Assert.ThrowsAsync<AntiSSRFException>(async () => await client.GetAsync($"http://[{testIPv6}]", CancellationToken.None));

            // Allowed IPv4
            using var response = await client.GetAsync($"https://{TestDomain}", CancellationToken.None);
            Assert.True(HttpStatusCode.OK == response.StatusCode || (uint)response.StatusCode == BlockedByAzureFirewall, $"Request to domain {TestDomain} should be allowed since it is not in the denied list, but got status code {response.StatusCode}");

            // Allowed different IPv6
            try
            {
                using var response2 = await client.GetAsync("http://[2606:4700:4700::1111]", CancellationToken.None);
                Assert.True(HttpStatusCode.OK == response2.StatusCode || (uint)response2.StatusCode == BlockedByAzureFirewall, $"Request to different IPv6 address should be allowed since it is not in the denied list, but got status code {response2.StatusCode}");
            }
            catch (Exception ex) when (ex is not AntiSSRFException)
            {
                // Pipeline struggles with some IPv6 addresses, so we catch exceptions that are not AntiSSRFException
            }
            catch (AntiSSRFException)
            {
                Assert.Fail("AntiSSRFException should not be thrown for different IPv6 address since it is not in the denied list");
            }

        }

        [Fact]
        public async Task BothAllowAndDeny()
        {
            AntiSSRFPolicy policy = new(PolicyConfigOptions.None);
            IPAddress testIp = Dns.GetHostAddresses(TestDomain)[0];
            policy.AddDeniedAddresses(new[] { testIp.ToString() });
            policy.AddAllowedAddresses(new[] { testIp.MapToIPv6().ToString() });
            using HttpClient client = new(policy.GetHandler())
            {
                Timeout = TimeSpan.FromSeconds(3)
            };

            using var response = await client.GetAsync("https://" + TestDomain, CancellationToken.None);
            Assert.True(HttpStatusCode.OK == response.StatusCode || (uint)response.StatusCode == BlockedByAzureFirewall, $"Request to domain {TestDomain} should be allowed since it is in the allowed list, but got status code {response.StatusCode}");
        }

        [Fact]
        public void NoEditsAfterHandler()
        {
            var policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1);
            policy.GetHandler();

            Assert.Throws<AntiSSRFException>(() => policy.AddAllowedAddresses(["1.2.3.4"]));
            Assert.Throws<AntiSSRFException>(() => policy.AddDeniedAddresses(["1.2.3.4"]));
        }
    }
}
