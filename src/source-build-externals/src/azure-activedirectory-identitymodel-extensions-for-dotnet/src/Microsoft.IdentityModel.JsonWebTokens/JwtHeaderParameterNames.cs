// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.JsonWebTokens
{
    /// <summary>
    /// List of header parameter names see: https://datatracker.ietf.org/doc/html/rfc7519#section-5.
    /// </summary>
    public struct JwtHeaderParameterNames
    {
        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7515#section-4.1.1
        /// </summary>
        public const string Alg = "alg";

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7515#section-4.1.10
        /// Also: https://datatracker.ietf.org/doc/html/rfc7519#section-5.2
        /// </summary>
        public const string Cty = "cty";

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7516#section-4.1.2
        /// </summary>
        public const string Enc = "enc";

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7518#section-4.7.1.1
        /// </summary>
        public const string IV = "iv";

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7515#section-4.1.2
        /// </summary>
        public const string Jku = "jku";

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7515#section-4.1.3
        /// </summary>
        public const string Jwk = "jwk";

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7515#section-4.1.4
        /// </summary>
        public const string Kid = "kid";

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7515#section-4.1.9
        /// Also: https://datatracker.ietf.org/doc/html/rfc7519#section-5.1
        /// </summary>
        public const string Typ = "typ";

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7515#section-4.1.6
        /// </summary>
        public const string X5c = "x5c";

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7515#page-12
        /// </summary>
        public const string X5t = "x5t";

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7515#section-4.1.5
        /// </summary>
        public const string X5u = "x5u";

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7516#section-4.1.3
        /// </summary>
        public const string Zip = "zip";

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7518#section-4.6.1.1
        /// </summary>
        public const string Epk = "epk";

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7518#section-4.6.1.2
        /// </summary>
        public const string Apu = "apu";

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7518#section-4.6.1.3
        /// </summary>
        public const string Apv = "apv";
    }
}
