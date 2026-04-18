// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect
{
    /// <summary>
    /// OpenID Provider Metadata parameter names
    /// <see href="https://openid.net/specs/openid-connect-discovery-1_0.html#ProviderMetadata"/>.
    /// </summary>
    public static class OpenIdProviderMetadataNames
    {
#pragma warning disable 1591
        public const string AcrValuesSupported = "acr_values_supported";
        public const string AuthorizationDetailsTypesSupported = "authorization_details_types_supported";
        public const string AuthorizationEndpoint = "authorization_endpoint";
        public const string AuthorizationEncryptionAlgValuesSupported = "authorization_encryption_alg_values_supported";
        public const string AuthorizationEncryptionEncValuesSupported = "authorization_encryption_enc_values_supported";
        public const string AuthorizationResponseIssParameterSupported = "authorization_response_iss_parameter_supported";
        public const string AuthorizationSigningAlgValuesSupported = "authorization_signing_alg_values_supported";
        public const string BackchannelAuthenticationEndpoint = "backchannel_authentication_endpoint";
        public const string BackchannelAuthenticationRequestSigningAlgValuesSupported = "backchannel_authentication_request_signing_alg_values_supported";
        public const string BackchannelTokenDeliveryModesSupported = "backchannel_token_delivery_modes_supported";
        public const string BackchannelUserCodeParameterSupported = "backchannel_user_code_parameter_supported";
        public const string CheckSessionIframe = "check_session_iframe";
        public const string ClaimsLocalesSupported = "claims_locales_supported";
        public const string ClaimsParameterSupported = "claims_parameter_supported";
        public const string ClaimsSupported = "claims_supported";
        public const string ClaimTypesSupported = "claim_types_supported";
        public const string CodeChallengeMethodsSupported = "code_challenge_methods_supported";
        public const string DeviceAuthorizationEndpoint = "device_authorization_endpoint";
        public const string Discovery = ".well-known/openid-configuration";
        public const string DisplayValuesSupported = "display_values_supported";
        public const string DPoPSigningAlgValuesSupported = "dpop_signing_alg_values_supported";
        public const string EndSessionEndpoint = "end_session_endpoint";
        public const string FrontchannelLogoutSessionSupported = "frontchannel_logout_session_supported";
        public const string FrontchannelLogoutSupported = "frontchannel_logout_supported";
        public const string HttpLogoutSupported = "http_logout_supported";
        public const string GrantTypesSupported = "grant_types_supported";
        public const string IdTokenEncryptionAlgValuesSupported = "id_token_encryption_alg_values_supported";
        public const string IdTokenEncryptionEncValuesSupported = "id_token_encryption_enc_values_supported";
        public const string IdTokenSigningAlgValuesSupported = "id_token_signing_alg_values_supported";
        public const string IntrospectionEndpoint = "introspection_endpoint";
        public const string IntrospectionEndpointAuthMethodsSupported = "introspection_endpoint_auth_methods_supported";
        public const string IntrospectionEndpointAuthSigningAlgValuesSupported = "introspection_endpoint_auth_signing_alg_values_supported";
        public const string JwksUri = "jwks_uri";
        public const string Issuer = "issuer";
        public const string LogoutSessionSupported = "logout_session_supported";
        public const string MicrosoftMultiRefreshToken = "microsoft_multi_refresh_token";
        public const string OpPolicyUri = "op_policy_uri";
        public const string OpTosUri = "op_tos_uri";
        public const string PromptValuesSupported = "prompt_values_supported";
        public const string PushedAuthorizationRequestEndpoint = "pushed_authorization_request_endpoint";
        public const string RegistrationEndpoint = "registration_endpoint";
        public const string RequestObjectEncryptionAlgValuesSupported = "request_object_encryption_alg_values_supported";
        public const string RequestObjectEncryptionEncValuesSupported = "request_object_encryption_enc_values_supported";
        public const string RequestObjectSigningAlgValuesSupported = "request_object_signing_alg_values_supported";
        public const string RequestParameterSupported = "request_parameter_supported";
        public const string RequestUriParameterSupported = "request_uri_parameter_supported";
        public const string RequirePushedAuthorizationRequests = "require_pushed_authorization_requests";
        public const string RequireRequestUriRegistration = "require_request_uri_registration";
        public const string ResponseModesSupported = "response_modes_supported";
        public const string ResponseTypesSupported = "response_types_supported";
        public const string RevocationEndpoint = "revocation_endpoint";
        public const string RevocationEndpointAuthMethodsSupported = "revocation_endpoint_auth_methods_supported";
        public const string RevocationEndpointAuthSigningAlgValuesSupported = "revocation_endpoint_auth_signing_alg_values_supported";
        public const string ServiceDocumentation = "service_documentation";
        public const string ScopesSupported = "scopes_supported";
        public const string SubjectTypesSupported = "subject_types_supported";
        public const string TokenEndpoint = "token_endpoint";
        public const string TokenEndpointAuthMethodsSupported = "token_endpoint_auth_methods_supported";
        public const string TokenEndpointAuthSigningAlgValuesSupported = "token_endpoint_auth_signing_alg_values_supported";
        public const string UILocalesSupported = "ui_locales_supported";
        public const string TlsClientCertificateBoundAccessTokens = "tls_client_certificate_bound_access_tokens";
        public const string UserInfoEndpoint = "userinfo_endpoint";
        public const string UserInfoEncryptionAlgValuesSupported = "userinfo_encryption_alg_values_supported";
        public const string UserInfoEncryptionEncValuesSupported = "userinfo_encryption_enc_values_supported";
        public const string UserInfoSigningAlgValuesSupported = "userinfo_signing_alg_values_supported";
#pragma warning restore 1591
    }

    /// <summary>
    /// OpenId Provider Metadata parameter names as UTF8Bytes
    /// Used by UTF8JsonReader/Writer for performance gains.
    /// http://openid.net/specs/openid-connect-discovery-1_0.html#ProviderMetadata
    /// </summary>
    internal static class OpenIdProviderMetadataUtf8Bytes
    {
        public static ReadOnlySpan<byte> AcrValuesSupported => "acr_values_supported"u8;
        public static ReadOnlySpan<byte> AuthorizationDetailsTypesSupported => "authorization_details_types_supported"u8;
        public static ReadOnlySpan<byte> AuthorizationEndpoint => "authorization_endpoint"u8;
        public static ReadOnlySpan<byte> AuthorizationEncryptionAlgValuesSupported => "authorization_encryption_alg_values_supported"u8;
        public static ReadOnlySpan<byte> AuthorizationEncryptionEncValuesSupported => "authorization_encryption_enc_values_supported"u8;
        public static ReadOnlySpan<byte> AuthorizationResponseIssParameterSupported => "authorization_response_iss_parameter_supported"u8;
        public static ReadOnlySpan<byte> AuthorizationSigningAlgValuesSupported => "authorization_signing_alg_values_supported"u8;
        public static ReadOnlySpan<byte> BackchannelAuthenticationEndpoint => "backchannel_authentication_endpoint"u8;
        public static ReadOnlySpan<byte> BackchannelAuthenticationRequestSigningAlgValuesSupported => "backchannel_authentication_request_signing_alg_values_supported"u8;
        public static ReadOnlySpan<byte> BackchannelTokenDeliveryModesSupported => "backchannel_token_delivery_modes_supported"u8;
        public static ReadOnlySpan<byte> BackchannelUserCodeParameterSupported => "backchannel_user_code_parameter_supported"u8;
        public static ReadOnlySpan<byte> CheckSessionIframe => "check_session_iframe"u8;
        public static ReadOnlySpan<byte> ClaimsLocalesSupported => "claims_locales_supported"u8;
        public static ReadOnlySpan<byte> ClaimsParameterSupported => "claims_parameter_supported"u8;
        public static ReadOnlySpan<byte> ClaimsSupported => "claims_supported"u8;
        public static ReadOnlySpan<byte> ClaimTypesSupported => "claim_types_supported"u8;
        public static ReadOnlySpan<byte> CodeChallengeMethodsSupported => "code_challenge_methods_supported"u8;
        public static ReadOnlySpan<byte> DeviceAuthorizationEndpoint => "device_authorization_endpoint"u8;
        public static ReadOnlySpan<byte> Discovery => ".well-known/openid-configuration"u8;
        public static ReadOnlySpan<byte> DisplayValuesSupported => "display_values_supported"u8;
        public static ReadOnlySpan<byte> DPoPSigningAlgValuesSupported => "dpop_signing_alg_values_supported"u8;
        public static ReadOnlySpan<byte> EndSessionEndpoint => "end_session_endpoint"u8;
        public static ReadOnlySpan<byte> FrontchannelLogoutSessionSupported => "frontchannel_logout_session_supported"u8;
        public static ReadOnlySpan<byte> FrontchannelLogoutSupported => "frontchannel_logout_supported"u8;
        public static ReadOnlySpan<byte> HttpLogoutSupported => "http_logout_supported"u8;
        public static ReadOnlySpan<byte> GrantTypesSupported => "grant_types_supported"u8;
        public static ReadOnlySpan<byte> IdTokenEncryptionAlgValuesSupported => "id_token_encryption_alg_values_supported"u8;
        public static ReadOnlySpan<byte> IdTokenEncryptionEncValuesSupported => "id_token_encryption_enc_values_supported"u8;
        public static ReadOnlySpan<byte> IdTokenSigningAlgValuesSupported => "id_token_signing_alg_values_supported"u8;
        public static ReadOnlySpan<byte> IntrospectionEndpoint => "introspection_endpoint"u8;
        public static ReadOnlySpan<byte> IntrospectionEndpointAuthMethodsSupported => "introspection_endpoint_auth_methods_supported"u8;
        public static ReadOnlySpan<byte> IntrospectionEndpointAuthSigningAlgValuesSupported => "introspection_endpoint_auth_signing_alg_values_supported"u8;
        public static ReadOnlySpan<byte> JwksUri => "jwks_uri"u8;
        public static ReadOnlySpan<byte> Issuer => "issuer"u8;
        public static ReadOnlySpan<byte> LogoutSessionSupported => "logout_session_supported"u8;
        public static ReadOnlySpan<byte> MicrosoftMultiRefreshToken => "microsoft_multi_refresh_token"u8;
        public static ReadOnlySpan<byte> OpPolicyUri => "op_policy_uri"u8;
        public static ReadOnlySpan<byte> OpTosUri => "op_tos_uri"u8;
        public static ReadOnlySpan<byte> PromptValuesSupported => "prompt_values_supported"u8;
        public static ReadOnlySpan<byte> PushedAuthorizationRequestEndpoint => "pushed_authorization_request_endpoint"u8;
        public static ReadOnlySpan<byte> RegistrationEndpoint => "registration_endpoint"u8;
        public static ReadOnlySpan<byte> RequestObjectEncryptionAlgValuesSupported => "request_object_encryption_alg_values_supported"u8;
        public static ReadOnlySpan<byte> RequestObjectEncryptionEncValuesSupported => "request_object_encryption_enc_values_supported"u8;
        public static ReadOnlySpan<byte> RequestObjectSigningAlgValuesSupported => "request_object_signing_alg_values_supported"u8;
        public static ReadOnlySpan<byte> RequestParameterSupported => "request_parameter_supported"u8;
        public static ReadOnlySpan<byte> RequestUriParameterSupported => "request_uri_parameter_supported"u8;
        public static ReadOnlySpan<byte> RequirePushedAuthorizationRequests => "require_pushed_authorization_requests"u8;
        public static ReadOnlySpan<byte> RequireRequestUriRegistration => "require_request_uri_registration"u8;
        public static ReadOnlySpan<byte> ResponseModesSupported => "response_modes_supported"u8;
        public static ReadOnlySpan<byte> ResponseTypesSupported => "response_types_supported"u8;
        public static ReadOnlySpan<byte> RevocationEndpoint => "revocation_endpoint"u8;
        public static ReadOnlySpan<byte> RevocationEndpointAuthMethodsSupported => "revocation_endpoint_auth_methods_supported"u8;
        public static ReadOnlySpan<byte> RevocationEndpointAuthSigningAlgValuesSupported => "revocation_endpoint_auth_signing_alg_values_supported"u8;
        public static ReadOnlySpan<byte> ServiceDocumentation => "service_documentation"u8;
        public static ReadOnlySpan<byte> ScopesSupported => "scopes_supported"u8;
        public static ReadOnlySpan<byte> SubjectTypesSupported => "subject_types_supported"u8;
        public static ReadOnlySpan<byte> TokenEndpoint => "token_endpoint"u8;
        public static ReadOnlySpan<byte> TokenEndpointAuthMethodsSupported => "token_endpoint_auth_methods_supported"u8;
        public static ReadOnlySpan<byte> TokenEndpointAuthSigningAlgValuesSupported => "token_endpoint_auth_signing_alg_values_supported"u8;
        public static ReadOnlySpan<byte> TlsClientCertificateBoundAccessTokens => "tls_client_certificate_bound_access_tokens"u8;
        public static ReadOnlySpan<byte> UILocalesSupported => "ui_locales_supported"u8;
        public static ReadOnlySpan<byte> UserInfoEndpoint => "userinfo_endpoint"u8;
        public static ReadOnlySpan<byte> UserInfoEncryptionAlgValuesSupported => "userinfo_encryption_alg_values_supported"u8;
        public static ReadOnlySpan<byte> UserInfoEncryptionEncValuesSupported => "userinfo_encryption_enc_values_supported"u8;
        public static ReadOnlySpan<byte> UserInfoSigningAlgValuesSupported => "userinfo_signing_alg_values_supported"u8;
    }
}
