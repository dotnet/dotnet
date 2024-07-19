// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// The type of the failure that occurred when validating a <see cref="SecurityToken"/>.
    /// </summary>
    internal abstract class ValidationFailureType
    {
        /// <summary>
        /// Creates an instance of <see cref="TokenValidationResult"/>
        /// </summary>
        protected ValidationFailureType(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets the name of the <see cref="ValidationFailureType"/>.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Defines a type that represents a required parameter was null.
        /// </summary>
        public static readonly ValidationFailureType NullArgument = new NullArgumentFailure("NullArgument");
        private class NullArgumentFailure : ValidationFailureType { internal NullArgumentFailure(string name) : base(name) { } }

        /// <summary>
        /// Defines a type that represents that issuer validation failed.
        /// </summary>
        public static readonly ValidationFailureType IssuerValidationFailed = new IssuerValidationFailure("IssuerValidationFailed");
        private class IssuerValidationFailure : ValidationFailureType { internal IssuerValidationFailure(string name) : base(name) { } }

        /// <summary>
        /// Defines a type that represents that audience validation failed.
        /// </summary>
        public static readonly ValidationFailureType AudienceValidationFailed = new AudienceValidationFailure("AudienceValidationFailed");
        private class AudienceValidationFailure : ValidationFailureType { internal AudienceValidationFailure(string name) : base(name) { } }

        /// <summary>
        /// Defines a type that represents that token type validation failed.
        /// </summary>
        public static readonly ValidationFailureType TokenTypeValidationFailed = new TokenTypeValidationFailure("TokenTypeValidationFailed");
        private class TokenTypeValidationFailure : ValidationFailureType { internal TokenTypeValidationFailure(string name) : base(name) { } }

        /// <summary>
        /// Defines a type that represents that signing key validation failed.
        /// </summary>
        public static readonly ValidationFailureType SigningKeyValidationFailed = new SigningKeyValidationFailure("SigningKeyValidationFailed");
        private class SigningKeyValidationFailure : ValidationFailureType { internal SigningKeyValidationFailure(string name) : base(name) { } }

        /// <summary>
        /// Defines a type that represents that lifetime validation failed.
        /// </summary>
        public static readonly ValidationFailureType LifetimeValidationFailed = new LifetimeValidationFailure("LifetimeValidationFailed");
        private class LifetimeValidationFailure : ValidationFailureType { internal LifetimeValidationFailure(string name) : base(name) { } }

        /// <summary>
        /// Defines a type that represents that token replay validation failed.
        /// </summary>
        public static readonly ValidationFailureType TokenReplayValidationFailed = new TokenReplayValidationFailure("TokenReplayValidationFailed");
        private class TokenReplayValidationFailure : ValidationFailureType { internal TokenReplayValidationFailure(string name) : base(name) { } }

        /// <summary>
        /// Defines a type that represents that no evaluation has taken place.
        /// </summary>
        public static readonly ValidationFailureType ValidationNotEvaluated = new NotEvaluated("NotEvaluated");
        private class NotEvaluated : ValidationFailureType { internal NotEvaluated(string name) : base(name) { } }

        /// <summary>
        /// Defines a type that represents that no evaluation has taken place.
        /// </summary>
        public static readonly ValidationFailureType ValidationSucceeded = new Succeeded("Succeeded");
        private class Succeeded : ValidationFailureType { internal Succeeded(string name) : base(name) { } }

    }
}
