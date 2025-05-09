// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect.Json.Tests
{
    internal static class LogMessages
    {
        // properties, configuration
        internal const string IDX21105 = "IDX21105: NonceLifetime must be greater than zero. value: '{0}'";
        internal const string IDX21106 = "IDX21106: Error in deserializing to json: '{0}'";

        internal const string IDX21806 = "IDX21806: Deserializing json string into json web keys.";
        internal const string IDX21808 = "IDX21808: Deserializing json into OpenIdConnectConfiguration object: '{0}'.";
        internal const string IDX21809 = "IDX21809: Serializing OpenIdConfiguration object to json string.";
        internal const string IDX21811 = "IDX21811: Deserializing the string: '{0}' obtained from metadata endpoint into openIdConnectConfiguration object.";
        internal const string IDX21812 = "IDX21812: Retrieving json web keys from: '{0}'.";
        internal const string IDX21813 = "IDX21813: Deserializing json web keys: '{0}'.";
        internal const string IDX21815 = "IDX21815: Error deserializing json: '{0}' into '{1}'.";
        internal const string IDX21816 = "IDX21816: The number of signing keys must be greater or equal to '{0}'. Value: '{1}'.";
        internal const string IDX21817 = "IDX21817: The OpenIdConnectConfiguration did not contain any JsonWebKeys. This is required to validate the configuration.";
        internal const string IDX21818 = "IDX21818: The OpenIdConnectConfiguration's valid signing keys cannot be less than {0}. Values: {1}. Invalid keys: {2}";
    }

    /// <summary>
    /// Contains OpenIdConnect configuration that can be populated from a json string.
    /// This is the original OpenIdConnectConfiguration in the 6x branch.
    /// Used for ensuring backcompat.
    /// </summary>
    [JsonObject]
    public class OpenIdConnectConfiguration6x : BaseConfiguration
    {
        private const string _className = "Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectConfiguration";

        /// <summary>
        /// Deserializes the json string into an <see cref="OpenIdConnectConfiguration"/> object.
        /// </summary>
        /// <param name="json">json string representing the configuration.</param>
        /// <returns><see cref="OpenIdConnectConfiguration"/> object representing the configuration.</returns>
        /// <exception cref="ArgumentNullException">If 'json' is null or empty.</exception>
        /// <exception cref="ArgumentException">If 'json' fails to deserialize.</exception>
        public static OpenIdConnectConfiguration6x Create(string json)
        {
            if (string.IsNullOrEmpty(json))
                throw LogHelper.LogArgumentNullException(nameof(json));

            if (LogHelper.IsEnabled(EventLogLevel.Verbose))
                LogHelper.LogVerbose(LogMessages.IDX21808, json);

            return new OpenIdConnectConfiguration6x(json);
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

            LogHelper.LogVerbose(LogMessages.IDX21809);
            return JsonConvert.SerializeObject(configuration);
        }

        /// <summary>
        /// Initializes an new instance of <see cref="OpenIdConnectConfiguration"/>.
        /// </summary>
        public OpenIdConnectConfiguration6x()
        {
        }

        /// <summary>
        /// Initializes an new instance of <see cref="OpenIdConnectConfiguration"/> from a json string.
        /// </summary>
        /// <param name="json">a json string containing the metadata</param>
        /// <exception cref="ArgumentNullException">If 'json' is null or empty.</exception>
        public OpenIdConnectConfiguration6x(string json)
        {
            if(string.IsNullOrEmpty(json))
                throw LogHelper.LogArgumentNullException(nameof(json));

            try
            {
                if (LogHelper.IsEnabled(EventLogLevel.Verbose))
                    LogHelper.LogVerbose(LogMessages.IDX21806, json, LogHelper.MarkAsNonPII(_className));

                JsonConvert.PopulateObject(json, this);
            }
            catch (Exception ex)
            {
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX21815, json, LogHelper.MarkAsNonPII(_className)), ex));
            }
        }

        /// <summary>
        /// When deserializing from JSON any properties that are not defined will be placed here.
        /// </summary>
        [JsonExtensionData]
        public virtual IDictionary<string, object> AdditionalData { get; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets the collection of 'acr_values_supported'
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.AcrValuesSupported, Required = Required.Default)]
        public ICollection<string> AcrValuesSupported { get; } = new Collection<string>();

        /// <summary>
        /// Gets or sets the 'authorization_endpoint'.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.AuthorizationEndpoint, Required = Required.Default)]
        public string AuthorizationEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the 'check_session_iframe'.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.CheckSessionIframe, Required = Required.Default)]
        public string CheckSessionIframe { get; set; }

        /// <summary>
        /// Gets the collection of 'claims_supported'
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.ClaimsSupported, Required = Required.Default)]
        public ICollection<string> ClaimsSupported { get; } = new Collection<string>();

        /// <summary>
        /// Gets the collection of 'claims_locales_supported'
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.ClaimsLocalesSupported, Required = Required.Default)]
        public ICollection<string> ClaimsLocalesSupported { get; } = new Collection<string>();

        /// <summary>
        /// Gets or sets the 'claims_parameter_supported'
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.ClaimsParameterSupported, Required = Required.Default)]
        public bool ClaimsParameterSupported { get; set; }

        /// <summary>
        /// Gets the collection of 'claim_types_supported'
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.ClaimTypesSupported, Required = Required.Default)]
        public ICollection<string> ClaimTypesSupported { get; } = new Collection<string>();

        /// <summary>
        /// Gets the collection of 'display_values_supported'
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.DisplayValuesSupported, Required = Required.Default)]
        public ICollection<string> DisplayValuesSupported { get; } = new Collection<string>();

        /// <summary>
        /// Gets or sets the 'end_session_endpoint'.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.EndSessionEndpoint, Required = Required.Default)]
        public string EndSessionEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the 'frontchannel_logout_session_supported'.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.FrontchannelLogoutSessionSupported, Required = Required.Default)]
        public string FrontchannelLogoutSessionSupported { get; set; }

        /// <summary>
        /// Gets or sets the 'frontchannel_logout_supported'.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.FrontchannelLogoutSupported, Required = Required.Default)]
        public string FrontchannelLogoutSupported { get; set; }

        /// <summary>
        /// Gets the collection of 'grant_types_supported'
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.GrantTypesSupported, Required = Required.Default)]
        public ICollection<string> GrantTypesSupported { get; } = new Collection<string>();

        /// <summary>
        /// Boolean value specifying whether the OP supports HTTP-based logout. Default is false.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.HttpLogoutSupported, Required = Required.Default)]
        public bool HttpLogoutSupported { get; set; }

        /// <summary>
        /// Gets the collection of 'id_token_encryption_alg_values_supported'.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.IdTokenEncryptionAlgValuesSupported, Required = Required.Default)]
        public ICollection<string> IdTokenEncryptionAlgValuesSupported { get; } = new Collection<string>();

        /// <summary>
        /// Gets the collection of 'id_token_encryption_enc_values_supported'.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.IdTokenEncryptionEncValuesSupported, Required = Required.Default)]
        public ICollection<string> IdTokenEncryptionEncValuesSupported { get; } = new Collection<string>();

        /// <summary>
        /// Gets the collection of 'id_token_signing_alg_values_supported'.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.IdTokenSigningAlgValuesSupported, Required = Required.Default)]
        public ICollection<string> IdTokenSigningAlgValuesSupported { get; } = new Collection<string>();

        /// <summary>
        /// Gets or sets the 'introspection_endpoint'.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.IntrospectionEndpoint, Required = Required.Default)]
        public string IntrospectionEndpoint { get; set; }

        /// <summary>
        /// Gets the collection of 'introspection_endpoint_auth_methods_supported'.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.IntrospectionEndpointAuthMethodsSupported, Required = Required.Default)]
        public ICollection<string> IntrospectionEndpointAuthMethodsSupported { get; } = new Collection<string>();

        /// <summary>
        /// Gets the collection of 'introspection_endpoint_auth_signing_alg_values_supported'.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.IntrospectionEndpointAuthSigningAlgValuesSupported, Required = Required.Default)]
        public ICollection<string> IntrospectionEndpointAuthSigningAlgValuesSupported { get; } = new Collection<string>();

        /// <summary>
        /// Gets or sets the 'issuer'.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.Issuer, Required = Required.Default)]
        public override string Issuer { get; set; }

        /// <summary>
        /// Gets or sets the 'jwks_uri'
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.JwksUri, Required = Required.Default)]
        public string JwksUri { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="JsonWebKeySet"/>
        /// </summary>
        public JsonWebKeySet JsonWebKeySet {get; set;}

        /// <summary>
        /// Boolean value specifying whether the OP can pass a sid (session ID) query parameter to identify the RP session at the OP when the logout_uri is used. Dafault Value is false.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.LogoutSessionSupported, Required = Required.Default)]
        public bool LogoutSessionSupported { get; set; }

        /// <summary>
        /// Gets or sets the 'op_policy_uri'
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.OpPolicyUri, Required = Required.Default)]
        public string OpPolicyUri { get; set; }

        /// <summary>
        /// Gets or sets the 'op_tos_uri'
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.OpTosUri, Required = Required.Default)]
        public string OpTosUri { get; set; }

        /// <summary>
        /// Gets or sets the 'registration_endpoint'
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.RegistrationEndpoint, Required = Required.Default)]
        public string RegistrationEndpoint { get; set; }

        /// <summary>
        /// Gets the collection of 'request_object_encryption_alg_values_supported'.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.RequestObjectEncryptionAlgValuesSupported, Required = Required.Default)]
        public ICollection<string> RequestObjectEncryptionAlgValuesSupported { get; } = new Collection<string>();

        /// <summary>
        /// Gets the collection of 'request_object_encryption_enc_values_supported'.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.RequestObjectEncryptionEncValuesSupported, Required = Required.Default)]
        public ICollection<string> RequestObjectEncryptionEncValuesSupported { get; } = new Collection<string>();

        /// <summary>
        /// Gets the collection of 'request_object_signing_alg_values_supported'.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.RequestObjectSigningAlgValuesSupported, Required = Required.Default)]
        public ICollection<string> RequestObjectSigningAlgValuesSupported { get; } = new Collection<string>();

        /// <summary>
        /// Gets or sets the 'request_parameter_supported'
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.RequestParameterSupported, Required = Required.Default)]
        public bool RequestParameterSupported { get; set; }

        /// <summary>
        /// Gets or sets the 'request_uri_parameter_supported'
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.RequestUriParameterSupported, Required = Required.Default)]
        public bool RequestUriParameterSupported { get; set; }

        /// <summary>
        /// Gets or sets the 'require_request_uri_registration'
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.RequireRequestUriRegistration, Required = Required.Default)]
        public bool RequireRequestUriRegistration { get; set; }

        /// <summary>
        /// Gets the collection of 'response_modes_supported'.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.ResponseModesSupported, Required = Required.Default)]
        public ICollection<string> ResponseModesSupported { get; } = new Collection<string>();

        /// <summary>
        /// Gets the collection of 'response_types_supported'.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.ResponseTypesSupported, Required = Required.Default)]
        public ICollection<string> ResponseTypesSupported { get; } = new Collection<string>();

        /// <summary>
        /// Gets or sets the 'service_documentation'
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.ServiceDocumentation, Required = Required.Default)]
        public string ServiceDocumentation { get; set; }

        /// <summary>
        /// Gets the collection of 'scopes_supported'
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.ScopesSupported, Required = Required.Default)]
        public ICollection<string> ScopesSupported { get; } = new Collection<string>();

        /// <summary>
        /// Gets the <see cref="ICollection{SecurityKey}"/> that the IdentityProvider indicates are to be used signing tokens.
        /// </summary>
        [JsonIgnore]
        public override ICollection<SecurityKey> SigningKeys { get; } = new Collection<SecurityKey>();

        /// <summary>
        /// Gets the collection of 'subject_types_supported'.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.SubjectTypesSupported, Required = Required.Default)]
        public ICollection<string> SubjectTypesSupported { get; } = new Collection<string>();

        /// <summary>
        /// Gets or sets the 'token_endpoint'.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.TokenEndpoint, Required = Required.Default)]
        public override string TokenEndpoint { get; set; }

        /// <summary>
        /// This base class property is not used in OpenIdConnect. 
        /// </summary>
        [JsonIgnore]
        public override string ActiveTokenEndpoint { get; set; }

        /// <summary>
        /// Gets the collection of 'token_endpoint_auth_methods_supported'.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.TokenEndpointAuthMethodsSupported, Required = Required.Default)]
        public ICollection<string> TokenEndpointAuthMethodsSupported { get; } = new Collection<string>();

        /// <summary>
        /// Gets the collection of 'token_endpoint_auth_signing_alg_values_supported'.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.TokenEndpointAuthSigningAlgValuesSupported, Required = Required.Default)]
        public ICollection<string> TokenEndpointAuthSigningAlgValuesSupported { get; } = new Collection<string>();

        /// <summary>
        /// Gets the collection of 'ui_locales_supported'
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.UILocalesSupported, Required = Required.Default)]
        public ICollection<string> UILocalesSupported { get; } = new Collection<string>();

        /// <summary>
        /// Gets or sets the 'user_info_endpoint'.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.UserInfoEndpoint, Required = Required.Default)]
        public string UserInfoEndpoint { get; set; }

        /// <summary>
        /// Gets the collection of 'userinfo_encryption_alg_values_supported'
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.UserInfoEncryptionAlgValuesSupported, Required = Required.Default)]
        public ICollection<string> UserInfoEndpointEncryptionAlgValuesSupported { get; } = new Collection<string>();

        /// <summary>
        /// Gets the collection of 'userinfo_encryption_enc_values_supported'
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.UserInfoEncryptionEncValuesSupported, Required = Required.Default)]
        public ICollection<string> UserInfoEndpointEncryptionEncValuesSupported { get; } = new Collection<string>();

        /// <summary>
        /// Gets the collection of 'userinfo_signing_alg_values_supported'
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = OpenIdProviderMetadataNames.UserInfoSigningAlgValuesSupported, Required = Required.Default)]
        public ICollection<string> UserInfoEndpointSigningAlgValuesSupported { get; } = new Collection<string>();

#region shouldserialize
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
