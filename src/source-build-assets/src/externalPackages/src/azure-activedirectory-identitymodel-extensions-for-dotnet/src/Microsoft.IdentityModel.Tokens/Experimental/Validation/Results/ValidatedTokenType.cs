// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

#nullable enable
namespace Microsoft.IdentityModel.Tokens.Experimental
{
    /// <summary>
    /// Represents a validated token type, including the number of valid types present in the validation parameters.
    /// </summary>
    public readonly struct ValidatedTokenType : IEquatable<ValidatedTokenType>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ValidatedTokenType"/>.
        /// </summary>
        /// <param name="type">The token type that was validated.</param>
        /// <param name="validTypeCount">The number of valid types present in the validation parameters.</param>
        public ValidatedTokenType(string type, int validTypeCount)
        {
            Type = type;
            ValidTypeCount = validTypeCount;
        }

        /// <summary>
        /// The token type that was validated.
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// The number of valid types present in the validation parameters.
        /// </summary>
        public int ValidTypeCount { get; }

        /// <summary>
        /// Determines whether the specified object is equal to the current instance of <see cref="ValidatedTokenType"/>.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified object is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is ValidatedTokenType other)
            {
                return Equals(other);
            }

            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance of <see cref="ValidatedTokenType"/>.
        /// </summary>
        /// <returns>The hash code for the current instance.</returns>
        public override int GetHashCode()
        {
            return Type.GetHashCode() ^ ValidTypeCount.GetHashCode();
        }

        /// <summary>
        /// Equality comparison operator for <see cref="ValidatedTokenType"/>.
        /// </summary>
        /// <param name="left">The left value to compare.</param>
        /// <param name="right">The right value to compare.</param>
        /// <returns>A boolean indicating whether the left value is equal to the right one.</returns>
        public static bool operator ==(ValidatedTokenType left, ValidatedTokenType right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Inequality comparison operator for <see cref="ValidatedTokenType"/>.
        /// </summary>
        /// <param name="left">The left value to compare.</param>
        /// <param name="right">The right value to compare.</param>
        /// <returns>A boolean indicating whether the left value is not equal to the right one.</returns>
        public static bool operator !=(ValidatedTokenType left, ValidatedTokenType right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Determines whether the specified <see cref="ValidatedTokenType"/> is equal to the current instance.
        /// </summary>
        /// <param name="other">The <see cref="ValidatedTokenType"/> to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified <see cref="ValidatedTokenType"/> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(ValidatedTokenType other)
        {
            if (other.Type != Type || other.ValidTypeCount != ValidTypeCount)
                return false;

            return true;
        }

        /// <summary>
        /// The validated token type's string representation.
        /// </summary>
        /// <returns>A string representing the validated token type and the amount of valid types.</returns>
        public override string ToString() => $"{Type} ({ValidTypeCount} valid types)";
    }
}
#nullable restore
