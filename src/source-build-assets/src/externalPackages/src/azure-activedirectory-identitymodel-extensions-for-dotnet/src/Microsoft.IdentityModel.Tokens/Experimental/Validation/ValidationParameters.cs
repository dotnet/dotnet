// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Claims;
using System.Threading;
using Microsoft.IdentityModel.Logging;

#nullable enable
namespace Microsoft.IdentityModel.Tokens.Experimental
{
    /// <summary>
    /// Contains a set of parameters that are used by a <see cref="SecurityTokenHandler"/> when validating a <see cref="SecurityToken"/>.
    /// </summary>
    public class ValidationParameters
    {
        private string? _authenticationType;
        private TimeSpan _clockSkew = DefaultClockSkew;
        private string _nameClaimType = ClaimsIdentity.DefaultNameClaimType;
        private string _roleClaimType = ClaimsIdentity.DefaultRoleClaimType;
        private Dictionary<string, object>? _instancePropertyBag;
        private IList<SecurityKey>? _signingKeys;
        private Dictionary<string, object>? _propertyBag;
        private IList<SecurityKey>? _decryptionKeys;
        private IList<string>? _validIssuers;
        private IList<string>? _validTokenTypes;
        private IList<string>? _validAudiences;
        private IList<string>? _validAlgorithms;

        private AlgorithmValidationDelegate _algorithmValidator = Validators.ValidateAlgorithm;
        private AudienceValidationDelegate _audienceValidator = Validators.ValidateAudience;
        private IssuerValidationDelegateAsync _issuerValidatorAsync = Validators.ValidateIssuerAsync;
        private LifetimeValidationDelegate _lifetimeValidator = Validators.ValidateLifetime;
        private SignatureValidationDelegate? _signatureValidator;
        private TokenReplayValidationDelegate _tokenReplayValidator = Validators.ValidateTokenReplay;
        private TokenTypeValidationDelegate _tokenTypeValidator = Validators.ValidateTokenType;
        private SignatureKeyValidationDelegate _signatureKeyValidator = Validators.ValidateSignatureKey;

        /// <summary>
        /// This is the default value of <see cref="ClaimsIdentity.AuthenticationType"/> when creating a <see cref="ClaimsIdentity"/>.
        /// The value is <c>"AuthenticationTypes.Federation"</c>.
        /// To change the value, set <see cref="AuthenticationType"/> to a different value.
        /// </summary>
        public const string DefaultAuthenticationType = "AuthenticationTypes.Federation";

        /// <summary>
        /// Default for the clock skew.
        /// </summary>
        /// <remarks>300 seconds (5 minutes).</remarks>
        public static readonly TimeSpan DefaultClockSkew = TimeSpan.FromSeconds(300);

        /// <summary>
        /// Default for the maximum token size.
        /// </summary>
        /// <remarks>250 KB (kilobytes).</remarks>
        public const int DefaultMaximumTokenSizeInBytes = 1024 * 250;

