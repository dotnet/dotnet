// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Specifies the segment of a JWT.
    /// </summary>
    public enum JwtSegmentType
    {
        /// <summary>
        /// The header segment of the JWT.
        /// </summary>
        Header,

        /// <summary>
        /// The payload segment of the JWT.
        /// </summary>
        Payload
    }
}
