// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography;
using System.Text;
using NuGet.Common;
using NuGet.Packaging.Signing;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Xunit;
using BcAccuracy = Org.BouncyCastle.Asn1.Tsp.Accuracy;
using BcAlgorithmIdentifier = Org.BouncyCastle.Asn1.X509.AlgorithmIdentifier;
using BcGeneralName = Org.BouncyCastle.Asn1.X509.GeneralName;
using BcMessageImprint = Org.BouncyCastle.Asn1.Tsp.MessageImprint;
using BcTstInfo = Org.BouncyCastle.Asn1.Tsp.TstInfo;

namespace NuGet.Packaging.Test
{
    public class TstInfoTests
    {
        [Fact]
        public void Read_WithInvalidAsn1_Throws()
        {
            Assert.Throws<CryptographicException>(
                () => TstInfo.Read(new byte[] { 0x30, 0x0b }));
        }

        [Fact]
        public void Read_WithOnlyRequiredFields_ReturnsInstance()
        {
            var test = new Test();
            var bcTstInfo = test.CreateBcTstInfo();

            var tstInfo = TstInfo.Read(bcTstInfo.GetDerEncoded());

            Verify(test, tstInfo);
            Verify(bcTstInfo, tstInfo);
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData(1, null, null)]
        [InlineData(null, 1, null)]
        [InlineData(null, null, 1)]
        [InlineData(1, 2, 3)]
        public void Read_WithAccuracy_ReturnsInstance(int? seconds, int? milliseconds, int? microseconds)
        {
            var test = new Test() { Accuracy = GetBcAccuracy(seconds, milliseconds, microseconds) };
            var bcTstInfo = test.CreateBcTstInfo();

            var tstInfo = TstInfo.Read(bcTstInfo.GetDerEncoded());

            Verify(test, tstInfo);
            Verify(bcTstInfo, tstInfo);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(false)]
        [InlineData(true)]
        public void Read_WithOrdering_ReturnsInstance(bool? ordering)
        {
            var test = new Test() { Ordering = ordering };
            var bcTstInfo = test.CreateBcTstInfo();

            var tstInfo = TstInfo.Read(bcTstInfo.GetDerEncoded());

            Verify(test, tstInfo);
            Verify(bcTstInfo, tstInfo);
        }

        [Fact]
        public void Read_WithNonce_ReturnsInstance()
        {
            var test = new Test() { Nonce = GetRandomInteger().ToByteArray() };
            var bcTstInfo = test.CreateBcTstInfo();

            var tstInfo = TstInfo.Read(bcTstInfo.GetDerEncoded());

            Verify(test, tstInfo);
            Verify(bcTstInfo, tstInfo);
        }

        [Fact]
        public void Read_WithTsa_ReturnsInstance()
        {
            var tsa = new BcGeneralName(new X509Name("C=US,ST=WA,L=Redmond,O=NuGet,CN=NuGet Test Certificate"));
            var test = new Test() { Tsa = tsa };
            var bcTstInfo = test.CreateBcTstInfo();

            var tstInfo = TstInfo.Read(bcTstInfo.GetDerEncoded());

            Verify(test, tstInfo);
            Verify(bcTstInfo, tstInfo);
        }

        [Fact]
        public void Read_WithExtensions_ReturnsInstance()
        {
            var extensionsGenerator = new X509ExtensionsGenerator();

            extensionsGenerator.AddExtension(
                new DerObjectIdentifier("1.2.3.4.5.1"), critical: true, extValue: new byte[1] { 1 });
            extensionsGenerator.AddExtension(
                new DerObjectIdentifier("1.2.3.4.5.2"), critical: false, extValue: new byte[1] { 2 });

            var extensions = extensionsGenerator.Generate();
            var test = new Test() { Extensions = extensions };
            var bcTstInfo = test.CreateBcTstInfo();

            var tstInfo = TstInfo.Read(bcTstInfo.GetDerEncoded());

            Verify(test, tstInfo);
            Verify(bcTstInfo, tstInfo);
        }

