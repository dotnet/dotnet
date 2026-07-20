// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

#nullable enable
namespace Microsoft.IdentityModel.Tokens.Experimental
{
    /// <summary>
    /// Failures that occur during validation of a <see cref="SecurityToken"/>.
    /// </summary>
    public abstract class ValidationFailureType
    {
        /// <summary>
        /// Creates an instance of <see cref="ValidationFailureType"/>
        /// </summary>
        protected ValidationFailureType(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets the name of the <see cref="ValidationFailureType"/>.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// A required parameter was null.
        /// </summary>
        public static readonly ValidationFailureType NullArgument = new NullArgumentFailure("NullArgument");
        private class NullArgumentFailure : ValidationFailureType { internal NullArgumentFailure(string name) : base(name) { } }

        /// <summary>
        /// CryptoProvider CreateForVerifying returned null.
        /// </summary>
        public static readonly ValidationFailureType CryptoProviderReturnedNull = new CryptoProviderReturnedNullFailure("CryptoProviderReturnedNullFailure");
        private class CryptoProviderReturnedNullFailure : ValidationFailureType { internal CryptoProviderReturnedNullFailure(string name) : base(name) { } }

        /// <summary>
        /// A token could not be read.
        /// </summary>
        public static readonly ValidationFailureType TokenReadingFailed = new TokenReadingFailure("TokenReadingFailed");
        private class TokenReadingFailure : ValidationFailureType { internal TokenReadingFailure(string name) : base(name) { } }

        /// <summary>
        /// A token exceeds the maximum size.
        /// </summary>
        public static readonly ValidationFailureType TokenExceedsMaximumSize = new TokenExceedsMaximumSizeFailure("TokenExceedsMaximumSize");
        private class TokenExceedsMaximumSizeFailure : ValidationFailureType { internal TokenExceedsMaximumSizeFailure(string name) : base(name) { } }

        /// <summary>
        /// A token could not be decrypted.
        /// </summary>
        public static readonly ValidationFailureType TokenDecryptionFailed = new TokenDecryptionFailure("TokenDecryptionFailed");
        private class TokenDecryptionFailure : ValidationFailureType { internal TokenDecryptionFailure(string name) : base(name) { } }

        /// <summary>
        /// KeyWrap failed.
        /// </summary>
        public static readonly ValidationFailureType KeyWrapFailed = new KeyWrapFailure("KeyWrapFailed");
        private class KeyWrapFailure : ValidationFailureType { internal KeyWrapFailure(string name) : base(name) { } }

        /// <summary>
        /// A token could not be decompressed.
        /// </summary>
        public static readonly ValidationFailureType TokenDecompressionFailed = new TokenDecompressionFailure("TokenDecompressionFailed");
        private class TokenDecompressionFailure : ValidationFailureType { internal TokenDecompressionFailure(string name) : base(name) { } }

        /// <summary>
        /// A token is not the correct type.
        /// </summary>
        public static readonly ValidationFailureType SecurityTokenNotExpectedType = new SecurityTokenNotExpectedTypeFailure("SecurityTokenNotExpectedType");
        private class SecurityTokenNotExpectedTypeFailure : ValidationFailureType { internal SecurityTokenNotExpectedTypeFailure(string name) : base(name) { } }

        /// <summary>
        /// A token is too large.
        /// </summary>
        public static readonly ValidationFailureType SecurityTokenTooLarge = new SecurityTokenTooLargeFailure("SecurityTokenTooLarge");
        private class SecurityTokenTooLargeFailure : ValidationFailureType { internal SecurityTokenTooLargeFailure(string name) : base(name) { } }

        /// <summary>
        /// Signed info is null.
        /// </summary>
        public static readonly ValidationFailureType SignedInfoNull = new SignedInfoNullFailure("SignedInfoNull");
        private class SignedInfoNullFailure : ValidationFailureType { internal SignedInfoNullFailure(string name) : base(name) { } }
    }