        /// <summary>
        /// Copy constructor for <see cref="ValidationParameters"/>.
        /// </summary>
        protected ValidationParameters(ValidationParameters other)
        {
            if (other == null)
                throw LogHelper.LogExceptionMessage(new ArgumentNullException(nameof(other)));

            ActorValidationParameters = other.ActorValidationParameters;
            AlgorithmValidator = other.AlgorithmValidator;
            AudienceValidator = other.AudienceValidator;
            _authenticationType = other._authenticationType;
            ClockSkew = other.ClockSkew;
            ConfigurationManager = other.ConfigurationManager;
            CryptoProviderFactory = other.CryptoProviderFactory;
            DebugId = other.DebugId;
            IncludeTokenOnFailedValidation = other.IncludeTokenOnFailedValidation;
            IgnoreTrailingSlashWhenValidatingAudience = other.IgnoreTrailingSlashWhenValidatingAudience;
            SignatureKeyResolver = other.SignatureKeyResolver;
            _signingKeys = other.SigningKeys is not null ? new List<SecurityKey>(other.SigningKeys) : null;
            SignatureKeyValidator = other.SignatureKeyValidator;
            IssuerValidatorAsync = other.IssuerValidatorAsync;
            LifetimeValidator = other.LifetimeValidator;
            LogTokenId = other.LogTokenId;
            NameClaimType = other.NameClaimType;
            NameClaimTypeRetriever = other.NameClaimTypeRetriever;
            foreach (var keyValue in other.PropertyBag)
                PropertyBag[keyValue.Key] = keyValue.Value;

            RefreshBeforeValidation = other.RefreshBeforeValidation;
            RoleClaimType = other.RoleClaimType;
            RoleClaimTypeRetriever = other.RoleClaimTypeRetriever;
            SaveSigninToken = other.SaveSigninToken;
            _signatureValidator = other.SignatureValidator;
            TimeProvider = other.TimeProvider;
            TryAllDecryptionKeys = other.TryAllDecryptionKeys;
            DecryptionKeyResolver = other.DecryptionKeyResolver;
            _decryptionKeys = other.DecryptionKeys is not null ? new List<SecurityKey>(other.DecryptionKeys) : null;
            TokenReplayCache = other.TokenReplayCache;
            TokenReplayValidator = other.TokenReplayValidator;
            TokenTypeValidator = other.TokenTypeValidator;
            ValidateActor = other.ValidateActor;
            ValidateWithLKG = other.ValidateWithLKG;
            _validIssuers = other.ValidIssuers is not null ? new List<string>(other.ValidIssuers) : null;
            _validAudiences = other.ValidAudiences is not null ? new List<string>(other.ValidAudiences) : null;
            _validAlgorithms = other.ValidAlgorithms is not null ? new List<string>(other.ValidAlgorithms) : null;
            _validTokenTypes = other.ValidTypes is not null ? new List<string>(other.ValidTypes) : null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationParameters"/> class.
        /// </summary>
        public ValidationParameters()
        {
        }

        /// <summary>
        /// Gets or sets <see cref="ValidationParameters"/>.
        /// </summary>
        public ValidationParameters? ActorValidationParameters { get; set; }

        /// <summary>
        /// Allows overriding the delegate used to validate the cryptographic algorithm used.
        /// </summary>
        /// <remarks>
        /// If no delegate is set, the default implementation will be used. The default checks the algorithm
        /// against the <see cref="ValidAlgorithms"/> property, if present. If not, it will succeed.
        /// </remarks>
        public AlgorithmValidationDelegate AlgorithmValidator
        {
            get { return _algorithmValidator; }
            set { _algorithmValidator = value ?? throw new ArgumentNullException(nameof(value), "AlgorithmValidator cannot be null."); }
        }

        /// <summary>
        /// Allows overriding the delegate that will be used to validate the audience.
        /// </summary>
        /// <remarks>
        /// If set, this delegate will be responsible for validating the 'audience', instead of default processing.
        /// This means that no default 'audience' validation will occur.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when the value is set as null.</exception>
        /// <returns>The <see cref="AudienceValidationDelegate"/> used to validate the issuer of a token</returns>
        public AudienceValidationDelegate AudienceValidator
        {
            get { return _audienceValidator; }
            set { _audienceValidator = value ?? throw new ArgumentNullException(nameof(value), "AudienceValidator cannot be set as null."); }
        }

        /// <summary>
        /// Gets or sets the AuthenticationType when creating a <see cref="ClaimsIdentity"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException">If 'value' is null or whitespace.</exception>
        public string? AuthenticationType
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

                _authenticationType = value!;
            }
        }

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
        /// Returns a new instance of <see cref="ValidationParameters"/> with values copied from this object.
        /// </summary>
        /// <returns>A new <see cref="ValidationParameters"/> object copied from this object</returns>
        /// <remarks>This is a shallow Clone.</remarks>
        public virtual ValidationParameters Clone()
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
            string nameClaimType;
            if (NameClaimTypeRetriever != null)
            {
                nameClaimType = NameClaimTypeRetriever(securityToken, issuer);
            }
            else
            {
                nameClaimType = NameClaimType;
            }