        private static void Verify(Test test, TstInfo tstInfo)
        {
            Assert.Equal(test.Version, tstInfo.Version);
            Assert.Equal(test.Policy, tstInfo.Policy.Value);
            Assert.Equal(test.HashAlgorithm.Value, tstInfo.MessageImprint.HashAlgorithm.Algorithm.Value);
            Assert.Equal(test.Hash, tstInfo.MessageImprint.HashedMessage);
            Assert.Equal(test.SerialNumber, tstInfo.SerialNumber);
            Assert.Equal(test.GenTime, tstInfo.GenTime);
            Assert.Equal(test.Accuracy == null, tstInfo.Accuracy == null);

            if (test.Accuracy != null)
            {
                Assert.Equal(test.Accuracy == null, tstInfo.Accuracy == null);
                Assert.Equal(test.Accuracy.Seconds == null, tstInfo.Accuracy.Seconds == null);

                if (test.Accuracy.Seconds != null)
                {
                    Assert.Equal(test.Accuracy.Seconds.Value.IntValue, tstInfo.Accuracy.Seconds.Value);
                }

                Assert.Equal(test.Accuracy.Millis == null, tstInfo.Accuracy.Milliseconds == null);

                if (test.Accuracy.Millis != null)
                {
                    Assert.Equal(test.Accuracy.Millis.Value.IntValue, tstInfo.Accuracy.Milliseconds.Value);
                }

                Assert.Equal(test.Accuracy.Micros == null, tstInfo.Accuracy.Microseconds == null);

                if (test.Accuracy.Micros != null)
                {
                    Assert.Equal(test.Accuracy.Micros.Value.IntValue, tstInfo.Accuracy.Microseconds.Value);
                }
            }

            Assert.Equal(test.Ordering ?? false, tstInfo.Ordering);
            Assert.Equal(test.Nonce, tstInfo.Nonce);

            Assert.Equal(test.Tsa == null, tstInfo.Tsa == null);

            if (test.Tsa != null)
            {
                Assert.Equal(test.Tsa.GetDerEncoded(), tstInfo.Tsa);
            }

            VerifyExtensions(test.Extensions, tstInfo);
        }

        private static void Verify(BcTstInfo bcTstInfo, TstInfo tstInfo)
        {
            Assert.Equal(bcTstInfo.Version.Value.IntValue, tstInfo.Version);
            Assert.Equal(bcTstInfo.Policy.Id, tstInfo.Policy.Value);
            Assert.Equal(bcTstInfo.MessageImprint.HashAlgorithm.Algorithm.Id, tstInfo.MessageImprint.HashAlgorithm.Algorithm.Value);
            Assert.Equal(bcTstInfo.MessageImprint.GetHashedMessage(), tstInfo.MessageImprint.HashedMessage);
            Assert.Equal(bcTstInfo.SerialNumber.Value.ToByteArray(), tstInfo.SerialNumber);
            Assert.Equal(bcTstInfo.GenTime.ToDateTime(), tstInfo.GenTime);

            Assert.Equal(bcTstInfo.Accuracy == null, tstInfo.Accuracy == null);

            if (bcTstInfo.Accuracy != null)
            {
                Assert.Equal(bcTstInfo.Accuracy == null, tstInfo.Accuracy == null);
                Assert.Equal(bcTstInfo.Accuracy.Seconds == null, tstInfo.Accuracy.Seconds == null);

                if (bcTstInfo.Accuracy.Seconds != null)
                {
                    Assert.Equal(bcTstInfo.Accuracy.Seconds.Value.IntValue, tstInfo.Accuracy.Seconds.Value);
                }

                Assert.Equal(bcTstInfo.Accuracy.Millis == null, tstInfo.Accuracy.Milliseconds == null);

                if (bcTstInfo.Accuracy.Millis != null)
                {
                    Assert.Equal(bcTstInfo.Accuracy.Millis.Value.IntValue, tstInfo.Accuracy.Milliseconds.Value);
                }

                Assert.Equal(bcTstInfo.Accuracy.Micros == null, tstInfo.Accuracy.Microseconds == null);

                if (bcTstInfo.Accuracy.Micros != null)
                {
                    Assert.Equal(bcTstInfo.Accuracy.Micros.Value.IntValue, tstInfo.Accuracy.Microseconds.Value);
                }
            }

            Assert.Equal(bcTstInfo.Ordering?.IsTrue ?? false, tstInfo.Ordering);
            Assert.Equal(bcTstInfo.Nonce == null, tstInfo.Nonce == null);

            if (bcTstInfo.Nonce != null)
            {
                Assert.Equal(bcTstInfo.Nonce.Value.ToByteArray(), tstInfo.Nonce);
            }

            Assert.Equal(bcTstInfo.Tsa == null, tstInfo.Tsa == null);

            if (bcTstInfo.Tsa != null)
            {
                Assert.Equal(bcTstInfo.Tsa.GetDerEncoded(), tstInfo.Tsa);
            }

            VerifyExtensions(bcTstInfo.Extensions, tstInfo);
        }

