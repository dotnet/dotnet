// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace System.IdentityModel.Tokens.Jwt
{
    /// <summary>
    /// List of header parameter names see: https://datatracker.ietf.org/doc/html/rfc7519#section-5.
    /// </summary>
    public struct JwtHeaderParameterNames
    {
        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7515#section-4.1.1
        /// </summary>
        public const string Alg = Microsoft.IdentityModel.JsonWebTokens.JwtHeaderParameterNames.Alg;

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7515#section-4.1.10
        /// Also: https://datatracker.ietf.org/doc/html/rfc7519#section-5.2
        /// </summary>
        public const string Cty = Microsoft.IdentityModel.JsonWebTokens.JwtHeaderParameterNames.Cty;

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7516#section-4.1.2
        /// </summary>
        public const string Enc = Microsoft.IdentityModel.JsonWebTokens.JwtHeaderParameterNames.Enc;

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7518#section-4.7.1.1
        /// </summary>
        public const string IV = Microsoft.IdentityModel.JsonWebTokens.JwtHeaderParameterNames.IV;

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7515#section-4.1.2
        /// </summary>
        public const string Jku = Microsoft.IdentityModel.JsonWebTokens.JwtHeaderParameterNames.Jku;

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7515#section-4.1.3
        /// </summary>
        public const string Jwk = Microsoft.IdentityModel.JsonWebTokens.JwtHeaderParameterNames.Jwk;

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7515#section-4.1.4
        /// </summary>
        public const string Kid = Microsoft.IdentityModel.JsonWebTokens.JwtHeaderParameterNames.Kid;

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7515#section-4.1.9
        /// Also: https://datatracker.ietf.org/doc/html/rfc7519#section-5.1
        /// </summary>
        public const string Typ = Microsoft.IdentityModel.JsonWebTokens.JwtHeaderParameterNames.Typ;

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7515#section-4.1.6
        /// </summary>
        public const string X5c = Microsoft.IdentityModel.JsonWebTokens.JwtHeaderParameterNames.X5c;

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7515#page-12
        /// </summary>
        public const string X5t = Microsoft.IdentityModel.JsonWebTokens.JwtHeaderParameterNames.X5t;

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7515#section-4.1.5
        /// </summary>
        public const string X5u = Microsoft.IdentityModel.JsonWebTokens.JwtHeaderParameterNames.X5u;

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7516#section-4.1.3
        /// </summary>
        public const string Zip = Microsoft.IdentityModel.JsonWebTokens.JwtHeaderParameterNames.Zip;

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7518#section-4.6.1.1
        /// </summary>
        public const string Epk = Microsoft.IdentityModel.JsonWebTokens.JwtHeaderParameterNames.Epk;

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7518#section-4.6.1.2
        /// </summary>
        public const string Apu = Microsoft.IdentityModel.JsonWebTokens.JwtHeaderParameterNames.Apu;

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7518#section-4.6.1.3
        /// </summary>
        public const string Apv = Microsoft.IdentityModel.JsonWebTokens.JwtHeaderParameterNames.Apv;
    }
}
