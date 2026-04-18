// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Xunit;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect.Tests
{
    /// <summary>
    /// </summary>
    public class OpenIdConnectConfigurationTests
    {
        [Fact]
        public void Constructors()
        {
            var context = new CompareContext { Title = "OpenIdConnectConfigurationTests.Constructors" };

            RunOpenIdConnectConfigurationTest(
                (string)null,
                new OpenIdConnectConfiguration(),
                ExpectedException.ArgumentNullException(),
                context);

            RunOpenIdConnectConfigurationTest(
                OpenIdConfigData.JsonAllValues,
                OpenIdConfigData.FullyPopulated,
                ExpectedException.NoExceptionExpected,
                context);

            RunOpenIdConnectConfigurationTest(
                OpenIdConfigData.OpenIdConnectMetatadataBadJson,
                null,
                new ExpectedException(typeof(ArgumentException), substringExpected: "IDX21815:", ignoreInnerException: true),
                context);

            TestUtilities.AssertFailIfErrors(context);
        }

        private void RunOpenIdConnectConfigurationTest(object obj, OpenIdConnectConfiguration compareTo, ExpectedException expectedException, CompareContext context, bool asString = true)
        {
            bool exceptionHit = false;

            OpenIdConnectConfiguration openIdConnectConfiguration = null;
            try
            {
                if (obj is string || asString)
                {
                    openIdConnectConfiguration = new OpenIdConnectConfiguration(obj as string);
                }

                expectedException.ProcessNoException(context.Diffs);
            }
            catch (Exception ex)
            {
                exceptionHit = true;
                expectedException.ProcessException(ex, context.Diffs);
            }

            if (!exceptionHit && compareTo != null)
            {
                IdentityComparer.AreEqual(openIdConnectConfiguration, compareTo, context);
            }
        }

        [Fact]
        public void Defaults()
        {
            OpenIdConnectConfiguration configuration = new OpenIdConnectConfiguration();
            Assert.NotNull(configuration.AcrValuesSupported);
            Assert.NotNull(configuration.AuthorizationDetailsTypesSupported);
            Assert.NotNull(configuration.AuthorizationEncryptionAlgValuesSupported);
            Assert.NotNull(configuration.AuthorizationEncryptionEncValuesSupported);
            Assert.NotNull(configuration.AuthorizationSigningAlgValuesSupported);
            Assert.False(configuration.AuthorizationResponseIssParameterSupported);
            Assert.NotNull(configuration.BackchannelAuthenticationRequestSigningAlgValuesSupported);
            Assert.NotNull(configuration.BackchannelTokenDeliveryModesSupported);
            Assert.False(configuration.BackchannelUserCodeParameterSupported);
            Assert.NotNull(configuration.ClaimsSupported);
            Assert.NotNull(configuration.ClaimsLocalesSupported);
            Assert.False(configuration.ClaimsParameterSupported);
            Assert.NotNull(configuration.ClaimTypesSupported);
            Assert.NotNull(configuration.CodeChallengeMethodsSupported);
            Assert.NotNull(configuration.DisplayValuesSupported);
            Assert.NotNull(configuration.DPoPSigningAlgValuesSupported);
            Assert.NotNull(configuration.GrantTypesSupported);
            Assert.False(configuration.HttpLogoutSupported);
            Assert.NotNull(configuration.IdTokenEncryptionAlgValuesSupported);
            Assert.NotNull(configuration.IdTokenEncryptionEncValuesSupported);
            Assert.NotNull(configuration.IdTokenSigningAlgValuesSupported);
            Assert.NotNull(configuration.IntrospectionEndpointAuthMethodsSupported);
            Assert.NotNull(configuration.IntrospectionEndpointAuthSigningAlgValuesSupported);
            Assert.NotNull(configuration.PromptValuesSupported);
            Assert.NotNull(configuration.RequestObjectEncryptionAlgValuesSupported);
            Assert.NotNull(configuration.RequestObjectEncryptionEncValuesSupported);
            Assert.NotNull(configuration.RequestObjectSigningAlgValuesSupported);
            Assert.False(configuration.RequestParameterSupported);
            Assert.False(configuration.RequirePushedAuthorizationRequests);
            Assert.False(configuration.RequestUriParameterSupported);
            Assert.False(configuration.RequireRequestUriRegistration);
            Assert.NotNull(configuration.ResponseModesSupported);
            Assert.NotNull(configuration.ResponseTypesSupported);
            Assert.NotNull(configuration.RevocationEndpointAuthMethodsSupported);
            Assert.NotNull(configuration.RevocationEndpointAuthSigningAlgValuesSupported);
            Assert.NotNull(configuration.ScopesSupported);
            Assert.NotNull(configuration.SigningKeys);
            Assert.NotNull(configuration.SubjectTypesSupported);
            Assert.NotNull(configuration.TokenEndpointAuthMethodsSupported);
            Assert.NotNull(configuration.TokenEndpointAuthSigningAlgValuesSupported);
            Assert.False(configuration.TlsClientCertificateBoundAccessTokens);
            Assert.NotNull(configuration.UILocalesSupported);
            Assert.NotNull(configuration.UserInfoEndpointEncryptionAlgValuesSupported);
            Assert.NotNull(configuration.UserInfoEndpointEncryptionEncValuesSupported);
            Assert.NotNull(configuration.UserInfoEndpointSigningAlgValuesSupported);
        }

        // If the OpenIdConnect metadata has a "SigningKeys" claim, it should NOT be deserialized into the corresponding OpenIdConnectConfiguration.SigningKeys property.
        // This value should only be populated from the 'jwks_uri' claim.
        [Fact]
        public void DeserializeOpenIdConnectConfigurationWithSigningKeys()
        {
            TestUtilities.WriteHeader($"{this}.DeserializeOpenIdConnectConfigurationWithSigningKeys");
            var context = new CompareContext();

            string json = OpenIdConnectConfiguration.Write(new OpenIdConnectConfiguration(OpenIdConfigData.JsonWithSigningKeys));

            var config = OpenIdConnectConfiguration.Create(json);

            // "SigningKeys" should be found in AdditionalData.
            if (!config.AdditionalData.ContainsKey("SigningKeys"))
                context.AddDiff(@"!config.AdditionalData.ContainsKey(""SigningKeys"")");

            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void GetSets()
        {
            OpenIdConnectConfiguration configuration = new OpenIdConnectConfiguration();
            Type type = typeof(OpenIdConnectConfiguration);
            PropertyInfo[] properties = type.GetProperties();
            if (properties.Length != 68)
                Assert.True(false, "Number of properties has changed from 68 to: " + properties.Length + ", adjust tests");

            TestUtilities.CallAllPublicInstanceAndStaticPropertyGets(configuration, "OpenIdConnectConfiguration_GetSets");

            GetSetContext context =
                new GetSetContext
                {
                    PropertyNamesAndSetGetValue = new List<KeyValuePair<string, List<object>>>
                        {
                            new KeyValuePair<string, List<object>>("AcrValuesSupported", new List<object>{ false, true, true }),
                            new KeyValuePair<string, List<object>>("AuthorizationDetailsTypesSupported", new List<object>{ false, true, true }),
                            new KeyValuePair<string, List<object>>("AuthorizationEndpoint", new List<object>{ (string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString() }),
                            new KeyValuePair<string, List<object>>("AuthorizationEncryptionAlgValuesSupported", new List<object>{ false, true, true }),
                            new KeyValuePair<string, List<object>>("AuthorizationEncryptionEncValuesSupported", new List<object>{ false, true, true }),
                            new KeyValuePair<string, List<object>>("AuthorizationResponseIssParameterSupported", new List<object>{ false, true, true }),
                            new KeyValuePair<string, List<object>>("AuthorizationSigningAlgValuesSupported", new List<object>{ false, true, true }),
                            new KeyValuePair<string, List<object>>("BackchannelAuthenticationEndpoint", new List<object>{ (string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString() }),
                            new KeyValuePair<string, List<object>>("BackchannelUserCodeParameterSupported", new List<object>{ false, true, true }),
                            new KeyValuePair<string, List<object>>("CheckSessionIframe", new List<object>{ (string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString() }),
                            new KeyValuePair<string, List<object>>("ClaimsParameterSupported", new List<object>{ false, true, false }),
                            new KeyValuePair<string, List<object>>("CodeChallengeMethodsSupported", new List<object>{ false, true, true }),
                            new KeyValuePair<string, List<object>>("DeviceAuthorizationEndpoint", new List<object>{ (string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString() }),
                            new KeyValuePair<string, List<object>>("EndSessionEndpoint", new List<object>{ (string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString() }),
                            new KeyValuePair<string, List<object>>("HttpLogoutSupported", new List<object>{ false, true, true }),
                            new KeyValuePair<string, List<object>>("IntrospectionEndpoint", new List<object>{ (string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString() }),
                            new KeyValuePair<string, List<object>>("Issuer",  new List<object>{ (string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString() }),
                            new KeyValuePair<string, List<object>>("JwksUri",  new List<object>{ (string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString() }),
                            new KeyValuePair<string, List<object>>("JsonWebKeySet",  new List<object>{ null, new JsonWebKeySet() }),
                            new KeyValuePair<string, List<object>>("LogoutSessionSupported", new List<object>{ false, true, true }),
                            new KeyValuePair<string, List<object>>("OpPolicyUri", new List<object>{ (string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString() }),
                            new KeyValuePair<string, List<object>>("OpTosUri", new List<object>{ (string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString() }),
                            new KeyValuePair<string, List<object>>("PushedAuthorizationRequestEndpoint", new List<object>{ (string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString() }),
                            new KeyValuePair<string, List<object>>("RegistrationEndpoint", new List<object>{ (string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString() }),
                            new KeyValuePair<string, List<object>>("RequestParameterSupported", new List<object>{ false, true, false }),
                            new KeyValuePair<string, List<object>>("RequestUriParameterSupported", new List<object>{ false, true, true }),
                            new KeyValuePair<string, List<object>>("RequirePushedAuthorizationRequests", new List<object>{ false, true, true }),
                            new KeyValuePair<string, List<object>>("RequireRequestUriRegistration", new List<object>{ false, true, true }),
                            new KeyValuePair<string, List<object>>("RevocationEndpoint", new List<object>{ (string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString() }),
                            new KeyValuePair<string, List<object>>("RevocationEndpointAuthMethodsSupported", new List<object>{ false, true, true }),
                            new KeyValuePair<string, List<object>>("RevocationEndpointAuthSigningAlgValuesSupported", new List<object>{ false, true, true }),
                            new KeyValuePair<string, List<object>>("ServiceDocumentation", new List<object>{ (string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString() }),
                            new KeyValuePair<string, List<object>>("TlsClientCertificateBoundAccessTokens", new List<object>{ false, true, false }),
                            new KeyValuePair<string, List<object>>("TokenEndpoint", new List<object>{ (string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString() }),
                            new KeyValuePair<string, List<object>>("UserInfoEndpoint", new List<object>{ (string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString() }),
                        },

                    Object = configuration,
                };

            TestUtilities.GetSet(context);
            TestUtilities.AssertFailIfErrors("OpenIdConnectConfiguration_GetSets", context.Errors);

            string authorization_Endpoint = Guid.NewGuid().ToString();
            string end_Session_Endpoint = Guid.NewGuid().ToString();
            string frontchannelLogoutSessionSupported = "true";
            string frontchannelLogoutSupported = "true";
            string introspection_Endpoint = Guid.NewGuid().ToString();
            string issuer = Guid.NewGuid().ToString();
            string jwks_Uri = Guid.NewGuid().ToString();
            string token_Endpoint = Guid.NewGuid().ToString();

            configuration = new OpenIdConnectConfiguration()
            {
                AuthorizationEndpoint = authorization_Endpoint,
                EndSessionEndpoint = end_Session_Endpoint,
                FrontchannelLogoutSessionSupported = frontchannelLogoutSessionSupported,
                FrontchannelLogoutSupported = frontchannelLogoutSupported,
                IntrospectionEndpoint = introspection_Endpoint,
                Issuer = issuer,
                JwksUri = jwks_Uri,
                TokenEndpoint = token_Endpoint,
            };

            List<SecurityKey> securityKeys = new List<SecurityKey> { new X509SecurityKey(KeyingMaterial.Cert_1024), new X509SecurityKey(KeyingMaterial.DefaultCert_2048) };
            configuration.SigningKeys.Add(new X509SecurityKey(KeyingMaterial.Cert_1024));
            configuration.SigningKeys.Add(new X509SecurityKey(KeyingMaterial.DefaultCert_2048));

            List<string> errors = new List<string>();

            if (!string.Equals(configuration.AuthorizationEndpoint, authorization_Endpoint))
                errors.Add(string.Format(CultureInfo.InvariantCulture, "configuration.AuthorizationEndpoint != authorization_Endpoint. '{0}', '{1}'.", configuration.AuthorizationEndpoint, authorization_Endpoint));

            if (!string.Equals(configuration.EndSessionEndpoint, end_Session_Endpoint))
                errors.Add(string.Format(CultureInfo.InvariantCulture, "configuration.EndSessionEndpoint != end_Session_Endpoint. '{0}', '{1}'.", configuration.EndSessionEndpoint, end_Session_Endpoint));

            if (!string.Equals(configuration.FrontchannelLogoutSessionSupported, frontchannelLogoutSessionSupported))
                errors.Add(string.Format(CultureInfo.InvariantCulture, "configuration.FrontchannelLogoutSessionSupported != frontchannelLogoutSessionSupported. '{0}', '{1}'.", configuration.FrontchannelLogoutSessionSupported, frontchannelLogoutSessionSupported));

            if (!string.Equals(configuration.FrontchannelLogoutSupported, frontchannelLogoutSupported))
                errors.Add(string.Format(CultureInfo.InvariantCulture, "configuration.FrontchannelLogoutSupported != efrontchannelLogoutSessionSupported. '{0}', '{1}'.", configuration.FrontchannelLogoutSupported, frontchannelLogoutSupported));

            if (!string.Equals(configuration.IntrospectionEndpoint, introspection_Endpoint))
                errors.Add(string.Format(CultureInfo.InvariantCulture, "configuration.IntrospectionEndpoint != introspection_Endpoint. '{0}', '{1}'.", configuration.IntrospectionEndpoint, introspection_Endpoint));

            if (!string.Equals(configuration.Issuer, issuer))
                errors.Add(string.Format(CultureInfo.InvariantCulture, "configuration.Issuer != issuer. '{0}', '{1}'.", configuration.Issuer, issuer));

            if (!string.Equals(configuration.JwksUri, jwks_Uri))
                errors.Add(string.Format(CultureInfo.InvariantCulture, "configuration.JwksUri != jwks_Uri. '{0}', '{1}'.", configuration.JwksUri, jwks_Uri));

            if (!string.Equals(configuration.TokenEndpoint, token_Endpoint))
                errors.Add(string.Format(CultureInfo.InvariantCulture, "configuration.TokenEndpoint != token_Endpoint. '{0}', '{1}'.", configuration.TokenEndpoint, token_Endpoint));

            CompareContext compareContext = new CompareContext();
            if (!IdentityComparer.AreEqual(configuration.SigningKeys, new Collection<SecurityKey>(securityKeys), compareContext))
                errors.AddRange(compareContext.Diffs);

            TestUtilities.AssertFailIfErrors("OpenIdConnectConfiguration_GetSets", errors);
        }

        [Fact]
        public void RoundTripFromJson()
        {
            var context = new CompareContext { Title = "RoundTripFromJson" };
            var oidcConfig1 = OpenIdConnectConfiguration.Create(OpenIdConfigData.JsonAllValues);
            var oidcConfig2 = new OpenIdConnectConfiguration(OpenIdConfigData.JsonAllValues);
            var oidcJson1 = OpenIdConnectConfiguration.Write(oidcConfig1);
            var oidcJson2 = OpenIdConnectConfiguration.Write(oidcConfig2);
            var oidcConfig3 = OpenIdConnectConfiguration.Create(oidcJson1);
            var oidcConfig4 = new OpenIdConnectConfiguration(oidcJson2);

            IdentityComparer.AreEqual(oidcConfig1, oidcConfig2, context);
            IdentityComparer.AreEqual(oidcConfig1, oidcConfig3, context);
            IdentityComparer.AreEqual(oidcConfig1, oidcConfig4, context);
            IdentityComparer.AreEqual(oidcJson1, oidcJson2, context);

            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void RoundTripFromJsonWithStream()
        {
            using MemoryStream stream = new();

            var context = new CompareContext { Title = "RoundTripFromJson" };
            var oidcConfig1 = OpenIdConnectConfiguration.Create(OpenIdConfigData.JsonAllValues);
            var oidcConfig2 = new OpenIdConnectConfiguration(OpenIdConfigData.JsonAllValues);

            OpenIdConnectConfiguration.Write(oidcConfig1, stream);
            var oidcJson1 = Encoding.UTF8.GetString(stream.ToArray());
            var oidcConfig3 = OpenIdConnectConfiguration.Create(oidcJson1);
            stream.SetLength(0);

            OpenIdConnectConfiguration.Write(oidcConfig2, stream);
            var oidcJson2 = Encoding.UTF8.GetString(stream.ToArray());
            var oidcConfig4 = new OpenIdConnectConfiguration(oidcJson2);

            IdentityComparer.AreEqual(oidcConfig1, oidcConfig2, context);
            IdentityComparer.AreEqual(oidcConfig1, oidcConfig3, context);
            IdentityComparer.AreEqual(oidcConfig1, oidcConfig4, context);
            IdentityComparer.AreEqual(oidcJson1, oidcJson2, context);

            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void EmptyCollectionSerialization()
        {
            var context = new CompareContext {Title = "EmptyCollectionSerialization"};
            // Initialize an OpenIdConnectConfiguration object with all collections empty.
            var oidcWithEmptyCollections = new OpenIdConnectConfiguration();
            var oidcWithEmptyCollectionsJson = OpenIdConnectConfiguration.Write(oidcWithEmptyCollections);

            IdentityComparer.AreEqual(oidcWithEmptyCollectionsJson, "{}", context);

            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void EmptyCollectionSerializationWithStream()
        {
            using MemoryStream stream = new();

            var context = new CompareContext {Title = "EmptyCollectionSerialization"};
            // Initialize an OpenIdConnectConfiguration object with all collections empty.
            var oidcWithEmptyCollections = new OpenIdConnectConfiguration();
            OpenIdConnectConfiguration.Write(oidcWithEmptyCollections, stream);
            var emptyCollectionBytes = Encoding.UTF8.GetBytes("{}");

            IdentityComparer.AreEqual(stream.ToArray(), emptyCollectionBytes, context);

            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void NonemptyCollectionSerialization()
        {
            var context = new CompareContext { Title = "NonemptyCollectionSerialization" };
            // Initialize an OpenIdConnectConfiguration object that has at least one element in each Collection.
            var oidcWithAllCollections = OpenIdConnectConfiguration.Create(OpenIdConfigData.JsonAllValues);
            var oidcWithAllCollectionsJson = OpenIdConnectConfiguration.Write(oidcWithAllCollections);
            // List of all collections that should be included in the serialized configuration.
            var collectionNames = new List<string>
            {
                "acr_values_supported",
                "authorization_details_types_supported",
                "authorization_encryption_alg_values_supported",
                "authorization_encryption_enc_values_supported",
                "authorization_signing_alg_values_supported",
                "backchannel_authentication_request_signing_alg_values_supported",
                "backchannel_token_delivery_modes_supported",
                "claims_supported",
                "claims_locales_supported",
                "claim_types_supported",
                "code_challenge_methods_supported",
                "device_authorization_endpoint",
                "display_values_supported",
                "dpop_signing_alg_values_supported",
                "grant_types_supported",
                "id_token_encryption_alg_values_supported",
                "id_token_encryption_enc_values_supported",
                "id_token_signing_alg_values_supported",
                "introspection_endpoint_auth_methods_supported",
                "introspection_endpoint_auth_signing_alg_values_supported",
                "prompt_values_supported",
                "request_object_encryption_alg_values_supported",
                "request_object_encryption_enc_values_supported",
                "request_object_signing_alg_values_supported",
                "response_modes_supported",
                "response_types_supported",
                "revocation_endpoint",
                "revocation_endpoint_auth_methods_supported",
                "revocation_endpoint_auth_signing_alg_values_supported",
                "scopes_supported",
                "subject_types_supported",
                "token_endpoint_auth_methods_supported",
                "token_endpoint_auth_signing_alg_values_supported",
                "ui_locales_supported",
                "userinfo_encryption_alg_values_supported",
                "userinfo_encryption_enc_values_supported",
                "userinfo_signing_alg_values_supported"
            };

            foreach (var collection in collectionNames)
            {
                if (!oidcWithAllCollectionsJson.Contains(collection))
                    context.Diffs.Add(collection + " should be serialized.");
            }
            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void NonemptyCollectionSerializationWithStream()
        {
            using MemoryStream stream = new();

            var context = new CompareContext { Title = "NonemptyCollectionSerialization" };
            // Initialize an OpenIdConnectConfiguration object that has at least one element in each Collection.
            var oidcWithAllCollections = OpenIdConnectConfiguration.Create(OpenIdConfigData.JsonAllValues);
            var oidcWithAllCollectionsJson = OpenIdConnectConfiguration.Write(oidcWithAllCollections);
            var oidcWithAllCollectionsBytes = Encoding.UTF8.GetBytes(oidcWithAllCollectionsJson);

            OpenIdConnectConfiguration.Write(oidcWithAllCollections, stream);

            IdentityComparer.AreBytesEqual(oidcWithAllCollectionsBytes, stream.GetBuffer(), context);
                
            TestUtilities.AssertFailIfErrors(context);
        }
    }
}
