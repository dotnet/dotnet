// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect
{
    /// <summary>
    /// OpenIdProviderConfiguration Names
    /// http://openid.net/specs/openid-connect-discovery-1_0.html#ProviderMetadata 
    /// </summary>
    public static class OpenIdProviderMetadataNames
    {
#pragma warning disable 1591
        public const string AcrValuesSupported = "acr_values_supported";
        public const string AuthorizationEndpoint = "authorization_endpoint";
        public const string CheckSessionIframe = "check_session_iframe";
        public const string ClaimsLocalesSupported = "claims_locales_supported";
        public const string ClaimsParameterSupported = "claims_parameter_supported";
        public const string ClaimsSupported = "claims_supported";
        public const string ClaimTypesSupported = "claim_types_supported";
        public const string Discovery = ".well-known/openid-configuration";
        public const string DisplayValuesSupported = "display_values_supported";
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
        public const string RegistrationEndpoint = "registration_endpoint";
        public const string RequestObjectEncryptionAlgValuesSupported = "request_object_encryption_alg_values_supported";
        public const string RequestObjectEncryptionEncValuesSupported = "request_object_encryption_enc_values_supported";
        public const string RequestObjectSigningAlgValuesSupported = "request_object_signing_alg_values_supported";
        public const string RequestParameterSupported = "request_parameter_supported";
        public const string RequestUriParameterSupported = "request_uri_parameter_supported";
        public const string RequireRequestUriRegistration = "require_request_uri_registration";
        public const string ResponseModesSupported = "response_modes_supported";
        public const string ResponseTypesSupported = "response_types_supported";
        public const string ServiceDocumentation = "service_documentation";
        public const string ScopesSupported = "scopes_supported";
        public const string SubjectTypesSupported = "subject_types_supported";
        public const string TokenEndpoint = "token_endpoint";
        public const string TokenEndpointAuthMethodsSupported = "token_endpoint_auth_methods_supported";
        public const string TokenEndpointAuthSigningAlgValuesSupported = "token_endpoint_auth_signing_alg_values_supported";
        public const string UILocalesSupported = "ui_locales_supported";
        public const string UserInfoEndpoint = "userinfo_endpoint";
        public const string UserInfoEncryptionAlgValuesSupported = "userinfo_encryption_alg_values_supported";
        public const string UserInfoEncryptionEncValuesSupported = "userinfo_encryption_enc_values_supported";
        public const string UserInfoSigningAlgValuesSupported = "userinfo_signing_alg_values_supported";
#pragma warning restore 1591
    }
}
