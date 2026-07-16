// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace System.IdentityModel.Tokens.Jwt.Tests
{
    public class JwtReferenceTests
    {
        [Theory, MemberData(nameof(Base64UrlEncodingTheoryData), DisableDiscoveryEnumeration = true)]
        public void Base64UrlEncoding(string testId, string dataToEncode, string encodedData)
        {
            TestUtilities.WriteHeader($"Base64UrlEncoding - {testId}", true);
            Assert.True(dataToEncode.Equals(Base64UrlEncoder.Decode(encodedData)), "dataToEncode.Equals(Base64UrlEncoder.Decode(encodedData))");
            Assert.True(encodedData.Equals(Base64UrlEncoder.Encode(dataToEncode)), "encodedData.Equals(Base64UrlEncoder.Encode(dataToEncode))");
        }

        public static TheoryData<string, string, string> Base64UrlEncodingTheoryData
        {
            get
            {
                var theoryData = new TheoryData<string, string, string>();

                theoryData.Add("Test1", RFC7520References.Payload, RFC7520References.PayloadEncoded);
                theoryData.Add("Test2", RFC7520References.RSAHeaderJson, RFC7520References.RSAHeaderEncoded);
                theoryData.Add("Test3", RFC7520References.ES512HeaderJson, RFC7520References.ES512HeaderEncoded);
                theoryData.Add("Test4", RFC7520References.SymmetricHeaderJson, RFC7520References.SymmetricHeaderEncoded);

                return theoryData;
            }
        }

        [Theory, MemberData(nameof(JwtEncodingTheoryData), DisableDiscoveryEnumeration = true)]
        public void JwtEncoding(string testId, JwtHeader header, string encodedData)
        {
            TestUtilities.WriteHeader($"JwtEncoding - {testId}", true);
            Assert.True(encodedData.Equals(header.Base64UrlEncode()), "encodedData.Equals(header.Base64UrlEncode())");
        }

        public static TheoryData<string, JwtHeader, string> JwtEncodingTheoryData
        {
            get
            {
                var theoryData = new TheoryData<string, JwtHeader, string>();

                theoryData.Add("Test1", RFC7520References.ES512JwtHeader, RFC7520References.ES512HeaderEncoded);
                theoryData.Add("Test2", RFC7520References.RSAJwtHeader, RFC7520References.RSAHeaderEncoded);
                theoryData.Add("Test3", RFC7520References.SymmetricJwtHeader, RFC7520References.SymmetricHeaderEncoded);

                return theoryData;
            }
        }

        [Theory, MemberData(nameof(JwtSigningTheoryData), DisableDiscoveryEnumeration = true)]
        public void JwtSigning(JwtSigningTestParams testParams)
        {
            var providerForSigning = CryptoProviderFactory.Default.CreateForSigning(testParams.PrivateKey, testParams.Algorithm);
            var providerForVerifying = CryptoProviderFactory.Default.CreateForVerifying(testParams.PublicKey, testParams.Algorithm);
            var signatureBytes = providerForSigning.Sign(Encoding.UTF8.GetBytes(testParams.EncodedData));
            var encodedSignature = Base64UrlEncoder.Encode(signatureBytes);

            // Signatures aren't necessarily deterministic across different algorithms
            if (testParams.DeterministicSignatures)
                Assert.True(testParams.EncodedSignature.Equals(encodedSignature), "encodedSignature != testParams.EncodedSignature");
            Assert.True(providerForVerifying.Verify(Encoding.UTF8.GetBytes(testParams.EncodedData), Base64UrlEncoder.DecodeBytes(testParams.EncodedSignature)), "Verify Failed");
        }

        public static TheoryData<JwtSigningTestParams> JwtSigningTheoryData
        {
            get
            {
                var theoryData = new TheoryData<JwtSigningTestParams>();

                theoryData.Add(new JwtSigningTestParams
                {
                    Algorithm = RFC7520References.RSAJwtHeader.Alg,
                    EncodedData = RFC7520References.RSAEncoded,
                    EncodedSignature = RFC7520References.RSASignatureEncoded,
                    DeterministicSignatures = true,
                    PrivateKey = RFC7520References.RSASigningPrivateKey,
                    PublicKey = RFC7520References.RSASigningPublicKey,
                    TestId = "Test1"
                });

                theoryData.Add(new JwtSigningTestParams
                {
                    Algorithm = RFC7520References.ES512JwtHeader.Alg,
                    EncodedData = RFC7520References.ES512Encoded,
                    EncodedSignature = RFC7520References.ES512SignatureEncoded,
                    DeterministicSignatures = false,
                    PrivateKey = RFC7520References.ECDsaPrivateKey,
                    PublicKey = RFC7520References.ECDsaPublicKey,
                    TestId = "Test2"
                });

                theoryData.Add(new JwtSigningTestParams
                {
                    Algorithm = RFC7520References.SymmetricJwtHeader.Alg,
                    EncodedData = RFC7520References.SymmetricEncoded,
                    EncodedSignature = RFC7520References.SymmetricSignatureEncoded,
                    DeterministicSignatures = true,
                    PrivateKey = RFC7520References.SymmetricKeyMac,
                    PublicKey = RFC7520References.SymmetricKeyMac,
                    TestId = "Test3"
                });

                return theoryData;
            }
        }

        public class JwtSigningTestParams
        {
            public string Algorithm { get; set; }
            public string EncodedData { get; set; }
            public string EncodedSignature { get; set; }
            public bool DeterministicSignatures { get; set; }
            public SecurityKey PrivateKey { get; set; }
            public SecurityKey PublicKey { get; set; }
            public string TestId { get; set; }

            public override string ToString()
            {
                return TestId + ", " + PrivateKey.KeyId + ", " + PublicKey.KeyId;
            }
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
