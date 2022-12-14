// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.IO;
using Xunit;

namespace System.Security.Cryptography.X509Certificates.Tests
{
    [SkipOnPlatform(TestPlatforms.Browser, "Browser doesn't support X.509 certificates")]
    public class ContentTypeTests
    {
        [Theory]
        [MemberData(nameof(GetFileNamesWithType))]
        public static void TestFileContentType(string fileName, X509ContentType contentType)
        {
            string fullPath = Path.Combine(TestFiles.TestDataFolder, fileName);
            X509ContentType fileType = X509Certificate2.GetCertContentType(fullPath);
            Assert.Equal(contentType, fileType);
        }

        [Theory]
        [MemberData(nameof(GetContentBlobsWithType))]
        public static void TestBlobContentType(string caseName, byte[] blob, X509ContentType contentType)
        {
            _ = caseName;
            X509ContentType blobType = X509Certificate2.GetCertContentType(blob);
            Assert.Equal(contentType, blobType);

            blobType = X509Certificate2.GetCertContentType(blob.AsSpan());
            Assert.Equal(contentType, blobType);
        }

        [Fact]
        public static void TestThrowsWhenGivenInvalidContent()
        {
            byte[] blob = new byte[] { 0x00, 0xFF, 0x00, 0xFF };
            Assert.ThrowsAny<CryptographicException>(() => X509Certificate2.GetCertContentType(blob));
            Assert.ThrowsAny<CryptographicException>(() => X509Certificate2.GetCertContentType(blob.AsSpan()));
        }

        public static IEnumerable<object[]> GetContentBlobsWithType()
        {
            return new[]
            {
                new object[] { "MsCertificate", TestData.MsCertificate, X509ContentType.Cert },
                new object[] { "MsCertificatePem", TestData.MsCertificatePemBytes, X509ContentType.Cert },
                new object[] { "Pkcs7ChainDer", TestData.Pkcs7ChainDerBytes, X509ContentType.Pkcs7 },
                new object[] { "Pkcs7ChainPem", TestData.Pkcs7ChainPemBytes, X509ContentType.Pkcs7 },
                new object[] { "Pkcs7EmptyDer", TestData.Pkcs7EmptyDerBytes, X509ContentType.Pkcs7 },
                new object[] { "Pkcs7EmptyPem", TestData.Pkcs7EmptyPemBytes, X509ContentType.Pkcs7 },
                new object[] { "Pkcs7SingleDer", TestData.Pkcs7SingleDerBytes, X509ContentType.Pkcs7 },
                new object[] { "Pkcs7SinglePem", TestData.Pkcs7SinglePemBytes, X509ContentType.Pkcs7 },
                new object[] { "PfxData", TestData.PfxData, X509ContentType.Pkcs12 },
                new object[] { "EmptyPfx", TestData.EmptyPfx, X509ContentType.Pkcs12 },
                new object[] { "MultiPrivatePfx", TestData.MultiPrivateKeyPfx, X509ContentType.Pkcs12 },
                new object[] { "ChainPfx", TestData.ChainPfxBytes, X509ContentType.Pkcs12 },
                new object[] { "ConcatenatedPem", TestData.ConcatenatedPemFile, X509ContentType.Cert }
            };
        }

        public static IEnumerable<object[]> GetFileNamesWithType()
        {
            return new[]
            {
                new object[] { TestFiles.PfxFileName, X509ContentType.Pkcs12 },
                new object[] { TestFiles.MyCertFileName, X509ContentType.Cert }
            };
        }
    }
}