    /// <summary>
    /// Failures that occur during validation of a <see cref="SecurityToken"/> audience.
    /// </summary>
    public abstract class AudienceValidationFailure : ValidationFailureType
    {
        /// <summary>
        /// Creates an instance of <see cref="AudienceValidationFailure"/>
        /// </summary>
        protected AudienceValidationFailure(string name) : base(name)
        {
        }

        /// <summary>
        /// Audience validation failed.
        /// </summary>
        public static readonly Experimental.AudienceValidationFailure AudienceDidNotMatch = new AudienceValidationFailed("AudienceDidNotMatch");
        private class AudienceValidationFailed : Experimental.AudienceValidationFailure { internal AudienceValidationFailed(string name) : base(name) { } }

        /// <summary>
        /// Audience validation delegate threw an exception.
        /// </summary>
        public static readonly Experimental.AudienceValidationFailure ValidatorThrew = new AudienceValidatorThrewFailure("AudienceValidatorThrew");
        private class AudienceValidatorThrewFailure : Experimental.AudienceValidationFailure { internal AudienceValidatorThrewFailure(string name) : base(name) { } }

        /// <summary>
        /// No audience found in the <see cref="SecurityToken"/>.
        /// </summary>
        public static readonly Experimental.AudienceValidationFailure NoAudienceInToken = new NoAudienceInTokenFailure("NoAudienceInToken");
        private class NoAudienceInTokenFailure : Experimental.AudienceValidationFailure { internal NoAudienceInTokenFailure(string name) : base(name) { } }

        /// <summary>
        /// No audiences were found in <see cref="ValidationParameters.ValidAudiences"/>.
        /// </summary>
        public static readonly Experimental.AudienceValidationFailure NoValidationParameterAudiencesProvided = new NoValidationParameterAudiencesProvidedFailure("NoValidationParameterAudiencesProvided");
        private class NoValidationParameterAudiencesProvidedFailure : Experimental.AudienceValidationFailure { internal NoValidationParameterAudiencesProvidedFailure(string name) : base(name) { } }
    }

    /// <summary>
    /// Failures that occur during validation of a <see cref="SecurityToken"/> algoritm.
    /// </summary>
    public abstract class AlgorithmValidationFailure : ValidationFailureType
    {
        /// <summary>
        /// Creates an instance of <see cref="AlgorithmValidationFailure"/>
        /// </summary>
        protected AlgorithmValidationFailure(string name) : base(name) { }

        /// <summary>
        /// General algorithm validation failed.
        /// </summary>
        public static readonly AlgorithmValidationFailure ValidationFailed = new AlgorithmValidationFailedType("AlgorithmValidationFailed");
        private class AlgorithmValidationFailedType : AlgorithmValidationFailure { internal AlgorithmValidationFailedType(string name) : base(name) { } }

        /// <summary>
        /// Algorithm validation delegate threw an exception.
        /// </summary>
        public static readonly AlgorithmValidationFailure ValidatorThrew = new AlgorithmValidatorThrewType("AlgorithmValidatorThrew");
        private class AlgorithmValidatorThrewType : AlgorithmValidationFailure { internal AlgorithmValidatorThrewType(string name) : base(name) { } }

        /// <summary>
        /// Unsupported algorithm.
        /// </summary>
        public static readonly AlgorithmValidationFailure AlgorithmIsNotSupported = new AlgorithmIsNotSupportedType("AlgorithmIsNotSupported");
        private class AlgorithmIsNotSupportedType : AlgorithmValidationFailure { internal AlgorithmIsNotSupportedType(string name) : base(name) { } }
    }

    /// <summary>
    /// Failures that occur during validation of a <see cref="SecurityToken"/> the signature key.
    /// </summary>
    public abstract class SignatureKeyValidationFailure : ValidationFailureType
    {
        /// <summary>
        /// Creates an instance of <see cref="SignatureKeyValidationFailure"/>
        /// </summary>
        protected SignatureKeyValidationFailure(string name) : base(name) { }

