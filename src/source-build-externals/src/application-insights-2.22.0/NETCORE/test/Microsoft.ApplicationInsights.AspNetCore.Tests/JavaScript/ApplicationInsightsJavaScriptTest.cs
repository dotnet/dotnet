namespace Microsoft.Framework.DependencyInjection.Test
{
    using System.Security.Principal;
    using System.Text.Encodings.Web;
    using System.Text.Unicode;
    using Microsoft.ApplicationInsights.AspNetCore;
    using Microsoft.ApplicationInsights.AspNetCore.Extensions;
    using Microsoft.ApplicationInsights.AspNetCore.Tests.Helpers;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
    using Xunit;

    public static class ApplicationInsightsJavaScriptTest
    {
        private static JavaScriptEncoder encoder = JavaScriptEncoder.Create(new UnicodeRange[] { UnicodeRanges.BasicLatin });

        [Fact]
        public static void SnippetWillBeEmptyWhenInstrumentationKeyIsNotDefined()
        {
            var telemetryConfigurationWithNullKey = new TelemetryConfiguration();
            var snippet = new JavaScriptSnippet(telemetryConfigurationWithNullKey, GetOptions(false), null, encoder);
            Assert.Equal(string.Empty, snippet.FullScript);
        }

        [Fact]
        public static void SnippetWillBeEmptyWhenInstrumentationKeyIsEmpty()
        {
            var telemetryConfigurationWithEmptyKey = new TelemetryConfiguration {InstrumentationKey = string.Empty};
            var snippet = new JavaScriptSnippet(telemetryConfigurationWithEmptyKey, GetOptions(false), null, encoder);
            Assert.Equal(string.Empty, snippet.FullScript);
        }

        [Fact]
        public static void SnippetWillBeEmptyWhenTelemetryDisabled()
        {
            var telemetryConfigurationWithEmptyKey = new TelemetryConfiguration
            {
                InstrumentationKey = "NonEmpty",
                DisableTelemetry = true
            };
            var snippet = new JavaScriptSnippet(telemetryConfigurationWithEmptyKey, GetOptions(false), null, encoder);
            Assert.Equal(string.Empty, snippet.FullScript);
        }

        [Fact]
        [Trait("Trait", "ConnectionString")]
        public static void SnippetWillIncludeConnectionStringAsSubstring()
        {
            string testConnString = "InstrumentationKey=00000000-0000-0000-0000-000000000000";
            var telemetryConfiguration = new TelemetryConfiguration { ConnectionString = testConnString };
            var snippet = new JavaScriptSnippet(telemetryConfiguration, GetOptions(false), null, encoder);
            Assert.Contains("connectionString: '" + testConnString + "'", snippet.FullScript);
        }

        [Fact]
        public static void SnippetWillIncludeInstrumentationKeyAsSubstring()
        {
            string unittestkey = "unittestkey";
            var telemetryConfiguration = new TelemetryConfiguration { InstrumentationKey = unittestkey };
            var snippet = new JavaScriptSnippet(telemetryConfiguration, GetOptions(false), null, encoder);
            Assert.Contains("instrumentationKey: '" + unittestkey + "'", snippet.FullScript);
        }

        [Fact]
        public static void SnippetWillIncludeAuthUserNameIfEnabledAndAuthenticated()
        {
            string unittestkey = "unittestkey";
            var telemetryConfiguration = new TelemetryConfiguration { InstrumentationKey = unittestkey };
            var snippet = new JavaScriptSnippet(telemetryConfiguration, GetOptions(true), GetHttpContextAccessor("username", true), encoder);
            Assert.Contains("setAuthenticatedUserContext(\"username\")", snippet.FullScript);
        }

        [Fact]
        public static void SnippetWillNotIncludeAuthUserNameIfEnabledAndAuthenticated()
        {
            string unittestkey = "unittestkey";
            var telemetryConfiguration = new TelemetryConfiguration { InstrumentationKey = unittestkey };
            var snippet = new JavaScriptSnippet(telemetryConfiguration, GetOptions(true), GetHttpContextAccessor("username", false), encoder);
            Assert.DoesNotContain("appInsights.setAuthenticatedUserContext(", snippet.FullScript);
        }

        [Fact]
        public static void SnippetWillIncludeEscapedAuthUserNameIfEnabledAndAuthenticated()
        {
            string unittestkey = "unittestkey";
            var telemetryConfiguration = new TelemetryConfiguration { InstrumentationKey = unittestkey };
            var snippet = new JavaScriptSnippet(telemetryConfiguration, GetOptions(true), GetHttpContextAccessor("user\\name", true), encoder);
            Assert.Contains("setAuthenticatedUserContext(\"user\\\\name\")", snippet.FullScript);
        }

        [Fact]
        public static void CrossSiteScriptingIsBlockedByEncoding()
        {
            string unittestkey = "unittestkey";
            var telemetryConfiguration = new TelemetryConfiguration { InstrumentationKey = unittestkey };
            var snippet = new JavaScriptSnippet(telemetryConfiguration, GetOptions(true), GetHttpContextAccessor("<script>alert('hack');</script>", true), encoder);
            Assert.Contains("setAuthenticatedUserContext(\"\\u003Cscript\\u003Ealert(\\u0027hack\\u0027);", snippet.FullScript);
        }

        private static IOptions<ApplicationInsightsServiceOptions> GetOptions(bool enableAuthSnippet)
        {
            return new OptionsWrapper<ApplicationInsightsServiceOptions>(new ApplicationInsightsServiceOptions()
            {
                EnableAuthenticationTrackingJavaScript = enableAuthSnippet
            });
        }

        private static IHttpContextAccessor GetHttpContextAccessor(string name, bool isAuthenticated)
        {
            return new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new GenericPrincipal(new IdentityStub() { Name = name, IsAuthenticated = isAuthenticated }, new string[0])
                }
            };
        }

        /// <summary>
        /// Class that is used in unit tests and allows to override main IIdentity properties.
        /// </summary>
        private class IdentityStub : IIdentity
        {
            /// <inheritdoc />
            public string AuthenticationType { get; set; }

            /// <inheritdoc />
            public bool IsAuthenticated { get; set; }

            /// <inheritdoc />
            public string Name { get; set; }
        }
    }
}
