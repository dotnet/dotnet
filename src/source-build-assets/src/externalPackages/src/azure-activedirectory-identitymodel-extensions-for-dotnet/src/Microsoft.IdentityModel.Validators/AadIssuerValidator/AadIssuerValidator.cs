// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.IdentityModel.Validators.AadIssuerValidator;

namespace Microsoft.IdentityModel.Validators
{
    /// <summary>
    /// Generic class that validates the issuer for either JsonWebTokens or JwtSecurityTokens issued from the Microsoft identity platform (AAD).
    /// </summary>
    public class AadIssuerValidator
    {
        private static readonly TimeSpan LastKnownGoodConfigurationLifetime = new TimeSpan(0, 24, 0, 0);

        internal const string V11EndpointSuffix = "/v1.1";
        internal const string V11EndpointSuffixWithTrailingSlash = $"{V11EndpointSuffix}/";

        internal const string V2EndpointSuffix = "/v2.0";
        internal const string V2EndpointSuffixWithTrailingSlash = $"{V2EndpointSuffix}/";

        internal const string TenantIdTemplate = "{tenantid}";

        private Func<string, BaseConfigurationManager> _configurationManagerProvider;

        internal AadIssuerValidator(
            HttpClient httpClient,
            string aadAuthority)
        {
            HttpClient = httpClient;
            AadAuthority = aadAuthority.TrimEnd('/');
            AadAuthorityVersion = GetProtocolVersion(AadAuthority);
            SetupAuthorities(AadAuthority, AadAuthorityVersion);
        }

        internal AadIssuerValidator(
            HttpClient httpClient,
            string aadAuthority,
            Func<string, BaseConfigurationManager> configurationManagerProvider)
            : this(httpClient, aadAuthority)
        {
            if (configurationManagerProvider == null)
                throw new ArgumentNullException(nameof(configurationManagerProvider));

            _configurationManagerProvider = configurationManagerProvider;
        }

        private HttpClient HttpClient { get; }

        private BaseConfigurationManager _configurationManagerV1;
        private BaseConfigurationManager _configurationManagerV11;
        private BaseConfigurationManager _configurationManagerV2;
        private IssuerLastKnownGood _issuerLKGV1;
        private IssuerLastKnownGood _issuerLKGV11;
        private IssuerLastKnownGood _issuerLKGV2;

        internal BaseConfigurationManager ConfigurationManagerV1
        {
            get
            {
                if (_configurationManagerV1 == null)
                    _configurationManagerV1 = CreateConfigManager(AadAuthorityV1);
            
                return _configurationManagerV1;
            }

            set
            {
                _configurationManagerV1 = value;
            }
        }

        internal BaseConfigurationManager ConfigurationManagerV11
        {
            get
            {
                if (_configurationManagerV11 == null)
                    _configurationManagerV11 = CreateConfigManager(AadAuthorityV11);

                return _configurationManagerV11;
            }

            set
            {
                _configurationManagerV11 = value;
            }
        }

        internal BaseConfigurationManager ConfigurationManagerV2
        {
            get
            {
                if (_configurationManagerV2 == null)
                    _configurationManagerV2 = CreateConfigManager(AadAuthorityV2);

                return _configurationManagerV2;
            }

            set
            {
                _configurationManagerV2 = value;
            }
        }

        internal string AadAuthorityV1
        {
            get;
            private set;
        }

        internal string AadAuthorityV11
        {
            get;
            private set;
        }

        internal string AadAuthorityV2
        {
            get;
            private set;
        }

        private void SetupAuthorities(string aadAuthority, ProtocolVersion version)
        {
            switch (version)
            {
                case ProtocolVersion.V1:
                    AadAuthorityV1 = aadAuthority;
                    AadAuthorityV11 = AadAuthorityV1 + V11EndpointSuffix;
                    AadAuthorityV2 = AadAuthorityV1 + V2EndpointSuffix;
                    break;
                    
                case ProtocolVersion.V11:
                    AadAuthorityV1 = CreateV1Authority(AadAuthority, V11EndpointSuffix);
                    AadAuthorityV11 = aadAuthority;
                    AadAuthorityV2 = AadAuthorityV1 + V2EndpointSuffix;
                    break;

                case ProtocolVersion.V2:
                    AadAuthorityV1 = CreateV1Authority(AadAuthority);
                    AadAuthorityV11 = AadAuthorityV1 + V11EndpointSuffix;
                    AadAuthorityV2 = aadAuthority;
                    break;

                default:
                    throw new InvalidOperationException("Unsupported protocol version.");
            }
        }

