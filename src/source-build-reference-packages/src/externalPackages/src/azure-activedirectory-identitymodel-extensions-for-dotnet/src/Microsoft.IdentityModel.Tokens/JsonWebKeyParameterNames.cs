// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// JsonWebKey parameter names
    /// see: https://datatracker.ietf.org/doc/html/rfc7517
    /// </summary>
    public static class JsonWebKeyParameterNames
    {
#pragma warning disable 1591
        public const string Alg = "alg";
        public const string Crv = "crv";
        public const string D = "d";
        public const string DP = "dp";
        public const string DQ = "dq";
        public const string E = "e";
        public const string K = "k";
        public const string KeyOps = "key_ops";
        public const string Keys = "keys";
        public const string Kid = "kid";
        public const string Kty = "kty";
        public const string N = "n";
        public const string Oth = "oth";
        public const string P = "p";
        public const string Q = "q";
        public const string QI = "qi";
        public const string Use = "use";
        public const string X = "x";
        public const string X5c = "x5c";
        public const string X5t = "x5t";
        public const string X5tS256 = "x5t#S256";
        public const string X5u = "x5u";
        public const string Y = "y";
#pragma warning restore 1591
    }

    /// <summary>
    /// JsonWebKey parameter names as UTF8 bytes
    /// Used by UTF8JsonReader/Writer for performance gains.
    /// </summary>
    internal readonly struct JsonWebKeyParameterUtf8Bytes
    {
        public static ReadOnlySpan<byte> Alg => "alg"u8;
        public static ReadOnlySpan<byte> Crv => "crv"u8;
        public static ReadOnlySpan<byte> D => "d"u8;
        public static ReadOnlySpan<byte> DP => "dp"u8;
        public static ReadOnlySpan<byte> DQ => "dq"u8;
        public static ReadOnlySpan<byte> E => "e"u8;
        public static ReadOnlySpan<byte> K => "k"u8;
        public static ReadOnlySpan<byte> KeyOps => "key_ops"u8;
        public static ReadOnlySpan<byte> Keys => "keys"u8;
        public static ReadOnlySpan<byte> Kid => "kid"u8;
        public static ReadOnlySpan<byte> Kty => "kty"u8;
        public static ReadOnlySpan<byte> N => "n"u8;
        public static ReadOnlySpan<byte> Oth => "oth"u8;
        public static ReadOnlySpan<byte> P => "p"u8;
        public static ReadOnlySpan<byte> Q => "q"u8;
        public static ReadOnlySpan<byte> QI => "qi"u8;
        public static ReadOnlySpan<byte> Use => "use"u8;
        public static ReadOnlySpan<byte> X5c => "x5c"u8;
        public static ReadOnlySpan<byte> X5t => "x5t"u8;
        public static ReadOnlySpan<byte> X5tS256 => "x5t#S256"u8;
        public static ReadOnlySpan<byte> X5u => "x5u"u8;
        public static ReadOnlySpan<byte> X => "x"u8;
        public static ReadOnlySpan<byte> Y => "y"u8;
    }
}
