// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// The reason for being unable to validate
    /// </summary>
    public enum ValidationFailure
    {
        /// <summary>
        /// Indicates no validation failures
        /// </summary>
        None = 0b_0000_0000_0000_0000, // 0
        /// <summary>
        /// Indicates that the lifetime was invalid
        /// </summary>
        InvalidLifetime = 0b_0000_0000_0000_0001, // 1
        /// <summary>
        /// Indicates that the issuer was invalid
        /// </summary>
        InvalidIssuer = 0b_0000_0000_0000_0010, // 2
    }
}
