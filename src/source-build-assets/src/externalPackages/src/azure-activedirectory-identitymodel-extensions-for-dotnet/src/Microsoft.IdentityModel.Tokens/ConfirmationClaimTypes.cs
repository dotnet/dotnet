// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Confirmation Claim ("cnf") related constants
    /// https://datatracker.ietf.org/doc/html/rfc7800
    /// </summary>
    public static class ConfirmationClaimTypes
    {
        /// <summary>
        /// https://datatracker.ietf.org/doc/html/rfc7800#section-6.1.1
        /// </summary>
        public const string Cnf = "cnf";

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/rfc7800#section-6.2.2
        /// </summary>
        public const string Jwk = "jwk";

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/rfc7800#section-6.2.2
        /// </summary>
        public const string Jwe = "jwe";

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/rfc7800#section-6.2.2
        /// </summary>
        public const string Jku = "jku";

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/rfc7800#section-6.2.2
        /// </summary>
        public const string Kid = "kid";

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/rfc9449#name-jwk-thumbprint-confirmation
        /// </summary>
        public const string Jkt = "jkt";
    }

    internal static class ConfirmationClaimTypesUtf8Bytes
    {
        public static ReadOnlySpan<byte> Cnf => "cnf"u8;
        public static ReadOnlySpan<byte> Jwk => "jwk"u8;
        public static ReadOnlySpan<byte> Jwe => "jwe"u8;
        public static ReadOnlySpan<byte> Jku => "jku"u8;
        public static ReadOnlySpan<byte> Kid => "kid"u8;
        public static ReadOnlySpan<byte> Jkt => "jkt"u8;
    }
}
