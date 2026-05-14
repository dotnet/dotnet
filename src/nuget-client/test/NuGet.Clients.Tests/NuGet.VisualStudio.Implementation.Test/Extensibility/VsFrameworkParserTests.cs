// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.Versioning;
using Moq;
using NuGet.Frameworks;
using NuGet.VisualStudio.Implementation.Extensibility;
using NuGet.VisualStudio.Telemetry;
using Xunit;

namespace NuGet.VisualStudio.Implementation.Test.Extensibility
{
    public class VsFrameworkParserTests
    {
        // known/expected errors should not be reported to telemetry, hence use MockBehavior.Strict
        private Mock<INuGetTelemetryProvider> _telemetryProvider = new Mock<INuGetTelemetryProvider>(MockBehavior.Strict);

        [Fact]
        public void VsFrameworkParser_ParseFrameworkName_RejectsNullInput()
        {
            // Arrange
            var target = new VsFrameworkParser(_telemetryProvider.Object);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => target.ParseFrameworkName(null));
        }

        [Fact]
        public void VsFrameworkParser_ParseFrameworkName_RejectsInvalidVersion()
        {
            // Arrange
            var target = new VsFrameworkParser(_telemetryProvider.Object);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => target.ParseFrameworkName("foo,Version=a.b"));
        }

        [Fact]
        public void VsFrameworkParser_ParseFrameworkName_RejectsInvalidProfile()
        {
            // Arrange
            var target = new VsFrameworkParser(_telemetryProvider.Object);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => target.ParseFrameworkName(".NETPortable,Version=1.0,Profile=a-b"));
        }

        [Fact]
        public void VsFrameworkParser_ParseFrameworkName_ParsesShortFrameworkName()
        {
            // Arrange
            var target = new VsFrameworkParser(_telemetryProvider.Object);

            // Act
            var frameworkName = target.ParseFrameworkName("net45");

            // Assert
            Assert.Equal(".NETFramework,Version=v4.5", frameworkName.ToString());
        }

        [Fact]
        public void VsFrameworkParser_ParseFrameworkName_ParsesLongFrameworkName()
        {
            // Arrange
            var target = new VsFrameworkParser(_telemetryProvider.Object);

            // Act
            var frameworkName = target.ParseFrameworkName(".NETFramework,Version=v4.5");

            // Assert
            Assert.Equal(".NETFramework,Version=v4.5", frameworkName.ToString());
        }

        [Fact]
        public void VsFrameworkParser_GetShortFrameworkName_Success()
        {
            // Arrange
            var target = new VsFrameworkParser(_telemetryProvider.Object);
            var frameworkName = new FrameworkName(".NETStandard", Version.Parse("1.3"));

            // Act
            var actual = target.GetShortFrameworkName(frameworkName);

            // Assert
            Assert.Equal("netstandard1.3", actual);
        }

        [Fact]
        public void VsFrameworkParser_GetShortFrameworkName_RejectsNull()
        {
            // Arrange
            var target = new VsFrameworkParser(_telemetryProvider.Object);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => target.GetShortFrameworkName(null));
        }

        [Fact]
        public void VsFrameworkParser_GetShortFrameworkName_RejectsInvalidFrameworkIdentifier()
        {
            // Arrange
            var target = new VsFrameworkParser(_telemetryProvider.Object);
            var frameworkName = new FrameworkName("!?", Version.Parse("4.5"));

            // Act & Assert
            Assert.Throws<ArgumentException>(() => target.GetShortFrameworkName(frameworkName));
        }

        [Fact]
        public void VsFrameworkParser_GetShortFrameworkName_RejectsInvalidPortable()
        {
            // Arrange
            var target = new VsFrameworkParser(_telemetryProvider.Object);
            var frameworkName = new FrameworkName(".NETPortable", Version.Parse("4.5"), "net45+portable-net451");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => target.GetShortFrameworkName(frameworkName));
        }

        [Fact]
        public void TryParse_NullInput_ThrowsArgumentNullException()
        {
            // Arrange
            var target = new VsFrameworkParser(_telemetryProvider.Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => target.TryParse(input: null, out _));

            // Assert
            Assert.Equal("input", exception.ParamName);
        }

        [Theory]
        [InlineData("net472")]
        [InlineData(".NETFramework,Version=4.7.2")]
        [InlineData("netstandard2.0")]
        [InlineData(".NETStandard,Version=2.0")]
        [InlineData("netcoreapp3.1")]
        [InlineData(".NETCoreApp,Version=3.1")]
        [InlineData("net5.0")]
        [InlineData(".NETCoreApp,Version=5.0")]
        [InlineData("net5.0-android10.0")]
        [InlineData("portable-net45+win8")]
        [InlineData(".NETPortable,Version=v0.0,Profile=Profile7")]
        public void TryParse_ValidInput_Succeeds(string input)
        {
            // Arrange
            var target = new VsFrameworkParser(_telemetryProvider.Object);
            IVsNuGetFramework actual;
            var expected = NuGetFramework.Parse(input);

            // Act
            var result = target.TryParse(input, out actual);

            // Assert
            Assert.True(result, "Return value was not true");
            Assert.Equal(expected.DotNetFrameworkName, actual.TargetFrameworkMoniker);
            Assert.Equal(expected.DotNetPlatformName, actual.TargetPlatformMoniker);
            Assert.Null(actual.TargetPlatformMinVersion);
        }

        [Theory]
        [InlineData("invalid")]
        [InlineData("any")]
        [InlineData("unsupported")]
        public void TryParse_InvalidInput_ReturnsFalse(string input)
        {
            // Arrange
            var target = new VsFrameworkParser(_telemetryProvider.Object);

            // Act
            var actual = target.TryParse(input, out IVsNuGetFramework parsed);

            // Assert
            Assert.False(actual, $"Expected false, but got true for {input}. TFM {parsed.TargetFrameworkMoniker}, TPV {parsed.TargetPlatformMoniker}, TPMV {parsed.TargetPlatformMinVersion}");
        }
    }
}
