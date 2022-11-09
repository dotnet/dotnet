//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect
{
    /// <summary>
    /// Parameter names for OpenIdConnect.
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
}
