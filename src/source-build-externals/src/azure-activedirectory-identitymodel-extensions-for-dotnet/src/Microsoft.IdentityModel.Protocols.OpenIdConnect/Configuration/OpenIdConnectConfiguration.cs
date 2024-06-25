// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Threading;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect
{
    /// <summary>
    /// Contains OpenIdConnect configuration that can be populated from a json string.
    /// </summary>
    public class OpenIdConnectConfiguration : BaseConfiguration
    {
        internal const string ClassName = "Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectConfiguration";

        // these are used to lazy create
        private Dictionary<string, object> _additionalData;
        private ICollection<string> _acrValuesSupported;
        private ICollection<string> _authorizationEncryptionAlgValuesSupported;
        private ICollection<string> _authorizationEncryptionEncValuesSupported;
        private ICollection<string> _authorizationSigningAlgValuesSupported;
        private ICollection<string> _backchannelAuthenticationRequestSigningAlgValuesSupported;
        private ICollection<string> _backchannelTokenDeliveryModesSupported;
        private ICollection<string> _claimsSupported;
        private ICollection<string> _claimsLocalesSupported;
        private ICollection<string> _claimTypesSupported;
        private ICollection<string> _codeChallengeMethodsSupported;
        private ICollection<string> _displayValuesSupported;
        private ICollection<string> _dPoPSigningAlgValuesSupported;
        private ICollection<string> _grantTypesSupported;
        private ICollection<string> _idTokenEncryptionAlgValuesSupported;
        private ICollection<string> _idTokenEncryptionEncValuesSupported;
        private ICollection<string> _idTokenSigningAlgValuesSupported;
        private ICollection<string> _introspectionEndpointAuthMethodsSupported;
        private ICollection<string> _introspectionEndpointAuthSigningAlgValuesSupported;
        private ICollection<string> _promptValuesSupported;
        private ICollection<string> _requestObjectEncryptionAlgValuesSupported;
        private ICollection<string> _requestObjectEncryptionEncValuesSupported;
        private ICollection<string> _requestObjectSigningAlgValuesSupported;
        private ICollection<string> _responseModesSupported;
        private ICollection<string> _responseTypesSupported;
        private ICollection<string> _revocationEndpointAuthMethodsSupported;
        private ICollection<string> _revocationEndpointAuthSigningAlgValuesSupported;
        private ICollection<string> _scopesSupported;
        private ICollection<string> _subjectTypesSupported;
        private ICollection<string> _tokenEndpointAuthMethodsSupported;
        private ICollection<string> _tokenEndpointAuthSigningAlgValuesSupported;
        private ICollection<string> _uILocalesSupported;
        private ICollection<string> _userInfoEndpointEncryptionAlgValuesSupported;
        private ICollection<string> _userInfoEndpointEncryptionEncValuesSupported;
        private ICollection<string> _userInfoEndpointSigningAlgValuesSupported;

        /// <summary>
        /// Deserializes the json string into an <see cref="OpenIdConnectConfiguration"/> object.
        /// </summary>
        /// <param name="json">json string representing the configuration.</param>
        /// <returns><see cref="OpenIdConnectConfiguration"/> object representing the configuration.</returns>
        /// <exception cref="ArgumentNullException">If 'json' is null or empty.</exception>
        /// <exception cref="ArgumentException">If 'json' fails to deserialize.</exception>
        public static OpenIdConnectConfiguration Create(string json)
        {
            if (string.IsNullOrEmpty(json))
                throw LogHelper.LogArgumentNullException(nameof(json));

            if (LogHelper.IsEnabled(EventLogLevel.Verbose))
                LogHelper.LogVerbose(LogMessages.IDX21808, json);

            return new OpenIdConnectConfiguration(json);
        }

        /// <summary>
        /// Serializes the <see cref="OpenIdConnectConfiguration"/> object to a json string.
        /// </summary>
        /// <param name="configuration"><see cref="OpenIdConnectConfiguration"/> object to serialize.</param>
        /// <returns>json string representing the configuration object.</returns>
        /// <exception cref="ArgumentNullException">If 'configuration' is null.</exception>
        public static string Write(OpenIdConnectConfiguration configuration)
        {
            if (configuration == null)
                throw LogHelper.LogArgumentNullException(nameof(configuration));

            if (LogHelper.IsEnabled(EventLogLevel.Verbose))
                LogHelper.LogVerbose(LogMessages.IDX21809);

            return OpenIdConnectConfigurationSerializer.Write(configuration);
        }

        /// <summary>
        /// Initializes an new instance of <see cref="OpenIdConnectConfiguration"/>.
        /// </summary>
        public OpenIdConnectConfiguration()
        {
        }

        /// <summary>
        /// Initializes an new instance of <see cref="OpenIdConnectConfiguration"/> from a json string.
        /// </summary>
        /// <param name="json">a json string containing the metadata</param>
        /// <exception cref="ArgumentNullException">If 'json' is null or empty.</exception>
        public OpenIdConnectConfiguration(string json)
        {
            if (string.IsNullOrEmpty(json))
                throw LogHelper.LogArgumentNullException(nameof(json));

            try
            {
                if (LogHelper.IsEnabled(EventLogLevel.Verbose))
                    LogHelper.LogVerbose(LogMessages.IDX21806, json, LogHelper.MarkAsNonPII(ClassName));

                OpenIdConnectConfigurationSerializer.Read(json, this);
            }
            catch (Exception ex)
            {
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX21815, json, LogHelper.MarkAsNonPII(ClassName)), ex));
            }
        }

        /// <summary>
        /// When deserializing from JSON any properties that are not defined will be placed here.
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, object> AdditionalData =>
            _additionalData ??
            Interlocked.CompareExchange(ref _additionalData, new Dictionary<string, object>(StringComparer.Ordinal), null) ??
            _additionalData;

        /// <summary>
        /// Gets the collection of 'acr_values_supported'
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.AcrValuesSupported)]
        public ICollection<string> AcrValuesSupported =>
            _acrValuesSupported ??
            Interlocked.CompareExchange(ref _acrValuesSupported, new Collection<string>(), null) ??
            _acrValuesSupported;

        /// <summary>
        /// Gets or sets the 'authorization_endpoint'.
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.AuthorizationEndpoint)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public string AuthorizationEndpoint { get; set; }

        /// <summary>
        /// Gets the collection of 'authorization_encryption_alg_values_supported'
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.AuthorizationEncryptionAlgValuesSupported)]
        public ICollection<string> AuthorizationEncryptionAlgValuesSupported =>
            _authorizationEncryptionAlgValuesSupported ??
            Interlocked.CompareExchange(ref _authorizationEncryptionAlgValuesSupported, new Collection<string>(), null) ??
            _authorizationEncryptionAlgValuesSupported;

        /// <summary>
        /// Gets the collection of 'authorization_encryption_enc_values_supported'
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.AuthorizationEncryptionEncValuesSupported)]
        public ICollection<string> AuthorizationEncryptionEncValuesSupported =>
            _authorizationEncryptionEncValuesSupported ??
            Interlocked.CompareExchange(ref _authorizationEncryptionEncValuesSupported, new Collection<string>(), null) ??
            _authorizationEncryptionEncValuesSupported;

        /// <summary>
        /// Gets or sets the 'authorization_response_iss_parameter_supported'
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.AuthorizationResponseIssParameterSupported)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
#endif
        public bool AuthorizationResponseIssParameterSupported { get; set; }

        /// <summary>
        /// Gets the collection of 'authorization_signing_alg_values_supported'
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.AuthorizationSigningAlgValuesSupported)]
        public ICollection<string> AuthorizationSigningAlgValuesSupported =>
            _authorizationSigningAlgValuesSupported ??
            Interlocked.CompareExchange(ref _authorizationSigningAlgValuesSupported, new Collection<string>(), null) ??
            _authorizationSigningAlgValuesSupported;

        /// <summary>
        /// Gets or sets the 'backchannel_authentication_endpoint'.
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.BackchannelAuthenticationEndpoint)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public string BackchannelAuthenticationEndpoint { get; set; }

        /// <summary>
        /// Gets the collection of 'backchannel_authentication_request_signing_alg_values_supported'
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.BackchannelAuthenticationRequestSigningAlgValuesSupported)]
        public ICollection<string> BackchannelAuthenticationRequestSigningAlgValuesSupported =>
            _backchannelAuthenticationRequestSigningAlgValuesSupported ??
            Interlocked.CompareExchange(ref _backchannelAuthenticationRequestSigningAlgValuesSupported, new Collection<string>(), null) ??
            _backchannelAuthenticationRequestSigningAlgValuesSupported;

        /// <summary>
        /// Gets the collection of 'backchannel_token_delivery_modes_supported'
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.BackchannelTokenDeliveryModesSupported)]
        public ICollection<string> BackchannelTokenDeliveryModesSupported =>
            _backchannelTokenDeliveryModesSupported ??
            Interlocked.CompareExchange(ref _backchannelTokenDeliveryModesSupported, new Collection<string>(), null) ??
            _backchannelTokenDeliveryModesSupported;

        /// <summary>
        /// Gets or sets the 'backchannel_user_code_parameter_supported'
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.BackchannelUserCodeParameterSupported)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
#endif
        public bool BackchannelUserCodeParameterSupported { get; set; }

        /// <summary>
        /// Gets or sets the 'check_session_iframe'.
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.CheckSessionIframe)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public string CheckSessionIframe { get; set; }

        /// <summary>
        /// Gets the collection of 'claims_supported'
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.ClaimsSupported)]
        public ICollection<string> ClaimsSupported =>
            _claimsSupported ??
            Interlocked.CompareExchange(ref _claimsSupported, new Collection<string>(), null) ??
            _claimsSupported;

        /// <summary>
        /// Gets the collection of 'claims_locales_supported'
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.ClaimsLocalesSupported)]
        public ICollection<string> ClaimsLocalesSupported =>
            _claimsLocalesSupported ??
            Interlocked.CompareExchange(ref _claimsLocalesSupported, new Collection<string>(), null) ??
            _claimsLocalesSupported;

        /// <summary>
        /// Gets or sets the 'claims_parameter_supported'
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.ClaimsParameterSupported)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
#endif
        public bool ClaimsParameterSupported { get; set; }

        /// <summary>
        /// Gets the collection of 'claim_types_supported'
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.ClaimTypesSupported)]
        public ICollection<string> ClaimTypesSupported =>
            _claimTypesSupported ??
            Interlocked.CompareExchange(ref _claimTypesSupported, new Collection<string>(), null) ??
            _claimTypesSupported;

        /// <summary>
        /// Gets the collection of 'code_challenge_methods_supported'
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.CodeChallengeMethodsSupported)]
        public ICollection<string> CodeChallengeMethodsSupported =>
            _codeChallengeMethodsSupported ??
            Interlocked.CompareExchange(ref _codeChallengeMethodsSupported, new Collection<string>(), null) ??
            _codeChallengeMethodsSupported;

        /// <summary>
        /// Gets or sets the 'device_authorization_endpoint'.
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.DeviceAuthorizationEndpoint)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public string DeviceAuthorizationEndpoint { get; set; }

        /// <summary>
        /// Gets the collection of 'display_values_supported'
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.DisplayValuesSupported)]
        public ICollection<string> DisplayValuesSupported =>
            _displayValuesSupported ??
            Interlocked.CompareExchange(ref _displayValuesSupported, new Collection<string>(), null) ??
            _displayValuesSupported;

        /// <summary>
        /// Gets the collection of 'dpop_signing_alg_values_supported'
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.DPoPSigningAlgValuesSupported)]
        public ICollection<string> DPoPSigningAlgValuesSupported =>
            _dPoPSigningAlgValuesSupported ??
            Interlocked.CompareExchange(ref _dPoPSigningAlgValuesSupported, new Collection<string>(), null) ??
            _dPoPSigningAlgValuesSupported;

        /// <summary>
        /// Gets or sets the 'end_session_endpoint'.
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.EndSessionEndpoint)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public string EndSessionEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the 'frontchannel_logout_session_supported'.
        /// </summary>
        /// <remarks>Would be breaking to change, in 6x it was string, spec says bool.
        /// TODO - add another property, obsolete and drop in 8x?
        /// see: https://openid.net/specs/openid-connect-frontchannel-1_0.html
        /// </remarks>
        [JsonPropertyName(OpenIdProviderMetadataNames.FrontchannelLogoutSessionSupported)]
        public string FrontchannelLogoutSessionSupported { get; set; }

        /// <summary>
        /// Gets or sets the 'frontchannel_logout_supported'.
        /// </summary>
        /// <remarks>Would be breaking to change, in 6x it was string, spec says bool.
        /// TODO - add another property, obsolete and drop in 8x?
        /// see: https://openid.net/specs/openid-connect-frontchannel-1_0.html
        /// </remarks>
        [JsonPropertyName(OpenIdProviderMetadataNames.FrontchannelLogoutSupported)]
        public string FrontchannelLogoutSupported { get; set; }

        /// <summary>
        /// Gets the collection of 'grant_types_supported'
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.GrantTypesSupported)]
        public ICollection<string> GrantTypesSupported =>
            _grantTypesSupported ??
            Interlocked.CompareExchange(ref _grantTypesSupported, new Collection<string>(), null) ??
            _grantTypesSupported;

        /// <summary>
        /// Boolean value specifying whether the OP supports HTTP-based logout. Default is false.
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.HttpLogoutSupported)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
#endif
        public bool HttpLogoutSupported { get; set; }

        /// <summary>
        /// Gets the collection of 'id_token_encryption_alg_values_supported'.
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.IdTokenEncryptionAlgValuesSupported)]
        public ICollection<string> IdTokenEncryptionAlgValuesSupported =>
            _idTokenEncryptionAlgValuesSupported ??
            Interlocked.CompareExchange(ref _idTokenEncryptionAlgValuesSupported, new Collection<string>(), null) ??
            _idTokenEncryptionAlgValuesSupported;

        /// <summary>
        /// Gets the collection of 'id_token_encryption_enc_values_supported'.
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.IdTokenEncryptionEncValuesSupported)]
        public ICollection<string> IdTokenEncryptionEncValuesSupported =>
            _idTokenEncryptionEncValuesSupported ??
            Interlocked.CompareExchange(ref _idTokenEncryptionEncValuesSupported, new Collection<string>(), null) ??
            _idTokenEncryptionEncValuesSupported;

        /// <summary>
        /// Gets the collection of 'id_token_signing_alg_values_supported'.
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.IdTokenSigningAlgValuesSupported)]
        public ICollection<string> IdTokenSigningAlgValuesSupported =>
            _idTokenSigningAlgValuesSupported ??
            Interlocked.CompareExchange(ref _idTokenSigningAlgValuesSupported, new Collection<string>(), null) ??
            _idTokenSigningAlgValuesSupported;

        /// <summary>
        /// Gets or sets the 'introspection_endpoint'.
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.IntrospectionEndpoint)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public string IntrospectionEndpoint { get; set; }

        /// <summary>
        /// Gets the collection of 'introspection_endpoint_auth_methods_supported'.
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.IntrospectionEndpointAuthMethodsSupported)]
        public ICollection<string> IntrospectionEndpointAuthMethodsSupported =>
            _introspectionEndpointAuthMethodsSupported ??
            Interlocked.CompareExchange(ref _introspectionEndpointAuthMethodsSupported, new Collection<string>(), null) ??
            _introspectionEndpointAuthMethodsSupported;

        /// <summary>
        /// Gets the collection of 'introspection_endpoint_auth_signing_alg_values_supported'.
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.IntrospectionEndpointAuthSigningAlgValuesSupported)]
        public ICollection<string> IntrospectionEndpointAuthSigningAlgValuesSupported =>
            _introspectionEndpointAuthSigningAlgValuesSupported ??
            Interlocked.CompareExchange(ref _introspectionEndpointAuthSigningAlgValuesSupported, new Collection<string>(), null) ??
            _introspectionEndpointAuthSigningAlgValuesSupported;

        /// <summary>
        /// Gets or sets the 'issuer'.
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.Issuer)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public override string Issuer { get; set; }

        /// <summary>
        /// Gets or sets the 'jwks_uri'
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.JwksUri)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public string JwksUri { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="JsonWebKeySet"/>
        /// </summary>
        [JsonIgnore]
        public JsonWebKeySet JsonWebKeySet { get; set; }

        /// <summary>
        /// Boolean value specifying whether the OP can pass a sid (session ID) query parameter to identify the RP session at the OP when the logout_uri is used. Dafault Value is false.
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.LogoutSessionSupported)]
        public bool LogoutSessionSupported { get; set; }

        /// <summary>
        /// Gets or sets the 'op_policy_uri'
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.OpPolicyUri)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public string OpPolicyUri { get; set; }

        /// <summary>
        /// Gets or sets the 'op_tos_uri'
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.OpTosUri)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public string OpTosUri { get; set; }

        /// <summary>
        /// Gets the collection of 'prompt_values_supported'
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.PromptValuesSupported)]
        public ICollection<string> PromptValuesSupported =>
            _promptValuesSupported ??
            Interlocked.CompareExchange(ref _promptValuesSupported, new Collection<string>(), null) ??
            _promptValuesSupported;

        /// <summary>
        /// Gets or sets the 'pushed_authorization_request_endpoint'.
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.PushedAuthorizationRequestEndpoint)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public string PushedAuthorizationRequestEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the 'registration_endpoint'
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.RegistrationEndpoint)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public string RegistrationEndpoint { get; set; }

        /// <summary>
        /// Gets the collection of 'request_object_encryption_alg_values_supported'.
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.RequestObjectEncryptionAlgValuesSupported)]
        public ICollection<string> RequestObjectEncryptionAlgValuesSupported =>
            _requestObjectEncryptionAlgValuesSupported ??
            Interlocked.CompareExchange(ref _requestObjectEncryptionAlgValuesSupported, new Collection<string>(), null) ??
            _requestObjectEncryptionAlgValuesSupported;

        /// <summary>
        /// Gets the collection of 'request_object_encryption_enc_values_supported'.
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.RequestObjectEncryptionEncValuesSupported)]
        public ICollection<string> RequestObjectEncryptionEncValuesSupported =>
            _requestObjectEncryptionEncValuesSupported ??
            Interlocked.CompareExchange(ref _requestObjectEncryptionEncValuesSupported, new Collection<string>(), null) ??
            _requestObjectEncryptionEncValuesSupported;

        /// <summary>
        /// Gets the collection of 'request_object_signing_alg_values_supported'.
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.RequestObjectSigningAlgValuesSupported)]
        public ICollection<string> RequestObjectSigningAlgValuesSupported =>
            _requestObjectSigningAlgValuesSupported ??
            Interlocked.CompareExchange(ref _requestObjectSigningAlgValuesSupported, new Collection<string>(), null) ??
            _requestObjectSigningAlgValuesSupported;

        /// <summary>
        /// Gets or sets the 'request_parameter_supported'
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.RequestParameterSupported)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
#endif
        public bool RequestParameterSupported { get; set; }

        /// <summary>
        /// Gets or sets the 'request_uri_parameter_supported'
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.RequestUriParameterSupported)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
#endif
        public bool RequestUriParameterSupported { get; set; }

        /// <summary>
        /// Gets or sets the 'require_pushed_authorization_requests'
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.RequirePushedAuthorizationRequests)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
#endif
        public bool RequirePushedAuthorizationRequests { get; set; }

        /// <summary>
        /// Gets or sets the 'require_request_uri_registration'
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.RequireRequestUriRegistration)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
#endif
        public bool RequireRequestUriRegistration { get; set; }

        /// <summary>
        /// Gets the collection of 'response_modes_supported'.
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.ResponseModesSupported)]
        public ICollection<string> ResponseModesSupported =>
            _responseModesSupported ??
            Interlocked.CompareExchange(ref _responseModesSupported, new Collection<string>(), null) ??
            _responseModesSupported;

        /// <summary>
        /// Gets the collection of 'response_types_supported'.
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.ResponseTypesSupported)]
        public ICollection<string> ResponseTypesSupported =>
            _responseTypesSupported ??
            Interlocked.CompareExchange(ref _responseTypesSupported, new Collection<string>(), null) ??
            _responseTypesSupported;

        /// <summary>
        /// Gets or sets the 'revocation_endpoint'
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.RevocationEndpoint)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public string RevocationEndpoint { get; set; }

        /// <summary>
        /// Gets the collection of 'revocation_endpoint_auth_methods_supported'.
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.RevocationEndpointAuthMethodsSupported)]
        public ICollection<string> RevocationEndpointAuthMethodsSupported =>
            _revocationEndpointAuthMethodsSupported ??
            Interlocked.CompareExchange(ref _revocationEndpointAuthMethodsSupported, new Collection<string>(), null) ??
            _revocationEndpointAuthMethodsSupported;

        /// <summary>
        /// Gets the collection of 'revocation_endpoint_auth_signing_alg_values_supported'.
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.RevocationEndpointAuthSigningAlgValuesSupported)]
        public ICollection<string> RevocationEndpointAuthSigningAlgValuesSupported =>
            _revocationEndpointAuthSigningAlgValuesSupported ??
            Interlocked.CompareExchange(ref _revocationEndpointAuthSigningAlgValuesSupported, new Collection<string>(), null) ??
            _revocationEndpointAuthSigningAlgValuesSupported;

        /// <summary>
        /// Gets or sets the 'service_documentation'
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.ServiceDocumentation)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public string ServiceDocumentation { get; set; }

        /// <summary>
        /// Gets the collection of 'scopes_supported'
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.ScopesSupported)]
        public ICollection<string> ScopesSupported =>
            _scopesSupported ??
            Interlocked.CompareExchange(ref _scopesSupported, new Collection<string>(), null) ??
            _scopesSupported;

        /// <summary>
        /// Gets the <see cref="ICollection{SecurityKey}"/> that the IdentityProvider indicates are to be used signing tokens.
        /// </summary>
        [JsonIgnore]
        public override ICollection<SecurityKey> SigningKeys { get; } = new Collection<SecurityKey>();

        /// <summary>
        /// Gets the collection of 'subject_types_supported'.
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.SubjectTypesSupported)]
        public ICollection<string> SubjectTypesSupported =>
            _subjectTypesSupported ??
            Interlocked.CompareExchange(ref _subjectTypesSupported, new Collection<string>(), null) ??
            _subjectTypesSupported;

        /// <summary>
        /// Gets or sets the 'token_endpoint'.
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.TokenEndpoint)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public override string TokenEndpoint { get; set; }

        /// <summary>
        /// This base class property is not used in OpenIdConnect.
        /// </summary>
        [JsonIgnore]
        public override string ActiveTokenEndpoint { get; set; }

        /// <summary>
        /// Gets the collection of 'token_endpoint_auth_methods_supported'.
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.TokenEndpointAuthMethodsSupported)]
        public ICollection<string> TokenEndpointAuthMethodsSupported =>
            _tokenEndpointAuthMethodsSupported ??
            Interlocked.CompareExchange(ref _tokenEndpointAuthMethodsSupported, new Collection<string>(), null) ??
            _tokenEndpointAuthMethodsSupported;

        /// <summary>
        /// Gets the collection of 'token_endpoint_auth_signing_alg_values_supported'.
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.TokenEndpointAuthSigningAlgValuesSupported)]
        public ICollection<string> TokenEndpointAuthSigningAlgValuesSupported =>
            _tokenEndpointAuthSigningAlgValuesSupported ??
            Interlocked.CompareExchange(ref _tokenEndpointAuthSigningAlgValuesSupported, new Collection<string>(), null) ??
            _tokenEndpointAuthSigningAlgValuesSupported;

        /// <summary>
        /// Gets or sets the 'tls_client_certificate_bound_access_tokens'
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.TlsClientCertificateBoundAccessTokens)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
#endif
        public bool TlsClientCertificateBoundAccessTokens { get; set; }

        /// <summary>
        /// Gets the collection of 'ui_locales_supported'
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.UILocalesSupported)]
        public ICollection<string> UILocalesSupported =>
            _uILocalesSupported ??
            Interlocked.CompareExchange(ref _uILocalesSupported, new Collection<string>(), null) ??
            _uILocalesSupported;

        /// <summary>
        /// Gets or sets the 'user_info_endpoint'.
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.UserInfoEndpoint)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public string UserInfoEndpoint { get; set; }

        /// <summary>
        /// Gets the collection of 'userinfo_encryption_alg_values_supported'
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.UserInfoEncryptionAlgValuesSupported)]
        public ICollection<string> UserInfoEndpointEncryptionAlgValuesSupported =>
            _userInfoEndpointEncryptionAlgValuesSupported ??
            Interlocked.CompareExchange(ref _userInfoEndpointEncryptionAlgValuesSupported, new Collection<string>(), null) ??
            _userInfoEndpointEncryptionAlgValuesSupported;

        /// <summary>
        /// Gets the collection of 'userinfo_encryption_enc_values_supported'
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.UserInfoEncryptionEncValuesSupported)]
        public ICollection<string> UserInfoEndpointEncryptionEncValuesSupported =>
            _userInfoEndpointEncryptionEncValuesSupported ??
            Interlocked.CompareExchange(ref _userInfoEndpointEncryptionEncValuesSupported, new Collection<string>(), null) ??
            _userInfoEndpointEncryptionEncValuesSupported;

        /// <summary>
        /// Gets the collection of 'userinfo_signing_alg_values_supported'
        /// </summary>
        [JsonPropertyName(OpenIdProviderMetadataNames.UserInfoSigningAlgValuesSupported)]
        public ICollection<string> UserInfoEndpointSigningAlgValuesSupported =>
            _userInfoEndpointSigningAlgValuesSupported ??
            Interlocked.CompareExchange(ref _userInfoEndpointSigningAlgValuesSupported, new Collection<string>(), null) ??
            _userInfoEndpointSigningAlgValuesSupported;

        #region shouldserialize
        // TODO - should we keep these, they were used by Newtonsoft to control serialization of collections.
        // May help users to keep them hanging around.
        /// <summary>
        /// Gets a bool that determines if the 'acr_values_supported' (AcrValuesSupported) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'acr_values_supported' (AcrValuesSupported) is not empty; otherwise, false.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeAcrValuesSupported()
        {
            return AcrValuesSupported.Count > 0;
        }

        /// <summary>
        /// Gets a bool that determines if the 'authorization_encryption_alg_values_supported' (AuthorizationEncryptionAlgValuesSupported) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'authorization_encryption_alg_values_supported' (AuthorizationEncryptionAlgValuesSupported) is not empty; otherwise, false.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeAuthorizationEncryptionAlgValuesSupported()
        {
            return AuthorizationEncryptionAlgValuesSupported.Count > 0;
        }

        /// <summary>
        /// Gets a bool that determines if the 'authorization_encryption_enc_values_supported' (AuthorizationEncryptionEncValuesSupported) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'authorization_encryption_enc_values_supported' (AuthorizationEncryptionEncValuesSupported) is not empty; otherwise, false.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeAuthorizationEncryptionEncValuesSupported()
        {
            return AuthorizationEncryptionEncValuesSupported.Count > 0;
        }

        /// <summary>
        /// Gets a bool that determines if the 'authorization_signing_alg_values_supported' (AuthorizationSigningAlgValuesSupported) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'authorization_signing_alg_values_supported' (AuthorizationSigningAlgValuesSupported) is not empty; otherwise, false.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeAuthorizationSigningAlgValuesSupported()
        {
            return AuthorizationSigningAlgValuesSupported.Count > 0;
        }

        /// <summary>
        /// Gets a bool that determines if the 'backchannel_token_delivery_modes_supported' (BackchannelTokenDeliveryModesSupported) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'backchannel_token_delivery_modes_supported' (BackchannelTokenDeliveryModesSupported) is not empty; otherwise, false.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeBackchannelTokenDeliveryModesSupported()
        {
            return BackchannelTokenDeliveryModesSupported.Count > 0;
        }

        /// <summary>
        /// Gets a bool that determines if the 'backchannel_authentication_request_signing_alg_values_supported' (BackchannelAuthenticationRequestSigningAlgValuesSupported) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'backchannel_authentication_request_signing_alg_values_supported' (BackchannelAuthenticationRequestSigningAlgValuesSupported) is not empty; otherwise, false.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeBackchannelAuthenticationRequestSigningAlgValuesSupported()
        {
            return BackchannelAuthenticationRequestSigningAlgValuesSupported.Count > 0;
        }

        /// <summary>
        /// Gets a bool that determines if the 'claims_supported' (ClaimsSupported) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'claims_supported' (ClaimsSupported) is not empty; otherwise, false.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeClaimsSupported()
        {
            return ClaimsSupported.Count > 0;
        }

        /// <summary>
        /// Gets a bool that determines if the 'claims_locales_supported' (ClaimsLocalesSupported) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'claims_locales_supported' (ClaimsLocalesSupported) is not empty; otherwise, false.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeClaimsLocalesSupported()
        {
            return ClaimsLocalesSupported.Count > 0;
        }

        /// <summary>
        /// Gets a bool that determines if the 'claim_types_supported' (ClaimTypesSupported) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'claim_types_supported' (ClaimTypesSupported) is not empty; otherwise, false.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeClaimTypesSupported()
        {
            return ClaimTypesSupported.Count > 0;
        }

        /// <summary>
        /// Gets a bool that determines if the 'code_challenge_methods_supported' (CodeChallengeMethodsSupported) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'code_challenge_methods_supported' (CodeChallengeMethodsSupported) is not empty; otherwise, false.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeCodeChallengeMethodsSupported()
        {
            return CodeChallengeMethodsSupported.Count > 0;
        }

        /// <summary>
        /// Gets a bool that determines if the 'display_values_supported' (DisplayValuesSupported) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'display_values_supported' (DisplayValuesSupported) is not empty; otherwise, false.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeDisplayValuesSupported()
        {
            return DisplayValuesSupported.Count > 0;
        }

        /// <summary>
        /// Gets a bool that determines if the 'dpop_signing_alg_values_supported' (DPoPSigningAlgValuesSupported) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'dpop_signing_alg_values_supported' (DPoPSigningAlgValuesSupported) is not empty; otherwise, false.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeDPoPSigningAlgValuesSupported()
        {
            return DPoPSigningAlgValuesSupported.Count > 0;
        }

        /// <summary>
        /// Gets a bool that determines if the 'grant_types_supported' (GrantTypesSupported) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'grant_types_supported' (GrantTypesSupported) is not empty; otherwise, false.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeGrantTypesSupported()
        {
            return GrantTypesSupported.Count > 0;
        }

        /// <summary>
        /// Gets a bool that determines if the 'id_token_encryption_alg_values_supported' (IdTokenEncryptionAlgValuesSupported) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'id_token_encryption_alg_values_supported' (IdTokenEncryptionAlgValuesSupported) is not empty; otherwise, false.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeIdTokenEncryptionAlgValuesSupported()
        {
            return IdTokenEncryptionAlgValuesSupported.Count > 0;
        }

        /// <summary>
        /// Gets a bool that determines if the 'id_token_encryption_enc_values_supported' (IdTokenEncryptionEncValuesSupported) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'id_token_encryption_enc_values_supported' (IdTokenEncryptionEncValuesSupported) is not empty; otherwise, false.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeIdTokenEncryptionEncValuesSupported()
        {
            return IdTokenEncryptionEncValuesSupported.Count > 0;
        }

        /// <summary>
        /// Gets a bool that determines if the 'id_token_signing_alg_values_supported' (IdTokenSigningAlgValuesSupported) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'id_token_signing_alg_values_supported' (IdTokenSigningAlgValuesSupported) is not empty; otherwise, false.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeIdTokenSigningAlgValuesSupported()
        {
            return IdTokenSigningAlgValuesSupported.Count > 0;
        }

        /// <summary>
        /// Gets a bool that determines if the 'introspection_endpoint_auth_methods_supported' (IntrospectionEndpointAuthMethodsSupported) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'introspection_endpoint_auth_methods_supported' (IntrospectionEndpointAuthMethodsSupported) is not empty; otherwise, false.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeIntrospectionEndpointAuthMethodsSupported()
        {
            return IntrospectionEndpointAuthMethodsSupported.Count > 0;
        }

        /// <summary>
        /// Gets a bool that determines if the 'introspection_endpoint_auth_signing_alg_values_supported' (IntrospectionEndpointAuthSigningAlgValuesSupported) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'introspection_endpoint_auth_signing_alg_values_supported' (IntrospectionEndpointAuthSigningAlgValuesSupported) is not empty; otherwise, false.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeIntrospectionEndpointAuthSigningAlgValuesSupported()
        {
            return IntrospectionEndpointAuthSigningAlgValuesSupported.Count > 0;
        }

        /// <summary>
        /// Gets a bool that determines if the 'prompt_values_supported' (PromptValuesSupported) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'prompt_values_supported' (PromptValuesSupported) is not empty; otherwise, false.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializePromptValuesSupported()
        {
            return PromptValuesSupported.Count > 0;
        }

        /// <summary>
        /// Gets a bool that determines if the 'request_object_encryption_alg_values_supported' (RequestObjectEncryptionAlgValuesSupported) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'request_object_encryption_alg_values_supported' (RequestObjectEncryptionAlgValuesSupported) is not empty; otherwise, false.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeRequestObjectEncryptionAlgValuesSupported()
        {
            return RequestObjectEncryptionAlgValuesSupported.Count > 0;
        }

        /// <summary>
        /// Gets a bool that determines if the 'request_object_encryption_enc_values_supported' (RequestObjectEncryptionEncValuesSupported) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'request_object_encryption_enc_values_supported' (RequestObjectEncryptionEncValuesSupported) is not empty; otherwise, false.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeRequestObjectEncryptionEncValuesSupported()
        {
            return RequestObjectEncryptionEncValuesSupported.Count > 0;
        }

        /// <summary>
        /// Gets a bool that determines if the 'request_object_signing_alg_values_supported' (RequestObjectSigningAlgValuesSupported) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'request_object_signing_alg_values_supported' (RequestObjectSigningAlgValuesSupported) is not empty; otherwise, false.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeRequestObjectSigningAlgValuesSupported()
        {
            return RequestObjectSigningAlgValuesSupported.Count > 0;
        }

        /// <summary>
        /// Gets a bool that determines if the 'response_modes_supported' (ResponseModesSupported) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'response_modes_supported' (ResponseModesSupported) is not empty; otherwise, false.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeResponseModesSupported()
        {
            return ResponseModesSupported.Count > 0;
        }

        /// <summary>
        /// Gets a bool that determines if the 'response_types_supported' (ResponseTypesSupported) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'response_types_supported' (ResponseTypesSupported) is not empty; otherwise, false.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeResponseTypesSupported()
        {
            return ResponseTypesSupported.Count > 0;
        }

        /// <summary>
        /// Gets a bool that determines if the 'revocation_endpoint_auth_methods_supported' (RevocationEndpointAuthMethodsSupported) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'revocation_endpoint_auth_methods_supported' (RevocationEndpointAuthMethodsSupported) is not empty; otherwise, false.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeRevocationEndpointAuthMethodsSupported()
        {
            return RevocationEndpointAuthMethodsSupported.Count > 0;
        }

        /// <summary>
        /// Gets a bool that determines if the 'revocation_endpoint_auth_signing_alg_values_supported' (RevocationEndpointAuthSigningAlgValuesSupported) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'revocation_endpoint_auth_signing_alg_values_supported' (RevocationEndpointAuthSigningAlgValuesSupported) is not empty; otherwise, false.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeRevocationEndpointAuthSigningAlgValuesSupported()
        {
            return RevocationEndpointAuthSigningAlgValuesSupported.Count > 0;
        }

        /// <summary>
        /// Gets a bool that determines if the 'SigningKeys' property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>This method always returns false.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeSigningKeys()
        {
            return false;
        }

        /// <summary>
        /// Gets a bool that determines if the 'scopes_supported' (ScopesSupported) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'scopes_supported' (ScopesSupported) is not empty; otherwise, false.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeScopesSupported()
        {
            return ScopesSupported.Count > 0;
        }

        /// <summary>
        /// Gets a bool that determines if the 'subject_types_supported' (SubjectTypesSupported) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'subject_types_supported' (SubjectTypesSupported) is not empty; otherwise, false.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeSubjectTypesSupported()
        {
            return SubjectTypesSupported.Count > 0;
        }

        /// <summary>
        /// Gets a bool that determines if the 'token_endpoint_auth_methods_supported' (TokenEndpointAuthMethodsSupported) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'token_endpoint_auth_methods_supported' (TokenEndpointAuthMethodsSupported) is not empty; otherwise, false.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeTokenEndpointAuthMethodsSupported()
        {
            return TokenEndpointAuthMethodsSupported.Count > 0;
        }

        /// <summary>
        /// Gets a bool that determines if the 'token_endpoint_auth_signing_alg_values_supported' (TokenEndpointAuthSigningAlgValuesSupported) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'token_endpoint_auth_signing_alg_values_supported' (TokenEndpointAuthSigningAlgValuesSupported) is not empty; otherwise, false.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeTokenEndpointAuthSigningAlgValuesSupported()
        {
            return TokenEndpointAuthSigningAlgValuesSupported.Count > 0;
        }

        /// <summary>
        /// Gets a bool that determines if the 'ui_locales_supported' (UILocalesSupported) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'ui_locales_supported' (UILocalesSupported) is not empty; otherwise, false.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeUILocalesSupported()
        {
            return UILocalesSupported.Count > 0;
        }

        /// <summary>
        /// Gets a bool that determines if the 'userinfo_encryption_alg_values_supported' (UserInfoEndpointEncryptionAlgValuesSupported ) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'userinfo_encryption_alg_values_supported' (UserInfoEndpointEncryptionAlgValuesSupported ) is not empty; otherwise, false.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeUserInfoEndpointEncryptionAlgValuesSupported()
        {
            return UserInfoEndpointEncryptionAlgValuesSupported.Count > 0;
        }

        /// <summary>
        /// Gets a bool that determines if the 'userinfo_encryption_enc_values_supported' (UserInfoEndpointEncryptionEncValuesSupported) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'userinfo_encryption_enc_values_supported' (UserInfoEndpointEncryptionEncValuesSupported) is not empty; otherwise, false.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeUserInfoEndpointEncryptionEncValuesSupported()
        {
            return UserInfoEndpointEncryptionEncValuesSupported.Count > 0;
        }

        /// <summary>
        /// Gets a bool that determines if the 'userinfo_signing_alg_values_supported' (UserInfoEndpointSigningAlgValuesSupported) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'userinfo_signing_alg_values_supported' (UserInfoEndpointSigningAlgValuesSupported) is not empty; otherwise, false.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeUserInfoEndpointSigningAlgValuesSupported()
        {
            return UserInfoEndpointSigningAlgValuesSupported.Count > 0;
        }
#endregion shouldserialize
    }
}
