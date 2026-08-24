// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

#nullable enable
namespace Microsoft.IdentityModel.Tokens.Experimental
{
    /// <summary>
    /// Represents a validated issuer, including the source of the validation.
    /// </summary>
    public readonly struct ValidatedIssuer : IEquatable<ValidatedIssuer>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ValidatedIssuer"/>.
        /// </summary>
        /// <param name="issuer">The issuer that was validated.</param>
        /// <param name="validationSource">The source of the validation, i.e. configuration, validation parameters.</param>
        public ValidatedIssuer(string issuer, IssuerValidationSource validationSource)
        {
            Issuer = issuer;
            ValidationSource = validationSource;
        }

        /// <summary>
        /// The issuer that was validated.
        /// </summary>
        public string Issuer { get; }

        /// <summary>
        /// The source of the validation, i.e. configuration, validation parameters.
        /// </summary>
        public IssuerValidationSource ValidationSource { get; }

        /// <summary>
        /// Determines whether the specified object is equal to the current instance of <see cref="ValidatedIssuer"/>.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified object is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is ValidatedIssuer other)
            {
                return Equals(other);
            }

            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance of <see cref="ValidatedIssuer"/>.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Issuer.GetHashCode() ^ ValidationSource.GetHashCode();
        }

        /// <summary>
        /// Equality comparison operator for <see cref="ValidatedIssuer"/>.
        /// </summary>
        /// <param name="left">The left value to compare.</param>
        /// <param name="right">The right value to compare.</param>
        /// <returns>A boolean indicating whether the left value is equal to the right one.</returns>
        public static bool operator ==(ValidatedIssuer left, ValidatedIssuer right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Inequality comparison operator for <see cref="ValidatedIssuer"/>.
        /// </summary>
        /// <param name="left">The left value to compare.</param>
        /// <param name="right">The right value to compare.</param>
        /// <returns>A boolean indicating whether the left value is not equal to the right one.</returns>
        public static bool operator !=(ValidatedIssuer left, ValidatedIssuer right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Determines whether the specified <see cref="ValidatedIssuer"/> is equal to the current instance.
        /// </summary>
        /// <param name="other">The <see cref="ValidatedIssuer"/> to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified <see cref="ValidatedIssuer"/> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(ValidatedIssuer other)
        {
            if (other.Issuer != Issuer || other.ValidationSource != ValidationSource)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// The validated issuer's string representation.
        /// </summary>
        /// <returns>A string representing the issuer and where it was validated from.</returns>
        public override string ToString() => $"{Issuer} (from {ValidationSource})";
    }
}
#nullable restore
