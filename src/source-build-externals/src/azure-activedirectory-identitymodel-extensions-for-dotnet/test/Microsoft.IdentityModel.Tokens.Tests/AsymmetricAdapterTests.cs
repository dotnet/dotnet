//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class AsymmetricAdapterTests
    {
        [Theory, MemberData(nameof(AsymmetricAdapterUsageTestCases))]
        public void AsymmetricAdapterUsageTests(AsymmetricAdapterTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.AsymmetricAdapterUsageTests", theoryData);

            byte[] bytes = Encoding.UTF8.GetBytes("var context = TestUtilities.WriteHeader($'{ this}.AsymmetricAdapterUsageTests', theoryData);");
            HashAlgorithm hashAlgorithm = CryptoProviderFactory.Default.CreateHashAlgorithm(theoryData.HashAlorithmString);

            try
            {
#if NET461 || NET472 || NETCOREAPP2_1 || NET6_0
                AsymmetricAdapter asymmetricdapter = new AsymmetricAdapter(theoryData.SecurityKey, theoryData.Algorithm, hashAlgorithm, SupportedAlgorithms.GetHashAlgorithmName(theoryData.Algorithm), true);
#else
                AsymmetricAdapter asymmetricdapter = new AsymmetricAdapter(theoryData.SecurityKey, theoryData.Algorithm, hashAlgorithm, true);
#endif
                byte[] signature = asymmetricdapter.Sign(bytes);
                if (!asymmetricdapter.Verify(bytes, signature))
                    context.AddDiff($"Verify failed for test: {theoryData.TestId}");

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<AsymmetricAdapterTheoryData> AsymmetricAdapterUsageTestCases
        {
#pragma warning disable SYSLIB0028 // Type or member is obsolete
            get => new TheoryData<AsymmetricAdapterTheoryData>
            {
                // X509
                new AsymmetricAdapterTheoryData
                {
                    Algorithm = SecurityAlgorithms.RsaSha256,
                    HashAlorithmString = SupportedAlgorithms.GetDigestFromSignatureAlgorithm(SecurityAlgorithms.RsaSha256),
                    SecurityKey = new X509SecurityKey(KeyingMaterial.CertSelfSigned2048_SHA256),
                    TestId = "KeyingMaterial_CertSelfSigned2048_SHA256"
                },

                // RSA
                // RSACertificateExtensions.GetRSAPrivateKey - this results in 
                #if NET461 || NET472 || NETCOREAPP2_1 || NET6_0
                new AsymmetricAdapterTheoryData
                {
                    Algorithm = SecurityAlgorithms.RsaSha256,
                    HashAlorithmString = SupportedAlgorithms.GetDigestFromSignatureAlgorithm(SecurityAlgorithms.RsaSha256),
                    SecurityKey = new RsaSecurityKey(RSACertificateExtensions.GetRSAPrivateKey(KeyingMaterial.CertSelfSigned2048_SHA256) as RSA),
                    TestId = "RSACertificateExtensions_GetRSAPrivateKey"
                },
                #endif

                // X509Certificte2.PrivateKey - this results in the RSA being of type RSACryptoServiceProviderProxy
                new AsymmetricAdapterTheoryData
                {
                    Algorithm = SecurityAlgorithms.RsaSha256,
                    HashAlorithmString = SupportedAlgorithms.GetDigestFromSignatureAlgorithm(SecurityAlgorithms.RsaSha256),
                    SecurityKey = new RsaSecurityKey(KeyingMaterial.CertSelfSigned2048_SHA256.PrivateKey as RSA),
                    TestId = "KeyingMaterial_CertSelfSigned2048_SHA256_PrivateKey"
                },

                // RSA.Create
                #if NET472 || NETCOREAPP2_1 || NET6_0
                new AsymmetricAdapterTheoryData
                {
                    Algorithm = SecurityAlgorithms.RsaSha256,
                    HashAlorithmString = SupportedAlgorithms.GetDigestFromSignatureAlgorithm(SecurityAlgorithms.RsaSha256),
                    SecurityKey = new RsaSecurityKey(RSA.Create(2048)),
                    TestId = "RSA_Create(2048)"
                },
                #endif

                // RSACryptoServiceProvider
                new AsymmetricAdapterTheoryData
                {
                    Algorithm = SecurityAlgorithms.RsaSha256,
                    HashAlorithmString = SupportedAlgorithms.GetDigestFromSignatureAlgorithm(SecurityAlgorithms.RsaSha256),
                    SecurityKey = new RsaSecurityKey(new RSACryptoServiceProvider(2048)),
                    TestId = "RSACryptoServiceProvider(2048)"
                },

                // RsaParameters
                new AsymmetricAdapterTheoryData
                {
                    Algorithm = SecurityAlgorithms.RsaSha256,
                    HashAlorithmString = SupportedAlgorithms.GetDigestFromSignatureAlgorithm(SecurityAlgorithms.RsaSha256),
                    SecurityKey = new RsaSecurityKey(KeyingMaterial.RsaParameters_2048),
                    TestId = "KeyingMaterial_RsaParameters_2048"
                },

                // ECD
                // ECD object
                new AsymmetricAdapterTheoryData
                {
                    Algorithm = SecurityAlgorithms.EcdsaSha256,
                    HashAlorithmString = SupportedAlgorithms.GetDigestFromSignatureAlgorithm(SecurityAlgorithms.EcdsaSha256),
                    SecurityKey = KeyingMaterial.Ecdsa256Key,
                    TestId = "KeyingMaterial_Ecdsa256Key"
                },

                #if NET472 || NETCOREAPP2_1 || NET6_0
                new AsymmetricAdapterTheoryData
                {
                    Algorithm = SecurityAlgorithms.EcdsaSha256,
                    HashAlorithmString = SupportedAlgorithms.GetDigestFromSignatureAlgorithm(SecurityAlgorithms.EcdsaSha256),
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    TestId = "KeyingMaterial_JsonWebKeyP256"
                },
                #endif

            };
#pragma warning restore SYSLIB0028 // Type or member is obsolete
        }

        public class AsymmetricAdapterTheoryData : TheoryDataBase
        {
            public string Algorithm { get; set; }

            public string HashAlorithmString { get; set; }

            public SecurityKey SecurityKey { get; set; }

        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
