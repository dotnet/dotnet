// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

#nullable enable
namespace Microsoft.IdentityModel.Tokens.Experimental
{
    /// <summary>
    /// Represents a validated lifetime, including the NotBefore and Expires values.
    /// </summary>
    public readonly struct ValidatedLifetime : IEquatable<ValidatedLifetime>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ValidatedLifetime"/>.
        /// </summary>
        /// <param name="notBefore">The <see cref="DateTime"/> representing the time from which the token is considered valid.</param>
        /// <param name="expires">The <see cref="DateTime"/> representing the token's expiration time.</param>
        public ValidatedLifetime(DateTime? notBefore, DateTime? expires)
        {
            NotBefore = notBefore;
            Expires = expires;
        }

        /// <summary>
        /// The <see cref="DateTime"/> representing the time from which the token is considered valid.
        /// </summary>
        public DateTime? NotBefore { get; }

        /// <summary>
        /// The <see cref="DateTime"/> representing the token's expiration time.
        /// </summary>
        public DateTime? Expires { get; }

        /// <summary>
        /// Determines whether the specified object is equal to the current instance of <see cref="ValidatedLifetime"/>.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified object is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is ValidatedLifetime other)
            {
                return Equals(other);
            }

            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance of <see cref="ValidatedLifetime"/>.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return NotBefore.GetHashCode() ^ Expires.GetHashCode();
        }

        /// <summary>
        /// Equality comparison operator for <see cref="ValidatedLifetime"/>.
        /// </summary>
        /// <param name="left">The left value to compare.</param>
        /// <param name="right">The right value to compare.</param>
        /// <returns>A boolean indicating whether the left value is equal to the right one.</returns>
        public static bool operator ==(ValidatedLifetime left, ValidatedLifetime right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Inequality comparison operator for <see cref="ValidatedLifetime"/>.
        /// </summary>
        /// <param name="left">The left value to compare.</param>
        /// <param name="right">The right value to compare.</param>
        /// <returns>A boolean indicating whether the left value is not equal to the right one.</returns>
        public static bool operator !=(ValidatedLifetime left, ValidatedLifetime right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Determines whether the specified <see cref="ValidatedLifetime"/> is equal to the current instance.
        /// </summary>
        /// <param name="other">The <see cref="ValidatedLifetime"/> to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified <see cref="ValidatedLifetime"/> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(ValidatedLifetime other)
        {
            if (other.NotBefore != NotBefore || other.Expires != Expires)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// The validated lifetime's string representation.
        /// </summary>
        /// <returns>A string representing the validated lifetime.</returns>
        public override string ToString() => $"[{NotBefore}, {Expires}]";
    }
}
#nullable restore
