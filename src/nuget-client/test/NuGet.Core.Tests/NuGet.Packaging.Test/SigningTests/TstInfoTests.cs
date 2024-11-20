// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Formats.Asn1;
using System.Numerics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Internal.NuGet.Testing.SignedPackages;
using NuGet.Common;
using Xunit;
using TestAccuracy = Microsoft.Internal.NuGet.Testing.SignedPackages.Asn1.Accuracy;
using TestAlgorithmIdentifier = Microsoft.Internal.NuGet.Testing.SignedPackages.Asn1.AlgorithmIdentifier;
using TestGeneralName = Microsoft.Internal.NuGet.Testing.SignedPackages.Asn1.GeneralName;
using TestMessageImprint = Microsoft.Internal.NuGet.Testing.SignedPackages.Asn1.MessageImprint;
using TestTstInfo = Microsoft.Internal.NuGet.Testing.SignedPackages.Asn1.TstInfo;
using TstInfo = NuGet.Packaging.Signing.TstInfo;

namespace NuGet.Packaging.Test
{
    public class TstInfoTests
    {
        private static readonly Random Random = new();

        [Fact]
        public void Read_WithInvalidAsn1_Throws()
        {
            Assert.Throws<CryptographicException>(
                () => TstInfo.Read(new byte[] { 0x30, 0x0b }));
        }

