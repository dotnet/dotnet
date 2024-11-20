// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Formats.Asn1;
using System.Security.Cryptography;
using NuGet.Packaging.Signing;
using Xunit;

namespace NuGet.Packaging.Test
{
    using Asn1Tags = Microsoft.Internal.NuGet.Testing.SignedPackages.Asn1.Asn1Tags;

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
            byte[] bytes = GetAccuracyBytes(seconds: -1, milliseconds: null, microseconds: null);

            CryptographicException exception = Assert.Throws<CryptographicException>(
                () => Accuracy.Read(bytes));

            Assert.Equal("The ASN.1 data is invalid.", exception.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1000)]
        public void Read_WithInvalidMilliseconds_Throws(int milliseconds)
        {
            byte[] bytes = GetAccuracyBytes(seconds: null, milliseconds, microseconds: null);

            CryptographicException exception = Assert.Throws<CryptographicException>(
                () => Accuracy.Read(bytes));

            Assert.Equal("The ASN.1 data is invalid.", exception.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1000)]
        public void Read_WithInvalidMicroseconds_Throws(int microseconds)
        {
            byte[] bytes = GetAccuracyBytes(seconds: null, milliseconds: null, microseconds);

            CryptographicException exception = Assert.Throws<CryptographicException>(
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
            byte[] bytes = GetAccuracyBytes(seconds, milliseconds, microseconds);
            Accuracy accuracy = Accuracy.Read(bytes);

            Assert.Equal(seconds, accuracy.Seconds);
            Assert.Equal(milliseconds, accuracy.Milliseconds);
            Assert.Equal(microseconds, accuracy.Microseconds);

            Assert.Equal(seconds is null, accuracy.Seconds is null);

            if (seconds is not null)
            {
                Assert.Equal(seconds.Value, accuracy.Seconds.Value);
            }

            Assert.Equal(milliseconds is null, accuracy.Milliseconds is null);

            if (milliseconds is not null)
            {
                Assert.Equal(milliseconds.Value, accuracy.Milliseconds.Value);
            }

            Assert.Equal(microseconds is null, accuracy.Microseconds is null);

            if (microseconds is not null)
            {
                Assert.Equal(microseconds.Value, accuracy.Microseconds.Value);
            }
        }

        [Fact]
        public void GetTotalMicroseconds_WithDefaultValues_ReturnsZero()
        {
            byte[] bytes = GetAccuracyBytes(seconds: null, milliseconds: null, microseconds: null);

            Accuracy accuracy = Accuracy.Read(bytes);

            Assert.Equal(0, accuracy.GetTotalMicroseconds());
        }

        [Fact]
        public void GetTotalMicroseconds_WithDifferentValues_ReturnsValue()
        {
            byte[] bytes = GetAccuracyBytes(seconds: 1, milliseconds: 2, microseconds: 3);

            Accuracy accuracy = Accuracy.Read(bytes);

            Assert.Equal(1_002_003, accuracy.GetTotalMicroseconds());
        }

        private static byte[] GetAccuracyBytes(int? seconds, int? milliseconds, int? microseconds)
        {
            AsnWriter writer = new(AsnEncodingRules.DER);

            using (writer.PushSequence())
            {
                if (seconds is not null)
                {
                    writer.WriteInteger((long)seconds);
                }

                if (milliseconds is not null)
                {
                    writer.WriteInteger((long)milliseconds, Asn1Tags.ContextSpecific0);
                }

                if (microseconds is not null)
                {
                    writer.WriteInteger((long)microseconds, Asn1Tags.ContextSpecific1);
                }
            }

            return writer.Encode();
        }
    }
}
