// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel;
using Xunit;

namespace NuGet.Versioning.Test
{
    public class SemanticVersionConverterTests
    {
        [Fact]
        public void ConverterIsRegistered()
        {
            var converter = TypeDescriptor.GetConverter(typeof(SemanticVersion));
            Assert.IsType(typeof(SemanticVersionConverter), converter);
        }

        [Fact]
        public void CanConvertFromString()
        {
            var converter = new SemanticVersionConverter();
            bool canConvert = converter.CanConvertFrom(typeof(string));
            Assert.True(canConvert);
        }

        [Theory]
        [InlineData(typeof(int))]
        [InlineData(typeof(object))]
        [InlineData(typeof(DateTime))]
        [InlineData(typeof(Version))]
        public void CannotConvertFromTypesOtherThanString(Type sourceType)
        {
            var converter = new SemanticVersionConverter();
            bool canConvert = converter.CanConvertFrom(sourceType);
            Assert.False(canConvert);
        }

        [Fact]
        public void CanConvertToString()
        {
            var converter = new SemanticVersionConverter();
            bool canConvert = converter.CanConvertTo(typeof(string));
            Assert.True(canConvert);
        }

        [Theory]
        [InlineData(typeof(int))]
        [InlineData(typeof(object))]
        [InlineData(typeof(DateTime))]
        [InlineData(typeof(Version))]
        public void CannotConvertToTypesOtherThanString(Type destinationType)
        {
            var converter = new SemanticVersionConverter();
            bool canConvert = converter.CanConvertTo(destinationType);
            Assert.False(canConvert);
        }

        [Theory]
        [InlineData("1.0.0")]
        [InlineData("0.0.1")]
        [InlineData("1.2.3")]
        [InlineData("1.2.3-alpha")]
        [InlineData("1.2.3-X.yZ.3.234.243.32423423.4.23423.4324.234.234.3242")]
        [InlineData("1.2.3-X.yZ.3.234.243.32423423.4.23423+METADATA")]
        [InlineData("1.2.3-X.y3+0")]
        [InlineData("1.2.3-X+0")]
        [InlineData("1.2.3+0")]
        [InlineData("1.2.3-0")]
        public void ConvertFromConvertsValidSemVerStrings(string input)
        {
            var converter = new SemanticVersionConverter();
            var result = converter.ConvertFrom(input);
            Assert.IsType(typeof(SemanticVersion), result);
            Assert.Equal(input, ((SemanticVersion)result!).ToFullString());
        }

        [Theory]
        [InlineData("2.7")]
        [InlineData("1.3.4.5")]
        [InlineData("1.3-alpha")]
        [InlineData("1.3 .4")]
        [InlineData("2.3.18.2-a")]
        [InlineData("1.2.3-A..B")]
        [InlineData("01.2.3")]
        [InlineData("1.02.3")]
        [InlineData("1.2.03")]
        [InlineData(".2.03")]
        [InlineData("1.2.")]
        [InlineData("1.2.3-a$b")]
        [InlineData("a.b.c")]
        [InlineData("1.2.3-00")]
        [InlineData("1.2.3-A.00.B")]
        public void ConvertFromThrowsForInvalidSemVerStrings(string input)
        {
            var converter = new SemanticVersionConverter();
            var exception = Record.Exception(() => converter.ConvertFrom(input));
            Assert.IsType(typeof(ArgumentException), exception);
        }

        [Theory]
        [InlineData("1.0.0")]
        [InlineData("0.0.1")]
        [InlineData("1.2.3")]
        [InlineData("1.2.3-alpha")]
        [InlineData("1.2.3-X.yZ.3.234.243.32423423.4.23423.4324.234.234.3242")]
        [InlineData("1.2.3-X.yZ.3.234.243.32423423.4.23423+METADATA")]
        [InlineData("1.2.3-X.y3+0")]
        [InlineData("1.2.3-X+0")]
        [InlineData("1.2.3+0")]
        [InlineData("1.2.3-0")]
        public void ConvertToConvertsSemVerToString(string input)
        {
            var converter = new SemanticVersionConverter();
            var semVer = SemanticVersion.Parse(input);
            var result = converter.ConvertTo(semVer, typeof(string));
            Assert.Equal(input, result);
        }
    }
}