        /// <summary>
        /// Signature key validation failed.
        /// </summary>
        public static readonly SignatureKeyValidationFailure ValidationFailed = new SignatureKeyValidationFailedType("SignatureKeyValidationFailed");
        private class SignatureKeyValidationFailedType : SignatureKeyValidationFailure { internal SignatureKeyValidationFailedType(string name) : base(name) { } }

        /// <summary>
        /// The signature key validator delegate threw an exception.
        /// </summary>
        public static readonly SignatureKeyValidationFailure ValidatorThrew = new SignatureKeyValidatorThrewType("SignatureKeyValidatorThrew");
        private class SignatureKeyValidatorThrewType : SignatureKeyValidationFailure { internal SignatureKeyValidatorThrewType(string name) : base(name) { } }

        /// <summary>
        /// Signature key was not yet valid.
        /// </summary>
        public static readonly SignatureKeyValidationFailure NotYetValid = new SigningKeyNotYetValidType("NotYetValid");
        private class SigningKeyNotYetValidType : SignatureKeyValidationFailure { internal SigningKeyNotYetValidType(string name) : base(name) { } }

        /// <summary>
        /// Signature key was expired.
        /// </summary>
        public static readonly SignatureKeyValidationFailure KeyExpired = new SigningKeyExpiredType("KeyExpired");
        private class SigningKeyExpiredType : SignatureKeyValidationFailure { internal SigningKeyExpiredType(string name) : base(name) { } }

        /// <summary>
        /// Signature key is null.
        /// </summary>
        public static readonly SignatureKeyValidationFailure KeyIsNull = new SigningKeyIsNullType("KeyIsNull");
        private class SigningKeyIsNullType : SignatureKeyValidationFailure { internal SigningKeyIsNullType(string name) : base(name) { } }
    }

    /// <summary>
    /// Failures that occur during validation of a <see cref="SecurityToken"/> issuer.
    /// </summary>
    public abstract class IssuerValidationFailure : ValidationFailureType
    {
        /// <summary>
        /// Creates an instance of <see cref="IssuerValidationFailure"/>
        /// </summary>
        protected IssuerValidationFailure(string name) : base(name) { }

        /// <summary>
        /// General issuer validation failure.
        /// </summary>
        public static readonly IssuerValidationFailure ValidationFailed = new IssuerValidationFailedType("IssuerValidationFailed");
        private class IssuerValidationFailedType : IssuerValidationFailure { internal IssuerValidationFailedType(string name) : base(name) { } }

        /// <summary>
        /// Issuer validation delegate threw an exception.
        /// </summary>
        public static readonly IssuerValidationFailure ValidatorThrew = new IssuerValidatorThrewType("IssuerValidatorThrew");
        private class IssuerValidatorThrewType : IssuerValidationFailure { internal IssuerValidatorThrewType(string name) : base(name) { } }

        /// <summary>
        /// No issuer was found in the security token.
        /// </summary>
        public static readonly IssuerValidationFailure NoIssuerInToken = new NoIssuerInTokenType("NoIssuerInToken");
        private class NoIssuerInTokenType : IssuerValidationFailure { internal NoIssuerInTokenType(string name) : base(name) { } }

        /// <summary>
        /// No issuers were found in <see cref="ValidationParameters.ValidIssuers"/>.
        /// </summary>
        public static readonly IssuerValidationFailure NoValidationParameterIssuersProvided = new NoValidationParameterIssuersProvidedType("NoValidationParameterIssuersProvided");
        private class NoValidationParameterIssuersProvidedType : IssuerValidationFailure { internal NoValidationParameterIssuersProvidedType(string name) : base(name) { } }
    }

    /// <summary>
    /// Failures that occur during validation of a <see cref="SecurityToken"/> lifetime.
    /// </summary>
    public abstract class LifetimeValidationFailure : ValidationFailureType
    {
        /// <summary>
        /// Creates an instance of <see cref="LifetimeValidationFailure"/>
        /// </summary>
        protected LifetimeValidationFailure(string name) : base(name) { }

        /// <summary>
        /// General lifetime validation failure.
        /// </summary>
        public static readonly LifetimeValidationFailure ValidationFailed = new ValidationFailedType("LifetimeValidationFailed");
        private class ValidationFailedType : LifetimeValidationFailure { internal ValidationFailedType(string name) : base(name) { } }

