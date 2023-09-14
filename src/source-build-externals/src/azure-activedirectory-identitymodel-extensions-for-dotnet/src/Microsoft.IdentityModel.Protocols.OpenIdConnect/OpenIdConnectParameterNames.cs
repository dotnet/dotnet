// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect
{
    /// <summary>
    /// Parameter names for OpenIdConnect Request/Response messages.
    /// </summary>
    public static class OpenIdConnectParameterNames
    {
#pragma warning disable 1591
        public const string AccessToken = "access_token";
        public const string AcrValues = "acr_values";
        public const string ClaimsLocales = "claims_locales";
        public const string ClientAssertion = "client_assertion";
        public const string ClientAssertionType = "client_assertion_type";
        public const string ClientId = "client_id";
        public const string ClientSecret = "client_secret";
        public const string Code = "code";
        public const string Display = "display";
        public const string DomainHint = "domain_hint";
        public const string Error = "error";
        public const string ErrorDescription = "error_description";
        public const string ErrorUri = "error_uri";
        public const string ExpiresIn = "expires_in";
        public const string GrantType = "grant_type";
        public const string Iss = "iss";
        public const string IdToken = "id_token";
        public const string IdTokenHint = "id_token_hint";
        public const string IdentityProvider = "identity_provider";
        public const string LoginHint = "login_hint";
        public const string MaxAge = "max_age";
        public const string Nonce = "nonce";
        public const string Password = "password";
        public const string PostLogoutRedirectUri = "post_logout_redirect_uri";
        public const string Prompt = "prompt";
        public const string RedirectUri = "redirect_uri";
        public const string RefreshToken = "refresh_token";
        public const string RequestUri = "request_uri";
        public const string Resource = "resource";
        public const string ResponseMode = "response_mode";
        public const string ResponseType = "response_type";
        public const string Scope = "scope";
        public const string SkuTelemetry = "x-client-SKU";
        public const string SessionState = "session_state";
        public const string Sid = "sid";
        public const string State = "state";
        public const string TargetLinkUri = "target_link_uri";
        public const string TokenType = "token_type";
        public const string UiLocales = "ui_locales";
        public const string UserId = "user_id";
        public const string Username = "username";
        public const string VersionTelemetry = "x-client-ver";
#pragma warning restore 1591
    }

    /// <summary>
    /// Parameter names for OpenIdConnect Request/Response messages as UTF8 bytes.
    /// Used by UTF8JsonReader/Writer for performance gains.
    /// </summary>
    internal static class OpenIdConnectParameterUtf8Bytes
    {
        public static ReadOnlySpan<byte> AccessToken => "access_token"u8;
        public static ReadOnlySpan<byte> AcrValues => "acr_values"u8;
        public static ReadOnlySpan<byte> ClaimsLocales => "claims_locales"u8;
        public static ReadOnlySpan<byte> ClientAssertion => "client_assertion"u8;
        public static ReadOnlySpan<byte> ClientAssertionType => "client_assertion_type"u8;
        public static ReadOnlySpan<byte> ClientId => "client_id"u8;
        public static ReadOnlySpan<byte> ClientSecret => "client_secret"u8;
        public static ReadOnlySpan<byte> Code => "code"u8;
        public static ReadOnlySpan<byte> Display => "display"u8;
        public static ReadOnlySpan<byte> DomainHint => "domain_hint"u8;
        public static ReadOnlySpan<byte> Error => "error"u8;
        public static ReadOnlySpan<byte> ErrorDescription => "error_description"u8;
        public static ReadOnlySpan<byte> ErrorUri => "error_uri"u8;
        public static ReadOnlySpan<byte> ExpiresIn => "expires_in"u8;
        public static ReadOnlySpan<byte> GrantType => "grant_type"u8;
        public static ReadOnlySpan<byte> Iss => "iss"u8;
        public static ReadOnlySpan<byte> IdToken => "id_token"u8;
        public static ReadOnlySpan<byte> IdTokenHint => "id_token_hint"u8;
        public static ReadOnlySpan<byte> IdentityProvider => "identity_provider"u8;
        public static ReadOnlySpan<byte> LoginHint => "login_hint"u8;
        public static ReadOnlySpan<byte> MaxAge => "max_age"u8;
        public static ReadOnlySpan<byte> Nonce => "nonce"u8;
        public static ReadOnlySpan<byte> Password => "password"u8;
        public static ReadOnlySpan<byte> PostLogoutRedirectUri => "post_logout_redirect_uri"u8;
        public static ReadOnlySpan<byte> Prompt => "prompt"u8;
        public static ReadOnlySpan<byte> RedirectUri => "redirect_uri"u8;
        public static ReadOnlySpan<byte> RefreshToken => "refresh_token"u8;
        public static ReadOnlySpan<byte> RequestUri => "request_uri"u8;
        public static ReadOnlySpan<byte> Resource => "resource"u8;
        public static ReadOnlySpan<byte> ResponseMode => "response_mode"u8;
        public static ReadOnlySpan<byte> ResponseType => "response_type"u8;
        public static ReadOnlySpan<byte> Scope => "scope"u8;
        public static ReadOnlySpan<byte> SkuTelemetry => "x-client-SKU"u8;
        public static ReadOnlySpan<byte> SessionState => "session_state"u8;
        public static ReadOnlySpan<byte> Sid => "sid"u8;
        public static ReadOnlySpan<byte> State => "state"u8;
        public static ReadOnlySpan<byte> TargetLinkUri => "target_link_uri"u8;
        public static ReadOnlySpan<byte> TokenType => "token_type"u8;
        public static ReadOnlySpan<byte> UiLocales => "ui_locales"u8;
        public static ReadOnlySpan<byte> UserId => "user_id"u8;
        public static ReadOnlySpan<byte> Username => "username"u8;
        public static ReadOnlySpan<byte> VersionTelemetry => "x-client-ver"u8;
    }
}