        [Fact]
        public void Read_WithOnlyRequiredFields_ReturnsInstance()
        {
            TestTstInfo testTstInfo = CreateTestTstInfo();

            Verify(testTstInfo);
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData(1, null, null)]
        [InlineData(null, 1, null)]
        [InlineData(null, null, 1)]
        [InlineData(1, 2, 3)]
        public void Read_WithAccuracy_ReturnsInstance(int? seconds, int? milliseconds, int? microseconds)
        {
            TestAccuracy testAccuracy = new(seconds, milliseconds, microseconds);
            TestTstInfo testTstInfo = CreateTestTstInfo(accuracy: testAccuracy);

            Verify(testTstInfo);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Read_WithOrdering_ReturnsInstance(bool ordering)
        {
            TestTstInfo testTstInfo = CreateTestTstInfo(ordering: ordering);

            Verify(testTstInfo);
        }

        [Fact]
        public void Read_WithNonce_ReturnsInstance()
        {
            BigInteger nonce = GetRandomInteger();
            TestTstInfo testTstInfo = CreateTestTstInfo(nonce: nonce);

            Verify(testTstInfo);
        }

        [Fact]
        public void Read_WithTsa_ReturnsInstance()
        {
            TestGeneralName tsa = new(
                directoryName: new X500DistinguishedName("CN=\"NuGet Test Certificate\", O=NuGet, L=Redmond, S=WA, C=US").RawData);
            TestTstInfo testTstInfo = CreateTestTstInfo(tsa: tsa);

            Verify(testTstInfo);
        }

        [Fact]
        public void Read_WithExtensions_ReturnsInstance()
        {
            X509ExtensionCollection extensions = new()
            {
                new X509Extension(new Oid("1.2.3.4.5.1"), new byte[1] { 1 }, critical: true),
                new X509Extension(new Oid("1.2.3.4.5.2"), new byte[1] { 2 }, critical: false)
            };
            TestTstInfo testTstInfo = CreateTestTstInfo(extensions: extensions);

            Verify(testTstInfo);
        }

        private static void Verify(TestTstInfo expectedTstInfo)
        {
            AsnWriter writer = new(AsnEncodingRules.DER);

            expectedTstInfo.Encode(writer);
            byte[] bytes = writer.Encode();
            TstInfo actualTstInfo = TstInfo.Read(bytes);

            Assert.Equal(expectedTstInfo.Version, actualTstInfo.Version);
            Assert.Equal(expectedTstInfo.Policy.Value, actualTstInfo.Policy.Value);
            Assert.Equal(expectedTstInfo.MessageImprint.HashAlgorithm.Algorithm.Value, actualTstInfo.MessageImprint.HashAlgorithm.Algorithm.Value);
            Assert.Equal(expectedTstInfo.MessageImprint.HashedMessage.ToArray(), actualTstInfo.MessageImprint.HashedMessage);
            SigningTestUtility.VerifySerialNumber(expectedTstInfo.SerialNumber, actualTstInfo.SerialNumber);
            Assert.Equal(expectedTstInfo.Timestamp, actualTstInfo.GenTime);
            Verify(expectedTstInfo.Accuracy, actualTstInfo.Accuracy);
            Assert.Equal(expectedTstInfo.Ordering, actualTstInfo.Ordering);
            Verify(expectedTstInfo.Nonce, actualTstInfo.Nonce);
            Verify(expectedTstInfo.Tsa, actualTstInfo.Tsa);
            Verify(expectedTstInfo.Extensions, actualTstInfo.Extensions);
        }

        private static void Verify(TestGeneralName? expectedTsa, byte[] actualTsa)
        {
            Assert.Equal(expectedTsa is null, actualTsa is null);

            if (expectedTsa is not null && actualTsa is not null)
            {
                Assert.NotNull(expectedTsa.DirectoryName);

                AsnWriter writer = new(AsnEncodingRules.DER);

                expectedTsa.Encode(writer);

                Assert.Equal(writer.Encode(), actualTsa);
            }
        }

        private static void Verify(TestAccuracy? expectedAccuracy, Signing.Accuracy? actualAccuracy)
        {
            Assert.Equal(expectedAccuracy is null, actualAccuracy is null);

            if (expectedAccuracy is not null && actualAccuracy is not null)
            {
                Assert.Equal(expectedAccuracy.Seconds, actualAccuracy.Seconds);
                Assert.Equal(expectedAccuracy.Milliseconds, actualAccuracy.Milliseconds);
                Assert.Equal(expectedAccuracy.Microseconds, actualAccuracy.Microseconds);
            }
        }

        private static void Verify(BigInteger? expectedNonce, byte[]? actualNonce)
        {
            if (expectedNonce is null)
            {
                Assert.Null(actualNonce);
            }
            else
            {
                Assert.NotNull(actualNonce);
                byte[] expected = expectedNonce.Value.ToByteArray();
                Array.Reverse(expected);
                Assert.Equal(expected, actualNonce);
            }
        }

        private static void Verify(X509ExtensionCollection? expectedExtensions, X509ExtensionCollection? actualExtensions)
        {
            Assert.Equal(expectedExtensions is null, actualExtensions is null);

            if (expectedExtensions is not null && actualExtensions is not null)
            {
                Assert.Equal(expectedExtensions.Count, actualExtensions.Count);

                foreach (X509Extension expectedExtension in expectedExtensions)
                {
                    X509Extension? actualExtension = actualExtensions[expectedExtension.Oid!.Value!];

                    Assert.NotNull(actualExtension);
                    Assert.NotNull(actualExtension.Oid);
                    Assert.Equal(expectedExtension.Oid.Value, actualExtension.Oid.Value);
                    Assert.Equal(expectedExtension.Critical, actualExtension.Critical);
                    Assert.Equal(expectedExtension.RawData, actualExtension.RawData);
                }
            }
        }

        private static byte[] GetDefaultSha256Hash()
        {
            byte[] data = Encoding.UTF8.GetBytes("peach");

            return Common.HashAlgorithmName.SHA256.ComputeHash(data);
        }

        private static TestTstInfo CreateTestTstInfo(
            TestAccuracy? accuracy = null,
            bool ordering = false,
            BigInteger? nonce = null,
            TestGeneralName? tsa = null,
            X509ExtensionCollection? extensions = null)
        {
            Random random = new();
            byte[] data = Encoding.UTF8.GetBytes("peach");
            ReadOnlyMemory<byte> hash = Common.HashAlgorithmName.SHA256.ComputeHash(data);
            TestMessageImprint messageImprint = new(new TestAlgorithmIdentifier(TestOids.Sha256), hash);
            BigInteger serialNumber = GetRandomInteger();
            DateTimeOffset timestamp = GetRoundedTimestamp();

            TestTstInfo tstInfo = new(
                BigInteger.One,
                new Oid("1.2.3.4.5"),
                messageImprint,
                serialNumber,
                timestamp,
                accuracy,
                ordering,
                nonce,
                tsa,
                extensions);

            return tstInfo;
        }

        private static DateTimeOffset GetRoundedTimestamp()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;

            return new DateTimeOffset(
                now.Year,
                now.Month,
                now.Day,
                now.Hour,
                now.Minute,
                now.Second,
                now.Millisecond,
                TimeSpan.Zero);
        }

        private static BigInteger GetRandomInteger()
        {
            int nonce = Random.Next(minValue: 1, maxValue: int.MaxValue);

            return new BigInteger(nonce);
        }
    }
}
