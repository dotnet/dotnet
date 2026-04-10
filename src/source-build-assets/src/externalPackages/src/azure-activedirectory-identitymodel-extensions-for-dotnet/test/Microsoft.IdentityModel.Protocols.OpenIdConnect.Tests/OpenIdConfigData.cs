// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Json;
using Microsoft.IdentityModel.Tokens.Json.Tests;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect.Tests
{
    /// <summary>
    /// This configuration data is used to test that loading metadata from a json string has expected values.
    /// jsonstrings are defined and objects are created then tests use them to ensure they are the same.
    /// </summary>
    public class OpenIdConfigData
    {
        public static OpenIdConnectConfiguration FullyPopulated = new OpenIdConnectConfiguration();

        public static OpenIdConnectConfiguration FullyPopulatedWithKeys
        {
            get
            {
                var config = Default;
                config.JsonWebKeySet = DataSets.JsonWebKeySet1;
                config.AdditionalData["microsoft_multi_refresh_token"] = true;
                config.SigningKeys.Add(KeyingMaterial.RsaSecurityKey1);
                config.SigningKeys.Add(KeyingMaterial.RsaSecurityKey2);
                config.SigningKeys.Add(KeyingMaterial.X509SecurityKey1);
                config.SigningKeys.Add(KeyingMaterial.X509SecurityKey2);
                return config;
            }
        }

        public static OpenIdConnectConfiguration PingLabs = new OpenIdConnectConfiguration();
        public static OpenIdConnectConfiguration SingleX509Data = new OpenIdConnectConfiguration();
        public static string AADCommonUrl = "https://login.windows.net/common/.well-known/openid-configuration";
        public static string AADCommonUrlV1 = "https://login.microsoftonline.com/common/.well-known/openid-configuration";
        public static string AADCommonUrlV2 = "https://login.microsoftonline.com/common/v2.0/.well-known/openid-configuration";
        public static string AccountsGoogle = "https://accounts.google.com/.well-known/openid-configuration";
        public static string BadUri = "_____NoSuchfile____";
        public static string HttpsBadUri = "https://_____NoSuchfile____";

        #region Configuration Strings
        public static string OpenIdConnectMetadataPingString = @"{""authorization_endpoint"": ""https:\/\/connect-interop.pinglabs.org:9031\/as\/authorization.oauth2"",
                                                                  ""issuer"": ""https:\/\/connect-interop.pinglabs.org:9031"",
                                                                  ""id_token_signing_alg_values_supported"": [""none"", ""HS256"", ""HS384"", ""HS512"", ""RS256"", ""RS384"", ""RS512"", ""ES256"", ""ES384"", ""ES512""],
                                                                  ""claim_types_supported"": [""normal""],
                                                                  ""claims_parameter_supported"": false,
                                                                  ""ping_end_session_endpoint"": ""https:\/\/connect-interop.pinglabs.org:9031\/idp\/startSLO.ping"",
                                                                  ""ping_revoked_sris_endpoint"": ""https:\/\/connect-interop.pinglabs.org:9031\/pf-ws\/rest\/sessionMgmt\/revokedSris"",
                                                                  ""request_parameter_supported"": false,
                                                                  ""request_uri_parameter_supported"": false,
                                                                  ""response_modes_supported"": [""fragment"", ""query"", ""form_post""],
                                                                  ""response_types_supported"": [""code"", ""token"", ""id_token"", ""code token"", ""code id_token"", ""token id_token"", ""code token id_token""],
                                                                  ""revocation_endpoint"": ""https:\/\/connect-interop.pinglabs.org:9031\/as\/revoke_token.oauth2"",
                                                                  ""scopes_supported"": [""phone"", ""address"", ""email"", ""openid"", ""profile""],
                                                                  ""subject_types_supported"": [""public""],
                                                                  ""token_endpoint"": ""https:\/\/connect-interop.pinglabs.org:9031\/as\/token.oauth2"",
                                                                  ""token_endpoint_auth_methods_supported"": [""client_secret_basic"", ""client_secret_post""],
                                                                  ""userinfo_endpoint"": ""https:\/\/connect-interop.pinglabs.org:9031\/idp\/userinfo.openid"",
                                                                  ""version"": ""3.0""}";

        public static string JsonFile = @"OpenIdConnectMetadata.json";
        public static string OpenIdConnectMetadataFileEnd2End = @"OpenIdConnectMetadataEnd2End.json";
        public static string OpenIdConnectMetadataFileEnd2EndEC = @"OpenIdConnectMetadataEnd2EndEC.json";
        public static string JsonWebKeySetBadUriFile = @"OpenIdConnectMetadataJsonWebKeySetBadUri.json";
        public static string JsonAllValues =
                                            @"{ ""acr_values_supported"": [""acr_value1"", ""acr_value2"", ""acr_value3""],
                                                ""authorization_details_types_supported"": [""payment_initiation"", ""account_information""],
                                                ""authorization_endpoint"": ""https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/oauth2/authorize"",
                                                ""authorization_encryption_alg_values_supported"": [""A192KW"", ""A256KW""],
                                                ""authorization_encryption_enc_values_supported"": [""A128CBC-HS256"", ""A256CBC-HS512""],
                                                ""authorization_response_iss_parameter_supported"": false,
                                                ""authorization_signing_alg_values_supported"": [""ES384"", ""ES512""],
                                                ""backchannel_authentication_endpoint"": ""https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/oauth2/bc-authorize"",
                                                ""backchannel_authentication_request_signing_alg_values_supported"": [""ES384"", ""ES512""],
                                                ""backchannel_token_delivery_modes_supported"": [""poll"", ""ping""],
                                                ""backchannel_user_code_parameter_supported"": false,
                                                ""code_challenge_methods_supported"": [""plain"", ""S256""],
                                                ""device_authorization_endpoint"": ""https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/oauth2/devicecode"",
                                                ""frontchannel_logout_session_supported"": ""true"",
                                                ""frontchannel_logout_supported"": ""true"",
                                                ""check_session_iframe"": ""https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/oauth2/checksession"",
                                                ""claims_locales_supported"": [""claim_local1"", ""claim_local2"", ""claim_local3"" ],
                                                ""claims_parameter_supported"": true,
                                                ""claims_supported"": [""sub"", ""iss"", ""aud"", ""exp"", ""iat"", ""auth_time"", ""acr"", ""amr"", ""nonce"", ""email"", ""given_name"", ""family_name"", ""nickname"" ],
                                                ""claim_types_supported"": [""Normal Claims"", ""Aggregated Claims"", ""Distributed Claims"" ],
                                                ""display_values_supported"": [""displayValue1"", ""displayValue2"", ""displayValue3"" ],
                                                ""dpop_signing_alg_values_supported"": [""ES384"", ""ES512""],
                                                ""end_session_endpoint"": ""https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/oauth2/logout"",
                                                ""grant_types_supported"": [""authorization_code"", ""implicit""],
                                                ""http_logout_supported"": true,
                                                ""id_token_encryption_alg_values_supported"": [""RSA1_5"", ""A256KW""],
                                                ""id_token_encryption_enc_values_supported"": [""A128CBC-HS256"", ""A256CBC-HS512""],
                                                ""id_token_signing_alg_values_supported"": [""RS256""],
                                                ""introspection_endpoint"": ""https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/oauth2/introspect"",
                                                ""introspection_endpoint_auth_methods_supported"": [""client_secret_post"", ""private_key_jwt""],
                                                ""introspection_endpoint_auth_signing_alg_values_supported"": [""ES192"", ""ES256""],
                                                ""issuer"": ""https://sts.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/"",
                                                ""jwks_uri"": ""JsonWebKeySet.json"",
                                                ""logout_session_supported"": true,
                                                ""microsoft_multi_refresh_token"": true,
                                                ""op_policy_uri"": ""https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/op_policy_uri"",
                                                ""op_tos_uri"": ""https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/op_tos_uri"",
                                                ""prompt_values_supported"": [""none"", ""login"", ""consent""],
                                                ""pushed_authorization_request_endpoint"": ""https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/oauth2/par"",
                                                ""request_object_encryption_alg_values_supported"": [""A192KW"", ""A256KW""],
                                                ""request_object_encryption_enc_values_supported"": [""A192GCM"", ""A256GCM""],
                                                ""request_object_signing_alg_values_supported"": [""PS256"", ""PS512""],
                                                ""request_parameter_supported"": true,
                                                ""request_uri_parameter_supported"": true,
                                                ""require_pushed_authorization_requests"": false,
                                                ""require_request_uri_registration"": true,
                                                ""response_modes_supported"": [""query"", ""fragment"", ""form_post""],
                                                ""response_types_supported"": [""code"", ""id_token"", ""code id_token""],
                                                ""revocation_endpoint"": ""https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/oauth2/revocation"",
                                                ""revocation_endpoint_auth_methods_supported"": [""client_secret_post"", ""client_secret_basic""],
                                                ""revocation_endpoint_auth_signing_alg_values_supported"": [""ES192"", ""ES256""],
                                                ""service_documentation"": ""https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/service_documentation"",
                                                ""scopes_supported"": [""openid""],
                                                ""subject_types_supported"": [""pairwise""],
                                                ""token_endpoint"": ""https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/oauth2/token"",
                                                ""token_endpoint_auth_methods_supported"": [""client_secret_post"", ""private_key_jwt""],
                                                ""token_endpoint_auth_signing_alg_values_supported"": [""ES192"", ""ES256""],
                                                ""tls_client_certificate_bound_access_tokens"": true,
                                                ""ui_locales_supported"": [""hak-CN"", ""en-us""],
                                                ""userinfo_endpoint"": ""https://login.microsoftonline.com/add29489-7269-41f4-8841-b63c95564420/openid/userinfo"",
                                                ""userinfo_encryption_alg_values_supported"": [""ECDH-ES+A128KW"", ""ECDH-ES+A192KW""],
                                                ""userinfo_encryption_enc_values_supported"": [""A256CBC-HS512"", ""A128CBC-HS256""],
                                                ""userinfo_signing_alg_values_supported"": [""ES384"", ""ES512""]
                                            }";

        public static string OpenIdConnectMetadataSingleX509DataString =
                                            @"{ ""authorization_endpoint"": ""https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/oauth2/authorize"",
                                                ""check_session_iframe"": ""https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/oauth2/checksession"",
                                                ""end_session_endpoint"": ""https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/oauth2/logout"",
                                                ""id_token_signing_alg_values_supported"": [""RS256""],
                                                ""issuer"": ""https://sts.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/"",
                                                ""jwks_uri"": ""JsonWebKeySetSingleX509Data.json"",
                                                ""microsoft_multi_refresh_token"":true,
                                                ""response_types_supported"": [""code"", ""id_token"", ""code id_token""],
                                                ""response_modes_supported"": [""query"", ""fragment"", ""form_post""],
                                                ""scopes_supported"": [""openid""],
                                                ""subject_types_supported"": [""pairwise""],
                                                ""token_endpoint"": ""https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/oauth2/token"",
                                                ""token_endpoint_auth_methods_supported"": [""client_secret_post"", ""private_key_jwt""]
                                            }";

        public static string JsonWithSigningKeys =
                                            @"{ ""authorization_endpoint"": ""https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/oauth2/authorize"",
                                                ""check_session_iframe"": ""https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/oauth2/checksession"",
                                                ""end_session_endpoint"": ""https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/oauth2/logout"",
                                                ""id_token_signing_alg_values_supported"": [""RS256""],
                                                ""issuer"": ""https://sts.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/"",
                                                ""jwks_uri"": ""JsonWebKeySetSingleX509Data.json"",
                                                ""microsoft_multi_refresh_token"":true,
                                                ""response_types_supported"": [""code"", ""id_token"", ""code id_token""],
                                                ""response_modes_supported"": [""query"", ""fragment"", ""form_post""],
                                                ""scopes_supported"": [""openid""],
                                                ""subject_types_supported"": [""pairwise""],
                                                ""token_endpoint"": ""https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/oauth2/token"",
                                                ""token_endpoint_auth_methods_supported"": [""client_secret_post"", ""private_key_jwt""],
                                                ""SigningKeys"": [""key1"", ""key2""]
                    }";

        public static string OpenIdConnectMetadataBadX509DataString = @"{""jwks_uri"": ""JsonWebKeySetBadX509Data.json""}";
        public static string OpenIdConnectMetadataBadBase64DataString = @"{""jwks_uri"": ""JsonWebKeySetBadBase64Data.json""}";
        public static string OpenIdConnectMetadataBadUriKeysString = @"{""jwks_uri"": ""___NoSuchFile___""}";
        public static string OpenIdConnectMetadataBadFormatString = @"{""issuer""::""https://sts.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/""}";
        public static string OpenIdConnectMetadataPingLabsJWKSString = @"{""jwks_uri"": ""PingLabsJWKS.json""}";
        public static string OpenIdConnectMetatadataBadJson = @"{...";
        #endregion

        #region WellKnownConfigurationStrings
        public static string Authority => "https://idp.com";

        public static string Issuer => Authority;

        public static string IssuerClaim =>
            $"""
            "{OpenIdProviderMetadataNames.Issuer}":"{Issuer}"
            """;

        public static string JksUri => Authority + "/jwks";

        public static string JksUriClaim =>
            $"""
            "{OpenIdProviderMetadataNames.JwksUri}":"{JksUri}"
            """;
        #endregion

        #region GOOGLE 2/2/2024 https://accounts.google.com/.well-known/openid-configuration
        public static string AccountsGoogleComJson =>
                $$"""
                {
                "issuer": "https://accounts.google.com",
                "authorization_endpoint": "https://accounts.google.com/o/oauth2/v2/auth",
                "device_authorization_endpoint": "https://oauth2.googleapis.com/device/code",
                "token_endpoint": "https://oauth2.googleapis.com/token",
                "userinfo_endpoint": "https://openidconnect.googleapis.com/v1/userinfo",
                "revocation_endpoint": "https://oauth2.googleapis.com/revoke",
                "jwks_uri": "https://www.googleapis.com/oauth2/v3/certs",
                "response_types_supported": ["code", "id_token", "code id_token"],
                "subject_types_supported": ["public"],
                "id_token_signing_alg_values_supported": ["RS256"],
                "scopes_supported": ["openid", "email", "profile"],
                "token_endpoint_auth_methods_supported": ["client_secret_post", "client_secret_basic"],
                "claims_supported": ["aud", "email", "email_verified", "exp", "family_name", "given_name", "iat", "iss", "locale", "name", "picture", "sub"],
                "code_challenge_methods_supported": ["plain", "S256"],
                "grant_types_supported": ["authorization_code", "refresh_token", "urn:ietf:params:oauth:grant-type:device_code", "urn:ietf:params:oauth:grant-type:jwt-bearer"]
                }
                """;
        public static OpenIdConnectConfiguration AccountsGoogleComConfig
        {
            get
            {
                // AccountsGoogleComConfig
                OpenIdConnectConfiguration config = new OpenIdConnectConfiguration
                {
                    AuthorizationEndpoint = "https://accounts.google.com/o/oauth2/v2/auth",
                    DeviceAuthorizationEndpoint = "https://oauth2.googleapis.com/device/code",
                    Issuer = "https://accounts.google.com",
                    JwksUri = "https://www.googleapis.com/oauth2/v3/certs",
                    RevocationEndpoint = "https://oauth2.googleapis.com/revoke",
                    TokenEndpoint = "https://oauth2.googleapis.com/token",
                    UserInfoEndpoint = "https://openidconnect.googleapis.com/v1/userinfo",
                };

                AddToCollection(config.CodeChallengeMethodsSupported, "plain",  "S256");
                AddToCollection(config.ResponseTypesSupported, "code", "id_token", "code id_token");
                config.SubjectTypesSupported.Add("public");
                config.IdTokenSigningAlgValuesSupported.Add("RS256");
                AddToCollection(config.ScopesSupported, "openid", "email", "profile");
                AddToCollection(config.TokenEndpointAuthMethodsSupported, "client_secret_post", "client_secret_basic");
                AddToCollection(config.ClaimsSupported, "aud", "email", "email_verified", "exp", "family_name", "given_name", "iat", "iss", "locale", "name", "picture", "sub");
                AddToCollection(config.GrantTypesSupported, "authorization_code", "refresh_token", "urn:ietf:params:oauth:grant-type:device_code", "urn:ietf:params:oauth:grant-type:jwt-bearer");

                return config;
            }
        }
        #endregion

        #region AADCommonV1 2/2/2024 https://login.microsoftonline.com/common/.well-known/openid-configuration
        public static string AADCommonV1Json =>
                """
                {
                "token_endpoint": "https://login.microsoftonline.com/common/oauth2/token",
                "token_endpoint_auth_methods_supported": ["client_secret_post", "private_key_jwt", "client_secret_basic"],
                "jwks_uri": "https://login.microsoftonline.com/common/discovery/keys",
                "response_modes_supported": ["query", "fragment", "form_post"],
                "subject_types_supported": ["pairwise"],
                "id_token_signing_alg_values_supported": ["RS256"],
                "response_types_supported": ["code", "id_token", "code id_token", "token id_token", "token"],
                "scopes_supported": ["openid"],
                "issuer": "https://sts.windows.net/{tenantid}/",
                "microsoft_multi_refresh_token": true,
                "authorization_endpoint": "https://login.microsoftonline.com/common/oauth2/authorize",
                "device_authorization_endpoint": "https://login.microsoftonline.com/common/oauth2/devicecode",
                "http_logout_supported": true,
                "frontchannel_logout_supported": true,
                "end_session_endpoint": "https://login.microsoftonline.com/common/oauth2/logout",
                "claims_supported": ["sub", "iss", "cloud_instance_name", "cloud_instance_host_name", "cloud_graph_host_name", "msgraph_host", "aud", "exp", "iat", "auth_time", "acr", "amr", "nonce", "email", "given_name", "family_name", "nickname"],
                "check_session_iframe": "https://login.microsoftonline.com/common/oauth2/checksession",
                "userinfo_endpoint": "https://login.microsoftonline.com/common/openid/userinfo",
                "kerberos_endpoint": "https://login.microsoftonline.com/common/kerberos",
                "tenant_region_scope": null,
                "cloud_instance_name": "microsoftonline.com",
                "cloud_graph_host_name": "graph.windows.net",
                "msgraph_host": "graph.microsoft.com",
                "rbac_url": "https://pas.windows.net"
                }
                """;

        public static OpenIdConnectConfiguration AADCommonV1Config
        {
            get
            {
                OpenIdConnectConfiguration config = new OpenIdConnectConfiguration
                {
                    AuthorizationEndpoint = "https://login.microsoftonline.com/common/oauth2/authorize",
                    DeviceAuthorizationEndpoint = "https://login.microsoftonline.com/common/oauth2/devicecode",
                    CheckSessionIframe = "https://login.microsoftonline.com/common/oauth2/checksession",
                    HttpLogoutSupported = true,
                    Issuer = "https://sts.windows.net/{tenantid}/",
                    JwksUri = "https://login.microsoftonline.com/common/discovery/keys",
                    TokenEndpoint = "https://login.microsoftonline.com/common/oauth2/token",
                    UserInfoEndpoint = "https://login.microsoftonline.com/common/openid/userinfo",
                    EndSessionEndpoint = "https://login.microsoftonline.com/common/oauth2/logout",
                    FrontchannelLogoutSupported = JsonSerializerPrimitives.True,
                };

                AddToCollection(config.ResponseModesSupported, "query", "fragment", "form_post");
                AddToCollection(config.ResponseTypesSupported, "code", "id_token", "code id_token", "token id_token", "token");
                AddToCollection(config.SubjectTypesSupported, "pairwise");
                AddToCollection(config.IdTokenSigningAlgValuesSupported, "RS256");
                AddToCollection(config.ScopesSupported, "openid");
                AddToCollection(config.TokenEndpointAuthMethodsSupported, "client_secret_post", "private_key_jwt", "client_secret_basic");
                AddToCollection(config.ClaimsSupported, "sub", "iss", "cloud_instance_name", "cloud_instance_host_name", "cloud_graph_host_name", "msgraph_host", "aud", "exp", "iat", "auth_time", "acr", "amr", "nonce", "email", "given_name", "family_name", "nickname");
                config.AdditionalData.Add("microsoft_multi_refresh_token", true);
                config.AdditionalData.Add("kerberos_endpoint", "https://login.microsoftonline.com/common/kerberos");
                config.AdditionalData.Add("tenant_region_scope", null);
                config.AdditionalData.Add("cloud_instance_name", "microsoftonline.com");
                config.AdditionalData.Add("cloud_graph_host_name", "graph.windows.net");
                config.AdditionalData.Add("msgraph_host", "graph.microsoft.com");
                config.AdditionalData.Add("rbac_url", "https://pas.windows.net");

                return config;
            }
        }
        #endregion

        #region AADCommonV2 2/2/2024 https://login.microsoftonline.com/common/v2.0/.well-known/openid-configuration
        public static string AADCommonV2Json =>
            """
            {
            "token_endpoint": "https://login.microsoftonline.com/common/oauth2/v2.0/token",
            "token_endpoint_auth_methods_supported": ["client_secret_post", "private_key_jwt", "client_secret_basic"],
            "jwks_uri": "https://login.microsoftonline.com/common/discovery/v2.0/keys",
            "response_modes_supported": ["query", "fragment", "form_post"],
            "subject_types_supported": ["pairwise"],
            "id_token_signing_alg_values_supported": ["RS256"],
            "response_types_supported": ["code", "id_token", "code id_token", "id_token token"],
            "scopes_supported": ["openid", "profile", "email", "offline_access"],
            "issuer": "https://login.microsoftonline.com/{tenantid}/v2.0",
            "request_uri_parameter_supported": false,
            "userinfo_endpoint": "https://graph.microsoft.com/oidc/userinfo",
            "authorization_endpoint": "https://login.microsoftonline.com/common/oauth2/v2.0/authorize",
            "device_authorization_endpoint": "https://login.microsoftonline.com/common/oauth2/v2.0/devicecode",
            "http_logout_supported": true,
            "frontchannel_logout_supported": true,
            "end_session_endpoint": "https://login.microsoftonline.com/common/oauth2/v2.0/logout",
            "claims_supported": ["sub", "iss", "cloud_instance_name", "cloud_instance_host_name", "cloud_graph_host_name", "msgraph_host", "aud", "exp", "iat", "auth_time", "acr", "nonce", "preferred_username", "name", "tid", "ver", "at_hash", "c_hash", "email"],
            "kerberos_endpoint": "https://login.microsoftonline.com/common/kerberos",
            "tenant_region_scope": null,
            "cloud_instance_name": "microsoftonline.com",
            "cloud_graph_host_name": "graph.windows.net",
            "msgraph_host": "graph.microsoft.com",
            "rbac_url": "https://pas.windows.net"
            }
            """;

        public static OpenIdConnectConfiguration AADCommonV2Config
        {
            get
            {
                OpenIdConnectConfiguration config = new OpenIdConnectConfiguration
                {
                    AuthorizationEndpoint = "https://login.microsoftonline.com/common/oauth2/v2.0/authorize",
                    DeviceAuthorizationEndpoint = "https://login.microsoftonline.com/common/oauth2/v2.0/devicecode",
                    HttpLogoutSupported = true,
                    Issuer = "https://login.microsoftonline.com/{tenantid}/v2.0",
                    JwksUri = "https://login.microsoftonline.com/common/discovery/v2.0/keys",
                    TokenEndpoint = "https://login.microsoftonline.com/common/oauth2/v2.0/token",
                    UserInfoEndpoint = "https://graph.microsoft.com/oidc/userinfo",
                    EndSessionEndpoint = "https://login.microsoftonline.com/common/oauth2/v2.0/logout",
                    FrontchannelLogoutSupported = JsonSerializerPrimitives.True,
                };

                AddToCollection(config.ResponseModesSupported, "query", "fragment", "form_post");
                AddToCollection(config.ResponseTypesSupported, "code", "id_token", "code id_token", "id_token token");
                AddToCollection(config.SubjectTypesSupported, "pairwise");
                AddToCollection(config.IdTokenSigningAlgValuesSupported, "RS256");
                AddToCollection(config.ScopesSupported, "openid", "profile", "email", "offline_access");
                AddToCollection(config.TokenEndpointAuthMethodsSupported, "client_secret_post", "private_key_jwt", "client_secret_basic");
                AddToCollection(config.ClaimsSupported, "sub", "iss", "cloud_instance_name", "cloud_instance_host_name", "cloud_graph_host_name", "msgraph_host", "aud", "exp", "iat", "auth_time", "acr", "nonce", "preferred_username", "name", "tid", "ver", "at_hash", "c_hash", "email");
                config.AdditionalData.Add("kerberos_endpoint", "https://login.microsoftonline.com/common/kerberos");
                config.AdditionalData.Add("tenant_region_scope", null);
                config.AdditionalData.Add("cloud_instance_name", "microsoftonline.com");
                config.AdditionalData.Add("cloud_graph_host_name", "graph.windows.net");
                config.AdditionalData.Add("msgraph_host", "graph.microsoft.com");
                config.AdditionalData.Add("rbac_url", "https://pas.windows.net");

                return config;
            }
        }
        #endregion

        #region Array
        public static string ArrayFirstObject =>
            $$"""
            {{{JsonData.ArrayClaim}},{{JksUriClaim}},{{IssuerClaim}}}
            """;
        public static string ArrayMiddleObject =>
            $$"""
            {{{JksUriClaim}},{{JsonData.ArrayClaim}},{{IssuerClaim}}}
            """;

        public static string ArrayLastObject =>
            $$"""
            {{{JksUriClaim}},{{IssuerClaim}},{{JsonData.ArrayClaim}}}
            """;

        public static OpenIdConnectConfiguration ArraysConfig
        {
            get
            {
                OpenIdConnectConfiguration config = new OpenIdConnectConfiguration
                {
                    Issuer = Issuer,
                    JwksUri = JksUri
                };

                config.AdditionalData.Add(JsonData.ArrayProperty, JsonUtilities.CreateJsonElement(JsonData.ArrayValue));
                return config;
            }
        }
        #endregion

        #region Object
        public static string ObjectFirstObject =>
            $$"""
            {{{JsonData.ObjectClaim}},{{IssuerClaim}},{{JksUriClaim}}}
            """;

        public static string ObjectMiddleObject =>
            $$"""
            {{{IssuerClaim}},{{JsonData.ObjectClaim}},{{JksUriClaim}}}
            """;

        public static string ObjectLastObject =>
            $$"""
            {{{IssuerClaim}},{{JksUriClaim}},{{JsonData.ObjectClaim}}}
            """;

        public static OpenIdConnectConfiguration ObjectConfig
        {
            get
            {
                OpenIdConnectConfiguration config = new OpenIdConnectConfiguration
                {
                    JwksUri = JksUri,
                    Issuer = Issuer
                };

                config.AdditionalData.Add(JsonData.ObjectProperty, JsonUtilities.CreateJsonElement(JsonData.ObjectValue));

                return config;
            }
        }
        #endregion

        #region Duplicates
        public static string Duplicates =>
            $$"""
            {
                "Request_parameter_supported": true,
                "claims_parameter_supported": true,
                "claims_parameter_Supported": false,
                "request_parameter_supported": false,
                {{IssuerClaim }},
                {{JsonData.ObjectClaim}},
                {{JksUriClaim}},
                {{JsonData.ArrayClaim}},
                {{JsonData.ObjectClaim}},
                {{IssuerClaim}},
                {{JsonData.ArrayClaim}},
                {{JksUriClaim}},
                {{JsonData.ObjectClaim}}
            }
            """;

        public static OpenIdConnectConfiguration DuplicatesConfig
        {             get
            {
                OpenIdConnectConfiguration config = new OpenIdConnectConfiguration
                {
                    ClaimsParameterSupported = false,
                    Issuer = Issuer,
                    JwksUri = JksUri,
                    RequestParameterSupported = false,
                };

                config.AdditionalData.Add(JsonData.ObjectProperty, JsonUtilities.CreateJsonElement(JsonData.ObjectValue));
                config.AdditionalData.Add(JsonData.ArrayProperty, JsonUtilities.CreateJsonElement(JsonData.ArrayValue));

                return config;
            }
        }
        #endregion

        #region FrontChannel one off tests
        // FrontChannelFalse, used for testing that the json is case insensitive.
        public static string FrontChannelTrue =>
            """
            {
                "frontchannel_logout_session_supported": "true",
                "frontchannel_logout_supported": "false"
            }
            """;

        public static OpenIdConnectConfiguration FrontChannelTrueConfig
        {
            get
            {
                OpenIdConnectConfiguration config = new OpenIdConnectConfiguration
                {
                    FrontchannelLogoutSessionSupported = JsonSerializerPrimitives.True,
                    FrontchannelLogoutSupported = JsonSerializerPrimitives.False
                };

                return config;
            }
        }

        public static string FrontChannelFalse =>
            """
            {
                "frontchannel_logout_session_supported": "false",
                "frontchannel_logout_supported": "true"
            }
            """;

        public static OpenIdConnectConfiguration FrontChannelFalseConfig
        {
            get
            {
                OpenIdConnectConfiguration config = new OpenIdConnectConfiguration
                {
                    FrontchannelLogoutSessionSupported = JsonSerializerPrimitives.False,
                    FrontchannelLogoutSupported = JsonSerializerPrimitives.True
                };

                return config;
            }
        }
        #endregion

        #region Singleton Objects for AdditionalData

        public static OpenIdConnectConfiguration StringConfig
        {
            get
            {
                OpenIdConnectConfiguration config = new OpenIdConnectConfiguration();
                config.AdditionalData.Add(JsonData.StringProperty, JsonData.StringValue);
                return config;
            }
        }

        public static OpenIdConnectConfiguration BoolFalseConfig
        {
            get
            {
                OpenIdConnectConfiguration config = new OpenIdConnectConfiguration();
                config.AdditionalData.Add(JsonData.FalseProperty, false);
                return config;
            }
        }

        public static OpenIdConnectConfiguration BoolTrueConfig
        {
            get
            {
                OpenIdConnectConfiguration config = new OpenIdConnectConfiguration();
                config.AdditionalData.Add(JsonData.TrueProperty, true);
                return config;
            }
        }

        public static OpenIdConnectConfiguration NullConfig
        {
            get
            {
                OpenIdConnectConfiguration config = new OpenIdConnectConfiguration();
                config.AdditionalData.Add(JsonData.NullProperty, null);
                return config;
            }
        }
        #endregion

        static OpenIdConfigData()
        {
            PingLabs = new OpenIdConnectConfiguration()
            {
                JwksUri = "PingLabsJWKS.json",
                JsonWebKeySet = new JsonWebKeySet()
            };

            PingLabs.SigningKeys.Add(KeyingMaterial.RsaSecurityKeyFromPing1);
            PingLabs.SigningKeys.Add(KeyingMaterial.RsaSecurityKeyFromPing2);
            PingLabs.SigningKeys.Add(KeyingMaterial.RsaSecurityKeyFromPing3);
            PingLabs.JsonWebKeySet.Keys.Add(DataSets.JsonWebKeyFromPing1);
            PingLabs.JsonWebKeySet.Keys.Add(DataSets.JsonWebKeyFromPing2);
            PingLabs.JsonWebKeySet.Keys.Add(DataSets.JsonWebKeyFromPing3);

            // matches with OpenIdConnectMetadataString
            SetDefaultConfiguration(FullyPopulated);
            FullyPopulated.AdditionalData["microsoft_multi_refresh_token"] = true;

            // Config with X509Data
            SingleX509Data.AdditionalData["microsoft_multi_refresh_token"] = true;
            SingleX509Data.AuthorizationEndpoint = "https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/oauth2/authorize";
            SingleX509Data.CheckSessionIframe = "https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/oauth2/checksession";
            SingleX509Data.EndSessionEndpoint = "https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/oauth2/logout";
            SingleX509Data.Issuer = "https://sts.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/";
            SingleX509Data.JsonWebKeySet = DataSets.JsonWebKeySetX509Data;
            SingleX509Data.JwksUri = "JsonWebKeySetSingleX509Data.json";
            SingleX509Data.IdTokenSigningAlgValuesSupported.Add("RS256");
            AddToCollection(SingleX509Data.ResponseTypesSupported, "code", "id_token", "code id_token");
            AddToCollection(SingleX509Data.ResponseModesSupported, "query", "fragment", "form_post");
            SingleX509Data.ScopesSupported.Add("openid");
            SingleX509Data.SigningKeys.Add(KeyingMaterial.X509SecurityKey1);
            SingleX509Data.SubjectTypesSupported.Add("pairwise");
            SingleX509Data.TokenEndpoint = "https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/oauth2/token";
            AddToCollection(SingleX509Data.TokenEndpointAuthMethodsSupported, new string[] { "client_secret_post", "private_key_jwt" });
        }

        public static OpenIdConnectConfiguration Default
        {
            get => SetDefaultConfiguration(new OpenIdConnectConfiguration());
        }

        private static OpenIdConnectConfiguration SetDefaultConfiguration(OpenIdConnectConfiguration config)
        {
            AddToCollection(config.AcrValuesSupported, "acr_value1", "acr_value2", "acr_value3");
            AddToCollection(config.AuthorizationDetailsTypesSupported, "payment_initiation", "account_information");
            config.AuthorizationEndpoint = "https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/oauth2/authorize";
            AddToCollection(config.AuthorizationEncryptionAlgValuesSupported, "A192KW", "A256KW");
            AddToCollection(config.AuthorizationEncryptionEncValuesSupported, "A128CBC-HS256", "A256CBC-HS512");
            config.AuthorizationResponseIssParameterSupported = false;
            AddToCollection(config.AuthorizationSigningAlgValuesSupported, "ES384", "ES512");
            config.BackchannelAuthenticationEndpoint = "https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/oauth2/bc-authorize";
            AddToCollection(config.BackchannelAuthenticationRequestSigningAlgValuesSupported, "ES384", "ES512");
            AddToCollection(config.BackchannelTokenDeliveryModesSupported, "poll", "ping");
            config.BackchannelUserCodeParameterSupported = false;
            config.CheckSessionIframe = "https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/oauth2/checksession";
            AddToCollection(config.ClaimsLocalesSupported, "claim_local1", "claim_local2", "claim_local3");
            config.ClaimsParameterSupported = true;
            AddToCollection(config.ClaimsSupported, "sub", "iss", "aud", "exp", "iat", "auth_time", "acr", "amr", "nonce", "email", "given_name", "family_name", "nickname");
            AddToCollection(config.ClaimTypesSupported, "Normal Claims", "Aggregated Claims", "Distributed Claims");
            AddToCollection(config.CodeChallengeMethodsSupported, "plain", "S256");
            config.DeviceAuthorizationEndpoint = "https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/oauth2/devicecode";
            AddToCollection(config.DisplayValuesSupported, "displayValue1", "displayValue2", "displayValue3");
            AddToCollection(config.DPoPSigningAlgValuesSupported, "ES384", "ES512");
            config.EndSessionEndpoint = "https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/oauth2/logout";
            config.FrontchannelLogoutSessionSupported = "true";
            config.FrontchannelLogoutSupported = "true";
            AddToCollection(config.GrantTypesSupported, "authorization_code", "implicit");
            config.HttpLogoutSupported = true;
            AddToCollection(config.IdTokenEncryptionAlgValuesSupported, "RSA1_5", "A256KW");
            AddToCollection(config.IdTokenEncryptionEncValuesSupported, "A128CBC-HS256", "A256CBC-HS512");
            AddToCollection(config.IdTokenSigningAlgValuesSupported, "RS256");
            config.IntrospectionEndpoint = "https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/oauth2/introspect";
            AddToCollection(config.IntrospectionEndpointAuthMethodsSupported, "client_secret_post", "private_key_jwt");
            AddToCollection(config.IntrospectionEndpointAuthSigningAlgValuesSupported, "ES192", "ES256");
            config.Issuer = "https://sts.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/";
            config.JwksUri = "JsonWebKeySet.json";
            config.LogoutSessionSupported = true;
            config.OpPolicyUri = "https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/op_policy_uri";
            config.OpTosUri = "https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/op_tos_uri";
            AddToCollection(config.PromptValuesSupported, "none", "login", "consent");
            config.PushedAuthorizationRequestEndpoint = "https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/oauth2/par";
            AddToCollection(config.RequestObjectEncryptionAlgValuesSupported, "A192KW", "A256KW");
            AddToCollection(config.RequestObjectEncryptionEncValuesSupported, "A192GCM", "A256GCM");
            AddToCollection(config.RequestObjectSigningAlgValuesSupported, "PS256", "PS512");
            config.RequestParameterSupported = true;
            config.RequestUriParameterSupported = true;
            config.RequirePushedAuthorizationRequests = false;
            config.RequireRequestUriRegistration = true;
            AddToCollection(config.ResponseModesSupported, "query", "fragment", "form_post");
            AddToCollection(config.ResponseTypesSupported, "code", "id_token", "code id_token");
            config.RevocationEndpoint = "https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/oauth2/revocation";
            AddToCollection(config.RevocationEndpointAuthMethodsSupported, "client_secret_post", "client_secret_basic");
            AddToCollection(config.RevocationEndpointAuthSigningAlgValuesSupported, "ES192", "ES256");
            config.ServiceDocumentation = "https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/service_documentation";
            config.ScopesSupported.Add("openid");
            config.SubjectTypesSupported.Add("pairwise");
            config.TokenEndpoint = "https://login.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/oauth2/token";
            AddToCollection(config.TokenEndpointAuthMethodsSupported, "client_secret_post", "private_key_jwt");
            AddToCollection(config.TokenEndpointAuthSigningAlgValuesSupported, "ES192", "ES256");
            config.TlsClientCertificateBoundAccessTokens = true;
            AddToCollection(config.UILocalesSupported, "hak-CN", "en-us");
            config.UserInfoEndpoint = "https://login.microsoftonline.com/add29489-7269-41f4-8841-b63c95564420/openid/userinfo";
            AddToCollection(config.UserInfoEndpointEncryptionAlgValuesSupported, "ECDH-ES+A128KW", "ECDH-ES+A192KW");
            AddToCollection(config.UserInfoEndpointEncryptionEncValuesSupported, "A256CBC-HS512", "A128CBC-HS256");
            AddToCollection(config.UserInfoEndpointSigningAlgValuesSupported, "ES384", "ES512");

            return config;
        }

        private static void AddToCollection(ICollection<string> collection, params string[] strings)
        {
            foreach (var str in strings)
                collection.Add(str);
        }
    }
}