        /// <summary>
        /// Lifetime validator delegate threw an exception.
        /// </summary>
        public static readonly LifetimeValidationFailure ValidatorThrew = new ValidatorThrewType("LifetimeValidatorThrew");
        private class ValidatorThrewType : LifetimeValidationFailure { internal ValidatorThrewType(string name) : base(name) { } }

        /// <summary>
        /// No expiration found in the security token.
        /// </summary>
        public static readonly LifetimeValidationFailure NoExpirationTime = new NoExpirationTimeType("NoExpirationTime");
        private class NoExpirationTimeType : LifetimeValidationFailure { internal NoExpirationTimeType(string name) : base(name) { } }

        /// <summary>
        /// NotBefore time is after the expires time.
        /// </summary>
        public static readonly LifetimeValidationFailure NotbeforeGreaterThanExpirationTime = new NotbeforeGreaterThanExpirationTimeType("NotbeforeGreaterThanExpirationTime");
        private class NotbeforeGreaterThanExpirationTimeType : LifetimeValidationFailure { internal NotbeforeGreaterThanExpirationTimeType(string name) : base(name) { } }

        /// <summary>
        /// Token was not yet valid.
        /// </summary>
        public static readonly LifetimeValidationFailure NotYetValid = new NotYetValidType("NotYetValid");
        private class NotYetValidType : LifetimeValidationFailure { internal NotYetValidType(string name) : base(name) { } }

        /// <summary>
        /// Token was expired.
        /// </summary>
        public static readonly LifetimeValidationFailure Expired = new LifetimeExpiredType("Expired");
        private class LifetimeExpiredType : LifetimeValidationFailure { internal LifetimeExpiredType(string name) : base(name) { } }

        /// <summary>
        /// Token was issued in the future.
        /// </summary>
        public static readonly LifetimeValidationFailure IssuedInFuture = new IssuedInFutureType("IssuedInFuture");
        private class IssuedInFutureType : LifetimeValidationFailure { internal IssuedInFutureType(string name) : base(name) { } }
    }

    /// <summary>
    /// Failures that occur during validation of a <see cref="SecurityToken"/> signature.
    /// </summary>
    public abstract class SignatureValidationFailure : ValidationFailureType
    {
        /// <summary>
        /// Creates an instance of <see cref="SignatureValidationFailure"/>
        /// </summary>
        protected SignatureValidationFailure(string name) : base(name) { }

        /// <summary>
        /// The token's signature validation failed.
        /// </summary>
        public static readonly SignatureValidationFailure ValidationFailed = new SignatureValidationFailedType("SignatureValidationFailed");
        private class SignatureValidationFailedType : SignatureValidationFailure { internal SignatureValidationFailedType(string name) : base(name) { } }

        /// <summary>
        /// The signature validation delegate threw an exception.
        /// </summary>
        public static readonly SignatureValidationFailure ValidatorThrew = new SignatureValidatorThrewType("SignatureValidatorThrew");
        private class SignatureValidatorThrewType : SignatureValidationFailure { internal SignatureValidatorThrewType(string name) : base(name) { } }

        /// <summary>
        /// The token is not signed.
        /// </summary>
        public static readonly SignatureValidationFailure TokenIsNotSigned = new TokenIsNotSignedType("TokenIsNotSigned");
        private class TokenIsNotSignedType : SignatureValidationFailure { internal TokenIsNotSignedType(string name) : base(name) { } }

        /// <summary>
        /// The token's signature algorithm validation failed.
        /// </summary>
        public static readonly SignatureValidationFailure AlgorithmValidationFailed = new AlgorithmValidationFailedType("AlgorithmValidationFailed");
        private class AlgorithmValidationFailedType : SignatureValidationFailure { internal AlgorithmValidationFailedType(string name) : base(name) { } }

