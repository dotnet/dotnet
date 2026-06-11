// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect.Tests
{
    public class OpenIdConnectConfigurationRetrieverTests
    {
        [Fact]
        public async Task FromNetwork()
        {
            OpenIdConnectConfiguration configuration = await GetConfigurationFromHttpAsync(OpenIdConfigData.AADCommonUrl, expectedException: ExpectedException.NoExceptionExpected);
            Assert.NotNull(configuration);

            await GetConfigurationFromHttpAsync(string.Empty, expectedException: ExpectedException.ArgumentNullException());
            await GetConfigurationFromHttpAsync(OpenIdConfigData.BadUri, expectedException: ExpectedException.ArgumentException("IDX20108:"));
            await GetConfigurationFromHttpAsync(OpenIdConfigData.HttpsBadUri, expectedException: ExpectedException.IOException(inner: typeof(HttpRequestException)));
        }

        [Fact]
        public async Task FromFile()
        {
            var context = new CompareContext
            {
                PropertiesToIgnoreWhenComparing = new Dictionary<Type, List<string>>
                {
                    // If the objects being compared are created from the same string and they are equal, the string itself can be ignored.
                    // The strings may not be equal because of whitespace, but the json they represent is semantically identical.
                    { typeof(JsonWebKeySet), [ "JsonData" ] },
                }
            };
            var configuration = await GetConfigurationAsync(
                OpenIdConfigData.JsonFile,
                ExpectedException.NoExceptionExpected,
                OpenIdConfigData.FullyPopulatedWithKeys,
                context);

            // jwt_uri points to bad formated JSON
            configuration = await GetConfigurationAsync(
                OpenIdConfigData.JsonWebKeySetBadUriFile,
                ExpectedException.IOException(inner: typeof(FileNotFoundException)),
                null,
                context);

            // reading form a file that does not exist
            configuration = await GetConfigurationAsync(
                "FileDoesNotExist.json",
                ExpectedException.IOException(inner: typeof(FileNotFoundException)), null, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public async Task FromJson()
        {
            var context = new CompareContext
            {
                PropertiesToIgnoreWhenComparing = new Dictionary<Type, List<string>>
                {
                    // If the objects being compared are created from the same string and they are equal, the string itself can be ignored.
                    // The strings may not be equal because of whitespace, but the json they represent is semantically identical.
                    { typeof(JsonWebKeySet), [ "JsonData" ] },
                }
            };
            var configuration = await GetConfigurationFromMixedAsync(
                OpenIdConfigData.OpenIdConnectMetadataPingString,
                expectedException: ExpectedException.NoExceptionExpected);

            configuration = await GetConfigurationFromMixedAsync(
                OpenIdConfigData.OpenIdConnectMetadataPingLabsJWKSString,
                expectedException: ExpectedException.NoExceptionExpected);
            IdentityComparer.AreEqual(configuration, OpenIdConfigData.PingLabs, context);

            configuration = await GetConfigurationFromMixedAsync(
                OpenIdConfigData.JsonAllValues,
                expectedException: ExpectedException.NoExceptionExpected);
            IdentityComparer.AreEqual(configuration, OpenIdConfigData.FullyPopulatedWithKeys, context);

            // jwt_uri is not reachable
            await GetConfigurationFromTextAsync(
                OpenIdConfigData.OpenIdConnectMetadataBadUriKeysString,
                string.Empty,
                expectedException: ExpectedException.IOException());

            // stream is not well formated
            await GetConfigurationFromTextAsync(
                OpenIdConfigData.OpenIdConnectMetadataBadFormatString,
                string.Empty,
                expectedException: new ExpectedException(typeExpected: typeof(System.Text.Json.JsonException), ignoreInnerException: true));

            configuration = await GetConfigurationFromMixedAsync(
                OpenIdConfigData.OpenIdConnectMetadataSingleX509DataString,
                expectedException: ExpectedException.NoExceptionExpected);
            IdentityComparer.AreEqual(configuration, OpenIdConfigData.SingleX509Data, context);

            // dnx 5.0 throws a different exception
            // 5.0 - Internal.Cryptography.CryptoThrowHelper+WindowsCryptographicException
            // 4.5.1 - System.Security.Cryptography.CryptographicException
            // for now turn off checking for inner
            var ee = ExpectedException.InvalidOperationException(inner: typeof(CryptographicException));
            ee.IgnoreInnerException = true;

            await GetConfigurationFromMixedAsync(
                OpenIdConfigData.OpenIdConnectMetadataBadX509DataString,
                expectedException: ExpectedException.NoExceptionExpected);

            await GetConfigurationFromMixedAsync(
                OpenIdConfigData.OpenIdConnectMetadataBadBase64DataString,
                expectedException: ExpectedException.NoExceptionExpected);

            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void Properties()
        {
            // ensure that each property can be set independently
            var context = new CompareContext();
            GetAndCheckConfiguration("authorization_endpoint", "AuthorizationEndpoint", context);
            GetAndCheckConfiguration("check_session_iframe", "CheckSessionIframe", context);
            GetAndCheckConfiguration("end_session_endpoint", "EndSessionEndpoint", context);
            GetAndCheckConfiguration("introspection_endpoint", "IntrospectionEndpoint", context);
            GetAndCheckConfiguration("jwks_uri", "JwksUri", context, OpenIdConfigData.AADCommonUrl);
            GetAndCheckConfiguration("token_endpoint", "TokenEndpoint", context);
            GetAndCheckConfiguration("userinfo_endpoint", "UserInfoEndpoint", context);

            TestUtilities.AssertFailIfErrors(context);
        }

        private async Task<OpenIdConnectConfiguration> GetConfigurationFromHttpAsync(string uri, ExpectedException expectedException, OpenIdConnectConfiguration expectedConfiguration = null)
        {
            OpenIdConnectConfiguration openIdConnectConfiguration = null;
            try
            {
                openIdConnectConfiguration = await OpenIdConnectConfigurationRetriever.GetAsync(uri, CancellationToken.None);
                expectedException.ProcessNoException();
            }
            catch (Exception exception)
            {
                expectedException.ProcessException(exception);
            }

            if (expectedConfiguration != null)
            {
                Assert.True(IdentityComparer.AreEqual(openIdConnectConfiguration, expectedConfiguration));
            }

            return openIdConnectConfiguration;
        }

        private async Task<OpenIdConnectConfiguration> GetConfigurationAsync(string uri, ExpectedException expectedException, OpenIdConnectConfiguration expectedConfiguration, CompareContext context)
        {
            OpenIdConnectConfiguration openIdConnectConfiguration = null;
            try
            {
                openIdConnectConfiguration = await OpenIdConnectConfigurationRetriever.GetAsync(uri, new FileDocumentRetriever(), CancellationToken.None);
                expectedException.ProcessNoException(context);
            }
            catch (Exception exception)
            {
                expectedException.ProcessException(exception, context.Diffs);
            }

            if (expectedConfiguration != null)
                IdentityComparer.AreEqual(openIdConnectConfiguration, expectedConfiguration, context);

            return openIdConnectConfiguration;
        }

        private async Task<OpenIdConnectConfiguration> GetConfigurationFromTextAsync(string primaryDocument, string secondaryDocument, ExpectedException expectedException, OpenIdConnectConfiguration expectedConfiguration = null)
        {
            OpenIdConnectConfiguration openIdConnectConfiguration = null;
            try
            {
                openIdConnectConfiguration = await OpenIdConnectConfigurationRetriever.GetAsync(
                     "primary", new TestDocumentRetriever(primaryDocument, secondaryDocument), CancellationToken.None);
                expectedException.ProcessNoException();
            }
            catch (Exception exception)
            {
                expectedException.ProcessException(exception);
            }

            if (expectedConfiguration != null)
            {
                Assert.True(IdentityComparer.AreEqual(openIdConnectConfiguration, expectedConfiguration));
            }

            return openIdConnectConfiguration;
        }

        private async Task<OpenIdConnectConfiguration> GetConfigurationFromMixedAsync(string primaryDocument, ExpectedException expectedException, OpenIdConnectConfiguration expectedConfiguration = null)
        {
            OpenIdConnectConfiguration openIdConnectConfiguration = null;
            try
            {
                openIdConnectConfiguration = await OpenIdConnectConfigurationRetriever.GetAsync("primary",
                    new TestDocumentRetriever(primaryDocument, new FileDocumentRetriever()), CancellationToken.None);
                expectedException.ProcessNoException();
            }
            catch (Exception exception)
            {
                expectedException.ProcessException(exception);
            }

            if (expectedConfiguration != null)
            {
                Assert.True(IdentityComparer.AreEqual(openIdConnectConfiguration, expectedConfiguration));
            }

            return openIdConnectConfiguration;
        }

        private void GetAndCheckConfiguration(string jsonName, string propertyName, CompareContext context, string propertyValue = null)
        {
            string jsonValue = propertyValue;
            if (jsonValue == null)
            {
                jsonValue = Guid.NewGuid().ToString();
            }

            string jsonString = @"{""" + jsonName + @""":""" + jsonValue + @"""}";
            try
            {
                OpenIdConnectConfiguration openIdConnectConfiguration = new OpenIdConnectConfiguration(jsonString);
                OpenIdConnectConfiguration expectedConfiguration = new OpenIdConnectConfiguration();
                TestUtilities.SetProperty(expectedConfiguration, propertyName, jsonValue);
                IdentityComparer.AreEqual(openIdConnectConfiguration, expectedConfiguration, context);
            }
            catch (Exception exception)
            {
                ExpectedException.NoExceptionExpected.ProcessException(exception, context.Diffs);
            }

            return;
        }

        private class TestDocumentRetriever : IDocumentRetriever
        {
            private string _primaryDocument;
            private string _secondaryDocument;
            private IDocumentRetriever _fallback;

            public TestDocumentRetriever(string primaryDocument, string secondaryDocument)
            {
                _primaryDocument = primaryDocument;
                _secondaryDocument = secondaryDocument;
            }

            public TestDocumentRetriever(string primaryDocument, IDocumentRetriever fallback)
            {
                _primaryDocument = primaryDocument;
                _fallback = fallback;
            }

            public Task<string> GetDocumentAsync(string address, CancellationToken cancel)
            {
                if (string.Equals("primary", address))
                {
                    return Task.FromResult(_primaryDocument);
                }
                if (string.Equals("secondary", address) && !string.IsNullOrWhiteSpace(_secondaryDocument))
                {
                    return Task.FromResult(_secondaryDocument);
                }
                if (_fallback != null)
                {
                    return _fallback.GetDocumentAsync(address, cancel);
                }
                throw new IOException("Document not found: " + address);
            }
        }
    }
}