        internal string AadIssuerV1 { get; set; }
        internal string AadIssuerV2 { get; set; }
        internal string AadAuthority { get; set; }
        internal ProtocolVersion AadAuthorityVersion { get; set; }

        internal static readonly IDictionary<string, AadIssuerValidator> s_issuerValidators = new ConcurrentDictionary<string, AadIssuerValidator>();

        /// <summary>
        /// Validate the issuer for single and multi-tenant applications of various audiences (Work and School accounts, or Work and School accounts +
        /// Personal accounts) and the various clouds.
        /// </summary>
        /// <param name="issuer">Issuer to validate (will be tenanted).</param>
        /// <param name="securityToken">Received security token.</param>
        /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> to be used for validating the token.</param>
        /// <example><code>
        /// AadIssuerValidator aadIssuerValidator = AadIssuerValidator.GetAadIssuerValidator(authority, httpClient);
        /// TokenValidationParameters.IssuerValidator = aadIssuerValidator.Validate;
        /// </code></example>
        /// <remarks>The issuer is considered as valid if it has the same HTTP scheme and authority as the
        /// authority from the configuration file, has a tenant ID, and optionally v2.0 (if this web API
        /// accepts both V1 and V2 tokens).</remarks>
        /// <returns>The <c>issuer</c> if it's valid, or otherwise <c>SecurityTokenInvalidIssuerException</c> is thrown.</returns>
        /// <exception cref="ArgumentNullException"> if <paramref name="securityToken"/> is null.</exception>
        /// <exception cref="ArgumentNullException"> if <paramref name="validationParameters"/> is null.</exception>
        /// <exception cref="SecurityTokenInvalidIssuerException">if the issuer is invalid or if there is a network issue. </exception>
        public string Validate(
            string issuer,
            SecurityToken securityToken,
            TokenValidationParameters validationParameters)
        {
            ValueTask<string> vt = ValidateAsync(issuer, securityToken, validationParameters);
            return vt.IsCompletedSuccessfully ?
                vt.Result :
                vt.AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Validate the issuer for single and multi-tenant applications of various audiences (Work and School accounts, or Work and School accounts +
        /// Personal accounts) and the various clouds.
        /// </summary>
        /// <param name="issuer">Issuer to validate (will be tenanted).</param>
        /// <param name="securityToken">Received security token.</param>
        /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> to be used for validating the token.</param>
        /// <example><code>
        /// AadIssuerValidator aadIssuerValidator = AadIssuerValidator.GetAadIssuerValidator(authority, httpClient);
        /// TokenValidationParameters.IssuerValidator = aadIssuerValidator.Validate;
        /// </code></example>
        /// <remarks>The issuer is considered as valid if it has the same HTTP scheme and authority as the
        /// authority from the configuration file, has a tenant ID, and optionally v2.0 (if this web API
        /// accepts both V1 and V2 tokens).</remarks>
        /// <returns>The <c>issuer</c> if it's valid, or otherwise <c>SecurityTokenInvalidIssuerException</c> is thrown.</returns>
        /// <exception cref="ArgumentNullException"> if <paramref name="securityToken"/> is null.</exception>
        /// <exception cref="ArgumentNullException"> if <paramref name="validationParameters"/> is null.</exception>
        /// <exception cref="SecurityTokenInvalidIssuerException">if the issuer is invalid or if there is a network issue. </exception>
        internal async ValueTask<string> ValidateAsync(
            string issuer,
            SecurityToken securityToken,
            TokenValidationParameters validationParameters)
        {
            _ = issuer ?? throw LogHelper.LogArgumentNullException(nameof(issuer));
            _ = securityToken ?? throw LogHelper.LogArgumentNullException(nameof(securityToken));
            _ = validationParameters ?? throw LogHelper.LogArgumentNullException(nameof(validationParameters));

            string tenantId = GetTenantIdFromToken(securityToken);

            if (string.IsNullOrWhiteSpace(tenantId))
                throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidIssuerException(LogMessages.IDX40003));

            if (validationParameters.ValidIssuers != null)
            {
                foreach (var validIssuerTemplate in validationParameters.ValidIssuers)
                {
                    if (IsValidIssuer(validIssuerTemplate, tenantId, issuer))
                        return issuer;
                }
            }

            if (validationParameters.ValidIssuer != null)
            {
                if (IsValidIssuer(validationParameters.ValidIssuer, tenantId, issuer))
                    return issuer;
            }

            try
            {
                var issuerVersion = GetTokenIssuerVersion(securityToken);
                var effectiveConfigurationManager = GetEffectiveConfigurationManager(issuerVersion);

                string aadIssuer = null;
                if (validationParameters.ValidateWithLKG)
                {
                    // returns null if LKG issuer expired
                    aadIssuer = GetEffectiveLKGIssuer(issuerVersion);
                }
                else
                {
                    var baseConfiguration = await GetBaseConfigurationAsync(effectiveConfigurationManager, validationParameters).ConfigureAwait(false);
                    aadIssuer = baseConfiguration.Issuer;
                }

                if (aadIssuer != null)
                {
                    var isIssuerValid = IsValidIssuer(aadIssuer, tenantId, issuer);

                    // The original LKG assignment behavior for previous self-state management.
                    if (isIssuerValid && !validationParameters.ValidateWithLKG)
                        SetEffectiveLKGIssuer(aadIssuer, issuerVersion, effectiveConfigurationManager.LastKnownGoodLifetime);

                    if (isIssuerValid)
                        return issuer;
                }
            }
            catch (Exception ex)
            {
                throw LogHelper.LogExceptionMessage(
                    new SecurityTokenInvalidIssuerException(
                        LogHelper.FormatInvariant(
                            LogMessages.IDX40001,
                            LogHelper.MarkAsNonPII(issuer)),
                        ex));
            }

            // If a valid issuer is not found, throw
            throw LogHelper.LogExceptionMessage(
                new SecurityTokenInvalidIssuerException(
                    LogHelper.FormatInvariant(
                        LogMessages.IDX40001,
                        LogHelper.MarkAsNonPII(issuer))));
        }

        /// <summary>
        /// Gets an <see cref="AadIssuerValidator"/> for an Azure Active Directory (AAD) authority.
        /// </summary>
        /// <param name="aadAuthority">The authority to create the validator for, e.g. https://login.microsoftonline.com/. </param>
        /// <param name="httpClient">Optional HttpClient to use to retrieve the endpoint metadata (can be null).</param>
        /// <example><code>
        /// AadIssuerValidator aadIssuerValidator = AadIssuerValidator.GetAadIssuerValidator(authority, httpClient);
        /// TokenValidationParameters.IssuerValidator = aadIssuerValidator.Validate;
        /// </code></example>
        /// <returns>A <see cref="AadIssuerValidator"/> for the aadAuthority.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="aadAuthority"/> is null or empty.</exception>
        public static AadIssuerValidator GetAadIssuerValidator(string aadAuthority, HttpClient httpClient)
        {
            return GetAadIssuerValidator(aadAuthority, httpClient, null);
        }

        /// <summary>
        /// Gets an <see cref="AadIssuerValidator"/> for an Azure Active Directory (AAD) authority.
        /// </summary>
        /// <param name="aadAuthority">The authority to create the validator for, e.g. https://login.microsoftonline.com/. </param>
        /// <example><code>
        /// AadIssuerValidator aadIssuerValidator = AadIssuerValidator.GetAadIssuerValidator(authority);
        /// TokenValidationParameters.IssuerValidator = aadIssuerValidator.Validate;
        /// </code></example>
        /// <returns>A <see cref="AadIssuerValidator"/> for the aadAuthority.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="aadAuthority"/> is null or empty.</exception>
        public static AadIssuerValidator GetAadIssuerValidator(string aadAuthority)
        {
            return GetAadIssuerValidator(aadAuthority, null, null);
        }

        /// <summary>
        /// Gets an <see cref="AadIssuerValidator"/> for an Azure Active Directory (AAD) authority.
        /// </summary>
        /// <param name="aadAuthority">The authority to create the validator for, e.g. https://login.microsoftonline.com/. </param>
        /// <param name="httpClient">Optional HttpClient to use to retrieve the endpoint metadata (can be null).</param>
        /// <param name="configurationManagerProvider">Configuration manager provider. Injection point for metadata managed outside of the class.</param>
        /// <example><code>
        /// AadIssuerValidator aadIssuerValidator = AadIssuerValidator.GetAadIssuerValidator(authority, configurationManagerProvider);
        /// TokenValidationParameters.IssuerValidator = aadIssuerValidator.Validate;
        /// </code></example>
        /// <returns>A <see cref="AadIssuerValidator"/> for the aadAuthority.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="aadAuthority"/> is null or empty.</exception>
        internal static AadIssuerValidator GetAadIssuerValidator(string aadAuthority, HttpClient httpClient, Func<string, BaseConfigurationManager> configurationManagerProvider)
        {
            if (string.IsNullOrEmpty(aadAuthority))
                throw LogHelper.LogArgumentNullException(nameof(aadAuthority));

            if (configurationManagerProvider != null)
                return new AadIssuerValidator(
                    httpClient,
                    aadAuthority,
                    configurationManagerProvider);

            if (s_issuerValidators.TryGetValue(aadAuthority, out AadIssuerValidator aadIssuerValidator))
                return aadIssuerValidator;

            s_issuerValidators[aadAuthority] = new AadIssuerValidator(
                httpClient,
                aadAuthority);

            return s_issuerValidators[aadAuthority];
        }


        private static string CreateV1Authority(string aadV2Authority, string suffixToReplace = V2EndpointSuffix)
        {
            if (suffixToReplace == V2EndpointSuffix && aadV2Authority.Contains(AadIssuerValidatorConstants.Organizations))
                return aadV2Authority.Replace($"{AadIssuerValidatorConstants.Organizations}{suffixToReplace}", AadIssuerValidatorConstants.Common);

            return aadV2Authority.Replace(suffixToReplace, string.Empty);
        }

        private ConfigurationManager<OpenIdConnectConfiguration> CreateConfigManager(
            string aadAuthority)
        {
            if (HttpClient != null)
            {
                return
                 new ConfigurationManager<OpenIdConnectConfiguration>(
                     $"{aadAuthority}{AadIssuerValidatorConstants.OidcEndpoint}",
                     new OpenIdConnectConfigurationRetriever(),
                     HttpClient)
                 { LastKnownGoodLifetime = LastKnownGoodConfigurationLifetime };
            }
            else
            {
                return
                new ConfigurationManager<OpenIdConnectConfiguration>(
                    $"{aadAuthority}{AadIssuerValidatorConstants.OidcEndpoint}",
                    new OpenIdConnectConfigurationRetriever())
                { LastKnownGoodLifetime = LastKnownGoodConfigurationLifetime };
            }
        }

        private static bool IsValidIssuer(string validIssuerTemplate, string tenantId, string actualIssuer)
        {
            if (string.IsNullOrEmpty(validIssuerTemplate))
                return false;

            if (validIssuerTemplate.Contains(TenantIdTemplate))
            {
                return validIssuerTemplate.Replace(TenantIdTemplate, tenantId) == actualIssuer;
            }
            else
            {
                return validIssuerTemplate == actualIssuer;
            }
        }

        private void SetEffectiveLKGIssuer(string aadIssuer, ProtocolVersion protocolVersion, TimeSpan lastKnownGoodLifetime)
        {
            var issuerLKG = new IssuerLastKnownGood
            {
                Issuer = aadIssuer,
                LastKnownGoodLifetime = lastKnownGoodLifetime
            };

            switch (protocolVersion)
            {
                case ProtocolVersion.V1:
                    _issuerLKGV1 = issuerLKG;
                    break;

                case ProtocolVersion.V11:
                    _issuerLKGV11 = issuerLKG;
                    break;

                case ProtocolVersion.V2:
                    _issuerLKGV2 = issuerLKG;
                    break;
            }
        }

        private string GetEffectiveLKGIssuer(ProtocolVersion protocolVersion)
        {
            IssuerLastKnownGood effectiveLKGIssuer = null;
            switch (protocolVersion)
            {
                case ProtocolVersion.V1:
                    effectiveLKGIssuer = _issuerLKGV1;
                    break;

                case ProtocolVersion.V11:
                    effectiveLKGIssuer = _issuerLKGV11;
                    break;

                case ProtocolVersion.V2:
                    effectiveLKGIssuer = _issuerLKGV2;
                    break;
            }

            if (effectiveLKGIssuer != null && effectiveLKGIssuer.IsValid)
            {
                return effectiveLKGIssuer.Issuer;
            }

            return null;
        }

        private static ProtocolVersion GetTokenIssuerVersion(SecurityToken securityToken)
        {
            if (securityToken.Issuer.EndsWith(V2EndpointSuffixWithTrailingSlash, StringComparison.OrdinalIgnoreCase) ||
                securityToken.Issuer.EndsWith(V2EndpointSuffix, StringComparison.OrdinalIgnoreCase))
                return ProtocolVersion.V2;

            if (securityToken.Issuer.EndsWith(V11EndpointSuffixWithTrailingSlash, StringComparison.OrdinalIgnoreCase) ||
                securityToken.Issuer.EndsWith(V11EndpointSuffix, StringComparison.OrdinalIgnoreCase))
                return ProtocolVersion.V11;

            return ProtocolVersion.V1;
        }

        private BaseConfigurationManager GetEffectiveConfigurationManager(ProtocolVersion protocolVersion)
        {        
            if (_configurationManagerProvider != null)
            {
                string aadAuthority = GetAuthority(protocolVersion);
                

                var configurationManager = _configurationManagerProvider(aadAuthority);
                if (configurationManager != null)
                    return configurationManager;
            }

            // If no provider or provider returned null, fallback to previous strategy            
            return GetConfigurationManager(protocolVersion);
        }

        private BaseConfigurationManager GetConfigurationManager(ProtocolVersion protocolVersion)
        {
            switch (protocolVersion)
            {
                case ProtocolVersion.V1:
                    return ConfigurationManagerV1;

                case ProtocolVersion.V11:
                    return ConfigurationManagerV11;

                case ProtocolVersion.V2:
                    return ConfigurationManagerV2;

                default:
                    return ConfigurationManagerV1;
            }
        }

        private string GetAuthority(ProtocolVersion protocolVersion)
        {
            switch (protocolVersion)
            {
                case ProtocolVersion.V1:
                    return AadAuthorityV1;

                case ProtocolVersion.V11:
                    return AadAuthorityV11;

                case ProtocolVersion.V2:
                    return AadAuthorityV2;

                default:
                    return AadAuthorityV1;
            }
        }

        private static ProtocolVersion GetProtocolVersion(string aadAuthority)
        {
            if (aadAuthority.Contains(V2EndpointSuffix))
                return ProtocolVersion.V2;

            if (aadAuthority.Contains(V11EndpointSuffix))
                return ProtocolVersion.V11;

            return ProtocolVersion.V1;
        }

        private static Task<BaseConfiguration> GetBaseConfigurationAsync(BaseConfigurationManager configurationManager, TokenValidationParameters validationParameters)
        {
            if (validationParameters.RefreshBeforeValidation)
                configurationManager.RequestRefresh();

            return configurationManager.GetBaseConfigurationAsync(CancellationToken.None);
        }

        /// <summary>Gets the tenant ID from a token.</summary>
        /// <param name="securityToken">A JWT token.</param>
        /// <returns>A string containing the tenant ID, if found or <see cref="string.Empty"/>.</returns>
        /// <remarks>Only <see cref="JwtSecurityToken"/> and <see cref="JsonWebToken"/> are acceptable types.</remarks>
        internal static string GetTenantIdFromToken(SecurityToken securityToken)
        {
            if (securityToken is JwtSecurityToken jwtSecurityToken)
            {
                if (jwtSecurityToken.Payload.TryGetValue(AadIssuerValidatorConstants.Tid, out object tid))
                    return (string)tid;

                if (jwtSecurityToken.Payload.TryGetValue(AadIssuerValidatorConstants.TenantId, out object tenantId))
                    return (string)tenantId;

                // Since B2C doesn't have "tid" as default, get it from issuer
                return GetTenantIdFromIss(jwtSecurityToken.Issuer);
            }

            if (securityToken is JsonWebToken jsonWebToken)
            {
                if (jsonWebToken.TryGetPayloadValue(AadIssuerValidatorConstants.Tid, out string tid))
                    return tid;

                if (jsonWebToken.TryGetPayloadValue(AadIssuerValidatorConstants.TenantId, out string tenantId))
                    return tenantId;

                // Since B2C doesn't have "tid" as default, get it from issuer
                return GetTenantIdFromIss(jsonWebToken.Issuer);
            }

            return string.Empty;
        }

        // The AAD "iss" claims contains the tenant ID in its value.
        // The URI can be
        // - {domain}/{tid}/v2.0
        // - {domain}/{tid}/v2.0/
        // - {domain}/{tfp}/{tid}/{userFlow}/v2.0/
        private static string GetTenantIdFromIss(string iss)
        {
            if (string.IsNullOrEmpty(iss))
                return string.Empty;

            var uri = new Uri(iss);

            if (uri.Segments.Length == 3)
                return uri.Segments[1].TrimEnd('/');

            if (uri.Segments.Length == 5 && uri.Segments[1].TrimEnd('/') == AadIssuerValidatorConstants.Tfp)
                throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidIssuerException(LogMessages.IDX40002));

            return string.Empty;
        }
    }
}
