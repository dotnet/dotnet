// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Xunit;

namespace NuGet.Versioning.Test
{
    public class VersionFormatterTests
    {
        [Theory]
        [InlineData("1.2.3.4-RC+99", "1.2.3.4-RC+99")]
        [InlineData("1.2.3", "1.2.3")]
        [InlineData("1.2.3-Pre.2", "1.2.3-Pre.2")]
        [InlineData("1.2.3+99", "1.2.3+99")]
        [InlineData("1.2-Pre", "1.2.0-Pre")]
        public void FullStringFormatTest(string versionString, string expected)
        {
            // arrange
            var formatter = new VersionFormatter();
            var version = NuGetVersion.Parse(versionString);

            // act
            var s = string.Format(formatter, "{0:F}", version);
            var s2 = version.ToString("F", formatter);

            // assert
            Assert.Equal(expected, s);
            Assert.Equal(expected, s2);
        }

        [Theory]
        [InlineData("1.2.3.4-RC+99", "1.2.3.4-RC")]
        [InlineData("1.2.3", "1.2.3")]
        [InlineData("1.2.3-Pre.2", "1.2.3-Pre.2")]
        [InlineData("1.2.3+99", "1.2.3")]
        [InlineData("1.2-Pre", "1.2.0-Pre")]
        public void NormalizedFormatTest(string versionString, string expected)
        {
            // arrange
            var formatter = new VersionFormatter();
            var version = NuGetVersion.Parse(versionString);

            // act
            var s = string.Format(formatter, "{0:N}", version);
            var s2 = version.ToString("N", formatter);

            // assert
            Assert.Equal(expected, s);
            Assert.Equal(expected, s2);
        }

        [Theory]
        [InlineData("1.2.3.4-RC+99", "99")]
        [InlineData("1.2.3", "")]
        [InlineData("1.2.3+A2", "A2")]
        public void FormatMetadataTest(string versionString, string expected)
        {
            // arrange
            var formatter = new VersionFormatter();
            var version = NuGetVersion.Parse(versionString);

            // act
            var s = string.Format(formatter, "{0:M}", version);
            var s2 = version.ToString("M", formatter);

            // assert
            Assert.Equal(expected, s);
            Assert.Equal(expected, s2);
        }

        [Theory]
        [InlineData("1.2.3.4-RC+99", "RC")]
        [InlineData("1.2.3.4-RC.2+99", "RC.2")]
        [InlineData("1.2.3", "")]
        [InlineData("1.2.3+A2", "")]
        public void FormatReleaseTest(string versionString, string expected)
        {
            // arrange
            var formatter = new VersionFormatter();
            var version = NuGetVersion.Parse(versionString);

            // act
            var s = string.Format(formatter, "{0:R}", version);
            var s2 = version.ToString("R", formatter);

            // assert
            Assert.Equal(expected, s);
            Assert.Equal(expected, s2);
        }

        [Theory]
        [InlineData("1.2.3.4-RC+99", "1.2.3.4")]
        [InlineData("1.2.3.4-RC.2+99", "1.2.3.4")]
        [InlineData("1.2.3", "1.2.3")]
        [InlineData("1.2+A2", "1.2.0")]
        public void FormatVersionTest(string versionString, string expected)
        {
            // arrange
            var formatter = new VersionFormatter();
            var version = NuGetVersion.Parse(versionString);

            // act
            var s = string.Format(formatter, "{0:V}", version);
            var s2 = version.ToString("V", formatter);

            // assert
            Assert.Equal(expected, s);
            Assert.Equal(expected, s2);
        }

        [Theory]
        [InlineData("1.2.3.4-RC+99", "1.1.2.3.4(99)*RC: 1.2.3.4")]
        [InlineData("1.2.3.4-RC.2+99", "1.1.2.3.4(99)*RC.2: 1.2.3.4")]
        [InlineData("1.2.3", "1.1.2.3.0()*: 1.2.3")]
        [InlineData("1.2.3+A2", "1.1.2.3.0(A2)*: 1.2.3")]
        public void FormatComplexTest(string versionString, string expected)
        {
            // arrange
            var formatter = new VersionFormatter();
            var version = NuGetVersion.Parse(versionString);

            // act
            var s = string.Format(formatter, "{0:x}.{0:x}.{0:y}.{0:z}.{0:r}({0:M})*{0:R}: {0:V}", version, version, version, version, version, version, version, version);
            var s2 = version.ToString("x.x.y.z.r(M)*R: V", formatter);

            // assert
            Assert.Equal(expected, s);
            Assert.Equal(expected, s2);
        }
    }
}
