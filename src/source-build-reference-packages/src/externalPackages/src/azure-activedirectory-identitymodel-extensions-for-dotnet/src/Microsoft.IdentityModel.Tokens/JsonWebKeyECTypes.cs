// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Constants for JsonWebKey Elliptical Curve Types
    /// https://datatracker.ietf.org/doc/html/rfc7518#section-6.2.1.1
    /// </summary>
    public static class JsonWebKeyECTypes
    {
#pragma warning disable 1591
        public const string P256 = "P-256";
        public const string P384 = "P-384";
        public const string P512 = "P-512";
        public const string P521 = "P-521"; // treat 512 as 521. 512 doesn't exist, but we released with "512" instead of "521", so don't break now.
#pragma warning restore 1591
    }
}
