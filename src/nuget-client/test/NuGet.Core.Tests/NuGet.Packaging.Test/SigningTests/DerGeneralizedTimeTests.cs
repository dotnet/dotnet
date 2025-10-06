// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography;
using Microsoft.Internal.NuGet.Testing.SignedPackages;
using NuGet.Packaging.Signing.DerEncoding;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class DerGeneralizedTimeTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Read_WhenDecodedTimeNullOrEmpty_Throws(string decodedTime)
        {
            var exception = Assert.Throws<CryptographicException>(() => DerGeneralizedTime.Read(decodedTime));

            Assert.Equal("ASN1 corrupted data.", exception.Message);
        }

        [Fact]
        public void Read_WithStringTooSmall_Throws()
        {
            var exception = Assert.Throws<CryptographicException>(() => DerGeneralizedTime.Read("1985110621062Z"));

            Assert.Equal("ASN1 corrupted data.", exception.Message);
        }

        [Theory]
        [InlineData("20180208142637.3")]
        [InlineData("20180208142637.3-0800")]
        public void Read_WithNonZuluTime_Throws(string decodedTime)
        {
            var exception = Assert.Throws<CryptographicException>(() => DerGeneralizedTime.Read(decodedTime));

            Assert.Equal("ASN1 corrupted data.", exception.Message);
        }

        [Fact]
        public void Read_WithNoDigitAfterDecimal_Throws()
        {
            var exception = Assert.Throws<CryptographicException>(() => DerGeneralizedTime.Read("19851106210627.Z"));

            Assert.Equal("ASN1 corrupted data.", exception.Message);
        }

        [Theory]
        [InlineData("20180208142637.0Z")]
        [InlineData("20180208142637.30Z")]
        public void Read_WithTrailingZero_Throws(string decodedTime)
        {
            var exception = Assert.Throws<CryptographicException>(() => DerGeneralizedTime.Read(decodedTime));

            Assert.Equal("ASN1 corrupted data.", exception.Message);
        }

        [Theory]
        [InlineData("00000208142637Z")]
        [InlineData("20180008142637Z")]
        [InlineData("20180200142637Z")]
        [InlineData("20180208992637Z")]
        [InlineData("20180208149937Z")]
        [InlineData("20180208142699Z")]
        public void Read_WithInvalidDateTimeComponents_Throws(string decodedTime)
        {
            var exception = Assert.Throws<CryptographicException>(() => DerGeneralizedTime.Read(decodedTime));

            Assert.Equal("ASN1 corrupted data.", exception.Message);
        }

        [Fact]
        public void Read_WithNoFractionalSeconds_ReturnsInstance()
        {
            var time = DerGeneralizedTime.Read("20180208142637Z");

            Assert.Equal(new DateTime(2018, 2, 8, 14, 26, 37, DateTimeKind.Utc), time.DateTime);
        }

        [Theory]
        [InlineData("1", 100)]
        [InlineData("01", 10)]
        [InlineData("001", 1)]
        [InlineData("12", 120)]
        [InlineData("123", 123)]
        public void Read_WithMilliseconds_ReturnsInstance(string millisecondsString, int milliseconds)
        {
            var time = DerGeneralizedTime.Read($"20180208142637.{millisecondsString}Z");

            Assert.Equal(new DateTime(2018, 2, 8, 14, 26, 37, milliseconds, DateTimeKind.Utc), time.DateTime);
        }

        [Fact]
        public void Read_WithFullDateTimePrecision_ReturnsInstance()
        {
            var utcNow = DateTimeOffset.UtcNow;
            var input = DerGeneralizedTimeUtility.ToDerGeneralizedTimeString(utcNow);

            var time = DerGeneralizedTime.Read(input);

            Assert.Equal(utcNow.UtcDateTime, time.DateTime);
        }

        [Fact]
        public void Read_WithNanoseconds_ReturnsInstanceWithSomeFractionalDigitsIgnored()
        {
            var time = DerGeneralizedTime.Read("20180208142637.123456789Z");

            Assert.Equal(new DateTime(ticks: 636536967971234567, kind: DateTimeKind.Utc), time.DateTime);
        }
    }
}
