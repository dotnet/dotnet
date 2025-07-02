// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Constants for Security Algorithm.
    /// </summary>
    public static class SecurityAlgorithms
    {
#pragma warning disable 1591
        // See: https://www.w3.org/TR/xmlenc-core1/#sec-AES
        public const string Aes128Encryption = "http://www.w3.org/2001/04/xmlenc#aes128-cbc";
        public const string Aes192Encryption = "http://www.w3.org/2001/04/xmlenc#aes192-cbc";
        public const string Aes256Encryption = "http://www.w3.org/2001/04/xmlenc#aes256-cbc";
        public const string DesEncryption = "http://www.w3.org/2001/04/xmlenc#des-cbc";

        // See: https://www.w3.org/TR/xmlenc-core1/#sec-kw-aes
        public const string Aes128KeyWrap = "http://www.w3.org/2001/04/xmlenc#kw-aes128";
        public const string Aes192KeyWrap = "http://www.w3.org/2001/04/xmlenc#kw-aes192";
        public const string Aes256KeyWrap = "http://www.w3.org/2001/04/xmlenc#kw-aes256";
        public const string RsaV15KeyWrap = "http://www.w3.org/2001/04/xmlenc#rsa-1_5";
        public const string Ripemd160Digest = "http://www.w3.org/2001/04/xmlenc#ripemd160";

        // See: https://www.w3.org/TR/xmlenc-core1/#sec-RSA-OAEP
        public const string RsaOaepKeyWrap = "http://www.w3.org/2001/04/xmlenc#rsa-oaep";

        // See: https://datatracker.ietf.org/doc/html/rfc7518#section-4.1
        public const string Aes128KW = "A128KW";
        public const string Aes192KW = "A192KW";
        public const string Aes256KW = "A256KW";
        public const string RsaPKCS1 = "RSA1_5";
        public const string RsaOAEP = "RSA-OAEP";

        // See: https://www.w3.org/TR/xmlenc-core1/#sec-Exclusive-Canonicalization
        public const string ExclusiveC14n = "http://www.w3.org/2001/10/xml-exc-c14n#";
        public const string ExclusiveC14nWithComments = "http://www.w3.org/2001/10/xml-exc-c14n#WithComments";

        // See: https://www.w3.org/TR/xmldsig-core1/#sec-EnvelopedSignature
        public const string EnvelopedSignature = "http://www.w3.org/2000/09/xmldsig#enveloped-signature";

        // See http://www.w3.org/TR/2002/REC-xmlenc-core-20021210/#sec-SHA256
        // and https://datatracker.ietf.org/doc/html/rfc6931#section-2.1.3
        public const string Sha256Digest = "http://www.w3.org/2001/04/xmlenc#sha256";
        public const string Sha384Digest = "http://www.w3.org/2001/04/xmldsig-more#sha384";
        public const string Sha512Digest = "http://www.w3.org/2001/04/xmlenc#sha512";

        // See: https://datatracker.ietf.org/doc/html/rfc7518#appendix-A
        public const string Sha256 = "SHA256";
        public const string Sha384 = "SHA384";
        public const string Sha512 = "SHA512";

        // See: https://datatracker.ietf.org/doc/html/rfc6931#section-2.3.6
        public const string EcdsaSha256Signature = "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha256";
        public const string EcdsaSha384Signature = "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha384";
        public const string EcdsaSha512Signature = "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha512";

        // See: https://datatracker.ietf.org/doc/html/rfc6931#section-2.2.2
        public const string HmacSha256Signature = "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256";
        public const string HmacSha384Signature = "http://www.w3.org/2001/04/xmldsig-more#hmac-sha384";
        public const string HmacSha512Signature = "http://www.w3.org/2001/04/xmldsig-more#hmac-sha512";

        // See: https://datatracker.ietf.org/doc/html/rfc6931#section-2.3.2
        public const string RsaSha256Signature = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
        public const string RsaSha384Signature = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha384";
        public const string RsaSha512Signature = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha512";

        // See: https://datatracker.ietf.org/doc/html/rfc6931#section-2.3.10
        public const string RsaSsaPssSha256Signature = "http://www.w3.org/2007/05/xmldsig-more#sha256-rsa-MGF1";
        public const string RsaSsaPssSha384Signature = "http://www.w3.org/2007/05/xmldsig-more#sha384-rsa-MGF1";
        public const string RsaSsaPssSha512Signature = "http://www.w3.org/2007/05/xmldsig-more#sha512-rsa-MGF1";

        // See: https://datatracker.ietf.org/doc/html/rfc7518#section-3
        public const string EcdsaSha256 = "ES256";
        public const string EcdsaSha384 = "ES384";
        public const string EcdsaSha512 = "ES512";
        public const string HmacSha256 = "HS256";
        public const string HmacSha384 = "HS384";
        public const string HmacSha512 = "HS512";
        public const string None = "none";
        public const string RsaSha256 = "RS256";
        public const string RsaSha384 = "RS384";
        public const string RsaSha512 = "RS512";
        public const string RsaSsaPssSha256 = "PS256";
        public const string RsaSsaPssSha384 = "PS384";
        public const string RsaSsaPssSha512 = "PS512";

        // See: https://datatracker.ietf.org/doc/html/rfc7518#section-5.1
        public const string Aes128CbcHmacSha256 = "A128CBC-HS256";
        public const string Aes192CbcHmacSha384 = "A192CBC-HS384";
        public const string Aes256CbcHmacSha512 = "A256CBC-HS512";
        public const string Aes128Gcm = "A128GCM";
        public const string Aes192Gcm = "A192GCM";
        public const string Aes256Gcm = "A256GCM";

        internal const string DefaultAsymmetricKeyWrapAlgorithm = RsaOaepKeyWrap;
        internal const string DefaultSymmetricEncryptionAlgorithm = Aes128CbcHmacSha256;

        // See: https://datatracker.ietf.org/doc/html/rfc7518#section-4.6
        public const string EcdhEsA128kw = "ECDH-ES+A128KW";
        public const string EcdhEsA192kw = "ECDH-ES+A192KW";
        public const string EcdhEsA256kw = "ECDH-ES+A256KW";

        // See: https://datatracker.ietf.org/doc/html/rfc7518#section-4.6
        public const string EcdhEs = "ECDH-ES";
#pragma warning restore 1591
    }
}