            string roleClaimType;
            if (RoleClaimTypeRetriever != null)
            {
                roleClaimType = RoleClaimTypeRetriever(securityToken, issuer);
            }
            else
            {
                roleClaimType = RoleClaimType;
            }

            // TODO: Add to CallContext
            //if (LogHelper.IsEnabled(EventLogLevel.Informational))
            //    LogHelper.LogInformation(LogMessages.IDX10245, securityToken);

            return ClaimsIdentityFactory.Create(
                            authenticationType: AuthenticationType ?? DefaultAuthenticationType,
                            nameType: nameClaimType ?? ClaimsIdentity.DefaultNameClaimType,
                            roleType: roleClaimType ?? ClaimsIdentity.DefaultRoleClaimType,
                            securityToken);
        }

        /// <summary>
        /// If set, this property will be used to obtain the issuer and signing keys associated with the metadata endpoint of <see cref="BaseConfiguration.Issuer"/>.
        /// The obtained issuer and signing keys will then be used along with those present on the ValidationParameters for validation of the incoming token.
        /// </summary>
        public BaseConfigurationManager? ConfigurationManager { get; set; }

        /// <summary>
        /// Users can override the default <see cref="CryptoProviderFactory"/> with this property. This factory will be used for creating signature providers.
        /// </summary>
        public CryptoProviderFactory? CryptoProviderFactory { get; set; }

        /// <summary>
        /// Gets or sets a string that helps with setting breakpoints when debugging.
        /// </summary>
        public string? DebugId { get; set; }

        /// <summary>
        /// Gets or sets a boolean that controls if a '/' is significant at the end of the audience.
        /// The default is <c>true</c>.
        /// </summary>
        [DefaultValue(true)]
        public bool IgnoreTrailingSlashWhenValidatingAudience { get; set; } = true;

        /// <summary>
        /// Gets or sets the flag that indicates whether to include the <see cref="SecurityToken"/> when the validation fails.
        /// </summary>
        public bool IncludeTokenOnFailedValidation { get; set; }

        /// <summary>
        /// Gets or sets a delegate for validating the <see cref="SecurityKey"/> that signed the token.
        /// </summary>
        /// <remarks>
        /// If set, this delegate will be called to validate the <see cref="SecurityKey"/> that signed the token.
        /// </remarks>
        public SignatureKeyValidationDelegate SignatureKeyValidator
        {
            get => _signatureKeyValidator;
            set => _signatureKeyValidator = value ?? throw new ArgumentNullException(nameof(value), "SignatureKeyValidator cannot be set as null.");
        }

        /// <summary>
        /// Gets a <see cref="IDictionary{TKey, TValue}"/> that is unique to this instance.
        /// Calling <see cref="Clone"/> will result in a new instance of this IDictionary.
        /// </summary>
        public IDictionary<string, object> InstancePropertyBag =>
            _instancePropertyBag ??
            Interlocked.CompareExchange(ref _instancePropertyBag, [], null) ??
            _instancePropertyBag;

        /// <summary>
        /// Gets a value indicating if <see cref="Clone"/> was called to obtain this instance.
        /// </summary>
        public bool IsClone { get; protected set; }

        /// <summary>
        /// Gets or sets a delegate that will be called to retrieve a <see cref="SecurityKey"/> used for signature validation.
        /// </summary>
        /// <remarks>
        /// This <see cref="SecurityKey"/> will be used to check the signature. This can be helpful when the <see cref="SecurityToken"/> does not contain a key identifier.
        /// </remarks>
        public SignatureKeyResolverDelegate? SignatureKeyResolver { get; set; }

        /// <summary>
        /// Gets the <see cref="IList{T}"/> used for signature validation.
        /// </summary>
        public IList<SecurityKey> SigningKeys
        {
            get
            {
                return _signingKeys ??
                    Interlocked.CompareExchange(ref _signingKeys, [], null) ??
                    _signingKeys;
            }
        }

        /// <summary>
        /// Allows overriding the delegate that will be used to validate the issuer of the token.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the value is set as null.</exception>
        /// <returns>The <see cref="IssuerValidationDelegateAsync"/> used to validate the issuer of a token</returns>
        public IssuerValidationDelegateAsync IssuerValidatorAsync
        {
            get { return _issuerValidatorAsync; }
            set { _issuerValidatorAsync = value ?? throw new ArgumentNullException(nameof(value), "IssuerValidatorAsync cannot be set as null."); }
        }

        /// <summary>
        /// Allows overriding the delegate that will be used to validate the lifetime of the token
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the value is set as null.</exception>
        /// <returns>The <see cref="LifetimeValidationDelegate"/> used to validate the lifetime of a token</returns>
        public LifetimeValidationDelegate LifetimeValidator
        {
            get { return _lifetimeValidator; }
            set { _lifetimeValidator = value ?? throw new ArgumentNullException(nameof(value), "LifetimeValidator cannot be set as null."); }
        }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> that will decide if the token identifier claim needs to be logged.
        /// Default value is <c>true</c>.
        /// </summary>
        [DefaultValue(true)]
        public bool LogTokenId { get; set; } = true;

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
        public Func<SecurityToken, string, string>? NameClaimTypeRetriever { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IDictionary{TKey, TValue}"/> that contains a collection of custom key/value pairs.
        /// This allows addition of parameters that could be used in custom token validation scenarios.
        /// </summary>
        public IDictionary<string, object> PropertyBag => _propertyBag ??
            Interlocked.CompareExchange(ref _propertyBag, [], null) ??
            _propertyBag;

        /// <summary>
        /// A boolean to control whether configuration should be refreshed before validating a token.
        /// </summary>
        /// <remarks>
        /// The default is <c>false</c>.
        /// </remarks>
        [DefaultValue(false)]
        public bool RefreshBeforeValidation { get; set; }

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
        public Func<SecurityToken, string, string>? RoleClaimTypeRetriever { get; set; }

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
        public SignatureValidationDelegate? SignatureValidator
        {
            get { return _signatureValidator; }
            set { _signatureValidator = value; }
        }

        /// <summary>
        /// Gets or sets the time provider.
        /// </summary>
        internal TimeProvider TimeProvider { get; set; } = TimeProvider.System;

        /// <summary>
        /// Gets or sets a delegate that will be called to retrieve a <see cref="SecurityKey"/> used for decryption.
        /// </summary>
        /// <remarks>
        /// This <see cref="SecurityKey"/> will be used to decrypt the token. This can be helpful when the <see cref="SecurityToken"/> does not contain a key identifier.
        /// </remarks>
        internal DecryptionKeyResolverDelegate? DecryptionKeyResolver { get; set; }

        /// <summary>
        /// Gets the <see cref="IList{T}"/> that is to be used for decrypting tokens.
        /// </summary>
        /// <remarks>
        /// The decryption keys in this <see cref="DecryptionKeys"/> collection will only be used if their <see cref="SecurityKey.KeyId"/> matches the 'kid' parameter in the token.
        /// </remarks>
        public IList<SecurityKey> DecryptionKeys
        {
            get
            {
                return _decryptionKeys ??
                    Interlocked.CompareExchange(ref _decryptionKeys, [], null) ??
                    _decryptionKeys;
            }
        }

        /// <summary>
        /// Gets or set the <see cref="ITokenReplayCache"/> that store tokens that can be checked to help detect token replay.
        /// </summary>
        /// <remarks>If set, then tokens must have an expiration time or the runtime will fault.</remarks>
        public ITokenReplayCache? TokenReplayCache { get; set; }

        /// <summary>
        /// Allows overriding the delegate that will be used to validate the token replay of the token.
        /// </summary>
        /// <remarks>
        /// If no delegate is set, the default implementation will be used.
        /// This means no default token replay validation will occur.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when the value is set as null.</exception>
        /// <returns>The <see cref="TokenReplayValidationDelegate"/> used to validate the token replay of the token.</returns>
        public TokenReplayValidationDelegate TokenReplayValidator
        {
            get { return _tokenReplayValidator; }
            set { _tokenReplayValidator = value ?? throw new ArgumentNullException(nameof(value), "TokenReplayValidator cannot be set as null."); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether all <see cref="DecryptionKeys"/> should be tried during token decryption when a key is not matched to token 'kid' or if token 'kid' is empty.
        /// The default is <c>true</c>.
        /// </summary>
        [DefaultValue(true)]
        public bool TryAllDecryptionKeys { get; set; } = true;

        /// <summary>
        /// If the SignatureKeyResolver is unable to resolve the key when validating the signature of the SecurityToken,
        /// all available keys will be tried.
        /// </summary>
        [DefaultValue(false)]
        public bool TryAllSigningKeys { get; set; }

        /// <summary>
        /// Allows overriding the delegate that will be used to validate the type of the token.
        /// If the token type cannot be validated, a <see cref="ValidationResult{TResult, TError}"/> MUST be returned by the delegate.
        /// Note: the 'type' parameter may be null if it couldn't be extracted from its usual location.
        /// Implementations that need to resolve it from a different location can use the 'token' parameter.
        /// </summary>
        /// <remarks>
        /// If no delegate is set, the default implementation will be used. The default checks the type
        /// against the <see cref="ValidTypes"/> property, if the type is present then, it will succeed.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when the value is set as null.</exception>
        /// <returns>The <see cref="TokenTypeValidationDelegate"/> used to validate the token type of a token</returns>
        public TokenTypeValidationDelegate TokenTypeValidator
        {
            get { return _tokenTypeValidator; }
            set { _tokenTypeValidator = value ?? throw new ArgumentNullException(nameof(value), "TypeValidator cannot be set as null."); }
        }

        /// <summary>
        /// Gets or sets a boolean to control if the LKG configuration will be used for token validation.
        /// </summary>
        /// <remarks>
        /// The default is <c>false</c>.
        /// </remarks>
        [DefaultValue(false)]
        public bool ValidateWithLKG { get; set; }

        /// <summary>
        /// Gets or sets the valid algorithms for cryptographic operations.
        /// </summary>
        /// <remarks>
        /// If set to a non-empty collection, only the algorithms listed will be considered valid.
        /// The default is <c>null</c>.
        /// </remarks>
        public IList<string> ValidAlgorithms
        {
            get
            {
                return _validAlgorithms ??
                    Interlocked.CompareExchange(ref _validAlgorithms, [], null) ??
                    _validAlgorithms;
            }
        }

        /// <summary>
        /// Gets the <see cref="IList{T}"/> that contains valid audiences that will be used to check against the token's audience.
        /// The default is an empty collection.
        /// </summary>
        public IList<string> ValidAudiences
        {
            get
            {
                return _validAudiences ??
                    Interlocked.CompareExchange(ref _validAudiences, [], null) ??
                    _validAudiences;
            }
        }

        /// <summary>
        /// Gets the <see cref="IList{T}"/> that contains valid issuers that will be used to check against the token's issuer.
        /// The default is an empty collection.
        /// </summary>
        /// <returns>The <see cref="IList{T}"/> that contains valid issuers that will be used to check against the token's 'iss' claim.</returns>
        public IList<string> ValidIssuers
        {
            get
            {
                return _validIssuers ??
                    Interlocked.CompareExchange(ref _validIssuers, [], null) ??
                    _validIssuers;
            }
        }

        /// <summary>
        /// Gets the <see cref="IList{T}"/> that contains valid types that will be used to check against the JWT header's 'typ' claim.
        /// If this property is not set, the 'typ' header claim will not be validated and all types will be accepted.
        /// In the case of a JWE, this property will ONLY apply to the inner token header.
        /// The default is an empty collection.
        /// </summary>
        /// <returns>The <see cref="IList{T}"/> that contains valid token types that will be used to check against the token's 'typ' claim.</returns>
        public IList<string> ValidTypes
        {
            get
            {
                return _validTokenTypes ??
                    Interlocked.CompareExchange(ref _validTokenTypes, [], null) ??
                    _validTokenTypes;
            }
        }

        /// <summary>
        /// Gets or sets a boolean that controls if the actor claim should be validated.
        /// </summary>
        /// <remarks>Default value is false.</remarks>
        [DefaultValue(false)]
        public bool ValidateActor { get; set; }
    }
}
#nullable restore