        /// <summary>
        /// Signing key not found.
        /// </summary>
        public static readonly SignatureValidationFailure SigningKeyNotFound = new SignatureKeyNotFoundType("SigningKeyNotFound");
        private class SignatureKeyNotFoundType : SignatureValidationFailure { internal SignatureKeyNotFoundType(string name) : base(name) { } }

        /// <summary>
        /// Defines a type that represents that an XML validation failed.
        /// </summary>
        public static readonly SignatureValidationFailure ReferenceDigestValidationFailed = new ReferenceDigestValidationFailedType("ReferenceDigestValidationFailed");
        private class ReferenceDigestValidationFailedType : SignatureValidationFailure { internal ReferenceDigestValidationFailedType(string name) : base(name) { } }
    }

    /// <summary>
    /// Failures that occur during validation of a <see cref="SecurityToken"/> token replay.
    /// </summary>
    public abstract class TokenReplayValidationFailure : ValidationFailureType
    {
        /// <summary>
        /// Creates an instance of <see cref="TokenReplayValidationFailure"/>
        /// </summary>
        protected TokenReplayValidationFailure(string name) : base(name) { }

        /// <summary>
        /// Token replay validation failed.
        /// </summary>
        public static readonly TokenReplayValidationFailure ValidationFailed = new TokenReplayValidationFailedType("TokenReplayValidationFailed");
        private class TokenReplayValidationFailedType : TokenReplayValidationFailure { internal TokenReplayValidationFailedType(string name) : base(name) { } }

        /// <summary>
        /// Token replay validation delegate threw an exception.
        /// </summary>
        public static readonly TokenReplayValidationFailure ValidatorThrew = new TokenReplayValidatorThrewType("TokenReplayValidatorThrew");
        private class TokenReplayValidatorThrewType : TokenReplayValidationFailure { internal TokenReplayValidatorThrewType(string name) : base(name) { } }

        /// <summary>
        /// No expiration time found in the security token.
        /// </summary>
        public static readonly TokenReplayValidationFailure NoExpiration = new TokenReplayNoExpirationType("NoExpiration");
        private class TokenReplayNoExpirationType : TokenReplayValidationFailure { internal TokenReplayNoExpirationType(string name) : base(name) { } }

        /// <summary>
        /// Token was found in the cache.
        /// </summary>
        public static readonly TokenReplayValidationFailure TokenFoundInCache = new TokenReplayTokenFoundType("TokenFoundInCache");
        private class TokenReplayTokenFoundType : TokenReplayValidationFailure { internal TokenReplayTokenFoundType(string name) : base(name) { } }

        /// <summary>
        /// Token could not be added to the cache.
        /// </summary>
        public static readonly TokenReplayValidationFailure AddToCacheFailed = new TokenReplayAddToCacheFailedType("AddToCacheFailed");
        private class TokenReplayAddToCacheFailedType : TokenReplayValidationFailure { internal TokenReplayAddToCacheFailedType(string name) : base(name) { } }
    }

    /// <summary>
    /// Failures that occur during validation of a <see cref="SecurityToken"/> token type.
    /// </summary>
    public abstract class TokenTypeValidationFailure : ValidationFailureType
    {
        /// <summary>
        /// Creates an instance of <see cref="TokenTypeValidationFailure"/>
        /// </summary>
        protected TokenTypeValidationFailure(string name) : base(name) { }

        /// <summary>
        /// Token type validation failed.
        /// </summary>
        public static readonly TokenTypeValidationFailure ValidationFailed = new TokenTypeValidationFailedType("TokenTypeValidationFailed");
        private class TokenTypeValidationFailedType : TokenTypeValidationFailure { internal TokenTypeValidationFailedType(string name) : base(name) { } }

        /// <summary>
        /// Token type validation delegate threw an exception.
        /// </summary>
        public static readonly TokenTypeValidationFailure ValidatorThrew = new TokenTypeValidatorThrewType("TokenTypeValidatorThrew");
        private class TokenTypeValidatorThrewType : TokenTypeValidationFailure { internal TokenTypeValidatorThrewType(string name) : base(name) { } }
    }
}
#nullable restore
