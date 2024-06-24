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
