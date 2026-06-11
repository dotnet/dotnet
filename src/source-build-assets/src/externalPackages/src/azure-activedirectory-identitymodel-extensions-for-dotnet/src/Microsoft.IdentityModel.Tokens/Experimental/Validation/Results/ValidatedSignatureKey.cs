// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

#nullable enable
namespace Microsoft.IdentityModel.Tokens.Experimental
{
    /// <summary>
    /// Represents a validated signature key.
    /// </summary>
    public readonly struct ValidatedSignatureKey : IEquatable<ValidatedSignatureKey>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ValidatedSignatureKey"/>.
        /// </summary>
        /// <param name="validFrom">The date from which the signing key is considered valid.</param>
        /// <param name="validTo">The date until which the signing key is considered valid.</param>
        /// <param name="validationTime">The time the validation occurred.</param>
        internal ValidatedSignatureKey(DateTime? validFrom, DateTime? validTo, DateTime? validationTime)
        {
            ValidFrom = validFrom;
            ValidTo = validTo;
            ValidationTime = validationTime;
        }

        /// <summary>
        /// The date from which the signing key is considered valid.
        /// </summary>
        public DateTime? ValidFrom { get; }

        /// <summary>
        /// The date until which the signing key is considered valid.
        /// </summary>
        public DateTime? ValidTo { get; }

        /// <summary>
        /// The time the validation occurred.
        /// </summary>
        public DateTime? ValidationTime { get; }

        /// <summary>
        /// Determines whether the specified object is equal to the current instance of <see cref="ValidatedSignatureKey"/>.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified object is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is ValidatedSignatureKey other)
            {
                return Equals(other);
            }

            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance of <see cref="ValidatedSignatureKey"/>.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ValidFrom.GetHashCode() ^ ValidTo.GetHashCode() ^ ValidationTime.GetHashCode();
        }

        /// <summary>
        /// Equality comparison operator for <see cref="ValidatedSignatureKey"/>.
        /// </summary>
        /// <param name="left">The left value to compare.</param>
        /// <param name="right">The right value to compare.</param>
        /// <returns>A boolean indicating whether the left value is equal to the right one.</returns>
        public static bool operator ==(ValidatedSignatureKey left, ValidatedSignatureKey right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Inequality comparison operator for <see cref="ValidatedSignatureKey"/>.
        /// </summary>
        /// <param name="left">The left value to compare.</param>
        /// <param name="right">The right value to compare.</param>
        /// <returns>A boolean indicating whether the left value is not equal to the right one.</returns>
        public static bool operator !=(ValidatedSignatureKey left, ValidatedSignatureKey right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Determines whether the specified <see cref="ValidatedSignatureKey"/> is equal to the current instance.
        /// </summary>
        /// <param name="other">The <see cref="ValidatedSignatureKey"/> to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified <see cref="ValidatedSignatureKey"/> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(ValidatedSignatureKey other)
        {
            if (other.ValidFrom != ValidFrom || other.ValidTo != ValidTo || other.ValidationTime != ValidationTime)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// The validated signing key lifetime's string representation.
        /// </summary>
        /// <returns>A string that represents the validated signing key lifetime and the validation time.</returns>
        public override string ToString() => $"{ValidationTime} ∊ [{ValidFrom}, {ValidTo}]";
    }
}
#nullable restore
