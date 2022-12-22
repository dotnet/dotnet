// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Security.Cryptography;
using NuGet.Packaging.Signing;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Math;
using Xunit;
using BcAccuracy = Org.BouncyCastle.Asn1.Tsp.Accuracy;

namespace NuGet.Packaging.Test
{
    public class AccuracyTests
    {
        [Fact]
        public void Read_WithInvalidAsn1_Throws()
        {
            Assert.Throws<CryptographicException>(
                () => Accuracy.Read(new byte[] { 0x30, 0x0b }));
        }

        [Fact]
        public void Read_WithInvalidSeconds_Throws()
        {
            var bcAccuracy = new DerSequence(new DerInteger(-1));
            var bytes = bcAccuracy.GetDerEncoded();

            var exception = Assert.Throws<CryptographicException>(
                () => Accuracy.Read(bytes));

            Assert.Equal("The ASN.1 data is invalid.", exception.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1000)]
        public void Read_WithInvalidMilliseconds_Throws(int milliseconds)
        {
            var derMilliseconds = new DerTaggedObject(
                explicitly: false,
                tagNo: 0,
                obj: new DerInteger(BigInteger.ValueOf(milliseconds)));
            var bcAccuracy = new DerSequence(derMilliseconds);
            var bytes = bcAccuracy.GetDerEncoded();

            var exception = Assert.Throws<CryptographicException>(
                () => Accuracy.Read(bytes));

            Assert.Equal("The ASN.1 data is invalid.", exception.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1000)]
        public void Read_WithInvalidMicroseconds_Throws(int microseconds)
        {
            var derMicroseconds = new DerTaggedObject(
                explicitly: false,
                tagNo: 1,
                obj: new DerInteger(BigInteger.ValueOf(microseconds)));
            var bcAccuracy = new DerSequence(derMicroseconds);
            var bytes = bcAccuracy.GetDerEncoded();

            var exception = Assert.Throws<CryptographicException>(
                () => Accuracy.Read(bytes));

            Assert.Equal("The ASN.1 data is invalid.", exception.Message);
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData(1, null, null)]
        [InlineData(null, 1, null)]
        [InlineData(null, null, 1)]
        [InlineData(1, 2, null)]
        [InlineData(1, null, 2)]
        [InlineData(null, 1, 2)]
        [InlineData(0, 1, 2)]
        [InlineData(1, 2, 3)]
        [InlineData(int.MaxValue, 999, 999)]
        public void Read_WithValidInput_ReturnsInstance(int? seconds, int? milliseconds, int? microseconds)
        {
            var bcAccuracy = GetBcAccuracy(seconds, milliseconds, microseconds);
            var bytes = bcAccuracy.GetDerEncoded();

            var accuracy = Accuracy.Read(bytes);

            Assert.Equal(seconds, accuracy.Seconds);
            Assert.Equal(milliseconds, accuracy.Milliseconds);
            Assert.Equal(microseconds, accuracy.Microseconds);

            Assert.Equal(bcAccuracy.Seconds == null, accuracy.Seconds == null);

            if (bcAccuracy.Seconds != null)
            {
                Assert.Equal(bcAccuracy.Seconds.Value.IntValue, accuracy.Seconds.Value);
            }

            Assert.Equal(bcAccuracy.Millis == null, accuracy.Milliseconds == null);

            if (bcAccuracy.Millis != null)
            {
                Assert.Equal(bcAccuracy.Millis.Value.IntValue, accuracy.Milliseconds.Value);
            }

            Assert.Equal(bcAccuracy.Micros == null, accuracy.Microseconds == null);

            if (bcAccuracy.Micros != null)
            {
                Assert.Equal(bcAccuracy.Micros.Value.IntValue, accuracy.Microseconds.Value);
            }
        }

        [Fact]
        public void GetTotalMicroseconds_WithDefaultValues_ReturnsZero()
        {
            var bcAccuracy = GetBcAccuracy(seconds: null, milliseconds: null, microseconds: null);
            var bytes = bcAccuracy.GetDerEncoded();

            var accuracy = Accuracy.Read(bytes);

            Assert.Equal(0, accuracy.GetTotalMicroseconds());
        }

        [Fact]
        public void GetTotalMicroseconds_WithDifferentValues_ReturnsValue()
        {
            var bcAccuracy = GetBcAccuracy(seconds: 1, milliseconds: 2, microseconds: 3);
            var bytes = bcAccuracy.GetDerEncoded();

            var accuracy = Accuracy.Read(bytes);

            Assert.Equal(1_002_003, accuracy.GetTotalMicroseconds());
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
    }
}
