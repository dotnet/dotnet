// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.IdentityModel.JsonWebTokens
{
    /// <summary>
    /// Defines the standard claim names as per the JWT (JSON Web Token) specification. 
    /// See: <see href="https://datatracker.ietf.org/doc/html/rfc7519#section-4"/>.
    /// See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#IDToken"/>.
    /// See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims"/>.
    /// </summary>
    public struct JwtRegisteredClaimNames
    {
        // Please keep in alphabetical order

        /// <summary>
        /// </summary>
        public const string Actort = "actort";

        /// <summary>
        /// See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#IDToken"/>.
        /// </summary>
        public const string Acr = "acr";

        /// <summary>
        /// See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims"/>.
        /// </summary>
        public const string Address = "address";

        /// <summary>
        /// See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#IDToken"/>.
        /// </summary>
        public const string Amr = "amr";

        /// <summary>
        /// See: <see href="https://datatracker.ietf.org/doc/html/rfc7519#section-4"/>.
        /// </summary>
        public const string Aud = "aud";

        /// <summary>
        /// See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#IDToken"/>.
        /// </summary>
        public const string AuthTime = "auth_time";

        /// <summary>
        /// See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#IDToken"/>.
        /// </summary>
        public const string Azp = "azp";

        /// <summary>
        /// See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims"/>.
        /// </summary>
        public const string Birthdate = "birthdate";

        /// <summary>
        /// See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#HybridIDToken"/>.
        /// </summary>
        public const string CHash = "c_hash";

        /// <summary>
        /// See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#CodeIDToken"/>.
        /// </summary>
        public const string AtHash = "at_hash";

        /// <summary>
        /// See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims"/>.
        /// </summary>
        public const string Email = "email";

        /// <summary>
        /// See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims"/>.
        /// </summary>
        public const string EmailVerified = "email_verified";

        /// <summary>
        /// See: <see href="https://datatracker.ietf.org/doc/html/rfc7519#section-4"/>.
        /// </summary>
        public const string Exp = "exp";

        /// <summary>
        /// See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims"/>.
        /// </summary>
        public const string Gender = "gender";

        /// <summary>
        /// See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims"/>.
        /// </summary>
        public const string FamilyName = "family_name";

        /// <summary>
        /// See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims"/>.
        /// </summary>
        public const string GivenName = "given_name";

        /// <summary>
        /// See: <see href="https://datatracker.ietf.org/doc/html/rfc7519#section-4"/>.
        /// </summary>
        public const string Iat = "iat";

        /// <summary>
        /// See: <see href="https://datatracker.ietf.org/doc/html/rfc7519#section-4"/>.
        /// </summary>
        public const string Iss = "iss";

        /// <summary>
        /// See: <see href="https://datatracker.ietf.org/doc/html/rfc7519#section-4"/>.
        /// </summary>
        public const string Jti = "jti";

        /// <summary>
        /// See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims"/>.
        /// </summary>
        public const string Locale = "locale";

        /// <summary>
        /// See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims"/>.
        /// </summary>
        public const string MiddleName = "middle_name";

        /// <summary>
        /// See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims"/>.
        /// </summary>
        public const string Name = "name";

        /// <summary>
        /// </summary>
        public const string NameId = "nameid";

        /// <summary>
        /// See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims"/>.
        /// </summary>
        public const string Nickname = "nickname";

        /// <summary>
        /// See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#AuthRequest"/>.
        /// </summary>
        public const string Nonce = "nonce";

        /// <summary>
        /// See: <see href="https://datatracker.ietf.org/doc/html/rfc7519#section-4"/>.
        /// </summary>
        public const string Nbf = "nbf";

        /// <summary>
        /// See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims"/>.
        /// </summary>
        public const string PhoneNumber = "phone_number";

        /// <summary>
        /// See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims"/>.
        /// </summary>
        public const string PhoneNumberVerified = "phone_number_verified";

        /// <summary>
        /// See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims"/>.
        /// </summary>
        public const string Picture = "picture";

        /// <summary>
        /// </summary>
        public const string Prn = "prn";

        /// <summary>
        /// See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims"/>.
        /// </summary>
        public const string PreferredUsername = "preferred_username";

        /// <summary>
        /// See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims"/>.
        /// </summary>
        public const string Profile = "profile";

        /// <summary>
        /// See: <see href="https://openid.net/specs/openid-connect-frontchannel-1_0.html#OPLogout"/>.
        /// </summary>
        public const string Sid = "sid";

        /// <summary>
        /// See: <see href="https://datatracker.ietf.org/doc/html/rfc7519#section-4"/>.
        /// </summary>
        public const string Sub = "sub";

        /// <summary>
        /// See: <see href="https://datatracker.ietf.org/doc/html/rfc7519#section-5"/>.
        /// </summary>
        public const string Typ = "typ";

        /// <summary>
        /// </summary>
        public const string UniqueName = "unique_name";

        /// <summary>
        /// See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims"/>.
        /// </summary>
        public const string UpdatedAt = "updated_at";

        /// <summary>
        /// See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims"/>.
        /// </summary>
        public const string Website = "website";

        /// <summary>
        /// See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims"/>.
        /// </summary>
        public const string ZoneInfo = "zoneinfo";
    }

    /// <summary>
    /// Parameter names for JsonWebToken registered claim names in UTF8 bytes.
    /// Used by UTF8JsonReader/Writer for performance gains.
    /// </summary>
    internal readonly struct JwtPayloadUtf8Bytes
    {
        // Please keep in alphabetical order

        public static ReadOnlySpan<byte> Actort => "actort"u8;
        public static ReadOnlySpan<byte> Acr => "acr"u8;
        public static ReadOnlySpan<byte> Amr => "amr"u8;
        public static ReadOnlySpan<byte> AtHash => "at_hash"u8;
        public static ReadOnlySpan<byte> Aud => "aud"u8;
        public static ReadOnlySpan<byte> AuthTime => "auth_time"u8;
        public static ReadOnlySpan<byte> Azp => "azp"u8;
        public static ReadOnlySpan<byte> Birthdate => "birthdate"u8;
        public static ReadOnlySpan<byte> CHash => "c_hash"u8;
        public static ReadOnlySpan<byte> Email => "email"u8;
        public static ReadOnlySpan<byte> Exp => "exp"u8;
        public static ReadOnlySpan<byte> Gender => "gender"u8;
        public static ReadOnlySpan<byte> FamilyName => "family_name"u8;
        public static ReadOnlySpan<byte> GivenName => "given_name"u8;
        public static ReadOnlySpan<byte> Iat => "iat"u8;
        public static ReadOnlySpan<byte> Iss => "iss"u8;
        public static ReadOnlySpan<byte> Jti => "jti"u8;
        public static ReadOnlySpan<byte> Name => "name"u8;
        public static ReadOnlySpan<byte> NameId => "nameid"u8;
        public static ReadOnlySpan<byte> Nonce => "nonce"u8;
        public static ReadOnlySpan<byte> Nbf => "nbf"u8;
        public static ReadOnlySpan<byte> PhoneNumber => "phone_number"u8;
        public static ReadOnlySpan<byte> PhoneNumberVerified => "phone_number_verified"u8;
        public static ReadOnlySpan<byte> Prn => "prn"u8;
        public static ReadOnlySpan<byte> Sid => "sid"u8;
        public static ReadOnlySpan<byte> Sub => "sub"u8;
        public static ReadOnlySpan<byte> Typ => "typ"u8;
        public static ReadOnlySpan<byte> UniqueName => "unique_name"u8;
        public static ReadOnlySpan<byte> Website => "website"u8;
    }
}
