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

        public static string OpenIdConnectMetadataEnd2EndAcrValuesLast = "OpenIdConnectMetadataEnd2EndAcrValuesLast.json";
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

                AddToCollection(config.CodeChallengeMethodsSupported, "plain", "S256");
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

        // 7/22/2024
        public static string AADCommonV1JwksString =>
            """
            {
                "keys": [
                    {
                        "kty": "RSA",
                        "use": "sig",
                        "kid": "23ntaxfH9Gk-8D_USzoAJgwjyt8",
                        "x5t": "23ntaxfH9Gk-8D_USzoAJgwjyt8",
                        "n": "hu2SJrLlDOUtU2s9T6-6OVGEPaba2zIT2_Jl50f4NGG-r-GyQdaOzTFASfAfMkMfMQMRnabqd-dp_Ooqha473bw6DMbM23nv2uhBn5Afp-S1W_d4NxEhfNlN1Tgjx3Sh6UblBSFCE4JGkugSkLi2SVouy43seskesQotXGVNv4iboFm4yO_twlMCG9EDwza32y6WZtV8i9gkQP42OfK0X1qy6EUz2DN7cpfZtmkNtsFJhFf9waOvNCR95LVCPGafeCOMAQEvu1VO3mrBSIg7Izu0CzvuaBQTwnGv29Ggxc3GO4gvb_OStkkmfIwchu3A8F6e0aJ4Ys8PFP7z7Z8lqQ",
                        "e": "AQAB",
                        "x5c": [
                            "MIIC/jCCAeagAwIBAgIJAJB4tJM2GkZjMA0GCSqGSIb3DQEBCwUAMC0xKzApBgNVBAMTImFjY291bnRzLmFjY2Vzc2NvbnRyb2wud2luZG93cy5uZXQwHhcNMjQwNTIyMTg1ODQwWhcNMjkwNTIyMTg1ODQwWjAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAhu2SJrLlDOUtU2s9T6+6OVGEPaba2zIT2/Jl50f4NGG+r+GyQdaOzTFASfAfMkMfMQMRnabqd+dp/Ooqha473bw6DMbM23nv2uhBn5Afp+S1W/d4NxEhfNlN1Tgjx3Sh6UblBSFCE4JGkugSkLi2SVouy43seskesQotXGVNv4iboFm4yO/twlMCG9EDwza32y6WZtV8i9gkQP42OfK0X1qy6EUz2DN7cpfZtmkNtsFJhFf9waOvNCR95LVCPGafeCOMAQEvu1VO3mrBSIg7Izu0CzvuaBQTwnGv29Ggxc3GO4gvb/OStkkmfIwchu3A8F6e0aJ4Ys8PFP7z7Z8lqQIDAQABoyEwHzAdBgNVHQ4EFgQUeGPdsxkVp8lIRku0u41SCzqW7LIwDQYJKoZIhvcNAQELBQADggEBAHMJCPO473QQJtTXJ49OhZ48kVCiVgbut+xElHxvBWQrfJ4Zb6WAi2RudjwrpwchVBciwjIelp/3Ryp5rVL94D479Ta/C5BzWNm9LsZCw3rPrsIvUdx26GmfQomHyL18AJQyBj8jZ+pVvdprvbV7v586TcgY24pW018IiYGQEO/fR8DSO4eN8ekTvT8hODBoKiJ9NFy+BruqW1AbMDptH12uzpU/N9bftysnWeDJEVZd5Rj8u8F9MRbB6V7dzxdoswaKkiJbxt+JrZgdtHSFqz6rDypIkumYwUkyiwH4/GQGPiyBLFbRp1EYVa3SFwAEmhl4a7On05aHVnOfCoyj/qA="
                        ]
                    },
                    {
                        "kty": "RSA",
                        "use": "sig",
                        "kid": "MGLqj98VNLoXaFfpJCBpgB4JaKs",
                        "x5t": "MGLqj98VNLoXaFfpJCBpgB4JaKs",
                        "n": "yfNcG8Ka_b4R7niLqdzlFvzRjrTdl2wTVEtqRWXqDhJAt_KIizVrqe0x3E1tNohmySHAMz3IS4llC41ML5YUDZVg33XR0RMc6UntOq-qCSOkPXdliC3QkwxspGAaJxVzhO5OZuQlMoQNL6_1FgdaR62gfayjLSJepB8M-o7yC8sOtRhatwe9kbO_5QJC54B8ni0ge5i9nANMln-9ZCHeRQYkgl0RSvR_KtfpWrEqAa4K2cyPaDqejOs8G8V0kM_8CLtDWi5diKpO_fvzRJwparEB5hfMdjAyJgdTOqCVUulZdL7tsoHzb8-_ufq-5QFJkyNUpYB9R1mVQwmRGdY0nQ",
                        "e": "AQAB",
                        "x5c": [
                            "MIIC/jCCAeagAwIBAgIJAJtuCSyF4i1FMA0GCSqGSIb3DQEBCwUAMC0xKzApBgNVBAMTImFjY291bnRzLmFjY2Vzc2NvbnRyb2wud2luZG93cy5uZXQwHhcNMjQwNjA5MTkxNzM5WhcNMjkwNjA5MTkxNzM5WjAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAyfNcG8Ka/b4R7niLqdzlFvzRjrTdl2wTVEtqRWXqDhJAt/KIizVrqe0x3E1tNohmySHAMz3IS4llC41ML5YUDZVg33XR0RMc6UntOq+qCSOkPXdliC3QkwxspGAaJxVzhO5OZuQlMoQNL6/1FgdaR62gfayjLSJepB8M+o7yC8sOtRhatwe9kbO/5QJC54B8ni0ge5i9nANMln+9ZCHeRQYkgl0RSvR/KtfpWrEqAa4K2cyPaDqejOs8G8V0kM/8CLtDWi5diKpO/fvzRJwparEB5hfMdjAyJgdTOqCVUulZdL7tsoHzb8+/ufq+5QFJkyNUpYB9R1mVQwmRGdY0nQIDAQABoyEwHzAdBgNVHQ4EFgQUzF0gtMcVDEn4JoNlDOxvhM8IHBswDQYJKoZIhvcNAQELBQADggEBAJe2muR0H2h3phiZ/v6FD8Yio6niulN9jr7+eC/UJV1M7l5xdHgVL83JbNZjUECDrJ/m+ICY1NbEXfv4fo3sfpU1AwG5GXAhxTrS4zMhH7Hvir3800wCd3ByJ/2vQW1y3orlqR8Q65BN9ayub6BCBTNmtUAOpAWcnP3FnGtIDmAL4APcacK92ZTg8ayVX586U7DDWmI4l7X6xCruK0ic5W2b13k2cay0EalHNWHl+gikqQg6tTGSvM295P6Xy5bQ1I5QtHjVCbm0315T/FylvR8fZhVD+AUCc1DwtOr3Yhm3EXftDb6hP08C4yDhGIDH3Q3+xuWlIA7KQjgljuiT67U="
                        ]
                    },
                    {
                        "kty": "RSA",
                        "use": "sig",
                        "kid": "inEVM76gXEQEonQ0PSQXBO_7HfU",
                        "x5t": "inEVM76gXEQEonQ0PSQXBO_7HfU",
                        "n": "q0sct8P8TxXmXX2QXzIhMnwZHdCO96SMFCMtfswF1TpxYqaIFObIhT_zxxpBTsvkYHAxG7CUQ6qVgd_TQhMx0TSZq_X3_0NG6cIRik0g-Woe0gT6tUJ-o6zdtO-6EvoOXovT3YMh8vN1Q5UJV6dudDqjnlTNHH1OxFcU4U6no1R6iILDMci_TGq7I2AJS5i_O9Ptp5NmgDT_kbwZHJz1Abbw4VuOPMFJ2Q1rN9odV9YHKjjowqa3BULVyTvP8FoGUzhoopu6O7oA-ehlO9fhEoSS0zNn0lWXQMZXUF7GSyui12121kIXyll2KlvuETQNdVkeXu0m95g_pnX-8iZ_cw",
                        "e": "AQAB",
                        "x5c": [
                            "MIIC/TCCAeWgAwIBAgIINBmSj+3xdxwwDQYJKoZIhvcNAQELBQAwLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldDAeFw0yNDA2MTIxNjA3MjBaFw0yOTA2MTIxNjA3MjBaMC0xKzApBgNVBAMTImFjY291bnRzLmFjY2Vzc2NvbnRyb2wud2luZG93cy5uZXQwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQCrSxy3w/xPFeZdfZBfMiEyfBkd0I73pIwUIy1+zAXVOnFipogU5siFP/PHGkFOy+RgcDEbsJRDqpWB39NCEzHRNJmr9ff/Q0bpwhGKTSD5ah7SBPq1Qn6jrN2077oS+g5ei9PdgyHy83VDlQlXp250OqOeVM0cfU7EVxThTqejVHqIgsMxyL9MarsjYAlLmL870+2nk2aANP+RvBkcnPUBtvDhW448wUnZDWs32h1X1gcqOOjCprcFQtXJO8/wWgZTOGiim7o7ugD56GU71+EShJLTM2fSVZdAxldQXsZLK6LXbXbWQhfKWXYqW+4RNA11WR5e7Sb3mD+mdf7yJn9zAgMBAAGjITAfMB0GA1UdDgQWBBSZbhe/r/sxfv0nYlyrwjx+b6W2RTANBgkqhkiG9w0BAQsFAAOCAQEAoWZ+C/snZySK1KiOsrn1iq7wrVzkuModPMZEshR3SuDIB6+C76fmP42I3UtDVIY5EeE79YjdwDwy86dPZjKVNbP7yUSbJC8uPM1TNMA9s8QpO6RZ63ZZ4i8hcgk6PXgi0PPjX2cmzUSNUa4gS8ibhf7JDu4aF9lUceBsNQghNQfz3tBs1ksJoW3WY5EfW6yMCv1Vim0uBlpnYdlynAd8O+9N2JR9wC+12PwPGrdGQDX3pos8bnmBxM55ueiGoqDH5yGI1h63POlGnpEdqOONT8N4cZNazQ1NswbBQuZMZSfbPXjiiFQ4bktyiXr421KbknQrkRYogi1F2Cjrd4SZJg=="
                        ]
                    },
                    {
                        "kty": "RSA",
                        "use": "sig",
                        "kid": "KQ2tAcrE7lBaVVGBmc5FobgdJo4",
                        "x5t": "KQ2tAcrE7lBaVVGBmc5FobgdJo4",
                        "n": "08xqQ-OBv9jvWmtvWw8g3IkcuDHVOAGCn3K6TXyKie0L6cAyQWNX4vqxbt0cHdaLunrzaFJ2mIGj_qfor8KR_FOFVFOF24FAakB5El96LvsTwlWJNIw4kpf1O_xibycZ_UcDAEqABJfe51JSPh-PxI2sXt0UMapSjvTdnps0Conp11Ay_yupl_h7nawVg0kzw3QDX5-vKTruiHAHr845YwDRW1yJLEgkUPYXdM8d_SrRgqb2RKJEN8D1c4-SUpFHKwGAwLgVYH1cqwADX9el857z_2uKqJoP48l8WqUOfNGdvx79RCgF1NzzRh07EQrk0GJ_EB8eO-EF4YHLPImVtQ",
                        "e": "AQAB",
                        "x5c": [
                            "MIIC/TCCAeWgAwIBAgIIRf5MUh/1XVIwDQYJKoZIhvcNAQELBQAwLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldDAeFw0yNDA3MTAxNjA0NTBaFw0yOTA3MTAxNjA0NTBaMC0xKzApBgNVBAMTImFjY291bnRzLmFjY2Vzc2NvbnRyb2wud2luZG93cy5uZXQwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQDTzGpD44G/2O9aa29bDyDciRy4MdU4AYKfcrpNfIqJ7QvpwDJBY1fi+rFu3Rwd1ou6evNoUnaYgaP+p+ivwpH8U4VUU4XbgUBqQHkSX3ou+xPCVYk0jDiSl/U7/GJvJxn9RwMASoAEl97nUlI+H4/Ejaxe3RQxqlKO9N2emzQKienXUDL/K6mX+HudrBWDSTPDdANfn68pOu6IcAevzjljANFbXIksSCRQ9hd0zx39KtGCpvZEokQ3wPVzj5JSkUcrAYDAuBVgfVyrAANf16XznvP/a4qomg/jyXxapQ580Z2/Hv1EKAXU3PNGHTsRCuTQYn8QHx474QXhgcs8iZW1AgMBAAGjITAfMB0GA1UdDgQWBBQGoURL0sKGdYALEdvfObZ6NEgmJTANBgkqhkiG9w0BAQsFAAOCAQEAl/UkmIa4OvsgULkBmGIZ6HeyJDvVuORphBK9/vpxEFsgnlitwMncBO54uMjJVr63baV490ODSI+ZTiCh7WGM+zrSjllCbVWDxjrdXA1ygHnXX7bXecIQyDmVb5/Hfb7DmQ4MHa3lEwf+pNS5XJeOhPoduRsfYCdD0QbxEADDgqV4FtgYx4I+iAoqbPDPou7wchEu9d3MuFuTMorkTvDLCyTHi2rgBnk9GBf2rArCGyTpvVPGXmxBttqgm9krFRujLj00u9jKUx4YkmAhS9YRddME8+gh6X4qFMxQMhyzkaBxjLs/E+pwMJaUwBqvostwt9+52qrMSUo+jkFgiGCe4Q=="
                        ]
                    },
                    {
                        "kty": "RSA",
                        "use": "sig",
                        "kid": "EHu9neGZBCDyv2IYq8U5JiRMFng",
                        "x5t": "EHu9neGZBCDyv2IYq8U5JiRMFng",
                        "n": "w1kH9dFGdaJS8fvQulDssuuNhkczzy1Mo6IiNoC3ih3K-L_VF5TQmSkqXrovWCUlhBCfc1VPR9Cn2G4UP7Sygn0nTqXBY1NFQQZecqwGESJFIuonRqjdlDhNYXjSF_eg63KyuyLV8A-Sn05Ufuc8ax0tyrxPbkOql0pB2hmRhj94iDAFB2LBoxfEgxCG3VT0ascVYW6voTCChs2P65-4RLC-ib1w1FjuACDwsB7KZDxxaUGLfnIoLWUjmw1zCaDRiRvhxB4jQXpB64IFxaYsqxA_x8bj2JEE7qALZ2dZ3fPy9yYSAnRfaTMetgouR9x4SKy4HxUxsADMm_7p9LiRZQ",
                        "e": "AQAB",
                        "x5c": [
                            "MIIC6jCCAdKgAwIBAgIJAO8yTjZIibNNMA0GCSqGSIb3DQEBCwUAMCMxITAfBgNVBAMTGGxvZ2luLm1pY3Jvc29mdG9ubGluZS51czAeFw0yNDA1MDYyMzA5MDJaFw0yOTA1MDYyMzA5MDJaMCMxITAfBgNVBAMTGGxvZ2luLm1pY3Jvc29mdG9ubGluZS51czCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAMNZB/XRRnWiUvH70LpQ7LLrjYZHM88tTKOiIjaAt4odyvi/1ReU0JkpKl66L1glJYQQn3NVT0fQp9huFD+0soJ9J06lwWNTRUEGXnKsBhEiRSLqJ0ao3ZQ4TWF40hf3oOtysrsi1fAPkp9OVH7nPGsdLcq8T25DqpdKQdoZkYY/eIgwBQdiwaMXxIMQht1U9GrHFWFur6EwgobNj+ufuESwvom9cNRY7gAg8LAeymQ8cWlBi35yKC1lI5sNcwmg0Ykb4cQeI0F6QeuCBcWmLKsQP8fG49iRBO6gC2dnWd3z8vcmEgJ0X2kzHrYKLkfceEisuB8VMbAAzJv+6fS4kWUCAwEAAaMhMB8wHQYDVR0OBBYEFJ4xtCt3JpPxlUVH7ATgJGM4ofg7MA0GCSqGSIb3DQEBCwUAA4IBAQB9WAEvE3VtO5wIOtN5N/QbIU63H5QPgMW3M9nOs43AhLgwvWupxaiATyMqtK53RPvcxYPe7QwSw/xH9McXii1bOBVmc71AcjlXYfuMJ/0IMEFEUQwZDEwj+vIlg07gWh0hleehyAgMblDUQRRN+b5J+soa9LBBAooY/48F/++y4DiTzKyoWn5cV4H2kdIFVyB43XzJRqDoK1ZhplVLTc1a3K1NL1/qP9rhvtx62YDzfNh4+FTJLu31ALcUbD+Qx2m0U9wuWq3EdUzEen5DeLvhx55YD7V1BASHNYBd8lGhHk97aTw53CMGAuTELvWO+4x7dFM9autw2KvSn76n/4Ql"
                        ]
                    },
                    {
                        "kty": "RSA",
                        "use": "sig",
                        "kid": "FB8_wii85nv_UW3qrldTvWwg-rE",
                        "x5t": "FB8_wii85nv_UW3qrldTvWwg-rE",
                        "n": "vusbA5UBNtCB0U2RmyQOCE-8fWl8bzCQXm3V5Nd7oockcyCpqXOWfhVNJD-Ifb5_zAmxRgHvRdfpA2btaqZiit5XaFYngtRK6mVxCcnOEgwxQGX9DLM5plXWtGTf_DF1FATBidFlM8KgicTS3MTyKZNrnTz0JD7ISxwV0TgSEiRrsm7eVsumuNYNW30Yb38DRDTei9U1YR0YDmdZyuf-OKTllxKH_BO-aj8Gkxcnkdriih2CINF6M6oASOHTJYO7P8CQE1DX2y2Zq7xxVvzm4IClk7WDdzuAoC-ZiKvDaU5plSyrnH3_VgjJrzXtuGN-HEd4Vg89h_2rE74cN5KRtQ",
                        "e": "AQAB",
                        "x5c": [
                            "MIIC6jCCAdKgAwIBAgIJAJOO92n+BJBLMA0GCSqGSIb3DQEBCwUAMCMxITAfBgNVBAMTGGxvZ2luLm1pY3Jvc29mdG9ubGluZS51czAeFw0yNDA2MTAxODA3NDVaFw0yOTA2MTAxODA3NDVaMCMxITAfBgNVBAMTGGxvZ2luLm1pY3Jvc29mdG9ubGluZS51czCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAL7rGwOVATbQgdFNkZskDghPvH1pfG8wkF5t1eTXe6KHJHMgqalzln4VTSQ/iH2+f8wJsUYB70XX6QNm7WqmYoreV2hWJ4LUSuplcQnJzhIMMUBl/QyzOaZV1rRk3/wxdRQEwYnRZTPCoInE0tzE8imTa5089CQ+yEscFdE4EhIka7Ju3lbLprjWDVt9GG9/A0Q03ovVNWEdGA5nWcrn/jik5ZcSh/wTvmo/BpMXJ5Ha4oodgiDRejOqAEjh0yWDuz/AkBNQ19stmau8cVb85uCApZO1g3c7gKAvmYirw2lOaZUsq5x9/1YIya817bhjfhxHeFYPPYf9qxO+HDeSkbUCAwEAAaMhMB8wHQYDVR0OBBYEFLBW6P0A+qHESOFg8Rgxqp38myYtMA0GCSqGSIb3DQEBCwUAA4IBAQAsZzkzk8w7RR3KCHOY+XLn3R2NanL/j+WILdOHnJn9Ot1VbG868MFQgwMp8Y2y7Kj5RekknY6EGcNuJi4rLgq5u1LSB/IoNPCs7l3MhRQqoedJX4sDNf4NfTVHK+4GNSQqP60eBoxClRexIbKcHJ0x57Ww/S9NNWtldBIfB7egoSj6UVcTHRLWZyPoZsOXHY4bYOf8ANNg21jT1KWwOXSWUx60v7tVxEXs8XAEUnmuMbuh3yAnjv3UoRdl7wcaQ5jq2/+vaAWZm0WlWN3CCY3y2mE0OZZg9HRCQu+o+58wt658sDIpP7PXGjyA5h23W9+i8QtyQ1PtqCXKj8zktivW"
                        ]
                    },
                    {
                        "kty": "RSA",
                        "use": "sig",
                        "kid": "ZZ8JkzXRCgYMWcMYdOn9sdDBeUA",
                        "x5t": "ZZ8JkzXRCgYMWcMYdOn9sdDBeUA",
                        "n": "iJd0N795eVyYQvWe417HOF_GHlRgOsPZRh1KwNHyWP_WKrjlOl8ftPAs-Sspv-s8v68TilHYkY9pjUdEkxBvBolJiPP6ntAKIKk_Fa4K7sutgQdyxKehhRnk4hAIc-mUM9ROrkyr4dIi-Au9T7aWaBSG5dCffXQBQ1DBVbIUMNOwr4ewQYeb49ujxzE6dPiCB-uDX2Z_hjy9M8wMrHS8e2vKDYqx3AJ3xyiDjIDB-wBEe-SF5bKVNSfExcsiL0KzV_iQkKQNALJrakX4Mw-hC3ssv7q3NQcza9kpyew0TKytSOcJcIheX9Cse22F-y1h873iOkYWiebNIu5TeTjVww",
                        "e": "AQAB",
                        "x5c": [
                            "MIIC6TCCAdGgAwIBAgIIHxgoSKq7mWgwDQYJKoZIhvcNAQELBQAwIzEhMB8GA1UEAxMYbG9naW4ubWljcm9zb2Z0b25saW5lLnVzMB4XDTI0MDYxMDE4MjgyOFoXDTI5MDYxMDE4MjgyOFowIzEhMB8GA1UEAxMYbG9naW4ubWljcm9zb2Z0b25saW5lLnVzMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAiJd0N795eVyYQvWe417HOF/GHlRgOsPZRh1KwNHyWP/WKrjlOl8ftPAs+Sspv+s8v68TilHYkY9pjUdEkxBvBolJiPP6ntAKIKk/Fa4K7sutgQdyxKehhRnk4hAIc+mUM9ROrkyr4dIi+Au9T7aWaBSG5dCffXQBQ1DBVbIUMNOwr4ewQYeb49ujxzE6dPiCB+uDX2Z/hjy9M8wMrHS8e2vKDYqx3AJ3xyiDjIDB+wBEe+SF5bKVNSfExcsiL0KzV/iQkKQNALJrakX4Mw+hC3ssv7q3NQcza9kpyew0TKytSOcJcIheX9Cse22F+y1h873iOkYWiebNIu5TeTjVwwIDAQABoyEwHzAdBgNVHQ4EFgQUcLvbIYVCbexuF1KXcKysM8kS6EMwDQYJKoZIhvcNAQELBQADggEBAF95Wf/yAfmHksmL42JiCemjsHN0KlZ2NsGTj2+zbDXbttj8zm+ZA74bPlAWI5aFvKfxxpC3Chfi26+GhKVeVRA65KyokTulQzE+BWbqphQZoH6Iz07J3GB3uUthPQbedtj6SDD/zE4jcmhmrY8o0lU5zJhkp9T5f8644ZR6rJRIXpFbDwmbsFM5H4Nz7D5FG+A4uYumICoTaiQjJ+cu/k8sDM8ut6R2cGmwlRMIGzD8HzNeGuaRtXsFqCGAI+qRbW29hJoFNZxhQBeFRDdBvwbNIa/o6ZAzKq81E4SdV1d33oM3vWDMBlR3b46a1d+Unm1Ou8uJ2yDfqMrZ7/NGNV8="
                        ]
                    }
                ]
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
                config.JsonWebKeySet = JsonWebKeySet.Create(AADCommonV1JwksString);

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

        public static string AADCommonV2JwksString =>
            """
            {
                "keys": [
                    {
                        "kty": "RSA",
                        "use": "sig",
                        "kid": "23ntaxfH9Gk-8D_USzoAJgwjyt8",
                        "x5t": "23ntaxfH9Gk-8D_USzoAJgwjyt8",
                        "n": "hu2SJrLlDOUtU2s9T6-6OVGEPaba2zIT2_Jl50f4NGG-r-GyQdaOzTFASfAfMkMfMQMRnabqd-dp_Ooqha473bw6DMbM23nv2uhBn5Afp-S1W_d4NxEhfNlN1Tgjx3Sh6UblBSFCE4JGkugSkLi2SVouy43seskesQotXGVNv4iboFm4yO_twlMCG9EDwza32y6WZtV8i9gkQP42OfK0X1qy6EUz2DN7cpfZtmkNtsFJhFf9waOvNCR95LVCPGafeCOMAQEvu1VO3mrBSIg7Izu0CzvuaBQTwnGv29Ggxc3GO4gvb_OStkkmfIwchu3A8F6e0aJ4Ys8PFP7z7Z8lqQ",
                        "e": "AQAB",
                        "x5c": [
                            "MIIC/jCCAeagAwIBAgIJAJB4tJM2GkZjMA0GCSqGSIb3DQEBCwUAMC0xKzApBgNVBAMTImFjY291bnRzLmFjY2Vzc2NvbnRyb2wud2luZG93cy5uZXQwHhcNMjQwNTIyMTg1ODQwWhcNMjkwNTIyMTg1ODQwWjAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAhu2SJrLlDOUtU2s9T6+6OVGEPaba2zIT2/Jl50f4NGG+r+GyQdaOzTFASfAfMkMfMQMRnabqd+dp/Ooqha473bw6DMbM23nv2uhBn5Afp+S1W/d4NxEhfNlN1Tgjx3Sh6UblBSFCE4JGkugSkLi2SVouy43seskesQotXGVNv4iboFm4yO/twlMCG9EDwza32y6WZtV8i9gkQP42OfK0X1qy6EUz2DN7cpfZtmkNtsFJhFf9waOvNCR95LVCPGafeCOMAQEvu1VO3mrBSIg7Izu0CzvuaBQTwnGv29Ggxc3GO4gvb/OStkkmfIwchu3A8F6e0aJ4Ys8PFP7z7Z8lqQIDAQABoyEwHzAdBgNVHQ4EFgQUeGPdsxkVp8lIRku0u41SCzqW7LIwDQYJKoZIhvcNAQELBQADggEBAHMJCPO473QQJtTXJ49OhZ48kVCiVgbut+xElHxvBWQrfJ4Zb6WAi2RudjwrpwchVBciwjIelp/3Ryp5rVL94D479Ta/C5BzWNm9LsZCw3rPrsIvUdx26GmfQomHyL18AJQyBj8jZ+pVvdprvbV7v586TcgY24pW018IiYGQEO/fR8DSO4eN8ekTvT8hODBoKiJ9NFy+BruqW1AbMDptH12uzpU/N9bftysnWeDJEVZd5Rj8u8F9MRbB6V7dzxdoswaKkiJbxt+JrZgdtHSFqz6rDypIkumYwUkyiwH4/GQGPiyBLFbRp1EYVa3SFwAEmhl4a7On05aHVnOfCoyj/qA="
                        ],
                        "issuer": "https://login.microsoftonline.com/{tenantid}/v2.0"
                    },
                    {
                        "kty": "RSA",
                        "use": "sig",
                        "kid": "MGLqj98VNLoXaFfpJCBpgB4JaKs",
                        "x5t": "MGLqj98VNLoXaFfpJCBpgB4JaKs",
                        "n": "yfNcG8Ka_b4R7niLqdzlFvzRjrTdl2wTVEtqRWXqDhJAt_KIizVrqe0x3E1tNohmySHAMz3IS4llC41ML5YUDZVg33XR0RMc6UntOq-qCSOkPXdliC3QkwxspGAaJxVzhO5OZuQlMoQNL6_1FgdaR62gfayjLSJepB8M-o7yC8sOtRhatwe9kbO_5QJC54B8ni0ge5i9nANMln-9ZCHeRQYkgl0RSvR_KtfpWrEqAa4K2cyPaDqejOs8G8V0kM_8CLtDWi5diKpO_fvzRJwparEB5hfMdjAyJgdTOqCVUulZdL7tsoHzb8-_ufq-5QFJkyNUpYB9R1mVQwmRGdY0nQ",
                        "e": "AQAB",
                        "x5c": [
                            "MIIC/jCCAeagAwIBAgIJAJtuCSyF4i1FMA0GCSqGSIb3DQEBCwUAMC0xKzApBgNVBAMTImFjY291bnRzLmFjY2Vzc2NvbnRyb2wud2luZG93cy5uZXQwHhcNMjQwNjA5MTkxNzM5WhcNMjkwNjA5MTkxNzM5WjAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAyfNcG8Ka/b4R7niLqdzlFvzRjrTdl2wTVEtqRWXqDhJAt/KIizVrqe0x3E1tNohmySHAMz3IS4llC41ML5YUDZVg33XR0RMc6UntOq+qCSOkPXdliC3QkwxspGAaJxVzhO5OZuQlMoQNL6/1FgdaR62gfayjLSJepB8M+o7yC8sOtRhatwe9kbO/5QJC54B8ni0ge5i9nANMln+9ZCHeRQYkgl0RSvR/KtfpWrEqAa4K2cyPaDqejOs8G8V0kM/8CLtDWi5diKpO/fvzRJwparEB5hfMdjAyJgdTOqCVUulZdL7tsoHzb8+/ufq+5QFJkyNUpYB9R1mVQwmRGdY0nQIDAQABoyEwHzAdBgNVHQ4EFgQUzF0gtMcVDEn4JoNlDOxvhM8IHBswDQYJKoZIhvcNAQELBQADggEBAJe2muR0H2h3phiZ/v6FD8Yio6niulN9jr7+eC/UJV1M7l5xdHgVL83JbNZjUECDrJ/m+ICY1NbEXfv4fo3sfpU1AwG5GXAhxTrS4zMhH7Hvir3800wCd3ByJ/2vQW1y3orlqR8Q65BN9ayub6BCBTNmtUAOpAWcnP3FnGtIDmAL4APcacK92ZTg8ayVX586U7DDWmI4l7X6xCruK0ic5W2b13k2cay0EalHNWHl+gikqQg6tTGSvM295P6Xy5bQ1I5QtHjVCbm0315T/FylvR8fZhVD+AUCc1DwtOr3Yhm3EXftDb6hP08C4yDhGIDH3Q3+xuWlIA7KQjgljuiT67U="
                        ],
                        "issuer": "https://login.microsoftonline.com/{tenantid}/v2.0"
                    },
                    {
                        "kty": "RSA",
                        "use": "sig",
                        "kid": "inEVM76gXEQEonQ0PSQXBO_7HfU",
                        "x5t": "inEVM76gXEQEonQ0PSQXBO_7HfU",
                        "n": "q0sct8P8TxXmXX2QXzIhMnwZHdCO96SMFCMtfswF1TpxYqaIFObIhT_zxxpBTsvkYHAxG7CUQ6qVgd_TQhMx0TSZq_X3_0NG6cIRik0g-Woe0gT6tUJ-o6zdtO-6EvoOXovT3YMh8vN1Q5UJV6dudDqjnlTNHH1OxFcU4U6no1R6iILDMci_TGq7I2AJS5i_O9Ptp5NmgDT_kbwZHJz1Abbw4VuOPMFJ2Q1rN9odV9YHKjjowqa3BULVyTvP8FoGUzhoopu6O7oA-ehlO9fhEoSS0zNn0lWXQMZXUF7GSyui12121kIXyll2KlvuETQNdVkeXu0m95g_pnX-8iZ_cw",
                        "e": "AQAB",
                        "x5c": [
                            "MIIC/TCCAeWgAwIBAgIINBmSj+3xdxwwDQYJKoZIhvcNAQELBQAwLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldDAeFw0yNDA2MTIxNjA3MjBaFw0yOTA2MTIxNjA3MjBaMC0xKzApBgNVBAMTImFjY291bnRzLmFjY2Vzc2NvbnRyb2wud2luZG93cy5uZXQwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQCrSxy3w/xPFeZdfZBfMiEyfBkd0I73pIwUIy1+zAXVOnFipogU5siFP/PHGkFOy+RgcDEbsJRDqpWB39NCEzHRNJmr9ff/Q0bpwhGKTSD5ah7SBPq1Qn6jrN2077oS+g5ei9PdgyHy83VDlQlXp250OqOeVM0cfU7EVxThTqejVHqIgsMxyL9MarsjYAlLmL870+2nk2aANP+RvBkcnPUBtvDhW448wUnZDWs32h1X1gcqOOjCprcFQtXJO8/wWgZTOGiim7o7ugD56GU71+EShJLTM2fSVZdAxldQXsZLK6LXbXbWQhfKWXYqW+4RNA11WR5e7Sb3mD+mdf7yJn9zAgMBAAGjITAfMB0GA1UdDgQWBBSZbhe/r/sxfv0nYlyrwjx+b6W2RTANBgkqhkiG9w0BAQsFAAOCAQEAoWZ+C/snZySK1KiOsrn1iq7wrVzkuModPMZEshR3SuDIB6+C76fmP42I3UtDVIY5EeE79YjdwDwy86dPZjKVNbP7yUSbJC8uPM1TNMA9s8QpO6RZ63ZZ4i8hcgk6PXgi0PPjX2cmzUSNUa4gS8ibhf7JDu4aF9lUceBsNQghNQfz3tBs1ksJoW3WY5EfW6yMCv1Vim0uBlpnYdlynAd8O+9N2JR9wC+12PwPGrdGQDX3pos8bnmBxM55ueiGoqDH5yGI1h63POlGnpEdqOONT8N4cZNazQ1NswbBQuZMZSfbPXjiiFQ4bktyiXr421KbknQrkRYogi1F2Cjrd4SZJg=="
                        ],
                        "issuer": "https://login.microsoftonline.com/{tenantid}/v2.0"
                    },
                    {
                        "kty": "RSA",
                        "use": "sig",
                        "kid": "KQ2tAcrE7lBaVVGBmc5FobgdJo4",
                        "x5t": "KQ2tAcrE7lBaVVGBmc5FobgdJo4",
                        "n": "08xqQ-OBv9jvWmtvWw8g3IkcuDHVOAGCn3K6TXyKie0L6cAyQWNX4vqxbt0cHdaLunrzaFJ2mIGj_qfor8KR_FOFVFOF24FAakB5El96LvsTwlWJNIw4kpf1O_xibycZ_UcDAEqABJfe51JSPh-PxI2sXt0UMapSjvTdnps0Conp11Ay_yupl_h7nawVg0kzw3QDX5-vKTruiHAHr845YwDRW1yJLEgkUPYXdM8d_SrRgqb2RKJEN8D1c4-SUpFHKwGAwLgVYH1cqwADX9el857z_2uKqJoP48l8WqUOfNGdvx79RCgF1NzzRh07EQrk0GJ_EB8eO-EF4YHLPImVtQ",
                        "e": "AQAB",
                        "x5c": [
                            "MIIC/TCCAeWgAwIBAgIIRf5MUh/1XVIwDQYJKoZIhvcNAQELBQAwLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldDAeFw0yNDA3MTAxNjA0NTBaFw0yOTA3MTAxNjA0NTBaMC0xKzApBgNVBAMTImFjY291bnRzLmFjY2Vzc2NvbnRyb2wud2luZG93cy5uZXQwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQDTzGpD44G/2O9aa29bDyDciRy4MdU4AYKfcrpNfIqJ7QvpwDJBY1fi+rFu3Rwd1ou6evNoUnaYgaP+p+ivwpH8U4VUU4XbgUBqQHkSX3ou+xPCVYk0jDiSl/U7/GJvJxn9RwMASoAEl97nUlI+H4/Ejaxe3RQxqlKO9N2emzQKienXUDL/K6mX+HudrBWDSTPDdANfn68pOu6IcAevzjljANFbXIksSCRQ9hd0zx39KtGCpvZEokQ3wPVzj5JSkUcrAYDAuBVgfVyrAANf16XznvP/a4qomg/jyXxapQ580Z2/Hv1EKAXU3PNGHTsRCuTQYn8QHx474QXhgcs8iZW1AgMBAAGjITAfMB0GA1UdDgQWBBQGoURL0sKGdYALEdvfObZ6NEgmJTANBgkqhkiG9w0BAQsFAAOCAQEAl/UkmIa4OvsgULkBmGIZ6HeyJDvVuORphBK9/vpxEFsgnlitwMncBO54uMjJVr63baV490ODSI+ZTiCh7WGM+zrSjllCbVWDxjrdXA1ygHnXX7bXecIQyDmVb5/Hfb7DmQ4MHa3lEwf+pNS5XJeOhPoduRsfYCdD0QbxEADDgqV4FtgYx4I+iAoqbPDPou7wchEu9d3MuFuTMorkTvDLCyTHi2rgBnk9GBf2rArCGyTpvVPGXmxBttqgm9krFRujLj00u9jKUx4YkmAhS9YRddME8+gh6X4qFMxQMhyzkaBxjLs/E+pwMJaUwBqvostwt9+52qrMSUo+jkFgiGCe4Q=="
                        ],
                        "issuer": "https://login.microsoftonline.com/{tenantid}/v2.0"
                    },
                    {
                        "kty": "RSA",
                        "use": "sig",
                        "kid": "EHu9neGZBCDyv2IYq8U5JiRMFng",
                        "x5t": "EHu9neGZBCDyv2IYq8U5JiRMFng",
                        "n": "w1kH9dFGdaJS8fvQulDssuuNhkczzy1Mo6IiNoC3ih3K-L_VF5TQmSkqXrovWCUlhBCfc1VPR9Cn2G4UP7Sygn0nTqXBY1NFQQZecqwGESJFIuonRqjdlDhNYXjSF_eg63KyuyLV8A-Sn05Ufuc8ax0tyrxPbkOql0pB2hmRhj94iDAFB2LBoxfEgxCG3VT0ascVYW6voTCChs2P65-4RLC-ib1w1FjuACDwsB7KZDxxaUGLfnIoLWUjmw1zCaDRiRvhxB4jQXpB64IFxaYsqxA_x8bj2JEE7qALZ2dZ3fPy9yYSAnRfaTMetgouR9x4SKy4HxUxsADMm_7p9LiRZQ",
                        "e": "AQAB",
                        "x5c": [
                            "MIIC6jCCAdKgAwIBAgIJAO8yTjZIibNNMA0GCSqGSIb3DQEBCwUAMCMxITAfBgNVBAMTGGxvZ2luLm1pY3Jvc29mdG9ubGluZS51czAeFw0yNDA1MDYyMzA5MDJaFw0yOTA1MDYyMzA5MDJaMCMxITAfBgNVBAMTGGxvZ2luLm1pY3Jvc29mdG9ubGluZS51czCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAMNZB/XRRnWiUvH70LpQ7LLrjYZHM88tTKOiIjaAt4odyvi/1ReU0JkpKl66L1glJYQQn3NVT0fQp9huFD+0soJ9J06lwWNTRUEGXnKsBhEiRSLqJ0ao3ZQ4TWF40hf3oOtysrsi1fAPkp9OVH7nPGsdLcq8T25DqpdKQdoZkYY/eIgwBQdiwaMXxIMQht1U9GrHFWFur6EwgobNj+ufuESwvom9cNRY7gAg8LAeymQ8cWlBi35yKC1lI5sNcwmg0Ykb4cQeI0F6QeuCBcWmLKsQP8fG49iRBO6gC2dnWd3z8vcmEgJ0X2kzHrYKLkfceEisuB8VMbAAzJv+6fS4kWUCAwEAAaMhMB8wHQYDVR0OBBYEFJ4xtCt3JpPxlUVH7ATgJGM4ofg7MA0GCSqGSIb3DQEBCwUAA4IBAQB9WAEvE3VtO5wIOtN5N/QbIU63H5QPgMW3M9nOs43AhLgwvWupxaiATyMqtK53RPvcxYPe7QwSw/xH9McXii1bOBVmc71AcjlXYfuMJ/0IMEFEUQwZDEwj+vIlg07gWh0hleehyAgMblDUQRRN+b5J+soa9LBBAooY/48F/++y4DiTzKyoWn5cV4H2kdIFVyB43XzJRqDoK1ZhplVLTc1a3K1NL1/qP9rhvtx62YDzfNh4+FTJLu31ALcUbD+Qx2m0U9wuWq3EdUzEen5DeLvhx55YD7V1BASHNYBd8lGhHk97aTw53CMGAuTELvWO+4x7dFM9autw2KvSn76n/4Ql"
                        ],
                        "issuer": "https://login.microsoftonline.com/{tenantid}/v2.0"
                    },
                    {
                        "kty": "RSA",
                        "use": "sig",
                        "kid": "FB8_wii85nv_UW3qrldTvWwg-rE",
                        "x5t": "FB8_wii85nv_UW3qrldTvWwg-rE",
                        "n": "vusbA5UBNtCB0U2RmyQOCE-8fWl8bzCQXm3V5Nd7oockcyCpqXOWfhVNJD-Ifb5_zAmxRgHvRdfpA2btaqZiit5XaFYngtRK6mVxCcnOEgwxQGX9DLM5plXWtGTf_DF1FATBidFlM8KgicTS3MTyKZNrnTz0JD7ISxwV0TgSEiRrsm7eVsumuNYNW30Yb38DRDTei9U1YR0YDmdZyuf-OKTllxKH_BO-aj8Gkxcnkdriih2CINF6M6oASOHTJYO7P8CQE1DX2y2Zq7xxVvzm4IClk7WDdzuAoC-ZiKvDaU5plSyrnH3_VgjJrzXtuGN-HEd4Vg89h_2rE74cN5KRtQ",
                        "e": "AQAB",
                        "x5c": [
                            "MIIC6jCCAdKgAwIBAgIJAJOO92n+BJBLMA0GCSqGSIb3DQEBCwUAMCMxITAfBgNVBAMTGGxvZ2luLm1pY3Jvc29mdG9ubGluZS51czAeFw0yNDA2MTAxODA3NDVaFw0yOTA2MTAxODA3NDVaMCMxITAfBgNVBAMTGGxvZ2luLm1pY3Jvc29mdG9ubGluZS51czCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAL7rGwOVATbQgdFNkZskDghPvH1pfG8wkF5t1eTXe6KHJHMgqalzln4VTSQ/iH2+f8wJsUYB70XX6QNm7WqmYoreV2hWJ4LUSuplcQnJzhIMMUBl/QyzOaZV1rRk3/wxdRQEwYnRZTPCoInE0tzE8imTa5089CQ+yEscFdE4EhIka7Ju3lbLprjWDVt9GG9/A0Q03ovVNWEdGA5nWcrn/jik5ZcSh/wTvmo/BpMXJ5Ha4oodgiDRejOqAEjh0yWDuz/AkBNQ19stmau8cVb85uCApZO1g3c7gKAvmYirw2lOaZUsq5x9/1YIya817bhjfhxHeFYPPYf9qxO+HDeSkbUCAwEAAaMhMB8wHQYDVR0OBBYEFLBW6P0A+qHESOFg8Rgxqp38myYtMA0GCSqGSIb3DQEBCwUAA4IBAQAsZzkzk8w7RR3KCHOY+XLn3R2NanL/j+WILdOHnJn9Ot1VbG868MFQgwMp8Y2y7Kj5RekknY6EGcNuJi4rLgq5u1LSB/IoNPCs7l3MhRQqoedJX4sDNf4NfTVHK+4GNSQqP60eBoxClRexIbKcHJ0x57Ww/S9NNWtldBIfB7egoSj6UVcTHRLWZyPoZsOXHY4bYOf8ANNg21jT1KWwOXSWUx60v7tVxEXs8XAEUnmuMbuh3yAnjv3UoRdl7wcaQ5jq2/+vaAWZm0WlWN3CCY3y2mE0OZZg9HRCQu+o+58wt658sDIpP7PXGjyA5h23W9+i8QtyQ1PtqCXKj8zktivW"
                        ],
                        "issuer": "https://login.microsoftonline.com/{tenantid}/v2.0"
                    },
                    {
                        "kty": "RSA",
                        "use": "sig",
                        "kid": "ZZ8JkzXRCgYMWcMYdOn9sdDBeUA",
                        "x5t": "ZZ8JkzXRCgYMWcMYdOn9sdDBeUA",
                        "n": "iJd0N795eVyYQvWe417HOF_GHlRgOsPZRh1KwNHyWP_WKrjlOl8ftPAs-Sspv-s8v68TilHYkY9pjUdEkxBvBolJiPP6ntAKIKk_Fa4K7sutgQdyxKehhRnk4hAIc-mUM9ROrkyr4dIi-Au9T7aWaBSG5dCffXQBQ1DBVbIUMNOwr4ewQYeb49ujxzE6dPiCB-uDX2Z_hjy9M8wMrHS8e2vKDYqx3AJ3xyiDjIDB-wBEe-SF5bKVNSfExcsiL0KzV_iQkKQNALJrakX4Mw-hC3ssv7q3NQcza9kpyew0TKytSOcJcIheX9Cse22F-y1h873iOkYWiebNIu5TeTjVww",
                        "e": "AQAB",
                        "x5c": [
                            "MIIC6TCCAdGgAwIBAgIIHxgoSKq7mWgwDQYJKoZIhvcNAQELBQAwIzEhMB8GA1UEAxMYbG9naW4ubWljcm9zb2Z0b25saW5lLnVzMB4XDTI0MDYxMDE4MjgyOFoXDTI5MDYxMDE4MjgyOFowIzEhMB8GA1UEAxMYbG9naW4ubWljcm9zb2Z0b25saW5lLnVzMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAiJd0N795eVyYQvWe417HOF/GHlRgOsPZRh1KwNHyWP/WKrjlOl8ftPAs+Sspv+s8v68TilHYkY9pjUdEkxBvBolJiPP6ntAKIKk/Fa4K7sutgQdyxKehhRnk4hAIc+mUM9ROrkyr4dIi+Au9T7aWaBSG5dCffXQBQ1DBVbIUMNOwr4ewQYeb49ujxzE6dPiCB+uDX2Z/hjy9M8wMrHS8e2vKDYqx3AJ3xyiDjIDB+wBEe+SF5bKVNSfExcsiL0KzV/iQkKQNALJrakX4Mw+hC3ssv7q3NQcza9kpyew0TKytSOcJcIheX9Cse22F+y1h873iOkYWiebNIu5TeTjVwwIDAQABoyEwHzAdBgNVHQ4EFgQUcLvbIYVCbexuF1KXcKysM8kS6EMwDQYJKoZIhvcNAQELBQADggEBAF95Wf/yAfmHksmL42JiCemjsHN0KlZ2NsGTj2+zbDXbttj8zm+ZA74bPlAWI5aFvKfxxpC3Chfi26+GhKVeVRA65KyokTulQzE+BWbqphQZoH6Iz07J3GB3uUthPQbedtj6SDD/zE4jcmhmrY8o0lU5zJhkp9T5f8644ZR6rJRIXpFbDwmbsFM5H4Nz7D5FG+A4uYumICoTaiQjJ+cu/k8sDM8ut6R2cGmwlRMIGzD8HzNeGuaRtXsFqCGAI+qRbW29hJoFNZxhQBeFRDdBvwbNIa/o6ZAzKq81E4SdV1d33oM3vWDMBlR3b46a1d+Unm1Ou8uJ2yDfqMrZ7/NGNV8="
                        ],
                        "issuer": "https://login.microsoftonline.com/{tenantid}/v2.0"
                    },
                    {
                        "kty": "RSA",
                        "use": "sig",
                        "kid": "dGtHQMhGltJUCcH_SQW64nEUoYE",
                        "x5t": "dGtHQMhGltJUCcH_SQW64nEUoYE",
                        "n": "io_Qh_kyYBnXCXPV54XbUZheP_fpWo5M0-_aWJQ6i-CebDHQVpxHurahUYj446qEvUBFK-goUEDU1Ah87F_KXNDQhsJq0F422joJPIzsHSsed_k0KlYnkJgCeUC8yHmtgSnNjH7jCnnBZ6Oznt0rdEw9MVd_2ofWgoA28XRUQ_arpXgGo8EWSPWuLGsG3cKTsSVW-1d_JSZ56S73j5YBDQz11ZPVm13nWohrGEgPBgswCCLUsZod0t1oTiRmKihRom-FhWvsfFixUZ4D39XSk51UjWttu1gnhhxhV7PVqlaqbvQ1D2urlpgMnAgyeQrIUC-3L-fN6hwD_1NZCaQdeQ",
                        "e": "AQAB",
                        "x5c": [
                            "MIIDCzCCAfOgAwIBAgIRAKdbZc1Eb0WDFn5HsLQuwwYwDQYJKoZIhvcNAQELBQAwKTEnMCUGA1UEAxMeTGl2ZSBJRCBTVFMgU2lnbmluZyBQdWJsaWMgS2V5MB4XDTI0MDcxNDE1MDI1MFoXDTI5MDcxNDE1MDI1MFowKTEnMCUGA1UEAxMeTGl2ZSBJRCBTVFMgU2lnbmluZyBQdWJsaWMgS2V5MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAio/Qh/kyYBnXCXPV54XbUZheP/fpWo5M0+/aWJQ6i+CebDHQVpxHurahUYj446qEvUBFK+goUEDU1Ah87F/KXNDQhsJq0F422joJPIzsHSsed/k0KlYnkJgCeUC8yHmtgSnNjH7jCnnBZ6Oznt0rdEw9MVd/2ofWgoA28XRUQ/arpXgGo8EWSPWuLGsG3cKTsSVW+1d/JSZ56S73j5YBDQz11ZPVm13nWohrGEgPBgswCCLUsZod0t1oTiRmKihRom+FhWvsfFixUZ4D39XSk51UjWttu1gnhhxhV7PVqlaqbvQ1D2urlpgMnAgyeQrIUC+3L+fN6hwD/1NZCaQdeQIDAQABoy4wLDAdBgNVHQ4EFgQUS8ytdfEdWAsFJApZy/6lA7wlfgowCwYDVR0PBAQDAgLEMA0GCSqGSIb3DQEBCwUAA4IBAQBDnyg3sAC3S64Pm4xA81r2kts96usCRu7tF34f3RJX7Met+rJMrllpRT8zVTzFTaPjHsJvhl5F/ApD0lZN6noy7UwNjbnMoC/lYluPLDuQE4ClstsgpNBdSNF0l+tWk085sIM7LF3wAuf17Yp5jIXCyokbbDJb5+XpNGZm4ukTLADajk/jk76z7p94shgV1XMla3fV+1d7jDL6UlbIvXNUSp3swvSLQPv90sSI2OUwTTulNZDokmeWtLUedTTIpnu9y+vLJWbKiwtenYbj3zM7VN/Qr5aXl4w3Ajx+QKnRydv1se8ycMabu28OFXgP92AsY1/NW4BF6321OOq2OmbC"
                        ],
                        "issuer": "https://login.microsoftonline.com/9188040d-6c67-4c5b-b112-36a304b66dad/v2.0"
                    },
                    {
                        "kty": "RSA",
                        "use": "sig",
                        "kid": "pb_TKRXVJY-27vIG_A81PMH-cdY",
                        "x5t": "pb_TKRXVJY-27vIG_A81PMH-cdY",
                        "n": "iM24cYc714exgvGQeAuw6pqYqkSf7NEuLug5jCYcGqa2APSSjzks5h7en-uEEnE0q03VeaJB6PWxaE3GTfGKXzr-sPudGCTTOsgnY3t4ms3DLeyhZvWi5ADc4JtpLBQOxYm1f4ReGwryZqsOHdvqNiYn7B7PyN_3dVbUuXWaueCJ3hhW5JyXkRGD75cOsgOm7GU3tYtOcxm29yjOzNcQXOiL_fChEz6G6bjOHzFYISgv5m7TffaOEFF4T4RoP4AQ35zvxjHx8XkBQPTz661TjTN1h_mYsFEwa2cDcErjJ4dJTdKSkM-VFPDklcSXsrDhkOw42ZeuKAoQTVep5EJ71w",
                        "e": "AQAB",
                        "x5c": [
                            "MIIDCjCCAfKgAwIBAgIQNas2IybvbYSgWXpOzctlSDANBgkqhkiG9w0BAQsFADApMScwJQYDVQQDEx5MaXZlIElEIFNUUyBTaWduaW5nIFB1YmxpYyBLZXkwHhcNMjQwNzAxMTgzMzA1WhcNMjkwNzAxMTgzMzA1WjApMScwJQYDVQQDEx5MaXZlIElEIFNUUyBTaWduaW5nIFB1YmxpYyBLZXkwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQCIzbhxhzvXh7GC8ZB4C7DqmpiqRJ/s0S4u6DmMJhwaprYA9JKPOSzmHt6f64QScTSrTdV5okHo9bFoTcZN8YpfOv6w+50YJNM6yCdje3iazcMt7KFm9aLkANzgm2ksFA7FibV/hF4bCvJmqw4d2+o2JifsHs/I3/d1VtS5dZq54IneGFbknJeREYPvlw6yA6bsZTe1i05zGbb3KM7M1xBc6Iv98KETPobpuM4fMVghKC/mbtN99o4QUXhPhGg/gBDfnO/GMfHxeQFA9PPrrVONM3WH+ZiwUTBrZwNwSuMnh0lN0pKQz5UU8OSVxJeysOGQ7DjZl64oChBNV6nkQnvXAgMBAAGjLjAsMB0GA1UdDgQWBBRsbUU+6mjS1sX/3Ek+xKEA6JTeLDALBgNVHQ8EBAMCAsQwDQYJKoZIhvcNAQELBQADggEBACO1MS24nE70L0Tcw4NRv80uZ8b5OWAfsAO+AN7zwXeo6J7TN/sslMuQ9FtL3Coot2ItdYaFHmfzKijuCV17EiWdXXccwoGEZqp3y2gvyYCof2OVQK4/KZVPUhI8wg2kR8dn09B9fdiMmwqd2+ZezWbgvSz1fQz5gZCg5FbFFojYvQL65bIq3tUZBtAT7ixrcGOfFbYzZbpi4mJdJItidd3Oh+TXfexzRL5Cw7Zn4LGlUVUOwildBfYtB+Fr022wutr/adxjJV7wgr6AxaTlls/hQz6+TOs8Vmyeb8KsU9CJZRXPIBKvZwAyMJsDZ3l4x+XPAZYQo3i6Oa4F5ROR9ZM="
                        ],
                        "issuer": "https://login.microsoftonline.com/9188040d-6c67-4c5b-b112-36a304b66dad/v2.0"
                    },
                    {
                        "kty": "RSA",
                        "use": "sig",
                        "kid": "yBm7yTZhZ3SWV3-9inQcQk1X51U",
                        "x5t": "yBm7yTZhZ3SWV3-9inQcQk1X51U",
                        "n": "nwNwsp54S9SUESzWUZXc0dY19bOVn4smmRSxANxPblU0nQEBpDPumlBVYmHI3XXVIshrh2DAl4BSVfQhVKLCu35Vyv7_P9cLvmqM_dvIHEjtrQPPFIBlH6fitB4v5zs7i7_zV-mTteGsNoUWg-TtHHKekJBrrBxoJ633vvaZ9AEFP8OdZoVGjXW1Wb76nczV8uhjgF9u69XrOPVrYB7YcxtiA-jRzn8AQRt8SfkrIvEjDL5ejtxRNyucz8dFzmbrCazoUY3oeei6UHjdtFgiODs4KE29e6p1Lm4CexjkcIrFWXkoxytOKEsB5zCGq8pQeI-tGmoCBhVnhNw7u5okjQ",
                        "e": "AQAB",
                        "x5c": [
                            "MIIDCzCCAfOgAwIBAgIRAMtLWPmqFNLXNg6BbZWr9EAwDQYJKoZIhvcNAQELBQAwKTEnMCUGA1UEAxMeTGl2ZSBJRCBTVFMgU2lnbmluZyBQdWJsaWMgS2V5MB4XDTI0MDYxODIyMzE1MFoXDTI5MDYxODIyMzE1MFowKTEnMCUGA1UEAxMeTGl2ZSBJRCBTVFMgU2lnbmluZyBQdWJsaWMgS2V5MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAnwNwsp54S9SUESzWUZXc0dY19bOVn4smmRSxANxPblU0nQEBpDPumlBVYmHI3XXVIshrh2DAl4BSVfQhVKLCu35Vyv7/P9cLvmqM/dvIHEjtrQPPFIBlH6fitB4v5zs7i7/zV+mTteGsNoUWg+TtHHKekJBrrBxoJ633vvaZ9AEFP8OdZoVGjXW1Wb76nczV8uhjgF9u69XrOPVrYB7YcxtiA+jRzn8AQRt8SfkrIvEjDL5ejtxRNyucz8dFzmbrCazoUY3oeei6UHjdtFgiODs4KE29e6p1Lm4CexjkcIrFWXkoxytOKEsB5zCGq8pQeI+tGmoCBhVnhNw7u5okjQIDAQABoy4wLDAdBgNVHQ4EFgQUFGb4FaXDu89wqG9A6JK1xLUMKPUwCwYDVR0PBAQDAgLEMA0GCSqGSIb3DQEBCwUAA4IBAQAw3cpDyl03Kgka9P2BJR6xU+C+IiWpJVLbLbdvBepqGI1/NqXCrG7E4INS5oRsFVO8DuYXws7Ko5kKCTV+iqkGngtG9b/JFP8QBcRrhngHTnE8EevwLkDtqFvpBdNnzTmOJDP4FdtYRuucJqx7aLE1MXr2jEkKfY7YLu2YEmOG6hnZfqWeCRm+g9eUolbhexllsdtj3bi9V9c8anXPLUsEeY/BRT7n4TBGJBWDD9kYEgoMKPLp58Om8aY6BucKN6vjf/v9RR//2ggCXX+qrZP3ebj9cXI6dWtgn5WwkBTfufIXnbbrzyCp/jdCP7q8SXbG2MeFiqKLKul5q5neiIdm"
                        ],
                        "issuer": "https://login.microsoftonline.com/9188040d-6c67-4c5b-b112-36a304b66dad/v2.0"
                    }
                ]
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
                config.JsonWebKeySet = JsonWebKeySet.Create(AADCommonV2JwksString);

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
                {{IssuerClaim}},
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
        {
            get
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
