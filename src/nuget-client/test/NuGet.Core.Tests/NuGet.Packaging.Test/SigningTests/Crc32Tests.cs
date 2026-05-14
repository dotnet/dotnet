// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;
using FluentAssertions;
using NuGet.Packaging.Signing;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class Crc32Tests
    {
        [Fact]
        public void CalculateCrc_WithSameInput_ReturnsSameCrc()
        {
            // Arrange
            var data = Encoding.ASCII.GetBytes("Test data");

            // Act
            var code1 = Crc32.CalculateCrc(data);
            var code2 = Crc32.CalculateCrc(data);
            var code3 = Crc32.CalculateCrc(data);

            // Assert
            code1.Should().Be(code2);
            code1.Should().Be(code3);
        }

        [Fact]
        public void CalculateCrc_WithDifferentInput_ReturnsDifferentCrc()
        {
            // Arrange
            var data1 = Encoding.ASCII.GetBytes("Test data1");
            var data2 = Encoding.ASCII.GetBytes("Test data2");

            // Act
            var code1 = Crc32.CalculateCrc(data1);
            var code2 = Crc32.CalculateCrc(data2);

            // Assert
            code1.Should().NotBe(code2);
        }

        [Fact]
        public void CalculateCrc_WithZeroBytes_CrcXorsToMagicNumber()
        {
            // From section 4.4.7 ("CRC-32") of the ZIP format specification
            // https://pkware.cachefly.net/webdocs/casestudies/APPNOTE.TXT
            const uint zipCrc32MagicNumber = 0xdebb20e3;

            var bytes = BitConverter.GetBytes(0U);
            var crc = Crc32.CalculateCrc(bytes);
            var actual = crc ^ 0xffffffff;

            Assert.Equal(zipCrc32MagicNumber, actual);
        }
    }
}