        private static void VerifyExtensions(X509Extensions expectedExtensions, TstInfo tstInfo)
        {
            Assert.Equal(expectedExtensions == null, tstInfo.Extensions == null);

            if (expectedExtensions != null)
            {
                Assert.Equal(expectedExtensions.GetExtensionOids().Length, tstInfo.Extensions.Count);

                foreach (var extensionOid in expectedExtensions.GetExtensionOids())
                {
                    var expectedExtension = expectedExtensions.GetExtension(extensionOid);
                    var actualExtension = tstInfo.Extensions[extensionOid.Id];

                    Assert.Equal(extensionOid.Id, actualExtension.Oid.Value);
                    Assert.Equal(expectedExtension.IsCritical, actualExtension.Critical);
                    Assert.Equal(expectedExtension.Value.GetOctets(), actualExtension.RawData);
                }
            }
        }

        private static BcAccuracy GetBcAccuracy(int? seconds, int? milliseconds, int? microseconds)
        {
            var derSeconds = GetOptionalInteger(seconds);
            var derMilliseconds = GetOptionalInteger(milliseconds);
            var derMicroseconds = GetOptionalInteger(microseconds);

            return new BcAccuracy(derSeconds, derMilliseconds, derMicroseconds);
        }

        private static DerInteger GetOptionalInteger(long? value)
        {
            if (value == null)
            {
                return null;
            }

            return new DerInteger(BigInteger.ValueOf(value.Value));
        }

        private static BigInteger GetRandomInteger()
        {
            var random = new SecureRandom();

            return BigIntegers.CreateRandomInRange(BigInteger.One, BigInteger.ValueOf(long.MaxValue), random);
        }

        private static byte[] GetDefaultSha256Hash()
        {
            var data = Encoding.UTF8.GetBytes("peach");

            return Common.HashAlgorithmName.SHA256.ComputeHash(data);
        }

        private sealed class Test
        {
            internal int Version { get; set; }
            internal string Policy { get; set; }
            internal Oid HashAlgorithm { get; }
            internal byte[] Hash { get; }
            internal byte[] SerialNumber { get; }
            internal DateTime GenTime { get; }
            internal BcAccuracy Accuracy { get; set; }
            internal bool? Ordering { get; set; }
            internal byte[] Nonce { get; set; }
            internal BcGeneralName Tsa { get; set; }
            internal X509Extensions Extensions { get; set; }

            internal Test()
            {
                Version = 1;
                Policy = "1.2.3.4.5";
                HashAlgorithm = new Oid(Oids.Sha256);

                var data = Encoding.UTF8.GetBytes("peach");

                Hash = Common.HashAlgorithmName.SHA256.ComputeHash(data);
                SerialNumber = GetRandomInteger().ToByteArray();
                var now = DateTime.UtcNow;

                // Round to milliseconds
                GenTime = new DateTime(
                    now.Year,
                    now.Month,
                    now.Day,
                    now.Hour,
                    now.Minute,
                    now.Second,
                    now.Millisecond,
                    DateTimeKind.Utc);
            }

            internal BcTstInfo CreateBcTstInfo()
            {
                var bcAlgorithmIdentifier = new BcAlgorithmIdentifier(new DerObjectIdentifier(HashAlgorithm.Value));
                var bcMessageImprint = new BcMessageImprint(bcAlgorithmIdentifier, Hash);
                var serialNumber = new BigInteger(SerialNumber);
                var ordering = GetOrdering();
                var nonce = GetNonce();

                var bcTstInfo = new BcTstInfo(
                    new DerObjectIdentifier(Policy),
                    bcMessageImprint,
                    new DerInteger(serialNumber),
                    GetGenTime(),
                    Accuracy,
                    ordering,
                    nonce,
                    Tsa,
                    Extensions);

                return bcTstInfo;
            }

            private DerGeneralizedTime GetGenTime()
            {
                var genTime = new StringBuilder();

                genTime.Append(GenTime.ToString("yyyyMMddHHmmss"));

                if (GenTime.Millisecond > 0)
                {
                    var milliseconds = GenTime.Millisecond.ToString().PadLeft(3, '0').TrimEnd('0');

                    genTime.Append($".{milliseconds}");
                }

                genTime.Append("Z");

                return new DerGeneralizedTime(genTime.ToString());
            }

            private DerBoolean GetOrdering()
            {
                if (Ordering == null)
                {
                    return null;
                }

                return Ordering.Value ? DerBoolean.True : DerBoolean.False;
            }

            private DerInteger GetNonce()
            {
                if (Nonce == null)
                {
                    return null;
                }

                return new DerInteger(new BigInteger(Nonce));
            }
        }
    }
}
