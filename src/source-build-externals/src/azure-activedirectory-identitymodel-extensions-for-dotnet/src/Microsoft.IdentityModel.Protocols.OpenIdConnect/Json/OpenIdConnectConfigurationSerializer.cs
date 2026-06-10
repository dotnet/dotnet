// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.IdentityModel.Logging;
using Utf8Bytes = Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdProviderMetadataUtf8Bytes;
using JsonPrimitives = Microsoft.IdentityModel.Tokens.Json.JsonSerializerPrimitives;
using MetadataName = Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdProviderMetadataNames;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect
{
    internal static class OpenIdConnectConfigurationSerializer
    {
        public const string ClassName = OpenIdConnectConfiguration.ClassName;

        // This is used to perform performant case-insensitive property names.
        // 6x used Newtonsoft and was case-insensitive w.r.t. property names.
        // The serializer is written to use Utf8JsonReader.ValueTextEquals(...), to match property names.
        // When we do not have a match, we check the uppercase name of the property against this table.
        // If not found, then we assume we should put the value into AdditionalData.
        // If we didn't do that, we would pay a performance penalty for those cases where there is AdditionalData
        // but otherwise the JSON properties are all lower case.
        public static HashSet<string> OpenIdProviderMetadataNamesUpperCase = new HashSet<string>
        {
            "ACR_VALUES_SUPPORTED",
            "AUTHORIZATION_ENDPOINT",
            "AUTHORIZATION_RESPONSE_ISS_PARAMETER_SUPPORTED",
            "BACKCHANNEL_AUTHENTICATION_ENDPOINT",
            "BACKCHANNEL_AUTHENTICATION_REQUEST_SIGNING_ALG_VALUES_SUPPORTED",
            "BACKCHANNEL_TOKEN_DELIVERY_MODES_SUPPORTED",
            "BACKCHANNEL_USER_CODE_PARAMETER_SUPPORTED",
            "CHECK_SESSION_IFRAME",
            "CLAIMS_LOCALES_SUPPORTED",
            "CLAIMS_PARAMETER_SUPPORTED",
            "CLAIMS_SUPPORTED",
            "CLAIM_TYPES_SUPPORTED",
            ".WELL-KNOWN/OPENID-CONFIGURATION",
            "DISPLAY_VALUES_SUPPORTED",
            "DPOP_SIGNING_ALG_VALUES_SUPPORTED",
            "END_SESSION_ENDPOINT",
            "FRONTCHANNEL_LOGOUT_SESSION_SUPPORTED",
            "FRONTCHANNEL_LOGOUT_SUPPORTED",
            "HTTP_LOGOUT_SUPPORTED",
            "GRANT_TYPES_SUPPORTED",
            "ID_TOKEN_ENCRYPTION_ALG_VALUES_SUPPORTED",
            "ID_TOKEN_ENCRYPTION_ENC_VALUES_SUPPORTED",
            "ID_TOKEN_SIGNING_ALG_VALUES_SUPPORTED",
            "INTROSPECTION_ENDPOINT",
            "INTROSPECTION_ENDPOINT_AUTH_METHODS_SUPPORTED",
            "INTROSPECTION_ENDPOINT_AUTH_SIGNING_ALG_VALUES_SUPPORTED",
            "JWKS_URI",
            "ISSUER",
            "LOGOUT_SESSION_SUPPORTED",
            "OP_POLICY_URI",
            "OP_TOS_URI",
            "PROMPT_VALUES_SUPPORTED",
            "PUSHED_AUTHORIZATION_REQUEST_ENDPOINT",
            "REGISTRATION_ENDPOINT",
            "REQUEST_OBJECT_ENCRYPTION_ALG_VALUES_SUPPORTED",
            "REQUEST_OBJECT_ENCRYPTION_ENC_VALUES_SUPPORTED",
            "REQUEST_OBJECT_SIGNING_ALG_VALUES_SUPPORTED",
            "REQUEST_PARAMETER_SUPPORTED",
            "REQUEST_URI_PARAMETER_SUPPORTED",
            "REQUIRE_PUSHED_AUTHORIZATION_REQUESTS",
            "REQUIRE_REQUEST_URI_REGISTRATION",
            "RESPONSE_MODES_SUPPORTED",
            "RESPONSE_TYPES_SUPPORTED",
            "SERVICE_DOCUMENTATION",
            "SCOPES_SUPPORTED",
            "SUBJECT_TYPES_SUPPORTED",
            "TOKEN_ENDPOINT",
            "TOKEN_ENDPOINT_AUTH_METHODS_SUPPORTED",
            "TOKEN_ENDPOINT_AUTH_SIGNING_ALG_VALUES_SUPPORTED",
            "UI_LOCALES_SUPPORTED",
            "USERINFO_ENDPOINT",
            "USERINFO_ENCRYPTION_ALG_VALUES_SUPPORTED",
            "USERINFO_ENCRYPTION_ENC_VALUES_SUPPORTED",
            "USERINFO_SIGNING_ALG_VALUES_SUPPORTED",
        };

        #region Read
        public static OpenIdConnectConfiguration Read(string json)
        {
            return Read(json, new OpenIdConnectConfiguration());
        }

        public static OpenIdConnectConfiguration Read(string json, OpenIdConnectConfiguration config)
        {
            Utf8JsonReader reader = new(Encoding.UTF8.GetBytes(json).AsSpan());
            try
            {
                return Read(ref reader, config);
            }
            catch(JsonException ex)
            {
                if (ex.GetType() == typeof(JsonException))
                    throw;

                throw LogHelper.LogExceptionMessage(
                    new JsonException(
                        LogHelper.FormatInvariant(
                            Tokens.LogMessages.IDX10805,
                            LogHelper.MarkAsNonPII(json),
                            LogHelper.MarkAsNonPII(ClassName))));
            }
        }

        /// <summary>
        /// Reads config. see: https://openid.net/specs/openid-connect-discovery-1_0.html
        /// </summary>
        /// <param name="reader">a <see cref="Utf8JsonReader"/> pointing at a StartObject.</param>
        /// <param name="config"></param>
        /// <returns>A <see cref="OpenIdConnectConfiguration"/>.</returns>
        public static OpenIdConnectConfiguration Read(ref Utf8JsonReader reader, OpenIdConnectConfiguration config)
        {
            if (!JsonPrimitives.IsReaderAtTokenType(ref reader, JsonTokenType.StartObject, true))
                throw LogHelper.LogExceptionMessage(
                    new JsonException(
                        LogHelper.FormatInvariant(
                        Tokens.LogMessages.IDX11023,
                        LogHelper.MarkAsNonPII("JsonTokenType.StartObject"),
                        LogHelper.MarkAsNonPII(reader.TokenType),
                        LogHelper.MarkAsNonPII(ClassName),
                        LogHelper.MarkAsNonPII(reader.TokenStartIndex),
                        LogHelper.MarkAsNonPII(reader.CurrentDepth),
                        LogHelper.MarkAsNonPII(reader.BytesConsumed))));

            while(true)
            {
                #region Check property name using ValueTextEquals
                // https://datatracker.ietf.org/doc/html/rfc7517#section-4, does not require that we reject JSON with duplicate member names.
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    // JsonPrimitives.Read(...) passes 'true' to advance reader to next token.
                    if (reader.ValueTextEquals(Utf8Bytes.AcrValuesSupported))
                        JsonPrimitives.ReadStrings(ref reader, config.AcrValuesSupported, MetadataName.AcrValuesSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.AuthorizationEndpoint))
                        config.AuthorizationEndpoint = JsonPrimitives.ReadString(ref reader, MetadataName.AuthorizationEndpoint, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.AuthorizationResponseIssParameterSupported))
                        config.AuthorizationResponseIssParameterSupported = JsonPrimitives.ReadBoolean(ref reader, MetadataName.AuthorizationResponseIssParameterSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.BackchannelAuthenticationEndpoint))
                        config.BackchannelAuthenticationEndpoint = JsonPrimitives.ReadString(ref reader, MetadataName.BackchannelAuthenticationEndpoint, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.BackchannelAuthenticationRequestSigningAlgValuesSupported))
                        JsonPrimitives.ReadStrings(ref reader, config.BackchannelAuthenticationRequestSigningAlgValuesSupported, MetadataName.BackchannelAuthenticationRequestSigningAlgValuesSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.BackchannelTokenDeliveryModesSupported))
                        JsonPrimitives.ReadStrings(ref reader, config.BackchannelTokenDeliveryModesSupported, MetadataName.BackchannelTokenDeliveryModesSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.BackchannelUserCodeParameterSupported))
                        config.BackchannelUserCodeParameterSupported = JsonPrimitives.ReadBoolean(ref reader, MetadataName.BackchannelUserCodeParameterSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.CheckSessionIframe))
                        config.CheckSessionIframe = JsonPrimitives.ReadString(ref reader, MetadataName.CheckSessionIframe, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.ClaimsLocalesSupported))
                        JsonPrimitives.ReadStrings(ref reader, config.ClaimsLocalesSupported, MetadataName.ClaimsLocalesSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.ClaimsParameterSupported))
                        config.ClaimsParameterSupported = JsonPrimitives.ReadBoolean(ref reader, MetadataName.ClaimsParameterSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.ClaimsSupported))
                        JsonPrimitives.ReadStrings(ref reader, config.ClaimsSupported, MetadataName.ClaimsSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.ClaimTypesSupported))
                        JsonPrimitives.ReadStrings(ref reader, config.ClaimTypesSupported, MetadataName.ClaimTypesSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.DisplayValuesSupported))
                        JsonPrimitives.ReadStrings(ref reader, config.DisplayValuesSupported, MetadataName.DisplayValuesSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.DPoPSigningAlgValuesSupported))
                        JsonPrimitives.ReadStrings(ref reader, config.DPoPSigningAlgValuesSupported, MetadataName.DPoPSigningAlgValuesSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.EndSessionEndpoint))
                        config.EndSessionEndpoint = JsonPrimitives.ReadString(ref reader, MetadataName.EndSessionEndpoint, ClassName, true);

                    // FrontchannelLogoutSessionSupported and FrontchannelLogoutSupported are per spec 'boolean'.
                    // We shipped pervious versions accepting a string and transforming to a boolean.
                    else if (reader.ValueTextEquals(Utf8Bytes.FrontchannelLogoutSessionSupported))
                    {
                        reader.Read();
                        if (reader.TokenType == JsonTokenType.True)
                            config.FrontchannelLogoutSessionSupported = JsonPrimitives.True;
                        else if (reader.TokenType == JsonTokenType.False)
                            config.FrontchannelLogoutSessionSupported = JsonPrimitives.False;
                        else
                            config.FrontchannelLogoutSessionSupported = JsonPrimitives.ReadStringAsBool(ref reader, MetadataName.FrontchannelLogoutSessionSupported, ClassName, false);
                    }
                    else if (reader.ValueTextEquals(Utf8Bytes.FrontchannelLogoutSupported))
                    {
                        reader.Read();
                        if (reader.TokenType == JsonTokenType.True)
                            config.FrontchannelLogoutSupported = JsonPrimitives.True;
                        else if (reader.TokenType == JsonTokenType.False)
                            config.FrontchannelLogoutSupported = JsonPrimitives.False;
                        else
                            config.FrontchannelLogoutSupported = JsonPrimitives.ReadStringAsBool(ref reader, MetadataName.FrontchannelLogoutSupported, ClassName, false);
                    }
                    else if (reader.ValueTextEquals(Utf8Bytes.GrantTypesSupported))
                        JsonPrimitives.ReadStrings(ref reader, config.GrantTypesSupported, MetadataName.GrantTypesSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.HttpLogoutSupported))
                        config.HttpLogoutSupported = JsonPrimitives.ReadBoolean(ref reader, MetadataName.HttpLogoutSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.IdTokenEncryptionAlgValuesSupported))
                        JsonPrimitives.ReadStrings(ref reader, config.IdTokenEncryptionAlgValuesSupported, MetadataName.IdTokenEncryptionAlgValuesSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.IdTokenEncryptionEncValuesSupported))
                        JsonPrimitives.ReadStrings(ref reader, config.IdTokenEncryptionEncValuesSupported, MetadataName.IdTokenEncryptionEncValuesSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.IdTokenSigningAlgValuesSupported))
                        JsonPrimitives.ReadStrings(ref reader, config.IdTokenSigningAlgValuesSupported, MetadataName.IdTokenSigningAlgValuesSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.IntrospectionEndpoint))
                        config.IntrospectionEndpoint = JsonPrimitives.ReadString(ref reader, MetadataName.IntrospectionEndpoint, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.IntrospectionEndpointAuthMethodsSupported))
                        JsonPrimitives.ReadStrings(ref reader, config.IntrospectionEndpointAuthMethodsSupported, MetadataName.IntrospectionEndpointAuthMethodsSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.IntrospectionEndpointAuthSigningAlgValuesSupported))
                        JsonPrimitives.ReadStrings(ref reader, config.IntrospectionEndpointAuthSigningAlgValuesSupported, MetadataName.IntrospectionEndpointAuthSigningAlgValuesSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.Issuer))
                        config.Issuer = JsonPrimitives.ReadString(ref reader, MetadataName.Issuer, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.JwksUri))
                        config.JwksUri = JsonPrimitives.ReadString(ref reader, MetadataName.JwksUri, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.LogoutSessionSupported))
                        config.LogoutSessionSupported = JsonPrimitives.ReadBoolean(ref reader, MetadataName.LogoutSessionSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.OpPolicyUri))
                        config.OpPolicyUri = JsonPrimitives.ReadString(ref reader, MetadataName.OpPolicyUri, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.OpTosUri))
                        config.OpTosUri = JsonPrimitives.ReadString(ref reader, MetadataName.OpTosUri, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.PromptValuesSupported))
                        JsonPrimitives.ReadStrings(ref reader, config.PromptValuesSupported, MetadataName.PromptValuesSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.PushedAuthorizationRequestEndpoint))
                        config.PushedAuthorizationRequestEndpoint = JsonPrimitives.ReadString(ref reader, MetadataName.PushedAuthorizationRequestEndpoint, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.RegistrationEndpoint))
                        config.RegistrationEndpoint = JsonPrimitives.ReadString(ref reader, MetadataName.RegistrationEndpoint, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.RequestObjectEncryptionAlgValuesSupported))
                        JsonPrimitives.ReadStrings(ref reader, config.RequestObjectEncryptionAlgValuesSupported, MetadataName.RequestObjectEncryptionAlgValuesSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.RequestObjectEncryptionEncValuesSupported))
                        JsonPrimitives.ReadStrings(ref reader, config.RequestObjectEncryptionEncValuesSupported, MetadataName.RequestObjectEncryptionEncValuesSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.RequestObjectSigningAlgValuesSupported))
                        JsonPrimitives.ReadStrings(ref reader, config.RequestObjectSigningAlgValuesSupported, MetadataName.RequestObjectSigningAlgValuesSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.RequestParameterSupported))
                        config.RequestParameterSupported = JsonPrimitives.ReadBoolean(ref reader, MetadataName.RequestParameterSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.RequestUriParameterSupported))
                        config.RequestUriParameterSupported = JsonPrimitives.ReadBoolean(ref reader, MetadataName.RequestUriParameterSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.RequireRequestUriRegistration))
                        config.RequireRequestUriRegistration = JsonPrimitives.ReadBoolean(ref reader, MetadataName.RequireRequestUriRegistration, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.ResponseModesSupported))
                        JsonPrimitives.ReadStrings(ref reader, config.ResponseModesSupported, MetadataName.ResponseModesSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.ResponseTypesSupported))
                        JsonPrimitives.ReadStrings(ref reader, config.ResponseTypesSupported, MetadataName.ResponseTypesSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.ScopesSupported))
                        JsonPrimitives.ReadStrings(ref reader, config.ScopesSupported, MetadataName.ScopesSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.ServiceDocumentation))
                        config.ServiceDocumentation = JsonPrimitives.ReadString(ref reader, MetadataName.ScopesSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.SubjectTypesSupported))
                        JsonPrimitives.ReadStrings(ref reader, config.SubjectTypesSupported, MetadataName.SubjectTypesSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.SubjectTypesSupported))
                        JsonPrimitives.ReadStrings(ref reader, config.SubjectTypesSupported, MetadataName.SubjectTypesSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.TokenEndpoint))
                        config.TokenEndpoint = JsonPrimitives.ReadString(ref reader, MetadataName.TokenEndpoint, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.TokenEndpointAuthMethodsSupported))
                        JsonPrimitives.ReadStrings(ref reader, config.TokenEndpointAuthMethodsSupported, MetadataName.TokenEndpointAuthMethodsSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.TokenEndpointAuthSigningAlgValuesSupported))
                        JsonPrimitives.ReadStrings(ref reader, config.TokenEndpointAuthSigningAlgValuesSupported, MetadataName.TokenEndpointAuthSigningAlgValuesSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.UILocalesSupported))
                        JsonPrimitives.ReadStrings(ref reader, config.UILocalesSupported, MetadataName.UILocalesSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.UserInfoEncryptionAlgValuesSupported))
                        JsonPrimitives.ReadStrings(ref reader, config.UserInfoEndpointEncryptionAlgValuesSupported, MetadataName.UserInfoEncryptionAlgValuesSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.UserInfoEncryptionEncValuesSupported))
                        JsonPrimitives.ReadStrings(ref reader, config.UserInfoEndpointEncryptionEncValuesSupported, MetadataName.UserInfoEncryptionEncValuesSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.UserInfoEndpoint))
                        config.UserInfoEndpoint = JsonPrimitives.ReadString(ref reader, MetadataName.UserInfoEndpoint, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.UserInfoSigningAlgValuesSupported))
                        JsonPrimitives.ReadStrings(ref reader, config.UserInfoEndpointSigningAlgValuesSupported, MetadataName.UserInfoSigningAlgValuesSupported, ClassName, true);

                    else if (reader.ValueTextEquals(Utf8Bytes.RequirePushedAuthorizationRequests))
                        config.RequirePushedAuthorizationRequests = JsonPrimitives.ReadBoolean(ref reader, MetadataName.RequirePushedAuthorizationRequests, ClassName, true);
                    #endregion
                    else
                    {
                        #region case-insensitive
                        // Falling back to checking property names as case insensitive.
                        // JsonPrimitives.Read(...) passes 'true' to advance reader to next token.
                        string propertyName = JsonPrimitives.ReadPropertyName(ref reader, OpenIdConnectConfiguration.ClassName, true);

                        // If the property name is not known add to AdditionalData avoiding unnecessary string compares.
                        if (!OpenIdProviderMetadataNamesUpperCase.Contains(propertyName.ToUpperInvariant()))
                        {
                            config.AdditionalData[propertyName] = JsonPrimitives.ReadPropertyValueAsObject(ref reader, propertyName, OpenIdConnectConfiguration.ClassName);
                        }
                        else
                        {
                            if (propertyName.Equals(MetadataName.AcrValuesSupported, StringComparison.OrdinalIgnoreCase))
                                JsonPrimitives.ReadStrings(ref reader, config.AcrValuesSupported, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.AuthorizationEndpoint, StringComparison.OrdinalIgnoreCase))
                                config.AuthorizationEndpoint = JsonPrimitives.ReadString(ref reader, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.AuthorizationResponseIssParameterSupported, StringComparison.OrdinalIgnoreCase))
                                config.AuthorizationResponseIssParameterSupported = JsonPrimitives.ReadBoolean(ref reader, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.BackchannelAuthenticationEndpoint, StringComparison.OrdinalIgnoreCase))
                                config.BackchannelAuthenticationEndpoint = JsonPrimitives.ReadString(ref reader, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.BackchannelAuthenticationRequestSigningAlgValuesSupported, StringComparison.OrdinalIgnoreCase))
                                JsonPrimitives.ReadStrings(ref reader, config.BackchannelAuthenticationRequestSigningAlgValuesSupported, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.BackchannelTokenDeliveryModesSupported, StringComparison.OrdinalIgnoreCase))
                                JsonPrimitives.ReadStrings(ref reader, config.BackchannelTokenDeliveryModesSupported, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.BackchannelUserCodeParameterSupported, StringComparison.OrdinalIgnoreCase))
                                config.BackchannelUserCodeParameterSupported = JsonPrimitives.ReadBoolean(ref reader, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.CheckSessionIframe, StringComparison.OrdinalIgnoreCase))
                                config.CheckSessionIframe = JsonPrimitives.ReadString(ref reader, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.ClaimsLocalesSupported, StringComparison.OrdinalIgnoreCase))
                                JsonPrimitives.ReadStrings(ref reader, config.ClaimsLocalesSupported, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.ClaimsParameterSupported, StringComparison.OrdinalIgnoreCase))
                                config.ClaimsParameterSupported = JsonPrimitives.ReadBoolean(ref reader, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.ClaimsSupported, StringComparison.OrdinalIgnoreCase))
                                JsonPrimitives.ReadStrings(ref reader, config.ClaimsSupported, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.ClaimTypesSupported, StringComparison.OrdinalIgnoreCase))
                                JsonPrimitives.ReadStrings(ref reader, config.ClaimTypesSupported, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.DisplayValuesSupported, StringComparison.OrdinalIgnoreCase))
                                JsonPrimitives.ReadStrings(ref reader, config.DisplayValuesSupported, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.DPoPSigningAlgValuesSupported, StringComparison.OrdinalIgnoreCase))
                                JsonPrimitives.ReadStrings(ref reader, config.DPoPSigningAlgValuesSupported, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.EndSessionEndpoint, StringComparison.OrdinalIgnoreCase))
                                config.EndSessionEndpoint = JsonPrimitives.ReadString(ref reader, propertyName, ClassName);

                            // FrontchannelLogoutSessionSupported and FrontchannelLogoutSupported are per spec 'boolean'.
                            // We shipped previous versions accepting a string and transforming to a boolean.
                            else if (propertyName.Equals(MetadataName.FrontchannelLogoutSessionSupported, StringComparison.OrdinalIgnoreCase))
                            {
                                if (reader.TokenType == JsonTokenType.True)
                                    config.FrontchannelLogoutSessionSupported = JsonPrimitives.True;
                                else if (reader.TokenType == JsonTokenType.False)
                                    config.FrontchannelLogoutSessionSupported = JsonPrimitives.False;
                                else
                                    config.FrontchannelLogoutSessionSupported = JsonPrimitives.ReadStringAsBool(ref reader, MetadataName.FrontchannelLogoutSessionSupported, ClassName);
                            }
                            else if (propertyName.Equals(MetadataName.FrontchannelLogoutSupported, StringComparison.OrdinalIgnoreCase))
                            {
                                if (reader.TokenType == JsonTokenType.True)
                                    config.FrontchannelLogoutSupported = JsonPrimitives.True;
                                else if (reader.TokenType == JsonTokenType.False)
                                    config.FrontchannelLogoutSupported = JsonPrimitives.False;
                                else
                                    config.FrontchannelLogoutSupported = JsonPrimitives.ReadStringAsBool(ref reader, propertyName, ClassName);
                            }
                            else if (propertyName.Equals(MetadataName.GrantTypesSupported, StringComparison.OrdinalIgnoreCase))
                                JsonPrimitives.ReadStrings(ref reader, config.GrantTypesSupported, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.HttpLogoutSupported, StringComparison.OrdinalIgnoreCase))
                                config.HttpLogoutSupported = JsonPrimitives.ReadBoolean(ref reader, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.IdTokenEncryptionAlgValuesSupported, StringComparison.OrdinalIgnoreCase))
                                JsonPrimitives.ReadStrings(ref reader, config.IdTokenEncryptionAlgValuesSupported, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.IdTokenEncryptionEncValuesSupported, StringComparison.OrdinalIgnoreCase))
                                JsonPrimitives.ReadStrings(ref reader, config.IdTokenEncryptionEncValuesSupported, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.IdTokenSigningAlgValuesSupported , StringComparison.OrdinalIgnoreCase))
                                JsonPrimitives.ReadStrings(ref reader, config.IdTokenSigningAlgValuesSupported, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.IntrospectionEndpoint, StringComparison.OrdinalIgnoreCase))
                                config.IntrospectionEndpoint = JsonPrimitives.ReadString(ref reader, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.IntrospectionEndpointAuthMethodsSupported, StringComparison.OrdinalIgnoreCase))
                                JsonPrimitives.ReadStrings(ref reader, config.IntrospectionEndpointAuthMethodsSupported, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.IntrospectionEndpointAuthSigningAlgValuesSupported, StringComparison.OrdinalIgnoreCase))
                                JsonPrimitives.ReadStrings(ref reader, config.IntrospectionEndpointAuthSigningAlgValuesSupported, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.Issuer, StringComparison.OrdinalIgnoreCase))
                                config.Issuer = JsonPrimitives.ReadString(ref reader, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.JwksUri, StringComparison.OrdinalIgnoreCase))
                                config.JwksUri = JsonPrimitives.ReadString(ref reader, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.LogoutSessionSupported, StringComparison.OrdinalIgnoreCase))
                                config.LogoutSessionSupported = JsonPrimitives.ReadBoolean(ref reader, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.OpPolicyUri, StringComparison.OrdinalIgnoreCase))
                                config.OpPolicyUri = JsonPrimitives.ReadString(ref reader, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.OpTosUri, StringComparison.OrdinalIgnoreCase))
                                config.OpTosUri = JsonPrimitives.ReadString(ref reader, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.PromptValuesSupported, StringComparison.OrdinalIgnoreCase))
                                JsonPrimitives.ReadStrings(ref reader, config.PromptValuesSupported, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.PushedAuthorizationRequestEndpoint, StringComparison.OrdinalIgnoreCase))
                                config.PushedAuthorizationRequestEndpoint = JsonPrimitives.ReadString(ref reader, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.RegistrationEndpoint, StringComparison.OrdinalIgnoreCase))
                                config.RegistrationEndpoint = JsonPrimitives.ReadString(ref reader, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.RequestObjectEncryptionAlgValuesSupported, StringComparison.OrdinalIgnoreCase))
                                JsonPrimitives.ReadStrings(ref reader, config.RequestObjectEncryptionAlgValuesSupported, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.RequestObjectEncryptionEncValuesSupported, StringComparison.OrdinalIgnoreCase))
                                JsonPrimitives.ReadStrings(ref reader, config.RequestObjectEncryptionEncValuesSupported, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.RequestObjectSigningAlgValuesSupported, StringComparison.OrdinalIgnoreCase))
                                JsonPrimitives.ReadStrings(ref reader, config.RequestObjectSigningAlgValuesSupported, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.RequestParameterSupported, StringComparison.OrdinalIgnoreCase))
                                config.RequestParameterSupported = JsonPrimitives.ReadBoolean(ref reader, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.RequestUriParameterSupported, StringComparison.OrdinalIgnoreCase))
                                config.RequestUriParameterSupported = JsonPrimitives.ReadBoolean(ref reader, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.RequirePushedAuthorizationRequests, StringComparison.OrdinalIgnoreCase))
                                config.RequirePushedAuthorizationRequests = JsonPrimitives.ReadBoolean(ref reader, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.RequireRequestUriRegistration, StringComparison.OrdinalIgnoreCase))
                                config.RequireRequestUriRegistration = JsonPrimitives.ReadBoolean(ref reader, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.ResponseModesSupported, StringComparison.OrdinalIgnoreCase))
                                JsonPrimitives.ReadStrings(ref reader, config.ResponseModesSupported, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.ResponseTypesSupported, StringComparison.OrdinalIgnoreCase))
                                JsonPrimitives.ReadStrings(ref reader, config.ResponseTypesSupported, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.ScopesSupported, StringComparison.OrdinalIgnoreCase))
                                JsonPrimitives.ReadStrings(ref reader, config.ScopesSupported, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.ServiceDocumentation, StringComparison.OrdinalIgnoreCase))
                                config.ServiceDocumentation = JsonPrimitives.ReadString(ref reader, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.SubjectTypesSupported, StringComparison.OrdinalIgnoreCase))
                                JsonPrimitives.ReadStrings(ref reader, config.SubjectTypesSupported, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.TokenEndpoint, StringComparison.OrdinalIgnoreCase))
                                config.TokenEndpoint = JsonPrimitives.ReadString(ref reader, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.TokenEndpointAuthMethodsSupported, StringComparison.OrdinalIgnoreCase))
                                JsonPrimitives.ReadStrings(ref reader, config.TokenEndpointAuthMethodsSupported, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.TokenEndpointAuthSigningAlgValuesSupported, StringComparison.OrdinalIgnoreCase))
                                JsonPrimitives.ReadStrings(ref reader, config.TokenEndpointAuthSigningAlgValuesSupported, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.UILocalesSupported, StringComparison.OrdinalIgnoreCase))
                                JsonPrimitives.ReadStrings(ref reader, config.UILocalesSupported, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.UserInfoEncryptionAlgValuesSupported, StringComparison.OrdinalIgnoreCase))
                                JsonPrimitives.ReadStrings(ref reader, config.UserInfoEndpointEncryptionAlgValuesSupported, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.UserInfoEncryptionEncValuesSupported, StringComparison.OrdinalIgnoreCase))
                                JsonPrimitives.ReadStrings(ref reader, config.UserInfoEndpointEncryptionEncValuesSupported, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.UserInfoEndpoint, StringComparison.OrdinalIgnoreCase))
                                config.UserInfoEndpoint = JsonPrimitives.ReadString(ref reader, propertyName, ClassName);

                            else if (propertyName.Equals(MetadataName.UserInfoSigningAlgValuesSupported, StringComparison.OrdinalIgnoreCase))
                                JsonPrimitives.ReadStrings(ref reader, config.UserInfoEndpointSigningAlgValuesSupported, propertyName, ClassName);
                        }
                        #endregion case-insensitive
                    }
                }
                // We read a JsonTokenType.StartObject above, exiting and positioning reader at next token.
                else if (JsonPrimitives.IsReaderAtTokenType(ref reader, JsonTokenType.EndObject, true))
                    break;
                else if (!reader.Read())
                    break;
            }

            return config;
        }
        #endregion

        #region Write
        public static string Write(OpenIdConnectConfiguration OpenIdConnectConfiguration)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Utf8JsonWriter writer = null;
                try
                {
                    writer = new Utf8JsonWriter(memoryStream, new JsonWriterOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
                    Write(ref writer, OpenIdConnectConfiguration);
                    writer.Flush();
                    return Encoding.UTF8.GetString(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                }
                finally
                {
                    writer?.Dispose();
                }
            }
        }

        public static void Write(ref Utf8JsonWriter writer, OpenIdConnectConfiguration config)
        {
            writer.WriteStartObject();

            if (config.AcrValuesSupported.Count > 0)
                JsonPrimitives.WriteStrings(ref writer, Utf8Bytes.AcrValuesSupported, config.AcrValuesSupported);

            if (!string.IsNullOrEmpty(config.AuthorizationEndpoint))
                writer.WriteString(Utf8Bytes.AuthorizationEndpoint, config.AuthorizationEndpoint);

            if (config.AuthorizationResponseIssParameterSupported)
                writer.WriteBoolean(Utf8Bytes.AuthorizationResponseIssParameterSupported, config.AuthorizationResponseIssParameterSupported);

            if (!string.IsNullOrEmpty(config.BackchannelAuthenticationEndpoint))
                writer.WriteString(Utf8Bytes.BackchannelAuthenticationEndpoint, config.BackchannelAuthenticationEndpoint);

            if (config.BackchannelAuthenticationRequestSigningAlgValuesSupported.Count > 0)
                JsonPrimitives.WriteStrings(ref writer, Utf8Bytes.BackchannelAuthenticationRequestSigningAlgValuesSupported, config.BackchannelAuthenticationRequestSigningAlgValuesSupported);

            if (config.BackchannelTokenDeliveryModesSupported.Count > 0)
                JsonPrimitives.WriteStrings(ref writer, Utf8Bytes.BackchannelTokenDeliveryModesSupported, config.BackchannelTokenDeliveryModesSupported);

            if (config.BackchannelUserCodeParameterSupported)
                writer.WriteBoolean(Utf8Bytes.BackchannelUserCodeParameterSupported, config.BackchannelUserCodeParameterSupported);

            if (!string.IsNullOrEmpty(config.CheckSessionIframe))
                writer.WriteString(Utf8Bytes.CheckSessionIframe, config.CheckSessionIframe);

            if (config.ClaimsSupported.Count > 0)
                JsonPrimitives.WriteStrings(ref writer, Utf8Bytes.ClaimsSupported, config.ClaimsSupported);

            if (config.ClaimsLocalesSupported.Count > 0)
                JsonPrimitives.WriteStrings(ref writer, Utf8Bytes.ClaimsLocalesSupported, config.ClaimsLocalesSupported);

            if (config.ClaimsParameterSupported)
                writer.WriteBoolean(Utf8Bytes.ClaimsParameterSupported, config.ClaimsParameterSupported);

            if (config.ClaimTypesSupported.Count > 0)
                JsonPrimitives.WriteStrings(ref writer, Utf8Bytes.ClaimTypesSupported, config.ClaimTypesSupported);

            if (config.DisplayValuesSupported.Count > 0)
                JsonPrimitives.WriteStrings(ref writer, Utf8Bytes.DisplayValuesSupported, config.DisplayValuesSupported);

            if (config.DPoPSigningAlgValuesSupported.Count > 0)
                JsonPrimitives.WriteStrings(ref writer, Utf8Bytes.DPoPSigningAlgValuesSupported, config.DPoPSigningAlgValuesSupported);

            if (!string.IsNullOrEmpty(config.EndSessionEndpoint))
                writer.WriteString(Utf8Bytes.EndSessionEndpoint, config.EndSessionEndpoint);

            if (!string.IsNullOrEmpty(config.FrontchannelLogoutSessionSupported))
                writer.WriteString(Utf8Bytes.FrontchannelLogoutSessionSupported, config.FrontchannelLogoutSessionSupported);

            if (!string.IsNullOrEmpty(config.FrontchannelLogoutSupported))
                writer.WriteString(Utf8Bytes.FrontchannelLogoutSupported, config.FrontchannelLogoutSupported);

            if (config.GrantTypesSupported.Count > 0)
                JsonPrimitives.WriteStrings(ref writer, Utf8Bytes.GrantTypesSupported, config.GrantTypesSupported);

            if (config.HttpLogoutSupported)
                writer.WriteBoolean(Utf8Bytes.HttpLogoutSupported, config.HttpLogoutSupported);

            if (config.IdTokenEncryptionAlgValuesSupported.Count > 0)
                JsonPrimitives.WriteStrings(ref writer, Utf8Bytes.IdTokenEncryptionAlgValuesSupported, config.IdTokenEncryptionAlgValuesSupported);

            if (config.IdTokenEncryptionEncValuesSupported.Count > 0)
                JsonPrimitives.WriteStrings(ref writer, Utf8Bytes.IdTokenEncryptionEncValuesSupported, config.IdTokenEncryptionEncValuesSupported);

            if (config.IdTokenSigningAlgValuesSupported.Count > 0)
                JsonPrimitives.WriteStrings(ref writer, Utf8Bytes.IdTokenSigningAlgValuesSupported, config.IdTokenSigningAlgValuesSupported);

            if (!string.IsNullOrEmpty(config.IntrospectionEndpoint))
                writer.WriteString(Utf8Bytes.IntrospectionEndpoint, config.IntrospectionEndpoint);

            if (config.IntrospectionEndpointAuthMethodsSupported.Count > 0)
                JsonPrimitives.WriteStrings(ref writer, Utf8Bytes.IntrospectionEndpointAuthMethodsSupported, config.IntrospectionEndpointAuthMethodsSupported);

            if (config.IntrospectionEndpointAuthSigningAlgValuesSupported.Count > 0)
                JsonPrimitives.WriteStrings(ref writer, Utf8Bytes.IntrospectionEndpointAuthSigningAlgValuesSupported, config.IntrospectionEndpointAuthSigningAlgValuesSupported);

            if (!string.IsNullOrEmpty(config.Issuer))
                writer.WriteString(Utf8Bytes.Issuer, config.Issuer);

            if (!string.IsNullOrEmpty(config.JwksUri))
                writer.WriteString(Utf8Bytes.JwksUri, config.JwksUri);

            if (config.LogoutSessionSupported)
                writer.WriteBoolean(Utf8Bytes.LogoutSessionSupported, config.LogoutSessionSupported);

            if (!string.IsNullOrEmpty(config.OpPolicyUri))
                writer.WriteString(Utf8Bytes.OpPolicyUri, config.OpPolicyUri);

            if (!string.IsNullOrEmpty(config.OpTosUri))
                writer.WriteString(Utf8Bytes.OpTosUri, config.OpTosUri);

            if (config.PromptValuesSupported.Count > 0)
                JsonPrimitives.WriteStrings(ref writer, Utf8Bytes.PromptValuesSupported, config.PromptValuesSupported);

            if (!string.IsNullOrEmpty(config.PushedAuthorizationRequestEndpoint))
                writer.WriteString(Utf8Bytes.PushedAuthorizationRequestEndpoint, config.PushedAuthorizationRequestEndpoint);

            if (!string.IsNullOrEmpty(config.RegistrationEndpoint))
                 writer.WriteString(Utf8Bytes.RegistrationEndpoint, config.RegistrationEndpoint);

            if (config.RequestObjectEncryptionAlgValuesSupported.Count > 0)
                JsonPrimitives.WriteStrings(ref writer, Utf8Bytes.RequestObjectEncryptionAlgValuesSupported, config.RequestObjectEncryptionAlgValuesSupported);

            if (config.RequestObjectEncryptionEncValuesSupported.Count > 0)
                JsonPrimitives.WriteStrings(ref writer, Utf8Bytes.RequestObjectEncryptionEncValuesSupported, config.RequestObjectEncryptionEncValuesSupported);

            if (config.RequestObjectSigningAlgValuesSupported.Count > 0)
                JsonPrimitives.WriteStrings(ref writer, Utf8Bytes.RequestObjectSigningAlgValuesSupported, config.RequestObjectSigningAlgValuesSupported);

            if (config.RequestParameterSupported)
                writer.WriteBoolean(Utf8Bytes.RequestParameterSupported, config.RequestParameterSupported);

            if (config.RequestUriParameterSupported)
                writer.WriteBoolean(Utf8Bytes.RequestUriParameterSupported, config.RequestUriParameterSupported);

            if (config.RequirePushedAuthorizationRequests)
                writer.WriteBoolean(Utf8Bytes.RequirePushedAuthorizationRequests, config.RequirePushedAuthorizationRequests);

            if (config.RequireRequestUriRegistration)
                writer.WriteBoolean(Utf8Bytes.RequireRequestUriRegistration, config.RequireRequestUriRegistration);

            if (config.ResponseModesSupported.Count > 0)
                JsonPrimitives.WriteStrings(ref writer, Utf8Bytes.ResponseModesSupported, config.ResponseModesSupported);

            if (config.ResponseTypesSupported.Count > 0)
                JsonPrimitives.WriteStrings(ref writer, Utf8Bytes.ResponseTypesSupported, config.ResponseTypesSupported);

            if (config.ScopesSupported.Count > 0)
                JsonPrimitives.WriteStrings(ref writer, Utf8Bytes.ScopesSupported, config.ScopesSupported);

            if (!string.IsNullOrEmpty(config.ServiceDocumentation))
                writer.WriteString(Utf8Bytes.ServiceDocumentation, config.ServiceDocumentation);

            if (config.SubjectTypesSupported.Count > 0)
                JsonPrimitives.WriteStrings(ref writer, Utf8Bytes.SubjectTypesSupported, config.SubjectTypesSupported);

            if (!string.IsNullOrEmpty(config.TokenEndpoint))
                writer.WriteString(Utf8Bytes.TokenEndpoint, config.TokenEndpoint);

            if (config.TokenEndpointAuthMethodsSupported.Count > 0)
                JsonPrimitives.WriteStrings(ref writer, Utf8Bytes.TokenEndpointAuthMethodsSupported, config.TokenEndpointAuthMethodsSupported);

            if (config.TokenEndpointAuthSigningAlgValuesSupported.Count > 0)
                JsonPrimitives.WriteStrings(ref writer, Utf8Bytes.TokenEndpointAuthSigningAlgValuesSupported, config.TokenEndpointAuthSigningAlgValuesSupported);

            if (config.UILocalesSupported.Count > 0)
                JsonPrimitives.WriteStrings(ref writer, Utf8Bytes.UILocalesSupported, config.UILocalesSupported);

            if (!string.IsNullOrEmpty(config.UserInfoEndpoint))
                writer.WriteString(Utf8Bytes.UserInfoEndpoint, config.UserInfoEndpoint);

            if (config.UserInfoEndpointEncryptionAlgValuesSupported.Count > 0)
                JsonPrimitives.WriteStrings(ref writer, Utf8Bytes.UserInfoEncryptionAlgValuesSupported, config.UserInfoEndpointEncryptionAlgValuesSupported);

            if (config.UserInfoEndpointEncryptionEncValuesSupported.Count > 0)
                JsonPrimitives.WriteStrings(ref writer, Utf8Bytes.UserInfoEncryptionEncValuesSupported, config.UserInfoEndpointEncryptionEncValuesSupported);

            if (config.UserInfoEndpointSigningAlgValuesSupported.Count > 0)
                JsonPrimitives.WriteStrings(ref writer, Utf8Bytes.UserInfoSigningAlgValuesSupported, config.UserInfoEndpointSigningAlgValuesSupported);

            if (config.AdditionalData.Count > 0)
                JsonPrimitives.WriteObjects(ref writer, config.AdditionalData);

            writer.WriteEndObject();
        }
        #endregion
    }
}

