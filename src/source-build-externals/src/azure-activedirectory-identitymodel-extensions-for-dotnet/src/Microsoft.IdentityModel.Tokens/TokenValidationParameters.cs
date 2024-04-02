// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Definition for AlgorithmValidator
    /// </summary>
    /// <param name="algorithm">The algorithm to validate.</param>
    /// <param name="securityKey">The <see cref="SecurityKey"/> that signed the <see cref="SecurityToken"/>.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    /// <returns><c>true</c> if the algorithm is considered valid</returns>
    public delegate bool AlgorithmValidator(string algorithm, SecurityKey securityKey, SecurityToken securityToken, TokenValidationParameters validationParameters);

    /// <summary>
    /// Definition for AudienceValidator.
    /// </summary>
    /// <param name="audiences">The audiences found in the <see cref="SecurityToken"/>.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    /// <returns>true if the audience is considered valid.</returns>
    public delegate bool AudienceValidator(IEnumerable<string> audiences, SecurityToken securityToken, TokenValidationParameters validationParameters);

    /// <summary>
    /// Definition for IssuerSigningKeyResolver.
    /// </summary>
    /// <param name="token">The <see cref="string"/> representation of the token that is being validated.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> that is being validated. It may be null.</param>
    /// <param name="kid">A key identifier. It may be null.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    /// <returns>A <see cref="SecurityKey"/> to use when validating a signature.</returns>
    /// <remarks> If both <see cref="IssuerSigningKeyResolverUsingConfiguration"/> and <see cref="IssuerSigningKeyResolver"/> are set, IssuerSigningKeyResolverUsingConfiguration takes
    /// priority.</remarks>
    public delegate IEnumerable<SecurityKey> IssuerSigningKeyResolver(string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters);

    /// <summary>
    /// Definition for IssuerSigningKeyResolverUsingConfiguration.
    /// </summary>
    /// <param name="token">The <see cref="string"/> representation of the token that is being validated.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> that is being validated. It may be null.</param>
    /// <param name="kid">A key identifier. It may be null.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    /// <param name="configuration"><see cref="BaseConfiguration"/> required for validation.</param>
    /// <returns>A <see cref="SecurityKey"/> to use when validating a signature.</returns>
    /// <remarks> If both <see cref="IssuerSigningKeyResolverUsingConfiguration"/> and <see cref="IssuerSigningKeyResolver"/> are set, IssuerSigningKeyResolverUsingConfiguration takes
    /// priority.</remarks>
    public delegate IEnumerable<SecurityKey> IssuerSigningKeyResolverUsingConfiguration(string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters, BaseConfiguration configuration);

    /// <summary>
    /// Definition for IssuerSigningKeyValidator.
    /// </summary>
    /// <param name="securityKey">The <see cref="SecurityKey"/> that signed the <see cref="SecurityToken"/>.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    /// <remarks> If both <see cref="IssuerSigningKeyResolverUsingConfiguration"/> and <see cref="IssuerSigningKeyResolver"/> are set, IssuerSigningKeyResolverUsingConfiguration takes
    /// priority.</remarks>
    public delegate bool IssuerSigningKeyValidator(SecurityKey securityKey, SecurityToken securityToken, TokenValidationParameters validationParameters);

    /// <summary>
    /// Definition for IssuerSigningKeyValidatorUsingConfiguration.
    /// </summary>
    /// <param name="securityKey">The <see cref="SecurityKey"/> that signed the <see cref="SecurityToken"/>.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    /// <param name="configuration"><see cref="BaseConfiguration"/> required for validation.</param>
    /// <remarks> If both <see cref="IssuerSigningKeyResolverUsingConfiguration"/> and <see cref="IssuerSigningKeyResolver"/> are set, IssuerSigningKeyResolverUsingConfiguration takes
    /// priority.</remarks>
    public delegate bool IssuerSigningKeyValidatorUsingConfiguration(SecurityKey securityKey, SecurityToken securityToken, TokenValidationParameters validationParameters, BaseConfiguration configuration);

    /// <summary>
    /// Definition for IssuerValidator.
    /// </summary>
    /// <param name="issuer">The issuer to validate.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> that is being validated.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    /// <returns>The issuer to use when creating the "Claim"(s) in a "ClaimsIdentity".</returns>
    /// <remarks>The delegate should return a non null string that represents the 'issuer'. If null a default value will be used.
    /// If both <see cref="IssuerValidatorUsingConfiguration"/> and <see cref="IssuerValidator"/> are set, IssuerValidatorUsingConfiguration takes
    /// priority.</remarks>
    public delegate string IssuerValidator(string issuer, SecurityToken securityToken, TokenValidationParameters validationParameters);

    /// <summary>
    /// Definition for IssuerValidatorUsingConfiguration.
    /// </summary>
    /// <param name="issuer">The issuer to validate.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> that is being validated.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    /// <param name="configuration"><see cref="BaseConfiguration"/> required for validation.</param>
    /// <returns>The issuer to use when creating the "Claim"(s) in a "ClaimsIdentity".</returns>
    /// <remarks>The delegate should return a non null string that represents the 'issuer'. If null a default value will be used.
    /// If both <see cref="IssuerValidatorUsingConfiguration"/> and <see cref="IssuerValidator"/> are set, IssuerValidatorUsingConfiguration takes
    /// priority.
    /// </remarks>
    public delegate string IssuerValidatorUsingConfiguration(string issuer, SecurityToken securityToken, TokenValidationParameters validationParameters, BaseConfiguration configuration);

    /// <summary>
    /// Definition for IssuerValidatorAsync. Left internal for now while we work out the details of async validation for all delegates.
    /// </summary>
    /// <param name="issuer">The issuer to validate.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> that is being validated.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    /// <returns>The issuer to use when creating the "Claim"(s) in a "ClaimsIdentity".</returns>
    /// <remarks>The delegate should return a non null string that represents the 'issuer'. If null a default value will be used.
    /// <see cref="IssuerValidatorAsync"/> if set, will be called before <see cref="IssuerSigningKeyValidatorUsingConfiguration"/> or <see cref="IssuerSigningKeyValidator"/>
    /// </remarks>
    internal delegate ValueTask<string> IssuerValidatorAsync(string issuer, SecurityToken securityToken, TokenValidationParameters validationParameters);

    /// <summary>
    /// Definition for LifetimeValidator.
    /// </summary>
    /// <param name="notBefore">The 'notBefore' time found in the <see cref="SecurityToken"/>.</param>
    /// <param name="expires">The 'expiration' time found in the <see cref="SecurityToken"/>.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    public delegate bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters);

    /// <summary>
    /// Definition for TokenReplayValidator.
    /// </summary>
    /// <param name="expirationTime">The 'expiration' time found in the <see cref="SecurityToken"/>.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    /// <returns></returns>
    public delegate bool TokenReplayValidator(DateTime? expirationTime, string securityToken, TokenValidationParameters validationParameters);

    /// <summary>
    /// Definition for SignatureValidator.
    /// </summary>
    /// <param name="token">A securityToken with a signature.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    public delegate SecurityToken SignatureValidator(string token, TokenValidationParameters validationParameters);

    /// <summary>
    /// Definition for SignatureValidator.
    /// </summary>
    /// <param name="token">A securityToken with a signature.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    /// <param name="configuration">The <see cref="BaseConfiguration"/> that is required for validation.</param>
    public delegate SecurityToken SignatureValidatorUsingConfiguration(string token, TokenValidationParameters validationParameters, BaseConfiguration configuration);

    /// <summary>
    /// Definition for TokenReader.
    /// </summary>
    /// <param name="token">A securityToken with a signature.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    public delegate SecurityToken TokenReader(string token, TokenValidationParameters validationParameters);

    /// <summary>
    /// Definition for TokenDecryptionKeyResolver.
    /// </summary>
    /// <param name="token">The <see cref="string"/> representation of the token to be decrypted.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> to be decrypted. The runtime by default passes null.</param>
    /// <param name="kid">A key identifier. It may be null.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    /// <returns>A <see cref="SecurityKey"/> to use when decrypting the token.</returns>
    public delegate IEnumerable<SecurityKey> TokenDecryptionKeyResolver(string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters);

    /// <summary>
    /// Definition for TypeValidator.
    /// </summary>
    /// <param name="type">The token type to validate.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> that is being validated.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    /// <returns>The actual token type, that may be the same as <paramref name="type"/> or a different value if the token type was resolved from a different location.</returns>
    public delegate string TypeValidator(string type, SecurityToken securityToken, TokenValidationParameters validationParameters);

    /// <summary>
    /// Definition for TransformBeforeSignatureValidation.
    /// </summary>
    /// <param name="token">The <see cref="SecurityToken"/> that is being validated.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    /// <returns>A transformed <see cref="SecurityToken"/>.</returns>
    public delegate SecurityToken TransformBeforeSignatureValidation(SecurityToken token, TokenValidationParameters validationParameters);

    /// <summary>
    /// Contains a set of parameters that are used by a <see cref="SecurityTokenHandler"/> when validating a <see cref="SecurityToken"/>.
    /// </summary>
    public class TokenValidationParameters
    {
        private string _authenticationType;
        private TimeSpan _clockSkew = DefaultClockSkew;
        private string _nameClaimType = ClaimsIdentity.DefaultNameClaimType;
        private string _roleClaimType = ClaimsIdentity.DefaultRoleClaimType;
        private Dictionary<string, object> _instancePropertyBag;

        /// <summary>
        /// This is the default value of <see cref="ClaimsIdentity.AuthenticationType"/> when creating a <see cref="ClaimsIdentity"/>.
        /// The value is <c>"AuthenticationTypes.Federation"</c>.
        /// To change the value, set <see cref="AuthenticationType"/> to a different value.
        /// </summary>
        public static readonly string DefaultAuthenticationType = "AuthenticationTypes.Federation"; // Note: The change was because 5.x removed the dependency on System.IdentityModel and we used a different string which was a mistake.

        /// <summary>
        /// Default for the clock skew.
        /// </summary>
        /// <remarks>300 seconds (5 minutes).</remarks>
        public static readonly TimeSpan DefaultClockSkew = TimeSpan.FromSeconds(300); // 5 min.

        /// <summary>
        /// Default for the maximum token size.
        /// </summary>
        /// <remarks>250 KB (kilobytes).</remarks>
        public const Int32 DefaultMaximumTokenSizeInBytes = 1024 * 250;

        /// <summary>
        /// Copy constructor for <see cref="TokenValidationParameters"/>.
        /// </summary>
        protected TokenValidationParameters(TokenValidationParameters other)
        {
            if (other == null)
                throw LogHelper.LogExceptionMessage(new ArgumentNullException(nameof(other)));

            AlgorithmValidator = other.AlgorithmValidator;
            ActorValidationParameters = other.ActorValidationParameters?.Clone();
            AudienceValidator = other.AudienceValidator;
            _authenticationType = other._authenticationType;
            ClockSkew = other.ClockSkew;
            ConfigurationManager = other.ConfigurationManager;
            CryptoProviderFactory = other.CryptoProviderFactory;
            DebugId = other.DebugId;
            IncludeTokenOnFailedValidation = other.IncludeTokenOnFailedValidation;
            IgnoreTrailingSlashWhenValidatingAudience = other.IgnoreTrailingSlashWhenValidatingAudience;
            IssuerSigningKey = other.IssuerSigningKey;
            IssuerSigningKeyResolver = other.IssuerSigningKeyResolver;
            IssuerSigningKeyResolverUsingConfiguration = other.IssuerSigningKeyResolverUsingConfiguration;
            IssuerSigningKeys = other.IssuerSigningKeys;
            IssuerSigningKeyValidator = other.IssuerSigningKeyValidator;
            IssuerSigningKeyValidatorUsingConfiguration = other.IssuerSigningKeyValidatorUsingConfiguration;
            IssuerValidator = other.IssuerValidator;
            IssuerValidatorAsync = other.IssuerValidatorAsync;
            IssuerValidatorUsingConfiguration = other.IssuerValidatorUsingConfiguration;
            LifetimeValidator = other.LifetimeValidator;
            LogTokenId = other.LogTokenId;
            LogValidationExceptions = other.LogValidationExceptions;
            NameClaimType = other.NameClaimType;
            NameClaimTypeRetriever = other.NameClaimTypeRetriever;
            PropertyBag = other.PropertyBag;
            RefreshBeforeValidation = other.RefreshBeforeValidation;
            RequireAudience = other.RequireAudience;
            RequireExpirationTime = other.RequireExpirationTime;
            RequireSignedTokens = other.RequireSignedTokens;
            RoleClaimType = other.RoleClaimType;
            RoleClaimTypeRetriever = other.RoleClaimTypeRetriever;
            SaveSigninToken = other.SaveSigninToken;
            SignatureValidator = other.SignatureValidator;
            SignatureValidatorUsingConfiguration = other.SignatureValidatorUsingConfiguration;
            TokenDecryptionKey = other.TokenDecryptionKey;
            TokenDecryptionKeyResolver = other.TokenDecryptionKeyResolver;
            TokenDecryptionKeys = other.TokenDecryptionKeys;
            TokenReader = other.TokenReader;
            TokenReplayCache = other.TokenReplayCache;
            TokenReplayValidator = other.TokenReplayValidator;
            TransformBeforeSignatureValidation = other.TransformBeforeSignatureValidation;
            TryAllIssuerSigningKeys = other.TryAllIssuerSigningKeys;
            TypeValidator = other.TypeValidator;
            ValidateActor = other.ValidateActor;
            ValidateAudience = other.ValidateAudience;
            ValidateIssuer = other.ValidateIssuer;
            ValidateIssuerSigningKey = other.ValidateIssuerSigningKey;
            ValidateLifetime = other.ValidateLifetime;
            ValidateSignatureLast = other.ValidateSignatureLast;
            ValidateTokenReplay = other.ValidateTokenReplay;
            ValidateWithLKG = other.ValidateWithLKG;
            ValidAlgorithms = other.ValidAlgorithms;
            ValidAudience = other.ValidAudience;
            ValidAudiences = other.ValidAudiences;
            ValidIssuer = other.ValidIssuer;
            ValidIssuers = other.ValidIssuers;
            ValidTypes = other.ValidTypes;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenValidationParameters"/> class.
        /// </summary>
        public TokenValidationParameters()
        {
            LogTokenId = true;
            LogValidationExceptions = true;
            RequireExpirationTime = true;
            RequireSignedTokens = true;
            RequireAudience = true;
            SaveSigninToken = false;
            TryAllIssuerSigningKeys = true;
            ValidateActor = false;
            ValidateAudience = true;
            ValidateIssuer = true;
            ValidateIssuerSigningKey = false;
            ValidateLifetime = true;
            ValidateTokenReplay = false;
        }

        /// <summary>
        /// Gets or sets <see cref="TokenValidationParameters"/>.
        /// </summary>
        public TokenValidationParameters ActorValidationParameters { get; set; }

        /// <summary>
        /// Gets or sets a delegate used to validate the cryptographic algorithm used.
        /// </summary>
        /// <remarks>
        /// If set, this delegate will validate the cryptographic algorithm used and
        /// the algorithm will not be checked against <see cref="ValidAlgorithms"/>.
        /// </remarks>
        public AlgorithmValidator AlgorithmValidator { get; set; }

        /// <summary>
        /// Gets or sets a delegate that will be used to validate the audience.
        /// </summary>
        /// <remarks>
        /// If set, this delegate will be called to validate the 'audience', instead of default processing.
        /// This means that no default 'audience' validation will occur.
        /// Even if <see cref="ValidateAudience"/> is false, this delegate will still be called.
        /// </remarks>
        public AudienceValidator AudienceValidator { get; set; }

        /// <summary>
        /// Gets or sets the AuthenticationType when creating a <see cref="ClaimsIdentity"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException">If 'value' is null or whitespace.</exception>
        public string AuthenticationType
        {
            get
            {
                return _authenticationType;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw LogHelper.LogExceptionMessage(new ArgumentNullException("AuthenticationType"));
                }

                _authenticationType = value;
            }
        }

        ///// <summary>
        ///// Gets or sets the <see cref="X509CertificateValidator"/> for validating X509Certificate2(s).
        ///// </summary>
        //public X509CertificateValidator CertificateValidator
        //{
        //    get
        //    {
        //        return _certificateValidator;
        //    }

        //    set
        //    {
        //        _certificateValidator = value;
        //    }
        //}

        /// <summary>
        /// Gets or sets the clock skew to apply when validating a time.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">If 'value' is less than 0.</exception>
        /// The default is <c>300</c> seconds (5 minutes).
        [DefaultValue(300)]
        public TimeSpan ClockSkew
        {
            get
            {
                return _clockSkew;
            }

            set
            {
                if (value < TimeSpan.Zero)
                    throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(value), LogHelper.FormatInvariant(LogMessages.IDX10100, LogHelper.MarkAsNonPII(value))));

                _clockSkew = value;
            }
        }

        /// <summary>
        /// Returns a new instance of <see cref="TokenValidationParameters"/> with values copied from this object.
        /// </summary>
        /// <returns>A new <see cref="TokenValidationParameters"/> object copied from this object</returns>
        /// <remarks>This is a shallow Clone.</remarks>
        public virtual TokenValidationParameters Clone()
        {
            return new(this)
            {
                IsClone = true
            };
        }

        /// <summary>
        /// Creates a <see cref="ClaimsIdentity"/> using:
        /// <para><see cref="AuthenticationType"/></para>
        /// <para>'NameClaimType': If NameClaimTypeRetriever is set, call delegate, else call NameClaimType. If the result is a null or empty string, use <see cref="ClaimsIdentity.DefaultNameClaimType"/></para>.
        /// <para>'RoleClaimType': If RoleClaimTypeRetriever is set, call delegate, else call RoleClaimType. If the result is a null or empty string, use <see cref="ClaimsIdentity.DefaultRoleClaimType"/></para>.
        /// </summary>
        /// <returns>A <see cref="ClaimsIdentity"/> with Authentication, NameClaimType and RoleClaimType set.</returns>
        public virtual ClaimsIdentity CreateClaimsIdentity(SecurityToken securityToken, string issuer)
        {
            string nameClaimType = null;
            if (NameClaimTypeRetriever != null)
            {
                nameClaimType = NameClaimTypeRetriever(securityToken, issuer);
            }
            else
            {
                nameClaimType = NameClaimType;
            }

            string roleClaimType = null;
            if (RoleClaimTypeRetriever != null)
            {
                roleClaimType = RoleClaimTypeRetriever(securityToken, issuer);
            }
            else
            {
                roleClaimType = RoleClaimType;
            }

            if (LogHelper.IsEnabled(EventLogLevel.Informational))
                LogHelper.LogInformation(LogMessages.IDX10245, securityToken);

            return new ClaimsIdentity(authenticationType: AuthenticationType ?? DefaultAuthenticationType, nameType: nameClaimType ?? ClaimsIdentity.DefaultNameClaimType, roleType: roleClaimType ?? ClaimsIdentity.DefaultRoleClaimType);
        }

        /// <summary>
        /// If set, this property will be used to obtain the issuer and signing keys associated with the metadata endpoint of <see cref="BaseConfiguration.Issuer"/>.
        /// The obtained issuer and signing keys will then be used along with those present on the TokenValidationParameters for validation of the incoming token.
        /// </summary>
        public BaseConfigurationManager ConfigurationManager { get; set; }

        /// <summary>
        /// Users can override the default <see cref="CryptoProviderFactory"/> with this property. This factory will be used for creating signature providers.
        /// </summary>
        public CryptoProviderFactory CryptoProviderFactory { get; set; }

        /// <summary>
        /// Gets or sets a string that helps with setting breakpoints when debugging.
        /// </summary>
        public string DebugId { get; set; }

        /// <summary>
        /// Gets or sets a boolean that controls if a '/' is significant at the end of the audience.
        /// The default is <c>true</c>.
        /// </summary>
        [DefaultValue(true)]
        public bool IgnoreTrailingSlashWhenValidatingAudience { get; set; } = true;

        /// <summary>
        /// Gets or sets the flag that indicates whether to include the <see cref="SecurityToken"/> when the validation fails.
        /// </summary>
        public bool IncludeTokenOnFailedValidation { get; set; } = false;

        /// <summary>
        /// Gets or sets a delegate for validating the <see cref="SecurityKey"/> that signed the token.
        /// </summary>
        /// <remarks>
        /// If set, this delegate will be called to validate the <see cref="SecurityKey"/> that signed the token, instead of default processing.
        /// This means that no default <see cref="SecurityKey"/> validation will occur.
        /// Even if <see cref="ValidateIssuerSigningKey"/> is false, this delegate will still be called.
        /// If both <see cref="IssuerSigningKeyValidatorUsingConfiguration"/> and <see cref="IssuerSigningKeyValidator"/> are set, IssuerSigningKeyResolverUsingConfiguration takes
        /// priority.
        /// </remarks>
        public IssuerSigningKeyValidator IssuerSigningKeyValidator { get; set; }

        /// <summary>
        /// Gets or sets a delegate for validating the <see cref="SecurityKey"/> that signed the token.
        /// </summary>
        /// <remarks>
        /// If set, this delegate will be called to validate the <see cref="SecurityKey"/> that signed the token, instead of default processing.
        /// This means that no default <see cref="SecurityKey"/> validation will occur.
        /// Even if <see cref="ValidateIssuerSigningKey"/> is false, this delegate will still be called.
        /// This delegate should be used if properties from the configuration retrieved from the authority are necessary to validate the
        /// issuer signing key.
        /// If both <see cref="IssuerSigningKeyValidatorUsingConfiguration"/> and <see cref="IssuerSigningKeyValidator"/> are set, IssuerSigningKeyValidatorUsingConfiguration takes
        /// priority.
        /// </remarks>
        public IssuerSigningKeyValidatorUsingConfiguration IssuerSigningKeyValidatorUsingConfiguration { get; set; }

        /// <summary>
        /// Gets a <see cref="IDictionary{String, Object}"/> that is unique to this instance.
        /// Calling <see cref="Clone"/> will result in a new instance of this IDictionary.
        /// </summary>
        public IDictionary<string, object> InstancePropertyBag => _instancePropertyBag ??= new Dictionary<string, object>();

        /// <summary>
        /// Gets a value indicating if <see cref="Clone"/> was called to obtain this instance.
        /// </summary>
        public bool IsClone { get; protected set; } = false;

        /// <summary>
        /// Gets or sets the <see cref="SecurityKey"/> that is to be used for signature validation.
        /// </summary>
        public SecurityKey IssuerSigningKey { get; set; }

        /// <summary>
        /// Gets or sets a delegate that will be called to retrieve a <see cref="SecurityKey"/> used for signature validation.
        /// </summary>
        /// <remarks>
        /// This <see cref="SecurityKey"/> will be used to check the signature. This can be helpful when the <see cref="SecurityToken"/> does not contain a key identifier.
        /// If both <see cref="IssuerSigningKeyResolverUsingConfiguration"/> and <see cref="IssuerSigningKeyResolver"/> are set, IssuerSigningKeyResolverUsingConfiguration takes
        /// priority.
        /// </remarks>
        public IssuerSigningKeyResolver IssuerSigningKeyResolver { get; set; }

        /// <summary>
        /// Gets or sets a delegate that will be called to retrieve a <see cref="SecurityKey"/> used for signature validation using the
        /// <see cref="TokenValidationParameters"/> and <see cref="BaseConfiguration"/>.
        /// </summary>
        /// <remarks>
        /// This <see cref="SecurityKey"/> will be used to check the signature. This can be helpful when the <see cref="SecurityToken"/> does not contain a key identifier.
        /// This delegate should be used if properties from the configuration retrieved from the authority are necessary to resolve the
        /// issuer signing key.
        /// If both <see cref="IssuerSigningKeyResolverUsingConfiguration"/> and <see cref="IssuerSigningKeyResolver"/> are set, IssuerSigningKeyResolverUsingConfiguration takes
        /// priority.
        /// </remarks>
        public IssuerSigningKeyResolverUsingConfiguration IssuerSigningKeyResolverUsingConfiguration { get; set; }

        /// <summary>
        /// Gets or sets an <see cref="IEnumerable{SecurityKey}"/> used for signature validation.
        /// </summary>
        public IEnumerable<SecurityKey> IssuerSigningKeys { get; set; }

        /// <summary>
        /// Gets or sets a delegate that will be used to validate the issuer of the token.
        /// </summary>
        /// <remarks>
        /// If set, this delegate will be called to validate the 'issuer' of the token, instead of default processing.
        /// This means that no default 'issuer' validation will occur.
        /// Even if <see cref="ValidateIssuer"/> is false, this delegate will still be called.
        /// If both <see cref="IssuerValidatorUsingConfiguration"/> and <see cref="IssuerValidator"/> are set, IssuerValidatorUsingConfiguration takes
        /// priority. 
        /// </remarks>
        public IssuerValidator IssuerValidator { get; set; }


        /// <summary>
        /// Gets or sets a delegate that will be used to validate the issuer of the token.
        /// </summary>
        /// <remarks>
        /// If set, this delegate will be called to validate the 'issuer' of the token, instead of default processing.
        /// This means that no default 'issuer' validation will occur.
        /// Even if <see cref="ValidateIssuer"/> is false, this delegate will still be called.
        /// IssuerValidatorAsync takes precedence over <see cref="IssuerValidatorUsingConfiguration"/> and <see cref="IssuerValidator"/>.
        /// </remarks>
        internal IssuerValidatorAsync IssuerValidatorAsync { get; set; }

        /// <summary>
        /// Gets or sets a delegate that will be used to validate the issuer of the token.
        /// </summary>
        /// <remarks>
        /// If set, this delegate will be called to validate the 'issuer' of the token, instead of default processing.
        /// This means that no default 'issuer' validation will occur.
        /// Even if <see cref="ValidateIssuer"/> is false, this delegate will still be called.
        /// This delegate should be used if properties from the configuration retrieved from the authority are necessary to validate the issuer.
        /// If both <see cref="IssuerValidatorUsingConfiguration"/> and <see cref="IssuerValidator"/> are set, IssuerValidatorUsingConfiguration takes
        /// priority.
        /// </remarks>
        public IssuerValidatorUsingConfiguration IssuerValidatorUsingConfiguration { get; set; }

        /// <summary>
        /// Gets or sets a delegate that will be called to transform a token to a supported format before validation.
        /// </summary>
        public TransformBeforeSignatureValidation TransformBeforeSignatureValidation { get; set; }

        /// <summary>
        /// Gets or sets a delegate that will be used to validate the lifetime of the token
        /// </summary>
        /// <remarks>
        /// If set, this delegate will be called to validate the lifetime of the token, instead of default processing.
        /// This means that no default lifetime validation will occur.
        /// Even if <see cref="ValidateLifetime"/> is false, this delegate will still be called.
        /// </remarks>
        public LifetimeValidator LifetimeValidator { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> that will decide if the token identifier claim needs to be logged.
        /// Default value is <c>true</c>.
        /// </summary>
        [DefaultValue(true)]
        public bool LogTokenId { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> that will decide if validation failure needs to be logged as an error.
        /// Default value is <c>true</c> for backward compatibility of the behavior.
        /// If set to false, validation failures are logged as Information and then thrown.
        /// </summary>
        [DefaultValue(true)]
        public bool LogValidationExceptions { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="string"/> that defines the <see cref="ClaimsIdentity.NameClaimType"/>.
        /// </summary>
        /// <remarks>
        /// Controls the value <see cref="ClaimsIdentity.Name"/> returns. It will return the first <see cref="Claim.Value"/> where the <see cref="Claim.Type"/> equals <see cref="NameClaimType"/>.
        /// The default is <see cref="ClaimsIdentity.DefaultNameClaimType"/>.
        /// </remarks>
        public string NameClaimType
        {
            get
            {
                return _nameClaimType;
            }

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(value), LogMessages.IDX10102));

                _nameClaimType = value;
            }
        }

        /// <summary>
        /// Gets or sets a delegate that will be called to set the property <see cref="ClaimsIdentity.NameClaimType"/> after validating a token.
        /// </summary>
        /// <remarks>
        /// The function will be passed:
        /// <para>The <see cref="SecurityToken"/> that is being validated.</para>
        /// <para>The issuer associated with the token.</para>
        /// <para>Returns the value that will set the property <see cref="ClaimsIdentity.NameClaimType"/>.</para>
        /// </remarks>
        public Func<SecurityToken, string, string> NameClaimTypeRetriever { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IDictionary{String, Object}"/> that contains a collection of custom key/value pairs. This allows addition of parameters that could be used in custom token validation scenarios.
        /// </summary>
        public IDictionary<string, object> PropertyBag { get; set; }

        /// <summary>
        /// Gets or sets a boolean to control if configuration required to be refreshed before token validation.
        /// </summary>
        /// <remarks>
        /// The default is <c>false</c>.
        /// </remarks>
        [DefaultValue(false)]
        public bool RefreshBeforeValidation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether SAML tokens must have at least one AudienceRestriction.
        /// The default is <c>true</c>.
        /// </summary>
        [DefaultValue(true)]
        public bool RequireAudience { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether tokens must have an 'expiration' value.
        /// The default is <c>true</c>.
        /// </summary>
        [DefaultValue(true)]
        public bool RequireExpirationTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a <see cref="SecurityToken"/> can be considered valid if not signed.
        /// The default is <c>true</c>.
        /// </summary>
        [DefaultValue(true)]
        public bool RequireSignedTokens { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="string"/> that defines the <see cref="ClaimsIdentity.RoleClaimType"/>.
        /// </summary>
        /// <remarks>
        /// <para>Controls the results of <see cref="ClaimsPrincipal.IsInRole( string )"/>.</para>
        /// <para>Each <see cref="Claim"/> where <see cref="Claim.Type"/> == <see cref="RoleClaimType"/> will be checked for a match against the 'string' passed to <see cref="ClaimsPrincipal.IsInRole(string)"/>.</para>
        /// The default is <see cref="ClaimsIdentity.DefaultRoleClaimType"/>.
        /// </remarks>
        public string RoleClaimType
        {
            get
            {
                return _roleClaimType;
            }

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(value), LogMessages.IDX10103));

                _roleClaimType = value;
            }
        }

        /// <summary>
        /// Gets or sets a delegate that will be called to set the property <see cref="ClaimsIdentity.RoleClaimType"/> after validating a token.
        /// </summary>
        /// <remarks>
        /// The function will be passed:
        /// <para>The <see cref="SecurityToken"/> that is being validated.</para>
        /// <para>The issuer associated with the token.</para>
        /// <para>Returns the value that will set the property <see cref="ClaimsIdentity.RoleClaimType"/>.</para>
        /// </remarks>
        public Func<SecurityToken, string, string> RoleClaimTypeRetriever { get; set; }

        /// <summary>
        /// Gets or sets a boolean to control if the original token should be saved after the security token is validated.
        /// </summary>
        /// <remarks>The runtime will consult this value and save the original token that was validated.
        /// The default is <c>false</c>.
        /// </remarks>
        [DefaultValue(false)]
        public bool SaveSigninToken { get; set; }

        /// <summary>
        /// Gets or sets a delegate that will be used to validate the signature of the token.
        /// </summary>
        /// <remarks>
        /// If set, this delegate will be called to validate the signature of the token, instead of default processing.
        /// </remarks>
        public SignatureValidator SignatureValidator { get; set; }

        /// <summary>
        /// Gets or sets a delegate that will be used to validate the signature of the token using the <see cref="TokenValidationParameters"/> and
        /// the <see cref="BaseConfiguration"/>.
        /// </summary>
        /// <remarks>
        /// If set, this delegate will be called to validate the signature of the token, instead of default processing.
        /// </remarks>
        public SignatureValidatorUsingConfiguration SignatureValidatorUsingConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="SecurityKey"/> that is to be used for decryption.
        /// </summary>
        public SecurityKey TokenDecryptionKey { get; set; }

        /// <summary>
        /// Gets or sets a delegate that will be called to retreive a <see cref="SecurityKey"/> used for decryption.
        /// </summary>
        /// <remarks>
        /// This <see cref="SecurityKey"/> will be used to decrypt the token. This can be helpful when the <see cref="SecurityToken"/> does not contain a key identifier.
        /// </remarks>
        public TokenDecryptionKeyResolver TokenDecryptionKeyResolver { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IEnumerable{SecurityKey}"/> that is to be used for decrypting inbound tokens.
        /// </summary>
        public IEnumerable<SecurityKey> TokenDecryptionKeys { get; set; }

        /// <summary>
        /// Gets or sets a delegate that will be used to read the token.
        /// </summary>
        /// <remarks>
        /// If set, this delegate will be called to read the token instead of default processing.
        /// </remarks>
        public TokenReader TokenReader { get; set; }

        /// <summary>
        /// Gets or set the <see cref="ITokenReplayCache"/> that store tokens that can be checked to help detect token replay.
        /// </summary>
        /// <remarks>If set, then tokens must have an expiration time or the runtime will fault.</remarks>
        public ITokenReplayCache TokenReplayCache { get; set; }

        /// <summary>
        /// Gets or sets a delegate that will be used to validate the token replay of the token
        /// </summary>
        /// <remarks>
        /// If set, this delegate will be called to validate the token replay of the token, instead of default processing.
        /// This means no default token replay validation will occur.
        /// Even if <see cref="ValidateTokenReplay"/> is false, this delegate will still be called.
        /// </remarks>
        public TokenReplayValidator TokenReplayValidator { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether all <see cref="IssuerSigningKeys"/> should be tried during signature validation when a key is not matched to token kid or if token kid is empty.
        /// The default is <c>true</c>.
        /// </summary>
        [DefaultValue(true)]
        public bool TryAllIssuerSigningKeys { get; set; }

        /// <summary>
        /// Gets or sets a delegate that will be used to validate the type of the token.
        /// If the token type cannot be validated, an exception MUST be thrown by the delegate.
        /// Note: the 'type' parameter may be null if it couldn't be extracted from its usual location.
        /// Implementations that need to resolve it from a different location can use the 'token' parameter.
        /// </summary>
        /// <remarks>
        /// If set, this delegate will be called to validate the 'type' of the token, instead of default processing.
        /// This means that no default 'type' validation will occur.
        /// </remarks>
        public TypeValidator TypeValidator { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if an actor token is detected, whether it should be validated.
        /// The default is <c>false</c>.
        /// </summary>
        [DefaultValue(false)]
        public bool ValidateActor { get; set; }

        /// <summary>
        /// Gets or sets a boolean to control if the audience will be validated during token validation.
        /// </summary>
        /// <remarks>Validation of the audience, mitigates forwarding attacks. For example, a site that receives a token, could not replay it to another site.
        /// A forwarded token would contain the audience of the original site.
        /// This boolean only applies to default audience validation. If <see cref="AudienceValidator"/> is set, it will be called regardless of whether this
        /// property is true or false.
        /// The default is <c>true</c>.
        /// </remarks>
        [DefaultValue(true)]
        public bool ValidateAudience { get; set; }

        /// <summary>
        /// Gets or sets a boolean to control if the issuer will be validated during token validation.
        /// </summary>
        /// <remarks>
        /// Validation of the issuer mitigates forwarding attacks that can occur when an
        /// IdentityProvider represents multiple tenants and signs tokens with the same keys.
        /// It is possible that a token issued for the same audience could be from a different tenant. For example an application could accept users from
        /// contoso.onmicrosoft.com but not fabrikam.onmicrosoft.com, both valid tenants. An application that accepts tokens from fabrikam could forward them
        /// to the application that accepts tokens for contoso.
        /// This boolean only applies to default issuer validation. If <see cref="IssuerValidator"/> is set, it will be called regardless of whether this
        /// property is true or false.
        /// The default is <c>true</c>.
        /// </remarks>
        [DefaultValue(true)]
        public bool ValidateIssuer { get; set; }

        /// <summary>
        /// Gets or sets a boolean to control if the LKG configuration will be used for token validation.
        /// </summary>
        /// <remarks>
        /// The default is <c>false</c>.
        /// </remarks>
        [DefaultValue(false)]
        public bool ValidateWithLKG { get; set; }

        /// <summary>
        /// Gets or sets a boolean that controls if validation of the <see cref="SecurityKey"/> that signed the securityToken is called.
        /// </summary>
        /// <remarks>It is possible for tokens to contain the public key needed to check the signature. For example, X509Data can be hydrated into an X509Certificate,
        /// which can be used to validate the signature. In these cases it is important to validate the SigningKey that was used to validate the signature. 
        /// This boolean only applies to default signing key validation. If <see cref= "IssuerSigningKeyValidator" /> is set, it will be called regardless of whether this
        /// property is true or false.
        /// The default is <c>false</c>.
        /// </remarks>
        [DefaultValue(false)]
        public bool ValidateIssuerSigningKey { get; set; }

        /// <summary>
        /// Gets or sets a boolean to control if the lifetime will be validated during token validation.
        /// </summary>
        /// <remarks>
        /// This boolean only applies to default lifetime validation. If <see cref= "LifetimeValidator" /> is set, it will be called regardless of whether this
        /// property is true or false.
        /// The default is <c>true</c>.
        /// </remarks>
        [DefaultValue(true)]
        public bool ValidateLifetime { get; set; }

        /// <summary>
        /// Gets or sets a boolean that controls the validation order of the payload and signature during token validation.
        /// </summary>
        /// <remarks>If <see cref= "ValidateSignatureLast" /> is set to true, it will validate payload ahead of signature.
        /// The default is <c>false</c>.
        /// </remarks>
        [DefaultValue(false)]
        public bool ValidateSignatureLast { get; set; }

        /// <summary>
        /// Gets or sets a boolean to control if the token replay will be validated during token validation.
        /// </summary> 
        /// <remarks>
        /// This boolean only applies to default token replay validation. If <see cref= "TokenReplayValidator" /> is set, it will be called regardless of whether this
        /// property is true or false.
        /// The default is <c>false</c>.
        /// </remarks>
        [DefaultValue(false)]
        public bool ValidateTokenReplay { get; set; }

        /// <summary>
        /// Gets or sets the valid algorithms for cryptographic operations.
        /// </summary>
        /// <remarks>
        /// If set to a non-empty collection, only the algorithms listed will be considered valid.
        /// The default is <c>null</c>.
        /// </remarks>
        public IEnumerable<string> ValidAlgorithms { get; set; }

        /// <summary>
        /// Gets or sets a string that represents a valid audience that will be used to check against the token's audience.
        /// The default is <c>null</c>.
        /// </summary>
        public string ValidAudience { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IEnumerable{String}"/> that contains valid audiences that will be used to check against the token's audience.
        /// The default is <c>null</c>.
        /// </summary>
        public IEnumerable<string> ValidAudiences { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="string"/> that represents a valid issuer that will be used to check against the token's issuer.
        /// The default is <c>null</c>.
        /// </summary>
        public string ValidIssuer { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IEnumerable{String}"/> that contains valid issuers that will be used to check against the token's issuer.
        /// The default is <c>null</c>.
        /// </summary>
        public IEnumerable<string> ValidIssuers { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IEnumerable{String}"/> that contains valid types that will be used to check against the JWT header's 'typ' claim.
        /// If this property is not set, the 'typ' header claim will not be validated and all types will be accepted.
        /// In the case of a JWE, this property will ONLY apply to the inner token header.
        /// The default is <c>null</c>.
        /// </summary>
        public IEnumerable<string> ValidTypes { get; set; }
    }
}
